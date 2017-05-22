using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Comet : MonoBehaviour {

    private Rigidbody2D rb;
    public GameObject explosion;

	// Use this for initialization
	void Start () {
        rb = gameObject.GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        rb.AddForce(new Vector2(-4, 0), ForceMode2D.Force);
	}

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D c)
    {
        Destroy(gameObject);
        Instantiate(explosion, transform.position, transform.rotation);
        
    }
}
