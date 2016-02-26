using UnityEngine;
using System.Collections;
using System.Collections.Generic; // this line is essential

public class Tower : MonoBehaviour
{

    public GameManager manager;
    private RedTower redModel;
    private GreenTower greenModel;
    private BlueTower blueModel;
    private int towerType, direction;
	private List<Bullet> bullets;

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


	public void shoot (int numBeats) {
		if (towerType == 0) {
			patternZero (numBeats);
		} else if (towerType == 1) {
			patternOne (numBeats);
		} else if (towerType == 2) {
			patternTwo (numBeats);
		}
	}

	private void patternZero(int numBeats) {
		if (numBeats % 2 == 0) {
			addBullet (this.transform.position.x, this.transform.position.y + 1);
		} else {
			eraseBullets ();
		}
	}

	private void patternOne(int numBeats) {
		if (numBeats % 4 == 0) {
			addBullet (this.transform.position.x, this.transform.position.y + 1);
			addBullet (this.transform.position.x, this.transform.position.y + 2);
		} else {
			eraseBullets ();
		}
	}

	private void patternTwo(int numBeats) {
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
		bullet.transform.position = new Vector3(x,y,0);		// Position the bullet at x,y.								

		bullet.init(this);							// Initialize the bullet script.

		bullets.Add(bullet);										// Add the bullet to the Bullets list for future access.
		bullet.name = "Bullet "+bullets.Count;						// Give the bullet object a name in the Hierarchy pane.

	}

	private void eraseBullets() {
		for (int i = 0; i < bullets.Count; i++) {
			Destroy (bullets [i].gameObject);
		}
		bullets = new List<Bullet> ();
	}


}
