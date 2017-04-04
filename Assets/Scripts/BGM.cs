using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM : MonoBehaviour {

	public AudioClip gameoverMusic;
	private AudioSource audioSource;
	private void play(){
		audioSource = GetComponent<AudioSource> ();
		audioSource.clip = gameoverMusic;
		audioSource.Play ();
	}
		

	public void Play(){
		if(GameOver.IsGameover()){
			play();
		}
	}

	public void Stop(){
		audioSource.Stop ();
	}
}
