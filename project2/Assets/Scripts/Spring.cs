using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : MonoBehaviour
{
    private Animator animator;
    private bool moving;

    // Use this for initialization
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        moving = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter2D(Collision2D c)
    {
        if (!moving) { 
            animator.SetTrigger("Move");
            moving = true;
        }
    }

    void OnCollisionExit2D(Collision2D c)
    {
        moving = false;
    }
}
