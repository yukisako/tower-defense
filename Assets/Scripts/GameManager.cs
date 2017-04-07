using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public enum eState{
		Wait,
		Gameover,
		Main
	}

	public enum eSelectMode{
		None,
		Buy,
		Upgrade
	}
	private BGM BGMComponent;
	private GameObject bgm;
	eSelectMode selectMode = eSelectMode.None;
	private Tower.TowerType selectTowerType; 
	private Tower.TowerType upgradeTowerType; 
	GameObject selectObject = null;
	Tower selectTower = null;
	Tower.TowerType selectBuyTowerType;


	int appearTimer = 0;
	List <Vec2D>[] paths = new List<Vec2D>[4];
	EnemyGenerator[] enemyGenerators = new EnemyGenerator[4];

	private Cursor cursor;
	private Layer2D collisionLayer;
	private Gui gui;
	private eState state = eState.Wait;
	private float nextWaveTimer;
	private WaveStart waveStart;
	private CursorRange cursorRange;


	// Use this for initialization
	void Start () {
		Time.timeScale = 1.0f;
		//所持金を初期化
		Global.Init();

		//敵の管理オブジェクトを生成
		Enemy.parent = new TokenMgr<Enemy>("Enemy", 128);
		//ショットを管理するオブジェクトを生成
		Shot.parent = new TokenMgr<Shot>("Shot",2048);
		//パーティクルの管理オブジェクト
		Particle.parent = new TokenMgr<Particle>("Particle", 4096);

		Tower.parent = new TokenMgr<Tower> ("Tower", 256);

		//マップ管理を生成
		GameObject prefab = null;
		prefab = Util.GetPrefab (prefab, "Field");

		Field field = Field.CreateInstance2<Field> (prefab, 0, 0);

		field.Load ();

		for (int i = 0; i < Global.Line; i++) {
			paths [i] = field.Paths [i];
		}

		cursor = GameObject.Find ("Cursor").GetComponent<Cursor> ();

		collisionLayer = field.CollisionLayer;

		gui = new Gui ();

		for (int i = 0; i < Global.Line; i++) {
			enemyGenerators [i] = new EnemyGenerator (paths [i],i);
		}

		waveStart = MyCanvas.Find<WaveStart> ("TextWaveStart");

		cursorRange = GameObject.Find ("CursorRange").GetComponent<CursorRange> ();

		ChangeSelectMode (eSelectMode.None);

		MyCanvas.SetActive ("ImageGameover", false);

	}


	void UpdateMain(){
		for (int i = 0; i < Global.Line; i++) {
			enemyGenerators [i].Update ();
		}


		int mask = 1 << LayerMask.NameToLayer ("Tower");
		Collider2D col = Physics2D.OverlapPoint (cursor.GetPosition (), mask);

		selectObject = null;

		if (col != null) {
			selectObject = col.gameObject;
		}

		//マウスクリック判定
		if(Input.GetMouseButtonDown(0) == false){
			return;
		}

		if (cursor.Placeable == false) {
			//配置できない頃をクリックしたので通常モードに戻る
			//ChangeSelectMode (eSelectMode.None);
//			gui.resetState ();
			return;
		}
			
		if (selectObject) {
			selectTower = selectObject.GetComponent<Tower> ();
			selectTowerType = selectTower.GetTowerType;
			ChangeSelectMode (eSelectMode.Upgrade);
		}

		switch (selectMode) {
		case eSelectMode.Buy:
			if (cursor.SelectObject == null) {
				//なにもないのでタワーを設置できる
				int cost = Cost.TowerProduction(selectTowerType);
				Global.UseMoney (cost);
				Tower.Add (cursor.X, cursor.Y, selectTowerType);
				//ChangeSelectMode (eSelectMode.None);

				//次のタワーの生産コストを取得
				int cost2 = Cost.TowerProduction(Tower.TowerType.Normal);
				if (Global.Money < cost2) {
					
					ChangeSelectMode (eSelectMode.None);
				}
			}
			break;
		}


	}

	// Update is called once per frame
	void Update () {
		gui.Update (selectMode,selectTower);

		cursor.Proc (collisionLayer);

		switch (state) {
		case eState.Wait:
			for (int i = 0; i < Global.Line; i++) {
				enemyGenerators [i].Start (Global.Wave,Enemy.WaveToType());
			}
			waveStart.Begin (Global.Wave);
			state = eState.Main;
			break;

		case eState.Main:
			UpdateMain ();
			if (GameOver.IsGameover()) {
				state = eState.Gameover;
				bgm = GameObject.Find ("BGM");
				BGMComponent = bgm.GetComponent<BGM> ();
				BGMComponent.Play();
				GameOver.GameoverAction ();
				break;
			}

			nextWave ();

			break;
		case eState.Gameover:
			break;
		}
	}
	
	public void OnClickBuy(){
		//MyCanvas.SetActive ("ButtonBuy", false);
		ChangeSelectMode (eSelectMode.Buy);
	}





	void ChangeSelectMode(eSelectMode mode){
		switch (mode) {
		case eSelectMode.None:
			//MyCanvas.SetActive ("ButtonBuy", true);
			MyCanvas.SetActive ("TextTowerInfo", false);
			cursorRange.SetVisible (false, 0, selectTowerType);
			SetActiveUpgrade (false);
			MyCanvas.SetActive ("TextName", false);
			MyCanvas.SetActive ("TextDescription", false);
//			MyCanvas.SetActive ("ButtonPause", true);
			break;

		case eSelectMode.Buy:
			MyCanvas.SetActive ("TextTowerInfo", true);
			cursorRange.SetVisible (false, 0,selectTowerType);
			SetActiveUpgrade (false);
			MyCanvas.SetActive ("TextName", true);
			MyCanvas.SetActive ("TextDescription", true);
//			MyCanvas.SetActive ("ButtonPause", false);
			break;

		case eSelectMode.Upgrade:
			//MyCanvas.SetActive ("ButtonBuy", true);
			MyCanvas.SetActive ("TextTowerInfo", true);
			cursorRange.SetVisible (true, selectTower.LevelRange, selectTowerType);
			cursorRange.SetPosition (cursor);
			SetActiveUpgrade (true);
			MyCanvas.SetActive ("TextName", true);
			MyCanvas.SetActive ("TextDescription", false);
//			MyCanvas.SetActive ("ButtonPause", false);
			gui.resetState ();
			break;

		}
		selectMode = mode;
	}

	private void nextWave(){
		if (Global.NextWave ()) {
			state = eState.Wait;
		}
	}

	private void SetActiveUpgrade(bool set){
		MyCanvas.SetActive("ButtonRange", set);
		MyCanvas.SetActive("ButtonFirerate", set);
		MyCanvas.SetActive("ButtonPower", set);
	}


	private void ExecUpgrade(Tower.eUpgrade type){
		int cost = selectTower.GetCost (type);
		Global.UseMoney (cost);
		selectTower.Upgrade (type);
		cursorRange.SetVisible (true, selectTower.LevelRange,selectTowerType);
	}

	public void onClickRange(){
		ExecUpgrade (Tower.eUpgrade.Range);
	}
	public void onClickFirerate(){
		ExecUpgrade (Tower.eUpgrade.Firerate);
	}

	public void onClickPower(){
		ExecUpgrade (Tower.eUpgrade.Power);
	}
		

	public void SelectNormalTower(){
		selectTowerType = Tower.TowerType.Normal;
		gui.resetState ();
		gui.onClickNormalBuy ();
	}

	public void SelectFireTower(){
		selectTowerType = Tower.TowerType.Fire;
		gui.resetState ();
		gui.onClickFireBuy ();
	}

	public void SelectFreezeTower(){
		selectTowerType = Tower.TowerType.Freeze;
		gui.resetState ();
		gui.onClickFreezeBuy ();
	}

	public void SelectDrainTower(){
		selectTowerType = Tower.TowerType.Drain;
		gui.resetState ();
		gui.onClickDrainBuy ();
	}

	public void SelectCoverTower(){
		selectTowerType = Tower.TowerType.Cover;
		gui.resetState ();
		gui.onClickCoverBuy ();
	}

	public void SelectNeedleTower(){
		selectTowerType = Tower.TowerType.Needle;
		gui.resetState ();
		gui.onClickNeedleBuy ();
	}
}


