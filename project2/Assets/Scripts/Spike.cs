using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour {

    public bool fall;

	// Use this for initialization
	void Start () {
        fall = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (fall)
        {
            transform.Translate(0, 5 * Time.deltaTime, 0);
        }
	}

    void OnTriggerEnter2D(Collider2D c)
    {
        Destroy(gameObject);
    }
}
