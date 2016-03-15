using UnityEngine;
using System.Collections;

public class IndicatorTile : MonoBehaviour {

    public GameManager manager; // public so it can be access by child
    private IndicatorTileModel model; // quad of tower

    public void init(int type, int health, GameManager m)
    {
        manager = m;

        var modelObject = GameObject.CreatePrimitive(PrimitiveType.Quad); // create quad
        modelObject.SetActive(true); // makes sure the quad is being displayed
        model = modelObject.AddComponent<IndicatorTileModel>(); // add TileModel script to quad
        model.transform.parent = this.transform; // make it the child of the Tile object
        model.transform.localPosition = new Vector3(0, 0, 0); // position relative to global coords
        model.transform.localScale = new Vector3(.5f, 1, 0);

        model.init(type, health, this);
    }
}
