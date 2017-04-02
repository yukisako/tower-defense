using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : Token {

	const float SHOT_SPEED = 10.0f;


	//タワーを管理するオブジェクト
	public static TokenMgr<Tower> parent;

	private TowerType towerType;
	public TowerType GetTowerType{
		get{
			return towerType;
		}
	}

	public int CostRange {
		get {
			return Cost.TowerUpGrade (eUpgrade.Range, levelRange,towerType);
		}
	}


	public int CostFirerate{
		get {
			return Cost.TowerUpGrade (eUpgrade.Firerate, levelFirerate,towerType);
		}
	}

	public int CostPower{
		get {
			return Cost.TowerUpGrade (eUpgrade.Power, levelPower,towerType);
		}
	}

	public int GetCost(eUpgrade type){
		switch(type){
		case eUpgrade.Range: return CostRange;
		case eUpgrade.Firerate: return CostFirerate;
		case eUpgrade.Power: return CostPower;
		}
		return 0;
	}

	//タワーを生成
	public static Tower Add(float px, float py,TowerType type){
		Tower tower = parent.Add (px, py);
		tower.towerType = type;
		if (tower == null) {
			return null;
		}
		tower.Init ();
		return tower;
	}

	public enum eUpgrade{
		Range,
		Firerate,
		Power
	}

	private void changeColor(){
		switch(towerType){
		case TowerType.Normal:
			SetColor(Color.white);
			break;
		case TowerType.Fire:
			SetColor(new Color(1f, 0.4f, 0.4f, 1f));	//赤
			break;
		case TowerType.Needle:
			SetColor(new Color(0.2f, 0.2f, 0.2f, 1f));	//黒
			break;
		case TowerType.Cover:
			SetColor(new Color(1.0f, 1.0f, 0.4f, 1f));	//黄色
			break;
		case TowerType.Freeze:
			SetColor(new Color(0.4f, 1.0f, 1.0f, 1f));	//水色
			break;
		case TowerType.Drain:
			SetColor(new Color(0.9f, 0.4f, 1.0f, 1f));	//紫
			break;
		default:
			Debug.Log ("bug");	
			break;
		}
	}


	private float range;

	private float firerate;

	private float firerateTimer;

	private int power;

	private int levelRange;
	public int LevelRange {
		get { return levelRange; }
	}

	private int levelFirerate;
	public int LevelFirerate {
		get { return levelFirerate; }
	}

	private int levelPower;
	public int LevelPower {
		get { return levelPower; }
	}




	// Use this for initialization
	void Init () {
		levelRange = 1;
		levelFirerate = 1;
		levelPower = 1;
		UpdateParam ();
		changeColor ();
	}
	
	// Update is called once per frame
	void Update () {
		firerateTimer += Time.deltaTime;
		//一番近い敵を求める
		Enemy enemy = Enemy.parent.Nearest (this);
		if (enemy == null) {
			return;
		}

		float dist = Util.DistanceBetween (this, enemy);
		if (dist > range) {
			//射程範囲外
			return;
		}

		float targetAngle = Util.AngleBetween (this, enemy);

		float angleDiff = Mathf.DeltaAngle (Angle, targetAngle);

		Angle += angleDiff * 0.2f;


		float angleDiff2 = Mathf.DeltaAngle (targetAngle, targetAngle);



		if (Mathf.Abs (angleDiff2) > 16) {
			//角度が大きい場合は打てない
			return;
		}

		if (firerateTimer < firerate) {
			return;
		}
		//ショットを打つ

		Shot.Add (X, Y, targetAngle, SHOT_SPEED, power,towerType);
		firerateTimer = 0;
	}

	void UpdateParam(){
		range = TowerParam.Range (LevelRange,towerType);
		firerate = TowerParam.Firerate (levelFirerate,towerType);
		power = TowerParam.Power (levelPower,towerType);
	}

	public void Upgrade(eUpgrade type){
		switch (type) {
		case eUpgrade.Range:
			levelRange++;
			break;
		case eUpgrade.Firerate:
			levelFirerate++;
			break;
		case eUpgrade.Power:
			levelPower++;
			break;
		}
		UpdateParam ();

		Particle particle = Particle.Add (Particle.eType.Eclipse, 20, X, Y, 0, 0);
		if (particle) {
			particle.SetColor (0.2f, 0.2f, 1);
		}
	}

	public enum TowerType{
		Normal,	//普通
		Fire, //パワーアップ
		Freeze, //敵のスピードダウン
		Needle, //連射
		Cover,	//広範囲
		Drain, //吸い取る
	}
}
