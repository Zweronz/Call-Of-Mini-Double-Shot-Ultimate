using System;
using UnityEngine;

public class ParticleRing : MonoBehaviour
{
	public float m_speed = 5f;

	public int m_count = 18;

	private void Start()
	{
		base.GetComponent<ParticleEmitter>().emit = false;
	}

	private void Update()
	{
	}

	public void Blast()
	{
		base.GetComponent<ParticleEmitter>().Emit(m_count);
		Particle[] particles = base.GetComponent<ParticleEmitter>().particles;
		float num = (float)Math.PI * 2f / (float)m_count;
		for (int i = 0; i < m_count; i++)
		{
			Vector3 vector = new Vector3(Mathf.Cos((float)i * num), 0f, Mathf.Sin((float)i * num));
			particles[i].velocity = vector * m_speed;
			particles[i].position += particles[i].velocity * Time.fixedDeltaTime * 5f;
		}
		base.GetComponent<ParticleEmitter>().particles = particles;
	}
}
