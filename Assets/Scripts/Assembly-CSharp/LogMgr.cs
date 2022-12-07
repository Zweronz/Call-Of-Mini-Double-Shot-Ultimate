using System;
using System.Collections.Generic;
using UnityEngine;

public class LogMgr
{
	private static List<string> m_lLogInfo;

	private static string m_strLogTitle;

	private static string m_strLogName;

	static LogMgr()
	{
		m_lLogInfo = new List<string>();
		m_strLogTitle = "[Log]";
		m_strLogName = string.Empty;
		m_strLogName += m_strLogTitle;
		m_strLogName += GetNowTime(true);
		m_strLogName = m_strLogName.Replace(':', '-');
		m_strLogName += ".txt";
		Debug.Log("LogFile Name: " + m_strLogName);
	}

	public static void AddLog(string log, bool IsShowType)
	{
		if (m_lLogInfo != null)
		{
			string text = GetNowTime(false) + ":\t";
			if (IsShowType)
			{
				m_lLogInfo.Add(log);
			}
			text += log;
			text += "\r\n";
			Utils.FileWriteSaveString(m_strLogName, text);
		}
	}

	public static List<string> GetLogInfoList()
	{
		return m_lLogInfo;
	}

	public static string GetNowTime(bool IsGetHMS)
	{
		DateTime dateTime = default(DateTime);
		dateTime = DateTime.Now;
		if (IsGetHMS)
		{
			return dateTime.ToLongTimeString().ToString();
		}
		return dateTime.ToString();
	}
}
