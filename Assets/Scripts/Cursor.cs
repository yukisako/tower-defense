using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : Token {

	//四角
	public Sprite RectSprite;

	//バッテン
	public Sprite CrossSprite;

	private GameObject selectObject = null;
	public GameObject SelectObject{
		get {return selectObject;}
	}

	void SetSelectObject(){
		int mask = 1 << LayerMask.NameToLayer ("Tower");
		Collider2D collider = Physics2D.OverlapPoint (GetPosition (), mask);
		selectObject = null;

		if (collider != null) {
			selectObject = collider.gameObject;
		}
	}



	private bool placeable = true;


	public bool Placeable{
		get {return placeable;}
		set {
			if (value) {
				SetSprite (RectSprite);
			} else {
				SetSprite (CrossSprite);
			}
			placeable = value;
		}
	}

	public void Proc(Layer2D collisionLayer){
		Vector3 screenPosition = Input.mousePosition;
		Vector2 worldPosition = Camera.main.ScreenToWorldPoint (screenPosition);

		//チップ座標系
		int i = Field.ToChipX(worldPosition.x);
		int j = Field.ToChipY (worldPosition.y);

		//ワールド座標系に再変換
		X = Field.ToWorldX (i);
		Y = Field.ToWorldY (j);


		Placeable = (collisionLayer.Get(i,j)==0);
		Visible = (collisionLayer.IsOutOfRange (i, j) == false);

		//選択しているオブジェクトを設定
		SetSelectObject();
	}


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
