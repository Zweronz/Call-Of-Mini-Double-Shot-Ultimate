using System.Collections;
using UnityEngine;
using Zombie3D;

public class PathDoor : MonoBehaviour
{
	public GameObject m_LeftDoor;

	public GameObject m_RightDoor;

	private Vector3 m_LeftDoorOriginPos;

	private Vector3 m_RightDoorOriginPos;

	public float m_ClosedDistance = 2.1899998f;

	public float m_OpenedDistance = 7.5f;

	private float m_DetectedDistance = 3.5f;

	private float m_DoorOpenMoveSpeed = 5f;

	private bool m_bOpening;

	private bool m_bOpened;

	private bool m_bClosing;

	private bool m_bClosed = true;

	private float m_LastCheckTime = -1f;

	private WormScript m_Worm;

	private void Start()
	{
		m_bClosed = true;
		m_bClosing = false;
		m_bOpened = false;
		m_bOpening = false;
		m_LeftDoorOriginPos = m_LeftDoor.transform.position;
		m_RightDoorOriginPos = m_RightDoor.transform.position;
	}

	private void Update()
	{
		if (m_bOpening)
		{
			Vector3 normalized = (m_LeftDoor.transform.position - m_RightDoor.transform.position).normalized;
			m_LeftDoor.transform.Translate(normalized * m_DoorOpenMoveSpeed * Time.deltaTime, Space.World);
			m_RightDoor.transform.Translate(-normalized * m_DoorOpenMoveSpeed * Time.deltaTime, Space.World);
			if (Vector3.Distance(m_LeftDoor.transform.position, m_RightDoor.transform.position) >= m_OpenedDistance)
			{
				Debug.Log("Opened - " + base.gameObject.name);
				m_bOpened = true;
				m_bOpening = false;
				m_bClosed = false;
			}
		}
		else if (m_bClosing)
		{
			Vector3 normalized2 = (m_RightDoor.transform.position - m_LeftDoor.transform.position).normalized;
			m_LeftDoor.transform.Translate(normalized2 * m_DoorOpenMoveSpeed * Time.deltaTime, Space.World);
			m_RightDoor.transform.Translate(-normalized2 * m_DoorOpenMoveSpeed * Time.deltaTime, Space.World);
			if (Vector3.Distance(m_LeftDoor.transform.position, m_RightDoor.transform.position) <= m_ClosedDistance)
			{
				m_LeftDoor.transform.position = m_LeftDoorOriginPos;
				m_RightDoor.transform.position = m_RightDoorOriginPos;
				Debug.Log("Closed - " + base.gameObject.name);
				m_bClosed = true;
				m_bClosing = false;
				m_bOpened = false;
			}
		}
		if (m_Worm != null && m_Worm.gameObject != null && m_Worm.GetHp() <= 0f)
		{
			Object.Destroy(m_Worm.gameObject);
		}
		if (!(m_Worm == null))
		{
			return;
		}
		float num = 2f;
		Player player = GameApp.GetInstance().GetGameScene().GetPlayer();
		if ((base.transform.position - player.GetTransform().position).sqrMagnitude < 400f)
		{
			num = 1f;
		}
		if (!(Time.time - m_LastCheckTime > num))
		{
			return;
		}
		m_LastCheckTime = Time.time;
		if (m_bClosed && !m_bOpening)
		{
			Player friendPlayer = GameApp.GetInstance().GetGameScene().GetFriendPlayer();
			if (friendPlayer != null && (base.transform.position - friendPlayer.GetTransform().position).sqrMagnitude < m_DetectedDistance * m_DetectedDistance)
			{
				Open();
			}
			if (!((base.transform.position - player.GetTransform().position).sqrMagnitude < m_DetectedDistance * m_DetectedDistance))
			{
				Hashtable enemies = GameApp.GetInstance().GetGameScene().GetEnemies();
				{
					foreach (Enemy value in enemies.Values)
					{
						if (value != null && (base.transform.position - value.GetTransform().position).sqrMagnitude < m_DetectedDistance * m_DetectedDistance)
						{
							Open();
							break;
						}
					}
					return;
				}
			}
			Open();
		}
		else
		{
			if (!m_bOpened || m_bClosing)
			{
				return;
			}
			bool flag = false;
			Player friendPlayer2 = GameApp.GetInstance().GetGameScene().GetFriendPlayer();
			if (friendPlayer2 != null)
			{
				if ((base.transform.position - player.GetTransform().position).sqrMagnitude >= m_DetectedDistance * m_DetectedDistance && (base.transform.position - friendPlayer2.GetTransform().position).sqrMagnitude >= m_DetectedDistance * m_DetectedDistance)
				{
					flag = true;
				}
			}
			else if ((base.transform.position - player.GetTransform().position).sqrMagnitude >= m_DetectedDistance * m_DetectedDistance)
			{
				flag = true;
			}
			Hashtable enemies2 = GameApp.GetInstance().GetGameScene().GetEnemies();
			foreach (Enemy value2 in enemies2.Values)
			{
				if (value2 != null && (base.transform.position - value2.GetTransform().position).sqrMagnitude < m_DetectedDistance * m_DetectedDistance)
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				Close();
			}
		}
	}

	public void Open()
	{
		Debug.Log("Open Begin " + base.gameObject.name);
		m_bOpening = true;
		m_bClosing = false;
		m_bClosed = false;
		m_bOpened = false;
	}

	public void Close()
	{
		Debug.Log("Close Begin " + base.gameObject.name);
		m_bClosing = true;
		m_bOpening = false;
		m_bClosed = false;
		m_bOpened = false;
	}

	public bool IsDoorClosed()
	{
		return m_bClosed;
	}

	public GameObject GetWorm()
	{
		if (m_Worm == null || m_Worm.gameObject == null)
		{
			return null;
		}
		return m_Worm.gameObject;
	}

	public void GenerateWorm()
	{
		if (m_Worm == null && m_bClosed)
		{
			GameObject gameObject = Object.Instantiate(Resources.Load("Zombie3D/Misc/Worm", typeof(GameObject)), base.transform.position + base.transform.up * 0.5f, base.transform.rotation) as GameObject;
			Debug.Log(gameObject.name);
			m_Worm = gameObject.GetComponent(typeof(WormScript)) as WormScript;
			m_Worm.maxHp = 220f;
		}
	}
}
