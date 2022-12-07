using UnityEngine;

public class FixedDoor : MonoBehaviour
{
	public Color _color = Color.white;

	public float _radius = 5f;

	public bool _showWireSphere;

	private void OnDrawGizmos()
	{
		Gizmos.color = _color;
		Gizmos.DrawSphere(base.transform.position, 0.3f);
		if (_showWireSphere)
		{
			Gizmos.DrawWireSphere(base.transform.position, _radius);
		}
	}
}
