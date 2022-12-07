using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zombie3D;

public class BattleUIScript : MonoBehaviour, UIHandler
{
	public enum Controls
	{
		kIDGamePause = 3000,
		kIDMoveJoystickBg = 3001,
		kIDMoveJoystickBtn = 3002,
		kIDShootJoystickBg = 3003,
		kIDShootJoystickBtn = 3004,
		kIDSwapWeaponBtn = 3005,
		kIDBattleItem = 3006,
		kIDBattleSpeedUp = 3007,
		kIDBattleItemResume = 3008,
		kIDPlayerDontResurrectionYes = 3009,
		kIDPlayerResurrectionYes = 3010,
		kIDPlayerResurrectionNo = 3011,
		kIDGameStartTap = 3012,
		kIDCameraModeDialogOK = 3013,
		kIDCameraModeTypeOne = 3014,
		kIDCameraModeTypeTwo = 3015,
		kIDSureCameraMode = 3016,
		kIDGameBeginAnim = 3017,
		kIDPlayNextWave = 3018,
		kIDPlayNextWaveText = 3019,
		kIDBattleItemUseErrorOK = 3020,
		kIDMap3RewardOK = 3021,
		kIDBattleItemBegin = 3022,
		kIDBattleItemLast = 3042,
		kIDLast = 3043
	}

	private UIManager m_UIManager;

	private string m_ui_material_path;

	protected Material m_MatCommonBg;

	protected Material m_MatBattleUI;

	protected Material m_MatPowerUPSIcons;

	protected Material m_MatDialog01;

	protected Material m_MatGamtStartEffect;

	protected Material m_MatPerfectWaveEffect;

	protected Material m_MatBattleDialog;

	public uiGroup m_DesktopGroup;

	public uiGroup m_ItemBarGroup;

	public uiGroup m_ControlCoverBarGroup;

	public uiGroup m_EnemiesDirectionGroup;

	public uiGroup m_EnemiesLeftInfoGroup;

	public uiGroup m_SurvivalModeArrowGroup;

	public uiGroup m_uiHintDialog;

	public uiGroup m_uiMap3WinRewardDialog;

	public UIGroupControl m_DeadShowDialog;

	public uiGroup m_uiCameraGroup;

	public uiGroup m_uiCameraDialogGroup;

	private UIImage m_FriendDirImg;

	private UIImage m_MoveJoystickBg;

	private UIJoystickButtonEx m_MoveJoystickBtn;

	private UIImage m_ShootJoystickBg;

	private UIJoystickButtonEx m_ShootJoystickBtn;

	private UIProgressBarRounded playerHpProgressBar;

	private UIProgressBarRounded playerStaminaProgressBar;

	private UIText m_LevelCountDownTimer;

	private UIAnimationControl playerHpProgressBarAnim;

	private UIAnimationControl playerStaminaProgressBarAnim;

	private UIAnimationControl playerHpDecreaseAnim;

	private float m_GameBeginAnimStartTime = -1f;

	private UIPushButton m_SpeedUpButton;

	private bool m_bCurrentSpeedUpState;

	private UIPushButton m_SkillBuildCannon;

	private bool m_bSkillBuildCannonStarted;

	private UIClickButton m_SkillThrowGrenade;

	private UIPushButton m_SkillCoverMe;

	private bool m_bSkillCoverMeStarted;

	private UIPushButton m_SkillDoubleTeam;

	private bool m_bSkillDoubleTeamStarted;

	private UIScrollPageView m_PowerUpsView;

	private UIDotScrollBar m_PowerUpsScrollBar;

	protected GameState gameState;

	protected GameScene gameScene;

	protected Player player;

	protected float lastUpdateTime;

	protected bool uiInited;

	protected float joystickBgImgAlpha = 0.3f;

	private bool m_bGameBeginEffectShow;

	private bool m_bGameBeginEffectBack;

	private bool m_bResurrectionPlayer;

	private float m_ResurrectionAnimStartTime;

	private bool m_bResurrectionFriendPlayer;

	private float m_ResurrectionAnimFriendStartTime;

	private float m_Map3WinTimer = -1f;

	private float m_Map3WinTime = 2f;

	private List<KeyValuePair<int, int>> m_Map3RewardItem;

	private bool m_bStagePassed;

	private float m_PlayerDeadTimer = -1f;

	private float m_StagePassedGotoNextSceneUITimer = -1f;

	private float m_lastHp;

	private float m_hpProgressBarFlashStartTime = -1f;

	private float m_lastStamina;

	private float m_staminaProgressBarFlashStartTime = -1f;

	private uiGroup m_EffectGroup;

	private UIEffect01 m_Effect01;

	private UIEffect01 m_Effect02;

	private List<UIImage> m_EnemiesDirection;

	private GameLoadingUI m_GameLoadingUI;

	private bool m_bPaused;

	private GameLoadingUI m_GameLoadingSurvivalUI;

	private float m_LastPercent;

	private float m_LastIndicatorUpdateTime;

	private bool m_bSurvivalModeGotoNextSceneIndicator;

	private List<UIAnimationControl> m_SurvivalModeLeftIndicatorDirection;

	private SurvivalModeExitDoor[] m_SurvivalModeExitDoors;

	private Rect[] m_rcActiveSkillBtnTex;

	private UIImage m_BeginGameCountDownImg;

	private uint m_uiCountDownNum = 3u;

	private float m_fBeginGameCountDownTimer = -1f;

	private float m_fBeginGameCountDownTime = 4f;

	public bool GetPaused()
	{
		return m_bPaused;
	}

	private Rect[] m_rectCountDownNumber = new Rect[5]
	{
		new Rect(934f, 875f, 90f, 110f),
		new Rect(934f, 767f, 90f, 110f),
		new Rect(934f, 659f, 90f, 110f),
		new Rect(835f, 659f, 90f, 110f),
		new Rect(835f, 659f, 90f, 110f)
	};

	public void IsCoundDownOver(float seconde)
	{
		string empty = string.Empty;
		empty = ((seconde != 0f) ? UtilsEx.TimeToStr_HMS((long)seconde) : UtilsEx.TimeToStr_HMS(0L));
		m_LevelCountDownTimer.SetText(empty);
		m_LevelCountDownTimer = null;
	}

	private void Start()
	{
		OpenClickPlugin.Hide();
		gameState = GameApp.GetInstance().GetGameState();
		gameScene = GameApp.GetInstance().GetGameScene();
		player = gameScene.GetPlayer();
		m_UIManager = base.gameObject.AddComponent<UIManager>() as UIManager;
		m_UIManager.SetViewPortInCenter(false);
		m_UIManager.SetParameter(8, 3, false);
		m_UIManager.SetUIHandler(this);
		m_MatCommonBg = LoadUIMaterial("Zombie3D/UI/Materials/CommonBgUI");
		m_MatBattleUI = LoadUIMaterial("Zombie3D/UI/Materials/BattleUI");
		m_MatDialog01 = LoadUIMaterial("Zombie3D/UI/Materials/Dialog01");
		m_MatPerfectWaveEffect = LoadUIMaterial("Zombie3D/UI/Materials/PerfectWaveEffectUI");
		Resources.UnloadUnusedAssets();
		SceneUIManager.Instance().SetMusicPlayerState(SceneUIManager.MusicState_BattleAudioState);
		uiInited = true;
		m_bPaused = false;
		m_bCurrentSpeedUpState = player.SpeedUpByStamina;
		m_lastHp = player.GetHp();
		m_lastStamina = player.GetStamina();
		m_rcActiveSkillBtnTex = new Rect[3]
		{
			new Rect(454f, 396f, 96f, 63f),
			new Rect(454f, 459f, 96f, 63f),
			new Rect(454f, 396f, 96f, 63f)
		};
		enSkillType enSkillType = enSkillType.FastRun;
		switch ((gameState.m_eGameMode.m_ePlayMode != 0) ? enSkillType.FastRun : gameState.m_CurSkillType)
		{
		case enSkillType.FastRun:
			m_rcActiveSkillBtnTex = new Rect[3]
			{
				new Rect(454f, 396f, 96f, 63f),
				new Rect(454f, 459f, 96f, 63f),
				new Rect(454f, 396f, 96f, 63f)
			};
			break;
		case enSkillType.BuildCannon:
			m_rcActiveSkillBtnTex = new Rect[3]
			{
				new Rect(358f, 396f, 96f, 63f),
				new Rect(358f, 459f, 96f, 63f),
				new Rect(358f, 396f, 96f, 63f)
			};
			break;
		case enSkillType.ThrowGrenade:
			m_rcActiveSkillBtnTex = new Rect[3]
			{
				new Rect(358f, 270f, 96f, 63f),
				new Rect(358f, 333f, 96f, 63f),
				new Rect(358f, 270f, 96f, 63f)
			};
			break;
		case enSkillType.CoverMe:
			m_rcActiveSkillBtnTex = new Rect[3]
			{
				new Rect(262f, 270f, 96f, 63f),
				new Rect(262f, 333f, 96f, 63f),
				new Rect(262f, 270f, 96f, 63f)
			};
			break;
		case enSkillType.DoubleTeam:
			m_rcActiveSkillBtnTex = new Rect[3]
			{
				new Rect(262f, 396f, 96f, 63f),
				new Rect(262f, 459f, 96f, 63f),
				new Rect(262f, 396f, 96f, 63f)
			};
			break;
		}
		if (gameState.m_bBattleIsBegin)
		{
			SetupBattleUI(true);
		}
		m_EnemiesDirection = new List<UIImage>();
		for (int i = 0; i < GameApp.GetInstance().GetGameScene().DDSTrigger.emenySpawnLimit; i++)
		{
			UIImage item = UIUtils.BuildImage(0, new Rect(0f, 0f, 38f, 35f), m_MatBattleUI, new Rect(375f, 54f, 38f, 35f), new Vector2(38f, 35f));
			m_EnemiesDirection.Add(item);
		}
		m_bGameBeginEffectShow = false;
		m_bGameBeginEffectBack = false;
		if (!gameScene.m_bMapStartZoomShow)
		{
			gameScene.m_bMapStartZoomShow = true;
			if (gameState.m_bIsSurvivalMode)
			{
				if (gameState.m_SurvivalModeBattledMapCount == 0)
				{
					((TopWatchingCameraScript)gameScene.GetCamera()).ShowGameStartEffect();
					m_bGameBeginEffectShow = true;
					SetupControlCoverUI(true);
				}
				else
				{
					gameScene.BattleBegin();
				}
			}
			else
			{
				((TopWatchingCameraScript)gameScene.GetCamera()).ShowGameStartEffect();
				m_bGameBeginEffectShow = true;
				SetupControlCoverUI(true);
			}
		}
		if (!gameState.m_bIsSurvivalMode)
		{
			return;
		}
		int num = 0;
		foreach (Enemy value in gameScene.GetEnemies().Values)
		{
			if (value != null && value.HP > 0f)
			{
				num++;
			}
		}
		if (num == 0)
		{
			ShowSurvivalModeIndicatorUI();
		}
	}

	private void Update()
	{
		if (m_bPaused)
		{
			return;
		}
		if (!Application.isMobilePlatform)
		{
			if (Input.GetKeyDown(KeyCode.Tab))
			{
				List<WeaponType> battleWeapons = gameState.GetBattleWeapons();
				if (battleWeapons.Count == 1 || battleWeapons.Count != 2)
				{
					return;
				}
				for (int i = 0; i < 2; i++)
				{
					WeaponType weaponType = battleWeapons[i];
					if (weaponType != player.GetWeapon().GetWeaponType())
					{
						Weapon w = WeaponFactory.GetInstance().CreateWeapon(weaponType);
						player.ChangeWeapon(w);
						break;
					}
				}
			}
			if (Input.GetKeyDown(KeyCode.E))
			{
				m_MoveJoystickBtn.Reset();
				((TopWatchingInputController)player.InputController).MoveDirection = Vector3.zero;
				player.StopRun();
				player.SetState(Player.IDLE_STATE);
				m_ShootJoystickBtn.Reset();
				((TopWatchingInputController)player.InputController).bFire = false;
				player.StopFire();
				SetupBattleItemUI(true);
				GameCollectionInfoManager.Instance().GetCurrentInfo().AddUIEnterLog(GameCollectionInfo.enUIEnterIndex.BattleUI_Item);
			}
		}
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
		if (m_GameLoadingUI != null)
		{
			if (m_StagePassedGotoNextSceneUITimer >= 0f)
			{
				m_StagePassedGotoNextSceneUITimer += num;
				if (m_StagePassedGotoNextSceneUITimer > 0.2f)
				{
					m_StagePassedGotoNextSceneUITimer = -1f;
					gameScene.BattleEnd();
				}
			}
		}
		else
		{
			if (m_GameLoadingSurvivalUI != null)
			{
				return;
			}
			if (m_fBeginGameCountDownTimer >= 0f)
			{
				m_fBeginGameCountDownTimer += num;
				uint num2 = (uint)(m_fBeginGameCountDownTime - m_fBeginGameCountDownTimer);
				if (num2 != m_uiCountDownNum)
				{
					m_uiCountDownNum = num2;
					Material material = LoadUIMaterial("Zombie3D/UI/Materials/NLobbyUI");
					Resources.UnloadUnusedAssets();
					if (m_BeginGameCountDownImg != null)
					{
						m_BeginGameCountDownImg.SetTexture(material, m_rectCountDownNumber[m_uiCountDownNum]);
					}
				}
			}
			if (m_fBeginGameCountDownTimer >= m_fBeginGameCountDownTime)
			{
				BeginGame();
				m_fBeginGameCountDownTimer = -1f;
			}
			if (Time.time - m_LastIndicatorUpdateTime > 0.05f)
			{
				m_LastIndicatorUpdateTime = Time.time;
				if (GameApp.GetInstance().GetGameState().m_eGameMode.m_ePlayMode == GameState.NetworkGameMode.PlayMode.E_Console)
				{
					SetupEnemiesDirectionUI(true);
				}
				Vector3 position = player.GetTransform().position;
				if (gameScene.GetFriendPlayer() != null)
				{
					Vector3 position2 = gameScene.GetFriendPlayer().GetTransform().position;
					if (Vector2.Distance(new Vector2(position.x, position.z), new Vector2(position2.x, position2.z)) > 10f)
					{
						Vector3 normalized = player.GetRespawnTransform().InverseTransformDirection(position2 - position).normalized;
						float num3 = Mathf.Atan2(normalized.z, normalized.x);
						Vector2 vector = new Vector2((float)(Screen.width / 2) + 500f * Mathf.Cos(num3), (float)(Screen.height / 2) + 500f * Mathf.Sin(num3));
						float num4 = vector.x;
						if (num4 < 140f)
						{
							num4 = 140f;
						}
						if (num4 > (float)(Screen.width - 140))
						{
							num4 = Screen.width - 140;
						}
						float num5 = vector.y;
						if (num5 < 120f)
						{
							num5 = 120f;
						}
						if (num5 > (float)(Screen.height - 100))
						{
							num5 = Screen.height - 100;
						}
						vector = new Vector2(num4, num5);
						if (m_FriendDirImg == null)
						{
							m_FriendDirImg = UIUtils.BuildImage(0, new Rect(vector.x, vector.y, 26f, 35f), m_MatBattleUI, new Rect(416f, 54f, 26f, 35f), new Vector2(26f, 35f));
							m_UIManager.Add(m_FriendDirImg);
						}
						m_FriendDirImg.Enable = true;
						m_FriendDirImg.Visible = true;
						m_FriendDirImg.Rect = AutoUI.AutoRect(new Rect(vector.x, vector.y, 26f, 35f));
						m_FriendDirImg.SetRotation(num3);
					}
					else if (m_FriendDirImg != null)
					{
						m_FriendDirImg.Enable = false;
						m_FriendDirImg.Visible = false;
					}
				}
				if (gameState.m_bIsSurvivalMode && Time.frameCount % 5 == 0)
				{
					SetupEnemiesLeftInfoUI(true);
				}
				if ((gameScene.DDSTrigger.MapIndex == 1 || gameScene.DDSTrigger.MapIndex == 2 || gameScene.DDSTrigger.MapIndex == 6 || gameScene.DDSTrigger.MapIndex == 7) && Time.frameCount % 10 == 0)
				{
					SetupPointsWavesInfoUI(true);
				}
				if (m_bSurvivalModeGotoNextSceneIndicator)
				{
					SetupSurvivalModeLeaveArrowUI(true);
				}
			}
			if (m_bGameBeginEffectShow && m_ControlCoverBarGroup != null && !((TopWatchingCameraScript)gameScene.GetCamera()).GetShowGameStartEffect())
			{
				m_bGameBeginEffectShow = false;
				SetupControlCoverUI(true);
				SetupGameStartTap();
			}
			if (m_bGameBeginEffectBack && !((TopWatchingCameraScript)gameScene.GetCamera()).GetShowGameStartEndEffect())
			{
				m_bGameBeginEffectBack = false;
				SetupControlCoverUI(false);
				m_GameBeginAnimStartTime = Time.time;
				SetupGameBeginEffect();
			}
			if (playerHpProgressBar != null)
			{
				if (player.GetHp() < m_lastHp)
				{
					m_lastHp = player.GetHp();
					m_hpProgressBarFlashStartTime = Time.time;
					playerHpProgressBarAnim.Enable = true;
					playerHpProgressBarAnim.Visible = true;
				}
				playerHpProgressBar.SetParam(m_MatBattleUI, AutoUI.AutoRect(new Rect(1f, 0f, 0f, 0f)), m_MatBattleUI, AutoUI.AutoRect(new Rect(320f, 6f, 3f, 17f)), AutoUI.AutoRect(new Rect(323f, 6f, 1f, 17f)), AutoUI.AutoRect(new Rect(327f, 6f, 5f, 17f)), Mathf.Clamp01(player.GetHp() / player.GetMaxHp()));
			}
			if (playerStaminaProgressBar != null)
			{
				if (player.GetStamina() < m_lastStamina)
				{
					m_lastStamina = player.GetStamina();
					m_staminaProgressBarFlashStartTime = Time.time;
					playerStaminaProgressBarAnim.Enable = true;
					playerStaminaProgressBarAnim.Visible = true;
				}
				playerStaminaProgressBar.SetParam(m_MatBattleUI, AutoUI.AutoRect(new Rect(1f, 0f, 0f, 0f)), m_MatBattleUI, AutoUI.AutoRect(new Rect(320f, 35f, 3f, 17f)), AutoUI.AutoRect(new Rect(323f, 35f, 1f, 17f)), AutoUI.AutoRect(new Rect(327f, 35f, 5f, 17f)), Mathf.Clamp01(player.GetStamina() / player.GetMaxStamina()));
			}
			if (m_hpProgressBarFlashStartTime >= 0f && Time.time - m_hpProgressBarFlashStartTime >= 1f)
			{
				playerHpProgressBarAnim.Enable = false;
				playerHpProgressBarAnim.Visible = false;
				m_hpProgressBarFlashStartTime = -1f;
			}
			if (m_staminaProgressBarFlashStartTime >= 0f && Time.time - m_staminaProgressBarFlashStartTime >= 1f)
			{
				playerStaminaProgressBarAnim.Enable = false;
				playerStaminaProgressBarAnim.Visible = false;
				m_staminaProgressBarFlashStartTime = -1f;
			}
			if (m_GameBeginAnimStartTime > 0f && Time.time - m_GameBeginAnimStartTime >= 1.2f && m_DesktopGroup != null)
			{
				m_DesktopGroup.Remove(3017);
				m_GameBeginAnimStartTime = -1f;
				if (m_MatGamtStartEffect == null)
				{
					m_MatGamtStartEffect = null;
				}
			}
			if (m_SpeedUpButton != null && m_bCurrentSpeedUpState && !player.SpeedUpByStamina)
			{
				m_SpeedUpButton.Set(false);
				m_bCurrentSpeedUpState = false;
			}
			switch (gameState.m_CurSkillType)
			{
			case enSkillType.BuildCannon:
				if (m_bSkillBuildCannonStarted && player.ActiveSkillImpl == null)
				{
					m_SkillBuildCannon.Set(false);
					m_bSkillBuildCannonStarted = false;
				}
				break;
			case enSkillType.CoverMe:
				if (gameScene.GetFriendPlayer() == null)
				{
					break;
				}
				if (gameScene.GetFriendPlayer().HP <= 0f)
				{
					m_SkillCoverMe.Set(false);
					m_bSkillCoverMeStarted = false;
					if (player.ActiveSkillImpl != null)
					{
						player.TerminateActiveSkill();
					}
				}
				else if (m_bSkillCoverMeStarted && player.ActiveSkillImpl == null)
				{
					m_SkillCoverMe.Set(false);
					m_bSkillCoverMeStarted = false;
				}
				break;
			case enSkillType.DoubleTeam:
				if (gameScene.GetFriendPlayer() == null)
				{
					break;
				}
				if (gameScene.GetFriendPlayer().HP <= 0f)
				{
					m_SkillDoubleTeam.Set(false);
					m_bSkillDoubleTeamStarted = false;
					if (player.ActiveSkillImpl != null)
					{
						player.TerminateActiveSkill();
					}
				}
				else if (m_bSkillDoubleTeamStarted && player.ActiveSkillImpl == null)
				{
					m_SkillDoubleTeam.Set(false);
					m_bSkillDoubleTeamStarted = false;
				}
				break;
			}
			if (m_Effect01 != null)
			{
				m_Effect01.Update(Time.deltaTime);
				if (m_Effect01.EffectOver())
				{
					m_Effect01.Clear();
					m_Effect01 = null;
					m_EffectGroup.Clear();
					m_EffectGroup = null;
					SceneUIManager.Instance().SetMusicPlayerState(SceneUIManager.MusicState_BattleAudioState);
					if (m_Effect02 == null)
					{
						m_MatPerfectWaveEffect = null;
						if (m_bStagePassed)
						{
							SetupLoadingToExchangeUI();
							if (m_StagePassedGotoNextSceneUITimer < 0f)
							{
								m_StagePassedGotoNextSceneUITimer = 0f;
							}
						}
					}
				}
			}
			if (m_Effect02 != null)
			{
				m_Effect02.Update(Time.deltaTime);
				if (m_Effect02.EffectOver())
				{
					m_Effect02 = null;
					if (m_Effect01 == null)
					{
						m_MatPerfectWaveEffect = null;
						if (m_bStagePassed)
						{
							SetupLoadingToExchangeUI();
							if (m_StagePassedGotoNextSceneUITimer < 0f)
							{
								m_StagePassedGotoNextSceneUITimer = 0f;
							}
						}
					}
				}
			}
			if (m_PlayerDeadTimer >= 0f)
			{
				m_PlayerDeadTimer += Time.deltaTime;
				if (m_PlayerDeadTimer >= 1f)
				{
					m_PlayerDeadTimer = -1f;
					SetupControlCoverUI(false);
					SetupPlayerDeadUI(true);
				}
			}
			if (m_bResurrectionPlayer && Time.time - m_ResurrectionAnimStartTime > 3f)
			{
				m_bResurrectionPlayer = false;
				SetupControlCoverUI(false);
				player.ResurrectionAtCurrentPos();
				if (gameScene.GetFriendPlayer() != null)
				{
					if (gameScene.GetFriendPlayer().HP <= 0f)
					{
						gameScene.GetFriendPlayer().ResurrectionAtCurrentPos();
					}
					gameScene.GetFriendPlayer().SetState(Player.IDLE_STATE);
				}
				if (gameScene.DDSTrigger.MapIndex != 5)
				{
					gameScene.DDSTrigger.Playing = true;
				}
				else if (gameScene.GetEnergyFeedways().Count > 0)
				{
					gameScene.DDSTrigger.Playing = true;
				}
				else
				{
					gameScene.DDSTrigger.StopRefreshEnemies();
				}
				Hashtable enemies = gameScene.GetEnemies();
				foreach (Enemy value in enemies.Values)
				{
					if (value != null)
					{
						value.SetState(Enemy.IDLE_STATE);
					}
				}
				Transform transform = GameApp.GetInstance().GetGameScene().GetCamera()
					.gameObject.transform.Find("Screen_Blood_Dead");
				if (transform != null)
				{
					transform.gameObject.active = false;
				}
			}
			if (m_bResurrectionFriendPlayer && Time.time - m_ResurrectionAnimFriendStartTime > 3f)
			{
				m_bResurrectionFriendPlayer = false;
				if (gameScene.GetFriendPlayer() != null)
				{
					gameScene.GetFriendPlayer().ResurrectionAtCurrentPos();
					gameScene.GetFriendPlayer().SetState(Player.IDLE_STATE);
				}
			}
			if (m_Map3WinTimer >= 0f)
			{
				m_Map3WinTimer += num;
				if (m_Map3WinTimer > m_Map3WinTime)
				{
					m_Map3WinTimer = -1f;
					SetupControlCoverUI(true);
					SetupMap3WinRewardUI(true, m_Map3RewardItem);
					m_MoveJoystickBtn.Reset();
					((TopWatchingInputController)player.InputController).MoveDirection = Vector3.zero;
					player.StopRun();
					player.SetState(Player.IDLE_STATE);
					m_ShootJoystickBtn.Reset();
					((TopWatchingInputController)player.InputController).bFire = false;
					player.StopFire();
				}
			}
			if (m_LevelCountDownTimer != null)
			{
				long num6 = (long)gameScene.GetRemainingTimeByTimeMode();
				if (num6 > 0)
				{
					string text = UtilsEx.TimeToStr_HMS(num6);
					m_LevelCountDownTimer.SetText(text);
				}
				else
				{
					string text2 = UtilsEx.TimeToStr_HMS(0L);
					m_LevelCountDownTimer.SetText(text2);
				}
			}
		}
	}

	private void LateUpdate()
	{
	}

	public void BeginGameTimer()
	{
		Material mat = LoadUIMaterial("Zombie3D/UI/Materials/NLobbyUI");
		Resources.UnloadUnusedAssets();
		m_fBeginGameCountDownTimer = 0f;
		uint num = (uint)(m_fBeginGameCountDownTime - m_fBeginGameCountDownTimer);
		if (num != m_uiCountDownNum)
		{
			m_uiCountDownNum = num;
			if (m_BeginGameCountDownImg == null)
			{
				m_BeginGameCountDownImg = UIUtils.BuildImage(0, new Rect(450f, 361f, 90f, 110f), mat, m_rectCountDownNumber[num], new Vector2(90f, 110f));
				m_DesktopGroup.Add(m_BeginGameCountDownImg);
			}
		}
	}

	public void BeginGame()
	{
		((TopWatchingCameraScript)gameScene.GetCamera()).ShowGameStartEffectEnd();
		m_bGameBeginEffectBack = true;
		SetupBattleUI(true);
		SetupControlCoverUI(false);
		gameScene.BattleBegin();
	}

	public void HandleEvent(UIControl control, int command, float wparam, float lparam)
	{
		if (m_GameLoadingSurvivalUI != null)
		{
			return;
		}
		if ((control.GetType() == typeof(UIClickButton) || control.GetType() == typeof(UISelectButton)) && GameApp.GetInstance().GetGameState().SoundOn)
		{
			SceneUIManager.Instance().PlayClickAudio();
		}
		if (control.Id == 3012)
		{
			m_DesktopGroup.Remove(3012);
			((TopWatchingCameraScript)gameScene.GetCamera()).ShowGameStartEffectEnd();
			if (gameState.m_iCameraModeType == 0)
			{
				SetupCameraTypeDialogUI(true);
				return;
			}
			if (gameState.m_iCameraModeType == 1)
			{
				((TopWatchingCameraScript)gameScene.GetCamera()).SetCameraLookType(1, false);
			}
			else if (gameState.m_iCameraModeType == 2)
			{
				((TopWatchingCameraScript)gameScene.GetCamera()).SetCameraLookType(2, false);
			}
			else
			{
				Debug.Log("Wrong CameraType!!!");
			}
			m_bGameBeginEffectBack = true;
			SetupBattleUI(true);
			SetupControlCoverUI(true);
			gameScene.BattleBegin();
		}
		else if (control.Id == 3013)
		{
			SetupCameraTypeDialogUI(false);
			SetupCameraTypeUI(true);
		}
		else if (control.Id == 3014)
		{
			((TopWatchingCameraScript)gameScene.GetCamera()).SetCameraLookType(1, true);
			gameState.m_iCameraModeType = 1;
			if (m_uiCameraGroup != null && !m_uiCameraGroup.GetControl(3016).Enable)
			{
				m_uiCameraGroup.GetControl(3016).Enable = true;
			}
		}
		else if (control.Id == 3015)
		{
			((TopWatchingCameraScript)gameScene.GetCamera()).SetCameraLookType(2, true);
			gameState.m_iCameraModeType = 2;
			if (m_uiCameraGroup != null && !m_uiCameraGroup.GetControl(3016).Enable)
			{
				m_uiCameraGroup.GetControl(3016).Enable = true;
			}
		}
		else if (control.Id == 3016)
		{
			if (gameState.m_iCameraModeType != 0)
			{
				SetupCameraTypeUI(false);
				if (gameState.m_iCameraModeType == 1)
				{
					((TopWatchingCameraScript)gameScene.GetCamera()).SetCameraLookType(1, false);
				}
				else if (gameState.m_iCameraModeType == 2)
				{
					((TopWatchingCameraScript)gameScene.GetCamera()).SetCameraLookType(2, false);
				}
				else
				{
					Debug.Log("Wrong CameraType!!!");
				}
				m_bGameBeginEffectBack = true;
				SetupBattleUI(true);
				SetupControlCoverUI(true);
				gameScene.BattleBegin();
				GameApp.GetInstance().Save();
			}
			else
			{
				SetupCameraTypeUI(true);
			}
		}
		else if (control.Id == 3002)
		{
			UIImage uIImage = m_DesktopGroup.GetControl(3001) as UIImage;
			float direction = ((UIJoystickButtonEx)control).Direction;
			if (command == 2)
			{
				uIImage.SetColor(new Color(1f, 1f, 1f, joystickBgImgAlpha));
				((TopWatchingInputController)player.InputController).MoveDirection = Vector3.zero;
				player.StopRun();
				player.SetState(Player.IDLE_STATE);
				return;
			}
			uIImage.SetColor(new Color(1f, 1f, 1f, 1f));
			Vector3 direction2 = new Vector3(Mathf.Cos(direction), 0f, Mathf.Sin(direction));
			direction2 = player.GetRespawnTransform().TransformDirection(direction2);
			((TopWatchingInputController)player.InputController).MoveDirection = direction2;
			player.Run();
			if (!((TopWatchingInputController)player.InputController).bFire)
			{
				player.SetState(Player.RUN_STATE);
			}
		}
		else if (control.Id == 3004)
		{
			UIImage uIImage2 = m_DesktopGroup.GetControl(3003) as UIImage;
			float direction3 = ((UIJoystickButtonEx)control).Direction;
			switch (command)
			{
			case 2:
				uIImage2.SetColor(new Color(1f, 1f, 1f, joystickBgImgAlpha));
				((TopWatchingInputController)player.InputController).bFire = false;
				player.StopFire();
				return;
			case 0:
				player.GetWeapon().SetWeaponTriggerTime(0.05f);
				player.GetTransform().localRotation = Quaternion.AngleAxis(90f - direction3 / (float)Math.PI * 180f, Vector3.up);
				break;
			}
			uIImage2.SetColor(new Color(1f, 1f, 1f, 1f));
			if (!player.Faint)
			{
				((TopWatchingInputController)player.InputController).ShootDirection = new Vector3(player.GetTransform().localEulerAngles.x, 90f - direction3 / (float)Math.PI * 180f, player.GetTransform().localEulerAngles.z);
				((TopWatchingInputController)player.InputController).bFire = true;
			}
		}
		else if (control.Id == 3000)
		{
			Time.timeScale = 0f;
			m_MoveJoystickBtn.Reset();
			((TopWatchingInputController)player.InputController).MoveDirection = Vector3.zero;
			player.StopRun();
			player.SetState(Player.IDLE_STATE);
			m_ShootJoystickBtn.Reset();
			((TopWatchingInputController)player.InputController).bFire = false;
			player.StopFire();
			m_bPaused = true;
			SceneUIManager.Instance().ChangeSceneUI(SceneUIManager.SceneUI.GamePauseUI, false);
			GameCollectionInfoManager.Instance().GetCurrentInfo().AddUIEnterLog(GameCollectionInfo.enUIEnterIndex.BattleUI_Pause);
		}
		else if (control.Id == 3005)
		{
			List<WeaponType> battleWeapons = gameState.GetBattleWeapons();
			if (battleWeapons.Count == 1 || battleWeapons.Count != 2)
			{
				return;
			}
			for (int i = 0; i < 2; i++)
			{
				WeaponType weaponType = battleWeapons[i];
				if (weaponType != player.GetWeapon().GetWeaponType())
				{
					Weapon w = WeaponFactory.GetInstance().CreateWeapon(weaponType);
					player.ChangeWeapon(w);
					break;
				}
			}
		}
		else if (control.Id == 3006)
		{
			m_MoveJoystickBtn.Reset();
			((TopWatchingInputController)player.InputController).MoveDirection = Vector3.zero;
			player.StopRun();
			player.SetState(Player.IDLE_STATE);
			m_ShootJoystickBtn.Reset();
			((TopWatchingInputController)player.InputController).bFire = false;
			player.StopFire();
			SetupBattleItemUI(true);
			GameCollectionInfoManager.Instance().GetCurrentInfo().AddUIEnterLog(GameCollectionInfo.enUIEnterIndex.BattleUI_Item);
		}
		else if (control.Id == 3007)
		{
			player.SpeedUpByStamina = !player.SpeedUpByStamina;
			if (GameApp.GetInstance().GetGameState().m_eGameMode.m_ePlayMode != 0)
			{
				if (player.SpeedUpByStamina)
				{
					GameSetup.Instance.ReqSyncPlayerInfo(GameSetup.NPlayerDataType.E_Skill_RunFast, 1f);
				}
				else
				{
					GameSetup.Instance.ReqSyncPlayerInfo(GameSetup.NPlayerDataType.E_Skill_RunFast, 0f);
				}
			}
			m_bCurrentSpeedUpState = player.SpeedUpByStamina;
			if (gameState.SoundOn)
			{
				GameApp.GetInstance().GetGameConfig().UIPlayerSpeedUp.Play();
				if (player.SpeedUpByStamina && TimerManager.GetInstance().Ready(90) && GameApp.GetInstance().GetGameState().SoundOn)
				{
					string text = "Zombie3D/Audio/RealPersonSound/BeginRun/BeginRun_";
					text += UnityEngine.Random.Range(0, 4);
					AudioManager.PlayMusicOnce(text, player.GetTransform());
					TimerManager.GetInstance().Do(90);
				}
			}
		}
		else if (control == m_SkillBuildCannon)
		{
			if (player.ActiveSkillImpl == null)
			{
				player.UseActiveSkill();
				if (player.ActiveSkillImpl != null)
				{
					m_bSkillBuildCannonStarted = true;
				}
			}
			else
			{
				player.TerminateActiveSkill();
			}
		}
		else if (control == m_SkillThrowGrenade)
		{
			if (player.ActiveSkillImpl == null)
			{
				player.UseActiveSkill();
			}
			else
			{
				player.TerminateActiveSkill();
			}
		}
		else if (control == m_SkillCoverMe)
		{
			if (gameScene.GetFriendPlayer() == null)
			{
				return;
			}
			if (player.ActiveSkillImpl == null && gameScene.GetFriendPlayer().HP > 0f)
			{
				player.UseActiveSkill();
				if (player.ActiveSkillImpl != null)
				{
					m_bSkillCoverMeStarted = true;
				}
			}
			else
			{
				player.TerminateActiveSkill();
			}
		}
		else if (control == m_SkillDoubleTeam)
		{
			if (gameScene.GetFriendPlayer() == null)
			{
				return;
			}
			if (player.ActiveSkillImpl == null && gameScene.GetFriendPlayer().HP > 0f)
			{
				player.UseActiveSkill();
				if (player.ActiveSkillImpl != null)
				{
					m_bSkillDoubleTeamStarted = true;
				}
			}
			else
			{
				player.TerminateActiveSkill();
			}
		}
		else if (control.Id >= 3022 && control.Id <= 3042)
		{
			int num = control.Id - 3022;
			switch (num)
			{
			case 11:
				return;
			case 12:
				if (gameScene.GetFriendPlayer() != null && gameScene.GetFriendPlayer().HP <= 0f)
				{
					Hashtable powerUPS = gameState.GetPowerUPS();
					int num2 = (int)powerUPS[num] - 1;
					powerUPS[num] = ((num2 > 0) ? num2 : 0);
					GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load("Zombie3D/PlayerBuff/relive"), gameScene.GetFriendPlayer().GetTransform().position, Quaternion.identity) as GameObject;
					RemoveTimerScript removeTimerScript = gameObject.AddComponent(typeof(RemoveTimerScript)) as RemoveTimerScript;
					removeTimerScript.life = 3f;
					Animation[] componentsInChildren = gameObject.GetComponentsInChildren<Animation>();
					for (int j = 0; j < componentsInChildren.Length; j++)
					{
						componentsInChildren[j].Play(componentsInChildren[j].clip.name);
					}
					m_bResurrectionFriendPlayer = true;
					m_ResurrectionAnimFriendStartTime = Time.time;
					break;
				}
				return;
			default:
				if (!player.UsePowerUps((ItemType)num))
				{
					SetupHintDialog(true, 3020, 0, 0, "A stronger effect is already active, you can't use this now!");
					return;
				}
				switch (num)
				{
				}
				break;
			}
			SetupBattleItemUI(false);
		}
		else if (control.Id == 3008)
		{
			SetupBattleItemUI(false);
		}
		else if (control.Id == 3009)
		{
			if (Time.timeScale < 1f)
			{
				Time.timeScale = 1f;
			}
			SetupPlayerDeadUI(false);
			SetupLoadingToExchangeUI();
			gameScene.BattleEnd();
		}
		else if (control.Id == 3010)
		{
			if (Time.timeScale < 1f)
			{
				Time.timeScale = 1f;
			}
			SetupPlayerDeadUI(false);
			((TopWatchingCameraScript)gameScene.GetCamera()).ShowPlayerReliveEffect();
			Hashtable powerUPS2 = gameState.GetPowerUPS();
			foreach (int key in powerUPS2.Keys)
			{
				if (key == 11)
				{
					powerUPS2[key] = (int)powerUPS2[key] - 1;
					if (gameState.IsGCArchievementLocked(6))
					{
						gameState.UnlockGCArchievement(6, "com.trinitigame.callofminibulletdudes.a7");
					}
					GameApp.GetInstance().Save();
					break;
				}
			}
			GameObject gameObject2 = UnityEngine.Object.Instantiate(Resources.Load("Zombie3D/PlayerBuff/relive"), player.GetTransform().position, Quaternion.identity) as GameObject;
			RemoveTimerScript removeTimerScript2 = gameObject2.AddComponent(typeof(RemoveTimerScript)) as RemoveTimerScript;
			removeTimerScript2.life = 3f;
			if (gameScene.GetFriendPlayer() != null && gameScene.GetFriendPlayer().HP <= 0f)
			{
				GameObject gameObject3 = UnityEngine.Object.Instantiate(Resources.Load("Zombie3D/PlayerBuff/relive"), gameScene.GetFriendPlayer().GetTransform().position, Quaternion.identity) as GameObject;
				RemoveTimerScript removeTimerScript3 = gameObject3.AddComponent(typeof(RemoveTimerScript)) as RemoveTimerScript;
				removeTimerScript3.life = 3f;
			}
			Animation[] componentsInChildren2 = gameObject2.GetComponentsInChildren<Animation>();
			for (int k = 0; k < componentsInChildren2.Length; k++)
			{
				componentsInChildren2[k].Play(componentsInChildren2[k].clip.name);
			}
			SetupControlCoverUI(true);
			m_bResurrectionPlayer = true;
			m_ResurrectionAnimStartTime = Time.time;
		}
		else if (control.Id == 3011)
		{
			if (Time.timeScale < 1f)
			{
				Time.timeScale = 1f;
			}
			SetupPlayerDeadUI(false);
			SetupLoadingToExchangeUI();
			gameScene.BattleEnd();
			GameCollectionInfoManager.Instance().GetCurrentInfo().SetLastGamePointsInfo(gameScene.DDSTrigger.MapIndex, gameScene.DDSTrigger.PointsIndex, -1);
		}
		else if (control.Id == 3020)
		{
			SetupHintDialog(false, 0, 0, 0, string.Empty);
		}
		else if (control.Id == 3021)
		{
			SetupMap3WinRewardUI(false, null);
			SetupControlCoverUI(false);
			SetupLoadingToExchangeUI();
			gameScene.BattleEnd();
		}
		else if (control.Id != 3043)
		{
		}
	}

	public void SetupBattleUI(bool bShow)
	{
		if (m_DesktopGroup != null)
		{
			m_DesktopGroup.Clear();
			m_DesktopGroup = null;
		}
		if (bShow)
		{
			m_DesktopGroup = new uiGroup(m_UIManager);
			UIClickButton control = UIUtils.BuildClickButton(3000, new Rect(0f, Screen.height - 75, 98f, 75f), m_MatBattleUI, new Rect(4f, 128f, 98f, 75f), new Rect(99f, 128f, 98f, 75f), new Rect(4f, 128f, 98f, 75f), new Vector2(98f, 75f), 0);
			m_DesktopGroup.Add(control);
			UIImage control2 = UIUtils.BuildImage(0, new Rect(Screen.width / 2 - 157, Screen.height - 60, 315f, 52f), m_MatBattleUI, new Rect(0f, 0f, 315f, 52f), new Vector2(315f, 52f));
			m_DesktopGroup.Add(control2);
			playerHpProgressBar = new UIProgressBarRounded();
			playerHpProgressBar.Id = 0;
			playerHpProgressBar.Rect = AutoUIResolution.ToShiftToRight(AutoUI.AutoRect(new Rect(Screen.width / 2 - 123 + 22, (float)Screen.height - 31f, 247f, 17f)), 2);
			playerHpProgressBar.SetParam(m_MatBattleUI, new Rect(1f, 1f, 1f, 1f), m_MatBattleUI, new Rect(320f, 6f, 3f, 17f), new Rect(323f, 6f, 1f, 17f), new Rect(327f, 6f, 5f, 17f), 1f);
			m_DesktopGroup.Add(playerHpProgressBar);
			playerHpProgressBarAnim = new UIAnimationControl();
			playerHpProgressBarAnim.Id = 0;
			playerHpProgressBarAnim.SetAnimationsPageCount(3);
			playerHpProgressBarAnim.Rect = AutoUI.AutoRect(new Rect(Screen.width / 2 - 128 + 22, Screen.height - 36, 257f, 27f));
			playerHpProgressBarAnim.SetTexture(0, m_MatBattleUI, AutoUI.AutoRect(new Rect(510f, 0f, 257f, 27f)), AutoUI.AutoSize(new Vector2(257f, 27f)));
			playerHpProgressBarAnim.SetTexture(1, m_MatBattleUI, AutoUI.AutoRect(new Rect(510f, 27f, 257f, 27f)), AutoUI.AutoSize(new Vector2(257f, 27f)));
			playerHpProgressBarAnim.SetTexture(2, m_MatBattleUI, AutoUI.AutoRect(new Rect(510f, 54f, 257f, 27f)), AutoUI.AutoSize(new Vector2(257f, 27f)));
			playerHpProgressBarAnim.SetTimeInterval(0.2f);
			playerHpProgressBarAnim.SetLoopCount(1000000);
			playerHpProgressBarAnim.Visible = false;
			playerHpProgressBarAnim.Enable = false;
			m_DesktopGroup.Add(playerHpProgressBarAnim);
			playerStaminaProgressBar = new UIProgressBarRounded();
			playerStaminaProgressBar.Id = 0;
			playerStaminaProgressBar.Rect = AutoUIResolution.ToShiftToRight(AutoUI.AutoRect(new Rect(Screen.width / 2 - 123 + 22, Screen.height - 60, 247f, 17f)), 2);
			playerStaminaProgressBar.SetParam(m_MatBattleUI, AutoUI.AutoRect(new Rect(1f, 0f, 0f, 0f)), m_MatBattleUI, AutoUI.AutoRect(new Rect(320f, 35f, 3f, 17f)), AutoUI.AutoRect(new Rect(323f, 35f, 1f, 17f)), AutoUI.AutoRect(new Rect(327f, 35f, 5f, 17f)), 1f);
			m_DesktopGroup.Add(playerStaminaProgressBar);
			playerStaminaProgressBarAnim = new UIAnimationControl();
			playerStaminaProgressBarAnim.Id = 0;
			playerStaminaProgressBarAnim.SetAnimationsPageCount(3);
			playerStaminaProgressBarAnim.Rect = AutoUI.AutoRect(new Rect(Screen.width / 2 - 128 + 22, Screen.height - 65, 257f, 27f));
			playerStaminaProgressBarAnim.SetTexture(0, m_MatBattleUI, AutoUI.AutoRect(new Rect(767f, 0f, 257f, 27f)), AutoUI.AutoSize(new Vector2(257f, 27f)));
			playerStaminaProgressBarAnim.SetTexture(1, m_MatBattleUI, AutoUI.AutoRect(new Rect(767f, 27f, 257f, 27f)), AutoUI.AutoSize(new Vector2(257f, 27f)));
			playerStaminaProgressBarAnim.SetTexture(2, m_MatBattleUI, AutoUI.AutoRect(new Rect(767f, 54f, 257f, 27f)), AutoUI.AutoSize(new Vector2(257f, 27f)));
			playerStaminaProgressBarAnim.SetTimeInterval(0.2f);
			playerStaminaProgressBarAnim.SetLoopCount(1000000);
			playerStaminaProgressBarAnim.Visible = false;
			playerStaminaProgressBarAnim.Enable = false;
			m_DesktopGroup.Add(playerStaminaProgressBarAnim);
			List<WeaponType> battleWeapons = gameState.GetBattleWeapons();
			if (battleWeapons.Count < 2)
			{
				control2 = UIUtils.BuildImage(0, new Rect(Screen.width - 360, 32f, 96f, 63f), m_MatBattleUI, new Rect(454f, 207f, 96f, 63f), new Vector2(96f, 63f), 1);
				m_DesktopGroup.Add(control2);
			}
			else
			{
				control = UIUtils.BuildClickButton(3005, new Rect(Screen.width - 360, 32f, 96f, 63f), m_MatBattleUI, new Rect(454f, 81f, 96f, 63f), new Rect(454f, 144f, 96f, 63f), new Rect(454f, 81f, 96f, 63f), new Vector2(96f, 63f), 1);
				m_DesktopGroup.Add(control);
			}
			control = UIUtils.BuildClickButton(3006, new Rect(260f, 32f, 96f, 63f), m_MatBattleUI, new Rect(454f, 270f, 96f, 63f), new Rect(454f, 270f, 96f, 63f), new Rect(454f, 270f, 96f, 63f), new Vector2(96f, 63f));
			m_DesktopGroup.Add(control);
			m_bCurrentSpeedUpState = player.SpeedUpByStamina;
			enSkillType enSkillType = enSkillType.FastRun;
			enSkillType = ((gameState.m_eGameMode.m_ePlayMode != 0) ? enSkillType.FastRun : gameState.m_CurSkillType);
			int num = Screen.width - 136;
			switch (enSkillType)
			{
			case enSkillType.FastRun:
				m_SpeedUpButton = UIUtils.BuildPushButton(3007, new Rect(num, 260f, 96f, 63f), m_MatBattleUI, m_rcActiveSkillBtnTex[0], m_rcActiveSkillBtnTex[1], m_rcActiveSkillBtnTex[2], new Vector2(96f, 63f), 1);
				m_SpeedUpButton.Set(m_bCurrentSpeedUpState);
				m_DesktopGroup.Add(m_SpeedUpButton);
				break;
			case enSkillType.BuildCannon:
				m_SkillBuildCannon = UIUtils.BuildPushButton(0, new Rect(num, 260f, 96f, 63f), m_MatBattleUI, m_rcActiveSkillBtnTex[0], m_rcActiveSkillBtnTex[1], m_rcActiveSkillBtnTex[2], new Vector2(96f, 63f), 1);
				m_SkillBuildCannon.Set(m_bSkillBuildCannonStarted);
				m_DesktopGroup.Add(m_SkillBuildCannon);
				break;
			case enSkillType.ThrowGrenade:
				m_SkillThrowGrenade = UIUtils.BuildClickButton(0, new Rect(num, 260f, 96f, 63f), m_MatBattleUI, m_rcActiveSkillBtnTex[0], m_rcActiveSkillBtnTex[1], m_rcActiveSkillBtnTex[2], new Vector2(96f, 63f), 1);
				m_DesktopGroup.Add(m_SkillThrowGrenade);
				break;
			case enSkillType.CoverMe:
				m_SkillCoverMe = UIUtils.BuildPushButton(0, new Rect(num, 260f, 96f, 63f), m_MatBattleUI, m_rcActiveSkillBtnTex[0], m_rcActiveSkillBtnTex[1], m_rcActiveSkillBtnTex[2], new Vector2(96f, 63f), 1);
				m_SkillCoverMe.Set(m_bSkillCoverMeStarted);
				m_DesktopGroup.Add(m_SkillCoverMe);
				break;
			case enSkillType.DoubleTeam:
				m_SkillDoubleTeam = UIUtils.BuildPushButton(0, new Rect(num, 260f, 96f, 63f), m_MatBattleUI, m_rcActiveSkillBtnTex[0], m_rcActiveSkillBtnTex[1], m_rcActiveSkillBtnTex[2], new Vector2(96f, 63f), 1);
				m_SkillDoubleTeam.Set(m_bSkillDoubleTeamStarted);
				m_DesktopGroup.Add(m_SkillDoubleTeam);
				break;
			}
			m_MoveJoystickBg = UIUtils.BuildImage(3001, new Rect(15f, 15f, 226f, 226f), m_MatBattleUI, new Rect(568f, 266f, 226f, 226f), new Vector2(226f, 226f), 0);
			m_MoveJoystickBg.SetColor(new Color(1f, 1f, 1f, joystickBgImgAlpha));
			m_DesktopGroup.Add(m_MoveJoystickBg);
			m_MoveJoystickBtn = UIUtils.BuildJoystickButtonEx(3002, new Rect(15f, 15f, 226f, 226f), m_MatBattleUI, new Rect(897f, 82f, 91f, 91f), new Rect(897f, 173f, 91f, 91f), new Rect(897f, 82f, 91f, 91f), new Vector2(91f, 91f), 10f, 78f);
			m_DesktopGroup.Add(m_MoveJoystickBtn);
			m_ShootJoystickBg = UIUtils.BuildImage(3003, new Rect(Screen.width - 248, 30f, 226f, 226f), m_MatBattleUI, new Rect(796f, 266f, 226f, 226f), new Vector2(226f, 226f), 1);
			m_ShootJoystickBg.SetColor(new Color(1f, 1f, 1f, joystickBgImgAlpha));
			m_DesktopGroup.Add(m_ShootJoystickBg);
			m_ShootJoystickBtn = UIUtils.BuildJoystickButtonEx(3004, new Rect(Screen.width - 248, 30f, 226f, 226f), m_MatBattleUI, new Rect(897f, 82f, 91f, 91f), new Rect(897f, 173f, 91f, 91f), new Rect(897f, 82f, 91f, 91f), new Vector2(91f, 91f), 10f, 78f, 1);
			m_DesktopGroup.Add(m_ShootJoystickBtn);
			if (gameScene.DDSTrigger.MapIndex != 5)
			{
				m_LevelCountDownTimer = null;
			}
		}
	}

	public void SetupControlCoverUI(bool bShow)
	{
		if (m_ControlCoverBarGroup != null)
		{
			m_ControlCoverBarGroup.Clear();
			m_ControlCoverBarGroup = null;
		}
		if (bShow)
		{
			m_ControlCoverBarGroup = new uiGroup(m_UIManager);
			UIBlock uIBlock = new UIBlock();
			uIBlock.Rect = new Rect(0f, 0f, Screen.width, Screen.height);
			m_ControlCoverBarGroup.Add(uIBlock);
		}
	}

	public void SetupBattleItemUI(bool bShow)
	{
		if (m_ItemBarGroup != null)
		{
			m_ItemBarGroup.Clear();
			m_ItemBarGroup = null;
		}
		if (!bShow)
		{
			OpenClickPlugin.Hide();
			Time.timeScale = 1f;
			return;
		}
		OpenClickPlugin.Show(false);
		Time.timeScale = 0f;
		m_ItemBarGroup = new uiGroup(m_UIManager);
		UIBlock uIBlock = new UIBlock();
		uIBlock.Rect = new Rect(0f, 0f, Screen.width, Screen.height);
		m_ItemBarGroup.Add(uIBlock);
		UIImage control = UIUtils.BuildImage(0, new Rect(0f, 0f, Screen.width, Screen.height), m_MatBattleUI, new Rect(321f, 28f, 1f, 1f), new Vector2(Screen.width, Screen.height));
		m_ItemBarGroup.Add(control);
		control = UIUtils.BuildImage(0, new Rect((float)Screen.width / 2f - 473f, (float)Screen.height / 2f - 121f, 946f, 241f), m_MatBattleUI, new Rect(78f, 783f, 946f, 241f), new Vector2(946f, 241f));
		m_ItemBarGroup.Add(control);
		SetupPowerUpsPageView(control.Rect);
		UIClickButton control2 = UIUtils.BuildClickButton(3008, new Rect((float)Screen.width / 2f - 93.5f, (float)Screen.height / 2f - 121f - 34f, 187f, 68f), m_MatBattleUI, new Rect(0f, 54f, 187f, 68f), new Rect(188f, 54f, 187f, 68f), new Rect(0f, 54f, 187f, 68f), new Vector2(187f, 68f));
		m_ItemBarGroup.Add(control2);
	}

	public void SetupPowerUpsPageView(Rect groundRect)
	{
		Hashtable powerUPS = gameState.GetPowerUPS();
		if (powerUPS.Count < 1)
		{
			return;
		}
		if (m_PowerUpsView != null)
		{
			m_ItemBarGroup.Remove(m_PowerUpsView);
			m_PowerUpsView = null;
		}
		int num = 0;
		foreach (int key in powerUPS.Keys)
		{
			if ((int)powerUPS[key] > 0)
			{
				num++;
			}
		}
		if (num <= 0)
		{
			return;
		}
		m_PowerUpsView = new UIScrollPageView();
		m_PowerUpsView.SetMoveParam(AutoUI.AutoRect(new Rect(groundRect.x - 7f, groundRect.y - 5f, groundRect.width + 14f, groundRect.height - 21f)), AutoUI.AutoDistance(20f), AutoUI.AutoDistance(20f));
		m_PowerUpsView.Rect = AutoUI.AutoRect(new Rect(groundRect.x + 25f, groundRect.y + 18f, groundRect.width - 48f, groundRect.height - 75f));
		m_PowerUpsView.ListOri = UIScrollPageView.ListOrientation.Horizontal;
		m_PowerUpsView.ViewSize = AutoUI.AutoSize(new Vector2(915f, 166f));
		m_PowerUpsView.ItemSpacingV = AutoUI.AutoDistance(0f);
		m_PowerUpsView.ItemSpacingH = AutoUI.AutoDistance(50f);
		m_PowerUpsView.SetClip(AutoUI.AutoRect(new Rect(groundRect.x + 25f, groundRect.y + 18f, groundRect.width - 48f, groundRect.height - 75f)));
		m_PowerUpsView.Bounds = AutoUI.AutoRect(new Rect(groundRect.x + 35f, groundRect.y + 18f, 865f, 166f));
		m_ItemBarGroup.Add(m_PowerUpsView);
		UIClickButton uIClickButton = null;
		int num3 = 0;
		UIGroupControl uIGroupControl = UIUtils.BuildUIGroupControl(0, new Rect(0f, 0f, 865f, 166f));
		foreach (int key2 in powerUPS.Keys)
		{
			if ((int)powerUPS[key2] <= 0)
			{
				continue;
			}
			UIImage control = UIUtils.BuildImage(0, new Rect(0 + num3 * 173, 0f, 173f, 166f), m_MatBattleUI, new Rect(551f, 82f, 173f, 166f), new Vector2(173f, 166f));
			uIGroupControl.Add(control);
			float num5 = 65 + num3 * 173;
			float num6 = 75f;
			if (m_MatPowerUPSIcons == null)
			{
				m_MatPowerUPSIcons = LoadUIMaterial("Zombie3D/UI/Materials/PowerUPSIcons");
			}
			Resources.UnloadUnusedAssets();
			Rect powerUpsIconTexture = ShopUIScript.GetPowerUpsIconTexture((ItemType)key2);
			control = UIUtils.BuildImage(0, new Rect(num5 - powerUpsIconTexture.width / 2f, num6 - powerUpsIconTexture.height / 2f, powerUpsIconTexture.width, powerUpsIconTexture.height), m_MatPowerUPSIcons, powerUpsIconTexture, new Vector2(powerUpsIconTexture.width, powerUpsIconTexture.height));
			uIGroupControl.Add(control);
			uIClickButton = UIUtils.BuildClickButton(3022 + key2, new Rect(0 + num3 * 173, 0f, 173f, 166f), m_MatBattleUI, new Rect(70f, 1f, 173f, 1f), new Rect(724f, 82f, 173f, 166f), new Rect(1f, 1f, 1f, 1f), new Vector2(173f, 166f));
			uIGroupControl.Add(uIClickButton);
			FixedConfig.PowerUPSCfg powerUPSCfg = ConfigManager.Instance().GetFixedConfig().GetPowerUPSCfg((ItemType)key2);
			UIText uIText = UIUtils.BuildUIText(0, new Rect(5 + num3 * 173, 125f, 180f, 20f), UIText.enAlignStyle.center);
			uIText.Set("Zombie3D/Font/037-CAI978-15", powerUPSCfg.name, new Color(0.81960785f, 0.5294118f, 2f / 51f, 1f));
			uIGroupControl.Add(uIText);
			float num7 = 105f;
			float num8 = 100f;
			float num9 = 0f + num7 + (float)(num3 * 173);
			float num10 = 0f + num8;
			if (AutoUI.IsRetain == AutoUI.RESOLUTION.LOWDEFINITION)
			{
				num9 -= 20f;
			}
			if (powerUPSCfg.type == ItemType.Pacemaker)
			{
			}
			if ((float)powerUPSCfg.stamina > 0f)
			{
				num10 -= 15f;
				string text = powerUPSCfg.stamina.ToString();
				if (powerUPSCfg.stamina >= 2000)
				{
					text = "Max";
				}
				uIText = UIUtils.BuildUIText(0, new Rect(num9, num10, 200f, 20f), UIText.enAlignStyle.left);
				uIText.Set("Zombie3D/Font/037-CAI978-10", "STA: " + text, new Color(0.81960785f, 0.5294118f, 2f / 51f, 1f));
				uIGroupControl.Add(uIText);
			}
			if (powerUPSCfg.staminaSpeedAdd > 0f)
			{
				num10 -= 15f;
				string text2 = powerUPSCfg.stamina.ToString();
				if (powerUPSCfg.stamina >= 2000)
				{
					text2 = "Max";
				}
				uIText = UIUtils.BuildUIText(0, new Rect(num9, num10, 200f, 20f), UIText.enAlignStyle.left);
				uIText.Set("Zombie3D/Font/037-CAI978-10", "STA: +" + powerUPSCfg.staminaSpeedAdd + "/s", new Color(0.81960785f, 0.5294118f, 2f / 51f, 1f));
				uIGroupControl.Add(uIText);
			}
			if (powerUPSCfg.hp > 0f)
			{
				num10 -= 15f;
				uIText = UIUtils.BuildUIText(0, new Rect(num9, num10, 200f, 20f), UIText.enAlignStyle.left);
				uIText.Set("Zombie3D/Font/037-CAI978-10", "HP: " + powerUPSCfg.hp * 100f + "%", new Color(0.81960785f, 0.5294118f, 2f / 51f, 1f));
				uIGroupControl.Add(uIText);
			}
			if (powerUPSCfg.keepTime > 0f)
			{
				num10 -= 15f;
				string text3 = powerUPSCfg.keepTime + "S";
				if (powerUPSCfg.keepTime < 0f)
				{
					text3 = "1 STAGE";
				}
				uIText = UIUtils.BuildUIText(0, new Rect(num9, num10, 200f, 20f), UIText.enAlignStyle.left);
				uIText.Set("Zombie3D/Font/037-CAI978-10", "TIME: " + text3, new Color(0.81960785f, 0.5294118f, 2f / 51f, 1f));
				uIGroupControl.Add(uIText);
			}
			if (powerUPSCfg.damagePercent > 0f)
			{
				num10 -= 15f;
				uIText = UIUtils.BuildUIText(0, new Rect(num9, num10, 200f, 20f), UIText.enAlignStyle.left);
				uIText.Set("Zombie3D/Font/037-CAI978-10", "DMG: " + powerUPSCfg.damagePercent * 100f + "%", new Color(0.81960785f, 0.5294118f, 2f / 51f, 1f));
				uIGroupControl.Add(uIText);
			}
			if (powerUPSCfg.damage > 0f)
			{
				num10 -= 15f;
				uIText = UIUtils.BuildUIText(0, new Rect(num9, num10, 200f, 20f), UIText.enAlignStyle.left);
				uIText.Set("Zombie3D/Font/037-CAI978-10", "DMG: " + powerUPSCfg.damage, new Color(0.81960785f, 0.5294118f, 2f / 51f, 1f));
				uIGroupControl.Add(uIText);
			}
			if (powerUPSCfg.type == ItemType.Shield)
			{
				num10 -= 15f;
				uIText = UIUtils.BuildUIText(0, new Rect(num9, num10, 200f, 20f), UIText.enAlignStyle.left);
				uIText.Set("Zombie3D/Font/037-CAI978-10", "HP: " + 150, new Color(0.81960785f, 0.5294118f, 2f / 51f, 1f));
				uIGroupControl.Add(uIText);
			}
			if (powerUPSCfg.type == ItemType.Pacemaker)
			{
				num10 -= 55f;
				uIText = UIUtils.BuildUIText(0, new Rect(num9, num10, 200f, 60f), UIText.enAlignStyle.left);
				uIText.Set("Zombie3D/Font/037-CAI978-10", "DEF: 100%", new Color(0.81960785f, 0.5294118f, 2f / 51f, 1f));
				uIGroupControl.Add(uIText);
			}
			UIText uIText2 = UIUtils.BuildUIText(0, new Rect(100 + num3 * 173, 33f, 100f, 30f), UIText.enAlignStyle.left);
			uIText2.Set("Zombie3D/Font/037-CAI978-22", "X " + (int)powerUPS[key2], Color.white);
			uIGroupControl.Add(uIText2);
			if (num3 < 4)
			{
				if (num3 >= num - m_PowerUpsView.PageCount * 5 - 1)
				{
					m_PowerUpsView.Add(uIGroupControl);
				}
				num3++;
			}
			else
			{
				num3 = 0;
				m_PowerUpsView.Add(uIGroupControl);
				uIGroupControl = UIUtils.BuildUIGroupControl(0, new Rect(0f, 0f, 865f, 166f));
			}
		}
		if (m_PowerUpsScrollBar != null)
		{
			m_ItemBarGroup.Remove(m_PowerUpsScrollBar);
			m_PowerUpsScrollBar = null;
		}
		int num11 = Mathf.CeilToInt((float)num / 5f);
		m_PowerUpsScrollBar = new UIDotScrollBar();
		m_PowerUpsScrollBar.Rect = AutoUI.AutoRect(new Rect((float)Screen.width / 2f - (float)num11 / 2f * 25f, 200f, 100f, 20f));
		m_PowerUpsScrollBar.ScrollOri = UIDotScrollBar.ScrollOrientation.Horizontal;
		m_PowerUpsScrollBar.DotPageWidth = AutoUI.AutoDistance(25f);
		m_PowerUpsScrollBar.SetPageCount(num11);
		m_PowerUpsScrollBar.SetScrollBarTexture(m_MatBattleUI, AutoUI.AutoRect(new Rect(341f, 35f, 11f, 11f)), m_MatBattleUI, AutoUI.AutoRect(new Rect(353f, 35f, 11f, 11f)));
		m_PowerUpsScrollBar.SetScrollPercent(-1f);
		m_ItemBarGroup.Add(m_PowerUpsScrollBar);
		m_PowerUpsView.ScrollBar = m_PowerUpsScrollBar;
	}

	public void SetupCameraTypeUI(bool bShow)
	{
		if (m_uiCameraGroup != null)
		{
			m_uiCameraGroup.Clear();
			m_uiCameraGroup = null;
		}
		if (bShow)
		{
			m_uiCameraGroup = new uiGroup(m_UIManager);
			Material mat = LoadUIMaterial("Zombie3D/UI/Materials/ChoosePoints1UI");
			Resources.UnloadUnusedAssets();
			UIImage uIImage = null;
			UIClickButton uIClickButton = null;
			UIText uIText = null;
			uIClickButton = UIUtils.BuildClickButton(3014, new Rect(289f, 149f, 153f, 49f), mat, new Rect(0f, 974f, 153f, 49f), new Rect(0f, 925f, 153f, 49f), new Rect(0f, 974f, 153f, 49f), new Vector2(153f, 49f));
			m_uiCameraGroup.Add(uIClickButton);
			uIClickButton = UIUtils.BuildClickButton(3015, new Rect(520f, 149f, 153f, 49f), mat, new Rect(153f, 974f, 153f, 49f), new Rect(153f, 925f, 153f, 49f), new Rect(153f, 974f, 153f, 49f), new Vector2(153f, 49f));
			m_uiCameraGroup.Add(uIClickButton);
			uIClickButton = UIUtils.BuildClickButton(3016, new Rect(420f, 67f, 153f, 49f), mat, new Rect(775f, 67f, 191f, 62f), new Rect(775f, 3f, 191f, 62f), new Rect(770f, 449f, 191f, 62f), new Vector2(153f, 49f));
			uIClickButton.Enable = false;
			m_uiCameraGroup.Add(uIClickButton);
		}
	}

	public void SetupCameraTypeDialogUI(bool bShow)
	{
		if (m_uiCameraDialogGroup != null)
		{
			m_uiCameraDialogGroup.Clear();
			m_uiCameraDialogGroup = null;
		}
		if (bShow)
		{
			m_uiCameraDialogGroup = new uiGroup(m_UIManager);
			Material material = LoadUIMaterial("Zombie3D/UI/Materials/ChoosePoints1UI");
			Resources.UnloadUnusedAssets();
			UIImage uIImage = null;
			UIClickButton uIClickButton = null;
			UIText uIText = null;
			uIImage = UIUtils.BuildImage(0, new Rect(215f, 167f, 515f, 301f), m_MatDialog01, new Rect(0f, 378f, 515f, 301f), new Vector2(515f, 301f));
			m_uiCameraDialogGroup.Add(uIImage);
			uIText = UIUtils.BuildUIText(0, new Rect(270f, 280f, 450f, 80f), UIText.enAlignStyle.left);
			uIText.Set("Zombie3D/Font/037-CAI978-18", "Tap to switch between two different camera angles.", new Color(69f / 85f, 0.5254902f, 0.09019608f, 1f));
			m_uiCameraDialogGroup.Add(uIText);
			Rect rect = new Rect(500f, 149f, 191f, 62f);
			UIAnimationControl uIAnimationControl = new UIAnimationControl();
			uIAnimationControl.Id = 0;
			uIAnimationControl.SetAnimationsPageCount(4);
			uIAnimationControl.Rect = AutoUI.AutoRect(rect);
			uIAnimationControl.SetTexture(0, material, AutoUI.AutoRect(new Rect(640f, 962f, 191f, 62f)), AutoUI.AutoSize(new Vector2(191f, 62f)));
			uIAnimationControl.SetTexture(1, material, AutoUI.AutoRect(new Rect(640f, 898f, 191f, 62f)), AutoUI.AutoSize(new Vector2(191f, 62f)));
			uIAnimationControl.SetTexture(2, material, AutoUI.AutoRect(new Rect(640f, 834f, 191f, 62f)), AutoUI.AutoSize(new Vector2(191f, 62f)));
			uIAnimationControl.SetTexture(3, material, AutoUI.AutoRect(new Rect(640f, 770f, 191f, 62f)), AutoUI.AutoSize(new Vector2(191f, 62f)));
			uIAnimationControl.SetTimeInterval(0.1f);
			uIAnimationControl.SetLoopCount(10000000);
			m_uiCameraDialogGroup.Add(uIAnimationControl);
			uIClickButton = UIUtils.BuildClickButton(3013, rect, material, new Rect(1f, 1f, 1f, 1f), new Rect(770f, 511f, 191f, 62f), new Rect(1f, 1f, 1f, 1f), new Vector2(191f, 62f));
			m_uiCameraDialogGroup.Add(uIClickButton);
		}
	}

	public void PlayerDead()
	{
		if (m_PlayerDeadTimer < 0f)
		{
			m_PlayerDeadTimer = 0f;
			SetupControlCoverUI(true);
		}
		int map_index = gameScene.DDSTrigger.MapIndex;
		int points_index = gameScene.DDSTrigger.PointsIndex;
		int wave_index = gameScene.DDSTrigger.WaveIndex;
		if (gameState.m_bIsSurvivalMode)
		{
			gameState.GetGameTriggerInfo(ref map_index, ref points_index, ref wave_index);
			points_index = (points_index - 1) * 50 + wave_index;
		}
		GameCollectionInfoManager.Instance().GetCurrentInfo().UpdateDeadInfo(map_index, points_index, wave_index);
	}

	public void SetupPlayerDeadUI(bool bShow)
	{
		if (!bShow)
		{
			OpenClickPlugin.Hide();
			if (m_DeadShowDialog != null)
			{
				m_UIManager.Remove(m_DeadShowDialog);
				m_DeadShowDialog = null;
			}
			return;
		}
		OpenClickPlugin.Show(false);
		int num = 0;
		bool flag = false;
		Hashtable powerUPS = gameState.GetPowerUPS();
		foreach (int key in powerUPS.Keys)
		{
			if (key == 11 && (int)powerUPS[key] > 0)
			{
				num = (int)powerUPS[key];
				flag = true;
			}
		}
		m_DeadShowDialog = UIUtils.BuildUIGroupControl(0, new Rect(Screen.width / 2 - 300, Screen.height / 2 - 190, 600f, 380f));
		m_UIManager.Add(m_DeadShowDialog);
		UIBlock uIBlock = new UIBlock();
		uIBlock.Rect = new Rect(0f, 0f, Screen.width, Screen.height);
		m_DeadShowDialog.Add(uIBlock);
		if (m_MatBattleDialog == null)
		{
			m_MatBattleDialog = LoadUIMaterial("Zombie3D/UI/Materials/BattleDialog01UI");
		}
		Resources.UnloadUnusedAssets();
		UIImage control = UIUtils.BuildImage(0, new Rect(Screen.width / 2 - 300, Screen.height / 2 - 190, 600f, 380f), m_MatBattleDialog, new Rect(0f, 0f, 600f, 380f), new Vector2(600f, 380f));
		m_DeadShowDialog.Add(control);
		UIText uIText = UIUtils.BuildUIText(0, new Rect(Screen.width / 2 + 60, Screen.height / 2, 100f, 40f), UIText.enAlignStyle.left);
		uIText.Set("Zombie3D/Font/037-CAI978-27", "X " + num, Color.white);
		m_DeadShowDialog.Add(uIText);
		uIText = UIUtils.BuildUIText(0, new Rect(Screen.width / 2 - 230, Screen.height / 2 - 120, 450f, 90f), UIText.enAlignStyle.left);
		uIText.Set("Zombie3D/Font/037-CAI978-18", "Do you want to use Epinephrine to continue the fight? (Epinephrine can be bought in the shop.)", Constant.TextCommonColor);
		m_DeadShowDialog.Add(uIText);
		UIClickButton uIClickButton = null;
		if (num > 0)
		{
			uIClickButton = UIUtils.BuildClickButton(3010, new Rect(Screen.width / 2 - 50 - 192, Screen.height / 2 - 200, 192f, 62f), m_MatDialog01, new Rect(640f, 62f, 192f, 62f), new Rect(832f, 62f, 192f, 62f), new Rect(0f, 682f, 192f, 62f), new Vector2(192f, 62f));
			m_DeadShowDialog.Add(uIClickButton);
		}
		else
		{
			control = UIUtils.BuildImage(0, new Rect(Screen.width / 2 - 50 - 192, Screen.height / 2 - 200, 192f, 62f), m_MatDialog01, new Rect(0f, 682f, 192f, 62f), new Vector2(192f, 62f));
			m_DeadShowDialog.Add(control);
		}
		uIClickButton = UIUtils.BuildClickButton(3011, new Rect(Screen.width / 2 + 50, Screen.height / 2 - 200, 192f, 62f), m_MatDialog01, new Rect(640f, 248f, 192f, 62f), new Rect(832f, 248f, 192f, 62f), new Rect(640f, 248f, 192f, 62f), new Vector2(192f, 62f));
		m_DeadShowDialog.Add(uIClickButton);
	}

	public void SetupGameStartTap()
	{
		if (m_DesktopGroup == null)
		{
			m_DesktopGroup = new uiGroup(m_UIManager);
		}
		if (gameState.m_eGameMode.m_ePlayMode == GameState.NetworkGameMode.PlayMode.E_Console)
		{
			UIClickButton control = UIUtils.BuildClickButton(3012, new Rect(-100f, -100f, 2000f, 2000f), m_MatBattleUI, new Rect(1f, 1f, 1f, 1f), new Rect(1f, 1f, 1f, 1f), new Rect(1f, 1f, 1f, 1f), new Vector2(2000f, 2000f));
			m_DesktopGroup.Add(control);
		}
	}

	public void SetupGameBeginEffect()
	{
		if (m_MatGamtStartEffect == null)
		{
			m_MatGamtStartEffect = LoadUIMaterial("Zombie3D/UI/Materials/GameStartEffectUI");
			Resources.UnloadUnusedAssets();
		}
		UIAnimationControl uIAnimationControl = new UIAnimationControl();
		uIAnimationControl = new UIAnimationControl();
		uIAnimationControl.Id = 3017;
		uIAnimationControl.SetAnimationsPageCount(4);
		uIAnimationControl.Rect = AutoUI.AutoRect(new Rect(93f, 222f, 774f, 195f));
		uIAnimationControl.SetTexture(0, m_MatGamtStartEffect, AutoUI.AutoRect(new Rect(0f, 0f, 774f, 195f)), AutoUI.AutoSize(new Vector2(774f, 195f)));
		uIAnimationControl.SetTexture(1, m_MatGamtStartEffect, AutoUI.AutoRect(new Rect(0f, 195f, 774f, 195f)), AutoUI.AutoSize(new Vector2(774f, 195f)));
		uIAnimationControl.SetTexture(2, m_MatGamtStartEffect, AutoUI.AutoRect(new Rect(0f, 390f, 774f, 195f)), AutoUI.AutoSize(new Vector2(774f, 195f)));
		uIAnimationControl.SetTexture(3, m_MatGamtStartEffect, AutoUI.AutoRect(new Rect(0f, 585f, 774f, 195f)), AutoUI.AutoSize(new Vector2(774f, 195f)));
		uIAnimationControl.SetTimeInterval(0.08f);
		uIAnimationControl.SetLoopCount(1000000);
		m_DesktopGroup.Add(uIAnimationControl);
	}

	public void SetupWavePassedEffect()
	{
		if (m_Effect01 != null)
		{
			m_Effect01.Clear();
			m_Effect01 = null;
		}
		m_Effect01 = new UIEffect01(m_UIManager);
		UIEffect01.Effect01DataItem effect01DataItem = null;
		effect01DataItem = new UIEffect01.Effect01DataItem();
		effect01DataItem.position = new Vector2(-1000f, 355f);
		effect01DataItem.time = 0.5f;
		m_Effect01.AddData(effect01DataItem);
		effect01DataItem = new UIEffect01.Effect01DataItem();
		effect01DataItem.position = new Vector2(207f, 355f);
		effect01DataItem.time = 1f;
		m_Effect01.AddData(effect01DataItem);
		effect01DataItem = new UIEffect01.Effect01DataItem();
		effect01DataItem.position = new Vector2(207f, 355f);
		effect01DataItem.time = 1f;
		m_Effect01.AddData(effect01DataItem);
		effect01DataItem = new UIEffect01.Effect01DataItem();
		effect01DataItem.position = new Vector2(1000f, 355f);
		effect01DataItem.time = 1f;
		m_Effect01.AddData(effect01DataItem);
		UIGroupControl uIGroupControl = UIUtils.BuildUIGroupControl(0, new Rect(0f, 355f, 500f, 500f));
		if (m_MatPerfectWaveEffect == null)
		{
			m_MatPerfectWaveEffect = LoadUIMaterial("Zombie3D/UI/Materials/PerfectWaveEffectUI");
			Resources.UnloadUnusedAssets();
		}
		UIImage uIImage = null;
		uIImage = UIUtils.BuildImage(0, new Rect(0f, 350f, 200f, 56f), m_MatPerfectWaveEffect, new Rect(0f, 900f, 200f, 56f), new Vector2(200f, 56f));
		uIGroupControl.Add(uIImage);
		float num = 0f;
		int battleWaves = gameState.m_BattleWaves;
		string text = battleWaves.ToString();
		if (text.Length < 2)
		{
			Rect waveShowFontTexRect = GetWaveShowFontTexRect(battleWaves);
			uIImage = UIUtils.BuildImage(0, new Rect(210f, 353f, waveShowFontTexRect.width, waveShowFontTexRect.height), m_MatPerfectWaveEffect, waveShowFontTexRect, new Vector2(waveShowFontTexRect.width, waveShowFontTexRect.height));
			uIGroupControl.Add(uIImage);
			num = waveShowFontTexRect.width;
		}
		else
		{
			int num2 = battleWaves / 10;
			Rect waveShowFontTexRect2 = GetWaveShowFontTexRect(num2);
			int num3 = battleWaves % 10;
			Rect waveShowFontTexRect3 = GetWaveShowFontTexRect(num3);
			UIImage control = UIUtils.BuildImage(0, new Rect(210f, 353f, waveShowFontTexRect2.width, waveShowFontTexRect2.height), m_MatPerfectWaveEffect, waveShowFontTexRect2, new Vector2(waveShowFontTexRect2.width, waveShowFontTexRect2.height));
			uIGroupControl.Add(control);
			UIImage control2 = UIUtils.BuildImage(0, new Rect(210f + waveShowFontTexRect2.width, 353f, waveShowFontTexRect3.width, waveShowFontTexRect3.height), m_MatPerfectWaveEffect, waveShowFontTexRect3, new Vector2(waveShowFontTexRect3.width, waveShowFontTexRect3.height));
			uIGroupControl.Add(control2);
			num = waveShowFontTexRect2.width + waveShowFontTexRect3.width;
		}
		uIImage = UIUtils.BuildImage(0, new Rect(210f + num + 10f, 350f, 370f, 56f), m_MatPerfectWaveEffect, new Rect(237f, 900f, 370f, 56f), new Vector2(370f, 56f));
		uIGroupControl.Add(uIImage);
		m_Effect01.Group = uIGroupControl;
		m_Effect01.Update(Time.deltaTime);
		if (m_Effect02 != null)
		{
			m_Effect02.Clear();
			m_Effect02 = null;
		}
		m_Effect02 = new UIEffect01(m_UIManager);
		UIEffect01.Effect01DataItem effect01DataItem2 = null;
		effect01DataItem2 = new UIEffect01.Effect01DataItem();
		effect01DataItem2.position = new Vector2(1000f, 260f);
		effect01DataItem2.time = 0.5f;
		m_Effect02.AddData(effect01DataItem2);
		effect01DataItem2 = new UIEffect01.Effect01DataItem();
		effect01DataItem2.position = new Vector2(374f, 260f);
		effect01DataItem2.time = 1f;
		m_Effect02.AddData(effect01DataItem2);
		effect01DataItem2 = new UIEffect01.Effect01DataItem();
		effect01DataItem2.position = new Vector2(374f, 260f);
		effect01DataItem2.time = 1f;
		m_Effect02.AddData(effect01DataItem2);
		effect01DataItem2 = new UIEffect01.Effect01DataItem();
		effect01DataItem2.position = new Vector2(-1000f, 260f);
		effect01DataItem2.time = 1f;
		m_Effect02.AddData(effect01DataItem2);
		UIGroupControl uIGroupControl2 = UIUtils.BuildUIGroupControl(0, new Rect(0f, 260f, 500f, 500f));
		uIImage = UIUtils.BuildImage(0, new Rect(0f, 260f, 200f, 40f), m_MatPerfectWaveEffect, new Rect(620f, 908f, 200f, 40f), new Vector2(200f, 40f));
		uIGroupControl2.Add(uIImage);
		m_Effect02.Group = uIGroupControl2;
		m_Effect02.Update(Time.deltaTime);
		m_EffectGroup = new uiGroup(m_UIManager);
		uIImage = UIUtils.BuildImage(0, new Rect(1f, 310f, 958f, 34f), m_MatPerfectWaveEffect, new Rect(0f, 956f, 958f, 34f), new Vector2(958f, 34f));
		m_EffectGroup.Add(uIImage);
	}

	public void SetupPerfectWavePassedEffect()
	{
		Debug.Log("SetupPerfectWavePassedEffect");
		SceneUIManager.Instance().SetMusicPlayerState(SceneUIManager.MusicState_PerfectWaveAudioState);
		if (m_Effect01 != null)
		{
			m_Effect01.Clear();
			m_Effect01 = null;
		}
		m_Effect01 = new UIEffect01(m_UIManager);
		UIEffect01.Effect01DataItem effect01DataItem = null;
		effect01DataItem = new UIEffect01.Effect01DataItem();
		effect01DataItem.position = new Vector2(-1000f, 230f);
		effect01DataItem.time = 0.1f;
		m_Effect01.AddData(effect01DataItem);
		effect01DataItem = new UIEffect01.Effect01DataItem();
		effect01DataItem.position = new Vector2(180f, 230f);
		effect01DataItem.time = 0.5f;
		m_Effect01.AddData(effect01DataItem);
		effect01DataItem = new UIEffect01.Effect01DataItem();
		effect01DataItem.position = new Vector2(180f, 230f);
		effect01DataItem.time = 1f;
		m_Effect01.AddData(effect01DataItem);
		effect01DataItem = new UIEffect01.Effect01DataItem();
		effect01DataItem.position = new Vector2(1000f, 230f);
		effect01DataItem.time = 1f;
		m_Effect01.AddData(effect01DataItem);
		UIGroupControl uIGroupControl = UIUtils.BuildUIGroupControl(0, new Rect(0f, 230f, 500f, 500f));
		if (m_MatPerfectWaveEffect == null)
		{
			m_MatPerfectWaveEffect = LoadUIMaterial("Zombie3D/UI/Materials/PerfectWaveEffectUI");
			Resources.UnloadUnusedAssets();
		}
		UIImage control = UIUtils.BuildImage(0, new Rect(0f, 340f, 508f, 55f), m_MatPerfectWaveEffect, new Rect(516f, 478f, 508f, 55f), new Vector2(508f, 55f));
		uIGroupControl.Add(control);
		m_Effect01.Group = uIGroupControl;
		m_Effect01.Update(Time.deltaTime);
		if (m_Effect02 != null)
		{
			m_Effect02.Clear();
			m_Effect02 = null;
		}
		m_Effect02 = new UIEffect01(m_UIManager);
		UIEffect01.Effect01DataItem effect01DataItem2 = null;
		effect01DataItem2 = new UIEffect01.Effect01DataItem();
		effect01DataItem2.position = new Vector2(1000f, 260f);
		effect01DataItem2.time = 0.1f;
		m_Effect02.AddData(effect01DataItem2);
		effect01DataItem2 = new UIEffect01.Effect01DataItem();
		effect01DataItem2.position = new Vector2(213f, 260f);
		effect01DataItem2.time = 0.5f;
		m_Effect02.AddData(effect01DataItem2);
		effect01DataItem2 = new UIEffect01.Effect01DataItem();
		effect01DataItem2.position = new Vector2(213f, 260f);
		effect01DataItem2.time = 1f;
		m_Effect02.AddData(effect01DataItem2);
		effect01DataItem2 = new UIEffect01.Effect01DataItem();
		effect01DataItem2.position = new Vector2(-1000f, 260f);
		effect01DataItem2.time = 1f;
		m_Effect02.AddData(effect01DataItem2);
		UIGroupControl uIGroupControl2 = UIUtils.BuildUIGroupControl(0, new Rect(0f, 260f, 500f, 500f));
		control = UIUtils.BuildImage(0, new Rect(0f, 270f, 200f, 40f), m_MatPerfectWaveEffect, new Rect(822f, 908f, 200f, 40f), new Vector2(200f, 40f));
		uIGroupControl2.Add(control);
		m_Effect02.Group = uIGroupControl2;
		m_Effect02.Update(Time.deltaTime);
		m_EffectGroup = new uiGroup(m_UIManager);
		control = UIUtils.BuildImage(0, new Rect(1f, 310f, 958f, 34f), m_MatPerfectWaveEffect, new Rect(0f, 990f, 958f, 34f), new Vector2(958f, 34f));
		m_EffectGroup.Add(control);
		UIAnimationControl uIAnimationControl = new UIAnimationControl();
		uIAnimationControl = new UIAnimationControl();
		uIAnimationControl.Id = 0;
		uIAnimationControl.SetAnimationsPageCount(16);
		uIAnimationControl.Rect = AutoUI.AutoRect(new Rect(256f, 100f, 468f, 472f));
		int num = 0;
		int num2 = 6;
		for (int i = 0; i < num2; i++)
		{
			uIAnimationControl.SetTexture(num++, m_MatPerfectWaveEffect, AutoUI.AutoRect(new Rect(0f, 0f, 0f, 0f)), AutoUI.AutoSize(new Vector2(468f, 472f)));
		}
		uIAnimationControl.SetTexture(num++, m_MatPerfectWaveEffect, AutoUI.AutoRect(new Rect(0f, 0f, 234f, 236f)), AutoUI.AutoSize(new Vector2(468f, 472f)));
		uIAnimationControl.SetTexture(num++, m_MatPerfectWaveEffect, AutoUI.AutoRect(new Rect(234f, 0f, 234f, 236f)), AutoUI.AutoSize(new Vector2(468f, 472f)));
		uIAnimationControl.SetTexture(num++, m_MatPerfectWaveEffect, AutoUI.AutoRect(new Rect(468f, 0f, 234f, 236f)), AutoUI.AutoSize(new Vector2(468f, 472f)));
		uIAnimationControl.SetTexture(num++, m_MatPerfectWaveEffect, AutoUI.AutoRect(new Rect(702f, 0f, 234f, 236f)), AutoUI.AutoSize(new Vector2(468f, 472f)));
		uIAnimationControl.SetTexture(num++, m_MatPerfectWaveEffect, AutoUI.AutoRect(new Rect(0f, 236f, 234f, 236f)), AutoUI.AutoSize(new Vector2(468f, 472f)));
		uIAnimationControl.SetTexture(num++, m_MatPerfectWaveEffect, AutoUI.AutoRect(new Rect(234f, 236f, 234f, 236f)), AutoUI.AutoSize(new Vector2(468f, 472f)));
		uIAnimationControl.SetTexture(num++, m_MatPerfectWaveEffect, AutoUI.AutoRect(new Rect(468f, 236f, 234f, 236f)), AutoUI.AutoSize(new Vector2(468f, 472f)));
		uIAnimationControl.SetTexture(num++, m_MatPerfectWaveEffect, AutoUI.AutoRect(new Rect(702f, 236f, 234f, 236f)), AutoUI.AutoSize(new Vector2(468f, 472f)));
		uIAnimationControl.SetTexture(num++, m_MatPerfectWaveEffect, AutoUI.AutoRect(new Rect(0f, 472f, 234f, 236f)), AutoUI.AutoSize(new Vector2(468f, 472f)));
		uIAnimationControl.SetTexture(num++, m_MatPerfectWaveEffect, AutoUI.AutoRect(new Rect(234f, 472f, 234f, 236f)), AutoUI.AutoSize(new Vector2(468f, 472f)));
		uIAnimationControl.SetTimeInterval(0.05f);
		uIAnimationControl.SetLoopCount(1);
		m_EffectGroup.Add(uIAnimationControl);
	}

	public void StagePassed()
	{
		m_bStagePassed = true;
		m_MoveJoystickBtn.Reset();
		((TopWatchingInputController)player.InputController).MoveDirection = Vector3.zero;
		player.StopRun();
		player.SetState(Player.IDLE_STATE);
		m_ShootJoystickBtn.Reset();
		((TopWatchingInputController)player.InputController).bFire = false;
		player.StopFire();
		SetupStagePassedEffect();
		SetupControlCoverUI(true);
	}

	public void StageLosed()
	{
		m_bStagePassed = true;
		m_MoveJoystickBtn.Reset();
		((TopWatchingInputController)player.InputController).MoveDirection = Vector3.zero;
		player.StopRun();
		player.SetState(Player.IDLE_STATE);
		m_ShootJoystickBtn.Reset();
		((TopWatchingInputController)player.InputController).bFire = false;
		player.StopFire();
		SetupStageLosedEffect();
		SetupControlCoverUI(true);
	}

	public void SetupStagePassedEffect()
	{
		if (m_Effect01 != null)
		{
			m_Effect01.Clear();
			m_Effect01 = null;
		}
		m_Effect01 = new UIEffect01(m_UIManager);
		UIEffect01.Effect01DataItem effect01DataItem = null;
		effect01DataItem = new UIEffect01.Effect01DataItem();
		effect01DataItem.position = new Vector2(-1000f, 355f);
		effect01DataItem.time = 0.5f;
		m_Effect01.AddData(effect01DataItem);
		effect01DataItem = new UIEffect01.Effect01DataItem();
		effect01DataItem.position = new Vector2(108f, 355f);
		effect01DataItem.time = 1f;
		m_Effect01.AddData(effect01DataItem);
		effect01DataItem = new UIEffect01.Effect01DataItem();
		effect01DataItem.position = new Vector2(108f, 355f);
		effect01DataItem.time = 1f;
		m_Effect01.AddData(effect01DataItem);
		effect01DataItem = new UIEffect01.Effect01DataItem();
		effect01DataItem.position = new Vector2(1000f, 355f);
		effect01DataItem.time = 1f;
		m_Effect01.AddData(effect01DataItem);
		UIGroupControl uIGroupControl = UIUtils.BuildUIGroupControl(0, new Rect(0f, 355f, 500f, 500f));
		if (m_MatPerfectWaveEffect == null)
		{
			m_MatPerfectWaveEffect = LoadUIMaterial("Zombie3D/UI/Materials/PerfectWaveEffectUI");
			Resources.UnloadUnusedAssets();
		}
		UIImage uIImage = null;
		uIImage = UIUtils.BuildImage(0, new Rect(0f, 350f, 764f, 61f), m_MatPerfectWaveEffect, new Rect(0f, 765f, 764f, 61f), new Vector2(764f, 61f));
		uIGroupControl.Add(uIImage);
		m_Effect01.Group = uIGroupControl;
		m_Effect01.Update(Time.deltaTime);
		if (m_Effect02 != null)
		{
			m_Effect02.Clear();
			m_Effect02 = null;
		}
		m_Effect02 = new UIEffect01(m_UIManager);
		UIEffect01.Effect01DataItem effect01DataItem2 = null;
		effect01DataItem2 = new UIEffect01.Effect01DataItem();
		effect01DataItem2.position = new Vector2(1000f, 260f);
		effect01DataItem2.time = 0.5f;
		m_Effect02.AddData(effect01DataItem2);
		effect01DataItem2 = new UIEffect01.Effect01DataItem();
		effect01DataItem2.position = new Vector2(182f, 260f);
		effect01DataItem2.time = 1f;
		m_Effect02.AddData(effect01DataItem2);
		effect01DataItem2 = new UIEffect01.Effect01DataItem();
		effect01DataItem2.position = new Vector2(182f, 260f);
		effect01DataItem2.time = 1f;
		m_Effect02.AddData(effect01DataItem2);
		effect01DataItem2 = new UIEffect01.Effect01DataItem();
		effect01DataItem2.position = new Vector2(-1000f, 260f);
		effect01DataItem2.time = 1f;
		m_Effect02.AddData(effect01DataItem2);
		UIGroupControl uIGroupControl2 = UIUtils.BuildUIGroupControl(0, new Rect(0f, 260f, 500f, 500f));
		uIImage = UIUtils.BuildImage(0, new Rect(0f, 260f, 590f, 52f), m_MatPerfectWaveEffect, new Rect(0f, 835f, 590f, 52f), new Vector2(590f, 52f));
		uIGroupControl2.Add(uIImage);
		m_Effect02.Group = uIGroupControl2;
		m_Effect02.Update(Time.deltaTime);
		m_EffectGroup = new uiGroup(m_UIManager);
		uIImage = UIUtils.BuildImage(0, new Rect(1f, 310f, 958f, 34f), m_MatPerfectWaveEffect, new Rect(0f, 990f, 958f, 34f), new Vector2(958f, 34f));
		m_EffectGroup.Add(uIImage);
	}

	public void SetupStageLosedEffect()
	{
		if (m_Effect01 != null)
		{
			m_Effect01.Clear();
			m_Effect01 = null;
		}
		m_Effect01 = new UIEffect01(m_UIManager);
		UIEffect01.Effect01DataItem effect01DataItem = null;
		effect01DataItem = new UIEffect01.Effect01DataItem();
		effect01DataItem.position = new Vector2(-1000f, 355f);
		effect01DataItem.time = 0.5f;
		m_Effect01.AddData(effect01DataItem);
		effect01DataItem = new UIEffect01.Effect01DataItem();
		effect01DataItem.position = new Vector2(300f, 355f);
		effect01DataItem.time = 1f;
		m_Effect01.AddData(effect01DataItem);
		effect01DataItem = new UIEffect01.Effect01DataItem();
		effect01DataItem.position = new Vector2(300f, 355f);
		effect01DataItem.time = 1f;
		m_Effect01.AddData(effect01DataItem);
		effect01DataItem = new UIEffect01.Effect01DataItem();
		effect01DataItem.position = new Vector2(1000f, 355f);
		effect01DataItem.time = 1f;
		m_Effect01.AddData(effect01DataItem);
		UIGroupControl uIGroupControl = UIUtils.BuildUIGroupControl(0, new Rect(0f, 355f, 500f, 500f));
		if (m_MatPerfectWaveEffect == null)
		{
			m_MatPerfectWaveEffect = LoadUIMaterial("Zombie3D/UI/Materials/PerfectWaveEffectUI");
			Resources.UnloadUnusedAssets();
		}
		UIImage uIImage = null;
		uIImage = UIUtils.BuildImage(0, new Rect(0f, 350f, 347f, 75f), m_MatPerfectWaveEffect, new Rect(607f, 828f, 347f, 75f), new Vector2(347f, 75f));
		uIGroupControl.Add(uIImage);
		m_Effect01.Group = uIGroupControl;
		m_Effect01.Update(Time.deltaTime);
		if (m_Effect02 != null)
		{
			m_Effect02.Clear();
			m_Effect02 = null;
		}
		m_Effect02 = new UIEffect01(m_UIManager);
		UIEffect01.Effect01DataItem effect01DataItem2 = null;
		effect01DataItem2 = new UIEffect01.Effect01DataItem();
		effect01DataItem2.position = new Vector2(1000f, 260f);
		effect01DataItem2.time = 0.5f;
		m_Effect02.AddData(effect01DataItem2);
		effect01DataItem2 = new UIEffect01.Effect01DataItem();
		effect01DataItem2.position = new Vector2(182f, 260f);
		effect01DataItem2.time = 1f;
		m_Effect02.AddData(effect01DataItem2);
		effect01DataItem2 = new UIEffect01.Effect01DataItem();
		effect01DataItem2.position = new Vector2(182f, 260f);
		effect01DataItem2.time = 1f;
		m_Effect02.AddData(effect01DataItem2);
		effect01DataItem2 = new UIEffect01.Effect01DataItem();
		effect01DataItem2.position = new Vector2(-1000f, 260f);
		effect01DataItem2.time = 1f;
		m_Effect02.AddData(effect01DataItem2);
		UIGroupControl uIGroupControl2 = UIUtils.BuildUIGroupControl(0, new Rect(0f, 260f, 500f, 500f));
		uIImage = UIUtils.BuildImage(0, new Rect(0f, 260f, 1f, 1f), m_MatPerfectWaveEffect, new Rect(0f, 0f, 1f, 1f), new Vector2(1f, 1f));
		uIGroupControl2.Add(uIImage);
		m_Effect02.Group = uIGroupControl2;
		m_Effect02.Update(Time.deltaTime);
		m_EffectGroup = new uiGroup(m_UIManager);
	}

	public void SetupEnemiesDirectionUI(bool bShow)
	{
		if (m_EnemiesDirectionGroup != null)
		{
			m_EnemiesDirectionGroup.Clear();
			m_EnemiesDirectionGroup = null;
		}
		m_EnemiesDirectionGroup = new uiGroup(m_UIManager);
		int num = 0;
		Hashtable enemies = GameApp.GetInstance().GetGameScene().GetEnemies();
		foreach (Enemy value in enemies.Values)
		{
			if (value == null)
			{
				continue;
			}
			Vector3 direction = value.GetTransform().position - player.GetTransform().position;
			if (direction.magnitude > 10f)
			{
				Vector3 normalized = player.GetRespawnTransform().InverseTransformDirection(direction).normalized;
				float num2 = Mathf.Atan2(normalized.z, normalized.x);
				Vector2 vector = new Vector2((float)(Screen.width / 2) + 500f * Mathf.Cos(num2), (float)(Screen.height / 2) + 500f * Mathf.Sin(num2));
				float num3 = vector.x;
				if (num3 < 140f)
				{
					num3 = 140f;
				}
				if (num3 > (float)(Screen.width - 140))
				{
					num3 = Screen.width - 140;
				}
				float num4 = vector.y;
				if (num4 < 120f)
				{
					num4 = 120f;
				}
				if (num4 > (float)(Screen.height - 100))
				{
					num4 = Screen.height - 100;
				}
				vector = new Vector2(num3, num4);
				if (num >= m_EnemiesDirection.Count)
				{
					UIImage item = UIUtils.BuildImage(0, new Rect(0f, 0f, 38f, 35f), m_MatBattleUI, new Rect(375f, 54f, 38f, 35f), new Vector2(38f, 35f));
					m_EnemiesDirection.Add(item);
				}
				m_EnemiesDirection[num].Rect = AutoUI.AutoRect(new Rect(vector.x, vector.y, 38f, 35f));
				m_EnemiesDirection[num].SetRotation(num2);
				m_EnemiesDirection[num].CatchMessage = false;
				m_EnemiesDirectionGroup.Add(m_EnemiesDirection[num]);
				num++;
			}
		}
	}

	public void SetupHintDialog(bool bShow, int okId, int yesId, int noId, string dialog_content)
	{
		if (m_uiHintDialog != null)
		{
			m_uiHintDialog.Clear();
			m_uiHintDialog = null;
		}
		if (bShow)
		{
			m_uiHintDialog = new uiGroup(m_UIManager);
			if (m_MatDialog01 == null)
			{
				m_MatDialog01 = LoadUIMaterial("Zombie3D/UI/Materials/Dialog01");
			}
			UIImage control = UIUtils.BuildImage(0, new Rect(0f, 0f, 960f, 640f), m_MatDialog01, new Rect(600f, 1f, 1f, 1f), new Vector2(960f, 640f));
			m_uiHintDialog.Add(control);
			float num = 215f;
			float num2 = 167f;
			control = UIUtils.BuildImage(0, new Rect(num, num2, 515f, 301f), m_MatDialog01, new Rect(0f, 378f, 515f, 301f), new Vector2(515f, 301f));
			m_uiHintDialog.Add(control);
			UIText uIText = UIUtils.BuildUIText(0, new Rect(num + 40f, num2 + 40f, 420f, 173f), UIText.enAlignStyle.left);
			uIText.Set("Zombie3D/Font/Arial12_bold", dialog_content, Color.white);
			m_uiHintDialog.Add(uIText);
			UIClickButton uIClickButton = null;
			if (okId > 0)
			{
				uIClickButton = UIUtils.BuildClickButton(okId, new Rect(num + 154f, num2 - 16f, 191f, 62f), m_MatDialog01, new Rect(640f, 0f, 191f, 62f), new Rect(832f, 0f, 191f, 62f), new Rect(640f, 0f, 191f, 62f), new Vector2(191f, 62f));
				m_uiHintDialog.Add(uIClickButton);
			}
			if (noId > 0)
			{
				uIClickButton = UIUtils.BuildClickButton(noId, new Rect(num + 21f, num2 - 16f, 191f, 62f), m_MatDialog01, new Rect(640f, 124f, 191f, 62f), new Rect(832f, 124f, 191f, 62f), new Rect(640f, 124f, 191f, 62f), new Vector2(191f, 62f));
				m_uiHintDialog.Add(uIClickButton);
			}
			if (yesId > 0)
			{
				uIClickButton = UIUtils.BuildClickButton(yesId, new Rect(num + 280f, num2 - 16f, 191f, 62f), m_MatDialog01, new Rect(640f, 62f, 191f, 62f), new Rect(832f, 62f, 191f, 62f), new Rect(640f, 62f, 191f, 62f), new Vector2(191f, 62f));
				m_uiHintDialog.Add(uIClickButton);
			}
		}
	}

	public void Map3_Type_Win(List<KeyValuePair<int, int>> giftItem)
	{
		m_Map3RewardItem = giftItem;
		m_Map3WinTimer = 0f;
	}

	public void SetupMap3WinRewardUI(bool bShow, List<KeyValuePair<int, int>> giftItem)
	{
		if (m_uiMap3WinRewardDialog != null)
		{
			m_uiMap3WinRewardDialog.Clear();
			m_uiMap3WinRewardDialog = null;
		}
		if (!bShow)
		{
			return;
		}
		m_uiMap3WinRewardDialog = new uiGroup(m_UIManager);
		if (m_MatDialog01 == null)
		{
			m_MatDialog01 = LoadUIMaterial("Zombie3D/UI/Materials/Dialog01");
		}
		UIImage control = UIUtils.BuildImage(0, new Rect(0f, 0f, 960f, 640f), m_MatDialog01, new Rect(600f, 1f, 1f, 1f), new Vector2(960f, 640f));
		m_uiMap3WinRewardDialog.Add(control);
		float num = 215f;
		float num2 = 167f;
		control = UIUtils.BuildImage(0, new Rect(num, num2, 515f, 301f), m_MatDialog01, new Rect(0f, 378f, 515f, 301f), new Vector2(515f, 301f));
		m_uiMap3WinRewardDialog.Add(control);
		float num3 = 460f;
		float num4 = 325f;
		if (m_MatPowerUPSIcons == null)
		{
			m_MatPowerUPSIcons = LoadUIMaterial("Zombie3D/UI/Materials/PowerUPSIcons");
		}
		Resources.UnloadUnusedAssets();
		ArrayList arrayList = new ArrayList();
		Vector2 vector = new Vector2(92f, 92f);
		ArrayList arrayList2 = new ArrayList();
		arrayList2.Add(new Vector2(num3 - vector.x / 2f, num4 - vector.y / 2f));
		arrayList.Add(arrayList2);
		ArrayList arrayList3 = new ArrayList();
		arrayList3.Add(new Vector2(num3 - vector.x - vector.x / 10f, num4 - vector.y / 2f));
		arrayList3.Add(new Vector2(num3 + vector.x / 10f, num4 - vector.y / 2f));
		arrayList.Add(arrayList3);
		ArrayList arrayList4 = new ArrayList();
		arrayList4.Add(new Vector2(num3 - vector.x / 2f - vector.x - vector.x / 10f, num4 - vector.y / 2f));
		arrayList4.Add(new Vector2(num3 - vector.x / 2f, num4 - vector.y / 2f));
		arrayList4.Add(new Vector2(num3 + vector.x / 2f + vector.x / 10f, num4 - vector.y / 2f));
		arrayList.Add(arrayList4);
		for (int i = 0; i < giftItem.Count; i++)
		{
			ArrayList arrayList5 = (ArrayList)arrayList[giftItem.Count - 1];
			Vector2 vector2 = (Vector2)arrayList5[i];
			if (giftItem[i].Key >= 0)
			{
				Rect powerUpsIconTexture = ShopUIScript.GetPowerUpsIconTexture((ItemType)giftItem[i].Key);
				control = UIUtils.BuildImage(0, new Rect(vector2.x, vector2.y, powerUpsIconTexture.width, powerUpsIconTexture.height), m_MatPowerUPSIcons, powerUpsIconTexture, new Vector2(powerUpsIconTexture.width, powerUpsIconTexture.height));
				m_uiMap3WinRewardDialog.Add(control);
			}
			else
			{
				Rect rcMat = new Rect(195f, 125f, 103f, 96f);
				control = UIUtils.BuildImage(0, new Rect(vector2.x, vector2.y, rcMat.width, rcMat.height), m_MatBattleUI, rcMat, new Vector2(rcMat.width, rcMat.height));
				m_uiMap3WinRewardDialog.Add(control);
			}
		}
		UIText uIText = UIUtils.BuildUIText(0, new Rect(num + 70f, num2 + 80f, 400f, 25f), UIText.enAlignStyle.left);
		uIText.Set("Zombie3D/Font/037-CAI978-15", "Boss defeated! You found some loot.", Constant.TextCommonColor);
		m_uiMap3WinRewardDialog.Add(uIText);
		UIClickButton uIClickButton = null;
		uIClickButton = UIUtils.BuildClickButton(3021, new Rect(num + 154f, num2 - 16f, 191f, 62f), m_MatDialog01, new Rect(640f, 0f, 191f, 62f), new Rect(832f, 0f, 191f, 62f), new Rect(640f, 0f, 191f, 62f), new Vector2(191f, 62f));
		m_uiMap3WinRewardDialog.Add(uIClickButton);
	}

	public void SetupEnemiesLeftInfoUI(bool bShow)
	{
		if (m_EnemiesLeftInfoGroup != null)
		{
			m_EnemiesLeftInfoGroup.Clear();
			m_EnemiesLeftInfoGroup = null;
		}
		if (bShow)
		{
			m_EnemiesLeftInfoGroup = new uiGroup(m_UIManager);
			UIImage control = UIUtils.BuildImage(0, new Rect(Screen.width - 80, Screen.height - 62, 66f, 58f), m_MatBattleUI, new Rect(383f, 98f, 66f, 58f), new Vector2(66f, 58f), 1);
			m_EnemiesLeftInfoGroup.Add(control);
			float value = (float)(gameScene.DDSTrigger.AllEnemiesOfCurWave + gameScene.DDSTrigger.GenExternEnemiesCountOfCurWave - gameScene.CurWaveKilled) / (float)(gameScene.DDSTrigger.AllEnemiesOfCurWave + gameScene.DDSTrigger.GenExternEnemiesCountOfCurWave);
			value = Mathf.Clamp01(value);
			string text = string.Format("{0:0.##}", value * 100f);
			UIText uIText = UIUtils.BuildUIText(0, new Rect(Screen.width - 200, Screen.height - 62, 200f, 25f), UIText.enAlignStyle.right, 1);
			uIText.Set("Zombie3D/Font/037-CAI978-18", text + "%", Constant.TextCommonColor);
			m_EnemiesLeftInfoGroup.Add(uIText);
		}
	}

	public void SetupPointsWavesInfoUI(bool bShow)
	{
		if (m_EnemiesLeftInfoGroup != null)
		{
			m_EnemiesLeftInfoGroup.Clear();
			m_EnemiesLeftInfoGroup = null;
		}
		if (bShow)
		{
			m_EnemiesLeftInfoGroup = new uiGroup(m_UIManager);
			UIImage control = UIUtils.BuildImage(0, new Rect(Screen.width - 80, Screen.height - 62, 66f, 58f), m_MatBattleUI, new Rect(383f, 98f, 66f, 58f), new Vector2(66f, 58f), 1);
			m_EnemiesLeftInfoGroup.Add(control);
			string text = string.Format(string.Concat(str2: ConfigManager.Instance().GetFixedConfig().GetMaxWavesOfPoints(gameScene.DDSTrigger.MapIndex, gameScene.DDSTrigger.PointsIndex)
				.ToString(), str0: gameScene.DDSTrigger.WaveIndex.ToString(), str1: "/"));
			UIText uIText = UIUtils.BuildUIText(0, new Rect(Screen.width - 45 - 100, Screen.height - 62, 200f, 25f), UIText.enAlignStyle.center, 1);
			uIText.Set("Zombie3D/Font/037-CAI978-18", text, Constant.TextCommonColor);
			m_EnemiesLeftInfoGroup.Add(uIText);
		}
	}

	public void ShowSurvivalModeIndicatorUI()
	{
		m_bSurvivalModeGotoNextSceneIndicator = true;
		GameObject survivalModeExitDoor_Parent = GameApp.GetInstance().GetGameConfig().SurvivalModeExitDoor_Parent;
		if (survivalModeExitDoor_Parent != null)
		{
			m_SurvivalModeExitDoors = survivalModeExitDoor_Parent.GetComponentsInChildren<SurvivalModeExitDoor>();
			if (m_SurvivalModeArrowGroup == null)
			{
				m_SurvivalModeArrowGroup = new uiGroup(m_UIManager);
			}
			m_SurvivalModeLeftIndicatorDirection = new List<UIAnimationControl>();
			for (int i = 0; i < m_SurvivalModeExitDoors.Length; i++)
			{
				UIAnimationControl uIAnimationControl = new UIAnimationControl();
				uIAnimationControl.Id = 0;
				uIAnimationControl.SetAnimationsPageCount(4);
				uIAnimationControl.Rect = AutoUI.AutoRect(new Rect(0f, 0f, 92f, 76f));
				uIAnimationControl.SetTexture(0, m_MatBattleUI, AutoUI.AutoRect(new Rect(0f, 207f, 92f, 76f)), AutoUI.AutoSize(new Vector2(92f, 76f)));
				uIAnimationControl.SetTexture(1, m_MatBattleUI, AutoUI.AutoRect(new Rect(92f, 207f, 92f, 76f)), AutoUI.AutoSize(new Vector2(92f, 76f)));
				uIAnimationControl.SetTexture(2, m_MatBattleUI, AutoUI.AutoRect(new Rect(0f, 283f, 92f, 76f)), AutoUI.AutoSize(new Vector2(92f, 76f)));
				uIAnimationControl.SetTexture(3, m_MatBattleUI, AutoUI.AutoRect(new Rect(92f, 283f, 92f, 76f)), AutoUI.AutoSize(new Vector2(92f, 76f)));
				uIAnimationControl.SetTimeInterval(0.1f);
				uIAnimationControl.SetLoopCount(10000000);
				uIAnimationControl.Visible = true;
				uIAnimationControl.Enable = true;
				m_SurvivalModeLeftIndicatorDirection.Add(uIAnimationControl);
				m_SurvivalModeArrowGroup.Add(uIAnimationControl);
			}
		}
	}

	public void SetupSurvivalModeLeaveArrowUI(bool bShow)
	{
		if (m_SurvivalModeArrowGroup != null)
		{
		}
		if (!bShow)
		{
			return;
		}
		if (m_SurvivalModeArrowGroup == null)
		{
			m_SurvivalModeArrowGroup = new uiGroup(m_UIManager);
		}
		if (m_SurvivalModeExitDoors == null || m_SurvivalModeExitDoors.Length <= 0)
		{
			return;
		}
		for (int i = 0; i < m_SurvivalModeExitDoors.Length; i++)
		{
			Vector3 direction = m_SurvivalModeExitDoors[i].transform.position - player.GetTransform().position;
			if (direction.magnitude > 10f)
			{
				Vector3 normalized = player.GetRespawnTransform().InverseTransformDirection(direction).normalized;
				float num = Mathf.Atan2(normalized.z, normalized.x);
				Vector2 vector = new Vector2((float)(Screen.width / 2) + 500f * Mathf.Cos(num), (float)(Screen.height / 2) + 500f * Mathf.Sin(num));
				float num2 = vector.x;
				if (num2 < 140f)
				{
					num2 = 140f;
				}
				if (num2 > (float)(Screen.width - 140))
				{
					num2 = Screen.width - 140;
				}
				float num3 = vector.y;
				if (num3 < 120f)
				{
					num3 = 120f;
				}
				if (num3 > (float)(Screen.height - 100))
				{
					num3 = Screen.height - 100;
				}
				vector = new Vector2(num2, num3);
				if (i >= m_SurvivalModeLeftIndicatorDirection.Count)
				{
					UIAnimationControl uIAnimationControl = new UIAnimationControl();
					uIAnimationControl.Id = 0;
					uIAnimationControl.SetAnimationsPageCount(4);
					uIAnimationControl.Rect = AutoUI.AutoRect(new Rect(0f, 0f, 92f, 76f));
					uIAnimationControl.SetTexture(0, m_MatBattleUI, AutoUI.AutoRect(new Rect(0f, 207f, 92f, 76f)), AutoUI.AutoSize(new Vector2(92f, 76f)));
					uIAnimationControl.SetTexture(1, m_MatBattleUI, AutoUI.AutoRect(new Rect(92f, 207f, 92f, 76f)), AutoUI.AutoSize(new Vector2(92f, 76f)));
					uIAnimationControl.SetTexture(2, m_MatBattleUI, AutoUI.AutoRect(new Rect(0f, 283f, 92f, 76f)), AutoUI.AutoSize(new Vector2(92f, 76f)));
					uIAnimationControl.SetTexture(3, m_MatBattleUI, AutoUI.AutoRect(new Rect(92f, 283f, 92f, 76f)), AutoUI.AutoSize(new Vector2(92f, 76f)));
					uIAnimationControl.SetTimeInterval(0.1f);
					uIAnimationControl.SetLoopCount(10000000);
					uIAnimationControl.Visible = true;
					uIAnimationControl.Enable = true;
					m_SurvivalModeLeftIndicatorDirection.Add(uIAnimationControl);
				}
				m_SurvivalModeLeftIndicatorDirection[i].Visible = true;
				m_SurvivalModeLeftIndicatorDirection[i].Enable = true;
				m_SurvivalModeLeftIndicatorDirection[i].Rect = AutoUI.AutoRect(new Rect(vector.x, vector.y, 92f, 76f));
				m_SurvivalModeLeftIndicatorDirection[i].SetRotation(num);
			}
			else if (i < m_SurvivalModeLeftIndicatorDirection.Count && m_SurvivalModeLeftIndicatorDirection[i] != null)
			{
				m_SurvivalModeLeftIndicatorDirection[i].Visible = false;
				m_SurvivalModeLeftIndicatorDirection[i].Enable = false;
			}
		}
	}

	public void SetupLoadingToExchangeUI()
	{
		m_GameLoadingUI = new GameLoadingUI();
		m_GameLoadingUI.SetupLoadingUI(true, m_UIManager, m_MatCommonBg);
	}

	public void SetupLoadingToNextSurvivalUI()
	{
		m_GameLoadingSurvivalUI = new GameLoadingUI();
		m_GameLoadingSurvivalUI.SetupLoadingUI(true, m_UIManager, m_MatCommonBg);
		SetupControlCoverUI(true);
	}

	public Rect GetWaveShowFontTexRect(int num)
	{
		switch (num)
		{
		case 0:
			return new Rect(0f, 710f, 46f, 49f);
		case 1:
			return new Rect(69f, 710f, 34f, 49f);
		case 2:
			return new Rect(122f, 710f, 50f, 49f);
		case 3:
			return new Rect(185f, 710f, 49f, 49f);
		case 4:
			return new Rect(245f, 710f, 50f, 49f);
		case 5:
			return new Rect(307f, 710f, 50f, 49f);
		case 6:
			return new Rect(372f, 710f, 50f, 49f);
		case 7:
			return new Rect(436f, 710f, 49f, 49f);
		case 8:
			return new Rect(496f, 710f, 48f, 49f);
		case 9:
			return new Rect(557f, 710f, 50f, 49f);
		default:
			return default(Rect);
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
