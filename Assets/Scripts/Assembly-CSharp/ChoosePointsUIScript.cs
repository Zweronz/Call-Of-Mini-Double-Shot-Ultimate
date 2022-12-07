using System;
using System.Collections.Generic;
using UnityEngine;
using Zombie3D;

public class ChoosePointsUIScript : MonoBehaviour, UIHandler
{
	public enum Controls
	{
		kIDLevels = 5000,
		kIDBoost = 5001,
		kIDFriends = 5002,
		kIDShop = 5003,
		kIDTChat = 5004,
		kIDOptions = 5005,
		kIDCup = 5006,
		kIDTopList = 5007,
		kIDJunjie = 5008,
		kIDJunjieClose = 5009,
		kIDGlobalBank = 5010,
		kIDMapBank = 5011,
		kIDMapMERC = 5012,
		kIDMapOline = 5013,
		kIDMapOline_PVE = 5014,
		kIDNBack = 5015,
		kIDSave = 5016,
		kIDSureModeOK = 5017,
		kIDEditName = 5018,
		kIDInputOpen = 5019,
		kIDLastStandingModel_Team = 5020,
		kIDLastStandingModel_Solo = 5021,
		kIDDeathMatch_Team = 5022,
		kIDDeathMatch_Solo = 5023,
		kIDNCreateRoom = 5024,
		kIDNRefreshRoomList = 5025,
		kIDNTeamPlay = 5026,
		kIDNSimplePlay = 5027,
		kIDNStartGame = 5028,
		kIDNMessageBoxOK = 5029,
		kIDNRoomIndexBegin = 5030,
		kIDNRoomIndexEnd = 5285,
		kIDNRoomSeatIndexBegin = 5286,
		kIDNRoomSeatIndexEnd = 5296,
		kIDMapSurvivalMode = 5297,
		kIDMapSurvivalModeBack = 5298,
		kIDMapSurvivalModeOK = 5299,
		kIDSurvivalModeScroll = 5300,
		kIDChooseMap1Group = 5301,
		kIDChooseMap2Group = 5302,
		kIDChooseMap1 = 5303,
		kIDChooseMap2 = 5304,
		kIDChooseMap3 = 5305,
		kIDChooseMap4 = 5306,
		kIDChooseMap5 = 5307,
		kIDChooseMap6 = 5308,
		kIDChooseMapLocked = 5309,
		kIDChooseMapNotHave = 5310,
		kIDChooseMapBack = 5311,
		kIDChoosePointBack = 5312,
		kIDChooseWaveBack = 5313,
		kIDChooseMapDetailOK = 5314,
		kIDChooseMapDetailLater = 5315,
		kIDChooseMapDetailELITE = 5316,
		kIDUnlockMap3WithDollor = 5317,
		kIDUnlockMap4WithDollor = 5318,
		kIDUnlockMap5WithDollor = 5319,
		kIDHintDialogOK = 5320,
		kIDHintDialogYes = 5321,
		kIDHintDialogNo = 5322,
		kIDGotoBank = 5323,
		kIDNewVersionFeatureYes = 5324,
		kIDNewVersionFeatureYes_1_2 = 5325,
		kIDNewVersionFeatureYes_1_3 = 5326,
		kIDDayAwardYes = 5327,
		kIDExternExpYes = 5328,
		kIDNotificationDialogOK = 5329,
		kIDPlayerPrograssBtn = 5330,
		kIDSendGiftGoldDollorOK = 5331,
		kIDChoosePointsBegin = 5332,
		kIDChooseMap7 = 533333,
		kIDChoosePointsLast = 5432,
		kIDChooseWavesBegin = 5433,
		kIDChooseWavesLast = 5533,
		kIDChoosePointsBegin_Boss = 5534,
		kIDChoosePointsLast_Boss = 5634,
		kIDLast = 5635
	}

	private UIManager m_UIManager;

	private string m_ui_material_path;

	private GameState gameState;

	protected Material m_MatCommonBg;

	protected Material m_MatChoosePointsUI;

	protected Material m_MatChoosePoints1UI;

	protected Material m_MatChoosePoints2UI;

	protected Material m_MatMap01UI;

	protected Material m_MatMap02UI;

	protected Material m_MatDialog01;

	public uiGroup m_uiGroup;

	public uiGroup m_MapsGroup;

	public uiGroup m_MapDetailGroup;

	public uiGroup m_ItemBarGroup;

	public uiGroup m_AroundUIGroup;

	public uiGroup m_DialogUI;

	public uiGroup m_JunjieUIGroup;

	public uiGroup m_uiHintDialog;

	public uiGroup m_Dialog01UI;

	public uiGroup m_SendGoldDollorUI;

	public uiGroup m_SurvivalModeMapsGroup;

	private bool m_ChoosePointsShowAnim;

	private bool m_ChoosePointsBegin;

	private UIBlock m_ChoosePointsMoveBlk;

	private UIScrollPageView m_ChoosePointsView;

	private UIDotScrollBar m_ChoosePointsViewScrollBar;

	private UIScrollView m_ChoosePointsView_Boss;

	private UIDotScrollBar m_ChoosePointsViewScrollBar_Boss;

	public GameLoadingUI m_GameLoadingUI;

	public UIGroupControl m_ChooseWaveGroup;

	protected float lastUpdateTime;

	protected bool uiInited;

	private int mapIndex = 1;

	private int pointsIndex = 1;

	private int waveIndex = 1;

	private static bool m_bShowDailyBonus;

	private int m_DailyBonus;

	protected float joystickBgImgAlpha = 0.3f;

	protected bool m_bBeginBattle;

	protected float m_BeginBattleTimer;

	private UIText m_Map3CDText;

	private float m_LastMapsCDCheckTime;

	private UIText m_Map4CDText;

	private float m_LastMap4MapsCDCheckTime;

	private UIText m_Map5CDText;

	private float m_LastMap5MapsCDCheckTime;

	private UIClickButton playerHpProgressBtn;

	private UIAnimationControl playerHpProgressBarBtnAnim;

	private UIAnimationControl m_MercAnim;

	private UIImageScroller m_SurvivalModeMapsScrollBar;

	private UIText m_strSurvivalModeMapNameShow;

	private uint m_SurvivalModeSelectedMapIndex;

	public bool m_bNeedShowAppStore;

	private void Start()
	{
		m_UIManager = base.gameObject.AddComponent<UIManager>() as UIManager;
		m_UIManager.SetParameter(8, 3, false);
		m_UIManager.SetUIHandler(this);
		UIUtils.BuildIpone5Frame(m_UIManager);
		m_MatCommonBg = LoadUIMaterial("Zombie3D/UI/Materials/CommonBgUI");
		m_MatChoosePointsUI = LoadUIMaterial("Zombie3D/UI/Materials/ChoosePointsUI");
		m_MatChoosePoints1UI = LoadUIMaterial("Zombie3D/UI/Materials/ChoosePoints1UI");
		m_MatChoosePoints2UI = LoadUIMaterial("Zombie3D/UI/Materials/ChoosePoints2UI");
		m_MatMap01UI = LoadUIMaterial("Zombie3D/UI/Materials/Map01UI");
		m_MatMap02UI = LoadUIMaterial("Zombie3D/UI/Materials/Map02UI");
		m_MatDialog01 = LoadUIMaterial("Zombie3D/UI/Materials/Dialog01");
		Resources.UnloadUnusedAssets();
		gameState = GameApp.GetInstance().GetGameState();
		uiInited = true;
		SetupChooseMapsUI(true);
		if (SceneUIManager.Instance().GetMusicPlayerState() != SceneUIManager.MusicState_GameStartFirstPlayState && SceneUIManager.Instance().GetMusicPlayerState() != SceneUIManager.MusicState_GameStartNotFirstPlayState)
		{
			SceneUIManager.Instance().SetMusicPlayerState(SceneUIManager.MusicState_ChoosePointsAudioState);
		}
		if (!gameState.CheckNewVersionFeatureShow())
		{
			CheckDailyBonus();
			if (gameState.BeHiredMoney > 0)
			{
				SetupNotificationDialogUI(true, "Your character was hired out for " + gameState.BeHiredMoney + " cash yesterday! The system took a cut of 25%.");
				gameState.AddGold(gameState.BeHiredMoney);
				gameState.BeHiredMoney = 0;
			}
		}
		GameCollectionInfoManager.Instance().GetCurrentInfo().AddUIEnterLog(GameCollectionInfo.enUIEnterIndex.ChoosePointsUI);
		gameState.m_eGameMode.m_ePlayMode = GameState.NetworkGameMode.PlayMode.E_Console;
		if (SmartFoxConnection.IsInitialized)
		{
			SmartFoxConnection.DisConnect();
		}
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
		if (m_bBeginBattle)
		{
			m_BeginBattleTimer += num;
			if (m_BeginBattleTimer > 0.1f)
			{
				if (gameState.m_bIsSurvivalMode)
				{
					if (m_SurvivalModeSelectedMapIndex + 1 == 1)
					{
						gameState.SetGameTriggerInfo(101, 1, 1);
						List<string> survivalModeSceneNames = GameScene.GetSurvivalModeSceneNames(101);
						List<string> list = new List<string>();
						for (int j = 0; j < survivalModeSceneNames.Count; j++)
						{
							if (survivalModeSceneNames[j] != Application.loadedLevelName)
							{
								list.Add(survivalModeSceneNames[j]);
							}
						}
						string text = list[UnityEngine.Random.Range(0, list.Count)];
						GameApp.GetInstance().SetLoadMap(true);
						Application.LoadLevel(text);
						SceneUIManager.Instance().ChangeSceneUI(SceneUIManager.SceneUI.BattleUI);
					}
					else if (m_SurvivalModeSelectedMapIndex + 1 == 2)
					{
						gameState.SetGameTriggerInfo(102, 1, 1);
						List<string> survivalModeSceneNames2 = GameScene.GetSurvivalModeSceneNames(102);
						List<string> list2 = new List<string>();
						for (int k = 0; k < survivalModeSceneNames2.Count; k++)
						{
							if (survivalModeSceneNames2[k] != Application.loadedLevelName)
							{
								list2.Add(survivalModeSceneNames2[k]);
							}
						}
						string text2 = list2[UnityEngine.Random.Range(0, list2.Count)];
						GameApp.GetInstance().SetLoadMap(true);
						Application.LoadLevel(text2);
						SceneUIManager.Instance().ChangeSceneUI(SceneUIManager.SceneUI.BattleUI);
					}
				}
				else
				{
					GameCollectionInfoManager.Instance().GetCurrentInfo().UpdatePointsInfo(mapIndex, pointsIndex, waveIndex);
					if (mapIndex == 1)
					{
						GameApp.GetInstance().SetLoadMap(true);
						Application.LoadLevel("Zombie3D_Judgement_Map01");
					}
					else if (mapIndex == 2)
					{
						GameApp.GetInstance().SetLoadMap(true);
						Application.LoadLevel("Zombie3D_Judgement_Map02");
					}
					else if (mapIndex == 3)
					{
						GameApp.GetInstance().SetLoadMap(true);
						Application.LoadLevel("Zombie3D_Judgement_Map03");
					}
					else if (mapIndex == 4)
					{
						GameApp.GetInstance().SetLoadMap(true);
						Application.LoadLevel("Zombie3D_Judgement_Map04");
					}
					else if (mapIndex == 5)
					{
						GameApp.GetInstance().SetLoadMap(true);
						Application.LoadLevel("Zombie3D_Judgement_Map10");
					}
					else if (mapIndex == 6)
					{
						GameApp.GetInstance().SetLoadMap(true);
						Application.LoadLevel("Zombie3D_Judgement_Map11");
					}
					else if (mapIndex == 7)
					{
						GameApp.GetInstance().SetLoadMap(true);
						Application.LoadLevel("Zombie3D_Judgement_Map19");
					}
					else if (mapIndex == 201)
					{
						Debug.Log("LoadLevel | NETMODE_MAP1INDEX");
						GameApp.GetInstance().SetLoadMap(true);
						Application.LoadLevel("Zombie3D_Judgement_Map15");
					}
					else if (mapIndex == 202)
					{
						Debug.Log("LoadLevel | NETMODE_MAP3INDEX");
						GameApp.GetInstance().SetLoadMap(true);
						Application.LoadLevel("Zombie3D_Judgement_Map16");
					}
					else if (mapIndex == 203)
					{
						Debug.Log("LoadLevel | NETMODE_MAP2INDEX");
						GameApp.GetInstance().SetLoadMap(true);
						Application.LoadLevel("Zombie3D_Judgement_Map17");
					}
					if (gameState.m_eGameMode.m_ePlayMode == GameState.NetworkGameMode.PlayMode.E_Console)
					{
						SceneUIManager.Instance().ChangeSceneUI(SceneUIManager.SceneUI.BattleUI);
					}
					else
					{
						SceneUIManager.Instance().ChangeSceneUI(SceneUIManager.SceneUI.NBattleUI, false);
					}
				}
				m_bBeginBattle = false;
			}
		}
		if (m_ChoosePointsShowAnim && m_ChoosePointsBegin)
		{
			Rect rect = m_ChoosePointsView_Boss.Rect;
			float t = 0.5f;
			m_ChoosePointsView_Boss.Rect = AutoUI.AutoRect(new Rect(Mathf.Lerp(rect.x, 70f, t), 207f, 960f, 201f));
			m_ChoosePointsView_Boss.Bounds = AutoUI.AutoRect(new Rect(Mathf.Lerp(m_ChoosePointsView_Boss.Bounds.x, 70f, t), 207f, 820f, 201f));
			if (m_ChoosePointsView_Boss.Rect.x - AutoUI.AutoDistance(70f) < AutoUI.AutoDistance(5f) && m_ChoosePointsMoveBlk.Enable)
			{
				m_ChoosePointsMoveBlk.Enable = false;
			}
			if (m_ChoosePointsView_Boss.Rect.x - AutoUI.AutoDistance(70f) < 0.1f)
			{
				m_ChoosePointsBegin = false;
				m_ChoosePointsView_Boss.Rect = AutoUI.AutoRect(new Rect(70f, 207f, 960f, 201f));
				m_ChoosePointsView_Boss.Bounds = AutoUI.AutoRect(new Rect(70f, 207f, 820f, 201f));
			}
		}
		if (m_Map3CDText != null && m_MapsGroup != null && Time.time - m_LastMapsCDCheckTime > 1f)
		{
			long mapsCDTime = gameState.GetMapsCDTime(3);
			int mapsCDTimeLength = GetMapsCDTimeLength(3);
			long nowDateSeconds = UtilsEx.getNowDateSeconds();
			long num2 = mapsCDTime + mapsCDTimeLength - nowDateSeconds;
			if (num2 > 0)
			{
				string text3 = UtilsEx.TimeToStr_HMS(num2);
				m_Map3CDText.SetText(text3);
			}
			else
			{
				SetupChooseMapsUI(true);
			}
		}
		if (m_Map4CDText != null && m_MapsGroup != null && Time.time - m_LastMap4MapsCDCheckTime > 1f)
		{
			long mapsCDTime2 = gameState.GetMapsCDTime(4);
			int mapsCDTimeLength2 = GetMapsCDTimeLength(4);
			long nowDateSeconds2 = UtilsEx.getNowDateSeconds();
			long num3 = mapsCDTime2 + mapsCDTimeLength2 - nowDateSeconds2;
			if (num3 > 0)
			{
				string text4 = UtilsEx.TimeToStr_HMS(num3);
				m_Map4CDText.SetText(text4);
			}
			else
			{
				SetupChooseMapsUI(true);
			}
		}
		if (m_Map5CDText != null && m_MapsGroup != null && Time.time - m_LastMap5MapsCDCheckTime > 1f)
		{
			long mapsCDTime3 = gameState.GetMapsCDTime(5);
			int mapsCDTimeLength3 = GetMapsCDTimeLength(5);
			long nowDateSeconds3 = UtilsEx.getNowDateSeconds();
			long num4 = mapsCDTime3 + mapsCDTimeLength3 - nowDateSeconds3;
			if (num4 > 0)
			{
				string text5 = UtilsEx.TimeToStr_HMS(num4);
				m_Map5CDText.SetText(text5);
			}
			else
			{
				SetupChooseMapsUI(true);
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
		if (control.Id == 5000)
		{
			return;
		}
		if (control.Id == 5001)
		{
			SceneUIManager.Instance().ChangeSceneUI(SceneUIManager.SceneUI.BoostUI);
		}
		else if (control.Id == 5002)
		{
			SceneUIManager.Instance().ChangeSceneUI(SceneUIManager.SceneUI.FriendUI);
		}
		else if (control.Id == 5003)
		{
			SceneUIManager.Instance().ChangeSceneUI(SceneUIManager.SceneUI.ShopUI);
		}
		else
		{
			if (control.Id == 5004)
			{
				return;
			}
			if (control.Id == 5005)
			{
				SceneUIManager.Instance().ChangeSceneUI(SceneUIManager.SceneUI.OptionUI);
			}
			else if (control.Id == 5006)
			{
				SetupNotificationDialogUI(true, "You need to log in to Game Center to proceed!");
			}
			else if (control.Id == 5007)
			{
				SetupNotificationDialogUI(true, "You need to log in to Game Center to proceed!");
			}
			else
			{
				if (control.Id == 5008)
				{
					return;
				}
				if (control.Id == 5009)
				{
					SetupJunjieUI(false);
				}
				else if (control.Id == 5303)
				{
					mapIndex = 1;
					SetupChooseMapDetailUI(true);
				}
				else if (control.Id == 5304)
				{
					mapIndex = 2;
					SetupChooseMapDetailUI(true);
				}
				else if (control.Id == 5305)
				{
					mapIndex = 3;
					SetupChooseMapDetailUI(true);
				}
				else if (control.Id == 5306)
				{
					mapIndex = 4;
					SetupChooseMapDetailUI(true);
				}
				else if (control.Id == 5307)
				{
					mapIndex = 5;
					SetupChooseMapDetailUI(true);
				}
				else if (control.Id == 5308)
				{
					mapIndex = 6;
					SetupChooseMapDetailUI(true);
				}
				else if (control.Id == 533333)
				{
					mapIndex = 7;
					SetupChooseMapDetailUI(true);
				}
				else if (control.Id == 5309)
				{
					mapIndex = 0;
					SetupChooseMapDetailUI(true);
				}
				else if (control.Id == 5310)
				{
					mapIndex = -1;
					SetupChooseMapDetailUI(true);
				}
				else if (control.Id == 5315)
				{
					SetupChooseMapDetailUI(false);
					OpenClickPlugin.Show(false);
				}
				else if (control.Id == 5314)
				{
					SetupChooseMapDetailUI(false);
					if (mapIndex > 0)
					{
						m_ChoosePointsShowAnim = true;
						if (mapIndex == 3 || mapIndex == 4 || mapIndex == 5)
						{
							SetupChoosePointsUI_Boss(true);
						}
						else
						{
							SetupChoosePointsUI(true);
						}
					}
				}
				else if (control.Id == 5316)
				{
					SetupChooseMapDetailUI(false);
					if (mapIndex > 0)
					{
						m_ChoosePointsShowAnim = true;
						if (mapIndex == 3 || mapIndex == 4 || mapIndex == 5)
						{
							SetupChoosePointsUI_Boss(true);
						}
						else
						{
							SetupChoosePointsUI(true, false);
						}
					}
				}
				else if (control.Id == 5317)
				{
					int num = 1;
					if (gameState.dollor >= num)
					{
						gameState.LoseDollor(num);
						gameState.UpdateCDEndTime(3, UtilsEx.getNowDateSeconds() - GetMapsCDTimeLength(3));
						SetupChooseMapsUI(true);
						SetupChooseMapDetailUI(false);
						if (mapIndex > 0)
						{
							m_ChoosePointsShowAnim = true;
							if (mapIndex == 3 || mapIndex == 4 || mapIndex == 5)
							{
								SetupChoosePointsUI_Boss(true);
							}
							else
							{
								SetupChoosePointsUI(true);
							}
						}
					}
					else
					{
						SetupNotHaveEnoughCrystalDialog(true);
					}
				}
				else if (control.Id == 5318)
				{
					int num2 = 1;
					if (gameState.dollor >= num2)
					{
						gameState.LoseDollor(num2);
						gameState.UpdateCDEndTime(4, UtilsEx.getNowDateSeconds() - GetMapsCDTimeLength(4));
						SetupChooseMapsUI(true);
						SetupChooseMapDetailUI(false);
						if (mapIndex > 0)
						{
							m_ChoosePointsShowAnim = true;
							if (mapIndex == 3 || mapIndex == 4 || mapIndex == 5)
							{
								SetupChoosePointsUI_Boss(true);
							}
							else
							{
								SetupChoosePointsUI(true);
							}
						}
					}
					else
					{
						SetupNotHaveEnoughCrystalDialog(true);
					}
				}
				else if (control.Id == 5319)
				{
					int num3 = 1;
					if (gameState.dollor >= num3)
					{
						gameState.LoseDollor(num3);
						gameState.UpdateCDEndTime(5, UtilsEx.getNowDateSeconds() - GetMapsCDTimeLength(5));
						SetupChooseMapsUI(true);
						SetupChooseMapDetailUI(false);
						if (mapIndex > 0)
						{
							m_ChoosePointsShowAnim = true;
							if (mapIndex == 3 || mapIndex == 4 || mapIndex == 5)
							{
								SetupChoosePointsUI_Boss(true);
							}
							else
							{
								SetupChoosePointsUI(true);
							}
						}
					}
					else
					{
						SetupNotHaveEnoughCrystalDialog(true);
					}
				}
				else if (control.Id >= 5332 && control.Id <= 5432)
				{
					pointsIndex = control.Id - 5332 + 1;
					gameState.m_bIsSurvivalMode = false;
					if (mapIndex == 1 || mapIndex == 2 || mapIndex == 6 || mapIndex == 7)
					{
						waveIndex = 1;
						gameState.SetGameTriggerInfo(mapIndex, pointsIndex, waveIndex);
						m_bBeginBattle = true;
						m_GameLoadingUI = new GameLoadingUI();
						m_GameLoadingUI.SetupLoadingUI(true, m_UIManager, m_MatCommonBg);
					}
					else if (mapIndex == 3 || mapIndex == 4 || mapIndex == 5)
					{
						gameState.SetGameTriggerInfo(mapIndex, pointsIndex, 1);
						m_bBeginBattle = true;
						m_GameLoadingUI = new GameLoadingUI();
						m_GameLoadingUI.SetupLoadingUI(true, m_UIManager, m_MatCommonBg);
					}
					gameState.AddDailyCollectionInfo(0, 0f, 1, 0);
				}
				else if (control.Id == 5312)
				{
					SetupChoosePointsUI(false);
					SetupChooseMapsUI(true);
				}
				else if (control.Id >= 5433 && control.Id <= 5533)
				{
					waveIndex = control.Id - 5433 + 1;
					gameState.SetGameTriggerInfo(mapIndex, pointsIndex, waveIndex);
					m_bBeginBattle = true;
					m_GameLoadingUI = new GameLoadingUI();
					m_GameLoadingUI.SetupLoadingUI(true, m_UIManager, m_MatCommonBg);
				}
				else if (control.Id == 5313)
				{
					SetupChooseWaveUI(false);
					m_ChoosePointsShowAnim = false;
					SetupChoosePointsUI(true);
				}
				else if (control.Id == 5324)
				{
					SetupNewVersionFeatureDialogUI(false, string.Empty);
					CheckDailyBonus();
				}
				else if (control.Id == 5325)
				{
					SetupNewVersionFeatureDialog02UI(false);
					CheckDailyBonus();
				}
				else if (control.Id == 5326)
				{
					SetupNewVersionFeatureDialog_1_3_UI(false);
					SceneUIManager.Instance().ChangeSceneUI(SceneUIManager.SceneUI.FriendsHireUI);
				}
				else if (control.Id == 5327)
				{
					SetupDailyBonusDialogUI(false, string.Empty);
					if (gameState.ExternExp > 0)
					{
						SetupExternExpDialogUI(true);
						gameState.AddExp(gameState.ExternExp);
						gameState.ExternExp = 0;
						GameApp.GetInstance().Save();
						//GameClient.SetUserData();
					}
					SetupAroundUI(true);
				}
				else if (control.Id == 5328)
				{
					SetupExternExpDialogUI(false);
					SetupAroundUI(true);
				}
				else if (control.Id == 5329)
				{
					SetupNotificationDialogUI(false, string.Empty);
					if (m_bNeedShowAppStore)
					{
						Application.OpenURL("http://itunes.apple.com/us/app/call-of-mini-double-shot/id459012341?mt=8");
						m_bNeedShowAppStore = false;
					}
				}
				else if (control.Id == 5320)
				{
					SetupNotHaveEnoughCrystalDialog(false);
				}
				else if (control.Id == 5321)
				{
					SetupNotHaveEnoughCrystalDialog(false);
				}
				else if (control.Id == 5322)
				{
					SetupNotHaveEnoughCrystalDialog(false);
				}
				else if (control.Id == 5323)
				{
					ShopUIScript.externalShopType = ShopUIScript.ShopType.Bank;
					SceneUIManager.Instance().ChangeSceneUI(SceneUIManager.SceneUI.ShopUI);
				}
				else if (control.Id == 5011)
				{
					ShopUIScript.externalShopType = ShopUIScript.ShopType.Bank;
					SceneUIManager.Instance().ChangeSceneUI(SceneUIManager.SceneUI.ShopUI);
				}
				else if (control.Id == 5330)
				{
					if (GameApp.GetInstance().GetGameState().GetNeedShowLevelupAnimation() && playerHpProgressBarBtnAnim != null)
					{
						playerHpProgressBarBtnAnim.Visible = false;
						playerHpProgressBarBtnAnim.Enable = false;
						GameApp.GetInstance().GetGameState().SetNeedShowLevelupAnimation(0);
						GameApp.GetInstance().Save();
					}
					SetupJunjieUI(true);
				}
				else if (control.Id == 5012)
				{
					if (UtilsEx.IsNetworkConnected())
					{
						control.Visible = false;
						if (m_MercAnim != null)
						{
							m_MercAnim.Enable = true;
							m_MercAnim.Visible = true;
						}
						SceneUIManager.Instance().ChangeSceneUI(SceneUIManager.SceneUI.FriendsHireUI);
					}
					else
					{
						SetupNotificationDialogUI(true, "Network connection required to visit the Merc Camp!");
					}
				}
				else if (control.Id == 5013)
				{
					gameState.m_bIsSurvivalMode = false;
					CheckVersion();
				}
				else if (control.Id == 5014)
				{
					gameState.m_bIsSurvivalMode = false;
					gameState.m_eGameMode.m_ePlayMode = GameState.NetworkGameMode.PlayMode.E_PVE_BossRush;
					gameState.m_eGameMode.m_eCooperaMode = GameState.NetworkGameMode.NetworkCooperationMode.E_Team;
					CheckVersion();
				}
				else if (control.Id == 5297)
				{
					SetupSurvivalModeChooseMapsUI(true);
				}
				else if (control.Id == 5298)
				{
					SetupSurvivalModeChooseMapsUI(false);
				}
				else if (control.Id == 5299)
				{
					gameState.m_bIsSurvivalMode = true;
					gameState.m_SurvivalModeBattledMapCount = 0u;
					if (m_SurvivalModeSelectedMapIndex == 0)
					{
						gameState.SetGameTriggerInfo(101, 1, 1);
					}
					else if (m_SurvivalModeSelectedMapIndex == 1)
					{
						gameState.SetGameTriggerInfo(102, 1, 1);
					}
					m_bBeginBattle = true;
					m_GameLoadingUI = new GameLoadingUI();
					m_GameLoadingUI.SetupLoadingUI(true, m_UIManager, m_MatCommonBg);
					if (m_SurvivalModeSelectedMapIndex + 1 == 1)
					{
						List<string> survivalModeSceneNames = GameScene.GetSurvivalModeSceneNames(101);
						List<string> list = new List<string>();
						for (int i = 0; i < survivalModeSceneNames.Count; i++)
						{
							if (survivalModeSceneNames[i] != Application.loadedLevelName)
							{
								list.Add(survivalModeSceneNames[i]);
							}
						}
						string text = list[UnityEngine.Random.Range(0, list.Count)];
						GameApp.GetInstance().SetLoadMap(true);
						Application.LoadLevel(text);
					}
					else
					{
						if (m_SurvivalModeSelectedMapIndex + 1 != 2)
						{
							return;
						}
						List<string> survivalModeSceneNames2 = GameScene.GetSurvivalModeSceneNames(102);
						List<string> list2 = new List<string>();
						for (int j = 0; j < survivalModeSceneNames2.Count; j++)
						{
							if (survivalModeSceneNames2[j] != Application.loadedLevelName)
							{
								list2.Add(survivalModeSceneNames2[j]);
							}
						}
						string text2 = list2[UnityEngine.Random.Range(0, list2.Count)];
						GameApp.GetInstance().SetLoadMap(true);
						Application.LoadLevel(text2);
					}
				}
				else if (control.Id == 5300)
				{
					if (command == 0)
					{
						int num4 = (int)(m_SurvivalModeSelectedMapIndex = (uint)Mathf.FloorToInt(wparam));
						string[] array = new string[2] { "Luxury Heights", "Central Hospital" };
						if (array.Length > 0 && m_strSurvivalModeMapNameShow != null)
						{
							m_strSurvivalModeMapNameShow.SetText(array[num4]);
						}
					}
				}
				else if (control.Id == 5331)
				{
					SetupSendGiftGoldDollor_1_3_2(false);
				}
				else if (control.Id == 5010)
				{
					ShopUIScript.externalShopType = ShopUIScript.ShopType.Bank;
					SceneUIManager.Instance().ChangeSceneUI(SceneUIManager.SceneUI.ShopUI);
				}
				else if (control.Id != 5635)
				{
				}
			}
		}
	}

	public void SetupCommonBarUI(uiGroup group)
	{
		UIImage uIImage = UIUtils.BuildImage(0, new Rect(0f, 0f, 960f, 640f), m_MatCommonBg, new Rect(0f, 0f, 960f, 640f), new Vector2(960f, 640f));
		SetupAroundUI(true);
		uIImage = UIUtils.BuildImage(0, new Rect(295f, 497f, 457f, 104f), m_MatCommonBg, new Rect(9f, 800f, 457f, 104f), new Vector2(457f, 104f));
		group.Add(uIImage);
		UIClickButton control = UIUtils.BuildClickButton(5000, new Rect(295f, 497f, 77f, 104f), m_MatCommonBg, new Rect(9f, 904f, 77f, 104f), new Rect(9f, 904f, 77f, 104f), new Rect(1022f, 1f, 1f, 1f), new Vector2(77f, 104f));
		group.Add(control);
		control = UIUtils.BuildClickButton(5001, new Rect(372f, 497f, 80f, 104f), m_MatCommonBg, new Rect(1022f, 1f, 1f, 1f), new Rect(939f, 707f, 80f, 104f), new Rect(1022f, 1f, 1f, 1f), new Vector2(80f, 104f));
		group.Add(control);
		control = UIUtils.BuildClickButton(5002, new Rect(452f, 497f, 76f, 104f), m_MatCommonBg, new Rect(1022f, 1f, 1f, 1f), new Rect(85f, 904f, 76f, 104f), new Rect(1022f, 1f, 1f, 1f), new Vector2(76f, 104f));
		group.Add(control);
		control = UIUtils.BuildClickButton(5003, new Rect(528f, 497f, 80f, 104f), m_MatCommonBg, new Rect(1022f, 1f, 1f, 1f), new Rect(160f, 904f, 80f, 104f), new Rect(1022f, 1f, 1f, 1f), new Vector2(80f, 104f));
		group.Add(control);
		control = UIUtils.BuildClickButton(5004, new Rect(603f, 503f, 80f, 104f), m_MatCommonBg, new Rect(1022f, 1f, 1f, 1f), new Rect(834f, 700f, 80f, 104f), new Rect(1022f, 1f, 1f, 1f), new Vector2(80f, 104f));
		control.Enable = false;
		group.Add(control);
		control = UIUtils.BuildClickButton(5005, new Rect(683f, 497f, 74f, 104f), m_MatCommonBg, new Rect(1022f, 1f, 1f, 1f), new Rect(240f, 904f, 74f, 104f), new Rect(1022f, 1f, 1f, 1f), new Vector2(74f, 104f));
		group.Add(control);
	}

	public void SetupAroundUI(bool bShow)
	{
		if (m_AroundUIGroup != null)
		{
			m_AroundUIGroup.Clear();
			m_AroundUIGroup = null;
		}
		if (bShow)
		{
			m_AroundUIGroup = new uiGroup(m_UIManager);
			UIImage control = UIUtils.BuildImage(0, new Rect(0f, 572f, 960f, 68f), m_MatCommonBg, new Rect(0f, 640f, 960f, 68f), new Vector2(960f, 68f));
			m_AroundUIGroup.Add(control);
			control = UIUtils.BuildImage(0, new Rect(0f, 0f, 62f, 572f), m_MatCommonBg, new Rect(960f, 0f, 62f, 572f), new Vector2(62f, 572f));
			m_AroundUIGroup.Add(control);
			control = UIUtils.BuildImage(0, new Rect(630f, 242f, 572f, 88f), m_MatCommonBg, new Rect(0f, 710f, 572f, 88f), new Vector2(572f, 88f));
			control.Rect = AutoUIResolution.ToShiftToRight(AutoUI.AutoRect(new Rect(872f, 0f, 88f, 572f)), 2);
			control.SetRotation((float)Math.PI / 2f);
			m_AroundUIGroup.Add(control);
			float playerExpNextLevelPercent = GameApp.GetInstance().GetGameState().GetPlayerExpNextLevelPercent();
			UIProgressBarProgressive control2 = UIUtils.BuildUIProgressBarRounded(0, new Rect(71f, 593f, 174f, 20f), m_MatCommonBg, m_MatCommonBg, new Rect(398f, 708f, 174f, 20f), new Rect(572f, 708f, 174f, 20f), playerExpNextLevelPercent);
			m_AroundUIGroup.Add(control2);
			UIText uIText = UIUtils.BuildUIText(0, new Rect(5f, 600f, 145f, 30f), UIText.enAlignStyle.left);
			uIText.Set("Zombie3D/Font/037-CAI978-22", "LV " + GameApp.GetInstance().GetGameState().GetPlayerLevel(), new Color(0.81960785f, 0.5294118f, 2f / 51f, 1f));
			m_AroundUIGroup.Add(uIText);
			control = UIUtils.BuildImage(0, new Rect(341f, 602f, 50f, 32f), m_MatCommonBg, new Rect(967f, 591f, 50f, 32f), new Vector2(50f, 32f));
			m_AroundUIGroup.Add(control);
			uIText = UIUtils.BuildUIText(0, new Rect(394f, 602f, 145f, 30f), UIText.enAlignStyle.left);
			uIText.Set("Zombie3D/Font/037-CAI978-22", GameApp.GetInstance().GetGameState().gold.ToString(), new Color(0.81960785f, 0.5294118f, 2f / 51f, 1f));
			m_AroundUIGroup.Add(uIText);
			control = UIUtils.BuildImage(0, new Rect(579f, 602f, 50f, 36f), m_MatCommonBg, new Rect(967f, 631f, 50f, 36f), new Vector2(50f, 36f));
			m_AroundUIGroup.Add(control);
			uIText = UIUtils.BuildUIText(0, new Rect(627f, 602f, 145f, 30f), UIText.enAlignStyle.left);
			uIText.Set("Zombie3D/Font/037-CAI978-22", GameApp.GetInstance().GetGameState().dollor.ToString(), new Color(0.81960785f, 0.5294118f, 2f / 51f, 1f));
			m_AroundUIGroup.Add(uIText);
			UIClickButton control3 = UIUtils.BuildClickButton(5010, new Rect(320f, 588f, 640f, 52f), m_MatDialog01, new Rect(0f, 798f, 640f, 52f), new Rect(0f, 850f, 640f, 52f), new Rect(0f, 798f, 640f, 52f), new Vector2(640f, 52f));
			m_AroundUIGroup.Add(control3);
			playerHpProgressBtn = new UIClickButton();
			playerHpProgressBtn.Id = 5330;
			playerHpProgressBtn.Rect = AutoUIResolution.ToShiftToRight(AutoUI.AutoRect(new Rect(0f, 564f, 101f, 76f)), 2);
			m_AroundUIGroup.Add(playerHpProgressBtn);
			playerHpProgressBarBtnAnim = new UIAnimationControl();
			playerHpProgressBarBtnAnim.Id = 0;
			playerHpProgressBarBtnAnim.SetAnimationsPageCount(3);
			playerHpProgressBarBtnAnim.Rect = AutoUI.AutoRect(new Rect(0f, 564f, 101f, 76f));
			playerHpProgressBarBtnAnim.SetTexture(0, m_MatCommonBg, AutoUI.AutoRect(new Rect(459f, 904f, 101f, 76f)), AutoUI.AutoSize(new Vector2(101f, 76f)));
			playerHpProgressBarBtnAnim.SetTexture(1, m_MatCommonBg, AutoUI.AutoRect(new Rect(563f, 904f, 101f, 76f)), AutoUI.AutoSize(new Vector2(101f, 76f)));
			playerHpProgressBarBtnAnim.SetTexture(2, m_MatCommonBg, AutoUI.AutoRect(new Rect(670f, 904f, 101f, 76f)), AutoUI.AutoSize(new Vector2(101f, 76f)));
			playerHpProgressBarBtnAnim.SetTimeInterval(0.2f);
			playerHpProgressBarBtnAnim.SetLoopCount(1000000);
			playerHpProgressBarBtnAnim.Visible = false;
			playerHpProgressBarBtnAnim.Enable = false;
			m_AroundUIGroup.Add(playerHpProgressBarBtnAnim);
			playerHpProgressBarBtnAnim.Visible = true;
			playerHpProgressBarBtnAnim.Enable = true;
		}
	}

	public void SetupJunjieUI(bool bShow)
	{
		if (m_JunjieUIGroup != null)
		{
			m_JunjieUIGroup.Clear();
			m_JunjieUIGroup = null;
		}
		if (bShow)
		{
			m_JunjieUIGroup = new uiGroup(m_UIManager);
			UIImage control = UIUtils.BuildImage(0, new Rect(50f, 420f, 226f, 150f), m_MatCommonBg, new Rect(798f, 874f, 226f, 150f), new Vector2(226f, 150f));
			m_JunjieUIGroup.Add(control);
			UIText uIText = UIUtils.BuildUIText(0, new Rect(100f, 500f, 150f, 20f), UIText.enAlignStyle.left);
			uIText.Set("Zombie3D/Font/037-CAI978-15", "Hp :   " + (int)GameApp.GetInstance().GetGameState().CalcMaxHp(), new Color(0.81960785f, 0.5294118f, 2f / 51f, 1f));
			m_JunjieUIGroup.Add(uIText);
			UIClickButton control2 = UIUtils.BuildClickButton(5009, new Rect(244f, 576f, 75f, 52f), m_MatCommonBg, new Rect(1020f, 1f, 1f, 1f), new Rect(1020f, 1f, 1f, 1f), new Rect(1020f, 1f, 1f, 1f), new Vector2(75f, 52f));
			m_JunjieUIGroup.Add(control2);
			control2 = UIUtils.BuildClickButton(5009, new Rect(50f, 420f, 226f, 150f), m_MatCommonBg, new Rect(1020f, 1f, 1f, 1f), new Rect(1020f, 1f, 1f, 1f), new Rect(1020f, 1f, 1f, 1f), new Vector2(226f, 150f));
			m_JunjieUIGroup.Add(control2);
		}
	}

	public void SetupChooseMapsUI(bool bShow)
	{
		if (m_MapsGroup != null)
		{
			m_MapsGroup.Clear();
			m_MapsGroup = null;
		}
		if (!bShow)
		{
			return;
		}
		OpenClickPlugin.Show(false);
		m_MapsGroup = new uiGroup(m_UIManager);
		UIViewMoveable uIViewMoveable = new UIViewMoveable(AutoUIResolution.ToShiftToRight(AutoUI.AutoRect(new Rect(0f, 0f, 960f, 640f)), 2), AutoUI.AutoDistance(20f), AutoUI.AutoDistance(20f));
		uIViewMoveable.Rect = AutoUIResolution.ToShiftToRight(AutoUI.AutoRect(new Rect(0f, -260f, 1729f, 900f)), 2);
		uIViewMoveable.SetClip(AutoUIResolution.ToShiftToRight(AutoUI.AutoRect(new Rect(0f, 0f, 960f, 640f)), 2));
		uIViewMoveable.m_MoveDirection = UIViewMoveable.ViewMoveDirection.Horizontal_Vertical;
		uIViewMoveable.m_MoveBound = AutoUIResolution.ToShiftToRight(AutoUI.AutoRect(new Rect(0f, 0f, 960f, 640f)), 2);
		m_MapsGroup.Add(uIViewMoveable);
		UIImage control = UIUtils.BuildImage(0, new Rect(0f, -260f, 1024f, 900f), m_MatMap01UI, new Rect(0f, 0f, 1024f, 900f), new Vector2(1024f, 900f));
		uIViewMoveable.Add(control);
		control = UIUtils.BuildImage(0, new Rect(1024f, -260f, 705f, 900f), m_MatMap02UI, new Rect(0f, 0f, 705f, 900f), new Vector2(705f, 900f));
		uIViewMoveable.Add(control);
		control = UIUtils.BuildImage(0, new Rect(0f, 0f, 960f, 640f), m_MatChoosePoints2UI, new Rect(0f, 0f, 960f, 640f), new Vector2(960f, 640f));
		control.Enable = false;
		m_MapsGroup.Add(control);
		SetupCommonBarUI(m_MapsGroup);
		UIClickButton control2 = UIUtils.BuildClickButton(5011, new Rect(542f, 73f, 150f, 150f), m_MatChoosePointsUI, new Rect(724f, 557f, 150f, 150f), new Rect(724f, 407f, 150f, 150f), new Rect(724f, 557f, 150f, 150f), new Vector2(150f, 150f));
		uIViewMoveable.Add(control2);
		control = UIUtils.BuildImage(0, new Rect(580f, 62f, 68f, 36f), m_MatChoosePointsUI, new Rect(810f, 305f, 68f, 36f), new Vector2(68f, 36f));
		control.CatchMessage = false;
		uIViewMoveable.Add(control);
		Material material = LoadUIMaterial("Zombie3D/UI/Materials/MercAnim");
		m_MercAnim = new UIAnimationControl();
		m_MercAnim.Id = 0;
		m_MercAnim.SetAnimationsPageCount(5);
		m_MercAnim.Rect = AutoUI.AutoRect(new Rect(756f, 202f, 131f, 125f));
		m_MercAnim.SetTexture(0, material, AutoUI.AutoRect(new Rect(0f, 0f, 131f, 125f)), AutoUI.AutoSize(new Vector2(131f, 125f)));
		m_MercAnim.SetTexture(1, material, AutoUI.AutoRect(new Rect(131f, 0f, 131f, 125f)), AutoUI.AutoSize(new Vector2(131f, 125f)));
		m_MercAnim.SetTexture(2, material, AutoUI.AutoRect(new Rect(262f, 0f, 131f, 125f)), AutoUI.AutoSize(new Vector2(131f, 125f)));
		m_MercAnim.SetTexture(3, material, AutoUI.AutoRect(new Rect(0f, 125f, 131f, 125f)), AutoUI.AutoSize(new Vector2(131f, 125f)));
		m_MercAnim.SetTexture(4, material, AutoUI.AutoRect(new Rect(131f, 125f, 131f, 125f)), AutoUI.AutoSize(new Vector2(131f, 125f)));
		m_MercAnim.SetTimeInterval(0.1f);
		m_MercAnim.SetLoopCount(1);
		m_MercAnim.Visible = false;
		m_MercAnim.Enable = false;
		uIViewMoveable.Add(m_MercAnim);
		Rect rcNormal = new Rect(874f, 407f, 150f, 150f);
		Rect rect = new Rect(874f, 557f, 150f, 150f);
		Rect rcNormal2 = new Rect(874f, 707f, 150f, 150f);
		Rect rect2 = new Rect(874f, 857f, 150f, 150f);
		Rect rcNormal3 = new Rect(724f, 707f, 150f, 150f);
		Rect rect3 = new Rect(724f, 857f, 150f, 150f);
		Rect rcNormal4 = new Rect(574f, 857f, 150f, 150f);
		Rect rect4 = new Rect(574f, 707f, 150f, 150f);
		control2 = UIUtils.BuildClickButton(5303, new Rect(119f, 195f, 150f, 150f), m_MatChoosePointsUI, rcNormal, rect, rect, new Vector2(150f, 150f));
		uIViewMoveable.Add(control2);
		Rect rcMat = new Rect(646f, 265f, 184f, 36f);
		control = UIUtils.BuildImage(0, new Rect(100f, 191f, rcMat.width, rcMat.height), m_MatChoosePointsUI, rcMat, new Vector2(rcMat.width, rcMat.height));
		control.CatchMessage = false;
		uIViewMoveable.Add(control);
		bool flag = false;
		if (gameState.GetPlayerLevel() >= 5)
		{
			control2 = UIUtils.BuildClickButton(5304, new Rect(398f, 379f, 150f, 150f), m_MatChoosePointsUI, rcNormal, rect, rect, new Vector2(150f, 150f));
			uIViewMoveable.Add(control2);
		}
		else
		{
			control2 = UIUtils.BuildClickButton(5304, new Rect(398f, 379f, 150f, 150f), m_MatChoosePointsUI, rcNormal2, rect2, rect2, new Vector2(150f, 150f));
			uIViewMoveable.Add(control2);
		}
		Rect rcMat2 = new Rect(833f, 265f, 192f, 36f);
		control = UIUtils.BuildImage(0, new Rect(380f, 374f, rcMat2.width, rcMat2.height), m_MatChoosePointsUI, rcMat2, new Vector2(rcMat2.width, rcMat2.height));
		control.CatchMessage = false;
		uIViewMoveable.Add(control);
		bool flag2 = false;
		if (gameState.GetPlayerLevel() >= 15)
		{
			long mapsCDTime = gameState.GetMapsCDTime(3);
			int mapsCDTimeLength = GetMapsCDTimeLength(3);
			long nowDateSeconds = UtilsEx.getNowDateSeconds();
			bool flag3 = nowDateSeconds - mapsCDTime < mapsCDTimeLength;
			m_Map3CDText = null;
			if (flag3)
			{
				control2 = UIUtils.BuildClickButton(5305, new Rect(217f, 50f, 150f, 150f), m_MatChoosePointsUI, rcNormal4, rect4, rect4, new Vector2(150f, 150f));
				uIViewMoveable.Add(control2);
				long seconds = mapsCDTimeLength - (nowDateSeconds - mapsCDTime);
				string text = UtilsEx.TimeToStr_HMS(seconds);
				m_Map3CDText = UIUtils.BuildUIText(0, new Rect(90f, 10f, 400f, 30f), UIText.enAlignStyle.center);
				m_Map3CDText.Set("Zombie3D/Font/037-CAI978-spec1", text, Color.white);
				uIViewMoveable.Add(m_Map3CDText);
				m_LastMapsCDCheckTime = Time.time;
			}
			else
			{
				control2 = UIUtils.BuildClickButton(5305, new Rect(217f, 50f, 150f, 150f), m_MatChoosePointsUI, rcNormal, rect, rect, new Vector2(150f, 150f));
				uIViewMoveable.Add(control2);
			}
			Rect rcMat3 = new Rect(715f, 347f, 162f, 36f);
			control = UIUtils.BuildImage(0, new Rect(207f, 43f, rcMat3.width, rcMat3.height), m_MatChoosePointsUI, rcMat3, new Vector2(rcMat3.width, rcMat3.height));
			control.CatchMessage = false;
			uIViewMoveable.Add(control);
		}
		else
		{
			control2 = UIUtils.BuildClickButton(5305, new Rect(217f, 50f, 150f, 150f), m_MatChoosePointsUI, rcNormal2, rect2, rect2, new Vector2(150f, 150f));
			uIViewMoveable.Add(control2);
			Rect rcMat4 = new Rect(715f, 347f, 162f, 36f);
			control = UIUtils.BuildImage(0, new Rect(207f, 43f, rcMat4.width, rcMat4.height), m_MatChoosePointsUI, rcMat4, new Vector2(rcMat4.width, rcMat4.height));
			control.CatchMessage = false;
			uIViewMoveable.Add(control);
		}
		bool flag4 = false;
		if (gameState.GetPlayerLevel() >= 15)
		{
			long mapsCDTime2 = gameState.GetMapsCDTime(4);
			int mapsCDTimeLength2 = GetMapsCDTimeLength(4);
			long nowDateSeconds2 = UtilsEx.getNowDateSeconds();
			bool flag5 = nowDateSeconds2 - mapsCDTime2 < mapsCDTimeLength2;
			m_Map4CDText = null;
			if (flag5)
			{
				control2 = UIUtils.BuildClickButton(5306, new Rect(633f, 356f, 150f, 150f), m_MatChoosePointsUI, rcNormal4, rect4, rect4, new Vector2(150f, 150f));
				uIViewMoveable.Add(control2);
				long seconds2 = mapsCDTimeLength2 - (nowDateSeconds2 - mapsCDTime2);
				string text2 = UtilsEx.TimeToStr_HMS(seconds2);
				m_Map4CDText = UIUtils.BuildUIText(0, new Rect(508f, 316f, 400f, 30f), UIText.enAlignStyle.center);
				m_Map4CDText.Set("Zombie3D/Font/037-CAI978-spec1", text2, Color.white);
				uIViewMoveable.Add(m_Map4CDText);
				m_LastMap4MapsCDCheckTime = Time.time;
			}
			else
			{
				control2 = UIUtils.BuildClickButton(5306, new Rect(633f, 356f, 150f, 150f), m_MatChoosePointsUI, rcNormal, rect, rect, new Vector2(150f, 150f));
				uIViewMoveable.Add(control2);
			}
			Rect rcMat5 = new Rect(0f, 787f, 210f, 36f);
			control = UIUtils.BuildImage(0, new Rect(598f, 349f, rcMat5.width, rcMat5.height), m_MatChoosePointsUI, rcMat5, new Vector2(rcMat5.width, rcMat5.height));
			control.CatchMessage = false;
			uIViewMoveable.Add(control);
		}
		else
		{
			control2 = UIUtils.BuildClickButton(5306, new Rect(633f, 356f, 150f, 150f), m_MatChoosePointsUI, rcNormal2, rect2, rect2, new Vector2(150f, 150f));
			uIViewMoveable.Add(control2);
			Rect rcMat6 = new Rect(0f, 787f, 210f, 36f);
			control = UIUtils.BuildImage(0, new Rect(598f, 349f, rcMat6.width, rcMat6.height), m_MatChoosePointsUI, rcMat6, new Vector2(rcMat6.width, rcMat6.height));
			control.CatchMessage = false;
			uIViewMoveable.Add(control);
		}
		bool flag6 = false;
		flag6 = gameState.GetPlayerLevel() >= 1;
		Rect rcMat7 = new Rect(210f, 748f, 258f, 31f);
		Rect scrRect = new Rect(1115f, 378f, 150f, 150f);
		if (flag6)
		{
			long mapsCDTime3 = gameState.GetMapsCDTime(5);
			int mapsCDTimeLength3 = GetMapsCDTimeLength(5);
			long nowDateSeconds3 = UtilsEx.getNowDateSeconds();
			bool flag7 = nowDateSeconds3 - mapsCDTime3 < mapsCDTimeLength3;
			m_Map5CDText = null;
			if (flag7)
			{
				control2 = UIUtils.BuildClickButton(5307, scrRect, m_MatChoosePointsUI, rcNormal4, rect4, rect4, new Vector2(150f, 150f));
				uIViewMoveable.Add(control2);
				long seconds3 = mapsCDTimeLength3 - (nowDateSeconds3 - mapsCDTime3);
				string text3 = UtilsEx.TimeToStr_HMS(seconds3);
				m_Map5CDText = UIUtils.BuildUIText(0, new Rect(scrRect.x + 75f - 200f, scrRect.y - 40f, 400f, 30f), UIText.enAlignStyle.center);
				m_Map5CDText.Set("Zombie3D/Font/037-CAI978-spec1", text3, Color.white);
				uIViewMoveable.Add(m_Map5CDText);
				m_LastMap5MapsCDCheckTime = Time.time;
			}
			else
			{
				control2 = UIUtils.BuildClickButton(5307, scrRect, m_MatChoosePointsUI, rcNormal, rect, rect, new Vector2(150f, 150f));
				uIViewMoveable.Add(control2);
			}
			control = UIUtils.BuildImage(0, new Rect(scrRect.x - 35f, scrRect.y - 7f, rcMat7.width, rcMat7.height), m_MatChoosePointsUI, rcMat7, new Vector2(rcMat7.width, rcMat7.height));
			control.CatchMessage = false;
			uIViewMoveable.Add(control);
		}
		else
		{
			control2 = UIUtils.BuildClickButton(5307, scrRect, m_MatChoosePointsUI, rcNormal, rect, rect, new Vector2(150f, 150f));
			uIViewMoveable.Add(control2);
			control = UIUtils.BuildImage(0, new Rect(scrRect.x - 35f, scrRect.y - 7f, rcMat7.width, rcMat7.height), m_MatChoosePointsUI, rcMat7, new Vector2(rcMat7.width, rcMat7.height));
			control.CatchMessage = false;
			uIViewMoveable.Add(control);
		}
		Rect rcMat8 = new Rect(210f, 787f, 170f, 36f);
		Rect scrRect2 = new Rect(920f, 350f, 150f, 150f);
		control2 = UIUtils.BuildClickButton(5308, scrRect2, m_MatChoosePointsUI, rcNormal, rect, rect, new Vector2(150f, 150f));
		uIViewMoveable.Add(control2);
		control = UIUtils.BuildImage(0, new Rect(scrRect2.x - 35f, scrRect2.y - 7f, rcMat8.width, rcMat8.height), m_MatChoosePointsUI, rcMat8, new Vector2(rcMat8.width, rcMat8.height));
		control.CatchMessage = false;
		uIViewMoveable.Add(control);
		control2 = UIUtils.BuildClickButton(5012, new Rect(746f, 190f, 150f, 150f), m_MatChoosePointsUI, new Rect(575f, 557f, 150f, 150f), new Rect(0f, 599f, 150f, 150f), new Rect(575f, 557f, 150f, 150f), new Vector2(150f, 150f));
		uIViewMoveable.Add(control2);
		control = UIUtils.BuildImage(0, new Rect(756f, 185f, 138f, 36f), m_MatChoosePointsUI, new Rect(398f, 787f, 138f, 36f), new Vector2(138f, 36f));
		control.CatchMessage = false;
		uIViewMoveable.Add(control);
		UIAnimationControl uIAnimationControl = new UIAnimationControl();
		uIAnimationControl.Id = 0;
		uIAnimationControl.SetAnimationsPageCount(4);
		uIAnimationControl.Rect = AutoUI.AutoRect(new Rect(352f, 223f, 300f, 154f));
		uIAnimationControl.SetTexture(0, m_MatChoosePoints2UI, AutoUI.AutoRect(new Rect(2f, 645f, 300f, 154f)), AutoUI.AutoSize(new Vector2(300f, 154f)));
		uIAnimationControl.SetTexture(1, m_MatChoosePoints2UI, AutoUI.AutoRect(new Rect(992f, 20f, 10f, 10f)), AutoUI.AutoSize(new Vector2(300f, 154f)));
		uIAnimationControl.SetTexture(2, m_MatChoosePoints2UI, AutoUI.AutoRect(new Rect(2f, 645f, 300f, 154f)), AutoUI.AutoSize(new Vector2(300f, 154f)));
		uIAnimationControl.SetTexture(3, m_MatChoosePoints2UI, AutoUI.AutoRect(new Rect(992f, 20f, 10f, 10f)), AutoUI.AutoSize(new Vector2(300f, 154f)));
		uIAnimationControl.SetTimeInterval(0.2f);
		uIAnimationControl.SetLoopCount(1000000);
		uIAnimationControl.Visible = false;
		uIAnimationControl.Enable = false;
		uIViewMoveable.Add(uIAnimationControl);
		uIAnimationControl.Visible = true;
		uIAnimationControl.Enable = true;
		control2 = UIUtils.BuildClickButton(5013, new Rect(352f, 223f, 300f, 154f), m_MatChoosePoints2UI, new Rect(608f, 645f, 300f, 154f), new Rect(305f, 645f, 300f, 154f), new Rect(608f, 645f, 300f, 154f), new Vector2(300f, 154f));
		uIViewMoveable.Add(control2);
		control = UIUtils.BuildImage(0, new Rect(912f, 447f, 70f, 40f), m_MatChoosePointsUI, new Rect(656f, 421f, 70f, 40f), new Vector2(70f, 40f));
		control.CatchMessage = false;
		uIViewMoveable.Add(control);
		control2 = UIUtils.BuildClickButton(5297, new Rect(103f, 402f, 131f, 125f), m_MatChoosePointsUI, new Rect(150f, 599f, 131f, 125f), new Rect(281f, 599f, 131f, 125f), new Rect(150f, 599f, 131f, 125f), new Vector2(131f, 125f));
		uIViewMoveable.Add(control2);
		control = UIUtils.BuildImage(0, new Rect(94f, 386f, 172f, 31f), m_MatChoosePointsUI, new Rect(0f, 755f, 172f, 31f), new Vector2(172f, 31f));
		control.CatchMessage = false;
		uIViewMoveable.Add(control);
		control = UIUtils.BuildImage(0, new Rect(90f, 486f, 70f, 40f), m_MatChoosePointsUI, new Rect(656f, 421f, 70f, 40f), new Vector2(70f, 40f));
		control.CatchMessage = false;
		uIViewMoveable.Add(control);
		control = UIUtils.BuildImage(0, new Rect(1111f, 478f, 70f, 40f), m_MatChoosePointsUI, new Rect(656f, 421f, 70f, 40f), new Vector2(70f, 40f));
		control.CatchMessage = false;
		uIViewMoveable.Add(control);
		Rect rcMat9 = new Rect(645f, 305f, 150f, 36f);
		Rect rcMat10 = new Rect(674f, 992f, 90f, 32f);
		control2 = UIUtils.BuildClickButton(5310, new Rect(775f, 186f, 150f, 150f), m_MatChoosePointsUI, rcNormal3, rect3, rect3, new Vector2(150f, 150f));
		control = UIUtils.BuildImage(0, new Rect(775f, 170f, rcMat9.width, rcMat9.height), m_MatChoosePointsUI, rcMat9, new Vector2(rcMat9.width, rcMat9.height));
		control.CatchMessage = false;
		control2 = UIUtils.BuildClickButton(533333, new Rect(1042f, 235f, 150f, 150f), m_MatChoosePointsUI, rcNormal, rect, rect, new Vector2(150f, 150f));
		uIViewMoveable.Add(control2);
		control = UIUtils.BuildImage(0, new Rect(1068f, 232f, rcMat10.width, rcMat10.height), m_MatChoosePointsUI, rcMat10, new Vector2(rcMat10.width, rcMat10.height));
		control.CatchMessage = false;
		uIViewMoveable.Add(control);
		control2 = UIUtils.BuildClickButton(5310, new Rect(1199f, -152f, 150f, 150f), m_MatChoosePointsUI, rcNormal3, rect3, rect3, new Vector2(150f, 150f));
		uIViewMoveable.Add(control2);
		control = UIUtils.BuildImage(0, new Rect(1199f, -168f, rcMat9.width, rcMat9.height), m_MatChoosePointsUI, rcMat9, new Vector2(rcMat9.width, rcMat9.height));
		control.CatchMessage = false;
		uIViewMoveable.Add(control);
		control2 = UIUtils.BuildClickButton(5310, new Rect(1415f, 244f, 150f, 150f), m_MatChoosePointsUI, rcNormal3, rect3, rect3, new Vector2(150f, 150f));
		uIViewMoveable.Add(control2);
		control = UIUtils.BuildImage(0, new Rect(1415f, 228f, rcMat9.width, rcMat9.height), m_MatChoosePointsUI, rcMat9, new Vector2(rcMat9.width, rcMat9.height));
		control.CatchMessage = false;
		uIViewMoveable.Add(control);
		control2 = UIUtils.BuildClickButton(5310, new Rect(1379f, 43f, 150f, 150f), m_MatChoosePointsUI, rcNormal3, rect3, rect3, new Vector2(150f, 150f));
		uIViewMoveable.Add(control2);
		control = UIUtils.BuildImage(0, new Rect(1379f, 27f, rcMat9.width, rcMat9.height), m_MatChoosePointsUI, rcMat9, new Vector2(rcMat9.width, rcMat9.height));
		control.CatchMessage = false;
		uIViewMoveable.Add(control);
	}

	public void SetupChooseMapDetailUI(bool bShow)
	{
		if (m_MapDetailGroup != null)
		{
			m_MapDetailGroup.Clear();
			m_MapDetailGroup = null;
		}
		if (!bShow)
		{
			return;
		}
		OpenClickPlugin.Hide();
		m_MapDetailGroup = new uiGroup(m_UIManager);
		UIImage uIImage = null;
		UIClickButton uIClickButton = null;
		Rect[] array = new Rect[8]
		{
			new Rect(0f, 414f, 483f, 206f),
			new Rect(0f, 0f, 483f, 206f),
			new Rect(0f, 206f, 483f, 206f),
			new Rect(0f, 622f, 483f, 206f),
			new Rect(485f, 0f, 483f, 206f),
			new Rect(485f, 206f, 483f, 206f),
			new Rect(485f, 206f, 483f, 206f),
			new Rect(485f, 414f, 483f, 206f)
		};
		string[] array2 = new string[8] { "?????", "Luxury Heights", "Central Hospital", "Town Square", "Emergency Clinic", "Reactor Control Room", "Nuclear Plant", "Chapel" };
		string[] array3 = new string[8] { "We'll be releasing new stages and missions as quickly as possible. For now, just keep holding the zombie swarms at bay so we can work!", "A new residential area built by Key Biology Corporation in Lakeside, it's only recently been hit by the virus outbreak. Keep an eye out for survivors and kill as many zombies as you can!", "Central Hospital seems to be ground-zero of the outbreak. Unfortunately, you have to go there to secure more supplies. Try to avoid getting infected like everyone else!", "This is a major network base for Lakeside, where you may find more useful info. A large number of zombies come out of nowhere, so be careful. ", "There are so many infected surrounding the clinic that emergency vehicles can't evacuate the area. While clearing the area, you should hunt for the plant-like infected spreading contagious spores.", "If you want to disable the plant's hostile defenses, you're going to have to destroy the four control stations before time runs out!", "The security facility is out of control and it's identified us as intruders. Don't forget, Lakeside's entire security perimeter relies on this plant, so we'll need to keep the zombies away too.", "The chapel was built to be a holy sanctuary, but now serves as a haunt for zombies and other foul mutants" };
		string[] array4 = new string[8]
		{
			"We'll be releasing new stages and missions as quickly as possible. For now, just keep holding the zombie swarms at bay so we can work!",
			string.Empty,
			"Reach level 5 to unlock the Central Hospital stage. Don't take too long, we're in a tight spot!",
			"Reach level 15 to unlock the Town Square stage. Don't take too long, we're in a tight spot!",
			"Reach level 15 to unlock the Emergency Clinic stage. Don't take too long, we're in a tight spot!",
			"Reach level 1 to unlock the Nuclear Plant stage. Don't take too long, we're in a tight spot!",
			"Reach level 1 to unlock the Reactor Control Room stage. Don't take too long, we're in a tight spot!",
			"Reach level 30 to unlock the Chapel stage. Don't take too long, we're in a tight spot!"
		};
		string text = array2[0];
		string text2 = array3[0];
		uIImage = UIUtils.BuildImage(0, new Rect(0f, 0f, 960f, 640f), m_MatChoosePoints1UI, new Rect(1010f, 1f, 1f, 1f), new Vector2(960f, 640f));
		m_MapDetailGroup.Add(uIImage);
		uIImage = UIUtils.BuildImage(0, new Rect(99f, 57f, 771f, 450f), m_MatChoosePoints1UI, new Rect(0f, 0f, 771f, 450f), new Vector2(771f, 450f));
		m_MapDetailGroup.Add(uIImage);
		if (mapIndex > 0)
		{
			text = array2[mapIndex];
			text2 = array3[mapIndex];
			Material mat = LoadUIMaterial("Zombie3D/UI/Materials/MapIcons");
			Resources.UnloadUnusedAssets();
			uIImage = UIUtils.BuildImage(0, new Rect(242f, 203f, 483f, 206f), mat, array[mapIndex], new Vector2(483f, 206f));
			m_MapDetailGroup.Add(uIImage);
			if (mapIndex == 1)
			{
				uIClickButton = UIUtils.BuildClickButton(5315, new Rect(147f, 39f, 194f, 63f), m_MatChoosePoints1UI, new Rect(777f, 194f, 194f, 63f), new Rect(777f, 130f, 194f, 63f), new Rect(777f, 194f, 194f, 63f), new Vector2(194f, 63f));
				m_MapDetailGroup.Add(uIClickButton);
				uIClickButton = UIUtils.BuildClickButton(5316, new Rect(417f, 39f, 194f, 63f), m_MatChoosePoints1UI, new Rect(770f, 575f, 194f, 63f), new Rect(770f, 638f, 194f, 63f), new Rect(770f, 575f, 194f, 63f), new Vector2(194f, 63f));
				m_MapDetailGroup.Add(uIClickButton);
				uIClickButton = UIUtils.BuildClickButton(5314, new Rect(619f, 39f, 194f, 63f), m_MatChoosePoints1UI, new Rect(777f, 65f, 194f, 63f), new Rect(777f, 1f, 194f, 63f), new Rect(777f, 65f, 194f, 63f), new Vector2(194f, 63f));
				m_MapDetailGroup.Add(uIClickButton);
			}
			else if (mapIndex == 2)
			{
				bool flag = false;
				flag = gameState.GetPlayerLevel() >= 5;
				if (!flag)
				{
					text2 = array4[mapIndex];
				}
				if (flag)
				{
					uIClickButton = UIUtils.BuildClickButton(5315, new Rect(147f, 39f, 194f, 63f), m_MatChoosePoints1UI, new Rect(777f, 194f, 194f, 63f), new Rect(777f, 130f, 194f, 63f), new Rect(777f, 194f, 194f, 63f), new Vector2(194f, 63f));
					m_MapDetailGroup.Add(uIClickButton);
					uIClickButton = UIUtils.BuildClickButton(5316, new Rect(417f, 39f, 194f, 63f), m_MatChoosePoints1UI, new Rect(770f, 575f, 194f, 63f), new Rect(770f, 638f, 194f, 63f), new Rect(770f, 575f, 194f, 63f), new Vector2(194f, 63f));
					m_MapDetailGroup.Add(uIClickButton);
					uIClickButton = UIUtils.BuildClickButton(5314, new Rect(619f, 39f, 194f, 63f), m_MatChoosePoints1UI, new Rect(777f, 65f, 194f, 63f), new Rect(777f, 1f, 194f, 63f), new Rect(777f, 65f, 194f, 63f), new Vector2(194f, 63f));
					m_MapDetailGroup.Add(uIClickButton);
				}
				else
				{
					uIClickButton = UIUtils.BuildClickButton(5315, new Rect(619f, 39f, 194f, 63f), m_MatChoosePoints1UI, new Rect(777f, 194f, 194f, 63f), new Rect(777f, 130f, 194f, 63f), new Rect(777f, 194f, 194f, 63f), new Vector2(194f, 63f));
					m_MapDetailGroup.Add(uIClickButton);
				}
			}
			else if (mapIndex == 3)
			{
				bool flag2 = false;
				flag2 = gameState.GetPlayerLevel() >= 15;
				long mapsCDTime = gameState.GetMapsCDTime(3);
				int mapsCDTimeLength = GetMapsCDTimeLength(3);
				long nowDateSeconds = UtilsEx.getNowDateSeconds();
				bool flag3 = mapsCDTime + mapsCDTimeLength > nowDateSeconds;
				Debug.Log(flag3 + "|" + mapsCDTime + "|" + mapsCDTimeLength + "|" + nowDateSeconds);
				if (flag2)
				{
					if (flag3)
					{
						text2 = "The zombies here are gone, you need to wait while they repopulate. You can try another stage or spend 1 tCrystal to instantly repopulate! Do you want to spend 1 tCrystal?";
					}
				}
				else
				{
					text2 = array4[mapIndex];
				}
				if (flag2)
				{
					if (flag3)
					{
						uIClickButton = UIUtils.BuildClickButton(5315, new Rect(417f, 39f, 194f, 63f), m_MatChoosePoints1UI, new Rect(777f, 194f, 194f, 63f), new Rect(777f, 130f, 194f, 63f), new Rect(777f, 194f, 194f, 63f), new Vector2(194f, 63f));
						m_MapDetailGroup.Add(uIClickButton);
						uIClickButton = UIUtils.BuildClickButton(5317, new Rect(619f, 39f, 194f, 63f), m_MatChoosePoints1UI, new Rect(777f, 65f, 194f, 63f), new Rect(777f, 1f, 194f, 63f), new Rect(777f, 65f, 194f, 63f), new Vector2(194f, 63f));
						m_MapDetailGroup.Add(uIClickButton);
					}
					else
					{
						uIClickButton = UIUtils.BuildClickButton(5315, new Rect(417f, 39f, 194f, 63f), m_MatChoosePoints1UI, new Rect(777f, 194f, 194f, 63f), new Rect(777f, 130f, 194f, 63f), new Rect(777f, 194f, 194f, 63f), new Vector2(194f, 63f));
						m_MapDetailGroup.Add(uIClickButton);
						uIClickButton = UIUtils.BuildClickButton(5314, new Rect(619f, 39f, 194f, 63f), m_MatChoosePoints1UI, new Rect(777f, 65f, 194f, 63f), new Rect(777f, 1f, 194f, 63f), new Rect(777f, 65f, 194f, 63f), new Vector2(194f, 63f));
						m_MapDetailGroup.Add(uIClickButton);
					}
				}
				else
				{
					uIClickButton = UIUtils.BuildClickButton(5315, new Rect(619f, 39f, 194f, 63f), m_MatChoosePoints1UI, new Rect(777f, 194f, 194f, 63f), new Rect(777f, 130f, 194f, 63f), new Rect(777f, 194f, 194f, 63f), new Vector2(194f, 63f));
					m_MapDetailGroup.Add(uIClickButton);
				}
			}
			else if (mapIndex == 4)
			{
				bool flag4 = false;
				flag4 = gameState.GetPlayerLevel() >= 15;
				long mapsCDTime2 = gameState.GetMapsCDTime(4);
				int mapsCDTimeLength2 = GetMapsCDTimeLength(4);
				long nowDateSeconds2 = UtilsEx.getNowDateSeconds();
				bool flag5 = mapsCDTime2 + mapsCDTimeLength2 > nowDateSeconds2;
				Debug.Log(flag5 + "|" + mapsCDTime2 + "|" + mapsCDTimeLength2 + "|" + nowDateSeconds2);
				if (flag4)
				{
					if (flag5)
					{
						text2 = "The zombies here are gone, you need to wait while they repopulate. You can try another stage or spend 1 tCrystal to instantly repopulate! Do you want to spend 1 tCrystal?";
					}
				}
				else
				{
					text2 = array4[mapIndex];
				}
				if (flag4)
				{
					if (flag5)
					{
						uIClickButton = UIUtils.BuildClickButton(5315, new Rect(417f, 39f, 194f, 63f), m_MatChoosePoints1UI, new Rect(777f, 194f, 194f, 63f), new Rect(777f, 130f, 194f, 63f), new Rect(777f, 194f, 194f, 63f), new Vector2(194f, 63f));
						m_MapDetailGroup.Add(uIClickButton);
						uIClickButton = UIUtils.BuildClickButton(5318, new Rect(619f, 39f, 194f, 63f), m_MatChoosePoints1UI, new Rect(777f, 65f, 194f, 63f), new Rect(777f, 1f, 194f, 63f), new Rect(777f, 65f, 194f, 63f), new Vector2(194f, 63f));
						m_MapDetailGroup.Add(uIClickButton);
					}
					else
					{
						uIClickButton = UIUtils.BuildClickButton(5315, new Rect(417f, 39f, 194f, 63f), m_MatChoosePoints1UI, new Rect(777f, 194f, 194f, 63f), new Rect(777f, 130f, 194f, 63f), new Rect(777f, 194f, 194f, 63f), new Vector2(194f, 63f));
						m_MapDetailGroup.Add(uIClickButton);
						uIClickButton = UIUtils.BuildClickButton(5314, new Rect(619f, 39f, 194f, 63f), m_MatChoosePoints1UI, new Rect(777f, 65f, 194f, 63f), new Rect(777f, 1f, 194f, 63f), new Rect(777f, 65f, 194f, 63f), new Vector2(194f, 63f));
						m_MapDetailGroup.Add(uIClickButton);
					}
				}
				else
				{
					uIClickButton = UIUtils.BuildClickButton(5315, new Rect(619f, 39f, 194f, 63f), m_MatChoosePoints1UI, new Rect(777f, 194f, 194f, 63f), new Rect(777f, 130f, 194f, 63f), new Rect(777f, 194f, 194f, 63f), new Vector2(194f, 63f));
					m_MapDetailGroup.Add(uIClickButton);
				}
			}
			else if (mapIndex == 5)
			{
				bool flag6 = false;
				flag6 = gameState.GetPlayerLevel() >= 1;
				long mapsCDTime3 = gameState.GetMapsCDTime(5);
				int mapsCDTimeLength3 = GetMapsCDTimeLength(5);
				long nowDateSeconds3 = UtilsEx.getNowDateSeconds();
				bool flag7 = mapsCDTime3 + mapsCDTimeLength3 > nowDateSeconds3;
				if (flag6)
				{
					if (flag7)
					{
						text2 = "The zombies here are gone, you need to wait while they repopulate. You can try another stage or spend 1 tCrystal to instantly repopulate! Do you want to spend 1 tCrystal?";
					}
				}
				else
				{
					text2 = array4[mapIndex];
				}
				if (flag6)
				{
					if (flag7)
					{
						uIClickButton = UIUtils.BuildClickButton(5315, new Rect(417f, 39f, 194f, 63f), m_MatChoosePoints1UI, new Rect(777f, 194f, 194f, 63f), new Rect(777f, 130f, 194f, 63f), new Rect(777f, 194f, 194f, 63f), new Vector2(194f, 63f));
						m_MapDetailGroup.Add(uIClickButton);
						uIClickButton = UIUtils.BuildClickButton(5319, new Rect(619f, 39f, 194f, 63f), m_MatChoosePoints1UI, new Rect(777f, 65f, 194f, 63f), new Rect(777f, 1f, 194f, 63f), new Rect(777f, 65f, 194f, 63f), new Vector2(194f, 63f));
						m_MapDetailGroup.Add(uIClickButton);
					}
					else
					{
						uIClickButton = UIUtils.BuildClickButton(5315, new Rect(417f, 39f, 194f, 63f), m_MatChoosePoints1UI, new Rect(777f, 194f, 194f, 63f), new Rect(777f, 130f, 194f, 63f), new Rect(777f, 194f, 194f, 63f), new Vector2(194f, 63f));
						m_MapDetailGroup.Add(uIClickButton);
						uIClickButton = UIUtils.BuildClickButton(5314, new Rect(619f, 39f, 194f, 63f), m_MatChoosePoints1UI, new Rect(777f, 65f, 194f, 63f), new Rect(777f, 1f, 194f, 63f), new Rect(777f, 65f, 194f, 63f), new Vector2(194f, 63f));
						m_MapDetailGroup.Add(uIClickButton);
					}
				}
				else
				{
					uIClickButton = UIUtils.BuildClickButton(5315, new Rect(619f, 39f, 194f, 63f), m_MatChoosePoints1UI, new Rect(777f, 194f, 194f, 63f), new Rect(777f, 130f, 194f, 63f), new Rect(777f, 194f, 194f, 63f), new Vector2(194f, 63f));
					m_MapDetailGroup.Add(uIClickButton);
				}
			}
			else if (mapIndex == 6)
			{
				uIClickButton = UIUtils.BuildClickButton(5315, new Rect(147f, 39f, 194f, 63f), m_MatChoosePoints1UI, new Rect(777f, 194f, 194f, 63f), new Rect(777f, 130f, 194f, 63f), new Rect(777f, 194f, 194f, 63f), new Vector2(194f, 63f));
				m_MapDetailGroup.Add(uIClickButton);
				uIClickButton = UIUtils.BuildClickButton(5316, new Rect(417f, 39f, 194f, 63f), m_MatChoosePoints1UI, new Rect(770f, 575f, 194f, 63f), new Rect(770f, 638f, 194f, 63f), new Rect(770f, 575f, 194f, 63f), new Vector2(194f, 63f));
				m_MapDetailGroup.Add(uIClickButton);
				uIClickButton = UIUtils.BuildClickButton(5314, new Rect(619f, 39f, 194f, 63f), m_MatChoosePoints1UI, new Rect(777f, 65f, 194f, 63f), new Rect(777f, 1f, 194f, 63f), new Rect(777f, 65f, 194f, 63f), new Vector2(194f, 63f));
				m_MapDetailGroup.Add(uIClickButton);
			}
			else if (mapIndex == 7)
			{
				bool flag = false;
				flag = gameState.GetPlayerLevel() >= 30;
				if (!flag)
				{
					text2 = array4[mapIndex];
				}
				if (flag)
				{
					uIClickButton = UIUtils.BuildClickButton(5315, new Rect(147f, 39f, 194f, 63f), m_MatChoosePoints1UI, new Rect(777f, 194f, 194f, 63f), new Rect(777f, 130f, 194f, 63f), new Rect(777f, 194f, 194f, 63f), new Vector2(194f, 63f));
					m_MapDetailGroup.Add(uIClickButton);
					uIClickButton = UIUtils.BuildClickButton(5316, new Rect(417f, 39f, 194f, 63f), m_MatChoosePoints1UI, new Rect(770f, 575f, 194f, 63f), new Rect(770f, 638f, 194f, 63f), new Rect(770f, 575f, 194f, 63f), new Vector2(194f, 63f));
					m_MapDetailGroup.Add(uIClickButton);
					uIClickButton = UIUtils.BuildClickButton(5314, new Rect(619f, 39f, 194f, 63f), m_MatChoosePoints1UI, new Rect(777f, 65f, 194f, 63f), new Rect(777f, 1f, 194f, 63f), new Rect(777f, 65f, 194f, 63f), new Vector2(194f, 63f));
					m_MapDetailGroup.Add(uIClickButton);
				}
				else
				{
					uIClickButton = UIUtils.BuildClickButton(5315, new Rect(619f, 39f, 194f, 63f), m_MatChoosePoints1UI, new Rect(777f, 194f, 194f, 63f), new Rect(777f, 130f, 194f, 63f), new Rect(777f, 194f, 194f, 63f), new Vector2(194f, 63f));
					m_MapDetailGroup.Add(uIClickButton);
				}
			}
		}
		else
		{
			Material mat2 = LoadUIMaterial("Zombie3D/UI/Materials/MapIcons");
			Resources.UnloadUnusedAssets();
			uIImage = UIUtils.BuildImage(0, new Rect(242f, 203f, 483f, 206f), mat2, array[0], new Vector2(483f, 206f));
			m_MapDetailGroup.Add(uIImage);
			uIClickButton = UIUtils.BuildClickButton(5315, new Rect(619f, 39f, 194f, 63f), m_MatChoosePoints1UI, new Rect(777f, 194f, 194f, 63f), new Rect(777f, 130f, 194f, 63f), new Rect(777f, 194f, 194f, 63f), new Vector2(194f, 63f));
			m_MapDetailGroup.Add(uIClickButton);
		}
		UIText uIText = UIUtils.BuildUIText(0, new Rect(284f, 455f, 400f, 30f), UIText.enAlignStyle.center);
		uIText.Set("Zombie3D/Font/037-CAI978-22", text, new Color(69f / 85f, 0.5254902f, 0.09019608f, 1f));
		m_MapDetailGroup.Add(uIText);
		float top = 125f;
		if (text2.Length < 100)
		{
			top = 105f;
		}
		uIText = UIUtils.BuildUIText(0, new Rect(210f, top, 560f, 80f), UIText.enAlignStyle.left);
		uIText.Set("Zombie3D/Font/037-CAI978-18", text2, new Color(69f / 85f, 0.5254902f, 0.09019608f, 1f));
		m_MapDetailGroup.Add(uIText);
	}

	public void SetupChoosePointsUI(bool bShow, bool bIsTopHalf = true)
	{
		if (m_uiGroup != null)
		{
			m_uiGroup.Clear();
			m_uiGroup = null;
		}
		if (!bShow)
		{
			return;
		}
		OpenClickPlugin.Hide();
		m_uiGroup = new uiGroup(m_UIManager);
		UIBlock uIBlock = new UIBlock();
		uIBlock.Rect = new Rect(0f, 0f, 960f, 640f);
		m_uiGroup.Add(uIBlock);
		UIImage control = UIUtils.BuildImage(0, new Rect(0f, 0f, 960f, 640f), m_MatCommonBg, new Rect(963f, 575f, 1f, 1f), new Vector2(960f, 640f));
		m_uiGroup.Add(control);
		control = UIUtils.BuildImage(0, new Rect(160f, 132f, 646f, 373f), m_MatChoosePointsUI, new Rect(0f, 0f, 646f, 373f), new Vector2(646f, 373f));
		m_uiGroup.Add(control);
		int maxPointsIndex = 0;
		int maxWaveIndex = 1;
		gameState.GetMaxMapCfg(mapIndex, ref maxPointsIndex, ref maxWaveIndex, bIsTopHalf);
		m_ChoosePointsViewScrollBar = new UIDotScrollBar();
		m_ChoosePointsView = new UIScrollPageView();
		m_ChoosePointsView.SetMoveParam(AutoUI.AutoRect(new Rect(160f, 132f, 646f, 373f)), AutoUI.AutoDistance(20f), AutoUI.AutoDistance(20f));
		m_ChoosePointsView.Rect = AutoUI.AutoRect(new Rect(160f, 132f, 646f, 373f));
		m_ChoosePointsView.ListOri = UIScrollPageView.ListOrientation.Horizontal;
		m_ChoosePointsView.ViewSize = AutoUI.AutoSize(new Vector2(625f, 203f));
		m_ChoosePointsView.ItemSpacingV = AutoUI.AutoDistance(50f);
		m_ChoosePointsView.ItemSpacingH = AutoUI.AutoDistance(50f);
		m_ChoosePointsView.SetClip(AutoUI.AutoRect(new Rect(170f, 132f, 618f, 330f)));
		m_ChoosePointsView.Bounds = AutoUI.AutoRect(new Rect(160f, 132f, 638f, 330f));
		m_ChoosePointsView.ScrollBar = m_ChoosePointsViewScrollBar;
		m_uiGroup.Add(m_ChoosePointsView);
		Rect[] array = new Rect[10]
		{
			new Rect(0f, 420f, 26f, 56f),
			new Rect(30f, 420f, 36f, 56f),
			new Rect(83f, 420f, 36f, 56f),
			new Rect(131f, 420f, 43f, 56f),
			new Rect(187f, 420f, 36f, 56f),
			new Rect(239f, 420f, 42f, 56f),
			new Rect(294f, 420f, 42f, 56f),
			new Rect(346f, 420f, 42f, 56f),
			new Rect(400f, 420f, 42f, 56f),
			new Rect(447f, 420f, 60f, 56f)
		};
		UIClickButton uIClickButton = null;
		int num = 50;
		int num2 = 0;
		num = ConfigManager.Instance().GetFixedConfig().GetMaxPointsOfMap(mapIndex);
		if (bIsTopHalf)
		{
			num = 50;
			num2 = 0;
		}
		else
		{
			num = 20;
			num2 = 50;
		}
		int num3 = 10;
		for (int i = 0; i < num / num3; i++)
		{
			UIGroupControl uIGroupControl = UIUtils.BuildUIGroupControl(0, new Rect(0f, 0f, 575f, 203f));
			for (int j = 0; j < num3; j++)
			{
				int num4 = i * num3 + j + num2;
				float num5 = 30 + j % 5 * 115 + 2;
				float num6 = -55 + (j / 5 + 1) % 2 * 115;
				string text = (num4 + 1).ToString();
				float textWidth = mgrFont.Instance().getFont("Zombie3D/Font/037-CAI978").GetTextWidth(text);
				UIText uIText = null;
				if (gameState.GetBattleStarByMapPointIndex(mapIndex, num4) >= 0)
				{
					if (num4 == maxPointsIndex - 1)
					{
						uIClickButton = UIUtils.BuildClickButton(5332 + num4, new Rect(num5, num6, 115f, 88f), m_MatChoosePointsUI, new Rect(646f, 109f, 115f, 88f), new Rect(906f, 319f, 115f, 88f), new Rect(646f, 109f, 115f, 88f), new Vector2(115f, 88f));
						uIGroupControl.Add(uIClickButton);
						uIText = UIUtils.BuildUIText(0, new Rect(num5 + 50f - textWidth / 2f, num6 + 48f - 20f, 100f, 40f), UIText.enAlignStyle.left);
						if (AutoUI.IsRetain == AutoUI.RESOLUTION.LOWDEFINITION)
						{
							uIText.Set("Zombie3D/Font/037-CAI978-1", text, Color.red);
						}
						else
						{
							uIText.Set("Zombie3D/Font/037-CAI978", text, Color.red);
						}
						uIGroupControl.Add(uIText);
					}
					else
					{
						uIClickButton = UIUtils.BuildClickButton(5332 + num4, new Rect(num5, num6, 115f, 88f), m_MatChoosePointsUI, new Rect(876f, 109f, 115f, 88f), new Rect(885f, 0f, 115f, 88f), new Rect(876f, 109f, 115f, 88f), new Vector2(115f, 88f));
						uIGroupControl.Add(uIClickButton);
						uIText = UIUtils.BuildUIText(0, new Rect(num5 + 56f - textWidth / 2f, num6 + 48f - 20f, 100f, 40f), UIText.enAlignStyle.left);
						if (AutoUI.IsRetain == AutoUI.RESOLUTION.LOWDEFINITION)
						{
							uIText.Set("Zombie3D/Font/037-CAI978-1", text, Color.white);
						}
						else
						{
							uIText.Set("Zombie3D/Font/037-CAI978", text, Color.white);
						}
						uIGroupControl.Add(uIText);
					}
					int battleStarByMapPointIndex = gameState.GetBattleStarByMapPointIndex(mapIndex, num4);
					Rect[] array2 = new Rect[3]
					{
						new Rect(518f, 420f, 93f, 56f),
						new Rect(483f, 476f, 93f, 56f),
						new Rect(575f, 476f, 93f, 56f)
					};
					if (battleStarByMapPointIndex > 0 && battleStarByMapPointIndex <= 3)
					{
						control = UIUtils.BuildImage(0, new Rect(num5 + 5f, num6 - 20f, 93f, 56f), m_MatChoosePointsUI, array2[battleStarByMapPointIndex - 1], new Vector2(93f, 56f));
						control.CatchMessage = false;
						uIGroupControl.Add(control);
					}
					else
					{
						Debug.Log("Star Is Out Of (0,3]");
					}
				}
				else
				{
					control = UIUtils.BuildImage(0, new Rect(num5, num6, 115f, 88f), m_MatChoosePointsUI, new Rect(761f, 109f, 115f, 88f), new Vector2(115f, 88f));
					uIGroupControl.Add(control);
				}
			}
			m_ChoosePointsView.Add(uIGroupControl);
		}
		m_ChoosePointsViewScrollBar.Rect = AutoUI.AutoRect(new Rect(480 - num / num3 * 30 / 2, 160f, 400f, 20f));
		m_ChoosePointsViewScrollBar.ScrollOri = UIDotScrollBar.ScrollOrientation.Horizontal;
		m_ChoosePointsViewScrollBar.DotPageWidth = AutoUI.AutoDistance(30f);
		m_ChoosePointsViewScrollBar.SetPageCount(num / num3);
		m_ChoosePointsViewScrollBar.SetScrollBarTexture(m_MatChoosePointsUI, AutoUI.AutoRect(new Rect(1000f, 9f, 11f, 11f)), m_MatChoosePointsUI, AutoUI.AutoRect(new Rect(1012f, 9f, 11f, 11f)));
		m_ChoosePointsViewScrollBar.SetScrollPercent(0f);
		m_uiGroup.Add(m_ChoosePointsViewScrollBar);
		uIClickButton = UIUtils.BuildClickButton(5312, new Rect(55f, 15f, 187f, 68f), m_MatChoosePointsUI, new Rect(647f, 198f, 187f, 68f), new Rect(835f, 198f, 187f, 68f), new Rect(647f, 198f, 187f, 68f), new Vector2(187f, 68f));
		m_uiGroup.Add(uIClickButton);
	}

	public void SetupChoosePointsUI_Boss(bool bShow)
	{
		if (m_uiGroup != null)
		{
			m_uiGroup.Clear();
			m_uiGroup = null;
		}
		if (!bShow)
		{
			return;
		}
		OpenClickPlugin.Hide();
		m_uiGroup = new uiGroup(m_UIManager);
		UIBlock uIBlock = new UIBlock();
		uIBlock.Rect = new Rect(0f, 0f, 960f, 640f);
		m_uiGroup.Add(uIBlock);
		UIImage control = UIUtils.BuildImage(0, new Rect(0f, 0f, 960f, 640f), m_MatCommonBg, new Rect(963f, 575f, 1f, 1f), new Vector2(960f, 640f));
		m_uiGroup.Add(control);
		control = UIUtils.BuildImage(0, new Rect(99f, 107f, 771f, 450f), m_MatChoosePoints1UI, new Rect(0f, 0f, 771f, 450f), new Vector2(771f, 450f));
		m_uiGroup.Add(control);
		UIText uIText = UIUtils.BuildUIText(0, new Rect(210f, 135f, 560f, 80f), UIText.enAlignStyle.center);
		uIText.Set("Zombie3D/Font/037-CAI978-18", "Take on elite bosses for epic rewards!", new Color(69f / 85f, 0.5254902f, 0.09019608f, 1f));
		m_uiGroup.Add(uIText);
		uIText = UIUtils.BuildUIText(0, new Rect(395f, 455f, 560f, 80f), UIText.enAlignStyle.left);
		uIText.Set("Zombie3D/Font/037-CAI978-27", "DIFFICULTY", new Color(69f / 85f, 0.5254902f, 0.09019608f, 1f));
		m_uiGroup.Add(uIText);
		int maxPointsIndex = 1;
		int maxWaveIndex = 1;
		gameState.GetMaxMapCfg(mapIndex, ref maxPointsIndex, ref maxWaveIndex);
		Rect[] array = new Rect[10]
		{
			new Rect(0f, 420f, 26f, 56f),
			new Rect(30f, 420f, 36f, 56f),
			new Rect(83f, 420f, 36f, 56f),
			new Rect(131f, 420f, 43f, 56f),
			new Rect(187f, 420f, 36f, 56f),
			new Rect(239f, 420f, 42f, 56f),
			new Rect(294f, 420f, 42f, 56f),
			new Rect(346f, 420f, 42f, 56f),
			new Rect(400f, 420f, 42f, 56f),
			new Rect(447f, 420f, 60f, 56f)
		};
		UIClickButton uIClickButton = null;
		int num = 10;
		if (mapIndex == 3)
		{
			num = 2;
		}
		else if (mapIndex == 4)
		{
			num = 2;
		}
		else if (mapIndex == 5)
		{
			num = 2;
		}
		for (int i = 0; i < num; i++)
		{
			switch (i)
			{
			case 0:
				uIClickButton = UIUtils.BuildClickButton(5332 + i, new Rect(199f, 270f, 266f, 186f), m_MatMap02UI, new Rect(757f, 0f, 266f, 186f), new Rect(757f, 186f, 266f, 186f), new Rect(757f, 0f, 266f, 186f), new Vector2(266f, 186f));
				m_uiGroup.Add(uIClickButton);
				break;
			case 1:
				uIClickButton = UIUtils.BuildClickButton(5332 + i, new Rect(495f, 270f, 266f, 186f), m_MatMap02UI, new Rect(757f, 372f, 266f, 186f), new Rect(757f, 558f, 266f, 186f), new Rect(757f, 372f, 266f, 186f), new Vector2(266f, 186f));
				m_uiGroup.Add(uIClickButton);
				break;
			default:
				Debug.Log("mapMaxPoints out of max point");
				break;
			}
		}
		uIClickButton = UIUtils.BuildClickButton(5312, new Rect(55f, 15f, 187f, 68f), m_MatChoosePointsUI, new Rect(647f, 198f, 187f, 68f), new Rect(835f, 198f, 187f, 68f), new Rect(647f, 198f, 187f, 68f), new Vector2(187f, 68f));
		m_uiGroup.Add(uIClickButton);
	}

	public void SetupChooseWaveUI(bool bShow)
	{
		if (!bShow)
		{
			m_UIManager.Remove(m_ChooseWaveGroup);
			m_ChooseWaveGroup = null;
			return;
		}
		m_ChooseWaveGroup = UIUtils.BuildUIGroupControl(0, new Rect(0f, 0f, 960f, 640f));
		m_UIManager.Add(m_ChooseWaveGroup);
		UIBlock uIBlock = new UIBlock();
		uIBlock.Rect = new Rect(0f, 0f, 960f, 640f);
		m_ChooseWaveGroup.Add(uIBlock);
		UIImage control = UIUtils.BuildImage(0, new Rect(160f, 132f, 646f, 373f), m_MatChoosePointsUI, new Rect(0f, 0f, 646f, 373f), new Vector2(646f, 373f));
		m_ChooseWaveGroup.Add(control);
		UIDotScrollBar uIDotScrollBar = new UIDotScrollBar();
		int maxPointsIndex = 1;
		int maxWaveIndex = 1;
		gameState.GetMaxMapCfg(mapIndex, ref maxPointsIndex, ref maxWaveIndex);
		if (pointsIndex < maxPointsIndex)
		{
			maxWaveIndex = 50;
		}
		UIScrollPageView uIScrollPageView = new UIScrollPageView();
		uIScrollPageView.SetMoveParam(new Rect(160f, 132f, 646f, 373f), AutoUI.AutoDistance(20f), AutoUI.AutoDistance(20f));
		uIScrollPageView.Rect = AutoUI.AutoRect(new Rect(160f, 132f, 646f, 373f));
		uIScrollPageView.ListOri = UIScrollPageView.ListOrientation.Horizontal;
		uIScrollPageView.ViewSize = AutoUI.AutoSize(new Vector2(625f, 203f));
		uIScrollPageView.ItemSpacingV = AutoUI.AutoDistance(50f);
		uIScrollPageView.ItemSpacingH = AutoUI.AutoDistance(50f);
		uIScrollPageView.SetClip(AutoUI.AutoRect(new Rect(170f, 132f, 618f, 330f)));
		uIScrollPageView.Bounds = AutoUI.AutoRect(new Rect(160f, 132f, 638f, 330f));
		uIScrollPageView.ScrollBar = uIDotScrollBar;
		m_ChooseWaveGroup.Add(uIScrollPageView);
		UIClickButton uIClickButton = null;
		int maxWavesOfPoints = ConfigManager.Instance().GetFixedConfig().GetMaxWavesOfPoints(mapIndex, pointsIndex);
		int num = 10;
		for (int i = 0; i < maxWavesOfPoints / num; i++)
		{
			UIGroupControl uIGroupControl = UIUtils.BuildUIGroupControl(0, new Rect(0f, 0f, 575f, 203f));
			for (int j = 0; j < num; j++)
			{
				int num2 = i * num + j;
				float num3 = 30 + j % 5 * 115 + 2;
				float num4 = -55 + (j / 5 + 1) % 2 * 115;
				string text = (num2 + 1).ToString();
				float textWidth = mgrFont.Instance().getFont("Zombie3D/Font/037-CAI978").GetTextWidth(text);
				UIText uIText = null;
				if (num2 < maxWaveIndex)
				{
					if (num2 == maxWaveIndex - 1 && num2 != maxWavesOfPoints - 1)
					{
						uIClickButton = UIUtils.BuildClickButton(5433 + num2, new Rect(num3, num4, 115f, 88f), m_MatChoosePointsUI, new Rect(646f, 109f, 115f, 88f), new Rect(906f, 319f, 115f, 88f), new Rect(646f, 109f, 115f, 88f), new Vector2(115f, 88f));
						uIGroupControl.Add(uIClickButton);
						uIText = UIUtils.BuildUIText(0, new Rect(num3 + 50f - textWidth / 2f, num4 + 48f - 20f, 100f, 40f), UIText.enAlignStyle.left);
						if (AutoUI.IsRetain == AutoUI.RESOLUTION.LOWDEFINITION)
						{
							uIText.Set("Zombie3D/Font/037-CAI978-1", text, Color.red);
						}
						else
						{
							uIText.Set("Zombie3D/Font/037-CAI978", text, Color.red);
						}
						uIGroupControl.Add(uIText);
					}
					else
					{
						uIClickButton = UIUtils.BuildClickButton(5433 + num2, new Rect(num3, num4, 115f, 88f), m_MatChoosePointsUI, new Rect(876f, 109f, 115f, 88f), new Rect(885f, 0f, 115f, 88f), new Rect(876f, 109f, 115f, 88f), new Vector2(115f, 88f));
						uIGroupControl.Add(uIClickButton);
						uIText = UIUtils.BuildUIText(0, new Rect(num3 + 56f - textWidth / 2f, num4 + 48f - 20f, 100f, 40f), UIText.enAlignStyle.left);
						if (AutoUI.IsRetain == AutoUI.RESOLUTION.LOWDEFINITION)
						{
							uIText.Set("Zombie3D/Font/037-CAI978-1", text, Color.white);
						}
						else
						{
							uIText.Set("Zombie3D/Font/037-CAI978", text, Color.white);
						}
						uIGroupControl.Add(uIText);
					}
					int battleStarByMapPointIndex = gameState.GetBattleStarByMapPointIndex(mapIndex, num2);
					Rect[] array = new Rect[3]
					{
						new Rect(518f, 420f, 93f, 56f),
						new Rect(483f, 476f, 93f, 56f),
						new Rect(575f, 476f, 93f, 56f)
					};
					if (battleStarByMapPointIndex > 0 && battleStarByMapPointIndex <= 3)
					{
						control = UIUtils.BuildImage(0, new Rect(num3 + 5f, num4 - 20f, 93f, 56f), m_MatChoosePointsUI, array[battleStarByMapPointIndex - 1], new Vector2(93f, 56f));
						uIGroupControl.Add(control);
					}
					else
					{
						Debug.Log("Star Is Out Of (0,3]");
					}
				}
				else
				{
					control = UIUtils.BuildImage(0, new Rect(num3, num4, 115f, 88f), m_MatChoosePointsUI, new Rect(761f, 109f, 115f, 88f), new Vector2(115f, 88f));
					uIGroupControl.Add(control);
				}
			}
			uIScrollPageView.Add(uIGroupControl);
		}
		uIDotScrollBar.Rect = AutoUI.AutoRect(new Rect(480f - (float)(maxWavesOfPoints / num) * AutoUI.AutoDistance(30f) / 2f, 170f, 400f, 20f));
		uIDotScrollBar.ScrollOri = UIDotScrollBar.ScrollOrientation.Horizontal;
		uIDotScrollBar.DotPageWidth = AutoUI.AutoDistance(30f);
		uIDotScrollBar.SetPageCount(maxWavesOfPoints / num);
		uIDotScrollBar.SetScrollBarTexture(m_MatChoosePointsUI, AutoUI.AutoRect(new Rect(1000f, 9f, 11f, 11f)), m_MatChoosePointsUI, AutoUI.AutoRect(new Rect(1012f, 9f, 11f, 11f)));
		uIDotScrollBar.SetScrollPercent(0f);
		m_ChooseWaveGroup.Add(uIDotScrollBar);
		uIClickButton = UIUtils.BuildClickButton(5313, new Rect(55f, 15f, 187f, 68f), m_MatChoosePointsUI, new Rect(647f, 198f, 187f, 68f), new Rect(835f, 198f, 187f, 68f), new Rect(647f, 198f, 187f, 68f), new Vector2(187f, 68f));
		m_uiGroup.Add(uIClickButton);
	}

	public void SetupNewVersionFeatureDialogUI(bool bShow, string context)
	{
		if (m_DialogUI != null)
		{
			m_DialogUI.Clear();
			m_DialogUI = null;
		}
		if (bShow)
		{
			m_DialogUI = new uiGroup(m_UIManager);
			UIBlock uIBlock = new UIBlock();
			uIBlock.Rect = new Rect(0f, 0f, 960f, 640f);
			m_DialogUI.Add(uIBlock);
			UIImage control = UIUtils.BuildImage(0, new Rect(0f, 0f, 960f, 640f), m_MatCommonBg, new Rect(1016f, 1f, 1f, 1f), new Vector2(960f, 640f));
			m_DialogUI.Add(control);
			control = UIUtils.BuildImage(0, new Rect(242f, 232f, 515f, 301f), m_MatDialog01, new Rect(0f, 378f, 515f, 301f), new Vector2(515f, 301f));
			m_DialogUI.Add(control);
			float top = 420f;
			if (context.Length < 50)
			{
				top = 350f;
			}
			UIText uIText = UIUtils.BuildUIText(0, new Rect(333f, top, 340f, 30f), UIText.enAlignStyle.left);
			uIText.Set("Zombie3D/Font/037-CAI978-18", context, Constant.TextCommonColor);
			m_DialogUI.Add(uIText);
			UIClickButton control2 = UIUtils.BuildClickButton(5324, new Rect(410f, 216f, 191f, 62f), m_MatDialog01, new Rect(640f, 0f, 191f, 62f), new Rect(832f, 0f, 191f, 62f), new Rect(640f, 0f, 191f, 62f), new Vector2(191f, 62f));
			m_DialogUI.Add(control2);
		}
	}

	public void SetupNewVersionFeatureDialog02UI(bool bShow)
	{
		if (m_DialogUI != null)
		{
			m_DialogUI.Clear();
			m_DialogUI = null;
		}
		if (bShow)
		{
			m_DialogUI = new uiGroup(m_UIManager);
			UIBlock uIBlock = new UIBlock();
			uIBlock.Rect = new Rect(0f, 0f, 960f, 640f);
			m_DialogUI.Add(uIBlock);
			UIImage control = UIUtils.BuildImage(0, new Rect(0f, 0f, 960f, 640f), m_MatCommonBg, new Rect(1016f, 1f, 1f, 1f), new Vector2(960f, 640f));
			m_DialogUI.Add(control);
			control = UIUtils.BuildImage(0, new Rect(236f, 173f, 515f, 301f), m_MatDialog01, new Rect(0f, 378f, 515f, 301f), new Vector2(515f, 301f));
			m_DialogUI.Add(control);
			control = UIUtils.BuildImage(0, new Rect(258f, 268f, 478f, 121f), m_MatChoosePointsUI, new Rect(0f, 478f, 478f, 121f), new Vector2(478f, 121f));
			m_DialogUI.Add(control);
			UIText uIText = UIUtils.BuildUIText(0, new Rect(340f, 217f, 308f, 50f), UIText.enAlignStyle.left);
			uIText.Set("Zombie3D/Font/037-CAI978-18", "Daily login rewards are now EVEN BETTER!", Constant.TextCommonColor);
			m_DialogUI.Add(uIText);
			UIClickButton control2 = UIUtils.BuildClickButton(5325, new Rect(393f, 149f, 191f, 62f), m_MatDialog01, new Rect(640f, 0f, 191f, 62f), new Rect(832f, 0f, 191f, 62f), new Rect(640f, 0f, 191f, 62f), new Vector2(191f, 62f));
			m_DialogUI.Add(control2);
		}
	}

	public void SetupNewVersionFeatureDialog_1_3_UI(bool bShow)
	{
		if (m_DialogUI != null)
		{
			m_DialogUI.Clear();
			m_DialogUI = null;
		}
		if (bShow)
		{
			m_DialogUI = new uiGroup(m_UIManager);
			UIBlock uIBlock = new UIBlock();
			uIBlock.Rect = new Rect(0f, 0f, 960f, 640f);
			m_DialogUI.Add(uIBlock);
			UIImage control = UIUtils.BuildImage(0, new Rect(0f, 0f, 960f, 640f), m_MatCommonBg, new Rect(1016f, 1f, 1f, 1f), new Vector2(960f, 640f));
			m_DialogUI.Add(control);
			control = UIUtils.BuildImage(0, new Rect(236f, 173f, 515f, 301f), m_MatDialog01, new Rect(0f, 378f, 515f, 301f), new Vector2(515f, 301f));
			m_DialogUI.Add(control);
			UIText uIText = UIUtils.BuildUIText(0, new Rect(300f, 350f, 368f, 25f), UIText.enAlignStyle.left);
			uIText.Set("Zombie3D/Font/037-CAI978-18", "Check out the new Merc Camp to hire other player's characters as your sidekick!", Constant.TextCommonColor);
			m_DialogUI.Add(uIText);
			UIAnimationControl uIAnimationControl = new UIAnimationControl();
			uIAnimationControl.Id = 0;
			uIAnimationControl.SetAnimationsPageCount(4);
			uIAnimationControl.Rect = AutoUI.AutoRect(new Rect(393f, 149f, 191f, 62f));
			uIAnimationControl.SetTexture(0, m_MatChoosePoints1UI, AutoUI.AutoRect(new Rect(833f, 962f, 191f, 62f)), AutoUI.AutoSize(new Vector2(191f, 62f)));
			uIAnimationControl.SetTexture(1, m_MatChoosePoints1UI, AutoUI.AutoRect(new Rect(833f, 898f, 191f, 62f)), AutoUI.AutoSize(new Vector2(191f, 62f)));
			uIAnimationControl.SetTexture(2, m_MatChoosePoints1UI, AutoUI.AutoRect(new Rect(833f, 834f, 191f, 62f)), AutoUI.AutoSize(new Vector2(191f, 62f)));
			uIAnimationControl.SetTexture(3, m_MatChoosePoints1UI, AutoUI.AutoRect(new Rect(833f, 770f, 191f, 62f)), AutoUI.AutoSize(new Vector2(191f, 62f)));
			uIAnimationControl.SetTimeInterval(0.1f);
			uIAnimationControl.SetLoopCount(10000000);
			m_DialogUI.Add(uIAnimationControl);
			UIClickButton control2 = UIUtils.BuildClickButton(5326, new Rect(393f, 149f, 191f, 62f), m_MatChoosePoints1UI, new Rect(1f, 1f, 1f, 1f), new Rect(771f, 388f, 191f, 62f), new Rect(1f, 1f, 1f, 1f), new Vector2(191f, 62f));
			m_DialogUI.Add(control2);
		}
	}

	public void SetupSendGiftGoldDollor_1_3_2(bool bShow)
	{
		if (m_SendGoldDollorUI != null)
		{
			m_SendGoldDollorUI.Clear();
			m_SendGoldDollorUI = null;
		}
		if (bShow)
		{
			m_SendGoldDollorUI = new uiGroup(m_UIManager);
			UIBlock uIBlock = new UIBlock();
			uIBlock.Rect = new Rect(0f, 0f, 960f, 640f);
			m_SendGoldDollorUI.Add(uIBlock);
			UIImage control = UIUtils.BuildImage(0, new Rect(0f, 0f, 960f, 640f), m_MatCommonBg, new Rect(1016f, 1f, 1f, 1f), new Vector2(960f, 640f));
			m_SendGoldDollorUI.Add(control);
			control = UIUtils.BuildImage(0, new Rect(236f, 173f, 515f, 301f), m_MatDialog01, new Rect(0f, 378f, 515f, 301f), new Vector2(515f, 301f));
			m_SendGoldDollorUI.Add(control);
			UIText uIText = UIUtils.BuildUIText(0, new Rect(300f, 375f, 368f, 25f), UIText.enAlignStyle.left);
			uIText.Set("Zombie3D/Font/037-CAI978-18", "Emergency Update! We've fixed a number of crash bugs to make the game more stable and added a gift of 10,000 Cash and 5 tCrystals to thank you for your patience and understanding. Happy hunting!", Constant.TextCommonColor);
			m_SendGoldDollorUI.Add(uIText);
			UIClickButton control2 = UIUtils.BuildClickButton(5331, new Rect(393f, 149f, 191f, 62f), m_MatDialog01, new Rect(640f, 0f, 191f, 62f), new Rect(832f, 0f, 191f, 62f), new Rect(640f, 0f, 191f, 62f), new Vector2(191f, 62f));
			m_SendGoldDollorUI.Add(control2);
		}
	}

	public void SetupDailyBonusDialogUI(bool bShow, string context)
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
			control = UIUtils.BuildImage(0, new Rect(242f, 232f, 515f, 301f), m_MatDialog01, new Rect(0f, 378f, 515f, 301f), new Vector2(515f, 301f));
			m_Dialog01UI.Add(control);
			float top = 390f;
			if (context.Length < 50)
			{
				top = 350f;
			}
			UIText uIText = UIUtils.BuildUIText(0, new Rect(333f, top, 340f, 30f), UIText.enAlignStyle.left);
			uIText.Set("Zombie3D/Font/037-CAI978-18", context, Constant.TextCommonColor);
			m_Dialog01UI.Add(uIText);
			UIClickButton control2 = UIUtils.BuildClickButton(5327, new Rect(410f, 216f, 191f, 62f), m_MatDialog01, new Rect(640f, 0f, 191f, 62f), new Rect(832f, 0f, 191f, 62f), new Rect(640f, 0f, 191f, 62f), new Vector2(191f, 62f));
			m_Dialog01UI.Add(control2);
		}
	}

	public void SetupExternExpDialogUI(bool bShow)
	{
		if (m_DialogUI != null)
		{
			m_DialogUI.Clear();
			m_DialogUI = null;
		}
		if (bShow)
		{
			m_DialogUI = new uiGroup(m_UIManager);
			UIBlock uIBlock = new UIBlock();
			uIBlock.Rect = new Rect(0f, 0f, 960f, 640f);
			m_DialogUI.Add(uIBlock);
			UIImage control = UIUtils.BuildImage(0, new Rect(0f, 0f, 960f, 640f), m_MatCommonBg, new Rect(1016f, 1f, 1f, 1f), new Vector2(960f, 640f));
			m_DialogUI.Add(control);
			control = UIUtils.BuildImage(0, new Rect(242f, 232f, 515f, 301f), m_MatDialog01, new Rect(0f, 378f, 515f, 301f), new Vector2(515f, 301f));
			m_DialogUI.Add(control);
			UIText uIText = UIUtils.BuildUIText(0, new Rect(333f, 414f, 340f, 30f), UIText.enAlignStyle.left);
			uIText.Set("Zombie3D/Font/037-CAI978-18", "You received " + GameApp.GetInstance().GetGameState().ExternExp + " for assisting your friend in battle!", Color.yellow);
			m_DialogUI.Add(uIText);
			UIClickButton control2 = UIUtils.BuildClickButton(5328, new Rect(410f, 216f, 191f, 62f), m_MatDialog01, new Rect(640f, 0f, 191f, 62f), new Rect(832f, 0f, 191f, 62f), new Rect(640f, 0f, 191f, 62f), new Vector2(191f, 62f));
			m_DialogUI.Add(control2);
		}
	}

	public void SetupNotificationDialogUI(bool bShow, string strContent)
	{
		if (m_DialogUI != null)
		{
			m_DialogUI.Clear();
			m_DialogUI = null;
		}
		if (bShow)
		{
			m_DialogUI = new uiGroup(m_UIManager);
			UIImage control = UIUtils.BuildImage(0, new Rect(0f, 0f, 960f, 640f), m_MatDialog01, new Rect(600f, 1f, 1f, 1f), new Vector2(960f, 640f));
			m_DialogUI.Add(control);
			float left = 242f;
			float top = 232f;
			control = UIUtils.BuildImage(0, new Rect(left, top, 515f, 301f), m_MatDialog01, new Rect(0f, 378f, 515f, 301f), new Vector2(515f, 301f));
			m_DialogUI.Add(control);
			UIText uIText = UIUtils.BuildUIText(0, new Rect(290f, 295f, 420f, 150f), UIText.enAlignStyle.left);
			uIText.Set("Zombie3D/Font/037-CAI978-18", strContent, Constant.TextCommonColor);
			m_DialogUI.Add(uIText);
			UIClickButton uIClickButton = null;
			uIClickButton = UIUtils.BuildClickButton(5329, new Rect(385f, 215f, 191f, 62f), m_MatDialog01, new Rect(640f, 0f, 191f, 62f), new Rect(832f, 0f, 191f, 62f), new Rect(640f, 0f, 191f, 62f), new Vector2(191f, 62f));
			m_DialogUI.Add(uIClickButton);
		}
	}

	public void SetupNotHaveEnoughCrystalDialog(bool bShow)
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
			uIText.Set("Zombie3D/Font/037-CAI978-18", "Insufficient crystals! Visit the bank now to get more.", Constant.TextCommonColor);
			m_uiHintDialog.Add(uIText);
			UIClickButton uIClickButton = null;
			uIClickButton = UIUtils.BuildClickButton(5321, new Rect(num + 21f, num2 - 16f, 191f, 62f), m_MatDialog01, new Rect(640f, 186f, 191f, 62f), new Rect(832f, 186f, 191f, 62f), new Rect(640f, 186f, 191f, 62f), new Vector2(191f, 62f));
			m_uiHintDialog.Add(uIClickButton);
			uIClickButton = UIUtils.BuildClickButton(5323, new Rect(num + 280f, num2 - 16f, 191f, 62f), m_MatDialog01, new Rect(640f, 316f, 191f, 62f), new Rect(832f, 316f, 191f, 62f), new Rect(640f, 316f, 191f, 62f), new Vector2(191f, 62f));
			m_uiHintDialog.Add(uIClickButton);
		}
	}

	public void SetupSurvivalModeChooseMapsUI(bool bShow)
	{
		if (m_SurvivalModeMapsGroup != null)
		{
			m_SurvivalModeMapsGroup.Clear();
			m_SurvivalModeMapsGroup = null;
		}
		if (bShow)
		{
			m_SurvivalModeMapsGroup = new uiGroup(m_UIManager);
			UIImage control = UIUtils.BuildImage(0, new Rect(0f, 0f, 960f, 640f), m_MatCommonBg, new Rect(963f, 575f, 1f, 1f), new Vector2(960f, 640f));
			m_SurvivalModeMapsGroup.Add(control);
			Rect[] array = new Rect[2]
			{
				new Rect(0f, 454f, 492f, 212f),
				new Rect(0f, 666f, 492f, 212f)
			};
			string[] array2 = new string[3] { "?????", "Luxury Heights", "Central Hospital" };
			control = UIUtils.BuildImage(0, new Rect(211f, 217f, 45f, 233f), m_MatChoosePointsUI, new Rect(483f, 558f, 45f, 225f), new Vector2(45f, 233f));
			m_SurvivalModeMapsGroup.Add(control);
			control = UIUtils.BuildImage(0, new Rect(697f, 217f, 45f, 233f), m_MatChoosePointsUI, new Rect(529f, 558f, 45f, 225f), new Vector2(45f, 233f));
			m_SurvivalModeMapsGroup.Add(control);
			m_SurvivalModeMapsScrollBar = new UIImageScroller(AutoUI.AutoRect(new Rect(182f, 77f, 590f, 500f)), AutoUI.AutoRect(new Rect(182f, 132f, 590f, 400f)), 1, AutoUI.AutoSize(new Vector2(483f, 206f)), ScrollerDir.Vertical, true);
			m_SurvivalModeMapsScrollBar.Id = 5300;
			m_SurvivalModeMapsScrollBar.SetImageSpacing(AutoUI.AutoSize(new Vector2(0f, 50f)));
			m_SurvivalModeMapsScrollBar.SetUIHandler(m_SurvivalModeMapsScrollBar);
			Material material = LoadUIMaterial("Zombie3D/UI/Materials/MapIcons");
			Resources.UnloadUnusedAssets();
			for (int i = 0; i < array.Length; i++)
			{
				control = UIUtils.BuildImage(0, new Rect(0f, 0f, array[i].width, array[i].height), m_MatChoosePoints1UI, array[i], new Vector2(array[i].width, array[i].height));
				m_SurvivalModeMapsScrollBar.Add(control);
			}
			m_SurvivalModeMapsScrollBar.EnableScroll();
			m_SurvivalModeMapsScrollBar.SetMaskImage(m_MatChoosePointsUI, new Rect(1020f, 2f, 1f, 1f), new Vector2(500f, 500f));
			m_SurvivalModeMapsScrollBar.EnableLongPress();
			m_SurvivalModeMapsScrollBar.Show();
			m_SurvivalModeMapsGroup.Add(m_SurvivalModeMapsScrollBar);
			m_strSurvivalModeMapNameShow = UIUtils.BuildUIText(0, new Rect(255f, 253f, 300f, 30f), UIText.enAlignStyle.left);
			m_strSurvivalModeMapNameShow.Set("Zombie3D/Font/037-CAI978-22", array2[1], new Color(0.9372549f, 0.6509804f, 0.14509805f, 1f));
			m_SurvivalModeMapsGroup.Add(m_strSurvivalModeMapNameShow);
			UIClickButton control2 = UIUtils.BuildClickButton(5299, new Rect(716f, 85f, 194f, 63f), m_MatChoosePoints1UI, new Rect(777f, 65f, 194f, 63f), new Rect(777f, 1f, 194f, 63f), new Rect(777f, 65f, 194f, 63f), new Vector2(194f, 63f));
			m_SurvivalModeMapsGroup.Add(control2);
			control2 = UIUtils.BuildClickButton(5298, new Rect(55f, 85f, 187f, 68f), m_MatChoosePointsUI, new Rect(647f, 198f, 187f, 68f), new Rect(835f, 198f, 187f, 68f), new Rect(647f, 198f, 187f, 68f), new Vector2(187f, 68f));
			m_SurvivalModeMapsGroup.Add(control2);
		}
	}

	public int GetMapsCDTimeLength(int mapIndex)
	{
		return 0;
	}

	public void CheckDailyBonus()
	{
		if (m_bShowDailyBonus)
		{
			return;
		}
		int dailyBonus = gameState.GetDailyBonus();
		if (dailyBonus > 0)
		{
			if (UtilsEx.IsNetworkConnected())
			{
				m_DailyBonus = dailyBonus % 3;
				string text = "200 cash.";
				if (m_DailyBonus == 0)
				{
					gameState.AddDollor(1);
					text = "1 crystal.";
				}
				else if (m_DailyBonus == 1)
				{
					gameState.AddGold(500);
					text = "500 cash.";
				}
				else if (m_DailyBonus == 2)
				{
					gameState.AddGold(500);
					text = "500 cash.";
				}
				GameApp.GetInstance().Save();
				SetupDailyBonusDialogUI(true, "You have logged in " + dailyBonus + " days in a row.\nDaily Login Reward: " + text);
				GameCollectionInfoManager.Instance().Send();
			}
			else
			{
				SetupDailyBonusDialogUI(true, "You must be online to receive your daily bonus.");
			}
		}
		m_bShowDailyBonus = true;
	}

	public void CheckVersion()
	{
		GameObject gameObject = UnityEngine.Object.Instantiate(new GameObject("CheckVersion")) as GameObject;
		VersionValidationScript versionValidationScript = gameObject.AddComponent(typeof(VersionValidationScript)) as VersionValidationScript;
		versionValidationScript.m_DownLoadErrorEvent = CheckVersionError;
		versionValidationScript.m_DownLoadOKEvent = CheckVersionOK;
	}

	public void CheckVersionOK(string _version, string _ip, string _port, string _zone)
	{
		if (_version == "2.0.2")
		{
			GameApp.GetInstance().GetGameState().m_bIsRightVersion = true;
			GameApp.GetInstance().GetGameState().m_strZoneName = _zone;
			GameApp.GetInstance().GetGameState().serverName = _ip;
			GameApp.GetInstance().GetGameState().serverPort = int.Parse(_port);
			GameApp.GetInstance().GetGameState().m_bIsRightVersion = true;
			SceneUIManager.Instance().ChangeSceneUI(SceneUIManager.SceneUI.NNetworkUI);
		}
		else
		{
			GameApp.GetInstance().GetGameState().m_bIsRightVersion = false;
			SetupNotificationDialogUI(true, "There's a new version available. Download now!");
			m_bNeedShowAppStore = true;
			gameState.m_eGameMode.m_ePlayMode = GameState.NetworkGameMode.PlayMode.E_Console;
			gameState.m_eGameMode.m_eCooperaMode = GameState.NetworkGameMode.NetworkCooperationMode.E_Team;
		}
		OpenClickPlugin.Hide();
	}

	public void CheckVersionError()
	{
		GameApp.GetInstance().GetGameState().m_bIsRightVersion = false;
		gameState.m_eGameMode.m_ePlayMode = GameState.NetworkGameMode.PlayMode.E_Console;
		gameState.m_eGameMode.m_eCooperaMode = GameState.NetworkGameMode.NetworkCooperationMode.E_Team;
		SetupNotificationDialogUI(true, "You are disconnect!!!");
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
