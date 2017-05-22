using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ChangeMusicVolume : MonoBehaviour {

	public DontDestroy DontDestory;
	public Slider Volume;
	private AudioSource myMusic;

	/*
	void Start(){
		myMusic = DontDestroy.GetComponent<GameObject>();
	}
	
	// Update is called once per frame
	void Update () {
		myMusic.volume = Volume.value;
	}*/
}
