using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cost : MonoBehaviour {

	public static int TowerProduction(){
		int num = Tower.parent.Count();

		int basic = 8;
		float ratio = Mathf.Pow (1.3f, num);

		return (int)(basic * ratio);

	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
