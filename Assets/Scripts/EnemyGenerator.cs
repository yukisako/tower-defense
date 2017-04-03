using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator{

	private List<Vec2D> _pathList;

	private float interval;
	private float intervalTimer;
	private int number;
	private int enemyLine;

	public int Number{
		get {return number;}
	}

	public EnemyGenerator(List<Vec2D> pathlist, int line){
		_pathList = pathlist;
		enemyLine = line;
	}

	// Use this for initialization
	public void Start (int numberWave, Enemy.EnemyType type) {
		Global.currentType = Enemy.WaveToType ();
		interval = EnemyParam.GenerationInterval();
		intervalTimer = 0;

		number = EnemyParam.GenerationNumber(type);
	}
	
	// Update is called once per frame
	public void Update () {
		if (number<=0) {
			//すべての敵出現

			return;
		}

		intervalTimer += Time.deltaTime;
		if (intervalTimer >= interval) {
			intervalTimer -= interval;
			Enemy.Add (_pathList, Enemy.WaveToType(),enemyLine);
			number--;
			return;
		}




	}
}
