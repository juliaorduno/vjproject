using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saw : MonoBehaviour {
    private float speed;

	// Use this for initialization
	void Start () {
        speed = 5f;
	}
	
	// Update is called once per frame
	void Update () {
         transform.Translate(speed * Time.deltaTime, 0, 0); 
	}

    void OnTriggerEnter2D(Collider2D c)
    {
        if(c.gameObject.name == "Right Bound")
        {
            Destroy(gameObject);
        }
    }
}
