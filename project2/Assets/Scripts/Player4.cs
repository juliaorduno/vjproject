using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player4 : MonoBehaviour {

    public bool jumpInit;
    public Animator a;
    public Rigidbody2D rb;
    public float speed;
    public float jumpSpeed;
    private static bool grounded;
    private bool facingRight;
    public GameObject kunai, doorOpen, blood, head, head2, boss, thunder, item;
    public int lightning, life, num;
    private static bool moving;
    private bool attacking, powerThunder, colliding;
    public Camera cam;
    private BarraVida vida;
    public AudioClip[] soundfx;

    // Use this for initialization
    void Start () {
        a = gameObject.GetComponent<Animator>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        grounded = false;
        facingRight = true;
        speed = 7f;
        jumpSpeed = 7f;
        lightning = 0;
        life = 100;
        moving = false;
        attacking = false;
        cam = Camera.allCameras[0];
        vida = cam.GetComponent<BarraVida>();
        num = 0;
        powerThunder = false;
        colliding = false;
        
        if (jumpInit)
        {
            rb.AddForce(new Vector2(-2, jumpSpeed), ForceMode2D.Impulse);
            grounded = false;
            a.SetBool("Grounded", grounded);
        }
    }
	
	// Update is called once per frame
	void Update () {
        Movement();
        Throw();
        Dead();
        vida.setHealth(life);
        if (life < 50 && !powerThunder)
        {
            Instantiate(item, new Vector3(50.68f, -5.808258f, transform.position.z), transform.rotation);
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene("Scene4");
    }

    private void Movement()
    {
        float h = Input.GetAxisRaw("Horizontal");
        if (h != 0)
        {
            moving = true;
            a.SetBool("Moving", moving);
            if ((h > 0 && !facingRight) || (h < 0 && facingRight))
            {
                facingRight = !facingRight;
                a.transform.Rotate(0, 180, 0);
            }
        }
        else
        {
            moving = false;
            a.SetBool("Moving", moving);
        }
        if (facingRight)
        {
            transform.Translate(h * speed * Time.deltaTime, 0, 0);
        }
        else
        {
            transform.Translate(-h * speed * Time.deltaTime, 0, 0);
        }


        if (Input.GetKeyDown("up") && grounded)
        {
            GetComponent<AudioSource>().clip = soundfx[0];
            GetComponent<AudioSource>().Play();
            rb.AddForce(new Vector2(0, jumpSpeed), ForceMode2D.Impulse);
            grounded = false;
            a.SetBool("Grounded", grounded);
        }

        if (moving && Input.GetKey("down"))
        {
            a.SetBool("Slide", true);
        }
        else
        {
            a.SetBool("Slide", false);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            GetComponent<AudioSource>().clip = soundfx[5];
            GetComponent<AudioSource>().Play();
            a.SetTrigger("Attack");
            attacking = true;
        }

        if (Input.GetKeyDown(KeyCode.S) && powerThunder)
        {
            Instantiate(thunder, new Vector2(boss.transform.position.x, 2.730425f), transform.rotation);
        }
    }

    void OnCollisionExit2D(Collision2D c)
    {
        if (c.gameObject.tag == "Enemy")
            attacking = false;
        if(c.gameObject == boss)
        {
            colliding = false;
            attacking = false;
        }
    }

    void OnCollisionEnter2D(Collision2D c)
    {
        if (c.gameObject.tag == "Ground")
        {
            grounded = true;
            a.SetBool("Grounded", grounded);
        }

        if(c.gameObject == boss)
        {

            if (!attacking)
            {
                GetComponent<AudioSource>().clip = soundfx[4];
                GetComponent<AudioSource>().Play();
                life -= 10;
            }
            colliding = true;
        }

        if(c.gameObject.tag == "Enemy" && !attacking)
        {
            bool isJumping = c.gameObject.GetComponent<Enemy3L3>().Jumping;
            
            if (isJumping)
            {
                num++;
                if(num == 1)
                {
                    GetComponent<AudioSource>().clip = soundfx[4];
                    GetComponent<AudioSource>().Play();
                    a.SetTrigger("Dead2");
                    Instantiate(blood, head.transform.position, head.transform.rotation);
                    Instantiate(head2, new Vector2(head.transform.position.x-1, head.transform.position.y+1), head.transform.rotation);
                    vida.setHealth(0);
                }
                
            }
        }
    }

    void OnTriggerExit2D(Collider2D c)
    {
        if(c.gameObject.name == "BossTrigger")
        {
            boss.GetComponent<FinalBoss>().Move();
        }
    }

    void OnTriggerStay2D(Collider2D c)
    {
        if(c.gameObject.name == "DoorUnlocked")
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                //GetComponent<AudioSource>().clip = soundfx[6];
                //GetComponent<AudioSource>().Play();
                Instantiate(doorOpen, new Vector2(c.transform.position.x, c.transform.position.y), transform.rotation);
                Destroy(c.gameObject);
                SceneManager.LoadScene("Scene4");
            }
        }
    }

    void OnTriggerEnter2D(Collider2D c)
    {
         if (c.gameObject.tag == "Enemy")
        {
            GetComponent<AudioSource>().clip = soundfx[4];
            GetComponent<AudioSource>().Play();
            life -= 5;
            AttackReaction(3);
        }
        else if (c.gameObject.name == "Botiquin")
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
        else if(c.gameObject.tag == "Item")
        {
            GetComponent<AudioSource>().clip = soundfx[2];
            GetComponent<AudioSource>().Play();
            Destroy(c.gameObject);
            powerThunder = true;
            
        }
    }

    private void Throw()
    {
        if (Input.GetKeyDown("space") && grounded)
        {
            if (GameObject.FindWithTag("Kunai") == null)
            {
                GetComponent<AudioSource>().clip = soundfx[3];
                GetComponent<AudioSource>().Play();
                a.SetTrigger("Throw");
                if (facingRight)
                {
                    GameObject kunaiclone = (GameObject)Instantiate(kunai, transform.position, Quaternion.Euler(new Vector3(0, 0, 0)));
                    kunaiclone.SendMessage("ChangeDirection", "right");
                }
                else
                {
                    GameObject kunaiclone = (GameObject)Instantiate(kunai, transform.position, Quaternion.Euler(new Vector3(0, 180, 0)));
                    kunaiclone.SendMessage("ChangeDirection", "left");
                }
            }
        }
    }

    private void Dead()
    {
        if (life <= 0)
        {
            a.SetTrigger("Dead");
        }
    }

    public static bool Grounded
    {
        get { return !grounded; }
    }

    public bool Attacking
    {
        get { return attacking && colliding; }
    }

    public bool Moving
    {
        get { return moving; }
    }

    public bool FacingRight
    {
        get { return facingRight; }
    }

    public void AttackReaction(int x)
    {
        if (facingRight)
            x *= -1;
        rb.AddRelativeForce(new Vector2(x, 5), ForceMode2D.Impulse);
    }

}
