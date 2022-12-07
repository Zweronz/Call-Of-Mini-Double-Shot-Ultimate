using System.Collections.Generic;
using TNetSdk;
using UnityEngine;
using Zombie3D;

public class NLoginScript : MonoBehaviour
{
	public enum GroupID
	{
		E_Default = 0,
		E_LastStand_Team = 1,
		E_LastStand_Solo = 2,
		E_DeathMatch_Team = 3,
		E_DeathMatch_Solo = 4,
		E_PVEBossRush = 5
	}

	private TNetObject smartFox;

	private string m_strZoneName = "CoMDS";

	public bool shuttingDown;

	public string serverName = "203.124.98.26";

	public int serverPort = 7001;

	private string username = string.Empty;

	private string loginErrorMessage = string.Empty;

	private NNetworkUIScript m_ChooseUIScript;

	public GroupID m_strJoinedGroupID;

	private bool m_bCanProcessEvent;

	public string ErrorMsg
	{
		get
		{
			return loginErrorMessage;
		}
		set
		{
			loginErrorMessage = value;
			if (!(loginErrorMessage != string.Empty))
			{
			}
		}
	}

	public TNetUser MineSelf
	{
		get
		{
			return smartFox.Myself;
		}
	}

	public TNetRoom LastJoinRoom
	{
		get
		{
			return smartFox.CurRoom;
		}
	}

	private void Awake()
	{
		m_ChooseUIScript = base.gameObject.GetComponent(typeof(NNetworkUIScript)) as NNetworkUIScript;
	}

	public void ReqLogin(string Name)
	{
		username = Name + "[Ma!cA@d#dres]" + Time.time + Random.Range(0f, 1000f);
		smartFox.Send(new LoginRequest(username));
		TimeManager.Instance.Init(3, 10f, TimeOut, null, "ReqLogin");
		ErrorMsg = "[ReqLogin] -" + m_strZoneName + "|" + username + "Begin_TimeManager_" + 3;
	}

	public void ReqCreateRoom(string name, GroupID groupId, short maxUser = 8, string password = "")
	{
		if (smartFox == null)
		{
			ErrorMsg = "ReqCreateRoom Failed!!! Because the smartFox is null";
			return;
		}
		if (GameApp.GetInstance().GetGameState().m_eGameMode.m_ePlayMode == GameState.NetworkGameMode.PlayMode.E_PVE_BossRush)
		{
			SFSObject sFSObject = new SFSObject();
			sFSObject.PutInt("PVE_Floor", 0);
			sFSObject.PutSFSObject("Boss_MSG", new SFSObject());
			smartFox.Send(new SetRoomVariableRequest(TNetRoomVarType.E_PVE_MSG, sFSObject));
		}
		smartFox.Send(new CreateRoomRequest(name + "'s Room", password, (int)groupId, maxUser, RoomCreateCmd.RoomType.open, RoomCreateCmd.RoomSwitchMasterType.Auto, string.Empty));
	}

	public GroupID GetGroupID(GameState.NetworkGameMode.PlayMode playMode, GameState.NetworkGameMode.NetworkCooperationMode copMode)
	{
		GroupID groupID = GroupID.E_Default;
		switch (playMode)
		{
		case GameState.NetworkGameMode.PlayMode.E_LastStand:
			if (copMode == GameState.NetworkGameMode.NetworkCooperationMode.E_Simple)
			{
				return GroupID.E_LastStand_Solo;
			}
			return GroupID.E_LastStand_Team;
		case GameState.NetworkGameMode.PlayMode.E_DeathMatch:
			if (copMode == GameState.NetworkGameMode.NetworkCooperationMode.E_Simple)
			{
				return GroupID.E_DeathMatch_Solo;
			}
			return GroupID.E_DeathMatch_Team;
		case GameState.NetworkGameMode.PlayMode.E_PVE_BossRush:
			return GroupID.E_PVEBossRush;
		default:
			return GroupID.E_Default;
		}
	}

	public void ReqGetRoomList(GroupID groupID, int page, int page_split, RoomDragListCmd.ListType type)
	{
		smartFox.Send(new GetRoomListRequest((int)groupID, page, page_split, type));
	}

	public void ReqAutoJoinRoom()
	{
		m_strJoinedGroupID = GetGroupID(GameApp.GetInstance().GetGameState().m_eGameMode.m_ePlayMode, GameApp.GetInstance().GetGameState().m_eGameMode.m_eCooperaMode);
		ReqGetRoomList(m_strJoinedGroupID, 0, 10, RoomDragListCmd.ListType.not_full_not_game);
	}

	public void ReqJoinRoom(int roomID)
	{
		smartFox.Send(new JoinRoomRequest(roomID, string.Empty));
	}

	public void ReqLeaveroom()
	{
		smartFox.Send(new LeaveRoomRequest());
		ErrorMsg = "[ReqLeaveroom] -";
	}

	public void UpdateRoomVariableOfRoomseats(TNetRoom room)
	{
	}

	public void ReqStartGame(int SceneLevel)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("RoomCMD", 1);
		sFSObject.PutInt("SceneLevel", SceneLevel);
		sFSObject.PutInt("PlayMode", (int)GameApp.GetInstance().GetGameState().m_eGameMode.m_ePlayMode);
		sFSObject.PutInt("CooperationMode", (int)GameApp.GetInstance().GetGameState().m_eGameMode.m_eCooperaMode);
		smartFox.Send(new BroadcastMessageRequest(sFSObject));
	}

	private void OnApplicationQuit()
	{
		shuttingDown = true;
	}

	private void FixedUpdate()
	{
		if (m_bCanProcessEvent)
		{
			smartFox.Update(Time.deltaTime);
		}
	}

	public void Connect()
	{
		m_strZoneName = GameApp.GetInstance().GetGameState().m_strZoneName;
		serverName = GameApp.GetInstance().GetGameState().serverName;
		serverPort = GameApp.GetInstance().GetGameState().serverPort;
		if (SmartFoxConnection.IsInitialized)
		{
			smartFox = SmartFoxConnection.Connection;
		}
		else
		{
			smartFox = new TNetObject();
		}
		smartFox.AddEventListener(TNetEventSystem.CONNECTION, OnConnection);
		smartFox.AddEventListener(TNetEventSystem.CONNECTION_TIMEOUT, OnConnectionTimeOut);
		smartFox.AddEventListener(TNetEventSystem.DISCONNECT, OnConnectionLost);
		smartFox.AddEventListener(TNetEventSystem.LOGIN, OnLogin);
		smartFox.AddEventListener(TNetEventSystem.REVERSE_HEART_TIMEOUT, OnHeartBeatTimeOut);
		smartFox.AddEventListener(TNetEventRoom.GET_ROOM_LIST, OnGetRoomList);
		smartFox.AddEventListener(TNetEventRoom.ROOM_JOIN, OnJoinRoom);
		smartFox.AddEventListener(TNetEventRoom.ROOM_VARIABLES_UPDATE, OnRoomVariablesUpdate);
		smartFox.AddEventListener(TNetEventRoom.USER_ENTER_ROOM, OnUserEnterRoom);
		smartFox.AddEventListener(TNetEventRoom.USER_EXIT_ROOM, OnUserLeaveRoom);
		smartFox.AddEventListener(TNetEventRoom.ROOM_MASTER_CHANGE, OnMasterChanged);
		smartFox.AddEventListener(TNetEventRoom.OBJECT_MESSAGE, OnObjectMessage);
		smartFox.AddEventListener(TNetEventRoom.USER_VARIABLES_UPDATE, OnUserVariableUpdate);
		smartFox.Connect(serverName, serverPort);
		TimeManager.Instance.Init(2, 10f, TimeOut, null, "ReqConnected");
		m_bCanProcessEvent = true;
		shuttingDown = false;
		ErrorMsg = "[ReqConnect] -" + serverName + ":" + serverPort + "|Begin_TimeManager_" + 2;
	}

	public void DisConnect()
	{
		SmartFoxConnection.DisConnect();
		ErrorMsg = "[ReqDisConnect] -";
	}

	private void UnregisterSFSSceneCallbacks()
	{
		smartFox.RemoveAllEventListeners();
		ErrorMsg = "[UnregisterSFSSceneCallbacks] -";
	}

	public void OnConnection(TNetEventData evt)
	{
		SmartFoxConnection.Connection = smartFox;
		m_ChooseUIScript.ServerSuccessed(NNetworkUIScript.NSuccessedCMD.E_CONNECTED);
		TimeManager.Instance.DestroyCalculagraph(2);
	}

	public void OnConnectionLost(TNetEventData evt)
	{
		UnregisterSFSSceneCallbacks();
		m_bCanProcessEvent = false;
		if (!shuttingDown)
		{
			SmartFoxConnection.Reset();
			SceneUIManager.Instance().ChangeSceneUI(SceneUIManager.SceneUI.ChoosePointsUI);
		}
	}

	public void OnConnectionTimeOut(TNetEventData evt)
	{
		TimeOut();
	}

	public void OnHeartBeatTimeOut(TNetEventData evt)
	{
		TimeOut();
	}

	public void OnLogin(TNetEventData evt)
	{
		if ((int)evt.data["result"] == 0)
		{
			TimeManager.Instance.DestroyCalculagraph(3);
			ErrorMsg = "[Receive OnLogin] -Success:|Destroy_TimeManager_" + 3;
			m_ChooseUIScript.ServerSuccessed(NNetworkUIScript.NSuccessedCMD.E_LOGIN);
		}
	}

	private void OnGetRoomList(TNetEventData evt)
	{
		List<TNetRoom> list = (List<TNetRoom>)evt.data["roomList"];
		if (list != null && list.Count > 0)
		{
			ReqJoinRoom(list[0].Id);
		}
		else if (GameApp.GetInstance().GetGameState().m_eGameMode.m_ePlayMode == GameState.NetworkGameMode.PlayMode.E_PVE_BossRush)
		{
			ReqCreateRoom(GameApp.GetInstance().GetGameState().GetNName(smartFox.Myself.Name), m_strJoinedGroupID, 4, string.Empty);
		}
		else
		{
			ReqCreateRoom(GameApp.GetInstance().GetGameState().GetNName(smartFox.Myself.Name), m_strJoinedGroupID, 8, string.Empty);
		}
	}

	private void OnJoinRoom(TNetEventData evt)
	{
		RoomJoinResCmd.Result result = (RoomJoinResCmd.Result)(int)evt.data["result"];
		TimeManager.Instance.DestroyCalculagraph(4);
		if (result == RoomJoinResCmd.Result.ok)
		{
			TNetRoom tNetRoom = (TNetRoom)evt.data["room"];
			m_ChooseUIScript.RoomVariableIsChanged();
			m_ChooseUIScript.ServerSuccessed(NNetworkUIScript.NSuccessedCMD.E_JoinRoom);
			m_ChooseUIScript.m_bShowRoomBackBtn = false;
			TimeManager.Instance.Init(6, 5f, m_ChooseUIScript.ShowBackBtn, null, "E_RoomCanBackTime");
			InitUserInfo();
		}
		else
		{
			ErrorMsg = "Join Room Error:" + result;
			TimeOut();
		}
	}

	public void OnRoomVariablesUpdate(TNetEventData evt)
	{
		TNetRoom curRoom = smartFox.CurRoom;
		TNetRoomVarType tNetRoomVarType = (TNetRoomVarType)(int)evt.data["key"];
		if (smartFox != SmartFoxConnection.Connection)
		{
			ErrorMsg = "[Receive OnRoomVariablesUpdate] -  SmartFoxConnection.Connection  !=  smartFox";
		}
		if (!smartFox.Myself.IsJoinedInRoom(curRoom))
		{
		}
	}

	public void OnUserVariableUpdate(TNetEventData evt)
	{
		TNetUser tNetUser = (TNetUser)evt.data["user"];
		if ((int)evt.data["key"] == 0)
		{
			m_ChooseUIScript.RoomVariableIsChanged();
		}
	}

	public bool StartGame(TNetRoom room)
	{
		if (room == null)
		{
			return false;
		}
		if (room.UserCount >= room.MaxUsers)
		{
			int[] array = new int[3] { 201, 202, 203 };
			int num = Random.Range(0, array.Length);
			if (GameApp.GetInstance().GetGameState().m_eGameMode.m_ePlayMode == GameState.NetworkGameMode.PlayMode.E_PVE_BossRush)
			{
				ReqStartGame(204);
			}
			else
			{
				ReqStartGame(array[num]);
			}
			smartFox.Send(new RoomStartRequest());
			return true;
		}
		return false;
	}

	public void OnUserEnterRoom(TNetEventData evt)
	{
		TNetUser tNetUser = (TNetUser)evt.data["user"];
		TNetRoom curRoom = smartFox.CurRoom;
		if (curRoom.RoomMasterID == smartFox.Myself.Id)
		{
			StartGame(curRoom);
		}
		m_ChooseUIScript.RoomVariableIsChanged();
	}

	public void OnUserLeaveRoom(TNetEventData evt)
	{
		TNetUser tNetUser = (TNetUser)evt.data["user"];
		TNetRoom curRoom = smartFox.CurRoom;
		if (tNetUser.Id == MineSelf.Id)
		{
			m_ChooseUIScript.ServerSuccessed(NNetworkUIScript.NSuccessedCMD.E_PVPLeaveRoom);
			m_strJoinedGroupID = GroupID.E_Default;
		}
		if (curRoom != null)
		{
			m_ChooseUIScript.RoomVariableIsChanged();
		}
	}

	public void OnMasterChanged(TNetEventData evt)
	{
		TNetUser tNetUser = (TNetUser)evt.data["user"];
		if (tNetUser.Id == MineSelf.Id)
		{
			TNetRoom curRoom = smartFox.CurRoom;
			if (!StartGame(curRoom))
			{
			}
		}
	}

	public void OnObjectMessage(TNetEventData evt)
	{
		TNetUser tNetUser = (TNetUser)evt.data["user"];
		SFSObject sFSObject = (SFSObject)evt.data["message"];
		switch (sFSObject.GetInt("RoomCMD"))
		{
		case 0:
			ReqLeaveroom();
			m_ChooseUIScript.SetupNetworkUIGroup(NNetworkUIScript.NUIState.E_LOBBY);
			break;
		case 1:
		{
			int @int = sFSObject.GetInt("SceneLevel");
			int int2 = sFSObject.GetInt("PlayMode");
			int int3 = sFSObject.GetInt("CooperationMode");
			Debug.Log("[Receive StartGame]" + @int + "|" + int2 + "|" + int3);
			if (int2 != (int)GameApp.GetInstance().GetGameState().m_eGameMode.m_ePlayMode)
			{
				ErrorMsg = "Start Game Error: The PlayMode Is Not Same As The GameState |" + int2 + "|" + GameApp.GetInstance().GetGameState().m_eGameMode.m_ePlayMode;
				Debug.Log(string.Concat("NowMode", (GameState.NetworkGameMode.PlayMode)int2, " | ", GameApp.GetInstance().GetGameState().m_eGameMode.m_ePlayMode));
				GameApp.GetInstance().GetGameState().m_eGameMode.m_ePlayMode = (GameState.NetworkGameMode.PlayMode)int2;
				Debug.Log("[Receive - StartGame | m_ePlayMode]" + GameApp.GetInstance().GetGameState().m_eGameMode.m_ePlayMode);
			}
			if (int3 != (int)GameApp.GetInstance().GetGameState().m_eGameMode.m_eCooperaMode)
			{
				ErrorMsg = "Start Game Error: The CooperationMode Is Not Same As The GameState |" + int3 + "|" + GameApp.GetInstance().GetGameState().m_eGameMode.m_eCooperaMode;
				Debug.Log(string.Concat("NowMode", (GameState.NetworkGameMode.NetworkCooperationMode)int3, " | ", GameApp.GetInstance().GetGameState().m_eGameMode.m_eCooperaMode));
				GameApp.GetInstance().GetGameState().m_eGameMode.m_eCooperaMode = (GameState.NetworkGameMode.NetworkCooperationMode)int3;
				Debug.Log("[Receive - StartGame | m_eCooperaMode]" + GameApp.GetInstance().GetGameState().m_eGameMode.m_eCooperaMode);
			}
			UnregisterSFSSceneCallbacks();
			m_ChooseUIScript.StartGame(@int);
			if (GameApp.GetInstance().GetGameState().m_eGameMode.m_ePlayMode == GameState.NetworkGameMode.PlayMode.E_PVE_BossRush)
			{
				GameApp.GetInstance().GetGameState().m_eGameMode.PVE_FLOOR = 0;
			}
			else
			{
				GameApp.GetInstance().GetGameState().AddNBattleStatisticsOnce(GameState.NetworkGameMode.NBattleStatistics.E_NBATTLETIMES);
			}
			GameCollectionInfoManager.Instance().GetCurrentInfo().AddPlayNetTimes(GameApp.GetInstance().GetGameState().m_eGameMode.m_ePlayMode, GameApp.GetInstance().GetGameState().m_eGameMode.m_eCooperaMode);
			break;
		}
		default:
			ErrorMsg = "No CMD, Please Check It!";
			break;
		}
	}

	private void InitUserInfo()
	{
		GameState gameState = GameApp.GetInstance().GetGameState();
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("WeaponId", (int)gameState.GetBattleWeapons()[0]);
		sFSObject.PutInt("AvatarHeadID", gameState.GetAvatarHeadID());
		sFSObject.PutInt("AvatarBodyID", gameState.GetAvatarBodyID());
		sFSObject.PutInt("GearScore", gameState.GetHireOutSelfPrice());
		smartFox.Send(new SetUserVariableRequest(TNetUserVarType.E_PlayerInfo, sFSObject));
		SFSObject sFSObject2 = new SFSObject();
		sFSObject2.PutInt("DeathCount", 0);
		sFSObject2.PutInt("KillCount", 0);
		smartFox.Send(new SetUserVariableRequest(TNetUserVarType.E_StatisticsInfo, sFSObject2));
		gameState.m_eGameMode.m_PlaersStatistics.Clear();
	}

	public void TimeOut()
	{
		SceneUIManager.Instance().ChangeSceneUI(SceneUIManager.SceneUI.ChoosePointsUI);
		SmartFoxConnection.DisConnect();
	}

	public void PVE_Doing()
	{
		ReqAutoJoinRoom();
	}
}
