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
		Buy
	}

	eSelectMode selectMode = eSelectMode.None;

	int appearTimer = 0;
	List <Vec2D> _path;
	private Cursor cursor;
	private Layer2D collisionLayer;
	private Gui gui;
	private eState state = eState.Wait;

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

		_path = field.Path;

		cursor = GameObject.Find ("Cursor").GetComponent<Cursor> ();

		collisionLayer = field.CollisionLayer;

		gui = new Gui ();

	}


	void UpdateMain(){
		appearTimer++;
		if (appearTimer % 50 == 0) {
			Enemy.Add (_path);
		}
		if (cursor.Placeable == false) {
			return;
		}

		//マウスクリック判定
		if(Input.GetMouseButtonDown(0) == false){
			return;
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
		
		gui.Update (selectMode);

		cursor.Proc (collisionLayer);

		switch (state) {
		case eState.Wait:
			state = eState.Main;
			break;

		case eState.Main:
			UpdateMain ();
			if (Enemy.EnemyCount () > 100) {
				state = eState.Gameover;
				MyCanvas.SetActive ("TextGameover", true);
				break;
			}
			break;
		case eState.Gameover:
			if (Input.GetMouseButton (0)) {
				Application.LoadLevel ("Main");
			}
			break;
		}
	}
	
	public void OnClickBuy(){
		MyCanvas.SetActive ("ButtonBuy", false);
		ChangeSelectMode (eSelectMode.Buy);
	}

	void ChangeSelectMode(eSelectMode mode){
		switch (mode) {
		case eSelectMode.None:
			MyCanvas.SetActive ("ButtonBuy", true);
			break;

		case eSelectMode.Buy:
			MyCanvas.SetActive ("ButtonBuy", false);
			break;
		}
		selectMode = mode;
	}


}
