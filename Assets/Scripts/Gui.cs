using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gui{
	private TextObj moneyText;
	private TextObj costText;
	private ButtonObj buyButton;
	private TextObj enemyCountText;
	private TextObj waveText;
	private TextObj towerInfoText;
	private TextObj timerText;
	private ButtonObj rangeButton;
	private ButtonObj firerateButton;
	private ButtonObj powerButton;

	public Gui(){
		moneyText = MyCanvas.Find<TextObj> ("TextMoney");
		buyButton = MyCanvas.Find<ButtonObj> ("ButtonBuy");
		costText = MyCanvas.Find<TextObj> ("TextCost");
		enemyCountText = MyCanvas.Find<TextObj> ("TextEnemyCount");
		costText.Label = "";
		waveText = MyCanvas.Find<TextObj> ("TextWave");
		timerText = MyCanvas.Find<TextObj> ("TextTimer");
		towerInfoText = MyCanvas.Find<TextObj> ("TextTowerInfo");
		rangeButton = MyCanvas.Find<ButtonObj> ("ButtonRange");
		firerateButton = MyCanvas.Find<ButtonObj> ("ButtonFirerate");
		powerButton = MyCanvas.Find<ButtonObj> ("ButtonPower");

	}


	public void Update (GameManager.eSelectMode selectMode,Tower tower) {
		waveText.SetLabelFormat ("Wave:{0}", Global.Wave);

		moneyText.SetLabelFormat ("¥ {0}", Global.Money);

		timerText.SetLabelFormat ("Next: {0}", (int)(Global.NextWaveTimer));

		costText.Label = "";

		if (selectMode == GameManager.eSelectMode.Upgrade) {
			towerInfoText.SetLabelFormat ("<<Tower Info>>\n  Rnage: Lv{0}\n  Firerate: Lv{1}\n  Power: Lv{2}"
				, tower.LevelRange, tower.LevelFirerate, tower.LevelPower);
		}

		int money = Global.Money;

		if (tower) {

			rangeButton.Enabled = (money >= tower.CostRange);
			rangeButton.FormatLabel ("Range ¥{0}", tower.CostRange);

			firerateButton.Enabled = (money >= tower.CostFirerate);
			firerateButton.FormatLabel ("Firerate ¥{0}", tower.CostFirerate);

			powerButton.Enabled = (money >= tower.CostPower);
			powerButton.FormatLabel ("Power ¥{0}", tower.CostPower);
		}

		int cost = Cost.TowerProduction ();

		if (selectMode == GameManager.eSelectMode.Buy) {
			costText.SetLabelFormat ("(cost ¥{0})", cost);
		}
		buyButton.Enabled = (Global.Money >= cost);

		buyButton.FormatLabel ("Buy (¥{0})", cost);

		countEnemy ();

	}

	private void countEnemy(){
		int enemyCount = Enemy.EnemyCount ();
		enemyCountText.SetLabelFormat ("{0}/100", enemyCount);
	}
}