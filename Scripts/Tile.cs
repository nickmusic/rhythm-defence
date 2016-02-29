using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {

    public GameManager manager; // public so it can be access by child
    private TileModel model; // quad of tower
    private bool hasTower; // if a tower has been placed

    public void init(int x, int y, GameManager m)
    {
        manager = m;
        hasTower = false;

        var modelObject = GameObject.CreatePrimitive(PrimitiveType.Quad); // create quad
        modelObject.SetActive(true); // makes sure the quad is being displayed
        model = modelObject.AddComponent<TileModel>(); // add TileModel script to quad
        model.transform.parent = this.transform; // make it the child of the Tile object
        model.transform.localPosition = new Vector3(0, 0, 0); // position relative to global coords

        model.init(this);
    }

    // Use this for initialization
    void Start () {
    }
	
    // Update is called once per frame
    void Update () {
    }

    // sets whether a tower has been placed on this tile
    public void setHasTower(bool value)
    {
        hasTower = value;
    }

    // returns true if a tower has been placed on this tile
    public bool HasTower()
    {
        return hasTower;
    }

    // returns the quad (e.g. for color manipulation)
    public TileModel getModel()
    {
        return model;
    }

}
