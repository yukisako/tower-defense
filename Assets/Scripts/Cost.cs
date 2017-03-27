using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cost{

	//タワー建設費用
	public static int TowerProduction(){
		int num = Tower.parent.Count();

		int basic = 8;
		float ratio = Mathf.Pow (1.3f, num);

		return (int)(basic * ratio);
	}

	//アップグレードコスト
	public static int TowerUpGrade(Tower.eUpgrade type, int level){
		float cost = 0;
		switch (type) {
		case Tower.eUpgrade.Range:
			cost = 10 * Mathf.Pow (1.5f, (level - 1));
			break;
		case Tower.eUpgrade.Firerate:
			cost = 15 * Mathf.Pow (1.5f, (level - 1));
			break;
		case Tower.eUpgrade.Power:
			cost = 20 * Mathf.Pow (1.5f, (level - 1));
			break;
		}
		return (int)cost;
	}
}
