using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Explosion : MonoBehaviour {

    public static bool restart;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Dead()
    {
        if (restart)
            Restart();
        Destroy(gameObject);
        
    }

    public void Restart()
    {
        SceneManager.LoadScene("Scene3");
    }
}
