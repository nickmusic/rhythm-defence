using UnityEngine;
using System.Collections;

public class BlueTower : MonoBehaviour
{

    private Tower owner; // parent object
    private Material mat; // material (for texture)
    private bool isPlaced; // whther the tower has been placed

    // called by the parent object when quad is created
    public void init(Tower t)
    {
        this.owner = t;
        isPlaced = false;

        transform.parent = owner.transform; // set position to same as parent
        transform.localPosition = new Vector3(0, 0, 0); // position relative to global coords

        mat = GetComponent<Renderer>().material; // get material component
        mat.shader = Shader.Find("Sprites/Default"); // set shader
        mat.mainTexture = Resources.Load<Texture2D>("Textures/blueTower"); // use blueTower texture
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

    // when the mouse moves to a new tile (thus exiting the quad)
    void OnMouseExit()
    {
        if (owner.manager.isPlacing() && (!isPlaced))
        {
            Vector3 vector = owner.transform.position; // move the tower to the new tile
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
	    // tell the manager which tiles to highlight
            int x = (int)owner.transform.position.x;
            int y = (int)owner.transform.position.y;
            if ((int)owner.transform.eulerAngles.z == 0)
            {
                owner.manager.highlight(x, y + 1);
            }
            else if ((int)owner.transform.eulerAngles.z == 90)
            {
                owner.manager.highlight(x - 1, y);
            }
            else if ((int)owner.transform.eulerAngles.z == 180)
            {
                owner.manager.highlight(x, y - 1);
            }
            else if ((int)owner.transform.eulerAngles.z == 270)
            {
                owner.manager.highlight(x + 1, y);
            }
        //}
	
	// if the tower is clicked
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
