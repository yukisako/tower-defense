using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NCMB;
using UnityEngine.UI;
using System;
using System.Text;

public class Ranking : MonoBehaviour {

	public InputField inputNickname;

	public int currentRank = 0;
	public List<NCMBObject> topRankers = null;
	public List<NCMBObject> neighbors  = null;

	private TextObj rankingStateText;


	// 現プレイヤーのハイスコアを受けとってランクを取得 ---------------
	public void fetchRank(){
		// データスコアの「HighScore」から検索
		NCMBQuery<NCMBObject> rankQuery = new NCMBQuery<NCMBObject> ("Ranking");
		int currentScore = Global.Score;
		rankQuery.WhereGreaterThanOrEqualTo("Score", currentScore);
		rankQuery.CountAsync((int count , NCMBException e )=>{
			if(e != null){
				//件数取得失敗
			}else{
				//件数取得成功
				currentRank = count;
				fetchTopRankers();
				fetchNeighbors();
			}
		});
	}


	// サーバーからトップ5を取得 ---------------    
	public void fetchTopRankers()
	{
		NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject> ("Ranking");

		//Scoreフィールドの降順でデータを取得
		query.OrderByDescending ("Score");

		//検索件数を5件に設定
		query.Limit = 5;
		//データストアでの検索を行う
		query.FindAsync ((List<NCMBObject> objList, NCMBException e) => {
			string ranking;
			if (e != null) {
				ranking = "Error!!\n Sorry, Couldn't load Ranking!!";
			} else {
				ranking = makeTopRanking(objList);
			}
			MyCanvas.SetActive ("TextRanking", true);
			MyCanvas.Find<TextObj> ("TextRanking").SetLabelFormat ("{0}", ranking);
		});
	}


	// サーバーからrankの前後2件を取得 ---------------
	public void fetchNeighbors()
	{
		neighbors = new List<NCMBObject>();

		// スキップする数を決める（ただし自分が1位か2位のときは調整する）
		int numSkip = currentRank - 3;
		if(numSkip < 0) numSkip = 0;

		// データストアの「HighScore」クラスから検索
		NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject> ("Ranking");
		query.OrderByDescending ("Score");
		query.Skip  = numSkip;
		query.Limit = 5;
		query.FindAsync ((List<NCMBObject> objList ,NCMBException e) => {
			string ranking;
			if (e != null) {
				//検索失敗時の処理
				ranking = "Error!!\n Sorry, Couldn't load Ranking!!";
			} else {
				//検索成功時の処理
				ranking = makeNeighborRanking(objList, numSkip+1);
			}
			MyCanvas.SetActive ("TextNeighbor", true);
			MyCanvas.Find<TextObj> ("TextNeighbor").SetLabelFormat ("{0}", ranking);
		});
	}

	public void register(){
		
		string nickname = inputNickname.text;
		MyCanvas.SetActive ("InputNickname", false);
		MyCanvas.SetActive ("ButtonResistRanking", false);
		MyCanvas.SetActive ("TextNicknameLabel", false);
		MyCanvas.SetActive ("TextRankingStates", true);
		NCMBObject ranking = new NCMBObject("Ranking");
		// オブジェクトに値を設定
		ranking ["Score"] = Global.Score;
		ranking ["Nickname"] = nickname;

				// データストアへの登録
		ranking.SaveAsync ((NCMBException e) => {      
			if (e != null) {
				rankingStateText = MyCanvas.Find<TextObj> ("TextRankingStates");
				rankingStateText.SetLabelFormat ("Sorry, Couldn't Send Data...");
			} else {
				MyCanvas.SetActive ("TextRankingStates", false);
				fetchRank();
			}                   
		});
	}

	void getRanking(){
		NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject> ("Ranking");

		//Scoreフィールドの降順でデータを取得
		query.OrderByDescending ("Score");

		//検索件数を5件に設定
		query.Limit = 5;

		//データストアでの検索を行う
		query.FindAsync ((List<NCMBObject> objList, NCMBException e) => {
			if (e != null) {
				//検索失敗時の処理
			} else {
				string ranking = makeTopRanking(objList);
			}
		});
	}
	//山中直幸

	private string makeTopRanking(List<NCMBObject> objList){
		string ranking = "";
		for (int i = 0; i < 5; i++) {
			string name = System.Convert.ToString (objList[i] ["Nickname"]);
			if (name == "") {
				name = "No Name";
			}

			if (currentRank == i+1) {
				MyCanvas.SetActive ($"TextRanking{i}", true);
				ranking += "<color=#ff3333>"+string.Format ("{0:D3} {1:D6}", i+1,System.Convert.ToInt32(objList[i] ["Score"])) + "</color>" + "\n";
				MyCanvas.Find<TextObj> ($"TextRanking{i}").SetLabelFormat ("<color=#ff3333>{0}</color>",name);
			} else {
				MyCanvas.SetActive ($"TextRanking{i}", true);
				ranking += string.Format ("{0:D3} {1:D6}", i+1,System.Convert.ToInt32(objList[i] ["Score"]))+ "\n";
				MyCanvas.Find<TextObj> ($"TextRanking{i}").SetLabelFormat ("{0}",name);
			}
		}
		return (ranking);
	}

	private string makeNeighborRanking(List<NCMBObject> objList, int skipNum){
		string ranking = "";
		for (int i = 0; i < objList.Count; i++) {
			string name = System.Convert.ToString (objList[i] ["Nickname"]);
			if (name == "") {
				name = "No Name";
			}

			if (skipNum + i == currentRank) {
				MyCanvas.SetActive ($"TextNeighbor{i}", true);

				ranking += "<color=#ff3333>"+string.Format ("{0:D3} {1:D6}", i+skipNum,System.Convert.ToInt32(objList[i] ["Score"])) + "</color>" + "\n";
				MyCanvas.Find<TextObj> ($"TextNeighbor{i}").SetLabelFormat ("<color=#ff3333>{0}</color>",name);
			} else {
				MyCanvas.SetActive ($"TextNeighbor{i}", true);
				ranking += string.Format ("{0:D3} {1:D6}", skipNum + i,System.Convert.ToInt32(objList[i] ["Score"]))+ "\n";
				MyCanvas.Find<TextObj> ($"TextNeighbor{i}").SetLabelFormat ("{0}",name);
			}
		}
		return (ranking);
	}

	private string makeRanking(NCMBObject obj){
		string name;
		int score = System.Convert.ToInt32(obj ["Score"]);
		if (obj["Nickname"] == "") {
			name = "No Name";
		} else {
			name = System.Convert.ToString(obj["Nickname"]);
		}
			
		return (string.Format("{0:D6}", score) + "  " + name);
	}

//
//	private string formatName(string name){
//		int num = 10;
//		Encoding sjisEnc = Encoding.GetEncoding("UTF-8");
//		int byteNum = sjisEnc.GetByteCount(name);
//		while (byteNum > 10) {
//			name = name.PadRight (num).Substring (0, num);
//			byteNum = sjisEnc.GetByteCount(name);
//			Debug.Log (byteNum);
//			num--;
//		}
//		return name;
//
//	}


}