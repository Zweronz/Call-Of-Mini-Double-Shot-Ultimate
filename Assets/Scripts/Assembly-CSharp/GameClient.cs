using System;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using Trinitigame.Http;
using UnityEngine;
using Zombie3D;

public class GameClient
{
	public enum GetServerDataStatus
	{
		Loading = 0,
		Done = 1,
		Error = 2
	}

	private class GetUserFriendList_Result
	{
		public int code;

		public List<Bindingbuddy> bindingbuddylist;
	}

	public class Bindingbuddy
	{
		public string gamecenterid;

		public string uuid;

		public string facebookid;
	}

	private class GetHireUserList_Result
	{
		public int code;

		public List<HireUserInfo> datas;
	}

	public class HireUserInfo
	{
		public string uuid;

		public int level;

		public int money;
	}

	public static PropUtils prop = new PropUtils();

	public static List<Bindingbuddy> friendListResult = new List<Bindingbuddy>();

	public static List<HireUserInfo> HireFriendListResult = new List<HireUserInfo>();

	public static int serverTime = 0;

	public static void Login(string device_id, string nick_name)
	{
		prop.SetProp("LoginStatus", 0);
		Hashtable hashtable = new Hashtable();
		hashtable["deviceid"] = device_id;
		hashtable["nickname"] = nick_name;
		hashtable["appname"] = "callofminidoubleshot";
		string text = JsonMapper.ToJson(hashtable);
		HttpWrapper.SendJsonToPlatformServer(stopLogin, "Login", text);
		Debug.Log("Login - " + text + "|" + Time.time);
	}

	public static void stopLogin(bool bError, string message)
	{
		if (bError)
		{
			Debug.Log("ERROR - can't login ....");
			return;
		}
		int num = 1;
		int num2 = 0;
		if (num > 0)
		{
			if (!string.IsNullOrEmpty(message))
			{
				try
				{
					Debug.Log("Login OK : ---- Data - " + message);
					JsonData jsonData = JsonMapper.ToObject(message);
					if (((IDictionary)jsonData).Contains((object)"code"))
					{
						int num3 = int.Parse(jsonData["code"].ToString());
						if (num3 == 0)
						{
							GameApp.GetInstance().GetGameState().UUID = (string)jsonData["uuid"];
							prop.SetProp("LoginStatus", 1);
						}
						else
						{
							Debug.Log("ERROR - stopLogin() | " + num + "|" + num3);
							prop.SetProp("LoginStatus", 2);
						}
					}
					return;
				}
				catch (Exception)
				{
					Debug.Log("Exception: Json");
					return;
				}
			}
			Debug.Log("ERROR - can't login ....");
			prop.SetProp("LoginStatus", 2);
		}
		else
		{
			Debug.Log("ERROR - stopLogin() | " + num + "|" + num2);
			prop.SetProp("LoginStatus", 2);
		}
	}

	public static void endLogin(object stateInfo)
	{
		CubeAppData.AccountDataSendJsonToPlatform accountDataSendJsonToPlatform = (CubeAppData.AccountDataSendJsonToPlatform)stateInfo;
		if (accountDataSendJsonToPlatform.Status() < 0 || accountDataSendJsonToPlatform.Status() > 0)
		{
			RunCallback callBack = CallBackManager.Instance().GetCallBack("LoginMethod");
			callBack.StopNow();
		}
	}

	public static void GetAppVersion()
	{
		prop.SetProp("GetAppVersionStatus", 0);
		Hashtable hashtable = new Hashtable();
		hashtable["deviceid"] = GameApp.GetInstance().GetGameState().DeviceID;
		hashtable["uuid"] = GameApp.GetInstance().GetGameState().UUID;
		hashtable["version"] = "2.02";
		string text = JsonMapper.ToJson(hashtable);
		Debug.Log(text + " | " + Time.time);
		HttpWrapper.SendJsonToGameServer(stopGetAppVersion, "JudgementGetGameVersion", text);
	}

	public static void stopGetAppVersion(bool bError, string message)
	{
		if (bError)
		{
			Debug.Log("ERROR - can't get app version ....");
			return;
		}
		int num = 1;
		int num2 = 0;
		if (num > 0)
		{
			Debug.Log("stopGetAppVersion() - " + message);
			if (!string.IsNullOrEmpty(message))
			{
				prop.SetProp("GetAppVersionStatus", 1);
				JsonData jsonData = JsonMapper.ToObject(message);
				try
				{
					if (((IDictionary)jsonData).Contains((object)"code"))
					{
						int num3 = int.Parse(jsonData["code"].ToString());
						if (num3 == 0)
						{
							string text = (string)jsonData["version"];
							long num4 = (long)jsonData["servertime"] / 1000;
							long nowIntervalSeconds = UtilsEx.getNowIntervalSeconds();
							num4 = (int)(num4 - nowIntervalSeconds);
							GameApp.GetInstance().GetGameState().m_bCheckDataTimeOK = true;
							Debug.LogWarning("GetAppVersion: " + text);
							prop.SetProp("ServerAppVersion", text);
						}
						else
						{
							Debug.Log("ERROR - stopGetAppVersion() - " + num + "|" + num3 + " | " + Time.time);
							prop.SetProp("GetAppVersionStatus", 2);
						}
					}
					return;
				}
				catch (Exception)
				{
					prop.SetProp("ServerAppVersion", "2.02");
					Debug.Log("Exception: Json");
					return;
				}
			}
			Debug.Log("ERROR - can't get app version....");
			prop.SetProp("GetAppVersionStatus", 2);
		}
		else
		{
			Debug.Log("ERROR - stopGetAppVersion() - " + num + "|" + num2 + " | " + Time.time);
			prop.SetProp("GetAppVersionStatus", 2);
		}
	}

	public static void endGetAppVersion(object stateInfo)
	{
		CubeAppData.AppDataSendJson appDataSendJson = (CubeAppData.AppDataSendJson)stateInfo;
		if (appDataSendJson.Status() < 0 || appDataSendJson.Status() > 0)
		{
			RunCallback callBack = CallBackManager.Instance().GetCallBack("GetAppVersionMethod");
			callBack.StopNow();
		}
	}

	public static void GetUserData()
	{
		prop.SetProp("GetUserDataStatus", 0);
		Hashtable hashtable = new Hashtable();
		hashtable["deviceid"] = GameApp.GetInstance().GetGameState().DeviceID;
		hashtable["uuid"] = GameApp.GetInstance().GetGameState().UUID;
		hashtable["nickname"] = string.Empty;
		string jsonText = JsonMapper.ToJson(hashtable);
		HttpWrapper.SendJsonToGameServer(stopGetUserData, "JudgementGetUserData", jsonText);
	}

	public static void stopGetUserData(bool bError, string message)
	{
		if (bError)
		{
			Debug.Log("ERROR - can't get user data....");
			return;
		}
		int num = 1;
		int num2 = 0;
		if (num > 0)
		{
			Debug.Log("stopGetUserData - " + message);
			if (!string.IsNullOrEmpty(message))
			{
				JsonData jsonData = JsonMapper.ToObject(message);
				prop.SetProp("GetUserDataStatus", 1);
				try
				{
					if (!((IDictionary)jsonData).Contains((object)"code"))
					{
						return;
					}
					int num3 = int.Parse(jsonData["code"].ToString());
					if (num3 == 0)
					{
						string text = (string)jsonData["data"];
						int num4 = (int)jsonData["externexp"];
						int num5 = 0;
						if (((IDictionary)jsonData).Contains((object)"mmoney"))
						{
							num5 = (int)jsonData["mmoney"];
						}
						prop.SetProp("UserData", text);
						prop.SetProp("UserExternExp", num4);
						prop.SetProp("UserHiredMoney", num5);
						Debug.Log("Loading Player data OK - " + text);
						Debug.Log("Player ExternExp: " + num4);
					}
					else
					{
						Debug.Log("ERROR - stopGetUserData() - " + num + "|" + num3);
						prop.SetProp("GetUserDataStatus", 2);
					}
					prop.SetProp("GetUserDataCode", num3);
					return;
				}
				catch (Exception)
				{
					Debug.Log("Exception: Json");
					return;
				}
			}
			Debug.Log("ERROR - can't get user data....");
			prop.SetProp("GetUserDataStatus", 2);
		}
		else
		{
			Debug.Log("ERROR - stopGetUserData() - " + num + "|" + num2);
			prop.SetProp("GetUserDataStatus", 2);
		}
	}

	public static void endGetUserData(object stateInfo)
	{
		CubeAppData.AppDataSendJson appDataSendJson = (CubeAppData.AppDataSendJson)stateInfo;
		if (appDataSendJson.Status() < 0 || appDataSendJson.Status() > 0)
		{
			RunCallback callBack = CallBackManager.Instance().GetCallBack("GetUserDataMethod");
			callBack.StopNow();
		}
	}

	public static void SetUserData()
	{
		Hashtable hashtable = new Hashtable();
		hashtable["deviceid"] = GameApp.GetInstance().GetGameState().DeviceID;
		hashtable["uid"] = "0";
		hashtable["uuid"] = GameApp.GetInstance().GetGameState().UUID;
		hashtable["nickname"] = string.Empty;
		hashtable["data"] = GameApp.GetInstance().GetGameState().GetDataToString();
		string jsonText = JsonMapper.ToJson(hashtable);
		HttpWrapper.SendJsonToGameServer(stopSetUserData, "JudgementSetUserData", jsonText);
	}

	public static void stopSetUserData(bool bError, string message)
	{
		if (bError)
		{
			Debug.Log("ERROR - can't get user data....");
			return;
		}
		int num = 1;
		int num2 = 0;
		if (num > 0)
		{
			if (string.IsNullOrEmpty(message))
			{
				Debug.Log("ERROR - can't get user data....");
				return;
			}
			JsonData jsonData = JsonMapper.ToObject(message);
			Debug.Log("Set User Data OK!!!~~");
		}
		else
		{
			Debug.Log("ERROR - stopSetUserData() - " + num + "|" + num2);
		}
	}

	public static void endSetUserData(object stateInfo)
	{
		CubeAppData.AppDataSendJson appDataSendJson = (CubeAppData.AppDataSendJson)stateInfo;
		if (appDataSendJson.Status() < 0 || appDataSendJson.Status() > 0)
		{
			RunCallback callBack = CallBackManager.Instance().GetCallBack("SetUserDataMethod");
			callBack.StopNow();
		}
	}

	public static void GetUserExternExp()
	{
		Hashtable hashtable = new Hashtable();
		hashtable["deviceid"] = GameApp.GetInstance().GetGameState().DeviceID;
		hashtable["uuid"] = GameApp.GetInstance().GetGameState().UUID;
		hashtable["nickname"] = string.Empty;
		string jsonText = JsonMapper.ToJson(hashtable);
		HttpWrapper.SendJsonToGameServer(stopGetUserExternExp, "JudgementGetUserExternExp", jsonText);
	}

	public static void stopGetUserExternExp(bool bError, string message)
	{
		if (bError)
		{
			Debug.Log("ERROR - can't get user extern exp....");
			return;
		}
		int num = 1;
		int num2 = 0;
		if (num > 0)
		{
			if (!string.IsNullOrEmpty(message))
			{
				JsonData jsonData = JsonMapper.ToObject(message);
				try
				{
					if (((IDictionary)jsonData).Contains((object)"code") && int.Parse(jsonData["code"].ToString()) == 0)
					{
						string text = (string)jsonData["data"];
					}
					GameApp.GetInstance().GetGameState().ExternExp = 1;
					return;
				}
				catch (Exception)
				{
					Debug.Log("Exception: Json");
					return;
				}
			}
			Debug.Log("ERROR - can't get user extern exp....");
		}
		else
		{
			Debug.Log("ERROR - stopGetUserExternExp() - " + num + "|" + num2);
		}
	}

	public static void endGetUserExternExp(object stateInfo)
	{
		CubeAppData.AppDataSendJson appDataSendJson = (CubeAppData.AppDataSendJson)stateInfo;
		if (appDataSendJson.Status() < 0 || appDataSendJson.Status() > 0)
		{
			RunCallback callBack = CallBackManager.Instance().GetCallBack("GetUserExternExpMethod");
			callBack.StopNow();
		}
	}

	public static void SetFriendUserExternExp(int extern_exp, string friend_uuid, string friend_deviceId)
	{
		Hashtable hashtable = new Hashtable();
		hashtable["deviceid"] = friend_deviceId;
		hashtable["uuid"] = friend_uuid;
		hashtable["externexp"] = extern_exp;
		string jsonText = JsonMapper.ToJson(hashtable);
		HttpWrapper.SendJsonToGameServer(stopSetFriendUserExternExp, "JudgementSetExternExp", jsonText);
	}

	public static void stopSetFriendUserExternExp(bool bError, string message)
	{
		if (bError)
		{
			Debug.Log("ERROR - can't set friend user Extern Exp....");
			return;
		}
		int num = 1;
		int num2 = 0;
		if (num > 0)
		{
			if (!string.IsNullOrEmpty(message))
			{
				JsonData jsonData = JsonMapper.ToObject(message);
				try
				{
					if (((IDictionary)jsonData).Contains((object)"code"))
					{
						int num3 = int.Parse(jsonData["code"].ToString());
						if (num3 == 0)
						{
							string text = (string)jsonData["uuid"];
							Debug.Log("Set Friend Extern Exp OK - " + num3 + "|" + text);
							Debug.Log("Set Friend User Extern Exp OK!!!~~");
						}
						else
						{
							Debug.Log("ERROR - stopSetFriendUserExternExp() - " + num + "|" + num2);
						}
					}
					return;
				}
				catch (Exception)
				{
					Debug.Log("Exception: Json");
					return;
				}
			}
			Debug.Log("ERROR - can't set friend user Extern Exp....");
		}
		else
		{
			Debug.Log("ERROR - stopSetFriendUserExternExp() - " + num + "|" + num2);
		}
	}

	public static void endSetFriendUserExternExp(object stateInfo)
	{
		CubeAppData.AppDataSendJson appDataSendJson = (CubeAppData.AppDataSendJson)stateInfo;
		if (appDataSendJson.Status() < 0 || appDataSendJson.Status() > 0)
		{
			RunCallback callBack = CallBackManager.Instance().GetCallBack("SetFriendUserExternExpMethod");
			callBack.StopNow();
		}
	}

	public static void GetFriendUserData(FriendUserData friend_user_data)
	{
		Debug.Log("GetFriendUserData() - " + friend_user_data.m_Name);
		prop.SetProp("GetFriendUserDataStatus", 0);
		Hashtable hashtable = new Hashtable();
		hashtable["deviceid"] = friend_user_data.m_DeviceId;
		hashtable["uuid"] = friend_user_data.m_UUID;
		hashtable["nickname"] = string.Empty;
		string jsonText = JsonMapper.ToJson(hashtable);
		HttpWrapper.SendJsonToGameServer(stopGetFriendUserData, "JudgementGetUserData", jsonText);
	}

	public static void stopGetFriendUserData(bool bError, string message)
	{
		if (bError)
		{
			Debug.Log("ERROR - can't get friend user data....");
			return;
		}
		int num = 1;
		int num2 = 0;
		if (num > 0)
		{
			if (!string.IsNullOrEmpty(message))
			{
				Debug.Log("stopGetFriendUserData()");
				prop.SetProp("GetFriendUserDataStatus", 1);
				JsonData jsonData = JsonMapper.ToObject(message);
				try
				{
					if (((IDictionary)jsonData).Contains((object)"code"))
					{
						int num3 = int.Parse(jsonData["code"].ToString());
						if (num3 == 0)
						{
							string text = (string)jsonData["data"];
							Debug.Log("stopGetFriendUserData : " + text);
							prop.SetProp("GetFriendUserData_Data", text);
						}
						else
						{
							Debug.Log("stopGetFriendUserData: Code " + num3);
							prop.SetProp("GetFriendUserData_Data", string.Empty);
						}
					}
					return;
				}
				catch (Exception)
				{
					Debug.Log("Exception: Json");
					return;
				}
			}
			Debug.Log("ERROR - can't get friend user data....");
			prop.SetProp("GetFriendUserDataStatus", 2);
		}
		else
		{
			Debug.Log("ERROR - stopGetFriendUserData() - " + num + "|" + num2);
			prop.SetProp("GetFriendUserDataStatus", 2);
		}
	}

	public static void endGetFriendUserData(object stateInfo)
	{
		CubeAppData.AppDataSendJson appDataSendJson = (CubeAppData.AppDataSendJson)stateInfo;
		if (appDataSendJson.Status() < 0 || appDataSendJson.Status() > 0)
		{
			RunCallback callBack = CallBackManager.Instance().GetCallBack("GetFriendUserDataMethod");
			callBack.StopNow();
		}
	}

	public static void GetUserFacebookFriendListInServer()
	{
		Hashtable hashtable = new Hashtable();
		hashtable["deviceid"] = GameApp.GetInstance().GetGameState().DeviceID;
		hashtable["uuid"] = GameApp.GetInstance().GetGameState().UUID;
		hashtable["nickname"] = string.Empty;
		string jsonText = JsonMapper.ToJson(hashtable);
		HttpWrapper.SendJsonToGameServer(stopGetUserFacebookFriendListInServer, "JudgementGetUserFacebookFriendListInServer", jsonText);
	}

	public static void stopGetUserFacebookFriendListInServer(bool bError, string message)
	{
		if (bError)
		{
			Debug.Log("ERROR - can't get user facebook friend list in server....");
			return;
		}
		int num = 1;
		int num2 = 0;
		if (num > 0)
		{
			if (!string.IsNullOrEmpty(message))
			{
				JsonData jsonData = JsonMapper.ToObject(message);
				try
				{
					if (((IDictionary)jsonData).Contains((object)"code"))
					{
						if (int.Parse(jsonData["code"].ToString()) == 0)
						{
							string message2 = (string)jsonData["data"];
							Debug.Log(message2);
							return;
						}
						Debug.Log("ERROR - stopGetUserFacebookFriendListInServer() - " + num + "|" + num2);
					}
					return;
				}
				catch (Exception)
				{
					Debug.Log("Exception: Json");
					return;
				}
			}
			Debug.Log("ERROR - can't get user facebook friend list in server....");
		}
		else
		{
			Debug.Log("ERROR - stopGetUserFacebookFriendListInServer() - " + num + "|" + num2);
		}
	}

	public static void endGetUserFacebookFriendListInServer(object stateInfo)
	{
		CubeAppData.AppDataSendJson appDataSendJson = (CubeAppData.AppDataSendJson)stateInfo;
		if (appDataSendJson.Status() < 0 || appDataSendJson.Status() > 0)
		{
			RunCallback callBack = CallBackManager.Instance().GetCallBack("GetUserFacebookFriendListInServerMethod");
			callBack.StopNow();
		}
	}

	public static void GetUserGameCenterFriendListInServer()
	{
		Hashtable hashtable = new Hashtable();
		hashtable["deviceid"] = GameApp.GetInstance().GetGameState().DeviceID;
		hashtable["uid"] = "0";
		hashtable["uuid"] = GameApp.GetInstance().GetGameState().UUID;
		hashtable["nickname"] = string.Empty;
		hashtable["data"] = string.Empty;
		string jsonText = JsonMapper.ToJson(hashtable);
		HttpWrapper.SendJsonToGameServer(stopGetUserGameCenterFriendListInServer, "JudgementGetUserGameCenterFriendListInServer", jsonText);
	}

	public static void stopGetUserGameCenterFriendListInServer(bool bError, string message)
	{
		if (bError)
		{
			Debug.Log("ERROR - can't get game center friend list in server....");
			return;
		}
		int num = 1;
		int num2 = 0;
		if (num > 0)
		{
			if (string.IsNullOrEmpty(message))
			{
				Debug.Log("ERROR - can't get game center friend list in server....");
				return;
			}
			JsonData jsonData = JsonMapper.ToObject(message);
			Debug.Log("Set Friend User Extern Exp OK!!!~~");
		}
		else
		{
			Debug.Log("ERROR - stopGetUserGameCenterFriendListInServer() - " + num + "|" + num2);
		}
	}

	public static void endGetUserGameCenterFriendListInServer(object stateInfo)
	{
		CubeAppData.AppDataSendJson appDataSendJson = (CubeAppData.AppDataSendJson)stateInfo;
		if (appDataSendJson.Status() < 0 || appDataSendJson.Status() > 0)
		{
			RunCallback callBack = CallBackManager.Instance().GetCallBack("GetUserGameCenterFriendListInServerMethod");
			callBack.StopNow();
		}
	}

	public static void BindAccountInfo()
	{
		Hashtable hashtable = new Hashtable();
		hashtable["uuid"] = GameApp.GetInstance().GetGameState().UUID;
		hashtable["facebookid"] = GameApp.GetInstance().GetGameState().FacebookID;
		hashtable["gamecenterid"] = GameApp.GetInstance().GetGameState().GameCenterID;
		string jsonText = JsonMapper.ToJson(hashtable);
		HttpWrapper.SendJsonToGameServer(stopBindAccountInfo, "BindingAccount", jsonText);
	}

	public static void stopBindAccountInfo(bool bError, string message)
	{
		if (bError)
		{
			Debug.LogError("ERROR - BindAccountInfo ERROR!!!....");
			return;
		}
		int num = 1;
		int num2 = 0;
		if (num > 0)
		{
			if (!string.IsNullOrEmpty(message))
			{
				JsonData jsonData = JsonMapper.ToObject(message);
				try
				{
					if (((IDictionary)jsonData).Contains((object)"code"))
					{
						int num3 = int.Parse(jsonData["code"].ToString());
						Debug.Log("BindAccountInfo OK - " + num3);
					}
					return;
				}
				catch (Exception)
				{
					Debug.Log("Exception: Json");
					return;
				}
			}
			Debug.LogError("ERROR - BindAccountInfo ERROR!!!....");
		}
		else
		{
			Debug.Log("ERROR - stopBindAccountInfo() - " + num + "|" + num2);
		}
	}

	public static void endBindAccountInfo(object stateInfo)
	{
		CubeAppData.AccountDataSendJsonToPlatform accountDataSendJsonToPlatform = (CubeAppData.AccountDataSendJsonToPlatform)stateInfo;
		if (accountDataSendJsonToPlatform.Status() < 0 || accountDataSendJsonToPlatform.Status() > 0)
		{
			RunCallback callBack = CallBackManager.Instance().GetCallBack("BindAccountInfoMethod");
			callBack.StopNow();
		}
	}

	public static void GetUserFriendListInServer(string facebook_or_gamecenter)
	{
		Debug.Log("GetUserFriendListInServer()");
		prop.SetProp("GetUserFriendListInServer_Status", 0);
		prop.SetProp("GetUserFriendListInServer_type", facebook_or_gamecenter);
		Hashtable hashtable = new Hashtable();
		hashtable["uuid"] = GameApp.GetInstance().GetGameState().UUID;
		hashtable["uid"] = 0;
		ArrayList arrayList = new ArrayList();
		ArrayList arrayList2 = new ArrayList();
		if (facebook_or_gamecenter == "facebook")
		{
			List<string> facebookFriendList = GameApp.GetInstance().GetGameState().FacebookFriendList;
			for (int i = 0; i < facebookFriendList.Count; i++)
			{
				arrayList.Add(facebookFriendList[i].ToString());
			}
		}
		else
		{
			List<string> gameCenterFriendList = GameApp.GetInstance().GetGameState().GameCenterFriendList;
			for (int j = 0; j < gameCenterFriendList.Count; j++)
			{
				arrayList2.Add(gameCenterFriendList[j].ToString());
			}
		}
		hashtable["facebookids"] = arrayList;
		hashtable["gamecenterids"] = arrayList2;
		string text = JsonMapper.ToJson(hashtable);
		Debug.Log("GetUserFriendListInServer() Datas: " + text);
		HttpWrapper.SendJsonToGameServer(stopGetUserFriendListInServer, "ListBindingAccounts", text);
		Debug.Log("GetUserFriendListInServer()2");
	}

	public static void stopGetUserFriendListInServer(bool bError, string message)
	{
		if (bError)
		{
			Debug.Log("ERROR - can't get user friend list in server....");
			return;
		}
		int num = 1;
		int num2 = 0;
		if (num > 0)
		{
			if (!string.IsNullOrEmpty(message))
			{
				prop.SetProp("GetUserFriendListInServer_Status", 1);
				Debug.Log("stopGetUserFriendListInServer() |" + message);
				JsonData jsonData = JsonMapper.ToObject(message);
				try
				{
					if (!((IDictionary)jsonData).Contains((object)"code"))
					{
						return;
					}
					if (int.Parse(jsonData["code"].ToString()) == 0)
					{
						GetUserFriendList_Result getUserFriendList_Result = JsonMapper.ToObject<GetUserFriendList_Result>(message);
						Debug.Log("stopGetUserFriendListInServer() - Code:" + getUserFriendList_Result.code);
						friendListResult.Clear();
						for (int i = 0; i < getUserFriendList_Result.bindingbuddylist.Count; i++)
						{
							friendListResult.Add(getUserFriendList_Result.bindingbuddylist[i]);
						}
						Debug.Log("GetUserFriendListInServer OK |" + message + "|" + friendListResult.Count);
					}
					else
					{
						Debug.Log("ERROR - stopGetUserFriendListInServer() - " + num + "|" + num2);
						prop.SetProp("GetUserFriendListInServer_Status", 2);
					}
					return;
				}
				catch (Exception)
				{
					Debug.Log("Exception: Json");
					return;
				}
			}
			Debug.Log("ERROR - can't get user friend list in server....");
		}
		else
		{
			Debug.Log("ERROR - stopGetUserFriendListInServer() - " + num + "|" + num2);
			prop.SetProp("GetUserFriendListInServer_Status", 2);
		}
	}

	public static void endGetUserFriendListInServer(object stateInfo)
	{
		CubeAppData.AccountDataSendJsonToPlatform accountDataSendJsonToPlatform = (CubeAppData.AccountDataSendJsonToPlatform)stateInfo;
		if (accountDataSendJsonToPlatform.Status() < 0 || accountDataSendJsonToPlatform.Status() > 0)
		{
			RunCallback callBack = CallBackManager.Instance().GetCallBack("GetUserFriendListInServerMethod");
			callBack.StopNow();
		}
	}

	public static void GetAccountByBindingId(int facebookId, int gameCenterId)
	{
		prop.SetProp("GetAccountByBindingIdStatus", 0);
		Hashtable hashtable = new Hashtable();
		hashtable["deviceid"] = GameApp.GetInstance().GetGameState().DeviceID;
		hashtable["facebookid"] = facebookId;
		hashtable["gamecenterid"] = gameCenterId;
		string jsonText = JsonMapper.ToJson(hashtable);
		HttpWrapper.SendJsonToGameServer(stopGetAccountByBindingId, "GetAccountByBindingID", jsonText);
	}

	public static void stopGetAccountByBindingId(bool bError, string message)
	{
		if (bError)
		{
			Debug.Log("ERROR - can't get app version....");
			return;
		}
		int num = 1;
		int num2 = 0;
		if (num > 0)
		{
			if (!string.IsNullOrEmpty(message))
			{
				JsonData jsonData = JsonMapper.ToObject(message);
				try
				{
					if (((IDictionary)jsonData).Contains((object)"code"))
					{
						int num3 = int.Parse(jsonData["code"].ToString());
						if (num3 == 0)
						{
							string text = (string)jsonData["uuid"];
							Debug.Log("GetAccountByBindingId OK - " + text);
							prop.SetProp("GetAccountByBindingIdStatus", 1);
							prop.SetProp("GetAccountByBindingId_ResultCode", num3);
							prop.SetProp("GetAccountByBindingId_ResultUUID", text);
						}
						else
						{
							Debug.Log("ERROR - stopGetAccountByBindingId() - " + num + "|" + num2);
							prop.SetProp("GetAccountByBindingIdStatus", 2);
						}
					}
					return;
				}
				catch (Exception)
				{
					Debug.Log("Exception: Json");
					return;
				}
			}
			Debug.Log("ERROR - can't get app version....");
			prop.SetProp("GetAccountByBindingIdStatus", 2);
		}
		else
		{
			Debug.Log("ERROR - stopGetAccountByBindingId() - " + num + "|" + num2);
			prop.SetProp("GetAccountByBindingIdStatus", 2);
		}
	}

	public static void endGetAccountByBindingId(object stateInfo)
	{
		CubeAppData.AccountDataSendJsonToPlatform accountDataSendJsonToPlatform = (CubeAppData.AccountDataSendJsonToPlatform)stateInfo;
		if (accountDataSendJsonToPlatform.Status() < 0 || accountDataSendJsonToPlatform.Status() > 0)
		{
			RunCallback callBack = CallBackManager.Instance().GetCallBack("GetAccountByBindingIdMethod");
			callBack.StopNow();
		}
	}

	public static void SendDailyCollectionInfo(string collectionInfo)
	{
		Hashtable hashtable = new Hashtable();
		hashtable["deviceid"] = GameApp.GetInstance().GetGameState().DeviceID;
		hashtable["gamename"] = "callofminidoubleshot";
		hashtable["data"] = collectionInfo;
		string jsonText = JsonMapper.ToJson(hashtable);
		HttpWrapper.SendJsonToGameServer(stopSendDailyCollectionInfo, "logAllInfo", jsonText);
		prop.SetProp("SendDailyCollectionInfo_Status", 0);
	}

	public static void stopSendDailyCollectionInfo(bool bError, string message)
	{
		if (bError)
		{
			Debug.Log("ERROR - Set daily login times ERROR - 1....");
			return;
		}
		int num = 1;
		int num2 = 0;
		if (num > 0)
		{
			if (string.IsNullOrEmpty(message))
			{
				Debug.Log("ERROR - Set daily login times ERROR - 1....");
				prop.SetProp("SendDailyCollectionInfo_Status", 2);
			}
			else
			{
				JsonData jsonData = JsonMapper.ToObject(message);
				prop.SetProp("SendDailyCollectionInfo_Status", 1);
				Debug.Log("Set daily login times OK - ");
			}
		}
		else
		{
			Debug.Log("ERROR - Set daily login times ERROR - 2....");
			prop.SetProp("SendDailyCollectionInfo_Status", 2);
		}
	}

	public static void endSendDailyCollectionInfo(object stateInfo)
	{
		CubeAppData.AccountDataSendJsonToPlatform accountDataSendJsonToPlatform = (CubeAppData.AccountDataSendJsonToPlatform)stateInfo;
		if (accountDataSendJsonToPlatform.Status() < 0 || accountDataSendJsonToPlatform.Status() > 0)
		{
			RunCallback callBack = CallBackManager.Instance().GetCallBack("SendDailyCollectionInfoMethod");
			callBack.StopNow();
		}
	}

	public static void Hire_HireOutSelf(int hire_price)
	{
		Hashtable hashtable = new Hashtable();
		hashtable["deviceid"] = GameApp.GetInstance().GetGameState().DeviceID;
		hashtable["gamename"] = "callofminidoubleshot";
		hashtable["uuid"] = GameApp.GetInstance().GetGameState().UUID;
		hashtable["money"] = hire_price.ToString();
		hashtable["lev"] = GameApp.GetInstance().GetGameState().GetPlayerLevel();
		string jsonText = JsonMapper.ToJson(hashtable);
		HttpWrapper.SendJsonToGameServer(stopHire_HireOutSelf, "JudgementPubMercenary", jsonText);
	}

	public static void stopHire_HireOutSelf(bool bError, string message)
	{
		if (bError)
		{
			Debug.Log("ERROR - Hire_HireOutSelf ERROR - 1....");
			return;
		}
		int num = 1;
		int num2 = 0;
		if (num > 0)
		{
			if (string.IsNullOrEmpty(message))
			{
				Debug.Log("ERROR - Hire_HireOutSelf ERROR - 1....");
				return;
			}
			JsonData jsonData = JsonMapper.ToObject(message);
			Debug.Log("Hire_HireOutSelf OK - ");
		}
		else
		{
			Debug.Log("ERROR - Hire_HireOutSelf ERROR - 2....");
		}
	}

	public static void endHire_HireOutSelf(object stateInfo)
	{
		CubeAppData.AppDataSendJson appDataSendJson = (CubeAppData.AppDataSendJson)stateInfo;
		if (appDataSendJson.Status() < 0 || appDataSendJson.Status() > 0)
		{
			RunCallback callBack = CallBackManager.Instance().GetCallBack("Hire_HireOutSelfMethod");
			callBack.StopNow();
		}
	}

	public static void Hire_GetHireArray()
	{
		Hashtable hashtable = new Hashtable();
		hashtable["deviceid"] = GameApp.GetInstance().GetGameState().DeviceID;
		hashtable["gamename"] = "callofminidoubleshot";
		hashtable["uuid"] = GameApp.GetInstance().GetGameState().UUID;
		hashtable["lev"] = GameApp.GetInstance().GetGameState().GetPlayerLevel()
			.ToString();
		string jsonText = JsonMapper.ToJson(hashtable);
		HttpWrapper.SendJsonToGameServer(stopHire_GetHireArray, "JudgementListMercenary", jsonText);
		prop.SetProp("GetHireArray_Status", 0);
	}

	public static void stopHire_GetHireArray(bool bError, string message)
	{
		if (bError)
		{
			Debug.Log("ERROR - stopHire_GetHireArray ERROR - 1....");
			return;
		}
		int num = 1;
		int num2 = 0;
		if (num > 0)
		{
			if (string.IsNullOrEmpty(message))
			{
				Debug.Log("ERROR - stopHire_GetHireArray ERROR - 1....");
				prop.SetProp("GetHireArray_Status", 2);
				return;
			}
			JsonData jsonData = JsonMapper.ToObject(message);
			Debug.Log(message);
			GetHireUserList_Result getHireUserList_Result = JsonMapper.ToObject<GetHireUserList_Result>(message);
			if (getHireUserList_Result.code == 0)
			{
				HireFriendListResult = getHireUserList_Result.datas;
				prop.SetProp("GetHireArray_Status", 1);
				Debug.Log("Set stopHire_GetHireArray OK - ");
			}
			else
			{
				Debug.Log("ERROR - stopHire_GetHireArray ERROR - 3....");
				prop.SetProp("GetHireArray_Status", 2);
			}
		}
		else
		{
			Debug.Log("ERROR - stopHire_GetHireArray ERROR - 2....");
			prop.SetProp("GetHireArray_Status", 2);
		}
	}

	public static void endHire_GetHireArray(object stateInfo)
	{
		CubeAppData.AppDataSendJson appDataSendJson = (CubeAppData.AppDataSendJson)stateInfo;
		if (appDataSendJson.Status() < 0 || appDataSendJson.Status() > 0)
		{
			RunCallback callBack = CallBackManager.Instance().GetCallBack("Hire_GetHireArrayMethod");
			callBack.StopNow();
		}
	}

	public static void Hire_HireOther(string other_uuid, int other_level)
	{
		Hashtable hashtable = new Hashtable();
		hashtable["deviceid"] = GameApp.GetInstance().GetGameState().DeviceID;
		hashtable["gamename"] = "callofminidoubleshot";
		hashtable["uuid"] = GameApp.GetInstance().GetGameState().UUID;
		hashtable["tarid"] = other_uuid;
		hashtable["level"] = other_level;
		string jsonText = JsonMapper.ToJson(hashtable);
		HttpWrapper.SendJsonToGameServer(stopHire_HireOther, "JudgementMercenaryOther", jsonText);
	}

	public static void stopHire_HireOther(bool bError, string message)
	{
		if (bError)
		{
			Debug.Log("ERROR - Set daily login times ERROR - 1....");
			return;
		}
		int num = 1;
		int num2 = 0;
		if (num > 0)
		{
			if (!string.IsNullOrEmpty(message))
			{
				JsonData jsonData = JsonMapper.ToObject(message);
				try
				{
					if (((IDictionary)jsonData).Contains((object)"code"))
					{
						switch (int.Parse(jsonData["code"].ToString()))
						{
						case 1:
							Debug.Log("Hire_HireOther OK - ");
							break;
						case 100001:
							Debug.Log("ERROR - Hire_HireOther ERROR - 100001....");
							break;
						case 100002:
							Debug.Log("ERROR - Hire_HireOther ERROR - 100002....");
							break;
						}
					}
					return;
				}
				catch (Exception)
				{
					Debug.Log("Exception: Json");
					return;
				}
			}
			Debug.Log("ERROR - Set daily login times ERROR - 1....");
		}
		else
		{
			Debug.Log("ERROR - Hire_HireOther ERROR - 2....");
		}
	}

	public static void endHire_HireOther(object stateInfo)
	{
		CubeAppData.AppDataSendJson appDataSendJson = (CubeAppData.AppDataSendJson)stateInfo;
		if (appDataSendJson.Status() < 0 || appDataSendJson.Status() > 0)
		{
			RunCallback callBack = CallBackManager.Instance().GetCallBack("Hire_HireOtherMethod");
			callBack.StopNow();
		}
	}

	public static void GetHireFriendUserData(FriendUserData friend_user_data)
	{
		Debug.Log("GetHireFriendUserData() - " + friend_user_data.m_Name);
		prop.SetProp("GetHireFriendUserDataStatus", 0);
		Hashtable hashtable = new Hashtable();
		hashtable["deviceid"] = friend_user_data.m_DeviceId;
		hashtable["uuid"] = friend_user_data.m_UUID;
		hashtable["nickname"] = string.Empty;
		string jsonText = JsonMapper.ToJson(hashtable);
		HttpWrapper.SendJsonToGameServer(stopGetHireFriendUserData, "JudgementGetUserData", jsonText);
	}

	public static void stopGetHireFriendUserData(bool bError, string message)
	{
		if (bError)
		{
			Debug.Log("ERROR - can't get friend user data....");
			return;
		}
		int num = 1;
		int num2 = 0;
		if (num > 0)
		{
			if (!string.IsNullOrEmpty(message))
			{
				Debug.Log("stopGetHireFriendUserData() - " + message);
				prop.SetProp("GetHireFriendUserDataStatus", 1);
				JsonData jsonData = JsonMapper.ToObject(message);
				try
				{
					if (((IDictionary)jsonData).Contains((object)"code"))
					{
						int num3 = int.Parse(jsonData["code"].ToString());
						if (num3 == 0)
						{
							string text = (string)jsonData["data"];
							Debug.Log("stopGetHireFriendUserData : " + text);
							prop.SetProp("GetHireFriendUserData_Data", text);
						}
						else
						{
							Debug.Log("stopGetHireFriendUserData: Code " + num3);
							prop.SetProp("GetHireFriendUserData_Data", string.Empty);
						}
					}
					return;
				}
				catch (Exception)
				{
					Debug.Log("Exception: Json");
					return;
				}
			}
			Debug.Log("ERROR - can't get friend user data....");
			prop.SetProp("GetHireFriendUserDataStatus", 2);
		}
		else
		{
			Debug.Log("ERROR - stopGetHireFriendUserData() - " + num + "|" + num2);
			prop.SetProp("GetHireFriendUserDataStatus", 2);
		}
	}

	public static void endGetHireFriendUserData(object stateInfo)
	{
		CubeAppData.AppDataSendJson appDataSendJson = (CubeAppData.AppDataSendJson)stateInfo;
		if (appDataSendJson.Status() < 0 || appDataSendJson.Status() > 0)
		{
			RunCallback callBack = CallBackManager.Instance().GetCallBack("GetHireFriendUserDataMethod");
			callBack.StopNow();
		}
	}
}
