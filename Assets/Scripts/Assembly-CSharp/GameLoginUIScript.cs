using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Zombie3D;

public class GameLoginUIScript : MonoBehaviour, UIHandler
{
	public enum Controls
	{
		kIDLoginWithFacebook = 1000,
		kIDLoginWithGameCenter = 1001,
		kIDLoginWithLocalGame = 1002,
		kIDLoginOk = 1003,
		kIDGetNewAppVersionOk = 1004,
		kIDGetNewAppVersionLater = 1005,
		kIDConnectNetworkYes = 1006,
		kIDConnectNetworkNo = 1007,
		kIDCartoonCancel = 1008,
		kIDNotificationDialogOK = 1009,
		kIDChoosePointsBegin = 1010,
		kIDChoosePointsLast = 1110,
		kIDChooseWavesBegin = 1111,
		kIDChooseWavesLast = 1211,
		kIDLast = 1212
	}

	private UIManager m_UIManager;

	private string m_ui_material_path;

	protected Material m_MatCommonBg;

	protected Material m_MatLoginUI;

	protected Material m_MatLoginBg;

	protected Material m_MatDialog01;

	public uiGroup m_uiGroup;

	public uiGroup m_LoadingUI;

	public uiGroup m_DialogNoticeUIGroup;

	private UIImage m_WheelAnimLeft;

	private UIImage m_WheelAnimRight;

	private UICartoonAnimControl m_CartoonAnim;

	private GameLoadingUI m_GameLoadingUI;

	private bool m_bBeginBattle;

	private float m_BeginBattleTimer;

	protected float lastUpdateTime;

	protected bool uiInited;

	private GameState gameState;

	private bool m_bConnectingNetwork;

	private float m_connectNetworkStartTime = -1f;

	private float m_connectNetworkTimer = 20f;

	protected int selectedLoginType;

	protected bool m_bFirstLogin;

	protected bool m_bManualLogin;

	protected bool m_bLoginToServer;

	protected bool m_bGetAppVersion;

	protected bool m_bLoginingWithFacebook;

	protected bool m_bLoginingWithGameCenter;

	protected bool m_bLoadingGameCenterFriends;

	protected float m_LoadingGameCenterFriendsStartTime = -1f;

	protected bool m_bLoadingUserDataFromServer;

	private bool m_bHaveGameCenter = true;

	private void Awake()
	{
		m_bManualLogin = false;
		selectedLoginType = 1002;
		m_bGetAppVersion = false;
		m_bLoginingWithFacebook = false;
		m_bLoginingWithGameCenter = false;
		m_bLoadingGameCenterFriends = false;
		m_bLoadingUserDataFromServer = false;
	}

	public bool bConnectAbility()
	{
		if (Application.internetReachability == NetworkReachability.NotReachable)
		{
			return false;
		}
		return true;
	}

	private IEnumerator Start()
	{
		yield return 0;
		MiscPlugin.CancelLocalNotification();
		MiscPlugin.ScheduleOfflineNotification(259200L, "Do you want to relist yourself for hire?");
		m_UIManager = base.gameObject.AddComponent<UIManager>() as UIManager;
		m_UIManager.SetParameter(8, 3, false);
		m_UIManager.SetUIHandler(this);
		UIUtils.BuildIpone5Frame(m_UIManager);
		m_MatCommonBg = LoadUIMaterial("Zombie3D/UI/Materials/CommonBgUI");
		m_MatLoginUI = LoadUIMaterial("Zombie3D/UI/Materials/GameLoginUI");
		m_MatLoginBg = LoadUIMaterial("Zombie3D/UI/Materials/GameLoginBgUI");
		Resources.UnloadUnusedAssets();
		uiInited = true;
		if (GameApp.GetInstance().GetGameState().exp == 0)
		{
			SceneUIManager.Instance().SetMusicPlayerState(SceneUIManager.MusicState_GameStartFirstPlayState);
		}
		else
		{
			SceneUIManager.Instance().SetMusicPlayerState(SceneUIManager.MusicState_GameStartNotFirstPlayState);
		}
		gameState = GameApp.GetInstance().GetGameState();
		gameState.AddDailyCollectionInfo(1, 0f, 0, 0);
		m_bHaveGameCenter = MiscPlugin.IsOS4_1Up();
		if (CheckAlreadyHaveUserData())
		{
			m_bFirstLogin = false;
		}
		else
		{
			m_bFirstLogin = true;
		}
		if (!m_bFirstLogin && gameState.m_bReLogin)
		{
			m_bFirstLogin = true;
		}
		Debug.Log("First LOGIN: " + m_bFirstLogin);
		if (bConnectAbility())
		{
			GameClient.Login(gameState.DeviceID, string.Empty);
			m_bLoginToServer = true;
			SetupPreLoadingUI(true);
			SetupLoadingUI(true, "Login");
		}
		else if (gameState.m_bReLogin)
		{
			m_bConnectingNetwork = false;
			SetupNetworkConnectUI(true);
		}
		else
		{
			m_bConnectingNetwork = false;
			if (m_bFirstLogin)
			{
				gameState.LoginType = GameLoginType.LoginType_Local;
				PlayerPrefs.SetInt("LoginType", (int)gameState.LoginType);
				SetupCartoonUI(true);
			}
			else
			{
				gameState.LoginType = GameLoginType.LoginType_Local;
				PlayerPrefs.SetInt("LoginType", (int)gameState.LoginType);
				SceneUIManager.Instance().ChangeSceneUI(SceneUIManager.SceneUI.ChoosePointsUI);
			}
		}
		ChartBoostAndroid.showInterstitial(null);
	}

	private void Update()
	{
		UITouchInner[] array = (Application.isMobilePlatform) ? iPhoneInputMgr.MockTouches() : WindowsInputMgr.MockTouches();
		foreach (UITouchInner touch in array)
		{
			if (!(m_UIManager != null) || m_UIManager.HandleInput(touch))
			{
			}
		}
		if (Time.time - lastUpdateTime < 0.001f || !uiInited)
		{
			return;
		}
		float num = Time.time - lastUpdateTime;
		lastUpdateTime = Time.time;
		if (m_bConnectingNetwork)
		{
			if (bConnectAbility())
			{
				m_bConnectingNetwork = false;
				GameClient.Login(gameState.DeviceID, string.Empty);
				m_bLoginToServer = true;
				SetupPreLoadingUI(true);
				SetupLoadingUI(true, "Login");
			}
			else if (Time.time - m_connectNetworkStartTime >= m_connectNetworkTimer)
			{
				m_bConnectingNetwork = false;
				SetupLoadingUI(false, string.Empty);
				SetupNetworkConnectUI(true);
			}
		}
		if (m_bLoginToServer)
		{
			switch (GameClient.prop.GetInt("LoginStatus", 0))
			{
			case 1:
				m_bLoginToServer = false;
				SetupLoadingUI(false, string.Empty);
				GameClient.GetAppVersion();
				m_bGetAppVersion = true;
				SetupPreLoadingUI(true);
				SetupLoadingUI(true, "GetAppVersion");
				break;
			case 2:
				m_bLoginToServer = false;
				SetupLoadingUI(false, string.Empty);
				GameClient.GetAppVersion();
				m_bGetAppVersion = true;
				SetupPreLoadingUI(true);
				SetupLoadingUI(true, "GetAppVersion");
				break;
			}
		}
		if (m_bGetAppVersion)
		{
			switch (GameClient.prop.GetInt("GetAppVersionStatus", 0))
			{
			case 1:
			{
				m_bGetAppVersion = false;
				SetupLoadingUI(false, string.Empty);
				string text = GameClient.prop.GetString("ServerAppVersion");
				if (string.IsNullOrEmpty(text))
				{
					text = "2.02";
				}
				if (float.Parse("2.02") < float.Parse(text))
				{
					SetupAppNewVersionUI(true);
					break;
				}
				if (m_bFirstLogin)
				{
					if (gameState.m_bReLogin || !m_bHaveGameCenter)
					{
						SetupGameLoginUI(true);
						return;
					}
					gameState.LoginType = GameLoginType.LoginType_Local;
					PlayerPrefs.SetInt("LoginType", (int)gameState.LoginType);
					GameClient.Login(gameState.DeviceID, string.Empty);
					SetupGameLoginUI(true);
					return;
				}
				if (gameState.LoginType == GameLoginType.LoginType_Facebook)
				{
					if (m_bManualLogin)
					{
						FacebookOperator.FacebookStatus();
						FacebookOperator.Login();
					}
					else
					{
						FacebookOperator.FacebookStatus();
						FacebookOperator.Resume();
					}
					SetupPreLoadingUI(true);
					SetupLoadingUI(true, "Login Facebook");
					m_bLoginingWithFacebook = true;
				}
				else if (gameState.LoginType == GameLoginType.LoginType_GameCenter)
				{
					if (!m_bManualLogin)
					{
						GameCenterPlugin.Initialize();
						GameCenterPlugin.Login();
					}
					SetupPreLoadingUI(true);
					SetupLoadingUI(true, "Login GameCenter");
					gameState.GameCenterFriendList.Clear();
					m_bLoginingWithGameCenter = true;
				}
				else if (CheckAlreadyHaveUserData())
				{
					gameState.LoadExchangeInfo();
					if (!gameState.m_bExchanged && (gameState.m_BattleGold > 0f || gameState.m_BattleExp > 0f))
					{
						gameState.m_bGameLoginExchange = true;
						SceneUIManager.Instance().ChangeSceneUI(SceneUIManager.SceneUI.ExchangeUI);
					}
					else
					{
						SceneUIManager.Instance().ChangeSceneUI(SceneUIManager.SceneUI.ChoosePointsUI);
					}
				}
				else
				{
					SceneUIManager.Instance().ChangeSceneUI(SceneUIManager.SceneUI.ChoosePointsUI);
				}
				break;
			}
			case 2:
				m_bGetAppVersion = false;
				SetupLoadingUI(false, string.Empty);
				if (m_bFirstLogin)
				{
					if (gameState.m_bReLogin || !m_bHaveGameCenter)
					{
						SetupGameLoginUI(true);
						return;
					}
					gameState.LoginType = GameLoginType.LoginType_GameCenter;
					PlayerPrefs.SetInt("LoginType", (int)gameState.LoginType);
					GameClient.Login(gameState.DeviceID, string.Empty);
					GameCenterPlugin.Initialize();
					GameCenterPlugin.Login();
					SetupPreLoadingUI(true);
					SetupLoadingUI(true, "Login GameCenter");
					m_bLoginingWithGameCenter = true;
					return;
				}
				if (gameState.LoginType == GameLoginType.LoginType_Facebook)
				{
					if (m_bManualLogin)
					{
						FacebookOperator.FacebookStatus();
						FacebookOperator.Login();
					}
					else
					{
						FacebookOperator.FacebookStatus();
						FacebookOperator.Resume();
					}
					SetupPreLoadingUI(true);
					SetupLoadingUI(true, "Login Facebook");
					gameState.FacebookFriendList.Clear();
					m_bLoginingWithFacebook = true;
				}
				else if (gameState.LoginType == GameLoginType.LoginType_GameCenter)
				{
					GameCenterPlugin.Initialize();
					GameCenterPlugin.Login();
					SetupPreLoadingUI(true);
					SetupLoadingUI(true, "Login GameCenter");
					gameState.GameCenterFriendList.Clear();
					m_bLoginingWithGameCenter = true;
				}
				else if (CheckAlreadyHaveUserData())
				{
					gameState.LoadExchangeInfo();
					if (!gameState.m_bExchanged && (gameState.m_BattleGold > 0f || gameState.m_BattleExp > 0f))
					{
						gameState.m_bGameLoginExchange = true;
						SceneUIManager.Instance().ChangeSceneUI(SceneUIManager.SceneUI.ExchangeUI);
					}
					else
					{
						SceneUIManager.Instance().ChangeSceneUI(SceneUIManager.SceneUI.ChoosePointsUI);
					}
				}
				else
				{
					SceneUIManager.Instance().ChangeSceneUI(SceneUIManager.SceneUI.ChoosePointsUI);
				}
				break;
			}
		}
		if (m_bLoginingWithFacebook)
		{
			int @int = FacebookOperator.prop.GetInt("FacebookLoginStatus", 0);
			switch (@int)
			{
			case 1:
			{
				Debug.Log("Login Facebook OK!!!");
				m_bLoginingWithFacebook = false;
				string @string = FacebookOperator.prop.GetString("FacebookId");
				gameState.FacebookID = @string;
				string string2 = FacebookOperator.prop.GetString("FacebookName");
				gameState.FacebookName = string2;
				if (m_bManualLogin)
				{
					GameClient.BindAccountInfo();
				}
				gameState.FacebookFriendList = new List<string>();
				ArrayList arrayList = FacebookOperator.prop.GetObject("FacebookFriends") as ArrayList;
				for (int j = 0; j < arrayList.Count; j++)
				{
					gameState.FacebookFriendList.Add(arrayList[j].ToString());
				}
				GameClient.GetUserFriendListInServer("facebook");
				FriendUserData friend_data = gameState.GenerateDefaultFriendPlayer();
				gameState.GetFriends().Clear();
				gameState.AddFriend(friend_data);
				SceneUIManager.Instance().LoadFriendFromServer();
				if (gameState.UUID != string.Empty)
				{
					GameClient.GetUserData();
					SetupPreLoadingUI(true);
					SetupLoadingUI(true, "GetUserData");
					m_bLoadingUserDataFromServer = true;
				}
				else
				{
					Debug.Log("ERROR: m_bLoginingWithFacebook - UUID:" + gameState.UUID);
				}
				break;
			}
			case 2:
			case 3:
			case 4:
				Debug.LogError("Login Facebook Cancel or Error or TimeOut!!! - " + @int);
				m_bLoginingWithFacebook = false;
				SetupLoadingUI(false, string.Empty);
				SetupGameLoginUI(true);
				MiscPlugin.ShowMessageBox1("Network Error", "Login facebook error!", "OK");
				break;
			}
		}
		else if (m_bLoginingWithGameCenter)
		{
			if (GameCenterPlugin.IsLogin())
			{
				m_bLoginingWithGameCenter = false;
				string account = GameCenterPlugin.GetAccount();
				string text2 = GameCenterPlugin.GetName();
				gameState.GameCenterID = account;
				gameState.GameCenterName = text2;
				if (m_bManualLogin)
				{
					GameClient.BindAccountInfo();
				}
				Debug.Log("Login GameCenter OK - |" + account + "|" + text2 + "|");
				SetupLoadingUI(false, string.Empty);
				SetupPreLoadingUI(true);
				SetupLoadingUI(true, "Loading GameCenter Friends");
				m_bLoadingGameCenterFriends = true;
				m_LoadingGameCenterFriendsStartTime = Time.time;
				GameCenterPlugin.LoadFriends();
			}
			else if (GameCenterPlugin.LoginStatus() == 2 || GameCenterPlugin.LoginStatus() == 3)
			{
				m_bLoginingWithGameCenter = false;
				SetupGameLoginUI(true);
				MiscPlugin.ShowMessageBox1("Network Error", "Login GameCenter error!", "OK");
			}
		}
		if (m_bLoadingGameCenterFriends)
		{
			bool flag = false;
			if (Time.time - m_LoadingGameCenterFriendsStartTime > 20f)
			{
				flag = true;
			}
			int loadFriendStatus = GameCenterPlugin.GetLoadFriendStatus();
			switch (loadFriendStatus)
			{
			case 3:
			{
				m_bLoadingGameCenterFriends = false;
				FriendsUIScript.m_bLoadingGameCenterFriendsError = false;
				Debug.Log("Get GameCenter friends OK - " + gameState.UUID);
				SetupLoadingUI(false, string.Empty);
				gameState.GameCenterFriendList = new List<string>();
				int friendCount = GameCenterPlugin.GetFriendCount();
				if (friendCount > 0)
				{
					for (int k = 0; k < friendCount; k++)
					{
						string friendAccount = GameCenterPlugin.GetFriendAccount(k);
						string text3 = GameCenterPlugin.GetFriendAccount(k) + string.Empty;
						gameState.GameCenterFriendList.Add(friendAccount);
					}
					GameClient.GetUserFriendListInServer("gamecenter");
					FriendUserData friend_data2 = gameState.GenerateDefaultFriendPlayer();
					gameState.GetFriends().Clear();
					gameState.AddFriend(friend_data2);
					SceneUIManager.Instance().LoadFriendFromServer();
				}
				if (gameState.UUID != string.Empty)
				{
					GameClient.GetUserData();
					SetupPreLoadingUI(true);
					SetupLoadingUI(true, "GetUserData");
					m_bLoadingUserDataFromServer = true;
				}
				else
				{
					Debug.Log("ERROR: m_bLoadingGameCenterFriends - UUID:" + gameState.UUID);
					SetupGameLoginUI(true);
				}
				break;
			}
			case 1:
				if (flag)
				{
					FriendsUIScript.m_bLoadingGameCenterFriendsError = true;
					m_bLoadingGameCenterFriends = false;
					Debug.Log("ERROR: m_bLoadingGameCenterFriends TIMEOUT");
					SetupLoadingUI(false, string.Empty);
					if (gameState.UUID != string.Empty)
					{
						GameClient.GetUserData();
						SetupPreLoadingUI(true);
						SetupLoadingUI(true, "GetUserData");
						m_bLoadingUserDataFromServer = true;
					}
					else
					{
						Debug.Log("ERROR: Login GameCetner Error AND UUID is null!!!");
						SetupGameLoginUI(true);
					}
				}
				break;
			default:
				if (flag || loadFriendStatus == 2)
				{
					FriendsUIScript.m_bLoadingGameCenterFriendsError = true;
					m_bLoadingGameCenterFriends = false;
					Debug.Log("ERROR: m_bLoadingGameCenterFriends " + flag + "|" + loadFriendStatus + " | " + gameState.UUID);
					SetupLoadingUI(false, string.Empty);
					if (gameState.UUID != string.Empty)
					{
						GameClient.GetUserData();
						SetupPreLoadingUI(true);
						SetupLoadingUI(true, "GetUserData");
						m_bLoadingUserDataFromServer = true;
					}
					else
					{
						Debug.Log("ERROR: Login GameCetner Error AND UUID is null!!!");
						SetupGameLoginUI(true);
					}
				}
				break;
			}
		}
		if (m_bLoadingUserDataFromServer)
		{
			bool flag2 = false;
			switch (GameClient.prop.GetInt("GetUserDataStatus", 0))
			{
			case 1:
				flag2 = true;
				m_bLoadingUserDataFromServer = false;
				Debug.Log("Loading Player data OK ");
				SetupLoadingUI(false, string.Empty);
				if (GameClient.prop.GetInt("GetUserDataCode", 1) == 0)
				{
					string string3 = GameClient.prop.GetString("UserData");
					int int2 = GameClient.prop.GetInt("UserExternExp", 0);
					int int3 = GameClient.prop.GetInt("UserHiredMoney", 0);
					gameState.ExternExp = int2;
					gameState.BeHiredMoney = int3;
					gameState.LoadData(string3);
					if (gameState.LoginType == GameLoginType.LoginType_GameCenter)
					{
						string account2 = GameCenterPlugin.GetAccount();
						string gameCenterName = GameCenterPlugin.GetName();
						gameState.GameCenterID = account2;
						gameState.GameCenterName = gameCenterName;
					}
					else if (gameState.LoginType == GameLoginType.LoginType_Facebook)
					{
						string string4 = FacebookOperator.prop.GetString("FacebookId");
						gameState.FacebookID = string4;
						string string5 = FacebookOperator.prop.GetString("FacebookName");
						gameState.FacebookName = string5;
					}
				}
				break;
			case 2:
				flag2 = true;
				Debug.LogError("Loading Player data From Server ERROR!!!");
				m_bLoadingUserDataFromServer = false;
				break;
			}
			if (flag2)
			{
				if (m_bManualLogin)
				{
					GameClient.BindAccountInfo();
				}
				if (m_bFirstLogin && !gameState.m_bReLogin)
				{
					SetupCartoonUI(true);
				}
				else if (CheckAlreadyHaveUserData())
				{
					gameState.LoadExchangeInfo();
					if (!gameState.m_bExchanged && (gameState.m_BattleGold > 0f || gameState.m_BattleExp > 0f))
					{
						gameState.m_bGameLoginExchange = true;
						SceneUIManager.Instance().ChangeSceneUI(SceneUIManager.SceneUI.ExchangeUI);
					}
					else
					{
						SceneUIManager.Instance().ChangeSceneUI(SceneUIManager.SceneUI.ChoosePointsUI);
					}
				}
				else
				{
					SceneUIManager.Instance().ChangeSceneUI(SceneUIManager.SceneUI.ChoosePointsUI);
				}
			}
		}
		if (m_WheelAnimLeft != null)
		{
			m_WheelAnimLeft.SetRotation(Time.time);
		}
		if (m_WheelAnimRight != null)
		{
			m_WheelAnimRight.SetRotation(0f - Time.time);
		}
		if (m_CartoonAnim != null && m_CartoonAnim.PlayEnd)
		{
			m_CartoonAnim.Enable = false;
			m_CartoonAnim.Visible = false;
			if (m_uiGroup != null)
			{
				m_uiGroup.Remove(m_CartoonAnim);
			}
			m_CartoonAnim = null;
			GameApp.GetInstance().GetGameState().SetGameTriggerInfo(1, 1, 1);
			m_bBeginBattle = true;
			m_GameLoadingUI = new GameLoadingUI();
			m_GameLoadingUI.SetupLoadingUI(true, m_UIManager, m_MatCommonBg);
		}
		if (m_bBeginBattle)
		{
			m_BeginBattleTimer += num;
			if (m_BeginBattleTimer > 0.1f)
			{
				GameApp.GetInstance().SetLoadMap(true);
				Application.LoadLevel("Zombie3D_Judgement_Map01");
				SceneUIManager.Instance().ChangeSceneUI(SceneUIManager.SceneUI.BattleUI);
				m_bBeginBattle = false;
			}
		}
	}

	private void LateUpdate()
	{
	}

	public void HandleEvent(UIControl control, int command, float wparam, float lparam)
	{
		if ((control.GetType() == typeof(UIClickButton) || control.GetType() == typeof(UISelectButton)) && GameApp.GetInstance().GetGameState().SoundOn)
		{
			SceneUIManager.Instance().PlayClickAudio();
		}
		if (control.Id == 1000)
		{
			selectedLoginType = 1000;
			UISelectButton uISelectButton = (UISelectButton)m_uiGroup.GetControl(1001);
			uISelectButton.Set(false);
			uISelectButton = (UISelectButton)m_uiGroup.GetControl(1002);
			uISelectButton.Set(false);
		}
		else if (control.Id == 1001)
		{
			selectedLoginType = 1001;
			UISelectButton uISelectButton2 = (UISelectButton)m_uiGroup.GetControl(1000);
			uISelectButton2.Set(false);
			uISelectButton2 = (UISelectButton)m_uiGroup.GetControl(1002);
			uISelectButton2.Set(false);
		}
		else if (control.Id == 1002)
		{
			selectedLoginType = 1002;
		}
		else if (control.Id == 1003)
		{
			gameState.LoginType = GameLoginType.LoginType_Local;
			if (CheckAlreadyHaveUserData())
			{
				SceneUIManager.Instance().ChangeSceneUI(SceneUIManager.SceneUI.ChoosePointsUI);
			}
			else
			{
				SetupCartoonUI(true);
			}
		}
		else if (control.Id == 1004)
		{
			Application.OpenURL("http://itunes.apple.com/us/app/call-of-mini-double-shot/id459012341?ls=1&mt=8");
		}
		else if (control.Id == 1005)
		{
			gameState.LoginType = GameLoginType.LoginType_Local;
			PlayerPrefs.SetInt("LoginType", (int)gameState.LoginType);
			if (m_bFirstLogin && !gameState.m_bReLogin)
			{
				SetupCartoonUI(true);
			}
			else
			{
				SceneUIManager.Instance().ChangeSceneUI(SceneUIManager.SceneUI.ChoosePointsUI);
			}
		}
		else if (control.Id == 1006)
		{
			m_bConnectingNetwork = true;
			m_connectNetworkStartTime = Time.time;
			SetupNetworkConnectUI(false);
			SetupPreLoadingUI(true);
			SetupLoadingUI(true, "Connect Network");
		}
		else if (control.Id == 1007)
		{
			m_bConnectingNetwork = false;
			if (m_bFirstLogin)
			{
				gameState.LoginType = GameLoginType.LoginType_Local;
				PlayerPrefs.SetInt("LoginType", (int)gameState.LoginType);
				SetupCartoonUI(true);
			}
			else
			{
				gameState.LoginType = GameLoginType.LoginType_Local;
				PlayerPrefs.SetInt("LoginType", (int)gameState.LoginType);
				SceneUIManager.Instance().ChangeSceneUI(SceneUIManager.SceneUI.ChoosePointsUI);
			}
		}
		else if (control.Id == 1008)
		{
			SceneUIManager.Instance().ChangeSceneUI(SceneUIManager.SceneUI.ChoosePointsUI);
		}
		else if (control.Id == 1009)
		{
			SetupNotificationDialogUI(false, string.Empty);
		}
		else if (control.Id != 1212)
		{
		}
	}

	public bool CheckAlreadyHaveUserData()
	{
		bool result = true;
		string text = Utils.SavePath();
		string path = text + "/MyGameData";
		if (File.Exists(path))
		{
			string text2 = string.Empty;
			StreamReader streamReader = null;
			try
			{
				streamReader = new StreamReader(path);
				text2 = streamReader.ReadToEnd();
			}
			catch
			{
				Debug.LogError("ERROR - Login - Read user data file Wrong!!!");
			}
			finally
			{
				if (streamReader != null)
				{
					streamReader.Close();
				}
			}
			if (text2.Length < 1)
			{
				result = false;
			}
		}
		else
		{
			result = false;
		}
		return result;
	}

	public void SetupPreLoadingUI(bool bShow)
	{
		if (m_uiGroup != null)
		{
			m_uiGroup.Clear();
			m_uiGroup = null;
		}
		if (bShow)
		{
			m_uiGroup = new uiGroup(m_UIManager);
			UIImage control = UIUtils.BuildImage(0, new Rect(0f, 0f, 960f, 640f), m_MatLoginBg, new Rect(0f, 0f, 960f, 640f), new Vector2(960f, 640f));
			m_uiGroup.Add(control);
			m_WheelAnimLeft = UIUtils.BuildImage(0, new Rect(95f, 50f, 138f, 138f), m_MatLoginBg, new Rect(777f, 642f, 138f, 138f), new Vector2(138f, 138f));
			m_uiGroup.Add(m_WheelAnimLeft);
			m_WheelAnimRight = UIUtils.BuildImage(0, new Rect(735f, 50f, 138f, 138f), m_MatLoginBg, new Rect(777f, 642f, 138f, 138f), new Vector2(138f, 138f));
			m_uiGroup.Add(m_WheelAnimRight);
			control = UIUtils.BuildImage(0, new Rect(111f, 42f, 775f, 205f), m_MatLoginBg, new Rect(0f, 642f, 775f, 205f), new Vector2(775f, 205f));
			m_uiGroup.Add(control);
		}
	}

	public void SetupGameLoginUI(bool bShow)
	{
		if (m_uiGroup != null)
		{
			m_uiGroup.Clear();
			m_uiGroup = null;
		}
		if (bShow)
		{
			m_uiGroup = new uiGroup(m_UIManager);
			UIImage control = UIUtils.BuildImage(0, new Rect(0f, 0f, 960f, 640f), m_MatLoginBg, new Rect(0f, 0f, 960f, 640f), new Vector2(960f, 640f));
			m_uiGroup.Add(control);
			m_WheelAnimLeft = UIUtils.BuildImage(0, new Rect(95f, 50f, 138f, 138f), m_MatLoginBg, new Rect(777f, 642f, 138f, 138f), new Vector2(138f, 138f));
			m_uiGroup.Add(m_WheelAnimLeft);
			m_WheelAnimRight = UIUtils.BuildImage(0, new Rect(735f, 50f, 138f, 138f), m_MatLoginBg, new Rect(777f, 642f, 138f, 138f), new Vector2(138f, 138f));
			m_uiGroup.Add(m_WheelAnimRight);
			control = UIUtils.BuildImage(0, new Rect(111f, 42f, 775f, 205f), m_MatLoginBg, new Rect(0f, 642f, 775f, 205f), new Vector2(775f, 205f));
			m_uiGroup.Add(control);
			control = UIUtils.BuildImage(0, new Rect(234f, 156f, 518f, 307f), m_MatLoginUI, new Rect(0f, 0f, 518f, 307f), new Vector2(518f, 307f));
			m_uiGroup.Add(control);
			UIText uIText = UIUtils.BuildUIText(0, new Rect(371f, 405f, 250f, 36f), UIText.enAlignStyle.left);
			uIText.Set("Zombie3D/Font/037-CAI978-27", "LOGIN WITH", new Color(0.81960785f, 0.5294118f, 2f / 51f, 1f));
			m_uiGroup.Add(uIText);
			UISelectButton uISelectButton = null;
			uISelectButton = UIUtils.BuildSelectButton(1002, new Rect(429f, 253f, 109f, 108f), m_MatLoginUI, new Rect(218f, 307f, 109f, 108f), new Rect(218f, 415f, 109f, 108f), new Rect(218f, 307f, 109f, 108f), new Vector2(109f, 108f));
			m_uiGroup.Add(uISelectButton);
			uIText = UIUtils.BuildUIText(0, new Rect(410f, 230f, 200f, 20f), UIText.enAlignStyle.left);
			uIText.Set("Zombie3D/Font/037-CAI978-15", "LOCAL GAME", new Color(0.81960785f, 0.5294118f, 2f / 51f, 1f));
			m_uiGroup.Add(uIText);
			selectedLoginType = 1002;
			UIClickButton control2 = UIUtils.BuildClickButton(1003, new Rect(390f, 138f, 191f, 62f), m_MatLoginUI, new Rect(327f, 307f, 191f, 62f), new Rect(327f, 370f, 191f, 62f), new Rect(327f, 307f, 191f, 62f), new Vector2(191f, 62f));
			m_uiGroup.Add(control2);
		}
	}

	public void SetupAppNewVersionUI(bool bShow)
	{
		if (m_uiGroup != null)
		{
			m_uiGroup.Clear();
			m_uiGroup = null;
		}
		if (bShow)
		{
			m_uiGroup = new uiGroup(m_UIManager);
			UIImage control = UIUtils.BuildImage(0, new Rect(0f, 0f, 960f, 640f), m_MatLoginBg, new Rect(0f, 0f, 960f, 640f), new Vector2(960f, 640f));
			m_uiGroup.Add(control);
			m_WheelAnimLeft = UIUtils.BuildImage(0, new Rect(95f, 50f, 138f, 138f), m_MatLoginBg, new Rect(777f, 642f, 138f, 138f), new Vector2(138f, 138f));
			m_uiGroup.Add(m_WheelAnimLeft);
			m_WheelAnimRight = UIUtils.BuildImage(0, new Rect(735f, 50f, 138f, 138f), m_MatLoginBg, new Rect(777f, 642f, 138f, 138f), new Vector2(138f, 138f));
			m_uiGroup.Add(m_WheelAnimRight);
			control = UIUtils.BuildImage(0, new Rect(111f, 42f, 775f, 205f), m_MatLoginBg, new Rect(0f, 642f, 775f, 205f), new Vector2(775f, 205f));
			m_uiGroup.Add(control);
			control = UIUtils.BuildImage(0, new Rect(234f, 156f, 518f, 307f), m_MatLoginUI, new Rect(0f, 0f, 518f, 307f), new Vector2(518f, 307f));
			m_uiGroup.Add(control);
			UIText uIText = UIUtils.BuildUIText(0, new Rect(340f, 410f, 400f, 30f), UIText.enAlignStyle.left);
			uIText.Set("Zombie3D/Font/037-CAI978-22", "New Update Available", Constant.TextCommonColor);
			m_uiGroup.Add(uIText);
			uIText = UIUtils.BuildUIText(0, new Rect(300f, 300f, 400f, 40f), UIText.enAlignStyle.left);
			uIText.Set("Zombie3D/Font/037-CAI978-15", "Please download the update in order to play this game online. Download now!", Constant.TextCommonColor);
			m_uiGroup.Add(uIText);
			UIClickButton control2 = UIUtils.BuildClickButton(1004, new Rect(520f, 138f, 191f, 62f), m_MatLoginUI, new Rect(327f, 307f, 191f, 62f), new Rect(327f, 370f, 191f, 62f), new Rect(327f, 307f, 191f, 62f), new Vector2(191f, 62f));
			m_uiGroup.Add(control2);
			control2 = UIUtils.BuildClickButton(1005, new Rect(260f, 138f, 191f, 62f), m_MatLoginUI, new Rect(327f, 433f, 191f, 62f), new Rect(327f, 496f, 191f, 62f), new Rect(327f, 433f, 191f, 62f), new Vector2(191f, 62f));
			m_uiGroup.Add(control2);
		}
	}

	public void SetupNetworkConnectUI(bool bShow)
	{
		if (m_uiGroup != null)
		{
			m_uiGroup.Clear();
			m_uiGroup = null;
		}
		if (bShow)
		{
			m_uiGroup = new uiGroup(m_UIManager);
			UIImage control = UIUtils.BuildImage(0, new Rect(0f, 0f, 960f, 640f), m_MatLoginBg, new Rect(0f, 0f, 960f, 640f), new Vector2(960f, 640f));
			m_uiGroup.Add(control);
			m_WheelAnimLeft = UIUtils.BuildImage(0, new Rect(95f, 50f, 138f, 138f), m_MatLoginBg, new Rect(777f, 642f, 138f, 138f), new Vector2(138f, 138f));
			m_uiGroup.Add(m_WheelAnimLeft);
			m_WheelAnimRight = UIUtils.BuildImage(0, new Rect(735f, 50f, 138f, 138f), m_MatLoginBg, new Rect(777f, 642f, 138f, 138f), new Vector2(138f, 138f));
			m_uiGroup.Add(m_WheelAnimRight);
			control = UIUtils.BuildImage(0, new Rect(111f, 42f, 775f, 205f), m_MatLoginBg, new Rect(0f, 642f, 775f, 205f), new Vector2(775f, 205f));
			m_uiGroup.Add(control);
			control = UIUtils.BuildImage(0, new Rect(234f, 156f, 518f, 307f), m_MatLoginUI, new Rect(0f, 0f, 518f, 307f), new Vector2(518f, 307f));
			m_uiGroup.Add(control);
			UIText uIText = UIUtils.BuildUIText(0, new Rect(450f, 405f, 250f, 36f), UIText.enAlignStyle.left);
			uIText.Set("Zombie3D/Font/037-CAI978-27", "NOTICE", Constant.TextCommonColor);
			m_uiGroup.Add(uIText);
			uIText = UIUtils.BuildUIText(0, new Rect(300f, 250f, 400f, 100f), UIText.enAlignStyle.left);
			uIText.Set("Zombie3D/Font/037-CAI978-15", "Do you want to connect to a network?", Constant.TextCommonColor);
			m_uiGroup.Add(uIText);
			UIClickButton control2 = UIUtils.BuildClickButton(1006, new Rect(520f, 138f, 191f, 62f), m_MatLoginUI, new Rect(327f, 307f, 191f, 62f), new Rect(327f, 370f, 191f, 62f), new Rect(327f, 307f, 191f, 62f), new Vector2(191f, 62f));
			m_uiGroup.Add(control2);
			control2 = UIUtils.BuildClickButton(1007, new Rect(260f, 138f, 191f, 62f), m_MatLoginUI, new Rect(518f, 124f, 191f, 62f), new Rect(710f, 124f, 191f, 62f), new Rect(518f, 124f, 191f, 62f), new Vector2(191f, 62f));
			m_uiGroup.Add(control2);
		}
	}

	public void SetupLoadingUI(bool bShow, string content = "")
	{
		if (m_LoadingUI != null)
		{
			m_LoadingUI.Clear();
			m_LoadingUI = null;
		}
		if (bShow)
		{
			m_LoadingUI = new uiGroup(m_UIManager);
			UIImage control = UIUtils.BuildImage(0, new Rect(0f, 0f, 960f, 640f), m_MatLoginUI, new Rect(1f, 581f, 1f, 1f), new Vector2(960f, 640f));
			m_LoadingUI.Add(control);
			Material material = LoadUIMaterial("Zombie3D/UI/Materials/MacOSLoadingUI");
			UIAnimationControl uIAnimationControl = new UIAnimationControl();
			uIAnimationControl.Id = 0;
			uIAnimationControl.SetAnimationsPageCount(8);
			uIAnimationControl.Rect = AutoUI.AutoRect(new Rect(454f, 294f, 52f, 52f));
			uIAnimationControl.SetTexture(0, material, AutoUI.AutoRect(new Rect(0f, 0f, 52f, 52f)), AutoUI.AutoSize(new Vector2(52f, 52f)));
			uIAnimationControl.SetTexture(1, material, AutoUI.AutoRect(new Rect(52f, 0f, 52f, 52f)), AutoUI.AutoSize(new Vector2(52f, 52f)));
			uIAnimationControl.SetTexture(2, material, AutoUI.AutoRect(new Rect(104f, 0f, 52f, 52f)), AutoUI.AutoSize(new Vector2(52f, 52f)));
			uIAnimationControl.SetTexture(3, material, AutoUI.AutoRect(new Rect(156f, 0f, 52f, 52f)), AutoUI.AutoSize(new Vector2(52f, 52f)));
			uIAnimationControl.SetTexture(4, material, AutoUI.AutoRect(new Rect(208f, 0f, 52f, 52f)), AutoUI.AutoSize(new Vector2(52f, 52f)));
			uIAnimationControl.SetTexture(5, material, AutoUI.AutoRect(new Rect(260f, 0f, 52f, 52f)), AutoUI.AutoSize(new Vector2(52f, 52f)));
			uIAnimationControl.SetTexture(6, material, AutoUI.AutoRect(new Rect(312f, 0f, 52f, 52f)), AutoUI.AutoSize(new Vector2(52f, 52f)));
			uIAnimationControl.SetTexture(7, material, AutoUI.AutoRect(new Rect(364f, 0f, 52f, 52f)), AutoUI.AutoSize(new Vector2(52f, 52f)));
			uIAnimationControl.SetTimeInterval(0.1f);
			uIAnimationControl.SetLoopCount(1000000);
			m_LoadingUI.Add(uIAnimationControl);
		}
	}

	public void SetupCartoonUI(bool bShow)
	{
//		Handheld.PlayFullScreenMovie("Story2.mp4", Color.black, FullScreenMovieControlMode.Hidden);
		GameApp.GetInstance().GetGameState().SetGameTriggerInfo(1, 1, 1);
		m_bBeginBattle = true;
		m_GameLoadingUI = new GameLoadingUI();
		m_GameLoadingUI.SetupLoadingUI(true, m_UIManager, m_MatCommonBg);
	}

	public void SetupNotificationDialogUI(bool bShow, string strContent)
	{
		if (m_DialogNoticeUIGroup != null)
		{
			m_DialogNoticeUIGroup.Clear();
			m_DialogNoticeUIGroup = null;
		}
		if (bShow)
		{
			m_DialogNoticeUIGroup = new uiGroup(m_UIManager);
			UIImage control = UIUtils.BuildImage(0, new Rect(0f, 0f, 960f, 640f), m_MatDialog01, new Rect(600f, 1f, 1f, 1f), new Vector2(960f, 640f));
			m_DialogNoticeUIGroup.Add(control);
			float left = 242f;
			float top = 232f;
			control = UIUtils.BuildImage(0, new Rect(left, top, 515f, 301f), m_MatDialog01, new Rect(0f, 378f, 515f, 301f), new Vector2(515f, 301f));
			m_DialogNoticeUIGroup.Add(control);
			UIText uIText = UIUtils.BuildUIText(0, new Rect(290f, 295f, 420f, 150f), UIText.enAlignStyle.left);
			uIText.Set("Zombie3D/Font/037-CAI978-18", strContent, Constant.TextCommonColor);
			m_DialogNoticeUIGroup.Add(uIText);
			UIClickButton uIClickButton = null;
			uIClickButton = UIUtils.BuildClickButton(1009, new Rect(385f, 215f, 191f, 62f), m_MatDialog01, new Rect(640f, 0f, 191f, 62f), new Rect(832f, 0f, 191f, 62f), new Rect(640f, 0f, 191f, 62f), new Vector2(191f, 62f));
			m_DialogNoticeUIGroup.Add(uIClickButton);
		}
	}

	public Material LoadUIMaterial(string name)
	{
		if (AutoUI.IsRetain == AutoUI.RESOLUTION.LOWDEFINITION)
		{
			name += "_LOW";
		}
		Material material = Resources.Load(name) as Material;
		if (material == null)
		{
			Debug.Log("load material error: " + name);
		}
		return material;
	}
}
