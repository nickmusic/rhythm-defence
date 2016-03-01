// Jun Li
// Enemy class
// 2/22/16

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : MonoBehaviour {

	private enemyModel model;		// The model object.
	private int enemyType;
	private float enemyx;
	private float enemyy;
	public GameManager m;		// A pointer to the manager (not needed here, but potentially useful in general).

	// The Start function is good for initializing objects, but doesn't allow you to pass in parameters.
	// For any initialization that requires input, you'll probably want your own init function. 

	public void init(int enemyType, GameManager m, float x, float y) {
		this.enemyType = enemyType;
		this.m = m;
		this.enemyx = x;
		this.enemyy = y;

		var modelObject = GameObject.CreatePrimitive(PrimitiveType.Quad);	// Create a quad object for holding the gem texture.
		model = modelObject.AddComponent<enemyModel>();						// Add a gemModel script to control visuals of the gem.
		model.init(enemyType, this);						
	}

	public void damage(int numBeats){
		model.damage (numBeats);
	}

	public void move(int numBeats){
		model.move (numBeats);
	}

	public float getX(){
		return model.getX ();
	}

	public float getY(){
		return model.getY ();
	}

	public int getHealth(){
		return model.getHealth();
	}

	public void destroy(){
		model.destroy ();
	}

}

