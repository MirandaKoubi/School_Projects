using UnityEngine;
using System.Collections;

public class Invincibility : MonoBehaviour 
{

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
    {

	}

    void OnTriggerEnter(Collider col)
    {

        if (col.gameObject.name == "Player")
        {
            PlayerController pC = FindObjectOfType<PlayerController>() as PlayerController;

            if(pC != null)
            {
                pC.invincibility = true;

                Debug.Log(pC.invincibility);
                Destroy(gameObject);

            }


        }

    }
}
