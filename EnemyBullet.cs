﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour {

    public float lifeTime;

    // Use this for initialization
    void Start()
    {
        // Check if 'lifeTime' variable was set in the inspector
        if (lifeTime <= 0)
        {
            // Assign a default value if one was not set
            lifeTime = 2.0f;

            // Prints a warning message to the Console
            // - Open Console by going to Window-->Console (or Ctrl+Shift+C)
            Debug.LogWarning("ProjectileForce not set. Defaulting to " + lifeTime);
        }

        Destroy(gameObject, lifeTime);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {


        if (collision.gameObject.tag == "Player")
        {

            Destroy(gameObject,0);
        }


    }


    // Update is called once per frame
    void Update () {
		
	}
}
