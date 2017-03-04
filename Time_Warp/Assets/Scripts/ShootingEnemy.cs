using UnityEngine;
using System.Collections;

public class ShootingEnemy : MonoBehaviour {

    public GameObject bulletPrefab;

    public GameObject anchorPoint;

    public Vector3 bulletVel;

    public float shootDelay = 2.0f;

    public float timeToShoot;

    PlayerController pC;


	// Use this for initialization
	void Start ()
    {

        timeToShoot = shootDelay;
        pC = FindObjectOfType<PlayerController>() as PlayerController;
	}
	
	// Update is called once per frame
	void Update () 
    {

        if (timeToShoot < Time.time)
        {
           GameObject bullet = Instantiate(bulletPrefab, anchorPoint.transform.position, Quaternion.identity) as GameObject;

           bullet.GetComponent<Bullet>().vel = bulletVel;

           timeToShoot = Time.time + shootDelay / pC.slowDownTime;

        }

	}

    void OnTriggerEnter(Collider other)
    {
        if (other.collider.gameObject.tag == "OldMan")
        {
            
            if (pC != null)
            {
                if (!pC.invincibility)
                {
                    pC.lives -= 1;
                    //Debug.Log(pC.lives);
                }
                else
                {
                    Destroy(gameObject);
                }
            }

        }

    }
}
