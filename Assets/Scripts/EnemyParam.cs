using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyParam{

	public static int Hp(){
		return 1 + (Global.Wave / 3);
	}

	public static float Speed(){
		return 3 + (0.1f * Global.Wave);
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
		return 1.5f;
	}


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
