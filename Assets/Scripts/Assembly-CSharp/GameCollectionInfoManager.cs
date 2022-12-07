using System;
using System.IO;
using UnityEngine;

public class GameCollectionInfoManager : MonoBehaviour
{
	private const int m_SendCounterMax = 60;

	private GameCollectionInfo m_GameCollectionInfo;

	private int m_CurDays;

	private int m_LastSendCounter;

	private static GameCollectionInfoManager m_Instance;

	public static GameCollectionInfoManager Instance()
	{
		if (m_Instance == null)
		{
			GameObject gameObject = new GameObject("GameCollectionInfoManager");
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
			m_Instance = gameObject.AddComponent<GameCollectionInfoManager>();
			m_Instance.Init();
		}
		return m_Instance;
	}

	public GameCollectionInfo GetCurrentInfo()
	{
		return m_GameCollectionInfo;
	}

	public void Init()
	{
		m_GameCollectionInfo = new GameCollectionInfo();
		m_LastSendCounter = 1;
		m_CurDays = new TimeSpan(DateTime.Now.Ticks).Days;
		string text = Utils.SavePath();
		string text2 = text + "/" + m_CurDays;
		if (File.Exists(text2))
		{
			m_GameCollectionInfo.LoadFromFile(text2);
		}
	}

	public void Send()
	{
		GameCollectionInfo gameCollectionInfo = new GameCollectionInfo();
		string empty = string.Empty;
		while (m_LastSendCounter < 60)
		{
			string text = Utils.SavePath();
			string text2 = text + "/" + (m_CurDays - m_LastSendCounter);
			if (File.Exists(text2))
			{
				gameCollectionInfo.LoadFromFile(text2);
				empty = gameCollectionInfo.ToJsonString();
				if (!string.IsNullOrEmpty(empty))
				{
					GameClient.SendDailyCollectionInfo(empty);
					break;
				}
				File.Delete(text2);
			}
			m_LastSendCounter++;
		}
	}

	public void Update()
	{
		if (m_LastSendCounter >= 60)
		{
			return;
		}
		switch (GameClient.prop.GetInt("SendDailyCollectionInfo_Status", -1))
		{
		case 1:
		{
			string text = Utils.SavePath();
			string path = text + "/" + (m_CurDays - m_LastSendCounter);
			if (File.Exists(path))
			{
				File.Delete(path);
			}
			if (m_LastSendCounter < 60)
			{
				m_LastSendCounter++;
				Send();
			}
			break;
		}
		case 2:
			if (m_LastSendCounter < 60)
			{
				m_LastSendCounter++;
				Send();
			}
			break;
		}
	}
}
