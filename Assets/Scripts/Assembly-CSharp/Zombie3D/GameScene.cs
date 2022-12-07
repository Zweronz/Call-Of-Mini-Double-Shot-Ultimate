using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zombie3D
{
	public class GameScene
	{
		protected Player player;

		protected Player FriendPlayer;

		protected BaseCameraScript camera;

		protected Hashtable enemyList;

		protected Quest quest;

		protected List<BombSpot> bombSpotList;

		protected List<TriggerScript> triggerList;

		protected GameObject[] woodboxList;

		protected List<JerricanScript> jerricans;

		protected List<EnergyFeedwayScript> energyFeedways;

		protected List<GameObject> machines;

		protected PathDoor[] pathDoors;

		protected Vector3[] path;

		protected GameState gameState;

		protected GameParametersScript gameParameters;

		protected ObjectPool hitBloodObjectPool = new ObjectPool();

		protected string sceneName;

		protected int sceneIndex;

		protected int enemyNum;

		public bool m_bMapStartZoomShow;

		public bool waveWounded;

		public int notWoundedLastWaves;

		public int friendNotDeadLastWaves;

		private float m_LastSaveExchangeInfoTime;

		private float m_FriendChangeWeaponTimer;

		private float m_FriendChangeWeaponTime = 10f;

		protected PlayingState playingState;

		protected float difficultyFactor = 1f;

		protected int killed;

		protected int triggerCount;

		protected int enemyID;

		protected float spawnWoodBoxesTime = -30f;

		protected int curWaveKilled;

		protected float m_LastWavePassedTime;

		protected GameParametersXML gameParamFromXML;

		private float _TimeBeginByTimeMode = -1f;

		private float _TimeEndByTimeMode = 2000f;

		protected DDSTriggerScript ddsTrigger;

		public DDSTriggerScript DDSTrigger
		{
			get
			{
				return ddsTrigger;
			}
			set
			{
				ddsTrigger = value;
			}
		}

		public PlayingState PlayingState
		{
			get
			{
				return playingState;
			}
			set
			{
				playingState = value;
			}
		}

		public int EnemyNum
		{
			get
			{
				return enemyNum;
			}
		}

		public int Killed
		{
			get
			{
				return killed;
			}
			set
			{
				killed = value;
			}
		}

		public int CurWaveKilled
		{
			get
			{
				return curWaveKilled;
			}
		}

		public ObjectPool HitBloodObjectPool
		{
			get
			{
				return hitBloodObjectPool;
			}
		}

		public float GetDifficultyFactor
		{
			get
			{
				return difficultyFactor;
			}
		}

		public void Init(int index)
		{
			gameState = GameApp.GetInstance().GetGameState();
			sceneIndex = index;
			sceneName = Application.loadedLevelName.Substring(9);
			CreateSceneData();
			hitBloodObjectPool.Init("HitBlood", GameApp.GetInstance().GetGameConfig().hitBlood, 3, 0.4f);
			camera = GameObject.Find("Main Camera").GetComponent<TPSSimpleCameraScript>();
			if (camera == null)
			{
				camera = GameObject.Find("Main Camera").GetComponent<TopWatchingCameraScript>();
			}
			else if (!camera.enabled)
			{
				camera = GameObject.Find("Main Camera").GetComponent<TopWatchingCameraScript>();
			}
			player = new Player();
			player.Init();
			ArrayList battleFriends = gameState.GetBattleFriends();
			if (battleFriends.Count > 0)
			{
				FriendUserData friend_data = (FriendUserData)battleFriends[0];
				ImportFriendPlayer(friend_data);
			}
			else
			{
				FriendUserData defaultFriendPlayer = gameState.GetDefaultFriendPlayer();
				ImportFriendPlayer(defaultFriendPlayer);
			}
			camera.Init(player.GetTransform());
			player.PlayerObject.transform.Rotate(new Vector3(0f, 180f, 0f), Space.Self);
			if (FriendPlayer != null)
			{
				FriendPlayer.PlayerObject.transform.Rotate(new Vector3(0f, 180f, 0f), Space.Self);
			}
			enemyList = new Hashtable();
			playingState = PlayingState.GamePlaying;
			enemyNum = 0;
			killed = 0;
			triggerCount = 0;
			enemyID = 0;
			curWaveKilled = 0;
			m_bMapStartZoomShow = false;
			TimerManager.GetInstance().SetTimer(92, 10f, true);
			TimerManager.GetInstance().SetTimer(94, 15f, true);
			TimerManager.GetInstance().SetTimer(93, 25f, true);
			gameState.m_IsPassStage = false;
			gameState.SaveExchangeInfo();
			Debug.Log("ChangeSceneUI In GameScene --- m_ePlayMode" + gameState.m_eGameMode.m_ePlayMode);
			if (gameState.m_eGameMode.m_ePlayMode == GameState.NetworkGameMode.PlayMode.E_Console)
			{
				SceneUIManager.Instance().ChangeSceneUI(SceneUIManager.SceneUI.BattleUI);
				Debug.Log("!!!");
			}
			else
			{
				SceneUIManager.Instance().ChangeSceneUI(SceneUIManager.SceneUI.NBattleUI, false);
				Debug.Log("!!-------------!");
			}
		}

		public void LoadMap(int map_index)
		{
		}

		public Player GetPlayer()
		{
			return player;
		}

		public Player GetFriendPlayer()
		{
			return FriendPlayer;
		}

		public void ImportFriendPlayer(FriendUserData friend_data)
		{
			ArrayList friends = gameState.GetFriends();
			if (FriendPlayer == null)
			{
				if (gameState.m_eGameMode.m_ePlayMode == GameState.NetworkGameMode.PlayMode.E_Console)
				{
					FriendPlayer = new Player();
					FriendPlayer.Init(true);
					FriendPlayer.SetState(Player.IDLE_STATE);
				}
				else
				{
					FriendPlayer = null;
				}
			}
			if (FriendPlayer != null)
			{
				FriendPlayer.Exp = friend_data.m_Exp;
				FriendPlayer.Level = friend_data.m_Level;
				Weapon w = WeaponFactory.GetInstance().CreateWeapon(friend_data.m_BattleWeapons[0]);
				FriendPlayer.ChangeWeapon(w);
				Hashtable avatars = FriendPlayer.GetAvatars();
				avatars.Clear();
				Avatar key = new Avatar((Avatar.AvatarSuiteType)friend_data.m_AvatarHeadSuiteType, Avatar.AvatarType.Head);
				avatars[key] = true;
				Avatar key2 = new Avatar((Avatar.AvatarSuiteType)friend_data.m_AvatarBodySuiteType, Avatar.AvatarType.Body);
				avatars[key2] = true;
				FriendPlayer.LoadAvatars();
			}
		}

		public BaseCameraScript GetCamera()
		{
			return camera;
		}

		public Hashtable GetEnemies()
		{
			return enemyList;
		}

		public Enemy GetEnemyByID(string enemyID)
		{
			return (Enemy)enemyList[enemyID];
		}

		public void ClearEnemies()
		{
			foreach (Enemy value in enemyList.Values)
			{
				Object.Destroy(value.enemyObject);
			}
			enemyList.Clear();
			enemyNum = 0;
			killed = 0;
			triggerCount = 0;
			enemyID = 0;
			curWaveKilled = 0;
		}

		public void BattleBegin()
		{
			if (gameState.m_bIsSurvivalMode)
			{
				if (gameState.m_SurvivalModeBattledMapCount == 0)
				{
					gameState.InitExchangeInfo();
				}
			}
			else
			{
				gameState.InitExchangeInfo();
				gameState.SinewResumeSpeed = 0f;
			}
			SceneUIManager.Instance().ResetMusicPlayerState();
			DDSTrigger.PullTrigger();
			GameCollectionInfoManager.Instance().GetCurrentInfo().SetLastGamePointsInfo(DDSTrigger.MapIndex, DDSTrigger.PointsIndex, 0);
			if (DDSTrigger.MapIndex == 5)
			{
			}
			RefreshMachine();
			gameState.m_bBattleIsBegin = true;
			if (DDSTrigger.MapIndex != 5)
			{
				return;
			}
			if (DDSTrigger.PointsIndex == 1)
			{
				_TimeEndByTimeMode = 240f;
			}
			else
			{
				_TimeEndByTimeMode = 360f;
			}
			Debug.Log("_TimeEndByTimeMode: " + _TimeEndByTimeMode);
			GameObject[] array = GameObject.FindGameObjectsWithTag("Energy_Laser");
			float[] array2 = new float[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array2[i] = Vector3.Distance(player.GetTransform().position, array[i].transform.position);
			}
			float num = 0f;
			for (int j = 0; j < GameApp.GetInstance().GetGameConfig().machines.Length; j++)
			{
				num = Mathf.Min(array2);
				for (int k = 0; k < array2.Length; k++)
				{
					if (array2[k] == num)
					{
						BaseMachine baseMachine = array[k].GetComponent(typeof(BaseMachine)) as BaseMachine;
						baseMachine.m_bIsWork = true;
						array2[k] = 10000f;
					}
				}
			}
		}

		public void BattleEnd()
		{
			if (gameState.m_eGameMode.m_ePlayMode == GameState.NetworkGameMode.PlayMode.E_Console)
			{
				DDSTrigger.StopRefreshEnemies();
				ClearEnemies();
				int maxWavesOfPoints = ConfigManager.Instance().GetFixedConfig().GetMaxWavesOfPoints(DDSTrigger.MapIndex, DDSTrigger.PointsIndex);
				int battleWaves = gameState.m_BattleWaves;
				if (DDSTrigger.MapIndex == 3 || DDSTrigger.MapIndex == 4 || DDSTrigger.MapIndex == 5)
				{
					battleWaves = 1;
				}
				float num = 0f;
				if (gameState.GetBattleStar() >= 2)
				{
					switch (gameState.GetBattleStar())
					{
					case 2:
						num += 0.1f;
						break;
					case 3:
						num += 0.2f;
						break;
					default:
						num = 0f;
						break;
					}
				}
				gameState.m_BattleTime = Time.time - gameState.m_BattleStartTime;
				gameState.m_BattleExpExchangePercent = 1f + gameState.m_WaveExternExpPercent + player.CalcExpAdditive() + num;
				gameState.m_BattleGoldExchangePercent = 1f + player.CalcGoldAdditive() + num;
				gameState.m_WaveExternExpPercent = 0f;
				gameState.SaveExchangeInfo();
				if (gameState.LoginType == GameLoginType.LoginType_Facebook || gameState.LoginType == GameLoginType.LoginType_GameCenter)
				{
					int num2 = Mathf.FloorToInt(gameState.m_BattleExp * 0.05f);
					if (num2 > 0)
					{
						ArrayList battleFriends = gameState.GetBattleFriends();
						if (gameState.m_SelectFriendIndex > 0 && battleFriends.Count > 0)
						{
							FriendUserData friendUserData = battleFriends[0] as FriendUserData;
							GameClient.SetFriendUserExternExp(num2, friendUserData.m_UUID, friendUserData.m_DeviceId);
						}
					}
				}
				GameApp.GetInstance().SetLoadMap(false);
				Application.LoadLevel("Zombie3D_Judgement_MainUI");
				if (gameState.m_BattleGold <= 0f || gameState.m_BattleExp <= 0f)
				{
					gameState.m_bExchanged = false;
					gameState.m_bGameLoginExchange = false;
					SceneUIManager.Instance().ChangeSceneUI(SceneUIManager.SceneUI.ShopUI);
				}
				else
				{
					gameState.m_bExchanged = true;
					gameState.m_bGameLoginExchange = false;
					SceneUIManager.Instance().ChangeSceneUI(SceneUIManager.SceneUI.ExchangeUI);
				}
			}
			else if (gameState.m_eGameMode.m_ePlayMode == GameState.NetworkGameMode.PlayMode.E_LastStand)
			{
				GameApp.GetInstance().SetLoadMap(false);
				Application.LoadLevel("Zombie3D_Judgement_MainUI");
				gameState.m_bExchanged = false;
				gameState.m_bGameLoginExchange = false;
				SceneUIManager.Instance().ChangeSceneUI(SceneUIManager.SceneUI.NExchangUI);
			}
			else if (gameState.m_eGameMode.m_ePlayMode == GameState.NetworkGameMode.PlayMode.E_DeathMatch)
			{
				GameApp.GetInstance().SetLoadMap(false);
				Application.LoadLevel("Zombie3D_Judgement_MainUI");
				gameState.m_bExchanged = false;
				gameState.m_bGameLoginExchange = false;
				SceneUIManager.Instance().ChangeSceneUI(SceneUIManager.SceneUI.NExchangUI);
			}
			else if (gameState.m_eGameMode.m_ePlayMode == GameState.NetworkGameMode.PlayMode.E_PVE_BossRush)
			{
				GameApp.GetInstance().SetLoadMap(false);
				Application.LoadLevel("Zombie3D_Judgement_MainUI");
				gameState.m_bExchanged = false;
				gameState.m_bGameLoginExchange = false;
				SceneUIManager.Instance().ChangeSceneUI(SceneUIManager.SceneUI.NExchangUI);
			}
		}

		public int IncreaseKills(EnemyType enemy_type, int count = 1)
		{
			gameState.IncreaseKills(enemy_type, count);
			if (Time.time - m_LastSaveExchangeInfoTime > 5f)
			{
				gameState.SaveExchangeInfo();
			}
			killed++;
			curWaveKilled++;
			return killed;
		}

		public int GetNextTriggerID()
		{
			triggerCount++;
			return triggerCount;
		}

		public int GetNextEnemyID()
		{
			enemyID++;
			return enemyID;
		}

		public void ModifyEnemyNum(int num)
		{
			enemyNum += num;
			if (DDSTrigger.MapIndex == 1 || DDSTrigger.MapIndex == 2 || DDSTrigger.MapIndex == 6 || DDSTrigger.MapIndex == 7)
			{
				if (!DDSTrigger.WaveRefreshEnd || !(player.HP > 0f) || enemyNum != 0)
				{
					return;
				}
				BattleUIScript battleUIScript = SceneUIManager.Instance().GetSceneUIObject().GetComponent(typeof(BattleUIScript)) as BattleUIScript;
				bool flag = false;
				if (!waveWounded)
				{
					notWoundedLastWaves++;
					if (notWoundedLastWaves >= 20 && gameState.IsGCArchievementLocked(23))
					{
						gameState.UnlockGCArchievement(23, "com.trinitigame.callofminibulletdudes.a24");
					}
					flag = true;
					if (gameState.IsGCArchievementLocked(0))
					{
						gameState.UnlockGCArchievement(0, "com.trinitigame.callofminibulletdudes.a1");
					}
				}
				else
				{
					notWoundedLastWaves = 0;
				}
				if (FriendPlayer.HP > 0f)
				{
					friendNotDeadLastWaves++;
					if (friendNotDeadLastWaves >= 20 && gameState.IsGCArchievementLocked(24))
					{
						gameState.UnlockGCArchievement(24, "com.trinitigame.callofminibulletdudes.a25");
					}
				}
				else
				{
					friendNotDeadLastWaves = 0;
				}
				if (Time.time - m_LastWavePassedTime < 5f)
				{
					return;
				}
				m_LastWavePassedTime = Time.time;
				gameState.IncreaseBattleWave(flag);
				Debug.Log("EEEEEEEEEEE " + enemyNum + " |  | Points:" + DDSTrigger.PointsIndex + ",Waves:" + DDSTrigger.WaveIndex);
				int maxWavesOfPoints = ConfigManager.Instance().GetFixedConfig().GetMaxWavesOfPoints(DDSTrigger.MapIndex, DDSTrigger.PointsIndex);
				if (DDSTrigger.WaveIndex >= maxWavesOfPoints)
				{
					battleUIScript.StagePassed();
					gameState.m_IsPassStage = true;
					gameState.m_bGameLoginExchange = false;
					GameCollectionInfoManager.Instance().GetCurrentInfo().SetLastGamePointsInfo(DDSTrigger.MapIndex, DDSTrigger.PointsIndex, 1);
					if (!gameState.m_bIsSurvivalMode)
					{
						gameState.ResetBattleStar();
						gameState.AddBattleStar();
						if ((int)gameState.m_BattleTime <= 300 + DDSTrigger.PointsIndex * 5)
						{
							gameState.AddBattleStar();
							gameState.m_IsFastPassBattle = true;
						}
						if (!(player.HP / player.GetMaxHp() < 0.75f))
						{
							gameState.AddBattleStar();
							gameState.m_IsNoBruisePassBattle = true;
						}
						gameState.UpdateMaxMapCfg(DDSTrigger.MapIndex, DDSTrigger.PointsIndex, DDSTrigger.WaveIndex);
						gameState.ChangeBattleStar(DDSTrigger.MapIndex, DDSTrigger.PointsIndex, gameState.GetBattleStar());
						GameApp.GetInstance().Save();
						gameState.SaveExchangeInfo();
					}
				}
				else if (flag)
				{
					if (battleUIScript != null)
					{
						battleUIScript.SetupPerfectWavePassedEffect();
					}
				}
				else if (battleUIScript != null)
				{
					battleUIScript.SetupWavePassedEffect();
				}
			}
			else if (DDSTrigger.MapIndex == 3)
			{
				if (enemyNum != 0 || Time.time - gameState.m_BattleStartTime < 6f)
				{
					return;
				}
				long nowDateSeconds = UtilsEx.getNowDateSeconds();
				gameState.UpdateCDEndTime(3, nowDateSeconds);
				gameState.UpdateMaxMapCfg(DDSTrigger.MapIndex, DDSTrigger.PointsIndex, DDSTrigger.WaveIndex);
				List<KeyValuePair<int, int>> map3_Type_Reward = GetMap3_Type_Reward(DDSTrigger.PointsIndex);
				for (int i = 0; i < map3_Type_Reward.Count; i++)
				{
					if (map3_Type_Reward[i].Key < 0)
					{
						gameState.AddDollor(map3_Type_Reward[i].Value);
						gameState.AddEveryDayCrystalLootTotalCount(map3_Type_Reward[i].Value);
						continue;
					}
					for (int j = 0; j < map3_Type_Reward[i].Value; j++)
					{
						gameState.AddPowerUPS((ItemType)map3_Type_Reward[i].Key);
					}
				}
				BattleUIScript battleUIScript2 = SceneUIManager.Instance().GetSceneUIObject().GetComponent(typeof(BattleUIScript)) as BattleUIScript;
				battleUIScript2.Map3_Type_Win(map3_Type_Reward);
			}
			else if (DDSTrigger.MapIndex == 4)
			{
				if (enemyNum != 0 || Time.time - gameState.m_BattleStartTime < 6f)
				{
					return;
				}
				long nowDateSeconds2 = UtilsEx.getNowDateSeconds();
				gameState.UpdateCDEndTime(4, nowDateSeconds2);
				gameState.UpdateMaxMapCfg(DDSTrigger.MapIndex, DDSTrigger.PointsIndex, DDSTrigger.WaveIndex);
				List<KeyValuePair<int, int>> map3_Type_Reward2 = GetMap3_Type_Reward(DDSTrigger.PointsIndex);
				for (int k = 0; k < map3_Type_Reward2.Count; k++)
				{
					if (map3_Type_Reward2[k].Key < 0)
					{
						gameState.AddDollor(map3_Type_Reward2[k].Value);
						gameState.AddEveryDayCrystalLootTotalCount(map3_Type_Reward2[k].Value);
						continue;
					}
					for (int l = 0; l < map3_Type_Reward2[k].Value; l++)
					{
						gameState.AddPowerUPS((ItemType)map3_Type_Reward2[k].Key);
					}
				}
				BattleUIScript battleUIScript3 = SceneUIManager.Instance().GetSceneUIObject().GetComponent(typeof(BattleUIScript)) as BattleUIScript;
				battleUIScript3.Map3_Type_Win(map3_Type_Reward2);
			}
			else if (DDSTrigger.MapIndex == 5)
			{
				if (enemyNum != 0 || Time.time - gameState.m_BattleStartTime < 6f || energyFeedways == null || energyFeedways.Count > 0)
				{
					return;
				}
				long nowDateSeconds3 = UtilsEx.getNowDateSeconds();
				gameState.UpdateCDEndTime(5, nowDateSeconds3);
				gameState.UpdateMaxMapCfg(DDSTrigger.MapIndex, DDSTrigger.PointsIndex, DDSTrigger.WaveIndex);
				HideAllMachine();
				List<KeyValuePair<int, int>> map3_Type_Reward3 = GetMap3_Type_Reward(DDSTrigger.PointsIndex);
				for (int m = 0; m < map3_Type_Reward3.Count; m++)
				{
					if (map3_Type_Reward3[m].Key < 0)
					{
						gameState.AddDollor(map3_Type_Reward3[m].Value);
						gameState.AddEveryDayCrystalLootTotalCount(map3_Type_Reward3[m].Value);
						continue;
					}
					for (int n = 0; n < map3_Type_Reward3[m].Value; n++)
					{
						gameState.AddPowerUPS((ItemType)map3_Type_Reward3[m].Key);
					}
				}
				BattleUIScript battleUIScript4 = SceneUIManager.Instance().GetSceneUIObject().GetComponent(typeof(BattleUIScript)) as BattleUIScript;
				battleUIScript4.Map3_Type_Win(map3_Type_Reward3);
			}
			else if (DDSTrigger.MapIndex == 101)
			{
				if (!DDSTrigger.WaveRefreshEnd || !(player.HP > 0f))
				{
					return;
				}
				Debug.Log("EEEEEEEEEEE " + enemyNum);
				if (enemyNum != 0)
				{
					return;
				}
				bool flag2 = false;
				if (!waveWounded)
				{
					notWoundedLastWaves++;
					if (notWoundedLastWaves >= 20 && gameState.IsGCArchievementLocked(23))
					{
						gameState.UnlockGCArchievement(23, "com.trinitigame.callofminibulletdudes.a24");
					}
					flag2 = true;
					if (gameState.IsGCArchievementLocked(0))
					{
						gameState.UnlockGCArchievement(0, "com.trinitigame.callofminibulletdudes.a1");
					}
				}
				else
				{
					notWoundedLastWaves = 0;
				}
				gameState.IncreaseBattleWave(flag2);
				gameState.m_SurvivalModeBattledMapCount++;
				gameState.m_BattleWaves = (int)gameState.m_SurvivalModeBattledMapCount;
				int map_index = 101;
				int points_index = 1;
				int wave_index = 1;
				gameState.GetGameTriggerInfo(ref map_index, ref points_index, ref wave_index);
				wave_index++;
				if (wave_index > 50)
				{
					wave_index = 1;
					points_index++;
				}
				if (points_index > 10)
				{
					points_index = 1;
				}
				gameState.SetGameTriggerInfo(101, points_index, wave_index);
				GameObject survivalModeExitDoor_Parent = GameApp.GetInstance().GetGameConfig().SurvivalModeExitDoor_Parent;
				if (survivalModeExitDoor_Parent != null)
				{
					survivalModeExitDoor_Parent.SetActiveRecursively(true);
					SurvivalModeExitDoor[] componentsInChildren = survivalModeExitDoor_Parent.GetComponentsInChildren<SurvivalModeExitDoor>();
					for (int num2 = 0; num2 < componentsInChildren.Length; num2++)
					{
						if (Vector3.SqrMagnitude(player.GetTransform().position - componentsInChildren[num2].transform.position) < 3f)
						{
							componentsInChildren[num2].gameObject.SetActiveRecursively(false);
						}
					}
				}
				BattleUIScript battleUIScript5 = SceneUIManager.Instance().GetSceneUIObject().GetComponent(typeof(BattleUIScript)) as BattleUIScript;
				if (flag2)
				{
					battleUIScript5.SetupPerfectWavePassedEffect();
				}
				else
				{
					battleUIScript5.SetupWavePassedEffect();
				}
				battleUIScript5.ShowSurvivalModeIndicatorUI();
			}
			else
			{
				if (DDSTrigger.MapIndex != 102 || !DDSTrigger.WaveRefreshEnd || !(player.HP > 0f))
				{
					return;
				}
				Debug.Log("EEEEEEEEEEE " + enemyNum);
				if (enemyNum != 0)
				{
					return;
				}
				bool flag3 = false;
				if (!waveWounded)
				{
					notWoundedLastWaves++;
					if (notWoundedLastWaves >= 20 && gameState.IsGCArchievementLocked(23))
					{
						gameState.UnlockGCArchievement(23, "com.trinitigame.callofminibulletdudes.a24");
					}
					flag3 = true;
					if (gameState.IsGCArchievementLocked(0))
					{
						gameState.UnlockGCArchievement(0, "com.trinitigame.callofminibulletdudes.a1");
					}
				}
				else
				{
					notWoundedLastWaves = 0;
				}
				gameState.IncreaseBattleWave(flag3);
				gameState.m_SurvivalModeBattledMapCount++;
				gameState.m_BattleWaves = (int)gameState.m_SurvivalModeBattledMapCount;
				int map_index2 = 102;
				int points_index2 = 1;
				int wave_index2 = 1;
				gameState.GetGameTriggerInfo(ref map_index2, ref points_index2, ref wave_index2);
				wave_index2++;
				if (wave_index2 > 50)
				{
					wave_index2 = 1;
					points_index2++;
				}
				if (points_index2 > 10)
				{
					points_index2 = 1;
				}
				gameState.SetGameTriggerInfo(102, points_index2, wave_index2);
				GameObject survivalModeExitDoor_Parent2 = GameApp.GetInstance().GetGameConfig().SurvivalModeExitDoor_Parent;
				if (survivalModeExitDoor_Parent2 != null)
				{
					survivalModeExitDoor_Parent2.SetActiveRecursively(true);
					SurvivalModeExitDoor[] componentsInChildren2 = survivalModeExitDoor_Parent2.GetComponentsInChildren<SurvivalModeExitDoor>();
					for (int num3 = 0; num3 < componentsInChildren2.Length; num3++)
					{
						if (Vector3.SqrMagnitude(player.GetTransform().position - componentsInChildren2[num3].transform.position) < 3f)
						{
							componentsInChildren2[num3].gameObject.SetActiveRecursively(false);
						}
					}
				}
				BattleUIScript battleUIScript6 = SceneUIManager.Instance().GetSceneUIObject().GetComponent(typeof(BattleUIScript)) as BattleUIScript;
				if (flag3)
				{
					battleUIScript6.SetupPerfectWavePassedEffect();
				}
				else
				{
					battleUIScript6.SetupWavePassedEffect();
				}
				battleUIScript6.ShowSurvivalModeIndicatorUI();
			}
		}

		public void ChangeToNextSurvivalModeScene()
		{
			BattleUIScript battleUIScript = SceneUIManager.Instance().GetSceneUIObject().GetComponent(typeof(BattleUIScript)) as BattleUIScript;
			battleUIScript.SetupLoadingToNextSurvivalUI();
			int map_index = -1;
			int points_index = 0;
			int wave_index = 0;
			gameState.GetGameTriggerInfo(ref map_index, ref points_index, ref wave_index);
			if (DDSTrigger.MapIndex >= 101 && DDSTrigger.MapIndex <= 102)
			{
				Debug.Log("Map Index Is Right" + map_index);
			}
			else
			{
				map_index = 101;
				Debug.LogError("MapIndex Is Wrong Of Survival!!!");
			}
			List<string> survivalModeSceneNames = GetSurvivalModeSceneNames(map_index);
			List<string> list = new List<string>();
			for (int i = 0; i < survivalModeSceneNames.Count; i++)
			{
				if (survivalModeSceneNames[i] != Application.loadedLevelName)
				{
					list.Add(survivalModeSceneNames[i]);
				}
			}
			string name = list[Random.Range(0, list.Count)];
			GameApp.GetInstance().SetLoadMap(true);
			Application.LoadLevel(name);
		}

		private List<KeyValuePair<int, int>> GetMap3_Type_Reward(int mode)
		{
			List<KeyValuePair<int, int>> list = new List<KeyValuePair<int, int>>();
			List<KeyValuePair<int, int>> list2 = new List<KeyValuePair<int, int>>();
			switch (mode)
			{
			case 1:
				list2.Add(new KeyValuePair<int, int>(1, 5));
				list2.Add(new KeyValuePair<int, int>(2, 10));
				list2.Add(new KeyValuePair<int, int>(3, 5));
				list2.Add(new KeyValuePair<int, int>(4, 10));
				list2.Add(new KeyValuePair<int, int>(6, 5));
				list2.Add(new KeyValuePair<int, int>(7, 10));
				list2.Add(new KeyValuePair<int, int>(8, 10));
				list2.Add(new KeyValuePair<int, int>(9, 5));
				list2.Add(new KeyValuePair<int, int>(10, 10));
				list2.Add(new KeyValuePair<int, int>(11, 10));
				list2.Add(new KeyValuePair<int, int>(12, 5));
				list2.Add(new KeyValuePair<int, int>(-1, 15));
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
			int index = RandomGift(list2);
			list.Add(new KeyValuePair<int, int>(list2[index].Key, 1));
			if (Random.Range(0, 100) < 80)
			{
				list2.RemoveAt(index);
				index = RandomGift(list2);
				list.Add(new KeyValuePair<int, int>(list2[index].Key, 1));
				if (mode == 2 && Random.Range(0, 100) < 40)
				{
					list2.RemoveAt(index);
					index = RandomGift(list2);
					list.Add(new KeyValuePair<int, int>(list2[index].Key, 1));
				}
			}
			return list;
		}

		private int RandomGift(List<KeyValuePair<int, int>> _lGiftItem)
		{
			int num = Random.Range(0, 100);
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
				num2 = ((everyDayCrystalLootTotalCount < 15) ? num2 : Random.Range(0, num2));
			}
			return num2;
		}

		private int RandomCount(int[] counters)
		{
			int result = 1;
			if (DDSTrigger.PointsIndex == 2)
			{
				int max = counters[0] + counters[1];
				int num = Random.Range(0, max);
				result = ((num < counters[0]) ? 1 : 2);
			}
			else if (DDSTrigger.PointsIndex == 3)
			{
				int max2 = counters[0] + counters[1] + counters[2];
				int num2 = Random.Range(0, max2);
				result = ((num2 < counters[0]) ? 1 : ((num2 >= counters[0] + counters[1]) ? 3 : 2));
			}
			else if (DDSTrigger.PointsIndex == 4)
			{
				int max3 = counters[0] + counters[1] + counters[2] + counters[3];
				int num3 = Random.Range(0, max3);
				result = ((num3 < counters[0]) ? 1 : ((num3 < counters[0] + counters[1]) ? 2 : ((num3 >= counters[0] + counters[1] + counters[2]) ? 4 : 3)));
			}
			else if (DDSTrigger.PointsIndex == 101)
			{
				int max4 = counters[0] + counters[1] + counters[2] + counters[3] + counters[4];
				int num4 = Random.Range(0, max4);
				result = ((num4 < counters[0]) ? 1 : ((num4 < counters[0] + counters[1]) ? 2 : ((num4 < counters[0] + counters[1] + counters[2]) ? 3 : ((num4 >= counters[0] + counters[1] + counters[2] + counters[3]) ? 5 : 4))));
			}
			else if (DDSTrigger.PointsIndex == 102)
			{
				int max5 = counters[0] + counters[1] + counters[2] + counters[3] + counters[4];
				int num5 = Random.Range(0, max5);
				result = ((num5 < counters[0]) ? 1 : ((num5 < counters[0] + counters[1]) ? 2 : ((num5 < counters[0] + counters[1] + counters[2]) ? 3 : ((num5 >= counters[0] + counters[1] + counters[2] + counters[3]) ? 5 : 4))));
			}
			return result;
		}

		public int GetSceneIndex()
		{
			return sceneIndex;
		}

		public string GetSceneName()
		{
			return sceneName;
		}

		public void AddTrigger(TriggerScript trigger)
		{
			triggerList.Add(trigger);
		}

		public bool TriggersAllMaxSpawned()
		{
			bool result = true;
			if (triggerList.Count == 0)
			{
				result = false;
			}
			foreach (TriggerScript trigger in triggerList)
			{
				if (!trigger.AlreadyMaxSpawned)
				{
					result = false;
				}
			}
			return result;
		}

		public void DoLogic(float deltaTime)
		{
			player.DoLogic(deltaTime);
			if (enemyList == null)
			{
				enemyList = new Hashtable();
			}
			object[] array = new object[enemyList.Count];
			int count = enemyList.Count;
			enemyList.Keys.CopyTo(array, 0);
			if (gameState.m_eGameMode.m_ePlayMode == GameState.NetworkGameMode.PlayMode.E_Console)
			{
				for (int i = 0; i < array.Length; i++)
				{
					Enemy enemy = enemyList[array[i]] as Enemy;
					if (enemy != null)
					{
						enemy.DoLogic(deltaTime);
						continue;
					}
					Debug.Log(enemyList.Keys.Count + " | " + array.Length + " | " + enemyList.Count + "|" + count);
				}
			}
			if (FriendPlayer != null)
			{
				FriendPlayer.DoLogic(deltaTime);
				m_FriendChangeWeaponTimer += deltaTime;
				if (FriendPlayer.HP > 0f && m_FriendChangeWeaponTimer > m_FriendChangeWeaponTime)
				{
					m_FriendChangeWeaponTimer = 0f;
					FriendUserData friendUserData = null;
					if (GameApp.GetInstance().GetGameState().m_SelectFriendIndex >= 0)
					{
						ArrayList friends = GameApp.GetInstance().GetGameState().GetFriends();
						friendUserData = (FriendUserData)friends[GameApp.GetInstance().GetGameState().m_SelectFriendIndex];
					}
					else if (GameApp.GetInstance().GetGameState().m_SelectHiredFriendIndex >= 0)
					{
						List<KeyValuePair<FriendUserData, long>> hiredFriends = GameApp.GetInstance().GetGameState().GetHiredFriends();
						friendUserData = hiredFriends[GameApp.GetInstance().GetGameState().m_SelectHiredFriendIndex].Key;
					}
					if (friendUserData != null && friendUserData.m_BattleWeapons.Count > 1)
					{
						WeaponType weaponType = FriendPlayer.GetWeapon().GetWeaponType();
						WeaponType weaponType2 = weaponType;
						for (int j = 0; j < friendUserData.m_BattleWeapons.Count; j++)
						{
							if (weaponType != friendUserData.m_BattleWeapons[j])
							{
								weaponType2 = friendUserData.m_BattleWeapons[j];
							}
						}
						if (weaponType2 != weaponType)
						{
							Weapon w = WeaponFactory.GetInstance().CreateWeapon(weaponType2);
							FriendPlayer.ChangeWeapon(w);
						}
					}
				}
			}
			if (FriendPlayer != null && FriendPlayer.HP > 0f)
			{
				if (player.HP <= 0f)
				{
					if (FriendPlayer.GetState() != Player.FORCEIDLE_STATE)
					{
						FriendPlayer.SetState(Player.FORCEIDLE_STATE);
					}
				}
				else
				{
					float num = 100f;
					Enemy enemy2 = null;
					if (FriendPlayer.friendTargetEnemy == null)
					{
						for (int k = 0; k < array.Length; k++)
						{
							Enemy enemy3 = enemyList[array[k]] as Enemy;
							if (enemy3 != null && enemy3.HP > 0f)
							{
								float num2 = Vector3.Distance(FriendPlayer.PlayerObject.transform.position, enemy3.GetPosition());
								if (num > num2)
								{
									num = num2;
									enemy2 = enemy3;
								}
							}
						}
					}
					else if (FriendPlayer.friendTargetEnemy.HP <= 0f)
					{
						FriendPlayer.friendTargetEnemy = null;
						for (int l = 0; l < array.Length; l++)
						{
							Enemy enemy4 = enemyList[array[l]] as Enemy;
							if (enemy4 != null && enemy4.HP > 0f)
							{
								float num3 = Vector3.Distance(FriendPlayer.PlayerObject.transform.position, enemy4.GetPosition());
								if (num > num3)
								{
									num = num3;
									enemy2 = enemy4;
								}
							}
						}
					}
					else if (FriendPlayer.friendTargetEnemy != null && FriendPlayer.friendTargetEnemy.enemyObject != null)
					{
						float num4 = Vector3.Distance(FriendPlayer.friendTargetEnemy.enemyObject.transform.position, FriendPlayer.GetTransform().position);
						if (num4 > gameParameters.FriendPlayerAttackRadius)
						{
							for (int m = 0; m < array.Length; m++)
							{
								Enemy enemy5 = enemyList[array[m]] as Enemy;
								if (enemy5 != null && enemy5.HP > 0f)
								{
									float num5 = Vector3.Distance(FriendPlayer.PlayerObject.transform.position, enemy5.GetPosition());
									if (num > num5)
									{
										num = num5;
										enemy2 = enemy5;
									}
								}
							}
						}
					}
					float num6 = Vector3.Distance(player.GetTransform().position, FriendPlayer.GetTransform().position);
					if (enemy2 != null)
					{
						FriendPlayer.friendTargetEnemy = null;
						if (num < gameParameters.FriendPlayerAttackRadius)
						{
							FriendPlayer.friendTargetEnemy = enemy2;
						}
					}
				}
			}
			quest.DoLogic();
			if (quest.QuestCompleted)
			{
				playingState = PlayingState.GameWin;
				player.GetWeapon().StopFire();
			}
			hitBloodObjectPool.DoLogic();
		}

		public void CreateSceneData()
		{
			Debug.Log("CreateSceneData");
			triggerList = new List<TriggerScript>();
			gameParameters = GameObject.FindGameObjectWithTag("Parameters").GetComponent<GameParametersScript>();
			Faceout3DTextPool.Instance().Init(20);
			GameObject[] array = GameObject.FindGameObjectsWithTag("Path");
			path = new Vector3[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				path[i] = array[i].transform.position;
			}
			woodboxList = GameObject.FindGameObjectsWithTag("WoodBox");
			RefreshJerricans();
			RefreshEnergyFeedways();
			GameObject[] array2 = GameObject.FindGameObjectsWithTag("PathDoor");
			pathDoors = new PathDoor[array2.Length];
			for (int j = 0; j < pathDoors.Length; j++)
			{
				pathDoors[j] = array2[j].GetComponent(typeof(PathDoor)) as PathDoor;
			}
			switch (gameParameters.questType)
			{
			case QuestType.Bomb:
				quest = new BombQuest();
				CreateBombSpots();
				quest.Init();
				break;
			case QuestType.KillAll:
				quest = new KillAllQuest();
				quest.Init();
				break;
			case QuestType.Survival:
				quest = new SurvivalQuest();
				quest.Init();
				break;
			}
		}

		public void UpdateDifficultyLevel(float difficultyFactor)
		{
		}

		public void CreateBombSpots()
		{
			bombSpotList = new List<BombSpot>();
			GameObject[] array = GameObject.FindGameObjectsWithTag("BombSpot");
			List<int> list = new List<int>();
			for (int i = 0; i < array.Length; i++)
			{
				list.Add(i);
			}
			if (list.Count <= 0)
			{
				return;
			}
			int index = Random.Range(0, list.Count);
			int num = list[index];
			list.Remove(num);
			index = Random.Range(0, list.Count - 1);
			int num2 = list[index];
			list.Remove(num2);
			index = Random.Range(0, list.Count - 2);
			int num3 = list[index];
			for (int j = 0; j < array.Length; j++)
			{
				if (j != num && j != num2 && j != num3)
				{
					Object.Destroy(array[j]);
					continue;
				}
				BombSpot bombSpot = new BombSpot();
				bombSpot.bombSpotObj = array[j];
				array[j].active = true;
				bombSpot.Init();
				bombSpotList.Add(bombSpot);
			}
		}

		public List<BombSpot> GetBombSpots()
		{
			return bombSpotList;
		}

		public Vector3[] GetPath()
		{
			return path;
		}

		public Quest GetQuest()
		{
			return quest;
		}

		public GameParametersScript GetGameParameters()
		{
			return gameParameters;
		}

		public GameObject[] GetWoodBoxes()
		{
			return woodboxList;
		}

		public void RefreshWoodBoxes()
		{
			Object.Instantiate(GameApp.GetInstance().GetGameConfig().woodBoxes);
			woodboxList = GameObject.FindGameObjectsWithTag("WoodBox");
			spawnWoodBoxesTime = Time.time;
		}

		public List<JerricanScript> GetJerricans()
		{
			return jerricans;
		}

		public void RefreshJerricans()
		{
			if (jerricans == null)
			{
				GameObject[] array = GameObject.FindGameObjectsWithTag("Jerrican");
				jerricans = new List<JerricanScript>();
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i] != null)
					{
						jerricans.Add(array[i].GetComponent(typeof(JerricanScript)) as JerricanScript);
					}
				}
				return;
			}
			for (int j = 0; j < jerricans.Count; j++)
			{
				if (jerricans[j] == null || jerricans[j].gameObject == null)
				{
					Debug.Log("77777777777777777 - " + j);
					jerricans.RemoveAt(j);
				}
			}
		}

		public List<EnergyFeedwayScript> GetEnergyFeedways()
		{
			return energyFeedways;
		}

		public void RefreshEnergyFeedways()
		{
			if (energyFeedways == null)
			{
				GameObject[] array = GameObject.FindGameObjectsWithTag("EnergyFeedway");
				energyFeedways = new List<EnergyFeedwayScript>();
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i] != null)
					{
						energyFeedways.Add(array[i].GetComponent(typeof(EnergyFeedwayScript)) as EnergyFeedwayScript);
						energyFeedways[i].Init();
					}
				}
				return;
			}
			for (int j = 0; j < energyFeedways.Count; j++)
			{
				if (energyFeedways[j] == null || energyFeedways[j].gameObject == null)
				{
					Debug.Log("77777777777777777 - " + j);
					energyFeedways.RemoveAt(j);
				}
			}
		}

		public void unlockNextBallPlaces(EnergyFeedwayScript ball)
		{
			for (int i = 0; i < ball.Energy_Laser.Count; i++)
			{
				BaseMachine baseMachine = ball.Energy_Laser[i].GetComponent(typeof(BaseMachine)) as BaseMachine;
				if (!baseMachine.m_bIsWork)
				{
					baseMachine.m_bIsWork = true;
				}
			}
			energyFeedways.Remove(ball);
			if (energyFeedways.Count <= 0)
			{
				DDSTrigger.StopRefreshEnemies();
				return;
			}
			for (int j = 0; j < energyFeedways.Count; j++)
			{
				if (energyFeedways[j] != null)
				{
					energyFeedways[j].AddBlood();
				}
			}
		}

		public List<GameObject> GetMachine()
		{
			return machines;
		}

		public void RefreshMachine()
		{
			if (machines == null)
			{
				GameObject[] array = GameObject.FindGameObjectsWithTag("laserPosition");
				GameObject[] array2 = RandomMachine(GameApp.GetInstance().GetGameConfig().machines);
				machines = new List<GameObject>();
				GameObject[] array3 = new GameObject[4];
				foreach (EnergyFeedwayScript energyFeedway in GetEnergyFeedways())
				{
					if (energyFeedway.gameObject.name == "Ball1")
					{
						array3[0] = energyFeedway.gameObject;
					}
					else if (energyFeedway.gameObject.name == "Ball2")
					{
						array3[1] = energyFeedway.gameObject;
					}
					else if (energyFeedway.gameObject.name == "Ball3")
					{
						array3[2] = energyFeedway.gameObject;
					}
					else if (energyFeedway.gameObject.name == "Ball4")
					{
						array3[3] = energyFeedway.gameObject;
					}
				}
				int num = 0;
				for (int i = 0; i < array.Length / array2.Length; i++)
				{
					for (int j = 0; j < array2.Length; j++)
					{
						GameObject gameObject = Object.Instantiate(array2[j], array[num].transform.position, array[num].transform.rotation) as GameObject;
						machines.Add(gameObject);
						string[] array4 = array[num].name.Split('_');
						if (array4[1] == "1")
						{
							EnergyFeedwayScript energyFeedwayScript = array3[0].GetComponent(typeof(EnergyFeedwayScript)) as EnergyFeedwayScript;
							energyFeedwayScript.Energy_Laser.Add(gameObject);
							energyFeedwayScript = array3[1].GetComponent(typeof(EnergyFeedwayScript)) as EnergyFeedwayScript;
							energyFeedwayScript.Energy_Laser.Add(gameObject);
						}
						else if (array4[1] == "2")
						{
							EnergyFeedwayScript energyFeedwayScript2 = array3[0].GetComponent(typeof(EnergyFeedwayScript)) as EnergyFeedwayScript;
							energyFeedwayScript2.Energy_Laser.Add(gameObject);
							energyFeedwayScript2 = array3[3].GetComponent(typeof(EnergyFeedwayScript)) as EnergyFeedwayScript;
							energyFeedwayScript2.Energy_Laser.Add(gameObject);
						}
						else if (array4[1] == "3")
						{
							EnergyFeedwayScript energyFeedwayScript3 = array3[2].GetComponent(typeof(EnergyFeedwayScript)) as EnergyFeedwayScript;
							energyFeedwayScript3.Energy_Laser.Add(gameObject);
							energyFeedwayScript3 = array3[3].GetComponent(typeof(EnergyFeedwayScript)) as EnergyFeedwayScript;
							energyFeedwayScript3.Energy_Laser.Add(gameObject);
						}
						else if (array4[1] == "4")
						{
							EnergyFeedwayScript energyFeedwayScript4 = array3[1].GetComponent(typeof(EnergyFeedwayScript)) as EnergyFeedwayScript;
							energyFeedwayScript4.Energy_Laser.Add(gameObject);
							energyFeedwayScript4 = array3[2].GetComponent(typeof(EnergyFeedwayScript)) as EnergyFeedwayScript;
							energyFeedwayScript4.Energy_Laser.Add(gameObject);
						}
						gameObject.tag = "Energy_Laser";
						gameObject.transform.Find("Root_Bone").GetComponent<Renderer>().enabled = true;
						BaseMachine baseMachine = gameObject.GetComponent(typeof(BaseMachine)) as BaseMachine;
						baseMachine.m_bIsWork = false;
						string[] array5 = array[num].transform.name.Split('_');
						if (int.Parse(array5[2]) == 3)
						{
							baseMachine.m_RoateAngle = 134f;
						}
						else
						{
							baseMachine.m_RoateAngle = 44f;
						}
						baseMachine.Init();
						num++;
					}
				}
				return;
			}
			for (int k = 0; k < machines.Count; k++)
			{
				if (machines[k] == null || machines[k].gameObject == null)
				{
					Debug.Log("77777777777777777 - " + k);
					machines.RemoveAt(k);
				}
			}
		}

		public GameObject[] RandomMachine(GameObject[] mc)
		{
			int num = mc.Length;
			GameObject[] array = new GameObject[num];
			ArrayList arrayList = new ArrayList();
			for (int i = 0; i < num; i++)
			{
				arrayList.Add(i);
			}
			for (int j = 0; j < array.Length; j++)
			{
				if (arrayList.Count >= 1)
				{
					int index = Random.Range(0, arrayList.Count - 1);
					int num2 = (int)arrayList[index];
					array[j] = mc[num2];
					arrayList.RemoveAt(index);
				}
				else
				{
					Debug.Log("Error");
				}
			}
			for (int k = 0; k < array.Length; k++)
			{
				if (array == null)
				{
					Debug.LogError("Error List !!!");
					return mc;
				}
			}
			return array;
		}

		public void StartWave()
		{
			waveWounded = false;
			GenerateWorms();
			if ((DDSTrigger.WaveIndex == 1 || DDSTrigger.WaveIndex == 2) && GetFriendPlayer() != null && GetFriendPlayer().HP <= 0f)
			{
				GetFriendPlayer().ResurrectionAtCurrentPos();
			}
		}

		public static List<string> GetSurvivalModeSceneNames(int SurvivalMapIndex)
		{
			List<string> list = new List<string>();
			switch (SurvivalMapIndex)
			{
			case 101:
				list.Add("Zombie3D_Judgement_Map05");
				list.Add("Zombie3D_Judgement_Map06");
				list.Add("Zombie3D_Judgement_Map07");
				list.Add("Zombie3D_Judgement_Map08");
				list.Add("Zombie3D_Judgement_Map09");
				break;
			case 102:
				list.Add("Zombie3D_Judgement_Map12");
				list.Add("Zombie3D_Judgement_Map13");
				list.Add("Zombie3D_Judgement_Map14");
				break;
			}
			return list;
		}

		public PathDoor[] GetPathDoors()
		{
			return pathDoors;
		}

		public void GenerateWorms()
		{
			if (pathDoors.Length <= 0)
			{
				return;
			}
			int num = 0;
			for (int i = 0; i < pathDoors.Length; i++)
			{
				if (pathDoors[i].GetWorm() != null)
				{
					num++;
				}
			}
			int num2 = 4 - num;
			for (int j = 0; j < pathDoors.Length; j++)
			{
				if (num2 <= 0)
				{
					break;
				}
				if (pathDoors[j].IsDoorClosed() && Random.Range(0, 4) == 0)
				{
					pathDoors[j].GenerateWorm();
					num2--;
				}
			}
		}

		public void HideAllMachine()
		{
			foreach (GameObject machine in machines)
			{
				machine.SetActiveRecursively(false);
			}
		}

		public void BattleEndByOverTime()
		{
			if (_TimeBeginByTimeMode >= _TimeEndByTimeMode)
			{
				_TimeBeginByTimeMode = -1f;
				BattleUIScript battleUIScript = SceneUIManager.Instance().GetSceneUIObject().GetComponent(typeof(BattleUIScript)) as BattleUIScript;
				battleUIScript.IsCoundDownOver(0f);
				battleUIScript.StageLosed();
				HideAllMachine();
			}
		}

		public void BeginCalculagraph()
		{
			if (_TimeBeginByTimeMode < 0f)
			{
				_TimeBeginByTimeMode = 0f;
			}
		}

		public void UpdateCalculagraph(float detletTime)
		{
			if (_TimeBeginByTimeMode >= 0f && player.HP > 0f)
			{
				_TimeBeginByTimeMode += detletTime;
			}
		}

		public float GetRemainingTimeByTimeMode()
		{
			return _TimeEndByTimeMode - _TimeBeginByTimeMode;
		}
	}
}
