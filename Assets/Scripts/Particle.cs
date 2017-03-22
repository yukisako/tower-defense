using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : Token {

	public Sprite sprite0;	//塗りつぶしの円
	public Sprite sprite1;	//リング

	public enum eType{
		Ball,
		Ring,
		Eclipse,
	}

	public static TokenMgr<Particle> parent;

	public static Particle Add(eType type, int timer, float px, float py, float direction, float speed){
		Particle particle = parent.Add (px, py, direction, speed);
		if (particle == null) {
			return null;
		}
		particle.Init (type, timer);
		return particle;
	}
	//メンバ変数定義
	private eType type; 

	//消滅タイマ
	int destroyTimer;

	const float SCALE_MAX = 4;
	float scaleTimer;

	void Init(eType type, int timer){
		switch(type){
		case eType.Ball:
			SetSprite(sprite0);
			break;

		case eType.Ring:
			SetSprite (sprite1);
			scaleTimer = SCALE_MAX;
			break;
		}

		destroyTimer = timer;

		Scale = 1.0f;
		Alpha = 1.0f;
	}




	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		switch (type) {
		case eType.Ball:
			MulVelocity (0.9f);
			MulScale (0.93f);
			break;


		case eType.Ring:
			scaleTimer *= 0.9f;
			Scale = (SCALE_MAX - scaleTimer);

			Alpha -= 0.05f;
			break;
		}

		destroyTimer--;
		if (destroyTimer < 1) {
			Vanish ();
		}
	}
}
