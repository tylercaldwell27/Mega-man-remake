using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class player : MonoBehaviour {

    //volume slider
    public Slider Volume;

    Rigidbody2D rb;
    public Rigidbody2D rb2;

    public RectTransform healthBar;
    int scaleFactor;

 

    //the sound effects
    public AudioSource gunfire;
    public AudioSource PlayerDeath;
    public AudioSource PlayerHit;
    public AudioSource jump;
    public AudioSource healthSound;
    public AudioSource speedBoostSound;
    public AudioSource Song;

    public float speed;//players movement speed
    public float jumpForce;//how high the player can jump
    public float fallSpeed;//how fast the player falls
    public float backupVolume;
    public int health = 140;// the player's health
    public int lives = 3;
    public Animator anim; // for calling toe animations

    public bool powerUp;//for the power up
    public bool onLadder;
    public bool stopMovingUp;
    public bool mute = false;
    //for the projectile
    public Rigidbody2D projectile;
    public Rigidbody2D Player;
    public float projectileForce;
    public Transform projectileSpawnPoint;

    
    public bool isFacingRight;// To see what way the player is facing
    //for seeing if hte player is on the ground
    public bool isGrounded;
    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask isGroundLayer;
    public Vector2 pos;
    public float speedBoostTime = 5.0f;
    // Use this for initialization

    EnemyJumper jumpEnemy;
    EnemyShield Shield;
   

    void Start () {
        Song.Play();

        //creates a rigidbody in the player
        rb = GetComponent<Rigidbody2D>();
        rb.mass = 1.0f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.sleepMode = RigidbodySleepMode2D.NeverSleep;

        //if the player doesn't have a rb than it adds one
        if (!rb2)
        {
            Debug.LogWarning("Rigidbody not found" + name + ". Adding");
            rb2 = gameObject.AddComponent<Rigidbody2D>();
        }

        // checks if the player has any speed
        if (speed <= 0 || speed > 5.0f)
        {
            speed = 5.0f;
            Debug.LogWarning("speed not set on" + name + ". Defaulting to 5");
        }
        // checks if the player has jump force
        if (jumpForce <= 0 || jumpForce > 20.0f)
        {
            jumpForce = 10.0f;
            Debug.LogWarning("jumpForce not set on" + name + ". Defaulting to 10");
        }
        
        // checks for groundcheck
        if (!groundCheck)
        {
            groundCheck = GameObject.Find("GroundCheck").GetComponent<Transform>();
        }
        if (groundCheckRadius <= 0)
        {
            groundCheckRadius = 0.1f;
        }
        
        // if the is no projectile
        if (!projectile)
        {
            Debug.LogError("projectile not found on" + name);
        }

        if (!projectileSpawnPoint)
        {
            Debug.LogError("projectilleSpawnPoint not found on" + name);
        }
        if (projectileForce <= 0)
        {
            projectileForce = 20.0f;
        }

        //health bar
        if (health <= 0)
        {
            health = 5;
            Debug.LogWarning("Health not set on " + name
                + ". Defaulting to " + health);
        }

        if (healthBar)
        {
            scaleFactor = (int)healthBar.sizeDelta.x / health;
        }

    }
	
	// Update is called once per frame
	void Update () {
        pos = transform.position;
        float moveValue = Input.GetAxisRaw("Horizontal");
        float moveValueY = Input.GetAxisRaw("Vertical");
        anim.SetFloat("speed", Mathf.Abs(moveValue));
       
        //checks if the player is on the ground
        if (groundCheck)
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, isGroundLayer);
        }

        //if the player is on the ground
        if (isGrounded)
        {
            //if the jump button is pressed while on the ground
            if (Input.GetButtonDown("Jump")) rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        
            anim.SetBool("jump",false);
            onLadder = false;
           
       
        }
        
        //if the fire button is pressed
        if (Input.GetButtonDown("Fire1"))
        {
            fire();// calls the fire merthod
            anim.SetBool("shoot",true);
           
                gunfire.Play();
            
            
        }

        // the the fire button isn't pressed
        if (!Input.GetButtonDown("Fire1"))
        {
            anim.SetBool("shoot", false);
        }

        // the the player is in the air
        if (!isGrounded)
        {
           
           
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallSpeed - 1) * Time.deltaTime;
            anim.SetBool("jump",true);
        }
        rb.velocity = new Vector2(moveValue * speed, rb.velocity.y);
        if (health <= 0)
        {
            lives -= 1;
            pos.x = -126.9f;
            pos.y = -20;
            transform.position = pos;
            respawn(pos.x);
           
                PlayerDeath.Play();
            
          

        }
        if(lives == 0)
        {
            anim.SetBool("death", true);
            Destroy(gameObject);
            SceneManager.LoadScene("GameOver");
        }
       // for switching the way the player is looking
        if ((moveValue < 0 && isFacingRight) ||
           (moveValue > 0 && !isFacingRight))
            flip();//calls the flip method

        // when the power up is in use
        if(powerUp == true)
        {
            speed = 15;
        }
        if (powerUp == false)
        {
            speed = 5;
        }
        if(onLadder == true  )
        {
            rb.velocity = new Vector2(moveValue * speed, moveValueY* speed);
        }
        else if(onLadder == false && isGrounded == true )
        {
            rb.velocity = new Vector2(moveValue * speed, rb.velocity.y );
        }
        else if (onLadder == false && isGrounded == false)
        {
            onLadder = false;
          
            rb.velocity = new Vector2(moveValue * speed, rb.velocity.y);

        }
        if(stopMovingUp == true)
        {
            moveValueY = 0;
            rb.velocity = new Vector2(moveValue * speed, 0);
        }

        if (healthBar)
        {
            healthBar.sizeDelta = new Vector2(health * scaleFactor,
                healthBar.sizeDelta.y);
        }
        if (Input.GetButtonDown("Jump"))
        {
            
                jump.Play();
            
        }

   
        volume();

    }
    //checking collision with the enemy
    void OnCollisionEnter2D(Collision2D collision)
    {

        
        if (collision.gameObject.tag == "Enemy")
        {
                health -= 10;
                anim.SetBool("hit", true);
         
                PlayerHit.Play();
            
            if (healthBar)
            {
                healthBar.sizeDelta = new Vector2(health * scaleFactor,
                    healthBar.sizeDelta.y);
            }
            StartCoroutine(hiteffect());  
        }
        if(collision.gameObject.tag == "spikes")
        {
            health = 0;
         
          //  Destroy(gameObject, 0.5f);
        }

    
        
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if 'Character' collides with something tagged as 'Collectible'
        if (collision.gameObject.tag == "SpeedBoost")
        {
            powerUp = true;
            anim.SetBool("powerup", true);
            StartCoroutine(StopPowerUp());
            Destroy(collision.gameObject);
        
                speedBoostSound.Play();
            
        }

        if(collision.gameObject.tag == "Health")
        {
            health += 25;
            Destroy(collision.gameObject);
           
                healthSound.Play();
            
        }
        if (collision.gameObject.tag == "winToken")
        {
            SceneManager.LoadScene("YouWin");

        }


        if (collision.gameObject.tag == "Ladder")
        {
            onLadder = true;
            stopMovingUp = false;
        }
        else if(collision.gameObject.tag != "Ladder")
        {
            onLadder = false;
        }
        if(collision.gameObject.tag == "topOfLadder")
        {
            stopMovingUp = true;
        }

    }
    //the fire gun method
         public void fire() {
        Debug.Log("Pew Pew ");
        if (projectile && projectileSpawnPoint)
        {
            //for having the bullet shoot the way the play is facing
            Rigidbody2D temp = Instantiate(projectile, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), temp.GetComponent<Collider2D>(), true);
            if (isFacingRight)
                temp.AddForce(projectileSpawnPoint.right * projectileForce, ForceMode2D.Impulse);
            
            else
                temp.AddForce(-projectileSpawnPoint.right * projectileForce, ForceMode2D.Impulse);
          
        }
    }

    public void getPos(float _x)
    {
        _x = pos.x;
    }
    void flip()
    {
        // Toggle variable
        isFacingRight = !isFacingRight;

        // Keep a copy of 'localScale' because scale cannot be changed directly
        Vector3 scaleFactor = transform.localScale;

        // Change sign of scale in 'x'
        scaleFactor.x *= -1; // or - -scaleFactor.x

        // Assign updated value back to 'localScale'
        transform.localScale = scaleFactor;
    }

    
    //respawning the player
   public void respawn(float _x)
    {
    
        _x = -126.9f;
        pos.y = 60f;
        pos.x = _x;
        health = 100;
    }

    public void muteGame()
    {
        mute = true;
        backupVolume = Volume.value;
        Volume.value = Volume.minValue;
        Song.volume = Volume.value;
        gunfire.volume = Volume.value;
        PlayerDeath.volume = Volume.value;
        PlayerHit.volume = Volume.value;
        jump.volume = Volume.value;
        healthSound.volume = Volume.value;
        speedBoostSound.volume = Volume.value;
     

    }
    public void unMuteGame()
    {
        mute = false;
        Volume.value = backupVolume;
        Song.volume = Volume.value;
        gunfire.volume = Volume.value;
        PlayerDeath.volume = Volume.value;
        PlayerHit.volume = Volume.value;
        jump.volume = Volume.value;
        healthSound.volume = Volume.value;
        speedBoostSound.volume = Volume.value;
     
    }
    public void volume()
    {
        // updating the volume
       
        Song.volume = Volume.value;
        gunfire.volume = Volume.value;
        PlayerDeath.volume = Volume.value;
        PlayerHit.volume = Volume.value;
        jump.volume = Volume.value;
        healthSound.volume = Volume.value;
        speedBoostSound.volume = Volume.value;
     
    }
  
    IEnumerator hiteffect()
    {
      
        yield return new WaitForEndOfFrame();//wait for the frame to end
        anim.SetBool("hit", false);
       
    }

    IEnumerator StopPowerUp()
    {

        yield return new WaitForSeconds(speedBoostTime);
        anim.SetBool("powerup", false);
        powerUp = false;

    }


}
