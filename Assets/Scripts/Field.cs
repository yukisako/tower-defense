using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : Token {

	public const int CHIP_NONE = 0;	 //なにもない状態
	public const int CHIP_PATH_START = 26;  //開始地点

	//パス (座標リスト)
	List<Vec2D> _path;
	public List<Vec2D> Path{
		get { return _path; }
	}

	private Layer2D collisionLayer;

	public Layer2D CollisionLayer{
		get {return collisionLayer;}
	}

	//チップサイズの基準となるスプライトの取得
	static Sprite GetChopSprite(){
		return Util.GetSprite ("Levels/tileset", "tileset_0");
	}

	public static float GetChopSize(){
		var sprite = GetChopSprite ();
		return sprite.bounds.size.x;
	}

	//チップのx座標をワールド座標系へ変換する
	public static float ToWorldX(int i){
		//カメラビューの左下の座標を取得
		Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0,0));

		var sprite = GetChopSprite ();
		var spriteWidth = sprite.bounds.size.x;
		float worldX = min.x + (spriteWidth * i) + spriteWidth / 2;

		return worldX;
	}

	//チップのy座標をワールド座標系へ変換する
	public static float ToWorldY(int i){
		//カメラビューの左下の座標を取得
		Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1,1));

		var sprite = GetChopSprite ();
		var spriteHeight = sprite.bounds.size.y;

		//チップ座標はY軸が上向きだがUnityのワールド座標系はY軸が下向きのため-1をかけている
		float worldY = max.y - 1 * ( + (spriteHeight * i) + spriteHeight / 2);

		return worldY;
	}


	public void Load () {

		//マップの読み込み
		TMXLoader tmx = new TMXLoader ();
		tmx.Load ("Levels/map");

		//経路レイヤーの取得
		Layer2D lPath = tmx.GetLayer("path");

		//開始地点の探索
		Vec2D pos = lPath.Search(CHIP_PATH_START);

		//座標リストを作成
		_path = new List<Vec2D>();

		_path.Add (new Vec2D (pos.X, pos.Y));

		lPath.Set (pos.X, pos.Y, CHIP_NONE);

		CreatePath (lPath, pos.X, pos.Y, _path);

//		foreach (Vec2D p in _path) {
//			p.Dump ();
//		}

		collisionLayer = tmx.GetLayer ("collision");

		//敵の取得
//		Enemy enemy = GameObject.Find("Enemy").GetComponent<Enemy>();
//
//		enemy.Init (_path);
	}

	public static int ToChipX(float x){
		Vector2 min = Camera.main.ViewportToWorldPoint (new Vector2 (0, 0));
		var sprite = GetChopSprite ();
		var spriteWidth = sprite.bounds.size.x;
		return (int)((x - min.x) / spriteWidth);
	}
			
	public static int ToChipY(float y){
		Vector2 max = Camera.main.ViewportToWorldPoint (new Vector2 (1, 1));
		var sprite = GetChopSprite ();
		var spriteHeight = sprite.bounds.size.y;
		return (int)((y - max.y) / -spriteHeight);
	}



	void CreatePath(Layer2D layer, int x, int y, List<Vec2D> path){
		int[] xTable = { -1, 0, 1, 0 };
		int[] yTable = { 0, -1, 0, 1 };
		for (var i = 0; i < xTable.Length; i++) {
			int x2 = x + xTable [i];
			int y2 = y + yTable [i];
			int val = layer.Get (x2, y2);
			if (val > CHIP_NONE) {
				//経路を発見
				//経路を塞ぐ
				layer.Set (x2, y2, CHIP_NONE);
				//座標を追加
				path.Add (new Vec2D (x2, y2));
				//再帰処理
				CreatePath (layer, x2, y2, path);

			}
		}
	}

	// Update is called once per frame
	void Update () {
		
	}
}
