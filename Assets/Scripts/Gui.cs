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

	private Tower.TowerType selectTowerType;

	public Gui(){
		moneyText = MyCanvas.Find<TextObj> ("TextMoney");
		costText = MyCanvas.Find<TextObj> ("TextCost");
		enemyCountText = MyCanvas.Find<TextObj> ("TextEnemyCount");
		costText.Label = "";
		waveText = MyCanvas.Find<TextObj> ("TextWave");
		timerText = MyCanvas.Find<TextObj> ("TextTimer");
		towerInfoText = MyCanvas.Find<TextObj> ("TextTowerInfo");
		rangeButton = MyCanvas.Find<ButtonObj> ("ButtonRange");
		firerateButton = MyCanvas.Find<ButtonObj> ("ButtonFirerate");
		powerButton = MyCanvas.Find<ButtonObj> ("ButtonPower");
		scoreText = MyCanvas.Find<TextObj> ("TextScore");

		buyFireButton = MyCanvas.Find<ButtonObj> ("ButtonBuyFire");
		buyNormalButton = MyCanvas.Find<ButtonObj> ("ButtonBuyNormal");
		buyNeedleButton = MyCanvas.Find<ButtonObj> ("ButtonBuyNeedle");
		buyCoverButton = MyCanvas.Find<ButtonObj> ("ButtonBuyCover");
		buyDrainButton = MyCanvas.Find<ButtonObj> ("ButtonBuyDrain");
		buyFreezeButton = MyCanvas.Find<ButtonObj> ("ButtonBuyFreeze");

		buyFireButtonIn = buyFireButton.GetComponent<Button> ();
		buyNormalButtonIn = buyNormalButton.GetComponent<Button> ();
		buyNeedleButtonIn = buyNeedleButton.GetComponent<Button> ();
		buyCoverButtonIn = buyCoverButton.GetComponent<Button> ();
		buyDrainButtonIn = buyDrainButton.GetComponent<Button> ();
		buyFreezeButtonIn = buyFreezeButton.GetComponent<Button> ();
	}


	public void Update (GameManager.eSelectMode selectMode,Tower tower) {
		waveText.SetLabelFormat ("{0}", Global.Wave);
		scoreText.SetLabelFormat ("{0:D5}", Global.Score);

		moneyText.SetLabelFormat ("$ {0}", Global.Money);

		timerText.SetLabelFormat ("{0}", (int)(Global.NextWaveTimer));

		costText.Label = "";

		if (selectMode == GameManager.eSelectMode.Upgrade) {
			towerInfoText.SetLabelFormat ("Tower Info\nRange: Lv{0}\nFirerate: Lv{1}\nPower: Lv{2}"
				, tower.LevelRange, tower.LevelFirerate, tower.LevelPower);
		}

		int money = Global.Money;

		if (tower) {

			rangeButton.Enabled = (money >= tower.CostRange);
			rangeButton.FormatLabel ("Range ${0}", tower.CostRange);

			firerateButton.Enabled = (money >= tower.CostFirerate);
			firerateButton.FormatLabel ("Firerate ${0}", tower.CostFirerate);

			powerButton.Enabled = (money >= tower.CostPower);
			powerButton.FormatLabel ("Power ${0}", tower.CostPower);
		}



		if (selectMode == GameManager.eSelectMode.Buy) {
			int cost = Cost.TowerProduction (selectTowerType);
			costText.SetLabelFormat ("cost:${0}", cost);
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