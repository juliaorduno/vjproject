using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Intro : MonoBehaviour {

	public float timer = 5.0f;

	public Image black;
	public Animator anim;
	public string sceneName;

	// Use this for initialization
	void Start () {
		
	}

	IEnumerator Fading(){
		anim.SetBool ("Fade", true);
		yield return new WaitUntil (() => black.color.a == 1);
		SceneManager.LoadScene(sceneName);
	}
	
	// Update is called once per frame
	void Update () {

		timer -= Time.deltaTime;

		if (timer < 0) {
			StartCoroutine (Fading());
		}
		
	}
}
