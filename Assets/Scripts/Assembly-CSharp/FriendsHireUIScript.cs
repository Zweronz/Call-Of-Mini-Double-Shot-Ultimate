using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zombie3D;

public class FriendsHireUIScript : MonoBehaviour, UIHandler
{
	public enum Controls
	{
		kIDLevels = 10000,
		kIDBoost = 10001,
		kIDFriends = 10002,
		kIDShop = 10003,
		kIDTChat = 10004,
		kIDOptions = 10005,
		kIDCup = 10006,
		kIDTopList = 10007,
		kIDJunjie = 10008,
		kIDJunjieClose = 10009,
		kIDGlobalBank = 10010,
		kIDPlayerRotateShowControl = 10011,
		kIDHireFriendListView = 10012,
		kIDLocalGameConnectRetry = 10013,
		kIDLocalGameConnectOK = 10014,
		kIDLocalGameConnectLater = 10015,
		kIDNotificationDialogOK = 10016,
		kIDHireFriendListRefresh = 10017,
		kIDHireOutSelf = 10018,
		kIDHireOutSelfYes = 10019,
		kIDHireOutSelfLater = 10020,
		kIDHireMercOK = 10021,
		kIDHireMercLater = 10022,
		kIDGotoBankLater = 10023,
		kIDGotoBank = 10024,
		kIDHintDialogOK = 10025,
		kIDFriendListBegin = 10026,
		kIDFriendListEnd = 10281,
		kIDFriendListHireBegin = 10282,
		kIDFriendListHireEnd = 10537,
		kIDLast = 10538
	}

	private UIManager m_UIManager;

	private string m_ui_material_path;

	protected Material m_MatCommonBg;

	protected Material m_MatFriendsUI;

	protected Material m_MatAvatarIcons;

	protected Material m_MatDialog01;

	public uiGroup m_uiGroup;

	public uiGroup m_DesktopGroup;

	public uiGroup m_LocalGameDialogUI;

	public uiGroup m_LocalGameDialog2UI;

	public uiGroup m_CommonBarGroup;

	public uiGroup m_uiBattleFriendInfo;

	public uiGroup m_AroundUIGroup;

	public uiGroup m_DialogUI;

	public uiGroup m_LoadingUI;

	public uiGroup m_uiHintDialog;

	protected GameState gameState;

	protected PlayerUIShow m_PlayerShow;

	protected float lastUpdateTime;

	protected bool uiInited;

	public static bool m_bLoadingHireFriendsError;

	private bool m_bLoadingHireFriends;

	private float m_LoadingHireFriendsStartTime;

	protected int m_CurHireFriendCount;

	private int m_CurSelectFriendIndexShow;

	private UIScrollView m_FriendsPageView;

	private UIScrollBar m_FriendsPageViewScrollBar;

	private UIText m_TextHireOutTimer;

	private float m_HireOutTimerLastUpdateTime;

	private int m_HireMercIndex;

	private static float m_LastRefreshTime;

	private float m_bLastUICheckTime = -1f;

	private UIGroupControl m_PageViewRoot;

	private void Start()
	{
		OpenClickPlugin.Hide();
		m_UIManager = base.gameObject.AddComponent<UIManager>() as UIManager;
		m_UIManager.SetParameter(8, 3, false);
		m_UIManager.SetUIHandler(this);
		UIUtils.BuildIpone5Frame(m_UIManager);
		m_MatCommonBg = LoadUIMaterial("Zombie3D/UI/Materials/CommonBgUI");
		m_MatFriendsUI = LoadUIMaterial("Zombie3D/UI/Materials/FriendsUI");
		m_MatAvatarIcons = LoadUIMaterial("Zombie3D/UI/Materials/AvatarIcons");
		m_MatDialog01 = LoadUIMaterial("Zombie3D/UI/Materials/Dialog01");
		Resources.UnloadUnusedAssets();
		gameState = GameApp.GetInstance().GetGameState();
		uiInited = true;
		m_CurHireFriendCount = gameState.GetHireFriends().Count;
		m_CurSelectFriendIndexShow = -1;
		m_PlayerShow = SceneUIManager.Instance().ShowPlayerUIDDS(true);
		WeaponType weapon_type = gameState.GetBattleWeapons()[0];
		m_PlayerShow.ChangeWeapon(weapon_type);
		Hashtable avatars = gameState.GetAvatars();
		foreach (Avatar key in avatars.Keys)
		{
			if ((bool)avatars[key])
			{
				m_PlayerShow.ChangeAvatar(key.SuiteType, key.AvtType);
			}
		}
		SetupHireFriendsUI(true);
		if (gameState.GetHireFriends().Count <= 0)
		{
			m_bLoadingHireFriends = true;
			m_LoadingHireFriendsStartTime = Time.time;
			GameClient.Hire_GetHireArray();
			m_LastRefreshTime = Time.time;
			SetupLoadingUI(true);
		}
		GameCollectionInfoManager.Instance().GetCurrentInfo().AddUIEnterLog(GameCollectionInfo.enUIEnterIndex.MERC_UI);
	}

	private void Update()
	{
		UITouchInner[] array = (Application.isMobilePlatform) ? iPhoneInputMgr.MockTouches() : WindowsInputMgr.MockTouches();
		foreach (UITouchInner touch in array)
		{
			if (m_UIManager.HandleInput(touch))
			{
			}
		}
		if (Time.time - lastUpdateTime < 0.001f || !uiInited)
		{
			return;
		}
		lastUpdateTime = Time.time;
		if (m_bLoadingHireFriends)
		{
			bool flag = false;
			if (Time.time - m_LoadingHireFriendsStartTime > 20f)
			{
				flag = true;
			}
			int @int = GameClient.prop.GetInt("GetHireArray_Status", -1);
			if (@int == 1)
			{
				SetupLoadingUI(false);
				m_bLoadingHireFriends = false;
				m_bLoadingHireFriendsError = false;
				gameState.GetHireFriends().Clear();
				m_CurHireFriendCount = -1;
				Debug.Log("SceneUIManager.Instance().LoadHireFriendFromServerw() - ");
				SceneUIManager.Instance().LoadHireFriendFromServer();
			}
			if (flag || @int == 2)
			{
				Debug.Log("ERROR: GetHireArray TIMEOUT!!!!!!!!!!");
				SetupLoadingUI(false);
				m_bLoadingHireFriends = false;
				SetupHireFriendsUI(true);
			}
		}
		if (m_CurHireFriendCount <= 0 || (m_CurHireFriendCount < gameState.GetHireFriends().Count && Time.time - m_bLastUICheckTime > 1f))
		{
			m_bLastUICheckTime = Time.time;
			SetupHireFriendPageView();
			m_CurHireFriendCount = gameState.GetHireFriends().Count;
		}
		if (m_TextHireOutTimer != null && Time.time - m_HireOutTimerLastUpdateTime > 1f && m_uiBattleFriendInfo != null)
		{
			long num = gameState.m_LastHireOutTime + 259200 - UtilsEx.getNowDateSeconds();
			if (num > 0)
			{
				string text = UtilsEx.TimeToStr_HMS(num);
				m_TextHireOutTimer.SetText(text);
			}
			else
			{
				SetupHiredSelfInfo(true);
			}
		}
		if (m_LastRefreshTime > 0f && Time.time - m_LastRefreshTime > 10f)
		{
			m_LastRefreshTime = -1f;
			if (m_LoadingUI != null)
			{
				SetupLoadingUI(false);
			}
			SetupHireFriendPageView();
			m_CurHireFriendCount = gameState.GetHireFriends().Count;
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
		if (control.Id == 10000)
		{
			SceneUIManager.Instance().ShowPlayerUIDDS(false);
			SceneUIManager.Instance().ChangeSceneUI(SceneUIManager.SceneUI.ChoosePointsUI);
		}
		else if (control.Id == 10001)
		{
			SceneUIManager.Instance().ShowPlayerUIDDS(false);
			SceneUIManager.Instance().ChangeSceneUI(SceneUIManager.SceneUI.BoostUI);
		}
		else if (control.Id == 10002)
		{
			SceneUIManager.Instance().ShowPlayerUIDDS(false);
			SceneUIManager.Instance().ChangeSceneUI(SceneUIManager.SceneUI.FriendUI);
		}
		else if (control.Id == 10003)
		{
			SceneUIManager.Instance().ShowPlayerUIDDS(false);
			SceneUIManager.Instance().ChangeSceneUI(SceneUIManager.SceneUI.ShopUI);
		}
		else
		{
			if (control.Id == 10004)
			{
				return;
			}
			if (control.Id == 10005)
			{
				SceneUIManager.Instance().ShowPlayerUIDDS(false);
				SceneUIManager.Instance().ChangeSceneUI(SceneUIManager.SceneUI.OptionUI);
			}
			else if (control.Id == 10006)
			{
				SetupNotificationDialogUI(true, "You need to log in to Game Center to proceed!");
			}
			else if (control.Id == 10007)
			{
				SetupNotificationDialogUI(true, "You need to log in to Game Center to proceed!");
			}
			else
			{
				if (control.Id == 10008 || control.Id == 10009)
				{
					return;
				}
				if (control.Id == 10011)
				{
					if (m_PlayerShow != null)
					{
						m_PlayerShow.gameObject.transform.Rotate(new Vector3(0f, (0f - wparam) / 400f * 360f, 0f));
					}
				}
				else if (control.Id >= 10026 && control.Id <= 10281)
				{
					int num = control.Id - 10026;
					List<FriendUserData> hireFriends = gameState.GetHireFriends();
					if (hireFriends != null && hireFriends.Count > num)
					{
						m_CurSelectFriendIndexShow = num;
						FriendUserData friendUserData = hireFriends[control.Id - 10026];
						WeaponType weapon_type = friendUserData.m_BattleWeapons[0];
						m_PlayerShow.ChangeWeapon(weapon_type);
						Avatar avatar = new Avatar((Avatar.AvatarSuiteType)friendUserData.m_AvatarHeadSuiteType, Avatar.AvatarType.Head);
						m_PlayerShow.ChangeAvatar(avatar.SuiteType, avatar.AvtType);
						Avatar avatar2 = new Avatar((Avatar.AvatarSuiteType)friendUserData.m_AvatarBodySuiteType, Avatar.AvatarType.Body);
						m_PlayerShow.ChangeAvatar(avatar2.SuiteType, avatar2.AvtType);
						SetupHireFriendPageView();
						SetupHiredSelfInfo(true);
					}
				}
				else if (control.Id >= 10282 && control.Id <= 10537)
				{
					List<KeyValuePair<FriendUserData, long>> hiredFriends = gameState.GetHiredFriends();
					if (hiredFriends.Count >= 5)
					{
						SetupHintDialog(true, "You can't hire anymore sidekicks today, come back again tomorrow.");
						return;
					}
					int num2 = control.Id - 10282;
					List<FriendUserData> hireFriends2 = gameState.GetHireFriends();
					if (hireFriends2 == null || hireFriends2.Count <= num2)
					{
						return;
					}
					m_HireMercIndex = num2;
					FriendUserData friendUserData2 = hireFriends2[m_HireMercIndex];
					int num3 = 5000;
					for (int i = 0; i < GameClient.HireFriendListResult.Count; i++)
					{
						if (friendUserData2.m_UUID == GameClient.HireFriendListResult[i].uuid)
						{
							num3 = GameClient.HireFriendListResult[i].money;
							break;
						}
					}
					if (gameState.gold < num3)
					{
						SetupDonnotHaveEnoughMoneyDialog(true, "You don't have enough money!");
					}
					else
					{
						SetupHireMercConfirmDialogUI(true, num3);
					}
				}
				else if (control.Id == 10021)
				{
					SetupHireMercConfirmDialogUI(false, 0);
					List<FriendUserData> hireFriends3 = gameState.GetHireFriends();
					if (hireFriends3 == null || hireFriends3.Count <= m_HireMercIndex)
					{
						return;
					}
					FriendUserData friendUserData3 = hireFriends3[m_HireMercIndex];
					int goldSpend = 5000;
					for (int j = 0; j < GameClient.HireFriendListResult.Count; j++)
					{
						if (friendUserData3.m_UUID == GameClient.HireFriendListResult[j].uuid)
						{
							goldSpend = GameClient.HireFriendListResult[j].money;
							break;
						}
					}
					gameState.LoseGold(goldSpend);
					gameState.AddNewHiredFriend(friendUserData3, UtilsEx.getNowDateSeconds());
					GameCollectionInfoManager.Instance().GetCurrentInfo().AddHireOtherTimes();
					GameClient.Hire_HireOther(friendUserData3.m_UUID, friendUserData3.m_Level);
					hireFriends3.RemoveAt(m_HireMercIndex);
					SetupHireFriendPageView();
					SetupHiredSelfInfo(true);
					SetupAroundUI(true);
				}
				else if (control.Id == 10022)
				{
					SetupHireMercConfirmDialogUI(false, 0);
				}
				else if (control.Id == 10013)
				{
					SetupLocalGameConnectConfirmDialog(true);
				}
				else if (control.Id == 10014)
				{
					GameApp.GetInstance().GetGameState().m_bReLogin = true;
					Application.LoadLevel("Zombie3D_Judgement_GameLogin");
				}
				else if (control.Id == 10015)
				{
					SetupLocalGameConnectConfirmDialog(false);
				}
				else if (control.Id == 10016)
				{
					SetupNotificationDialogUI(false, string.Empty);
				}
				else if (control.Id == 10018)
				{
					int hireOutSelfPrice = gameState.GetHireOutSelfPrice();
					SetupHireOurSelfConfirmDialogUI(true, "Do you want to hire yourself out at the going rate of " + hireOutSelfPrice + " cash?");
				}
				else if (control.Id == 10019)
				{
					SetupHireOurSelfConfirmDialogUI(false, string.Empty);
					GameCollectionInfoManager.Instance().GetCurrentInfo().AddHireSelfTimes();
					HireOut();
					SetupHiredSelfInfo(true);
					MiscPlugin.CancelLocalNotification();
					MiscPlugin.ScheduleOfflineNotification(259200L, "Do you want to relist yourself for hire?");
				}
				else if (control.Id == 10020)
				{
					SetupHireOurSelfConfirmDialogUI(false, string.Empty);
				}
				else if (control.Id == 10017)
				{
					m_LastRefreshTime = Time.time;
					SetupLoadingUI(true);
					m_bLoadingHireFriends = true;
					m_LoadingHireFriendsStartTime = Time.time;
					GameClient.Hire_GetHireArray();
				}
				else if (control.Id == 10024)
				{
					SetupDonnotHaveEnoughMoneyDialog(false, string.Empty);
					SceneUIManager.Instance().ShowPlayerUIDDS(false);
					ShopUIScript.externalShopType = ShopUIScript.ShopType.Bank;
					SceneUIManager.Instance().ChangeSceneUI(SceneUIManager.SceneUI.ShopUI);
				}
				else if (control.Id == 10023)
				{
					SetupDonnotHaveEnoughMoneyDialog(false, string.Empty);
				}
				else if (control.Id == 10025)
				{
					SetupHintDialog(false, string.Empty);
				}
				else if (control.Id == 10010)
				{
					SceneUIManager.Instance().ShowPlayerUIDDS(false);
					ShopUIScript.externalShopType = ShopUIScript.ShopType.Bank;
					SceneUIManager.Instance().ChangeSceneUI(SceneUIManager.SceneUI.ShopUI);
				}
				else if (control.Id != 10538)
				{
				}
			}
		}
	}

	public void SetupCommonBarUI(bool bShow)
	{
		if (m_CommonBarGroup != null)
		{
			m_CommonBarGroup.Clear();
			m_CommonBarGroup = null;
		}
		if (bShow)
		{
			m_CommonBarGroup = new uiGroup(m_UIManager);
			UIImage control = UIUtils.BuildImage(0, new Rect(0f, 0f, 960f, 640f), m_MatCommonBg, new Rect(0f, 0f, 960f, 640f), new Vector2(960f, 640f));
			m_CommonBarGroup.Add(control);
			SetupAroundUI(true);
			control = UIUtils.BuildImage(0, new Rect(295f, 497f, 457f, 104f), m_MatCommonBg, new Rect(9f, 800f, 457f, 104f), new Vector2(457f, 104f));
			m_CommonBarGroup.Add(control);
			UIClickButton control2 = UIUtils.BuildClickButton(10000, new Rect(295f, 497f, 77f, 104f), m_MatCommonBg, new Rect(1022f, 1f, 1f, 1f), new Rect(9f, 904f, 77f, 104f), new Rect(1022f, 1f, 1f, 1f), new Vector2(77f, 104f));
			m_CommonBarGroup.Add(control2);
			control2 = UIUtils.BuildClickButton(10001, new Rect(372f, 497f, 80f, 104f), m_MatCommonBg, new Rect(1022f, 1f, 1f, 1f), new Rect(939f, 707f, 80f, 104f), new Rect(1022f, 1f, 1f, 1f), new Vector2(80f, 104f));
			m_CommonBarGroup.Add(control2);
			control2 = UIUtils.BuildClickButton(10002, new Rect(452f, 497f, 76f, 104f), m_MatCommonBg, new Rect(1022f, 1f, 1f, 1f), new Rect(85f, 904f, 76f, 104f), new Rect(1022f, 1f, 1f, 1f), new Vector2(76f, 104f));
			m_CommonBarGroup.Add(control2);
			control2 = UIUtils.BuildClickButton(10003, new Rect(528f, 497f, 80f, 104f), m_MatCommonBg, new Rect(1022f, 1f, 1f, 1f), new Rect(160f, 904f, 80f, 104f), new Rect(1022f, 1f, 1f, 1f), new Vector2(80f, 104f));
			m_CommonBarGroup.Add(control2);
			control2 = UIUtils.BuildClickButton(10004, new Rect(603f, 503f, 80f, 104f), m_MatCommonBg, new Rect(1022f, 1f, 1f, 1f), new Rect(834f, 700f, 80f, 104f), new Rect(1022f, 1f, 1f, 1f), new Vector2(80f, 104f));
			control2.Enable = false;
			m_CommonBarGroup.Add(control2);
			control2 = UIUtils.BuildClickButton(10005, new Rect(683f, 497f, 74f, 104f), m_MatCommonBg, new Rect(1022f, 1f, 1f, 1f), new Rect(240f, 904f, 74f, 104f), new Rect(1022f, 1f, 1f, 1f), new Vector2(74f, 104f));
			m_CommonBarGroup.Add(control2);
		}
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
			UIClickButton control3 = UIUtils.BuildClickButton(10010, new Rect(320f, 588f, 640f, 52f), m_MatDialog01, new Rect(0f, 798f, 640f, 52f), new Rect(0f, 850f, 640f, 52f), new Rect(0f, 798f, 640f, 52f), new Vector2(640f, 52f));
			m_AroundUIGroup.Add(control3);
		}
	}

	public void SetupHireFriendsUI(bool bShow)
	{
		if (m_uiGroup != null)
		{
			m_uiGroup.Clear();
			m_uiGroup = null;
		}
		if (bShow)
		{
			m_uiGroup = new uiGroup(m_UIManager);
			SetupCommonBarUI(true);
			UIMoveOuter control = UIUtils.BuildUIMoveOuter(10011, new Rect(0f, 0f, 393f, 575f), 10f, 10f);
			m_uiGroup.Add(control);
			UIImage control2 = UIUtils.BuildImage(0, new Rect(362f, 50f, 564f, 442f), m_MatFriendsUI, new Rect(0f, 0f, 564f, 442f), new Vector2(564f, 442f));
			m_uiGroup.Add(control2);
			UIClickButton control3 = UIUtils.BuildClickButton(10017, new Rect(537f, 11f, 191f, 62f), m_MatFriendsUI, new Rect(586f, 406f, 191f, 62f), new Rect(781f, 406f, 191f, 62f), new Rect(586f, 406f, 191f, 62f), new Vector2(191f, 62f));
			m_uiGroup.Add(control3);
			SetupHireFriendPageView();
			SetupHiredSelfInfo(true);
		}
	}

	public void SetupHireFriendPageView()
	{
		if (m_PageViewRoot == null)
		{
			m_PageViewRoot = new UIGroupControl();
			m_uiGroup.Add(m_PageViewRoot);
		}
		else
		{
			m_PageViewRoot.Clear();
		}
		UIClickButton uIClickButton = null;
		UIImage uIImage = null;
		UIText uIText = null;
		if (m_FriendsPageViewScrollBar != null)
		{
			m_PageViewRoot.Remove(m_FriendsPageViewScrollBar);
		}
		m_FriendsPageViewScrollBar = new UIScrollBar();
		m_FriendsPageViewScrollBar.ScrollOri = UIScrollBar.ScrollOrientation.Vertical;
		m_FriendsPageViewScrollBar.Rect = AutoUI.AutoRect(new Rect(860f, 105f, 20f, 274f));
		m_FriendsPageViewScrollBar.SetScrollBarTexture(m_MatFriendsUI, AutoUI.AutoRect(new Rect(564f, 0f, 20f, 274f)), m_MatFriendsUI, AutoUI.AutoRect(new Rect(564f, 274f, 20f, 86f)));
		m_FriendsPageViewScrollBar.SetSliderSize(AutoUI.AutoSize(new Vector2(20f, 86f)));
		m_FriendsPageViewScrollBar.SetScrollPercent(0f);
		m_PageViewRoot.Add(m_FriendsPageViewScrollBar);
		float num = 0f;
		int num2 = 0;
		if (m_FriendsPageView != null)
		{
			num = m_FriendsPageView.ScrollPosV;
			num2 = m_FriendsPageView.GetControlsCount();
			m_PageViewRoot.Remove(m_FriendsPageView);
		}
		m_FriendsPageView = new UIScrollView();
		m_FriendsPageView.SetMoveParam(AutoUI.AutoRect(new Rect(362f, 50f, 598f, 373f)), AutoUI.AutoDistance(20f), AutoUI.AutoDistance(20f));
		m_FriendsPageView.Id = 10012;
		m_FriendsPageView.Rect = AutoUI.AutoRect(new Rect(380f, 86f, 485f, 290f));
		m_FriendsPageView.ScrollOri = UIScrollView.ScrollOrientation.Vertical;
		m_FriendsPageView.ListOri = UIScrollView.ListOrientation.Vertical;
		m_FriendsPageView.ItemSpacingV = AutoUI.AutoDistance(4f);
		m_FriendsPageView.ItemSpacingH = AutoUI.AutoDistance(0f);
		m_FriendsPageView.SetClip(AutoUI.AutoRect(new Rect(380f, 80f, 485f, 296f)));
		m_FriendsPageView.Bounds = AutoUI.AutoRect(new Rect(380f, 80f, 485f, 296f));
		m_FriendsPageView.ScrollBar = m_FriendsPageViewScrollBar;
		m_PageViewRoot.Add(m_FriendsPageView);
		List<FriendUserData> hireFriends = gameState.GetHireFriends();
		Debug.Log("SetupHireFriendPageView() - " + hireFriends.Count);
		for (int i = 0; i < hireFriends.Count; i++)
		{
			FriendUserData friendUserData = hireFriends[i];
			UIGroupControl uIGroupControl = UIUtils.BuildUIGroupControl(0, new Rect(0f, 0f, 435f, 72f));
			float num3 = 414f;
			float num4 = 330 + 68 * i;
			uIImage = UIUtils.BuildImage(0, new Rect(45f, 0f, 437f, 65f), m_MatFriendsUI, new Rect(0f, 526f, 437f, 65f), new Vector2(437f, 65f));
			uIGroupControl.Add(uIImage);
			if (i == m_CurSelectFriendIndexShow)
			{
				uIImage = UIUtils.BuildImage(0, new Rect(45f, 0f, 434f, 65f), m_MatFriendsUI, new Rect(0f, 593f, 434f, 65f), new Vector2(434f, 65f));
				uIGroupControl.Add(uIImage);
			}
			else
			{
				uIClickButton = UIUtils.BuildClickButton(10026 + i, new Rect(45f, 0f, 437f, 65f), m_MatFriendsUI, new Rect(1023f, 1f, 1f, 1f), new Rect(0f, 593f, 437f, 65f), new Rect(1023f, 1f, 1f, 1f), new Vector2(437f, 65f));
				uIGroupControl.Add(uIClickButton);
			}
			Vector2 vector = new Vector2(63f, 38f);
			Vector2 rect_size = new Vector2(70f, 70f);
			Rect avatarIconTexture = ShopUIScript.GetAvatarIconTexture((Avatar.AvatarSuiteType)friendUserData.m_AvatarHeadSuiteType, Avatar.AvatarType.Head);
			if (avatarIconTexture.width > avatarIconTexture.height)
			{
				if (avatarIconTexture.width > rect_size.x)
				{
					rect_size = new Vector2(rect_size.x, rect_size.x / avatarIconTexture.width * avatarIconTexture.height);
				}
			}
			else if (avatarIconTexture.height > rect_size.y)
			{
				rect_size = new Vector2(rect_size.y / avatarIconTexture.height * avatarIconTexture.width, rect_size.y);
			}
			uIImage = UIUtils.BuildImage(0, new Rect(vector.x - rect_size.x / 2f, vector.y - rect_size.y / 2f, rect_size.x, rect_size.y), m_MatAvatarIcons, avatarIconTexture, rect_size);
			uIImage.CatchMessage = false;
			uIGroupControl.Add(uIImage);
			uIText = UIUtils.BuildUIText(0, new Rect(120f, 25f, 200f, 25f), UIText.enAlignStyle.left);
			uIText.Set("Zombie3D/Font/037-CAI978-22", "LV " + friendUserData.m_Level, Constant.TextCommonColor);
			uIGroupControl.Add(uIText);
			uIImage = UIUtils.BuildImage(0, new Rect(205f, 20f, 50f, 35f), m_MatFriendsUI, new Rect(860f, 365f, 50f, 35f), new Vector2(50f, 35f));
			uIImage.CatchMessage = false;
			uIGroupControl.Add(uIImage);
			int num5 = 5000;
			for (int j = 0; j < GameClient.HireFriendListResult.Count; j++)
			{
				if (friendUserData.m_UUID == GameClient.HireFriendListResult[j].uuid)
				{
					num5 = GameClient.HireFriendListResult[j].money;
					break;
				}
			}
			uIText = UIUtils.BuildUIText(0, new Rect(260f, 25f, 100f, 25f), UIText.enAlignStyle.center);
			uIText.Set("Zombie3D/Font/037-CAI978-22", num5.ToString(), Constant.TextCommonColor);
			uIGroupControl.Add(uIText);
			uIClickButton = UIUtils.BuildClickButton(10282 + i, new Rect(377f, 18f, 85f, 40f), m_MatFriendsUI, new Rect(880f, 178f, 85f, 40f), new Rect(792f, 178f, 85f, 40f), new Rect(880f, 178f, 85f, 40f), new Vector2(85f, 40f));
			uIGroupControl.Add(uIClickButton);
			m_FriendsPageView.Add(uIGroupControl);
		}
		if (num > 0f)
		{
			float num6 = num * (float)num2 / (float)m_FriendsPageView.GetControlsCount();
			m_FriendsPageView.ScrollPosV = num6;
			m_FriendsPageViewScrollBar.SetScrollPercent(num6);
		}
	}

	public void SetupHiredSelfInfo(bool bShow)
	{
		if (m_uiBattleFriendInfo != null)
		{
			m_uiBattleFriendInfo.Clear();
			m_uiBattleFriendInfo = null;
		}
		if (!bShow)
		{
			return;
		}
		m_uiBattleFriendInfo = new uiGroup(m_UIManager);
		Hashtable avatars = gameState.GetAvatars();
		foreach (Avatar key in avatars.Keys)
		{
			if (!(bool)avatars[key] || key.AvtType != 0)
			{
				continue;
			}
			Vector2 vector = new Vector2(410f, 467f);
			Vector2 rect_size = new Vector2(100f, 100f);
			Rect avatarIconTexture = ShopUIScript.GetAvatarIconTexture(key.SuiteType, key.AvtType);
			if (avatarIconTexture.width > avatarIconTexture.height)
			{
				if (avatarIconTexture.width > rect_size.x)
				{
					rect_size = new Vector2(rect_size.x, rect_size.x / avatarIconTexture.width * avatarIconTexture.height);
				}
			}
			else if (avatarIconTexture.height > rect_size.y)
			{
				rect_size = new Vector2(rect_size.y / avatarIconTexture.height * avatarIconTexture.width, rect_size.y);
			}
			UIImage control = UIUtils.BuildImage(0, new Rect(vector.x - rect_size.x / 2f, vector.y - rect_size.y / 2f, rect_size.x, rect_size.y), m_MatAvatarIcons, avatarIconTexture, rect_size);
			m_uiBattleFriendInfo.Add(control);
		}
		m_TextHireOutTimer = null;
		long num = gameState.m_LastHireOutTime + 259200 - UtilsEx.getNowDateSeconds();
		if (num > 0)
		{
			string text = UtilsEx.TimeToStr_HMS(num);
			m_TextHireOutTimer = UIUtils.BuildUIText(0, new Rect(480f, 440f, 200f, 30f), UIText.enAlignStyle.left);
			m_TextHireOutTimer.Set("Zombie3D/Font/037-CAI978-22", text, Constant.TextCommonColor);
			m_uiBattleFriendInfo.Add(m_TextHireOutTimer);
			UIImage control2 = UIUtils.BuildImage(0, new Rect(678f, 434f, 133f, 40f), m_MatFriendsUI, new Rect(792f, 137f, 133f, 40f), new Vector2(133f, 40f));
			m_uiBattleFriendInfo.Add(control2);
		}
		else
		{
			UIClickButton control3 = UIUtils.BuildClickButton(10018, new Rect(678f, 434f, 133f, 40f), m_MatFriendsUI, new Rect(586f, 362f, 133f, 40f), new Rect(722f, 362f, 133f, 40f), new Rect(586f, 362f, 133f, 40f), new Vector2(133f, 40f));
			m_uiBattleFriendInfo.Add(control3);
		}
	}

	public void SetupLocalGameRetryDialog(bool bShow)
	{
		if (m_LocalGameDialogUI != null)
		{
			m_LocalGameDialogUI.Clear();
			m_LocalGameDialogUI = null;
		}
		if (bShow)
		{
			m_LocalGameDialogUI = new uiGroup(m_UIManager);
			UIText uIText = UIUtils.BuildUIText(0, new Rect(457f, 240f, 440f, 40f), UIText.enAlignStyle.left);
			uIText.Set("Zombie3D/Font/037-CAI978-18", "Accessing your friend's info requires network connection.", Constant.TextCommonColor);
			m_LocalGameDialogUI.Add(uIText);
			UIClickButton control = UIUtils.BuildClickButton(10013, new Rect(530f, 30f, 204f, 75f), m_MatFriendsUI, new Rect(586f, 285f, 204f, 75f), new Rect(790f, 285f, 204f, 75f), new Rect(586f, 285f, 204f, 75f), new Vector2(204f, 75f));
			m_LocalGameDialogUI.Add(control);
		}
	}

	public void SetupLocalGameConnectConfirmDialog(bool bShow)
	{
		if (m_LocalGameDialog2UI != null)
		{
			m_LocalGameDialog2UI.Clear();
			m_LocalGameDialog2UI = null;
		}
		if (bShow)
		{
			m_LocalGameDialog2UI = new uiGroup(m_UIManager);
			UIImage control = UIUtils.BuildImage(0, new Rect(380f, 50f, 522f, 352f), m_MatDialog01, new Rect(600f, 1f, 1f, 1f), new Vector2(548f, 352f));
			m_LocalGameDialog2UI.Add(control);
			float left = 385f;
			float top = 120f;
			control = UIUtils.BuildImage(0, new Rect(left, top, 515f, 301f), m_MatDialog01, new Rect(0f, 378f, 515f, 301f), new Vector2(515f, 301f));
			m_LocalGameDialog2UI.Add(control);
			UIText uIText = UIUtils.BuildUIText(0, new Rect(420f, 200f, 440f, 100f), UIText.enAlignStyle.left);
			uIText.Set("Zombie3D/Font/037-CAI978-18", "You have to reload the game to refresh your connection. Proceed?", Constant.TextCommonColor);
			m_LocalGameDialog2UI.Add(uIText);
			UIClickButton uIClickButton = null;
			uIClickButton = UIUtils.BuildClickButton(10014, new Rect(410f, 110f, 191f, 62f), m_MatDialog01, new Rect(640f, 0f, 191f, 62f), new Rect(832f, 0f, 191f, 62f), new Rect(640f, 0f, 191f, 62f), new Vector2(191f, 62f));
			m_LocalGameDialog2UI.Add(uIClickButton);
			uIClickButton = UIUtils.BuildClickButton(10015, new Rect(666f, 110f, 191f, 62f), m_MatDialog01, new Rect(640f, 186f, 191f, 62f), new Rect(832f, 186f, 191f, 62f), new Rect(640f, 186f, 191f, 62f), new Vector2(191f, 62f));
			m_LocalGameDialog2UI.Add(uIClickButton);
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
			uIClickButton = UIUtils.BuildClickButton(10016, new Rect(385f, 215f, 191f, 62f), m_MatDialog01, new Rect(640f, 0f, 191f, 62f), new Rect(832f, 0f, 191f, 62f), new Rect(640f, 0f, 191f, 62f), new Vector2(191f, 62f));
			m_DialogUI.Add(uIClickButton);
		}
	}

	public void SetupHireOurSelfConfirmDialogUI(bool bShow, string strContent)
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
			UIText uIText = UIUtils.BuildUIText(0, new Rect(290f, 255f, 420f, 150f), UIText.enAlignStyle.left);
			uIText.Set("Zombie3D/Font/037-CAI978-18", strContent, Constant.TextCommonColor);
			m_DialogUI.Add(uIText);
			UIClickButton uIClickButton = null;
			uIClickButton = UIUtils.BuildClickButton(10020, new Rect(260f, 215f, 191f, 62f), m_MatDialog01, new Rect(640f, 186f, 191f, 62f), new Rect(832f, 186f, 191f, 62f), new Rect(640f, 186f, 191f, 62f), new Vector2(191f, 62f));
			m_DialogUI.Add(uIClickButton);
			uIClickButton = UIUtils.BuildClickButton(10019, new Rect(520f, 215f, 191f, 62f), m_MatDialog01, new Rect(640f, 0f, 191f, 62f), new Rect(832f, 0f, 191f, 62f), new Rect(640f, 0f, 191f, 62f), new Vector2(191f, 62f));
			m_DialogUI.Add(uIClickButton);
		}
	}

	public void SetupHireMercConfirmDialogUI(bool bShow, int price)
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
			UIText uIText = UIUtils.BuildUIText(0, new Rect(290f, 255f, 420f, 150f), UIText.enAlignStyle.left);
			uIText.Set("Zombie3D/Font/037-CAI978-18", "Do you want to spend " + price + " cash to hire this merc for 24 hours?", Constant.TextCommonColor);
			m_DialogUI.Add(uIText);
			UIClickButton uIClickButton = null;
			uIClickButton = UIUtils.BuildClickButton(10022, new Rect(260f, 215f, 191f, 62f), m_MatDialog01, new Rect(640f, 186f, 191f, 62f), new Rect(832f, 186f, 191f, 62f), new Rect(640f, 186f, 191f, 62f), new Vector2(191f, 62f));
			m_DialogUI.Add(uIClickButton);
			uIClickButton = UIUtils.BuildClickButton(10021, new Rect(520f, 215f, 191f, 62f), m_MatDialog01, new Rect(640f, 0f, 191f, 62f), new Rect(832f, 0f, 191f, 62f), new Rect(640f, 0f, 191f, 62f), new Vector2(191f, 62f));
			m_DialogUI.Add(uIClickButton);
		}
	}

	public void SetupLoadingUI(bool bShow)
	{
		Debug.Log("SetupLoadingUI - " + bShow);
		if (m_LoadingUI != null)
		{
			m_LoadingUI.Clear();
			m_LoadingUI = null;
		}
		if (bShow)
		{
			m_LoadingUI = new uiGroup(m_UIManager);
			Material material = LoadUIMaterial("Zombie3D/UI/Materials/MacOSLoadingUI");
			UIImage control = UIUtils.BuildImage(0, new Rect(0f, 0f, 960f, 640f), material, new Rect(2f, 58f, 1f, 1f), new Vector2(960f, 640f));
			m_LoadingUI.Add(control);
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
			m_LoadingUI.Add(uIAnimationControl);
		}
	}

	public void SetupDonnotHaveEnoughMoneyDialog(bool bShow, string dialog_content)
	{
		if (m_uiHintDialog != null)
		{
			m_uiHintDialog.Clear();
			m_uiHintDialog = null;
		}
		if (bShow)
		{
			m_uiHintDialog = new uiGroup(m_UIManager);
			UIImage control = UIUtils.BuildImage(0, new Rect(0f, 0f, 960f, 640f), m_MatDialog01, new Rect(600f, 1f, 1f, 1f), new Vector2(960f, 640f));
			m_uiHintDialog.Add(control);
			float num = 242f;
			float num2 = 232f;
			control = UIUtils.BuildImage(0, new Rect(num, num2, 515f, 301f), m_MatDialog01, new Rect(0f, 378f, 515f, 301f), new Vector2(515f, 301f));
			m_uiHintDialog.Add(control);
			UIText uIText = UIUtils.BuildUIText(0, new Rect(num + 40f, num2 + 40f, 420f, 173f), UIText.enAlignStyle.left);
			uIText.Set("Zombie3D/Font/037-CAI978-18", dialog_content, Constant.TextCommonColor);
			m_uiHintDialog.Add(uIText);
			UIClickButton uIClickButton = null;
			uIClickButton = UIUtils.BuildClickButton(10023, new Rect(num + 21f, num2 - 16f, 191f, 62f), m_MatDialog01, new Rect(640f, 186f, 191f, 62f), new Rect(832f, 186f, 191f, 62f), new Rect(640f, 186f, 191f, 62f), new Vector2(191f, 62f));
			m_uiHintDialog.Add(uIClickButton);
			uIClickButton = UIUtils.BuildClickButton(10024, new Rect(num + 280f, num2 - 16f, 191f, 62f), m_MatDialog01, new Rect(640f, 316f, 191f, 62f), new Rect(832f, 316f, 191f, 62f), new Rect(640f, 316f, 191f, 62f), new Vector2(191f, 62f));
			m_uiHintDialog.Add(uIClickButton);
		}
	}

	public void SetupHintDialog(bool bShow, string dialog_content)
	{
		if (m_uiHintDialog != null)
		{
			m_uiHintDialog.Clear();
			m_uiHintDialog = null;
		}
		if (bShow)
		{
			m_uiHintDialog = new uiGroup(m_UIManager);
			UIImage control = UIUtils.BuildImage(0, new Rect(0f, 0f, 960f, 640f), m_MatDialog01, new Rect(600f, 1f, 1f, 1f), new Vector2(960f, 640f));
			m_uiHintDialog.Add(control);
			float num = 242f;
			float num2 = 232f;
			control = UIUtils.BuildImage(0, new Rect(num, num2, 515f, 301f), m_MatDialog01, new Rect(0f, 378f, 515f, 301f), new Vector2(515f, 301f));
			m_uiHintDialog.Add(control);
			UIText uIText = UIUtils.BuildUIText(0, new Rect(num + 40f, num2 + 40f, 420f, 173f), UIText.enAlignStyle.left);
			uIText.Set("Zombie3D/Font/037-CAI978-18", dialog_content, Constant.TextCommonColor);
			m_uiHintDialog.Add(uIText);
			UIClickButton uIClickButton = null;
			uIClickButton = UIUtils.BuildClickButton(10025, new Rect(num + 150f, num2 - 16f, 191f, 62f), m_MatDialog01, new Rect(640f, 0f, 191f, 62f), new Rect(832f, 0f, 191f, 62f), new Rect(640f, 0f, 191f, 62f), new Vector2(191f, 62f));
			m_uiHintDialog.Add(uIClickButton);
		}
	}

	public void HireOut()
	{
		gameState.HireOut(UtilsEx.getNowDateSeconds());
		int hireOutSelfPrice = gameState.GetHireOutSelfPrice();
		GameClient.Hire_HireOutSelf(hireOutSelfPrice);
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
