using UnityEngine;
using System.Collections;

public class WalkingEnemy : MonoBehaviour
{

    public Animator anime3;

    public float speed;
    public float left;
    public float right;

    public float direction;

    Vector3 walkAmount;

    PlayerController pC;

    // Use this for initialization
    void Start()
    {

        pC = FindObjectOfType<PlayerController>() as PlayerController;

    }

    // Update is called once per frame
    void Update()
    {

        anime3.SetFloat("Direction", direction);

        walkAmount.x = direction * speed * (Time.deltaTime * pC.slowDownTime);

        if (direction > 0.0f && transform.position.x >= right)
        {

            direction = -1.0f;

        }

        else if (direction < 0.0f && transform.position.x <= left)
        {

            direction = 1.0f;

        }

        transform.Translate(walkAmount);


    }

    void OnTriggerEnter(Collider other)
    {
        if (other.collider.gameObject.tag == "OldMan")
        {
            PlayerController pC = FindObjectOfType<PlayerController>() as PlayerController;

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
