using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{

     GameObject character;

     Vector3 newPosition;

	// Use this for initialization
	void Start () 
    {

        character = GameObject.Find("Player");

        newPosition = Camera.main.transform.position;

	}
	
	// Update is called once per frame
	void Update () 
    {

        newPosition.y = character.transform.position.y + 3;

        Camera.main.transform.position = newPosition;

	}
}
