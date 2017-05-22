using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBoss : MonoBehaviour {

    private Rigidbody2D rb;
    public GameObject player, bullet, explosion, david;
    private float speed;
    private float minDistance, minBlueDistance;
    private float range;
    private Vector3 target;
    private Animator animator;
    private float pastDistance;
    private bool facingRight;
    private int life;
    private static bool move, isMoving, isUp;

    // Use this for initialization
    void Start () {
        minDistance = 5f;
        minBlueDistance = 5f;
        target = player.transform.position;
        pastDistance = transform.position.x - target.x;
        animator = GetComponent<Animator>();
        StartCoroutine(targetChange());
        facingRight = false;
        life = 10;
        move = false;
        speed = 5f;
        isMoving = false;
        isUp = false;
        StartCoroutine(WaitToYellow());
    }
	
	// Update is called once per frame
	void Update () {
        if (move)
        {
            Blue();

        }

        if (life <= 0)
        {
            move = false;
            Instantiate(explosion, transform.position, transform.rotation);
            Instantiate(david, new Vector2(48.53092f, 18.8f), transform.rotation);
            Destroy(gameObject);
        }

    }

    public void Movement()
    {
        if (move)
        {
            range = Vector2.Distance(transform.position, target);

            if (range > minDistance && facingRight != player.GetComponent<Player4>().FacingRight)
            {
                StopCoroutine(WaitToMove());
                animator.SetBool("move2", true);
                isMoving = true;
                
            }
            else if (range > minDistance && facingRight == player.GetComponent<Player4>().FacingRight)
            {
                animator.SetBool("move2", false);
                isMoving = false;
            }
        }

        if (isMoving)
        {
            transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
        }
        else
            transform.Translate(0, 0, 0);
    }

    public void Yellow()
    {
        if (facingRight == player.GetComponent<Player4>().FacingRight && player.GetComponent<Player4>().Moving)
        {
            animator.SetTrigger("yellow");
            
        } 
        isMoving = false;
    }

    public void Blue()
    {
        if (GameObject.FindGameObjectWithTag("Kunai") != null)
        {
            animator.SetBool("move2", false);
            float rangeKunai = Vector2.Distance(transform.position, GameObject.FindGameObjectWithTag("Kunai").transform.position);
            if (rangeKunai < minBlueDistance)
                animator.SetTrigger("blue");
            isMoving = false;
        }
        else if (player.GetComponent<Player4>().Attacking && !isMoving)
        {
            animator.SetBool("move2", false);
            animator.SetTrigger("blue");
            isMoving = false;
        }
        else
            Movement();
    }

    IEnumerator WaitToYellow()
    {
        while (true)
        {
            //Yellow();
            Shoot();
            yield return new WaitForSeconds(3f);
        }
    }


    public void EndBlue()
    {
        if (player.GetComponent<Player4>().FacingRight)
            transform.position = new Vector2(target.x - 7f, transform.position.y);
        else
            transform.position = new Vector2(target.x + 7f, transform.position.y);
        animator.SetTrigger("endblue");
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
                animator.SetBool("move2", false);
            }
            pastDistance = distance;
            yield return new WaitForSeconds(1f);
        }
    }


   private void Shoot()
    {
        if (GameObject.FindWithTag("Bullet") == null && (facingRight == player.GetComponent<Player4>().FacingRight))
        {
            animator.SetTrigger("throw");
            if (facingRight)
            {
                GameObject bulletclone = (GameObject)Instantiate(bullet, transform.position, Quaternion.Euler(new Vector3(0, 0, 0)));
                bulletclone.SendMessage("ChangeDirection", "right");
            }
            else
            {
                GameObject bulletclone = (GameObject)Instantiate(bullet, transform.position, Quaternion.Euler(new Vector3(0, 180, 0)));
                bulletclone.SendMessage("ChangeDirection", "left");
            }
        }
        isMoving = false;
    }

    void OnCollisionExit2D(Collision2D c)
    {
        if(c.gameObject == player)
        {
            if (isMoving)
            {
                animator.SetBool("move2", false);
                StartCoroutine(WaitToMove());
            }
        }
    }

    void OnCollisionEnter2D(Collision2D c)
    {
        if(c.gameObject == player && player.GetComponent<Player4>().Attacking)
        {
            life -= 5;
        }
    }

    void OnTriggerEnter2D(Collider2D c)
    {
        if(c.gameObject.tag == "Kunai")
        {
            life -= 5;
        }
        if(c.gameObject.tag == "rayo")
        {
            life -= 10;
            animator.SetTrigger("blue");
            isMoving = false;
        }
    }

    IEnumerator WaitToMove()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f);
        }
    }

    public void Move()
    {
        move = true;
    }


}
