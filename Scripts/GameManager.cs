using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GameManager : MonoBehaviour
{

    public int boardWidth, boardHeight; // board size init is in Unity editor
    private GameObject tileFolder, towerFolder, enemyFolder;// folders for object organization
    private int numTiles; // # of tiles for labeling each tile
    private Tile[,] board; // 2d array containing all tiles
    private List<Tower> towers; // list of all placed towers 
    private List<Enemy> enemies; // list of all placed towers 
    private List<int> currentbullets; //list of current bullets on the board
    private Tower currentTower; // tower currently being placed
    private bool placing; // whether a tower is being placed
    private bool started; // whether enemies are permitted to move

    int constraint0 = 1; //Constraints of red tower
    int constraint1 = 1; //Constraints of green tower
    int constraint2 = 1; //Constraints of blue tower

    Texture2D redtexture; // texture for red tower
    Texture2D greentexture; // texture for green tower
    Texture2D bluetexture; // texture for blue tower

    // Beat tracking
    private float clock;
    private float startTime;
    private float BEAT = .5f;
    private int numBeats = 0;

    // Level number
    public int level = 0;

    // Use this for initialization
    void Start()
    {

        numTiles = 0;
        placing = false;
        started = false;
        currentbullets = new List<int>();	// list of current bullets on the board in terms of tile number

        redtexture = Resources.Load<Texture2D>("Textures/redTower");
        greentexture = Resources.Load<Texture2D>("Textures/greenTower");
        bluetexture = Resources.Load<Texture2D>("Textures/blueTower");

        // set up folder for tiles
        tileFolder = new GameObject();
        tileFolder.name = "Tiles";
        board = new Tile[boardWidth, boardHeight];

        //set up folder for towers
        towerFolder = new GameObject();
        towerFolder.name = "Towers";
        towers = new List<Tower>();

        //set up folder for enemies
        enemyFolder = new GameObject();
        enemyFolder.name = "Enemies";
        enemies = new List<Enemy>();

        makeLevel();
    }

    // Update is called once per frame
    void Update()
    {
        if (this.started) {
            // Beat counting
            clock = clock + Time.deltaTime;

            if (clock - startTime >= BEAT) {
                startTime = clock; // Resets counter for next beat

                // Makes every tower fire
                for (int i = 0; i < towers.Count; i++) {
                    towers[i].shoot(numBeats);
                }

                // Makes every enemy move
                for (int i = 0; i < enemies.Count; i++) {
                    enemies[i].move(numBeats);
                }
                numBeats++;
            }

            // Update the current bullets
            for (int i = 0; i < towers.Count; i++) {
                List<Bullet> bullets = towers[i].getBullets();
                for (int j = 0; j < bullets.Count; j++) {
                    int a = onTile(bullets[j].transform.position.x, bullets[j].transform.position.y);
                    if (!currentbullets.Contains(a)) {
                        currentbullets.Add(a);
                    }
                }
            }

            // Update interaction between enemies and bullets
            for (int i = 0; i < enemies.Count; i++) {
                Enemy enemy = enemies[i];
                int h = enemy.getHealth();
                for (int j = 0; j < currentbullets.Count; j++) {
                    int b = onTile(enemy.getX(), enemy.getY());
                    if (b == currentbullets[j]) {
                        enemy.damage(numBeats);
                        if (enemy.getHealth() == 0) {
                            enemy.destroy();
                            enemies.Remove(enemy);
                        }
                        break;
                    }
                }
            }
            currentbullets.Clear();
        }
    }

    //adds a single game tile to the location x, y (unity units)
    private void addTile(int x, int y)
    {
        GameObject tileObject = new GameObject(); // create empty game object
        Tile tile = tileObject.AddComponent<Tile>(); // add tile script to object
        tile.transform.parent = tileFolder.transform; // make the tile folder its parent
        tile.transform.position = new Vector3(x, y, 0);
        tile.init(x, y, this); // initialize the tile
        board[x, y] = tile; // store the tile in board
        tile.name = "Tile " + numTiles; // name tile for easy finding
        numTiles += 1;
    }

    // creates a rectangular board of tiles using addTile
    private void buildBoard()
    {
        for (int x = 0; x < boardWidth; x++)
        {
            for (int y = 0; y < boardHeight; y++)
            {
                addTile(x, y);
            }
        }
    }

    private void destroyBoard()
    {
        for (int x = 0; x < boardWidth; x++)
        {
            for (int y = 0; y < boardHeight; y++)
            {
                DestroyImmediate(board[x, y].gameObject);
            }
        }
        Array.Clear(board, 0, board.Length);
        board = new Tile[boardWidth, boardHeight];
    }

    // creates a tower (hiden at first) which is then moved around as the player mouses over tiles
    private void addTower(int type)
    {
        GameObject towerObject = new GameObject(); // create empty game object
        Tower tower = towerObject.AddComponent<Tower>(); // add tower script to object
        tower.transform.parent = towerFolder.transform; // make the tower folder its parent
        tower.transform.position = new Vector3(0, 0, 0); // set location in relation to global coords
        tower.init(type, this); // initialize tower
        tower.name = "Tower " + towers.Count; // name tower for easy finding
        currentTower = tower; // make the tower the one currently being placed
    }

    // places the tower that is currently being placed (now fixed on the board)
    public bool placeTower()
    {
        int x = (int)currentTower.transform.position.x;
        int y = (int)currentTower.transform.position.y;
        // ask the tile under the tower if it has a tower on it
        if (!(board[x, y].HasTower()))
        {
            towers.Add(currentTower); // add tower to list of placed towers
            currentTower.transform.position = new Vector3(x, y, -.5f); // lower the tower so it is under the placing lvl
            currentTower = null;
            placing = false;
            board[x, y].setHasTower(true);
            return true;
        }
        else
        {
            return false;
        }
    }

    // calls the destroy method of tower (possibly not necessary)
    public void destroyTower(Tower tower)
    {
        towers.Remove(tower);
        Destroy(tower.gameObject);
    }

    private void destroyTowers()
    {
        for (int i = 0; i < towers.Count; i++)
        {
            towers[i].eraseBullets();
            DestroyImmediate(towers[i].gameObject);
        }
        towers = null;
        towers = new List<Tower>();
    }

	// returns the tower whcih is currently being placed
	public Tower getCurrent()
	{
		return currentTower;
	}

	// returns true if there is currently a tower being placed
	public bool isPlacing()
	{
		return placing;
	}

	// changes the sprite color of the board at location x,y to green
	public void highlight(int x, int y)
	{
		if ((x >= 0) && (x < boardWidth) && (y >= 0) && (y < boardHeight))
		{
			board[x, y].getModel().getMat().color = Color.green;
		}
	}

	// sets the color of all the tiles back to (1,1,1,1)
	public void resetColors()
	{
		for (int x = 0; x < boardWidth; x++)
		{
			for (int y = 0; y < boardHeight; y++)
			{
				board[x, y].getModel().getMat().color = new Color(1, 1, 1, 1);
			}
		}
	}

	// 
	private void makeLevel() {
		addEnemies ();
		buildBoard ();
		setConstraints ();
	}

    public void resetLevel()
    {
        destroyTowers();
        if (placing) { DestroyImmediate(currentTower.gameObject); }
        destroyEnemies();
        destroyBoard();
        numBeats = 0;
        started = false;
        placing = false;
        numTiles = 0;

        makeLevel();

    }

	// set constraints based on level
	private void setConstraints() {
		if (level == 0) {
			constraint0 = 0;
			constraint1 = 0;
			constraint2 = 2;
		}
	}

	// add enemies
	private void addEnemies()
    {
		if (level == 0)
        {
			addEnemy (2, -1, 5);
			addEnemy (2, -3, 5);
		}
	}

	// add a single enemy
	public void addEnemy(int enemyType, int x, int y){
		GameObject enemyObject = new GameObject();
		Enemy enemy = enemyObject.AddComponent<Enemy>();
		enemy.transform.parent = enemyFolder.transform;
		enemy.transform.position = new Vector3 (x, y, 0);
		enemy.init(enemyType, this, x, y);
		enemies.Add(enemy);
		enemy.name = "Enemy " + enemies.Count;
	}

    private void destroyEnemies()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            DestroyImmediate(enemies[i].gameObject);
        }
        enemies = null;
        enemies = new List<Enemy>();
    }

	// check the tile the object is on
	int onTile(float x, float y){
		int a = (int) Math.Round(x);
		int b = (int) Math.Round(y);
		int ontile = a * boardWidth + b;
		return ontile;
	}

    public bool isStarted()
    {
        return started;
    }

    //used to update how many towers are available when one is destroyed
    public void addConstraint(int type, int x)
    {
        if (type == 0) { constraint0 += x; }
        if (type == 1) { constraint1 += x; }
        if (type == 2) { constraint2 += x; }
    }

    //so towers can access tiles
    public Tile getTile(int x, int y)
    {
        return board[x, y];
    }

    private void destroyBullets()
    {

        currentbullets.Clear();
    }

    // logic for the GUI
    void OnGUI()
	{
        //labels for how many towers are left
        GUI.Label(new Rect(Screen.width - 155, 70, 110, 110), constraint0.ToString());
        GUI.Label(new Rect(Screen.width - 155, 205, 110, 110), constraint1.ToString());
        GUI.Label(new Rect(Screen.width - 155, 340, 110, 110), constraint2.ToString());

        if (!started)
        {
            if (GUI.Button(new Rect(25, 25, 110, 30), "START")) {
                started = true;
            }
        }
		if (placing)
		{
			// if the rotate button is pressed
			if (GUI.Button(new Rect(Screen.width-135, Screen.height - 55, 110, 30), "ROTATE"))
			{
				currentTower.rotate(); // rotate the tower being placed
			}
		}

        if (GUI.Button(new Rect(25, 80, 110, 30), "RESTART"))
        {
            resetLevel();
        }

            // button for RED tower
            if (GUI.Button(new Rect(Screen.width - 135, 25, 110, 110), image: redtexture))
		{
            if (placing)
            {
                int type = currentTower.getTowerType();
                destroyTower(currentTower);
                if (type == 0) // if already placing that tower, stop placing
                {
                    placing = false;
                    constraint0 += 1;
                }
                else if (constraint0 > 0) // if placing another tower, switch to this one if one is left
                {
                    if (type == 1) { constraint1 += 1; }
                    if (type == 2) { constraint2 += 1; }
                    addTower(0);
                    constraint0 = constraint0 - 1;
                }
            }
            else if (constraint0 > 0) // if not already placing, start placing this one if one is left
            {
                addTower(0);
                constraint0 = constraint0 - 1;
                placing = true;
            }
		}
        // button for GREEN tower
        if (GUI.Button(new Rect(Screen.width - 135, 160, 110, 110), image: greentexture))
		{
            if (placing)
            {
                int type = currentTower.getTowerType();
                destroyTower(currentTower);
                if (type == 1) // if already placing that tower, stop placing
                {
                    placing = false;
                    constraint1 += 1;
                }
                else if (constraint1 > 0) // if placing another tower, switch to this one if one is left
                {
                    if (type == 0) { constraint0 += 1; }
                    if (type == 2) { constraint2 += 1; }
                    addTower(1);
                    constraint1 = constraint1 - 1;
                }
            }
            else if (constraint1 > 0) // if not already placing, start placing this one if one is left
            {
                addTower(1);
                constraint1 = constraint1 - 1;
                placing = true;
            }
		}
		// button for BLUE tower
		if (GUI.Button(new Rect(Screen.width - 135, 295, 110, 110), image: bluetexture))
		{
            if (placing)
            {
                int type = currentTower.getTowerType(); // previous tower type
                destroyTower(currentTower);
                if (type == 2) // if already placing that tower, stop placing
                {
                    placing = false;
                    constraint2 += 1;
                }
                else if (constraint2 > 0) // if placing another tower, switch to this one if one is left
                {
                    if (type == 0) { constraint0 += 1; }
                    if (type == 1) { constraint1 += 1; }
                    addTower(2);
                    constraint2 = constraint2 - 1;
                }
            }
            else if (constraint2 > 0) // if not already placing, start placing this one if one is left
            {
                addTower(2);
                constraint2 = constraint2 - 1;
                placing = true;
            }
            
		}
	}
}