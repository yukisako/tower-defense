using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartButton : MonoBehaviour {

	private bool japanese = true; 




	public void SceneLoad (){
		Application.LoadLevel ("Main");
	}

	public void HowToPlay(){
		MyCanvas.SetActive ("LanguageButton", true);
		MyCanvas.SetActive ("ExplainImage", true);
		MyCanvas.SetActive ("CloseButton", true);
		MyCanvas.SetActive ("ExplainEnglishText", false);
		MyCanvas.SetActive ("ExplainJapaneseText", true);
	}

	public void CloseButton(){
		MyCanvas.SetActive ("ExplainImage", false);
		MyCanvas.SetActive ("CloseButton", false);
		MyCanvas.SetActive ("LanguageButton", false);
		MyCanvas.SetActive ("ExplainEnglishText", false);
		MyCanvas.SetActive ("ExplainJapaneseText", false);
	}

	public void selectLangage(){
		Debug.Log (japanese);
		if (japanese == false) {
			//日本語から英語へ切り替え
			MyCanvas.SetActive ("ExplainEnglishText", false);
			MyCanvas.SetActive ("ExplainJapaneseText", true);
//			selectLangageButton.FormatLabel ("Japanese");
			japanese = true;
		} else {
			Debug.Log (japanese);
			MyCanvas.SetActive ("ExplainEnglishText", true);
			MyCanvas.SetActive ("ExplainJapaneseText", false);
//			selectLangageButton.FormatLabel ("English");
			japanese = false;
		}
	}
//	private Canvas _canvas;
//	void Start () {
//		// Canvasコンポーネントを保持
//		_canvas = GetComponent<Canvas>();
//	}
//
//	/// 表示・非表示を設定する
//	public void SetActive(string name, bool b) {
//		foreach(Transform child in _canvas.transform) {
//			// 子の要素をたどる
//			if(child.name == name) {
//				// 指定した名前と一致
//				// 表示フラグを設定
//				child.gameObject.SetActive(b);
//				// おしまい
//				return;
//			}
//		}
//		// 指定したオブジェクト名が見つからなかった
//		Debug.LogWarning("Not found objname:"+name);
//	}
}
