using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2L3 : MonoBehaviour {

    public GameObject projectile, bullet, explosion;
    private float speed;
    private Transform shootPos, upPos, downPos;
    private int life;

    // Use this for initialization
    void Start() {
        speed = 5f;
        shootPos = transform.GetChild(0);
        upPos = transform.GetChild(1);
        downPos = transform.GetChild(2);
        life = 100;
       StartCoroutine("ShootProjectile");
        StartCoroutine("ShootBullets");
    }

    // Update is called once per frame
    void Update() {
        if (transform.position.y > 7.8 || transform.position.y < -7.7)
            speed *= -1;

        transform.Translate(speed * Time.deltaTime, Time.deltaTime*-15f, 0);
        Dead();
    }

    private IEnumerator ShootProjectile() {
        while (true)
        {
            Instantiate(projectile, shootPos.position, transform.rotation);
            yield return new WaitForSeconds(3f);
        }

    }

    private IEnumerator ShootBullets() {
        while (true) {
            Instantiate(bullet, upPos.position, transform.rotation);
            Instantiate(bullet, downPos.position, transform.rotation);
            yield return new WaitForSeconds(0.15f);
        }
    }

    void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject.tag == "Laser")
            life -= 5;
        if (c.gameObject.tag == "Missile")
            life -= 20;
    }

    public void Dead()
    {
        if(life <= 0)
        {
            Instantiate(explosion, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
