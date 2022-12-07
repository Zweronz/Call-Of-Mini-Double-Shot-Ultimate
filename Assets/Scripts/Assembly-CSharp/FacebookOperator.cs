using System.Collections;
using UnityEngine;

public class FacebookOperator
{
	public static PropUtils prop = new PropUtils();

	private static float facebookLoginTimeOut = 60f;

	public static void Login()
	{
		prop.SetProp("FacebookAction", "Login");
		FBWrapper.Login();
	}

	public static void Resume()
	{
		prop.SetProp("FacebookAction", "ResumeSession");
		FBWrapper.ResumeSession();
	}

	public static void FacebookStatus()
	{
		Debug.Log("## begin facebook status");
		prop.SetProp("FBLoginTTime", 0f);
		prop.SetProp("FacebookLoginStatus", 0);
		RunCallback runCallback = CallBackManager.Instance().CreateCallBack("FacebookStatusMethod", FacebookStatusPeriod, FacebookStatusStop, string.Empty);
		runCallback.TimeOut = facebookLoginTimeOut + 10f;
	}

	public static void FacebookTimeout()
	{
		Debug.Log("FacebookTimeout()");
		FBWrapper.LoginTimeout();
	}

	public static void FacebookStatusPeriod(object stateInfo)
	{
		float num = prop.GetFloat("FBLoginTTime", 0f) + Time.deltaTime;
		prop.SetProp("FBLoginTTime", num);
		int num2 = FBWrapper.Status();
		Debug.Log("FacebookOperator - " + num2);
		switch (num2)
		{
		case 0:
			if (num >= facebookLoginTimeOut)
			{
				Debug.Log("FacebookOperator.TIMEOUT");
				prop.SetProp("FacebookLoginStatus", 3);
				prop.SetProp("FBLoginTTime", 0f);
				FacebookTimeout();
				RunCallback callBack5 = CallBackManager.Instance().GetCallBack("FacebookStatusMethod");
				callBack5.StopNow();
			}
			break;
		case 1:
		{
			Debug.Log("FBConnectState.Done ------------------------------------------------");
			string id = string.Empty;
			string name = string.Empty;
			if (name == string.Empty)
			{
				FBWrapper.GetInfo(ref id, ref name);
			}
			if (name.Trim() == string.Empty)
			{
				RunCallback callBack2 = CallBackManager.Instance().GetCallBack("FacebookStatusMethod");
				callBack2.CallbackTime = 1.5f;
				break;
			}
			if (Utils.IsChineseLetter(name))
			{
				name = id;
			}
			if (FBWrapper.BuddyStatus() == 0)
			{
				RunCallback callBack3 = CallBackManager.Instance().GetCallBack("FacebookStatusMethod");
				callBack3.CallbackTime = 1f;
				prop.SetProp("CalFriendsTimes", prop.GetInt("CalFriendsTimes", 0) + 1);
				break;
			}
			prop.SetProp("FacebookId", id);
			prop.SetProp("FacebookName", name);
			prop.SetProp("FacebookLoginStatus", num2);
			Debug.Log("FriendInfo ------------------------------------------------");
			ArrayList arrayList = new ArrayList();
			for (int i = 0; i < FBWrapper.GetFriendCount(); i++)
			{
				string id2 = string.Empty;
				string name2 = string.Empty;
				FBWrapper.GetFriendInfo(i, ref id2, ref name2);
				arrayList.Add(id2);
				Debug.Log(id2);
			}
			prop.SetProp("FacebookFriends", arrayList);
			Debug.Log("FriendInfo ------------------------------------------------");
			RunCallback callBack4 = CallBackManager.Instance().GetCallBack("FacebookStatusMethod");
			callBack4.StopNow();
			Debug.Log("## FB login ok.... ##");
			if (prop.GetString("FacebookAction") == "Login")
			{
				Debug.Log("## FB reday to get back data.... ##");
			}
			else if (prop.GetString("FacebookAction") == "ResumeSession" && PlayerPrefs.GetInt("Permission_publish_stream") != 0)
			{
			}
			break;
		}
		case 2:
		case 3:
		{
			Debug.Log("## fb login cancel or error. - " + num2);
			RunCallback callBack = CallBackManager.Instance().GetCallBack("FacebookStatusMethod");
			callBack.StopNow();
			prop.SetProp("FacebookLoginStatus", num2);
			if (!(prop.GetString("FacebookAction") == "ResumeSession"))
			{
			}
			break;
		}
		}
	}

	public static void FacebookStatusStop(object stateInfo)
	{
	}
}
