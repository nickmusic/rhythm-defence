using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{

    public int boardWidth, boardHeight;
    private GameObject tileFolder, towerFolder;
    private int numTiles;
    private Tile[,] board;
    private List<Tower> towers;
    private Tower currentTower;
    private bool placing;

    // Use this for initialization
    void Start()
    {

        numTiles = 0;
        placing = false;

        tileFolder = new GameObject();
        tileFolder.name = "Tiles";
        board = new Tile[boardWidth, boardHeight];

        towerFolder = new GameObject();
        towerFolder.name = "Towers";
        towers = new List<Tower>();

        buildBoard();

    }

    // Update is called once per frame
    void Update()
    {

    }

    //adds a single game tile to the location x, y (unity units)
    private void addTile(int x, int y)
    {
        GameObject tileObject = new GameObject();
        Tile tile = tileObject.AddComponent<Tile>();
        tile.transform.parent = tileFolder.transform;
        tile.transform.position = new Vector3(x, y, 0);
        tile.init(x, y, this);
        board[x, y] = tile;
        tile.name = "Tile " + numTiles;
        numTiles += 1;
    }

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

    private void addTower(int type)
    {
        GameObject towerObject = new GameObject();
        Tower tower = towerObject.AddComponent<Tower>();
        tower.transform.parent = towerFolder.transform;
        tower.transform.position = new Vector3(0, 0, 0);
        tower.init(type, this);
        tower.name = "Tower " + towers.Count;
        currentTower = tower;
    }

    public bool placeTower()
    {
        int x = (int)currentTower.transform.position.x;
        int y = (int)currentTower.transform.position.y;
        if (!(board[x, y].HasTower()))
        {
            towers.Add(currentTower);
            currentTower.transform.position = new Vector3(x, y, -.5f);
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

    public void destroyTower(Tower tower)
    {
        tower.destroy();
    }

    public Tower getCurrent()
    {
        return currentTower;
    }

    public bool isPlacing()
    {
        return placing;
    }

    public void highlight(int x, int y)
    {
        if ((x >= 0) && (x < boardWidth) && (y >= 0) && (y < boardHeight))
        {
            board[x, y].getModel().getMat().color = Color.green;
        }
    }

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

    void OnGUI()
    {
        if (placing)
        {
            if (GUI.Button(new Rect(Screen.width-135, Screen.height - 250, 110, 30), "ROTATE"))
            {
                currentTower.rotate();
            }
        }
        if (GUI.Button(new Rect(25, Screen.height - 250, 110, 30), "RED"))
        {
            if (placing)
            {
                destroyTower(currentTower);
                if (currentTower.getTowerType() == 0)
                {
                    placing = false;
                }
                else
                {
                    addTower(0);
                }
            }
            else
            {
                addTower(0);
                placing = true;
            }
        }
        if (GUI.Button(new Rect(25, Screen.height - 150, 110, 30), "GREEN"))
        {
            if (placing)
            {
                destroyTower(currentTower);
                if (currentTower.getTowerType() == 1)
                {
                    placing = false;
                }
                else
                {
                    addTower(1);
                }
            }
            else
            {
                addTower(1);
                placing = true;
            }
        }
        if (GUI.Button(new Rect(25, Screen.height - 50, 110, 30), "BLUE"))
        {
            if (placing)
            {
                destroyTower(currentTower);
                if (currentTower.getTowerType() == 2)
                {
                    placing = false;
                }
                else
                {
                    addTower(2);
                }
            }
            else
            {
                addTower(2);
                placing = true;
            }
        }
    }
}