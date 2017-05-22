using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Head : MonoBehaviour {

    private Rigidbody2D rb;
    private Quaternion q;
    private float angle;

    // Use this for initialization
    void Start () {
        rb = gameObject.GetComponent<Rigidbody2D>();
        rb.AddForce(new Vector2(-2,3), ForceMode2D.Impulse);
        angle = 60;
    }
	
	// Update is called once per frame
	void Update () {
          q = Quaternion.AngleAxis(angle, Vector3.forward);
          transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * 10);
        
    }

    void OnCollisionEnter2D(Collision2D c)
    {
        if(c.gameObject.tag == "Ground")
            angle = 0;
    }
}
