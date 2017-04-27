using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class BarraVida : MonoBehaviour {

	public AudioClip sound = null;
	public Scrollbar HealthBar;
	public static float Health = 100;

	public static void Damage(float value) {
		Health = value;
		//HealthBar.size = Health / 100f;

	}

	void gameOver() {
		//SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		//print ("Game Over");

	}

	public void win () {
		AudioSource.PlayClipAtPoint (sound, transform.position, 1);
	}



}