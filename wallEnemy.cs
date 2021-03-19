using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wallEnemy : MonoBehaviour {


    Rigidbody2D rb;

    public AudioSource EnemyKilled;


    public bool killed;

    public int shots;
    
    public int health = 1;



    Animator anim;
  
    public Vector2 pos;
    public Vector2 Enemypos;

    public player _player;

    public Rigidbody2D projectile;
    public float projectileForce = 50;
    public Transform projectileSpawnPoint;
    
    public int nextShot = 40;
    // Use this for initialization
    void Start()
    {
        tag = "Enemy";

        shots = 100;
      
        rb = GetComponent<Rigidbody2D>();

        rb.mass = 1.0f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.sleepMode = RigidbodySleepMode2D.NeverSleep;

        
        anim = GetComponent<Animator>();
       

    }

    // Update is called once per frame
    void Update()
    {
        if (shots > 0)
        {
            Rigidbody2D temp = Instantiate(projectile, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), temp.GetComponent<Collider2D>(), true);
            temp.AddForce(-projectileSpawnPoint.right * projectileForce, ForceMode2D.Impulse);
            shots -= 1;
        }
        if (shots == 0)
        {
            Rigidbody2D temp = Instantiate(projectile, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), temp.GetComponent<Collider2D>(), true);
            temp.AddForce(-projectileSpawnPoint.right * projectileForce, ForceMode2D.Impulse);
            shots += 100;
        }
    }
    


       

       
        
          

           

    void OnCollisionEnter2D(Collision2D collision)
    {


        if (collision.gameObject.tag == "bullet")
        {
           
                health -= 1;
                Destroy(collision.gameObject);
       
        }
    }

}
