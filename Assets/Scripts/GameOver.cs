using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour {

	const int MaxEnemyCount = 100;
	private static TextObj GameoverText;



	public static bool IsGameover(){
		return Enemy.EnemyCount () > MaxEnemyCount;
	}

	public static void GameoverAction(){

		Time.timeScale = 0;
		MyCanvas.SetActive ("TextGameoverLabel", true);
		MyCanvas.SetActive ("TextGameover", true);
		GameoverText = MyCanvas.Find<TextObj> ("TextGameoverLabel");

		GameoverText.SetLabelFormat ("Wave {0} Score {1:D6}", Global.Wave, Global.Score);
		MyCanvas.SetActive ("TextWaveStart", false);
		MyCanvas.SetActive ("ButtonReplay", true);
		MyCanvas.SetActive ("ButtonTweet", true);
		MyCanvas.SetActive ("ImageGameover", true);
		MyCanvas.SetActive ("ButtonResistRanking", true);
		MyCanvas.SetActive ("InputNickname", true);
		MyCanvas.SetActive ("TextNicknameLabel", true);
	}




	public void Tweet(){

		string oldUrl = "DEFENDERでウェーブ" + Global.Wave + "まで到達、そしてスコア" + Global.Score+"を獲得しました。" ;
		Debug.Log (oldUrl);
		string url = WWW.EscapeURL(oldUrl);
		Debug.Log (url);
		Application.OpenURL("https://twitter.com/intent/tweet?text="+url+"&hashtags=BANGBANG100");
	}
}
