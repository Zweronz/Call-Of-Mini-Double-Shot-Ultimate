using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zombie3D
{
	public class Player
	{
		public static PlayerState IDLE_STATE = new PlayerIdleState();

		public static PlayerState RUN_STATE = new PlayerRunState();

		public static PlayerState SHOOT_STATE = new PlayerShootState();

		public static PlayerState RUNSHOOT_STATE = new PlayerRunAndShootState();

		public static PlayerState GOTHIT_STATE = new PlayerGotHitState();

		public static PlayerState HITBACK_STATE = new PlayerHitBackState();

		public static PlayerState DEAD_STATE = new PlayerDeadState();

		public static PlayerState FORCEIDLE_STATE = new PlayerForceIdleState();

		protected GameObject playerObject;

		protected BaseCameraScript gameCamera;

		protected Transform playerTransform;

		protected CharacterController charController;

		protected Animation animation;

		protected Collider collider;

		protected GameObject powerObj;

		protected Transform respawnTrans;

		public AudioPlayer audioPlayer;

		protected bool bFriendPlayer;

		public Enemy friendTargetEnemy;

		public Vector3 friendMoveTarget;

		public Vector3 friendSkillMoveDistance;

		protected Transform m_EnemyTarget;

		protected PlayerState playerState;

		protected EPlayerState epState;

		protected InputController inputController;

		protected Vector3 getHitFlySpeed;

		protected BombSpot bombSpot;

		protected Vector3 lastHitPosition;

		protected float exp;

		protected int gold;

		protected int dollor;

		protected float gold_exp_addition;

		protected float dragSpeedValueByEnemy;

		protected float dragSpeedTime;

		protected float dragSpeedTimer = -1f;

		protected bool bFaint;

		public float faintTime;

		protected float faintTimer = -1f;

		public float hitBackDistance;

		public Vector3 hitBackDir;

		protected float m_IgnorePlayerEnemyCollisionTime = 1f;

		protected float m_IgnorePlayerEnemyCollisionTimer = -1f;

		protected float m_ColorChangeTime = 0.5f;

		protected float m_ColorChangeTimer = -1f;

		protected float m_ColorChangeTime2 = 1f;

		protected float m_ColorChangeTimer2 = -1f;

		protected bool m_bColorChanging;

		protected Hashtable m_PowerUPS = new Hashtable();

		protected Hashtable m_Avatars = new Hashtable();

		private GameObject m_BuffEffectShield;

		private GameObject m_BuffEffectAssault;

		private GameObject m_BuffEffectSpeedUp;

		protected Weapon weapon;

		protected List<WeaponType> weaponList;

		protected float maxHp;

		protected float hp;

		protected float m_nHpFac = 5f;

		public float m_nHpLimit;

		protected float attack;

		protected float attackSpeedFac;

		protected float defense;

		protected float guiHp;

		protected float walkSpeed;

		protected float powerBuff;

		protected float powerBuffStartTime;

		protected float lastUpdateNearestWayPointTime;

		protected int currentWeaponIndex;

		protected float m_Stamina;

		protected float m_MaxStamina;

		protected bool m_bSpeedUpByStamina;

		protected float m_ResurrectionNoAttackTimer = -1f;

		public bool m_bIdle;

		public bool m_bRunning;

		public bool m_bShooting;

		public bool m_bRunAndShooting;

		public bool m_bFirstShoot;

		public float m_LastShootBeginTime;

		public int m_TwoHandGunAnimSpecialCounter;

		protected bool m_bPowerUpsShieldFac;

		protected float m_PowerUpsShieldDamage;

		protected bool m_bPowerUpsAttackFac;

		protected float m_PowerUpsAttackStartTime;

		protected float m_PowerUpsAttackTime;

		protected float m_PowerUpsAttackAdd;

		protected float gothitEndTime;

		protected string weaponNameEnd;

		protected bool isRunning;

		private SkillImpl m_ActiveSkillImpl;

		public float m_SpeedUpConsume = 5f;

		private Dictionary<enSkillType, SkillImpl> m_PassiveSkills;

		private GameObject m_GOInvincible;

		private GameObject m_SkillEffect_FancyFootwork;

		private GameObject m_SkillEffect_HailMary;

		private float m_SkillEffect_HailMary_Timer = -1f;

		private float m_SkillEffect_HailMary_Time = 1f;

		private GameObject m_SkillEffect_Vertigo;

		private bool m_bIsVertigo;

		public BloodRect bloodRect;

		public int m_iNGroupID;

		public bool m_bIsNetControl = true;

		public int m_iKillerID = -1;

		public bool m_bCirculationAttack;

		private Dictionary<enBattlefieldProps, NBattleShopItemImpl> m_NBattleShopItemList;

		public List<WeaponType> WeaponList
		{
			get
			{
				return weaponList;
			}
		}

		public Vector3 HitPoint { get; set; }

		public bool FriendPlayer
		{
			get
			{
				return bFriendPlayer;
			}
			set
			{
				bFriendPlayer = value;
			}
		}

		public Transform EnemyTarget
		{
			get
			{
				return m_EnemyTarget;
			}
			set
			{
				m_EnemyTarget = value;
			}
		}

		public float WalkSpeed
		{
			get
			{
				return walkSpeed;
			}
		}

		public SkillImpl ActiveSkillImpl
		{
			get
			{
				return m_ActiveSkillImpl;
			}
			set
			{
				m_ActiveSkillImpl = value;
			}
		}

		public float Exp
		{
			get
			{
				return exp;
			}
			set
			{
				exp = value;
			}
		}

		public int Level { get; set; }

		public int Gold
		{
			get
			{
				return gold;
			}
			set
			{
				gold = value;
			}
		}

		public int Dollor
		{
			get
			{
				return dollor;
			}
			set
			{
				dollor = value;
			}
		}

		public float Stamina
		{
			get
			{
				return m_Stamina;
			}
			set
			{
				m_Stamina = value;
				if (m_Stamina > m_MaxStamina)
				{
					m_Stamina = m_MaxStamina;
				}
				if (m_Stamina < 0f)
				{
					m_Stamina = 0f;
				}
				if (!FriendPlayer && GameApp.GetInstance().GetGameState().m_bIsSurvivalMode)
				{
					GameApp.GetInstance().GetGameState().playerStaInSurvivalMode = m_Stamina;
				}
			}
		}

		public bool Faint
		{
			get
			{
				return bFaint;
			}
			set
			{
				bFaint = value;
			}
		}

		public InputController InputController
		{
			get
			{
				return inputController;
			}
		}

		public bool IsRunning
		{
			get
			{
				return isRunning;
			}
		}

		public string WeaponNameEnd
		{
			get
			{
				return weaponNameEnd;
			}
			set
			{
				weaponNameEnd = value;
			}
		}

		public Vector3 GetHitFlySpeed
		{
			get
			{
				return getHitFlySpeed;
			}
		}

		public Vector3 LastHitPosition
		{
			get
			{
				return lastHitPosition;
			}
			set
			{
				lastHitPosition = value;
			}
		}

		public BombSpot BombSpot
		{
			get
			{
				return bombSpot;
			}
			set
			{
				bombSpot = value;
			}
		}

		public GameObject PlayerObject
		{
			get
			{
				return playerObject;
			}
			set
			{
				playerObject = value;
			}
		}

		public float PowerBuff
		{
			get
			{
				return powerBuff;
			}
		}

		public float Attack
		{
			get
			{
				return attack;
			}
		}

		public float AttackSpeedAdditive
		{
			get
			{
				return attackSpeedFac;
			}
		}

		public float Defense
		{
			get
			{
				return defense;
			}
			set
			{
				defense = value;
			}
		}

		public bool SpeedUpByStamina
		{
			get
			{
				return m_bSpeedUpByStamina;
			}
			set
			{
				m_bSpeedUpByStamina = value;
				CalcWalkSpeed();
				if (m_bSpeedUpByStamina)
				{
					GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load("Zombie3D/PlayerBuff/speed"), PlayerObject.transform.position, Quaternion.identity) as GameObject;
					gameObject.transform.parent = PlayerObject.transform;
					gameObject.transform.localPosition = new Vector3(0f, 0.06f, 0f);
					RemoveTimerScript removeTimerScript = gameObject.AddComponent(typeof(RemoveTimerScript)) as RemoveTimerScript;
					removeTimerScript.life = 0.8f;
					SetupSpeedUpEffect();
				}
				else if (m_BuffEffectSpeedUp != null)
				{
					if (TimerManager.GetInstance().Ready(91) && GameApp.GetInstance().GetGameState().SoundOn)
					{
						string text = "Zombie3D/Audio/RealPersonSound/StopRun/StopRun_";
						text += UnityEngine.Random.Range(0, 1);
						AudioManager.PlayMusicOnce(text, playerTransform);
						TimerManager.GetInstance().Do(91);
					}
					UnityEngine.Object.Destroy(m_BuffEffectSpeedUp);
					m_BuffEffectSpeedUp = null;
				}
			}
		}

		public float PowerUpsAttackAdd
		{
			get
			{
				return m_PowerUpsAttackAdd;
			}
			set
			{
				m_PowerUpsAttackAdd = value;
			}
		}

		public float HP
		{
			get
			{
				return hp;
			}
			set
			{
				hp = value;
				if (hp > maxHp)
				{
					hp = maxHp;
				}
				if (hp < 0f)
				{
					hp = 0f;
				}
				if (GameApp.GetInstance().GetGameState().m_bIsSurvivalMode)
				{
					if (FriendPlayer)
					{
						GameApp.GetInstance().GetGameState().friendPlayerHpInSurvivalMode = hp;
					}
					else
					{
						GameApp.GetInstance().GetGameState().playerHpInSurvivalMode = hp;
					}
				}
			}
		}

		public void CalcWalkSpeed()
		{
			FixedConfig.WeaponCfg weaponCfg = ConfigManager.Instance().GetFixedConfig().GetWeaponCfg(GetWeapon().GetWeaponType());
			float playerWalkSpeed = GameApp.GetInstance().GetGameScene().GetGameParameters()
				.PlayerWalkSpeed;
			float spd = weaponCfg.spd;
			GameState gameState = GameApp.GetInstance().GetGameState();
			float num = dragSpeedValueByEnemy;
			float num2 = 0f;
			if (SpeedUpByStamina)
			{
				num2 = 0.1f;
				Skill skill = null;
				List<Skill> playerSkilles = GameApp.GetInstance().GetGameState().GetPlayerSkilles();
				foreach (Skill item in playerSkilles)
				{
					if (item.SkillType == enSkillType.FastRun)
					{
						skill = item;
						break;
					}
				}
				if (skill != null)
				{
					num2 = 0.1f + (float)(skill.Level - 1) * 0.05f;
				}
			}
			float num3 = 0f;
			if (GameApp.GetInstance().GetGameState().m_SelectFriendIndex == 4)
			{
				num3 = 0.05f;
			}
			float num4 = 0f;
			foreach (Avatar key in m_Avatars.Keys)
			{
				if (key == null || !(bool)m_Avatars[key])
				{
					continue;
				}
				ArrayList avatarCfgs = ConfigManager.Instance().GetFixedConfig().avatarCfgs;
				for (int i = 0; i < avatarCfgs.Count; i++)
				{
					FixedConfig.AvatarCfg avatarCfg = (FixedConfig.AvatarCfg)avatarCfgs[i];
					if (key.SuiteType == avatarCfg.suiteType && key.AvtType == avatarCfg.avtType)
					{
						int id = gameState.SetPropsAdditionsID((int)avatarCfg.suiteType, (int)avatarCfg.avtType);
						float num5 = avatarCfg.prop.m_SpeedAdditive;
						PropsAdditionImpl propsAdditionImpl = gameState.CheckAgeing(id, enPropsAdditionType.E_SpeedAdditive, (enPropsAdditionPart)avatarCfg.avtType);
						if (propsAdditionImpl != null)
						{
							num5 = propsAdditionImpl.GetEffect(num5);
						}
						num4 += Mathf.Clamp01(num5);
					}
				}
			}
			walkSpeed = playerWalkSpeed * (1f + spd + num + num2 + num3);
			Debug.Log("walkSpeed_After" + walkSpeed + "|" + spd + "|" + num + "|" + num2 + "|" + num3 + " | " + num4);
		}

		public void SetFaint(bool faint, float faint_time)
		{
			Faint = faint;
			faintTime = faint_time;
			faintTimer = 0f;
		}

		public void SetEnemyDragSpeed(float drag_speed_time, float drag_speed_value)
		{
			if (dragSpeedTimer < 0f)
			{
				dragSpeedTime = drag_speed_time;
				dragSpeedTimer = 0f;
				dragSpeedValueByEnemy = Mathf.Clamp(drag_speed_value, -1f, 1f);
				CalcWalkSpeed();
			}
		}

		public void SetResurrectionNoAttack()
		{
			m_ResurrectionNoAttackTimer = 0f;
			ShowResurrectionNoAttck(true);
			if (GameApp.GetInstance().GetGameState().m_eGameMode.m_ePlayMode != 0)
			{
				GameSetup.Instance.ReqSyncPlayerInfo(GameSetup.NPlayerDataType.E_Invincible, 1f);
			}
		}

		public void ShowResurrectionNoAttck(bool bShow)
		{
			if (bShow)
			{
				if (m_GOInvincible == null)
				{
					m_GOInvincible = UnityEngine.Object.Instantiate(Resources.Load("Zombie3D/Misc/InvincibleEffect"), PlayerObject.transform.position, Quaternion.identity) as GameObject;
					m_GOInvincible.transform.parent = PlayerObject.transform;
					m_GOInvincible.transform.localPosition = new Vector3(0f, 0f, 0f);
					m_GOInvincible.transform.localRotation = Quaternion.Euler(270f, 0f, 0f);
					if (m_GOInvincible.GetComponent<Animation>() != null)
					{
						m_GOInvincible.GetComponent<Animation>()[m_GOInvincible.GetComponent<Animation>().clip.name].wrapMode = WrapMode.Loop;
						m_GOInvincible.GetComponent<Animation>().Play(m_GOInvincible.GetComponent<Animation>().clip.name);
					}
				}
				else if (m_GOInvincible.GetComponent<Animation>() != null)
				{
					UnityEngine.Object.Destroy(m_GOInvincible.GetComponent(typeof(RemoveTimerScript)) as RemoveTimerScript);
					m_GOInvincible.GetComponent<Animation>()[m_GOInvincible.GetComponent<Animation>().clip.name].wrapMode = WrapMode.Loop;
					m_GOInvincible.GetComponent<Animation>().Play(m_GOInvincible.GetComponent<Animation>().clip.name);
				}
			}
			else if (m_GOInvincible != null)
			{
				UnityEngine.Object.Destroy(m_GOInvincible);
			}
		}

		public void CreateScreenBlood(float damage)
		{
			if (gameCamera != null)
			{
				gameCamera.CreateScreenBlood(damage / 10f);
			}
		}

		public void Move(Vector3 motion)
		{
			if (!bFaint && charController != null && !m_bIsVertigo)
			{
				charController.Move(motion);
			}
		}

		public float GetGuiHp()
		{
			return guiHp;
		}

		public float GetHp()
		{
			return hp;
		}

		public float GetThisAttack()
		{
			float result = Attack;
			if (m_PassiveSkills.ContainsKey(enSkillType.KillShot))
			{
				float doubleDamagePercent = ((SkillKillShot)m_PassiveSkills[enSkillType.KillShot]).m_DoubleDamagePercent;
				int num = UnityEngine.Random.Range(0, 10000);
				if ((float)num < doubleDamagePercent * 10000f)
				{
					result = Attack * 2f;
				}
			}
			return result;
		}

		public void CalcAttack()
		{
			attack = GetWeapon().Damage;
			GameState gameState = GameApp.GetInstance().GetGameState();
			PropsAdditionImpl propsAdditionImpl = gameState.CheckAgeing(gameState.SetPropsAdditionsID((int)GetWeapon().GetWeaponType(), 2), enPropsAdditionType.E_Damage, enPropsAdditionPart.E_Weapon);
			if (propsAdditionImpl != null)
			{
				attack = propsAdditionImpl.GetEffect(GetWeapon().Damage) * GetWeapon().AttackFrequency;
			}
			float num = 0f;
			if (GameApp.GetInstance().GetGameState().m_SelectFriendIndex == 5)
			{
				num = 5f;
			}
			float num2 = 0f;
			foreach (Avatar key in m_Avatars.Keys)
			{
				if (key == null || !(bool)m_Avatars[key])
				{
					continue;
				}
				ArrayList avatarCfgs = ConfigManager.Instance().GetFixedConfig().avatarCfgs;
				for (int i = 0; i < avatarCfgs.Count; i++)
				{
					FixedConfig.AvatarCfg avatarCfg = (FixedConfig.AvatarCfg)avatarCfgs[i];
					if (key.SuiteType == avatarCfg.suiteType && key.AvtType == avatarCfg.avtType)
					{
						int id = gameState.SetPropsAdditionsID((int)avatarCfg.suiteType, (int)avatarCfg.avtType);
						float num3 = avatarCfg.prop.m_AttackAdditive;
						propsAdditionImpl = gameState.CheckAgeing(id, enPropsAdditionType.E_AttackAdditive, (enPropsAdditionPart)avatarCfg.avtType);
						if (propsAdditionImpl != null)
						{
							num3 = propsAdditionImpl.GetEffect(num3);
						}
						num2 += Mathf.Clamp01(num3);
					}
				}
			}
			float num4 = 0f;
			if (m_bPowerUpsAttackFac && Time.time - m_PowerUpsAttackStartTime < m_PowerUpsAttackTime)
			{
				FixedConfig.WeaponCfg weaponCfg = ConfigManager.Instance().GetFixedConfig().GetWeaponCfg(GetWeapon().GetWeaponType());
				num4 = Mathf.Clamp01(PowerUpsAttackAdd);
			}
			if (FriendPlayer)
			{
				FixedConfig.WeaponCfg weaponCfg2 = ConfigManager.Instance().GetFixedConfig().GetWeaponCfg(GetWeapon().GetWeaponType());
				if (weaponCfg2.mClass < 5)
				{
					attack *= 0.6f;
				}
				else if (weaponCfg2.mClass < 7)
				{
					attack *= 0.4f;
				}
				else if (attack < 8f)
				{
					attack *= 0.3f;
				}
				if (GameApp.GetInstance().GetGameScene().GetPlayer()
					.ActiveSkillImpl != null)
				{
					switch (GameApp.GetInstance().GetGameScene().GetPlayer()
						.ActiveSkillImpl.GetSkill().SkillType)
					{
					case enSkillType.CoverMe:
						attack += attack * ((SkillCoverMe)GameApp.GetInstance().GetGameScene().GetPlayer()
							.ActiveSkillImpl).m_FriendAttackAdd;
						break;
					case enSkillType.DoubleTeam:
						attack += attack * ((SkillDoubleTeam)GameApp.GetInstance().GetGameScene().GetPlayer()
							.ActiveSkillImpl).m_FriendAttackAdd;
						break;
					}
				}
			}
			else
			{
				attack = (attack + num) * (1f + num2 + num4);
			}
			if (FriendPlayer)
			{
				attack *= 0.7f;
			}
		}

		public float CalcAttackSpeedAdditive()
		{
			attackSpeedFac = 0f;
			GameState gameState = GameApp.GetInstance().GetGameState();
			float num = 0f;
			foreach (Avatar key in m_Avatars.Keys)
			{
				if (key == null || !(bool)m_Avatars[key])
				{
					continue;
				}
				ArrayList avatarCfgs = ConfigManager.Instance().GetFixedConfig().avatarCfgs;
				for (int i = 0; i < avatarCfgs.Count; i++)
				{
					FixedConfig.AvatarCfg avatarCfg = (FixedConfig.AvatarCfg)avatarCfgs[i];
					if (key.SuiteType == avatarCfg.suiteType && key.AvtType == avatarCfg.avtType)
					{
						int id = gameState.SetPropsAdditionsID((int)avatarCfg.suiteType, (int)avatarCfg.avtType);
						float num2 = avatarCfg.prop.m_AttackSpeedAdditive;
						PropsAdditionImpl propsAdditionImpl = gameState.CheckAgeing(id, enPropsAdditionType.E_AttackSpeedAdditive, (enPropsAdditionPart)avatarCfg.avtType);
						if (propsAdditionImpl != null)
						{
							num2 = propsAdditionImpl.GetEffect(num2);
						}
						num += Mathf.Clamp01(num2);
					}
				}
			}
			attackSpeedFac += num;
			return attackSpeedFac;
		}

		private void CalcDefence()
		{
			Defense = 0f;
			GameState gameState = GameApp.GetInstance().GetGameState();
			foreach (Avatar key in m_Avatars.Keys)
			{
				if (key == null || !(bool)m_Avatars[key])
				{
					continue;
				}
				ArrayList avatarCfgs = ConfigManager.Instance().GetFixedConfig().avatarCfgs;
				for (int i = 0; i < avatarCfgs.Count; i++)
				{
					FixedConfig.AvatarCfg avatarCfg = (FixedConfig.AvatarCfg)avatarCfgs[i];
					if (key.SuiteType == avatarCfg.suiteType && key.AvtType == avatarCfg.avtType)
					{
						int id = gameState.SetPropsAdditionsID((int)avatarCfg.suiteType, (int)avatarCfg.avtType);
						float num = avatarCfg.prop.m_DefenceAdditive;
						PropsAdditionImpl propsAdditionImpl = gameState.CheckAgeing(id, enPropsAdditionType.E_DefenceAdditive, (enPropsAdditionPart)avatarCfg.avtType);
						if (propsAdditionImpl != null)
						{
							num = propsAdditionImpl.GetEffect(num);
						}
						Defense += num;
					}
				}
			}
			if (!FriendPlayer && GameApp.GetInstance().GetGameState().m_SelectFriendIndex == 2)
			{
				Defense += 0.05f;
			}
		}

		public float GetMaxHp()
		{
			return maxHp;
		}

		public void SetMaxHp(float h)
		{
			maxHp = h;
		}

		public float CalcMaxHp()
		{
			float num = 100f;
			float num2 = GameApp.GetInstance().GetGameState().GetLevelHpAffect(Level);
			if (GameApp.GetInstance().GetGameState().m_eGameMode.m_ePlayMode != 0)
			{
				num2 = ((!(num2 < 250f)) ? (num2 - (num2 - 250f) * 0.5f) : ((250f - num2) * 0.5f + num2));
			}
			GameState gameState = GameApp.GetInstance().GetGameState();
			float num3 = 0f;
			foreach (Avatar key in m_Avatars.Keys)
			{
				if (key == null || !(bool)m_Avatars[key])
				{
					continue;
				}
				ArrayList avatarCfgs = ConfigManager.Instance().GetFixedConfig().avatarCfgs;
				for (int i = 0; i < avatarCfgs.Count; i++)
				{
					FixedConfig.AvatarCfg avatarCfg = (FixedConfig.AvatarCfg)avatarCfgs[i];
					if (key.SuiteType == avatarCfg.suiteType && key.AvtType == avatarCfg.avtType)
					{
						int id = gameState.SetPropsAdditionsID((int)avatarCfg.suiteType, (int)avatarCfg.avtType);
						float num4 = avatarCfg.prop.m_HpAdditive;
						PropsAdditionImpl propsAdditionImpl = gameState.CheckAgeing(id, enPropsAdditionType.E_HpAdditive, (enPropsAdditionPart)avatarCfg.avtType);
						if (propsAdditionImpl != null)
						{
							num4 = propsAdditionImpl.GetEffect(num4);
						}
						num3 += Mathf.Clamp01(num4);
					}
				}
			}
			if (!FriendPlayer)
			{
				float num5 = 0f;
				float num6 = 0f;
				if (GameApp.GetInstance().GetGameState().m_SelectFriendIndex == 1)
				{
					num5 = 0.05f;
				}
				else if (GameApp.GetInstance().GetGameState().m_SelectFriendIndex > 5)
				{
					num6 = GameApp.GetInstance().GetGameState().m_SelectFriendIndex - 5;
				}
				num = (num + num2 + num6) * (1f + Mathf.Clamp01(num3) + num5);
			}
			else
			{
				num = (num + num2) * (1f + Mathf.Clamp01(num3));
			}
			if (GameApp.GetInstance().GetGameState().m_eGameMode.m_ePlayMode != 0)
			{
				num *= m_nHpFac;
			}
			num += m_nHpLimit;
			if (GameApp.GetInstance().GetGameState().m_eGameMode.m_ePlayMode != 0)
			{
				GameSetup.Instance.ReqSyncPlayerInfo(GameSetup.NPlayerDataType.E_MaxHP, num);
			}
			return num;
		}

		public float CalcGoldAdditive()
		{
			float num = 0f;
			GameState gameState = GameApp.GetInstance().GetGameState();
			float num2 = 0f;
			foreach (Avatar key in m_Avatars.Keys)
			{
				if (key == null || !(bool)m_Avatars[key])
				{
					continue;
				}
				ArrayList avatarCfgs = ConfigManager.Instance().GetFixedConfig().avatarCfgs;
				for (int i = 0; i < avatarCfgs.Count; i++)
				{
					FixedConfig.AvatarCfg avatarCfg = (FixedConfig.AvatarCfg)avatarCfgs[i];
					if (key.SuiteType == avatarCfg.suiteType && key.AvtType == avatarCfg.avtType)
					{
						int id = gameState.SetPropsAdditionsID((int)avatarCfg.suiteType, (int)avatarCfg.avtType);
						float num3 = avatarCfg.prop.m_GoldAdditive;
						PropsAdditionImpl propsAdditionImpl = gameState.CheckAgeing(id, enPropsAdditionType.E_CashAdditive, (enPropsAdditionPart)avatarCfg.avtType);
						if (propsAdditionImpl != null)
						{
							num3 = propsAdditionImpl.GetEffect(num3);
						}
						num2 += Mathf.Clamp01(num3);
					}
				}
			}
			return num + num2;
		}

		public float CalcExpAdditive()
		{
			float num = 0f;
			GameState gameState = GameApp.GetInstance().GetGameState();
			float num2 = 0f;
			foreach (Avatar key in m_Avatars.Keys)
			{
				if (key == null || !(bool)m_Avatars[key])
				{
					continue;
				}
				ArrayList avatarCfgs = ConfigManager.Instance().GetFixedConfig().avatarCfgs;
				for (int i = 0; i < avatarCfgs.Count; i++)
				{
					FixedConfig.AvatarCfg avatarCfg = (FixedConfig.AvatarCfg)avatarCfgs[i];
					if (key.SuiteType == avatarCfg.suiteType && key.AvtType == avatarCfg.avtType)
					{
						int id = gameState.SetPropsAdditionsID((int)avatarCfg.suiteType, (int)avatarCfg.avtType);
						float num3 = avatarCfg.prop.m_ExpAdditive;
						PropsAdditionImpl propsAdditionImpl = gameState.CheckAgeing(id, enPropsAdditionType.E_ExpAdditive, (enPropsAdditionPart)avatarCfg.avtType);
						if (propsAdditionImpl != null)
						{
							num3 = propsAdditionImpl.GetEffect(num3);
						}
						num2 += Mathf.Clamp01(num3);
					}
				}
			}
			return num + num2;
		}

		public Hashtable GetAvatars()
		{
			return m_Avatars;
		}

		public float GetStamina()
		{
			return m_Stamina;
		}

		public float GetMaxStamina()
		{
			return m_MaxStamina;
		}

		public void CalcMaxStamina()
		{
			m_MaxStamina = 100f;
			if (!FriendPlayer && m_PassiveSkills.ContainsKey(enSkillType.MachoMan))
			{
				float maxStaminaAdd = ((SkillMachoMan)m_PassiveSkills[enSkillType.MachoMan]).m_MaxStaminaAdd;
				m_MaxStamina += maxStaminaAdd;
			}
			GameState gameState = GameApp.GetInstance().GetGameState();
			foreach (Avatar key in m_Avatars.Keys)
			{
				if (key == null || !(bool)m_Avatars[key])
				{
					continue;
				}
				ArrayList avatarCfgs = ConfigManager.Instance().GetFixedConfig().avatarCfgs;
				for (int i = 0; i < avatarCfgs.Count; i++)
				{
					FixedConfig.AvatarCfg avatarCfg = (FixedConfig.AvatarCfg)avatarCfgs[i];
					if (key.SuiteType == avatarCfg.suiteType && key.AvtType == avatarCfg.avtType)
					{
						int id = gameState.SetPropsAdditionsID((int)avatarCfg.suiteType, (int)avatarCfg.avtType);
						float num = avatarCfg.prop.m_StaminaAdd;
						PropsAdditionImpl propsAdditionImpl = gameState.CheckAgeing(id, enPropsAdditionType.E_StaminaAdditive, (enPropsAdditionPart)avatarCfg.avtType);
						if (propsAdditionImpl != null)
						{
							num = propsAdditionImpl.GetEffect(num);
						}
						m_MaxStamina += num * 100f;
					}
				}
			}
			if (!FriendPlayer && GameApp.GetInstance().GetGameState().m_SelectFriendIndex == 3)
			{
				m_MaxStamina += 5f;
			}
		}

		public Transform GetTransform()
		{
			return playerTransform;
		}

		public Collider GetCollider()
		{
			return collider;
		}

		public Transform GetRespawnTransform()
		{
			return respawnTrans;
		}

		public void Init(bool friend_player = false)
		{
			FriendPlayer = friend_player;
			GameObject gameObject = GameObject.FindGameObjectWithTag("Respawn");
			respawnTrans = gameObject.transform;
			m_nHpLimit = 0f;
			if (playerObject == null)
			{
				m_bIsNetControl = false;
				playerObject = (GameObject)UnityEngine.Object.Instantiate(GameApp.GetInstance().GetGameConfig().player, gameObject.transform.position, gameObject.transform.rotation);
				playerObject.transform.position = new Vector3(playerObject.transform.position.x, 10000.1f, playerObject.transform.position.z);
				if (GameApp.GetInstance().GetGameState().m_eGameMode.m_ePlayMode != 0)
				{
					if (GameSetup.Instance == null)
					{
						Debug.LogError("GameSetup.Instance NULL");
					}
					else
					{
						Debug.LogWarning("GameSetup.Instance  Is  Not  Null");
					}
					m_iNGroupID = GameSetup.Instance.GetGroupId(0, true);
					Debug.LogWarning("MyPlayerInit _GroupId:" + m_iNGroupID);
					int posId = GameSetup.Instance.GetPosId(m_iNGroupID);
					playerObject.transform.position = PlayerManager.Instance.GetPlayerPosition(GameApp.GetInstance().GetGameState().m_eGameMode.m_eCooperaMode, posId);
					GameSetup.Instance.ReqSpawnPlayer(posId);
					GameObject gameObject2 = playerObject.transform.Find("BloodRect").gameObject;
					if (gameObject2 != null)
					{
						gameObject2.SetActiveRecursively(false);
					}
					if (m_iNGroupID == 1 || m_iNGroupID == 2)
					{
						GameObject gameObject3 = ((m_iNGroupID != 1) ? ((GameObject)UnityEngine.Object.Instantiate(PlayerManager.Instance.m_BlueGroupGOPrefab, Vector3.zero, Quaternion.identity)) : ((GameObject)UnityEngine.Object.Instantiate(PlayerManager.Instance.m_RedGroupGOPrefab, Vector3.zero, Quaternion.identity)));
						gameObject3.transform.parent = PlayerObject.transform;
						gameObject3.transform.localPosition = Vector3.zero;
					}
					else
					{
						GameObject gameObject4 = null;
						if (m_iNGroupID != 0)
						{
							gameObject4 = null;
						}
						if (gameObject4 != null)
						{
							gameObject4.transform.parent = PlayerObject.transform;
							gameObject4.transform.localPosition = Vector3.zero;
						}
					}
					int avatarHeadID = GameApp.GetInstance().GetGameState().GetAvatarHeadID();
					GameApp.GetInstance().GetGameState().InitPlayerStatistics(GameSetup.Instance.MineUser.Id, GameSetup.Instance.MineUser.Name, m_iNGroupID, avatarHeadID);
				}
			}
			if (FriendPlayer)
			{
				playerObject.transform.Translate(new Vector3(-2f, 0f, 0f));
			}
			EnemyTarget = playerObject.transform;
			GameObject gameObject5 = new GameObject("OneHandWeaponRoot");
			gameObject5.transform.position = playerObject.transform.position;
			gameObject5.transform.rotation = playerObject.transform.rotation;
			gameObject5.transform.parent = playerObject.transform.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Neck/Bip01 R Clavicle/Bip01 R UpperArm/Bip01 R Forearm/Bip01 R Hand/Weapon_Dummy");
			gameObject5.transform.localPosition = Vector3.zero;
			GameObject gameObject6 = new GameObject("TwoHandLeftWeaponRoot");
			gameObject6.transform.position = playerObject.transform.position;
			gameObject6.transform.rotation = playerObject.transform.rotation;
			gameObject6.transform.parent = playerObject.transform.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Neck/Bip01 L Clavicle/Bip01 L UpperArm/Bip01 L Forearm/Bip01 L Hand/Weapon_Bone_L");
			gameObject6.transform.localPosition = Vector3.zero;
			GameObject gameObject7 = new GameObject("TwoHandRightWeaponRoot");
			gameObject7.transform.position = playerObject.transform.position;
			gameObject7.transform.rotation = playerObject.transform.rotation;
			gameObject7.transform.parent = playerObject.transform.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Neck/Bip01 R Clavicle/Bip01 R UpperArm/Bip01 R Forearm/Bip01 R Hand/Weapon_Bone_R");
			gameObject7.transform.localPosition = Vector3.zero;
			if (FriendPlayer)
			{
				playerObject.name = "PlayerFriend";
			}
			else
			{
				playerObject.name = "Player";
			}
			playerTransform = playerObject.transform;
			gameCamera = GameApp.GetInstance().GetGameScene().GetCamera();
			charController = playerObject.GetComponent<CharacterController>();
			animation = playerObject.GetComponent<Animation>();
			collider = playerObject.GetComponent<Collider>();
			audioPlayer = new AudioPlayer();
			Transform transform = playerTransform.Find("Audio");
			if (GameApp.GetInstance().GetGameState().SoundOn)
			{
				string text = "Zombie3D/Audio/RealPersonSound/EnterToll-Gate/EnterToll-Gate_";
				text += UnityEngine.Random.Range(0, 5);
				AudioManager.PlaySoundOnce(text);
			}
			TimerManager.GetInstance().SetTimer(90, 15f, true);
			TimerManager.GetInstance().SetTimer(91, 15f, true);
			GameApp.GetInstance().GetGameState().InitWeapons();
			SetState(IDLE_STATE);
			if (FriendPlayer)
			{
				WeaponType weaponType = WeaponType.Beretta_33;
				weaponList = new List<WeaponType>();
				weaponList.Add(weaponType);
				ChangeWeapon(weapon = WeaponFactory.GetInstance().CreateWeapon(weaponType));
				m_PassiveSkills = new Dictionary<enSkillType, SkillImpl>();
				m_Avatars = new Hashtable();
				Avatar key = new Avatar(Avatar.AvatarSuiteType.Driver, Avatar.AvatarType.Head);
				Avatar key2 = new Avatar(Avatar.AvatarSuiteType.Driver, Avatar.AvatarType.Body);
				m_Avatars[key] = true;
				m_Avatars[key2] = true;
				LoadAvatars();
				SetAvatarEffect();
			}
			else
			{
				weaponList = new List<WeaponType>();
				foreach (WeaponType battleWeapon in GameApp.GetInstance().GetGameState().GetBattleWeapons())
				{
					weaponList.Add(battleWeapon);
				}
				ChangeWeapon(weapon = WeaponFactory.GetInstance().CreateWeapon(GameApp.GetInstance().GetGameState().GetBattleWeapons()[0]));
				m_PassiveSkills = new Dictionary<enSkillType, SkillImpl>();
				List<Skill> playerSkilles = GameApp.GetInstance().GetGameState().GetPlayerSkilles();
				foreach (Skill item in playerSkilles)
				{
					switch (item.SkillType)
					{
					case enSkillType.KillShot:
					{
						SkillKillShot skillKillShot = new SkillKillShot();
						skillKillShot.Init(this, item);
						m_PassiveSkills.Add(item.SkillType, skillKillShot);
						break;
					}
					case enSkillType.FancyFootwork:
					{
						SkillFancyFootwork skillFancyFootwork = new SkillFancyFootwork();
						skillFancyFootwork.Init(this, item);
						m_PassiveSkills.Add(item.SkillType, skillFancyFootwork);
						break;
					}
					case enSkillType.HailMary:
					{
						SkillHailMary skillHailMary = new SkillHailMary();
						skillHailMary.Init(this, item);
						m_PassiveSkills.Add(item.SkillType, skillHailMary);
						Debug.Log("yyyyyyyyyy");
						break;
					}
					case enSkillType.MachoMan:
					{
						SkillMachoMan skillMachoMan = new SkillMachoMan();
						skillMachoMan.Init(this, item);
						m_PassiveSkills.Add(item.SkillType, skillMachoMan);
						break;
					}
					}
				}
				Exp = GameApp.GetInstance().GetGameState().exp;
				Level = GameApp.GetInstance().GetGameState().GetPlayerLevel();
				gold = GameApp.GetInstance().GetGameState().gold;
				dollor = GameApp.GetInstance().GetGameState().dollor;
				m_PowerUPS = GameApp.GetInstance().GetGameState().GetPowerUPS();
				m_Avatars = GameApp.GetInstance().GetGameState().GetAvatars();
				LoadAvatars();
				if (!m_bIsNetControl)
				{
					SetAvatarEffect();
				}
			}
			if (!m_bIsNetControl)
			{
				GameState gameState = GameApp.GetInstance().GetGameState();
				if (gameState.m_eGameMode.m_ePlayMode != 0)
				{
					m_NBattleShopItemList = new Dictionary<enBattlefieldProps, NBattleShopItemImpl>();
					ItemQuickRevive itemQuickRevive = new ItemQuickRevive();
					if (gameState.m_eGameMode.m_PlaersNBattleBuff.ContainsKey(enBattlefieldProps.E_QuickRevive))
					{
						itemQuickRevive.Init(this, new NBattleShopItem(enBattlefieldProps.E_QuickRevive), gameState.m_eGameMode.m_PlaersNBattleBuff[enBattlefieldProps.E_QuickRevive]);
						itemQuickRevive.Do();
					}
					else
					{
						itemQuickRevive.Init(this, new NBattleShopItem(enBattlefieldProps.E_QuickRevive));
					}
					m_NBattleShopItemList.Add(itemQuickRevive.GetItem().BattlefieldProps, itemQuickRevive);
					ItemBestRunner itemBestRunner = new ItemBestRunner();
					if (gameState.m_eGameMode.m_PlaersNBattleBuff.ContainsKey(enBattlefieldProps.E_BestRunner))
					{
						itemBestRunner.Init(this, new NBattleShopItem(enBattlefieldProps.E_BestRunner), gameState.m_eGameMode.m_PlaersNBattleBuff[enBattlefieldProps.E_BestRunner]);
						itemBestRunner.Do();
					}
					else
					{
						itemBestRunner.Init(this, new NBattleShopItem(enBattlefieldProps.E_BestRunner));
					}
					m_NBattleShopItemList.Add(itemBestRunner.GetItem().BattlefieldProps, itemBestRunner);
					ItemTenacity itemTenacity = new ItemTenacity();
					if (gameState.m_eGameMode.m_PlaersNBattleBuff.ContainsKey(enBattlefieldProps.E_Tenacity))
					{
						itemTenacity.Init(this, new NBattleShopItem(enBattlefieldProps.E_Tenacity), gameState.m_eGameMode.m_PlaersNBattleBuff[enBattlefieldProps.E_Tenacity]);
						itemTenacity.Do();
					}
					else
					{
						itemTenacity.Init(this, new NBattleShopItem(enBattlefieldProps.E_Tenacity));
					}
					m_NBattleShopItemList.Add(itemTenacity.GetItem().BattlefieldProps, itemTenacity);
					ItemAnaestheticProjectile itemAnaestheticProjectile = new ItemAnaestheticProjectile();
					if (gameState.m_eGameMode.m_PlaersNBattleBuff.ContainsKey(enBattlefieldProps.E_AnaestheticProjectile))
					{
						itemAnaestheticProjectile.Init(this, new NBattleShopItem(enBattlefieldProps.E_AnaestheticProjectile), gameState.m_eGameMode.m_PlaersNBattleBuff[enBattlefieldProps.E_AnaestheticProjectile]);
						itemAnaestheticProjectile.Do();
					}
					else
					{
						itemAnaestheticProjectile.Init(this, new NBattleShopItem(enBattlefieldProps.E_AnaestheticProjectile));
					}
					m_NBattleShopItemList.Add(itemAnaestheticProjectile.GetItem().BattlefieldProps, itemAnaestheticProjectile);
					ItemStrongWeapon itemStrongWeapon = new ItemStrongWeapon();
					itemStrongWeapon.Init(this, new NBattleShopItem(enBattlefieldProps.E_StrongWeapon));
					m_NBattleShopItemList.Add(itemStrongWeapon.GetItem().BattlefieldProps, itemStrongWeapon);
				}
				maxHp = CalcMaxHp();
			}
			if (GameApp.GetInstance().GetGameState().m_bIsSurvivalMode)
			{
				if (GameApp.GetInstance().GetGameState().m_SurvivalModeBattledMapCount != 0)
				{
					if (FriendPlayer)
					{
						float friendPlayerHpInSurvivalMode = GameApp.GetInstance().GetGameState().friendPlayerHpInSurvivalMode;
						if (friendPlayerHpInSurvivalMode > 0f)
						{
							HP = friendPlayerHpInSurvivalMode;
						}
						else
						{
							HP = maxHp;
						}
						HP = GameApp.GetInstance().GetGameState().friendPlayerHpInSurvivalMode;
					}
					else
					{
						HP = GameApp.GetInstance().GetGameState().playerHpInSurvivalMode;
					}
				}
				else
				{
					HP = maxHp;
				}
			}
			else
			{
				HP = maxHp;
			}
			if (!m_bIsNetControl && GameApp.GetInstance().GetGameState().m_eGameMode.m_ePlayMode != 0)
			{
				GameSetup.Instance.ReqSyncPlayerInfo(GameSetup.NPlayerDataType.E_HP, HP);
			}
			CalcAttack();
			CalcAttackSpeedAdditive();
			CalcDefence();
			CalcMaxStamina();
			if (GameApp.GetInstance().GetGameState().m_bIsSurvivalMode)
			{
				if (GameApp.GetInstance().GetGameState().m_SurvivalModeBattledMapCount == 0)
				{
					m_Stamina = GetMaxStamina();
				}
				else
				{
					m_Stamina = GameApp.GetInstance().GetGameState().playerStaInSurvivalMode;
					Debug.Log("stamina - " + m_Stamina);
				}
			}
			else
			{
				m_Stamina = GetMaxStamina();
			}
			Debug.Log("stamina - " + m_Stamina);
			CalcWalkSpeed();
			ChangeToNormalState();
			if (!FriendPlayer)
			{
				if (gameCamera.GetCameraType() == CameraType.TPSCamera)
				{
					inputController = new TPSInputController();
					inputController.Init();
				}
				else if (gameCamera.GetCameraType() == CameraType.TopWatchingCamera)
				{
					inputController = new TopWatchingInputController();
					inputController.Init();
				}
			}
			UpdateNearestWayPoint();
		}

		public void ResurrectionAtRespawnPos()
		{
			Transform transform = gameCamera.gameObject.transform.Find("Screen_Blood_Dead");
			if (transform != null)
			{
				transform.gameObject.active = false;
			}
			playerObject.transform.position = respawnTrans.position;
			playerObject.transform.rotation = Quaternion.EulerAngles(respawnTrans.eulerAngles);
			maxHp = CalcMaxHp();
			HP = maxHp;
			if (playerObject.layer != 8)
			{
				playerObject.layer = 8;
			}
			SetState(IDLE_STATE);
			SetResurrectionNoAttack();
			if (FriendPlayer || !GameApp.GetInstance().GetGameState().SoundOn)
			{
				return;
			}
			GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load("Zombie3D/Audio/PlayerRelive")) as GameObject;
			if (gameObject != null)
			{
				gameObject.transform.position = playerObject.transform.position;
				RemoveTimerScript removeTimerScript = gameObject.AddComponent<RemoveTimerScript>();
				removeTimerScript.life = 2f;
				AudioSource audio = gameObject.GetComponent<AudioSource>();
				if (audio != null)
				{
					audio.loop = false;
					audio.Play();
				}
			}
		}

		public void ResurrectionAtCurrentPos()
		{
			maxHp = CalcMaxHp();
			HP = maxHp;
			CalcMaxStamina();
			Stamina = m_MaxStamina;
			if (playerObject.layer != 8)
			{
				playerObject.layer = 8;
			}
			SetState(IDLE_STATE);
			if (!FriendPlayer)
			{
				SetResurrectionNoAttack();
				Transform transform = gameCamera.gameObject.transform.Find("Screen_Blood_Dead");
				if (transform != null)
				{
					transform.gameObject.active = false;
				}
				if (GameApp.GetInstance().GetGameState().SoundOn)
				{
					string text = "Zombie3D/Audio/RealPersonSound/ReliveBoth/ReliveBoth_";
					text += UnityEngine.Random.Range(0, 4);
					AudioManager.PlayMusicOnce(text, playerTransform);
				}
				if (FriendPlayer || !GameApp.GetInstance().GetGameState().SoundOn)
				{
					return;
				}
				GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load("Zombie3D/Audio/PlayerRelive")) as GameObject;
				if (gameObject != null)
				{
					gameObject.transform.position = playerObject.transform.position;
					RemoveTimerScript removeTimerScript = gameObject.AddComponent<RemoveTimerScript>();
					removeTimerScript.life = 2f;
					AudioSource audio = gameObject.GetComponent<AudioSource>();
					if (audio != null)
					{
						audio.loop = false;
						audio.Play();
					}
				}
			}
			else if (GameApp.GetInstance().GetGameState().SoundOn)
			{
				string text2 = "Zombie3D/Audio/RealPersonSound/FriendIsRelive/FriendIsRelive_";
				text2 += UnityEngine.Random.Range(0, 2);
				AudioManager.PlayMusicOnce(text2, playerTransform);
			}
		}

		public void NResurrectionAtCurrentPos(bool bIsCurrentPos = true)
		{
			maxHp = CalcMaxHp();
			HP = maxHp;
			CalcMaxStamina();
			Stamina = m_MaxStamina;
			GameSetup.Instance.ReqSyncPlayerInfo(GameSetup.NPlayerDataType.E_HP, HP);
			if (!bIsCurrentPos)
			{
				playerObject.transform.position = PlayerManager.Instance.GetPlayerPosition(GameApp.GetInstance().GetGameState().m_eGameMode.m_eCooperaMode);
			}
			if (playerObject.layer != 8)
			{
				if (GameApp.GetInstance().GetGameState().m_eGameMode.m_ePlayMode == GameState.NetworkGameMode.PlayMode.E_Console)
				{
					playerObject.layer = 8;
				}
				else if (GameApp.GetInstance().GetGameState().m_eGameMode.m_ePlayMode == GameState.NetworkGameMode.PlayMode.E_DeathMatch || GameApp.GetInstance().GetGameState().m_eGameMode.m_ePlayMode == GameState.NetworkGameMode.PlayMode.E_LastStand)
				{
					playerObject.layer = 27;
				}
				else
				{
					playerObject.layer = 8;
				}
			}
			GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load("Zombie3D/Misc/SkillCannonEffect_Born"), GetTransform().position, GetTransform().rotation) as GameObject;
			RemoveTimerScript removeTimerScript = gameObject.AddComponent<RemoveTimerScript>();
			removeTimerScript.life = 1f;
			GameSetup.Instance.ReqSyncPlayerInfo2Value(GameSetup.NPlayerDataType.E_PlayerALive, playerObject.transform.position.x, playerObject.transform.position.z);
			SetState(IDLE_STATE);
			if (!FriendPlayer)
			{
				SetResurrectionNoAttack();
				((TopWatchingCameraScript)GameApp.GetInstance().GetGameScene().GetCamera()).SetCameraTarget(GetTransform());
				if (GameApp.GetInstance().GetGameState().SoundOn)
				{
					string text = "Zombie3D/Audio/RealPersonSound/ReliveBoth/ReliveBoth_";
					text += UnityEngine.Random.Range(0, 4);
					AudioManager.PlayMusicOnce(text, playerTransform);
				}
			}
			else if (GameApp.GetInstance().GetGameState().SoundOn)
			{
				string text2 = "Zombie3D/Audio/RealPersonSound/FriendIsRelive/FriendIsRelive_";
				text2 += UnityEngine.Random.Range(0, 2);
				AudioManager.PlayMusicOnce(text2, playerTransform);
			}
			if (FriendPlayer || !GameApp.GetInstance().GetGameState().SoundOn)
			{
				return;
			}
			GameObject gameObject2 = UnityEngine.Object.Instantiate(Resources.Load("Zombie3D/Audio/PlayerRelive")) as GameObject;
			if (gameObject2 != null)
			{
				gameObject2.transform.position = playerObject.transform.position;
				RemoveTimerScript removeTimerScript2 = gameObject2.AddComponent<RemoveTimerScript>();
				removeTimerScript2.life = 2f;
				AudioSource audio = gameObject2.GetComponent<AudioSource>();
				if (audio != null)
				{
					audio.loop = false;
					audio.Play();
				}
			}
		}

		public void UpdateNearestWayPoint()
		{
		}

		public void Run()
		{
			isRunning = true;
		}

		public void StopRun()
		{
			if (!FriendPlayer)
			{
				bool flag = playerObject != null;
				bool flag2 = GameApp.GetInstance().GetGameScene() != null;
				if (GameApp.GetInstance().GetGameScene().GetFriendPlayer() == null)
				{
					isRunning = false;
					return;
				}
				bool flag3 = GameApp.GetInstance().GetGameScene().GetFriendPlayer() != null;
				bool flag4 = GameApp.GetInstance().GetGameScene().GetFriendPlayer()
					.PlayerObject != null;
				bool flag5 = GameApp.GetInstance().GetGameScene().GetFriendPlayer()
					.GetTransform() != null;
				if (flag && flag2 && flag3 && flag4 && flag5)
				{
					if (isRunning)
					{
						List<Vector3> list = new List<Vector3>();
						float num = 3f;
						bool flag6 = false;
						bool flag7 = false;
						if (ActiveSkillImpl != null)
						{
							switch (ActiveSkillImpl.GetSkill().SkillType)
							{
							case enSkillType.CoverMe:
								flag6 = true;
								break;
							case enSkillType.DoubleTeam:
								flag7 = true;
								break;
							}
						}
						if (flag6 || flag7)
						{
							isRunning = false;
							return;
						}
						if (Vector3.Distance(playerObject.transform.position, GameApp.GetInstance().GetGameScene().GetFriendPlayer()
							.PlayerObject.transform.position) > num)
						{
							for (int i = 0; i < 8; i++)
							{
								float f = (float)i * ((float)Math.PI / 4f);
								Vector3 vector = new Vector3(Mathf.Cos(f), 0f, Mathf.Sin(f));
								RaycastHit hitInfo = default(RaycastHit);
								float num2 = 2f;
								if (!Physics.Raycast(new Ray(playerObject.transform.position, vector), out hitInfo, num2, 10240))
								{
									Vector3 item = playerObject.transform.position + num2 * vector;
									list.Add(item);
								}
							}
							if (list.Count < 1)
							{
								GameApp.GetInstance().GetGameScene().GetFriendPlayer()
									.friendMoveTarget = playerObject.transform.position;
							}
							else
							{
								int index = UnityEngine.Random.Range(0, list.Count);
								Vector3 vector2 = new Vector3(list[index].x, playerObject.transform.position.y, list[index].z);
								GameApp.GetInstance().GetGameScene().GetFriendPlayer()
									.friendMoveTarget = vector2;
							}
						}
						else
						{
							GameApp.GetInstance().GetGameScene().GetFriendPlayer()
								.friendMoveTarget = GameApp.GetInstance().GetGameScene().GetFriendPlayer()
								.GetTransform()
								.position;
						}
					}
				}
				else
				{
					Debug.Log("Exception - " + flag + " | " + flag2 + " | " + flag3 + " | " + flag4 + " | " + flag5);
				}
			}
			isRunning = false;
		}

		public void SetState(PlayerState state)
		{
			if (!(playerObject.name != "PlayerFriend") || playerState != state)
			{
			}
			if (playerState == null)
			{
				playerState = state;
				playerState.OnEnter(this);
			}
			else if (playerState != DEAD_STATE)
			{
				playerState.OnExit(this);
				playerState = state;
				playerState.OnEnter(this);
			}
			else if (HP > 0f)
			{
				playerState.OnExit(this);
				playerState = state;
				playerState.OnEnter(this);
			}
		}

		public PlayerState GetState()
		{
			return playerState;
		}

		public bool IsPlayingAnimation(string name)
		{
			return animation.IsPlaying(name);
		}

		public bool AnimationEnds(string name)
		{
			if (animation[name].time >= animation[name].length / animation[name].speed * 1f)
			{
				if (name == "Shoot01_Shotgun")
				{
				}
				return true;
			}
			if (name == "Shoot01_Shotgun")
			{
			}
			return false;
		}

		public void Animate(string animationName, WrapMode wrapMode, float fadeTime = 0.3f, float speedTime = 1f)
		{
			if (playerObject.name == "Player")
			{
			}
			animation[animationName].wrapMode = wrapMode;
			if (IsPlayingAnimation("Damage01" + WeaponNameEnd) && !(animationName == "Death03"))
			{
				return;
			}
			if (wrapMode == WrapMode.Loop || (!animation.IsPlaying(animationName) && animationName != "Damage01" + WeaponNameEnd))
			{
				if (wrapMode == WrapMode.Loop && fadeTime <= 0f)
				{
					animation.Play(animationName);
					if (GameApp.GetInstance().GetGameState().m_eGameMode.m_ePlayMode != 0)
					{
						GameSetup.Instance.ReqSyncAnimation(animationName, wrapMode, fadeTime, speedTime);
					}
				}
				else
				{
					animation.CrossFade(animationName, fadeTime);
					if (GameApp.GetInstance().GetGameState().m_eGameMode.m_ePlayMode != 0)
					{
						GameSetup.Instance.ReqSyncAnimation(animationName, wrapMode, fadeTime, speedTime);
					}
				}
				if (!(playerObject.name == "Player"))
				{
				}
			}
			else
			{
				animation.Play(animationName);
				if (GameApp.GetInstance().GetGameState().m_eGameMode.m_ePlayMode != 0)
				{
					GameSetup.Instance.ReqSyncAnimation(animationName, wrapMode, fadeTime, speedTime);
				}
				if (!(playerObject.name == "Player"))
				{
				}
			}
		}

		public void PlayAnim(string animationName, WrapMode wrapMode, float speed = 1f)
		{
			Animate(animationName, wrapMode, 0.3f, speed);
		}

		public void CheckBombSpot()
		{
			bombSpot = null;
			List<BombSpot> bombSpots = GameApp.GetInstance().GetGameScene().GetBombSpots();
			foreach (BombSpot item in bombSpots)
			{
				item.DoLogic();
				if (item.CheckInSpot())
				{
					bombSpot = item;
				}
				else if (item.isInstalling())
				{
					bombSpot = item;
				}
			}
		}

		public void ZoomIn(float deltaTime)
		{
			if (weapon.GetWeaponType() == WeaponType.Beretta_33)
			{
				gameCamera.ZoomIn(deltaTime);
			}
		}

		public void ZoomOut(float deltaTime)
		{
			gameCamera.ZoomOut(deltaTime);
		}

		public void AutoAim(float deltaTime)
		{
			weapon.AutoAim(deltaTime);
		}

		public void Fire(float deltaTime)
		{
			if (!bFaint)
			{
				weapon.PullTrigger();
				if (GameApp.GetInstance().GetGameState().m_eGameMode.m_ePlayMode != 0)
				{
					GameSetup.Instance.ReqSyncPlayerData(GameSetup.NPlayerDataType.E_Fire);
				}
			}
		}

		public void StopFire()
		{
			weapon.StopFire();
			if (GameApp.GetInstance().GetGameState().m_eGameMode.m_ePlayMode != 0)
			{
				GameSetup.Instance.ReqSyncPlayerData(GameSetup.NPlayerDataType.E_StopFire);
			}
		}

		public void SetCirculationAttack(bool bCircu)
		{
			m_bCirculationAttack = bCircu;
		}

		public void SetCirculationAttackFromNetwork(bool bCircu, bool IsCopyFire = true)
		{
			m_bCirculationAttack = bCircu;
		}

		public void DoLogic(float deltaTime)
		{
			if (GameApp.GetInstance().GetGameScene().GetPlayer() == this && playerState != null)
			{
				playerState.NextState(this, deltaTime);
			}
			if (GameApp.GetInstance().GetGameScene().GetFriendPlayer() == this)
			{
				playerState.NextState(this, deltaTime);
			}
			if (m_bCirculationAttack && GetWeapon().CouldMakeNextShoot())
			{
				GetWeapon().PullTrigger();
			}
			if (bloodRect != null && maxHp > 0f)
			{
				bloodRect.SetBloodPercent(Mathf.Clamp01(HP / maxHp));
			}
			if (m_ResurrectionNoAttackTimer >= 0f)
			{
				m_ResurrectionNoAttackTimer += deltaTime;
				if (m_ResurrectionNoAttackTimer > 5f)
				{
					m_ResurrectionNoAttackTimer = -1f;
					ShowResurrectionNoAttck(false);
					if (GameApp.GetInstance().GetGameState().m_eGameMode.m_ePlayMode != 0)
					{
						GameSetup.Instance.ReqSyncPlayerInfo(GameSetup.NPlayerDataType.E_Invincible, 0f);
					}
				}
			}
			if (bFaint)
			{
				faintTimer += deltaTime;
				if (faintTimer >= faintTime)
				{
					bFaint = false;
				}
			}
			if (!FriendPlayer)
			{
				if (ActiveSkillImpl != null)
				{
					if (ActiveSkillImpl.GetSkill().SkillType == enSkillType.CoverMe)
					{
						GameApp.GetInstance().GetGameScene().GetFriendPlayer()
							.friendMoveTarget = playerObject.transform.position + friendSkillMoveDistance;
					}
					else if (ActiveSkillImpl.GetSkill().SkillType == enSkillType.DoubleTeam && GameApp.GetInstance().GetGameScene().GetFriendPlayer() != null)
					{
						GameApp.GetInstance().GetGameScene().GetFriendPlayer()
							.friendMoveTarget = playerObject.transform.position + friendSkillMoveDistance;
					}
				}
				else if (isRunning && GameApp.GetInstance().GetGameScene().GetFriendPlayer() != null)
				{
					GameApp.GetInstance().GetGameScene().GetFriendPlayer()
						.friendMoveTarget = playerObject.transform.position;
				}
			}
			if (m_bPowerUpsAttackFac && Time.time - m_PowerUpsAttackStartTime > m_PowerUpsAttackTime)
			{
				PowerUpsAttackAdd = 0f;
				CalcAttack();
				if ((bool)m_BuffEffectAssault)
				{
					UnityEngine.Object.Destroy(m_BuffEffectAssault);
				}
				m_bPowerUpsAttackFac = false;
			}
			if (dragSpeedTimer >= 0f)
			{
				dragSpeedTimer += deltaTime;
				if (dragSpeedTimer >= dragSpeedTime)
				{
					dragSpeedValueByEnemy = 0f;
					dragSpeedTimer = -1f;
					CalcWalkSpeed();
				}
			}
			if (m_ColorChangeTimer >= 0f)
			{
				m_bColorChanging = true;
				m_ColorChangeTimer += deltaTime;
				if (m_ColorChangeTimer >= m_ColorChangeTime)
				{
					m_ColorChangeTimer = -1f;
					StopColorChangeAnimation(1);
				}
			}
			if (m_ColorChangeTimer2 >= 0f)
			{
				m_ColorChangeTimer2 += deltaTime;
				if (m_ColorChangeTimer2 >= m_ColorChangeTime2)
				{
					m_bColorChanging = false;
					m_ColorChangeTimer2 = -1f;
					StopColorChangeAnimation(2);
				}
			}
			if (m_IgnorePlayerEnemyCollisionTimer > 0f)
			{
				m_IgnorePlayerEnemyCollisionTimer += deltaTime;
				if (m_IgnorePlayerEnemyCollisionTimer >= m_ColorChangeTime + m_ColorChangeTime2 + 0.1f)
				{
					Physics.IgnoreLayerCollision(8, 9, false);
					m_IgnorePlayerEnemyCollisionTimer = -1f;
				}
			}
			if (guiHp != hp)
			{
				float num = Mathf.Abs(guiHp - hp);
				guiHp = Mathf.MoveTowards(guiHp, hp, num * 5f * deltaTime);
			}
			if (powerBuff != 1f && Time.time - powerBuffStartTime > 30f)
			{
				ChangeToNormalState();
			}
			if (weapon != null)
			{
				weapon.DoLogic(deltaTime);
			}
			if (Time.time - lastUpdateNearestWayPointTime > 1f)
			{
				UpdateNearestWayPoint();
				lastUpdateNearestWayPointTime = Time.time;
			}
			if (HP > 0f)
			{
				if (SpeedUpByStamina)
				{
					Stamina -= deltaTime * m_SpeedUpConsume;
					if (Stamina <= 0f)
					{
						Stamina = 0f;
						SpeedUpByStamina = false;
						if (GameApp.GetInstance().GetGameState().m_eGameMode.m_ePlayMode != 0)
						{
							GameSetup.Instance.ReqSyncPlayerInfo(GameSetup.NPlayerDataType.E_Skill_RunFast, 0f);
						}
						if (m_BuffEffectSpeedUp != null)
						{
							UnityEngine.Object.Destroy(m_BuffEffectSpeedUp);
							m_BuffEffectSpeedUp = null;
						}
					}
				}
				if (Stamina < m_MaxStamina)
				{
					float num2 = 2f;
					Stamina += deltaTime * (num2 + GameApp.GetInstance().GetGameState().SinewResumeSpeed);
					if (Stamina >= m_MaxStamina)
					{
						Stamina = m_MaxStamina;
					}
				}
			}
			if (ActiveSkillImpl != null)
			{
				ActiveSkillImpl.Update(deltaTime);
			}
			if (!(m_SkillEffect_HailMary_Timer >= 0f))
			{
				return;
			}
			m_SkillEffect_HailMary_Timer += deltaTime;
			if (!(m_SkillEffect_HailMary_Timer > m_SkillEffect_HailMary_Time))
			{
				return;
			}
			if (m_SkillEffect_HailMary != null)
			{
				ParticleEmitter[] componentsInChildren = m_SkillEffect_HailMary.GetComponentsInChildren<ParticleEmitter>();
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					componentsInChildren[i].emit = false;
				}
			}
			m_SkillEffect_HailMary_Timer = -1f;
		}

		public void CheckAttackBloodSuck()
		{
			if (FriendPlayer)
			{
				return;
			}
			bool flag = false;
			flag = ((this == GameApp.GetInstance().GetGameScene().GetPlayer()) ? true : false);
			if (!flag || !m_PassiveSkills.ContainsKey(enSkillType.HailMary))
			{
				return;
			}
			bool flag2 = false;
			float hailMaryHpPercent = ((SkillHailMary)m_PassiveSkills[enSkillType.HailMary]).m_HailMaryHpPercent;
			int num = 10000;
			int num2 = UnityEngine.Random.Range(0, num);
			if ((float)num2 < (float)num * hailMaryHpPercent)
			{
				flag2 = true;
			}
			if (flag2)
			{
				float hailMaryHp = ((SkillHailMary)m_PassiveSkills[enSkillType.HailMary]).m_HailMaryHp;
				HP += hailMaryHp;
				if (GameApp.GetInstance().GetGameState().m_eGameMode.m_ePlayMode == GameState.NetworkGameMode.PlayMode.E_Console)
				{
					ShowSkillEffect_HailMary();
					return;
				}
				GameSetup.Instance.ReqSyncPlayerData(GameSetup.NPlayerDataType.E_Skill_HailMary);
				GameSetup.Instance.ReqSyncPlayerInfo(GameSetup.NPlayerDataType.E_HP, HP);
			}
		}

		public void CheckVertigo(int userID)
		{
			if (FriendPlayer)
			{
				return;
			}
			bool flag = false;
			flag = ((this == GameApp.GetInstance().GetGameScene().GetPlayer()) ? true : false);
			if (!flag || m_NBattleShopItemList[enBattlefieldProps.E_AnaestheticProjectile].NumberOfUse < 0)
			{
				return;
			}
			bool flag2 = false;
			float probability = ((ItemAnaestheticProjectile)m_NBattleShopItemList[enBattlefieldProps.E_AnaestheticProjectile]).GetProbability();
			int num = 10000;
			int num2 = UnityEngine.Random.Range(0, num);
			if ((float)num2 < (float)num * probability)
			{
				flag2 = true;
			}
			if (flag2)
			{
				if (GameApp.GetInstance().GetGameState().m_eGameMode.m_ePlayMode == GameState.NetworkGameMode.PlayMode.E_Console)
				{
					ShowSkillEffect_Vertigo();
				}
				else
				{
					GameSetup.Instance.ReqSyncPlayerInfo(GameSetup.NPlayerDataType.E_Skill_Vertigo, 1f, userID);
				}
			}
		}

		public void ShowSkillEffect_HailMary()
		{
			if (m_SkillEffect_HailMary == null)
			{
				m_SkillEffect_HailMary = UnityEngine.Object.Instantiate(Resources.Load("Zombie3D/Misc/SkillHailMaryEffect"), PlayerObject.transform.position, Quaternion.identity) as GameObject;
				m_SkillEffect_HailMary.transform.parent = PlayerObject.transform;
				m_SkillEffect_HailMary.transform.localPosition = new Vector3(0f, 0f, 0f);
				ParticleEmitter[] componentsInChildren = m_SkillEffect_HailMary.GetComponentsInChildren<ParticleEmitter>();
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					componentsInChildren[i].emit = true;
					componentsInChildren[i].Emit();
				}
				m_SkillEffect_HailMary_Timer = 0f;
			}
			else
			{
				ParticleEmitter[] componentsInChildren2 = m_SkillEffect_HailMary.GetComponentsInChildren<ParticleEmitter>();
				for (int j = 0; j < componentsInChildren2.Length; j++)
				{
					componentsInChildren2[j].emit = true;
					componentsInChildren2[j].Emit();
				}
				m_SkillEffect_HailMary_Timer = 0f;
			}
		}

		public void StopVertigoShow()
		{
			if (GameApp.GetInstance().GetGameState().m_eGameMode.m_ePlayMode != 0)
			{
				GameSetup.Instance.ReqSyncPlayerInfo(GameSetup.NPlayerDataType.E_Skill_VertigoEffect, 0f);
			}
			if (m_bIsVertigo)
			{
				m_bIsVertigo = false;
			}
		}

		public void ShowSkillEffect_Vertigo(bool bShow = true)
		{
			if (!bShow)
			{
				if (m_SkillEffect_Vertigo != null)
				{
					UnityEngine.Object.Destroy(m_SkillEffect_Vertigo);
					m_SkillEffect_Vertigo = null;
				}
			}
			else if (m_SkillEffect_Vertigo == null)
			{
				m_SkillEffect_Vertigo = UnityEngine.Object.Instantiate(Resources.Load("Zombie3D/Misc/SkillFancyVertigoEffect"), PlayerObject.transform.position, Quaternion.identity) as GameObject;
				RemoveTimerScript removeTimerScript = m_SkillEffect_Vertigo.AddComponent(typeof(RemoveTimerScript)) as RemoveTimerScript;
				removeTimerScript.Init(StopVertigoShow);
				m_bIsVertigo = true;
				removeTimerScript.life = ((ItemAnaestheticProjectile)PlayerManager.Instance.GetPlayerClass().GetNBattleItemImpl(enBattlefieldProps.E_AnaestheticProjectile)).GetTimer();
				m_SkillEffect_Vertigo.transform.parent = PlayerObject.transform;
				m_SkillEffect_Vertigo.transform.localPosition = new Vector3(0f, 0f, 0f);
				m_SkillEffect_Vertigo.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
				if (m_SkillEffect_Vertigo.GetComponent<Animation>() != null)
				{
					m_SkillEffect_Vertigo.GetComponent<Animation>()[m_SkillEffect_Vertigo.GetComponent<Animation>().clip.name].wrapMode = WrapMode.Loop;
					m_SkillEffect_Vertigo.GetComponent<Animation>().Play(m_SkillEffect_Vertigo.GetComponent<Animation>().clip.name);
				}
			}
			else if (m_SkillEffect_Vertigo.GetComponent<Animation>() != null)
			{
				UnityEngine.Object.Destroy(m_SkillEffect_Vertigo.GetComponent(typeof(RemoveTimerScript)) as RemoveTimerScript);
				RemoveTimerScript removeTimerScript2 = m_SkillEffect_Vertigo.AddComponent(typeof(RemoveTimerScript)) as RemoveTimerScript;
				removeTimerScript2.Init(StopVertigoShow);
				m_bIsVertigo = true;
				removeTimerScript2.life = ((ItemAnaestheticProjectile)m_NBattleShopItemList[enBattlefieldProps.E_AnaestheticProjectile]).GetTimer();
				m_SkillEffect_Vertigo.GetComponent<Animation>()[m_SkillEffect_Vertigo.GetComponent<Animation>().clip.name].wrapMode = WrapMode.Loop;
				m_SkillEffect_Vertigo.GetComponent<Animation>().Play(m_SkillEffect_Vertigo.GetComponent<Animation>().clip.name);
			}
		}

		public void ShowSkillEffect_FancyFootwork()
		{
			if (m_SkillEffect_FancyFootwork == null)
			{
				m_SkillEffect_FancyFootwork = UnityEngine.Object.Instantiate(Resources.Load("Zombie3D/Misc/SkillFancyFootworkEffect"), PlayerObject.transform.position, Quaternion.identity) as GameObject;
				RemoveTimerScript removeTimerScript = m_SkillEffect_FancyFootwork.AddComponent(typeof(RemoveTimerScript)) as RemoveTimerScript;
				removeTimerScript.life = m_SkillEffect_FancyFootwork.GetComponent<Animation>()[m_SkillEffect_FancyFootwork.GetComponent<Animation>().clip.name].length;
				m_SkillEffect_FancyFootwork.transform.parent = PlayerObject.transform;
				m_SkillEffect_FancyFootwork.transform.localPosition = new Vector3(0f, 0f, 0f);
				m_SkillEffect_FancyFootwork.transform.localRotation = Quaternion.Euler(270f, 0f, 0f);
				if (m_SkillEffect_FancyFootwork.GetComponent<Animation>() != null)
				{
					m_SkillEffect_FancyFootwork.GetComponent<Animation>()[m_SkillEffect_FancyFootwork.GetComponent<Animation>().clip.name].wrapMode = WrapMode.Once;
					m_SkillEffect_FancyFootwork.GetComponent<Animation>().Play(m_SkillEffect_FancyFootwork.GetComponent<Animation>().clip.name);
				}
			}
			else if (m_SkillEffect_FancyFootwork.GetComponent<Animation>() != null)
			{
				UnityEngine.Object.Destroy(m_SkillEffect_FancyFootwork.GetComponent(typeof(RemoveTimerScript)) as RemoveTimerScript);
				RemoveTimerScript removeTimerScript2 = m_SkillEffect_FancyFootwork.AddComponent(typeof(RemoveTimerScript)) as RemoveTimerScript;
				removeTimerScript2.life = m_SkillEffect_FancyFootwork.GetComponent<Animation>()[m_SkillEffect_FancyFootwork.GetComponent<Animation>().clip.name].length;
				m_SkillEffect_FancyFootwork.GetComponent<Animation>()[m_SkillEffect_FancyFootwork.GetComponent<Animation>().clip.name].wrapMode = WrapMode.Once;
				m_SkillEffect_FancyFootwork.GetComponent<Animation>().Play(m_SkillEffect_FancyFootwork.GetComponent<Animation>().clip.name);
			}
		}

		public void OnHit(float damage, int NAttackerID = -1)
		{
			float num = damage * Mathf.Clamp01(1f - Defense);
			float hP = HP;
			if (hP <= 0f)
			{
				return;
			}
			bool flag = false;
			if (!FriendPlayer)
			{
				float num2 = 0f;
				if (m_PassiveSkills.ContainsKey(enSkillType.FancyFootwork))
				{
					float fancyPercent = ((SkillFancyFootwork)m_PassiveSkills[enSkillType.FancyFootwork]).m_FancyPercent;
					num2 += fancyPercent;
				}
				if (GetWeapon().GetWeaponType() == WeaponType.Messiah)
				{
					num2 += 0.1f;
				}
				if (num2 > 0f)
				{
					int num3 = UnityEngine.Random.Range(0, 10000);
					if ((float)num3 < num2 * 10000f)
					{
						num = 0f;
						flag = true;
						if (GameApp.GetInstance().GetGameState().m_eGameMode.m_ePlayMode == GameState.NetworkGameMode.PlayMode.E_Console)
						{
							ShowSkillEffect_FancyFootwork();
						}
						else
						{
							GameSetup.Instance.ReqSyncPlayerData(GameSetup.NPlayerDataType.E_Skill_FancyFootwork);
						}
					}
				}
			}
			if (m_bPowerUpsShieldFac)
			{
				m_PowerUpsShieldDamage -= num;
				if (m_PowerUpsShieldDamage <= 0f)
				{
					m_bPowerUpsShieldFac = false;
					UnityEngine.Object.Destroy(m_BuffEffectShield);
				}
			}
			else if (m_ResurrectionNoAttackTimer < 0f && num > 0f)
			{
				if (!FriendPlayer)
				{
					HP -= num;
					HP = (int)HP;
					HP = Mathf.Clamp(HP, 0f, maxHp);
				}
				if (GameApp.GetInstance().GetGameState().m_eGameMode.m_ePlayMode != 0)
				{
					GameSetup.Instance.ReqSyncPlayerInfo(GameSetup.NPlayerDataType.E_HP, HP);
					if (HP <= 0f && NAttackerID != -1)
					{
						m_iKillerID = NAttackerID;
						GameSetup.Instance.BattleStatisticsOfDeath(NAttackerID);
					}
				}
				if (!FriendPlayer)
				{
					GameScene gameScene = GameApp.GetInstance().GetGameScene();
					gameScene.waveWounded = true;
				}
			}
			if (!flag && m_ResurrectionNoAttackTimer < 0f)
			{
				if (GameApp.GetInstance().GetGameState().m_eGameMode.m_ePlayMode == GameState.NetworkGameMode.PlayMode.E_Console)
				{
					OnHitChangeColor(num, true);
				}
				else
				{
					OnHitChangeColor(num, true);
					GameSetup.Instance.ReqSyncPlayerInfo(GameSetup.NPlayerDataType.E_HittedColorChanged, num);
				}
			}
			if (FriendPlayer)
			{
				hp = maxHp;
			}
		}

		public void OnHitChangeColor(float realDamage, bool IsMine)
		{
			if (m_ColorChangeTimer < 0f)
			{
				m_ColorChangeTimer = 0f;
				m_ColorChangeTimer2 = -1f;
				PlayColorChangeAnimation(new Color(1f, 0f, 0f, 1f));
			}
			if (GameApp.GetInstance().GetGameState().m_eGameMode.m_ePlayMode == GameState.NetworkGameMode.PlayMode.E_Console)
			{
				Physics.IgnoreLayerCollision(8, 9, true);
			}
			else if (GameApp.GetInstance().GetGameState().m_eGameMode.m_ePlayMode == GameState.NetworkGameMode.PlayMode.E_DeathMatch || GameApp.GetInstance().GetGameState().m_eGameMode.m_ePlayMode == GameState.NetworkGameMode.PlayMode.E_LastStand)
			{
				Physics.IgnoreLayerCollision(27, 27, true);
			}
			m_IgnorePlayerEnemyCollisionTimer = 0f;
			if (IsMine)
			{
				playerState.OnHit(this, realDamage);
			}
		}

		public void OnHitBack(float time, float distance, Vector3 dir)
		{
			float hP = HP;
			if (!(hP <= 0f) && !m_bColorChanging)
			{
				Vector3 forward = respawnTrans.TransformDirection(-dir);
				playerObject.transform.forward = forward;
				SetFaint(true, time);
				hitBackDistance = distance;
				hitBackDir = dir.normalized;
				SetState(HITBACK_STATE);
			}
		}

		private void ColorChangeFun(Renderer r, Color color)
		{
			ColorAnimationScript colorAnimationScript = r.GetComponent(typeof(ColorAnimationScript)) as ColorAnimationScript;
			if (colorAnimationScript == null)
			{
				colorAnimationScript = r.gameObject.AddComponent(typeof(ColorAnimationScript)) as ColorAnimationScript;
			}
			colorAnimationScript.m_AnimPeriod = 0.15f;
			colorAnimationScript.m_StartColor = new Color(20f / 51f, 0f, 0f, 1f);
			colorAnimationScript.m_EndColor = color;
			colorAnimationScript.SetColorAnimation();
			colorAnimationScript.PlayColorAnimation();
		}

		private void PlayColorChangeAnimation(Color color)
		{
			Renderer[] componentsInChildren = PlayerObject.GetComponentsInChildren<Renderer>();
			Renderer[] array = componentsInChildren;
			foreach (Renderer renderer in array)
			{
				if (renderer.enabled && renderer.transform.name != "shadow" && (renderer.gameObject.name.Contains("_body") || renderer.gameObject.name.Contains("_hat") || renderer.gameObject.name.Contains("_equipment") || renderer.gameObject.name.Contains("_Eyeglass") || renderer.gameObject.name.Contains("_head")))
				{
					ColorChangeFun(renderer, color);
				}
			}
		}

		private void StopColorChangeAnimation(int section)
		{
			Renderer[] componentsInChildren = PlayerObject.GetComponentsInChildren<Renderer>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				if (componentsInChildren[i].enabled && componentsInChildren[i].transform.parent.gameObject == PlayerObject && componentsInChildren[i].transform.name != "shadow")
				{
					ColorAnimationScript colorAnimationScript = componentsInChildren[i].GetComponent(typeof(ColorAnimationScript)) as ColorAnimationScript;
					if (colorAnimationScript != null)
					{
						colorAnimationScript.StopColorAnimation();
						colorAnimationScript.GetComponent<Renderer>().material.SetColor("_TintColor", new Color(0.8f, 0.8f, 0.8f, 1f));
					}
				}
			}
			if (section == 1)
			{
				m_ColorChangeTimer2 = 0f;
				PlayColorChangeAnimation(new Color(1f, 0f, 0f, 1f));
			}
			if (GameApp.GetInstance().GetGameState().SoundOn)
			{
				string text = "Zombie3D/Audio/RealPersonSound/BruiseMyself/BruiseMyself_";
				text += UnityEngine.Random.Range(0, 3);
				AudioManager.PlayMusicOnce(text, playerTransform);
			}
		}

		public bool CouldGetAnotherHit()
		{
			if (Time.time - gothitEndTime > 0.5f)
			{
				gothitEndTime = Time.time;
				return true;
			}
			return false;
		}

		public void ShowNPlayerDead()
		{
			audioPlayer.PlaySound("Dead");
			playerObject.layer = 18;
			NetworkTransformInterpolation networkTransformInterpolation = playerObject.GetComponent(typeof(NetworkTransformInterpolation)) as NetworkTransformInterpolation;
			if (networkTransformInterpolation != null)
			{
				networkTransformInterpolation.ClearBuffer();
			}
		}

		public void ShowNPlayerALive(Vector3 pos)
		{
			if (GameApp.GetInstance().GetGameState().m_eGameMode.m_ePlayMode == GameState.NetworkGameMode.PlayMode.E_DeathMatch || GameApp.GetInstance().GetGameState().m_eGameMode.m_ePlayMode == GameState.NetworkGameMode.PlayMode.E_LastStand)
			{
				playerObject.layer = 27;
			}
			else
			{
				playerObject.layer = 8;
			}
			GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load("Zombie3D/Misc/SkillCannonEffect_Born"), pos, GetTransform().rotation) as GameObject;
			RemoveTimerScript removeTimerScript = gameObject.AddComponent<RemoveTimerScript>();
			removeTimerScript.life = 1f;
			playerObject.transform.position = pos;
		}

		public void OnDead()
		{
			audioPlayer.PlaySound("Dead");
			weapon.StopFire();
			playerObject.layer = 18;
			Animate("Death03", WrapMode.ClampForever);
			SetState(DEAD_STATE);
			if (SpeedUpByStamina)
			{
				SpeedUpByStamina = false;
				if (GameApp.GetInstance().GetGameState().m_eGameMode.m_ePlayMode != 0)
				{
					GameSetup.Instance.ReqSyncPlayerInfo(GameSetup.NPlayerDataType.E_Skill_RunFast, 0f);
				}
			}
			if (!FriendPlayer)
			{
				if (GameApp.GetInstance().GetGameState().m_eGameMode.m_ePlayMode != 0)
				{
					GameSetup.Instance.ReqSyncPlayerData(GameSetup.NPlayerDataType.E_PlayerDeath);
				}
				GameState gameState = GameApp.GetInstance().GetGameState();
				if (gameState.m_eGameMode.m_ePlayMode == GameState.NetworkGameMode.PlayMode.E_Console)
				{
					Transform transform = gameCamera.gameObject.transform.Find("Screen_Blood_Dead");
					if (transform != null)
					{
						transform.gameObject.active = true;
					}
				}
				GameScene gameScene = GameApp.GetInstance().GetGameScene();
				if (gameScene.GetFriendPlayer() != null && gameScene.GetFriendPlayer().HP > 0f)
				{
					gameScene.GetFriendPlayer().SetState(FORCEIDLE_STATE);
				}
				if (GameApp.GetInstance().GetGameState().m_eGameMode.m_ePlayMode == GameState.NetworkGameMode.PlayMode.E_Console)
				{
					gameScene.DDSTrigger.StopRefreshEnemies();
					Hashtable enemies = gameScene.GetEnemies();
					foreach (Enemy value in enemies.Values)
					{
						if (value != null && value.HP > 0f)
						{
							value.SetState(Enemy.FORCEIDLE_STATE);
						}
					}
					Debug.Log("PlayerONDEAD() - " + enemies.Count);
				}
				m_Stamina = 0f;
				if (gameState.m_eGameMode.m_ePlayMode == GameState.NetworkGameMode.PlayMode.E_Console)
				{
					BattleUIScript battleUIScript = SceneUIManager.Instance().GetSceneUIObject().GetComponent(typeof(BattleUIScript)) as BattleUIScript;
					if (battleUIScript != null)
					{
						battleUIScript.PlayerDead();
					}
				}
				else if (gameState.m_eGameMode.m_ePlayMode == GameState.NetworkGameMode.PlayMode.E_DeathMatch)
				{
					Player recipient = PlayerManager.Instance.GetRecipient(m_iKillerID);
					if (recipient != null)
					{
						((TopWatchingCameraScript)gameScene.GetCamera()).SetCameraTarget(recipient.GetTransform());
					}
				}
				else if (gameState.m_eGameMode.m_ePlayMode == GameState.NetworkGameMode.PlayMode.E_LastStand && m_iKillerID != -1)
				{
					Player recipient2 = PlayerManager.Instance.GetRecipient(m_iKillerID);
					if (recipient2 != null)
					{
						((TopWatchingCameraScript)gameScene.GetCamera()).SetCameraTarget(recipient2.GetTransform());
					}
				}
				GameApp.GetInstance().GetGameState().AddGCPlayerDeadTimes();
				if (gameState.m_eGameMode.m_ePlayMode == GameState.NetworkGameMode.PlayMode.E_Console)
				{
					((TopWatchingCameraScript)gameScene.GetCamera()).ShowPlayerDeadEffect();
				}
				if ((bool)gameCamera.GetComponent<AudioSource>())
				{
					gameCamera.GetComponent<AudioSource>().Stop();
				}
				if ((bool)gameCamera.loseAudio)
				{
					gameCamera.loseAudio.Play();
				}
				GameCollectionInfoManager.Instance().GetCurrentInfo().AddUIEnterLog(GameCollectionInfo.enUIEnterIndex.BattleUI_Dead);
			}
			else
			{
				GameApp.GetInstance().GetGameState().AddGCFriendPlayerDeadTimes();
				if (GameApp.GetInstance().GetGameState().SoundOn)
				{
					string text = "Zombie3D/Audio/RealPersonSound/FriendIsDead_/FriendIsDead_";
					text += UnityEngine.Random.Range(0, 4);
					AudioManager.PlayMusicOnce(text, playerTransform);
				}
			}
		}

		public void GetHealed(int point)
		{
			hp += point;
			hp = Mathf.Clamp(hp, 0f, maxHp);
		}

		public void GetFullyHealed()
		{
			hp = maxHp;
		}

		public void ChangeWeapon(Weapon w, bool bIsNetwork = false)
		{
			Debug.Log(string.Concat("ChangeWeapon - Success - ", w.GetWeaponType(), "|", w.Name));
			if (weapon != null)
			{
				weapon.GunOff();
			}
			weapon = w;
			weapon.changeReticle();
			weapon.Init(this);
			weapon.GunOn();
			audioPlayer.PlaySound("Switch");
			weaponNameEnd = GetWeaponAnimSuffix(weapon.GetWeaponType());
			gameCamera.isAngelVFixed = false;
			if (GameApp.GetInstance().GetGameState().m_eGameMode.m_ePlayMode != 0 && !bIsNetwork)
			{
				GameSetup.Instance.ReqChangeWeapon((int)weapon.GetWeaponType());
			}
			if (!m_bIsNetControl)
			{
				CalcAttack();
				CalcAttackSpeedAdditive();
				CalcDefence();
				CalcMaxHp();
				CalcWalkSpeed();
			}
		}

		public static string GetWeaponAnimSuffix(WeaponType weapon_type)
		{
			switch (weapon_type)
			{
			case WeaponType.Tomahawk:
			case WeaponType.Volcano:
			case WeaponType.Hellfire:
			case WeaponType.NeutronRifle:
			case WeaponType.BigFirework:
			case WeaponType.Stormgun:
				return "_RPG";
			case WeaponType.RemingtonPipe:
			case WeaponType.ParkerGaussRifle:
			case WeaponType.ZombieBusters:
			case WeaponType.MassacreCannon:
			case WeaponType.DoubleSnake:
			case WeaponType.Longinus:
			case WeaponType.Messiah:
				return "_Shotgun";
			case WeaponType.Beretta_33:
			case WeaponType.UZI_E:
			case WeaponType.Barrett_P90:
			case WeaponType.SimonovPistol:
			case WeaponType.Nailer:
			case WeaponType.Lightning:
			case WeaponType.CrossBow:
			case WeaponType.Ion_CannonSub:
				return "_Two";
			default:
				return string.Empty;
			}
		}

		public void ChangeAvatar(Avatar.AvatarSuiteType suite_type, Avatar.AvatarType avt_type)
		{
			foreach (Avatar key in m_Avatars.Keys)
			{
				if (key != null && (bool)m_Avatars[key] && key.AvtType == avt_type)
				{
					m_Avatars[key] = false;
					break;
				}
			}
			foreach (Avatar key2 in m_Avatars.Keys)
			{
				if (key2.SuiteType == suite_type && key2.AvtType == avt_type)
				{
					m_Avatars[key2] = true;
					break;
				}
			}
		}

		public void LoadAvatars()
		{
			Renderer[] componentsInChildren = PlayerObject.GetComponentsInChildren<Renderer>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				if (componentsInChildren[i].transform.parent.gameObject == PlayerObject && componentsInChildren[i].transform.name != "shadow")
				{
					componentsInChildren[i].enabled = false;
				}
			}
			for (int j = 0; j < componentsInChildren.Length; j++)
			{
				if (!(componentsInChildren[j].transform.parent.gameObject == PlayerObject) || !(componentsInChildren[j].transform.name != "shadow"))
				{
					continue;
				}
				foreach (Avatar key in m_Avatars.Keys)
				{
					if (!(bool)m_Avatars[key])
					{
						continue;
					}
					for (int k = 0; k < key.MeshPathList.Count; k++)
					{
						if (componentsInChildren[j].gameObject.name == key.MeshPathList[k])
						{
							componentsInChildren[j].enabled = true;
							componentsInChildren[j].material = Resources.Load("Zombie3D/Avatar/" + key.MatPathList[k]) as Material;
						}
					}
				}
			}
			CalcMaxStamina();
			if (Stamina > m_MaxStamina)
			{
				Stamina = m_MaxStamina;
			}
		}

		public void SetAvatarEffect()
		{
			foreach (Avatar key in m_Avatars.Keys)
			{
				if (!(bool)m_Avatars[key])
				{
					continue;
				}
				if (key.SuiteType == Avatar.AvatarSuiteType.Gladiator && key.AvtType == Avatar.AvatarType.Body)
				{
					GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load("Zombie3D/AvatarEffect/Gladiator_body"), PlayerObject.transform.position, PlayerObject.transform.rotation) as GameObject;
					gameObject.transform.parent = PlayerObject.transform;
					gameObject.transform.localPosition = new Vector3(0f, 0.2f, 0f);
				}
				if (key.SuiteType == Avatar.AvatarSuiteType.Hacker && key.AvtType == Avatar.AvatarType.Body)
				{
					GameObject gameObject2 = UnityEngine.Object.Instantiate(Resources.Load("Zombie3D/AvatarEffect/Hacker_body"), PlayerObject.transform.position, PlayerObject.transform.rotation) as GameObject;
					gameObject2.transform.parent = PlayerObject.transform;
				}
				if (key.SuiteType == Avatar.AvatarSuiteType.X800 && key.AvtType == Avatar.AvatarType.Head)
				{
					GameObject gameObject3 = UnityEngine.Object.Instantiate(Resources.Load("Zombie3D/AvatarEffect/X800_head"), PlayerObject.transform.position, PlayerObject.transform.rotation) as GameObject;
					Transform transform = PlayerObject.transform.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1");
					if (transform != null)
					{
						gameObject3.transform.parent = transform;
						gameObject3.transform.localRotation = Quaternion.Euler(270f, 90f, 0f);
						gameObject3.transform.localPosition = new Vector3(0.548f, -0.058f, 0f);
					}
				}
				if (key.SuiteType == Avatar.AvatarSuiteType.X800 && key.AvtType == Avatar.AvatarType.Body)
				{
					GameObject gameObject4 = UnityEngine.Object.Instantiate(Resources.Load("Zombie3D/AvatarEffect/X800_body"), PlayerObject.transform.position, PlayerObject.transform.rotation) as GameObject;
					Transform transform2 = PlayerObject.transform.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1");
					if (transform2 != null)
					{
						gameObject4.transform.parent = transform2;
						gameObject4.transform.localRotation = Quaternion.Euler(270f, 90f, 0f);
						gameObject4.transform.localPosition = new Vector3(0.548f, -0.058f, 0f);
					}
				}
				if (key.SuiteType == Avatar.AvatarSuiteType.ViolenceFr && key.AvtType == Avatar.AvatarType.Head)
				{
					GameObject gameObject5 = UnityEngine.Object.Instantiate(Resources.Load("Zombie3D/AvatarEffect/ViolenceFr_head"), PlayerObject.transform.position, PlayerObject.transform.rotation) as GameObject;
					gameObject5.transform.parent = PlayerObject.transform;
					Transform transform3 = PlayerObject.transform.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Neck/Bip01 Head/Bip01 HeadNub");
					if (transform3 != null)
					{
						gameObject5.transform.parent = transform3;
						gameObject5.transform.localRotation = Quaternion.Euler(270f, 90.56f, 0f);
						gameObject5.transform.localPosition = new Vector3(0.525f, -0.222f, 0.086f);
						Transform transform4 = gameObject5.transform.Find("ViolenceFr_01");
						transform4.GetComponent<Animation>().clip.wrapMode = WrapMode.Loop;
						transform4.GetComponent<Animation>().Play(transform4.GetComponent<Animation>().clip.name);
					}
				}
				if (key.SuiteType == Avatar.AvatarSuiteType.Ninjalong && key.AvtType == Avatar.AvatarType.Head)
				{
					GameObject gameObject6 = UnityEngine.Object.Instantiate(Resources.Load("Zombie3D/AvatarEffect/NinjalongEffect_head"), PlayerObject.transform.position, PlayerObject.transform.rotation) as GameObject;
					Transform transform5 = PlayerObject.transform.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Neck/Bip01 Head/Bip01 HeadNub");
					if (transform5 != null)
					{
						gameObject6.transform.parent = transform5;
						gameObject6.transform.localPosition = new Vector3(1.472f, 0.01f, 0.01f);
					}
				}
				if (key.SuiteType == Avatar.AvatarSuiteType.Ninjalong && key.AvtType == Avatar.AvatarType.Body)
				{
					GameObject gameObject7 = UnityEngine.Object.Instantiate(Resources.Load("Zombie3D/AvatarEffect/NinjalongEffect_body"), PlayerObject.transform.position, PlayerObject.transform.rotation) as GameObject;
					gameObject7.transform.parent = PlayerObject.transform;
				}
				if (key.SuiteType == Avatar.AvatarSuiteType.SuperNemesis && key.AvtType == Avatar.AvatarType.Head)
				{
					Renderer[] componentsInChildren = PlayerObject.gameObject.GetComponentsInChildren<Renderer>();
					for (int i = 0; i < componentsInChildren.Length; i++)
					{
						if (!(componentsInChildren[i].transform.parent.gameObject == PlayerObject.gameObject) || !(componentsInChildren[i].transform.name != "shadow"))
						{
							continue;
						}
						for (int j = 0; j < key.MeshPathList.Count; j++)
						{
							if (componentsInChildren[i].gameObject.name == key.MeshPathList[j])
							{
								AvatarEffect01 avatarEffect = componentsInChildren[i].gameObject.AddComponent<AvatarEffect01>();
								if (avatarEffect != null)
								{
									avatarEffect.m_AnimPeriod = 1f;
									avatarEffect.m_StartColor = new Color(1f, 1f, 1f, 0f);
									avatarEffect.m_EndColor = new Color(1f, 1f, 1f, 1f);
								}
							}
						}
					}
				}
				if ((key.SuiteType == Avatar.AvatarSuiteType.SuperNemesis || key.SuiteType == Avatar.AvatarSuiteType.DemonLord) && key.AvtType == Avatar.AvatarType.Body)
				{
					Renderer[] componentsInChildren2 = PlayerObject.gameObject.GetComponentsInChildren<Renderer>();
					for (int k = 0; k < componentsInChildren2.Length; k++)
					{
						if (!(componentsInChildren2[k].transform.parent.gameObject == PlayerObject.gameObject) || !(componentsInChildren2[k].transform.name != "shadow") || !(componentsInChildren2[k].transform.parent.gameObject == PlayerObject.gameObject) || !(componentsInChildren2[k].transform.name != "shadow"))
						{
							continue;
						}
						for (int l = 0; l < key.MeshPathList.Count; l++)
						{
							if (componentsInChildren2[k].gameObject.name == key.MeshPathList[l])
							{
								AvatarEffect01 avatarEffect2 = componentsInChildren2[k].gameObject.AddComponent<AvatarEffect01>();
								if (avatarEffect2 != null)
								{
									avatarEffect2.m_AnimPeriod = 1f;
									avatarEffect2.m_StartColor = new Color(1f, 1f, 1f, 0f);
									avatarEffect2.m_EndColor = new Color(1f, 1f, 1f, 1f);
								}
							}
						}
					}
				}
				if (key.SuiteType == Avatar.AvatarSuiteType.Shinobi && key.AvtType == Avatar.AvatarType.Body)
				{
					GameObject gameObject8 = UnityEngine.Object.Instantiate(Resources.Load("Zombie3D/AvatarEffect/Shinobi_body"), PlayerObject.transform.position, PlayerObject.transform.rotation) as GameObject;
					gameObject8.transform.parent = PlayerObject.transform;
					gameObject8.transform.localPosition = new Vector3(0f, 0.2f, 0f);
				}
				if (key.SuiteType == Avatar.AvatarSuiteType.Kunoichi && key.AvtType == Avatar.AvatarType.Body)
				{
					GameObject gameObject9 = UnityEngine.Object.Instantiate(Resources.Load("Zombie3D/AvatarEffect/Kunoichi_body"), PlayerObject.transform.position, PlayerObject.transform.rotation) as GameObject;
					gameObject9.transform.parent = PlayerObject.transform;
					gameObject9.transform.localPosition = new Vector3(0f, 0.2f, 0f);
				}
				if (key.SuiteType == Avatar.AvatarSuiteType.Eskimo && key.AvtType == Avatar.AvatarType.Body)
				{
					GameObject gameObject10 = UnityEngine.Object.Instantiate(Resources.Load("Zombie3D/AvatarEffect/Eskimo_body"), PlayerObject.transform.position, PlayerObject.transform.rotation) as GameObject;
					gameObject10.transform.parent = PlayerObject.transform;
					gameObject10.transform.localPosition = new Vector3(0f, 0.2f, 0f);
				}
				if (key.SuiteType == Avatar.AvatarSuiteType.DemonLord && key.AvtType == Avatar.AvatarType.Body)
				{
					GameObject gameObject11 = new GameObject();
					gameObject11.name = "DemonLord_Fire";
					gameObject11.transform.parent = PlayerObject.transform;
					gameObject11.transform.localPosition = Vector3.zero;
					AvatarEffect03 avatarEffect3 = gameObject11.AddComponent<AvatarEffect03>();
					if (avatarEffect3 != null)
					{
						avatarEffect3.genTimeInterval = 0.1f;
					}
				}
			}
		}

		public void OnPickUp(ItemType itemID)
		{
			audioPlayer.PlaySound("GetItem");
		}

		public Weapon GetWeapon()
		{
			return weapon;
		}

		public void ChangeToPowerBuffState()
		{
			powerBuff = 5f;
			powerBuffStartTime = Time.time;
		}

		public void ChangeToNormalState()
		{
			powerBuff = 1f;
			Color color = new Color(0.8f, 0.8f, 0.8f);
		}

		public void AddPowerUps(ItemType item_type, int count)
		{
			if (m_PowerUPS[(int)item_type] != null)
			{
				m_PowerUPS[(int)item_type] = (int)m_PowerUPS[(int)item_type] + count;
			}
			else
			{
				m_PowerUPS[(int)item_type] = count;
			}
		}

		public bool UsePowerUps(ItemType item_type)
		{
			bool flag = true;
			FixedConfig.PowerUPSCfg powerUPSCfg = ConfigManager.Instance().GetFixedConfig().GetPowerUPSCfg(item_type);
			switch (item_type)
			{
			case ItemType.Hp:
				HP = Mathf.Clamp(HP + 0.2f * maxHp, 0f, maxHp);
				break;
			case ItemType.PowerSmall:
				CalcMaxStamina();
				Stamina += powerUPSCfg.stamina;
				Stamina = Mathf.Min(Stamina, m_MaxStamina);
				if (GameApp.GetInstance().GetGameState().SinewResumeSpeed < powerUPSCfg.staminaSpeedAdd)
				{
					GameApp.GetInstance().GetGameState().SinewResumeSpeed = 0f;
					GameApp.GetInstance().GetGameState().SinewResumeSpeed = powerUPSCfg.staminaSpeedAdd;
				}
				break;
			case ItemType.PowerMiddle:
				CalcMaxStamina();
				Stamina += powerUPSCfg.stamina;
				Stamina = Mathf.Min(Stamina, m_MaxStamina);
				if (GameApp.GetInstance().GetGameState().SinewResumeSpeed < powerUPSCfg.staminaSpeedAdd)
				{
					GameApp.GetInstance().GetGameState().SinewResumeSpeed = 0f;
					GameApp.GetInstance().GetGameState().SinewResumeSpeed = powerUPSCfg.staminaSpeedAdd;
				}
				break;
			case ItemType.PowerSpecial:
				CalcMaxStamina();
				Stamina += powerUPSCfg.stamina;
				Stamina = Mathf.Min(Stamina, m_MaxStamina);
				if (GameApp.GetInstance().GetGameState().SinewResumeSpeed < powerUPSCfg.staminaSpeedAdd)
				{
					GameApp.GetInstance().GetGameState().SinewResumeSpeed = 0f;
					GameApp.GetInstance().GetGameState().SinewResumeSpeed = powerUPSCfg.staminaSpeedAdd;
				}
				break;
			case ItemType.FragGrenade:
			{
				Vector3 vector4 = new Vector3(playerObject.transform.position.x, 10000.1f, playerObject.transform.position.z);
				float num6 = -3f;
				float num7 = 3f;
				Vector3 vector5 = playerObject.transform.forward * num7;
				float num8 = 10f;
				float num9 = num7 / num8;
				float num10 = (num6 - 0.5f * Physics.gravity.y * num9 * num9) / num9;
				Vector3 vector6 = Vector3.up * num10 + vector5.normalized * num8;
				GameObject gameObject5 = UnityEngine.Object.Instantiate(GameApp.GetInstance().GetGameConfig().FragGrenade, playerObject.transform.position + Vector3.up * (0f - num6), Quaternion.LookRotation(-vector6)) as GameObject;
				GrenadeItem component2 = gameObject5.GetComponent<GrenadeItem>();
				component2.explodeTime = 2f;
				component2.damage = powerUPSCfg.damage;
				component2.explodeObj = GameApp.GetInstance().GetGameConfig().Exlposion01;
				gameObject5.GetComponent<Rigidbody>().AddForce(vector6, ForceMode.Impulse);
				string name = "GrenadesColorAnim1";
				AnimationState animationState = gameObject5.GetComponent<Animation>()[name];
				if (animationState != null)
				{
					animationState.wrapMode = WrapMode.Loop;
					gameObject5.GetComponent<Animation>().Play(name);
				}
				break;
			}
			case ItemType.StormGrenade:
			{
				Vector3 vector = new Vector3(playerObject.transform.position.x, 10000.1f, playerObject.transform.position.z);
				float num = -3f;
				float num2 = 3f;
				Vector3 vector2 = playerObject.transform.forward * num2;
				float num3 = 10f;
				float num4 = num2 / num3;
				float num5 = (num - 0.5f * Physics.gravity.y * num4 * num4) / num4;
				Vector3 vector3 = Vector3.up * num5 + vector2.normalized * num3;
				GameObject gameObject = UnityEngine.Object.Instantiate(GameApp.GetInstance().GetGameConfig().StormGrenade, playerObject.transform.position + Vector3.up * (0f - num), Quaternion.LookRotation(-vector3)) as GameObject;
				GrenadeItem component = gameObject.GetComponent<GrenadeItem>();
				component.explodeTime = 2f;
				component.damage = powerUPSCfg.damage;
				component.explodeObj = GameApp.GetInstance().GetGameConfig().Exlposion03;
				gameObject.GetComponent<Rigidbody>().AddForce(vector3, ForceMode.Impulse);
				break;
			}
			case ItemType.HpSmall:
			{
				HP += GetMaxHp() * Mathf.Clamp01(powerUPSCfg.hp);
				if (HP > GetMaxHp())
				{
					HP = GetMaxHp();
				}
				GameObject gameObject4 = UnityEngine.Object.Instantiate(Resources.Load("Zombie3D/PlayerBuff/hp"), PlayerObject.transform.position, Quaternion.identity) as GameObject;
				RemoveTimerScript removeTimerScript3 = gameObject4.AddComponent(typeof(RemoveTimerScript)) as RemoveTimerScript;
				removeTimerScript3.life = 2f;
				gameObject4.transform.parent = PlayerObject.transform;
				gameObject4.transform.localPosition = new Vector3(0f, 1.8f, 0f);
				break;
			}
			case ItemType.HpMiddle:
			{
				HP += GetMaxHp() * Mathf.Clamp01(powerUPSCfg.hp);
				if (HP > GetMaxHp())
				{
					HP = GetMaxHp();
				}
				GameObject gameObject2 = UnityEngine.Object.Instantiate(Resources.Load("Zombie3D/PlayerBuff/hp"), PlayerObject.transform.position, Quaternion.identity) as GameObject;
				RemoveTimerScript removeTimerScript = gameObject2.AddComponent(typeof(RemoveTimerScript)) as RemoveTimerScript;
				removeTimerScript.life = 2f;
				gameObject2.transform.parent = PlayerObject.transform;
				gameObject2.transform.localPosition = new Vector3(0f, 1.8f, 0f);
				break;
			}
			case ItemType.HpLarge:
			{
				HP += GetMaxHp() * Mathf.Clamp01(powerUPSCfg.hp);
				if (HP > GetMaxHp())
				{
					HP = GetMaxHp();
				}
				GameObject gameObject3 = UnityEngine.Object.Instantiate(Resources.Load("Zombie3D/PlayerBuff/hp"), PlayerObject.transform.position, Quaternion.identity) as GameObject;
				RemoveTimerScript removeTimerScript2 = gameObject3.AddComponent(typeof(RemoveTimerScript)) as RemoveTimerScript;
				removeTimerScript2.life = 2f;
				gameObject3.transform.parent = PlayerObject.transform;
				gameObject3.transform.localPosition = new Vector3(0f, 1.8f, 0f);
				break;
			}
			case ItemType.Shield:
				if (m_bPowerUpsShieldFac)
				{
					flag = false;
					break;
				}
				m_bPowerUpsShieldFac = true;
				m_PowerUpsShieldDamage = 150f;
				m_BuffEffectShield = UnityEngine.Object.Instantiate(Resources.Load("Zombie3D/PlayerBuff/fense_02"), PlayerObject.transform.position, Quaternion.identity) as GameObject;
				m_BuffEffectShield.transform.parent = PlayerObject.transform;
				m_BuffEffectShield.transform.localPosition = Vector3.zero;
				m_BuffEffectShield.transform.GetChild(0).GetComponent<Animation>().clip.wrapMode = WrapMode.Loop;
				m_BuffEffectShield.transform.GetChild(0).GetComponent<Animation>().Play(m_BuffEffectShield.transform.GetChild(0).GetComponent<Animation>().clip.name);
				break;
			case ItemType.Doping:
				if (m_bPowerUpsAttackFac)
				{
					if (m_PowerUpsAttackTime >= powerUPSCfg.keepTime)
					{
						flag = false;
						break;
					}
					m_bPowerUpsAttackFac = true;
					m_PowerUpsAttackStartTime = Time.time;
					m_PowerUpsAttackTime = powerUPSCfg.keepTime;
					if (powerUPSCfg.keepTime < 0f)
					{
						m_PowerUpsAttackTime = 100000000f;
					}
					m_PowerUpsAttackAdd = powerUPSCfg.damagePercent;
					CalcAttack();
					break;
				}
				m_bPowerUpsAttackFac = true;
				m_PowerUpsAttackStartTime = Time.time;
				m_PowerUpsAttackTime = powerUPSCfg.keepTime;
				if (powerUPSCfg.keepTime < 0f)
				{
					m_PowerUpsAttackTime = 100000000f;
				}
				m_PowerUpsAttackAdd = powerUPSCfg.damagePercent;
				CalcAttack();
				m_BuffEffectAssault = UnityEngine.Object.Instantiate(Resources.Load("Zombie3D/PlayerBuff/assault"), PlayerObject.transform.position, Quaternion.identity) as GameObject;
				m_BuffEffectAssault.transform.parent = PlayerObject.transform;
				m_BuffEffectAssault.transform.localPosition = Vector3.zero;
				break;
			case ItemType.NuclearCocacola:
				if (m_bPowerUpsAttackFac)
				{
					if (m_PowerUpsAttackTime >= powerUPSCfg.keepTime)
					{
						flag = false;
						break;
					}
					m_bPowerUpsAttackFac = true;
					m_PowerUpsAttackStartTime = Time.time;
					m_PowerUpsAttackTime = powerUPSCfg.keepTime;
					if (powerUPSCfg.keepTime < 0f)
					{
						m_PowerUpsAttackTime = 100000000f;
					}
					m_PowerUpsAttackAdd = powerUPSCfg.damagePercent;
					CalcAttack();
					break;
				}
				m_bPowerUpsAttackFac = true;
				m_PowerUpsAttackStartTime = Time.time;
				m_PowerUpsAttackTime = powerUPSCfg.keepTime;
				if (powerUPSCfg.keepTime < 0f)
				{
					m_PowerUpsAttackTime = 100000000f;
				}
				m_PowerUpsAttackAdd = powerUPSCfg.damagePercent;
				CalcAttack();
				m_BuffEffectAssault = UnityEngine.Object.Instantiate(Resources.Load("Zombie3D/PlayerBuff/assault"), PlayerObject.transform.position, Quaternion.identity) as GameObject;
				m_BuffEffectAssault.transform.parent = PlayerObject.transform;
				m_BuffEffectAssault.transform.localPosition = Vector3.zero;
				break;
			case ItemType.Pacemaker:
				flag = false;
				break;
			case ItemType.Defibrilator:
				flag = false;
				break;
			}
			if (flag)
			{
				Debug.Log("Use PowerUPS: " + item_type);
				int num11 = (int)m_PowerUPS[(int)item_type] - 1;
				m_PowerUPS[(int)item_type] = ((num11 > 0) ? num11 : 0);
			}
			else
			{
				Debug.Log("Cannot Use PowerUPS: " + item_type);
			}
			if (GameApp.GetInstance().GetGameState().SoundOn)
			{
				string text = "Zombie3D/Audio/RealPersonSound/UseGoods/UseGoods_";
				text += UnityEngine.Random.Range(0, 5);
				AudioManager.PlayMusicOnce(text, playerTransform);
			}
			GameCollectionInfoManager.Instance().GetCurrentInfo().AddTodayPowerUpsUsed((int)item_type, 1);
			return flag;
		}

		public void SetupSpeedUpEffect()
		{
			if (m_BuffEffectSpeedUp == null)
			{
				m_BuffEffectSpeedUp = UnityEngine.Object.Instantiate(Resources.Load("Zombie3D/PlayerBuff/SpeedUp"), PlayerObject.transform.position, Quaternion.identity) as GameObject;
				m_BuffEffectSpeedUp.transform.parent = PlayerObject.transform;
				m_BuffEffectSpeedUp.transform.localPosition = new Vector3(0f, 0.8f, 0f);
			}
		}

		public void UpdateAvatarEffect()
		{
		}

		public void UseActiveSkill()
		{
			Skill skill = null;
			List<Skill> playerSkilles = GameApp.GetInstance().GetGameState().GetPlayerSkilles();
			foreach (Skill item in playerSkilles)
			{
				if (item.SkillType == GameApp.GetInstance().GetGameState().m_CurSkillType)
				{
					skill = item;
					break;
				}
			}
			if (skill == null)
			{
				return;
			}
			switch (skill.SkillType)
			{
			case enSkillType.FastRun:
				m_ActiveSkillImpl = new SkillFastRun();
				m_ActiveSkillImpl.Init(this, skill);
				break;
			case enSkillType.BuildCannon:
				m_ActiveSkillImpl = new SkillBuildCannon();
				m_ActiveSkillImpl.Init(this, skill);
				break;
			case enSkillType.ThrowGrenade:
				m_ActiveSkillImpl = new SkillThrowGrenade();
				if (m_Stamina >= ((SkillThrowGrenade)m_ActiveSkillImpl).m_StaminaSpend)
				{
					m_ActiveSkillImpl.Init(this, skill);
					m_ActiveSkillImpl = null;
				}
				else
				{
					m_ActiveSkillImpl = null;
				}
				break;
			case enSkillType.CoverMe:
				m_ActiveSkillImpl = new SkillCoverMe();
				m_ActiveSkillImpl.Init(this, skill);
				friendSkillMoveDistance = -GetTransform().forward * 2f;
				if (GameApp.GetInstance().GetGameScene().GetFriendPlayer() != null && GameApp.GetInstance().GetGameScene().GetFriendPlayer()
					.HP > 0f)
				{
					GameApp.GetInstance().GetGameScene().GetFriendPlayer()
						.SetState(RUN_STATE);
				}
				break;
			case enSkillType.DoubleTeam:
				m_ActiveSkillImpl = new SkillDoubleTeam();
				m_ActiveSkillImpl.Init(this, skill);
				friendSkillMoveDistance = GetTransform().InverseTransformPoint(GetTransform().position + GetTransform().right * 2f);
				if (GameApp.GetInstance().GetGameScene().GetFriendPlayer() != null && GameApp.GetInstance().GetGameScene().GetFriendPlayer()
					.HP > 0f)
				{
					GameApp.GetInstance().GetGameScene().GetFriendPlayer()
						.SetState(RUN_STATE);
				}
				break;
			}
		}

		public void TerminateActiveSkill()
		{
			ActiveSkillImpl.Stop();
			ActiveSkillImpl = null;
		}

		public void NPlayerOnHitted(float damage)
		{
			if (GameApp.GetInstance().GetGameState().m_eGameMode.m_ePlayMode != 0)
			{
				int recipientId = PlayerManager.Instance.GetRecipientId(this);
				if (recipientId != -1)
				{
					GameSetup.Instance.ReqSyncPlayerInfo(GameSetup.NPlayerDataType.E_Hitted, damage, recipientId);
				}
				else
				{
					Debug.LogError("Wrong player Id");
				}
				if (PlayerManager.Instance.GetPlayerClass().GetNBattleItemImpl(enBattlefieldProps.E_AnaestheticProjectile).NumberOfUse >= 0)
				{
					PlayerManager.Instance.GetPlayerClass().CheckVertigo(recipientId);
				}
			}
		}

		public bool ModificationWeaponList(WeaponType wType, bool bNeedChange = false, int index = 0)
		{
			if (weaponList == null)
			{
				return false;
			}
			if (weaponList.Count >= 0)
			{
				if (weaponList.Count == 1)
				{
					weaponList.Add(wType);
					NBattleUIScript nBattleUIScript = SceneUIManager.Instance().GetSceneUIObject().GetComponent(typeof(NBattleUIScript)) as NBattleUIScript;
					if (nBattleUIScript != null)
					{
						nBattleUIScript.SetupBattleUI(true);
					}
					if (bNeedChange)
					{
						Weapon w = WeaponFactory.GetInstance().CreateWeapon(wType);
						ChangeWeapon(w);
					}
					return true;
				}
				if (weaponList.Contains(wType))
				{
					Debug.Log("Modification WeaponList Error. Because you already have it.");
					return false;
				}
				index = (int)Mathf.Clamp01(index);
				weaponList.RemoveAt(index);
				weaponList.Insert(index, wType);
				if (bNeedChange)
				{
					Weapon w2 = WeaponFactory.GetInstance().CreateWeapon(wType);
					ChangeWeapon(w2);
				}
				return true;
			}
			return false;
		}

		public Dictionary<enBattlefieldProps, NBattleShopItemImpl> GetNBattleItemList()
		{
			return m_NBattleShopItemList;
		}

		public NBattleShopItemImpl GetNBattleItemImpl(enBattlefieldProps type)
		{
			if (m_NBattleShopItemList.ContainsKey(type))
			{
				return m_NBattleShopItemList[type];
			}
			return null;
		}

		public void SetBloodVisable(bool bShow)
		{
			GameObject gameObject = playerObject.transform.Find("BloodRect").gameObject;
			if (gameObject != null)
			{
				gameObject.SetActiveRecursively(bShow);
			}
		}

		public void Clear()
		{
			weapon.Clear();
			UnityEngine.Object.Destroy(playerObject);
		}
	}
}
