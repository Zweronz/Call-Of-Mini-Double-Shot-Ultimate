using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zombie3D;

public class FriendsUIScript : MonoBehaviour, UIHandler
{
	public enum Controls
	{
		kIDLevels = 6000,
		kIDBoost = 6001,
		kIDFriends = 6002,
		kIDShop = 6003,
		kIDTChat = 6004,
		kIDOptions = 6005,
		kIDCup = 6006,
		kIDTopList = 6007,
		kIDJunjie = 6008,
		kIDJunjieClose = 6009,
		kIDGlobalBank = 6010,
		kIDPlayerRotateShowControl = 6011,
		kIDLocalGameConnectRetry = 6012,
		kIDLocalGameConnectOK = 6013,
		kIDLocalGameConnectLater = 6014,
		kIDNotificationDialogOK = 6015,
		kIDShowFriendsList = 6016,
		kIDShowHiredList = 6017,
		kIDGotoMerc = 6018,
		kIDFriendListBegin = 6019,
		kIDFriendListEnd = 6274,
		kIDFriendListJoinBegin = 6275,
		kIDFriendListJoinEnd = 6530,
		kIDLast = 6531
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

	public uiGroup m_JunjieUIGroup;

	public uiGroup m_DialogUI;

	public uiGroup m_LoadingUI;

	protected GameState gameState;

	protected PlayerUIShow m_PlayerShow;

	protected float lastUpdateTime;

	protected bool uiInited;

	public static bool m_bLoadingGameCenterFriendsError;

	private bool m_bLoadingGameCenterFriends;

	private float m_LoadingGameCenterFriendsStartTime;

	private UIText m_CountPrefix;

	private UIText m_CountSuffix;

	protected int m_CurFriendCount = 1;

	private int m_CurSelectFriendIndexShow = -1;

	private int m_CurSelectHiredFriendIndexShow = -1;

	private UIScrollView m_FriendsPageView;

	private UIScrollBar m_FriendsPageViewScrollBar;

	private bool m_bFriendsOrHired;

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
		m_bFriendsOrHired = false;
		if (gameState.m_SelectHiredFriendIndex < 0)
		{
			m_CurSelectHiredFriendIndexShow = -1;
			m_CurSelectFriendIndexShow = gameState.m_SelectFriendIndex;
			gameState.m_SelectHiredFriendIndex = -1;
			ArrayList friends = gameState.GetFriends();
			m_CurFriendCount = friends.Count;
			if (m_CurSelectFriendIndexShow >= m_CurFriendCount)
			{
				m_CurSelectFriendIndexShow = 0;
				gameState.m_SelectFriendIndex = 0;
			}
			if (friends.Count > m_CurSelectFriendIndexShow)
			{
				FriendUserData battleFriends = (FriendUserData)friends[m_CurSelectFriendIndexShow];
				gameState.SetBattleFriends(battleFriends);
			}
		}
		else
		{
			m_CurSelectFriendIndexShow = -1;
			m_CurSelectHiredFriendIndexShow = gameState.m_SelectHiredFriendIndex;
			gameState.m_SelectFriendIndex = -1;
			m_CurFriendCount = gameState.GetHiredFriends().Count;
			if (m_CurSelectHiredFriendIndexShow >= m_CurFriendCount)
			{
				m_CurSelectHiredFriendIndexShow = 0;
				gameState.m_SelectHiredFriendIndex = 0;
			}
			List<KeyValuePair<FriendUserData, long>> hiredFriends = gameState.GetHiredFriends();
			if (hiredFriends.Count > m_CurSelectHiredFriendIndexShow)
			{
				FriendUserData key = hiredFriends[m_CurSelectHiredFriendIndexShow].Key;
				gameState.SetBattleFriends(key);
			}
			else
			{
				ArrayList friends2 = gameState.GetFriends();
				FriendUserData battleFriends2 = (FriendUserData)friends2[0];
				gameState.SetBattleFriends(battleFriends2);
			}
		}
		m_PlayerShow = SceneUIManager.Instance().ShowPlayerUIDDS(true);
		SetupFriendsUI(true);
		if (m_bLoadingGameCenterFriendsError)
		{
			m_bLoadingGameCenterFriends = true;
			m_LoadingGameCenterFriendsStartTime = Time.time;
			GameCenterPlugin.LoadFriends();
		}
		if (m_bFriendsOrHired)
		{
			GameCollectionInfoManager.Instance().GetCurrentInfo().AddUIEnterLog(GameCollectionInfo.enUIEnterIndex.TeamUI_Friend);
		}
		else
		{
			GameCollectionInfoManager.Instance().GetCurrentInfo().AddUIEnterLog(GameCollectionInfo.enUIEnterIndex.TeamUI_MERC);
		}
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
		if (m_bLoadingGameCenterFriends)
		{
			bool flag = false;
			if (Time.time - m_LoadingGameCenterFriendsStartTime > 20f)
			{
				flag = true;
			}
			int loadFriendStatus = GameCenterPlugin.GetLoadFriendStatus();
			if (loadFriendStatus == 3)
			{
				m_bLoadingGameCenterFriends = false;
				m_bLoadingGameCenterFriendsError = false;
				FriendUserData value = gameState.GenerateDefaultFriendPlayer();
				gameState.GetFriends().Clear();
				gameState.GetFriends().Add(value);
				SceneUIManager.Instance().LoadFriendFromServer();
			}
			else if (flag || loadFriendStatus == 2)
			{
				m_bLoadingGameCenterFriends = false;
				SetupFriendsUI(true);
			}
		}
		if (m_bFriendsOrHired)
		{
			if (gameState.LoginType != 0 && m_CurFriendCount < gameState.GetFriends().Count)
			{
				SetupFriendPageView();
				m_CurFriendCount = gameState.GetFriends().Count;
			}
		}
		else if (m_CurFriendCount < gameState.GetHiredFriends().Count)
		{
			SetupHiredFriendPageView();
			m_CurFriendCount = gameState.GetHiredFriends().Count;
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
		if (control.Id == 6000)
		{
			SceneUIManager.Instance().ShowPlayerUIDDS(false);
			SceneUIManager.Instance().ChangeSceneUI(SceneUIManager.SceneUI.ChoosePointsUI);
		}
		else if (control.Id == 6001)
		{
			SceneUIManager.Instance().ShowPlayerUIDDS(false);
			SceneUIManager.Instance().ChangeSceneUI(SceneUIManager.SceneUI.BoostUI);
		}
		else
		{
			if (control.Id == 6002)
			{
				return;
			}
			if (control.Id == 6003)
			{
				SceneUIManager.Instance().ShowPlayerUIDDS(false);
				SceneUIManager.Instance().ChangeSceneUI(SceneUIManager.SceneUI.ShopUI);
			}
			else
			{
				if (control.Id == 6004)
				{
					return;
				}
				if (control.Id == 6005)
				{
					SceneUIManager.Instance().ShowPlayerUIDDS(false);
					SceneUIManager.Instance().ChangeSceneUI(SceneUIManager.SceneUI.OptionUI);
				}
				else if (control.Id == 6006)
				{
					SetupNotificationDialogUI(true, "You need to log in to Game Center to proceed!");
				}
				else if (control.Id == 6007)
				{
					SetupNotificationDialogUI(true, "You need to log in to Game Center to proceed!");
				}
				else
				{
					if (control.Id == 6008 || control.Id == 6009)
					{
						return;
					}
					if (control.Id == 6011)
					{
						if (m_PlayerShow != null)
						{
							m_PlayerShow.gameObject.transform.Rotate(new Vector3(0f, (0f - wparam) / 400f * 360f, 0f));
						}
					}
					else if (control.Id >= 6019 && control.Id <= 6274)
					{
						if (m_bFriendsOrHired)
						{
							m_CurSelectFriendIndexShow = control.Id - 6019;
							ArrayList friends = gameState.GetFriends();
							FriendUserData friendUserData = friends[control.Id - 6019] as FriendUserData;
							WeaponType weapon_type = friendUserData.m_BattleWeapons[0];
							m_PlayerShow.ChangeWeapon(weapon_type);
							Avatar avatar = new Avatar((Avatar.AvatarSuiteType)friendUserData.m_AvatarHeadSuiteType, Avatar.AvatarType.Head);
							m_PlayerShow.ChangeAvatar(avatar.SuiteType, avatar.AvtType);
							Avatar avatar2 = new Avatar((Avatar.AvatarSuiteType)friendUserData.m_AvatarBodySuiteType, Avatar.AvatarType.Body);
							m_PlayerShow.ChangeAvatar(avatar2.SuiteType, avatar2.AvtType);
							SetupFriendPageView();
						}
						else
						{
							m_CurSelectHiredFriendIndexShow = control.Id - 6019;
							List<KeyValuePair<FriendUserData, long>> hiredFriends = gameState.GetHiredFriends();
							FriendUserData key = hiredFriends[m_CurSelectHiredFriendIndexShow].Key;
							WeaponType weapon_type2 = key.m_BattleWeapons[0];
							m_PlayerShow.ChangeWeapon(weapon_type2);
							Avatar avatar3 = new Avatar((Avatar.AvatarSuiteType)key.m_AvatarHeadSuiteType, Avatar.AvatarType.Head);
							m_PlayerShow.ChangeAvatar(avatar3.SuiteType, avatar3.AvtType);
							Avatar avatar4 = new Avatar((Avatar.AvatarSuiteType)key.m_AvatarBodySuiteType, Avatar.AvatarType.Body);
							m_PlayerShow.ChangeAvatar(avatar4.SuiteType, avatar4.AvtType);
							SetupHiredFriendPageView();
						}
					}
					else if (control.Id >= 6275 && control.Id <= 6530)
					{
						if (m_bFriendsOrHired)
						{
							gameState.m_SelectHiredFriendIndex = -1;
							ArrayList friends2 = gameState.GetFriends();
							FriendUserData friendUserData2 = friends2[control.Id - 6275] as FriendUserData;
							gameState.m_SelectFriendIndex = control.Id - 6275;
							gameState.SetBattleFriends(friendUserData2);
							WeaponType weapon_type3 = friendUserData2.m_BattleWeapons[0];
							m_PlayerShow.ChangeWeapon(weapon_type3);
							Avatar avatar5 = new Avatar((Avatar.AvatarSuiteType)friendUserData2.m_AvatarHeadSuiteType, Avatar.AvatarType.Head);
							m_PlayerShow.ChangeAvatar(avatar5.SuiteType, avatar5.AvtType);
							Avatar avatar6 = new Avatar((Avatar.AvatarSuiteType)friendUserData2.m_AvatarBodySuiteType, Avatar.AvatarType.Body);
							m_PlayerShow.ChangeAvatar(avatar6.SuiteType, avatar6.AvtType);
							SetupFriendPageView();
						}
						else
						{
							gameState.m_SelectFriendIndex = -1;
							List<KeyValuePair<FriendUserData, long>> hiredFriends2 = gameState.GetHiredFriends();
							FriendUserData key2 = hiredFriends2[control.Id - 6275].Key;
							gameState.m_SelectHiredFriendIndex = control.Id - 6275;
							gameState.SetBattleFriends(key2);
							WeaponType weapon_type4 = key2.m_BattleWeapons[0];
							m_PlayerShow.ChangeWeapon(weapon_type4);
							Avatar avatar7 = new Avatar((Avatar.AvatarSuiteType)key2.m_AvatarHeadSuiteType, Avatar.AvatarType.Head);
							m_PlayerShow.ChangeAvatar(avatar7.SuiteType, avatar7.AvtType);
							Avatar avatar8 = new Avatar((Avatar.AvatarSuiteType)key2.m_AvatarBodySuiteType, Avatar.AvatarType.Body);
							m_PlayerShow.ChangeAvatar(avatar8.SuiteType, avatar8.AvtType);
							SetupHiredFriendPageView();
						}
						SetupBattleFriendInfo(true);
					}
					else if (control.Id == 6012)
					{
						SetupLocalGameConnectConfirmDialog(true);
					}
					else if (control.Id == 6013)
					{
						GameApp.GetInstance().GetGameState().m_bReLogin = true;
						Application.LoadLevel("Zombie3D_Judgement_GameLogin");
					}
					else if (control.Id == 6014)
					{
						SetupLocalGameConnectConfirmDialog(false);
					}
					else if (control.Id == 6015)
					{
						SetupNotificationDialogUI(false, string.Empty);
					}
					else if (control.Id == 6016)
					{
						if (!m_bFriendsOrHired)
						{
							m_bFriendsOrHired = true;
							m_CurSelectFriendIndexShow = gameState.m_SelectFriendIndex;
							if (m_FriendsPageView != null)
							{
								m_FriendsPageView.ScrollPosV = 0f;
								m_FriendsPageViewScrollBar.SetScrollPercent(0f);
							}
							SetupFriendsUI(true);
							GameCollectionInfoManager.Instance().GetCurrentInfo().AddUIEnterLog(GameCollectionInfo.enUIEnterIndex.TeamUI_Friend);
						}
					}
					else if (control.Id == 6017)
					{
						if (m_bFriendsOrHired)
						{
							m_bFriendsOrHired = false;
							m_CurSelectHiredFriendIndexShow = gameState.m_SelectHiredFriendIndex;
							if (m_FriendsPageView != null)
							{
								m_FriendsPageView.ScrollPosV = 0f;
								m_FriendsPageViewScrollBar.SetScrollPercent(0f);
							}
							SetupFriendsUI(true);
							GameCollectionInfoManager.Instance().GetCurrentInfo().AddUIEnterLog(GameCollectionInfo.enUIEnterIndex.TeamUI_MERC);
						}
					}
					else if (control.Id == 6018)
					{
						SceneUIManager.Instance().ShowPlayerUIDDS(false);
						SceneUIManager.Instance().ChangeSceneUI(SceneUIManager.SceneUI.FriendsHireUI);
					}
					else if (control.Id == 6010)
					{
						SceneUIManager.Instance().ShowPlayerUIDDS(false);
						ShopUIScript.externalShopType = ShopUIScript.ShopType.Bank;
						SceneUIManager.Instance().ChangeSceneUI(SceneUIManager.SceneUI.ShopUI);
					}
					else if (control.Id != 6531)
					{
					}
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
			UIClickButton control2 = UIUtils.BuildClickButton(6000, new Rect(295f, 497f, 77f, 104f), m_MatCommonBg, new Rect(1022f, 1f, 1f, 1f), new Rect(9f, 904f, 77f, 104f), new Rect(1022f, 1f, 1f, 1f), new Vector2(77f, 104f));
			m_CommonBarGroup.Add(control2);
			control2 = UIUtils.BuildClickButton(6001, new Rect(372f, 497f, 80f, 104f), m_MatCommonBg, new Rect(1022f, 1f, 1f, 1f), new Rect(939f, 707f, 80f, 104f), new Rect(1022f, 1f, 1f, 1f), new Vector2(80f, 104f));
			m_CommonBarGroup.Add(control2);
			control2 = UIUtils.BuildClickButton(6002, new Rect(452f, 497f, 76f, 104f), m_MatCommonBg, new Rect(85f, 904f, 76f, 104f), new Rect(85f, 904f, 76f, 104f), new Rect(1022f, 1f, 1f, 1f), new Vector2(76f, 104f));
			m_CommonBarGroup.Add(control2);
			control2 = UIUtils.BuildClickButton(6003, new Rect(528f, 497f, 80f, 104f), m_MatCommonBg, new Rect(1022f, 1f, 1f, 1f), new Rect(160f, 904f, 80f, 104f), new Rect(1022f, 1f, 1f, 1f), new Vector2(80f, 104f));
			m_CommonBarGroup.Add(control2);
			control2 = UIUtils.BuildClickButton(6004, new Rect(603f, 503f, 80f, 104f), m_MatCommonBg, new Rect(1022f, 1f, 1f, 1f), new Rect(834f, 700f, 80f, 104f), new Rect(1022f, 1f, 1f, 1f), new Vector2(80f, 104f));
			control2.Enable = false;
			m_CommonBarGroup.Add(control2);
			control2 = UIUtils.BuildClickButton(6005, new Rect(683f, 497f, 74f, 104f), m_MatCommonBg, new Rect(1022f, 1f, 1f, 1f), new Rect(240f, 904f, 74f, 104f), new Rect(1022f, 1f, 1f, 1f), new Vector2(74f, 104f));
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
			UIClickButton control3 = UIUtils.BuildClickButton(6010, new Rect(320f, 588f, 640f, 52f), m_MatDialog01, new Rect(0f, 798f, 640f, 52f), new Rect(0f, 850f, 640f, 52f), new Rect(0f, 798f, 640f, 52f), new Vector2(640f, 52f));
			m_AroundUIGroup.Add(control3);
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
			UIText uIText = UIUtils.BuildUIText(0, new Rect(75f, 536f, 500f, 20f), UIText.enAlignStyle.left);
			uIText.Set("Zombie3D/Font/037-CAI978-15", ConfigManager.Instance().GetFixedConfig().GetRankName(GameApp.GetInstance().GetGameState().GetPlayerLevel()), new Color(0.81960785f, 0.5294118f, 2f / 51f, 1f));
			m_JunjieUIGroup.Add(uIText);
			uIText = UIUtils.BuildUIText(0, new Rect(170f, 500f, 150f, 20f), UIText.enAlignStyle.left);
			uIText.Set("Zombie3D/Font/037-CAI978-15", "HP: +" + GameApp.GetInstance().GetGameState().GetLevelHpAffect(GameApp.GetInstance().GetGameState().GetPlayerLevel()), new Color(0.81960785f, 0.5294118f, 2f / 51f, 1f));
			m_JunjieUIGroup.Add(uIText);
			int nextRankLevel = ConfigManager.Instance().GetFixedConfig().GetNextRankLevel(GameApp.GetInstance().GetGameState().GetPlayerLevel());
			if (nextRankLevel <= 230)
			{
				uIText = UIUtils.BuildUIText(0, new Rect(70f, 450f, 200f, 34f), UIText.enAlignStyle.left);
				uIText.Set("Zombie3D/Font/037-CAI978-13", "Reach the next Rank at Lv " + nextRankLevel, new Color(0.81960785f, 0.5294118f, 2f / 51f, 1f));
				m_JunjieUIGroup.Add(uIText);
			}
			UIClickButton control2 = UIUtils.BuildClickButton(6009, new Rect(244f, 576f, 75f, 52f), m_MatCommonBg, new Rect(1020f, 1f, 1f, 1f), new Rect(1020f, 1f, 1f, 1f), new Rect(1020f, 1f, 1f, 1f), new Vector2(75f, 52f));
			m_JunjieUIGroup.Add(control2);
			control2 = UIUtils.BuildClickButton(6009, new Rect(50f, 420f, 226f, 150f), m_MatCommonBg, new Rect(1020f, 1f, 1f, 1f), new Rect(1020f, 1f, 1f, 1f), new Rect(1020f, 1f, 1f, 1f), new Vector2(226f, 150f));
			m_JunjieUIGroup.Add(control2);
		}
	}

	public void SetupFriendsUI(bool bShow)
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
		m_uiGroup = new uiGroup(m_UIManager);
		SetupCommonBarUI(true);
		UIMoveOuter control = UIUtils.BuildUIMoveOuter(6011, new Rect(0f, 0f, 393f, 575f), 10f, 10f);
		m_uiGroup.Add(control);
		UIImage control2 = UIUtils.BuildImage(0, new Rect(362f, 50f, 564f, 374f), m_MatFriendsUI, new Rect(0f, 68f, 564f, 374f), new Vector2(564f, 374f));
		m_uiGroup.Add(control2);
		control2 = UIUtils.BuildImage(0, new Rect(615f, 412f, 323f, 80f), m_MatFriendsUI, new Rect(454f, 528f, 323f, 80f), new Vector2(323f, 80f));
		m_uiGroup.Add(control2);
		UIClickButton uIClickButton = null;
		if (m_bFriendsOrHired)
		{
			uIClickButton = UIUtils.BuildClickButton(6017, new Rect(369f, 419f, 123f, 55f), m_MatFriendsUI, new Rect(485f, 472f, 123f, 55f), new Rect(613f, 472f, 123f, 55f), new Rect(485f, 472f, 123f, 55f), new Vector2(123f, 55f));
			uIClickButton.Enable = false;
			m_uiGroup.Add(uIClickButton);
		}
		else
		{
			uIClickButton = UIUtils.BuildClickButton(6017, new Rect(369f, 419f, 123f, 55f), m_MatFriendsUI, new Rect(613f, 472f, 123f, 55f), new Rect(485f, 472f, 123f, 55f), new Rect(613f, 472f, 123f, 55f), new Vector2(123f, 55f));
			uIClickButton.Enable = false;
			m_uiGroup.Add(uIClickButton);
		}
		if (m_bFriendsOrHired)
		{
			if (gameState.LoginType == GameLoginType.LoginType_Local)
			{
				SetupLocalGameRetryDialog(true);
			}
			else if (gameState.LoginType == GameLoginType.LoginType_GameCenter)
			{
				if (m_bLoadingGameCenterFriendsError)
				{
					SetupLocalGameRetryDialog(true);
				}
				else
				{
					SetupFriendPageView();
				}
			}
			else
			{
				SetupFriendPageView();
			}
		}
		else if (gameState.GetHiredFriends().Count > 0)
		{
			SetupHiredFriendPageView();
		}
		else
		{
			SetupDonnotHaveHiredFriendDialog(true);
		}
		SetupBattleFriendInfo(true);
	}

	public void SetupFriendPageView()
	{
		Debug.Log("SetupFriendPageView - " + gameState.m_SelectFriendIndex + " | " + gameState.m_SelectHiredFriendIndex);
		UIClickButton uIClickButton = null;
		UIImage uIImage = null;
		UIText uIText = null;
		if (m_FriendsPageViewScrollBar != null)
		{
			m_uiGroup.Remove(m_FriendsPageViewScrollBar);
		}
		m_FriendsPageViewScrollBar = new UIScrollBar();
		m_FriendsPageViewScrollBar.ScrollOri = UIScrollBar.ScrollOrientation.Vertical;
		m_FriendsPageViewScrollBar.Rect = AutoUI.AutoRect(new Rect(860f, 105f, 20f, 274f));
		m_FriendsPageViewScrollBar.SetScrollBarTexture(m_MatFriendsUI, AutoUI.AutoRect(new Rect(564f, 0f, 20f, 274f)), m_MatFriendsUI, AutoUI.AutoRect(new Rect(564f, 274f, 20f, 86f)));
		m_FriendsPageViewScrollBar.SetSliderSize(AutoUI.AutoSize(new Vector2(20f, 86f)));
		m_FriendsPageViewScrollBar.SetScrollPercent(0f);
		m_uiGroup.Add(m_FriendsPageViewScrollBar);
		float num = 0f;
		if (m_FriendsPageView != null)
		{
			num = m_FriendsPageView.ScrollPosV;
			m_uiGroup.Remove(m_FriendsPageView);
		}
		m_FriendsPageView = new UIScrollView();
		m_FriendsPageView.SetMoveParam(AutoUI.AutoRect(new Rect(362f, 50f, 598f, 373f)), AutoUI.AutoDistance(20f), AutoUI.AutoDistance(20f));
		m_FriendsPageView.Rect = AutoUI.AutoRect(new Rect(414f, 86f, 435f, 290f));
		m_FriendsPageView.ScrollOri = UIScrollView.ScrollOrientation.Vertical;
		m_FriendsPageView.ListOri = UIScrollView.ListOrientation.Vertical;
		m_FriendsPageView.ItemSpacingV = AutoUI.AutoDistance(4f);
		m_FriendsPageView.ItemSpacingH = AutoUI.AutoDistance(0f);
		m_FriendsPageView.SetClip(AutoUI.AutoRect(new Rect(414f, 80f, 435f, 296f)));
		m_FriendsPageView.Bounds = AutoUI.AutoRect(new Rect(414f, 80f, 435f, 296f));
		m_FriendsPageView.ScrollBar = m_FriendsPageViewScrollBar;
		m_uiGroup.Add(m_FriendsPageView);
		ArrayList friends = gameState.GetFriends();
		string text = friends.Count.ToString();
		m_uiGroup.Remove(m_CountPrefix);
		m_CountPrefix = UIUtils.BuildUIText(0, new Rect(380f, 380f, 300f, 40f), UIText.enAlignStyle.left);
		m_CountPrefix.Set("Zombie3D/Font/037-CAI978-22", text, new Color(66f / 85f, 0.6509804f, 0.5294118f, 1f));
		m_uiGroup.Add(m_CountPrefix);
		float textWidth = mgrFont.Instance().getFont("Zombie3D/Font/037-CAI978-22").GetTextWidth(text);
		float left = 380f + textWidth;
		if (AutoUI.IsRetain == AutoUI.RESOLUTION.LOWDEFINITION)
		{
			left = 380f + textWidth + 10f;
		}
		m_uiGroup.Remove(m_CountSuffix);
		m_CountSuffix = UIUtils.BuildUIText(0, new Rect(left, 380f, 300f, 40f), UIText.enAlignStyle.left);
		m_CountSuffix.Set("Zombie3D/Font/037-CAI978-18", "/" + 100, new Color(66f / 85f, 0.6509804f, 0.5294118f, 1f));
		m_uiGroup.Add(m_CountSuffix);
		for (int i = 0; i < friends.Count; i++)
		{
			FriendUserData friendUserData = (FriendUserData)friends[i];
			UIGroupControl uIGroupControl = UIUtils.BuildUIGroupControl(0, new Rect(0f, 0f, 435f, 72f));
			float num2 = 414f;
			float num3 = 330 + 68 * i;
			uIImage = UIUtils.BuildImage(0, new Rect(25f, 0f, 377f, 68f), m_MatFriendsUI, new Rect(586f, 68f, 377f, 68f), new Vector2(377f, 68f));
			uIGroupControl.Add(uIImage);
			if (i == m_CurSelectFriendIndexShow)
			{
				if (gameState.m_SelectFriendIndex != i)
				{
					uIImage = UIUtils.BuildImage(0, new Rect(25f, 0f, 377f, 68f), m_MatFriendsUI, new Rect(586f, 0f, 377f, 68f), new Vector2(377f, 68f));
					uIGroupControl.Add(uIImage);
				}
			}
			else
			{
				uIClickButton = UIUtils.BuildClickButton(6019 + i, new Rect(25f, 0f, 377f, 68f), m_MatFriendsUI, new Rect(1023f, 1f, 1f, 1f), new Rect(586f, 0f, 377f, 68f), new Rect(1023f, 1f, 1f, 1f), new Vector2(377f, 68f));
				uIGroupControl.Add(uIClickButton);
			}
			if (gameState.m_SelectFriendIndex == i)
			{
				uIImage = UIUtils.BuildImage(0, new Rect(25f, 0f, 377f, 68f), m_MatFriendsUI, new Rect(586f, 217f, 377f, 68f), new Vector2(377f, 68f));
				uIImage.CatchMessage = false;
				uIGroupControl.Add(uIImage);
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
			uIText = UIUtils.BuildUIText(0, new Rect(104f, 35f, 200f, 20f), UIText.enAlignStyle.left);
			uIText.Set("Zombie3D/Font/037-CAI978-18", friendUserData.m_Name, new Color(66f / 85f, 0.6509804f, 0.5294118f, 1f));
			uIGroupControl.Add(uIText);
			if (i > 0)
			{
				uIText = UIUtils.BuildUIText(0, new Rect(104f, 10f, 200f, 20f), UIText.enAlignStyle.left);
				uIText.Set("Zombie3D/Font/037-CAI978-13", "LV " + friendUserData.m_Level, new Color(66f / 85f, 0.6509804f, 0.5294118f, 1f));
				uIGroupControl.Add(uIText);
			}
			string text2 = string.Empty;
			if (i == 0)
			{
				text2 = string.Empty;
			}
			else if (i == 1)
			{
				text2 = "Hp + 5%";
			}
			else if (i == 2)
			{
				text2 = "Def + 2%";
			}
			else if (i == 3)
			{
				text2 = "Sta + 5";
			}
			else if (i == 4)
			{
				text2 = "Spd + 5%";
			}
			else if (i == 5)
			{
				text2 = "Dmg + 5";
			}
			else if (i > 5)
			{
				text2 = "Hp + " + (i - 5);
			}
			if (text2 != string.Empty)
			{
				uIText = UIUtils.BuildUIText(0, new Rect(204f, 10f, 200f, 20f), UIText.enAlignStyle.left);
				uIText.Set("Zombie3D/Font/037-CAI978-13", text2, Color.green);
				uIGroupControl.Add(uIText);
			}
			if (gameState.m_SelectFriendIndex == i)
			{
				uIImage = UIUtils.BuildImage(0, new Rect(280f, 0f, 114f, 72f), m_MatFriendsUI, new Rect(675f, 136f, 114f, 72f), new Vector2(114f, 72f));
				uIImage.CatchMessage = false;
				uIGroupControl.Add(uIImage);
			}
			else
			{
				uIClickButton = UIUtils.BuildClickButton(6275 + i, new Rect(293f, 12f, 85f, 40f), m_MatFriendsUI, new Rect(586f, 136f, 85f, 40f), new Rect(586f, 176f, 85f, 40f), new Rect(586f, 136f, 85f, 40f), new Vector2(85f, 40f));
				uIGroupControl.Add(uIClickButton);
			}
			m_FriendsPageView.Add(uIGroupControl);
		}
		if (num > 0f)
		{
			m_FriendsPageView.ScrollPosV = num;
		}
	}

	public void SetupHiredFriendPageView()
	{
		Debug.Log("SetupHiredFriendPageView - " + gameState.m_SelectFriendIndex + " | " + gameState.m_SelectHiredFriendIndex);
		UIClickButton uIClickButton = null;
		UIImage uIImage = null;
		UIText uIText = null;
		if (m_FriendsPageViewScrollBar != null)
		{
			m_uiGroup.Remove(m_FriendsPageViewScrollBar);
		}
		m_FriendsPageViewScrollBar = new UIScrollBar();
		m_FriendsPageViewScrollBar.ScrollOri = UIScrollBar.ScrollOrientation.Vertical;
		m_FriendsPageViewScrollBar.Rect = AutoUI.AutoRect(new Rect(860f, 105f, 20f, 274f));
		m_FriendsPageViewScrollBar.SetScrollBarTexture(m_MatFriendsUI, AutoUI.AutoRect(new Rect(564f, 0f, 20f, 274f)), m_MatFriendsUI, AutoUI.AutoRect(new Rect(564f, 274f, 20f, 86f)));
		m_FriendsPageViewScrollBar.SetSliderSize(AutoUI.AutoSize(new Vector2(20f, 86f)));
		m_FriendsPageViewScrollBar.SetScrollPercent(0f);
		m_uiGroup.Add(m_FriendsPageViewScrollBar);
		float num = 0f;
		if (m_FriendsPageView != null)
		{
			num = m_FriendsPageView.ScrollPosV;
			m_uiGroup.Remove(m_FriendsPageView);
		}
		m_FriendsPageView = new UIScrollView();
		m_FriendsPageView.SetMoveParam(AutoUI.AutoRect(new Rect(362f, 50f, 598f, 373f)), AutoUI.AutoDistance(20f), AutoUI.AutoDistance(20f));
		m_FriendsPageView.Rect = AutoUI.AutoRect(new Rect(414f, 86f, 435f, 290f));
		m_FriendsPageView.ScrollOri = UIScrollView.ScrollOrientation.Vertical;
		m_FriendsPageView.ListOri = UIScrollView.ListOrientation.Vertical;
		m_FriendsPageView.ItemSpacingV = AutoUI.AutoDistance(4f);
		m_FriendsPageView.ItemSpacingH = AutoUI.AutoDistance(0f);
		m_FriendsPageView.SetClip(AutoUI.AutoRect(new Rect(414f, 80f, 435f, 296f)));
		m_FriendsPageView.Bounds = AutoUI.AutoRect(new Rect(414f, 80f, 435f, 296f));
		m_FriendsPageView.ScrollBar = m_FriendsPageViewScrollBar;
		m_uiGroup.Add(m_FriendsPageView);
		List<KeyValuePair<FriendUserData, long>> hiredFriends = gameState.GetHiredFriends();
		string text = hiredFriends.Count.ToString();
		m_uiGroup.Remove(m_CountPrefix);
		m_CountPrefix = UIUtils.BuildUIText(0, new Rect(380f, 380f, 300f, 40f), UIText.enAlignStyle.left);
		m_CountPrefix.Set("Zombie3D/Font/037-CAI978-22", text, new Color(66f / 85f, 0.6509804f, 0.5294118f, 1f));
		m_uiGroup.Add(m_CountPrefix);
		float textWidth = mgrFont.Instance().getFont("Zombie3D/Font/037-CAI978-22").GetTextWidth(text);
		float left = 380f + textWidth;
		if (AutoUI.IsRetain == AutoUI.RESOLUTION.LOWDEFINITION)
		{
			left = 380f + textWidth + 10f;
		}
		m_uiGroup.Remove(m_CountSuffix);
		m_CountSuffix = UIUtils.BuildUIText(0, new Rect(left, 380f, 300f, 40f), UIText.enAlignStyle.left);
		m_CountSuffix.Set("Zombie3D/Font/037-CAI978-18", "/5", new Color(66f / 85f, 0.6509804f, 0.5294118f, 1f));
		m_uiGroup.Add(m_CountSuffix);
		for (int i = 0; i < hiredFriends.Count; i++)
		{
			FriendUserData key = hiredFriends[i].Key;
			UIGroupControl uIGroupControl = UIUtils.BuildUIGroupControl(0, new Rect(0f, 0f, 435f, 72f));
			float num2 = 414f;
			float num3 = 330 + 68 * i;
			uIImage = UIUtils.BuildImage(0, new Rect(25f, 0f, 377f, 68f), m_MatFriendsUI, new Rect(586f, 68f, 377f, 68f), new Vector2(377f, 68f));
			uIGroupControl.Add(uIImage);
			if (i == m_CurSelectHiredFriendIndexShow)
			{
				if (gameState.m_SelectHiredFriendIndex != i)
				{
					uIImage = UIUtils.BuildImage(0, new Rect(25f, 0f, 377f, 68f), m_MatFriendsUI, new Rect(586f, 0f, 377f, 68f), new Vector2(377f, 68f));
					uIGroupControl.Add(uIImage);
				}
			}
			else
			{
				uIClickButton = UIUtils.BuildClickButton(6019 + i, new Rect(25f, 0f, 377f, 68f), m_MatFriendsUI, new Rect(1023f, 1f, 1f, 1f), new Rect(586f, 0f, 377f, 68f), new Rect(1023f, 1f, 1f, 1f), new Vector2(377f, 68f));
				uIGroupControl.Add(uIClickButton);
			}
			if (gameState.m_SelectHiredFriendIndex == i)
			{
				uIImage = UIUtils.BuildImage(0, new Rect(25f, 0f, 377f, 68f), m_MatFriendsUI, new Rect(586f, 217f, 377f, 68f), new Vector2(377f, 68f));
				uIImage.CatchMessage = false;
				uIGroupControl.Add(uIImage);
			}
			Vector2 vector = new Vector2(63f, 38f);
			Vector2 rect_size = new Vector2(70f, 70f);
			Rect avatarIconTexture = ShopUIScript.GetAvatarIconTexture((Avatar.AvatarSuiteType)key.m_AvatarHeadSuiteType, Avatar.AvatarType.Head);
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
			uIText = UIUtils.BuildUIText(0, new Rect(104f, 20f, 200f, 20f), UIText.enAlignStyle.left);
			uIText.Set("Zombie3D/Font/037-CAI978-13", "LV " + key.m_Level, new Color(66f / 85f, 0.6509804f, 0.5294118f, 1f));
			uIGroupControl.Add(uIText);
			if (gameState.m_SelectHiredFriendIndex == i)
			{
				uIImage = UIUtils.BuildImage(0, new Rect(280f, 0f, 114f, 72f), m_MatFriendsUI, new Rect(675f, 136f, 114f, 72f), new Vector2(114f, 72f));
				uIImage.CatchMessage = false;
				uIGroupControl.Add(uIImage);
			}
			else
			{
				uIClickButton = UIUtils.BuildClickButton(6275 + i, new Rect(293f, 12f, 85f, 40f), m_MatFriendsUI, new Rect(586f, 136f, 85f, 40f), new Rect(586f, 176f, 85f, 40f), new Rect(586f, 136f, 85f, 40f), new Vector2(85f, 40f));
				uIGroupControl.Add(uIClickButton);
			}
			m_FriendsPageView.Add(uIGroupControl);
		}
		if (num > 0f)
		{
			m_FriendsPageView.ScrollPosV = num;
		}
		uIClickButton = UIUtils.BuildClickButton(6018, new Rect(530f, 30f, 204f, 75f), m_MatFriendsUI, new Rect(790f, 532f, 204f, 75f), new Rect(790f, 610f, 204f, 75f), new Rect(790f, 532f, 204f, 75f), new Vector2(204f, 75f));
		m_uiGroup.Add(uIClickButton);
	}

	public void SetupBattleFriendInfo(bool bShow)
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
		ArrayList battleFriends = gameState.GetBattleFriends();
		if (battleFriends.Count <= 0)
		{
			return;
		}
		FriendUserData friendUserData = battleFriends[0] as FriendUserData;
		Vector2 vector = new Vector2(660f, 464f);
		Vector2 rect_size = new Vector2(100f, 100f);
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
		UIImage control = UIUtils.BuildImage(0, new Rect(vector.x - rect_size.x / 2f, vector.y - rect_size.y / 2f, rect_size.x, rect_size.y), m_MatAvatarIcons, avatarIconTexture, rect_size);
		m_uiBattleFriendInfo.Add(control);
		string text = friendUserData.m_Name;
		if (text.Length > 5)
		{
			string text2 = text;
			text = text2.Substring(0, 5) + "...";
		}
		UIText uIText = UIUtils.BuildUIText(0, new Rect(705f, 430f, 200f, 40f), UIText.enAlignStyle.left);
		uIText.Set("Zombie3D/Font/037-CAI978-22", text, Constant.TextCommonColor);
		m_uiBattleFriendInfo.Add(uIText);
		if (gameState.m_SelectFriendIndex != 0)
		{
			uIText = UIUtils.BuildUIText(0, new Rect(844f, 430f, 200f, 40f), UIText.enAlignStyle.left);
			uIText.Set("Zombie3D/Font/037-CAI978-22", "LV " + friendUserData.m_Level, Constant.TextCommonColor);
			m_uiBattleFriendInfo.Add(uIText);
		}
		WeaponType weapon_type = friendUserData.m_BattleWeapons[0];
		m_PlayerShow.ChangeWeapon(weapon_type);
		Avatar avatar = new Avatar((Avatar.AvatarSuiteType)friendUserData.m_AvatarHeadSuiteType, Avatar.AvatarType.Head);
		m_PlayerShow.ChangeAvatar(avatar.SuiteType, avatar.AvtType);
		Avatar avatar2 = new Avatar((Avatar.AvatarSuiteType)friendUserData.m_AvatarBodySuiteType, Avatar.AvatarType.Body);
		m_PlayerShow.ChangeAvatar(avatar2.SuiteType, avatar2.AvtType);
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
			UIClickButton control = UIUtils.BuildClickButton(6012, new Rect(530f, 30f, 204f, 75f), m_MatFriendsUI, new Rect(586f, 285f, 204f, 75f), new Rect(790f, 285f, 204f, 75f), new Rect(586f, 285f, 204f, 75f), new Vector2(204f, 75f));
			m_LocalGameDialogUI.Add(control);
		}
	}

	public void SetupDonnotHaveHiredFriendDialog(bool bShow)
	{
		if (m_LocalGameDialogUI != null)
		{
			m_LocalGameDialogUI.Clear();
			m_LocalGameDialogUI = null;
		}
		if (bShow)
		{
			m_LocalGameDialogUI = new uiGroup(m_UIManager);
			UIText uIText = UIUtils.BuildUIText(0, new Rect(437f, 240f, 440f, 40f), UIText.enAlignStyle.left);
			uIText.Set("Zombie3D/Font/037-CAI978-18", "You haven't hired any mercenaries yet. Visit the Merc Camp now to find one that suits your fancy!", Constant.TextCommonColor);
			m_LocalGameDialogUI.Add(uIText);
			UIClickButton control = UIUtils.BuildClickButton(6018, new Rect(530f, 30f, 204f, 75f), m_MatFriendsUI, new Rect(790f, 532f, 204f, 75f), new Rect(790f, 610f, 204f, 75f), new Rect(790f, 532f, 204f, 75f), new Vector2(204f, 75f));
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
			uIClickButton = UIUtils.BuildClickButton(6013, new Rect(410f, 110f, 191f, 62f), m_MatDialog01, new Rect(640f, 0f, 191f, 62f), new Rect(832f, 0f, 191f, 62f), new Rect(640f, 0f, 191f, 62f), new Vector2(191f, 62f));
			m_LocalGameDialog2UI.Add(uIClickButton);
			uIClickButton = UIUtils.BuildClickButton(6014, new Rect(666f, 110f, 191f, 62f), m_MatDialog01, new Rect(640f, 186f, 191f, 62f), new Rect(832f, 186f, 191f, 62f), new Rect(640f, 186f, 191f, 62f), new Vector2(191f, 62f));
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
			uIClickButton = UIUtils.BuildClickButton(6015, new Rect(385f, 215f, 191f, 62f), m_MatDialog01, new Rect(640f, 0f, 191f, 62f), new Rect(832f, 0f, 191f, 62f), new Rect(640f, 0f, 191f, 62f), new Vector2(191f, 62f));
			m_DialogUI.Add(uIClickButton);
		}
	}

	public void SetupLoadingUI(bool bShow)
	{
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
