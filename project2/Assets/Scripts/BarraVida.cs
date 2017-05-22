using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class BarraVida : MonoBehaviour {

	//public AudioClip sound = null;

	public Scrollbar HealthBar;
	public float Health = 100;

	public void setHealth(float value) {
		Health = value;
		HealthBar.size = Health / 100f;
	}

}