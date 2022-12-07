using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TNetSdk;
using UnityEngine;
using Zombie3D;

public class PlayerManager : MonoBehaviour
{
	public GameObject m_playerObjPrefab;

	public GameObject m_RedGroupGOPrefab;

	public GameObject m_BlueGroupGOPrefab;

	private GameState gameState = GameApp.GetInstance().GetGameState();

	private float m_fUITimer = -2f;

	private float m_fUITime = 3f;

	public float m_fBrushStrangeTimer = -1f;

	private float m_fBrushStrangeTime = 8f;

	private static PlayerManager instance;

	private Dictionary<int, Player> recipients = new Dictionary<int, Player>();

	[CompilerGenerated]
	private static Comparison<KeyValuePair<int, float>> _003C_003Ef__am_0024cacheA;

	public static PlayerManager Instance
	{
		get
		{
			return instance;
		}
	}

	public Player GetPlayerClass()
	{
		return GameApp.GetInstance().GetGameScene().GetPlayer();
	}

	public GameObject GetPlayerObject()
	{
		return GameApp.GetInstance().GetGameScene().GetPlayer()
			.PlayerObject;
	}

	private void Awake()
	{
		instance = this;
	}

	private void Update()
	{
		if (m_fUITimer >= 0f)
		{
			m_fUITimer += Time.deltaTime;
		}
		if (m_fUITimer >= m_fUITime)
		{
			NBattleUIScript nBattleUIScript = SceneUIManager.Instance().GetSceneUIObject().GetComponent(typeof(NBattleUIScript)) as NBattleUIScript;
			nBattleUIScript.ResetLoadingToWaittingBeginGameUI();
			nBattleUIScript.BeginGameTimer();
			NetworkTransformSender networkTransformSender = GetPlayerObject().GetComponent(typeof(NetworkTransformSender)) as NetworkTransformSender;
			networkTransformSender.StartSendTransform();
			if (gameState.m_eGameMode.m_ePlayMode == GameState.NetworkGameMode.PlayMode.E_PVE_BossRush)
			{
				m_fBrushStrangeTimer = 0f;
			}
			m_fUITimer = -1f;
		}
		if (m_fBrushStrangeTimer >= 0f)
		{
			m_fBrushStrangeTimer += Time.deltaTime;
		}
		if (m_fBrushStrangeTimer >= m_fBrushStrangeTime)
		{
			NEnemyManager.Instance.SetBrushStrange();
			m_fBrushStrangeTimer = -1f;
		}
		foreach (Player value in recipients.Values)
		{
			value.DoLogic(Time.deltaTime);
		}
	}

	public void JudgeGameStart()
	{
		if (recipients.Count >= GameSetup.Instance.GetRoomUserList().Count - 1)
		{
			GameSetup.Instance.ReqStartbattle();
		}
	}

	public void SpawnPlayer(int id, int posId, int AvatarHeadId, int AvatarBodyId, int WeaponId, string name)
	{
		Debug.Log("SpawnPlayer|" + name + "|" + id + "|" + posId);
		if (GetRecipient(id) != null)
		{
			return;
		}
		Player player = new Player();
		player.PlayerObject = (GameObject)UnityEngine.Object.Instantiate(m_playerObjPrefab, Vector3.zero, GetPlayerObject().transform.rotation);
		player.Init();
		player.PlayerObject.name = "PlayerCopy_" + id;
		if (posId >= 0)
		{
			player.PlayerObject.transform.position = GetPlayerPosition(gameState.m_eGameMode.m_eCooperaMode, posId);
		}
		else
		{
			player.PlayerObject.transform.position = Vector3.zero;
		}
		recipients[id] = player;
		if (GameSetup.Instance.GetGroupId(id) == -1)
		{
			DestroyEnemy(id);
			return;
		}
		player.m_iNGroupID = GameSetup.Instance.GetGroupId(id);
		GameObject gameObject = player.GetTransform().Find("BloodRect").gameObject;
		if (gameObject != null)
		{
			player.bloodRect = gameObject.GetComponent(typeof(BloodRect)) as BloodRect;
			if (player.bloodRect != null)
			{
				player.bloodRect.SetBloodPercent(1f);
			}
		}
		if (player.m_iNGroupID == 1 || player.m_iNGroupID == 2)
		{
			GameObject gameObject2;
			if (player.m_iNGroupID == 1)
			{
				gameObject2 = (GameObject)UnityEngine.Object.Instantiate(m_RedGroupGOPrefab, Vector3.zero, Quaternion.identity);
				if (gameObject != null)
				{
					gameObject.transform.Find("blood_01").gameObject.GetComponent<Renderer>().material.SetColor("_TintColor", Constant.RedGroupColor);
				}
			}
			else
			{
				gameObject2 = (GameObject)UnityEngine.Object.Instantiate(m_BlueGroupGOPrefab, Vector3.zero, Quaternion.identity);
				if (gameObject != null)
				{
					gameObject.transform.Find("blood_01").gameObject.GetComponent<Renderer>().material.SetColor("_TintColor", Constant.BlueGroupColor);
				}
			}
			gameObject2.transform.parent = player.PlayerObject.transform;
			gameObject2.transform.localPosition = Vector3.zero;
		}
		else
		{
			GameObject gameObject3;
			if (player.m_iNGroupID == 0)
			{
				gameObject3 = (GameObject)UnityEngine.Object.Instantiate(m_RedGroupGOPrefab, Vector3.zero, Quaternion.identity);
				if (gameObject != null)
				{
					gameObject.transform.Find("blood_01").gameObject.GetComponent<Renderer>().material.SetColor("_TintColor", Constant.RedGroupColor);
				}
			}
			else
			{
				gameObject3 = null;
			}
			if (gameObject3 != null)
			{
				gameObject3.transform.parent = player.PlayerObject.transform;
				gameObject3.transform.localPosition = Vector3.zero;
			}
		}
		SetPlayerAvater(player.PlayerObject, AvatarHeadId, 0, AvatarBodyId, 1);
		Weapon w = WeaponFactory.GetInstance().CreateWeapon((WeaponType)WeaponId);
		player.ChangeWeapon(w, true);
		JudgeGameStart();
		GameApp.GetInstance().GetGameState().InitPlayerStatistics(id, name, player.m_iNGroupID, AvatarHeadId);
	}

	public void SyncAnimation(int id, string animationName, int wrapMode, float fadeTime, float speedTime)
	{
		Player recipient = GetRecipient(id);
		if (recipient != null)
		{
			if (animationName.StartsWith("Death"))
			{
				recipient.PlayerObject.GetComponent<Animation>()[animationName].wrapMode = WrapMode.ClampForever;
			}
			else if (recipient != null && recipient.PlayerObject != null)
			{
				recipient.PlayerObject.GetComponent<Animation>()[animationName].wrapMode = WrapMode.Loop;
			}
			if (animationName.StartsWith("Shoot"))
			{
				recipient.SetCirculationAttackFromNetwork(true);
			}
			else
			{
				recipient.SetCirculationAttackFromNetwork(false);
			}
			recipient.PlayerObject.GetComponent<Animation>()[animationName].speed = speedTime;
			recipient.PlayerObject.GetComponent<Animation>().CrossFade(animationName, fadeTime);
		}
	}

	public void SyncWeapon(int id, int weaponTypeID)
	{
		Weapon w = WeaponFactory.GetInstance().CreateWeapon((WeaponType)weaponTypeID);
		if (GetRecipient(id) == null)
		{
			if (GameSetup.Instance.GetUserByID(id) == null)
			{
				return;
			}
			TNetUser userByID = GameSetup.Instance.GetUserByID(id);
			int avatarHeadId = 1;
			int avatarBodyId = 1;
			int weaponId = 1;
			if (userByID.ContainsVariable(TNetUserVarType.E_PlayerInfo))
			{
				SFSObject variable = userByID.GetVariable(TNetUserVarType.E_PlayerInfo);
				avatarHeadId = variable.GetInt("AvatarHeadID");
				avatarBodyId = variable.GetInt("AvatarBodyID");
				weaponId = variable.GetInt("WeaponId");
			}
			SpawnPlayer(id, -1, avatarHeadId, avatarBodyId, weaponId, userByID.Name);
		}
		GetRecipient(id).ChangeWeapon(w, true);
	}

	public void CheckPVEEnd()
	{
		if (!GameSetup.Instance.RoomOwnerIsMe)
		{
			return;
		}
		bool flag = true;
		foreach (Player value in recipients.Values)
		{
			if (value.HP > 0f)
			{
				flag = false;
			}
		}
		if (flag && GetPlayerClass().HP <= 0f)
		{
			GameSetup.Instance.ReqSyncPlayerInfo(GameSetup.NPlayerDataType.E_BattleEnd, -1f);
			GameSetup.Instance.m_bIsSendBattleEndMsg = true;
		}
	}

	public void SyncPlayerDataInfo(int id, int myId, GameSetup.NPlayerDataType dataType, float Value, float Value2 = -99.9f)
	{
		switch (dataType)
		{
		case GameSetup.NPlayerDataType.E_Fire:
			if (myId != id)
			{
				Player recipient2 = GetRecipient(id);
				if (recipient2 != null || recipient2.GetWeapon().GetWeaponType() == WeaponType.Hellfire)
				{
					recipient2.SetCirculationAttackFromNetwork(true);
				}
			}
			break;
		case GameSetup.NPlayerDataType.E_StopFire:
			if (myId != id)
			{
				Player recipient = GetRecipient(id);
				if (recipient != null)
				{
					recipient.SetCirculationAttackFromNetwork(false);
				}
			}
			break;
		case GameSetup.NPlayerDataType.E_HP:
			if (myId != id && Value != -99.9f && GetRecipient(id) != null)
			{
				GetRecipient(id).HP = Value;
			}
			if (GameApp.GetInstance().GetGameState().m_eGameMode.m_ePlayMode == GameState.NetworkGameMode.PlayMode.E_PVE_BossRush)
			{
				CheckPVEEnd();
			}
			break;
		case GameSetup.NPlayerDataType.E_MaxHP:
			if (myId != id && Value != -99.9f && GetRecipient(id) != null)
			{
				GetRecipient(id).SetMaxHp(Value);
			}
			break;
		case GameSetup.NPlayerDataType.E_Hitted:
			if (Value != -99.9f)
			{
				GetPlayerClass().OnHit(Value, id);
			}
			break;
		case GameSetup.NPlayerDataType.E_Skill_Vertigo:
			if (Value != -99.9f)
			{
				GetPlayerClass().ShowSkillEffect_Vertigo();
				GameSetup.Instance.ReqSyncPlayerInfo(GameSetup.NPlayerDataType.E_Skill_VertigoEffect, 1f);
			}
			break;
		case GameSetup.NPlayerDataType.E_HittedColorChanged:
			if (myId != id && Value != -99.9f && GetRecipient(id) != null)
			{
				GetRecipient(id).OnHitChangeColor(Value, false);
			}
			break;
		case GameSetup.NPlayerDataType.E_PlayerDeath:
			if (myId != id && GetRecipient(id) != null)
			{
				GetRecipient(id).ShowNPlayerDead();
			}
			break;
		case GameSetup.NPlayerDataType.E_PlayerALive:
			if (myId == id)
			{
				break;
			}
			if (Value != -99.9f && Value2 != -99.9f)
			{
				Vector3 pos = new Vector3(Value, 10000.1f, Value2);
				if (GetRecipient(id) != null)
				{
					GetRecipient(id).ShowNPlayerALive(pos);
				}
			}
			if (id != myId && GetRecipient(id) != null)
			{
				GetRecipient(id).SetBloodVisable(true);
			}
			break;
		case GameSetup.NPlayerDataType.E_Skill_FancyFootwork:
			if (id == myId)
			{
				GetPlayerClass().ShowSkillEffect_FancyFootwork();
			}
			else if (GetRecipient(id) != null)
			{
				GetRecipient(id).ShowSkillEffect_FancyFootwork();
			}
			break;
		case GameSetup.NPlayerDataType.E_Skill_HailMary:
			if (id == myId)
			{
				GetPlayerClass().ShowSkillEffect_HailMary();
			}
			else if (GetRecipient(id) != null)
			{
				GetRecipient(id).ShowSkillEffect_HailMary();
			}
			break;
		case GameSetup.NPlayerDataType.E_Skill_VertigoEffect:
			if (myId == id || Value == -99.9f)
			{
				break;
			}
			if (Value == 0f)
			{
				if (GetRecipient(id) != null)
				{
					GetRecipient(id).ShowSkillEffect_Vertigo(false);
				}
			}
			else if (GetRecipient(id) != null)
			{
				GetRecipient(id).ShowSkillEffect_Vertigo();
			}
			break;
		case GameSetup.NPlayerDataType.E_Skill_RunFast:
			if (myId == id || Value == -99.9f)
			{
				break;
			}
			if (Value == 0f)
			{
				if (GetRecipient(id) != null)
				{
					GetRecipient(id).SpeedUpByStamina = false;
				}
			}
			else if (GetRecipient(id) != null)
			{
				GetRecipient(id).SpeedUpByStamina = true;
			}
			break;
		case GameSetup.NPlayerDataType.E_Invincible:
			if (myId == id || Value == -99.9f)
			{
				break;
			}
			if (Value == 0f)
			{
				if (GetRecipient(id) != null)
				{
					GetRecipient(id).ShowResurrectionNoAttck(false);
				}
			}
			else if (GetRecipient(id) != null)
			{
				GetRecipient(id).ShowResurrectionNoAttck(true);
			}
			break;
		case GameSetup.NPlayerDataType.E_Statistics_Death:
			if (GameApp.GetInstance().GetGameState().m_eGameMode.m_eCooperaMode == GameState.NetworkGameMode.NetworkCooperationMode.E_Simple)
			{
				GameSetup.Instance.m_iDeathPlayerCount++;
			}
			else if (GetRecipient(id) != null && GetRecipient(id).m_iNGroupID != GetPlayerClass().m_iNGroupID)
			{
				GameSetup.Instance.m_iDeathPlayerCount++;
			}
			if (id != myId)
			{
				GetRecipient(id).SetBloodVisable(false);
			}
			break;
		case GameSetup.NPlayerDataType.E_BattleStart:
			if (m_fUITimer == -2f)
			{
				m_fUITimer = 0f;
			}
			if (Value != -99.9f && Value != -1f && Value > 0f && myId != id)
			{
				GameSetup.Instance.m_fCountDownTime = Value;
				GameSetup.Instance.m_fCountDownTimer = 0f;
			}
			break;
		case GameSetup.NPlayerDataType.E_GamerTimer:
			if (Value != -99.9f && Value != -1f && Value >= 0f)
			{
				if (myId != id)
				{
					GameSetup.Instance.m_fCountDownTimer = Value;
				}
				NBattleUIScript nBattleUIScript2 = SceneUIManager.Instance().GetSceneUIObject().GetComponent(typeof(NBattleUIScript)) as NBattleUIScript;
				nBattleUIScript2.SyncCountDownText();
			}
			break;
		case GameSetup.NPlayerDataType.E_BattleEnd:
		{
			int winID = -1;
			if (Value != -99.9f)
			{
				winID = (int)Value;
			}
			(GetPlayerObject().GetComponent(typeof(NetworkTransformSender)) as NetworkTransformSender).StopSendTransform();
			foreach (KeyValuePair<int, GameState.NetworkGameMode.NetworkPlayerStatistics> plaersStatistic in GameApp.GetInstance().GetGameState().m_eGameMode.m_PlaersStatistics)
			{
				bool flag = false;
				foreach (TNetUser roomUser in GameSetup.Instance.GetRoomUserList())
				{
					if (plaersStatistic.Key == roomUser.Id)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					gameState.m_eGameMode.m_PlaersStatistics[plaersStatistic.Key].m_bIsOline = false;
				}
				else
				{
					gameState.m_eGameMode.m_PlaersStatistics[plaersStatistic.Key].m_bIsOline = true;
				}
			}
			JudgeWinner(winID);
			GameSetup.Instance.UnsubscribeDelegates(TNetEventRoom.USER_VARIABLES_UPDATE, GameSetup.Instance.OnUserVariableUpdate);
			if (gameState.m_eGameMode.m_PlaersStatistics[myId].m_bIsWinner || gameState.m_eGameMode.m_PlaersStatistics[myId].m_bIsBestKiller)
			{
				gameState.m_eGameMode.m_PlaersStatistics[myId].m_lsReward.Clear();
				if (gameState.m_eGameMode.m_PlaersStatistics[myId].m_bIsWinner)
				{
					if (gameState.m_eGameMode.m_eCooperaMode == GameState.NetworkGameMode.NetworkCooperationMode.E_Simple)
					{
						if (gameState.m_eGameMode.m_PlaersStatistics[myId].m_iKillCount > 0)
						{
							foreach (KeyValuePair<int, int> item in GetMap3_Type_Reward(2))
							{
								gameState.m_eGameMode.m_PlaersStatistics[myId].m_lsReward.Add(item);
							}
						}
					}
					else
					{
						foreach (KeyValuePair<int, int> item2 in GetMap3_Type_Reward(1))
						{
							gameState.m_eGameMode.m_PlaersStatistics[myId].m_lsReward.Add(item2);
						}
					}
				}
				if (gameState.m_eGameMode.m_PlaersStatistics[myId].m_bIsBestKiller)
				{
					if (gameState.m_eGameMode.m_eCooperaMode == GameState.NetworkGameMode.NetworkCooperationMode.E_Simple)
					{
						if (gameState.m_eGameMode.m_PlaersStatistics[myId].m_iKillCount > 0)
						{
							foreach (KeyValuePair<int, int> item3 in GetMap3_Type_Reward(2))
							{
								gameState.m_eGameMode.m_PlaersStatistics[myId].m_lsReward.Add(item3);
							}
						}
					}
					else
					{
						foreach (KeyValuePair<int, int> item4 in GetMap3_Type_Reward(2))
						{
							gameState.m_eGameMode.m_PlaersStatistics[myId].m_lsReward.Add(item4);
						}
					}
				}
				if (gameState.m_eGameMode.m_PlaersStatistics[myId].m_bIsWinner)
				{
					gameState.AddPVPWinTimes();
					if (gameState.m_eGameMode.m_PlaersStatistics[myId].m_iDeathCount <= 0)
					{
						gameState.CompleteNoHeartToWinOneGame();
					}
					if (GameSetup.Instance.m_oncePVPCombatDR != null && !GameSetup.Instance.m_oncePVPCombatDR.m_UseFastRun)
					{
						gameState.AddNoUseBestRunWinGameCount();
					}
				}
				if (gameState.m_eGameMode.m_PlaersStatistics[myId].m_bIsBestKiller)
				{
					gameState.AddPVPBestKillerCount();
				}
				if (GameSetup.Instance.MineUser.ContainsVariable(TNetUserVarType.E_PlayerInfo))
				{
					SFSObject variable = GameSetup.Instance.MineUser.GetVariable(TNetUserVarType.E_PlayerInfo);
					int @int = variable.GetInt("AvatarHeadID");
					int int2 = variable.GetInt("AvatarBodyID");
					if (@int == 22 && int2 == 22)
					{
						gameState.AddWearShinobiToPlayTimes();
					}
					else if (@int == 23 && int2 == 23)
					{
						gameState.AddWearShinobiToPlayTimes();
					}
					if (@int == int2)
					{
						gameState.WearDifferentAvatarTimes((Avatar.AvatarSuiteType)@int);
					}
				}
			}
			if (gameState.m_eGameMode.m_ePlayMode == GameState.NetworkGameMode.PlayMode.E_PVE_BossRush)
			{
				gameState.m_eGameMode.m_PlaersStatistics[GameSetup.Instance.MineUser.Id].AddlsReward(new KeyValuePair<int, int>(102, gameState.m_eGameMode.m_PlaersStatistics[GameSetup.Instance.MineUser.Id].m_iMyDamage / 10));
				gameState.m_eGameMode.m_PlaersStatistics[GameSetup.Instance.MineUser.Id].AddlsReward(new KeyValuePair<int, int>(101, gameState.m_eGameMode.m_PlaersStatistics[GameSetup.Instance.MineUser.Id].m_iMyDamage / 20));
			}
			GameSetup.Instance.ReqSyncPlayerInfoListIntValue(GameSetup.NPlayerDataType.E_BattleEndReward, gameState.m_eGameMode.m_PlaersStatistics[myId].m_lsReward, gameState.m_eGameMode.m_PlaersStatistics[myId].m_iMyDamage);
			NBattleUIScript nBattleUIScript = SceneUIManager.Instance().GetSceneUIObject().GetComponent(typeof(NBattleUIScript)) as NBattleUIScript;
			if (GameApp.GetInstance().GetGameState().m_eGameMode.m_ePlayMode == GameState.NetworkGameMode.PlayMode.E_PVE_BossRush)
			{
				nBattleUIScript.StagePassed();
			}
			else if (GameSetup.Instance.m_fCountDownTimer <= 10f)
			{
				nBattleUIScript.StageLosed();
			}
			else
			{
				nBattleUIScript.StagePassed();
			}
			break;
		}
		}
	}

	public void SyncPlayerDataInfo(int id, int myId, GameSetup.NPlayerDataType dataType, SFSObject data)
	{
		switch (dataType)
		{
		case GameSetup.NPlayerDataType.E_BattleEndReward:
			if (myId != id)
			{
				List<KeyValuePair<int, int>> list = new List<KeyValuePair<int, int>>();
				int[] intArray = data.GetSFSObject("CMDValueObjList").GetIntArray("key");
				int[] intArray2 = data.GetSFSObject("CMDValueObjList").GetIntArray("value");
				for (int i = 0; i < Mathf.Min(intArray.Length, intArray2.Length); i++)
				{
					KeyValuePair<int, int> item = new KeyValuePair<int, int>(intArray[i], intArray2[i]);
					list.Add(item);
				}
				if (list.Count > 0 && gameState.GetPlayerStatistics(id) != null)
				{
					gameState.GetPlayerStatistics(id).m_lsReward = list;
				}
				if (data.ContainsKey("SDam"))
				{
					gameState.GetPlayerStatistics(id).m_iMyDamage = data.GetInt("SDam");
				}
			}
			break;
		case GameSetup.NPlayerDataType.E_Statistics_Killed:
		{
			if (!data.ContainsKey("CMDValue_Double"))
			{
				break;
			}
			double @double = data.GetDouble("CMDValue_Double");
			GameSetup.Instance.BattleStatisticsOfKill();
			GameApp.GetInstance().GetGameState().AddNBattleStatisticsOnce(GameState.NetworkGameMode.NBattleStatistics.E_NBATTLEKILLS);
			gameState.AddPVPKillPlayerCount();
			if (GameSetup.Instance.m_oncePVPCombatDR != null)
			{
				GameSetup.Instance.m_oncePVPCombatDR.KillPlayer(id, @double);
			}
			Faceout3DTextPool.Instance().Create3DText(GetPlayerObject().transform.position + Vector3.up * 1.5f, Quaternion.Euler(55f, 0f, 0f), "+100", Color.green);
			if (GameApp.GetInstance().GetGameState().m_eGameMode.m_ePlayMode != GameState.NetworkGameMode.PlayMode.E_LastStand)
			{
				break;
			}
			bool flag = false;
			int num = -1;
			if (GameApp.GetInstance().GetGameState().m_eGameMode.m_eCooperaMode == GameState.NetworkGameMode.NetworkCooperationMode.E_Simple)
			{
				foreach (Player recipientPlayer in Instance.GetRecipientPlayerList())
				{
					if (recipientPlayer.HP > 0f)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					num = myId;
				}
			}
			else if (GameApp.GetInstance().GetGameState().m_eGameMode.m_eCooperaMode == GameState.NetworkGameMode.NetworkCooperationMode.E_Team)
			{
				int iNGroupID = Instance.GetPlayerClass().m_iNGroupID;
				foreach (Player recipientPlayer2 in Instance.GetRecipientPlayerList())
				{
					if (recipientPlayer2.m_iNGroupID != iNGroupID && recipientPlayer2.HP > 0f)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					num = GetPlayerClass().m_iNGroupID;
				}
			}
			if (!flag && num != -1)
			{
				GameSetup.Instance.ReqSyncPlayerInfo(GameSetup.NPlayerDataType.E_BattleEnd, num);
				GameSetup.Instance.m_bIsSendBattleEndMsg = true;
			}
			break;
		}
		}
	}

	public Dictionary<int, Player> GetRecipientList()
	{
		return recipients;
	}

	public List<Player> GetRecipientPlayerList()
	{
		List<Player> list = new List<Player>();
		foreach (Player value in recipients.Values)
		{
			list.Add(value);
		}
		return list;
	}

	public Player GetRecipient(int id)
	{
		if (recipients.ContainsKey(id))
		{
			return recipients[id];
		}
		return null;
	}

	public int GetRecipientId(Player player)
	{
		if (recipients.ContainsValue(player))
		{
			foreach (int key in recipients.Keys)
			{
				if (recipients[key] == player)
				{
					return key;
				}
			}
		}
		return -1;
	}

	public Player GetRecipientByObj(GameObject obj)
	{
		foreach (Player value in recipients.Values)
		{
			if (value.PlayerObject == obj)
			{
				return value;
			}
		}
		return null;
	}

	public void DestroyEnemy(int id)
	{
		Player recipient = GetRecipient(id);
		if (recipient != null)
		{
			recipient.Clear();
			recipients.Remove(id);
		}
	}

	public void SetPlayerAvater(GameObject playerObj, int HeadSuiteType, int HeadType, int BodySuiteType, int BodyType)
	{
		Avatar avatar = new Avatar((Avatar.AvatarSuiteType)HeadSuiteType, (Avatar.AvatarType)HeadType);
		Avatar avatar2 = new Avatar((Avatar.AvatarSuiteType)BodySuiteType, (Avatar.AvatarType)BodyType);
		Renderer[] componentsInChildren = playerObj.GetComponentsInChildren<Renderer>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (componentsInChildren[i].transform.parent.gameObject == playerObj && componentsInChildren[i].transform.name != "shadow")
			{
				componentsInChildren[i].enabled = false;
			}
		}
		for (int j = 0; j < componentsInChildren.Length; j++)
		{
			if (!(componentsInChildren[j].transform.parent.gameObject == playerObj) || !(componentsInChildren[j].transform.name != "shadow"))
			{
				continue;
			}
			for (int k = 0; k < avatar.MeshPathList.Count; k++)
			{
				if (componentsInChildren[j].gameObject.name == avatar.MeshPathList[k])
				{
					componentsInChildren[j].enabled = true;
					componentsInChildren[j].material = Resources.Load("Zombie3D/Avatar/" + avatar.MatPathList[k]) as Material;
				}
			}
			for (int l = 0; l < avatar2.MeshPathList.Count; l++)
			{
				if (componentsInChildren[j].gameObject.name == avatar2.MeshPathList[l])
				{
					componentsInChildren[j].enabled = true;
					componentsInChildren[j].material = Resources.Load("Zombie3D/Avatar/" + avatar2.MatPathList[l]) as Material;
				}
			}
		}
	}

	public Vector3 GetPlayerPosition(GameState.NetworkGameMode.NetworkCooperationMode copMode, int PosId = -1)
	{
		Vector3 result = Vector3.zero;
		int num = 0;
		num = ((PosId == -1) ? UnityEngine.Random.Range(0, GameSetup.Instance.m_spawnSimpleGO.transform.GetChildCount()) : PosId);
		switch (copMode)
		{
		case GameState.NetworkGameMode.NetworkCooperationMode.E_Simple:
			result = new Vector3(GameSetup.Instance.m_spawnSimpleGO.transform.GetChild(num).position.x, 10000.1f, GameSetup.Instance.m_spawnSimpleGO.transform.GetChild(num).position.z);
			break;
		case GameState.NetworkGameMode.NetworkCooperationMode.E_Team:
			result = ((PosId != -1) ? new Vector3(GameSetup.Instance.m_spawnTeamGO.transform.GetChild(num).position.x, 10000.1f, GameSetup.Instance.m_spawnTeamGO.transform.GetChild(num).position.z) : new Vector3(GameSetup.Instance.m_spawnSimpleGO.transform.GetChild(num).position.x, 10000.1f, GameSetup.Instance.m_spawnSimpleGO.transform.GetChild(num).position.z));
			break;
		default:
			result = new Vector3(0f, 10000.1f, 0f);
			GameSetup.Instance.DebugMsg = "Get Wrong Position";
			break;
		}
		return result;
	}

	public int CalPlayerScore(int id)
	{
		int iDeathCount = GameApp.GetInstance().GetGameState().GetPlayerStatistics(id)
			.m_iDeathCount;
		int iKillCount = GameApp.GetInstance().GetGameState().GetPlayerStatistics(id)
			.m_iKillCount;
		return iKillCount - iDeathCount;
	}

	public int CalWinner(GameState.NetworkGameMode.NetworkCooperationMode mode)
	{
		Dictionary<int, GameState.NetworkGameMode.NetworkPlayerStatistics> plaersStatistics = GameApp.GetInstance().GetGameState().m_eGameMode.m_PlaersStatistics;
		int result = 0;
		int num = -999;
		int num2 = -999;
		Dictionary<int, int> dictionary = new Dictionary<int, int>();
		foreach (KeyValuePair<int, Player> recipient in recipients)
		{
			int iNGroupID = recipient.Value.m_iNGroupID;
			num = CalPlayerScore(recipient.Key);
			switch (mode)
			{
			case GameState.NetworkGameMode.NetworkCooperationMode.E_Simple:
				if (num > num2)
				{
					result = recipient.Key;
					num2 = num;
				}
				break;
			case GameState.NetworkGameMode.NetworkCooperationMode.E_Team:
				if (dictionary.ContainsKey(iNGroupID))
				{
					dictionary[iNGroupID] += num;
				}
				else
				{
					dictionary.Add(iNGroupID, num);
				}
				break;
			}
		}
		switch (mode)
		{
		case GameState.NetworkGameMode.NetworkCooperationMode.E_Simple:
			num = CalPlayerScore(GameSetup.Instance.MineUser.Id);
			if (num > num2)
			{
				result = GameSetup.Instance.MineUser.Id;
			}
			break;
		case GameState.NetworkGameMode.NetworkCooperationMode.E_Team:
		{
			int iNGroupID2 = GetPlayerClass().m_iNGroupID;
			num = CalPlayerScore(GameSetup.Instance.MineUser.Id);
			if (dictionary.ContainsKey(iNGroupID2))
			{
				dictionary[iNGroupID2] += num;
			}
			else
			{
				dictionary.Add(iNGroupID2, num);
			}
			int num3 = -999;
			int num4 = -999;
			{
				foreach (KeyValuePair<int, int> item in dictionary)
				{
					num3 = item.Value;
					if (num3 > num4)
					{
						result = item.Key;
						num4 = num3;
					}
				}
				return result;
			}
		}
		}
		return result;
	}

	public List<int> SortPlaersStatistics()
	{
		List<int> list = new List<int>();
		List<KeyValuePair<int, float>> list2 = new List<KeyValuePair<int, float>>();
		foreach (KeyValuePair<int, GameState.NetworkGameMode.NetworkPlayerStatistics> plaersStatistic in GameApp.GetInstance().GetGameState().m_eGameMode.m_PlaersStatistics)
		{
			float value = plaersStatistic.Value.m_iKillCount - plaersStatistic.Value.m_iDeathCount;
			KeyValuePair<int, float> item = new KeyValuePair<int, float>(plaersStatistic.Key, value);
			list2.Add(item);
		}
		if (_003C_003Ef__am_0024cacheA == null)
		{
			_003C_003Ef__am_0024cacheA = _003CSortPlaersStatistics_003Em__0;
		}
		list2.Sort(_003C_003Ef__am_0024cacheA);
		for (int num = list2.Count - 1; num >= 0; num--)
		{
			list.Add(list2[num].Key);
		}
		return list;
	}

	public void JudgeWinner(int WinID)
	{
		if (WinID == -1)
		{
			return;
		}
		if (gameState.m_eGameMode.m_eCooperaMode == GameState.NetworkGameMode.NetworkCooperationMode.E_Simple)
		{
			gameState.m_eGameMode.m_PlaersStatistics[WinID].m_bIsWinner = true;
		}
		else if (gameState.m_eGameMode.m_eCooperaMode == GameState.NetworkGameMode.NetworkCooperationMode.E_Team)
		{
			foreach (KeyValuePair<int, GameState.NetworkGameMode.NetworkPlayerStatistics> plaersStatistic in gameState.m_eGameMode.m_PlaersStatistics)
			{
				if (plaersStatistic.Value.m_iNGroup == WinID)
				{
					plaersStatistic.Value.m_bIsWinner = true;
				}
			}
		}
		int num = GameSetup.Instance.MineUser.Id;
		if (gameState.m_eGameMode.m_eCooperaMode == GameState.NetworkGameMode.NetworkCooperationMode.E_Simple)
		{
			foreach (KeyValuePair<int, GameState.NetworkGameMode.NetworkPlayerStatistics> plaersStatistic2 in gameState.m_eGameMode.m_PlaersStatistics)
			{
				if (plaersStatistic2.Key != GameSetup.Instance.MineUser.Id && plaersStatistic2.Value.m_bIsOline && gameState.GetPlayerStatistics(num).m_iKillCount <= plaersStatistic2.Value.m_iKillCount)
				{
					num = plaersStatistic2.Key;
				}
			}
		}
		else if (gameState.m_eGameMode.m_eCooperaMode == GameState.NetworkGameMode.NetworkCooperationMode.E_Team)
		{
			foreach (KeyValuePair<int, GameState.NetworkGameMode.NetworkPlayerStatistics> plaersStatistic3 in gameState.m_eGameMode.m_PlaersStatistics)
			{
				if (plaersStatistic3.Key != GameSetup.Instance.MineUser.Id && plaersStatistic3.Value.m_bIsOline && CalPlayerScore(num) <= CalPlayerScore(plaersStatistic3.Key))
				{
					num = plaersStatistic3.Key;
				}
			}
		}
		gameState.m_eGameMode.m_PlaersStatistics[num].m_bIsBestKiller = true;
	}

	private List<KeyValuePair<int, int>> GetMap3_Type_Reward(int mode, int GiveCount = 1)
	{
		List<KeyValuePair<int, int>> list = new List<KeyValuePair<int, int>>();
		List<KeyValuePair<int, int>> list2 = new List<KeyValuePair<int, int>>();
		switch (mode)
		{
		case 1:
			list2.Add(new KeyValuePair<int, int>(2, 20));
			list2.Add(new KeyValuePair<int, int>(4, 15));
			list2.Add(new KeyValuePair<int, int>(7, 20));
			list2.Add(new KeyValuePair<int, int>(10, 15));
			list2.Add(new KeyValuePair<int, int>(8, 15));
			list2.Add(new KeyValuePair<int, int>(12, 15));
			break;
		case 2:
			list2.Add(new KeyValuePair<int, int>(2, 20));
			list2.Add(new KeyValuePair<int, int>(4, 15));
			list2.Add(new KeyValuePair<int, int>(7, 20));
			list2.Add(new KeyValuePair<int, int>(10, 10));
			list2.Add(new KeyValuePair<int, int>(8, 10));
			list2.Add(new KeyValuePair<int, int>(12, 15));
			list2.Add(new KeyValuePair<int, int>(-1, 10));
			break;
		default:
			list.Add(new KeyValuePair<int, int>(0, 1));
			Debug.LogError("Error mode !!" + mode);
			return list;
		}
		for (int i = 0; i < GiveCount; i++)
		{
			int index = RandomGift(list2);
			list.Add(new KeyValuePair<int, int>(list2[index].Key, 1));
		}
		return list;
	}

	private int RandomGift(List<KeyValuePair<int, int>> _lGiftItem)
	{
		int num = UnityEngine.Random.Range(0, 100);
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		for (int i = 0; i < _lGiftItem.Count; i++)
		{
			int value = _lGiftItem[i].Value;
			num3 = num4;
			num4 = value + num3;
			if (num >= num3 && num < num4)
			{
				num2 = Mathf.Clamp(i, 0, _lGiftItem.Count);
				break;
			}
		}
		if (_lGiftItem[num2].Key == -1)
		{
			int everyDayCrystalLootTotalCount = gameState.everyDayCrystalLootTotalCount;
			num2 = ((everyDayCrystalLootTotalCount < 15) ? num2 : UnityEngine.Random.Range(0, num2));
		}
		return num2;
	}

	[CompilerGenerated]
	private static int _003CSortPlaersStatistics_003Em__0(KeyValuePair<int, float> k1, KeyValuePair<int, float> k2)
	{
		return k1.Value.CompareTo(k2.Value);
	}
}
