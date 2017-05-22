using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour {

	public Image black;
	public Animator anim;
	public string sceneName;
    public Button newGame;

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


		if (Input.GetKeyDown(KeyCode.Return)) {
			StartCoroutine (Fading());
		}

        if (Input.GetMouseButtonDown(0))
        {
            SceneManager.LoadScene("Scene1");
        }

	}
}
