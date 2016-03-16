using UnityEngine;
using System.Collections;

public class IndicatorTileModel : MonoBehaviour {

    private IndicatorTile owner;
    private Material mat;

    public void init(int type, int health, IndicatorTile t)
    {
        owner = t;

        transform.parent = owner.transform;
        transform.localPosition = new Vector3(0, 0, 0);

        mat = GetComponent<Renderer>().material;
        mat.shader = Shader.Find("Sprites/Default");

        if (type == 0)
        {
            if (health == 1)
            {
                mat.mainTexture = Resources.Load<Texture2D>("Textures/indicatorGhoulRed");
            }
            else if (health == 2)
            {
                mat.mainTexture = Resources.Load<Texture2D>("Textures/indicatorGhoulYellow");
            }
            else if (health > 2)
            {
                mat.mainTexture = Resources.Load<Texture2D>("Textures/indicatorGhoulGreen");
            }
        }
        else if (type == 1)
        {
            if (health == 1)
            {
                mat.mainTexture = Resources.Load<Texture2D>("Textures/indicatorSpiderRed");
            }
            else if (health == 2)
            {
                mat.mainTexture = Resources.Load<Texture2D>("Textures/indicatorSpiderYellow");
            }
            else if (health > 2)
            {
                mat.mainTexture = Resources.Load<Texture2D>("Textures/indicatorSpiderGreen");
            }
        }
        else if (type > 1)
        {
            if (health == 1)
            {
                mat.mainTexture = Resources.Load<Texture2D>("Textures/indicatorSlimeRed");
            }
            else if (health == 2)
            {
                mat.mainTexture = Resources.Load<Texture2D>("Textures/indicatorSlimeYellow");
            }
            else if (health > 2)
            {
                mat.mainTexture = Resources.Load<Texture2D>("Textures/indicatorSlimeGreen");
            }
        }
        mat.color = new Color(1, 1, 1);
    }
}
