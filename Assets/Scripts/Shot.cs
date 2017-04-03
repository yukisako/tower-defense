using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : Token {

	public static TokenMgr<Shot> parent;

	public static Shot Add(float px,float py, float direction, float speed, int power,Tower.TowerType type){
		Shot shot = parent.Add(px,py,direction,speed);
		if (shot == null) {
			return null;
		}
		shot.Init (power);
		if (type == Tower.TowerType.Freeze) {
			shot.tag = "slow";
		}
		if (type == Tower.TowerType.Drain) {
			shot.tag = "drain";
		}
		return shot;
	}

	int _power;

	public int Power {
		get { return _power; }
	}

	public void Init(int power){
		_power = power;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (IsOutside ()) {
			//画面外に出たら消滅
			Vanish();
		}
	}

	public override void Vanish(){
		for (int i = 0; i < 4; i++) {
			int timer = Random.Range (20, 40);

			float direction = Direction - 180 + Random.Range (-60, 60);

			float speed = Random.Range (1.0f, 1.5f);

			Particle particle = Particle.Add (Particle.eType.Ball, timer, X, Y, direction, speed);

			if (particle) {
				particle.Scale = 0.6f;
				particle.SetColor (1, 0.0f, 0.0f);
			}
		}
		base.Vanish ();
	}

}
