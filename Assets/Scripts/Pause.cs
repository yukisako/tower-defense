using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour {

	public void PauseFinish(){
		if (Time.timeScale == 0.0f) {
			Time.timeScale = 1.0f;
			MyCanvas.SetActive ("ButtonReplay", false);
			MyCanvas.SetActive ("ButtonBack", false);
			MyCanvas.SetActive ("ImageGameover", false);
		}
	}

	public void PauseStart(){
		{
			Time.timeScale = 0.0f;
			MyCanvas.SetActive ("ButtonReplay", true);
			MyCanvas.SetActive ("ButtonBack", true);
			MyCanvas.SetActive ("ImageGameover", true);
		}
	}


}
