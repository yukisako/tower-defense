using UnityEngine;
using UnityEngine.Advertisements;

public class UnityAds : MonoBehaviour
{
	void Awake () {
		// ゲームIDを入力して、Unity Adsを初期化する
		if (Advertisement.isSupported)
		{
			//2つ目の引数は、true＝テストモードなので、アプリリリース時には、falseに設定すること
			Advertisement.Initialize ("1368885",true);
		}  else {
			Debug.Log("Platform not supported");
		}
	}


	public void ShowAd()
	{
		Debug.Log ("showAd1");
		Debug.Log (Advertisement.IsReady());

		if (Advertisement.IsReady("rewardedVideo"))
		{
			var options = new ShowOptions { resultCallback = HandleShowResult };
			Debug.Log ("showAd2");
			Advertisement.Show("rewardedVideo", options);
		}
	}

	private void HandleShowResult(ShowResult result)
	{
		switch (result)
		{
		case ShowResult.Finished:
			Debug.Log("The ad was successfully shown.");
			//Application.LoadLevel ("Main");
			//
			// YOUR CODE TO REWARD THE GAMER
			// Give coins etc.
			break;
		case ShowResult.Skipped:
			Debug.Log("The ad was skipped before reaching the end.");
			break;
		case ShowResult.Failed:
			Debug.LogError("The ad failed to be shown.");
			break;
		}
	}
}


/*
using UnityEngine;
using UnityEngine.Advertisements;

public class UnityAdsExample : MonoBehaviour
{
  public void ShowAd()
  {
    if (Advertisement.IsReady())
    {
      Advertisement.Show();
    }
  }
}
*/