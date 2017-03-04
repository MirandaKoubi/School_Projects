using UnityEngine;
using System.Collections;

public class MimickingEnemy : MonoBehaviour
{

    GameObject player;
    float direction;
    public float speed;

    public Animator anime4;

    Vector3 moveAmount;

    PlayerController pC;

    // Use this for initialization
    void Start()
    {

        player = GameObject.Find("Player");

        pC = FindObjectOfType<PlayerController>() as PlayerController;

    }

    // Update is called once per frame
    void Update()
    {

        anime4.SetFloat("Direction", direction);

        //Debug.Log(player.transform.position);

        moveAmount.x = direction * speed * (Time.deltaTime * pC.slowDownTime);

        if (transform.position.x >= player.transform.position.x + 3 && transform.position.x > -9.0f)
        {

            direction = -1.0f;

        }

        else if (transform.position.x <= player.transform.position.x - 3 && transform.position.x < 9.0f)
        {


            direction = 1.0f;

        }


        transform.Translate(moveAmount);

    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name == "Player")
        {

            Physics.IgnoreCollision(col.gameObject.collider, collider);

            PlayerController pC = FindObjectOfType<PlayerController>() as PlayerController;

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
