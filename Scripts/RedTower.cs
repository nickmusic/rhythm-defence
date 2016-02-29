<<<<<<< HEAD
﻿using UnityEngine;
using System.Collections;

public class RedTower : MonoBehaviour {

    private Tower owner;
    private Material mat;
    private bool isPlaced;

    public void init(Tower t)
    {
        this.owner = t;
        isPlaced = false;

        transform.parent = owner.transform;
        transform.localPosition = new Vector3(0, 0, 0);

        mat = GetComponent<Renderer>().material;
        mat.shader = Shader.Find("Sprites/Default");
        mat.mainTexture = Resources.Load<Texture2D>("Textures/redTower");
        mat.color = new Color(1, 1, 1);
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

    void OnMouseExit()
    {
        if (owner.manager.isPlacing() && (!isPlaced))
        {
            Vector3 vector = owner.transform.position;
            vector.z = 1;
            owner.transform.position = vector;
            owner.manager.resetColors();
        }
        owner.manager.resetColors();
    }

    void OnMouseOver()
    {
        //if (!isPlaced)
        //{
            int x = (int)owner.transform.position.x;
            int y = (int)owner.transform.position.y;
            if ((int)owner.transform.eulerAngles.z == 0)
            {
                owner.manager.highlight(x, y + 1);
                owner.manager.highlight(x, y + 2);
            }
            else if ((int)owner.transform.eulerAngles.z == 90)
            {
                owner.manager.highlight(x - 1, y);
                owner.manager.highlight(x - 2, y);
            }
            else if ((int)owner.transform.eulerAngles.z == 180)
            {
                owner.manager.highlight(x, y - 1);
                owner.manager.highlight(x, y - 2);
            }
            else if ((int)owner.transform.eulerAngles.z == 270)
            {
                owner.manager.highlight(x + 1, y);
                owner.manager.highlight(x + 2, y);
            }
        //}
        if (Input.GetMouseButtonDown(0))
        {
            if (!isPlaced)
            {
                bool test = owner.manager.placeTower();
                isPlaced = test;
                owner.manager.resetColors();
            }
        }
    }
}
=======
﻿using UnityEngine;
using System.Collections;

public class RedTower : MonoBehaviour {

    private Tower owner; // object that created it
    private Material mat; // material (for texture)
    private bool isPlaced; // whether the tower has been placed

    public void init(Tower t)
    {
        this.owner = t;
        isPlaced = false;

        transform.parent = owner.transform; // make this objects location the same as the parent
        transform.localPosition = new Vector3(0, 0, 0); // set relative position

        mat = GetComponent<Renderer>().material; // get the material component
        mat.shader = Shader.Find("Sprites/Default"); // shader
        mat.mainTexture = Resources.Load<Texture2D>("Textures/redTower"); // assign redTower texture
        mat.color = new Color(1, 1, 1); // color
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

    // when the mouse moves to a different tile
    void OnMouseExit()
    {
	// if the tower is still being placed
        if (owner.manager.isPlacing() && (!isPlaced))
        {
            Vector3 vector = owner.transform.position; // move the tower to that tile
            vector.z = 1;
            owner.transform.position = vector;
            owner.manager.resetColors();
        }
        owner.manager.resetColors();
    }

    void OnMouseOver()
    {
        //if (!isPlaced)
        //{
	    // tell the manager which tiles to highlight (the tiles that will be targeted)
            int x = (int)owner.transform.position.x;
            int y = (int)owner.transform.position.y;
            if ((int)owner.transform.eulerAngles.z == 0)
            {
                owner.manager.highlight(x, y + 1);
                owner.manager.highlight(x, y + 2);
            }
            else if ((int)owner.transform.eulerAngles.z == 90)
            {
                owner.manager.highlight(x - 1, y);
                owner.manager.highlight(x - 2, y);
            }
            else if ((int)owner.transform.eulerAngles.z == 180)
            {
                owner.manager.highlight(x, y - 1);
                owner.manager.highlight(x, y - 2);
            }
            else if ((int)owner.transform.eulerAngles.z == 270)
            {
                owner.manager.highlight(x + 1, y);
                owner.manager.highlight(x + 2, y);
            }
        //}
	
	// if the mouse is click over the tile
        if (Input.GetMouseButtonDown(0))
        {
            if (!isPlaced) // if the tower has not been placed yet
            {
                bool test = owner.manager.placeTower();
                isPlaced = test;
                owner.manager.resetColors();
            }
        }
    }
}
>>>>>>> master
