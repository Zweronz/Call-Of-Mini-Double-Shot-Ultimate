using UnityEngine;

public class ScreenInfoMsg : MonoBehaviour
{
	private Rect[] m_rLabel = new Rect[6]
	{
		new Rect(10f, 0f, 960f, 20f),
		new Rect(10f, 25f, 960f, 20f),
		new Rect(10f, 50f, 960f, 20f),
		new Rect(10f, 75f, 960f, 20f),
		new Rect(10f, 100f, 960f, 20f),
		new Rect(10f, 125f, 960f, 20f)
	};

	private void OnGUI()
	{
		if (SmartFoxConnection.IsInitialized && SmartFoxConnection.Connection != null)
		{
			GUILayout.Label("Ping:" + SmartFoxConnection.Connection.TimeManager.AveragePing);
		}
	}
}
