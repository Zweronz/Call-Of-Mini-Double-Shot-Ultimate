using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using UnityEngine;

public class UtilsEx
{
	private static string m_SavePath;

	private static string m_deviceId;

	public static long ServerTime;

	public static string DeviceId
	{
		get
		{
			return SystemInfo.deviceUniqueIdentifier;
		}
	}

	public static string SavePath
	{
		get
		{
			return m_SavePath;
		}
		set
		{
			m_SavePath = value;
		}
	}

	static UtilsEx()
	{
		string dataPath = Application.dataPath;
		dataPath = Application.persistentDataPath;
		dataPath += "/Documents";
		if (!Directory.Exists(dataPath))
		{
			Directory.CreateDirectory(dataPath);
		}
		m_SavePath = dataPath;
	}

	public static bool inEditor()
	{
		string text = Application.platform.ToString();
		if (text.IndexOf("Editor") != -1)
		{
			return true;
		}
		return false;
	}

	public static bool inWindow()
	{
		string text = Application.platform.ToString();
		if (text.IndexOf("Windows") != -1)
		{
			return true;
		}
		return false;
	}

	public static bool IsNumeric(string str)
	{
		//Discarded unreachable code: IL_000e, IL_001b
		try
		{
			long.Parse(str);
			return true;
		}
		catch
		{
			return false;
		}
	}

	public static string[] Split(string str, string separator)
	{
		ArrayList arrayList = new ArrayList();
		int length = separator.Length;
		while (str.IndexOf(separator) != -1)
		{
			int num = str.IndexOf(separator);
			arrayList.Add(str.Substring(0, num));
			str = str.Substring(num + length, str.Length - num - length);
		}
		if (str.Length > 0)
		{
			arrayList.Add(str);
		}
		string[] array = new string[arrayList.Count];
		for (int i = 0; i < arrayList.Count; i++)
		{
			array[i] = arrayList[i].ToString();
		}
		return array;
	}

	public static string TimeLeft(long seconds)
	{
		long num = seconds / 60;
		long num2 = seconds % 60;
		long num3 = num / 60;
		num %= 60;
		long num4 = num3 / 24;
		num3 %= 24;
		int num5 = 0;
		string text = string.Empty;
		if (num4 > 0)
		{
			text = num4 + "d ";
			num5++;
		}
		if (num3 > 0)
		{
			text = text + num3 + "h ";
			num5++;
		}
		if (num > 0 && num5 < 2)
		{
			text = text + num + "m ";
			num5++;
		}
		if (num2 > 0 && num5 < 2)
		{
			text = text + num2 + "s";
			num5++;
		}
		return text;
	}

	public static string TimeLeftFullShow(long seconds)
	{
		long num = seconds / 60;
		long num2 = seconds % 60;
		long num3 = num / 60;
		num %= 60;
		long num4 = num3 / 24;
		num3 %= 24;
		int num5 = 0;
		string text = string.Empty;
		if (num4 > 0)
		{
			num5++;
			text = num4 + " Day" + ((num4 <= 1) ? string.Empty : "s") + ((num3 <= 0 || num5 >= 2) ? string.Empty : ", ");
		}
		if (num3 > 0)
		{
			num5++;
			string text2 = text;
			text = text2 + num3 + " Hour" + ((num3 <= 1) ? string.Empty : "s") + ((num <= 0 || num5 >= 2) ? string.Empty : ", ");
		}
		if (num > 0 && num5 < 2)
		{
			num5++;
			string text2 = text;
			text = text2 + num + " Minute" + ((num <= 1) ? string.Empty : "s") + ((num2 <= 0 || num5 >= 2) ? string.Empty : ", ");
		}
		if (num2 > 0 && num5 < 2)
		{
			num5++;
			string text2 = text;
			text = text2 + num2 + " Second" + ((num2 <= 1) ? string.Empty : "s");
		}
		return text;
	}

	public static string TimeLeftFullShowDHM(long seconds)
	{
		long num = seconds % 60;
		long num2 = seconds / 60;
		long num3 = num2 / 60;
		num2 %= 60;
		long num4 = num3 / 24;
		num3 %= 24;
		int num5 = 0;
		string text = string.Empty;
		if (num4 > 0)
		{
			num5++;
			text = num4 + " Day" + ((num4 <= 1) ? string.Empty : "s") + ((num3 <= 0 || num5 >= 2) ? string.Empty : ", ");
		}
		if (num3 > 0)
		{
			num5++;
			string text2 = text;
			text = text2 + num3 + " Hour" + ((num3 <= 1) ? string.Empty : "s") + ((num2 <= 0) ? string.Empty : ", ");
		}
		if (num2 > 0)
		{
			num5++;
			text = text + num2 + " Min";
		}
		return text;
	}

	public static string MinuteTimeBase(int minutes)
	{
		int num = minutes % 60;
		int num2 = minutes / 60;
		int num3 = num2 / 24;
		num2 %= 24;
		string text = string.Empty;
		if (num3 > 0)
		{
			text = text + num3 + "d ";
		}
		if (num2 > 0)
		{
			text = text + num2 + "h ";
		}
		if (num > 0)
		{
			text = text + num + "m";
		}
		return text;
	}

	public static string TimeToStr_HMS(long seconds)
	{
		long num = seconds / 60;
		long num2 = seconds % 60;
		long num3 = num / 60;
		num %= 60;
		long num4 = num3 / 24;
		num3 %= 24;
		int num5 = 0;
		string empty = string.Empty;
		num5++;
		string text = num3.ToString();
		if (num3 < 10)
		{
			text = "0" + num3;
		}
		empty = empty + text + ":";
		num5++;
		string text2 = num.ToString();
		if (num < 10)
		{
			text2 = "0" + num;
		}
		empty = empty + text2 + ":";
		num5++;
		string text3 = num2.ToString();
		if (num2 < 10)
		{
			text3 = "0" + num2;
		}
		return empty + text3;
	}

	public static Vector2 calcTextSize(string text, UnityEngine.Font tfont)
	{
		GUIStyle gUIStyle = new GUIStyle();
		gUIStyle.font = tfont;
		return gUIStyle.CalcSize(new GUIContent(text));
	}

	public static Vector3 toWorldPosition(Vector3 pos)
	{
		return Camera.main.ScreenPointToRay(pos).GetPoint(20f);
	}

	public static long getNowDateSeconds()
	{
		PlayerPrefs.SetInt("DiffTime", GameClient.serverTime);
		TimeSpan timeSpan = new TimeSpan(DateTime.UtcNow.Ticks - new DateTime(1970, 1, 1, 0, 0, 0).Ticks);
		long num = (long)timeSpan.TotalSeconds;
		return num + PlayerPrefs.GetInt("DiffTime");
	}

	public static DateTime getDateTimeBySeconds(long seconds)
	{
		DateTime result = new DateTime(TimeSpan.FromSeconds(seconds).Ticks + new DateTime(1970, 1, 1, 0, 0, 0).Ticks);
		return result;
	}

	public static DateTime getNextDayDateTime(long nowSeconds, DayOfWeek day)
	{
		int num = Mathf.Abs(day - getDateTimeBySeconds(nowSeconds).DayOfWeek);
		DateTime dateTimeBySeconds = getDateTimeBySeconds(nowSeconds + num * 24 * 60 * 60);
		return new DateTime(dateTimeBySeconds.Year, dateTimeBySeconds.Month, dateTimeBySeconds.Day, 0, 0, 0);
	}

	public static long getNowDateMinutes()
	{
		return (long)new TimeSpan(DateTime.Now.ToUniversalTime().Ticks).TotalMinutes;
	}

	public static long getNowIntervalSeconds()
	{
		TimeSpan timeSpan = new TimeSpan(DateTime.UtcNow.Ticks - new DateTime(1970, 1, 1, 0, 0, 0).Ticks);
		return (long)timeSpan.TotalSeconds;
	}

	public static long getNowIntervalMinutes()
	{
		TimeSpan timeSpan = new TimeSpan(DateTime.UtcNow.Ticks - new DateTime(1970, 1, 1, 0, 0, 0).Ticks);
		return (long)timeSpan.TotalMinutes;
	}

	public static string CurrentMethod(StackTrace st)
	{
		if (st.FrameCount > 0)
		{
			StackFrame frame = st.GetFrame(0);
			return frame.GetMethod().Name;
		}
		return string.Empty;
	}

	public static float AngleToRadian(float angle)
	{
		return (float)Math.PI / 180f * angle;
	}

	public static float RadianToAngle(float radian)
	{
		return radian * (float)Math.PI / 180f;
	}

	public static bool IsNumberLetter(string input)
	{
		bool result = true;
		for (int i = 0; i < input.Length; i++)
		{
			int num = Convert.ToInt32(Convert.ToChar(input.Substring(i, 1)));
			if ((num < 48 || num > 57) && (num < 65 || num > 90) && (num < 97 || num > 122))
			{
				result = false;
				break;
			}
		}
		return result;
	}

	public static bool IsChineseLetter(string input)
	{
		for (int i = 0; i < input.Length; i++)
		{
			int num = Convert.ToInt32(Convert.ToChar(input.Substring(i, 1)));
			if (num >= 128)
			{
				return true;
			}
		}
		return false;
	}

	public static Vector3 cameraRoundPos()
	{
		Vector3 position = Camera.main.transform.position;
		return new Vector3(UnityEngine.Random.Range(position.x - 5f, position.x + 5f), 10f, position.x - 10f);
	}

	public static void OpenWebSite(string url)
	{
	}

	public static bool IsNetworkConnected()
	{
		return true;
	}

	public static bool OSIsJailBreak()
	{
		return true;
	}
}
