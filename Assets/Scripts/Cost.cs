using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cost{
	
	//タワー建設費用
	public static int TowerProduction(Tower.TowerType type){
		int cost = 300;
		switch (type) {
		case Tower.TowerType.Normal:
			cost = 100;
			break;
		case Tower.TowerType.Fire:
			cost = 1200; 
			break;
		case Tower.TowerType.Drain:
			cost = 2000;
			break;
		case Tower.TowerType.Freeze:
			cost = 500;
			break;
		case Tower.TowerType.Needle:
			cost = 1500;
			break;
		case Tower.TowerType.Cover:
			cost = 300;
			break;
		}

		return cost;

//		int num = Tower.parent.Count();
//
//		int basic = 8;
//		float ratio = Mathf.Pow (1.3f, num);

//		return (int)(basic * ratio);
	}

	public static int TowerUpdateBasic(Tower.TowerType type){
		int cost = TowerProduction(type);
		return cost;
	}


	//アップグレードコスト
	public static int TowerUpGrade(Tower.eUpgrade upgradeType, int level, Tower.TowerType towerType){
		float cost = 0;
		switch (upgradeType) {
		case Tower.eUpgrade.Range:
			cost = 0.5f * TowerUpdateBasic(towerType) * Mathf.Pow (1.8f, (level - 1));
			break;
		case Tower.eUpgrade.Firerate:
			cost = 0.8f * TowerUpdateBasic(towerType) * Mathf.Pow (1.8f, (level - 1));
			break;
		case Tower.eUpgrade.Power:
			cost = 0.9f * TowerUpdateBasic(towerType) * Mathf.Pow (1.8f, (level - 1));
			break;
		}
		return (int)cost;
	}
}
