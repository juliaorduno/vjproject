using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Player3 : MonoBehaviour {

    private float speed, shakeSpeed, shakeAmount;
    public int lightning, life;
    public Rigidbody2D rb;
    public GameObject laser1, enemy1, comet, explosion, missile, enemy2, ninja;
    public Transform point, point2;
    public Camera cam;
    private BarraVida vida;
    public Sprite sprite1, sprite2, sprite3;
    SpriteRenderer sr;
    private bool power1, power2,positionate1, positionate2, inst2, ninjaOff;
    public AudioClip[] soundfx;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody2D>();
        speed = 15f;
        lightning = 0;
        life = 100;
        point = transform.GetChild(1);
        point2 = transform.GetChild(2);
        vida = cam.GetComponent<BarraVida>();
        sr = transform.gameObject.GetComponent<SpriteRenderer>();
        power1 = false;
        power2 = false;
        positionate1 = false;
        positionate2 = false;
        inst2 = true;
        ninjaOff = false;
    }

    // Update is called once per frame
    void Update() {
        Movement();
        Shoot();
        Reposition();
        Dead();
        vida.setHealth(life);

        if (Input.GetKey(KeyCode.C) && power2)
        {
            sr.sprite = sprite2;
            if (Input.GetKeyUp(KeyCode.RightArrow))
            { 
                GetComponent<AudioSource>().clip = soundfx[4];
                GetComponent<AudioSource>().Play();
                Instantiate(missile, point2.position, missile.transform.rotation);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            sr.sprite = sprite3;
            Instantiate(ninja, new Vector3(transform.position.x - 5, transform.position.y + 2, 0), new Quaternion(0, 0, 0, 0));
            ninjaOff = true;
        }
        else if(Input.GetKeyUp(KeyCode.C))
            sr.sprite = sprite1;

        
    }

    private void Movement() {
        float v = Input.GetAxisRaw("Vertical");
        transform.Translate(Time.deltaTime * speed, v * Time.deltaTime * speed, 0);
        //transform.position = new Vector3(Mathf.Sin(Time.time * shakeSpeed), v, 0);
        //transform.Translate(0, v * Time.deltaTime*speed, 0);
    }

    private void Shoot()
    {
        if(Input.GetKeyDown(KeyCode.Space) && power1)
            StartCoroutine("InstantiateLaser");

        else if(Input.GetKeyUp(KeyCode.Space)  && power1)
            StopCoroutine("InstantiateLaser");
            
    }

    IEnumerator InstantiateLaser() {
        while(true) {
            GetComponent<AudioSource>().clip = soundfx[3];
            GetComponent<AudioSource>().Play();
            Instantiate(laser1, point.position, transform.rotation);
            yield return new WaitForSeconds(0.15f);
        }
    }

    IEnumerator InstantiateEnemy1()
    {
        while (true)
        {

            Instantiate(enemy1, new Vector3(transform.position.x + 35.7f, Random.Range(-14.82786f, 13.77214f), 0), enemy1.transform.rotation);
            yield return new WaitForSeconds(0.5f);
        }
    }


    void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject.name == "Botiquin")
        {
            GetComponent<AudioSource>().clip = soundfx[2];
            GetComponent<AudioSource>().Play();
            life += 20;
            if (life > 100)
            {
                life = 100;
            }
            Destroy(c.gameObject);
        }
        if (c.gameObject.tag == "rayo")
        {
            GetComponent<AudioSource>().clip = soundfx[1];
            GetComponent<AudioSource>().Play();
            lightning++;
            Destroy(c.gameObject);
        }
        if (c.gameObject.name == "TEnemy1")
            StartCoroutine("InstantiateEnemy1");
        else if (c.gameObject.name == "TEnemy1s")
        {
            StopCoroutine("InstantiateEnemy1");
            if (!power1)
                positionate1 = true;
        }
        if (c.gameObject.name == "TComet")
        {
            Instantiate(comet, new Vector2(transform.position.x + 38f, 11.44398f), new Quaternion(0, 0, 0, 0));
        }
        if (c.gameObject.tag == "Item")
        {
            GetComponent<AudioSource>().clip = soundfx[2];
            GetComponent<AudioSource>().Play();
            Destroy(c.gameObject);
            if (!power1)
                power1 = true;
            else if (!power2)
                power2 = true;
        }

        if (c.gameObject.name == "TEnemy2" && inst2)
            Instantiate(enemy2, new Vector3(transform.position.x + 30f, transform.position.y, 0), enemy2.transform.rotation);
        else if (c.gameObject.name == "TEnemy2s")
        {
            if (GameObject.FindGameObjectWithTag("Enemy") != null)
                positionate2 = true;
            else
            {
                inst2 = true;
                positionate2 = false;
            }
        }

    }

    IEnumerator DestroyForceField()
    {
        while (true)
        {
            if (transform.GetChild(3) != null)
            {
                Destroy(GameObject.Find("forcefield"));
                yield return new WaitForSeconds(15f);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D c)
    {
        if (c.gameObject.tag == "Comet")
        {
            life -= 50;
        }
        if (c.gameObject.tag == "Enemy" || c.gameObject.tag == "Bullet")
            life -= 5;
        if (c.gameObject.tag == "Bomb")
            life -= 15;

        if (c.gameObject.tag == "Saw")
        {
            GetComponent<AudioSource>().clip = soundfx[5];
            GetComponent<AudioSource>().Play();
            if (ninjaOff)
            {
                Instantiate(explosion, transform.position, transform.rotation);
                Destroy(gameObject);
            }
            else
                life = 0;
        }

    }

    public void Reposition()
    {
        if (positionate1 && GameObject.FindGameObjectWithTag("Enemy") == null)
        {
            Transform tcomet = GameObject.Find("TComet").transform;
            transform.position = new Vector3(0, transform.position.y, 0);
            tcomet.position = new Vector3(150f, tcomet.position.y, 0);
            positionate1 = false;
        }

        else if (positionate2)
        {
            try
            {
                Transform enemyship2 = GameObject.FindGameObjectWithTag("Enemy").transform;
                transform.position = new Vector3(150, transform.position.y, 0);
                enemyship2.position = new Vector3(transform.position.x + 30f, enemyship2.position.y, 0);
                positionate2 = false;
            } catch (MissingReferenceException e)
            {

            }
            inst2 = false;
        }
    }

    private void Dead()
    {
        if (life <= 0)
        {
            Instantiate(explosion, transform.position, transform.rotation);
            Explosion.restart = true;
            Destroy(gameObject);

        }
    }



}
