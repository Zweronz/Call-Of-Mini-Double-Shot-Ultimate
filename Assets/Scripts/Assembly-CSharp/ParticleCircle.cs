using System;
using UnityEngine;

public class ParticleCircle : MonoBehaviour
{
	public float m_disMin = 1f;

	public float m_disMax = 1f;

	public float m_speedMin;

	public float m_speedMax;

	private void Start()
	{
		base.GetComponent<ParticleEmitter>().emit = false;
		Blast();
	}

	private void Update()
	{
	}

	private void Blast()
	{
		base.GetComponent<ParticleEmitter>().Emit();
		Particle[] particles = base.GetComponent<ParticleEmitter>().particles;
		Vector3 lhs = -Camera.main.transform.forward;
		Vector3 normalized = Vector3.Cross(lhs, Vector3.up).normalized;
		Vector3 normalized2 = Vector3.Cross(lhs, normalized).normalized;
		float num = (float)Math.PI * 2f / (float)particles.Length;
		for (int i = 0; i < particles.Length; i++)
		{
			float num2 = (float)i * num;
			num2 += UnityEngine.Random.Range(-(float)Math.PI / 10f, (float)Math.PI / 10f);
			float num3 = UnityEngine.Random.Range(m_disMin, m_disMax);
			Vector3 vector = normalized * Mathf.Cos(num2) + normalized2 * Mathf.Sin(num2);
			float num4 = UnityEngine.Random.Range(m_speedMin, m_speedMax);
			particles[i].velocity = vector * num4;
			particles[i].position += vector * num3;
			particles[i].rotation = num2 * 57.29578f;
		}
		base.GetComponent<ParticleEmitter>().particles = particles;
	}
}
