using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour {

	const int MaxEnemyCount = 10;
	private static TextObj GameoverText;

	public static bool IsGameover(){
		return Enemy.EnemyCount () > MaxEnemyCount;
	}

	public static void GameoverAction(){
		MyCanvas.SetActive ("TextGameoverLabel", true);
		MyCanvas.SetActive ("TextGameover", true);
		GameoverText = MyCanvas.Find<TextObj> ("TextGameoverLabel");

		GameoverText.SetLabelFormat ("Waves {0}\n Score {1}", Global.Wave, Global.Score);
		MyCanvas.SetActive ("ButtonReplay", true);
		MyCanvas.SetActive ("ButtonTweet", true);
	}

	public void Tweet(){

		string oldUrl = "DEFENDERでウェーブ" + Global.Wave + "、そしてスコア" + Global.Score+"を獲得しました。" ;
		Debug.Log (oldUrl);
		string url = WWW.EscapeURL(oldUrl);
		Debug.Log (url);
		Application.OpenURL("https://twitter.com/intent/tweet?text="+url+"&hashtags=DEFENDER");
		
//		string oldUrl = "Reached Wave" + Global.Wave + ",and Got Score " + Global.Score ;
//		Debug.Log (oldUrl);
//		string url = WWW.EscapeURL(oldUrl);
//		Debug.Log (url);
//		Application.OpenURL("https://twitter.com/intent/tweet?text="+url+"&hashtags=DEFENDER");

	}
}
