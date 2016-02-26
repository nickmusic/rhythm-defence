using UnityEngine;
using System.Collections;

public class BulletModel : MonoBehaviour {

	private Bullet owner;			// Pointer to the parent object.
	private Material mat;		// Material for setting/changing texture and color.

	public void init(Bullet owner) {
		this.owner = owner;

		transform.parent = owner.transform;					// Set the model's parent to the gem.
		transform.localPosition = new Vector3(0,0,0);		// Center the model on the parent.
		name = "Bullet Model";									// Name the object.

		mat = GetComponent<Renderer>().material;					// Get the material component of this quad object.
		mat.shader = Shader.Find ("Sprites/Default");	
		mat.mainTexture = Resources.Load<Texture2D>("Textures/marble");	// Set the texture.  Must be in Resources folder.
		mat.color = new Color (8, 8, 8);

	}


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
