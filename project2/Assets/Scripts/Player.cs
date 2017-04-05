﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        grounded = false;
        facingRight = true;
        speed = 5f;
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
                if (lightning == 3)
                {
                    Instantiate(doorOpen, new Vector2(c.transform.position.x, c.transform.position.y), transform.rotation);
                    Destroy(c.gameObject);
                    print("Level Ended");
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
            Destroy(c.gameObject);
            life -= 5;
            print("Life: " + life);
        }
        else if (c.gameObject.tag == "Enemy")
        {
            life -= 10;
            print("Life: " + life);
        }
        
        else if (c.gameObject.tag == "rayo")
        {
            Destroy(c.gameObject);
            lightning++;
        }
        else if (c.gameObject.name == "Item1")
        {
            Destroy(c.gameObject);
            powerClimb = true;
            print("Power Up: You can climb now!");
        }

        else if(c.gameObject.name == "Item2")
        {
            Destroy(c.gameObject);
            powerSlide = true;
            print("Power Up: You can slide now!");
        }
        else if (c.gameObject.name == "Botiquin")
        {
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

}
