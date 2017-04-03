using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerParam{

	const float baseRange = 3.0f;
	const float baseFirerate = 2.0f;
	const float basePower = 50.0f; 


	public static float Range(int level,Tower.TowerType towerType){
		int k = 1;
		if (towerType == Tower.TowerType.Cover) {
			k = 2;
		}
		float size = Field.GetChipSize ();
		return (3.0f * size * k * Mathf.Pow(1.2f,(level-1)));
	}


	public static float Firerate(int level, Tower.TowerType towerType){
		int k = 1;
		if (towerType == Tower.TowerType.Needle) {
			k = 5;
		}
		return 2.0f * (Mathf.Pow (0.8f, (level - 1)))/k;
	}

	public static int Power(int level,Tower.TowerType towerType){
		int k = 1;
		if (towerType == Tower.TowerType.Fire) {
			k = 5;
		}
		return (int)(basePower*k*(Mathf.Pow (1.5f, (level - 1))));
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
