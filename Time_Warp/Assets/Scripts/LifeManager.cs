using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LifeManager : MonoBehaviour {

    Vector3 position;

    public PlayerController pC;

    public GameObject lifePrefab;

    public GameObject emptyLifePrefab;

    int previousLives;

    float xDelta = -0.05f;

    float xBase = 1.037f;

    int lives;

    public GUIText livesWithWords;

    List<GameObject> lifeList;

	// Use this for initialization
	void Start () 
    {

        pC = FindObjectOfType<PlayerController>() as PlayerController;
         
       /* position = new Vector3(xBase, 0.977f, 0);

        pC = FindObjectOfType<PlayerController>() as PlayerController;

        lives = pC.lives;

        lifeList = new List<GameObject>();

        for (int i = 0; i < lives; i++)
        {

            Vector3 newPosition = new Vector3(xBase + (i * xDelta), position.y, position.z);

            GameObject life = (GameObject)Instantiate(lifePrefab, newPosition, Quaternion.identity);

            lifeList.Add(life);
        }*/

	}

    // Update is called once per frame
    void Update()
    {

        lives = pC.lives;

        livesWithWords.text = "Lives: " + lives;
        /*
        if (lives != previousLives)
        {

            int diff = lives - previousLives;

            if (diff > 0)
            {

                Vector3 newPosition = new Vector3(xBase + ((lives - 1) * xDelta), position.y, position.z);

                GameObject life = (GameObject)Instantiate(lifePrefab, newPosition, Quaternion.identity);

                lifeList.Add(life);
            }

            else if (diff < 0)
            {

                if (lives == 2)
                {
                    Debug.Log("Doseqieowueroqipwe");
                    Vector3 oldPosition = lifeList[2].transform.position;
                    GameObject life = (GameObject)Instantiate(emptyLifePrefab, oldPosition, Quaternion.identity);

                    Destroy(lifeList[2]);

                }
                Debug.Log("Lost Life!");

                Debug.Log(lives);

                Vector3 oldPosition2 = lifeList[lives].transform.position;

                GameObject life2 = (GameObject)Instantiate(emptyLifePrefab, oldPosition2, Quaternion.identity);

                Destroy(lifeList[lives]);
            }

        }

        previousLives = lives;*/

    }

}
