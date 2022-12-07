using System.Collections.Generic;
using UnityEngine;

public class AvatarEffect02 : MonoBehaviour
{
	private float m_LastEmitTime;

	private float m_EmitRandomPeriod = 1f;

	private List<GameObject> m_ParticleObjs;

	private void Start()
	{
		m_ParticleObjs = new List<GameObject>();
		Transform transform = base.gameObject.transform.Find("x800_10_01");
		if (transform != null && transform.GetComponent<ParticleEmitter>() != null)
		{
			m_ParticleObjs.Add(transform.gameObject);
		}
		Transform transform2 = base.gameObject.transform.Find("x800_10_02");
		if (transform2 != null && transform2.GetComponent<ParticleEmitter>() != null)
		{
			m_ParticleObjs.Add(transform2.gameObject);
		}
	}

	private void Update()
	{
		if (m_ParticleObjs.Count < 1)
		{
			return;
		}
		if (Time.time - m_LastEmitTime > 0.3f)
		{
			for (int i = 0; i < m_ParticleObjs.Count; i++)
			{
				if (m_ParticleObjs[i].GetComponent<ParticleEmitter>() != null && m_ParticleObjs[i].GetComponent<ParticleEmitter>().emit)
				{
					m_ParticleObjs[i].GetComponent<ParticleEmitter>().emit = false;
				}
			}
		}
		if (Time.time - m_LastEmitTime > m_EmitRandomPeriod)
		{
			m_LastEmitTime = Time.time;
			m_EmitRandomPeriod = Random.Range(1f, 5f);
			int index = Random.Range(0, m_ParticleObjs.Count);
			if (m_ParticleObjs[index].GetComponent<ParticleEmitter>() != null)
			{
				m_ParticleObjs[index].GetComponent<ParticleEmitter>().emit = true;
				m_ParticleObjs[index].GetComponent<ParticleEmitter>().Emit();
			}
		}
	}
}
