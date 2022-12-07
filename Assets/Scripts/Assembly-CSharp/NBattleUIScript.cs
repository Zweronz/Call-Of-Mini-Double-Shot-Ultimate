using System;
using System.Collections;
using System.Collections.Generic;
using TNetSdk;
using UnityEngine;
using Zombie3D;

public class NBattleUIScript : MonoBehaviour, UIHandler
{
	public class NPlayersUI
	{
		public uiGroup group;

		public Player player;

		public UIProgressBarRounded proBar;

		public float lastHpPercent = 1f;
	}

	public enum Controls
	{
		kIDGamePause = 3000,
		kIDMoveJoystickBg = 3001,
		kIDMoveJoystickBtn = 3002,
		kIDShootJoystickBg = 3003,
		kIDShootJoystickBtn = 3004,
		kIDSwapWeaponBtn = 3005,
		kIDBattleItem = 3006,
		kIDRequite = 3007,
		kIDGameResume = 3008,
		kIDGameSurrender = 3009,
		kIDGameMusicSwitch = 3010,
		kIDGameMusicSwitchImg = 3011,
		kIDGameSFXSwitch = 3012,
		kIDGameSFXSwitchImg = 3013,
		kIDGameSurrenderYes = 3014,
		kIDGameSurrenderNo = 3015,
		kIDBattleSpeedUp = 3016,
		kIDBattleItemResume = 3017,
		kIDBattleItemDetailBuy = 3018,
		kIDBattleItemDetailBack = 3019,
		kIDNotEnoughMoneyOK = 3020,
		kIDPlayerDontResurrectionYes = 3021,
		kIDPlayerResurrectionYes = 3022,
		kIDPlayerResurrectionNo = 3023,
		kIDGameStartTap = 3024,
		kIDCameraModeDialogOK = 3025,
		kIDCameraModeTypeOne = 3026,
		kIDCameraModeTypeTwo = 3027,
		kIDSureCameraMode = 3028,
		kIDGameBeginAnim = 3029,
		kIDPlayNextWave = 3030,
		kIDPlayNextWaveText = 3031,
		kIDBattleItemUseErrorOK = 3032,
		kIDMap3RewardOK = 3033,
		kIDBattleItemBegin = 3034,
		kIDBattleItemLast = 3054,
		kIDFloorBalanceBegin = 3055,
		kIDFloorBalanceLast = 3075,
		kIDCountDownText = 3076,
		kIDFloorbalanceItemIDBegin = 3077,
		kIDFloorbalanceItemIDLast = 3097,
		kIDRescue = 3098,
		kIDLast = 3099
	}

	private UIManager m_UIManager;

	private string m_ui_material_path;

	protected Material m_MatBattleUI;

	protected Material m_MatDialog01;

	protected Material m_MatGamtStartEffect;

	protected Material m_MatPerfectWaveEffect;

	protected Material m_MatBattleDialog;

	protected Material m_MatAvatarIcons;

	public uiGroup m_DesktopGroup;

	public uiGroup m_NFastkillMsgInfoGroup;

	public UIGroupControl m_NPaushGroup;

	public bool m_bNPaushGroupIsInUIManager;

	public UIGroupControl m_ItemBarGroup;

	public bool m_bItemBarGroupIsInUIManager;

	public uiGroup m_ControlCoverBarGroup;

	public uiGroup m_EnemiesDirectionGroup;

	public uiGroup m_EnemiesLeftInfoGroup;

	public uiGroup m_SurvivalModeArrowGroup;

	public uiGroup m_uiHintDialog;

	public uiGroup m_uiHintDialog_Money;

	public uiGroup m_uiMap3WinRewardDialog;

	public UIGroupControl m_DeadShowDialog;

	public uiGroup m_uiCameraGroup;

	public uiGroup m_uiCameraDialogGroup;

	public uiGroup m_NBattleMsgGroup;

	public UIGroupControl m_NBattleShopItemDetailGroup;

	public bool m_bNBattleShopItemDetailGroupIsInManager;

	public UIGroupControl m_NBattleMsgAllGroup;

	public bool m_bNBattleMsgAllGroupIsInManager;

	public uiGroup m_NBattleDeathUIGroup;

	public uiGroup m_IphoneFrame;

	public uiGroup m_ShowFloorBalanceUIGroup;

	private UIText m_FloorBalanceCountDownText;

	private int m_iFloorBalanceCountDownTimer = -1;

	private int m_iFloorBalanceSelectIndex = -1;

	private int m_iFloorBalanceMaxCount = 5;

	private List<KeyValuePair<int, int>> m_lsFloorBalanceGift = new List<KeyValuePair<int, int>>();

	private UIText m_RescueCountText;

	public UIClickButton m_NQuiteBtn;

	private Dictionary<int, NPlayersUI> m_NPlayersMsgUI = new Dictionary<int, NPlayersUI>();

	private UIImage m_FriendDirImg;

	private UIImage m_MoveJoystickBg;

	private UIJoystickButtonEx m_MoveJoystickBtn;

	private UIImage m_ShootJoystickBg;

	private UIJoystickButtonEx m_ShootJoystickBtn;

	private UIProgressBarRounded playerHpProgressBar;

	private UIProgressBarRounded playerStaminaProgressBar;

	private UIText m_LevelCountDownTimer;

	private UIText m_KillMessage_Killer;

	private UIText m_KillMessage_Defunct;

	private UIText m_RespawnText;

	private int m_iRespawnTime = -1;

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

	private UIEffect01 m_Effect03;

	private List<UIImage> m_EnemiesDirection;

	private List<UIImage> m_FriendsDirection;

	public GameLoadingUI m_GameLoadingUI;

	public GameLoadingUI m_GameLoadingWaitingPlayerUI;

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

	private Rect[] m_rectCountDownNumber = new Rect[5]
	{
		new Rect(934f, 648f, 90f, 110f),
		new Rect(844f, 648f, 90f, 110f),
		new Rect(751f, 659f, 90f, 110f),
		new Rect(453f, 283f, 90f, 110f),
		new Rect(453f, 283f, 90f, 110f)
	};

	private Vector2 m_vc2PlayerMsgFirst = new Vector2(20f, 525f);

	private int m_iPlaerMsgDiscrepancyHeight = 65;

	private int m_ShopItemIndex;

	private uiGroup m_LostConnectGroup;

	private UIAnimationControl m_LoseConnectingAnim;

	public bool GetPaused()
	{
		return m_bPaused;
	}

	public void IsCoundDownOver(float seconde)
	{
		string empty = string.Empty;
		empty = ((seconde != 0f) ? UtilsEx.TimeToStr_HMS((long)seconde) : UtilsEx.TimeToStr_HMS(0L));
		if (m_LevelCountDownTimer != null)
		{
			m_LevelCountDownTimer.SetText(empty);
			m_LevelCountDownTimer = null;
		}
	}

	private void Awake()
	{
		OpenClickPlugin.Hide();
		m_UIManager = base.gameObject.AddComponent<UIManager>() as UIManager;
		m_UIManager.SetViewPortInCenter(false);
		m_UIManager.SetParameter(8, 3, false);
		m_UIManager.SetUIHandler(this);
		SetupLoadingToWaittingBeginGameUI();
	}

	private IEnumerator Start()
	{
		yield return 0;
		gameState = GameApp.GetInstance().GetGameState();
		gameScene = GameApp.GetInstance().GetGameScene();
		player = gameScene.GetPlayer();
		m_MatBattleUI = LoadUIMaterial("Zombie3D/UI/Materials/NBattleUI");
		m_MatDialog01 = null;
		m_MatPerfectWaveEffect = LoadUIMaterial("Zombie3D/UI/Materials/NPerfectWaveEffectUI");
		m_MatAvatarIcons = LoadUIMaterial("Zombie3D/UI/Materials/NAvatarIcons");
		Resources.UnloadUnusedAssets();
		SceneUIManager.Instance().SetMusicPlayerState(SceneUIManager.MusicState_BattleAudioState);
		uiInited = true;
		m_bPaused = false;
		m_bCurrentSpeedUpState = player.SpeedUpByStamina;
		m_lastHp = player.GetHp();
		m_lastStamina = player.GetStamina();
		m_rcActiveSkillBtnTex = new Rect[3]
		{
			new Rect(0f, 206f, 96f, 63f),
			new Rect(98f, 206f, 96f, 63f),
			new Rect(0f, 206f, 96f, 63f)
		};
		enSkillType p_CurSkillType = enSkillType.FastRun;
		switch ((gameState.m_eGameMode.m_ePlayMode != 0) ? enSkillType.FastRun : gameState.m_CurSkillType)
		{
		case enSkillType.FastRun:
			m_rcActiveSkillBtnTex = new Rect[3]
			{
				new Rect(0f, 206f, 96f, 63f),
				new Rect(98f, 206f, 96f, 63f),
				new Rect(0f, 206f, 96f, 63f)
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
		for (int i = 0; i < 10; i++)
		{
			UIImage img = UIUtils.BuildImage(0, new Rect(0f, 0f, 38f, 35f), m_MatBattleUI, new Rect(375f, 54f, 38f, 35f), new Vector2(38f, 35f));
			m_EnemiesDirection.Add(img);
		}
		m_FriendsDirection = new List<UIImage>();
		for (int j = 0; j < 10; j++)
		{
			UIImage img2 = UIUtils.BuildImage(0, new Rect(0f, 0f, 26f, 35f), m_MatBattleUI, new Rect(416f, 54f, 26f, 35f), new Vector2(26f, 35f));
			m_FriendsDirection.Add(img2);
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
		if (gameState.m_bIsSurvivalMode)
		{
			int emenyLiveCount = 0;
			foreach (Enemy enemy in gameScene.GetEnemies().Values)
			{
				if (enemy != null && enemy.HP > 0f)
				{
					emenyLiveCount++;
				}
			}
			if (emenyLiveCount == 0)
			{
				ShowSurvivalModeIndicatorUI();
			}
		}
		((TopWatchingCameraScript)gameScene.GetCamera()).SetCameraLookType(1, false);
	}

	private void Update()
	{
		if (m_bPaused)
		{
			return;
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
				if (m_LostConnectGroup != null)
				{
					TimeManager.Instance.DestroyCalculagraph(9);
					SetupLoseConnectUI(false);
				}
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
			if (gameState.m_eGameMode.m_eCooperaMode == GameState.NetworkGameMode.NetworkCooperationMode.E_Team && m_GameLoadingWaitingPlayerUI == null)
			{
				foreach (KeyValuePair<int, Player> recipient in PlayerManager.Instance.GetRecipientList())
				{
					if (recipient.Value.m_iNGroupID == PlayerManager.Instance.GetPlayerClass().m_iNGroupID)
					{
						UpdatePlayerMsg(recipient.Key);
					}
				}
			}
			if (m_fBeginGameCountDownTimer >= 0f)
			{
				m_fBeginGameCountDownTimer += num;
				uint num2 = (uint)(m_fBeginGameCountDownTime - m_fBeginGameCountDownTimer);
				if (num2 != m_uiCountDownNum)
				{
					m_uiCountDownNum = num2;
					if (m_BeginGameCountDownImg != null)
					{
						m_BeginGameCountDownImg.SetTexture(m_MatBattleUI, AutoUI.AutoRect(m_rectCountDownNumber[m_uiCountDownNum]));
					}
				}
			}
			if (m_fBeginGameCountDownTimer >= m_fBeginGameCountDownTime)
			{
				BeginGame();
				m_fBeginGameCountDownTimer = -1f;
			}
			if (m_GameLoadingWaitingPlayerUI != null)
			{
				return;
			}
			if (Time.time - m_LastIndicatorUpdateTime > 0.05f && m_GameLoadingWaitingPlayerUI == null)
			{
				m_LastIndicatorUpdateTime = Time.time;
				SetupEnemiesDirectionUI(true);
				Vector3 position = player.GetTransform().position;
				if (gameScene.GetFriendPlayer() != null)
				{
					Vector3 position2 = gameScene.GetFriendPlayer().GetTransform().position;
					if (Vector2.Distance(new Vector2(position.x, position.z), new Vector2(position2.x, position2.z)) > 10f)
					{
						Vector3 normalized = player.GetRespawnTransform().InverseTransformDirection(position2 - position).normalized;
						float num3 = Mathf.Atan2(normalized.z, normalized.x);
						Vector2 vector = new Vector2(480f + 500f * Mathf.Cos(num3), 320f + 500f * Mathf.Sin(num3));
						float num4 = vector.x;
						if (num4 < 140f)
						{
							num4 = 140f;
						}
						if (num4 > 820f)
						{
							num4 = 820f;
						}
						float num5 = vector.y;
						if (num5 < 120f)
						{
							num5 = 120f;
						}
						if (num5 > 540f)
						{
							num5 = 540f;
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
				if (gameState.m_eGameMode.m_ePlayMode == GameState.NetworkGameMode.PlayMode.E_Console)
				{
					m_GameBeginAnimStartTime = Time.time;
					SetupGameBeginEffect();
				}
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
				m_DesktopGroup.Remove(3029);
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
			if (m_Effect03 != null)
			{
				m_Effect03.Update(Time.deltaTime);
				if (m_Effect03.EffectOver())
				{
					m_Effect03 = null;
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
			SyncCountDownText();
		}
	}

	private void LateUpdate()
	{
	}

	public void BeginGameTimer()
	{
		m_fBeginGameCountDownTimer = 0f;
		uint num = (uint)(m_fBeginGameCountDownTime - m_fBeginGameCountDownTimer);
		if (num == m_uiCountDownNum)
		{
			return;
		}
		m_uiCountDownNum = num;
		if (m_BeginGameCountDownImg == null)
		{
			m_BeginGameCountDownImg = UIUtils.BuildImage(0, new Rect(450f, 361f, 90f, 110f), m_MatBattleUI, m_rectCountDownNumber[num], new Vector2(90f, 110f));
			if (m_DesktopGroup == null)
			{
				m_DesktopGroup = new uiGroup(m_UIManager);
			}
			m_DesktopGroup.Add(m_BeginGameCountDownImg);
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
		if (control.Id == 3024)
		{
			m_DesktopGroup.Remove(3024);
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
			return;
		}
		if (control.Id == 3025)
		{
			SetupCameraTypeDialogUI(false);
			SetupCameraTypeUI(true);
			return;
		}
		if (control.Id == 3026)
		{
			((TopWatchingCameraScript)gameScene.GetCamera()).SetCameraLookType(1, true);
			gameState.m_iCameraModeType = 1;
			if (m_uiCameraGroup != null && !m_uiCameraGroup.GetControl(3028).Enable)
			{
				m_uiCameraGroup.GetControl(3028).Enable = true;
			}
			return;
		}
		if (control.Id == 3027)
		{
			((TopWatchingCameraScript)gameScene.GetCamera()).SetCameraLookType(2, true);
			gameState.m_iCameraModeType = 2;
			if (m_uiCameraGroup != null && !m_uiCameraGroup.GetControl(3028).Enable)
			{
				m_uiCameraGroup.GetControl(3028).Enable = true;
			}
			return;
		}
		if (control.Id == 3028)
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
			return;
		}
		if (control.Id == 3002)
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
			return;
		}
		if (control.Id == 3004)
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
			return;
		}
		if (control.Id == 3000)
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
			return;
		}
		if (control.Id == 3005)
		{
			List<WeaponType> weaponList = player.WeaponList;
			if (weaponList.Count == 1 || weaponList.Count != 2)
			{
				return;
			}
			for (int i = 0; i < 2; i++)
			{
				WeaponType weaponType = weaponList[i];
				if (weaponType != player.GetWeapon().GetWeaponType())
				{
					Weapon w = WeaponFactory.GetInstance().CreateWeapon(weaponType);
					player.ChangeWeapon(w);
					break;
				}
			}
			return;
		}
		if (control.Id == 3006)
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
			return;
		}
		if (control.Id == 3007)
		{
			SetupPaushUI(true);
		}
		if (control.Id == 3008)
		{
			SetupPaushUI(false);
			OpenClickPlugin.Hide();
		}
		else if (control.Id == 3009)
		{
			SetupHintDialog(true, 0, 3014, 3015, "Quitting now will bring you straight to the battle report. Proceed?", 2);
		}
		else if (control.Id == 3010)
		{
			GameApp.GetInstance().GetGameState().MusicOn = !GameApp.GetInstance().GetGameState().MusicOn;
			SetupPaushUI(false);
		}
		else if (control.Id == 3012)
		{
			GameApp.GetInstance().GetGameState().SoundOn = !GameApp.GetInstance().GetGameState().SoundOn;
			SetupPaushUI(false);
		}
		else if (control.Id == 3014)
		{
			SetupHintDialog(false, 0, 0, 0, string.Empty);
			GameSetup.Instance.DisConnect();
		}
		else if (control.Id == 3015)
		{
			SetupHintDialog(false, 0, 0, 0, string.Empty);
		}
		else if (control.Id == 3016)
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
				if (GameSetup.Instance.m_oncePVPCombatDR != null && !GameSetup.Instance.m_oncePVPCombatDR.m_UseFastRun)
				{
					GameSetup.Instance.m_oncePVPCombatDR.m_UseFastRun = true;
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
		else if (control.Id >= 3034 && control.Id <= 3054)
		{
			m_ShopItemIndex = control.Id - 3034;
			NBattleShopItemImpl nBattleItemImpl = GameApp.GetInstance().GetGameScene().GetPlayer()
				.GetNBattleItemImpl((enBattlefieldProps)m_ShopItemIndex);
			Debug.LogWarning(string.Concat("BattleItem_", nBattleItemImpl.GetItem().BattlefieldProps, "|", nBattleItemImpl.NumberOfUse));
			if (nBattleItemImpl.NumberOfUse < 0)
			{
				SetupBattleItemUI(false);
				SetupShopItemDetailUI(true);
			}
		}
		else
		{
			if (control.Id == 3098)
			{
				if (!(PlayerManager.Instance.GetPlayerClass().HP > 0f) || gameState.GetPowerUPS()[12] == null || (int)gameState.GetPowerUPS()[12] <= 0)
				{
					return;
				}
				{
					foreach (KeyValuePair<int, Player> recipient in PlayerManager.Instance.GetRecipientList())
					{
						if (recipient.Value.HP <= 0f)
						{
							GameSetup.Instance.SendLock(recipient.Key);
						}
					}
					return;
				}
			}
			if (control.Id == 3017)
			{
				SetupBattleItemUI(false);
			}
			else if (control.Id == 3019)
			{
				SetupShopItemDetailUI(false);
				SetupBattleItemUI(true);
			}
			else if (control.Id == 3018)
			{
				enBattlefieldProps shopItemIndex = (enBattlefieldProps)m_ShopItemIndex;
				Player playerClass = PlayerManager.Instance.GetPlayerClass();
				NBattleShopItemImpl nBattleItemImpl2 = playerClass.GetNBattleItemImpl(shopItemIndex);
				if (nBattleItemImpl2.CanBuy())
				{
					switch (shopItemIndex)
					{
					case enBattlefieldProps.E_QuickRevive:
						((ItemQuickRevive)nBattleItemImpl2).Buy();
						((ItemQuickRevive)nBattleItemImpl2).Do();
						break;
					case enBattlefieldProps.E_BestRunner:
						((ItemBestRunner)nBattleItemImpl2).Buy();
						((ItemBestRunner)nBattleItemImpl2).Do();
						break;
					case enBattlefieldProps.E_Tenacity:
						((ItemTenacity)nBattleItemImpl2).Buy();
						((ItemTenacity)nBattleItemImpl2).Do();
						break;
					case enBattlefieldProps.E_AnaestheticProjectile:
						((ItemAnaestheticProjectile)nBattleItemImpl2).Buy();
						((ItemAnaestheticProjectile)nBattleItemImpl2).Do();
						break;
					case enBattlefieldProps.E_StrongWeapon:
						((ItemStrongWeapon)nBattleItemImpl2).Buy();
						((ItemStrongWeapon)nBattleItemImpl2).Do();
						break;
					}
					SetupShopItemDetailUI(false);
					gameState.BuyNbattleBuff(nBattleItemImpl2.GetItem().BattlefieldProps);
				}
				else
				{
					SetupDonnotHaveEnoughMoneyDialog(true, "Insufficient Money!");
				}
			}
			else if (control.Id == 3020)
			{
				SetupDonnotHaveEnoughMoneyDialog(false, string.Empty);
			}
			else if (control.Id == 3021)
			{
				if (Time.timeScale < 1f)
				{
					Time.timeScale = 1f;
				}
				SetupPlayerDeadUI(false);
				SetupLoadingToExchangeUI();
				gameScene.BattleEnd();
			}
			else if (control.Id == 3022)
			{
				if (Time.timeScale < 1f)
				{
					Time.timeScale = 1f;
				}
				SetupPlayerDeadUI(false);
				((TopWatchingCameraScript)gameScene.GetCamera()).ShowPlayerReliveEffect();
				Hashtable powerUPS = gameState.GetPowerUPS();
				foreach (int key in powerUPS.Keys)
				{
					if (key == 11)
					{
						powerUPS[key] = (int)powerUPS[key] - 1;
						if (gameState.IsGCArchievementLocked(6))
						{
							gameState.UnlockGCArchievement(6, "com.trinitigame.callofminibulletdudes.a7");
						}
						GameApp.GetInstance().Save();
						break;
					}
				}
				GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load("Zombie3D/PlayerBuff/relive"), player.GetTransform().position, Quaternion.identity) as GameObject;
				RemoveTimerScript removeTimerScript = gameObject.AddComponent(typeof(RemoveTimerScript)) as RemoveTimerScript;
				removeTimerScript.life = 3f;
				if (gameScene.GetFriendPlayer() != null && gameScene.GetFriendPlayer().HP <= 0f)
				{
					GameObject gameObject2 = UnityEngine.Object.Instantiate(Resources.Load("Zombie3D/PlayerBuff/relive"), gameScene.GetFriendPlayer().GetTransform().position, Quaternion.identity) as GameObject;
					RemoveTimerScript removeTimerScript2 = gameObject2.AddComponent(typeof(RemoveTimerScript)) as RemoveTimerScript;
					removeTimerScript2.life = 3f;
				}
				Animation[] componentsInChildren = gameObject.GetComponentsInChildren<Animation>();
				for (int j = 0; j < componentsInChildren.Length; j++)
				{
					componentsInChildren[j].Play(componentsInChildren[j].clip.name);
				}
				SetupControlCoverUI(true);
				m_bResurrectionPlayer = true;
				m_ResurrectionAnimStartTime = Time.time;
			}
			else if (control.Id == 3023)
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
			else if (control.Id == 3032)
			{
				SetupHintDialog(false, 0, 0, 0, string.Empty);
			}
			else if (control.Id == 3033)
			{
				SetupMap3WinRewardUI(false, null);
				SetupControlCoverUI(false);
				SetupLoadingToExchangeUI();
				gameScene.BattleEnd();
			}
			else if (control.Id >= 3077 && control.Id <= 3097)
			{
				ChooseAFloorbalanceGift(control.Id);
			}
			else if (control.Id != 3099)
			{
			}
		}
	}

	public void NBattleMsgTimeOut()
	{
		SetupNBattleMsg(false, -1);
		m_RespawnText = null;
		m_iRespawnTime = -1;
		Resources.UnloadUnusedAssets();
	}

	public void UpdateNRespawnTimer(float time)
	{
		if (m_RespawnText != null)
		{
			int num = (int)time;
			if (m_iRespawnTime != num)
			{
				m_RespawnText.SetText("RESPAWN IN:  " + num);
				m_iRespawnTime = num;
			}
		}
	}

	public void SetupNBattleMsg(bool bShow, int headImgId)
	{
		if (m_NBattleMsgGroup != null)
		{
			m_NBattleMsgGroup.Clear();
			m_NBattleMsgGroup = null;
		}
		if (bShow)
		{
			m_NBattleMsgGroup = new uiGroup(m_UIManager);
			UIImage control = UIUtils.BuildImage(0, new Rect(315f, 185f, 312f, 52f), m_MatBattleUI, new Rect(3f, 284f, 312f, 52f), new Vector2(312f, 52f));
			m_NBattleMsgGroup.Add(control);
			Material mat = LoadUIMaterial("Zombie3D/UI/Materials/AvatarIconsHead");
			Resources.UnloadUnusedAssets();
			Rect avatarIconHeadTexture = ShopUIScript.GetAvatarIconHeadTexture((Avatar.AvatarSuiteType)headImgId);
			control = UIUtils.BuildImage(0, new Rect(320f, 190f, avatarIconHeadTexture.width, avatarIconHeadTexture.height), mat, avatarIconHeadTexture, new Vector2(avatarIconHeadTexture.width, avatarIconHeadTexture.height));
			control.CatchMessage = false;
			m_NBattleMsgGroup.Add(control);
			UIText uIText = UIUtils.BuildUIText(0, new Rect(420f, 190f, 200f, 30f), UIText.enAlignStyle.center);
			uIText.Set("Zombie3D/Font/037-CAI978-22", "GOTCHA!!!!", new Color(0.81960785f, 0.5294118f, 2f / 51f, 1f));
			m_NBattleMsgGroup.Add(uIText);
			if (m_RespawnText == null)
			{
				m_RespawnText = UIUtils.BuildUIText(0, new Rect(300f, 410f, 400f, 30f), UIText.enAlignStyle.center);
				m_RespawnText.Set("Zombie3D/Font/037-CAI978-22", "RESPAWN IN:  " + (GameSetup.Instance.ReviveTime - GameSetup.Instance.ReviveTimer), Color.white);
				m_NBattleMsgGroup.Add(m_RespawnText);
			}
		}
	}

	public void SetupBattleUI(bool bShow)
	{
		if (m_DesktopGroup != null)
		{
			m_DesktopGroup.Clear();
			m_DesktopGroup = null;
		}
		if (!bShow)
		{
			return;
		}
		m_DesktopGroup = new uiGroup(m_UIManager);
		UIImage control = UIUtils.BuildImage(0, new Rect(Screen.width / 2 - 157, Screen.height - 60, 315f, 52f), m_MatBattleUI, new Rect(0f, 0f, 315f, 52f), new Vector2(315f, 52f));
		m_DesktopGroup.Add(control);
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
		List<WeaponType> list = new List<WeaponType>();
		list = ((player.WeaponList.Count <= 0) ? gameState.GetBattleWeapons() : player.WeaponList);
		UIClickButton control2;
		if (list.Count < 2)
		{
			control = UIUtils.BuildImage(0, new Rect(Screen.width - 360, 32f, 96f, 63f), m_MatBattleUI, new Rect(454f, 207f, 96f, 63f), new Vector2(96f, 63f), 1);
			m_DesktopGroup.Add(control);
		}
		else
		{
			control2 = UIUtils.BuildClickButton(3005, new Rect(Screen.width - 360, 32f, 96f, 63f), m_MatBattleUI, new Rect(454f, 81f, 96f, 63f), new Rect(454f, 144f, 96f, 63f), new Rect(454f, 81f, 96f, 63f), new Vector2(96f, 63f), 1);
			m_DesktopGroup.Add(control2);
		}
		control2 = UIUtils.BuildClickButton(3006, new Rect(260f, 32f, 96f, 63f), m_MatBattleUI, new Rect(255f, 214f, 96f, 63f), new Rect(356f, 214f, 96f, 63f), new Rect(255f, 214f, 96f, 63f), new Vector2(96f, 63f));
		m_DesktopGroup.Add(control2);
		if (m_NQuiteBtn != null)
		{
			m_NQuiteBtn = null;
		}
		m_NQuiteBtn = UIUtils.BuildClickButton(3007, new Rect(0f, Screen.height - 75, 98f, 75f), m_MatBattleUI, new Rect(4f, 128f, 98f, 75f), new Rect(99f, 128f, 98f, 75f), new Rect(4f, 128f, 98f, 75f), new Vector2(98f, 75f), 0);
		m_UIManager.Add(m_NQuiteBtn);
		enSkillType enSkillType = enSkillType.FastRun;
		enSkillType = ((gameState.m_eGameMode.m_ePlayMode != 0) ? enSkillType.FastRun : gameState.m_CurSkillType);
		int num = Screen.width - 136;
		switch (enSkillType)
		{
		case enSkillType.FastRun:
			m_SpeedUpButton = UIUtils.BuildPushButton(3016, new Rect(num, 260f, 96f, 63f), m_MatBattleUI, m_rcActiveSkillBtnTex[0], m_rcActiveSkillBtnTex[1], m_rcActiveSkillBtnTex[2], new Vector2(96f, 63f), 1);
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
		if (GameApp.GetInstance().GetGameState().m_eGameMode.m_ePlayMode == GameState.NetworkGameMode.PlayMode.E_DeathMatch)
		{
			m_LevelCountDownTimer = UIUtils.BuildUIText(3076, new Rect(Screen.width / 2 - 200, (float)Screen.height - 95f, 400f, 30f), UIText.enAlignStyle.center);
			string text = UtilsEx.TimeToStr_HMS((long)(GameSetup.Instance.m_fCountDownTime - GameSetup.Instance.m_fCountDownTimer));
			m_LevelCountDownTimer.Set("Zombie3D/Font/037-CAI978-spec1", text, Color.white);
			m_DesktopGroup.Add(m_LevelCountDownTimer);
			GameSetup.Instance.ReqSyncBattleTimer();
		}
		else
		{
			m_LevelCountDownTimer = null;
		}
		if (gameState.m_eGameMode.m_ePlayMode != GameState.NetworkGameMode.PlayMode.E_PVE_BossRush)
		{
			return;
		}
		Material mat = LoadUIMaterial("Zombie3D/UI/Materials/PowerUPSIcons");
		Resources.UnloadUnusedAssets();
		Rect powerUpsIconTexture = ShopUIScript.GetPowerUpsIconTexture(ItemType.Defibrilator);
		control2 = UIUtils.BuildClickButton(3098, new Rect(420f, 32f, powerUpsIconTexture.width, powerUpsIconTexture.height), mat, powerUpsIconTexture, powerUpsIconTexture, powerUpsIconTexture, new Vector2(powerUpsIconTexture.width, powerUpsIconTexture.height));
		m_DesktopGroup.Add(control2);
		if (m_RescueCountText == null)
		{
			m_RescueCountText = UIUtils.BuildUIText(0, new Rect(440f, 32f, 400f, 30f), UIText.enAlignStyle.left);
			int num2 = 0;
			if (gameState.GetPowerUPS().ContainsKey(12))
			{
				num2 = (int)gameState.GetPowerUPS()[12];
			}
			m_RescueCountText.Set("Zombie3D/Font/037-CAI978-22", "  X" + num2, Color.white);
			m_DesktopGroup.Add(m_RescueCountText);
		}
	}

	public void SetupLoseConnectUI(bool bShow)
	{
		if (m_LostConnectGroup != null)
		{
			m_LostConnectGroup.Clear();
			m_LostConnectGroup = null;
		}
		if (bShow)
		{
			m_LostConnectGroup = new uiGroup(m_UIManager);
			Material material = LoadUIMaterial("Zombie3D/UI/Materials/Reconnecting");
			Resources.UnloadUnusedAssets();
			m_LoseConnectingAnim = new UIAnimationControl();
			m_LoseConnectingAnim.Id = 0;
			m_LoseConnectingAnim.SetAnimationsPageCount(4);
			m_LoseConnectingAnim.Rect = AutoUI.AutoRect(new Rect(246f, 261f, 482f, 143f));
			m_LoseConnectingAnim.SetTexture(0, material, AutoUI.AutoRect(new Rect(0f, 0f, 482f, 143f)), AutoUI.AutoSize(new Vector2(482f, 143f)));
			m_LoseConnectingAnim.SetTexture(1, material, AutoUI.AutoRect(new Rect(485f, 1f, 1f, 1f)), AutoUI.AutoSize(new Vector2(482f, 143f)));
			m_LoseConnectingAnim.SetTexture(2, material, AutoUI.AutoRect(new Rect(0f, 0f, 482f, 143f)), AutoUI.AutoSize(new Vector2(482f, 143f)));
			m_LoseConnectingAnim.SetTexture(3, material, AutoUI.AutoRect(new Rect(485f, 1f, 1f, 1f)), AutoUI.AutoSize(new Vector2(482f, 143f)));
			m_LoseConnectingAnim.SetTimeInterval(0.2f);
			m_LoseConnectingAnim.SetLoopCount(1000000);
			m_LoseConnectingAnim.Visible = true;
			m_LoseConnectingAnim.Enable = true;
			m_LostConnectGroup.Add(m_LoseConnectingAnim);
		}
	}

	public void ResetPlayersUIMsg()
	{
		m_NPlayersMsgUI.Clear();
	}

	public void InitPlayerMsg(int playerID, Player _player, string _name)
	{
		if (m_NPlayersMsgUI.ContainsKey(playerID))
		{
			Debug.LogError("Already Owned The PlayerUI");
			return;
		}
		NPlayersUI nPlayersUI = new NPlayersUI();
		nPlayersUI.player = _player;
		nPlayersUI.group = new uiGroup(m_UIManager);
		float percent = (nPlayersUI.lastHpPercent = Mathf.Clamp01(nPlayersUI.player.HP / nPlayersUI.player.GetMaxHp()));
		m_NPlayersMsgUI.Add(playerID, nPlayersUI);
		float num = m_vc2PlayerMsgFirst.y - (float)((m_NPlayersMsgUI.Count - 1) * m_iPlaerMsgDiscrepancyHeight);
		nPlayersUI.proBar = new UIProgressBarRounded();
		nPlayersUI.proBar.Id = 0;
		nPlayersUI.proBar.Rect = AutoUI.AutoRect(new Rect(m_vc2PlayerMsgFirst.x + 40f, num + 1f, 118f, 16f));
		nPlayersUI.proBar.SetParam(m_MatBattleUI, new Rect(1f, 1f, 1f, 1f), m_MatPerfectWaveEffect, new Rect(830f, 798f, 3f, 16f), new Rect(870f, 798f, 1f, 16f), new Rect(943f, 798f, 5f, 16f), percent);
		nPlayersUI.group.Add(nPlayersUI.proBar);
		UIImage control = UIUtils.BuildImage(0, new Rect(m_vc2PlayerMsgFirst.x, num, 178f, 18f), m_MatPerfectWaveEffect, new Rect(823f, 766f, 178f, 18f), new Vector2(178f, 18f), 0);
		nPlayersUI.group.Add(control);
		SFSObject variable = GameSetup.Instance.GetUserByID(playerID).GetVariable(TNetUserVarType.E_PlayerInfo);
		int @int = variable.GetInt("AvatarHeadID");
		Avatar.AvatarSuiteType avatar_suite_type = (Avatar.AvatarSuiteType)@int;
		Rect avatarIconTexture = ShopUIScript.GetAvatarIconTexture(avatar_suite_type, Avatar.AvatarType.Head);
		control = UIUtils.BuildImage(rcMat: new Rect(avatarIconTexture.x / 2f, avatarIconTexture.y / 2f, avatarIconTexture.width / 2f, avatarIconTexture.height / 2f), id: 0, scrRect: new Rect(m_vc2PlayerMsgFirst.x - 10f, num, 45f, 40f), mat: m_MatAvatarIcons, rect_size: new Vector2(45f, 40f), bNeedShiftToRight: 0);
		control.CatchMessage = false;
		nPlayersUI.group.Add(control);
		UIText uIText = null;
		uIText = UIUtils.BuildUIText(0, new Rect(m_vc2PlayerMsgFirst.x + 40f, num + 10f, 100f, 30f), UIText.enAlignStyle.left, 0);
		uIText.Set("Zombie3D/Font/037-CAI978-15", _name, new Color(0.81960785f, 0.5294118f, 2f / 51f, 1f));
		nPlayersUI.group.Add(uIText);
	}

	public void UpdatePlayerMsg(int playerID)
	{
		if (m_NPlayersMsgUI.ContainsKey(playerID))
		{
			NPlayersUI nPlayersUI = m_NPlayersMsgUI[playerID];
			float num = Mathf.Clamp01(nPlayersUI.player.HP / nPlayersUI.player.GetMaxHp());
			if (!(Mathf.Abs(nPlayersUI.lastHpPercent - num) <= 0.01f))
			{
				nPlayersUI.lastHpPercent = num;
				float num2 = m_vc2PlayerMsgFirst.y + (float)((m_NPlayersMsgUI.Count - 1) * m_iPlaerMsgDiscrepancyHeight);
				nPlayersUI.proBar.SetParam(m_MatBattleUI, new Rect(1f, 1f, 1f, 1f), m_MatPerfectWaveEffect, new Rect(830f, 798f, 3f, 16f), new Rect(870f, 798f, 1f, 16f), new Rect(943f, 798f, 5f, 16f), num);
			}
		}
		else
		{
			InitPlayerMsg(playerID, PlayerManager.Instance.GetRecipient(playerID), gameState.GetNName(GameSetup.Instance.GetUserByID(playerID).Name));
		}
	}

	public void SyncCountDownText()
	{
		if (m_LevelCountDownTimer != null)
		{
			long num = (long)(GameSetup.Instance.m_fCountDownTime - GameSetup.Instance.m_fCountDownTimer);
			if (num > 0)
			{
				string text = UtilsEx.TimeToStr_HMS(num);
				m_LevelCountDownTimer.SetText(text);
			}
			else
			{
				string text2 = UtilsEx.TimeToStr_HMS(0L);
				m_LevelCountDownTimer.SetText(text2);
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

	public void SetupNBattleDeathUI(bool bShow)
	{
		if (m_NBattleDeathUIGroup != null)
		{
			m_NBattleDeathUIGroup.Clear();
			m_NBattleDeathUIGroup = null;
		}
		if (bShow)
		{
			m_NBattleDeathUIGroup = new uiGroup(m_UIManager);
			UIClickButton uIClickButton = null;
			uIClickButton = UIUtils.BuildClickButton(3006, new Rect(260f, 32f, 96f, 63f), m_MatBattleUI, new Rect(255f, 214f, 96f, 63f), new Rect(356f, 214f, 96f, 63f), new Rect(255f, 214f, 96f, 63f), new Vector2(96f, 63f));
			m_NBattleDeathUIGroup.Add(uIClickButton);
		}
	}

	public void SetupShopItemDetailUI(bool bShow)
	{
		if (m_NBattleShopItemDetailGroup != null)
		{
			m_NBattleShopItemDetailGroup.Clear();
			m_NBattleShopItemDetailGroup = null;
		}
		if (!bShow)
		{
			if (m_bNBattleShopItemDetailGroupIsInManager)
			{
				m_UIManager.Remove(m_NBattleShopItemDetailGroup);
				m_bNBattleShopItemDetailGroupIsInManager = false;
			}
		}
		else if (m_NBattleShopItemDetailGroup == null)
		{
			m_NBattleShopItemDetailGroup = new UIGroupControl();
			Material material = LoadUIMaterial("Zombie3D/UI/Materials/ChoosePoints1UI");
			Material mat = LoadUIMaterial("Zombie3D/UI/Materials/ShopUI");
			Resources.UnloadUnusedAssets();
			UIImage uIImage = null;
			UIClickButton uIClickButton = null;
			UIText uIText = null;
			uIImage = UIUtils.BuildImage(0, new Rect(0f, 0f, Screen.width, Screen.height), m_MatBattleUI, new Rect(149f, 589f, 1f, 1f), new Vector2(Screen.width, Screen.height));
			m_NBattleShopItemDetailGroup.Add(uIImage);
			uIImage = UIUtils.BuildImage(0, new Rect(218f, 196f, 530f, 339f), m_MatBattleUI, new Rect(0f, 440f, 530f, 339f), new Vector2(530f, 339f));
			m_NBattleShopItemDetailGroup.Add(uIImage);
			enBattlefieldProps shopItemIndex = (enBattlefieldProps)m_ShopItemIndex;
			Player playerClass = PlayerManager.Instance.GetPlayerClass();
			NBattleShopItemImpl nBattleItemImpl = playerClass.GetNBattleItemImpl(shopItemIndex);
			Rect rcMat = new Rect(0f, 0f, 0f, 0f);
			if (shopItemIndex != enBattlefieldProps.E_StrongWeapon)
			{
				rcMat = GetBuffIconTexture(shopItemIndex);
			}
			else if (nBattleItemImpl != null)
			{
				rcMat = GetWeaponTexture(((ItemStrongWeapon)nBattleItemImpl).GetWeaponType());
			}
			uIImage = UIUtils.BuildImage(0, new Rect(403f, 325f, 141f, 130f), m_MatBattleUI, rcMat, new Vector2(141f, 130f));
			m_NBattleShopItemDetailGroup.Add(uIImage);
			uIText = UIUtils.BuildUIText(0, new Rect(644f, 326f, 100f, 30f), UIText.enAlignStyle.left);
			if (nBattleItemImpl.PriceDollor > 0)
			{
				uIImage = UIUtils.BuildImage(0, new Rect(593f, 322f, 50f, 45f), m_MatPerfectWaveEffect, new Rect(972f, 44f, 50f, 45f), new Vector2(50f, 45f));
				uIText.Set("Zombie3D/Font/037-CAI978-15", nBattleItemImpl.PriceDollor.ToString(), new Color(0.81960785f, 0.5294118f, 2f / 51f, 1f));
			}
			else if (nBattleItemImpl.PriceGold > 0)
			{
				uIImage = UIUtils.BuildImage(0, new Rect(593f, 322f, 50f, 45f), m_MatPerfectWaveEffect, new Rect(971f, 1f, 50f, 45f), new Vector2(50f, 45f));
				uIText.Set("Zombie3D/Font/037-CAI978-15", nBattleItemImpl.PriceGold.ToString(), new Color(0.81960785f, 0.5294118f, 2f / 51f, 1f));
			}
			m_NBattleShopItemDetailGroup.Add(uIImage);
			m_NBattleShopItemDetailGroup.Add(uIText);
			uIText = UIUtils.BuildUIText(0, new Rect(261f, 248f, 450f, 60f), UIText.enAlignStyle.center);
			uIText.Set("Zombie3D/Font/037-CAI978-15", nBattleItemImpl.Introduc, new Color(0.81960785f, 0.5294118f, 2f / 51f, 1f));
			m_NBattleShopItemDetailGroup.Add(uIText);
			uIClickButton = UIUtils.BuildClickButton(3018, new Rect(561f, 129f, 187f, 68f), mat, new Rect(833f, 260f, 191f, 62f), new Rect(833f, 322f, 191f, 62f), new Rect(833f, 260f, 191f, 62f), new Vector2(191f, 62f));
			m_NBattleShopItemDetailGroup.Add(uIClickButton);
			uIClickButton = UIUtils.BuildClickButton(3019, new Rect(184f, 129f, 187f, 68f), m_MatBattleUI, new Rect(0f, 54f, 187f, 68f), new Rect(188f, 54f, 187f, 68f), new Rect(0f, 54f, 187f, 68f), new Vector2(187f, 68f));
			m_NBattleShopItemDetailGroup.Add(uIClickButton);
			m_UIManager.Add(m_NBattleShopItemDetailGroup);
			m_bNBattleShopItemDetailGroupIsInManager = true;
		}
		else
		{
			m_UIManager.Remove(m_NBattleShopItemDetailGroup);
			m_UIManager.Add(m_NBattleShopItemDetailGroup);
			m_bNBattleShopItemDetailGroupIsInManager = true;
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
			if (m_bItemBarGroupIsInUIManager)
			{
				m_UIManager.Remove(m_ItemBarGroup);
				m_bItemBarGroupIsInUIManager = false;
			}
			return;
		}
		OpenClickPlugin.Show(false);
		if (m_ItemBarGroup == null)
		{
			m_ItemBarGroup = new UIGroupControl();
			UIBlock uIBlock = new UIBlock();
			uIBlock.Rect = new Rect(0f, 0f, Screen.width, Screen.height);
			m_ItemBarGroup.Add(uIBlock);
			UIImage control = UIUtils.BuildImage(0, new Rect(0f, 0f, Screen.width, Screen.height), m_MatBattleUI, new Rect(321f, 28f, 1f, 1f), new Vector2(Screen.width, Screen.height));
			m_ItemBarGroup.Add(control);
			control = UIUtils.BuildImage(0, new Rect((float)Screen.width / 2f - 473f, (float)Screen.height / 2f - 121f, 946f, 241f), m_MatBattleUI, new Rect(78f, 783f, 946f, 241f), new Vector2(946f, 241f));
			m_ItemBarGroup.Add(control);
			SetupNBattleShopPageView(control.Rect);
			UIClickButton control2 = UIUtils.BuildClickButton(3017, new Rect((float)Screen.width / 2f - 93.5f, (float)Screen.height / 2f - 121f - 34f, 187f, 68f), m_MatBattleUI, new Rect(0f, 54f, 187f, 68f), new Rect(188f, 54f, 187f, 68f), new Rect(0f, 54f, 187f, 68f), new Vector2(187f, 68f));
			m_ItemBarGroup.Add(control2);
			m_UIManager.Add(m_ItemBarGroup);
			m_bItemBarGroupIsInUIManager = true;
		}
		else
		{
			m_UIManager.Remove(m_ItemBarGroup);
			m_UIManager.Add(m_ItemBarGroup);
			m_bItemBarGroupIsInUIManager = true;
		}
	}

	public void SetupNBattleShopPageView(Rect groundRect)
	{
		if (GameApp.GetInstance().GetGameScene().GetPlayer()
			.GetNBattleItemList()
			.Count < 1)
		{
			return;
		}
		if (m_PowerUpsView != null)
		{
			m_ItemBarGroup.Remove(m_PowerUpsView);
		}
		int num = 0;
		foreach (enBattlefieldProps key2 in GameApp.GetInstance().GetGameScene().GetPlayer()
			.GetNBattleItemList()
			.Keys)
		{
			num++;
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
		int num2 = 0;
		UIGroupControl uIGroupControl = UIUtils.BuildUIGroupControl(0, new Rect(0f, 0f, 865f, 166f));
		foreach (KeyValuePair<enBattlefieldProps, NBattleShopItemImpl> nBattleItem in GameApp.GetInstance().GetGameScene().GetPlayer()
			.GetNBattleItemList())
		{
			enBattlefieldProps key = nBattleItem.Key;
			Debug.LogWarning(string.Concat("NBattleUI PageView", key, "|", nBattleItem.Value.NumberOfUse));
			UIImage control = UIUtils.BuildImage(0, new Rect(0 + num2 * 173, 0f, 173f, 166f), m_MatBattleUI, new Rect(551f, 82f, 173f, 166f), new Vector2(173f, 166f));
			uIGroupControl.Add(control);
			float num3 = 90 + num2 * 173;
			float num4 = 75f;
			Rect rcMat = GetBuffIconTexture(key);
			if (key != enBattlefieldProps.E_StrongWeapon)
			{
				rcMat = GetBuffIconTexture(key);
			}
			else
			{
				Player playerClass = PlayerManager.Instance.GetPlayerClass();
				NBattleShopItemImpl nBattleItemImpl = playerClass.GetNBattleItemImpl(key);
				if (nBattleItemImpl != null)
				{
					rcMat = GetWeaponTexture(((ItemStrongWeapon)nBattleItemImpl).GetWeaponType());
				}
			}
			control = UIUtils.BuildImage(0, new Rect(num3 - rcMat.width / 2f, num4 - rcMat.height / 2f, rcMat.width, rcMat.height), m_MatBattleUI, rcMat, new Vector2(rcMat.width, rcMat.height));
			uIGroupControl.Add(control);
			uIClickButton = UIUtils.BuildClickButton((int)(3034 + key), new Rect(0 + num2 * 173, 0f, 173f, 166f), m_MatBattleUI, new Rect(70f, 1f, 173f, 1f), new Rect(724f, 82f, 173f, 166f), new Rect(1f, 1f, 1f, 1f), new Vector2(173f, 166f));
			uIGroupControl.Add(uIClickButton);
			UIText uIText = UIUtils.BuildUIText(0, new Rect(5 + num2 * 173, 125f, 180f, 20f), UIText.enAlignStyle.center);
			uIText.Set("Zombie3D/Font/037-CAI978-15", GetBuffName(key), new Color(0.81960785f, 0.5294118f, 2f / 51f, 1f));
			uIGroupControl.Add(uIText);
			if (key == enBattlefieldProps.E_StrongWeapon)
			{
				if (nBattleItem.Value.NumberOfUse >= 0)
				{
					control = UIUtils.BuildImage(0, new Rect(num3 - 62f, num4 - 46f, 125f, 93f), m_MatPerfectWaveEffect, new Rect(622f, 650f, 125f, 93f), new Vector2(125f, 93f));
					uIGroupControl.Add(control);
				}
			}
			else if (nBattleItem.Value.NumberOfUse >= 0)
			{
				control = UIUtils.BuildImage(0, new Rect(num3 - 62f, num4 - 46f, 151f, 112f), m_MatPerfectWaveEffect, new Rect(797f, 641f, 151f, 112f), new Vector2(151f, 112f));
				uIGroupControl.Add(control);
				if (nBattleItem.Value.NumberOfUse > 0)
				{
					uIText = UIUtils.BuildUIText(0, new Rect(num3 - 50f, num4 - 46f, 180f, 20f), UIText.enAlignStyle.center);
					uIText.Set("Zombie3D/Font/037-CAI978-15", nBattleItem.Value.NumberOfUse.ToString(), new Color(0.81960785f, 0.5294118f, 2f / 51f, 1f));
					uIGroupControl.Add(uIText);
				}
			}
			if (num2 < 4)
			{
				if (num2 >= num - m_PowerUpsView.PageCount * 5 - 1)
				{
					m_PowerUpsView.Add(uIGroupControl);
				}
				num2++;
			}
			else
			{
				num2 = 0;
				m_PowerUpsView.Add(uIGroupControl);
				uIGroupControl = UIUtils.BuildUIGroupControl(0, new Rect(0f, 0f, 865f, 166f));
			}
		}
		if (m_PowerUpsScrollBar != null)
		{
			m_ItemBarGroup.Remove(m_PowerUpsScrollBar);
			m_PowerUpsScrollBar = null;
		}
		int num5 = Mathf.CeilToInt((float)num / 5f);
		m_PowerUpsScrollBar = new UIDotScrollBar();
		m_PowerUpsScrollBar.Rect = AutoUI.AutoRect(new Rect((float)Screen.width / 2f - (float)num5 / 2f * 25f, 200f, 100f, 20f));
		m_PowerUpsScrollBar.ScrollOri = UIDotScrollBar.ScrollOrientation.Horizontal;
		m_PowerUpsScrollBar.DotPageWidth = AutoUI.AutoDistance(25f);
		m_PowerUpsScrollBar.SetPageCount(num5);
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
			uIClickButton = UIUtils.BuildClickButton(3026, new Rect(289f, 149f, 153f, 49f), mat, new Rect(0f, 974f, 153f, 49f), new Rect(0f, 925f, 153f, 49f), new Rect(0f, 974f, 153f, 49f), new Vector2(153f, 49f));
			m_uiCameraGroup.Add(uIClickButton);
			uIClickButton = UIUtils.BuildClickButton(3027, new Rect(520f, 149f, 153f, 49f), mat, new Rect(153f, 974f, 153f, 49f), new Rect(153f, 925f, 153f, 49f), new Rect(153f, 974f, 153f, 49f), new Vector2(153f, 49f));
			m_uiCameraGroup.Add(uIClickButton);
			uIClickButton = UIUtils.BuildClickButton(3028, new Rect(420f, 67f, 153f, 49f), mat, new Rect(775f, 67f, 191f, 62f), new Rect(775f, 3f, 191f, 62f), new Rect(770f, 449f, 191f, 62f), new Vector2(153f, 49f));
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
			if (m_MatDialog01 == null)
			{
				m_MatDialog01 = LoadUIMaterial("Zombie3D/UI/Materials/NDialog01");
				Resources.UnloadUnusedAssets();
			}
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
			uIClickButton = UIUtils.BuildClickButton(3025, rect, material, new Rect(1f, 1f, 1f, 1f), new Rect(770f, 511f, 191f, 62f), new Rect(1f, 1f, 1f, 1f), new Vector2(191f, 62f));
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
		if (m_MatDialog01 == null)
		{
			m_MatDialog01 = LoadUIMaterial("Zombie3D/UI/Materials/NDialog01");
			Resources.UnloadUnusedAssets();
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
		m_DeadShowDialog = UIUtils.BuildUIGroupControl(0, new Rect(223f, 128f, 600f, 380f));
		m_UIManager.Add(m_DeadShowDialog);
		UIBlock uIBlock = new UIBlock();
		uIBlock.Rect = new Rect(0f, 0f, 960f, 640f);
		m_DeadShowDialog.Add(uIBlock);
		if (m_MatBattleDialog == null)
		{
			m_MatBattleDialog = LoadUIMaterial("Zombie3D/UI/Materials/BattleDialog01UI");
		}
		Resources.UnloadUnusedAssets();
		UIImage control = UIUtils.BuildImage(0, new Rect(223f, 128f, 600f, 380f), m_MatBattleDialog, new Rect(0f, 0f, 600f, 380f), new Vector2(600f, 380f));
		m_DeadShowDialog.Add(control);
		UIText uIText = UIUtils.BuildUIText(0, new Rect(570f, 295f, 100f, 40f), UIText.enAlignStyle.left);
		uIText.Set("Zombie3D/Font/037-CAI978-27", "X " + num, Color.white);
		m_DeadShowDialog.Add(uIText);
		uIText = UIUtils.BuildUIText(0, new Rect(300f, 190f, 450f, 90f), UIText.enAlignStyle.left);
		uIText.Set("Zombie3D/Font/037-CAI978-18", "Do you want to use Epinephrine to continue the fight? (Epinephrine can be bought in the shop.)", Constant.TextCommonColor);
		m_DeadShowDialog.Add(uIText);
		UIClickButton uIClickButton = null;
		if (num > 0)
		{
			uIClickButton = UIUtils.BuildClickButton(3022, new Rect(263f, 120f, 192f, 62f), m_MatDialog01, new Rect(640f, 62f, 192f, 62f), new Rect(832f, 62f, 192f, 62f), new Rect(0f, 682f, 192f, 62f), new Vector2(192f, 62f));
			m_DeadShowDialog.Add(uIClickButton);
		}
		else
		{
			control = UIUtils.BuildImage(0, new Rect(263f, 120f, 192f, 62f), m_MatDialog01, new Rect(0f, 682f, 192f, 62f), new Vector2(192f, 62f));
			m_DeadShowDialog.Add(control);
		}
		uIClickButton = UIUtils.BuildClickButton(3023, new Rect(570f, 120f, 192f, 62f), m_MatDialog01, new Rect(640f, 248f, 192f, 62f), new Rect(832f, 248f, 192f, 62f), new Rect(640f, 248f, 192f, 62f), new Vector2(192f, 62f));
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
			UIClickButton control = UIUtils.BuildClickButton(3024, new Rect(-100f, -100f, 2000f, 2000f), m_MatBattleUI, new Rect(1f, 1f, 1f, 1f), new Rect(1f, 1f, 1f, 1f), new Rect(1f, 1f, 1f, 1f), new Vector2(2000f, 2000f));
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
		uIAnimationControl.Id = 3029;
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
			m_MatPerfectWaveEffect = LoadUIMaterial("Zombie3D/UI/Materials/NPerfectWaveEffectUI");
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
			m_MatPerfectWaveEffect = LoadUIMaterial("Zombie3D/UI/Materials/NPerfectWaveEffectUI");
			Resources.UnloadUnusedAssets();
		}
		UIImage control = UIUtils.BuildImage(0, new Rect(0f, 340f, 508f, 55f), m_MatPerfectWaveEffect, new Rect(0f, 650f, 508f, 55f), new Vector2(508f, 55f));
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
		if (m_MoveJoystickBtn != null)
		{
			m_MoveJoystickBtn.Reset();
		}
		((TopWatchingInputController)player.InputController).MoveDirection = Vector3.zero;
		player.StopRun();
		player.SetState(Player.IDLE_STATE);
		if (m_ShootJoystickBtn != null)
		{
			m_ShootJoystickBtn.Reset();
		}
		((TopWatchingInputController)player.InputController).bFire = false;
		player.StopFire();
		SetupStagePassedEffect();
		SetupControlCoverUI(true);
	}

	public void StageLosed()
	{
		m_bStagePassed = true;
		if (m_MoveJoystickBtn != null)
		{
			m_MoveJoystickBtn.Reset();
		}
		((TopWatchingInputController)player.InputController).MoveDirection = Vector3.zero;
		player.StopRun();
		player.SetState(Player.IDLE_STATE);
		if (m_ShootJoystickBtn != null)
		{
			m_ShootJoystickBtn.Reset();
		}
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
			m_MatPerfectWaveEffect = LoadUIMaterial("Zombie3D/UI/Materials/NPerfectWaveEffectUI");
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
			m_MatPerfectWaveEffect = LoadUIMaterial("Zombie3D/UI/Materials/NPerfectWaveEffectUI");
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

	public void SetupBossComeInEffect()
	{
		if (m_Effect03 != null)
		{
			m_Effect03.Clear();
			m_Effect03 = null;
		}
		m_Effect03 = new UIEffect01(m_UIManager);
		UIEffect01.Effect01DataItem effect01DataItem = null;
		effect01DataItem = new UIEffect01.Effect01DataItem();
		effect01DataItem.position = new Vector2(-1000f, 325f);
		effect01DataItem.time = 0.5f;
		m_Effect03.AddData(effect01DataItem);
		effect01DataItem = new UIEffect01.Effect01DataItem();
		effect01DataItem.position = new Vector2(210f, 325f);
		effect01DataItem.time = 1f;
		m_Effect03.AddData(effect01DataItem);
		effect01DataItem = new UIEffect01.Effect01DataItem();
		effect01DataItem.position = new Vector2(210f, 325f);
		effect01DataItem.time = 1f;
		m_Effect03.AddData(effect01DataItem);
		effect01DataItem = new UIEffect01.Effect01DataItem();
		effect01DataItem.position = new Vector2(1000f, 325f);
		effect01DataItem.time = 1f;
		m_Effect03.AddData(effect01DataItem);
		UIGroupControl uIGroupControl = UIUtils.BuildUIGroupControl(0, new Rect(0f, 325f, 500f, 500f));
		if (m_MatPerfectWaveEffect == null)
		{
			m_MatPerfectWaveEffect = LoadUIMaterial("Zombie3D/UI/Materials/NPerfectWaveEffectUI");
			Resources.UnloadUnusedAssets();
		}
		UIImage uIImage = null;
		uIImage = UIUtils.BuildImage(0, new Rect(0f, 350f, 570f, 65f), m_MatPerfectWaveEffect, new Rect(0f, 640f, 570f, 65f), new Vector2(570f, 65f));
		uIGroupControl.Add(uIImage);
		m_Effect03.Group = uIGroupControl;
		m_Effect03.Update(Time.deltaTime);
	}

	public List<Player> GetPlayerInGroup(bool bIsSame)
	{
		List<Player> list = new List<Player>();
		foreach (Player recipientPlayer in PlayerManager.Instance.GetRecipientPlayerList())
		{
			if (bIsSame)
			{
				if (recipientPlayer.m_iNGroupID == player.m_iNGroupID)
				{
					list.Add(recipientPlayer);
				}
			}
			else if (recipientPlayer.m_iNGroupID != player.m_iNGroupID)
			{
				list.Add(recipientPlayer);
			}
		}
		return list;
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
		List<Player> list = PlayerManager.Instance.GetRecipientPlayerList();
		if (GameApp.GetInstance().GetGameState().m_eGameMode.m_eCooperaMode == GameState.NetworkGameMode.NetworkCooperationMode.E_Team)
		{
			list.Clear();
			list = GetPlayerInGroup(false);
		}
		foreach (Player item4 in list)
		{
			if (item4 == null)
			{
				continue;
			}
			Vector3 direction = item4.GetTransform().position - player.GetTransform().position;
			if (direction.magnitude > 10f)
			{
				Vector3 normalized = player.GetRespawnTransform().InverseTransformDirection(direction).normalized;
				float num2 = Mathf.Atan2(normalized.z, normalized.x);
				Vector2 vector = new Vector2(480f + 500f * Mathf.Cos(num2), 320f + 500f * Mathf.Sin(num2));
				float num3 = vector.x;
				if (num3 < 140f)
				{
					num3 = 140f;
				}
				if (num3 > 820f)
				{
					num3 = 820f;
				}
				float num4 = vector.y;
				if (num4 < 120f)
				{
					num4 = 120f;
				}
				if (num4 > 540f)
				{
					num4 = 540f;
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
		if (GameApp.GetInstance().GetGameState().m_eGameMode.m_eCooperaMode == GameState.NetworkGameMode.NetworkCooperationMode.E_Simple)
		{
			return;
		}
		int num5 = 0;
		List<Player> list2 = new List<Player>();
		if (GameApp.GetInstance().GetGameState().m_eGameMode.m_eCooperaMode == GameState.NetworkGameMode.NetworkCooperationMode.E_Team)
		{
			list2 = GetPlayerInGroup(true);
		}
		foreach (Player item5 in list2)
		{
			if (item5 == null)
			{
				continue;
			}
			Vector3 direction2 = item5.GetTransform().position - player.GetTransform().position;
			if (direction2.magnitude > 10f)
			{
				Vector3 normalized2 = player.GetRespawnTransform().InverseTransformDirection(direction2).normalized;
				float num6 = Mathf.Atan2(normalized2.z, normalized2.x);
				Vector2 vector2 = new Vector2(480f + 500f * Mathf.Cos(num6), 320f + 500f * Mathf.Sin(num6));
				float num7 = vector2.x;
				if (num7 < 140f)
				{
					num7 = 140f;
				}
				if (num7 > 820f)
				{
					num7 = 820f;
				}
				float num8 = vector2.y;
				if (num8 < 120f)
				{
					num8 = 120f;
				}
				if (num8 > 540f)
				{
					num8 = 540f;
				}
				vector2 = new Vector2(num7, num8);
				if (num5 >= m_FriendsDirection.Count)
				{
					UIImage item2 = UIUtils.BuildImage(0, new Rect(0f, 0f, 26f, 35f), m_MatBattleUI, new Rect(416f, 54f, 26f, 35f), new Vector2(26f, 35f));
					m_FriendsDirection.Add(item2);
				}
				m_FriendsDirection[num].Rect = AutoUI.AutoRect(new Rect(vector2.x, vector2.y, 38f, 35f));
				m_FriendsDirection[num].SetRotation(num6);
				m_FriendsDirection[num].CatchMessage = false;
				m_EnemiesDirectionGroup.Add(m_FriendsDirection[num]);
				num++;
			}
		}
		if (GameApp.GetInstance().GetGameState().m_eGameMode.m_ePlayMode != GameState.NetworkGameMode.PlayMode.E_PVE_BossRush)
		{
			return;
		}
		int num9 = 0;
		List<Enemy> list3 = new List<Enemy>();
		list3 = NEnemyManager.Instance.GetEnemyList();
		foreach (Enemy item6 in list3)
		{
			if (item6 == null)
			{
				continue;
			}
			Vector3 direction3 = item6.GetTransform().position - player.GetTransform().position;
			if (direction3.magnitude > 10f)
			{
				Vector3 normalized3 = player.GetRespawnTransform().InverseTransformDirection(direction3).normalized;
				float num10 = Mathf.Atan2(normalized3.z, normalized3.x);
				Vector2 vector3 = new Vector2(480f + 500f * Mathf.Cos(num10), 320f + 500f * Mathf.Sin(num10));
				float num11 = vector3.x;
				if (num11 < 140f)
				{
					num11 = 140f;
				}
				if (num11 > 820f)
				{
					num11 = 820f;
				}
				float num12 = vector3.y;
				if (num12 < 120f)
				{
					num12 = 120f;
				}
				if (num12 > 540f)
				{
					num12 = 540f;
				}
				vector3 = new Vector2(num11, num12);
				if (num9 >= m_FriendsDirection.Count)
				{
					UIImage item3 = UIUtils.BuildImage(0, new Rect(0f, 0f, 38f, 35f), m_MatBattleUI, new Rect(375f, 54f, 38f, 35f), new Vector2(38f, 35f));
					m_FriendsDirection.Add(item3);
				}
				m_FriendsDirection[num].Rect = AutoUI.AutoRect(new Rect(vector3.x, vector3.y, 38f, 35f));
				m_FriendsDirection[num].SetRotation(num10);
				m_FriendsDirection[num].CatchMessage = false;
				m_EnemiesDirectionGroup.Add(m_FriendsDirection[num]);
				num++;
			}
		}
	}

	public void SetupHintDialog(bool bShow, int okId, int yesId, int noId, string dialog_content, int textMode = 1)
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
			if (textMode == 2)
			{
				UIText uIText = UIUtils.BuildUIText(0, new Rect(num + 40f, num2 + 40f, 420f, 130f), UIText.enAlignStyle.left);
				uIText.Set("Zombie3D/Font/037-CAI978-18", dialog_content, Constant.TextCommonColor);
				m_uiHintDialog.Add(uIText);
			}
			else
			{
				UIText uIText2 = UIUtils.BuildUIText(0, new Rect(num + 40f, num2 + 40f, 420f, 173f), UIText.enAlignStyle.left);
				uIText2.Set("Zombie3D/Font/Arial12_bold", dialog_content, Color.white);
				m_uiHintDialog.Add(uIText2);
			}
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
			UIImage control = UIUtils.BuildImage(0, new Rect(880f, 578f, 66f, 58f), m_MatBattleUI, new Rect(383f, 98f, 66f, 58f), new Vector2(66f, 58f));
			m_EnemiesLeftInfoGroup.Add(control);
			float value = (float)(gameScene.DDSTrigger.AllEnemiesOfCurWave + gameScene.DDSTrigger.GenExternEnemiesCountOfCurWave - gameScene.CurWaveKilled) / (float)(gameScene.DDSTrigger.AllEnemiesOfCurWave + gameScene.DDSTrigger.GenExternEnemiesCountOfCurWave);
			value = Mathf.Clamp01(value);
			string text = string.Format("{0:0.##}", value * 100f);
			UIText uIText = UIUtils.BuildUIText(0, new Rect(750f, 578f, 200f, 25f), UIText.enAlignStyle.right);
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
			UIImage control = UIUtils.BuildImage(0, new Rect(880f, 578f, 66f, 58f), m_MatBattleUI, new Rect(383f, 98f, 66f, 58f), new Vector2(66f, 58f));
			m_EnemiesLeftInfoGroup.Add(control);
			string text = string.Format(string.Concat(str2: ConfigManager.Instance().GetFixedConfig().GetMaxWavesOfPoints(gameScene.DDSTrigger.MapIndex, gameScene.DDSTrigger.PointsIndex)
				.ToString(), str0: gameScene.DDSTrigger.WaveIndex.ToString(), str1: "/"));
			UIText uIText = UIUtils.BuildUIText(0, new Rect(815f, 578f, 200f, 25f), UIText.enAlignStyle.center);
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
				Vector2 vector = new Vector2(480f + 500f * Mathf.Cos(num), 320f + 500f * Mathf.Sin(num));
				float num2 = vector.x;
				if (num2 < 140f)
				{
					num2 = 140f;
				}
				if (num2 > 820f)
				{
					num2 = 820f;
				}
				float num3 = vector.y;
				if (num3 < 120f)
				{
					num3 = 120f;
				}
				if (num3 > 540f)
				{
					num3 = 540f;
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

	public void SetupDonnotHaveEnoughMoneyDialog(bool bShow, string dialog_content)
	{
		if (m_uiHintDialog_Money != null)
		{
			m_uiHintDialog_Money.Clear();
			m_uiHintDialog_Money = null;
		}
		if (bShow)
		{
			if (m_MatDialog01 == null)
			{
				m_MatDialog01 = LoadUIMaterial("Zombie3D/UI/Materials/NDialog01");
				Resources.UnloadUnusedAssets();
			}
			m_uiHintDialog_Money = new uiGroup(m_UIManager);
			UIImage uIImage = null;
			uIImage = UIUtils.BuildImage(0, new Rect(0f, 0f, 960f, 640f), m_MatDialog01, new Rect(600f, 1f, 1f, 1f), new Vector2(960f, 640f));
			m_uiHintDialog_Money.Add(uIImage);
			float num = 270f;
			float num2 = 200f;
			uIImage = UIUtils.BuildImage(0, new Rect(num, num2, 515f, 301f), m_MatDialog01, new Rect(0f, 378f, 515f, 301f), new Vector2(515f, 301f));
			m_uiHintDialog_Money.Add(uIImage);
			UIText uIText = UIUtils.BuildUIText(0, new Rect(num + 40f, num2 + 40f, 420f, 173f), UIText.enAlignStyle.left);
			uIText.Set("Zombie3D/Font/037-CAI978-18", dialog_content, Constant.TextCommonColor);
			m_uiHintDialog_Money.Add(uIText);
			UIClickButton uIClickButton = null;
			uIClickButton = UIUtils.BuildClickButton(3020, new Rect(num + 280f, num2 - 16f, 191f, 62f), m_MatDialog01, new Rect(640f, 0f, 191f, 62f), new Rect(832f, 0f, 191f, 62f), new Rect(640f, 0f, 191f, 62f), new Vector2(191f, 62f));
			m_uiHintDialog_Money.Add(uIClickButton);
		}
	}

	public void SetStagePassedGotoNextSceneUITimer(float _time)
	{
		if (m_StagePassedGotoNextSceneUITimer < 0f)
		{
			m_StagePassedGotoNextSceneUITimer = _time;
		}
	}

	public void SetupLoadingToExchangeUI()
	{
		Debug.LogWarning("LoadingToExchangeUI");
		if (m_MatPerfectWaveEffect == null)
		{
			m_MatPerfectWaveEffect = LoadUIMaterial("Zombie3D/UI/Materials/NPerfectWaveEffectUI");
		}
		m_GameLoadingUI = new GameLoadingUI();
		m_GameLoadingUI.SetupLoadingUI(true, m_UIManager, m_MatPerfectWaveEffect, true);
		GameSetup.Instance.DebugMsg = "(NBattleUIScript) [Receive SetupLoadingToExchangeUI]";
	}

	public void SetupLoadingToWaittingBeginGameUI()
	{
		if (m_MatPerfectWaveEffect == null)
		{
			m_MatPerfectWaveEffect = LoadUIMaterial("Zombie3D/UI/Materials/NPerfectWaveEffectUI");
		}
		m_GameLoadingWaitingPlayerUI = new GameLoadingUI();
		m_GameLoadingWaitingPlayerUI.SetupLoadingUI(true, m_UIManager, m_MatPerfectWaveEffect, true);
	}

	public void ResetLoadingToWaittingBeginGameUI()
	{
		if (m_GameLoadingWaitingPlayerUI.m_uiGroup != null)
		{
			m_GameLoadingWaitingPlayerUI.m_uiGroup.Clear();
			m_GameLoadingWaitingPlayerUI.m_uiGroup = null;
		}
		m_GameLoadingWaitingPlayerUI = null;
	}

	public void SetupFastKillMsgInfo(bool bShow, int id = 0)
	{
		if (m_NFastkillMsgInfoGroup != null)
		{
			m_NFastkillMsgInfoGroup.Clear();
			m_NFastkillMsgInfoGroup = null;
		}
		m_NFastkillMsgInfoGroup = new uiGroup(m_UIManager);
		if (id != 0)
		{
			Material mat = LoadUIMaterial("Zombie3D/UI/Materials/FastkillShowImgUI");
			Resources.UnloadUnusedAssets();
			UIImage uIImage = null;
			Rect rcMat = new Rect(0f, 0f, 0f, 0f);
			if (id == 2)
			{
				rcMat = new Rect(2f, 2f, 312f, 58f);
			}
			if (id == 4)
			{
				rcMat = new Rect(317f, 2f, 298f, 54f);
			}
			if (id == 3)
			{
				rcMat = new Rect(618f, 2f, 274f, 58f);
			}
			if (id == 5)
			{
				rcMat = new Rect(317f, 59f, 258f, 64f);
			}
			uIImage = UIUtils.BuildImage(0, new Rect(320f, 440f, rcMat.width, rcMat.height), mat, rcMat, new Vector2(rcMat.width, rcMat.height));
			m_NFastkillMsgInfoGroup.Add(uIImage);
		}
	}

	public void SetupLoadingToNextSurvivalUI()
	{
		m_GameLoadingSurvivalUI = new GameLoadingUI();
		m_GameLoadingSurvivalUI.SetupLoadingUI(true, m_UIManager, m_MatPerfectWaveEffect, true);
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

	public void BattleMsgAllTimeOut()
	{
		AddMessage(false, string.Empty, NetWorkMessageInfo.E_NetCMD.E_StruckInformation);
	}

	public void AddMessage(bool bShow, string msg, NetWorkMessageInfo.E_NetCMD type)
	{
		if (m_NBattleMsgAllGroup != null)
		{
			m_NBattleMsgAllGroup.Clear();
			m_NBattleMsgAllGroup = null;
		}
		if (!bShow)
		{
			if (m_bNBattleMsgAllGroupIsInManager)
			{
				m_UIManager.Remove(m_NBattleMsgAllGroup);
				m_bNBattleMsgAllGroupIsInManager = false;
			}
		}
		else if (m_NBattleMsgAllGroup == null)
		{
			m_NBattleMsgAllGroup = new UIGroupControl();
			TimeManager.Instance.Init(8, 3f, BattleMsgAllTimeOut, null, "BattleMsgAll");
			if (type == NetWorkMessageInfo.E_NetCMD.E_StruckInformation)
			{
				if (m_KillMessage_Killer == null && m_KillMessage_Defunct == null)
				{
					m_KillMessage_Killer = UIUtils.BuildUIText(0, new Rect(324f, 145f, 100f, 30f), UIText.enAlignStyle.center);
					m_KillMessage_Killer.Set("Zombie3D/Font/037-CAI978-15", gameState.GetNDeathNameInfo(msg)[0], Color.white);
					m_NBattleMsgAllGroup.Add(m_KillMessage_Killer);
					m_KillMessage_Defunct = UIUtils.BuildUIText(0, new Rect(524f, 145f, 100f, 30f), UIText.enAlignStyle.center);
					m_KillMessage_Defunct.Set("Zombie3D/Font/037-CAI978-15", gameState.GetNDeathNameInfo(msg)[1], Color.white);
					m_NBattleMsgAllGroup.Add(m_KillMessage_Defunct);
					UIImage control = UIUtils.BuildImage(0, new Rect(434f, 132f, 108f, 51f), m_MatBattleUI, new Rect(98f, 368f, 108f, 51f), new Vector2(108f, 51f));
					m_NBattleMsgAllGroup.Add(control);
				}
				else
				{
					m_KillMessage_Killer.SetText(gameState.GetNDeathNameInfo(msg)[0]);
					m_KillMessage_Defunct.SetText(gameState.GetNDeathNameInfo(msg)[1]);
					m_NBattleMsgAllGroup.Add(m_KillMessage_Killer);
					m_NBattleMsgAllGroup.Add(m_KillMessage_Defunct);
					UIImage control2 = UIUtils.BuildImage(0, new Rect(434f, 132f, 108f, 51f), m_MatBattleUI, new Rect(98f, 368f, 108f, 51f), new Vector2(108f, 51f));
					m_NBattleMsgAllGroup.Add(control2);
				}
			}
			m_UIManager.Add(m_NBattleMsgAllGroup);
			m_bNBattleMsgAllGroupIsInManager = true;
		}
		else
		{
			m_UIManager.Remove(m_NBattleMsgAllGroup);
			m_UIManager.Add(m_NBattleMsgAllGroup);
			m_bNBattleMsgAllGroupIsInManager = true;
			Debug.LogWarning("m_bNBattleMsgAllGroupIsInManager = true");
		}
	}

	public void SetupPaushUI(bool bShow)
	{
		if (m_NPaushGroup != null)
		{
			m_NPaushGroup.Clear();
			m_NPaushGroup = null;
		}
		if (!bShow)
		{
			if (m_bNPaushGroupIsInUIManager)
			{
				m_UIManager.Remove(m_NPaushGroup);
				m_bNPaushGroupIsInUIManager = false;
			}
		}
		else if (m_NPaushGroup == null)
		{
			m_NPaushGroup = new UIGroupControl();
			Material mat = LoadUIMaterial("Zombie3D/UI/Materials/GamePauseUI");
			Resources.UnloadUnusedAssets();
			Vector2 rect_size = new Vector2(Screen.width, Screen.height);
			UIImage uIImage = UIUtils.BuildImage(0, new Rect(0f, 0f, Screen.width, Screen.height), mat, new Rect(1f, 1f, 1f, 1f), rect_size);
			uIImage.Rect = new Rect(0f, 0f, rect_size.x, rect_size.y);
			m_NPaushGroup.Add(uIImage);
			UIClickButton control = UIUtils.BuildClickButton(3008, new Rect(1f / 3f * (float)Screen.width, 0.625f * (float)Screen.height, 300f, 76f), mat, new Rect(0f, 0f, 300f, 76f), new Rect(301f, 0f, 300f, 76f), new Rect(0f, 0f, 300f, 76f), new Vector2(300f, 76f));
			m_NPaushGroup.Add(control);
			control = UIUtils.BuildClickButton(3009, new Rect(1f / 3f * (float)Screen.width, 61f / 128f * (float)Screen.height, 300f, 76f), mat, new Rect(0f, 76f, 300f, 76f), new Rect(301f, 76f, 300f, 76f), new Rect(0f, 76f, 300f, 76f), new Vector2(300f, 76f));
			m_NPaushGroup.Add(control);
			if (GameApp.GetInstance().GetGameState().MusicOn)
			{
				control = UIUtils.BuildClickButton(3010, new Rect(1f / 3f * (float)Screen.width, 21f / 64f * (float)Screen.height, 300f, 76f), mat, new Rect(0f, 154f, 300f, 76f), new Rect(301f, 154f, 300f, 76f), new Rect(0f, 154f, 300f, 76f), new Vector2(300f, 76f));
				m_NPaushGroup.Add(control);
			}
			else
			{
				control = UIUtils.BuildClickButton(3010, new Rect(1f / 3f * (float)Screen.width, 21f / 64f * (float)Screen.height, 300f, 76f), mat, new Rect(301f, 154f, 300f, 76f), new Rect(0f, 154f, 300f, 76f), new Rect(301f, 154f, 300f, 76f), new Vector2(300f, 76f));
				m_NPaushGroup.Add(control);
			}
			if (GameApp.GetInstance().GetGameState().SoundOn)
			{
				control = UIUtils.BuildClickButton(3012, new Rect(1f / 3f * (float)Screen.width, 0.178125f * (float)Screen.height, 300f, 76f), mat, new Rect(0f, 230f, 300f, 76f), new Rect(301f, 230f, 300f, 76f), new Rect(0f, 230f, 300f, 76f), new Vector2(300f, 76f));
				m_NPaushGroup.Add(control);
			}
			else
			{
				control = UIUtils.BuildClickButton(3012, new Rect(1f / 3f * (float)Screen.width, 0.178125f * (float)Screen.height, 300f, 76f), mat, new Rect(301f, 230f, 300f, 76f), new Rect(0f, 230f, 300f, 76f), new Rect(301f, 230f, 300f, 76f), new Vector2(300f, 76f));
				m_NPaushGroup.Add(control);
			}
			m_UIManager.Add(m_NPaushGroup);
			m_bNPaushGroupIsInUIManager = true;
		}
		else
		{
			m_UIManager.Remove(m_NPaushGroup);
			m_UIManager.Add(m_NPaushGroup);
			m_bNPaushGroupIsInUIManager = true;
		}
	}

	public void SetupFloorbalance(bool bShow, float timer = -1f, int level = -1)
	{
		if (m_ShowFloorBalanceUIGroup != null)
		{
			m_ShowFloorBalanceUIGroup.Clear();
			m_ShowFloorBalanceUIGroup = null;
		}
		if (bShow)
		{
			if (m_MatDialog01 == null)
			{
				m_MatDialog01 = LoadUIMaterial("Zombie3D/UI/Materials/NDialog01");
				Resources.UnloadUnusedAssets();
			}
			m_ShowFloorBalanceUIGroup = new uiGroup(m_UIManager);
			Material mat = LoadUIMaterial("Zombie3D/UI/Materials/NMsgBox");
			Resources.UnloadUnusedAssets();
			UIImage control = UIUtils.BuildImage(0, new Rect(99f, 57f, 772f, 450f), mat, new Rect(2f, 2f, 772f, 450f), new Vector2(772f, 450f));
			m_ShowFloorBalanceUIGroup.Add(control);
			UIText uIText = UIUtils.BuildUIText(0, new Rect(284f, 455f, 400f, 30f), UIText.enAlignStyle.center);
			uIText.Set("Zombie3D/Font/037-CAI978-22", "CHOOSE A CHEST", new Color(69f / 85f, 0.5254902f, 0.09019608f, 1f));
			m_ShowFloorBalanceUIGroup.Add(uIText);
			for (int i = 0; i < m_iFloorBalanceMaxCount; i++)
			{
				UIClickButton control2 = UIUtils.BuildClickButton(3077 + i, new Rect(155 + i * 130, 181f, 166f, 200f), m_MatDialog01, new Rect(526f, 398f, 166f, 200f), new Rect(526f, 398f, 166f, 200f), new Rect(526f, 398f, 166f, 200f), new Vector2(166f, 200f));
				m_ShowFloorBalanceUIGroup.Add(control2);
			}
			if (m_FloorBalanceCountDownText == null)
			{
				m_FloorBalanceCountDownText = UIUtils.BuildUIText(0, new Rect(300f, 410f, 400f, 30f), UIText.enAlignStyle.center);
				m_FloorBalanceCountDownText.Set("Zombie3D/Font/037-CAI978-22", "OVER IN:  " + timer, Color.white);
				m_FloorBalanceCountDownText.Visible = false;
				m_ShowFloorBalanceUIGroup.Add(m_FloorBalanceCountDownText);
			}
		}
	}

	public void UpdateFloorbalanceTimer(float timer)
	{
		if (m_FloorBalanceCountDownText != null)
		{
			int num = (int)timer;
			if (m_iFloorBalanceCountDownTimer != num)
			{
				m_FloorBalanceCountDownText.SetText("OVER IN:  " + ((num > 0) ? num : 0));
				m_iFloorBalanceCountDownTimer = num;
			}
		}
		if (timer >= 2f)
		{
			return;
		}
		if (timer >= 0f && timer < 2f)
		{
			if (m_iFloorBalanceSelectIndex == -1)
			{
				int num2 = UnityEngine.Random.Range(0, m_iFloorBalanceMaxCount);
				ChooseAFloorbalanceGift(num2 + 3077);
			}
		}
		else if (timer < 0f)
		{
			Debug.LogWarning("Wrong Time" + timer);
		}
	}

	public void UpdateFloorbalanceTimer()
	{
		if (m_RescueCountText != null)
		{
			int num = 0;
			if (gameState.GetPowerUPS().ContainsKey(12))
			{
				num = (int)gameState.GetPowerUPS()[12];
			}
			m_RescueCountText.SetText("  X" + num);
		}
	}

	public void FloorbalanceTimeout()
	{
		m_FloorBalanceCountDownText = null;
		m_iFloorBalanceCountDownTimer = -1;
		m_iFloorBalanceSelectIndex = -1;
		SetupFloorbalance(false);
		SetupControlCoverUI(false);
		if (gameState.m_eGameMode.m_ePlayMode == GameState.NetworkGameMode.PlayMode.E_PVE_BossRush)
		{
			PlayerManager.Instance.m_fBrushStrangeTimer = 0f;
		}
	}

	public void RestJoystick()
	{
		if (m_MoveJoystickBtn != null)
		{
			m_MoveJoystickBtn.Reset();
		}
		if (m_ShootJoystickBtn != null)
		{
			m_ShootJoystickBtn.Reset();
		}
		((TopWatchingInputController)player.InputController).MoveDirection = Vector3.zero;
		player.StopRun();
		player.SetState(Player.IDLE_STATE);
		((TopWatchingInputController)player.InputController).bFire = false;
		player.StopFire();
	}

	public void StickShopUI()
	{
		if (m_bItemBarGroupIsInUIManager)
		{
			m_UIManager.Remove(m_ItemBarGroup);
			m_UIManager.Add(m_ItemBarGroup);
			m_bItemBarGroupIsInUIManager = true;
		}
		if (m_bNBattleShopItemDetailGroupIsInManager)
		{
			m_UIManager.Remove(m_NBattleShopItemDetailGroup);
			m_UIManager.Add(m_NBattleShopItemDetailGroup);
			m_bNBattleShopItemDetailGroupIsInManager = true;
		}
	}

	public void UpdateUIToHeight()
	{
		StickShopUI();
		if (m_DesktopGroup != null)
		{
			if (m_NQuiteBtn == null)
			{
				m_NQuiteBtn = UIUtils.BuildClickButton(3007, new Rect(0f, 565f, 98f, 75f), m_MatBattleUI, new Rect(4f, 128f, 98f, 75f), new Rect(99f, 128f, 98f, 75f), new Rect(4f, 128f, 98f, 75f), new Vector2(98f, 75f));
				m_UIManager.Add(m_NQuiteBtn);
			}
			else
			{
				m_UIManager.Remove(m_NQuiteBtn);
				m_UIManager.Add(m_NQuiteBtn);
			}
		}
		if (m_bNPaushGroupIsInUIManager)
		{
			m_UIManager.Remove(m_NPaushGroup);
			m_UIManager.Add(m_NPaushGroup);
			m_bNPaushGroupIsInUIManager = true;
		}
	}

	public void ChooseAFloorbalanceGift(int giftIndex)
	{
		m_iFloorBalanceSelectIndex = giftIndex;
		UIClickButton uIClickButton = (UIClickButton)m_ShowFloorBalanceUIGroup.GetControl(giftIndex);
		uIClickButton.Visible = false;
		UIBlock uIBlock = new UIBlock();
		uIBlock.Rect = new Rect(99f, 57f, 772f, 450f);
		m_ShowFloorBalanceUIGroup.Add(uIBlock);
		SetFloorBalanceGiftList(gameState.m_eGameMode.PVE_FLOOR, m_iFloorBalanceSelectIndex);
		ShowOpenFloorbalanceAnimation(giftIndex - 3077, uIClickButton.Rect, AfterGiftChooseOKCallBack);
	}

	public void SetFloorBalanceGiftList(int floorLevel, int selectID)
	{
		if (m_lsFloorBalanceGift == null)
		{
			m_lsFloorBalanceGift = new List<KeyValuePair<int, int>>();
		}
		else
		{
			m_lsFloorBalanceGift.Clear();
		}
		int num = selectID - 3077;
		for (int i = 0; i < m_iFloorBalanceMaxCount; i++)
		{
			foreach (KeyValuePair<int, int> item in GetMap3_Type_Reward(1))
			{
				m_lsFloorBalanceGift.Add(item);
			}
		}
		if (floorLevel % 2 == 0)
		{
			int num2 = UnityEngine.Random.Range(0, 100);
			int num3 = 40;
			int num4 = UnityEngine.Random.Range(0, 100);
			int num5 = 50;
			int num6 = -1;
			int num7 = 7;
			switch (floorLevel)
			{
			case 2:
				num7 = 7;
				break;
			case 4:
				num7 = UnityEngine.Random.Range(5, 7);
				break;
			case 6:
				num7 = UnityEngine.Random.Range(4, 7);
				break;
			case 8:
				num7 = UnityEngine.Random.Range(3, 6);
				break;
			case 10:
				num7 = UnityEngine.Random.Range(1, 4);
				break;
			default:
				num7 = 7;
				Debug.LogWarning("Wrong FloorID_" + floorLevel);
				break;
			}
			if (gameState.GetOwnBuffCount() <= 0)
			{
				num3 = 100;
				num5 = 100;
			}
			if (num2 <= num3)
			{
				num6 = ((num4 > num5) ? gameState.GiveAvatarPropsAddition(num7) : gameState.GiveWeaponPropsAddition(num7));
				m_lsFloorBalanceGift[num] = new KeyValuePair<int, int>(1000 + num6, 1);
				return;
			}
			num6 = ((num4 > num5) ? gameState.GiveAvatarPropsAddition(num7, false) : gameState.GiveWeaponPropsAddition(num7, false));
			int num8 = num;
			switch (num8)
			{
			case 0:
				num8++;
				break;
			case 4:
				num8--;
				break;
			default:
				num8 = ((UnityEngine.Random.Range(0, 1) != 0) ? (num8 + 1) : (num8 - 1));
				break;
			}
			m_lsFloorBalanceGift[num8] = new KeyValuePair<int, int>(1000 + num6, 1);
			List<KeyValuePair<int, int>> list = new List<KeyValuePair<int, int>>();
			list.Add(m_lsFloorBalanceGift[num]);
			AddReward(list);
		}
		else
		{
			List<KeyValuePair<int, int>> list2 = new List<KeyValuePair<int, int>>();
			list2.Add(m_lsFloorBalanceGift[num]);
			AddReward(list2);
		}
	}

	private void AddReward(List<KeyValuePair<int, int>> ls)
	{
		for (int i = 0; i < ls.Count; i++)
		{
			if (ls[i].Key < 0)
			{
				gameState.AddDollor(ls[i].Value);
				gameState.AddEveryDayCrystalLootTotalCount(ls[i].Value);
				continue;
			}
			for (int j = 0; j < ls[i].Value; j++)
			{
				if (ls[i].Key == 101)
				{
					gameState.AddGold(ls[i].Value);
				}
				else if (ls[i].Key == 102)
				{
					gameState.AddExp(ls[i].Value);
				}
				else
				{
					gameState.BuyPowerUPS((ItemType)ls[i].Key);
				}
			}
		}
	}

	public void ShowOtherGiftAnimation()
	{
		for (int i = 3077; i < 3077 + m_iFloorBalanceMaxCount; i++)
		{
			if (m_iFloorBalanceSelectIndex != i)
			{
				UIClickButton uIClickButton = (UIClickButton)m_ShowFloorBalanceUIGroup.GetControl(i);
				uIClickButton.Visible = false;
				ShowOpenFloorbalanceAnimation(i - 3077, uIClickButton.Rect, AfterOtherGiftShowOKCallBack);
			}
		}
	}

	public void ShowOpenFloorbalanceAnimation(int aniIndex, Rect rect, UIAnimationEnd_CallBackEvent callback)
	{
		if (m_MatDialog01 == null)
		{
			m_MatDialog01 = LoadUIMaterial("Zombie3D/UI/Materials/NDialog01");
			Resources.UnloadUnusedAssets();
		}
		UIAnimationControl uIAnimationControl = new UIAnimationControl();
		uIAnimationControl.Id = aniIndex;
		uIAnimationControl.SetAnimationsPageCount(3);
		uIAnimationControl.Rect = AutoUI.AutoRect(rect);
		uIAnimationControl.SetTexture(0, m_MatDialog01, AutoUI.AutoRect(new Rect(526f, 398f, 166f, 200f)), AutoUI.AutoSize(new Vector2(166f, 200f)));
		uIAnimationControl.SetTexture(1, m_MatDialog01, AutoUI.AutoRect(new Rect(692f, 398f, 166f, 200f)), AutoUI.AutoSize(new Vector2(166f, 200f)));
		uIAnimationControl.SetTexture(2, m_MatDialog01, AutoUI.AutoRect(new Rect(858f, 398f, 166f, 200f)), AutoUI.AutoSize(new Vector2(166f, 200f)));
		uIAnimationControl.SetTimeInterval(0.1f);
		uIAnimationControl.SetLoopCount(1);
		uIAnimationControl.m_AnimationEndCallback = callback;
		m_ShowFloorBalanceUIGroup.Add(uIAnimationControl);
	}

	public void AfterGiftChooseOKCallBack(UIAnimationControl control)
	{
		if (m_MatDialog01 == null)
		{
			m_MatDialog01 = LoadUIMaterial("Zombie3D/UI/Materials/NDialog01");
			Resources.UnloadUnusedAssets();
		}
		int key = m_lsFloorBalanceGift[control.Id].Key;
		int value = m_lsFloorBalanceGift[control.Id].Value;
		if (m_ShowFloorBalanceUIGroup != null)
		{
			UIImage control2 = UIUtils.BuildImage(0, control.Rect, m_MatDialog01, new Rect(858f, 398f, 166f, 200f), new Vector2(166f, 200f));
			m_ShowFloorBalanceUIGroup.Add(control2);
			ShowGiftImage(m_ShowFloorBalanceUIGroup, key, control.Rect, value);
		}
		ShowOtherGiftAnimation();
	}

	public void AfterOtherGiftShowOKCallBack(UIAnimationControl control)
	{
		if (m_MatDialog01 == null)
		{
			m_MatDialog01 = LoadUIMaterial("Zombie3D/UI/Materials/NDialog01");
			Resources.UnloadUnusedAssets();
		}
		int key = m_lsFloorBalanceGift[control.Id].Key;
		int value = m_lsFloorBalanceGift[control.Id].Value;
		if (m_ShowFloorBalanceUIGroup != null)
		{
			UIImage control2 = UIUtils.BuildImage(0, control.Rect, m_MatDialog01, new Rect(858f, 398f, 166f, 200f), new Vector2(166f, 200f));
			m_ShowFloorBalanceUIGroup.Add(control2);
			ShowGiftImage(m_ShowFloorBalanceUIGroup, key, control.Rect, value);
		}
		if (!m_FloorBalanceCountDownText.Visible)
		{
			m_FloorBalanceCountDownText.Visible = true;
		}
	}

	public void ShowGiftImage(uiGroup group, int itemID, Rect animationRect, int itemCount)
	{
		UIImage uIImage = null;
		if (itemID >= 1000)
		{
			int num = 60;
			KeyValuePair<int, int> worAIDByPropsID = gameState.GetWorAIDByPropsID(itemID - 1000);
			if (worAIDByPropsID.Key == 0)
			{
				Rect avatarIconTexture = ShopUIScript.GetAvatarIconTexture((Avatar.AvatarSuiteType)worAIDByPropsID.Value, Avatar.AvatarType.Head);
				Rect rcMat = new Rect(avatarIconTexture.x / 2f, avatarIconTexture.y / 2f, avatarIconTexture.width / 2f, avatarIconTexture.height / 2f);
				uIImage = UIUtils.BuildImage(0, new Rect(animationRect.x + 50f, animationRect.y + 110f, rcMat.width, rcMat.height), m_MatAvatarIcons, rcMat, new Vector2(rcMat.width, rcMat.height));
				group.Add(uIImage);
			}
			else if (worAIDByPropsID.Key == 1)
			{
				Rect avatarIconTexture2 = ShopUIScript.GetAvatarIconTexture((Avatar.AvatarSuiteType)worAIDByPropsID.Value, Avatar.AvatarType.Body);
				Rect rcMat2 = new Rect(avatarIconTexture2.x / 2f, avatarIconTexture2.y / 2f, avatarIconTexture2.width / 2f, avatarIconTexture2.height / 2f);
				uIImage = UIUtils.BuildImage(0, new Rect(animationRect.x + 40f, animationRect.y + 100f, rcMat2.width, rcMat2.height), m_MatAvatarIcons, rcMat2, new Vector2(rcMat2.width, rcMat2.height));
				group.Add(uIImage);
			}
			else if (worAIDByPropsID.Key == 2)
			{
				Material mat = LoadUIMaterial("Zombie3D/UI/Materials/WeaponIcons");
				Resources.UnloadUnusedAssets();
				Rect weaponIconTexture = ShopUIScript.GetWeaponIconTexture((WeaponType)worAIDByPropsID.Value);
				int num2 = (int)(weaponIconTexture.width / 1.5f);
				int num3 = (int)(weaponIconTexture.height / 1.5f);
				uIImage = UIUtils.BuildImage(0, new Rect(animationRect.x + 25f - 25f, animationRect.y + 120f, num2, num3), mat, weaponIconTexture, new Vector2(num2, num3));
				group.Add(uIImage);
			}
			else
			{
				Debug.Log("Wrong");
			}
			return;
		}
		int num4 = 60;
		int num5 = 45;
		if (itemID < 0)
		{
			uIImage = UIUtils.BuildImage(0, new Rect(animationRect.x + (float)num4, animationRect.y + 120f, 50f, 45f), m_MatPerfectWaveEffect, new Rect(972f, 44f, 50f, 45f), new Vector2(50f, 45f));
			group.Add(uIImage);
			UIText uIText = UIUtils.BuildUIText(0, new Rect(uIImage.Rect.x + (float)num5, uIImage.Rect.y + 40f, 25f, 25f), UIText.enAlignStyle.left);
			uIText.Set("Zombie3D/Font/037-CAI978-18", " X" + itemCount, Color.white);
			group.Add(uIText);
			return;
		}
		switch (itemID)
		{
		case 101:
		{
			uIImage = UIUtils.BuildImage(0, new Rect(animationRect.x + (float)num4 - 40f, animationRect.y + 120f, 100f, 90f), m_MatPerfectWaveEffect, new Rect(971f, 1f, 50f, 45f), new Vector2(100f, 90f));
			group.Add(uIImage);
			UIText uIText3 = UIUtils.BuildUIText(0, new Rect(uIImage.Rect.x + (float)num5, uIImage.Rect.y + 40f, 25f, 25f), UIText.enAlignStyle.left);
			uIText3.Set("Zombie3D/Font/037-CAI978-18", " X" + itemCount, Color.white);
			group.Add(uIText3);
			break;
		}
		case 102:
		{
			uIImage = UIUtils.BuildImage(0, new Rect(animationRect.x + (float)num4 - 20f, animationRect.y + 120f, 63f, 42f), m_MatPerfectWaveEffect, new Rect(961f, 90f, 63f, 42f), new Vector2(63f, 42f));
			group.Add(uIImage);
			UIText uIText2 = UIUtils.BuildUIText(0, new Rect(uIImage.Rect.x + (float)num5 + 20f, uIImage.Rect.y + 40f, 25f, 25f), UIText.enAlignStyle.left);
			uIText2.Set("Zombie3D/Font/037-CAI978-18", " X" + itemCount, Color.white);
			group.Add(uIText2);
			break;
		}
		default:
		{
			Material mat2 = LoadUIMaterial("Zombie3D/UI/Materials/PowerUPSIcons");
			Resources.UnloadUnusedAssets();
			Rect powerUpsIconTexture = ShopUIScript.GetPowerUpsIconTexture((ItemType)itemID);
			uIImage = UIUtils.BuildImage(0, new Rect(animationRect.x + (float)num4 - 35f, animationRect.y + 120f, powerUpsIconTexture.width, powerUpsIconTexture.height), mat2, powerUpsIconTexture, new Vector2(powerUpsIconTexture.width, powerUpsIconTexture.height));
			group.Add(uIImage);
			break;
		}
		}
	}

	private List<KeyValuePair<int, int>> GetMap3_Type_Reward(int mode, int GiveCount = 1)
	{
		List<KeyValuePair<int, int>> list = new List<KeyValuePair<int, int>>();
		List<KeyValuePair<int, int>> list2 = new List<KeyValuePair<int, int>>();
		if (mode == 1)
		{
			list2.Add(new KeyValuePair<int, int>(2, 10));
			list2.Add(new KeyValuePair<int, int>(4, 10));
			list2.Add(new KeyValuePair<int, int>(7, 15));
			list2.Add(new KeyValuePair<int, int>(10, 10));
			list2.Add(new KeyValuePair<int, int>(8, 10));
			list2.Add(new KeyValuePair<int, int>(12, 10));
			list2.Add(new KeyValuePair<int, int>(-1, 5));
			list2.Add(new KeyValuePair<int, int>(101, 15));
			list2.Add(new KeyValuePair<int, int>(102, 15));
			for (int i = 0; i < GiveCount; i++)
			{
				int index = RandomGift(list2);
				if (list2[index].Key == 101)
				{
					list.Add(new KeyValuePair<int, int>(list2[index].Key, gameState.Level * 10));
				}
				else if (list2[index].Key == 102)
				{
					list.Add(new KeyValuePair<int, int>(list2[index].Key, gameState.Level * 20));
				}
				else
				{
					list.Add(new KeyValuePair<int, int>(list2[index].Key, 1));
				}
			}
			return list;
		}
		list.Add(new KeyValuePair<int, int>(0, 1));
		Debug.LogError("Error mode !!" + mode);
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

	public void BuildIpone5Frame(bool bShow)
	{
		if (m_IphoneFrame != null)
		{
			m_IphoneFrame.Clear();
			m_IphoneFrame = null;
		}
		if (bShow)
		{
			m_IphoneFrame = new uiGroup(m_UIManager);
			float num = (float)Screen.width / (float)Screen.height;
			float num2 = 1.5f;
			UIImage uIImage = null;
			if (num > num2)
			{
				float num3 = 640f / (float)Screen.height * (float)Screen.width - 960f;
				Material mat = Resources.Load("Zombie3D/UI/Materials/VerticalFrame") as Material;
				uIImage = UIUtils.BuildImage(0, new Rect(0f - num3 / 2f, 0f, 90f, 640f), mat, new Rect(0f, 0f, 90f, 640f), new Vector2(90f, 640f), 0);
				m_IphoneFrame.Add(uIImage);
				uIImage = UIUtils.BuildImage(0, new Rect(960f + num3 / 2f - 90f, 0f, 90f, 640f), mat, new Rect(90f, 0f, 90f, 640f), new Vector2(90f, 640f), 0);
				m_IphoneFrame.Add(uIImage);
			}
			else if (num != num2)
			{
				float num4 = 960f / (float)Screen.width * (float)Screen.height - 640f;
				Material mat2 = Resources.Load("Zombie3D/UI/Materials/HorizontalFrameUI") as Material;
				uIImage = UIUtils.BuildImage(0, new Rect(0f, 640f + num4 / 2f - 64f, 1024f, 64f), mat2, new Rect(0f, 0f, 1024f, 64f), new Vector2(1024f, 64f), 0);
				m_IphoneFrame.Add(uIImage);
				uIImage = UIUtils.BuildImage(0, new Rect(0f, 0f - num4 / 2f, 1024f, 64f), mat2, new Rect(0f, 64f, 1024f, 64f), new Vector2(1024f, 64f), 0);
				m_IphoneFrame.Add(uIImage);
			}
		}
	}

	public Rect GetWeaponTexture(WeaponType item_type)
	{
		switch (item_type)
		{
		case WeaponType.Longinus:
			return new Rect(567f, 507f, 141f, 124f);
		case WeaponType.Messiah:
			return new Rect(567f, 642f, 136f, 128f);
		case WeaponType.MassacreCannon:
			return new Rect(708f, 521f, 132f, 116f);
		case WeaponType.Lightning:
			return new Rect(840f, 521f, 136f, 121f);
		default:
			return new Rect(0f, 0f, 0f, 0f);
		}
	}

	public Rect GetBuffIconTexture(enBattlefieldProps item_type)
	{
		switch (item_type)
		{
		case enBattlefieldProps.E_AnaestheticProjectile:
			return new Rect(353f, 121f, 88f, 88f);
		case enBattlefieldProps.E_BestRunner:
			return new Rect(3f, 353f, 88f, 88f);
		case enBattlefieldProps.E_Tenacity:
			return new Rect(214f, 346f, 88f, 88f);
		case enBattlefieldProps.E_StrongWeapon:
			return new Rect(323f, 291f, 88f, 88f);
		case enBattlefieldProps.E_QuickRevive:
			return new Rect(323f, 291f, 88f, 88f);
		default:
			return new Rect(0f, 0f, 0f, 0f);
		}
	}

	public string GetBuffName(enBattlefieldProps item_type)
	{
		switch (item_type)
		{
		case enBattlefieldProps.E_AnaestheticProjectile:
			return "Anesthesia";
		case enBattlefieldProps.E_BestRunner:
			return "Marathoner";
		case enBattlefieldProps.E_QuickRevive:
			return "Hyper Regen";
		case enBattlefieldProps.E_StrongWeapon:
			return "Shiny New Gun";
		case enBattlefieldProps.E_Tenacity:
			return "Tough Guy";
		default:
			return string.Empty;
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
