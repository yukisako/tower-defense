using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Token {

	//マネージャオブジェクト
	public static TokenMgr<Enemy> parent = null;

	//アニメーション用のスプライト
	public Sprite sprite0;
	public Sprite sprite1;

	//アニメーションのタイマ
	int animationTimer = 0;

	private float speed = 0;
	private float tSpeed = 0;	//補完値

	//経路座標のリスト
	private List<Vec2D> _path;

	//現在の経路の番号
	private int pathIndex;

	//チップの座標
	private Vec2D previousPoint;
	private Vec2D nextPoint;

	private int hp;
	private int money;

	public void Init(List<Vec2D> path){
		_path = path;
		pathIndex = 0;
		speed = 10.0f;
		tSpeed = 0;

		MoveNext ();
		previousPoint.Copy (nextPoint);
		previousPoint.x -= Field.GetChopSize ();
		FixedUpdate ();
		hp = 2;
		money = 1;
	}

	//敵を生成する
	public static Enemy Add(List<Vec2D> path){
		Enemy e = parent.Add (0, 0);
		if (e == null) {
			return null;
		}
		e.Init (path);
		return e;
	}

	void Start () {
		
	}

	void FixedUpdate () {
		animationTimer++;
		if (animationTimer % 32 < 16) {
			SetSprite (sprite0);
		} else {
			SetSprite (sprite1);
		}

		tSpeed += speed;

		if (tSpeed >= 100.0f) {
			tSpeed -= 100.0f;
			MoveNext ();
		}


		//線形補間で移動
		X = Mathf.Lerp (previousPoint.x, nextPoint.x, tSpeed / 100.0f);
		Y = Mathf.Lerp (previousPoint.y, nextPoint.y, tSpeed / 100.0f);



	}

	void MoveNext(){
		bool isPathFinish = pathIndex >= _path.Count;

		if (isPathFinish) {
			pathIndex = 2;

		}

		//移動先を移動元にコピー
		previousPoint.Copy (nextPoint);

		//チップ座標の取り出し
		Vec2D tipPoint = _path[pathIndex];
		nextPoint.x = Field.ToWorldX (tipPoint.X);
		nextPoint.y = Field.ToWorldY (tipPoint.Y);
		pathIndex++;

		//角度を更新
		UpdateAngle ();
	}

	void UpdateAngle(){
		float dx = nextPoint.x - previousPoint.x;
		float dy = nextPoint.y - previousPoint.y;

		Angle = Mathf.Atan2 (dy, dx) * Mathf.Rad2Deg;
	}

	void OnTriggerEnter2D(Collider2D other){
		string name = LayerMask.LayerToName (other.gameObject.layer);
		if (name == "Shot") {
			Shot shot = other.gameObject.GetComponent<Shot> ();
			shot.Vanish ();
			Damage (1);

			if (Exists == false) {
				Global.AddMoney (money);
			}
		}
	}


	void Damage(int val){
		hp -= val;
		if (hp <= 0) {
			Vanish ();
		}
	}

	public override void Vanish(){
		{
			Particle particle = Particle.Add (Particle.eType.Ring, 30, X, Y, 0, 0);
			if (particle) {
				particle.SetColor (0.7f, 1, 0.7f);
			}
		}
		float dir = Random.Range (35, 55);

		for (int i = 0; i < 8; i++) {
			int timer = Random.Range (20, 40);

			float speed = Random.Range (0.5f, 2.5f);

			Particle particle = Particle.Add (Particle.eType.Ball,timer, X, Y, dir, speed);

			dir += Random.Range (35, 55);

			if(particle){
				particle.SetColor (0.0f, 1.0f, 0.0f);
				particle.Scale = 0.8f;
			}
			base.Vanish();
		}
	}
	//ボール
	public static int EnemyCount(){
		return Enemy.parent.Count ();
	}
}
