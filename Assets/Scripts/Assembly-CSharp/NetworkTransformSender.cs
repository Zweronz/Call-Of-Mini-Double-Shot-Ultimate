using UnityEngine;

public class NetworkTransformSender : MonoBehaviour
{
	public static readonly float sendingPeriod = 0.2f;

	private readonly float accuracy = 0.002f;

	private float timeLastSending;

	private bool send;

	private NetworkTransform lastState;

	private Transform thisTransform;

	private void Start()
	{
		thisTransform = base.transform;
		lastState = NetworkTransform.FromTransform(thisTransform);
	}

	public void StartSendTransform()
	{
		send = true;
	}

	public void StopSendTransform()
	{
		send = false;
	}

	private void FixedUpdate()
	{
		if (send)
		{
			SendTransform();
		}
	}

	private void SendTransform()
	{
		if (SmartFoxConnection.Connection != null && SmartFoxConnection.Connection.TimeManager != null)
		{
			if (timeLastSending >= sendingPeriod)
			{
				lastState = NetworkTransform.FromTransform(thisTransform);
				lastState.TimeStamp = SmartFoxConnection.Connection.TimeManager.NetworkTime;
				GameSetup.Instance.SendTransform(lastState, GameSetup.Instance.GetObjID(base.gameObject));
				timeLastSending = 0f;
			}
			else
			{
				timeLastSending += Time.deltaTime;
			}
		}
	}
}
