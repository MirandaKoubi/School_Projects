using UnityEngine;
using System.Collections;

public class SlowTime : MonoBehaviour 
{

    public Animator anime2;

    bool popped = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
    {

        anime2.SetBool("isPopped", popped);

	}

    void OnTriggerEnter(Collider col)
    {

        Debug.Log(col.gameObject.name);

        if (col.gameObject.name == "Player")
        {
            PlayerController pC = FindObjectOfType<PlayerController>() as PlayerController;

            Debug.Log(col.gameObject.name);

            if (pC != null)
            {

                pC.slowTime = true;

                Debug.Log(pC.slowTime + "Setting thing to thing.");

                popped = true;

                StartCoroutine("Pop");

                //Destroy(gameObject);

            }


        }

    }

    IEnumerator Pop()
    {

        yield return new WaitForSeconds(0.5f);

        Destroy(gameObject);

    }
}
