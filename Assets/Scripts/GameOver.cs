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
	}
}
