using UnityEngine;
using System.Collections;

public class ExtraLife : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.name == "Player")
        {
            PlayerController pC = FindObjectOfType<PlayerController>() as PlayerController;

            if(pC != null)
            {
                pC.lives += 1;

                Debug.Log(pC.lives);
                Destroy(gameObject);

            }
           
         
         }

    }
}
