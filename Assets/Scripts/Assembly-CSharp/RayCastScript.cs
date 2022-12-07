using UnityEngine;

public class RayCastScript : MonoBehaviour
{
	public float life;

	public Vector3 beginPos;

	public Vector3 endPos;

	public Vector3 beginPtDir;

	private void Start()
	{
	}

	private void Update()
	{
		Debug.DrawLine(beginPos, endPos, Color.white);
		Debug.DrawRay(beginPos, beginPtDir, Color.yellow);
	}

	private void OnDrawGizmos()
	{
		GameObject gameObject = GameObject.Find("Begin");
		GameObject gameObject2 = GameObject.Find("End");
		Ray ray = new Ray(gameObject.transform.position, gameObject.transform.position - gameObject.transform.position);
		RaycastHit hitInfo;
		if (Physics.Raycast(ray, out hitInfo, 100f, 2048))
		{
			Gizmos.DrawLine(gameObject.transform.position, gameObject2.transform.position);
		}
	}
}
