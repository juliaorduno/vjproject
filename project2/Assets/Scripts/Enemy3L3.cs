using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy3L3 : MonoBehaviour {

    private float speed;
    public Rigidbody2D rb;
    private bool move, facingRight, jumping;
    public GameObject explosion;
    public Transform player;
    private float pastDistance;
    private Vector3 target;
    private Animator animator;
    public int level;

    // Use this for initialization
    void Start () {
        speed = 2f;
        rb = transform.GetComponent<Rigidbody2D>();
        facingRight = false;
        jumping = false;
        target = player.transform.position;
        pastDistance = transform.position.x - target.x;
        StartCoroutine(targetChange());
        StartCoroutine(Jump());
        animator = gameObject.GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
        transform.Translate(speed * Time.deltaTime, 0, 0);
	}

    IEnumerator targetChange()
    {
        while (true)
        {
            
            target = player.transform.position;
            float distance = transform.position.x - target.x;
            if (Mathf.Sign(pastDistance) != Mathf.Sign(distance))
            {
                animator.transform.Rotate(0, 180, 0);
                facingRight = !facingRight;
            }
            pastDistance = distance;
            yield return new WaitForSeconds(1f);
        }
    }

    IEnumerator Jump()
    {
        while (true)
        {
            rb.AddForce(new Vector2(0, 7f), ForceMode2D.Impulse);
            jumping = true;
            yield return new WaitForSeconds(2f);
        }
    }


    void OnTriggerEnter2D(Collider2D c)
    {
        if(c.gameObject.tag == "Kunai")
        {
            Instantiate(gameObject, transform.position, transform.rotation);
        }
    }

    void OnCollisionEnter2D(Collision2D c)
    {
        if (c.gameObject.tag == "Ground")
            jumping = false;
        if (level == 2)
        {
            if (c.gameObject.name == "Ninja" && Player2.Attacking)
            {
                Instantiate(explosion, transform.position, transform.rotation);
                Destroy(gameObject);
            }
        }
        else {
            if (c.gameObject.name == "Ninja" && c.gameObject.GetComponent<Player4>().Attacking)
            {
                Instantiate(explosion, transform.position, transform.rotation);
                Destroy(gameObject);
            }
        } 
    }

   

    public bool Jumping
    {
        get { return jumping; }
    }
}
