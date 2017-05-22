using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    private Animator animator;
    public float speed;
    public float jumpSpeed;
    public Rigidbody2D rb;
    private bool climb, powerClimb, powerSlide, detectSwitchKey, instSkeleton, moving;
    private static bool grounded;
    private bool facingRight;
    public GameObject kunai, skeleton, doorOpen;
    public int lightning, life;
    private static bool switchOn;
    public Camera cam;
    private BarraVida vida;
    public AudioClip[] soundfx; // Jump=0, Rayo=1, Item=2, Knife=3, Switch=4, Pain=5, Locked=6, Win=7,Spring=8

    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
        vida = cam.GetComponent<BarraVida>();
        rb = GetComponent<Rigidbody2D>();
        grounded = false;
        facingRight = true;
        speed = 7f;
        jumpSpeed = 7f;
        climb = false;
        powerClimb = false;
        powerSlide = false;
        lightning = 0;
        switchOn = false;
        detectSwitchKey = false;
        life = 100;
        instSkeleton = true;
        moving = false;
        StartCoroutine("InstantiateSkeleton");

    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Throw();
        Dead();
        vida.setHealth(life);
        if (detectSwitchKey)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                switchOn = true;
            }
        }
    }

    private void Movement()
    {
        float h = Input.GetAxisRaw("Horizontal");
        if (h != 0)
        {
            moving = true;
            animator.SetBool("Moving", moving);
            if ((h > 0 && !facingRight) || (h < 0 && facingRight))
            {
                facingRight = !facingRight;
                animator.transform.Rotate(0, 180, 0);
            }
        }
        else
        {
            moving = false;
            animator.SetBool("Moving", moving);
        }
        if (facingRight)
        {
            transform.Translate(h * speed * Time.deltaTime, 0, 0);
        }
        else
        {
            transform.Translate(-h * speed * Time.deltaTime, 0, 0);
        }

        if (climb)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            float v = Input.GetAxisRaw("Vertical");
            transform.Translate(0, v * speed * Time.deltaTime, 0);
        }
        else
        {
            if (Input.GetKeyDown("up") && grounded)
            {
                GetComponent<AudioSource>().clip = soundfx[0];
                GetComponent<AudioSource>().Play();
                rb.AddForce(new Vector2(0, jumpSpeed), ForceMode2D.Impulse);
                grounded = false;
                animator.SetBool("Grounded", grounded);
            }
        }

        if (moving && Input.GetKey("down") && powerSlide)
        {
            animator.SetBool("Slide", true);
        }
        else
        {
            animator.SetBool("Slide", false);
        }
    }

    void OnCollisionEnter2D(Collision2D c)
    {
        if (c.gameObject.tag == "Ground")
        {
            grounded = true;
            animator.SetBool("Grounded", grounded);
        }
        else if(c.gameObject.name == "P1Jump")
        {
            jumpSpeed = 11f;
        }

    }

    void OnCollisionExit2D(Collision2D c)
    {
        if (c.gameObject.name == "P1Jump")
        {
            jumpSpeed = 7f;
        }
        else if(c.gameObject.name == "TopFence")
        {
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            climb = false;
            animator.SetBool("OnFence", climb);
        }
        else if(c.gameObject.name == "Spring")
        {
            GetComponent<AudioSource>().clip = soundfx[8];
            GetComponent<AudioSource>().Play();
            rb.AddForce(new Vector2(0, jumpSpeed*2.5f), ForceMode2D.Impulse);
            grounded = false;
            animator.SetBool("Grounded", grounded);
        }
    }


    void OnTriggerStay2D(Collider2D c)
    {
        if (c.gameObject.tag == "Fence" && powerClimb)
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                climb = true;
                animator.SetBool("OnFence", climb);
            }
        }
        else if (c.gameObject.name == "Switch")
        {
            detectSwitchKey = true;
        }
        else if(c.gameObject.name == "Door")
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                GetComponent<AudioSource>().clip = soundfx[6];
                GetComponent<AudioSource>().Play();
                if (lightning == 3)
                {
                    GetComponent<AudioSource>().clip = soundfx[7];
                    GetComponent<AudioSource>().Play();
                    Instantiate(doorOpen, new Vector2(c.transform.position.x, c.transform.position.y), transform.rotation);
                    Destroy(c.gameObject);
                    print("Level Ended");
                    SceneManager.LoadScene("Scene2");
                }
                else
                {
                    print("You need 3 Entrepreneur Lightnings!");
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject.tag == "Fence" && powerClimb)
        {
            print("Press C to activate Climb Mode");
        }
        else if (c.gameObject.tag == "Bomb")
        {
            GetComponent<AudioSource>().clip = soundfx[4];
            GetComponent<AudioSource>().Play();
            Destroy(c.gameObject);
            life -= 5;
            print("Life: " + life);
            AttackReaction(1);
        }
        else if (c.gameObject.tag == "Enemy")
        {
            GetComponent<AudioSource>().clip = soundfx[4];
            GetComponent<AudioSource>().Play();
            life -= 5;
            print("Life: " + life);
            AttackReaction(1);
        }
        
        else if (c.gameObject.tag == "rayo")
        {
            GetComponent<AudioSource>().clip = soundfx[1];
            GetComponent<AudioSource>().Play();
            Destroy(c.gameObject);
            lightning++;
        }
        else if (c.gameObject.name == "Item1")
        {
            GetComponent<AudioSource>().clip = soundfx[2];
            GetComponent<AudioSource>().Play();
            Destroy(c.gameObject);
            powerClimb = true;
            print("Power Up: You can climb now!");
        }

        else if(c.gameObject.name == "Item2")
        {
            GetComponent<AudioSource>().clip = soundfx[2];
            GetComponent<AudioSource>().Play();
            Destroy(c.gameObject);
            powerSlide = true;
            print("Power Up: You can slide now!");
        }
        else if (c.gameObject.name == "Botiquin")
        {
            GetComponent<AudioSource>().clip = soundfx[2];
            GetComponent<AudioSource>().Play();
            life += 20;
            Destroy(c.gameObject);
            print("Life: " + life);
        }
        else if (c.gameObject.name == "Switch")
        {
            print("Press Enter to activate Moving Platform");
        }
        else if(c.gameObject.name == "Door")
        {
            print("Press Enter to open the door");
        }
    }

    void OnTriggerExit2D(Collider2D c)
    {
        if (c.gameObject.tag == "Fence")
        {
            climb = false;
            animator.SetBool("OnFence", climb);
        }
        else if(c.gameObject.name == "Switch")
        {
            detectSwitchKey = false;
        }     
    }

    private void Throw() {
        if (Input.GetKeyDown("space") && grounded)
        {
            if (GameObject.FindWithTag("Kunai") == null)
            {
                GetComponent<AudioSource>().clip = soundfx[3];
                GetComponent<AudioSource>().Play();
                animator.SetTrigger("Throw");
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

    public static bool SwitchOn
    {
        get { return switchOn; }
    }

    public static bool Grounded
    {
        get { return !grounded; }
    }

    IEnumerator InstantiateSkeleton()
    {
        while (instSkeleton)
        {
            Instantiate(skeleton, new Vector2(44.21721f, 10.42f), transform.rotation);
            yield return new WaitForSeconds(5f);
        }   
    }

    private void Dead()
    {
        if (life <= 0)
        {
            animator.SetTrigger("Dead");
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene("Scene1");
    }

    public void AttackReaction(int x)
    {
        if (facingRight)
            x *= -1;
        rb.AddRelativeForce(new Vector2(x, 5), ForceMode2D.Impulse);
    }
}
