using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gui{
	private TextObj moneyText;
	private TextObj costText;
	private ButtonObj buyButton;
	private TextObj enemyCountText;

	public Gui(){
		moneyText = MyCanvas.Find<TextObj> ("TextMoney");
		buyButton = MyCanvas.Find<ButtonObj> ("ButtonBuy");
		costText = MyCanvas.Find<TextObj> ("TextCost");
		enemyCountText = MyCanvas.Find<TextObj> ("TextEnemyCount");
		costText.Label = "";
	}


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	public void Update (GameManager.eSelectMode selectMode) {
		moneyText.SetLabelFormat ("¥ {0}", Global.Money);
		int cost = Cost.TowerProduction ();

		costText.Label = "";

		if (selectMode == GameManager.eSelectMode.Buy) {
			costText.SetLabelFormat ("(cost ¥{0})", cost);

			buyButton.enabled = (Global.Money >= cost);

			buyButton.FormatLabel ("Buy (¥{0})", cost);
		}
		countEnemy ();

	}

	private void countEnemy(){
		int enemyCount = Enemy.EnemyCount ();
		enemyCountText.SetLabelFormat ("{0}/100", enemyCount);
	}
}