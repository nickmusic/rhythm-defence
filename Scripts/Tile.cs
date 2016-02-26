using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {

    public GameManager manager;
    private TileModel model;
    private bool hasTower;

    public void init(int x, int y, GameManager m)
    {
        manager = m;
        hasTower = false;

        var modelObject = GameObject.CreatePrimitive(PrimitiveType.Quad);
        modelObject.SetActive(true);
        model = modelObject.AddComponent<TileModel>();
        model.transform.parent = this.transform;
        model.transform.localPosition = new Vector3(0, 0, 0);

        model.init(this);
    }

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void setHasTower(bool value)
    {
        hasTower = value;
    }

    public bool HasTower()
    {
        return hasTower;
    }

    public TileModel getModel()
    {
        return model;
    }

}
