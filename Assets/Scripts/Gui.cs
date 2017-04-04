using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gui{
	private TextObj moneyText;
	private TextObj costText;
	private TextObj enemyCountText;
	private TextObj waveText;
	private TextObj towerInfoText;
	private TextObj timerText;
	private TextObj scoreText;
	private TextObj nameText;
	private TextObj descriptionText;
	private ButtonObj rangeButton;
	private ButtonObj firerateButton;
	private ButtonObj powerButton;

	private ButtonObj buyNormalButton;
	private ButtonObj buyFireButton;
	private ButtonObj buyNeedleButton;
	private ButtonObj buyCoverButton;
	private ButtonObj buyDrainButton;
	private ButtonObj buyFreezeButton;

	private Button buyNormalButtonIn;
	private Button buyFireButtonIn;
	private Button buyNeedleButtonIn;
	private Button buyCoverButtonIn;
	private Button buyDrainButtonIn;
	private Button buyFreezeButtonIn;
	private GameObject gameoverImage;

	private Tower.TowerType selectTowerType;

	public Gui(){
		moneyText = MyCanvas.Find<TextObj> ("TextMoney");
		costText = MyCanvas.Find<TextObj> ("TextCost");
		costText.Label = "";
		enemyCountText = MyCanvas.Find<TextObj> ("TextEnemyCount");
		waveText = MyCanvas.Find<TextObj> ("TextWave");
		timerText = MyCanvas.Find<TextObj> ("TextTimer");
		towerInfoText = MyCanvas.Find<TextObj> ("TextTowerInfo");
		rangeButton = MyCanvas.Find<ButtonObj> ("ButtonRange");
		firerateButton = MyCanvas.Find<ButtonObj> ("ButtonFirerate");
		powerButton = MyCanvas.Find<ButtonObj> ("ButtonPower");
		scoreText = MyCanvas.Find<TextObj> ("TextScore");
		nameText = MyCanvas.Find<TextObj> ("TextName");
		descriptionText = MyCanvas.Find<TextObj> ("TextDescription");

		buyFireButton = MyCanvas.Find<ButtonObj> ("ButtonBuyFire");
		buyNormalButton = MyCanvas.Find<ButtonObj> ("ButtonBuyNormal");
		buyNeedleButton = MyCanvas.Find<ButtonObj> ("ButtonBuyNeedle");
		buyCoverButton = MyCanvas.Find<ButtonObj> ("ButtonBuyCover");
		buyDrainButton = MyCanvas.Find<ButtonObj> ("ButtonBuyDrain");
		buyFreezeButton = MyCanvas.Find<ButtonObj> ("ButtonBuyFreeze");
//		gameoverImage = MyCanvas.Find<GameObject> ("ImageGameover");

		buyFireButtonIn = buyFireButton.GetComponent<Button> ();
		buyNormalButtonIn = buyNormalButton.GetComponent<Button> ();
		buyNeedleButtonIn = buyNeedleButton.GetComponent<Button> ();
		buyCoverButtonIn = buyCoverButton.GetComponent<Button> ();
		buyDrainButtonIn = buyDrainButton.GetComponent<Button> ();
		buyFreezeButtonIn = buyFreezeButton.GetComponent<Button> ();
	}


	public void Update (GameManager.eSelectMode selectMode,Tower tower) {
		waveText.SetLabelFormat ("{0}", Global.Wave);
		scoreText.SetLabelFormat ("{0:D6}", Global.Score);

		moneyText.SetLabelFormat ("$ {0}", Global.Money);

		timerText.SetLabelFormat ("{0}", (int)(Global.NextWaveTimer));

		costText.Label = "";

		if (selectMode == GameManager.eSelectMode.Upgrade) {
			//表示上，距離は10倍してみる
			float range = 10.0f*TowerParam.Range(tower.LevelRange,tower.GetTowerType);
			float firerate = 10.0f/TowerParam.Firerate(tower.LevelFirerate,tower.GetTowerType);
			int power = (int)TowerParam.Power (tower.LevelPower, tower.GetTowerType);
			towerInfoText.SetLabelFormat ("Range: {0:f1}\nRapid: {1:f1}\nPower: {2}"
				, range, firerate, power);
			nameText.SetLabelFormat ("{0}", tower.GetTowerType);
			descriptionText.SetLabelFormat ("{0}", Tower.getTowerDescription(tower.GetTowerType));
		}

		if (selectMode == GameManager.eSelectMode.Buy) {
			float range = 10.0f*TowerParam.Range(1,selectTowerType);
			float firerate = 10.0f/TowerParam.Firerate(1,selectTowerType);
			float power = (int)TowerParam.Power (1, selectTowerType);
			towerInfoText.SetLabelFormat ("Range: {0:f1}\nRapid: {1:f1}\nPower: {2}",
				range, firerate, power);
			nameText.SetLabelFormat ("{0}", selectTowerType);
			descriptionText.SetLabelFormat ("{0}", Tower.getTowerDescription(selectTowerType));
		}


		int money = Global.Money;

		if (tower) {
			if (tower.LevelRange > 5) {
				rangeButton.Enabled = false;
				rangeButton.FormatLabel ("Range MAX");
			} else {
				rangeButton.Enabled = (money >= tower.CostRange);
				rangeButton.FormatLabel ("Range ${0}", tower.CostRange);
			}
			if (tower.LevelFirerate > 5) {
				firerateButton.Enabled = false;
				firerateButton.FormatLabel ("Rapid MAX");
			} else {
				firerateButton.Enabled = (money >= tower.CostFirerate);
				firerateButton.FormatLabel ("Rapid ${0}", tower.CostFirerate);
			}

			if (tower.LevelPower > 5) {
				powerButton.Enabled = false;
				powerButton.FormatLabel ("Power MAX");
		
			} else {
				powerButton.Enabled = (money >= tower.CostPower);
				powerButton.FormatLabel ("Power ${0}", tower.CostPower);
			}
		}

		if (selectMode == GameManager.eSelectMode.Buy) {
			int cost = Cost.TowerProduction (selectTowerType);
			costText.SetLabelFormat ("cost ${0}", cost);
		}


		buttonEnable ();
		//buyButton.FormatLabel ("Buy (${0})", cost);

		countEnemy ();


	}

	private void buttonEnable(){
		buyNormalButton.Enabled = (Global.Money >= Cost.TowerProduction(Tower.TowerType.Normal));
		buyFireButton.Enabled = (Global.Money >= Cost.TowerProduction(Tower.TowerType.Fire));
		buyNeedleButton.Enabled = (Global.Money >= Cost.TowerProduction(Tower.TowerType.Needle));
		buyFreezeButton.Enabled = (Global.Money >= Cost.TowerProduction(Tower.TowerType.Freeze));
		buyDrainButton.Enabled = (Global.Money >= Cost.TowerProduction(Tower.TowerType.Drain));
		buyCoverButton.Enabled = (Global.Money >= Cost.TowerProduction(Tower.TowerType.Cover));
	}

	private void countEnemy(){
		int enemyCount = Enemy.EnemyCount ();
		enemyCountText.SetLabelFormat ("{0}/100", enemyCount);
	}

	private enum ButtonState{
		selected,
		normal
	}

	public void onClickFireBuy(){
		selectMode(buyFireButtonIn,ButtonState.selected);
		selectTowerType = Tower.TowerType.Fire;
	}

	public void onClickNormalBuy(){
		selectMode(buyNormalButtonIn,ButtonState.selected);
		selectTowerType = Tower.TowerType.Normal;
	}

	public void onClickCoverBuy(){
		selectMode(buyCoverButtonIn,ButtonState.selected);
		selectTowerType = Tower.TowerType.Cover;
	}

	public void onClickNeedleBuy(){
		selectMode (buyNeedleButtonIn,ButtonState.selected);
		selectTowerType = Tower.TowerType.Needle;
	}

	public void onClickDrainBuy(){
		selectMode(buyDrainButtonIn,ButtonState.selected);
		selectTowerType = Tower.TowerType.Drain;
	}

	public void onClickFreezeBuy(){
		selectMode(buyFreezeButtonIn,ButtonState.selected);
		selectTowerType = Tower.TowerType.Freeze;
	}

	private void selectMode(Button button,ButtonState state){
		ColorBlock colors = button.colors;
		switch (state) {
		case ButtonState.normal:
			colors.normalColor = new Color (1.0f, 1.0f, 1.0f);
			break;
		case ButtonState.selected:
			colors.normalColor = new Color (1.0f, 0.5f, 0.5f);
			break;
		}

		button.colors = colors;
	}


	public void resetState(){
		selectMode(buyFireButtonIn,ButtonState.normal);
		selectMode(buyNormalButtonIn,ButtonState.normal);
		selectMode(buyCoverButtonIn,ButtonState.normal);
		selectMode(buyNeedleButtonIn,ButtonState.normal);
		selectMode(buyFreezeButtonIn,ButtonState.normal);
		selectMode(buyDrainButtonIn,ButtonState.normal);
	}

}