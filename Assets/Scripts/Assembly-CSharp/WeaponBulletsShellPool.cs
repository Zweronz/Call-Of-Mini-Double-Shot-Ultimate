using System.Collections.Generic;
using UnityEngine;
using Zombie3D;

public class WeaponBulletsShellPool
{
	private GameObject m_FolderObject;

	private List<GameObject> m_BulletsShell;

	private List<float> m_BulletsShellStartTime;

	public void Init(string poolName, GameObject bulletHitParticlePrefab, float scale, int initNum)
	{
		if (GameApp.GetInstance().GetGameState().m_eGameMode.m_ePlayMode == GameState.NetworkGameMode.PlayMode.E_Console)
		{
			m_FolderObject = new GameObject(poolName);
			m_FolderObject.transform.position = new Vector3(0f, 10000.1f, 0f);
			m_BulletsShell = new List<GameObject>(initNum);
			m_BulletsShellStartTime = new List<float>(initNum);
			for (int i = 0; i < initNum; i++)
			{
				GameObject gameObject = Object.Instantiate(bulletHitParticlePrefab) as GameObject;
				gameObject.transform.localScale *= scale;
				gameObject.SetActiveRecursively(false);
				m_BulletsShell.Add(gameObject);
				m_BulletsShellStartTime.Add(0f);
				gameObject.transform.parent = m_FolderObject.transform;
			}
		}
	}

	public GameObject CreateBulletShell(Vector3 position)
	{
		if (GameApp.GetInstance().GetGameState().m_eGameMode.m_ePlayMode != 0)
		{
			return null;
		}
		bool flag = false;
		for (int i = 0; i < m_BulletsShell.Count; i++)
		{
			if (!m_BulletsShell[i].active)
			{
				flag = true;
				m_BulletsShell[i].SetActiveRecursively(true);
				m_BulletsShell[i].transform.position = position;
				m_BulletsShellStartTime[i] = Time.time;
				ParticleEmitter[] componentsInChildren = m_BulletsShell[i].GetComponentsInChildren<ParticleEmitter>();
				for (int j = 0; j < componentsInChildren.Length; j++)
				{
					componentsInChildren[j].emit = true;
					componentsInChildren[j].Emit();
				}
				return m_BulletsShell[i];
			}
		}
		if (!flag && m_BulletsShell.Count > 0)
		{
			GameObject gameObject = Object.Instantiate(m_BulletsShell[0]) as GameObject;
			gameObject.SetActiveRecursively(true);
			gameObject.transform.parent = m_FolderObject.transform;
			m_BulletsShell.Add(gameObject);
			m_BulletsShellStartTime.Add(Time.time);
			gameObject.transform.position = position;
			return gameObject;
		}
		return null;
	}

	public void DoLogic()
	{
		if (GameApp.GetInstance().GetGameState().m_eGameMode.m_ePlayMode != 0)
		{
			return;
		}
		for (int i = 0; i < m_BulletsShell.Count; i++)
		{
			if (m_BulletsShell[i].active && Time.time - m_BulletsShellStartTime[i] >= 1f)
			{
				DeleteBulletShell(m_BulletsShell[i]);
			}
		}
	}

	public GameObject DeleteBulletShell(GameObject obj)
	{
		ParticleEmitter[] componentsInChildren = obj.GetComponentsInChildren<ParticleEmitter>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].emit = false;
		}
		obj.SetActiveRecursively(false);
		return obj;
	}

	public void DestroyPool()
	{
		if (GameApp.GetInstance().GetGameState().m_eGameMode.m_ePlayMode == GameState.NetworkGameMode.PlayMode.E_Console)
		{
			m_BulletsShell.Clear();
			Object.Destroy(m_FolderObject);
		}
	}
}
