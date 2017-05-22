using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy4L3 : MonoBehaviour {

    public GameObject projectile, explosion;
    private float speed;
    private Transform shootPos;
    private int life;

    // Use this for initialization
    void Start () {
        speed = 5f;
        shootPos = transform.GetChild(0);
        life = 100;
        StartCoroutine("Shoot");
    }
	
	// Update is called once per frame
	void Update () {
        if (transform.position.y > 7.8 || transform.position.y < -7.7)
            speed *= -1;

        transform.Translate(speed * Time.deltaTime, Time.deltaTime * -15f, 0);
        Dead();
    }

    public void Dead()
    {
        if (life <= 0)
        {
            Instantiate(explosion, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }

    private IEnumerator Shoot()
    {
        while (true)
        {
            Instantiate(projectile, shootPos.position, transform.rotation);
            yield return new WaitForSeconds(1.5f);
        }

    }

    void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject.tag == "Laser")
            life -= 10;
        if (c.gameObject.tag == "Missile")
            life -= 25;
    }
}
