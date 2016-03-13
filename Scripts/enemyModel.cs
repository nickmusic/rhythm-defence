using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class enemyModel : MonoBehaviour
{
	private float clock;	// to keep track of the time(not used for now)
	private Enemy owner;	// object that created it
	private Material mat;	// material (for texture)
	private int enemyType;	// the type of the enemy(0, 1, 2)
	private int initHealth;
	private float moverhythm;	
	private float movebuf;
	private float healthcolor;
	private int healthval;
	private int beat;
	private int damagebuf;

	public void init(int enemyType, int initHealth, Enemy owner) {
		this.owner = owner;
		healthcolor = 1;
		this.enemyType = enemyType;
		healthval = initHealth;
		beat = 0;
		damagebuf = 0;

		// set up rhythm of enemy
		if (enemyType == 0) {
			moverhythm = 4;
		} else if (enemyType == 1) {
			moverhythm = 2;
		} else if (enemyType == 2 || enemyType == 3) {
			moverhythm = 1;
		}

		movebuf = 0;

		transform.parent = owner.transform;	
		transform.localPosition = new Vector3(0,0,0);
		name = "Enemy Model";
		if (enemyType == 0) {
			mat = GetComponent<Renderer> ().material;	
			mat.shader = Shader.Find ("Sprites/Default");
			mat.mainTexture = Resources.Load<Texture2D> ("Textures/ghoul");
			mat.color = new Color (1, 1, 1, 1);
		} else if (enemyType == 1) {
			mat = GetComponent<Renderer> ().material;		
			mat.shader = Shader.Find ("Sprites/Default");	
			mat.mainTexture = Resources.Load<Texture2D> ("Textures/spider");	
			mat.color = new Color (1, 1, 1, 1);
		} else if (enemyType == 2 || enemyType == 3) {
			mat = GetComponent<Renderer> ().material;								
			mat.shader = Shader.Find ("Sprites/Default");						
			mat.mainTexture = Resources.Load<Texture2D> ("Textures/slimeGrey");	
			mat.color = new Color (1, 1, 1, 1);
		}
	}

	void Start(){
		clock = 0f;
	}

	void Update(){
		clock += Time.deltaTime;

		if (healthval >= 3) {
			mat.color = new Color (1, 8, 1);
		} else if (healthval == 2) {
			mat.color = new Color (8, 8, 1);
		} else if (healthval  == 1) {
			mat.color = new Color (8, 1, 1);
		}
	}

	public void damage(int numBeats){
		if (numBeats >= 1 + damagebuf) {
			damagebuf = numBeats;
			healthcolor -= (float)(0.4-0.1*(moverhythm/2));
			mat.color = new Color (1, 1, 1, healthcolor);
			healthval -= 1;
		}

	}

	public void destroy(){
		DestroyImmediate (gameObject);
	}

	public void move(int numBeats){
		if (this != null) {
			if ((numBeats % moverhythm == 0) && owner.m.isStarted()) {
				if (transform.position.x < owner.m.boardWidth) {
					transform.position = new Vector3 (transform.position.x + 1, transform.position.y);
				}
			}
		}
	}

	void OnGUI(){
		if(transform.position.x == owner.m.boardWidth){
            GUIStyle myStyle = new GUIStyle (GUI.skin.GetStyle("label"));
         	myStyle.fontSize = 70;
     		GUI.Label(new Rect(Screen.width/2-600/2, 100, 600, 100), "Press 'R' to restart",myStyle);

	}
	}

	public float getX(){
		return transform.position.x;
	}

	public float getY(){
		return transform.position.y;
	}

	public int getHealth(){
		return healthval;
	}

}