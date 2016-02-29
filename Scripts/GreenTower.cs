using UnityEngine;
using System.Collections;

public class GreenTower : MonoBehaviour
{

    private Tower owner; // object that created it
    private Material mat; // material (for texture)
    private bool isPlaced; // whether the tower has been placed

    // called by the parent object
    public void init(Tower t)
    {
        this.owner = t;
        isPlaced = false;

        transform.parent = owner.transform; // make quad the same location as the parent
        transform.localPosition = new Vector3(0, 0, 0); // position relative to parent object

        mat = GetComponent<Renderer>().material; // get material component
        mat.shader = Shader.Find("Sprites/Default"); // set shader
        mat.mainTexture = Resources.Load<Texture2D>("Textures/greenTower"); // set greenTower.png as textre
        mat.color = new Color(1, 1, 1); // set color
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // when the mouse moves to a new tile
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
	    // instruct manager which tiles to highlight
            int x = (int)owner.transform.position.x;
            int y = (int)owner.transform.position.y;
            owner.manager.highlight(x, y + 1);
            owner.manager.highlight(x - 1, y);
            owner.manager.highlight(x, y - 1);
            owner.manager.highlight(x + 1, y);
        //}
	
	// if the player clicks the tower
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
