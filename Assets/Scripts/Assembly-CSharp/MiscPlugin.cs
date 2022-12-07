using System.IO;
using UnityEngine;

public class MiscPlugin
{
	public static void TakePhoto(string photo_key)
	{
		try
		{
			Texture2D texture2D = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
			texture2D.ReadPixels(new Rect(0f, 0f, Screen.width, Screen.height), 0, 0);
			texture2D.Apply();
			byte[] buffer = texture2D.EncodeToPNG();
			string path = Utils.SavePath() + "/Avatar/" + photo_key + "_photo.png";
			FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write);
			BinaryWriter binaryWriter = new BinaryWriter(fileStream);
			binaryWriter.Write(buffer);
			binaryWriter.Close();
			fileStream.Close();
			Object.Destroy(texture2D);
		}
		catch
		{
			Debug.Log("AvataTakePhotoWin32 error");
		}
	}

	public static bool CheckOSIsIAPCrack()
	{
		return false;
	}

	public static bool CheckOSIsJailbreak()
	{
		return false;
	}

	public static void ToSendMail(string address, string subject, string content)
	{
	}

	public static int ShowMessageBox1(string title, string message, string button)
	{
		return 0;
	}

	public static int ShowMessageBox2(string title, string message, string button1, string button2)
	{
		return 0;
	}

	public static string GetPlayerDataEncryptKey(string input_check_key)
	{
		return "[V@F#G@UL].#980>";
	}

	public static void ScheduleOfflineNotification(long timeFromNowOn, string message)
	{
	}

	public static void CancelLocalNotification()
	{
	}

	public static bool IsOS5Up()
	{
		return true;
	}

	public static bool IsOS3_2Up()
	{
		return true;
	}

	public static bool IsOS4_1Up()
	{
		return true;
	}

	public static int GetActiveSignal()
	{
		return 0;
	}

	public static string GetMacAddr()
	{
		return "000000000000";
	}
}
