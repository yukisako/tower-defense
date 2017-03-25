using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : Token {

	//タワーを管理するオブジェクト
	public static TokenMgr<Tower> parent;

	//タワーを生成
	public static Tower Add(float px, float py){
		Tower tower = parent.Add (px, py);
		if (tower == null) {
			return null;
		}

		tower.Init ();
		return tower;
	}

	const float SHOT_SPEED = 15.0f;

	private float range;

	private float firerate;

	private float firerateTimer;

	// Use this for initialization
	void Init () {
		range = Field.GetChopSize () * 10.0f;
		firerate = 2.0f;
		firerateTimer = 0;

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

		Shot.Add (X, Y, targetAngle, SHOT_SPEED);
		firerateTimer = 0;
	}
}
