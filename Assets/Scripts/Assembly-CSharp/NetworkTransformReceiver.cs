using UnityEngine;

public class NetworkTransformReceiver : MonoBehaviour
{
	private Transform thisTransform;

	private NetworkTransformInterpolation interpolator;

	private void Awake()
	{
		thisTransform = base.transform;
		interpolator = GetComponent<NetworkTransformInterpolation>();
		if (interpolator != null)
		{
			interpolator.StartReceiving();
		}
	}

	public void ReceiveTransform(NetworkTransform ntransform)
	{
		if (interpolator == null)
		{
			Awake();
		}
		if (interpolator != null)
		{
			interpolator.ReceivedTransform(ntransform);
			return;
		}
		thisTransform.position = ntransform.Position;
		thisTransform.localEulerAngles = ntransform.AngleRotationFPS;
	}
}
