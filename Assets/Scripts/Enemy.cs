using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : Token {

	public enum EnemyType{
		Tank,
		Drone,
		Balloon,
		AirPlane,
		Rocket,
		Fighter,
		UFO
	}
		
	//マネージャオブジェクト
	public static TokenMgr<Enemy> parent = null;
	public Sprite TankSprite1;
	public Sprite TankSprite2;
	public Sprite DroneSprite1;
	public Sprite DroneSprite2;
	public Sprite BalloonSprite;
	public Sprite AirPlaneSprite;
	public Sprite RocketSprite;
	public Sprite FighterSprite;
	public Sprite UFOSprite;

	//アニメーションのタイマ
	int animationTimer = 0;

	//経路座標のリスト
	private List<Vec2D> _path;

	//現在の経路の番号
	private int pathIndex;

	//チップの座標
	private Vec2D previousPoint;
	private Vec2D nextPoint;

	private int hp;
	private int maxHp;
	private int money;
	private float speed;
	private int enemyLine;
	private float tSpeed = 0;	//補完値
	EnemyType enemyType;

	public void Init(List<Vec2D> path, EnemyType type, int line){
		enemyType = type;
		_path = path;
		pathIndex = 0;
		enemyLine = line;
		speed = EnemyParam.Speed (enemyType);
		tSpeed = 0;
		MoveNext ();
		previousPoint.Copy (nextPoint);
		if (line < 2) {
			previousPoint.x -= Field.GetChipSize ();
		} else {
			previousPoint.y += Field.GetChipSize ();
		}
		FixedUpdate ();
		maxHp = EnemyParam.Hp (type);
		hp = EnemyParam.Hp(type);
		money = EnemyParam.Money(type);
		Alpha = 1.0f;
	}

	//敵を生成する
	public static Enemy Add(List<Vec2D> path, EnemyType type, int line){
		Global.currentType = type;
		Enemy e = parent.Add (0, 0);
		if (e == null) {
			return null;
		}
		e.Init (path, type, line);
		return e;
	}

	void FixedUpdate () {
		animation ();
		tSpeed += speed;

		if (tSpeed >= 100.0f) {
			tSpeed -= 100.0f;
			MoveNext ();
		}

		//Alpha = hp/maxHp;
		//線形補間で移動
		X = Mathf.Lerp (previousPoint.x, nextPoint.x, tSpeed / 100.0f);
		Y = Mathf.Lerp (previousPoint.y, nextPoint.y, tSpeed / 100.0f);



	}

	void MoveNext(){
		bool isPathFinish = pathIndex >= _path.Count;

		if (isPathFinish) {
			switch (enemyLine) {
			case 0:
				pathIndex = 2;
				break;
			case 1:
				pathIndex = 3;
				break;
			case 2:
				pathIndex = 6;
				break;
			case 3:
				pathIndex = 7;
				break;
			}
		}

		//移動先を移動元にコピー
		previousPoint.Copy (nextPoint);

		//チップ座標の取り出し
		Vec2D tipPoint = _path[pathIndex];
		nextPoint.x = Field.ToWorldX (tipPoint.X);
		nextPoint.y = Field.ToWorldY (tipPoint.Y);
		pathIndex++;

		//角度を更新
		if (isRotate (enemyType)) {
			UpdateAngle ();
		} else {
			Angle = Mathf.Atan2 (0, 1) * Mathf.Rad2Deg;
		}
	}

	void UpdateAngle(){
		float dx = nextPoint.x - previousPoint.x;
		float dy = nextPoint.y - previousPoint.y;

		Angle = Mathf.Atan2 (dy, dx) * Mathf.Rad2Deg;
	}

	void OnTriggerEnter2D(Collider2D other){

		string name = LayerMask.LayerToName (other.gameObject.layer);
		Shot shot = other.gameObject.GetComponent<Shot> ();
		shot.Vanish ();

		if (name == "Shot") {
			if (other.tag == "slow") {
				speed = (int)(speed * 0.7);
			}
			if (other.tag == "drain") {
				Damage ((int)(hp * 0.3));
			} else {
				Damage (shot.Power);
			}

			if (Exists == false) {
				Global.AddMoney (money);
			}
		}
	}


	void Damage(int val){
		hp -= val;
		Alpha = (float)hp / (float)maxHp*0.9f+0.1f;
		if (hp <= 0) {
			Global.Score += Global.Wave*10;
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

	//タイプが渡されたらスプライトを返す
	private Sprite typeToSprite(EnemyType type, bool first){
		switch (type) {
		case EnemyType.Tank:
			if (first) {
				return TankSprite1;
			} else {
				return TankSprite2;
			}
		case EnemyType.Drone:
			if (first) {
				return DroneSprite1;
			} else {
				return DroneSprite2;
			}

		case EnemyType.AirPlane:
			return AirPlaneSprite;
		case EnemyType.Balloon:
			return BalloonSprite;
		case EnemyType.Rocket:
			return RocketSprite;
		case EnemyType.Fighter:
			return FighterSprite;
		case EnemyType.UFO:
			return UFOSprite;
		}
		return TankSprite1;
	}

	//タイプが渡されたら回転するかどうかを返す
	private bool isRotate(EnemyType type){
		if((type == EnemyType.Drone) || (type == EnemyType.UFO) || (type == EnemyType.Balloon)){
			return false;
		} else {
			return true;
		}
	}
			

	//タイプが渡されたら複数枚有るかどうかを返す
	private bool isSomeSprite(EnemyType type){
		if((type == EnemyType.Tank)|| (type == EnemyType.Drone)){
			return true;
		} else {
			return false;
		}
	}

	private void animation(){
		animationTimer++;
		if (isSomeSprite(enemyType)) {
			if (animationTimer % 32 < 16) {
				SetSprite (typeToSprite (enemyType, true));
			} else {
				SetSprite (typeToSprite (enemyType, false));
			}
		} else {
			SetSprite(typeToSprite(enemyType, true));
		}
	}

	public static EnemyType WaveToType(){
		switch(Global.Wave % 7){
		case 1:
			return EnemyType.Balloon;
		case 2:
			return EnemyType.Drone;
		case 3:
			return EnemyType.AirPlane;
		case 4:
			return EnemyType.Tank;
		case 5:
			return EnemyType.Rocket;
		case 6:
			return EnemyType.UFO;
		case 0:
			return EnemyType.Fighter;
		}
		return EnemyType.Tank;
	}


	//ボール
	public static int EnemyCount(){
		return Enemy.parent.Count ();
	}


}
