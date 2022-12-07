using System.Collections.Generic;
using System.Text.RegularExpressions;
using Sfs2X.Entities;
using Sfs2X.Entities.Managers;
using TNetSdk;
using UnityEngine;
using Zombie3D;

public class NNetworkUIScript : MonoBehaviour, UIHandler
{
	public enum NSuccessedCMD
	{
		E_CONNECTED = 0,
		E_LOGIN = 1,
		E_JoinRoom = 2,
		E_PVPLeaveRoom = 3
	}

	public enum NUIState
	{
		E_NONE = 0,
		E_CREATENAME = 1,
		E_SELECTMODE = 2,
		E_MODEINTRODUCE = 3,
		E_LOBBY = 4,
		E_CREATEROOM = 5,
		E_ROOM = 6
	}

	public enum Controls
	{
		kIDLevels = 14000,
		kIDMapOline = 14001,
		kIDNBack = 14002,
		kIDSave = 14003,
		kIDSureModeOK = 14004,
		kIDEditName = 14005,
		kIDInputOpen = 14006,
		kIDLastStandingModel_Team = 14007,
		kIDLastStandingModel_Solo = 14008,
		kIDDeathMatch_Team = 14009,
		kIDDeathMatch_Solo = 14010,
		kIDNCreateRoom = 14011,
		kIDNRefreshRoomList = 14012,
		kIDNTeamPlay = 14013,
		kIDNSimplePlay = 14014,
		kIDNStartGame = 14015,
		kIDNMessageBoxOK = 14016,
		kIDNRoomIndexBegin = 14017,
		kIDNRoomIndexEnd = 14272,
		kIDNRoomSeatIndexBegin = 14273,
		kIDNRoomSeatIndexEnd = 14283,
		kIDLast = 14284
	}

	private UIManager m_UIManager;

	private string m_ui_material_path;

	private GameState gameState;

	protected float lastUpdateTime;

	public GameLoadingUI m_GameLoadingUI;

	protected bool m_bBeginBattle;

	protected float m_BeginBattleTimer;

	private int mapIndex = 1;

	private int pointsIndex = 1;

	private int waveIndex = 1;

	protected Material m_MatCommonBg;

	protected Material m_MatNetWorkUI;

	protected Material m_MatAvatarIcons;

	protected Material m_MatRoomUI;

	protected Material m_MatNDialog01;

	protected Material m_MatNEditName;

	public uiGroup m_Dialog01UI;

	public uiGroup m_WaittingServerMsgGroup;

	public uiGroup m_CreateNameGroup;

	public uiGroup m_SelectModeGroup;

	public uiGroup m_IntroduceModeGroup;

	public uiGroup m_CreateRoomGroup;

	public uiGroup m_LobbyGroup;

	public uiGroup m_RoomListGroup;

	public uiGroup m_RoomGroup;

	public int m_iMyRoomSeatIndex;

	public bool m_bShowRoomBackBtn;

	private UIScrollView m_RoomListView;

	private UIDotScrollBar m_RoomListViewScrollBar;

	private UIText m_textName;

	private TouchScreenKeyboard m_iphoneKeyboard;

	protected Regex myRex;

	public string m_strName = string.Empty;

	private int NameMaxLength = 6;

	public NUIState m_eNState;

	public NLoginScript m_nLogin;

	public List<Room> m_lsRoomList;

	private void Start()
	{
		m_UIManager = base.gameObject.AddComponent<UIManager>() as UIManager;
		m_UIManager.SetParameter(8, 3, false);
		m_UIManager.SetUIHandler(this);
		UIUtils.BuildIpone5Frame(m_UIManager);
		m_nLogin = base.gameObject.AddComponent(typeof(NLoginScript)) as NLoginScript;
		m_MatAvatarIcons = LoadUIMaterial("Zombie3D/UI/Materials/AvatarIcons");
		m_MatNetWorkUI = LoadUIMaterial("Zombie3D/UI/Materials/NLobbyUI");
		m_MatCommonBg = LoadUIMaterial("Zombie3D/UI/Materials/NNetworkBackGround");
		m_MatRoomUI = LoadUIMaterial("Zombie3D/UI/Materials/NLobbySeatBackground");
		m_MatNDialog01 = null;
		m_MatNEditName = null;
		Resources.UnloadUnusedAssets();
		gameState = GameApp.GetInstance().GetGameState();
		gameState.m_bBattleIsBegin = false;
		if (gameState.NetPlayerName == string.Empty)
		{
			SetupNetworkUIGroup(NUIState.E_CREATENAME);
		}
		else
		{
			m_nLogin.Connect();
			SetupNWaitServerMsgUI(true);
		}
		myRex = new Regex("^[A-Za-z0-9]+$");
		OpenClickPlugin.Hide();
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
		if (Time.time - lastUpdateTime < 0.001f)
		{
			return;
		}
		float num = Time.time - lastUpdateTime;
		lastUpdateTime = Time.time;
		if (!m_bBeginBattle)
		{
			return;
		}
		m_BeginBattleTimer += num;
		if (m_BeginBattleTimer > 0.1f)
		{
			GameCollectionInfoManager.Instance().GetCurrentInfo().UpdatePointsInfo(mapIndex, pointsIndex, waveIndex);
			if (mapIndex == 201)
			{
				Debug.Log("LoadLevel | NETMODE_MAP1INDEX");
				GameApp.GetInstance().SetLoadMap(true);
				Application.LoadLevel("Zombie3D_Judgement_Map15");
			}
			else if (mapIndex == 202)
			{
				Debug.Log("LoadLevel | NETMODE_MAP2INDEX");
				GameApp.GetInstance().SetLoadMap(true);
				Application.LoadLevel("Zombie3D_Judgement_Map16");
			}
			else if (mapIndex == 203)
			{
				Debug.Log("LoadLevel | NETMODE_MAP3INDEX");
				GameApp.GetInstance().SetLoadMap(true);
				Application.LoadLevel("Zombie3D_Judgement_Map17");
			}
			else if (mapIndex == 204)
			{
				Debug.Log("LoadLevel | NETMODE_MAP4INDEX");
				GameApp.GetInstance().SetLoadMap(true);
				Application.LoadLevel("Zombie3D_Judgement_Map18");
			}
			if (gameState.m_eGameMode.m_ePlayMode == GameState.NetworkGameMode.PlayMode.E_Console)
			{
				SceneUIManager.Instance().ChangeSceneUI(SceneUIManager.SceneUI.BattleUI);
			}
			else
			{
				SceneUIManager.Instance().ChangeSceneUI(SceneUIManager.SceneUI.NBattleUI, false);
			}
			m_bBeginBattle = false;
		}
	}

	public void HandleEvent(UIControl control, int command, float wparam, float lparam)
	{
		if ((control.GetType() == typeof(UIClickButton) || control.GetType() == typeof(UISelectButton)) && GameApp.GetInstance().GetGameState().SoundOn)
		{
			SceneUIManager.Instance().PlayClickAudio();
		}
		if (control.Id == 14016)
		{
			SetupMessageBoxUI(false, string.Empty);
		}
		else if (control.Id == 14003)
		{
			Match match = myRex.Match(m_iphoneKeyboard.text);
			if (match.Success && m_iphoneKeyboard.text.Length >= 1)
			{
				if (m_iphoneKeyboard.text.Length > NameMaxLength)
				{
					m_iphoneKeyboard.text = m_iphoneKeyboard.text.Substring(0, NameMaxLength);
					m_strName = m_strName.Substring(0, NameMaxLength);
				}
				gameState.NetPlayerName = m_iphoneKeyboard.text;
				GameApp.GetInstance().Save();
				m_nLogin.Connect();
				SetupNWaitServerMsgUI(true);
			}
			else
			{
				m_iphoneKeyboard.text = string.Empty;
				m_strName = m_iphoneKeyboard.text;
				SetupNName(m_strName);
				SetupMessageBoxUI(true, "Names can only contain letters and numbers!");
				Debug.Log("name error!");
			}
		}
		else if (control.Id == 14006)
		{
			m_iphoneKeyboard = TouchScreenKeyboard.Open(m_strName);
		}
		else if (control.Id == 14005)
		{
			m_nLogin.shuttingDown = true;
			m_nLogin.DisConnect();
			SetupNetworkUIGroup(NUIState.E_CREATENAME);
		}
		else if (control.Id == 14009)
		{
			gameState.m_eGameMode.m_ePlayMode = GameState.NetworkGameMode.PlayMode.E_DeathMatch;
			gameState.m_eGameMode.m_eCooperaMode = GameState.NetworkGameMode.NetworkCooperationMode.E_Team;
			SetupNWaitServerMsgUI(true);
			m_nLogin.ReqAutoJoinRoom();
		}
		else if (control.Id == 14010)
		{
			gameState.m_eGameMode.m_ePlayMode = GameState.NetworkGameMode.PlayMode.E_DeathMatch;
			gameState.m_eGameMode.m_eCooperaMode = GameState.NetworkGameMode.NetworkCooperationMode.E_Simple;
			SetupNWaitServerMsgUI(true);
			m_nLogin.ReqAutoJoinRoom();
		}
		else if (control.Id == 14007)
		{
			gameState.m_eGameMode.m_ePlayMode = GameState.NetworkGameMode.PlayMode.E_LastStand;
			gameState.m_eGameMode.m_eCooperaMode = GameState.NetworkGameMode.NetworkCooperationMode.E_Team;
			SetupNWaitServerMsgUI(true);
			m_nLogin.ReqAutoJoinRoom();
		}
		else if (control.Id == 14008)
		{
			gameState.m_eGameMode.m_ePlayMode = GameState.NetworkGameMode.PlayMode.E_LastStand;
			gameState.m_eGameMode.m_eCooperaMode = GameState.NetworkGameMode.NetworkCooperationMode.E_Simple;
			SetupNWaitServerMsgUI(true);
			m_nLogin.ReqAutoJoinRoom();
		}
		else if (control.Id == 14011)
		{
			SetupNetworkUIGroup(NUIState.E_CREATEROOM);
		}
		else
		{
			if (control.Id == 14013 || control.Id == 14014)
			{
				return;
			}
			if (control.Id == 14012)
			{
				SetRoomList(true);
			}
			else if (control.Id == 14004)
			{
				SetupNWaitServerMsgUI(true);
			}
			else if (control.Id == 14002)
			{
				switch (m_eNState)
				{
				case NUIState.E_CREATENAME:
					if (gameState.NetPlayerName == string.Empty)
					{
						SetupNetworkUIGroup(NUIState.E_NONE);
						SceneUIManager.Instance().ChangeSceneUI(SceneUIManager.SceneUI.ChoosePointsUI);
						gameState.m_eGameMode.m_ePlayMode = GameState.NetworkGameMode.PlayMode.E_Console;
						m_nLogin.DisConnect();
					}
					else
					{
						m_nLogin.Connect();
						SetupNWaitServerMsgUI(true);
					}
					break;
				case NUIState.E_SELECTMODE:
					SetupNetworkUIGroup(NUIState.E_NONE);
					SceneUIManager.Instance().ChangeSceneUI(SceneUIManager.SceneUI.ChoosePointsUI);
					gameState.m_eGameMode.m_ePlayMode = GameState.NetworkGameMode.PlayMode.E_Console;
					m_nLogin.DisConnect();
					break;
				case NUIState.E_MODEINTRODUCE:
					SetupNetworkUIGroup(NUIState.E_SELECTMODE);
					break;
				case NUIState.E_LOBBY:
					SetupNetworkUIGroup(NUIState.E_SELECTMODE);
					break;
				case NUIState.E_CREATEROOM:
					SetupNetworkUIGroup(NUIState.E_LOBBY);
					break;
				case NUIState.E_ROOM:
					if (gameState.m_eGameMode.m_ePlayMode == GameState.NetworkGameMode.PlayMode.E_PVE_BossRush)
					{
						SetupNetworkUIGroup(NUIState.E_NONE);
						SceneUIManager.Instance().ChangeSceneUI(SceneUIManager.SceneUI.ChoosePointsUI);
						gameState.m_eGameMode.m_ePlayMode = GameState.NetworkGameMode.PlayMode.E_Console;
						m_nLogin.DisConnect();
					}
					else
					{
						m_nLogin.ReqLeaveroom();
						SetupNWaitServerMsgUI(true);
					}
					break;
				}
			}
			else if (control.Id >= 14017 && control.Id <= 14272)
			{
				int num = control.Id - 14017;
			}
			else if (control.Id == 14015)
			{
				int[] array = new int[2] { 201, 202 };
				int num2 = Random.Range(0, array.Length);
				m_nLogin.ReqStartGame(array[0]);
			}
		}
	}

	public void TurnToChooseMap()
	{
		m_eNState = NUIState.E_NONE;
		SetupNWaitServerMsgUI(false);
		SetupNCreateNameUI(false);
		SetupNSelectModeUI(false);
		SetupNModeIntroduce(false);
		SetupNLobbyUI(false);
		SetRoomList(false);
		SetupNCreateRoomUI(false);
		SetupNRoomUI(false);
	}

	public void StartGame(int level)
	{
		Resources.UnloadUnusedAssets();
		mapIndex = level;
		gameState.SetGameTriggerInfo(level, 1, 1);
		m_bBeginBattle = true;
		m_GameLoadingUI = new GameLoadingUI();
		m_GameLoadingUI.SetupLoadingUI(true, m_UIManager, m_MatCommonBg);
		Debug.Log("StartGame(" + level + ")");
	}

	private void OnGUI()
	{
		if (m_eNState == NUIState.E_CREATENAME && m_iphoneKeyboard != null)
		{
			m_strName = m_iphoneKeyboard.text;
			Debug.Log("!!!!!!!!!!!!" + m_strName);
			if (m_iphoneKeyboard.done)
			{
				SetupNName(m_strName);
			}
		}
	}

	public void SetupNetworkUIGroup(NUIState state)
	{
		if (m_eNState != state)
		{
			NUIState eNState = m_eNState;
			if (eNState == NUIState.E_CREATEROOM && state == NUIState.E_ROOM)
			{
				NetworkUIShow(NUIState.E_LOBBY, false);
			}
			NetworkUIShow(eNState, false);
			m_eNState = state;
			NetworkUIShow(state, true);
		}
		else
		{
			Debug.Log("Else here" + state);
			NetworkUIShow(state, true);
		}
	}

	public void NetworkUIShow(NUIState state, bool bShow)
	{
		switch (state)
		{
		case NUIState.E_NONE:
			break;
		case NUIState.E_CREATENAME:
			SetupNCreateNameUI(bShow);
			break;
		case NUIState.E_SELECTMODE:
			SetupNSelectModeUI(bShow);
			break;
		case NUIState.E_MODEINTRODUCE:
			SetupNModeIntroduce(bShow);
			break;
		case NUIState.E_LOBBY:
			SetupNLobbyUI(bShow);
			SetRoomList(bShow);
			break;
		case NUIState.E_CREATEROOM:
			SetupNCreateRoomUI(bShow);
			break;
		case NUIState.E_ROOM:
			SetupNRoomUI(bShow);
			break;
		}
	}

	public void SetupMessageBoxUI(bool bShow, string context)
	{
		if (m_Dialog01UI != null)
		{
			m_Dialog01UI.Clear();
			m_Dialog01UI = null;
		}
		if (bShow)
		{
			m_Dialog01UI = new uiGroup(m_UIManager);
			UIBlock uIBlock = new UIBlock();
			uIBlock.Rect = new Rect(0f, 0f, 960f, 640f);
			m_Dialog01UI.Add(uIBlock);
			UIImage control = UIUtils.BuildImage(0, new Rect(0f, 0f, 960f, 640f), m_MatCommonBg, new Rect(1016f, 1f, 1f, 1f), new Vector2(960f, 640f));
			m_Dialog01UI.Add(control);
			control = UIUtils.BuildImage(0, new Rect(242f, 232f, 516f, 302f), m_MatCommonBg, new Rect(2f, 645f, 516f, 302f), new Vector2(516f, 302f));
			m_Dialog01UI.Add(control);
			float top = 390f;
			if (context.Length < 50)
			{
				top = 350f;
			}
			UIText uIText = UIUtils.BuildUIText(0, new Rect(333f, top, 340f, 30f), UIText.enAlignStyle.left);
			uIText.Set("Zombie3D/Font/037-CAI978-18", context, Constant.TextCommonColor);
			m_Dialog01UI.Add(uIText);
			UIClickButton control2 = UIUtils.BuildClickButton(14016, new Rect(410f, 216f, 192f, 62f), m_MatCommonBg, new Rect(521f, 645f, 192f, 62f), new Rect(716f, 645f, 192f, 62f), new Rect(521f, 645f, 192f, 62f), new Vector2(192f, 62f));
			m_Dialog01UI.Add(control2);
		}
	}

	public void SetupNWaitServerMsgUI(bool bShow)
	{
		if (m_WaittingServerMsgGroup != null)
		{
			m_WaittingServerMsgGroup.Clear();
			m_WaittingServerMsgGroup = null;
		}
		if (bShow)
		{
			m_WaittingServerMsgGroup = new uiGroup(m_UIManager);
			Material material = LoadUIMaterial("Zombie3D/UI/Materials/MacOSLoadingUI");
			UIImage control = UIUtils.BuildImage(0, new Rect(0f, 0f, 960f, 640f), material, new Rect(2f, 58f, 1f, 1f), new Vector2(960f, 640f));
			m_WaittingServerMsgGroup.Add(control);
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
			m_WaittingServerMsgGroup.Add(uIAnimationControl);
		}
	}

	public void SetupNName(string name)
	{
		Debug.Log("SetupNName|" + name + "|||" + (m_textName == null));
		if (name.Length > NameMaxLength)
		{
			name = name.Substring(0, NameMaxLength);
		}
		if (m_CreateNameGroup != null)
		{
			Debug.Log("SetupNName|" + name + "|||" + (m_textName == null));
			if (m_textName == null)
			{
				m_textName = UIUtils.BuildUIText(0, new Rect(365f, 390f, 237f, 44f), UIText.enAlignStyle.center);
				m_textName.Set("Zombie3D/Font/037-CAI978-22", name, Constant.TextCommonColor);
				m_CreateNameGroup.Add(m_textName);
			}
			else
			{
				m_textName.SetText(name);
				m_CreateNameGroup.Add(m_textName);
			}
		}
		else
		{
			Debug.Log("m_CreateNameGroup is null");
			m_CreateNameGroup = new uiGroup(m_UIManager);
			m_textName = null;
			m_textName = UIUtils.BuildUIText(0, new Rect(415f, 380f, 237f, 44f), UIText.enAlignStyle.center);
			m_textName.Set("Zombie3D/Font/037-CAI978-22", name, Constant.TextCommonColor);
			m_CreateNameGroup.Add(m_textName);
		}
	}

	public void SetupNCreateNameUI(bool bShow)
	{
		if (m_CreateNameGroup != null)
		{
			m_CreateNameGroup.Clear();
			m_CreateNameGroup = null;
		}
		if (bShow)
		{
			m_CreateNameGroup = new uiGroup(m_UIManager);
			m_strName = string.Empty;
			m_iphoneKeyboard = TouchScreenKeyboard.Open(m_strName);
			UIImage control = UIUtils.BuildImage(0, new Rect(0f, 0f, 960f, 640f), m_MatCommonBg, new Rect(0f, 0f, 960f, 640f), new Vector2(960f, 640f));
			m_CreateNameGroup.Add(control);
			SetupNBackGroundAround(m_CreateNameGroup);
			if (m_MatNEditName == null)
			{
				m_MatNEditName = LoadUIMaterial("Zombie3D/UI/Materials/NNetworkEditName");
				Resources.UnloadUnusedAssets();
			}
			control = UIUtils.BuildImage(0, new Rect(150f, 180f, 666f, 314f), m_MatNEditName, new Rect(2f, 2f, 666f, 314f), new Vector2(666f, 314f));
			m_CreateNameGroup.Add(control);
			control = UIUtils.BuildImage(0, new Rect(260f, 470f, 409f, 77f), m_MatNEditName, new Rect(0f, 410f, 544f, 104f), new Vector2(409f, 77f));
			m_CreateNameGroup.Add(control);
			UIClickButton uIClickButton = null;
			uIClickButton = UIUtils.BuildClickButton(14003, new Rect(679f, 15f, 187f, 68f), m_MatCommonBg, new Rect(521f, 645f, 192f, 62f), new Rect(716f, 645f, 192f, 62f), new Rect(521f, 645f, 192f, 62f), new Vector2(192f, 62f));
			m_CreateNameGroup.Add(uIClickButton);
			uIClickButton = UIUtils.BuildClickButton(14006, new Rect(415f, 402f, 237f, 44f), m_MatNEditName, new Rect(0f, 0f, 1f, 1f), new Rect(1f, 1f, 1f, 1f), new Rect(1f, 1f, 1f, 1f), new Vector2(237f, 44f));
			m_CreateNameGroup.Add(uIClickButton);
			uIClickButton = UIUtils.BuildClickButton(14002, new Rect(55f, 15f, 187f, 68f), m_MatCommonBg, new Rect(521f, 710f, 188f, 68f), new Rect(712f, 710f, 188f, 68f), new Rect(521f, 710f, 188f, 68f), new Vector2(188f, 68f));
			m_CreateNameGroup.Add(uIClickButton);
		}
	}

	public void SetupNSelectModeUI(bool bShow)
	{
		if (m_SelectModeGroup != null)
		{
			m_SelectModeGroup.Clear();
			m_SelectModeGroup = null;
		}
		if (bShow)
		{
			m_SelectModeGroup = new uiGroup(m_UIManager);
			UIText uIText = null;
			UIImage control = UIUtils.BuildImage(0, new Rect(0f, 0f, 960f, 640f), m_MatCommonBg, new Rect(0f, 0f, 960f, 640f), new Vector2(960f, 640f));
			m_SelectModeGroup.Add(control);
			SetupNBackGroundAround(m_SelectModeGroup);
			UIClickButton uIClickButton = null;
			control = UIUtils.BuildImage(0, new Rect(25f, 280f, 893f, 258f), m_MatNetWorkUI, new Rect(0f, 0f, 893f, 258f), new Vector2(893f, 258f));
			m_SelectModeGroup.Add(control);
			uIClickButton = UIUtils.BuildClickButton(14009, new Rect(180f, 300f, 201f, 142f), m_MatNetWorkUI, new Rect(407f, 403f, 201f, 142f), new Rect(609f, 403f, 201f, 142f), new Rect(407f, 403f, 201f, 142f), new Vector2(201f, 142f));
			m_SelectModeGroup.Add(uIClickButton);
			uIClickButton = UIUtils.BuildClickButton(14010, new Rect(600f, 300f, 201f, 142f), m_MatNetWorkUI, new Rect(407f, 259f, 201f, 142f), new Rect(609f, 259f, 201f, 142f), new Rect(407f, 259f, 201f, 142f), new Vector2(201f, 142f));
			m_SelectModeGroup.Add(uIClickButton);
			uIClickButton = UIUtils.BuildClickButton(14002, new Rect(55f, 15f, 187f, 68f), m_MatCommonBg, new Rect(521f, 710f, 188f, 68f), new Rect(712f, 710f, 188f, 68f), new Rect(521f, 710f, 188f, 68f), new Vector2(188f, 68f));
			m_SelectModeGroup.Add(uIClickButton);
			uIClickButton = UIUtils.BuildClickButton(14005, new Rect(635f, 15f, 275f, 68f), m_MatNetWorkUI, new Rect(0f, 636f, 275f, 68f), new Rect(0f, 567f, 275f, 68f), new Rect(0f, 636f, 275f, 68f), new Vector2(275f, 68f));
			m_SelectModeGroup.Add(uIClickButton);
			control = UIUtils.BuildImage(0, new Rect(240f, 130f, 494f, 139f), m_MatNetWorkUI, new Rect(277f, 567f, 494f, 139f), new Vector2(494f, 139f));
			m_SelectModeGroup.Add(control);
			int avatarHeadID = gameState.GetAvatarHeadID();
			Rect avatarIconTexture = ShopUIScript.GetAvatarIconTexture((Avatar.AvatarSuiteType)avatarHeadID, Avatar.AvatarType.Head);
			control = UIUtils.BuildImage(0, new Rect(300f, 170f, 90f, 80f), m_MatAvatarIcons, avatarIconTexture, new Vector2(90f, 80f));
			control.CatchMessage = false;
			m_SelectModeGroup.Add(control);
			uIText = UIUtils.BuildUIText(0, new Rect(300f, 145f, 100f, 30f), UIText.enAlignStyle.center);
			uIText.Set("Zombie3D/Font/037-CAI978-22", gameState.GetNName(m_nLogin.MineSelf.Name), new Color(0.8745098f, 0.6039216f, 2f / 15f, 1f));
			m_SelectModeGroup.Add(uIText);
			float left = 520f;
			float left2 = 590f;
			uIText = UIUtils.BuildUIText(0, new Rect(left, 210f, 50f, 30f), UIText.enAlignStyle.right);
			uIText.Set("Zombie3D/Font/037-CAI978-22", "GAMES: ", new Color(0.8745098f, 0.6039216f, 2f / 15f, 1f));
			m_SelectModeGroup.Add(uIText);
			uIText = UIUtils.BuildUIText(0, new Rect(left2, 210f, 100f, 30f), UIText.enAlignStyle.left);
			uIText.Set("Zombie3D/Font/037-CAI978-22", gameState.GetNBattleStatistics(GameState.NetworkGameMode.NBattleStatistics.E_NBATTLETIMES).ToString(), new Color(0.8745098f, 0.6039216f, 2f / 15f, 1f));
			m_SelectModeGroup.Add(uIText);
			uIText = UIUtils.BuildUIText(0, new Rect(left, 180f, 50f, 30f), UIText.enAlignStyle.right);
			uIText.Set("Zombie3D/Font/037-CAI978-22", "KILLS: ", new Color(0.8745098f, 0.6039216f, 2f / 15f, 1f));
			m_SelectModeGroup.Add(uIText);
			uIText = UIUtils.BuildUIText(0, new Rect(left2, 180f, 100f, 30f), UIText.enAlignStyle.left);
			uIText.Set("Zombie3D/Font/037-CAI978-22", gameState.GetNBattleStatistics(GameState.NetworkGameMode.NBattleStatistics.E_NBATTLEKILLS).ToString(), new Color(0.8745098f, 0.6039216f, 2f / 15f, 1f));
			m_SelectModeGroup.Add(uIText);
			uIText = UIUtils.BuildUIText(0, new Rect(left, 145f, 50f, 30f), UIText.enAlignStyle.right);
			uIText.Set("Zombie3D/Font/037-CAI978-22", "DEATHS: ", new Color(0.8745098f, 0.6039216f, 2f / 15f, 1f));
			m_SelectModeGroup.Add(uIText);
			uIText = UIUtils.BuildUIText(0, new Rect(left2, 145f, 100f, 30f), UIText.enAlignStyle.left);
			uIText.Set("Zombie3D/Font/037-CAI978-22", gameState.GetNBattleStatistics(GameState.NetworkGameMode.NBattleStatistics.E_NBATTLEDEATHS).ToString(), new Color(0.8745098f, 0.6039216f, 2f / 15f, 1f));
			m_SelectModeGroup.Add(uIText);
		}
	}

	public void SetupNModeIntroduce(bool bShow)
	{
		if (m_IntroduceModeGroup != null)
		{
			m_IntroduceModeGroup.Clear();
			m_IntroduceModeGroup = null;
		}
		if (bShow)
		{
			m_IntroduceModeGroup = new uiGroup(m_UIManager);
			string[] array = new string[4] { "Team", "Solo", "Team", "Solo" };
			string[] array2 = new string[4] { "Last Stand", "Last Stand", "Death Match", "Death Match" };
			int num = 0;
			if (gameState.m_eGameMode.m_ePlayMode == GameState.NetworkGameMode.PlayMode.E_DeathMatch)
			{
				num = ((gameState.m_eGameMode.m_eCooperaMode != 0) ? 3 : 2);
			}
			else if (gameState.m_eGameMode.m_ePlayMode == GameState.NetworkGameMode.PlayMode.E_LastStand)
			{
				num = ((gameState.m_eGameMode.m_eCooperaMode != 0) ? 1 : 0);
			}
			else
			{
				Debug.Log("Error ID");
			}
			string text = array[num];
			string text2 = array2[num];
			UIImage control = UIUtils.BuildImage(0, new Rect(0f, 0f, 960f, 640f), m_MatCommonBg, new Rect(0f, 0f, 960f, 640f), new Vector2(960f, 640f));
			m_IntroduceModeGroup.Add(control);
			if (m_MatNDialog01 == null)
			{
				m_MatNDialog01 = LoadUIMaterial("Zombie3D/UI/Materials/NMsgBox");
				Resources.UnloadUnusedAssets();
			}
			control = UIUtils.BuildImage(0, new Rect(99f, 57f, 772f, 450f), m_MatNDialog01, new Rect(2f, 2f, 772f, 450f), new Vector2(772f, 450f));
			m_IntroduceModeGroup.Add(control);
			UIText uIText = UIUtils.BuildUIText(0, new Rect(284f, 455f, 400f, 30f), UIText.enAlignStyle.center);
			uIText.Set("Zombie3D/Font/037-CAI978-22", text, new Color(69f / 85f, 0.5254902f, 0.09019608f, 1f));
			m_IntroduceModeGroup.Add(uIText);
			float top = 125f;
			if (text2.Length < 100)
			{
				top = 105f;
			}
			uIText = UIUtils.BuildUIText(0, new Rect(210f, top, 560f, 80f), UIText.enAlignStyle.left);
			uIText.Set("Zombie3D/Font/037-CAI978-18", text2, new Color(69f / 85f, 0.5254902f, 0.09019608f, 1f));
			m_IntroduceModeGroup.Add(uIText);
			UIClickButton control2 = UIUtils.BuildClickButton(14002, new Rect(55f, 15f, 187f, 68f), m_MatCommonBg, new Rect(521f, 710f, 188f, 68f), new Rect(712f, 710f, 188f, 68f), new Rect(521f, 710f, 188f, 68f), new Vector2(188f, 68f));
			m_IntroduceModeGroup.Add(control2);
			control2 = UIUtils.BuildClickButton(14004, new Rect(659f, 15f, 187f, 68f), m_MatCommonBg, new Rect(521f, 645f, 192f, 62f), new Rect(716f, 645f, 192f, 62f), new Rect(521f, 645f, 192f, 62f), new Vector2(192f, 62f));
			m_IntroduceModeGroup.Add(control2);
		}
	}

	public void SetupNLobbyUI(bool bShow)
	{
		if (m_LobbyGroup != null)
		{
			m_LobbyGroup.Clear();
			m_LobbyGroup = null;
		}
		if (bShow)
		{
			m_LobbyGroup = new uiGroup(m_UIManager);
			if (m_MatNDialog01 == null)
			{
				m_MatNDialog01 = LoadUIMaterial("Zombie3D/UI/Materials/NMsgBox");
				Resources.UnloadUnusedAssets();
			}
			UIImage control = UIUtils.BuildImage(0, new Rect(0f, 0f, 960f, 640f), m_MatNDialog01, new Rect(600f, 1f, 1f, 1f), new Vector2(960f, 640f));
			m_LobbyGroup.Add(control);
			UIClickButton uIClickButton = null;
			uIClickButton = UIUtils.BuildClickButton(14011, new Rect(455f, 15f, 188f, 68f), m_MatNetWorkUI, new Rect(836f, 373f, 188f, 68f), new Rect(836f, 441f, 188f, 68f), new Rect(836f, 373f, 188f, 68f), new Vector2(188f, 68f));
			m_LobbyGroup.Add(uIClickButton);
			m_LobbyGroup.Add(uIClickButton);
			uIClickButton = UIUtils.BuildClickButton(14002, new Rect(55f, 15f, 187f, 68f), m_MatCommonBg, new Rect(521f, 710f, 188f, 68f), new Rect(712f, 710f, 188f, 68f), new Rect(521f, 710f, 188f, 68f), new Vector2(188f, 68f));
			m_LobbyGroup.Add(uIClickButton);
			SetRoomList(true);
		}
	}

	public void ServerSuccessed(NSuccessedCMD cmd)
	{
		SetupNWaitServerMsgUI(false);
		switch (cmd)
		{
		case NSuccessedCMD.E_CONNECTED:
			if (gameState.NetPlayerName == string.Empty)
			{
				SetupNetworkUIGroup(NUIState.E_CREATENAME);
			}
			else
			{
				m_nLogin.ReqLogin(gameState.NetPlayerName);
			}
			break;
		case NSuccessedCMD.E_LOGIN:
			if (gameState.m_eGameMode.m_ePlayMode != GameState.NetworkGameMode.PlayMode.E_PVE_BossRush)
			{
				SetupNetworkUIGroup(NUIState.E_SELECTMODE);
			}
			else
			{
				m_nLogin.PVE_Doing();
			}
			break;
		case NSuccessedCMD.E_JoinRoom:
			SetupNetworkUIGroup(NUIState.E_ROOM);
			break;
		case NSuccessedCMD.E_PVPLeaveRoom:
			SetupNetworkUIGroup(NUIState.E_SELECTMODE);
			break;
		}
	}

	public void RoomListIsChanged()
	{
		SetRoomList(true);
	}

	public void RoomVariableIsChanged()
	{
		SetupNetworkUIGroup(NUIState.E_ROOM);
	}

	public List<Room> RoomFilter(List<Room> rls, IRoomManager roomManager)
	{
		string empty = string.Empty;
		if (gameState.m_eGameMode.m_ePlayMode == GameState.NetworkGameMode.PlayMode.E_LastStand)
		{
			empty = "LastStand";
		}
		else
		{
			if (gameState.m_eGameMode.m_ePlayMode != GameState.NetworkGameMode.PlayMode.E_DeathMatch)
			{
				return rls;
			}
			empty = "DeatchMatch";
		}
		return roomManager.GetRoomListFromGroup(empty);
	}

	public void SetRoomList(bool bShow)
	{
	}

	public void SetupNCreateRoomUI(bool bShow)
	{
		if (m_CreateRoomGroup != null)
		{
			m_CreateRoomGroup.Clear();
			m_CreateRoomGroup = null;
		}
		if (bShow)
		{
			m_CreateRoomGroup = new uiGroup(m_UIManager);
			if (m_MatNDialog01 == null)
			{
				m_MatNDialog01 = LoadUIMaterial("Zombie3D/UI/Materials/NMsgBox");
				Resources.UnloadUnusedAssets();
			}
			UIImage control = UIUtils.BuildImage(0, new Rect(0f, 0f, 960f, 640f), m_MatNDialog01, new Rect(600f, 1f, 1f, 1f), new Vector2(960f, 640f));
			m_CreateRoomGroup.Add(control);
			UIClickButton uIClickButton = null;
			uIClickButton = UIUtils.BuildClickButton(14013, new Rect(197f, 270f, 266f, 188f), m_MatNetWorkUI, new Rect(0f, 186f, 268f, 186f), new Rect(0f, 0f, 268f, 186f), new Rect(0f, 186f, 268f, 186f), new Vector2(268f, 186f));
			m_CreateRoomGroup.Add(uIClickButton);
			uIClickButton = UIUtils.BuildClickButton(14014, new Rect(495f, 270f, 266f, 188f), m_MatNetWorkUI, new Rect(268f, 186f, 268f, 186f), new Rect(268f, 0f, 268f, 186f), new Rect(268f, 186f, 268f, 186f), new Vector2(268f, 186f));
			m_CreateRoomGroup.Add(uIClickButton);
			uIClickButton = UIUtils.BuildClickButton(14002, new Rect(55f, 15f, 187f, 68f), m_MatCommonBg, new Rect(521f, 710f, 188f, 68f), new Rect(712f, 710f, 188f, 68f), new Rect(521f, 710f, 188f, 68f), new Vector2(188f, 68f));
			m_CreateRoomGroup.Add(uIClickButton);
		}
	}

	public void ShowBackBtn()
	{
		UIClickButton control = UIUtils.BuildClickButton(14002, new Rect(55f, 15f, 187f, 68f), m_MatCommonBg, new Rect(521f, 710f, 188f, 68f), new Rect(712f, 710f, 188f, 68f), new Rect(521f, 710f, 188f, 68f), new Vector2(188f, 68f));
		if (m_RoomGroup != null)
		{
			m_RoomGroup.Add(control);
		}
		m_bShowRoomBackBtn = true;
	}

	public void SetupNRoomUI(bool bShow)
	{
		if (m_RoomGroup != null)
		{
			m_RoomGroup.Clear();
			m_RoomGroup = null;
		}
		if (bShow)
		{
			m_RoomGroup = new uiGroup(m_UIManager);
			if (gameState.m_eGameMode.m_ePlayMode == GameState.NetworkGameMode.PlayMode.E_PVE_BossRush)
			{
				NRoomUIBackGround_PVE(m_RoomGroup);
				NRoomUISeat_PVE(m_RoomGroup);
			}
			else
			{
				NRoomUIBackGround(m_RoomGroup);
				NRoomUISeat(m_RoomGroup);
			}
		}
	}

	public void NRoomUIBackGround(uiGroup group)
	{
		UIClickButton uIClickButton = null;
		UIImage uIImage = null;
		UIText uIText = null;
		uIImage = UIUtils.BuildImage(0, new Rect(0f, 0f, 960f, 640f), m_MatCommonBg, new Rect(0f, 0f, 960f, 640f), new Vector2(960f, 640f));
		group.Add(uIImage);
		SetupNBackGroundAround(group);
		uIImage = UIUtils.BuildImage(0, new Rect(37f, 336f, 881f, 240f), m_MatNetWorkUI, new Rect(0f, 784f, 881f, 240f), new Vector2(881f, 240f));
		group.Add(uIImage);
		uIImage = UIUtils.BuildImage(0, new Rect(37f, 88f, 881f, 240f), m_MatNetWorkUI, new Rect(0f, 784f, 881f, 240f), new Vector2(881f, 240f));
		group.Add(uIImage);
		if (m_nLogin.LastJoinRoom.UserCount < m_nLogin.LastJoinRoom.MaxUsers)
		{
			uIImage = UIUtils.BuildImage(0, new Rect(244f, 580f, 472f, 33f), m_MatNetWorkUI, new Rect(278f, 713f, 472f, 33f), new Vector2(472f, 33f));
			group.Add(uIImage);
		}
		bool flag = false;
		if ((gameState.m_eGameMode.m_eCooperaMode == GameState.NetworkGameMode.NetworkCooperationMode.E_Team) ? true : false)
		{
			uIImage = UIUtils.BuildImage(0, new Rect(52f, 540f, 140f, 40f), m_MatNetWorkUI, new Rect(19f, 714f, 140f, 30f), new Vector2(140f, 40f));
			group.Add(uIImage);
			uIImage = UIUtils.BuildImage(0, new Rect(52f, 290f, 140f, 40f), m_MatNetWorkUI, new Rect(15f, 746f, 140f, 30f), new Vector2(140f, 40f));
			group.Add(uIImage);
		}
		int num = 4;
		int num2 = 362;
		int num3 = 117;
		for (int i = 0; i < num; i++)
		{
			uIImage = UIUtils.BuildImage(0, new Rect(58 + 226 * i, num2, 155f, 148f), m_MatNetWorkUI, new Rect(781f, 570f, 155f, 148f), new Vector2(155f, 148f));
			group.Add(uIImage);
		}
		for (int j = 0; j < num; j++)
		{
			uIImage = UIUtils.BuildImage(0, new Rect(58 + 226 * j, num3, 155f, 148f), m_MatNetWorkUI, new Rect(781f, 570f, 155f, 148f), new Vector2(155f, 148f));
			group.Add(uIImage);
		}
		if (m_bShowRoomBackBtn)
		{
			uIClickButton = UIUtils.BuildClickButton(14002, new Rect(55f, 15f, 187f, 68f), m_MatCommonBg, new Rect(521f, 710f, 188f, 68f), new Rect(712f, 710f, 188f, 68f), new Rect(521f, 710f, 188f, 68f), new Vector2(188f, 68f));
			group.Add(uIClickButton);
		}
	}

	public void NRoomUISeat(uiGroup group)
	{
		if (group == null)
		{
			return;
		}
		TNetRoom curRoom = SmartFoxConnection.Connection.CurRoom;
		for (int i = 0; i < curRoom.UserList.Count; i++)
		{
			TNetUser tNetUser = curRoom.UserList[i];
			int sitIndex = tNetUser.SitIndex;
			if (tNetUser == null)
			{
				continue;
			}
			if (SmartFoxConnection.Connection.Myself.Equals(tNetUser))
			{
				m_iMyRoomSeatIndex = sitIndex;
			}
			int avatar_suite_type = 1;
			if (tNetUser.ContainsVariable(TNetUserVarType.E_PlayerInfo))
			{
				SFSObject variable = tNetUser.GetVariable(TNetUserVarType.E_PlayerInfo);
				if (variable.ContainsKey("AvatarHeadID"))
				{
					avatar_suite_type = variable.GetInt("AvatarHeadID");
				}
			}
			Rect avatarIconTexture = ShopUIScript.GetAvatarIconTexture((Avatar.AvatarSuiteType)avatar_suite_type, Avatar.AvatarType.Head);
			Rect scrRect = ((sitIndex >= 4) ? new Rect(95 + 225 * (sitIndex - 4), 163f, 90f, 80f) : new Rect(95 + 225 * sitIndex, 406f, 90f, 80f));
			UIImage uIImage = UIUtils.BuildImage(0, scrRect, m_MatAvatarIcons, avatarIconTexture, new Vector2(90f, 80f));
			uIImage.CatchMessage = false;
			group.Add(uIImage);
			int roomMasterID = curRoom.RoomMasterID;
			Color color = new Color(0.81960785f, 0.5294118f, 2f / 51f, 1f);
			if (tNetUser.Id == SmartFoxConnection.Connection.Myself.Id)
			{
				uIImage = UIUtils.BuildImage(0, new Rect(scrRect.x - 45f, scrRect.y - 52f, 173f, 166f), m_MatRoomUI, new Rect(0f, 0f, 173f, 166f), new Vector2(173f, 166f));
				uIImage.CatchMessage = false;
				group.Add(uIImage);
			}
			UIText uIText = UIUtils.BuildUIText(0, new Rect(scrRect.x + 15f, scrRect.y + 70f, 70f, 25f), UIText.enAlignStyle.center);
			uIText.Set("Zombie3D/Font/037-CAI978-18", gameState.GetNName(tNetUser.Name), color);
			group.Add(uIText);
			if (tNetUser.ContainsVariable(TNetUserVarType.E_PlayerInfo))
			{
				SFSObject variable2 = tNetUser.GetVariable(TNetUserVarType.E_PlayerInfo);
				int num = 0;
				if (variable2.ContainsKey("GearScore"))
				{
					num = variable2.GetInt("GearScore");
				}
				uIText = UIUtils.BuildUIText(0, new Rect(scrRect.x + 15f, scrRect.y - 30f, 70f, 25f), UIText.enAlignStyle.center);
				uIText.Set("Zombie3D/Font/037-CAI978-18", num.ToString(), new Color(0.81960785f, 0.5294118f, 2f / 51f, 1f));
				group.Add(uIText);
			}
			uIImage = UIUtils.BuildImage(0, new Rect(scrRect.x - 30f, scrRect.y - 40f, 47f, 48f), m_MatNetWorkUI, new Rect(199f, 715f, 47f, 48f), new Vector2(47f, 48f));
			group.Add(uIImage);
		}
	}

	public void NRoomUIBackGround_PVE(uiGroup group)
	{
		UIClickButton uIClickButton = null;
		UIImage uIImage = null;
		UIText uIText = null;
		uIImage = UIUtils.BuildImage(0, new Rect(0f, 0f, 960f, 640f), m_MatCommonBg, new Rect(0f, 0f, 960f, 640f), new Vector2(960f, 640f));
		group.Add(uIImage);
		SetupNBackGroundAround(group);
		uIImage = UIUtils.BuildImage(0, new Rect(37f, 336f, 881f, 240f), m_MatNetWorkUI, new Rect(0f, 784f, 881f, 240f), new Vector2(881f, 240f));
		group.Add(uIImage);
		Material mat = LoadUIMaterial("Zombie3D/UI/Materials/RoomBackGround");
		Resources.UnloadUnusedAssets();
		uIImage = UIUtils.BuildImage(0, new Rect(37f, 140f, 860f, 140f), mat, new Rect(0f, 0f, 860f, 140f), new Vector2(860f, 140f));
		group.Add(uIImage);
		string[] array = new string[3] { "Instantly revive a teammate with a Defibrillator!", "Every time you kill a boss you'll get a treasure chest!", "The more damage you deal, the better the rewards you'll get!" };
		uIText = UIUtils.BuildUIText(0, new Rect(uIImage.Rect.x + 20f, uIImage.Rect.y + 90f, 860f, 25f), UIText.enAlignStyle.left);
		uIText.Set("Zombie3D/Font/037-CAI978-18", "Tips:", new Color(0.81960785f, 0.5294118f, 2f / 51f, 1f));
		group.Add(uIText);
		uIText = UIUtils.BuildUIText(0, new Rect(uIImage.Rect.x + 20f, uIImage.Rect.y + 60f, 860f, 25f), UIText.enAlignStyle.left);
		uIText.Set("Zombie3D/Font/037-CAI978-18", "    " + array[Random.Range(0, array.Length)], new Color(0.81960785f, 0.5294118f, 2f / 51f, 1f));
		group.Add(uIText);
		if (m_nLogin.LastJoinRoom.UserCount < m_nLogin.LastJoinRoom.MaxUsers)
		{
			uIImage = UIUtils.BuildImage(0, new Rect(244f, 580f, 472f, 33f), m_MatNetWorkUI, new Rect(278f, 713f, 472f, 33f), new Vector2(472f, 33f));
			group.Add(uIImage);
		}
		bool flag = false;
		int num = 4;
		int num2 = 362;
		int num3 = 117;
		for (int i = 0; i < num; i++)
		{
			uIImage = UIUtils.BuildImage(0, new Rect(58 + 226 * i, num2, 155f, 148f), m_MatNetWorkUI, new Rect(781f, 570f, 155f, 148f), new Vector2(155f, 148f));
			group.Add(uIImage);
		}
		if (m_bShowRoomBackBtn)
		{
			uIClickButton = UIUtils.BuildClickButton(14002, new Rect(55f, 15f, 187f, 68f), m_MatCommonBg, new Rect(521f, 710f, 188f, 68f), new Rect(712f, 710f, 188f, 68f), new Rect(521f, 710f, 188f, 68f), new Vector2(188f, 68f));
			group.Add(uIClickButton);
		}
	}

	public void NRoomUISeat_PVE(uiGroup group)
	{
		if (group == null)
		{
			return;
		}
		TNetRoom curRoom = SmartFoxConnection.Connection.CurRoom;
		for (int i = 0; i < curRoom.UserList.Count; i++)
		{
			TNetUser tNetUser = curRoom.UserList[i];
			int sitIndex = tNetUser.SitIndex;
			if (tNetUser == null)
			{
				continue;
			}
			if (SmartFoxConnection.Connection.Myself.Equals(tNetUser))
			{
				m_iMyRoomSeatIndex = sitIndex;
			}
			int avatar_suite_type = 1;
			if (tNetUser.ContainsVariable(TNetUserVarType.E_PlayerInfo))
			{
				SFSObject variable = tNetUser.GetVariable(TNetUserVarType.E_PlayerInfo);
				if (variable.ContainsKey("AvatarHeadID"))
				{
					avatar_suite_type = variable.GetInt("AvatarHeadID");
				}
			}
			Rect avatarIconTexture = ShopUIScript.GetAvatarIconTexture((Avatar.AvatarSuiteType)avatar_suite_type, Avatar.AvatarType.Head);
			Rect scrRect = ((sitIndex >= 4) ? new Rect(95 + 225 * (sitIndex - 4), 163f, 90f, 80f) : new Rect(95 + 225 * sitIndex, 406f, 90f, 80f));
			UIImage uIImage = UIUtils.BuildImage(0, scrRect, m_MatAvatarIcons, avatarIconTexture, new Vector2(90f, 80f));
			uIImage.CatchMessage = false;
			group.Add(uIImage);
			int roomMasterID = curRoom.RoomMasterID;
			Color color = new Color(0.81960785f, 0.5294118f, 2f / 51f, 1f);
			if (tNetUser.Id == SmartFoxConnection.Connection.Myself.Id)
			{
				uIImage = UIUtils.BuildImage(0, new Rect(scrRect.x - 45f, scrRect.y - 52f, 173f, 166f), m_MatRoomUI, new Rect(0f, 0f, 173f, 166f), new Vector2(173f, 166f));
				uIImage.CatchMessage = false;
				group.Add(uIImage);
			}
			UIText uIText = UIUtils.BuildUIText(0, new Rect(scrRect.x + 15f, scrRect.y + 70f, 70f, 25f), UIText.enAlignStyle.center);
			uIText.Set("Zombie3D/Font/037-CAI978-18", gameState.GetNName(tNetUser.Name), color);
			group.Add(uIText);
			if (tNetUser.ContainsVariable(TNetUserVarType.E_PlayerInfo))
			{
				SFSObject variable2 = tNetUser.GetVariable(TNetUserVarType.E_PlayerInfo);
				int num = 0;
				if (variable2.ContainsKey("GearScore"))
				{
					num = variable2.GetInt("GearScore");
				}
				uIText = UIUtils.BuildUIText(0, new Rect(scrRect.x + 15f, scrRect.y - 30f, 70f, 25f), UIText.enAlignStyle.center);
				uIText.Set("Zombie3D/Font/037-CAI978-18", num.ToString(), new Color(0.81960785f, 0.5294118f, 2f / 51f, 1f));
				group.Add(uIText);
			}
			uIImage = UIUtils.BuildImage(0, new Rect(scrRect.x - 30f, scrRect.y - 40f, 47f, 48f), m_MatNetWorkUI, new Rect(199f, 715f, 47f, 48f), new Vector2(47f, 48f));
			group.Add(uIImage);
		}
	}

	public void SetupNBackGroundAround(uiGroup group)
	{
		if (group != null)
		{
			UIImage uIImage = null;
			Rect rcMat = new Rect(972f, 28f, 52f, 262f);
			Rect rcMat2 = new Rect(904f, 15f, 62f, 378f);
			Rect rcMat3 = new Rect(944f, 400f, 80f, 624f);
			Rect rcMat4 = new Rect(2f, 953f, 960f, 66f);
			uIImage = UIUtils.BuildImage(0, new Rect(-1f, rcMat2.height - 2f, rcMat.width, rcMat.height), m_MatNetWorkUI, rcMat, new Vector2(rcMat.width, rcMat.height));
			uIImage.CatchMessage = false;
			group.Add(uIImage);
			uIImage = UIUtils.BuildImage(0, new Rect(0f, 0f, rcMat2.width, rcMat2.height), m_MatNetWorkUI, rcMat2, new Vector2(rcMat2.width, rcMat2.height));
			uIImage.CatchMessage = false;
			group.Add(uIImage);
			uIImage = UIUtils.BuildImage(0, new Rect(960f - rcMat3.width, 0f, rcMat3.width, rcMat3.height), m_MatNetWorkUI, rcMat3, new Vector2(rcMat3.width, rcMat3.height));
			uIImage.CatchMessage = false;
			group.Add(uIImage);
			uIImage = UIUtils.BuildImage(0, new Rect(0f, 640f - rcMat4.height, rcMat4.width, rcMat4.height), m_MatCommonBg, rcMat4, new Vector2(rcMat4.width, rcMat4.height));
			uIImage.CatchMessage = false;
			group.Add(uIImage);
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
