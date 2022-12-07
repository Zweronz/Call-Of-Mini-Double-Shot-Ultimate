using UnityEngine;

public class ParticleHalo : MonoBehaviour
{
	public Transform m_camera;

	private Vector3 m_offset;

	private void Start()
	{
		if (m_camera == null)
		{
			m_camera = Camera.main.transform;
		}
		m_offset = base.transform.localPosition;
	}

	private void Update()
	{
		Vector3 localPosition = m_camera.position - base.transform.root.position;
		localPosition.y = 0f;
		localPosition.Normalize();
		localPosition *= 0.1f;
		localPosition.y = m_offset.y;
		base.transform.localPosition = localPosition;
	}
}
