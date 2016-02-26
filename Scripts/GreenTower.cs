﻿using UnityEngine;
using System.Collections;

public class GreenTower : MonoBehaviour
{

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
        mat.mainTexture = Resources.Load<Texture2D>("Textures/greenTower");
        mat.color = new Color(1, 1, 1);
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

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
            owner.manager.highlight(x, y + 1);
            owner.manager.highlight(x - 1, y);
            owner.manager.highlight(x, y - 1);
            owner.manager.highlight(x + 1, y);
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