using UnityEngine;
using System.Collections;

public class Tower : MonoBehaviour
{

    public GameManager manager; // pointer to manager

    // placeholders for quads
    private RedTower redModel;
    private GreenTower greenModel;
    private BlueTower blueModel;

    private int towerType, direction;

    // called by the manager when a tower is created
    public void init(int type, GameManager m)
    {
        this.manager = m;
        towerType = type;
        direction = 0;

        var modelObject = GameObject.CreatePrimitive(PrimitiveType.Quad); // create quad
        modelObject.SetActive(true); // amkes sure the quad is active

        if (type == 0) // if the tower is RED
        {
            redModel = modelObject.AddComponent<RedTower>(); // add RedTower script to quad
            redModel.transform.parent = this.transform; // make the Tower object its parent
            redModel.transform.localPosition = new Vector3(0, 0, 0); // position relative to global coords

            redModel.init(this);
        }
        if (type == 1) // if the tower is GREEN
        {
            greenModel = modelObject.AddComponent<GreenTower>(); // add GreenTower script to quad
            greenModel.transform.parent = this.transform;
            greenModel.transform.localPosition = new Vector3(0, 0, 0);

            greenModel.init(this);
        }
        if (type == 2) // if the tower is BLUE
        {
            blueModel = modelObject.AddComponent<BlueTower>(); // same BlueTower script to quad
            blueModel.transform.parent = this.transform;
            blueModel.transform.localPosition = new Vector3(0, 0, 0);

            blueModel.init(this);
        }
    }

    // destorys this tower (I think we dont need this)
    public void destroy()
    {
        DestroyImmediate(gameObject);
    }

    // returns the integer of the tower type
    public int getTowerType()
    {
        return towerType;
    }
    
    // rotates the object 90 degrees counterclockwise
    // maybe bind this to right mouse or key later
    public void rotate()
    {
        direction = (direction + 1) % 4;
        transform.eulerAngles = new Vector3(0, 0, direction * 90);
    }
    
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

}
