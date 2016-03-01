﻿using UnityEngine;
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

	// Beat tracking
	private float clock;
	private float startTime;
	private float BEAT = .5f;
	private int numBeats = 0;

	// Use this for initialization
	void Start()
	{

		numTiles = 0;
		placing = false;
		currentbullets = new List<int>();	// list of current bullets on the board in terms of tile number

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

		addEnemies ();
		buildBoard();

	}

	// Update is called once per frame
	void Update()
	{
		// Beat counting
		clock = clock + Time.deltaTime;

		if (clock - startTime >= BEAT) {	
			startTime = clock; // Resets counter for next beat

			// Makes every tower fire
			for (int i = 0; i < towers.Count; i++) {
				towers [i].shoot (numBeats);
			}

			// Makes every enemy move
			for (int i = 0; i < enemies.Count; i++){
				enemies [i].move (numBeats);
			}
			numBeats++;
		}
			
		// Update the current bullets
		for (int i = 0; i < towers.Count; i++){
			List<Bullet> bullets = towers [i].getBullets();
			for (int j = 0; j < bullets.Count; j++) {
				int a = onTile(bullets [j].transform.position.x, bullets [j].transform.position.y);
				if (!currentbullets.Contains (a)) {
					currentbullets.Add (a);
				}
			}
		}
	
		// Update interaction between enemies and bullets
		for (int i = 0; i < enemies.Count; i++){
			Enemy enemy = enemies [i];
			int h = enemy.getHealth ();
			for (int j = 0; j < currentbullets.Count; j++) {
				int b = onTile(enemy.getX(), enemy.getY());
				if (b == currentbullets[j]) {
					enemy.damage (numBeats);
					if (enemy.getHealth() == 0) {
						enemy.destroy ();
						enemies.Remove (enemy);
					}
					break;
				}
			}
		}
		currentbullets.Clear ();
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

	// creates a tower (hiden at first) which is then moved around as the player mouses over tiles
	private void addTower(int type)
	{
		GameObject towerObject = new GameObject(); // create empty game object
		Tower tower = towerObject.AddComponent<Tower>(); // add tower script to object
		tower.transform.parent = towerFolder.transform; // make the tower folder its parent
		tower.transform.position = new Vector3(0, 0, 0); // set location in relation to global coords
		tower.init(type, this); // initialize tower
		tower.name = "Tower " + towers.Count; // name tower for easy finding
		towers.Add(tower); // add tower into the towers list
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
		tower.destroy();
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

	// add enemies
	public void addEnemies(){
		/*int i;
		for (i = 0; i < boardHeight; i++) {
			int type = i % 3;
			if (i % 2 == 1) {
				addEnemy (type, 0, i);
			}
		}*/
		addEnemy (2, -1, 5);
		addEnemy (2, -3, 5);
	}

	// add a single enemy
	public void addEnemy(int enemyType, int x, int y){
		GameObject enemyObject = new GameObject();
		Enemy enemy = enemyObject.AddComponent<Enemy>();
		enemy.transform.parent = enemyFolder.transform;
		enemy.transform.position = new Vector3 (x, y, 0);
		enemy.init (enemyType, this, x, y);
		enemies.Add (enemy);
		enemy.name = "Enemy " + enemies.Count;
	}

	// check the tile the object is on
	int onTile(float x, float y){
		int a = (int) Math.Round(x);
		int b = (int) Math.Round(y);
		int ontile = a * boardWidth + b;
		return ontile;
	}

	// logic for the GUI
	void OnGUI()
	{
		if (placing)
		{
			// if the rotate button is pressed
			if (GUI.Button(new Rect(Screen.width-135, Screen.height - 250, 110, 30), "ROTATE"))
			{
				currentTower.rotate(); // rotate the tower being placed
			}
		}
		// if the RED button is placed
		if (GUI.Button(new Rect(25, Screen.height - 250, 110, 30), "RED"))
		{
			// if a tower was already being placed
			if (placing)
			{
				destroyTower(currentTower); // destroy the tower currently being placed
				// if the tower was red
				if (currentTower.getTowerType() == 0)
				{
					placing = false; // player no longer wants to place the tower
				}
				else
				{
					addTower(0); //  player wants to place a RED tower
				}
			}
			else
			{
				addTower(0); // player wants to begin placing a tower
				placing = true;
			}
		}
		// same as above but for GREEN button
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
		// same as above but for BLUE button
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