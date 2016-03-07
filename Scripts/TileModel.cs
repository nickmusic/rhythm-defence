using UnityEngine;
using System.Collections;

public class TileModel : MonoBehaviour {

    private Tile owner;
    private Material mat;

    public void init(Tile t)
    {
        owner = t;

        transform.parent = owner.transform;
        transform.localPosition = new Vector3(0, 0, 0);

        mat = GetComponent<Renderer>().material;
        mat.shader = Shader.Find("Sprites/Default");
        mat.mainTexture = Resources.Load<Texture2D>("Textures/tileBlank");
        mat.color = new Color(1, 1, 1);
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public Material getMat()
    {
        return mat;
    }

    // when the tile is moused over
    void OnMouseOver()
    {
        if (owner.manager.isPlacing())
        {
	    // move the current tower to this tile
            Tower tower = owner.manager.getCurrent();
            Vector3 vector = owner.transform.position;
            vector.z = -1;
            tower.transform.position = vector;
        }
        else
        {
	    // highlight the tile under the mouse
            mat.color = Color.grey;
        }
    }
    
    // when the mouse leaves the tile
    void OnMouseExit()
    {
        if (!owner.manager.isPlacing())
        {
	    //reset color of texture
            mat.color = new Color(1, 1, 1, 1);
        }
    }
    
}
