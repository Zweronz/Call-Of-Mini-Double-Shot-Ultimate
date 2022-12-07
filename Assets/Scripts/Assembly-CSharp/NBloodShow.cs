using UnityEngine;
using Zombie3D;

public class NBloodShow : MonoBehaviour
{
	private GameState m_gamestate;

	private Rect m_rBlood = new Rect(550f, 100f, 260f, 25f);

	private void Start()
	{
		m_gamestate = GameApp.GetInstance().GetGameState();
	}

	private void OnGUI()
	{
		if (SceneUIManager.Instance().GetSceneUIMode() == SceneUIManager.SceneUI.ChoosePointsUI)
		{
			ShowPing();
		}
		if (SceneUIManager.Instance().GetSceneUIMode() == SceneUIManager.SceneUI.NBattleUI && GameApp.GetInstance().GetGameState().m_eGameMode.m_ePlayMode != 0 && GameApp.GetInstance().GetGameScene().GetPlayer() != null)
		{
			ShowBlood();
		}
	}

	private void ShowPing()
	{
	}

	private void ShowBlood()
	{
	}
}
