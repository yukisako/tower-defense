using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : Token {

	public const int CHIP_NONE = 0;	 //なにもない状態
	public const int CHIP_PATH_START = 26;  //開始地点

	//パス (座標リスト)
	List<Vec2D>[] paths = new List<Vec2D>[4];

	public List<Vec2D>[] Paths{
		get { return paths; }
	}



	private Layer2D collisionLayer;

	public Layer2D CollisionLayer{
		get {return collisionLayer;}
	}

	//チップサイズの基準となるスプライトの取得
	static Sprite GetChipSprite(){
		return Util.GetSprite ("Levels/tileset", "tileset_0");
	}

	public static float GetChipSize(){
		var sprite = GetChipSprite ();
		return sprite.bounds.size.x;
	}

	//チップのx座標をワールド座標系へ変換する
	public static float ToWorldX(int i){
		//カメラビューの左下の座標を取得
		Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0,0));

		var sprite = GetChipSprite ();
		var spriteWidth = sprite.bounds.size.x;
		float worldX = min.x + (spriteWidth * i) + spriteWidth / 2;

		return worldX;
	}

	//チップのy座標をワールド座標系へ変換する
	public static float ToWorldY(int i){
		//カメラビューの左下の座標を取得
		Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1,1));

		var sprite = GetChipSprite ();
		var spriteHeight = sprite.bounds.size.y;

		//チップ座標はY軸が上向きだがUnityのワールド座標系はY軸が下向きのため-1をかけている
		float worldY = max.y - 1 * ( + (spriteHeight * i) + spriteHeight / 2);

		return worldY;
	}

	public void Load () {

		//マップの読み込み
		TMXLoader tmx = new TMXLoader ();
		tmx.Load ("Levels/map");

		Vec2D[] positions = new Vec2D[4];
		Layer2D[] layerPaths = new Layer2D[4];


		for (int i = 0; i < Global.Line; i++) {
			layerPaths[i] = tmx.GetLayer($"path{i}");
			positions[i] = layerPaths[i].Search(CHIP_PATH_START);
			paths[i] = new List<Vec2D>();
			paths[i].Add (new Vec2D (positions[i].X, positions[i].Y));
			layerPaths[i].Set (positions[i].X, positions[i].Y, CHIP_NONE);
			CreatePath (layerPaths[i], positions[i].X, positions[i].Y, paths[i]);
		}
			
		collisionLayer = tmx.GetLayer ("collision");

	}

	public static int ToChipX(float x){
		Vector2 min = Camera.main.ViewportToWorldPoint (new Vector2 (0, 0));
		var sprite = GetChipSprite ();
		var spriteWidth = sprite.bounds.size.x;
		return (int)((x - min.x) / spriteWidth);
	}
			
	public static int ToChipY(float y){
		Vector2 max = Camera.main.ViewportToWorldPoint (new Vector2 (1, 1));
		var sprite = GetChipSprite ();
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
