using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : MonoBehaviour {

    private float left, right, left2, right2,right3, left3;
    private float speed;
    public GameObject bomb;

	// Use this for initialization
	void Start () {
        right = 65.02f;
        left = 36.31f;
        speed = 3.5f;

        right2 = 56.98f;
        left2 = 42.6f;
        right3 = 117.9f;
        left3 = 108.0f;

        if (gameObject.name == "Ghost1")
        {
            StartCoroutine("ThrowBomb");
        }
        

    }
	
	// Update is called once per frame
	void Update () {
       
        if(gameObject.name == "Ghost")
        {
            Movement1();
        }
        else if (gameObject.name == "Ghost3" || gameObject.name == "Ghost4")
        {
            Movement3();
        }
        else
        {
            Movement2();

        }
        transform.Translate(speed * Time.deltaTime, 0, 0);
    }


    void OnTriggerEnter2D(Collider2D c)
    {
        if(c.gameObject.tag == "Kunai")
        {
            Destroy(gameObject);
        }
    }

    private void Movement1()
    {
        if (transform.position.x < left)
        {
            transform.Rotate(0, 180, 0);
            GameObject bombclone = (GameObject)Instantiate(bomb, transform.position, Quaternion.Euler(new Vector3(0, 0, 0)));
            bombclone.SendMessage("ChangeDirection", "right");
        }
        else if (transform.position.x > right)
        {
            transform.Rotate(0, 180, 0);
            GameObject bombclone = (GameObject)Instantiate(bomb, transform.position, Quaternion.Euler(new Vector3(0, 0, 0)));
            bombclone.SendMessage("ChangeDirection", "left");
        }
    }

    private void Movement2()
    {
        if (transform.position.x < left2)
        {
            transform.Rotate(0, 180, 0);  
        }
        else if (transform.position.x > right2)
        {
            transform.Rotate(0, 180, 0);
        }
        
    }

    private void Movement3()
    {
        if (transform.position.x < left3)
        {
            transform.Rotate(0, 180, 0);
            GameObject bombclone = (GameObject)Instantiate(bomb, transform.position, Quaternion.Euler(new Vector3(0, 0, 0)));
            bombclone.SendMessage("ChangeDirection", "right");
        }
        else if (transform.position.x > right3)
        {
            transform.Rotate(0, 180, 0);
            GameObject bombclone = (GameObject)Instantiate(bomb, transform.position, Quaternion.Euler(new Vector3(0, 0, 0)));
            bombclone.SendMessage("ChangeDirection", "left");
        }
    }

    IEnumerator ThrowBomb()
    {
        while (true)
        {
            GameObject bombclone = (GameObject)Instantiate(bomb, transform.position, Quaternion.Euler(new Vector3(0, 0, 0)));
            bombclone.SendMessage("ChangeDirection", "down");
            
            yield return new WaitForSeconds(1);
        }
        
    }

}
