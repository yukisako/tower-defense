using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : Token {

	//タワーを管理するオブジェクト
	public static TokenMgr<Tower> parent;

	public int CostRange {
		get {
			return Cost.TowerUpGrade (eUpgrade.Range, levelRange);
		}
	}

	public int CostFirerate{
		get {
			return Cost.TowerUpGrade (eUpgrade.Firerate, levelFirerate);
		}
	}

	public int CostPower{
		get {
			return Cost.TowerUpGrade (eUpgrade.Power, levelPower);
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
	public static Tower Add(float px, float py){
		Tower tower = parent.Add (px, py);
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

	const float SHOT_SPEED = 15.0f;

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

		Shot.Add (X, Y, targetAngle, SHOT_SPEED, power);
		firerateTimer = 0;
	}

	void UpdateParam(){
		range = TowerParam.Range (LevelRange);
		firerate = TowerParam.Firerate (levelFirerate);
		power = TowerParam.Power (levelPower);
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

}
