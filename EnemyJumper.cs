using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyJumper : MonoBehaviour {

    Rigidbody2D rb;

    public float speed = 5;


    public bool killed;

    public AudioSource EnemyKilled;

    public int health = 1;
    public float fallSpeed = 2;

    public bool onGround;
    Animator anim;
    public float jumpForce;
    public bool isGrounded;
    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask isGroundLayer;
    public Vector2 pos;
    public Vector2 Enemypos;

    public player _player;

    // Use this for initialization
    void Start () {
        tag = "Enemy";

       
        rb = GetComponent<Rigidbody2D>();

        rb.mass = 1.0f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.sleepMode = RigidbodySleepMode2D.NeverSleep;

        if (speed <= 0 || speed > 5.0f)
        {
            speed = 5.0f;
        }
        anim = GetComponent<Animator>();
        // checks for groundcheck
        if (!groundCheck)
        {
            groundCheck = GameObject.Find("EnemyGroundCheck").GetComponent<Transform>();
        }
        if (groundCheckRadius <= 0)
        {
            groundCheckRadius = 0.1f;
        }

        // checks if the player has jump force
        if (jumpForce <= 0 || jumpForce > 20.0f)
        {
            jumpForce = 10.0f;
            Debug.LogWarning("jumpForce not set on" + name + ". Defaulting to 10");
        }

    }
	
	// Update is called once per frame
	void Update () {
        Enemypos = transform.position;

        _player.getPos(_player.pos.x);
        
       if (groundCheck)
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, isGroundLayer);
        }
 

        //if the player is on the ground
    
          //  rb.velocity = new Vector2(-speed, rb.velocity.y);
        
       
        if (isGrounded)
        {
            anim.SetBool("jump", false);
            //if the jump button is pressed while on the ground
            if (_player.pos.x < Enemypos.x)
            {
                rb.velocity = new Vector2(-speed, jumpForce);
            }
            else if (_player.pos.x > Enemypos.x)
            {
                rb.velocity = new Vector2(speed, jumpForce);
            }
            //anim.SetBool("jump", false);


        }
        if (!isGrounded)
        {
            anim.SetBool("jump",true);
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallSpeed - 1) * Time.deltaTime;
            
        }
        if(killed == true)
        {
            anim.SetBool("dead", true);
           
            Destroy(gameObject);
        }

    }

    void OnCollisionEnter2D(Collision2D collision)
    {


        if (collision.gameObject.tag == "bullet")
        {
            
            killed = true;
            Destroy(collision.gameObject);
            if (_player.mute == false)
            {
                EnemyKilled.Play();
            }


        }
    }
}
