using TNetSdk;
using UnityEngine;

public class SmartFoxConnection : MonoBehaviour
{
	private static SmartFoxConnection mInstance;

	private static TNetObject smartFox;

	public static TNetObject Connection
	{
		get
		{
			if (mInstance == null)
			{
				mInstance = new GameObject("SmartFoxConnection").AddComponent(typeof(SmartFoxConnection)) as SmartFoxConnection;
			}
			return smartFox;
		}
		set
		{
			if (mInstance == null)
			{
				mInstance = new GameObject("SmartFoxConnection").AddComponent(typeof(SmartFoxConnection)) as SmartFoxConnection;
			}
			smartFox = value;
		}
	}

	public static bool IsInitialized
	{
		get
		{
			return smartFox != null;
		}
	}

	public static void DisConnect()
	{
		if (smartFox != null)
		{
			smartFox.Close();
			Reset();
		}
	}

	public static void Reset()
	{
		smartFox = null;
	}

	private void OnApplicationQuit()
	{
		if (smartFox != null && smartFox.GetStatus() != TNetObject.STATUS.kClosed)
		{
			DisConnect();
		}
	}
}
