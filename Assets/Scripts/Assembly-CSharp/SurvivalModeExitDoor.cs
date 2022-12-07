using UnityEngine;
using Zombie3D;

public class SurvivalModeExitDoor : MonoBehaviour
{
	private Player m_Player;

	private void Start()
	{
		Animation[] componentsInChildren = base.gameObject.GetComponentsInChildren<Animation>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i][componentsInChildren[i].clip.name].wrapMode = WrapMode.Loop;
		}
	}

	private void Update()
	{
		if (m_Player == null && GameApp.GetInstance().GetGameScene() != null)
		{
			m_Player = GameApp.GetInstance().GetGameScene().GetPlayer();
		}
	}

	private void OnControllerColliderHit(ControllerColliderHit hit)
	{
	}

	private void OnTriggerEnter(Collider collider)
	{
		GameApp.GetInstance().GetGameScene().ChangeToNextSurvivalModeScene();
	}
}
