using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Boss : MonoBehaviour
{
    public GameObject player, bullet;
    public float speed = 2f;
    private float minDistance, minShootDistance;
    private float range;
    private Vector3 target;
    private Animator animator;
    private float pastDistance;
    private bool facingRight;
    private int life;
    private static bool move;
    // Use this for initialization

    void Start()
    {
        minDistance = 10f;
        minShootDistance = 5f;
        target = player.transform.position;
        pastDistance = transform.position.x - target.x;
        animator = GetComponent<Animator>();
        StartCoroutine(targetChange());
        facingRight = false;
        life = 100;
        move = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (move)
        {
            range = Vector2.Distance(transform.position, target);

            if (range > minDistance)
            {
                animator.SetBool("Moving", true);
                transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
                if (range > minShootDistance && Player2.Moving)
                {
                    Shoot();
                }
            }
            else
            {
                transform.Translate(0, speed * Time.deltaTime, 0);
                animator.SetBool("Moving", false);
            }
        }

        if(life <= 0)
        {
            move = false;
            animator.SetTrigger("Dead");
        }

    }

    void OnCollisionEnter2D(Collision2D collider)
    {
        if (collider.gameObject.name == "Ninja")
        {
            if (Player2.Attacking)
            {
                life -= 10;
                print("Boss life: " + life);
                animator.SetTrigger("Melee");
            }
        }
        
    }

    void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject.tag == "Kunai")
        {
            life -= 5;
            print("Boss life: " + life);
            Shoot();
            Destroy(c.gameObject);
        }
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

    private void Shoot()
    {
      if (GameObject.FindWithTag("Bullet") == null)
            {
                animator.SetTrigger("Shoot");
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
        }

    public static void Move()
    {
        move = true;
    }

    public void DestroyBoss()
    {
        Destroy(gameObject);
        SceneManager.LoadScene("Scene3");
    }
}
