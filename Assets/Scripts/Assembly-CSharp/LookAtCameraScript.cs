using UnityEngine;

public class LookAtCameraScript : MonoBehaviour
{
	protected Transform cameraTransform;

	protected float lastUpdateTime;

	private void Start()
	{
		cameraTransform = Camera.main.transform;
	}

	private void Update()
	{
		if (!(Time.time - lastUpdateTime < 0.001f))
		{
			lastUpdateTime = Time.time;
			base.transform.LookAt(cameraTransform);
		}
	}
}
