using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1L3 : MonoBehaviour {

    public int maxSpeed;

    private Vector3 startPosition;

    public Transform targetTransform;
    public Animator a;
    private bool stop;
    private int life;

    // Use this for initialization
    void Start()
    {
        maxSpeed = 20;
        a = GetComponent<Animator>();
        try
        {
            targetTransform = GameObject.Find("Ship").transform;
        } catch(MissingReferenceException e)
        {

        }
        
        stop = false;
        life = 100;
    }

    // Update is called once per frame
    void Update()
    {
        MoveVertical();
        if(life <= 0)
        {
            a.SetTrigger("Explode");
            stop = true;
        }
    }

    void MoveVertical()
    {
        //transform.position = new Vector3(transform.position.x, startPosition.y + Mathf.Sin(Time.time * maxSpeed), transform.position.z);
        if (!stop && targetTransform != null)
        {
            //transform.position = new Vector3(transform.position.x, Mathf.Sin(Time.time * maxSpeed + 1) * 4, transform.position.z);
            transform.Translate(maxSpeed * Time.deltaTime, 0, 0);
            Vector3 vectorToTarget = targetTransform.position - transform.position;
            float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * maxSpeed);
        }
        
    }

    void OnCollisionEnter2D(Collision2D c) {
        if (c.gameObject.name == "Ship")
            life = 0;
    }

    void OnTriggerEnter2D(Collider2D c)
    {
        if(c.gameObject.tag == "Laser")
            life -= 30;
    }

    public void Dead() {
        Destroy(gameObject);
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

}
