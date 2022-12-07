using UnityEngine;

public class ParticleBlast : MonoBehaviour
{
	public float m_speedMin = 1f;

	public float m_speedMax = 2f;

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
		float num = Random.Range(m_speedMin, m_speedMax);
		for (int i = 0; i < particles.Length; i++)
		{
			Vector3 normalized = (particles[i].position - base.transform.position).normalized;
			particles[i].velocity = normalized * num;
			Vector3 rhs = Vector3.Cross(lhs, normalized);
			Vector3 from = Vector3.Cross(lhs, rhs);
			particles[i].rotation = Vector3.Angle(from, Vector3.right);
		}
		base.GetComponent<ParticleEmitter>().particles = particles;
	}
}
