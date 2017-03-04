using UnityEngine;
using System.Collections;

public class TimerManager : MonoBehaviour {


    public GUIText powerupTimer;

    public PlayerController pC;
     
	// Use this for initialization
	void Start () 
    {
	
        pC = FindObjectOfType<PlayerController>() as PlayerController;

	}
	
	// Update is called once per frame
	void Update ()
    {

        if (pC.invincibility == true)
        {
            
            powerupTimer.text = "Timer: " + (int) pC.timer;

        }

        if (pC.slowTime == true)
        {

            powerupTimer.text = "Timer: " + (int) pC.timer;

        }

        if (pC.jetPackOn == true)
        {

            powerupTimer.text = "Timer: " + (int) pC.timer2;

        }

	}
}
