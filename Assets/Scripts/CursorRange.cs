using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorRange : Token {

	public void SetVisible(bool visible, int levelRange,Tower.TowerType towerType){
		//元はtowerTypeは渡してなく，一個だった
		float range = TowerParam.Range (levelRange,towerType);
		Scale = range / (1.5f * Field.GetChipSize ()) * 5f;
		Visible = visible;
	}


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
