using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	int appearTimer = 0;
	List <Vec2D> _path;
	private Cursor cursor;
	private Layer2D collisionLayer;

	// Use this for initialization
	void Start () {
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

	}
	
	// Update is called once per frame
	void Update () {
		appearTimer++;
		if (appearTimer % 10 == 0) {
			Enemy.Add (_path);
		}

		cursor.Proc (collisionLayer);

		if (cursor.Placeable == false) {
			return;
		}

		//マウスクリック判定
		if(Input.GetMouseButtonDown(0) == false){
			return;
		}



		if (cursor.SelectObject == null) {
			//なにもないのでタワーを設置できる
			Tower.Add (cursor.X, cursor.Y);
		}
	}
}
