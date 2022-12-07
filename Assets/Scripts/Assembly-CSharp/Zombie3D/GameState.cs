using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace Zombie3D
{
	public class GameState
	{
		public class NetworkGameMode
		{
			public enum PlayMode
			{
				E_Console = 0,
				E_DeathMatch = 1,
				E_LastStand = 2,
				E_PVE_BossRush = 3
			}

			public enum NetworkCooperationMode
			{
				E_Team = 0,
				E_Simple = 1
			}

			public class NetworkPlayerStatistics
			{
				public string m_strName = string.Empty;

				public int m_iNGroup;

				public int m_iHeadAvatarID = 1;

				public int m_iDeathCount;

				public int m_iKillCount;

				public int m_iMyDamage;

				public bool m_bIsOline;

				public bool m_bIsWinner;

				public bool m_bIsBestKiller;

				public List<KeyValuePair<int, int>> m_lsReward = new List<KeyValuePair<int, int>>();

				public void AddlsReward(KeyValuePair<int, int> kp)
				{
					KeyValuePair<int, int> item = LsRewardContainsKey(kp.Key);
					if (item.Key != -9999)
					{
						m_lsReward.Remove(item);
						if (kp.Key >= 1000)
						{
							m_lsReward.Add(new KeyValuePair<int, int>(kp.Key, kp.Value));
						}
						else
						{
							m_lsReward.Add(new KeyValuePair<int, int>(kp.Key, kp.Value + item.Value));
						}
					}
					else
					{
						m_lsReward.Add(kp);
					}
				}

				public KeyValuePair<int, int> LsRewardContainsKey(int key)
				{
					foreach (KeyValuePair<int, int> item in m_lsReward)
					{
						if (key == item.Key)
						{
							return item;
						}
					}
					return new KeyValuePair<int, int>(-9999, -9999);
				}
			}

			public enum NBattleStatistics
			{
				E_NBATTLETIMES = 0,
				E_NBATTLEKILLS = 1,
				E_NBATTLEDEATHS = 2
			}

			public class CombatDataRecord
			{
				public int iPVPWinTimes;

				public int iPVPKillPlayerCount;

				public int iPVPDoubleKillCount;

				public int iPVPThreeKillCount;

				public int iPVPFourKillCount;

				public int iPVPFiveKillCount;

				public int iPVPBestKillerCount;

				public int iWearShinobiToPlayTimes;

				public int iFirstBloodCount;

				public int iPVPNoUseFastRunCount;

				public int iPVPKillMoreTenPlayersOnceWar;

				public bool bIsNoDeathToWinOneGame;

				public List<enBattlefieldProps> lsBuyNBattleShop = new List<enBattlefieldProps>();

				public List<Avatar.AvatarSuiteType> lsWearAvatarTimes = new List<Avatar.AvatarSuiteType>();

				public int bShowFirstBuffPresentation;

				public int iGetBuffCount;
			}

			public PlayMode m_ePlayMode;

			public NetworkCooperationMode m_eCooperaMode;

			public string m_strNetName = string.Empty;

			public Dictionary<int, NetworkPlayerStatistics> m_PlaersStatistics = new Dictionary<int, NetworkPlayerStatistics>();

			public Dictionary<enBattlefieldProps, int> m_PlaersNBattleBuff = new Dictionary<enBattlefieldProps, int>();

			private int m_uiFloor;

			public Dictionary<NBattleStatistics, int> m_NBattleStatistics = new Dictionary<NBattleStatistics, int>();

			public CombatDataRecord m_CombatDataRecord = new CombatDataRecord();

			public int PVE_FLOOR
			{
				get
				{
					return m_uiFloor;
				}
				set
				{
					m_uiFloor = value;
				}
			}
		}

		private const uint m_MapCount = 7u;

		public bool m_bCheckDataTimeOK;

		private long lastSaveTime;

		private GameLoginType m_LoginType;

		protected int theDays;

		public int everyDayCrystalLootTotalCount;

		private int m_IsLevelUp;

		private bool m_FinishedLevelupAnimation = true;

		private bool m_LevelUp;

		public int m_GetFirstGift;

		public int m_ReviewCount;

		public int m_BattleCount;

		private int m_GCTotalBattleTime;

		public List<int> m_GameCenterUnlockListInfo;

		private int m_GCKilledAllCount;

		private int m_GCBoomerAttackTimes;

		private int m_GCDeadTimes;

		private int m_GCIapTimes;

		private float m_GCIapTotalDollor;

		private int m_GCPlayWithNetFriendTimes;

		private int m_GCFriendPlayerDeadTimes;

		protected int m_GameMapIndex = 1;

		protected int m_GamePointsIndex = 1;

		protected int m_GameWaveIndex = 1;

		public int m_SelectFriendIndex;

		public int m_SelectHiredFriendIndex = -1;

		public long lastDailyBonusGetTime;

		public int lastDailyBonusLevel = 1;

		public float m_WaveExternExpPercent;

		public int gold;

		public int dollor;

		public int exp;

		private int m_Level = 1;

		public long exchangeUpgradeEndTime;

		public List<long> m_MapsCDEndTime = new List<long>(4);

		private float m_LastMapsCDEndSaveTime;

		public bool m_bIsSurvivalMode;

		public uint m_SurvivalModeBattledMapCount;

		public float playerHpInSurvivalMode = 100f;

		public float playerStaInSurvivalMode = 100f;

		public float friendPlayerHpInSurvivalMode = 100f;

		protected int score;

		protected Hashtable weaponInfo;

		protected List<WeaponType> battleWeapons;

		private string weaponAccouter = string.Empty;

		protected List<Skill> m_Skilles;

		public enSkillType m_CurSkillType = enSkillType.FastRun;

		protected bool inited;

		protected bool weaponsInited;

		private bool m_MusicOn;

		protected Hashtable m_PowerUPS = new Hashtable();

		private string powerUpsAccount = string.Empty;

		protected Hashtable Avatars = new Hashtable();

		private string avatarAccouter = string.Empty;

		protected ArrayList m_Friends = new ArrayList();

		protected ArrayList m_BattleFriends = new ArrayList();

		protected List<FriendUserData> m_HireFriends;

		protected List<KeyValuePair<FriendUserData, long>> m_HiredFriendsInfo;

		public long m_LastHireOutTime;

		public bool m_bGameLoginExchange;

		public bool m_bReLogin;

		private float m_SinewResumeSpeed;

		public GameObject m_goIAPEventListener;

		public GameObject m_goIAPManager;

		public bool m_bIAPIsInitOK;

		public int m_iPlayGameTimes;

		public int m_iCameraModeType;

		public bool m_bBattleIsBegin;

		public bool m_bExchanged;

		public bool m_bExchangeGold;

		public bool m_bExchangeExp;

		public int m_BattleMapId;

		public float m_BattleStartTime;

		public float m_BattleTime;

		public int m_BattleWaves;

		public int m_BattlePerfectWaves;

		public int m_KilledCount;

		public Hashtable m_KilledEnemyInfo = new Hashtable();

		public float m_BattleGold;

		public float m_BattleGoldExchangePercent = 1f;

		public float m_BattleExp;

		public float m_BattleExpExchangePercent = 1f;

		private int m_BattleStar;

		public ArrayList m_htBattleStar;

		public bool m_IsFastPassBattle;

		public bool m_IsNoBruisePassBattle;

		public bool m_IsPassStage;

		public int m_iPlayerLastLevel;

		public int m_SendGiftGoldDollor_1_3_2;

		public List<string> m_lsGameCenterMsg = new List<string>();

		public bool m_bIsRightVersion;

		public string m_strZoneName = "CoMDS";

		public string serverName = "118.139.176.202";

		public int serverPort = 9933;

		public NetworkGameMode m_eGameMode = new NetworkGameMode();

		private Dictionary<int, PropsAdditionImpl> m_dictPropsAdditions = new Dictionary<int, PropsAdditionImpl>();

		private string m_FirstLoginTime = string.Empty;

		private int m_TotalExp;

		private int m_TotalGameTime;

		private int m_TotalGoldGet;

		private int m_TotalDollorGet;

		private bool m_IsShareWithTwitter = true;

		private int m_bHaveNovicegiftbag;

		public int m_iNovicegiftAllowBuyTimes = 1;

		private int m_bHaveNovicegiftbagSec;

		public int m_iNovicegiftAllowBuyTimesSec = 1;

		private int m_bHaveNovicegiftbagThird;

		public int m_iNovicegiftAllowBuyTimesThird = 1;

		public string DeviceID { get; set; }

		public string UUID { get; set; }

		public string UserID { get; set; }

		public string FacebookID { get; set; }

		public string FacebookName { get; set; }

		public List<string> FacebookFriendList { get; set; }

		public string GameCenterID { get; set; }

		public string GameCenterName { get; set; }

		public List<string> GameCenterFriendList { get; set; }

		public int ExternExp { get; set; }

		public int BeHiredMoney { get; set; }

		public bool LEVEL_UP
		{
			get
			{
				return m_LevelUp;
			}
			set
			{
				m_LevelUp = value;
			}
		}

		public int Level
		{
			get
			{
				return m_Level;
			}
			set
			{
				m_Level = Mathf.Clamp(value, 1, 255);
			}
		}

		public int WaveNum { get; set; }

		public bool FirstTimeGame { get; set; }

		public bool MusicOn
		{
			get
			{
				return m_MusicOn;
			}
			set
			{
				m_MusicOn = value;
				MusicManager.Instance().ChangeMusicOption();
			}
		}

		public bool SoundOn { get; set; }

		public float SinewResumeSpeed
		{
			get
			{
				return m_SinewResumeSpeed;
			}
			set
			{
				m_SinewResumeSpeed = value;
			}
		}

		public string NetPlayerName
		{
			get
			{
				return m_eGameMode.m_strNetName;
			}
			set
			{
				m_eGameMode.m_strNetName = value;
			}
		}

		public string FirstLoginTime
		{
			get
			{
				return m_FirstLoginTime;
			}
		}

		public int TotalExp
		{
			get
			{
				return m_TotalExp;
			}
		}

		public int TotalGameTime
		{
			get
			{
				return m_TotalGameTime;
			}
		}

		public int TotalGoldGet
		{
			get
			{
				return m_TotalGoldGet;
			}
		}

		public int TotalDollorGet
		{
			get
			{
				return m_TotalDollorGet;
			}
		}

		public bool ShareWithTwitter
		{
			get
			{
				return m_IsShareWithTwitter;
			}
			set
			{
				m_IsShareWithTwitter = value;
			}
		}

		public int NoviceGiftBag
		{
			get
			{
				return m_bHaveNovicegiftbag;
			}
			set
			{
				m_bHaveNovicegiftbag = value;
			}
		}

		public int NoviceGiftBagSec
		{
			get
			{
				return m_bHaveNovicegiftbagSec;
			}
			set
			{
				m_bHaveNovicegiftbagSec = value;
			}
		}

		public int NoviceGiftBagThird
		{
			get
			{
				return m_bHaveNovicegiftbagThird;
			}
			set
			{
				m_bHaveNovicegiftbagThird = value;
			}
		}

		public GameLoginType LoginType
		{
			get
			{
				return m_LoginType;
			}
			set
			{
				m_LoginType = value;
			}
		}

		public int TheDays
		{
			get
			{
				return theDays;
			}
		}

		public int Killed
		{
			get
			{
				return m_KilledCount;
			}
			set
			{
				m_KilledCount = value;
			}
		}

		public GameState()
		{
			inited = false;
			DeviceID = UtilsEx.DeviceId;
			LoginType = GameLoginType.LoginType_Local;
			if (PlayerPrefs.HasKey("LoginType"))
			{
				LoginType = (GameLoginType)PlayerPrefs.GetInt("LoginType");
			}
			else
			{
				PlayerPrefs.SetInt("LoginType", 0);
			}
			UUID = string.Empty;
			UserID = string.Empty;
			FacebookID = string.Empty;
			FacebookName = string.Empty;
			FacebookFriendList = new List<string>();
			GameCenterID = string.Empty;
			GameCenterName = string.Empty;
			GameCenterFriendList = new List<string>();
			ExternExp = 0;
			BeHiredMoney = 0;
			m_GameCenterUnlockListInfo = new List<int>(60);
			for (int i = 0; i < 60; i++)
			{
				m_GameCenterUnlockListInfo.Add(0);
			}
			exp = 0;
			Level = 1;
			ArrayList weapons = ConfigManager.Instance().GetFixedConfig().weapons;
			weaponInfo = new Hashtable();
			for (int j = 0; j < weapons.Count; j++)
			{
				FixedConfig.WeaponCfg weaponCfg = (FixedConfig.WeaponCfg)weapons[j];
				weaponInfo[(WeaponType)weaponCfg.type] = -1;
			}
			WeaponType weaponType = WeaponType.Beretta_33;
			weaponInfo[weaponType] = 0;
			battleWeapons = new List<WeaponType>();
			battleWeapons.Add(weaponType);
			m_PowerUPS = new Hashtable();
			AddPowerUPS(ItemType.PowerSmall);
			AddPowerUPS(ItemType.PowerSmall);
			AddPowerUPS(ItemType.HpSmall);
			AddPowerUPS(ItemType.FragGrenade);
			Avatars = new Hashtable();
			Avatar key = new Avatar(Avatar.AvatarSuiteType.Driver, Avatar.AvatarType.Head);
			Avatar key2 = new Avatar(Avatar.AvatarSuiteType.Driver, Avatar.AvatarType.Body);
			Avatars[key] = true;
			Avatars[key2] = true;
		}

		public void AddGamePlayTime(float battle_time)
		{
			m_TotalGameTime += Mathf.CeilToInt(battle_time);
		}

		public void UpdateMaxMapCfg(int passed_map_index, int passed_points_index, int passed_wave_index)
		{
			bool flag = false;
			if (passed_map_index <= m_htBattleStar.Count)
			{
				ArrayList arrayList = (ArrayList)m_htBattleStar[passed_map_index - 1];
				if (passed_points_index <= arrayList.Count)
				{
					if (passed_points_index + 1 <= arrayList.Count)
					{
						ChangeBattleStar(passed_map_index, passed_points_index + 1, 0);
					}
					flag = true;
				}
			}
			if (flag)
			{
				GameApp.GetInstance().Save();
				//GameClient.SetUserData();
			}
		}

		public void AddScore(int scoreAdd)
		{
			score += scoreAdd;
		}

		public int GetScore()
		{
			return score;
		}

		public void CreateAmazonIAPPrefab()
		{
			m_goIAPEventListener = UnityEngine.Object.Instantiate(Resources.Load("Zombie3D/Android_IAP/Amazon/EventListener")) as GameObject;
			m_goIAPManager = UnityEngine.Object.Instantiate(Resources.Load("Zombie3D/Android_IAP/Amazon/Manager")) as GameObject;
		}

		public void CreateIABIAPPrefab()
		{
			m_goIAPEventListener = UnityEngine.Object.Instantiate(Resources.Load("Zombie3D/Android_IAP/IAB/EventListener")) as GameObject;
			m_goIAPManager = UnityEngine.Object.Instantiate(Resources.Load("Zombie3D/Android_IAP/IAB/Manager")) as GameObject;
		}

		public void Init()
		{
			if (inited)
			{
				return;
			}
			Debug.Log("game state init");
			gold = 0;
			dollor = 0;
			MusicOn = true;
			SoundOn = true;
			WaveNum = 1;
			m_htBattleStar = new ArrayList();
			for (int i = 0; (long)i < 7L; i++)
			{
				ArrayList arrayList = new ArrayList();
				for (int j = 0; j < ConfigManager.Instance().GetFixedConfig().GetMaxPointsOfMap(i + 1); j++)
				{
					if (j == 0)
					{
						arrayList.Add(0);
					}
					else
					{
						arrayList.Add(-1);
					}
				}
				if (arrayList.Count > 50 && (i == 0 || i == 1 || i == 5 || i == 6) && (int)arrayList[50] <= 0)
				{
					arrayList[50] = 0;
				}
				m_htBattleStar.Add(arrayList);
			}
			m_MapsCDEndTime = new List<long>(7);
			for (int k = 0; (long)k < 7L; k++)
			{
				m_MapsCDEndTime.Add(0L);
			}
			m_HireFriends = new List<FriendUserData>();
			m_HiredFriendsInfo = new List<KeyValuePair<FriendUserData, long>>();
			m_CurSkillType = enSkillType.FastRun;
			m_Skilles = new List<Skill>();
			m_Skilles.Add(new Skill(m_CurSkillType, 1u));
			if (m_eGameMode.m_NBattleStatistics.Count == 0)
			{
				m_eGameMode.m_NBattleStatistics.Add(NetworkGameMode.NBattleStatistics.E_NBATTLEDEATHS, 0);
				m_eGameMode.m_NBattleStatistics.Add(NetworkGameMode.NBattleStatistics.E_NBATTLEKILLS, 0);
				m_eGameMode.m_NBattleStatistics.Add(NetworkGameMode.NBattleStatistics.E_NBATTLETIMES, 0);
			}
			FriendUserData friend_data = GenerateDefaultFriendPlayer();
			AddFriend(friend_data);
			DateTime now = DateTime.Now;
			m_FirstLoginTime = now.ToString(string.Format("{0:yyyy}-{0:MM}-{0:dd}|{0:HH:mm:ss}", now.Ticks));
			inited = true;
			CreateIABIAPPrefab();
			if (m_goIAPEventListener != null)
			{
				UnityEngine.Object.DontDestroyOnLoad(m_goIAPEventListener);
			}
			if (m_goIAPManager != null)
			{
				UnityEngine.Object.DontDestroyOnLoad(m_goIAPManager);
			}
		}

		public void InitWeapons()
		{
		}

		public List<WeaponType> GetBattleWeapons()
		{
			return battleWeapons;
		}

		public void OneDayPassed()
		{
			theDays++;
		}

		public void UpdateCDEndTime(int mapIndex, long time)
		{
			if (m_MapsCDEndTime.Count >= mapIndex)
			{
				m_MapsCDEndTime[mapIndex - 1] = time;
				GameApp.GetInstance().Save();
			}
		}

		public long GetMapsCDTime(int mapIndex)
		{
			if (m_MapsCDEndTime.Count >= mapIndex)
			{
				return m_MapsCDEndTime[mapIndex - 1];
			}
			return 0L;
		}

		public List<Skill> GetPlayerSkilles()
		{
			return m_Skilles;
		}

		public void AddSkill(Skill skill)
		{
			bool flag = false;
			for (int i = 0; i < m_Skilles.Count; i++)
			{
				if (m_Skilles[i].SkillType == skill.SkillType)
				{
					flag = true;
				}
			}
			if (!flag)
			{
				m_Skilles.Add(skill);
			}
			GameApp.GetInstance().Save();
		}

		public void UpdateSkill(enSkillType skill_type, uint skill_level_add)
		{
			bool flag = false;
			for (int i = 0; i < m_Skilles.Count; i++)
			{
				if (m_Skilles[i].SkillType == skill_type)
				{
					flag = true;
					m_Skilles[i].Level += skill_level_add;
				}
			}
			if (!flag)
			{
			}
			GameApp.GetInstance().Save();
		}

		public void SetGameTriggerInfo(int map_index, int points_index, int wave_index)
		{
			m_GameMapIndex = map_index;
			m_GamePointsIndex = points_index;
			m_GameWaveIndex = wave_index;
		}

		public void GetGameTriggerInfo(ref int map_index, ref int points_index, ref int wave_index)
		{
			map_index = m_GameMapIndex;
			points_index = m_GamePointsIndex;
			wave_index = m_GameWaveIndex;
		}

		public void AddExp(int experience)
		{
			Debug.Log("AddExp: " + exp + "|" + Level + "|" + experience);
			m_TotalExp += experience;
			int num = exp + experience;
			int num2 = 0;
			int num3 = 0;
			for (int i = 0; Level + i <= 255; i++)
			{
				int num4 = CalcLevelExp(Level + i);
				if (num >= num4)
				{
					num2++;
					num3 += num4;
					num -= num4;
					continue;
				}
				exp = num;
				Level += num2;
				break;
			}
			Debug.Log("AddExp 2 : " + experience + "|" + exp + "|" + Level);
			GameCollectionInfoManager.Instance().GetCurrentInfo().AddExp(experience);
			AddDailyCollectionInfo(0, 0f, 0, 0);
		}

		public void AddGold(int goldGot)
		{
			gold += goldGot;
			GameCollectionInfoManager.Instance().GetCurrentInfo().AddGold(goldGot);
			GameApp.GetInstance().Save();
			AddDailyCollectionInfo(0, 0f, 0, 0);
			m_TotalGoldGet += goldGot;
		}

		public void LoseGold(int goldSpend)
		{
			gold -= goldSpend;
			GameCollectionInfoManager.Instance().GetCurrentInfo().LoseGold(goldSpend);
			GameApp.GetInstance().Save();
			AddDailyCollectionInfo(0, 0f, 0, 0);
		}

		public int GetGold()
		{
			return gold;
		}

		public void AddDollor(int dollorGot)
		{
			dollor += dollorGot;
			GameCollectionInfoManager.Instance().GetCurrentInfo().AddDollor(dollorGot);
			GameApp.GetInstance().Save();
			//GameClient.SetUserData();
			AddDailyCollectionInfo(0, 0f, 0, 0);
			m_TotalDollorGet += dollorGot;
		}

		public void LoseDollor(int dollorGot)
		{
			dollor -= dollorGot;
			GameCollectionInfoManager.Instance().GetCurrentInfo().LoseDollor(dollorGot);
			GameApp.GetInstance().Save();
			//GameClient.SetUserData();
			AddDailyCollectionInfo(0, 0f, 0, 0);
		}

		public int GetDollor()
		{
			return dollor;
		}

		public bool HaveEnoughGold(int glod_count)
		{
			return gold >= glod_count;
		}

		public bool HaveEnoughDollor(int dollor_count)
		{
			return dollor >= dollor_count;
		}

		public Hashtable GetWeaponInfo()
		{
			return weaponInfo;
		}

		public List<WeaponType> GetWeapons()
		{
			List<WeaponType> list = new List<WeaponType>();
			foreach (int key in weaponInfo.Keys)
			{
				if ((int)weaponInfo[(WeaponType)key] == 0 || (int)weaponInfo[(WeaponType)key] == 1)
				{
					list.Add((WeaponType)key);
				}
			}
			return list;
		}

		public void RemoveWeapon(WeaponType wt)
		{
			foreach (int key in weaponInfo.Keys)
			{
				if ((int)weaponInfo[(WeaponType)key] == 0 && key == (int)wt)
				{
					weaponInfo[(WeaponType)key] = -1;
					break;
				}
			}
			GameApp.GetInstance().Save();
		}

		public void BuyWeapon(WeaponType wt, string price_type)
		{
			switch (wt)
			{
			case WeaponType.ZombieBusters:
				if (IsGCArchievementLocked(5))
				{
					UnlockGCArchievement(5, "com.trinitigame.callofminibulletdudes.a6");
				}
				break;
			case WeaponType.BigFirework:
				if (IsGCArchievementLocked(14))
				{
					UnlockGCArchievement(14, "com.trinitigame.callofminibulletdudes.a15");
				}
				break;
			case WeaponType.Volcano:
				if (IsGCArchievementLocked(15))
				{
					UnlockGCArchievement(15, "com.trinitigame.callofminibulletdudes.a16");
				}
				break;
			case WeaponType.MassacreCannon:
				if (IsGCArchievementLocked(22))
				{
					UnlockGCArchievement(22, "com.trinitigame.callofminibulletdudes.a23");
				}
				break;
			}
			if (price_type == "gold")
			{
				weaponInfo[wt] = 0;
				GameApp.GetInstance().Save();
			}
			else if (price_type == "dollor")
			{
				weaponInfo[wt] = 0;
				GameApp.GetInstance().Save();
				//GameClient.SetUserData();
			}
		}

		public void GetMaxMapCfg(int mapIndex, ref int maxPointsIndex, ref int maxWaveIndex, bool bIsTopHalf = true)
		{
			if (m_htBattleStar.Count >= mapIndex)
			{
				ArrayList arrayList = (ArrayList)m_htBattleStar[mapIndex - 1];
				int num = 0;
				int num2 = 50;
				if (bIsTopHalf)
				{
					num = 0;
					num2 = 50;
				}
				else
				{
					num = 50;
					num2 = 20;
				}
				for (int i = num; i < num + num2; i++)
				{
					if ((int)arrayList[i] <= 0)
					{
						maxPointsIndex = i + 1;
						break;
					}
				}
			}
			else
			{
				Debug.Log("ERROR - GameState.GetMaxMapCfg()!!! " + m_htBattleStar.Count + "|" + mapIndex + "|" + maxPointsIndex + "|" + maxWaveIndex);
			}
		}

		public Hashtable GetPowerUPS()
		{
			return m_PowerUPS;
		}

		public void AddPowerUPS(ItemType item_type)
		{
			if (m_PowerUPS.ContainsKey((int)item_type))
			{
				m_PowerUPS[(int)item_type] = (int)m_PowerUPS[(int)item_type] + 1;
			}
			else
			{
				m_PowerUPS[(int)item_type] = 1;
			}
		}

		public void BuyPowerUPS(ItemType item_type)
		{
			AddPowerUPS(item_type);
			GameApp.GetInstance().Save();
			//GameClient.SetUserData();
		}

		public Hashtable GetAvatars()
		{
			return Avatars;
		}

		public int GetAvatarHeadID()
		{
			foreach (Avatar key in Avatars.Keys)
			{
				if (key != null && (bool)Avatars[key] && key.AvtType == Avatar.AvatarType.Head)
				{
					return (int)key.SuiteType;
				}
			}
			return 1;
		}

		public int GetAvatarBodyID()
		{
			foreach (Avatar key in Avatars.Keys)
			{
				if (key != null && (bool)Avatars[key] && key.AvtType == Avatar.AvatarType.Body)
				{
					return (int)key.SuiteType;
				}
			}
			return 1;
		}

		public void BuyAvatar(Avatar.AvatarSuiteType avatar_suite_type, Avatar.AvatarType avatar_type)
		{
			Avatar key = new Avatar(avatar_suite_type, avatar_type);
			Avatars[key] = false;
			switch (avatar_suite_type)
			{
			case Avatar.AvatarSuiteType.X800:
			{
				if (!IsGCArchievementLocked(3))
				{
					break;
				}
				Avatar.AvatarType avatarType4 = Avatar.AvatarType.Head;
				if (avatar_type == Avatar.AvatarType.Head)
				{
					avatarType4 = Avatar.AvatarType.Body;
				}
				foreach (Avatar key2 in Avatars.Keys)
				{
					if (key2.SuiteType == avatar_suite_type && key2.AvtType == avatarType4)
					{
						UnlockGCArchievement(3, "com.trinitigame.callofminibulletdudes.a4");
					}
				}
				break;
			}
			case Avatar.AvatarSuiteType.Ninjalong:
			{
				if (!IsGCArchievementLocked(4))
				{
					break;
				}
				Avatar.AvatarType avatarType2 = Avatar.AvatarType.Head;
				if (avatar_type == Avatar.AvatarType.Head)
				{
					avatarType2 = Avatar.AvatarType.Body;
				}
				foreach (Avatar key3 in Avatars.Keys)
				{
					if (key3.SuiteType == avatar_suite_type && key3.AvtType == avatarType2)
					{
						UnlockGCArchievement(4, "com.trinitigame.callofminibulletdudes.a5");
					}
				}
				break;
			}
			case Avatar.AvatarSuiteType.ZombieAssassin:
			{
				if (!IsGCArchievementLocked(13))
				{
					break;
				}
				Avatar.AvatarType avatarType6 = Avatar.AvatarType.Head;
				if (avatar_type == Avatar.AvatarType.Head)
				{
					avatarType6 = Avatar.AvatarType.Body;
				}
				foreach (Avatar key4 in Avatars.Keys)
				{
					if (key4.SuiteType == avatar_suite_type && key4.AvtType == avatarType6)
					{
						UnlockGCArchievement(13, "com.trinitigame.callofminibulletdudes.a14");
					}
				}
				break;
			}
			case Avatar.AvatarSuiteType.Hacker:
			{
				if (!IsGCArchievementLocked(16))
				{
					break;
				}
				Avatar.AvatarType avatarType3 = Avatar.AvatarType.Head;
				if (avatar_type == Avatar.AvatarType.Head)
				{
					avatarType3 = Avatar.AvatarType.Body;
				}
				foreach (Avatar key5 in Avatars.Keys)
				{
					if (key5.SuiteType == avatar_suite_type && key5.AvtType == avatarType3)
					{
						UnlockGCArchievement(16, "com.trinitigame.callofminibulletdudes.a17");
					}
				}
				break;
			}
			case Avatar.AvatarSuiteType.DeathSquads:
			{
				if (!IsGCArchievementLocked(17))
				{
					break;
				}
				Avatar.AvatarType avatarType5 = Avatar.AvatarType.Head;
				if (avatar_type == Avatar.AvatarType.Head)
				{
					avatarType5 = Avatar.AvatarType.Body;
				}
				foreach (Avatar key6 in Avatars.Keys)
				{
					if (key6.SuiteType == avatar_suite_type && key6.AvtType == avatarType5)
					{
						UnlockGCArchievement(17, "com.trinitigame.callofminibulletdudes.a18");
					}
				}
				break;
			}
			case Avatar.AvatarSuiteType.Gladiator:
			{
				if (!IsGCArchievementLocked(18))
				{
					break;
				}
				Avatar.AvatarType avatarType = Avatar.AvatarType.Head;
				if (avatar_type == Avatar.AvatarType.Head)
				{
					avatarType = Avatar.AvatarType.Body;
				}
				foreach (Avatar key7 in Avatars.Keys)
				{
					if (key7.SuiteType == avatar_suite_type && key7.AvtType == avatarType)
					{
						UnlockGCArchievement(18, "com.trinitigame.callofminibulletdudes.a19");
					}
				}
				break;
			}
			}
			GameApp.GetInstance().Save();
			//GameClient.SetUserData();
		}

		public void ChangeAvatar(Avatar.AvatarSuiteType suite_type, Avatar.AvatarType avt_type)
		{
			foreach (Avatar key in Avatars.Keys)
			{
				if ((bool)Avatars[key] && key.AvtType == avt_type)
				{
					Avatars[key] = false;
					break;
				}
			}
			foreach (Avatar key2 in Avatars.Keys)
			{
				if (key2.SuiteType == suite_type && key2.AvtType == avt_type)
				{
					Avatars[key2] = true;
					break;
				}
			}
			GameApp.GetInstance().Save();
		}

		public void RemoveAvatar(Avatar.AvatarSuiteType suite_type, Avatar.AvatarType avt_type)
		{
			foreach (Avatar key in Avatars.Keys)
			{
				if (key.SuiteType == suite_type && key.AvtType == avt_type)
				{
					Avatars.Remove(key);
					break;
				}
			}
			GameApp.GetInstance().Save();
		}

		public ArrayList GetBattleFriends()
		{
			return m_BattleFriends;
		}

		public void SetBattleFriends(FriendUserData friend_data)
		{
			m_BattleFriends = new ArrayList();
			m_BattleFriends.Add(friend_data);
		}

		public ArrayList GetFriends()
		{
			return m_Friends;
		}

		public List<FriendUserData> GetHireFriends()
		{
			return m_HireFriends;
		}

		public List<KeyValuePair<FriendUserData, long>> GetHiredFriends()
		{
			return m_HiredFriendsInfo;
		}

		public void AddHireFriend(FriendUserData friend_data)
		{
			if (m_HireFriends == null)
			{
				m_HireFriends = new List<FriendUserData>();
			}
			if (m_HireFriends.Count < 25)
			{
				m_HireFriends.Add(friend_data);
			}
			Debug.Log("GameState.AddHireFriend() " + m_HireFriends.Count + " | " + friend_data.m_UUID);
		}

		private void AddHiredFriend(FriendUserData friend_data, long time)
		{
			m_HiredFriendsInfo.Add(new KeyValuePair<FriendUserData, long>(friend_data, time));
			Debug.Log("AddHiredFriend - " + m_HiredFriendsInfo.Count);
		}

		public FriendUserData GenerateDefaultFriendPlayer()
		{
			FriendUserData friendUserData = new FriendUserData();
			friendUserData.m_Name = "John";
			friendUserData.m_DeviceId = string.Empty;
			friendUserData.m_UUID = string.Empty;
			friendUserData.m_Exp = 0;
			friendUserData.m_Level = 1;
			friendUserData.m_BattleWeapons.Add(WeaponType.Beretta_33);
			friendUserData.m_BattleWeapons.Add(WeaponType.GrewCar_15);
			friendUserData.m_AvatarHeadSuiteType = 1;
			friendUserData.m_AvatarBodySuiteType = 1;
			return friendUserData;
		}

		public FriendUserData GetDefaultFriendPlayer()
		{
			return (FriendUserData)m_Friends[0];
		}

		public void AddFriend(FriendUserData friend_data)
		{
			Debug.Log("GameState.AddFriend() " + friend_data.m_Name);
			if (m_Friends == null)
			{
				m_Friends = new ArrayList();
			}
			if (m_Friends.Count < 100)
			{
				m_Friends.Add(friend_data);
			}
		}

		public void SetTestFriends()
		{
			m_Friends = new ArrayList();
			FriendUserData value = GenerateDefaultFriendPlayer();
			m_Friends.Add(value);
			value = new FriendUserData();
			value.m_Name = "John01";
			value.m_DeviceId = string.Empty;
			value.m_UUID = string.Empty;
			value.m_Exp = 0;
			value.m_Level = 1;
			value.m_BattleWeapons.Add(WeaponType.Beretta_33);
			value.m_BattleWeapons.Add(WeaponType.GrewCar_15);
			value.m_AvatarHeadSuiteType = 11;
			value.m_AvatarBodySuiteType = 17;
			m_Friends.Add(value);
			value = new FriendUserData();
			value.m_Name = "John02";
			value.m_DeviceId = string.Empty;
			value.m_UUID = string.Empty;
			value.m_Exp = 0;
			value.m_Level = 1;
			value.m_BattleWeapons.Add(WeaponType.UZI_E);
			value.m_BattleWeapons.Add(WeaponType.GrewCar_15);
			value.m_AvatarHeadSuiteType = 17;
			value.m_AvatarBodySuiteType = 4;
			m_Friends.Add(value);
			value = new FriendUserData();
			value.m_Name = "John03";
			value.m_DeviceId = string.Empty;
			value.m_UUID = string.Empty;
			value.m_Exp = 0;
			value.m_Level = 1;
			value.m_BattleWeapons.Add(WeaponType.Springfield_9mm);
			value.m_BattleWeapons.Add(WeaponType.ZombieBusters);
			value.m_AvatarHeadSuiteType = 4;
			value.m_AvatarBodySuiteType = 8;
			m_Friends.Add(value);
			value = new FriendUserData();
			value.m_Name = "John04";
			value.m_DeviceId = string.Empty;
			value.m_UUID = string.Empty;
			value.m_Exp = 0;
			value.m_Level = 1;
			value.m_BattleWeapons.Add(WeaponType.ParkerGaussRifle);
			value.m_BattleWeapons.Add(WeaponType.SimonovPistol);
			value.m_AvatarHeadSuiteType = 8;
			value.m_AvatarBodySuiteType = 13;
			m_Friends.Add(value);
			value = new FriendUserData();
			value.m_Name = "John05";
			value.m_DeviceId = string.Empty;
			value.m_UUID = string.Empty;
			value.m_Exp = 0;
			value.m_Level = 1;
			value.m_BattleWeapons.Add(WeaponType.BarrettSplitIII);
			value.m_BattleWeapons.Add(WeaponType.GrewCar_15);
			value.m_AvatarHeadSuiteType = 13;
			value.m_AvatarBodySuiteType = 5;
			m_Friends.Add(value);
			value = new FriendUserData();
			value.m_Name = "John06";
			value.m_DeviceId = string.Empty;
			value.m_UUID = string.Empty;
			value.m_Exp = 0;
			value.m_Level = 1;
			value.m_BattleWeapons.Add(WeaponType.Volcano);
			value.m_BattleWeapons.Add(WeaponType.NeutronRifle);
			value.m_AvatarHeadSuiteType = 5;
			value.m_AvatarBodySuiteType = 6;
			m_Friends.Add(value);
		}

		public void SetTestHireFriends()
		{
			m_HireFriends = new List<FriendUserData>();
			FriendUserData friendUserData = new FriendUserData();
			friendUserData.m_Name = "MERC";
			friendUserData.m_DeviceId = string.Empty;
			friendUserData.m_UUID = "MERC01";
			friendUserData.m_Exp = 0;
			friendUserData.m_Level = 1;
			friendUserData.m_BattleWeapons.Add(WeaponType.Beretta_33);
			friendUserData.m_BattleWeapons.Add(WeaponType.GrewCar_15);
			friendUserData.m_AvatarHeadSuiteType = 11;
			friendUserData.m_AvatarBodySuiteType = 17;
			AddHireFriend(friendUserData);
			friendUserData = new FriendUserData();
			friendUserData.m_Name = "MERC";
			friendUserData.m_DeviceId = string.Empty;
			friendUserData.m_UUID = "MERC02";
			friendUserData.m_Exp = 0;
			friendUserData.m_Level = 1;
			friendUserData.m_BattleWeapons.Add(WeaponType.UZI_E);
			friendUserData.m_BattleWeapons.Add(WeaponType.GrewCar_15);
			friendUserData.m_AvatarHeadSuiteType = 17;
			friendUserData.m_AvatarBodySuiteType = 4;
			AddHireFriend(friendUserData);
			friendUserData = new FriendUserData();
			friendUserData.m_Name = "MERC";
			friendUserData.m_DeviceId = string.Empty;
			friendUserData.m_UUID = "MERC03";
			friendUserData.m_Exp = 0;
			friendUserData.m_Level = 1;
			friendUserData.m_BattleWeapons.Add(WeaponType.Springfield_9mm);
			friendUserData.m_BattleWeapons.Add(WeaponType.ZombieBusters);
			friendUserData.m_AvatarHeadSuiteType = 4;
			friendUserData.m_AvatarBodySuiteType = 8;
			AddHireFriend(friendUserData);
			friendUserData = new FriendUserData();
			friendUserData.m_Name = "MERC";
			friendUserData.m_DeviceId = string.Empty;
			friendUserData.m_UUID = "MERC04";
			friendUserData.m_Exp = 0;
			friendUserData.m_Level = 1;
			friendUserData.m_BattleWeapons.Add(WeaponType.ParkerGaussRifle);
			friendUserData.m_BattleWeapons.Add(WeaponType.SimonovPistol);
			friendUserData.m_AvatarHeadSuiteType = 8;
			friendUserData.m_AvatarBodySuiteType = 13;
			AddHireFriend(friendUserData);
			friendUserData = new FriendUserData();
			friendUserData.m_Name = "MERC";
			friendUserData.m_DeviceId = string.Empty;
			friendUserData.m_UUID = "MERC05";
			friendUserData.m_Exp = 0;
			friendUserData.m_Level = 1;
			friendUserData.m_BattleWeapons.Add(WeaponType.BarrettSplitIII);
			friendUserData.m_BattleWeapons.Add(WeaponType.GrewCar_15);
			friendUserData.m_AvatarHeadSuiteType = 13;
			friendUserData.m_AvatarBodySuiteType = 5;
			AddHireFriend(friendUserData);
			friendUserData = new FriendUserData();
			friendUserData.m_Name = "MERC";
			friendUserData.m_DeviceId = string.Empty;
			friendUserData.m_UUID = "MERC06";
			friendUserData.m_Exp = 0;
			friendUserData.m_Level = 1;
			friendUserData.m_BattleWeapons.Add(WeaponType.Volcano);
			friendUserData.m_BattleWeapons.Add(WeaponType.NeutronRifle);
			friendUserData.m_AvatarHeadSuiteType = 5;
			friendUserData.m_AvatarBodySuiteType = 6;
			AddHireFriend(friendUserData);
		}

		public int GetDailyBonus()
		{
			long nowDateSeconds = UtilsEx.getNowDateSeconds();
			long num = nowDateSeconds / 86400;
			long num2 = lastDailyBonusGetTime / 86400;
			long num3 = num - num2;
			if (num3 > 0)
			{
				everyDayCrystalLootTotalCount = 0;
				if (num3 == 1)
				{
					lastDailyBonusLevel++;
					if (lastDailyBonusLevel == 7 && IsGCArchievementLocked(27))
					{
						UnlockGCArchievement(27, "com.trinitigame.callofminibulletdudes.a28");
					}
				}
				else
				{
					lastDailyBonusLevel = 1;
				}
				lastDailyBonusGetTime = nowDateSeconds;
				UnlockAllUnlockedGCArchievements();
				return lastDailyBonusLevel;
			}
			return 0;
		}

		public int GetPlayerLevel()
		{
			return Level;
		}

		public static int CalcLevelExp(int level)
		{
			float f = 65f;
			if (level == 1)
			{
				f = 65f;
			}
			else if (level < 11)
			{
				f = 15 * (level * level) + 50;
			}
			else if (level < 21)
			{
				f = 5 * (level * level) + 1250;
			}
			else if (level < 31)
			{
				f = 5 * (level * level) + 1500;
			}
			else if (level < 51)
			{
				f = 3 * (level * level) + 3500;
			}
			else if (level < 81)
			{
				f = 4 * (level * level) + 2000;
			}
			else if (level < 100)
			{
				f = 5 * (level * level) + 1000;
			}
			else if (level <= 255)
			{
				f = 3 * (level * level) + 25000;
			}
			return Mathf.FloorToInt(f);
		}

		public float GetPlayerExpNextLevelPercent()
		{
			if (Level < 255)
			{
				int num = exp;
				int num2 = CalcLevelExp(Level);
				return Mathf.Clamp01((float)num / (float)num2);
			}
			return 1f;
		}

		public void InitExchangeInfo()
		{
			m_BattleMapId = m_GameMapIndex;
			m_bExchanged = true;
			m_BattleStartTime = Time.time;
			m_BattleTime = 0f;
			m_BattleWaves = 0;
			m_BattlePerfectWaves = 0;
			m_KilledCount = 0;
			m_KilledEnemyInfo = new Hashtable();
			m_BattleGold = 0f;
			m_BattleGoldExchangePercent = 1f;
			m_BattleExpExchangePercent = 1f;
			m_BattleExp = 0f;
			m_BattleStar = 0;
		}

		public void IncreaseKills(EnemyType enemy_type, int count = 1)
		{
			m_bExchanged = false;
			m_bExchangeGold = false;
			m_bExchangeExp = false;
			if (m_KilledEnemyInfo.ContainsKey((int)enemy_type))
			{
				m_KilledEnemyInfo[(int)enemy_type] = (int)m_KilledEnemyInfo[(int)enemy_type] + count;
			}
			else
			{
				m_KilledEnemyInfo[(int)enemy_type] = count;
			}
			Killed++;
			m_GCKilledAllCount++;
			if (m_GCKilledAllCount >= 1000)
			{
				if (IsGCArchievementLocked(1))
				{
					UnlockGCArchievement(1, "com.trinitigame.callofminibulletdudes.a2");
				}
				if (m_GCKilledAllCount >= 10000 && IsGCArchievementLocked(7))
				{
					UnlockGCArchievement(7, "com.trinitigame.callofminibulletdudes.a8");
				}
			}
			m_BattleTime = Time.time - m_BattleStartTime;
		}

		public void IncreaseBattleGold(float gain_gold)
		{
			m_BattleGold += gain_gold;
			m_bExchanged = false;
			m_bExchangeGold = false;
		}

		public void IncreaseBattleExp(float gain_exp)
		{
			m_BattleExp += gain_exp;
			m_bExchanged = false;
			m_bExchangeExp = false;
		}

		public void IncreaseBattleWave(bool bPerfectWave)
		{
			if (m_BattleWaves == 0 && m_WaveExternExpPercent > 0f)
			{
				m_WaveExternExpPercent = 0f;
			}
			m_BattleWaves++;
			Debug.Log("GameState.IncreaseBattleWave - " + m_BattleWaves);
			float num = 0.02f;
			if (bPerfectWave)
			{
				num = 0.05f;
				m_BattlePerfectWaves++;
			}
			m_WaveExternExpPercent += num;
			GameApp.GetInstance().Save();
		}

		public void ExchangeGold(float exchange_percent)
		{
			m_bExchangeGold = true;
			if (m_bExchangeExp)
			{
				m_bExchanged = true;
			}
			AddGold(Mathf.FloorToInt(m_BattleGold * exchange_percent));
			SaveExchangeInfo();
			GameApp.GetInstance().Save();
		}

		public void ExchangeExp(float exchange_percent)
		{
			m_bExchangeExp = true;
			if (m_bExchangeGold)
			{
				m_bExchanged = true;
			}
			AddExp(Mathf.FloorToInt(m_BattleExp * exchange_percent));
			if (IsGCArchievementLocked(12) && GetPlayerLevel() >= 14)
			{
				UnlockGCArchievement(12, "com.trinitigame.callofminibulletdudes.a13");
			}
			AddGCBattleTime(Mathf.FloorToInt(m_BattleTime));
			UpdateGCCatogery();
			AddGamePlayTime(Mathf.FloorToInt(m_BattleTime));
			SaveExchangeInfo();
			GameApp.GetInstance().Save();
		}

		public void SaveExchangeInfo()
		{
			string path = Utils.SavePath() + "/ExchangeInfo";
			if (File.Exists(path))
			{
				File.Delete(path);
			}
			string path2 = Utils.SavePath() + "/ExchangeInfoN";
			string empty = string.Empty;
			int num = (m_bExchanged ? 1 : 0);
			string text = empty;
			empty = text + num + ";" + m_BattleMapId + ";" + Mathf.FloorToInt(m_BattleTime) + ";" + m_BattleWaves + ";" + m_BattlePerfectWaves + ";" + (m_bExchangeGold ? 1 : 0) + ";" + m_BattleGold + ";" + (m_bExchangeExp ? 1 : 0) + ";" + m_BattleExp + ";" + m_KilledCount + ";" + m_BattleGoldExchangePercent + ";" + m_BattleExpExchangePercent;
			if (m_KilledEnemyInfo.Count > 0)
			{
				empty += ";";
				foreach (int key in m_KilledEnemyInfo.Keys)
				{
					text = empty;
					empty = text + key + ":" + (int)m_KilledEnemyInfo[key] + ",";
				}
			}
			empty = empty + ";" + m_BattleStar;
			empty = ((!m_bIsSurvivalMode) ? (empty + ";0") : (empty + ";1"));
			empty = ((!m_IsPassStage) ? (empty + ";0") : (empty + ";1"));
			empty = ((!m_IsFastPassBattle) ? (empty + ";0") : (empty + ";1"));
			empty = ((!m_IsNoBruisePassBattle) ? (empty + ";0") : (empty + ";1"));
			string text2 = empty;
			string playerDataEncryptKey = MiscPlugin.GetPlayerDataEncryptKey("ME_2_@_YOU_DD");
			byte[] inArray = XXTEAUtils.Encrypt(Encoding.UTF8.GetBytes(empty), Encoding.ASCII.GetBytes(playerDataEncryptKey));
			text2 = Convert.ToBase64String(inArray);
			StreamWriter streamWriter = new StreamWriter(path2, false);
			streamWriter.Write(text2);
			streamWriter.Flush();
			streamWriter.Close();
		}

		public void AddExchangeUpgradeTime(long seconds)
		{
			long nowDateSeconds = UtilsEx.getNowDateSeconds();
			if (exchangeUpgradeEndTime <= nowDateSeconds)
			{
				exchangeUpgradeEndTime = nowDateSeconds + seconds;
			}
			else
			{
				exchangeUpgradeEndTime += seconds;
			}
		}

		public void AddGCBattleTime(int time)
		{
			m_GCTotalBattleTime += time;
		}

		public void UpdateGCCatogery()
		{
		}

		public void UnlockAllUnlockedGCArchievements()
		{
		}

		public bool IsGCArchievementLocked(int index)
		{
			return m_GameCenterUnlockListInfo[index] <= 0;
		}

		public void UnlockGCArchievement(int index, string archievement_id, int percent = 100)
		{
			FixedConfig.GameCenterArchievementsCfg gCArchievementCfg = ConfigManager.Instance().GetFixedConfig().GetGCArchievementCfg(archievement_id);
			if (percent >= 100)
			{
				m_GameCenterUnlockListInfo[index] = 1;
				GameApp.GetInstance().Save();
				if (gCArchievementCfg != null)
				{
					AddAchievementUI(gCArchievementCfg.name);
				}
			}
			if (gCArchievementCfg == null)
			{
			}
		}

		public void AddGCBoomerAttackTimes()
		{
			m_GCBoomerAttackTimes++;
			if (m_GCBoomerAttackTimes >= 100 && IsGCArchievementLocked(10))
			{
				UnlockGCArchievement(10, "com.trinitigame.callofminibulletdudes.a11");
			}
		}

		public void AddGCPlayerDeadTimes()
		{
			m_GCDeadTimes++;
			if (m_GCDeadTimes >= 99 && IsGCArchievementLocked(11))
			{
				UnlockGCArchievement(11, "com.trinitigame.callofminibulletdudes.a12");
			}
		}

		public void AddIapTimes()
		{
			m_GCIapTimes++;
			if (m_GCIapTimes > 0 && IsGCArchievementLocked(19))
			{
				UnlockGCArchievement(19, "com.trinitigame.callofminibulletdudes.a20");
			}
		}

		public void AddIapSpendDollor(float spend)
		{
			m_GCIapTotalDollor += spend;
			if (m_GCIapTotalDollor >= 49.99f)
			{
				if (IsGCArchievementLocked(20))
				{
					UnlockGCArchievement(20, "com.trinitigame.callofminibulletdudes.a21");
				}
			}
			else if (m_GCIapTotalDollor >= 99.99f && IsGCArchievementLocked(21))
			{
				UnlockGCArchievement(21, "com.trinitigame.callofminibulletdudes.a22");
			}
		}

		public void AddGCPlayWithNetFriendTimes()
		{
			m_GCPlayWithNetFriendTimes++;
			if (m_GCPlayWithNetFriendTimes > 0 && IsGCArchievementLocked(25))
			{
				UnlockGCArchievement(25, "com.trinitigame.callofminibulletdudes.a26");
			}
		}

		public void AddGCFriendPlayerDeadTimes()
		{
			m_GCFriendPlayerDeadTimes++;
			if (m_GCFriendPlayerDeadTimes >= 200 && IsGCArchievementLocked(26))
			{
				UnlockGCArchievement(26, "com.trinitigame.callofminibulletdudes.a27");
			}
		}

		public void LoadExchangeInfo()
		{
			//Discarded unreachable code: IL_008b, IL_0320
			string path = Utils.SavePath() + "/ExchangeInfoN";
			string text = string.Empty;
			StreamReader streamReader = null;
			try
			{
				streamReader = new StreamReader(path);
				text = streamReader.ReadToEnd();
			}
			catch
			{
				Debug.Log("ERROR - GameState Load()!!!");
			}
			finally
			{
				if (streamReader != null)
				{
					streamReader.Close();
				}
			}
			string text2 = text;
			try
			{
				byte[] data = Convert.FromBase64String(text);
				string playerDataEncryptKey = MiscPlugin.GetPlayerDataEncryptKey("ME_2_@_YOU_DD");
				byte[] bytes = XXTEAUtils.Decrypt(data, Encoding.ASCII.GetBytes(playerDataEncryptKey));
				text2 = Encoding.UTF8.GetString(bytes);
			}
			catch
			{
				return;
			}
			Debug.Log(text2);
			if (text2 != string.Empty)
			{
				string[] array = text2.Split(';');
				try
				{
					m_bExchanged = ((!(array[0] == "0")) ? true : false);
					m_BattleMapId = int.Parse(array[1]);
					m_BattleTime = float.Parse(array[2]);
					m_BattleWaves = int.Parse(array[3]);
					m_BattlePerfectWaves = int.Parse(array[4]);
					m_bExchangeGold = ((!(array[5] == "0")) ? true : false);
					m_BattleGold = float.Parse(array[6]);
					m_bExchangeExp = ((!(array[7] == "0")) ? true : false);
					m_BattleExp = float.Parse(array[8]);
					m_KilledCount = int.Parse(array[9].ToString());
					m_BattleGoldExchangePercent = float.Parse(array[10].ToString());
					m_BattleExpExchangePercent = float.Parse(array[11].ToString());
					m_KilledEnemyInfo = new Hashtable();
					string[] array2 = array[12].Split(',');
					for (int i = 0; i < array2.Length; i++)
					{
						string[] array3 = array2[i].Split(':');
						if (array3.Length == 2)
						{
							int num = int.Parse(array3[0]);
							int num2 = int.Parse(array3[1]);
							m_KilledEnemyInfo[num] = num2;
						}
					}
					m_BattleStar = int.Parse(array[13]);
					if (array.Length >= 15)
					{
						int num3 = int.Parse(array[14]);
						if (num3 == 1)
						{
							m_bIsSurvivalMode = true;
						}
						else
						{
							m_bIsSurvivalMode = false;
						}
					}
					if (array.Length >= 16)
					{
						int num4 = int.Parse(array[15]);
						if (num4 == 1)
						{
							m_IsPassStage = true;
						}
						else
						{
							m_IsPassStage = false;
						}
					}
					if (array.Length >= 17)
					{
						int num5 = int.Parse(array[16]);
						if (num5 == 1)
						{
							m_IsFastPassBattle = true;
						}
						else
						{
							m_IsFastPassBattle = false;
						}
					}
					if (array.Length >= 18)
					{
						int num6 = int.Parse(array[17]);
						if (num6 == 1)
						{
							m_IsNoBruisePassBattle = true;
						}
						else
						{
							m_IsNoBruisePassBattle = false;
						}
					}
				}
				catch
				{
					return;
				}
			}
			Debug.Log("Load Exchange ------------ " + m_bExchanged + "|" + m_bExchangeGold + "|" + m_bExchangeExp);
		}

		public void AddEveryDayCrystalLootTotalCount(int loot_count)
		{
			everyDayCrystalLootTotalCount += loot_count;
		}

		public int GetHireOutSelfPrice()
		{
			int num = 0;
			num += Level * 5;
			for (int i = 0; i < battleWeapons.Count; i++)
			{
				int num2 = 0;
				FixedConfig.WeaponCfg weaponCfg = ConfigManager.Instance().GetFixedConfig().GetWeaponCfg(battleWeapons[i]);
				if (weaponCfg.mClass == 1)
				{
					num2 = 50;
				}
				else if (weaponCfg.mClass == 2)
				{
					num2 = 75;
				}
				else if (weaponCfg.mClass == 3)
				{
					num2 = 125;
				}
				else if (weaponCfg.mClass == 4)
				{
					num2 = 200;
				}
				else if (weaponCfg.mClass == 5)
				{
					num2 = 300;
				}
				else if (weaponCfg.mClass == 6)
				{
					num2 = 425;
				}
				else if (weaponCfg.mClass == 7)
				{
					num2 = 575;
				}
				else if (weaponCfg.mClass == 8)
				{
					num2 = 750;
				}
				if (weaponCfg.priceType == "dollor")
				{
					num2 += weaponCfg.price * 5;
				}
				num += num2;
			}
			foreach (Avatar key in Avatars.Keys)
			{
				if ((bool)Avatars[key])
				{
					int num3 = 0;
					FixedConfig.AvatarCfg avatarCfg = ConfigManager.Instance().GetFixedConfig().GetAvatarCfg(key.SuiteType, key.AvtType);
					if (avatarCfg.m_Class == 1)
					{
						num3 = 50;
					}
					else if (avatarCfg.m_Class == 2)
					{
						num3 = 75;
					}
					else if (avatarCfg.m_Class == 3)
					{
						num3 = 125;
					}
					else if (avatarCfg.m_Class == 4)
					{
						num3 = 200;
					}
					else if (avatarCfg.m_Class == 5)
					{
						num3 = 300;
					}
					else if (avatarCfg.m_Class == 6)
					{
						num3 = 425;
					}
					else if (avatarCfg.m_Class == 7)
					{
						num3 = 575;
					}
					else if (avatarCfg.m_Class == 8)
					{
						num3 = 750;
					}
					if (avatarCfg.priceType == "dollor")
					{
						num3 += avatarCfg.price * 5;
					}
					num += num3;
				}
			}
			return Mathf.Min(num, 5000);
		}

		public void HireOut(long time)
		{
			m_LastHireOutTime = time;
			GameApp.GetInstance().Save();
		}

		private void CheckCrackedUserData()
		{
			if (dollor <= 0)
			{
				return;
			}
			bool flag = false;
			if (m_GCIapTimes > 0)
			{
				if (dollor > 2000)
				{
					flag = true;
				}
			}
			else if (dollor > 33)
			{
				flag = true;
			}
			if (!flag)
			{
				return;
			}
			gold = 0;
			dollor = 0;
			Level = 1;
			exp = 0;
			m_htBattleStar = new ArrayList();
			int num = 5;
			for (int i = 0; i < num; i++)
			{
				ArrayList arrayList = new ArrayList();
				for (int j = 0; j < ConfigManager.Instance().GetFixedConfig().GetMaxPointsOfMap(i + 1); j++)
				{
					if (j == 0)
					{
						arrayList.Add(0);
					}
					else
					{
						arrayList.Add(-1);
					}
				}
				m_htBattleStar.Add(arrayList);
			}
			ArrayList weapons = ConfigManager.Instance().GetFixedConfig().weapons;
			weaponInfo = new Hashtable();
			for (int k = 0; k < weapons.Count; k++)
			{
				FixedConfig.WeaponCfg weaponCfg = (FixedConfig.WeaponCfg)weapons[k];
				weaponInfo[(WeaponType)weaponCfg.type] = -1;
			}
			WeaponType weaponType = WeaponType.Beretta_33;
			weaponInfo[weaponType] = 0;
			battleWeapons = new List<WeaponType>();
			battleWeapons.Add(weaponType);
			Avatars = new Hashtable();
			Avatar key = new Avatar(Avatar.AvatarSuiteType.Driver, Avatar.AvatarType.Head);
			Avatar key2 = new Avatar(Avatar.AvatarSuiteType.Driver, Avatar.AvatarType.Body);
			Avatars[key] = true;
			Avatars[key2] = true;
			m_PowerUPS = new Hashtable();
		}

		public void Load(string file_path)
		{
			Debug.Log("GameState.Load()");
			if (!inited)
			{
				Init();
			}
			string input_data = string.Empty;
			StreamReader streamReader = null;
			try
			{
				streamReader = new StreamReader(file_path);
				input_data = streamReader.ReadToEnd();
			}
			catch
			{
				Debug.Log("ERROR - GameState Load()!!!");
			}
			finally
			{
				if (streamReader != null)
				{
					streamReader.Close();
				}
			}
			LoadData(input_data);
		}

		public void GetShijiaoPeizhi()
		{
			string path = Utils.SavePath() + "/Abc.txt";
			string text = string.Empty;
			StreamReader streamReader = null;
			try
			{
				streamReader = new StreamReader(path);
				text = streamReader.ReadToEnd();
			}
			catch
			{
				Debug.Log("ERROR - GameState Load()!!!");
			}
			finally
			{
				if (streamReader != null)
				{
					streamReader.Close();
				}
			}
			if (text != string.Empty)
			{
				string[] array = text.Split('\r', '\n');
				int num = int.Parse(array[0].Trim());
				if (num == 2)
				{
					m_iCameraModeType = 2;
				}
				else
				{
					m_iCameraModeType = 1;
				}
			}
		}

		public void LoadData(string input_data, bool isFriendData = false)
		{
			//Discarded unreachable code: IL_00e9
			string text = input_data;
			int num = 2;
			try
			{
				byte[] data = Convert.FromBase64String(input_data);
				byte[] array = XXTEAUtils.Decrypt(data, Encoding.ASCII.GetBytes("T_Zombie_DDS_1"));
				if (array != null)
				{
					string @string = Encoding.UTF8.GetString(array);
					if (@string.IndexOf("LastSaveTime") >= 0 && @string.IndexOf("WeaponList") >= 0 && @string.IndexOf("AvatarList") >= 0)
					{
						text = @string;
						num = 1;
					}
					else
					{
						string playerDataEncryptKey = MiscPlugin.GetPlayerDataEncryptKey("ME_2_@_YOU_DD");
						byte[] bytes = XXTEAUtils.Decrypt(data, Encoding.ASCII.GetBytes(playerDataEncryptKey));
						text = Encoding.UTF8.GetString(bytes);
					}
				}
				else
				{
					string playerDataEncryptKey2 = MiscPlugin.GetPlayerDataEncryptKey("ME_2_@_YOU_DD");
					byte[] bytes2 = XXTEAUtils.Decrypt(data, Encoding.ASCII.GetBytes(playerDataEncryptKey2));
					text = Encoding.UTF8.GetString(bytes2);
				}
			}
			catch (Exception)
			{
				Debug.Log("ERROR: - GameState.LoadData() - Exception01");
				return;
			}
			if (text != string.Empty)
			{
				string[] array2 = text.Split('\r', '\n');
				long num2 = 1L;
				for (int i = 0; i < array2.Length; i++)
				{
					if (array2[i] == null)
					{
						continue;
					}
					string[] array3 = array2[i].Split('\t');
					if (array3.Length >= 2)
					{
						string text2 = array3[0];
						if (text2 == "LastSaveTime")
						{
							num2 = long.Parse(array3[1]);
							break;
						}
					}
				}
				if (num2 < lastSaveTime)
				{
					Debug.Log("LastSaveTime is TOO OLD!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
					return;
				}
				bool flag = false;
				for (int j = 0; j < array2.Length; j++)
				{
					if (array2[j] == null)
					{
						continue;
					}
					try
					{
						string[] array4 = array2[j].Split('\t');
						if (array4.Length < 2)
						{
							continue;
						}
						string text3 = array4[0];
						switch (text3)
						{
						case "WeaponList":
						{
							weaponInfo = new Hashtable();
							weaponAccouter = string.Empty;
							weaponAccouter = array4[1];
							string[] array7 = array4[1].Split(',');
							for (int num4 = 0; num4 < array7.Length; num4++)
							{
								try
								{
									WeaponType weaponType = (WeaponType)int.Parse(array7[num4]);
									weaponInfo[weaponType] = 0;
								}
								catch (Exception)
								{
									weaponInfo[WeaponType.Beretta_33] = 0;
									flag = true;
								}
							}
							battleWeapons = new List<WeaponType>();
							string[] array8 = array4[2].Split(',');
							for (int num5 = 0; num5 < array8.Length; num5++)
							{
								WeaponType weaponType2 = (WeaponType)int.Parse(array8[num5]);
								weaponInfo[weaponType2] = 1;
								battleWeapons.Add(weaponType2);
							}
							break;
						}
						case "AvatarList":
						{
							Avatars = new Hashtable();
							avatarAccouter = string.Empty;
							avatarAccouter = array4[1];
							string[] array9 = array4[1].Split(';');
							for (int num6 = 0; num6 < array9.Length; num6++)
							{
								string[] array10 = array9[num6].Split(',');
								if (array10.Length == 3)
								{
									int avatar_suite_type = int.Parse(array10[0]);
									int avatar_type = int.Parse(array10[1]);
									int num7 = int.Parse(array10[2]);
									bool flag2 = false;
									if (num7 == 1)
									{
										flag2 = true;
									}
									Avatar key = new Avatar((Avatar.AvatarSuiteType)avatar_suite_type, (Avatar.AvatarType)avatar_type);
									Avatars[key] = flag2;
								}
								else
								{
									Debug.LogError("ERROE: Avatar info wrong!!!");
								}
							}
							break;
						}
						case "PowerUPSList":
						{
							m_PowerUPS = new Hashtable();
							powerUpsAccount = string.Empty;
							powerUpsAccount = array4[1];
							string[] array11 = array4[1].Split(';');
							for (int num8 = 0; num8 < array11.Length; num8++)
							{
								string[] array12 = array11[num8].Split(',');
								if (array12.Length == 2)
								{
									int num9 = 1;
									int num10 = 1;
									try
									{
										num9 = int.Parse(array12[0]);
										num10 = int.Parse(array12[1]);
									}
									catch
									{
										flag = true;
										Debug.Log("PowerUPSList Error!");
									}
									m_PowerUPS[num9] = num10;
								}
								else
								{
									Debug.LogError("ERROE: PowerUPS info wrong!!!");
								}
							}
							break;
						}
						case "GCUnlockList":
						{
							m_GameCenterUnlockListInfo = new List<int>(60);
							for (int m = 0; m < 60; m++)
							{
								m_GameCenterUnlockListInfo.Add(0);
							}
							string[] array6 = array4[1].Split(';');
							for (int n = 0; n < array6.Length; n++)
							{
								try
								{
									m_GameCenterUnlockListInfo[n] = int.Parse(array6[n]);
								}
								catch
								{
									m_GameCenterUnlockListInfo[n] = 0;
								}
							}
							break;
						}
						case "LastSaveTime":
							lastSaveTime = long.Parse(array4[1]);
							break;
						case "uuid":
							UUID = array4[1];
							break;
						case "UserId":
							UserID = array4[1];
							break;
						case "FacebookID":
							FacebookID = array4[1];
							break;
						case "FacebookName":
							FacebookName = array4[1];
							break;
						case "GameCenterID":
							GameCenterID = array4[1];
							break;
						case "GameCenterName":
							GameCenterName = array4[1];
							break;
						case "Exp":
							exp = int.Parse(array4[1]);
							break;
						case "Level":
							Level = int.Parse(array4[1]);
							break;
						case "Gold":
							gold = int.Parse(array4[1]);
							break;
						case "Dollor":
							dollor = int.Parse(array4[1]);
							break;
						case "MusicOn":
							MusicOn = ((!(array4[1].Trim() == "0")) ? true : false);
							break;
						case "SoundOn":
							SoundOn = ((!(array4[1].Trim() == "0")) ? true : false);
							break;
						case "DailyBonusGetTime":
							lastDailyBonusGetTime = long.Parse(array4[1]);
							break;
						case "DailyBonusLevel":
							lastDailyBonusLevel = int.Parse(array4[1]);
							break;
						case "ExchangeUpgradeEndTime":
							exchangeUpgradeEndTime = long.Parse(array4[1]);
							break;
						case "GCTotalBattleTime":
							m_GCTotalBattleTime = int.Parse(array4[1]);
							break;
						case "GCKilledAll":
							m_GCKilledAllCount = int.Parse(array4[1]);
							break;
						case "GCBoomerAttackTimes":
							m_GCBoomerAttackTimes = int.Parse(array4[1]);
							break;
						case "GCDeadTimes":
							m_GCDeadTimes = int.Parse(array4[1]);
							break;
						case "GCIapTimes":
							m_GCIapTimes = int.Parse(array4[1]);
							break;
						case "GCIapSpendDollor":
							m_GCIapTotalDollor = float.Parse(array4[1]);
							break;
						case "GCPlayWithFriendTimes":
							m_GCPlayWithNetFriendTimes = int.Parse(array4[1]);
							break;
						case "GCFriendPlayerDeadTimes":
							m_GCFriendPlayerDeadTimes = int.Parse(array4[1]);
							break;
						case "Gift":
							m_GetFirstGift = int.Parse(array4[1]);
							break;
						case "Review":
							m_ReviewCount = int.Parse(array4[1]);
							break;
						case "BattleCount":
							m_BattleCount = int.Parse(array4[1]);
							break;
						case "MapsCDTime":
						{
							m_MapsCDEndTime = new List<long>();
							string[] array5 = array4[1].Split(',');
							for (int k = 0; k < array5.Length; k++)
							{
								m_MapsCDEndTime.Add(long.Parse(array5[k].Trim()));
							}
							if ((long)m_MapsCDEndTime.Count < 7L)
							{
								int num3 = 7 - m_MapsCDEndTime.Count;
								for (int l = 0; l < num3; l++)
								{
									m_MapsCDEndTime.Add(0L);
								}
							}
							break;
						}
						default:
							if ("RoleIsLevelUp" == text3)
							{
								m_IsLevelUp = int.Parse(array4[1]);
							}
							break;
						}
						switch (text3)
						{
						case "MapPointBattleStar":
						{
							m_htBattleStar = new ArrayList();
							for (int num18 = 1; num18 < array4.Length; num18++)
							{
								string[] array23 = array4[num18].Split(',');
								if (array23.Length <= 0)
								{
									continue;
								}
								ArrayList arrayList = new ArrayList();
								for (int num19 = 0; num19 < array23.Length; num19++)
								{
									arrayList.Add(int.Parse(array23[num19]));
								}
								if (arrayList.Count <= ConfigManager.Instance().GetFixedConfig().GetMaxPointsOfMap(num18))
								{
									for (int num20 = arrayList.Count; num20 < ConfigManager.Instance().GetFixedConfig().GetMaxPointsOfMap(num18); num20++)
									{
										if ((int)arrayList[num20 - 1] > 0)
										{
											arrayList.Add(0);
										}
										else
										{
											arrayList.Add(-1);
										}
									}
								}
								if (arrayList.Count > 50 && (num18 == 1 || num18 == 2 || num18 == 6 || num18 == 7) && (int)arrayList[50] <= 0)
								{
									arrayList[50] = 0;
								}
								m_htBattleStar.Add(arrayList);
							}
							if ((long)m_htBattleStar.Count >= 7L)
							{
								break;
							}
							int num21 = 7 - m_htBattleStar.Count;
							for (int num22 = 0; num22 < num21; num22++)
							{
								ArrayList arrayList2 = new ArrayList();
								for (int num23 = 0; num23 < ConfigManager.Instance().GetFixedConfig().GetMaxPointsOfMap(m_htBattleStar.Count + 1); num23++)
								{
									if (num23 == 0)
									{
										arrayList2.Add(0);
									}
									else
									{
										arrayList2.Add(-1);
									}
								}
								m_htBattleStar.Add(arrayList2);
							}
							break;
						}
						case "LastHireOutTime":
							m_LastHireOutTime = long.Parse(array4[1]);
							break;
						case "HiredFriendsInfo":
						{
							m_HiredFriendsInfo = new List<KeyValuePair<FriendUserData, long>>();
							string[] array15 = array4[1].Split(';');
							for (int num13 = 0; num13 < array15.Length; num13++)
							{
								FriendUserData friendUserData = new FriendUserData();
								string[] array16 = array15[num13].Split(',');
								friendUserData.m_UUID = array16[0];
								string text4 = Utils.SavePath();
								string path = text4 + "/" + friendUserData.m_UUID;
								if (File.Exists(path))
								{
									AddHiredFriend(friendUserData, long.Parse(array16[1]));
								}
							}
							CheckHiredFriendsTime();
							GetHiredFriendsData();
							break;
						}
						case "SendGiftGoldDollor_1_3_2":
							m_SendGiftGoldDollor_1_3_2 = int.Parse(array4[1]);
							break;
						case "Skilles":
						{
							m_Skilles = new List<Skill>();
							string[] array21 = array4[1].Split(';');
							for (int num16 = 0; num16 < array21.Length; num16++)
							{
								Skill skill = new Skill();
								string[] array22 = array21[num16].Split(',');
								skill.SkillType = (enSkillType)int.Parse(array22[0]);
								skill.Level = uint.Parse(array22[1]);
								bool flag3 = false;
								for (int num17 = 0; num17 < m_Skilles.Count; num17++)
								{
									if (m_Skilles[num17].SkillType == skill.SkillType)
									{
										flag3 = true;
									}
								}
								if (!flag3)
								{
									m_Skilles.Add(skill);
								}
							}
							break;
						}
						case "CurSkillType":
							m_CurSkillType = (enSkillType)int.Parse(array4[1]);
							break;
						case "FirstLoginTime":
							m_FirstLoginTime = array4[1];
							break;
						case "TotalExp":
							m_TotalExp = int.Parse(array4[1]);
							break;
						case "TotalGameTime":
							m_TotalGameTime = int.Parse(array4[1]);
							break;
						case "TotalGoldGet":
							m_TotalGoldGet = int.Parse(array4[1]);
							break;
						case "TotalDollorGet":
							m_TotalDollorGet = int.Parse(array4[1]);
							break;
						case "ShareWithTwitter":
							if (int.Parse(array4[1]) == 0)
							{
								ShareWithTwitter = false;
							}
							else
							{
								ShareWithTwitter = true;
							}
							break;
						case "CameraTypeInfo":
							m_iCameraModeType = int.Parse(array4[1]);
							break;
						case "NetworkName":
							m_eGameMode.m_strNetName = array4[1];
							break;
						case "OwnedNoviceGiftBag":
							NoviceGiftBag = int.Parse(array4[1]);
							break;
						case "OwnedNoviceGiftBagSec":
							NoviceGiftBagSec = int.Parse(array4[1]);
							break;
						case "OwnedNoviceGiftBagThird":
							NoviceGiftBagThird = int.Parse(array4[1]);
							break;
						case "NBattleBuff":
						{
							if (m_eGameMode.m_PlaersNBattleBuff == null)
							{
								m_eGameMode.m_PlaersNBattleBuff = new Dictionary<enBattlefieldProps, int>();
							}
							else
							{
								m_eGameMode.m_PlaersNBattleBuff.Clear();
							}
							string[] array19 = array4[1].Split(';');
							for (int num15 = 0; num15 < array19.Length; num15++)
							{
								string[] array20 = array19[num15].Split(',');
								enBattlefieldProps key3 = (enBattlefieldProps)int.Parse(array20[0]);
								int value2 = int.Parse(array20[1]);
								m_eGameMode.m_PlaersNBattleBuff.Add(key3, value2);
							}
							break;
						}
						case "NBattleStatistics":
						{
							if (m_eGameMode.m_NBattleStatistics == null)
							{
								m_eGameMode.m_NBattleStatistics = new Dictionary<NetworkGameMode.NBattleStatistics, int>();
								m_eGameMode.m_NBattleStatistics.Add(NetworkGameMode.NBattleStatistics.E_NBATTLEDEATHS, 0);
								m_eGameMode.m_NBattleStatistics.Add(NetworkGameMode.NBattleStatistics.E_NBATTLEKILLS, 0);
								m_eGameMode.m_NBattleStatistics.Add(NetworkGameMode.NBattleStatistics.E_NBATTLETIMES, 0);
							}
							else
							{
								m_eGameMode.m_NBattleStatistics.Clear();
							}
							string[] array17 = array4[1].Split(';');
							for (int num14 = 0; num14 < array17.Length; num14++)
							{
								string[] array18 = array17[num14].Split(',');
								NetworkGameMode.NBattleStatistics key2 = (NetworkGameMode.NBattleStatistics)int.Parse(array18[0]);
								int value = int.Parse(array18[1]);
								m_eGameMode.m_NBattleStatistics.Add(key2, value);
							}
							break;
						}
						case "PVPWinTimes":
							m_eGameMode.m_CombatDataRecord.iPVPWinTimes = int.Parse(array4[1]);
							break;
						case "PVPKillPlayerCount":
							m_eGameMode.m_CombatDataRecord.iPVPKillPlayerCount = int.Parse(array4[1]);
							break;
						case "PVPDoubleKillCount":
							m_eGameMode.m_CombatDataRecord.iPVPDoubleKillCount = int.Parse(array4[1]);
							break;
						case "PVPThreeKillCount":
							m_eGameMode.m_CombatDataRecord.iPVPThreeKillCount = int.Parse(array4[1]);
							break;
						case "PVPFourKillCount":
							m_eGameMode.m_CombatDataRecord.iPVPFourKillCount = int.Parse(array4[1]);
							break;
						case "PVPFiveKillCount":
							m_eGameMode.m_CombatDataRecord.iPVPFiveKillCount = int.Parse(array4[1]);
							break;
						case "PVPBestKillerCount":
							m_eGameMode.m_CombatDataRecord.iPVPBestKillerCount = int.Parse(array4[1]);
							break;
						case "WearShinobiToPlayTimes":
							m_eGameMode.m_CombatDataRecord.iWearShinobiToPlayTimes = int.Parse(array4[1]);
							break;
						case "FirstBloodCount":
							m_eGameMode.m_CombatDataRecord.iFirstBloodCount = int.Parse(array4[1]);
							break;
						case "PVPNoUseFastRunCount":
							m_eGameMode.m_CombatDataRecord.iPVPNoUseFastRunCount = int.Parse(array4[1]);
							break;
						case "PVPKillMoreTenPlayersOnceWar":
							m_eGameMode.m_CombatDataRecord.iPVPKillMoreTenPlayersOnceWar = int.Parse(array4[1]);
							break;
						case "IsNoHeartToWinOneGame":
							if (int.Parse(array4[1]) == 0)
							{
								m_eGameMode.m_CombatDataRecord.bIsNoDeathToWinOneGame = false;
							}
							else
							{
								m_eGameMode.m_CombatDataRecord.bIsNoDeathToWinOneGame = true;
							}
							break;
						case "BuyNBattleShop":
						{
							m_eGameMode.m_CombatDataRecord.lsBuyNBattleShop.Clear();
							string[] array14 = array4[1].Split(',');
							for (int num12 = 0; num12 < array14.Length; num12++)
							{
								m_eGameMode.m_CombatDataRecord.lsBuyNBattleShop.Add((enBattlefieldProps)int.Parse(array14[num12]));
							}
							break;
						}
						case "WearAvatarTimes":
						{
							m_eGameMode.m_CombatDataRecord.lsWearAvatarTimes.Clear();
							string[] array13 = array4[1].Split(',');
							for (int num11 = 0; num11 < array13.Length; num11++)
							{
								m_eGameMode.m_CombatDataRecord.lsWearAvatarTimes.Add((Avatar.AvatarSuiteType)int.Parse(array13[j]));
							}
							break;
						}
						case "PropsAdditionsList":
							PropsAdditionsFromString(array4[1]);
							break;
						case "ShowFirstBuffPresentation":
							m_eGameMode.m_CombatDataRecord.bShowFirstBuffPresentation = int.Parse(array4[1]);
							break;
						case "OwnBuffMaxCount":
							m_eGameMode.m_CombatDataRecord.iGetBuffCount = int.Parse(array4[1]);
							break;
						default:
							if (!(text3 == string.Empty))
							{
							}
							break;
						}
					}
					catch
					{
						Debug.Log("GameState.LoadData() Exception!!!");
					}
				}
				if (flag)
				{
					Debug.Log("ErrorData:|" + text + "|");
				}
			}
			if (num == 1)
			{
				CheckCrackedUserData();
			}
		}

		public void Save(string file_path)
		{
			lastSaveTime = UtilsEx.getNowDateSeconds();
			string dataToString = GetDataToString();
			StreamWriter streamWriter = new StreamWriter(file_path, false);
			streamWriter.Write(dataToString);
			streamWriter.Flush();
			streamWriter.Close();
		}

		public string GetDataToString()
		{
			string text = DataToString();
			string text2 = text;
			string playerDataEncryptKey = MiscPlugin.GetPlayerDataEncryptKey("ME_2_@_YOU_DD");
			byte[] inArray = XXTEAUtils.Encrypt(Encoding.UTF8.GetBytes(text), Encoding.ASCII.GetBytes(playerDataEncryptKey));
			return Convert.ToBase64String(inArray);
		}

		public string DataToString()
		{
			string empty = string.Empty;
			string text = empty;
			empty = text + "LastSaveTime\t" + lastSaveTime + "\n";
			text = empty;
			empty = text + "SendGiftGoldDollor_1_3_2\t" + m_SendGiftGoldDollor_1_3_2 + "\n";
			empty = empty + "DeviceId\t" + DeviceID + "\n";
			empty = empty + "uuid\t" + UUID + "\n";
			empty = empty + "UserId\t" + UserID + "\n";
			empty = empty + "FacebookID\t" + FacebookID + "\n";
			empty = empty + "FacebookName\t" + FacebookName + "\n";
			empty = empty + "GameCenterID\t" + GameCenterID + "\n";
			empty = empty + "GameCenterName\t" + GameCenterName + "\n";
			text = empty;
			empty = text + "DailyBonusGetTime\t" + lastDailyBonusGetTime + "\n";
			text = empty;
			empty = text + "DailyBonusLevel\t" + lastDailyBonusLevel + "\n";
			text = empty;
			empty = text + "ExchangeUpgradeEndTime\t" + exchangeUpgradeEndTime + "\n";
			text = empty;
			empty = text + "Gift\t" + m_GetFirstGift + "\n";
			text = empty;
			empty = text + "Review\t" + m_ReviewCount + "\n";
			text = empty;
			empty = text + "BattleCount\t" + m_BattleCount + "\n";
			text = empty;
			empty = text + "Gold\t" + gold + "\n";
			text = empty;
			empty = text + "Dollor\t" + dollor + "\n";
			text = empty;
			empty = text + "Exp\t" + exp + "\n";
			text = empty;
			empty = text + "Level\t" + Level + "\n";
			text = empty;
			empty = text + "MusicOn\t" + (MusicOn ? 1 : 0) + "\n";
			text = empty;
			empty = text + "SoundOn\t" + (SoundOn ? 1 : 0) + "\n";
			if (weaponInfo.Count > 0)
			{
				empty += "WeaponList\t";
				List<WeaponType> weapons = GetWeapons();
				for (int i = 0; i < weapons.Count; i++)
				{
					empty += (int)weapons[i];
					if (i < weapons.Count - 1)
					{
						empty += ",";
					}
				}
				empty += "\t";
				for (int j = 0; j < battleWeapons.Count; j++)
				{
					empty += (int)battleWeapons[j];
					if (j < battleWeapons.Count - 1)
					{
						empty += ",";
					}
				}
				empty += "\n";
			}
			if (Avatars.Count > 0)
			{
				empty += "AvatarList\t";
				int num = 0;
				foreach (Avatar key in Avatars.Keys)
				{
					int num2 = (((bool)Avatars[key]) ? 1 : 0);
					text = empty;
					empty = text + (int)key.SuiteType + "," + (int)key.AvtType + "," + num2;
					if (num < Avatars.Keys.Count - 1)
					{
						empty += ";";
					}
					num++;
				}
				empty += "\n";
			}
			if (m_PowerUPS.Count > 0)
			{
				empty += "PowerUPSList\t";
				int num3 = 0;
				foreach (int key2 in m_PowerUPS.Keys)
				{
					text = empty;
					empty = text + key2 + "," + (int)m_PowerUPS[key2];
					if (num3 < m_PowerUPS.Keys.Count - 1)
					{
						empty += ";";
					}
					num3++;
				}
				empty += "\n";
			}
			if (m_GameCenterUnlockListInfo.Count > 0)
			{
				empty += "GCUnlockList\t";
				for (int k = 0; k < m_GameCenterUnlockListInfo.Count; k++)
				{
					empty += m_GameCenterUnlockListInfo[k];
					if (k < m_GameCenterUnlockListInfo.Count - 1)
					{
						empty += ";";
					}
				}
				empty += "\n";
			}
			text = empty;
			empty = text + "GCTotalBattleTime\t" + m_GCTotalBattleTime + "\n";
			empty = empty + "GCKilledAll\t" + m_GCKilledAllCount + "\n";
			empty = empty + "GCBoomerAttackTimes\t" + m_GCBoomerAttackTimes + "\n";
			empty = empty + "GCDeadTimes\t" + m_GCDeadTimes + "\n";
			empty = empty + "GCIapTimes\t" + m_GCIapTimes + "\n";
			empty = empty + "GCIapSpendDollor\t" + m_GCIapTotalDollor + "\n";
			empty = empty + "GCPlayWithFriendTimes\t" + m_GCPlayWithNetFriendTimes + "\n";
			empty = empty + "GCFriendPlayerDeadTimes\t" + m_GCFriendPlayerDeadTimes + "\n";
			if (m_MapsCDEndTime.Count > 0)
			{
				empty += "MapsCDTime\t";
				for (int l = 0; l < m_MapsCDEndTime.Count; l++)
				{
					empty += m_MapsCDEndTime[l];
					if (l < m_MapsCDEndTime.Count - 1)
					{
						empty += ",";
					}
				}
				empty += "\n";
			}
			empty = empty + "RoleIsLevelUp\t " + m_IsLevelUp + "\n";
			if (m_htBattleStar.Count > 0)
			{
				empty += "MapPointBattleStar\t";
				for (int m = 0; m < m_htBattleStar.Count; m++)
				{
					for (int n = 0; n < ((ArrayList)m_htBattleStar[m]).Count; n++)
					{
						string text2 = ((int)((ArrayList)m_htBattleStar[m])[n]).ToString();
						if (n < ((ArrayList)m_htBattleStar[m]).Count - 1)
						{
							text2 += ",";
						}
						empty += text2;
					}
					if (m < m_htBattleStar.Count - 1)
					{
						empty += "\t";
					}
				}
				empty += "\n";
			}
			text = empty;
			empty = text + "LastHireOutTime\t" + m_LastHireOutTime + "\n";
			if (m_HiredFriendsInfo.Count > 0)
			{
				empty += "HiredFriendsInfo\t";
				for (int num5 = 0; num5 < m_HiredFriendsInfo.Count; num5++)
				{
					text = empty;
					empty = text + m_HiredFriendsInfo[num5].Key.m_UUID + "," + m_HiredFriendsInfo[num5].Value;
					if (num5 < m_HiredFriendsInfo.Count - 1)
					{
						empty += ";";
					}
				}
				empty += "\n";
			}
			if (m_Skilles.Count > 0)
			{
				empty += "Skilles\t";
				for (int num6 = 0; num6 < m_Skilles.Count; num6++)
				{
					text = empty;
					empty = text + (int)m_Skilles[num6].SkillType + "," + m_Skilles[num6].Level;
					if (num6 < m_Skilles.Count - 1)
					{
						empty += ";";
					}
				}
				empty += "\n";
			}
			text = empty;
			empty = text + "CurSkillType\t" + (int)m_CurSkillType + "\n";
			empty = empty + "FirstLoginTime\t" + m_FirstLoginTime + "\n";
			text = empty;
			empty = text + "TotalExp\t" + m_TotalExp + "\n";
			text = empty;
			empty = text + "TotalGameTime\t" + m_TotalGameTime + "\n";
			text = empty;
			empty = text + "TotalGoldGet\t" + m_TotalGoldGet + "\n";
			text = empty;
			empty = text + "TotalDollorGet\t" + m_TotalDollorGet + "\n";
			if (ShareWithTwitter)
			{
				text = empty;
				empty = text + "ShareWithTwitter\t" + 1 + "\n";
			}
			else
			{
				text = empty;
				empty = text + "ShareWithTwitter\t" + 0 + "\n";
			}
			text = empty;
			empty = text + "CameraTypeInfo\t" + m_iCameraModeType + "\n";
			empty = empty + "NetworkName\t" + m_eGameMode.m_strNetName + "\n";
			text = empty;
			empty = text + "OwnedNoviceGiftBag\t" + NoviceGiftBag + "\n";
			text = empty;
			empty = text + "OwnedNoviceGiftBagSec\t" + NoviceGiftBagSec + "\n";
			text = empty;
			empty = text + "OwnedNoviceGiftBagThird\t" + NoviceGiftBagThird + "\n";
			if (m_eGameMode.m_PlaersNBattleBuff.Count > 0)
			{
				empty += "NBattleBuff\t";
				string text3 = string.Empty;
				int num7 = 0;
				foreach (KeyValuePair<enBattlefieldProps, int> item in m_eGameMode.m_PlaersNBattleBuff)
				{
					text = text3;
					text3 = text + (int)item.Key + "," + item.Value;
					if (num7 < m_eGameMode.m_PlaersNBattleBuff.Count - 1)
					{
						text3 += ";";
					}
					num7++;
				}
				empty = empty + text3 + "\n";
			}
			if (m_eGameMode.m_NBattleStatistics.Count > 0)
			{
				empty += "NBattleStatistics\t";
				string text4 = string.Empty;
				int num8 = 0;
				foreach (KeyValuePair<NetworkGameMode.NBattleStatistics, int> nBattleStatistic in m_eGameMode.m_NBattleStatistics)
				{
					text = text4;
					text4 = text + (int)nBattleStatistic.Key + "," + nBattleStatistic.Value;
					if (num8 < m_eGameMode.m_NBattleStatistics.Count - 1)
					{
						text4 += ";";
					}
					num8++;
				}
				empty = empty + text4 + "\n";
			}
			text = empty;
			empty = text + "PVPWinTimes\t" + m_eGameMode.m_CombatDataRecord.iPVPWinTimes + "\n";
			text = empty;
			empty = text + "PVPKillPlayerCount\t" + m_eGameMode.m_CombatDataRecord.iPVPKillPlayerCount + "\n";
			text = empty;
			empty = text + "PVPDoubleKillCount\t" + m_eGameMode.m_CombatDataRecord.iPVPDoubleKillCount + "\n";
			text = empty;
			empty = text + "PVPThreeKillCount\t" + m_eGameMode.m_CombatDataRecord.iPVPThreeKillCount + "\n";
			text = empty;
			empty = text + "PVPFourKillCount\t" + m_eGameMode.m_CombatDataRecord.iPVPFourKillCount + "\n";
			text = empty;
			empty = text + "PVPFiveKillCount\t" + m_eGameMode.m_CombatDataRecord.iPVPFiveKillCount + "\n";
			text = empty;
			empty = text + "PVPBestKillerCount\t" + m_eGameMode.m_CombatDataRecord.iPVPBestKillerCount + "\n";
			text = empty;
			empty = text + "WearShinobiToPlayTimes\t" + m_eGameMode.m_CombatDataRecord.iWearShinobiToPlayTimes + "\n";
			text = empty;
			empty = text + "FirstBloodCount\t" + m_eGameMode.m_CombatDataRecord.iFirstBloodCount + "\n";
			text = empty;
			empty = text + "PVPNoUseFastRunCount\t" + m_eGameMode.m_CombatDataRecord.iPVPNoUseFastRunCount + "\n";
			text = empty;
			empty = text + "PVPKillMoreTenPlayersOnceWar\t" + m_eGameMode.m_CombatDataRecord.iPVPKillMoreTenPlayersOnceWar + "\n";
			if (m_eGameMode.m_CombatDataRecord.bIsNoDeathToWinOneGame)
			{
				text = empty;
				empty = text + "IsNoHeartToWinOneGame\t" + 1 + "\n";
			}
			else
			{
				text = empty;
				empty = text + "IsNoHeartToWinOneGame\t" + 0 + "\n";
			}
			if (m_eGameMode.m_CombatDataRecord.lsBuyNBattleShop.Count > 0)
			{
				empty += "BuyNBattleShop\t";
				string text5 = string.Empty;
				int num9 = 0;
				foreach (enBattlefieldProps item2 in m_eGameMode.m_CombatDataRecord.lsBuyNBattleShop)
				{
					text5 += (int)item2;
					if (num9 < m_eGameMode.m_CombatDataRecord.lsBuyNBattleShop.Count - 1)
					{
						text5 += ",";
					}
					num9++;
				}
				empty = empty + text5 + "\n";
			}
			if (m_eGameMode.m_CombatDataRecord.lsWearAvatarTimes.Count > 0)
			{
				empty += "WearAvatarTimes\t";
				string text6 = string.Empty;
				int num10 = 0;
				foreach (Avatar.AvatarSuiteType lsWearAvatarTime in m_eGameMode.m_CombatDataRecord.lsWearAvatarTimes)
				{
					text6 += (int)lsWearAvatarTime;
					if (num10 < m_eGameMode.m_CombatDataRecord.lsWearAvatarTimes.Count - 1)
					{
						text6 += ",";
					}
					num10++;
				}
				empty = empty + text6 + "\n";
			}
			if (m_dictPropsAdditions.Count >= 0)
			{
				empty = empty + "PropsAdditionsList\t" + PropsAdditionsToString() + "\n";
			}
			text = empty;
			empty = text + "ShowFirstBuffPresentation\t" + m_eGameMode.m_CombatDataRecord.bShowFirstBuffPresentation + "\n";
			text = empty;
			return text + "OwnBuffMaxCount\t" + m_eGameMode.m_CombatDataRecord.iGetBuffCount + "\n";
		}

		public bool CheckNewVersionFeatureShow()
		{
			bool flag = false;
			string text = Utils.SavePath();
			string path = text + "/Version";
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
					Debug.Log("ERROR - GameState Load()!!!");
				}
				finally
				{
					if (streamReader != null)
					{
						streamReader.Close();
					}
				}
				if (text2 != "2.02")
				{
					flag = true;
				}
			}
			else
			{
				flag = true;
			}
			if (flag)
			{
				StreamWriter streamWriter = new StreamWriter(path, false);
				streamWriter.Write("2.02");
				streamWriter.Flush();
				streamWriter.Close();
			}
			return flag;
		}

		public void AddDailyCollectionInfo(int times, float iap_value, int tollGate, int playingTime)
		{
			int num = 0;
			float num2 = 0f;
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			int num6 = 0;
			int num7 = 0;
			string empty = string.Empty;
			string empty2 = string.Empty;
			string empty3 = string.Empty;
			string text = Utils.SavePath();
			string path = text + "/Misc";
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
					Debug.Log("ERROR - GameState AddDailyCollectionInfo()!!!");
				}
				finally
				{
					if (streamReader != null)
					{
						streamReader.Close();
					}
				}
				string[] array = text2.Split('\r', '\n');
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i] == null)
					{
						continue;
					}
					string[] array2 = array[i].Split('\t');
					if (array2.Length >= 2)
					{
						string text3 = array2[0];
						if (text3 == "times")
						{
							num = int.Parse(array2[1]);
						}
						else if (text3 == "iap")
						{
							num2 = float.Parse(array2[1]);
						}
					}
					string text4 = array2[0];
					if (text4 == "tollGate")
					{
						num3 = int.Parse(array2[1]);
					}
					if (text4 == "playingTime")
					{
						num5 = int.Parse(array2[1]);
					}
				}
			}
			num += times;
			num2 += iap_value;
			num3 += tollGate;
			num5 += playingTime;
			num4 = Level;
			num6 = gold;
			num7 = dollor;
			empty = weaponAccouter;
			empty2 = avatarAccouter;
			empty3 = powerUpsAccount;
			string empty4 = string.Empty;
			string text5 = empty4;
			empty4 = text5 + "times\t" + num + "\n";
			text5 = empty4;
			empty4 = text5 + "iap\t" + num2 + "\n";
			text5 = empty4;
			empty4 = text5 + "tollGate\t" + num3 + "\n";
			text5 = empty4;
			empty4 = text5 + "level\t" + num4 + "\n";
			text5 = empty4;
			empty4 = text5 + "playingTime\t" + num5 + "\n";
			text5 = empty4;
			empty4 = text5 + "gold\t" + num6 + "\n";
			text5 = empty4;
			empty4 = text5 + "dollar\t" + num7 + "\n";
			empty4 = empty4 + "weaponAccouter\t" + empty + "\n";
			empty4 = empty4 + "avatarAccouter\t" + empty2 + "\n";
			empty4 = empty4 + "powerUpsAccount\t" + empty3 + "\n";
			StreamWriter streamWriter = new StreamWriter(path, false);
			streamWriter.Write(empty4);
			streamWriter.Flush();
			streamWriter.Close();
		}

		public void GetDailyCollectionInfo(ref int times, ref float iap_value, ref int tollGate, ref int level, ref int playingTime, ref int gold, ref int dollar, ref string weaponAccouter, ref string avatarAccouter, ref string powerUpsAccount)
		{
			times = 0;
			iap_value = 0f;
			tollGate = 0;
			level = 0;
			playingTime = 0;
			gold = 0;
			dollar = 0;
			weaponAccouter = string.Empty;
			avatarAccouter = string.Empty;
			powerUpsAccount = string.Empty;
			string text = Utils.SavePath();
			string path = text + "/Misc";
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
					Debug.Log("ERROR - GameState GetDailyCollectionInfo()!!!");
				}
				finally
				{
					if (streamReader != null)
					{
						streamReader.Close();
					}
				}
				string[] array = text2.Split('\r', '\n');
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i] == null)
					{
						continue;
					}
					string[] array2 = array[i].Split('\t');
					if (array2.Length >= 2)
					{
						string text3 = array2[0];
						if (text3 == "times")
						{
							times = int.Parse(array2[1]);
						}
						else if (text3 == "iap")
						{
							iap_value = float.Parse(array2[1]);
						}
					}
					string text4 = array2[0];
					if (text4 == "tollGate")
					{
						tollGate = int.Parse(array2[1]);
					}
					if (text4 == "level")
					{
						level = int.Parse(array2[1]);
					}
					if (text4 == "playingTime")
					{
						playingTime = int.Parse(array2[1]);
					}
					if (text4 == "gold")
					{
						gold = int.Parse(array2[1]);
					}
					if (text4 == "dollar")
					{
						dollar = int.Parse(array2[1]);
					}
					if (text4 == "weaponAccouter")
					{
						weaponAccouter = array2[1];
					}
					if (text4 == "avatarAccouter")
					{
						avatarAccouter = array2[1];
					}
					if (text4 == "powerUpsAccount")
					{
						powerUpsAccount = array2[1];
					}
				}
			}
			string empty = string.Empty;
			string text5 = empty;
			empty = text5 + "times\t" + 0 + "\n";
			text5 = empty;
			empty = text5 + "iap\t" + 0 + "\n";
			text5 = empty;
			empty = text5 + "tollGate\t" + 0 + "\n";
			text5 = empty;
			empty = text5 + "level\t" + 0 + "\n";
			text5 = empty;
			empty = text5 + "playingTime\t" + 0 + "\n";
			text5 = empty;
			empty = text5 + "gold\t" + 0 + "\n";
			text5 = empty;
			empty = text5 + "dollar\t" + 0 + "\n";
			empty += "weaponAccouter\t\n";
			empty += "avatarAccouter\t\n";
			empty += "powerUpsAccount\t\n";
			StreamWriter streamWriter = new StreamWriter(path, false);
			streamWriter.Write(empty);
			streamWriter.Flush();
			streamWriter.Close();
		}

		public string Test123()
		{
			return string.Empty;
		}

		public void AddNewHiredFriend(FriendUserData friend_data, long time)
		{
			if (friend_data.m_UUID.Trim() != string.Empty)
			{
				AddHiredFriend(friend_data, time);
				GameApp.GetInstance().Save();
				string value = friend_data.DataToString();
				string text = Utils.SavePath();
				string path = text + "/" + friend_data.m_UUID;
				StreamWriter streamWriter = new StreamWriter(path, false);
				streamWriter.Write(value);
				streamWriter.Flush();
				streamWriter.Close();
			}
			else
			{
				Debug.Log("Error: GameState.AddNewHiredFriend() - UUID is NULL !!!!!!!");
			}
		}

		public void GetHiredFriendsData()
		{
			List<KeyValuePair<FriendUserData, long>> list = new List<KeyValuePair<FriendUserData, long>>();
			for (int i = 0; i < m_HiredFriendsInfo.Count; i++)
			{
				string uUID = m_HiredFriendsInfo[i].Key.m_UUID;
				string text = Utils.SavePath();
				string path = text + "/" + uUID;
				if (!File.Exists(path))
				{
					continue;
				}
				string text2 = string.Empty;
				StreamReader streamReader = null;
				try
				{
					streamReader = new StreamReader(path);
					text2 = streamReader.ReadToEnd();
				}
				catch
				{
					Debug.Log("ERROR - GameState.GetHiredFriendsData()!!!");
				}
				finally
				{
					if (streamReader != null)
					{
						streamReader.Close();
					}
				}
				if (text2.Length > 1)
				{
					m_HiredFriendsInfo[i].Key.LoadFriendUserData(text2);
					list.Add(new KeyValuePair<FriendUserData, long>(m_HiredFriendsInfo[i].Key, m_HiredFriendsInfo[i].Value));
				}
			}
			m_HiredFriendsInfo = list;
		}

		public void CheckHiredFriendsTime()
		{
			List<KeyValuePair<FriendUserData, long>> list = new List<KeyValuePair<FriendUserData, long>>();
			long nowDateSeconds = UtilsEx.getNowDateSeconds();
			for (int i = 0; i < m_HiredFriendsInfo.Count; i++)
			{
				if (nowDateSeconds - m_HiredFriendsInfo[i].Value < 86400)
				{
					list.Add(m_HiredFriendsInfo[i]);
					continue;
				}
				string text = Utils.SavePath();
				string path = text + "/" + m_HiredFriendsInfo[i].Key.m_UUID;
				if (File.Exists(path))
				{
					File.Delete(path);
				}
			}
			m_HiredFriendsInfo = list;
		}

		public bool GetNeedShowLevelupAnimation()
		{
			if (m_IsLevelUp == 1)
			{
				return true;
			}
			if (m_IsLevelUp == 0)
			{
				return false;
			}
			return false;
		}

		public void SetNeedShowLevelupAnimation(int isNeed)
		{
			if (isNeed == 0 || isNeed == 1)
			{
				m_IsLevelUp = isNeed;
			}
			else
			{
				Debug.Log("SetNeedShowLevelupAnimation(int) is wrong parameter!!!");
			}
		}

		public float CalcMaxHp()
		{
			float num = 100f;
			float num2 = GetLevelHpAffect(Level);
			float num3 = 0f;
			foreach (Avatar key in Avatars.Keys)
			{
				if (key == null || !(bool)Avatars[key])
				{
					continue;
				}
				ArrayList avatarCfgs = ConfigManager.Instance().GetFixedConfig().avatarCfgs;
				for (int i = 0; i < avatarCfgs.Count; i++)
				{
					FixedConfig.AvatarCfg avatarCfg = (FixedConfig.AvatarCfg)avatarCfgs[i];
					if (key.SuiteType == avatarCfg.suiteType && key.AvtType == avatarCfg.avtType)
					{
						num3 += Mathf.Clamp01(avatarCfg.prop.m_HpAdditive);
					}
				}
			}
			return (num + num2) * (1f + Mathf.Clamp01(num3));
		}

		public float CalcMaxStamina()
		{
			float num = 100f;
			foreach (Avatar key in Avatars.Keys)
			{
				if (key == null || !(bool)Avatars[key])
				{
					continue;
				}
				ArrayList avatarCfgs = ConfigManager.Instance().GetFixedConfig().avatarCfgs;
				for (int i = 0; i < avatarCfgs.Count; i++)
				{
					FixedConfig.AvatarCfg avatarCfg = (FixedConfig.AvatarCfg)avatarCfgs[i];
					if (key.SuiteType == avatarCfg.suiteType && key.AvtType == avatarCfg.avtType)
					{
						num += avatarCfg.prop.m_StaminaAdd;
					}
				}
			}
			if (m_SelectFriendIndex == 3)
			{
				num += 5f;
			}
			return num;
		}

		public int GetLevelHpAffect(int player_level)
		{
			int result = 0;
			if (player_level < 26)
			{
				result = player_level * 4;
			}
			else if (player_level >= 26 && player_level < 61)
			{
				result = 100 + (player_level - 25) * 3;
			}
			else if (player_level >= 61 && player_level < 100)
			{
				result = 172 + (player_level - 60) * 3;
			}
			else if (player_level >= 100 && player_level <= 255)
			{
				result = 292 + (player_level - 99) * 1;
			}
			return result;
		}

		public int GetBattleStar()
		{
			if (m_BattleStar >= 0)
			{
				return m_BattleStar;
			}
			Debug.Log("Star Is Wrong Is Less Then Zero");
			return 0;
		}

		public void ResetBattleStar()
		{
			m_BattleStar = 0;
			m_IsFastPassBattle = false;
			m_IsNoBruisePassBattle = false;
		}

		public void AddBattleStar()
		{
			m_BattleStar++;
		}

		public int GetBattleStarByMapPointIndex(int mapIndex, int pointIndex)
		{
			mapIndex--;
			if (mapIndex + 1 > m_htBattleStar.Count)
			{
				return -1;
			}
			if (pointIndex > ((ArrayList)m_htBattleStar[mapIndex]).Count)
			{
				return -1;
			}
			ArrayList arrayList = (ArrayList)m_htBattleStar[mapIndex];
			return (int)arrayList[pointIndex];
		}

		public void ChangeBattleStar(int mapIndex, int pointIndex, int StarNum)
		{
			Debug.LogWarning("mapIndex:" + mapIndex + "|||  pointIndex:" + pointIndex + " ||| StarNum:" + StarNum);
			if (m_htBattleStar.Count <= 0 || StarNum <= -1 || StarNum > 3 || mapIndex > m_htBattleStar.Count || pointIndex > ((ArrayList)m_htBattleStar[mapIndex - 1]).Count)
			{
				return;
			}
			int battleStarByMapPointIndex = GetBattleStarByMapPointIndex(mapIndex, pointIndex - 1);
			mapIndex--;
			pointIndex--;
			ArrayList arrayList = (ArrayList)m_htBattleStar[mapIndex];
			if (StarNum == 0)
			{
				Debug.LogWarning("[[[Unlock Array Index:]]]mapIndex:" + mapIndex + "|||  pointIndex:" + pointIndex + " ||| StarNum:" + StarNum);
				if ((int)arrayList[pointIndex] == -1)
				{
					arrayList[pointIndex] = 0;
				}
			}
			else if (StarNum > battleStarByMapPointIndex)
			{
				arrayList[pointIndex] = StarNum;
			}
			else
			{
				Debug.Log("Star ago is big to this!!!!");
			}
		}

		public void InitPlayerStatistics(int id, string name, int groupID, int AvatarHeadID)
		{
			NetworkGameMode.NetworkPlayerStatistics networkPlayerStatistics = new NetworkGameMode.NetworkPlayerStatistics();
			networkPlayerStatistics.m_strName = name;
			networkPlayerStatistics.m_iNGroup = groupID;
			networkPlayerStatistics.m_iHeadAvatarID = AvatarHeadID;
			networkPlayerStatistics.m_bIsOline = true;
			m_eGameMode.m_PlaersStatistics.Add(id, networkPlayerStatistics);
		}

		public NetworkGameMode.NetworkPlayerStatistics GetPlayerStatistics(int id)
		{
			if (m_eGameMode.m_PlaersStatistics.ContainsKey(id))
			{
				return m_eGameMode.m_PlaersStatistics[id];
			}
			return null;
		}

		public void ChangeNPlayerStatistics(int id, int killCount, int deathCount)
		{
			if (killCount != -1 && GetPlayerStatistics(id) != null)
			{
				GetPlayerStatistics(id).m_iKillCount = killCount;
			}
			if (deathCount != -1 && GetPlayerStatistics(id) != null)
			{
				GetPlayerStatistics(id).m_iDeathCount = deathCount;
			}
		}

		public string GetNName(string name)
		{
			string empty = string.Empty;
			string[] array = name.Split(new string[1] { "[Ma!cA@d#dres]" }, StringSplitOptions.RemoveEmptyEntries);
			return array[0];
		}

		public string[] GetNDeathNameInfo(string name)
		{
			return name.Split(new string[1] { "[De!ath#Msg%]" }, StringSplitOptions.RemoveEmptyEntries);
		}

		public int GetNBattleStatistics(NetworkGameMode.NBattleStatistics _key)
		{
			if (m_eGameMode.m_NBattleStatistics.ContainsKey(_key))
			{
				return m_eGameMode.m_NBattleStatistics[_key];
			}
			m_eGameMode.m_NBattleStatistics.Add(_key, 0);
			GameApp.GetInstance().Save();
			return 0;
		}

		public void AddNBattleStatisticsOnce(NetworkGameMode.NBattleStatistics _key)
		{
			if (m_eGameMode.m_NBattleStatistics.ContainsKey(_key))
			{
				Dictionary<NetworkGameMode.NBattleStatistics, int> nBattleStatistics;
				Dictionary<NetworkGameMode.NBattleStatistics, int> dictionary = (nBattleStatistics = m_eGameMode.m_NBattleStatistics);
				NetworkGameMode.NBattleStatistics key;
				NetworkGameMode.NBattleStatistics key2 = (key = _key);
				int num = nBattleStatistics[key];
				dictionary[key2] = num + 1;
				GameApp.GetInstance().Save();
			}
			else
			{
				Debug.Log("No this key");
			}
		}

		public void AddAchievementUI(string text)
		{
			if (text != string.Empty)
			{
				m_lsGameCenterMsg.Add(text);
			}
		}

		public void AchievementUI(string text)
		{
			if (text != string.Empty)
			{
				SceneUIManager.Instance().SetupGameCenterUnlockUI(text);
			}
		}

		public void AddPVPWinTimes(int count = 1)
		{
			m_eGameMode.m_CombatDataRecord.iPVPWinTimes += count;
			GameApp.GetInstance().Save();
			int num = 1;
			if (m_eGameMode.m_CombatDataRecord.iPVPWinTimes >= num && IsGCArchievementLocked(36))
			{
				UnlockGCArchievement(36, "com.trinitigame.callofminibulletdudes.a37");
				AddAchievementUI("+1 tCrystals");
				AddDollor(1);
			}
			num = 10;
			if (m_eGameMode.m_CombatDataRecord.iPVPWinTimes >= num)
			{
				if (IsGCArchievementLocked(37))
				{
					UnlockGCArchievement(37, "com.trinitigame.callofminibulletdudes.a38");
					AddAchievementUI("+1 tCrystals");
					AddDollor(1);
				}
			}
			else
			{
				UnlockGCArchievement(37, "com.trinitigame.callofminibulletdudes.a38", m_eGameMode.m_CombatDataRecord.iPVPWinTimes * 100 / num);
			}
			num = 50;
			Debug.LogWarning("Unlock GCArchievement39|" + m_eGameMode.m_CombatDataRecord.iPVPWinTimes + "/" + num);
			if (m_eGameMode.m_CombatDataRecord.iPVPWinTimes >= num)
			{
				if (IsGCArchievementLocked(38))
				{
					UnlockGCArchievement(38, "com.trinitigame.callofminibulletdudes.a39");
					AddAchievementUI("+1 tCrystals");
					AddDollor(1);
				}
			}
			else
			{
				UnlockGCArchievement(38, "com.trinitigame.callofminibulletdudes.a39", m_eGameMode.m_CombatDataRecord.iPVPWinTimes * 100 / num);
			}
			num = 200;
			if (m_eGameMode.m_CombatDataRecord.iPVPWinTimes >= num)
			{
				if (IsGCArchievementLocked(39))
				{
					UnlockGCArchievement(39, "com.trinitigame.callofminibulletdudes.a40");
					AddAchievementUI("+2 tCrystals");
					AddDollor(2);
				}
			}
			else
			{
				UnlockGCArchievement(39, "com.trinitigame.callofminibulletdudes.a40", m_eGameMode.m_CombatDataRecord.iPVPWinTimes * 100 / num);
			}
			num = 500;
			if (m_eGameMode.m_CombatDataRecord.iPVPWinTimes >= num)
			{
				if (IsGCArchievementLocked(40))
				{
					UnlockGCArchievement(40, "com.trinitigame.callofminibulletdudes.a41");
					AddAchievementUI("+3 tCrystals");
					AddDollor(3);
				}
			}
			else
			{
				UnlockGCArchievement(40, "com.trinitigame.callofminibulletdudes.a41", m_eGameMode.m_CombatDataRecord.iPVPWinTimes * 100 / num);
			}
		}

		public void AddPVPKillPlayerCount(int count = 1)
		{
			m_eGameMode.m_CombatDataRecord.iPVPKillPlayerCount += count;
			GameApp.GetInstance().Save();
			int num = 100;
			if (m_eGameMode.m_CombatDataRecord.iPVPKillPlayerCount >= num)
			{
				if (IsGCArchievementLocked(41))
				{
					UnlockGCArchievement(41, "com.trinitigame.callofminibulletdudes.a42");
					AddAchievementUI("+1 tCrystals");
					AddDollor(1);
				}
			}
			else
			{
				UnlockGCArchievement(41, "com.trinitigame.callofminibulletdudes.a42", m_eGameMode.m_CombatDataRecord.iPVPKillPlayerCount * 100 / num);
			}
			num = 500;
			if (m_eGameMode.m_CombatDataRecord.iPVPKillPlayerCount >= num)
			{
				if (IsGCArchievementLocked(42))
				{
					UnlockGCArchievement(42, "com.trinitigame.callofminibulletdudes.a43");
					AddAchievementUI("+1 tCrystals");
					AddDollor(1);
				}
			}
			else
			{
				UnlockGCArchievement(42, "com.trinitigame.callofminibulletdudes.a43", m_eGameMode.m_CombatDataRecord.iPVPKillPlayerCount * 100 / num);
			}
			num = 2000;
			if (m_eGameMode.m_CombatDataRecord.iPVPKillPlayerCount >= num)
			{
				if (IsGCArchievementLocked(43))
				{
					UnlockGCArchievement(43, "com.trinitigame.callofminibulletdudes.a44");
					AddAchievementUI("+1 tCrystals");
					AddDollor(1);
				}
			}
			else
			{
				UnlockGCArchievement(43, "com.trinitigame.callofminibulletdudes.a44", m_eGameMode.m_CombatDataRecord.iPVPKillPlayerCount * 100 / num);
			}
			num = 5000;
			if (m_eGameMode.m_CombatDataRecord.iPVPKillPlayerCount >= num)
			{
				if (IsGCArchievementLocked(44))
				{
					UnlockGCArchievement(44, "com.trinitigame.callofminibulletdudes.a45");
					AddAchievementUI("+2 tCrystals");
					AddDollor(2);
				}
			}
			else
			{
				UnlockGCArchievement(44, "com.trinitigame.callofminibulletdudes.a45", m_eGameMode.m_CombatDataRecord.iPVPKillPlayerCount * 100 / num);
			}
			num = 10000;
			if (m_eGameMode.m_CombatDataRecord.iPVPKillPlayerCount >= num)
			{
				if (IsGCArchievementLocked(45))
				{
					UnlockGCArchievement(45, "com.trinitigame.callofminibulletdudes.a46");
					AddAchievementUI("+3 tCrystals");
					AddDollor(3);
				}
			}
			else
			{
				UnlockGCArchievement(45, "com.trinitigame.callofminibulletdudes.a46", m_eGameMode.m_CombatDataRecord.iPVPKillPlayerCount * 100 / num);
			}
		}

		public void AddPVPDoubleKillCount(int count = 1)
		{
			m_eGameMode.m_CombatDataRecord.iPVPDoubleKillCount += count;
			GameApp.GetInstance().Save();
			int num = 100;
			if (m_eGameMode.m_CombatDataRecord.iPVPDoubleKillCount >= num)
			{
				if (IsGCArchievementLocked(46))
				{
					UnlockGCArchievement(46, "com.trinitigame.callofminibulletdudes.a47");
					AddAchievementUI("+1 tCrystals");
					AddDollor(1);
				}
			}
			else
			{
				UnlockGCArchievement(46, "com.trinitigame.callofminibulletdudes.a47", m_eGameMode.m_CombatDataRecord.iPVPDoubleKillCount * 100 / num);
			}
		}

		public void AddPVPThreeKillCount(int count = 1)
		{
			m_eGameMode.m_CombatDataRecord.iPVPThreeKillCount += count;
			GameApp.GetInstance().Save();
			int num = 100;
			if (m_eGameMode.m_CombatDataRecord.iPVPThreeKillCount >= num)
			{
				if (IsGCArchievementLocked(47))
				{
					UnlockGCArchievement(47, "com.trinitigame.callofminibulletdudes.a48");
					AddAchievementUI("+1 tCrystals");
					AddDollor(1);
				}
			}
			else
			{
				UnlockGCArchievement(47, "com.trinitigame.callofminibulletdudes.a48", m_eGameMode.m_CombatDataRecord.iPVPThreeKillCount * 100 / num);
			}
		}

		public void AddPVPFourKillCount(int count = 1)
		{
			m_eGameMode.m_CombatDataRecord.iPVPFourKillCount += count;
			GameApp.GetInstance().Save();
			int num = 100;
			if (m_eGameMode.m_CombatDataRecord.iPVPFourKillCount >= num)
			{
				if (IsGCArchievementLocked(48))
				{
					UnlockGCArchievement(48, "com.trinitigame.callofminibulletdudes.a49");
					AddAchievementUI("+2 tCrystals");
					AddDollor(2);
				}
			}
			else
			{
				UnlockGCArchievement(48, "com.trinitigame.callofminibulletdudes.a49", m_eGameMode.m_CombatDataRecord.iPVPFourKillCount * 100 / num);
			}
		}

		public void AddPVPFiveKillCount(int count = 1)
		{
			m_eGameMode.m_CombatDataRecord.iPVPFiveKillCount += count;
			GameApp.GetInstance().Save();
			int num = 100;
			if (m_eGameMode.m_CombatDataRecord.iPVPFiveKillCount >= num)
			{
				if (IsGCArchievementLocked(49))
				{
					UnlockGCArchievement(49, "com.trinitigame.callofminibulletdudes.a50");
					AddAchievementUI("+3 tCrystals");
					AddDollor(3);
				}
			}
			else
			{
				UnlockGCArchievement(49, "com.trinitigame.callofminibulletdudes.a50", m_eGameMode.m_CombatDataRecord.iPVPFiveKillCount * 100 / num);
			}
		}

		public void AddPVPBestKillerCount(int count = 1)
		{
			m_eGameMode.m_CombatDataRecord.iPVPBestKillerCount += count;
			GameApp.GetInstance().Save();
			int num = 1;
			if (m_eGameMode.m_CombatDataRecord.iPVPBestKillerCount >= num && IsGCArchievementLocked(56))
			{
				UnlockGCArchievement(56, "com.trinitigame.callofminibulletdudes.a57");
				AddAchievementUI("+1 tCrystals");
				AddDollor(1);
			}
		}

		public void AddWearShinobiToPlayTimes(int count = 1)
		{
			m_eGameMode.m_CombatDataRecord.iWearShinobiToPlayTimes += count;
			GameApp.GetInstance().Save();
			int num = 10;
			if (m_eGameMode.m_CombatDataRecord.iWearShinobiToPlayTimes >= num)
			{
				if (IsGCArchievementLocked(58))
				{
					UnlockGCArchievement(58, "com.trinitigame.callofminibulletdudes.a59");
					AddAchievementUI("+1 tCrystals");
					AddDollor(1);
				}
			}
			else
			{
				UnlockGCArchievement(58, "com.trinitigame.callofminibulletdudes.a59", m_eGameMode.m_CombatDataRecord.iWearShinobiToPlayTimes * 100 / num);
			}
		}

		public void AddFirstBloodCount(int count = 1)
		{
			m_eGameMode.m_CombatDataRecord.iFirstBloodCount += count;
			int num = 50;
			if (m_eGameMode.m_CombatDataRecord.iFirstBloodCount >= num)
			{
				if (IsGCArchievementLocked(59))
				{
					UnlockGCArchievement(59, "com.trinitigame.callofminibulletdudes.a60");
					AddAchievementUI("+1 tCrystals");
				}
			}
			else
			{
				UnlockGCArchievement(59, "com.trinitigame.callofminibulletdudes.a60", m_eGameMode.m_CombatDataRecord.iFirstBloodCount * 100 / num);
			}
		}

		public void AddNoUseBestRunWinGameCount(int count = 1)
		{
			m_eGameMode.m_CombatDataRecord.iPVPNoUseFastRunCount += count;
			GameApp.GetInstance().Save();
			int num = 10;
			Debug.LogWarning("Unlock GCArchievement55|" + m_eGameMode.m_CombatDataRecord.iPVPNoUseFastRunCount + "/" + num);
			if (m_eGameMode.m_CombatDataRecord.iPVPNoUseFastRunCount >= num)
			{
				if (IsGCArchievementLocked(54))
				{
					UnlockGCArchievement(54, "com.trinitigame.callofminibulletdudes.a55");
					AddAchievementUI("+1 tCrystals");
					AddDollor(1);
				}
			}
			else
			{
				UnlockGCArchievement(54, "com.trinitigame.callofminibulletdudes.a55", m_eGameMode.m_CombatDataRecord.iPVPNoUseFastRunCount * 100 / num);
			}
		}

		public void AddKillMoreTenPlayersOnceWar(int count = 1)
		{
			m_eGameMode.m_CombatDataRecord.iPVPKillMoreTenPlayersOnceWar += count;
			GameApp.GetInstance().Save();
			int num = 5;
			if (m_eGameMode.m_CombatDataRecord.iPVPKillMoreTenPlayersOnceWar >= num)
			{
				if (IsGCArchievementLocked(59))
				{
					UnlockGCArchievement(59, "com.trinitigame.callofminibulletdudes.a60");
					AddAchievementUI("+1 tCrystals");
					AddDollor(1);
				}
			}
			else
			{
				UnlockGCArchievement(59, "com.trinitigame.callofminibulletdudes.a60", m_eGameMode.m_CombatDataRecord.iPVPKillMoreTenPlayersOnceWar * 100 / num);
			}
		}

		public void CompleteNoHeartToWinOneGame(bool bComplete = true)
		{
			if (!m_eGameMode.m_CombatDataRecord.bIsNoDeathToWinOneGame && IsGCArchievementLocked(51))
			{
				UnlockGCArchievement(51, "com.trinitigame.callofminibulletdudes.a52");
				m_eGameMode.m_CombatDataRecord.bIsNoDeathToWinOneGame = true;
				AddAchievementUI("+3 tCrystals");
				AddDollor(3);
			}
		}

		public void WearDifferentAvatarTimes(Avatar.AvatarSuiteType ava)
		{
			if (m_eGameMode.m_CombatDataRecord.lsWearAvatarTimes.Contains(ava))
			{
				return;
			}
			m_eGameMode.m_CombatDataRecord.lsWearAvatarTimes.Add(ava);
			GameApp.GetInstance().Save();
			int num = 5;
			if (m_eGameMode.m_CombatDataRecord.lsWearAvatarTimes.Count >= num)
			{
				if (IsGCArchievementLocked(57))
				{
					UnlockGCArchievement(57, "com.trinitigame.callofminibulletdudes.a58");
					AddAchievementUI("+1 tCrystals");
					AddDollor(1);
				}
			}
			else
			{
				UnlockGCArchievement(57, "com.trinitigame.callofminibulletdudes.a58", m_eGameMode.m_CombatDataRecord.lsWearAvatarTimes.Count * 100 / num);
			}
		}

		public void BuyNbattleBuff(enBattlefieldProps enPor)
		{
			if (m_eGameMode.m_CombatDataRecord.lsBuyNBattleShop.Contains(enPor))
			{
				return;
			}
			m_eGameMode.m_CombatDataRecord.lsBuyNBattleShop.Add(enPor);
			GameApp.GetInstance().Save();
			int num = 5;
			if (m_eGameMode.m_CombatDataRecord.lsBuyNBattleShop.Count >= num)
			{
				if (IsGCArchievementLocked(53))
				{
					UnlockGCArchievement(53, "com.trinitigame.callofminibulletdudes.a54");
					AddAchievementUI("+1 tCrystals");
					AddDollor(1);
				}
			}
			else
			{
				UnlockGCArchievement(53, "com.trinitigame.callofminibulletdudes.a54", m_eGameMode.m_CombatDataRecord.lsBuyNBattleShop.Count * 100 / num);
			}
		}

		public void AddPropsAddition(int id, enPropsAdditionType type, enPropsAdditionPart part)
		{
			if (GetPropsAddition(id) != null)
			{
				m_dictPropsAdditions.Remove(id);
			}
			PropsAdditionImpl propsAdditionImpl = new PropsAdditionImpl();
			propsAdditionImpl.Init(new PropsAddition(type, part, 1u));
			m_dictPropsAdditions.Add(id, propsAdditionImpl);
			AddGetBuffCount();
			GameApp.GetInstance().Save();
		}

		public void AddPropsAddition(int id, enPropsAdditionType type, enPropsAdditionPart part, uint level, long beginTime, long maxTime)
		{
			if (GetPropsAddition(id) != null)
			{
				m_dictPropsAdditions.Remove(id);
			}
			PropsAdditionImpl propsAdditionImpl = new PropsAdditionImpl();
			propsAdditionImpl.Init(new PropsAddition(type, part, level), beginTime, maxTime);
			m_dictPropsAdditions.Add(id, propsAdditionImpl);
			AddGetBuffCount();
			GameApp.GetInstance().Save();
		}

		public PropsAdditionImpl GetPropsAddition(int id)
		{
			if (m_dictPropsAdditions.ContainsKey(id))
			{
				return m_dictPropsAdditions[id];
			}
			return null;
		}

		public PropsAdditionImpl CheckAgeing(int id, enPropsAdditionType type, enPropsAdditionPart part)
		{
			PropsAdditionImpl propsAddition = GetPropsAddition(id);
			if (propsAddition != null)
			{
				if (propsAddition.GetPropsAddition().PropsType == type && propsAddition.GetPropsAddition().PropsPart == part)
				{
					if (propsAddition.CheckAgeing())
					{
						return propsAddition;
					}
					m_dictPropsAdditions.Remove(id);
					return null;
				}
				return null;
			}
			return null;
		}

		public string PropsAdditionsToString()
		{
			string text = string.Empty;
			int num = 0;
			foreach (KeyValuePair<int, PropsAdditionImpl> dictPropsAddition in m_dictPropsAdditions)
			{
				string text2 = text;
				text = text2 + dictPropsAddition.Key + "," + dictPropsAddition.Value.ToString();
				if (num < m_dictPropsAdditions.Count - 1)
				{
					text += ";";
				}
				num++;
			}
			return text;
		}

		public void PropsAdditionsFromString(string str)
		{
			string[] array = str.Split(';');
			for (int i = 0; i < array.Length; i++)
			{
				string[] array2 = array[i].Split(',');
				if (array2.Length >= 6)
				{
					int id = int.Parse(array2[0]);
					enPropsAdditionType type = (enPropsAdditionType)int.Parse(array2[1]);
					enPropsAdditionPart part = (enPropsAdditionPart)int.Parse(array2[2]);
					uint level = uint.Parse(array2[3]);
					long beginTime = long.Parse(array2[4]);
					long maxTime = long.Parse(array2[5]);
					AddPropsAddition(id, type, part, level, beginTime, maxTime);
				}
			}
		}

		public int SetPropsAdditionsID(int id, int type)
		{
			int result = 0;
			switch (type)
			{
			case 0:
				result = id;
				break;
			case 1:
				result = id + 200;
				break;
			}
			if (type == 2)
			{
				result = id + 400;
			}
			return result;
		}

		public KeyValuePair<int, int> GetWorAIDByPropsID(int id)
		{
			int num = -1;
			int num2 = -1;
			if (id >= 0 && id < 200)
			{
				num = 0;
				num2 = id;
			}
			else if (id >= 200 && id < 400)
			{
				num = 1;
				num2 = id - 200;
			}
			else if (id >= 400)
			{
				num = 2;
				num2 = id - 400;
			}
			else
			{
				num = -1;
				num2 = -1;
			}
			return new KeyValuePair<int, int>(num, num2);
		}

		public List<FixedConfig.WeaponCfg> GetListClassWeapons(int classID)
		{
			List<FixedConfig.WeaponCfg> list = new List<FixedConfig.WeaponCfg>();
			ArrayList weapons = ConfigManager.Instance().GetFixedConfig().weapons;
			for (int i = 0; i < weapons.Count; i++)
			{
				FixedConfig.WeaponCfg weaponCfg = (FixedConfig.WeaponCfg)weapons[i];
				if (weaponCfg.mClass == classID)
				{
					list.Add(weaponCfg);
				}
			}
			return list;
		}

		public List<FixedConfig.AvatarCfg> GetListClassAvatars(int classID)
		{
			List<FixedConfig.AvatarCfg> list = new List<FixedConfig.AvatarCfg>();
			ArrayList avatarCfgs = ConfigManager.Instance().GetFixedConfig().avatarCfgs;
			for (int i = 0; i < avatarCfgs.Count; i++)
			{
				FixedConfig.AvatarCfg avatarCfg = (FixedConfig.AvatarCfg)avatarCfgs[i];
				if (avatarCfg.m_Class == classID)
				{
					list.Add(avatarCfg);
				}
			}
			return list;
		}

		public int GiveWeaponPropsAddition(int classID, bool bGive = true)
		{
			List<FixedConfig.WeaponCfg> listClassWeapons = GetListClassWeapons(classID);
			int index = UnityEngine.Random.Range(0, listClassWeapons.Count);
			FixedConfig.WeaponCfg weaponCfg = listClassWeapons[index];
			int num = SetPropsAdditionsID(weaponCfg.type, 2);
			if (bGive)
			{
				AddPropsAddition(num, enPropsAdditionType.E_Damage, enPropsAdditionPart.E_Weapon);
			}
			return num;
		}

		public int GiveAvatarPropsAddition(int classID, bool bGive = true)
		{
			List<FixedConfig.AvatarCfg> listClassAvatars = GetListClassAvatars(classID);
			int index = UnityEngine.Random.Range(0, listClassAvatars.Count);
			FixedConfig.AvatarCfg avatarCfg = listClassAvatars[index];
			int num = SetPropsAdditionsID((int)avatarCfg.suiteType, (int)avatarCfg.avtType);
			List<enPropsAdditionType> list = new List<enPropsAdditionType>();
			if (avatarCfg.prop.m_AttackAdditive > 0f)
			{
				list.Add(enPropsAdditionType.E_AttackAdditive);
			}
			else if (avatarCfg.prop.m_DefenceAdditive > 0f)
			{
				list.Add(enPropsAdditionType.E_DefenceAdditive);
			}
			else if (avatarCfg.prop.m_SpeedAdditive != 0f)
			{
				list.Add(enPropsAdditionType.E_SpeedAdditive);
			}
			else if (avatarCfg.prop.m_HpAdditive > 0f)
			{
				list.Add(enPropsAdditionType.E_HpAdditive);
			}
			else if (avatarCfg.prop.m_AttackSpeedAdditive > 0f)
			{
				list.Add(enPropsAdditionType.E_AttackSpeedAdditive);
			}
			else if (avatarCfg.prop.m_StaminaAdd > 0f)
			{
				list.Add(enPropsAdditionType.E_StaminaAdditive);
			}
			else if (avatarCfg.prop.m_ExpAdditive > 0f)
			{
				list.Add(enPropsAdditionType.E_ExpAdditive);
			}
			else if (avatarCfg.prop.m_GoldAdditive > 0f)
			{
				list.Add(enPropsAdditionType.E_CashAdditive);
			}
			enPropsAdditionType type = list[UnityEngine.Random.Range(0, list.Count)];
			if (bGive)
			{
				AddPropsAddition(num, type, (enPropsAdditionPart)avatarCfg.avtType);
			}
			return num;
		}

		public void AddMineDamage(int damage)
		{
			if (!(GameSetup.Instance == null) && GetPlayerStatistics(GameSetup.Instance.MineUser.Id) != null)
			{
				GetPlayerStatistics(GameSetup.Instance.MineUser.Id).m_iMyDamage += damage;
			}
		}

		public void AddGetBuffCount(int count = 1)
		{
			m_eGameMode.m_CombatDataRecord.iGetBuffCount += count;
		}

		public int GetOwnBuffCount()
		{
			return m_eGameMode.m_CombatDataRecord.iGetBuffCount;
		}

		public bool CanShowBuffPresentation()
		{
			if (m_eGameMode.m_CombatDataRecord.bShowFirstBuffPresentation <= 0 && m_eGameMode.m_CombatDataRecord.iGetBuffCount > 0)
			{
				return true;
			}
			return false;
		}

		public void ShowBuffPresentationOK()
		{
			m_eGameMode.m_CombatDataRecord.bShowFirstBuffPresentation = 1;
		}

		public List<int> CheckIAPSales(List<IAPSalesClass> _lsIAPS)
		{
			bool flag = false;
			if (_lsIAPS.Count > 0)
			{
				flag = true;
			}
			List<int> list = new List<int>();
			if (flag)
			{
				foreach (IAPSalesClass _lsIAP in _lsIAPS)
				{
					int num = ConfigManager.Instance().GetFixedConfig().FindIAPConfigData(_lsIAP._iapID);
					if (num >= 0)
					{
						list.Add(num);
						ConfigManager.Instance().GetFixedConfig().RemoveIAPConfigData(num);
						ConfigManager.Instance().GetFixedConfig().AddIAPConfigData(_lsIAP._iapCfg, num);
					}
				}
				return list;
			}
			return list;
		}
	}
}
