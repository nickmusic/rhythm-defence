﻿using UnityEngine;
using System.Collections;
using System;

public class Bullet : MonoBehaviour {

	Tower owner;
	BulletModel model;

	public void init(Tower t) {
		this.owner = t;

		var modelObject = GameObject.CreatePrimitive(PrimitiveType.Quad);	// Create a quad object for holding the gem texture.
		model = modelObject.AddComponent<BulletModel>();						// Add a towerModel script to control visuals of the gem.
        model.transform.parent = transform.parent;
		model.init (this);

	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void move(float x, float y, float beat){
		model.move (x, y, beat);
	}
}
