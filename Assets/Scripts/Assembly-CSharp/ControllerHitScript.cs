using UnityEngine;

public class ControllerHitScript : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnControllerColliderHit(ControllerColliderHit hit)
	{
		if (hit.collider.gameObject.layer == 22)
		{
			Debug.Log(hit.collider.gameObject.name);
		}
		else
		{
			Debug.Log(hit.collider.gameObject.name);
		}
	}
}
