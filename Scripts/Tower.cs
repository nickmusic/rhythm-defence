using UnityEngine;
using System.Collections;

public class Tower : MonoBehaviour
{

    public GameManager manager;
    private RedTower redModel;
    private GreenTower greenModel;
    private BlueTower blueModel;
    private int towerType, direction;

    public void init(int type, GameManager m)
    {
        this.manager = m;
        towerType = type;
        direction = 0;

        var modelObject = GameObject.CreatePrimitive(PrimitiveType.Quad);
        modelObject.SetActive(true);

        if (type == 0)
        {
            redModel = modelObject.AddComponent<RedTower>();
            redModel.transform.parent = this.transform;
            redModel.transform.localPosition = new Vector3(0, 0, 0);

            redModel.init(this);
        }
        if (type == 1)
        {
            greenModel = modelObject.AddComponent<GreenTower>();
            greenModel.transform.parent = this.transform;
            greenModel.transform.localPosition = new Vector3(0, 0, 0);

            greenModel.init(this);
        }
        if (type == 2)
        {
            blueModel = modelObject.AddComponent<BlueTower>();
            blueModel.transform.parent = this.transform;
            blueModel.transform.localPosition = new Vector3(0, 0, 0);

            blueModel.init(this);
        }
    }

    public void destroy()
    {
        DestroyImmediate(gameObject);
    }

    public int getTowerType()
    {
        return towerType;
    }
    
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
