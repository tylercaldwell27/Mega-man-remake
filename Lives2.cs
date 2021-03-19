using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lives2 : MonoBehaviour {

    public player _player;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (_player.lives < 2)
        {
            Destroy(gameObject, 0);
        }
    }
}
