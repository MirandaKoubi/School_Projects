using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{

    public Animator anime;

    public float movementSpeed = 5.0f;

    bool moveRight = false;
    bool moveLeft = false;

    public float onPlatformSpeed;
    public float onPlatformDirection;

    public int lives = 5;

    public bool isDead = false;

    public float timer = 10;

    public float timer2 = 5;

    public float slowDownTime = 1.0f;

    bool grounded = true;

    public bool jetPackOn = false;

    public bool invincibility = false;

    public bool slowTime = false;

    GameObject jetPack;

    Vector3 onPlatformMove;

    public GameObject backAnchor;



	// Use this for initialization
	void Start () 
    {

	}
	
	// Update is called once per frame
	void Update () 
    {
        RaycastHit hitInfo;

        float h = Input.GetAxis("Horizontal");

        anime.SetFloat("Speed", Mathf.Abs(h));

        anime.SetFloat("Direction", h);

        anime.SetBool("isGrounded", grounded);

        anime.SetBool("isDead", isDead);

        anime.SetBool("moveRight", moveRight);

        anime.SetBool("moveLeft", moveLeft);



        if (lives <= 0)
        {

            isDead = true;

            Collider[] cols = GetComponents<Collider>();

            foreach (Collider c in cols)
            {

                c.isTrigger = true;

            }

            GetComponent<PlayerController>().enabled = false;

            anime.SetTrigger("isDead");

            StartCoroutine("ReloadGame");

        }

        if (slowTime == true)
        {

            slowDownTime = 0.3f;

            timer -= Time.deltaTime;

            Debug.Log(timer);


            if (timer <= 0)
            {

                slowDownTime = 1.0f;
                slowTime = false;

                timer = 10.0f;

                Debug.Log(slowTime);
            }

        }

        if (invincibility == true)
        {

            timer -= Time.deltaTime;

            Debug.Log(timer);


            if (timer <= 0)
            {

                invincibility = false;

                timer = 10.0f;

            }

        }


        if (jetPackOn == true)
        {
            
            timer2 -= Time.deltaTime;

            Debug.Log(timer2);


            if (timer2 <= 0)
            {
                
                Debug.Log("Destroy!");
                
                jetPackOn = false;
                
                Destroy(jetPack);
                
                timer2 = 5.0f;
                
                Debug.Log("Should be gone.");

            }



            if (Input.GetKeyDown(KeyCode.RightArrow) && transform.position.x < 9.0f)
            {

                rigidbody.AddForce(Vector3.right * 200.0f);

            }


            if (Input.GetKeyDown(KeyCode.LeftArrow) && transform.position.x > -9.0f)
            {
            
                rigidbody.AddForce(-Vector3.right * 200.0f);

            }

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {

                transform.position += movementSpeed * Time.deltaTime * Vector3.up;
                rigidbody.AddForce(Vector3.up * 200.0f);

            }

            if (Input.GetKey(KeyCode.UpArrow))
            {

                rigidbody.AddForce(Vector3.up * 25.3f);

            }

            if (Input.GetKey(KeyCode.DownArrow))
            {

                rigidbody.AddForce(-Vector3.up * 6.0f);

            }


        }

        if(Input.GetKeyDown(KeyCode.F12))
        {

            jetPackOn = true;
            invincibility = true;

            timer2 = Mathf.Infinity;

            timer = Mathf.Infinity;

        }

        if(Input.GetKey(KeyCode.RightArrow) && transform.position.x < 9.0f)
        {

           if (!rigidbody.SweepTest(Vector3.right, out hitInfo, movementSpeed * Time.deltaTime))
            {

                rigidbody.velocity += (movementSpeed - rigidbody.velocity.x) * Vector3.right;

            }
           
            else
            {

                rigidbody.velocity -= rigidbody.velocity.x * Vector3.right;
                
            }

           moveRight = true;
        }

        else if(!Input.GetKey(KeyCode.RightArrow))
        {

            moveRight = false;

        }

        
        if(Input.GetKey(KeyCode.LeftArrow) && transform.position.x > -9.0f)
        {

            if (!rigidbody.SweepTest(-Vector3.right, out hitInfo, movementSpeed * Time.deltaTime))
            {
                rigidbody.velocity += (-movementSpeed - rigidbody.velocity.x) * Vector3.right;

            }

            else
            {
                rigidbody.velocity -= rigidbody.velocity.x * Vector3.right;
            }

            moveLeft = true;

        }

        else if(!Input.GetKey(KeyCode.LeftArrow))
        {

            moveLeft = false;

        }

        if (!Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow))
        {
            
            rigidbody.velocity -= rigidbody.velocity.x * Vector3.right;

        }


        if(Input.GetKeyDown(KeyCode.Space) && grounded == true)
        {
            rigidbody.AddForce(Vector3.up * 700.0f);
        }

        onPlatformMove.x = onPlatformDirection * onPlatformSpeed * (Time.deltaTime * slowDownTime);

        transform.Translate(onPlatformMove);
	
	}

    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag == "Ground")
        {
            grounded = true;
         
            if (col.gameObject.name == "MovingPlatform")
            {

                onPlatformSpeed = col.gameObject.GetComponent<MovingPlatform>().speed;

            }
        }


    }

    void OnCollisionStay(Collision col)
    {
        if (col.gameObject.name == "MovingPlatform")
        {

            onPlatformDirection = col.gameObject.GetComponent<MovingPlatform>().direction;

        }

    }

    void OnCollisionExit(Collision col)
    {
        if(col.gameObject.tag == "Ground")
        {
            grounded = false;

            if (col.gameObject.name == "MovingPlatform")
            {

                onPlatformSpeed = 0.0f;

            }
        }
    }

    void OnTriggerEnter(Collider col)
    {

        if (col.gameObject.name == "JetPack")
        {

            jetPack = col.gameObject;
            jetPackOn = true;

            col.gameObject.transform.parent = backAnchor.transform;
            col.gameObject.transform.localPosition = Vector3.zero;

            timer2 -= Time.deltaTime;

            Debug.Log(timer);

            if (timer <= 0)
            {
                Debug.Log("Destroy!");

                jetPackOn = false;

                Destroy(col.gameObject);

                timer = 5.0f;

                Debug.Log("Should be gone.");

            }


        }

    }

    IEnumerator ReloadGame()
    {

        yield return new WaitForSeconds(2);

        Destroy(gameObject);

        Application.LoadLevel("MainMenu");

    }

}
