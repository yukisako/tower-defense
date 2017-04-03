using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyParam{

	//TODO: HPベーシックを決めておいて，それぞれの敵はそれをいい感じに

	/* 特徴
	【Balloon】遅い，脆い，数も少ない
	【Drone】普通，脆い，普通
	【AirPlane】普通，普通，数も少ない
	【Tank】遅い，硬い，普通
	【Rocket】速い，普通，数も少ない
	【UFO】遅い，脆い，数が多い，数も多い
	【Fighter】速い，硬い，数も少ない
	*/

	/*
	HPのベースは100
	脆いのは半分，強いのは2倍
	Waveが進むと1.2倍ずつ



	*/

	const int baseHP = 30;

	public static int Hp(Enemy.EnemyType type){
		float k = 1.0f;
		switch(type){
			case Enemy.EnemyType.Balloon:
			case Enemy.EnemyType.Drone:
			case Enemy.EnemyType.UFO:
			//脆い
			k = 0.5f;
			break;
			case Enemy.EnemyType.Rocket:
			case Enemy.EnemyType.AirPlane:
			k = 1.0f;
			break;
			case Enemy.EnemyType.Tank:
			case Enemy.EnemyType.Fighter:
			k = 2.0f;
			break;
		}

		return (int)(baseHP * k * Mathf.Pow (1.1f, (Global.Wave)) * Mathf.Pow (1.5f, (int)(Global.Wave/7)));
	}

	public static float Speed(Enemy.EnemyType type){
		return speedParam (type);
	}

	private static int speedParam(Enemy.EnemyType type){
		int speed = 5;
		switch (type) {
		case Enemy.EnemyType.Balloon:
		case Enemy.EnemyType.Tank:
		case Enemy.EnemyType.UFO:
			speed -= 2;
			break;
		case Enemy.EnemyType.Drone:
		case Enemy.EnemyType.AirPlane:
			break;
		case Enemy.EnemyType.Rocket:
		case Enemy.EnemyType.Fighter:
			speed += 3;
			break;
		}
		return speed;
	}



	public static int Money(Enemy.EnemyType type){
		switch (type) {
		case Enemy.EnemyType.Tank:
			return 10;
		case Enemy.EnemyType.Drone:
			return 6;
		case Enemy.EnemyType.AirPlane:
			return 8;
		case Enemy.EnemyType.Balloon:
			return 6;
		case Enemy.EnemyType.Rocket:
			return 10;
		case Enemy.EnemyType.Fighter:
			return 16;
		case Enemy.EnemyType.UFO:
			return 4;
		}
		return 30;
	}

	/* 特徴
	【Balloon】遅い，脆い，数も少ない
	【Drone】普通，脆い，普通
	【AirPlane】普通，普通，数も少ない
	【Tank】遅い，硬い，普通
	【Rocket】速い，普通，数も少ない
	【UFO】遅い，脆い，数が多い，数も多い
	【Fighter】速い，硬い，数も少ない
	*/

	public static int GenerationNumber(Enemy.EnemyType type){
		switch (type) {
		case Enemy.EnemyType.Balloon:
		case Enemy.EnemyType.AirPlane:
		case Enemy.EnemyType.Rocket:
		case Enemy.EnemyType.Fighter:
		//脆い
			return 3;
			break;
		case Enemy.EnemyType.Drone:
		case Enemy.EnemyType.Tank:
			return 5;
			break;
		case Enemy.EnemyType.UFO:
			return 10;
			break;
		}
		return 5;
	}



	public static float GenerationInterval(){
		int speed = speedParam (Global.currentType);
		return 3.0f/speed;
	}


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
