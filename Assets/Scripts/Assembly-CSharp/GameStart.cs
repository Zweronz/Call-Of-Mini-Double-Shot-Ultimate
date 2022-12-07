using UnityEngine;
using Zombie3D;

public class GameStart : MonoBehaviour
{
	private void Start()
	{
		GameApp.GetInstance().SetLoadMap(false);
		SceneUIManager.Instance().ChangeSceneUI(SceneUIManager.SceneUI.ChoosePointsUI);
	}

	private void Update()
	{
	}
}
