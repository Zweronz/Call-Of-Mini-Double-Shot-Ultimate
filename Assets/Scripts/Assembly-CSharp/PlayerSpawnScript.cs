using UnityEngine;

public class PlayerSpawnScript : MonoBehaviour
{
	public Transform[] m_TransPositions;

	private void Awake()
	{
		if (m_TransPositions.Length > 0)
		{
			base.transform.position = m_TransPositions[Random.Range(0, m_TransPositions.Length)].position;
		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawSphere(base.transform.position, 0.3f);
	}
}
