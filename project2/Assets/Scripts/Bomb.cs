using UnityEngine;
using System.Collections;

public class Bomb : MonoBehaviour
{
    // Values
    public float speed;
    private string direction;

    void Start()
    {
        speed = 7f;
    }

    void Update()
    {
        Move();
    }
    private void Move()
    {
        if (direction == "right")
        {
            transform.Translate(speed * Time.deltaTime, 0, 0, Space.World);
        }
        else if (direction == "left")
        {
            transform.Translate(-speed * Time.deltaTime, 0, 0, Space.World);
        }
        else if (direction == "up")
        {
            transform.Translate(0, speed * Time.deltaTime, 0, Space.World);
        }
        else if (direction == "down")
        {
            transform.Translate(0, -speed * Time.deltaTime, 0, Space.World);
        }
    }
    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
    void ChangeDirection(string dir)
    {
        direction = dir;
    }

    void OnTriggerEnter2D(Collider2D c)
    {
        
        if(c.gameObject.name == "LimitBomb")
        {
            Destroy(gameObject);
        }
    }

}
