using UnityEngine;
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
        if (Input.GetMouseButtonDown(0))
        {
            if (!isPlaced)
            {
                bool test = owner.manager.placeTower();
                isPlaced = test;
                owner.manager.resetColors();
            }
            else
            {
                owner.manager.getTile(x, y).setHasTower(false);
                owner.manager.destroyTower(owner);
                owner.manager.addConstraint(0, 1);
                owner.manager.resetColors();
            }
        }
    }
		
	public bool placed() {
		return isPlaced;
	}
}
