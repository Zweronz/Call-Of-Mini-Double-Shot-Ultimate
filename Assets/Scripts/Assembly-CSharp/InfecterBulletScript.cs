using UnityEngine;
using Zombie3D;

public class InfecterBulletScript : MonoBehaviour
{
	protected float lastUpdateTime;

	public float damage = 15f;

	public float userFaintTime = 0.3f;

	private float startTime;

	private float radius = 1f;

	private float time = 0.3f;

	public float flySpeed = 3f;

	private Vector3 dir;

	private int turnTimes = 4;

	private GameObject m_playerGO;

	private void Start()
	{
		if (m_playerGO == null)
		{
			startTime = Time.time;
			Player player = GameApp.GetInstance().GetGameScene().GetPlayer();
			m_playerGO = player.PlayerObject;
			dir = new Vector3(m_playerGO.transform.position.x, base.transform.position.y, m_playerGO.transform.position.z) - base.transform.position;
			base.transform.forward = dir;
		}
	}

	public void Init(GameObject playerGO)
	{
		startTime = Time.time;
		m_playerGO = playerGO;
		dir = new Vector3(m_playerGO.transform.position.x, base.transform.position.y, m_playerGO.transform.position.z) - base.transform.position;
		base.transform.forward = dir;
	}

	private void Update()
	{
		if (Time.time - lastUpdateTime < 0.001f)
		{
			return;
		}
		float num = Time.time - lastUpdateTime;
		lastUpdateTime = Time.time;
		if (turnTimes > 0 && Time.time - startTime >= time)
		{
			if (m_playerGO == null)
			{
				m_playerGO = GameApp.GetInstance().GetGameScene().GetPlayer()
					.PlayerObject;
				Debug.Log("m_playerGO == null");
			}
			dir = new Vector3(m_playerGO.transform.position.x, base.transform.position.y, m_playerGO.transform.position.z) - base.transform.position;
			turnTimes--;
			startTime = Time.time;
		}
		Player player = GameApp.GetInstance().GetGameScene().GetPlayer();
		float num2 = Vector3.Distance(player.GetTransform().position, new Vector3(base.transform.position.x, player.GetTransform().position.y, base.transform.position.z));
		if (num2 <= radius)
		{
			player.OnHit(damage);
			Object.Destroy(base.gameObject);
			return;
		}
		if (GameApp.GetInstance().GetGameState().m_eGameMode.m_ePlayMode == GameState.NetworkGameMode.PlayMode.E_Console)
		{
			Player friendPlayer = GameApp.GetInstance().GetGameScene().GetFriendPlayer();
			if (friendPlayer != null)
			{
				float num3 = Vector3.Distance(friendPlayer.GetTransform().position, new Vector3(base.transform.position.x, friendPlayer.GetTransform().position.y, base.transform.position.z));
				if (num3 <= radius)
				{
					friendPlayer.OnHit(damage);
					Object.Destroy(base.gameObject);
					return;
				}
			}
			else if (base.transform.position.y < 0.1f)
			{
				Object.Destroy(base.gameObject);
				return;
			}
		}
		else
		{
			foreach (Player recipientPlayer in PlayerManager.Instance.GetRecipientPlayerList())
			{
				float num4 = Vector3.Distance(recipientPlayer.GetTransform().position, new Vector3(base.transform.position.x, recipientPlayer.GetTransform().position.y, base.transform.position.z));
				if (num2 <= radius)
				{
					Object.Destroy(base.gameObject);
				}
			}
		}
		base.transform.forward = Vector3.Lerp(base.transform.forward, dir, Time.deltaTime);
		base.transform.Translate(base.transform.forward * flySpeed * Time.deltaTime, Space.World);
	}
}
