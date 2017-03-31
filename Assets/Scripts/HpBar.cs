using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour {

	private Slider slider;
	// Use this for initialization
	void Start () {
		slider = GameObject.Find ("HP").GetComponent<Slider> ();
	}

	float hp = 0;

	// Update is called once per frame
	void Update () {
		hp += 0.01f;
		if (hp > 1) {
			hp = 0;
		}

		slider.value = hp;
	}
}
