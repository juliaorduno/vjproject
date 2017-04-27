﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class Player2 : MonoBehaviour
{

    private Animator animator;
    public float speed;
    public float jumpSpeed;
    public Rigidbody2D rb;
    private bool climb, gliding, grow, powerAttack, powerGlide, instSkeleton;
    private static bool grounded;
    private bool facingRight;
    public GameObject kunai, saw;
    public int lightning, life;
    private static bool attacking, moving;
    public Spike[] spikes;
    private int spikeNo;
    public GameObject skeleton;


    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        grounded = false;
        facingRight = true;
        speed = 7f;
        jumpSpeed = 7f;
        climb = false;
        lightning = 0;
        life = 100;
        moving = false;
        attacking = false;
        gliding = false;
        grow = false;
        powerAttack = false;
        powerGlide = false;
        StartCoroutine("InstantiateSaw");
        spikeNo = 0;

        StartCoroutine("InstantiateSkeleton");
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Throw();
        Dead();
        Grow();
    }

    public void Restart()
    {
        SceneManager.LoadScene("Scene2");
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
                rb.AddForce(new Vector2(0, jumpSpeed), ForceMode2D.Impulse);
                grounded = false;
                animator.SetBool("Grounded", grounded);
            }
        }

        if (moving && Input.GetKey("down"))
        {
            animator.SetBool("Slide", true);
        }
        else
        {
            animator.SetBool("Slide", false);
        }

        if (Input.GetKeyDown(KeyCode.A) && powerAttack)
        {
            animator.SetTrigger("Attack");
            attacking = true;
        }
        if (gliding)
        {
            transform.Translate(0, -5 * Time.deltaTime, 0);
        }
    }

    void OnCollisionEnter2D(Collision2D c)
    {
        if (c.gameObject.tag == "Ground")
        {
            grounded = true;
            animator.SetBool("Grounded", grounded);
        }
        else if (c.gameObject.name == "Boss" && !attacking)
        {
             AttackReaction(5);
             life -= 10;
             print("Life: " + life);
        }
    }

    void OnCollisionExit2D(Collision2D c)
    {
         if(c.gameObject.name == "TopFence")
        {
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            climb = false;
            animator.SetBool("OnFence", climb);
        }
        if(c.gameObject.name == "Boss")
        {
            attacking = false;
        }
        
    }

    void OnTriggerStay2D(Collider2D c)
    {
        if (c.gameObject.tag == "Fence")
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                climb = true;
                animator.SetBool("OnFence", climb);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject.tag == "Fence")
        {
            print("Press C to activate Climb Mode");
        }
        else if (c.gameObject.tag == "Enemy")
        {
            life -= 5;
            print("Life: " + life);
            AttackReaction(3);
        }
        
        else if (c.gameObject.tag == "rayo")
        {
            Destroy(c.gameObject);
            lightning++;
        }
        else if (c.gameObject.name == "Botiquin")
        {
            life += 20;
            if(life > 100)
            {
                life = 100;
            }
            Destroy(c.gameObject);
            print("Life: " + life);
        }
        else if(c.gameObject.tag == "Bullet")
        {
            life -= 10;
            Destroy(c.gameObject);
            print("Life: " + life);
            AttackReaction(3);
        }
        else if (c.gameObject.name == "GlideTrigger1")
        {
            if (powerGlide)
            {
                gliding = false;
                animator.SetBool("Glide", gliding);
                animator.SetBool("Moving", false);
                animator.SetBool("Grounded", true);
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                Destroy(c.gameObject);
            }
            else
            {
                life = 0;
            }
            
        }
        else if(c.gameObject.name == "BossTrigger")
        {
            Destroy(c.gameObject);
            Boss.Move();
        }
        else if(c.gameObject.name == "Item1")
        {
            Destroy(c.gameObject);
            grow = true;
        }
        else if (c.gameObject.name == "Item2")
        {
            Destroy(c.gameObject);
            powerAttack = true;
        }
        else if(c.gameObject.name == "Item3")
        {
            Destroy(c.gameObject);
            powerGlide = true;
        }
        else if(c.gameObject.tag == "Acid")
        {
            Dead();
        }
    }

    void OnTriggerExit2D(Collider2D c)
    {
        if (c.gameObject.tag == "Fence")
        {
            climb = false;
            animator.SetBool("OnFence", climb);
        }
        else if(c.gameObject.name == "GlideTrigger" && powerGlide)
        {
            Glide();
            Destroy(c.gameObject);
        }
        else if (c.gameObject.tag == "SpikeTrigger")
        {
            Destroy(c.gameObject);
            spikes[spikeNo].fall = true;
            spikeNo++;
        }
    }

    private void Throw() {
        if (Input.GetKeyDown("space") && grounded)
        {
            if (GameObject.FindWithTag("Kunai") == null)
            {
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

    private void Glide()
    {
        gliding = true;
        animator.SetBool("Glide", gliding);
        rb.constraints = RigidbodyConstraints2D.FreezePositionY;
        
    }

    private void Dead()
    {
        if(life <= 0)
        {
            animator.SetTrigger("Dead");
        }
    }

    private void Grow()
    {
        if (Input.GetKeyDown(KeyCode.Return) && grow)
        {
            transform.localScale = new Vector2(1f, 1f);
            kunai.transform.localScale = new Vector2(1f, 1f);
            Kunai.speed *= 2;
            speed *= 2;
            jumpSpeed = 10f;
        }
    }

    public static bool Grounded
    {
        get { return !grounded; }
    }

    public static bool Attacking
    {
        get { return attacking; }
    }

    public static bool Moving
    {
        get { return moving; }
    }

    public void AttackReaction(int x)
    {
        if (facingRight)
            x *= -1;
        rb.AddRelativeForce(new Vector2(x, 5), ForceMode2D.Impulse);
    }

    IEnumerator InstantiateSaw()
    {
        while (true)
        {
            Instantiate(saw, new Vector2(36.79f, -9.58f), transform.rotation);
            yield return new WaitForSeconds(2f);
        }
    }

    IEnumerator InstantiateSkeleton()
    {
        while (instSkeleton)
        {
            Instantiate(skeleton, new Vector2(100.07f, 10.99f), transform.rotation);
            yield return new WaitForSeconds(15f);
        }


    }
}
