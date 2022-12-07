using System.Collections.Generic;
using UnityEngine;

public class WeaponBulletsHitParticlePool
{
	private GameObject m_FolderObject;

	private List<GameObject> m_BulletsHitParticles;

	private List<float> m_BulletsHitParticlesStartTime;

	public void Init(string poolName, GameObject bulletHitParticlePrefab, int initNum)
	{
		m_FolderObject = new GameObject(poolName);
		m_FolderObject.transform.position = new Vector3(0f, 10000.1f, 0f);
		m_BulletsHitParticles = new List<GameObject>(initNum);
		m_BulletsHitParticlesStartTime = new List<float>(initNum);
		for (int i = 0; i < initNum; i++)
		{
			GameObject gameObject = Object.Instantiate(bulletHitParticlePrefab) as GameObject;
			ParticleEmitter[] componentsInChildren = gameObject.GetComponentsInChildren<ParticleEmitter>();
			for (int j = 0; j < componentsInChildren.Length; j++)
			{
				componentsInChildren[j].emit = false;
			}
			ParticleAnimator[] componentsInChildren2 = gameObject.GetComponentsInChildren<ParticleAnimator>();
			for (int k = 0; k < componentsInChildren2.Length; k++)
			{
				componentsInChildren2[k].autodestruct = false;
			}
			gameObject.SetActiveRecursively(false);
			m_BulletsHitParticles.Add(gameObject);
			m_BulletsHitParticlesStartTime.Add(0f);
			gameObject.transform.parent = m_FolderObject.transform;
		}
	}

	public GameObject CreateBulletHitParticle(Vector3 position)
	{
		bool flag = false;
		for (int i = 0; i < m_BulletsHitParticles.Count; i++)
		{
			if (!m_BulletsHitParticles[i].active)
			{
				flag = true;
				m_BulletsHitParticles[i].SetActiveRecursively(true);
				m_BulletsHitParticles[i].transform.position = position;
				m_BulletsHitParticlesStartTime[i] = Time.time;
				ParticleEmitter[] componentsInChildren = m_BulletsHitParticles[i].GetComponentsInChildren<ParticleEmitter>();
				for (int j = 0; j < componentsInChildren.Length; j++)
				{
					componentsInChildren[j].emit = true;
					componentsInChildren[j].Emit();
				}
				return m_BulletsHitParticles[i];
			}
		}
		if (!flag && m_BulletsHitParticles.Count > 0)
		{
			GameObject gameObject = Object.Instantiate(m_BulletsHitParticles[0]) as GameObject;
			gameObject.SetActiveRecursively(true);
			gameObject.transform.parent = m_FolderObject.transform;
			m_BulletsHitParticles.Add(gameObject);
			m_BulletsHitParticlesStartTime.Add(Time.time);
			gameObject.transform.position = position;
			return gameObject;
		}
		return null;
	}

	public void DoLogic()
	{
		for (int i = 0; i < m_BulletsHitParticles.Count; i++)
		{
			if (m_BulletsHitParticles[i].active && Time.time - m_BulletsHitParticlesStartTime[i] >= 0.2f)
			{
				DeleteBullet(m_BulletsHitParticles[i]);
			}
		}
	}

	public GameObject DeleteBullet(GameObject obj)
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
		m_BulletsHitParticles.Clear();
		Object.Destroy(m_FolderObject);
	}
}
