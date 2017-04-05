using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : MonoBehaviour {

    private float speed, left, right;
    private bool jumping;

    // Use this for initialization
    void Start () {
        speed = 3.5f;
        right = 44.21721f;
        left = 22.30468f;
        jumping = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (transform.position.x < left)
        {
            transform.Rotate(0, 180, 0);

        }
        else if (transform.position.x > right)
        {
            transform.Rotate(0, 180, 0);

        }

        transform.Translate(-speed * Time.deltaTime, 0, 0);
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D c)
    {
        jumping = Player.Grounded;

        if(c.gameObject.name == "Ninja" && jumping)
        {
            Destroy(gameObject);
        }
    }
}
