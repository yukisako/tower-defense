using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerParam{
	public static float Range(int level,Tower.TowerType towerType){
		float size = Field.GetChipSize ();
		return size+(1.0f * size * level);
	}


	public static float Firerate(int level, Tower.TowerType towerType){
		return 2.0f * (Mathf.Pow (0.9f, (level - 1)));
	}

	public static int Power(int level,Tower.TowerType towerType){
		return 1*level;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
