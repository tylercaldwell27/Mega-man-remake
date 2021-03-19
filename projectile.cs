using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectile : MonoBehaviour {

    public float lifeTime;
    public ParticleSystem deathExplosion;
    public ParticleSystem hitExplosion;
    // Use this for initialization
    void Start()
    {
     
        // Check if 'lifeTime' variable was set in the inspector
        if (lifeTime <= 0)
        {
            // Assign a default value if one was not set
            lifeTime = 1.0f;

            // Prints a warning message to the Console
            // - Open Console by going to Window-->Console (or Ctrl+Shift+C)
            Debug.LogWarning("ProjectileForce not set. Defaulting to " + lifeTime);
        }

        Destroy(gameObject, lifeTime);
       
    }

    // Update is called once per frame
    void Update()
    {
        Instantiate(deathExplosion, transform.position, transform.rotation);
        Destroy(deathExplosion, 1);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);

        if (hitExplosion)
            Instantiate(hitExplosion, transform.position, transform.rotation);
             Destroy(hitExplosion, 0.5f);

    }

}
