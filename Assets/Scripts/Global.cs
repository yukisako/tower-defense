using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global : MonoBehaviour {

	private static int wave = 1;

	public static int Wave{
		get {return wave;}
	}

	public static void NextWave(){
		wave++;
	}

	private static int line = 4;
	public static int Line{
		get {return line;}
	}


	const int MONEY_INIT = 30;
	private static int money;
	public static int Money{
		get {return money;}
	}

	public static void Init(){
		wave = 1;
		money = MONEY_INIT;
	}

	public static void AddMoney(int value){
		money += value;
	}

	public static void UseMoney(int value){
		money -= value;
		if (money < 0) {
			money = 0;
		}
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
