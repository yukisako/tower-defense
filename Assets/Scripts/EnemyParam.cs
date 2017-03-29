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


	public static int Hp(){
		return 1 + (Global.Wave / 3);
	}

	public static float Speed(Enemy.EnemyType type){
		return speedParam (type);
	}

	private static int speedParam(Enemy.EnemyType type){
		int speed = 10;
		switch (type) {
		case Enemy.EnemyType.Balloon:
		case Enemy.EnemyType.Tank:
		case Enemy.EnemyType.UFO:
			speed -= 5;
			break;
		case Enemy.EnemyType.Drone:
		case Enemy.EnemyType.AirPlane:
			break;
		case Enemy.EnemyType.Rocket:
		case Enemy.EnemyType.Fighter:
			speed += 5;
			break;
		}
		return speed;
	}


	public static int Money(){
		if (Global.Wave < 5) {
			return 2;
		} 
		return 1;
	}


	public static int GenerationNumber(){
		return 5 + Global.Wave;
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
