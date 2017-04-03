using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NCMB;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using System.Text;

public class Ranking : MonoBehaviour {

	public InputField inputNickname;

	public int currentRank = 0;
	public List<NCMBObject> topRankers = null;
	public List<NCMBObject> neighbors  = null;

	private TextObj rankingText;
	private TextObj neighborText;
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
			rankingText = MyCanvas.Find<TextObj> ("TextRanking");
			rankingText.SetLabelFormat ("{0}", ranking);

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
			neighborText = MyCanvas.Find<TextObj> ("TextNeighbor");
			neighborText.SetLabelFormat ("{0}",ranking );

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


	private string makeTopRanking(List<NCMBObject> objList){
		string ranking = "";
		for (int i = 0; i < 5; i++) {
			if (currentRank == i+1) {
				ranking += "<color=#ff3333>"+string.Format ("{0} ", i+1) + makeRanking (objList [i]) + "</color>" + "\n";
			} else {
				ranking += string.Format ("{0} ", i+1) + makeRanking (objList [i]) + "\n";
			}
		}
		return (ranking);
	}

	private string makeNeighborRanking(List<NCMBObject> objList, int skipNum){
		string ranking = "";
		for (int i = 0; i < objList.Count; i++) {
			if (skipNum + i == currentRank) {
				ranking = ranking + "<color=#ff3333>" + (string.Format ("{0} ", skipNum + i) + makeRanking (objList [i]) + "</color>" + "\n");
		
			} else {
				ranking = ranking + (string.Format ("{0} ", skipNum + i) + makeRanking (objList [i]) + "\n");
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
			
		return (string.Format("{0:D6}", score) + "  " + formatName(name));
	}

	private string formatName(string name){
		int num = 8;
		Encoding sjisEnc = Encoding.GetEncoding("Shift_JIS");
		int byteNum = sjisEnc.GetByteCount(name);
		while (byteNum > 10) {
			name = name.PadRight (num).Substring (0, num);
			byteNum = sjisEnc.GetByteCount(name);
			Debug.Log (byteNum);
			num--;
		}
		return name;

	}


}