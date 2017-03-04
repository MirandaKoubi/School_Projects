using UnityEngine;
using System.Collections;

public class MovingPlatform : MonoBehaviour 
{
    public float speed;
    public float left;
    public float right;

    public float direction;

    Vector3 moveAmount;

    PlayerController pC;

    // Use this for initialization
    void Start()
    {

        pC = FindObjectOfType<PlayerController>() as PlayerController;

    }

    // Update is called once per frame
    void Update()
    {

        moveAmount.x = direction * speed * (Time.deltaTime * pC.slowDownTime);

        if (direction > 0.0f && transform.position.x >= right)
        {

            direction = -1.0f;

        }

        else if (direction < 0.0f && transform.position.x <= left)
        {

            direction = 1.0f;

        }

        transform.Translate(moveAmount);


    }
}
