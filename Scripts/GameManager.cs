// The gamemanager of the rhythm-defense game
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

    Texture2D levels; // texture for blue tower
    Texture2D restart;
    Texture2D start;
    Texture2D next;

    // Beat tracking
    private float clock;
    private float startTime;
    private float BEAT = .5f;
    private int numBeats = 0;
    int enemybeaten = 0;
    int enemynum = 0;

    // Level number

    private int level = 99;


    //button locations
    float trayx = 0;
    float traywidth = 0;
    float trayspace = 0;

    // Sound stuff
    public AudioSource music;
    public AudioSource sfx;

    // Music clips
    private AudioClip idle;
    private AudioClip gametrack;
    private AudioClip winmusic;

    // Sound effect clips
    private AudioClip enemyDead;
    private AudioClip enemyHit;
    private AudioClip click;

    //stuff for indicator
    private GameObject indicatorFolder;
    private List<IndicatorTile> indicatorTiles;

    private GameObject titlePage;
    private GameObject cover;
    // Use this for initialization
    void Start()
    {

        numTiles = 0;
        placing = false;
        started = false;
        currentbullets = new List<int>();   // list of current bullets on the board in terms of tile number


        redtexture = Resources.Load<Texture2D>("Textures/redTower");
        greentexture = Resources.Load<Texture2D>("Textures/greenTower");
        bluetexture = Resources.Load<Texture2D>("Textures/blueTower");
        levels = Resources.Load<Texture2D>("Textures/levels");
        restart = Resources.Load<Texture2D>("Textures/restart");
        start = Resources.Load<Texture2D>("Textures/start");
        next = Resources.Load<Texture2D>("Textures/next");
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

        //create indicator stuff
        indicatorFolder = new GameObject();
        indicatorFolder.name = "Indicator Tiles";
        //indicator = new IndicatorTile[4, boardHeight];
        indicatorTiles = new List<IndicatorTile>();

        makeLevel();
        buildBoard();
        makeOverlay();

        //set the camera based on aspect ratio
        Camera cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        cam.transform.position = new Vector3(4, 3, -10);
        float aspect = (float)Math.Round(cam.aspect, 2);
        if (aspect == 1.25) //5:4
        {
            cam.orthographicSize = 6.75f;
            trayx = Screen.width - (Screen.width / 8);
            traywidth = Screen.width / 9;
            trayspace = Screen.height / 48;
        }
        else if (aspect == 1.33f) //4:3
        {
            cam.orthographicSize = 6.25f;
            trayx = Screen.width - (Screen.width / 8.5f);
            traywidth = Screen.width / 9.5f;
            trayspace = Screen.height / 46;
        }
        else if (aspect == 1.5f) //3:2
        {
            cam.orthographicSize = 5.75f;
            trayx = Screen.width - (Screen.width / 8);
            traywidth = Screen.width / 9;
            trayspace = Screen.height / 36;
        }
        else if (aspect == 1.6f) //16:10
        {
            cam.orthographicSize = 5.25f;
            trayx = Screen.width - (Screen.width / 8.5f);
            traywidth = Screen.width / 9.5f;
            trayspace = Screen.height / 44;
        }
        else if (aspect == 1.78f) //16:9
        {
            cam.orthographicSize = 4.85f; // 5:4
            trayx = Screen.width - (Screen.width / 8);
            traywidth = Screen.width / 9;
            trayspace = Screen.height / 30;
        }

        titlePage = GameObject.CreatePrimitive(PrimitiveType.Quad);
        Material mat = titlePage.GetComponent<Renderer>().material;
        mat.shader = Shader.Find("Sprites/Default");
        mat.mainTexture = Resources.Load<Texture2D>("Textures/titlePage");
        mat.color = new Color(1, 1, 1);
        titlePage.transform.position = new Vector3(4, 3, -2);
        titlePage.transform.localScale = new Vector3(20, 14, 0);

        // setting up music
        SoundSetUp();

        PlayMusic(idle);
    }


    public float getBeat()
    {
        return BEAT;
    }


    private void SoundSetUp()
    {
        // music
        idle = Resources.Load<AudioClip>("Music/title song");
        gametrack = Resources.Load<AudioClip>("Music/Main song loop");
        winmusic = Resources.Load<AudioClip>("Music/You Win Song");

        // sfx
        enemyDead = Resources.Load<AudioClip>("Music/enemy defeated");
        enemyHit = Resources.Load<AudioClip>("Music/enemy hit by tower");
        click = Resources.Load<AudioClip>("Music/Mouse Click");

    }

    // Update is called once per frame
    void Update()
    {
        if (enemybeaten > 0 && enemynum == enemybeaten && music.clip != winmusic)
        {
            PlayMusic(winmusic);
        }

        if (this.started)
        {
            // Beat counting
            clock = clock + Time.deltaTime;

            if (clock - startTime >= BEAT)
            {
                startTime = clock; // Resets counter for next beat

                // Makes every tower fire
                for (int i = 0; i < towers.Count; i++)
                {
                    towers[i].shoot(numBeats);
                }

                // Makes every enemy move
                for (int i = 0; i < enemies.Count; i++)
                {
                    enemies[i].move(numBeats);
                }
                numBeats++;
            }

            // Update the current bullets
            for (int i = 0; i < towers.Count; i++)
            {
                List<Bullet> bullets = towers[i].getBullets();
                for (int j = 0; j < bullets.Count; j++)
                {
                    int a = onTile(bullets[j].getX(), bullets[j].getY());
                    if (!currentbullets.Contains(a))
                    {
                        currentbullets.Add(a);
                    }
                }
            }

            // Update interaction between enemies and bullets
            for (int i = 0; i < enemies.Count; i++)
            {
                Enemy enemy = enemies[i];
                int h = enemy.getHealth();
                if (enemy.getHealth() == 0)
                {
                    PlayEffect(enemyDead);
                    enemy.destroy();
                    enemies.Remove(enemy);
                    enemybeaten += 1;
                }
                else {
                    for (int j = 0; j < currentbullets.Count; j++)
                    {
                        int b = onTile(enemy.getX(), enemy.getY());
                        if (b == currentbullets[j])
                        {
                            enemy.damage(numBeats);
                            break;
                        }
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
        tile.transform.position = new Vector3(x, y, 1);
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
            int x = (int)towers[i].transform.position.x;
            int y = (int)towers[i].transform.position.y;
            board[x, y].setHasTower(false);
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


    private void makeLevel()
    {
        if (level != 100 && level != 99)
        {
            addEnemies();
            setConstraints();
        }
    }



    public void resetLevel()
    {
        destroyTowers();
        if (placing) { DestroyImmediate(currentTower.gameObject); }
        destroyEnemies();
        //destroyBoard();
        numBeats = 0;
        started = false;
        placing = false;
        enemynum = 0;
        enemybeaten = 0;
        //numTiles = 0;
        destoryIndicator();

        PlayMusic(idle);

        makeLevel();


    }

    public void restartLevel()
    {
        destroyEnemies();
        destroyBoard();
        destroyBullets();
        destoryIndicator();
        numBeats = 0;
        started = false;
        addEnemies();
        buildBoard();
    }

    // set constraints based on level
    private void setConstraints()
    {
        // Noel's levels
        if (level == 9)
        {
            // 2 health blobs, every other beat
            constraint0 = 0;
            constraint1 = 0;
            constraint2 = 2;
        }
        else if (level == 1)
        {
            // 1 health blobs, every other beat
            constraint0 = 1;
            constraint1 = 0;
            constraint2 = 0;
        }
        else if (level == 2)
        {
            // 1 health blobs, every beat
            constraint0 = 2;
            constraint1 = 0;
            constraint2 = 0;
        }
        else if (level == 3)
        {
            // Stack of two 1 health blobs
            constraint0 = 2;
            constraint1 = 0;
            constraint2 = 0;
        }
        else if (level == 4)
        {
            // same as previous (stack of two)
            constraint0 = 0;
            constraint1 = 2;
            constraint2 = 0;
        }
        else if (level == 8)
        {
            // stack of three 1 health blobs
            constraint0 = 1;
            constraint1 = 2;
            constraint2 = 0;
        }
        else if (level == 6)
        {
            constraint0 = 0;
            constraint1 = 0;
            constraint2 = 1;
        }

        // Jun's levels
        else if (level == 11)
        {
            constraint0 = 1;
            constraint1 = 0;
            constraint2 = 1;
        }
        else if (level == 13)
        {
            constraint0 = 2;
            constraint1 = 1;
            constraint2 = 0;
        }
        else if (level == 12)
        {
            constraint0 = 0;
            constraint1 = 0;
            constraint2 = 3;
        }

        // Luxing's levels
        if (level == 5)
        {
            constraint0 = 0;
            constraint1 = 2;
            constraint2 = 0;
        }
        else if (level == 14)
        {
            constraint0 = 0;
            constraint1 = 0;
            constraint2 = 4;

        }
        else if (level == 15)
        {
            constraint0 = 0;
            constraint1 = 0;
            constraint2 = 3;
        }
        else if (level == 7)
        {
            constraint0 = 3;
            constraint1 = 0;
            constraint2 = 0;
        }
        else if (level == 10)
        {
            constraint0 = 1;
            constraint1 = 3;
            constraint2 = 0;
        }
    }



    // add enemies
    private void addEnemies()
    {
        // Noel's levels
        if (level == 9)
        {
            for (int i = 0; i < 6; i++)
            {
                addEnemy(2, 2, (i * -2) - 1, 5);
            }
        }
        else if (level == 1)
        {
            for (int i = 0; i < 6; i++)
            {
                addEnemy(3, 1, (i * -2) - 1, 5);
            }
        }
        else if (level == 2)
        {
            for (int i = 0; i < 6; i++)
            {
                addEnemy(3, 1, (i * -2) - 1, 5);
                addEnemy(3, 1, (i * -2) - 2, 5);
            }
        }
        else if (level == 3 || level == 4 || level == 8)
        {
            for (int i = 0; i < 6; i++)
            {
                addEnemy(3, 1, (i * -2) - 1, 5);
                addEnemy(3, 1, (i * -2) - 1, 4);
                if (level == 8)
                {
                    addEnemy(3, 1, (i * -2) - 1, 3);
                }
            }
        }
        else if (level == 6)
        {
            for (int i = 0; i < 6; i++)
            {
                addEnemy(3, 1, (i * -4) - 1, 5);
                addEnemy(3, 1, (i * -4) - 1, 4);
                addEnemy(3, 1, (i * -4) - 3, 3);
            }

        }

        // Jun's levels
        else if (level == 11)
        {
            for (int i = 0; i < 6; i++)
            {
                addEnemy(3, 1, (i * -4) - 1, 5);
                addEnemy(3, 1, (i * -4) - 1, 4);
                addEnemy(3, 1, (i * -2) - 1, 3);
            }
        }
        else if (level == 13)
        {
			for (int i = 0; i < 6; i++) {
				addEnemy (2, 1, -2+(i * -4), 3);
				addEnemy (2, 1, -2+(i * -4), 4);
				addEnemy (2, 1, -2+(i * -4), 5);
				addEnemy (2, 1, -1+(i * -4), 4);
				addEnemy (2, 1, -3+(i * -4), 4);
			}
        }
        else if (level == 12)
        {


            for (int i = 0; i < 6; i++)
            {
                addEnemy(2, 1, (i * -2) - 1, 5);
                addEnemy(2, 1, (i * -2) - 1, 4);
                addEnemy(3, 1, (i * -4) - 3, 3);
                addEnemy(3, 1, (i * -4) - 3, 6);

            }
        }


        // Luxing's levels
        else if (level == 5)
        {
			for (int i = 0; i < 4; i++) {
				addEnemy (2, 1, -1+(i * -4), 3);
				addEnemy (2, 1, -3+(i * -4), 3);
				addEnemy (1, 2, -1+(i * -4), 4);
			}
        }
        else if (level == 14)
        {
            for (int i = 0; i < 6; i++)
            {
                addEnemy(2, 1, (i * -2) - 1, 5);
            }
            for (int i = 0; i < 4; i++)
            {
                addEnemy(2, 2, (i * -2) - 1, 3);
            }
            for (int i = 0; i < 6; i++)
            {
                addEnemy(2, 1, (i * -2) - 1, 1);
            }
        }
        else if (level == 15)
        {
            addEnemy(2, 1, -1, 3);
            addEnemy(1, 2, -1, 4);
            addEnemy(0, 3, -1, 5);
            addEnemy(2, 1, -3, 5);
            addEnemy(1, 2, -3, 4);
            addEnemy(0, 3, -3, 3);
        }
        else if (level == 7)
        {
            for (int i = 0; i < 6; i++)
            {
                addEnemy(2, 1, (i * -2) - 1, 5);
            }
            for (int i = 0; i < 4; i++)
            {
                addEnemy(2, 2, (i * -2) - 1, 3);
            }
        }
        else if (level == 10)
        {
            addEnemy(2, 1, -1, 5);
            addEnemy(2, 1, -2, 5);
            addEnemy(2, 1, -1, 6);
            addEnemy(2, 1, -2, 6);
            addEnemy(1, 2, -1, 2);
        }

    }

    // add a single enemy
    public void addEnemy(int enemyType, int initHealth, int x, int y)
    {
        GameObject enemyObject = new GameObject();
        Enemy enemy = enemyObject.AddComponent<Enemy>();
        enemy.transform.parent = enemyFolder.transform;
        enemy.transform.position = new Vector3(x, y, 0);
        enemy.init(enemyType, initHealth, this, x, y);
        enemies.Add(enemy);
        enemynum++;
        enemy.name = "Enemy " + enemies.Count;

        addIndicatorTile(enemyType, initHealth, x, y);
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
    int onTile(float x, float y)
    {
        int a = (int)Math.Round(x);
        int b = (int)Math.Round(y);
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


	if (level == 100){ //level selction

            for (int i = 1; i < 7; i++) {
                for (int j = 1; j < 3; j++)
                {
                    int t = (j - 1) * 6 + i;
                    GUIStyle BStyle = new GUIStyle(GUI.skin.GetStyle("Button"));
                    BStyle.fontSize = 25;
                    if (GUI.Button(new Rect(Screen.width / 8 + (i - 1) * Screen.width / 8, Screen.height / 5 + (j - 1) * Screen.height / 5, Screen.width / 8 - Screen.width / 8 / 4, Screen.width / 8 - Screen.width / 8 / 4), t.ToString(), BStyle))
                    {
                        resetLevel();
                        level = t;
                        makeLevel();
                        DestroyImmediate(cover);
                    }
                }
        }
		for (int i = 1; i < 4; i++) {
			 int j = 3;
				int t=(j-1)*6+i;
			GUIStyle BStyle = new GUIStyle (GUI.skin.GetStyle("Button"));
        	BStyle.fontSize = 25;
            if (GUI.Button(new Rect(Screen.width/8+(i-1)*Screen.width/8, Screen.height/5+(j-1)*Screen.height/5, Screen.width/8-Screen.width/8/4, Screen.width/8-Screen.width/8/4), t.ToString(), BStyle)) {
					resetLevel ();
	                level = t;
					makeLevel();
                    DestroyImmediate(cover);
                }
            
        }

            if (GUI.Button(new Rect(25, Screen.height - 55, 110, 30), "QUIT (Esc)") ||Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }


        }

        if (level == 99)
        { //level selction
             GUIStyle myStyle = new GUIStyle(GUI.skin.GetStyle("label"));
            myStyle.fontSize = 40;

            if (GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height / 2, 200, 50), "START GAME"))
            {
                level = 1;
                makeLevel();
                DestroyImmediate(titlePage);
            }
            if (GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height / 2 + 150, 200, 50), "SELECT LEVEL"))
            {
                level = 100;
                makeLevel();
                DestroyImmediate(titlePage);

                //cover game board temporarily
                cover = GameObject.CreatePrimitive(PrimitiveType.Quad);
                Material mat = cover.GetComponent<Renderer>().material;
                mat.shader = Shader.Find("Sprites/Default");
                mat.mainTexture = Resources.Load<Texture2D>("Textures/backdrop");
                mat.color = new Color(1, 1, 1);
                cover.transform.position = new Vector3(4, 3, -2);
                cover.transform.localScale = new Vector3(20, 14, 0);
            }

            if (GUI.Button(new Rect(25, Screen.height - 55, 110, 30), "QUIT (Esc)") || Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }


        }


        if (level != 100 && level != 99)
        {



       if (GUI.Button(new Rect(25, Screen.height - 55, 110, 30), "Select Levels") ){
       	    level = 100;
            makeLevel();

                //cover game board temporarily
                cover = GameObject.CreatePrimitive(PrimitiveType.Quad);
                Material mat = cover.GetComponent<Renderer>().material;
                mat.shader = Shader.Find("Sprites/Default");
                mat.mainTexture = Resources.Load<Texture2D>("Textures/backdrop");
                mat.color = new Color(1, 1, 1);
                cover.transform.position = new Vector3(4, 3, -2);
                cover.transform.localScale = new Vector3(20, 14, 0);
            }
        //labels for how many towers are left
        GUIStyle MyStyle = new GUIStyle (GUI.skin.GetStyle("label"));
        MyStyle.fontSize = 25;
            GUI.Label(new Rect(Screen.width / 2 - traywidth / 2, trayspace, traywidth*2, traywidth*2), "LEVEL " + level.ToString(),MyStyle);


            GUI.Label(new Rect(trayx + (traywidth / 2.17f), trayspace + traywidth, 110, 110), constraint0.ToString());
            GUI.Label(new Rect(trayx + (traywidth / 2.17f), trayspace * 2 + traywidth * 2, 110, 110), constraint1.ToString());
            GUI.Label(new Rect(trayx + (traywidth / 2.17f), trayspace * 3 + traywidth * 3, 110, 110), constraint2.ToString());
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }


            if (!started)
            {
                if (GUI.Button(new Rect(trayspace, trayspace, traywidth, traywidth / 3), "START (S)") || Input.GetKeyDown(KeyCode.S))
                {
                    started = true;
                    PlayEffect(click);
                    PlayMusic(gametrack);
                }
            }
            if (enemybeaten == enemynum)
            {
                GUIStyle myStyle = new GUIStyle(GUI.skin.GetStyle("label"));
                myStyle.fontSize = 80;
                GUIStyle myStyle2 = new GUIStyle(GUI.skin.GetStyle("label"));
                myStyle2.fontSize = 30;
                GUI.Label(new Rect(Screen.width / 2 - 600 / 2, 100, 600, 100), "YOU GOT IT!", myStyle);
                GUI.Label(new Rect(Screen.width / 2 - 250, 200, 500, 100), "Press 'Space' to next level", myStyle2);
                if (GUI.Button(new Rect(Screen.width / 2 - 80, 300, 80, 80), image: next) || Input.GetKeyDown(KeyCode.Space))
                {
                    enemynum = 0;
                    enemybeaten = 0;
                    level++;
                    resetLevel();

                }

            }




            if (placing)

            {
                // if the rotate button is pressed

                if (GUI.Button(new Rect(trayx, traywidth * 3 + trayspace * 4, traywidth, traywidth / 3), "ROTATE") )
                {
                    PlayEffect(click);
                    currentTower.rotate(); // rotate the tower being placed
                }
            }




            if (GUI.Button(new Rect(trayspace * 2 + traywidth, trayspace, traywidth, traywidth / 3), "RESTART") )

            {
                PlayEffect(click);
                resetLevel();

            }



            if (GUI.Button(new Rect(trayx, trayspace, traywidth, traywidth), image: redtexture) )
            {
                PlayEffect(click);

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
                    else
                    {
                        addTower(type);
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
        if (GUI.Button(new Rect(trayx, traywidth + trayspace * 2, traywidth, traywidth), image: greentexture) )
        {
            PlayEffect(click);


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
                    else
                    {
                        addTower(type);
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
        if (GUI.Button(new Rect(trayx, traywidth * 2 + trayspace * 3, traywidth, traywidth), image: bluetexture) )
        {
            PlayEffect(click);


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
                    else
                    {
                        addTower(type);
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


    //creates quads for backround images
    private void makeOverlay()
    {
        //the indicator to the left of the board
        var background = GameObject.CreatePrimitive(PrimitiveType.Quad);
        Material mat = background.GetComponent<Renderer>().material;
        mat.shader = Shader.Find("Sprites/Default");
        mat.mainTexture = Resources.Load<Texture2D>("Textures/indicator");
        mat.color = new Color(1, 1, 1);
        background.transform.position = new Vector3(-2.2f, 3, -1);
        background.transform.localScale = new Vector3(4, 8, 0);

        //decoration under the game board (also beat box)
        background = GameObject.CreatePrimitive(PrimitiveType.Quad);
        mat = background.GetComponent<Renderer>().material;
        mat.shader = Shader.Find("Sprites/Default");
        mat.mainTexture = Resources.Load<Texture2D>("Textures/wires");
        mat.color = new Color(1, 1, 1);
        background.transform.position = new Vector3(4, -1.5f, 1);
        background.transform.localScale = new Vector3(9, 2, 0);

        //panel on the right of the board for towers (includes port thing)
        background = GameObject.CreatePrimitive(PrimitiveType.Quad);
        mat = background.GetComponent<Renderer>().material;
        mat.shader = Shader.Find("Sprites/Default");
        mat.mainTexture = Resources.Load<Texture2D>("Textures/towerTray");
        mat.color = new Color(1, 1, 1);
        background.transform.position = new Vector3(12, 3, 1);
        background.transform.localScale = new Vector3(7, 14, 0);

        //the background (currently just grey)
        background = GameObject.CreatePrimitive(PrimitiveType.Quad);
        mat = background.GetComponent<Renderer>().material;
        mat.shader = Shader.Find("Sprites/Default");
        mat.mainTexture = Resources.Load<Texture2D>("Textures/backdrop");
        mat.color = new Color(1, 1, 1);
        background.transform.position = new Vector3(5, 3, 1);
        background.transform.localScale = new Vector3(24, 18, 0);

        //panel to conceal enemies
        background = GameObject.CreatePrimitive(PrimitiveType.Quad);
        mat = background.GetComponent<Renderer>().material;
        mat.shader = Shader.Find("Sprites/Default");
        mat.mainTexture = Resources.Load<Texture2D>("Textures/backdrop");
        mat.color = new Color(1, 1, 1);
        background.transform.position = new Vector3(-3, 3, -.5f);
        background.transform.localScale = new Vector3(5, 7, 0);
    }

    // Music section
    public void PlayEffect(AudioClip clip)
    {
        sfx.clip = clip;
        sfx.Play();
    }

    public void PlayMusic(AudioClip clip)
    {
        this.music.loop = true;
        this.music.clip = clip;
        this.music.Play();
    }

    //fills in the rectangle on the indicator that corresponds to the enemy
    private void addIndicatorTile(int type, int health, int x, int y)
    {
        if ((x < 0) && (x > -5))
        {
            GameObject indicatorObject = new GameObject(); // create empty game object
            IndicatorTile tile4 = indicatorObject.AddComponent<IndicatorTile>(); // add tile script to object
            tile4.transform.parent = indicatorFolder.transform; // make the tile folder its parent
            tile4.transform.position = new Vector3(x / 2f - 1.75f, y, -.75f);
            tile4.init(type, health, this); // initialize the tile
            indicatorTiles.Add(tile4);
            tile4.name = "Indicator Tile " + indicatorTiles.Count; // name tile for easy finding
        }
    }

    //removes all the indicator tiles and resets the list
    private void destoryIndicator()
    {
        for (int i = 0; i < indicatorTiles.Count; i++)
        {
            DestroyImmediate(indicatorTiles[i].gameObject);
        }
        indicatorTiles = null;
        indicatorTiles = new List<IndicatorTile>();
    }
}
