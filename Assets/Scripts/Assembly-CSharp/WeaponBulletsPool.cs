using System.Collections.Generic;
using UnityEngine;

public class WeaponBulletsPool
{
	private GameObject m_FolderObject;

	private List<GameObject> m_Bullets;

	public void Init(string poolName, GameObject bulletPrefab, int initNum)
	{
		m_FolderObject = new GameObject(poolName);
		m_FolderObject.transform.position = new Vector3(0f, 10000.1f, 0f);
		m_Bullets = new List<GameObject>(initNum);
		for (int i = 0; i < initNum; i++)
		{
			GameObject gameObject = Object.Instantiate(bulletPrefab) as GameObject;
			gameObject.SetActiveRecursively(false);
			m_Bullets.Add(gameObject);
			gameObject.transform.parent = m_FolderObject.transform;
		}
	}

	public GameObject CreateBullet(Vector3 position, Quaternion rotation)
	{
		bool flag = false;
		for (int i = 0; i < m_Bullets.Count; i++)
		{
			if (!m_Bullets[i].active)
			{
				flag = true;
				m_Bullets[i].SetActiveRecursively(true);
				m_Bullets[i].transform.position = position;
				m_Bullets[i].transform.rotation = rotation;
				return m_Bullets[i];
			}
		}
		if (!flag && m_Bullets.Count > 0)
		{
			GameObject gameObject = Object.Instantiate(m_Bullets[0]) as GameObject;
			gameObject.SetActiveRecursively(true);
			gameObject.transform.parent = m_FolderObject.transform;
			m_Bullets.Add(gameObject);
			gameObject.transform.position = position;
			gameObject.transform.rotation = rotation;
			return gameObject;
		}
		return null;
	}

	public GameObject CreateSubBullet(Vector3 position, Quaternion rotation, GameObject obj)
	{
		GameObject gameObject = Object.Instantiate(obj) as GameObject;
		gameObject.SetActiveRecursively(true);
		gameObject.transform.parent = m_FolderObject.transform;
		if (!m_Bullets.Contains(gameObject))
		{
			m_Bullets.Add(gameObject);
		}
		gameObject.transform.position = position;
		gameObject.transform.rotation = rotation;
		return gameObject;
	}

	public void DoLogic()
	{
	}

	public GameObject DeleteBullet(GameObject obj)
	{
		obj.SetActiveRecursively(false);
		return obj;
	}

	public void DestroyPool()
	{
		for (int i = 0; i < m_Bullets.Count; i++)
		{
			WeaponBulletScript component = m_Bullets[i].GetComponent<WeaponBulletScript>();
			if (component != null)
			{
				component.DestroyBullet(true);
			}
		}
		m_Bullets.Clear();
		Object.Destroy(m_FolderObject);
	}
}
