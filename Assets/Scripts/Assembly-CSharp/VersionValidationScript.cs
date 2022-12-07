using System;
using System.Collections;
using System.Text;
using UnityEngine;

public class VersionValidationScript : MonoBehaviour
{
	private string url = "http://account.trinitigame.com/game/CoMDS/CoMDSAndroid.txt";

	public string m_strVersion = "2.0.2";

	public string m_strServerIP = "203.124.98.26";

	public string m_strServerPort = "7001";

	public string m_strZone = "CoMDS";

	public HandlerEvent_VesionCheck m_DownLoadErrorEvent;

	public HandlerEvent_VesionCheckOK m_DownLoadOKEvent;

	public IEnumerator Start()
	{
		WWW www = new WWW(url);
		yield return www;
		if (www.error != null)
		{
			Debug.Log(www.error);
			if (m_DownLoadErrorEvent != null)
			{
				m_DownLoadErrorEvent();
			}
			if (base.gameObject != null)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}
		LoadData(www.text);
		if (m_DownLoadOKEvent != null)
		{
			m_DownLoadOKEvent(m_strVersion, m_strServerIP, m_strServerPort, m_strZone);
		}
		if (base.gameObject != null)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	private void LoadData(string input)
	{
		Debug.Log("|" + input + "|");
		string text = string.Empty;
		try
		{
			text = Decrypt(input);
		}
		catch (Exception)
		{
		}
		if (!(text != string.Empty))
		{
			return;
		}
		string[] array = text.Split('\r', '\n');
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i] == null)
			{
				continue;
			}
			try
			{
				string[] array2 = array[i].Split('\t');
				if (array2.Length >= 2)
				{
					switch (array2[0])
					{
					case "Version":
						m_strVersion = array2[1];
						break;
					case "ServerIP":
						m_strServerIP = array2[1];
						break;
					case "ServerPort":
						m_strServerPort = array2[1];
						break;
					case "ZoneName":
						m_strZone = array2[1];
						break;
					}
				}
			}
			catch
			{
				Debug.Log("GameState.LoadData() Exception!!!");
			}
		}
	}

	private string Decrypt(string input_data)
	{
		string empty = string.Empty;
		byte[] data = Convert.FromBase64String(input_data);
		string playerDataEncryptKey = MiscPlugin.GetPlayerDataEncryptKey("ME_2_@_YOU_DD");
		byte[] bytes = XXTEAUtils.Decrypt(data, Encoding.ASCII.GetBytes(playerDataEncryptKey));
		return Encoding.UTF8.GetString(bytes);
	}
}
