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
		
	eSelectMode selectMode = eSelectMode.None;
	GameObject selectObject = null;
	Tower selectTower = null;

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

		//所持金を初期化
		Global.Init();

		//敵の管理オブジェクトを生成
		Enemy.parent = new TokenMgr<Enemy>("Enemy", 128);
		//ショットを管理するオブジェクトを生成
		Shot.parent = new TokenMgr<Shot>("Shot", 128);
		//パーティクルの管理オブジェクト
		Particle.parent = new TokenMgr<Particle>("Particle", 256);

		Tower.parent = new TokenMgr<Tower> ("Tower", 64);

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
	}


	void UpdateMain(){
		for (int i = 0; i < Global.Line; i++) {
			enemyGenerators [i].Update ();
		}
		if (cursor.Placeable == false) {
			return;
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


		if (selectObject) {
			selectTower = selectObject.GetComponent<Tower> ();
			ChangeSelectMode (eSelectMode.Upgrade);
		}

		switch (selectMode) {
		case eSelectMode.Buy:
			if (cursor.SelectObject == null) {
				//なにもないのでタワーを設置できる
				int cost = Cost.TowerProduction();
				Global.UseMoney (cost);
				Tower.Add (cursor.X, cursor.Y);

				//次のタワーの生産コストを取得
				int cost2 = Cost.TowerProduction();
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
				enemyGenerators [i].Start (Global.Wave);
			}
			waveStart.Begin (Global.Wave);
			state = eState.Main;
			break;

		case eState.Main:
			UpdateMain ();
			if (Enemy.EnemyCount () > 100) {
				state = eState.Gameover;
				MyCanvas.SetActive ("TextGameover", true);
				break;
			}

			nextWave ();

			break;
		case eState.Gameover:
			if (Input.GetMouseButton (0)) {
				Application.LoadLevel ("Main");
			}
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
			MyCanvas.SetActive ("ButtonBuy", true);
			MyCanvas.SetActive ("TextTowerInfo", false);
			cursorRange.SetVisible (false, 0);
			SetActiveUpgrade (false);
			break;


		case eSelectMode.Buy:
			//MyCanvas.SetActive ("ButtonBuy", false);
			MyCanvas.SetActive ("TextTowerInfo", false);
			cursorRange.SetVisible (false, 0);
			SetActiveUpgrade (false);
			break;

		case eSelectMode.Upgrade:
			MyCanvas.SetActive ("ButtonBuy", true);
			MyCanvas.SetActive ("TextTowerInfo", true);
			cursorRange.SetVisible (true, selectTower.LevelRange);
			cursorRange.SetPosition (cursor);
			SetActiveUpgrade (true);
			break;

		}
		selectMode = mode;
	}

	private void nextWave(){
		nextWaveTimer += Time.deltaTime;
		if (nextWaveTimer > 30) {
			nextWaveTimer = 0;
			Global.NextWave ();
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
		cursorRange.SetVisible (true, selectTower.LevelRange);
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



}


