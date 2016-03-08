using UnityEngine;
using System.Collections;
using System.Collections.Generic; // this line is essential

public class Tower : MonoBehaviour
{

    public GameManager manager; // pointer to manager

    // placeholders for quads
    private RedTower redModel;
    private GreenTower greenModel;
    private BlueTower blueModel;

    private int towerType, direction;

	// List of bullets
	private List<Bullet> bullets;

    // called by the manager when a tower is created
    public void init(int type, GameManager m)
    {
        this.manager = m;
        towerType = type;
        direction = 0;
		bullets = new List<Bullet> ();

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
            blueModel = modelObject.AddComponent<BlueTower>(); // add BlueTower script to quad
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
        direction = (direction + 1) % 4; // 0 = up, 1 = left, etc.
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

	public void shoot (int numBeats) {
		if (this.placed()) {
		// Pattern<Number> refers to the shooting pattern for each type of tower
			if (towerType == 0) {
				patternZero (numBeats);
			} else if (towerType == 1) {
				patternOne (numBeats);
			} else if (towerType == 2) {
				patternTwo (numBeats);
			}
		}
	}

	private void patternZero(int numBeats) { // RED: Shoots 1  in front of it every 2 beats
		if (numBeats % 2 == 0) {
			if (direction == 0) { // makes sure 1 in front corresponds with tower direction
				addBullet (this.transform.position.x, this.transform.position.y + 1);
			} else if (direction == 1) {
				addBullet (this.transform.position.x - 1, this.transform.position.y);
			} else if (direction == 2) {
				addBullet (this.transform.position.x, this.transform.position.y - 1);
			} else {
				addBullet (this.transform.position.x + 1, this.transform.position.y);
			}
		} else {
			eraseBullets ();
		}
	}

	private void patternOne(int numBeats) { // GREEN: Shoots 2 in front every 4 beats
		if (numBeats % 4 == 0) {
			if (direction == 0) { // makes sure shooting corresponds with tower direction
				addBullet (this.transform.position.x, this.transform.position.y + 1);
				addBullet (this.transform.position.x, this.transform.position.y + 2);
			} else if (direction == 1) {
				addBullet (this.transform.position.x - 1, this.transform.position.y);
				addBullet (this.transform.position.x - 2, this.transform.position.y);
			} else if (direction == 2) {
				addBullet (this.transform.position.x, this.transform.position.y - 1);
				addBullet (this.transform.position.x, this.transform.position.y - 2);
			} else {
				addBullet (this.transform.position.x + 1, this.transform.position.y);
				addBullet (this.transform.position.x + 2, this.transform.position.y);
			}		
		} else {
			eraseBullets ();
		}
	}

	private void patternTwo(int numBeats) { // BLUE: Cycles directions
		if (numBeats % 4 == 0) {
			eraseBullets ();
			addBullet (this.transform.position.x, this.transform.position.y + 1);
		} else if (numBeats % 4 == 1) {
			eraseBullets ();
			addBullet (this.transform.position.x + 1, this.transform.position.y);
		} else if (numBeats % 4 == 2) {
			eraseBullets ();
			addBullet (this.transform.position.x, this.transform.position.y - 1);
		} else if (numBeats % 4 == 3) {
			eraseBullets ();
			addBullet (this.transform.position.x - 1, this.transform.position.y);
		}
	}

	private void addBullet (float x, float y) {
		GameObject bulletObject = new GameObject();			// Create a new empty game object that will hold a bullet.
		Bullet bullet = bulletObject.AddComponent<Bullet>();			// Add the bullet.cs script to the object.
		// We can now refer to the object via this script.
		//bullet.transform.position = new Vector3(this.transform.position.x, this.transform.position.y,0);		// Position the bullet at x,y.
		//bullet.move(x, y, this.manager.getBeat());
		bullet.transform.position = new Vector3(x, y, 0);

		bullet.init(this);							// Initialize the bullet script.

		bullets.Add(bullet);										// Add the bullet to the Bullets list for future access.
		bullet.name = "Bullet "+bullets.Count;						// Give the bullet object a name in the Hierarchy pane.

	}

	public void eraseBullets() {
		for (int i = 0; i < bullets.Count; i++) {
			Destroy (bullets [i].gameObject);
		}
		bullets = new List<Bullet> ();
	}

	private bool placed() {
		if (towerType == 0) { // if the tower is RED 
			return redModel.placed();
		} else if (towerType == 1) { // if the tower is GREEN 
			return greenModel.placed();
		} else if (towerType == 2) {// if the tower is BLUE
			return blueModel.placed();
		}
		return false;

	}

	public List<Bullet> getBullets(){
		return bullets;
	}
}