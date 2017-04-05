using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {

    private float up, down, up2, down2;
    private float speed;

	// Use this for initialization
	void Start () {
        up = 4f;
        down = -2f;

        up2 = 22.81f;
        down2 = 15f;
    }
	
	// Update is called once per frame
	void Update () {

        if (Player.SwitchOn && gameObject.name == "Tile1")
        {
            if (transform.position.y > up)
            {
                speed = -2.5f;
            }
            else if (transform.position.y < down)
            {
                speed = 2.5f;
            }
            transform.Translate(0, speed * Time.deltaTime, 0);
        } 
        
        else if(gameObject.name == "Tile2")
        {
            if (transform.position.y > up2)
            {
                speed = -3.5f;
            }
            else if (transform.position.y < down2)
            {
                speed = 3.5f;
            }
            transform.Translate(0, speed * Time.deltaTime, 0);
        } 
    }
}
