using UnityEngine;
using Zombie3D;

public class GameLogin : MonoBehaviour
{
	private void Start()
	{
		Application.targetFrameRate = 120;
		OpenClickPlugin.Initialize("8FABBCBD-EAEA-4F82-BDAF-99891A6C34D6");
		GameObject gameObject = new GameObject("_ChartBoostEventListener");
		gameObject.AddComponent<ChartBoostAndroidEventListener>();
		Object.DontDestroyOnLoad(gameObject);
		ChartBoostAndroid.init("50e7a23b16ba477806000067", "8b242000484ea407e7c358d181bb32b2e3b2ffdc");
		ChartBoostAndroid.onStart();
		ChartBoostAndroid.cacheInterstitial(null);
		GameApp.GetInstance().SetLoadMap(false);
		SceneUIManager.Instance().ChangeSceneUI(SceneUIManager.SceneUI.ChoosePointsUI);
	}

	private void Update()
	{
	}
}
