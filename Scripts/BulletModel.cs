using UnityEngine;
using System.Collections;

public class BulletModel : MonoBehaviour {

	private Bullet owner;			// Pointer to the parent object.
	private Material mat;		// Material for setting/changing texture and color.
	float clock;
	int ismoving;
	float x;
	float y;
	float beat;

	public void init(Bullet owner) {
		this.owner = owner;

		transform.parent = owner.transform;					// Set the model's parent to the gem.
		transform.localPosition = new Vector3(0,0,-1);		// Center the model on the parent.
		name = "Bullet Model";									// Name the object.

		mat = GetComponent<Renderer>().material;					// Get the material component of this quad object.
		mat.shader = Shader.Find ("Sprites/Default");	
		mat.mainTexture = Resources.Load<Texture2D>("Textures/explosion");	// Set the texture.  Must be in Resources folder.
		mat.color = new Color (1, 1, 1);
		ismoving = 0;
		x = 0;
		y = 0;
		beat = 0f;

	}


	// Use this for initialization
	void Start () {
		clock = 0f;
	}

	// Update is called once per frame
	void Update () {
		clock = clock + Time.deltaTime;
		if (ismoving == 1) {
			if (transform.position.x != x) {
				transform.position = new Vector3 (transform.position.x + beat*(x - transform.position.x), transform.position.y, 0);
			}
			if (transform.position.y != y) {
				transform.position = new Vector3 (transform.position.x, transform.position.y + beat*(y - transform.position.y), 0);
			}
		}
	}

	public void move(float x, float y, float beat){
		ismoving = 1;
		this.x = x;
		this.y = y;
		if (Mathf.Abs (x - transform.position.x) == 2 || Mathf.Abs (y - transform.position.y) == 2) {
			beat = beat * 2;
		}
	}
}
