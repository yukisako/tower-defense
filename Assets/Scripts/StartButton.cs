using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartButton : MonoBehaviour {

	public void SceneLoad (){
		Application.LoadLevel ("Main");
	}

	public void HowToPlay(){
		MyCanvas.SetActive ("ExplainEnglishText", false);
		MyCanvas.SetActive ("ExplainJapaneseText", true);
		MyCanvas.SetActive ("ExplainImage", true);
		MyCanvas.SetActive ("CloseButton", true);
		MyCanvas.SetActive ("JapaneseButton", false);
		MyCanvas.SetActive ("EnglishButton", true);
	}

	public void CloseButton(){
		MyCanvas.SetActive ("ExplainImage", false);
		MyCanvas.SetActive ("CloseButton", false);
		MyCanvas.SetActive ("JapaneseButton", false);
		MyCanvas.SetActive ("EnglishButton", false);
		MyCanvas.SetActive ("ExplainEnglishText", false);
		MyCanvas.SetActive ("ExplainJapaneseText", false);
	}

	public void selectJapanese(){
		MyCanvas.SetActive ("ExplainEnglishText", false);
		MyCanvas.SetActive ("ExplainJapaneseText", true);
		MyCanvas.SetActive ("JapaneseButton", false);
		MyCanvas.SetActive ("EnglishButton", true);
	}

	public void selectEnglish(){
		MyCanvas.SetActive ("ExplainEnglishText", true);
		MyCanvas.SetActive ("ExplainJapaneseText", false);
		MyCanvas.SetActive ("JapaneseButton", true);
		MyCanvas.SetActive ("EnglishButton", false);
	}
}