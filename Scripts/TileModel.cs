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

    void OnMouseOver()
    {
        if (owner.manager.isPlacing())
        {
            Tower tower = owner.manager.getCurrent();
            Vector3 vector = owner.transform.position;
            vector.z = -1;
            tower.transform.position = vector;
        }
        else
        {
            mat.color = Color.grey;
        }
    }
    
    void OnMouseExit()
    {
        if (!owner.manager.isPlacing())
        {
            mat.color = new Color(1, 1, 1, 1);
        }
    }
    
}
