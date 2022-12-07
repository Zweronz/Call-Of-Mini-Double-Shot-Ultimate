using UnityEngine;

public class WayPoint : MonoBehaviour
{
	public int index;

	public float radius = 0.5f;

	public WayPoint[] edges;

	public void OnDrawGizmos()
	{
		Gizmos.color = new Color(Color.white.r, Color.white.g, Color.white.b, 1f);
		Gizmos.DrawSphere(base.transform.position, radius);
		Gizmos.color = Color.white;
		WayPoint[] array = edges;
		foreach (WayPoint wayPoint in array)
		{
			Gizmos.DrawLine(base.transform.position, wayPoint.transform.position);
		}
	}

	public void OnDrawGizmosSelected()
	{
		Gizmos.color = new Color(Color.blue.r, Color.blue.g, Color.blue.b, 0.4f);
		Gizmos.DrawSphere(base.transform.position, radius);
		Gizmos.color = Color.white;
		WayPoint[] array = edges;
		foreach (WayPoint wayPoint in array)
		{
			if (wayPoint == null)
			{
				Debug.Log(base.gameObject.name);
			}
			Gizmos.DrawLine(base.transform.position, wayPoint.transform.position);
		}
	}
}
