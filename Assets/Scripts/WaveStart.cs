using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveStart : TextObj {

	enum eState{
		Appear,
		Wait,
		Disapper,
		End
	}


	const float CENTER_X = -40;
	const float OFFSET_X = 600;

	eState state = eState.End;

	float timer = 0;

	// Use this for initialization
	void Start () {
		X = CENTER_X + OFFSET_X;
		Visible = false;
	}

	public void Begin (int numberWave){
		if (numberWave != 1) {
			int bonus = (int)(Global.Money * 0.05);
			Global.AddMoney(bonus);
			Label = "Wave " + numberWave + "\nBonus $" + bonus;
		} else {
			Label = "Wave " + numberWave;
		}
		timer = OFFSET_X;
		state = eState.Appear;

		Visible = true;
	}

	void FixedUpdate(){
		switch (state) {
		case eState.Appear:
			timer *= 0.95f;
			X = CENTER_X - timer;
			if (timer < 1) {
				timer = 40;
				state = eState.Wait;
			}
			break;
		case eState.Wait:
			timer -= 1;
			if (timer < 1) {
				timer = OFFSET_X;
				state = eState.Disapper;
			}
			break;

		case eState.Disapper:
			timer *= 0.95f;
			X = CENTER_X + (OFFSET_X - timer);
			if (timer < 1) {
				state = eState.End;
				Visible = false;
			}
			break;

		case eState.End:
			break;

		}
	}


	// Update is called once per frame
	void Update () {
		
	}
}
