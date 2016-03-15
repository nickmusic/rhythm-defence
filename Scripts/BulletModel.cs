using UnityEngine;
using System.Collections;

public class BulletModel : MonoBehaviour {

	private Bullet owner;			// Pointer to the parent object.
	private Material mat;		// Material for setting/changing texture and color.
	float clock;
	float x;
	float y;
	float beat;
	int direction;
	private int movetoX;
	private int movetoY;

	public void init(Bullet owner) {
		this.owner = owner;

		transform.parent = owner.transform;					// Set the model's parent to the gem.
		transform.localPosition = new Vector3(0,0,-1);		// Center the model on the parent.
		name = "Bullet Model";									// Name the object.

		mat = GetComponent<Renderer>().material;					// Get the material component of this quad object.
		mat.shader = Shader.Find ("Sprites/Default");	
		mat.mainTexture = Resources.Load<Texture2D>("Textures/explosion");	// Set the texture.  Must be in Resources folder.
		mat.color = new Color (1, 1, 1);
		x = transform.position.x;
		y = transform.position.y;
		beat = 0f;
		movetoX = (int)transform.position.x;
		movetoY = (int)transform.position.y;
	}


	// Use this for initialization
	void Start () {
		clock = 0f;
	}

	// Update is called once per frame
	void Update () {
		clock = clock + Time.deltaTime;


		// go up
		/*if (Mathf.Abs (x - transform.position.x) >= 0.1 || Mathf.Abs (y - transform.position.y) >= 0.1) {
			print ("aa");
			transform.position = new Vector3 (this.transform.position.x + beat * (direction % 2) * (direction - 2), 
				this.transform.position.y + beat * ((1 + direction) % 2) * (1 - direction));
			print (transform.position.y);
			print ("y: " + y);
		}

		/*if (transform.position.x != x) {
			transform.position = new Vector3 (transform.position.x+beat, transform.position.y, 0);
		}
		if (transform.position.y != y) {
			transform.position = new Vector3 (transform.position.x, transform.position.y+beat, 0);
		}*/
		/*if (transform.position.x < x) {
			transform.position = new Vector3 (transform.position.x+beat*2, transform.position.y, 0);
			print ("aa");
		}*/
	if (Mathf.Abs(transform.position.x - movetoX)>=0.0001) {
			float diff = movetoX - transform.position.x;
			transform.position = new Vector3 (transform.position.x + (Mathf.Abs(diff)/diff)*beat / 5, transform.position.y, -1);
		}
	if (Mathf.Abs(transform.position.y - movetoY)>=0.0001) {
		float diff = movetoY - transform.position.y;
		transform.position = new Vector3 (transform.position.x, transform.position.y + (Mathf.Abs(diff)/diff)*beat / 5, -1);
	}
		//transform.position = new Vector3 (transform.position.x+owner.m.getBeat()/5, transform.position.y, 0);
		//print (transform.position.x);

	}

	public void move(int x, int y, float beat, int direction){
		movetoX = x;
		movetoY = y;
		this.beat = beat;
		this.direction = direction;
	}

	public int getX(){
		return movetoX;
	}

	public int getY(){
		return movetoY;
	}
}
