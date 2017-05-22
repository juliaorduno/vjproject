using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public float speed;
    public GameObject explosion;
    public Transform point;

	// Use this for initialization
	void Start () {
        point = transform.GetChild(0);
	}
	
	// Update is called once per frame
	void Update () {
        if (gameObject.tag != "Laser")
            transform.Translate(0, speed * Time.deltaTime, 0);
        else if(gameObject.tag == "Missile")
        {
            transform.Translate(0, speed * Time.deltaTime, 0);
        }
        else
        {
            transform.Translate(speed * Time.deltaTime, 0, 0);
        }
    }

    void OnTriggerEnter2D(Collider2D c)
    {
       if((c.gameObject.name == "Ship" && gameObject.tag != "Laser") || ((gameObject.tag == "Laser" || gameObject.tag == "Missile") && c.gameObject.tag == "Enemy"))
        {
            Instantiate(explosion, point.position, transform.rotation);
            Destroy(gameObject);
        }

       if(c.gameObject.name == "TEnemy2" || c.gameObject.name == "TEnemy3")
        {
            Destroy(gameObject);
        }
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
