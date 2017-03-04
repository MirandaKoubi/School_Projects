using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

    public Vector3 vel;

    PlayerController pC;

	// Use this for initialization
	void Start () 
    {

      pC = FindObjectOfType<PlayerController>() as PlayerController;
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (transform.position.x < -10.5f || transform.position.x > 10.5f)
        {
            Destroy(gameObject);
        }

     transform.position += vel * (Time.deltaTime * pC.slowDownTime);
	
	}

   void OnCollisionEnter(Collision other) 
   {

       //Debug.Log(other.collider.gameObject.tag);

        if(other.collider.gameObject.tag == "OldMan")
        {

            if (pC != null)
            {
                if (!pC.invincibility)
                {
                    pC.lives -= 1;
                    Destroy(gameObject);
                }

                else
                {

                    Destroy(gameObject);

                }

                //Debug.Log(pC.lives);
            }

        }

   }
}
