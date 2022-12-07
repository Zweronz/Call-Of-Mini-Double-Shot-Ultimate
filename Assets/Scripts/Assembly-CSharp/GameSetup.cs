using System.Collections;
using System.Collections.Generic;
using TNetSdk;
using UnityEngine;
using Zombie3D;

public class GameSetup : MonoBehaviour
{
	public enum NPlayerDataType
	{
		E_HP = 0,
		E_MaxHP = 1,
		E_Hitted = 2,
		E_HittedColorChanged = 3,
		E_Fire = 4,
		E_StopFire = 5,
		E_PlayerALive = 6,
		E_PlayerDeath = 7,
		E_Invincible = 8,
		E_Skill_Vertigo = 9,
		E_Skill_VertigoEffect = 10,
		E_Skill_RunFast = 11,
		E_Skill_HailMary = 12,
		E_Skill_FancyFootwork = 13,
		E_Statistics_Killed = 14,
		E_Statistics_Death = 15,
		E_BattleStart = 16,
		E_BattleEnd = 17,
		E_BattleEndReward = 18,
		E_GamerTimer = 19
	}

	private TNetObject smartFox;

	protected float lastUpdateTime;

	protected float lastSendMsgTime;

	public OncePVPCombatDataRecord m_oncePVPCombatDR;

	public GameObject m_spawnTeamGO;

	public GameObject m_spawnSimpleGO;

	public int m_iDeathPlayerCount;

	private string m_strLastAnimationName = string.Empty;

	public bool m_bIsSendBattleEndMsg;

	private string m_strGameMessage = string.Empty;

	public float m_fCountDownTimer = -2f;

	public float m_fCountDownTime;

	private float m_fReviveCountDownTimer = -1f;

	private float m_fReviveCountDownTime = 10f;

	private static GameSetup instance;

	public string DebugMsg
	{
		get
		{
			return m_strGameMessage;
		}
		set
		{
			m_strGameMessage = value;
			if (!(m_strGameMessage != string.Empty))
			{
			}
		}
	}

	public float ReviveTimer
	{
		get
		{
			return m_fReviveCountDownTimer;
		}
		set
		{
			m_fReviveCountDownTimer = value;
		}
	}

	public float ReviveTime
	{
		get
		{
			return m_fReviveCountDownTime;
		}
		set
		{
			m_fReviveCountDownTime = value;
		}
	}

	public TNetUser MineUser
	{
		get
		{
			return smartFox.Myself;
		}
	}

	public TNetRoom MineRoom
	{
		get
		{
			return smartFox.CurRoom;
		}
	}

	public bool RoomOwnerIsMe
	{
		get
		{
			if (smartFox != null && MineRoom.RoomMasterID == MineUser.Id)
			{
				return true;
			}
			return false;
		}
	}

	public static GameSetup Instance
	{
		get
		{
			return instance;
		}
	}

	public TNetUser GetUserByID(int id)
	{
		return smartFox.CurRoom.GetUserById(id);
	}

	private void Awake()
	{
		instance = this;
		smartFox = SmartFoxConnection.Connection;
		if (smartFox == null)
		{
			SceneUIManager.Instance().ChangeSceneUI(SceneUIManager.SceneUI.ChoosePointsUI);
		}
		else
		{
			m_oncePVPCombatDR = new OncePVPCombatDataRecord();
		}
	}

	private void Start()
	{
		SubscribeDelegates();
		m_fCountDownTime = 180f;
		DebugMsg = "GameSetup_Start -|Begin SFSHeartBeat";
	}

	private void Update()
	{
		if (m_fCountDownTimer >= 0f)
		{
			m_fCountDownTimer += Time.deltaTime;
			if (Time.time - lastSendMsgTime >= 5f)
			{
				ReqSyncBattleTimer();
				lastSendMsgTime = Time.time;
			}
			if (m_fCountDownTimer >= m_fCountDownTime)
			{
				m_fCountDownTimer = -1f;
				NBattleUIScript nBattleUIScript = SceneUIManager.Instance().GetSceneUIObject().GetComponent(typeof(NBattleUIScript)) as NBattleUIScript;
				nBattleUIScript.IsCoundDownOver(0f);
				ReqDeathMatchBattleEnd();
			}
		}
		if (!(Time.time - lastUpdateTime < 0.001f))
		{
			lastUpdateTime = Time.time;
		}
	}

	private void FixedUpdate()
	{
		smartFox.Update(Time.deltaTime);
	}

	private void SubscribeDelegates()
	{
		smartFox.AddEventListener(TNetEventSystem.DISCONNECT, OnConnectionLost);
		smartFox.AddEventListener(TNetEventSystem.REVERSE_HEART_TIMEOUT, OnHeartBeatTimeOut);
		smartFox.AddEventListener(TNetEventRoom.USER_EXIT_ROOM, OnUserLeaveRoom);
		smartFox.AddEventListener(TNetEventRoom.USER_VARIABLES_UPDATE, OnUserVariableUpdate);
		smartFox.AddEventListener(TNetEventRoom.ROOM_VARIABLES_UPDATE, OnRoomVariablesUpdate);
		smartFox.AddEventListener(TNetEventRoom.ROOM_MASTER_CHANGE, OnMasterChanged);
		smartFox.AddEventListener(TNetEventRoom.LOCK_STH, OnSthLocked);
		smartFox.AddEventListener(TNetEventRoom.UNLOCK_STH, OnSthUnLocked);
		smartFox.AddEventListener(TNetEventRoom.OBJECT_MESSAGE, OnNetworkMessage);
	}

	public void UnsubscribeDelegates()
	{
		smartFox.RemoveAllEventListeners();
		DebugMsg = "UnsubscribeDelegates|Time" + Time.time;
	}

	public void UnsubscribeDelegates(TNetEventSystem eventType, EventListenerDelegate listener)
	{
		smartFox.RemoveEventListener(eventType, listener);
		DebugMsg = "UnsubscribeDelegates|Time" + Time.time;
	}

	public void UnsubscribeDelegates(TNetEventRoom eventType, EventListenerDelegate listener)
	{
		smartFox.RemoveEventListener(eventType, listener);
		DebugMsg = "UnsubscribeDelegates|Time" + Time.time;
	}

	public List<TNetUser> GetRoomUserList()
	{
		return smartFox.CurRoom.UserList;
	}

	public bool UserIsJoinInRoom(TNetUser user)
	{
		return user.IsJoinedInRoom(MineRoom);
	}

	public bool UserIsJoinInRoom(int userID)
	{
		if (GetUserByID(userID) != null)
		{
			return GetUserByID(userID).IsJoinedInRoom(MineRoom);
		}
		return false;
	}

	public int GetGroupId(int userId, bool IsMine = false)
	{
		DebugMsg = "GetGroupId Function | userId:" + userId + "|IsMine|" + IsMine;
		TNetUser tNetUser = ((!IsMine) ? GetUserByID(userId) : smartFox.Myself);
		int num = 0;
		int num2 = 0;
		if (tNetUser != null)
		{
			num2 = tNetUser.SitIndex;
			if (GameApp.GetInstance().GetGameState().m_eGameMode.m_eCooperaMode == GameState.NetworkGameMode.NetworkCooperationMode.E_Team)
			{
				num = ((num2 >= 0 && num2 < 4) ? 1 : 2);
			}
			DebugMsg = "GetGroupId Function | GroupID:" + num + "|SeatID|" + num2;
			return num;
		}
		return -1;
	}

	public int GetPosId(int groupId)
	{
		int result = 0;
		int sitIndex = smartFox.Myself.SitIndex;
		if (GameApp.GetInstance().GetGameState().m_eGameMode.m_eCooperaMode == GameState.NetworkGameMode.NetworkCooperationMode.E_Team)
		{
			int num = m_spawnTeamGO.transform.GetChildCount() / 2;
			switch (groupId)
			{
			case 1:
				result = sitIndex;
				break;
			case 2:
				result = sitIndex;
				break;
			default:
				DebugMsg = "Error GetPosId Function | groupId:" + groupId;
				break;
			}
		}
		else if (GameApp.GetInstance().GetGameState().m_eGameMode.m_eCooperaMode == GameState.NetworkGameMode.NetworkCooperationMode.E_Simple)
		{
			result = Random.Range(sitIndex * 3, (sitIndex + 1) * 3);
		}
		else
		{
			DebugMsg = "Error GetPosId Function | groupId:" + groupId;
		}
		return result;
	}

	public int GetObjID(GameObject obj)
	{
		if (GameApp.GetInstance().GetGameState().m_eGameMode.m_ePlayMode == GameState.NetworkGameMode.PlayMode.E_PVE_BossRush)
		{
			return NEnemyManager.Instance.GetNEnemyIDByObj(obj);
		}
		return -1;
	}

	public void ShowKillBattleUI(int headImg)
	{
		NBattleUIScript nBattleUIScript = SceneUIManager.Instance().GetSceneUIObject().GetComponent(typeof(NBattleUIScript)) as NBattleUIScript;
		nBattleUIScript.SetupNBattleMsg(true, headImg);
		nBattleUIScript.SetupControlCoverUI(true);
		nBattleUIScript.SetupNBattleDeathUI(true);
		nBattleUIScript.RestJoystick();
		nBattleUIScript.UpdateUIToHeight();
		TimeManager.Instance.Init(7, Instance.ReviveTime, RespawnTimeOut, nBattleUIScript.UpdateNRespawnTimer, "Respawn");
	}

	public void RespawnTimeOut()
	{
		NBattleUIScript nBattleUIScript = SceneUIManager.Instance().GetSceneUIObject().GetComponent(typeof(NBattleUIScript)) as NBattleUIScript;
		nBattleUIScript.NBattleMsgTimeOut();
		nBattleUIScript.SetupControlCoverUI(false);
		nBattleUIScript.SetupNBattleDeathUI(false);
		PlayerManager.Instance.GetPlayerClass().NResurrectionAtCurrentPos(false);
	}

	public void BattleStatisticsOfDeath(int AttackeerID)
	{
		TNetUser myself = smartFox.Myself;
		TNetUser userById = smartFox.CurRoom.GetUserById(AttackeerID);
		if (myself.ContainsVariable(TNetUserVarType.E_StatisticsInfo))
		{
			SFSObject variable = myself.GetVariable(TNetUserVarType.E_StatisticsInfo);
			int @int = variable.GetInt("DeathCount");
			@int++;
			variable.PutInt("DeathCount", @int);
			smartFox.Send(new SetUserVariableRequest(TNetUserVarType.E_StatisticsInfo, variable));
			DebugMsg = "[Require UserVariables] -||Time:" + Time.time;
			ReqSyncPlayerData(NPlayerDataType.E_Statistics_Death);
			ReqSyncPlayerInfo_ObjDouble(NPlayerDataType.E_Statistics_Killed, smartFox.TimeManager.NetworkTime, userById.Id);
			string msg = GameApp.GetInstance().GetGameState().GetNName(userById.Name) + "[De!ath#Msg%]" + GameApp.GetInstance().GetGameState().GetNName(myself.Name);
			ReqChatMsg(msg, NetWorkMessageInfo.E_NetCMD.E_StruckInformation);
			ShowKillBattleUI(GameApp.GetInstance().GetGameState().GetPlayerStatistics(userById.Id)
				.m_iHeadAvatarID);
				GameApp.GetInstance().GetGameState().AddNBattleStatisticsOnce(GameState.NetworkGameMode.NBattleStatistics.E_NBATTLEDEATHS);
				if (m_oncePVPCombatDR != null)
				{
					m_oncePVPCombatDR.AddDeathCount();
				}
				PlayDeathAudio();
			}
		}

		public void PlayDeathAudio()
		{
			if (!GameApp.GetInstance().GetGameState().SoundOn)
			{
				return;
			}
			string text = "BekilledAudio0" + Random.Range(1, 9);
			GameObject gameObject = Object.Instantiate(Resources.Load("Zombie3D/Audio/Network/" + text)) as GameObject;
			if (gameObject != null)
			{
				gameObject.transform.position = PlayerManager.Instance.GetPlayerObject().transform.position;
				RemoveTimerScript removeTimerScript = gameObject.AddComponent<RemoveTimerScript>();
				AudioSource audioSource = gameObject.GetComponent<AudioSource>();
				removeTimerScript.life = audioSource.clip.length + 0.1f;
				if (audioSource != null)
				{
					audioSource.loop = false;
					audioSource.Play();
				}
			}
		}

		public void BattleStatisticsOfKill()
		{
			TNetUser myself = smartFox.Myself;
			if (myself.ContainsVariable(TNetUserVarType.E_StatisticsInfo))
			{
				SFSObject variable = myself.GetVariable(TNetUserVarType.E_StatisticsInfo);
				int @int = variable.GetInt("KillCount");
				@int++;
				variable.PutInt("KillCount", @int);
				smartFox.Send(new SetUserVariableRequest(TNetUserVarType.E_StatisticsInfo, variable));
			}
		}

		public void ReqSpawnPlayer(int PosId)
		{
			SFSObject sFSObject = new SFSObject();
			sFSObject.PutInt("Game", 3);
			sFSObject.PutInt("PosID", PosId);
			smartFox.Send(new BroadcastMessageRequest(sFSObject));
		}

		public void ReqStartbattle()
		{
			if (RoomOwnerIsMe)
			{
				if (GameApp.GetInstance().GetGameState().m_eGameMode.m_ePlayMode == GameState.NetworkGameMode.PlayMode.E_DeathMatch)
				{
					ReqSyncPlayerInfo(NPlayerDataType.E_BattleStart, m_fCountDownTime);
				}
				else if (GameApp.GetInstance().GetGameState().m_eGameMode.m_ePlayMode == GameState.NetworkGameMode.PlayMode.E_LastStand)
				{
					ReqSyncPlayerData(NPlayerDataType.E_BattleStart);
				}
				else if (GameApp.GetInstance().GetGameState().m_eGameMode.m_ePlayMode == GameState.NetworkGameMode.PlayMode.E_PVE_BossRush)
				{
					ReqSyncPlayerData(NPlayerDataType.E_BattleStart);
				}
				SFSObject sFSObject = new SFSObject();
				sFSObject.PutDouble("BeginBattleTime", smartFox.TimeManager.NetworkTime);
				smartFox.Send(new SetRoomVariableRequest(TNetRoomVarType.E_BeginBattleTime, sFSObject));
			}
		}

		public void ReqSyncBattleTimer()
		{
			if (GameApp.GetInstance().GetGameState().m_eGameMode.m_ePlayMode == GameState.NetworkGameMode.PlayMode.E_DeathMatch && smartFox != null && smartFox.CurRoom != null && RoomOwnerIsMe)
			{
				if (m_fCountDownTimer < 0f)
				{
					m_fCountDownTimer = 0f;
				}
				ReqSyncPlayerInfo(NPlayerDataType.E_GamerTimer, m_fCountDownTimer);
			}
		}

		public void ReqDeathMatchBattleEnd()
		{
			if (!m_bIsSendBattleEndMsg && GameApp.GetInstance().GetGameState().m_eGameMode.m_ePlayMode == GameState.NetworkGameMode.PlayMode.E_DeathMatch && RoomOwnerIsMe)
			{
				int num = PlayerManager.Instance.CalWinner(GameApp.GetInstance().GetGameState().m_eGameMode.m_eCooperaMode);
				Instance.ReqSyncPlayerInfo(NPlayerDataType.E_BattleEnd, num);
				m_bIsSendBattleEndMsg = true;
			}
		}

		public void JudgeGameTimeOver()
		{
			if (m_fCountDownTimer == -1f)
			{
				ReqDeathMatchBattleEnd();
			}
		}

		public void JudgeWinnerOfUserLeave()
		{
			if (m_bIsSendBattleEndMsg)
			{
				return;
			}
			if (GameApp.GetInstance().GetGameState().m_eGameMode.m_eCooperaMode == GameState.NetworkGameMode.NetworkCooperationMode.E_Simple)
			{
				DebugMsg = "[JudgeWinnerOfUserLeave] -Simple|" + smartFox.CurRoom.UserCount;
				if (smartFox.CurRoom.UserCount == 1)
				{
					Instance.ReqSyncPlayerInfo(NPlayerDataType.E_BattleEnd, MineUser.Id);
					m_bIsSendBattleEndMsg = true;
				}
			}
			else
			{
				if (GameApp.GetInstance().GetGameState().m_eGameMode.m_eCooperaMode != 0 || !RoomOwnerIsMe)
				{
					return;
				}
				bool flag = false;
				int iNGroupID = PlayerManager.Instance.GetPlayerClass().m_iNGroupID;
				foreach (Player recipientPlayer in PlayerManager.Instance.GetRecipientPlayerList())
				{
					DebugMsg = iNGroupID + "|" + recipientPlayer.m_iNGroupID;
					if (recipientPlayer.m_iNGroupID != iNGroupID)
					{
						flag = true;
						break;
					}
				}
				DebugMsg = "bHaveEnemy|" + flag;
				if (!flag)
				{
					Instance.ReqSyncPlayerInfo(NPlayerDataType.E_BattleEnd, iNGroupID);
					m_bIsSendBattleEndMsg = true;
				}
			}
		}

		public void ReqChangeWeapon(int weaponTypeID)
		{
			SFSObject sFSObject = new SFSObject();
			sFSObject.PutInt("Game", 6);
			sFSObject.PutInt("WeaponTypeID", weaponTypeID);
			smartFox.Send(new BroadcastMessageRequest(sFSObject));
		}

		public void SendTransform(NetworkTransform ntransform, int targetId)
		{
			if (smartFox != null && SmartFoxConnection.IsInitialized && (!(PlayerManager.Instance.GetPlayerClass().HP <= 0f) || targetId != -1))
			{
				SFSObject sFSObject = new SFSObject();
				ntransform.ToSFSObject(sFSObject);
				sFSObject.PutInt("Game", 8);
				sFSObject.PutInt("tarId", targetId);
				smartFox.Send(new BroadcastMessageRequest(sFSObject));
			}
		}

		public void ReqSyncEnemyAnimation(int enemyId, string animationName, WrapMode wrapMode)
		{
			if (smartFox != null && SmartFoxConnection.IsInitialized)
			{
				SFSObject sFSObject = new SFSObject();
				sFSObject.PutInt("Game", 14);
				sFSObject.PutInt("EID", enemyId);
				sFSObject.PutUtfString("EAnName", animationName);
				sFSObject.PutInt("EWMode", (int)wrapMode);
				smartFox.Send(new BroadcastMessageRequest(sFSObject));
			}
		}

		public void ReqSyncAnimation(string animationName, WrapMode wrapMode, float fadeTime = 0.3f, float speedTime = 1f)
		{
			if (smartFox != null && SmartFoxConnection.IsInitialized && !(m_strLastAnimationName == animationName))
			{
				SFSObject sFSObject = new SFSObject();
				sFSObject.PutInt("Game", 7);
				sFSObject.PutUtfString("AnimationName", animationName);
				sFSObject.PutInt("WrapMode", (int)wrapMode);
				sFSObject.PutFloat("FadeTime", fadeTime);
				sFSObject.PutFloat("SpeedTime", speedTime);
				smartFox.Send(new BroadcastMessageRequest(sFSObject));
				m_strLastAnimationName = animationName;
			}
		}

		public void ReqSyncPlayerData(NPlayerDataType dataType)
		{
			if (smartFox != null && SmartFoxConnection.IsInitialized)
			{
				SFSObject sFSObject = new SFSObject();
				sFSObject.PutInt("Game", 9);
				sFSObject.PutInt("CMDKey", (int)dataType);
				smartFox.Send(new BroadcastMessageRequest(sFSObject));
			}
		}

		public void ReqSyncPlayerData(NPlayerDataType dataType, TNetUser us)
		{
			if (smartFox != null && SmartFoxConnection.IsInitialized)
			{
				SFSObject sFSObject = new SFSObject();
				sFSObject.PutInt("Game", 9);
				sFSObject.PutInt("CMDKey", (int)dataType);
				smartFox.Send(new ObjectMessageRequest(us.Id, sFSObject));
			}
		}

		public void ReqSyncPlayerInfo(NPlayerDataType infoType, float value, int userID = -1)
		{
			if (smartFox != null && SmartFoxConnection.IsInitialized && (infoType != NPlayerDataType.E_BattleEnd || !m_bIsSendBattleEndMsg))
			{
				SFSObject sFSObject = new SFSObject();
				sFSObject.PutInt("Game", 9);
				sFSObject.PutInt("CMDKey", (int)infoType);
				sFSObject.PutFloat("CMDValue", value);
				if (userID == -1)
				{
					smartFox.Send(new BroadcastMessageRequest(sFSObject));
				}
				else
				{
					smartFox.Send(new ObjectMessageRequest(userID, sFSObject));
				}
			}
		}

		public void ReqSyncPlayerInfo2Value(NPlayerDataType infoType, float value1, float value2, int userID = -1)
		{
			if (smartFox != null && SmartFoxConnection.IsInitialized)
			{
				SFSObject sFSObject = new SFSObject();
				sFSObject.PutInt("Game", 9);
				sFSObject.PutInt("CMDKey", (int)infoType);
				sFSObject.PutFloat("CMDValue", value1);
				sFSObject.PutFloat("CMDValue2", value2);
				if (userID == -1)
				{
					smartFox.Send(new BroadcastMessageRequest(sFSObject));
				}
				else
				{
					smartFox.Send(new ObjectMessageRequest(userID, sFSObject));
				}
			}
		}

		public void ReqSyncPlayerInfo_ObjDouble(NPlayerDataType infoType, double value, int userID = -1)
		{
			if (smartFox != null && SmartFoxConnection.IsInitialized)
			{
				SFSObject sFSObject = new SFSObject();
				sFSObject.PutInt("Game", 9);
				sFSObject.PutInt("CMDKey", (int)infoType);
				sFSObject.PutDouble("CMDValue_Double", value);
				sFSObject.PutInt("CMDValueObj", 0);
				if (userID == -1)
				{
					smartFox.Send(new BroadcastMessageRequest(sFSObject));
				}
				else
				{
					smartFox.Send(new ObjectMessageRequest(userID, sFSObject));
				}
			}
		}

		public void ReqSyncPlayerInfoListIntValue(NPlayerDataType infoType, List<KeyValuePair<int, int>> ls, int damage, int userID = -1)
		{
			if (smartFox == null || !SmartFoxConnection.IsInitialized)
			{
				return;
			}
			SFSObject sFSObject = new SFSObject();
			sFSObject.PutInt("Game", 9);
			sFSObject.PutInt("CMDKey", (int)infoType);
			sFSObject.PutInt("SDam", damage);
			sFSObject.PutInt("CMDValueObj", ls.Count);
			ISFSObject iSFSObject = new SFSObject();
			int[] array = new int[ls.Count];
			int[] array2 = new int[ls.Count];
			int num = 0;
			foreach (KeyValuePair<int, int> l in ls)
			{
				array[num] = l.Key;
				array2[num] = l.Value;
				num++;
			}
			iSFSObject.PutIntArray("key", array);
			iSFSObject.PutIntArray("value", array2);
			sFSObject.PutSFSObject("CMDValueObjList", iSFSObject);
			if (userID == -1)
			{
				smartFox.Send(new BroadcastMessageRequest(sFSObject));
			}
			else
			{
				smartFox.Send(new ObjectMessageRequest(userID, sFSObject));
			}
		}

		public void ReqChatMsg(string msg, NetWorkMessageInfo.E_NetCMD type)
		{
			if (smartFox != null && SmartFoxConnection.IsInitialized)
			{
				SFSObject sFSObject = new SFSObject();
				sFSObject.PutInt("Game", (int)type);
				sFSObject.PutUtfString("CMDValue", msg);
				smartFox.Send(new BroadcastMessageRequest(sFSObject));
			}
		}

		public void ReqSetEnemyInfoToRoomVariable(SFSObject obj)
		{
			smartFox.Send(new SetRoomVariableRequest(TNetRoomVarType.E_PVE_MSG, obj));
		}

		public void ReqThisFloorOverAndUpdateMsg(int level)
		{
			SFSObject variable = smartFox.CurRoom.GetVariable(TNetRoomVarType.E_PVE_MSG);
			variable.PutInt("PVE_Floor", level);
			ReqSetEnemyInfoToRoomVariable(variable);
		}

		public void ReqSpawnEnemy(SFSObject obj)
		{
			if (smartFox != null && SmartFoxConnection.IsInitialized)
			{
				smartFox.Send(new BroadcastMessageRequest(obj));
			}
		}

		public void ReqEnemySkill(int enemyID, enEnemySkillType type, List<int> targetUserID = null, List<Vector3> place = null)
		{
			if (smartFox == null || !SmartFoxConnection.IsInitialized)
			{
				return;
			}
			SFSObject sFSObject = new SFSObject();
			sFSObject.PutInt("EnemyID", enemyID);
			sFSObject.PutInt("ESkillType", (int)type);
			if (targetUserID != null)
			{
				ISFSArray iSFSArray = new SFSArray();
				foreach (int item in targetUserID)
				{
					iSFSArray.AddInt(item);
				}
				sFSObject.PutSFSArray("ESkTarg", iSFSArray);
			}
			if (place != null)
			{
				ISFSArray iSFSArray2 = new SFSArray();
				foreach (Vector3 item2 in place)
				{
					ISFSArray iSFSArray3 = new SFSArray();
					iSFSArray3.AddFloat(item2.x);
					iSFSArray3.AddFloat(item2.y);
					iSFSArray3.AddFloat(item2.z);
					iSFSArray2.AddSFSArray(iSFSArray3);
				}
				sFSObject.PutSFSArray("posi", iSFSArray2);
			}
			smartFox.Send(new BroadcastMessageRequest(sFSObject));
		}

		public void ReqSyncEnemyHp(int enemyId, float hp, float maxHp = -1f)
		{
			SFSObject sFSObject = new SFSObject();
			sFSObject.PutInt("Game", 3);
			sFSObject.PutInt("EID", enemyId);
			sFSObject.PutFloat("EHp", hp);
			if (maxHp > 0f)
			{
				sFSObject.PutFloat("EMHp", maxHp);
			}
			smartFox.Send(new BroadcastMessageRequest(sFSObject));
		}

		public void ReqHitNEnemy(int enemyId, float damage)
		{
			NEnemyManager.NEnemy nEnemyInfoByID = NEnemyManager.Instance.GetNEnemyInfoByID(enemyId);
			if (nEnemyInfoByID != null)
			{
				if (nEnemyInfoByID.isCopy > 0)
				{
					SFSObject sFSObject = new SFSObject();
					sFSObject.PutInt("Game", 17);
					sFSObject.PutInt("EID", enemyId);
					sFSObject.PutFloat("EDam", damage);
					smartFox.Send(new BroadcastMessageRequest(sFSObject));
				}
				else
				{
					nEnemyInfoByID.enemy.NEnemyOnHitted(MineUser.Id, damage);
				}
				GameApp.GetInstance().GetGameState().AddMineDamage((int)damage);
			}
		}

		public void ReqShowFloorBalance(int level, float time)
		{
			SFSObject sFSObject = new SFSObject();
			sFSObject.PutInt("Game", 18);
			sFSObject.PutInt("FlrLevl", level);
			sFSObject.PutFloat("MTim", time);
			smartFox.Send(new BroadcastMessageRequest(sFSObject));
		}

		public void ReqRescue(int playerId)
		{
			SFSObject sFSObject = new SFSObject();
			sFSObject.PutInt("Game", 10);
			sFSObject.PutInt("PRescueID", playerId);
			smartFox.Send(new ObjectMessageRequest(playerId, sFSObject));
		}

		public void ReqUnLockOfRescue(TNetUser user)
		{
			SFSObject sFSObject = new SFSObject();
			sFSObject.PutInt("Game", 11);
			sFSObject.PutInt("ULocPId", MineUser.Id);
			smartFox.Send(new ObjectMessageRequest(user.Id, sFSObject));
		}

		public void SendLock(int id)
		{
			if (smartFox.CurRoom != null)
			{
				smartFox.Send(new LockSthRequest(id.ToString()));
			}
		}

		public void SendUnLock(int id)
		{
			if (smartFox.CurRoom != null)
			{
				smartFox.Send(new UnLockSthRequest(id.ToString()));
			}
		}

		private void OnUserLeaveRoom(TNetEventData evt)
		{
			TNetUser tNetUser = (TNetUser)evt.data["user"];
			TNetRoom curRoom = smartFox.CurRoom;
			PlayerManager.Instance.DestroyEnemy(tNetUser.Id);
			NBattleUIScript nBattleUIScript = SceneUIManager.Instance().GetSceneUIObject().GetComponent(typeof(NBattleUIScript)) as NBattleUIScript;
			if (nBattleUIScript != null)
			{
				nBattleUIScript.ResetPlayersUIMsg();
			}
			Debug.Log("User " + tNetUser.Name + " left");
			if (!curRoom.ContainsVariable(TNetRoomVarType.E_BeginBattleTime))
			{
				PlayerManager.Instance.JudgeGameStart();
			}
			if (GameApp.GetInstance().GetGameState().m_eGameMode.m_ePlayMode == GameState.NetworkGameMode.PlayMode.E_PVE_BossRush)
			{
				NEnemyManager.Instance.CheckEnemyHasOwner(curRoom);
				PlayerManager.Instance.CheckPVEEnd();
			}
			else
			{
				JudgeWinnerOfUserLeave();
			}
		}

		public void OnUserVariableUpdate(TNetEventData evt)
		{
			TNetUser tNetUser = (TNetUser)evt.data["user"];
			TNetUserVarType tNetUserVarType = (TNetUserVarType)(int)evt.data["key"];
			if (tNetUser.ContainsVariable(TNetUserVarType.E_StatisticsInfo))
			{
				SFSObject variable = tNetUser.GetVariable(TNetUserVarType.E_StatisticsInfo);
				int @int = variable.GetInt("KillCount");
				int int2 = variable.GetInt("DeathCount");
				GameApp.GetInstance().GetGameState().ChangeNPlayerStatistics(tNetUser.Id, @int, int2);
			}
		}

		public void OnRoomVariablesUpdate(TNetEventData evt)
		{
			TNetRoom curRoom = smartFox.CurRoom;
			TNetRoomVarType tNetRoomVarType = (TNetRoomVarType)(int)evt.data["key"];
			if (MineUser.IsJoinedInRoom(curRoom) && tNetRoomVarType == TNetRoomVarType.E_PVE_MSG)
			{
				SFSObject variable = curRoom.GetVariable(TNetRoomVarType.E_PVE_MSG);
				NEnemyManager.Instance.UpdateEnemyInfo(variable.GetSFSObject("Boss_MSG"));
				GameApp.GetInstance().GetGameState().m_eGameMode.PVE_FLOOR = variable.GetInt("PVE_Floor");
				if (GameApp.GetInstance().GetGameState().m_eGameMode.m_ePlayMode == GameState.NetworkGameMode.PlayMode.E_PVE_BossRush)
				{
					NEnemyManager.Instance.CheckEnemyHasOwner(curRoom);
				}
			}
		}

		public void OnMasterChanged(TNetEventData evt)
		{
			TNetUser tNetUser = (TNetUser)evt.data["user"];
			TNetRoom curRoom = smartFox.CurRoom;
			if (!curRoom.ContainsVariable(TNetRoomVarType.E_BeginBattleTime))
			{
				PlayerManager.Instance.JudgeGameStart();
			}
			if (GameApp.GetInstance().GetGameState().m_eGameMode.m_ePlayMode == GameState.NetworkGameMode.PlayMode.E_PVE_BossRush)
			{
				NEnemyManager.Instance.CheckEnemyHasOwner(curRoom);
				PlayerManager.Instance.CheckPVEEnd();
			}
			else
			{
				JudgeWinnerOfUserLeave();
				JudgeGameTimeOver();
			}
		}

		public void OnSthLocked(TNetEventData evt)
		{
			string s = (string)evt.data["key"];
			RoomLockResCmd.Result result = (RoomLockResCmd.Result)(int)evt.data["result"];
			if (result == RoomLockResCmd.Result.ok)
			{
				HandleLockAcquire(0, int.Parse(s));
			}
			else
			{
				Debug.LogWarning("Lock Error Because:" + result);
			}
		}

		public void OnSthUnLocked(TNetEventData evt)
		{
		}

		public void OnNetworkMessage(TNetEventData evt)
		{
			TNetUser tNetUser = (TNetUser)evt.data["user"];
			SFSObject sFSObject = (SFSObject)evt.data["message"];
			switch (sFSObject.GetInt("Game"))
			{
			case 3:
				if (tNetUser.Id != smartFox.Myself.Id)
				{
					HandleInstantiatePlayer(tNetUser, sFSObject);
				}
				break;
			case 4:
			case 5:
				break;
			case 6:
				if (tNetUser.Id != smartFox.Myself.Id)
				{
					HandleSyncPlayerWeapon(tNetUser, sFSObject);
				}
				break;
			case 7:
				if (tNetUser.Id != smartFox.Myself.Id)
				{
					HandleSyncPlayerAnimation(tNetUser, sFSObject);
				}
				break;
			case 8:
				HandleTransform(tNetUser, sFSObject);
				break;
			case 9:
				HandleSyncPlayerDataInfo(tNetUser, sFSObject);
				break;
			case 10:
				PlayerManager.Instance.GetPlayerClass().NResurrectionAtCurrentPos();
				ReqUnLockOfRescue(tNetUser);
				break;
			case 11:
				SendUnLock(sFSObject.GetInt("ULocPId"));
				break;
			case 13:
				NEnemyManager.Instance.SpawnEnemy(NEnemyManager.Instance.FromOject(sFSObject));
				break;
			case 14:
				if (tNetUser.Id != smartFox.Myself.Id)
				{
					NEnemyManager.Instance.ReceiveEnemyAnimation(sFSObject);
				}
				break;
			case 15:
				if (tNetUser.Id != smartFox.Myself.Id)
				{
					NEnemyManager.Instance.EnemyIsAttack(sFSObject);
				}
				break;
			case 16:
				if (tNetUser.Id != smartFox.Myself.Id)
				{
					NEnemyManager.Instance.ReceiveSyncHp(sFSObject);
				}
				break;
			case 17:
				NEnemyManager.Instance.ReceiveNEnemyOnHitted(sFSObject, tNetUser);
				break;
			case 18:
				ReceiveShowFloorBalance(sFSObject);
				break;
			case 12:
			{
				string utfString = sFSObject.GetUtfString("CMDValue");
				NBattleUIScript nBattleUIScript = SceneUIManager.Instance().GetSceneUIObject().GetComponent(typeof(NBattleUIScript)) as NBattleUIScript;
				nBattleUIScript.AddMessage(true, utfString, NetWorkMessageInfo.E_NetCMD.E_StruckInformation);
				nBattleUIScript.UpdateUIToHeight();
				break;
			}
			default:
				DebugMsg = string.Empty;
				break;
			}
		}

		private void HandleLockAcquire(int code, int id)
		{
			if (code == 0)
			{
				Hashtable powerUPS = GameApp.GetInstance().GetGameState().GetPowerUPS();
				int num = (int)powerUPS[12] - 1;
				powerUPS[12] = ((num > 0) ? num : 0);
				NBattleUIScript nBattleUIScript = SceneUIManager.Instance().GetSceneUIObject().GetComponent(typeof(NBattleUIScript)) as NBattleUIScript;
				nBattleUIScript.UpdateFloorbalanceTimer();
				ReqRescue(id);
				DebugMsg = "[Receive Extension_LockAcquire] -Sucessed";
			}
			else
			{
				DebugMsg = "[Receive Extension_LockAcquire] -Error" + code;
			}
		}

		private void ShowTimeOutUI()
		{
			NBattleUIScript nBattleUIScript = SceneUIManager.Instance().GetSceneUIObject().GetComponent(typeof(NBattleUIScript)) as NBattleUIScript;
			if (nBattleUIScript.m_GameLoadingUI == null)
			{
				nBattleUIScript.SetupControlCoverUI(true);
				nBattleUIScript.SetupLoseConnectUI(true);
				TimeManager.Instance.Init(9, 5f, TimeOut, null, "E_LoseConnect");
			}
		}

		private void TimeOut()
		{
			SmartFoxConnection.DisConnect();
			(PlayerManager.Instance.GetPlayerObject().GetComponent(typeof(NetworkTransformSender)) as NetworkTransformSender).StopSendTransform();
			NBattleUIScript nBattleUIScript = SceneUIManager.Instance().GetSceneUIObject().GetComponent(typeof(NBattleUIScript)) as NBattleUIScript;
			nBattleUIScript.StagePassed();
		}

		private void HandleInstantiatePlayer(TNetUser sender, SFSObject dt)
		{
			int num = 0;
			int avatarHeadId = 1;
			int avatarBodyId = 1;
			int posId = 0;
			int weaponId = 1;
			if (sender.ContainsVariable(TNetUserVarType.E_PlayerInfo))
			{
				posId = dt.GetInt("PosID");
				SFSObject variable = sender.GetVariable(TNetUserVarType.E_PlayerInfo);
				avatarHeadId = variable.GetInt("AvatarHeadID");
				avatarBodyId = variable.GetInt("AvatarBodyID");
				weaponId = variable.GetInt("WeaponId");
			}
			num = sender.SitIndex;
			PlayerManager.Instance.SpawnPlayer(sender.Id, posId, avatarHeadId, avatarBodyId, weaponId, sender.Name);
		}

		private void HandleSyncPlayerAnimation(TNetUser sender, SFSObject data)
		{
			string utfString = data.GetUtfString("AnimationName");
			int @int = data.GetInt("WrapMode");
			float @float = data.GetFloat("FadeTime");
			float float2 = data.GetFloat("SpeedTime");
			PlayerManager.Instance.SyncAnimation(sender.Id, utfString, @int, @float, float2);
		}

		private void HandleTransform(TNetUser sender, SFSObject obj)
		{
			if (obj == null || sender == null)
			{
				return;
			}
			int id = sender.Id;
			NetworkTransform ntransform = NetworkTransform.FromSFSObject(obj);
			int @int = obj.GetInt("tarId");
			if (@int == -1)
			{
				if (id == smartFox.Myself.Id)
				{
					return;
				}
				Player recipient = PlayerManager.Instance.GetRecipient(id);
				if (recipient != null)
				{
					NetworkTransformReceiver networkTransformReceiver = recipient.PlayerObject.GetComponent(typeof(NetworkTransformReceiver)) as NetworkTransformReceiver;
					if (networkTransformReceiver != null)
					{
						networkTransformReceiver.ReceiveTransform(ntransform);
					}
				}
				return;
			}
			NEnemyManager.NEnemy nEnemyInfoByID = NEnemyManager.Instance.GetNEnemyInfoByID(@int);
			if (nEnemyInfoByID != null && nEnemyInfoByID.enemy != null && nEnemyInfoByID.isCopy == 1 && nEnemyInfoByID.enemy.enemyObject != null)
			{
				NetworkTransformReceiver networkTransformReceiver2 = nEnemyInfoByID.enemy.enemyObject.GetComponent(typeof(NetworkTransformReceiver)) as NetworkTransformReceiver;
				if (networkTransformReceiver2 != null)
				{
					networkTransformReceiver2.ReceiveTransform(ntransform);
				}
			}
		}

		private void HandleSyncPlayerWeapon(TNetUser sender, SFSObject dt)
		{
			if (dt.ContainsKey("WeaponTypeID"))
			{
				int @int = dt.GetInt("WeaponTypeID");
				PlayerManager.Instance.SyncWeapon(sender.Id, @int);
			}
		}

		private void HandleSyncPlayerDataInfo(TNetUser sender, SFSObject data)
		{
			float value = -99.9f;
			float num = -99.9f;
			NPlayerDataType @int = (NPlayerDataType)data.GetInt("CMDKey");
			if (data.ContainsKey("CMDValue"))
			{
				value = data.GetFloat("CMDValue");
			}
			if (data.ContainsKey("CMDValue2"))
			{
				num = data.GetFloat("CMDValue2");
				PlayerManager.Instance.SyncPlayerDataInfo(sender.Id, smartFox.Myself.Id, @int, value, num);
			}
			else
			{
				PlayerManager.Instance.SyncPlayerDataInfo(sender.Id, smartFox.Myself.Id, @int, value);
			}
			if (data.ContainsKey("CMDValueObj"))
			{
				PlayerManager.Instance.SyncPlayerDataInfo(sender.Id, smartFox.Myself.Id, @int, data);
			}
		}

		public void ReceiveShowFloorBalance(SFSObject table)
		{
			int @int = table.GetInt("FlrLevl");
			float @float = table.GetFloat("MTim");
			NBattleUIScript nBattleUIScript = SceneUIManager.Instance().GetSceneUIObject().GetComponent(typeof(NBattleUIScript)) as NBattleUIScript;
			if (nBattleUIScript != null)
			{
				nBattleUIScript.SetupControlCoverUI(true);
				nBattleUIScript.SetupFloorbalance(true, @float, @int);
				nBattleUIScript.RestJoystick();
				TimeManager.Instance.Init(10, @float, nBattleUIScript.FloorbalanceTimeout, nBattleUIScript.UpdateFloorbalanceTimer, "FloorBalance");
			}
		}

		public void OnConnectionLost(TNetEventData evt)
		{
			DebugMsg = "[Receive OnConnectionLost] -|Time:" + Time.time;
			UnsubscribeDelegates();
			SmartFoxConnection.Reset();
			NBattleUIScript nBattleUIScript = SceneUIManager.Instance().GetSceneUIObject().GetComponent(typeof(NBattleUIScript)) as NBattleUIScript;
			nBattleUIScript.SetupLoadingToExchangeUI();
			nBattleUIScript.SetStagePassedGotoNextSceneUITimer(0f);
		}

		public void OnHeartBeatTimeOut(TNetEventData evt)
		{
			ShowTimeOutUI();
		}

		public void DisConnect()
		{
			SmartFoxConnection.DisConnect();
			DebugMsg = "[ReqDisConnect] -";
		}

		private void OnApplicationQuit()
		{
			UnsubscribeDelegates();
		}

		private void OnApplicationPause(bool pause)
		{
			if (pause)
			{
				Debug.Log("SmartFoxDisconnect");
				DisConnect();
			}
			else
			{
				Debug.Log("SmartFoxDisconnect");
				TimeOut();
			}
		}
	}
