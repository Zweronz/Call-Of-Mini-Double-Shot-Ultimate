using System;
using System.Collections;
using UnityEngine;
using Zombie3D;

public class OptionUIScript : MonoBehaviour, UIHandler
{
	public enum Controls
	{
		kIDLevels = 7000,
		kIDBoost = 7001,
		kIDFriends = 7002,
		kIDShop = 7003,
		kIDTChat = 7004,
		kIDOptions = 7005,
		kIDCup = 7006,
		kIDTopList = 7007,
		kIDJunjie = 7008,
		kIDJunjieClose = 7009,
		kIDGlobalBank = 7010,
		kIDOptionMusic = 7011,
		kIDOptionSfx = 7012,
		kIDOptionCameraType = 7013,
		kIDOptionLogout = 7014,
		kIDOptionTeam = 7015,
		kIDOptionSave = 7016,
		kIDOptionUpdate = 7017,
		kIDOptionShare = 7018,
		kIDOptionReview = 7019,
		kIDOptionSupport = 7020,
		kIDOptionHowTo = 7021,
		kIDOptionStory = 7022,
		kIDOptionForum = 7023,
		kIDOptionStory1 = 7024,
		kIDOptionStory2 = 7025,
		kIDLogoutConfirmOK = 7026,
		kIDLogoutConfirmLater = 7027,
		kIDNotificationDialogOK = 7028,
		kIDLast = 7029
	}

	private UIManager m_UIManager;

	private string m_ui_material_path;

	protected Material m_MatCommonBg;

	protected Material m_MatOptionUI;

	protected Material m_MatOptionUI01;

	protected Material m_MatDialog01;

	public uiGroup m_uiGroup;

	public uiGroup m_CommonBarGroup;

	public uiGroup m_AroundUIGroup;

	public uiGroup m_OptionDetailGroup;

	public uiGroup m_JunjieUIGroup;

	public uiGroup m_DialogUIGroup;

	public uiGroup m_DialogUI;

	public uiGroup m_CartoonGroup;

	private UICartoonAnimControl m_CartoonAnim;

	protected int m_BattleFriendSelectIndex;

	protected float lastUpdateTime;

	protected bool uiInited;

	private int mapIndex = 1;

	private int pointsIndex = 1;

	private int waveIndex = 1;

	protected float joystickBgImgAlpha = 0.3f;

	private ArrayList m_OptionSelBtnGroup;

	private void Start()
	{
		OpenClickPlugin.Show(false);
		m_UIManager = base.gameObject.AddComponent<UIManager>() as UIManager;
		m_UIManager.SetParameter(8, 3, false);
		m_UIManager.SetUIHandler(this);
		UIUtils.BuildIpone5Frame(m_UIManager);
		m_MatCommonBg = LoadUIMaterial("Zombie3D/UI/Materials/CommonBgUI");
		m_MatOptionUI = LoadUIMaterial("Zombie3D/UI/Materials/OptionUI");
		m_MatOptionUI01 = LoadUIMaterial("Zombie3D/UI/Materials/OptionUI01");
		m_MatDialog01 = LoadUIMaterial("Zombie3D/UI/Materials/Dialog01");
		Resources.UnloadUnusedAssets();
		uiInited = true;
		SetupOptionsUI(true);
		GameCollectionInfoManager.Instance().GetCurrentInfo().AddUIEnterLog(GameCollectionInfo.enUIEnterIndex.OptionUI);
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
		lastUpdateTime = Time.time;
		if (m_CartoonAnim != null && m_CartoonAnim.PlayEnd)
		{
			m_CartoonAnim.Enable = false;
			m_CartoonAnim.Visible = false;
			if (m_OptionDetailGroup != null)
			{
				m_OptionDetailGroup.Remove(m_CartoonAnim);
			}
			m_CartoonAnim = null;
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
		if (control.Id == 7000)
		{
			SceneUIManager.Instance().ChangeSceneUI(SceneUIManager.SceneUI.ChoosePointsUI);
		}
		else if (control.Id == 7001)
		{
			SceneUIManager.Instance().ChangeSceneUI(SceneUIManager.SceneUI.BoostUI);
		}
		else if (control.Id == 7002)
		{
			SceneUIManager.Instance().ChangeSceneUI(SceneUIManager.SceneUI.FriendUI);
		}
		else if (control.Id == 7003)
		{
			SceneUIManager.Instance().ChangeSceneUI(SceneUIManager.SceneUI.ShopUI);
		}
		else
		{
			if (control.Id == 7004 || control.Id == 7005)
			{
				return;
			}
			if (control.Id == 7006)
			{
				SetupNotificationDialogUI(true, "You need to log in to Game Center to proceed!");
			}
			else if (control.Id == 7007)
			{
				SetupNotificationDialogUI(true, "You need to log in to Game Center to proceed!");
			}
			else
			{
				if (control.Id == 7008 || control.Id == 7009)
				{
					return;
				}
				if (control.Id == 7011)
				{
					GameApp.GetInstance().GetGameState().MusicOn = !GameApp.GetInstance().GetGameState().MusicOn;
					if (GameApp.GetInstance().GetGameState().MusicOn)
					{
						((UIClickButton)control).SetTexture(UIButtonBase.State.Normal, m_MatOptionUI, AutoUI.AutoRect(new Rect(0f, 0f, 301f, 75f)));
						((UIClickButton)control).SetTexture(UIButtonBase.State.Pressed, m_MatOptionUI, AutoUI.AutoRect(new Rect(301f, 0f, 301f, 75f)));
					}
					else
					{
						((UIClickButton)control).SetTexture(UIButtonBase.State.Normal, m_MatOptionUI, AutoUI.AutoRect(new Rect(301f, 0f, 301f, 75f)));
						((UIClickButton)control).SetTexture(UIButtonBase.State.Pressed, m_MatOptionUI, AutoUI.AutoRect(new Rect(0f, 0f, 301f, 75f)));
					}
					HandleOption(7011);
				}
				else if (control.Id == 7012)
				{
					GameApp.GetInstance().GetGameState().SoundOn = !GameApp.GetInstance().GetGameState().SoundOn;
					if (GameApp.GetInstance().GetGameState().SoundOn)
					{
						((UIClickButton)control).SetTexture(UIButtonBase.State.Normal, m_MatOptionUI, AutoUI.AutoRect(new Rect(0f, 75f, 301f, 77f)));
						((UIClickButton)control).SetTexture(UIButtonBase.State.Pressed, m_MatOptionUI, AutoUI.AutoRect(new Rect(301f, 75f, 301f, 77f)));
					}
					else
					{
						((UIClickButton)control).SetTexture(UIButtonBase.State.Normal, m_MatOptionUI, AutoUI.AutoRect(new Rect(301f, 75f, 301f, 77f)));
						((UIClickButton)control).SetTexture(UIButtonBase.State.Pressed, m_MatOptionUI, AutoUI.AutoRect(new Rect(0f, 75f, 301f, 77f)));
					}
					HandleOption(7012);
				}
				else if (control.Id == 7013)
				{
					if (GameApp.GetInstance().GetGameState().m_iCameraModeType == 1)
					{
						GameApp.GetInstance().GetGameState().m_iCameraModeType = 2;
					}
					else if (GameApp.GetInstance().GetGameState().m_iCameraModeType == 2)
					{
						GameApp.GetInstance().GetGameState().m_iCameraModeType = 1;
					}
					else
					{
						Debug.Log("CameraType Is 0 !!!");
					}
					if (GameApp.GetInstance().GetGameState().m_iCameraModeType == 2)
					{
						((UIClickButton)control).SetTexture(UIButtonBase.State.Normal, m_MatOptionUI01, AutoUI.AutoRect(new Rect(0f, 435f, 300f, 76f)));
						((UIClickButton)control).SetTexture(UIButtonBase.State.Pressed, m_MatOptionUI01, AutoUI.AutoRect(new Rect(0f, 359f, 300f, 76f)));
					}
					else
					{
						((UIClickButton)control).SetTexture(UIButtonBase.State.Normal, m_MatOptionUI01, AutoUI.AutoRect(new Rect(0f, 359f, 300f, 76f)));
						((UIClickButton)control).SetTexture(UIButtonBase.State.Pressed, m_MatOptionUI01, AutoUI.AutoRect(new Rect(0f, 435f, 300f, 76f)));
					}
					HandleOption(7013);
				}
				else if (control.Id == 7014)
				{
					HandleOption(7014);
				}
				else if (control.Id == 7015)
				{
					HandleOption(7015);
				}
				else if (control.Id == 7016)
				{
					HandleOption(7016);
				}
				else if (control.Id == 7017)
				{
					HandleOption(7017);
				}
				else if (control.Id == 7018)
				{
					HandleOption(7018);
				}
				else if (control.Id == 7023)
				{
					HandleOption(7023);
				}
				else if (control.Id == 7019)
				{
					HandleOption(7019);
				}
				else if (control.Id == 7020)
				{
					HandleOption(7020);
				}
				else if (control.Id == 7021)
				{
					HandleOption(7021);
				}
				else if (control.Id == 7022)
				{
					HandleOption(7022);
				}
				else if (control.Id == 7026)
				{
					GameApp.GetInstance().GetGameState().m_bReLogin = true;
					Application.LoadLevel("Zombie3D_Judgement_GameLogin");
				}
				else if (control.Id == 7027)
				{
					SetupLogoutConfirmDialog(false);
				}
				else if (control.Id == 7028)
				{
					SetupNotificationDialogUI(false, string.Empty);
				}
				else if (control.Id == 7010)
				{
					ShopUIScript.externalShopType = ShopUIScript.ShopType.Bank;
					SceneUIManager.Instance().ChangeSceneUI(SceneUIManager.SceneUI.ShopUI);
				}
				else if (control.Id == 7024)
				{
					if (m_CartoonGroup != null)
					{
						m_CartoonGroup.Clear();
						m_CartoonGroup = null;
					}
					m_CartoonGroup = new uiGroup(m_UIManager);
					m_CartoonAnim = new UICartoonAnimControl();
					m_CartoonGroup.Add(m_CartoonAnim);
				}
				else if (control.Id == 7025)
				{
//					Handheld.PlayFullScreenMovie("Story2.mp4", Color.black, FullScreenMovieControlMode.Hidden);
				}
				else if (control.Id != 7029)
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
			UIClickButton control2 = UIUtils.BuildClickButton(7000, new Rect(295f, 497f, 77f, 104f), m_MatCommonBg, new Rect(1022f, 1f, 1f, 1f), new Rect(9f, 904f, 77f, 104f), new Rect(1022f, 1f, 1f, 1f), new Vector2(77f, 104f));
			m_CommonBarGroup.Add(control2);
			control2 = UIUtils.BuildClickButton(7001, new Rect(372f, 497f, 80f, 104f), m_MatCommonBg, new Rect(1022f, 1f, 1f, 1f), new Rect(939f, 707f, 80f, 104f), new Rect(1022f, 1f, 1f, 1f), new Vector2(80f, 104f));
			m_CommonBarGroup.Add(control2);
			control2 = UIUtils.BuildClickButton(7002, new Rect(452f, 497f, 76f, 104f), m_MatCommonBg, new Rect(1022f, 1f, 1f, 1f), new Rect(85f, 904f, 76f, 104f), new Rect(1022f, 1f, 1f, 1f), new Vector2(76f, 104f));
			m_CommonBarGroup.Add(control2);
			control2 = UIUtils.BuildClickButton(7003, new Rect(528f, 497f, 80f, 104f), m_MatCommonBg, new Rect(1022f, 1f, 1f, 1f), new Rect(160f, 904f, 80f, 104f), new Rect(1022f, 1f, 1f, 1f), new Vector2(80f, 104f));
			m_CommonBarGroup.Add(control2);
			control2 = UIUtils.BuildClickButton(7004, new Rect(603f, 503f, 80f, 104f), m_MatCommonBg, new Rect(1022f, 1f, 1f, 1f), new Rect(834f, 700f, 80f, 104f), new Rect(1022f, 1f, 1f, 1f), new Vector2(80f, 104f));
			control2.Enable = false;
			m_CommonBarGroup.Add(control2);
			control2 = UIUtils.BuildClickButton(7005, new Rect(683f, 497f, 74f, 104f), m_MatCommonBg, new Rect(240f, 904f, 74f, 104f), new Rect(240f, 904f, 74f, 104f), new Rect(1022f, 1f, 1f, 1f), new Vector2(74f, 104f));
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
			UIClickButton control3 = UIUtils.BuildClickButton(7010, new Rect(320f, 588f, 640f, 52f), m_MatDialog01, new Rect(0f, 798f, 640f, 52f), new Rect(0f, 850f, 640f, 52f), new Rect(0f, 798f, 640f, 52f), new Vector2(640f, 52f));
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
			UIClickButton control2 = UIUtils.BuildClickButton(7009, new Rect(244f, 576f, 75f, 52f), m_MatCommonBg, new Rect(1020f, 1f, 1f, 1f), new Rect(1020f, 1f, 1f, 1f), new Rect(1020f, 1f, 1f, 1f), new Vector2(75f, 52f));
			m_JunjieUIGroup.Add(control2);
			control2 = UIUtils.BuildClickButton(7009, new Rect(50f, 420f, 226f, 150f), m_MatCommonBg, new Rect(1020f, 1f, 1f, 1f), new Rect(1020f, 1f, 1f, 1f), new Rect(1020f, 1f, 1f, 1f), new Vector2(226f, 150f));
			m_JunjieUIGroup.Add(control2);
		}
	}

	public void SetupOptionsUI(bool bShow)
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
			m_OptionSelBtnGroup = new ArrayList();
			UIClickButton uIClickButton = null;
			UISelectButton uISelectButton = null;
			UIImage uIImage = null;
			uIImage = UIUtils.BuildImage(0, new Rect(22f, 65f, 474f, 491f), m_MatOptionUI, new Rect(0f, 462f, 474f, 491f), new Vector2(474f, 491f));
			uIImage.CatchMessage = false;
			m_uiGroup.Add(uIImage);
			UIScrollView uIScrollView = new UIScrollView();
			uIScrollView.SetMoveParam(AutoUI.AutoRect(new Rect(512f, 0f, 400f, 500f)), AutoUI.AutoDistance(20f), AutoUI.AutoDistance(20f));
			uIScrollView.Rect = AutoUI.AutoRect(new Rect(566f, 60f, 300f, 400f));
			uIScrollView.ScrollOri = UIScrollView.ScrollOrientation.Vertical;
			uIScrollView.ListOri = UIScrollView.ListOrientation.Vertical;
			uIScrollView.ItemSpacingV = AutoUI.AutoDistance(0f);
			uIScrollView.ItemSpacingH = AutoUI.AutoDistance(0f);
			uIScrollView.SetClip(AutoUI.AutoRect(new Rect(555f, 60f, 300f, 400f)));
			uIScrollView.Bounds = AutoUI.AutoRect(new Rect(555f, 60f, 300f, 400f));
			m_uiGroup.Add(uIScrollView);
			UIGroupControl uIGroupControl = UIUtils.BuildUIGroupControl(0, new Rect(0f, 0f, 301f, 94f));
			uIClickButton = UIUtils.BuildClickButton(7023, new Rect(0f, 18f, 301f, 76f), m_MatOptionUI, new Rect(600f, 773f, 301f, 76f), new Rect(600f, 856f, 301f, 76f), new Rect(600f, 773f, 301f, 76f), new Vector2(301f, 76f));
			uIGroupControl.Add(uIClickButton);
			uIScrollView.Add(uIGroupControl);
			uIGroupControl = UIUtils.BuildUIGroupControl(0, new Rect(0f, 0f, 301f, 94f));
			uIClickButton = UIUtils.BuildClickButton(7020, new Rect(0f, 19f, 301f, 75f), m_MatOptionUI, new Rect(600f, 0f, 301f, 75f), new Rect(600f, 75f, 301f, 75f), new Rect(600f, 75f, 301f, 75f), new Vector2(301f, 75f));
			uIGroupControl.Add(uIClickButton);
			uIScrollView.Add(uIGroupControl);
			uIGroupControl = UIUtils.BuildUIGroupControl(0, new Rect(0f, 0f, 301f, 94f));
			if (GameApp.GetInstance().GetGameState().MusicOn)
			{
				uIClickButton = UIUtils.BuildClickButton(7011, new Rect(0f, 19f, 301f, 75f), m_MatOptionUI, new Rect(0f, 0f, 301f, 75f), new Rect(301f, 0f, 301f, 75f), new Rect(0f, 0f, 301f, 75f), new Vector2(301f, 75f));
				uIGroupControl.Add(uIClickButton);
				uIScrollView.Add(uIGroupControl);
			}
			else
			{
				uIClickButton = UIUtils.BuildClickButton(7011, new Rect(0f, 19f, 301f, 75f), m_MatOptionUI, new Rect(301f, 0f, 301f, 75f), new Rect(0f, 0f, 301f, 75f), new Rect(301f, 0f, 301f, 75f), new Vector2(301f, 75f));
				uIGroupControl.Add(uIClickButton);
				uIScrollView.Add(uIGroupControl);
			}
			uIGroupControl = UIUtils.BuildUIGroupControl(0, new Rect(0f, 0f, 301f, 94f));
			uIClickButton = ((!GameApp.GetInstance().GetGameState().SoundOn) ? UIUtils.BuildClickButton(7012, new Rect(0f, 17f, 301f, 77f), m_MatOptionUI, new Rect(301f, 75f, 301f, 77f), new Rect(0f, 75f, 301f, 77f), new Rect(301f, 75f, 301f, 77f), new Vector2(301f, 77f)) : UIUtils.BuildClickButton(7012, new Rect(0f, 17f, 301f, 77f), m_MatOptionUI, new Rect(0f, 75f, 301f, 77f), new Rect(301f, 75f, 301f, 77f), new Rect(0f, 75f, 301f, 77f), new Vector2(301f, 77f)));
			uIGroupControl.Add(uIClickButton);
			uIScrollView.Add(uIGroupControl);
			uIGroupControl = UIUtils.BuildUIGroupControl(0, new Rect(0f, 0f, 301f, 94f));
			uIClickButton = ((GameApp.GetInstance().GetGameState().m_iCameraModeType != 2) ? UIUtils.BuildClickButton(7013, new Rect(0f, 17f, 301f, 77f), m_MatOptionUI01, new Rect(0f, 359f, 300f, 76f), new Rect(0f, 435f, 300f, 76f), new Rect(0f, 359f, 300f, 76f), new Vector2(301f, 77f)) : UIUtils.BuildClickButton(7013, new Rect(0f, 17f, 301f, 77f), m_MatOptionUI01, new Rect(0f, 435f, 300f, 76f), new Rect(0f, 359f, 300f, 76f), new Rect(0f, 435f, 300f, 76f), new Vector2(301f, 77f)));
			uIGroupControl.Add(uIClickButton);
			uIScrollView.Add(uIGroupControl);
			uIGroupControl = UIUtils.BuildUIGroupControl(0, new Rect(0f, 0f, 301f, 94f));
			uIClickButton = UIUtils.BuildClickButton(7014, new Rect(0f, 17f, 301f, 76f), m_MatOptionUI, new Rect(600f, 152f, 301f, 76f), new Rect(600f, 228f, 301f, 76f), new Rect(600f, 152f, 301f, 76f), new Vector2(301f, 76f));
			uIGroupControl.Add(uIClickButton);
			uIScrollView.Add(uIGroupControl);
			uIGroupControl = UIUtils.BuildUIGroupControl(0, new Rect(0f, 0f, 301f, 94f));
			uISelectButton = UIUtils.BuildSelectButton(7015, new Rect(0f, 18f, 301f, 76f), m_MatOptionUI, new Rect(0f, 152f, 301f, 76f), new Rect(301f, 152f, 301f, 76f), new Rect(0f, 152f, 301f, 76f), new Vector2(301f, 76f));
			m_OptionSelBtnGroup.Add(uISelectButton);
			uIGroupControl.Add(uISelectButton);
			uIScrollView.Add(uIGroupControl);
			uIGroupControl = UIUtils.BuildUIGroupControl(0, new Rect(0f, 0f, 301f, 94f));
			uISelectButton = UIUtils.BuildSelectButton(7016, new Rect(0f, 14f, 301f, 80f), m_MatOptionUI, new Rect(0f, 228f, 301f, 80f), new Rect(301f, 228f, 301f, 80f), new Rect(0f, 228f, 301f, 80f), new Vector2(301f, 80f));
			m_OptionSelBtnGroup.Add(uISelectButton);
			uIGroupControl.Add(uISelectButton);
			uIScrollView.Add(uIGroupControl);
			uIGroupControl = UIUtils.BuildUIGroupControl(0, new Rect(0f, 0f, 301f, 94f));
			uISelectButton = UIUtils.BuildSelectButton(7017, new Rect(0f, 16f, 301f, 78f), m_MatOptionUI, new Rect(600f, 459f, 301f, 78f), new Rect(600f, 536f, 301f, 78f), new Rect(600f, 459f, 301f, 78f), new Vector2(301f, 78f));
			m_OptionSelBtnGroup.Add(uISelectButton);
			uIGroupControl.Add(uISelectButton);
			uIScrollView.Add(uIGroupControl);
			uIGroupControl = UIUtils.BuildUIGroupControl(0, new Rect(0f, 0f, 301f, 94f));
			uIClickButton = UIUtils.BuildClickButton(7019, new Rect(0f, 20f, 301f, 74f), m_MatOptionUI, new Rect(0f, 385f, 301f, 74f), new Rect(301f, 385f, 301f, 74f), new Rect(0f, 385f, 301f, 74f), new Vector2(301f, 74f));
			uIGroupControl.Add(uIClickButton);
			uIScrollView.Add(uIGroupControl);
			uIGroupControl = UIUtils.BuildUIGroupControl(0, new Rect(0f, 0f, 301f, 94f));
			uIClickButton = UIUtils.BuildClickButton(7022, new Rect(0f, 16f, 301f, 78f), m_MatOptionUI, new Rect(600f, 612f, 301f, 78f), new Rect(600f, 690f, 301f, 78f), new Rect(600f, 612f, 301f, 78f), new Vector2(301f, 78f));
			uIGroupControl.Add(uIClickButton);
			uIScrollView.Add(uIGroupControl);
			UIScrollBar uIScrollBar = new UIScrollBar();
			uIScrollBar.ScrollOri = UIScrollBar.ScrollOrientation.Vertical;
			uIScrollBar.Rect = AutoUI.AutoRect(new Rect(870f, 120f, 20f, 274f));
			uIScrollBar.SetScrollBarTexture(m_MatOptionUI, AutoUI.AutoRect(new Rect(1003f, 0f, 20f, 274f)), m_MatOptionUI, AutoUI.AutoRect(new Rect(1003f, 280f, 20f, 86f)));
			uIScrollBar.SetSliderSize(AutoUI.AutoSize(new Vector2(20f, 86f)));
			uIScrollBar.SetScrollPercent(0f);
			m_uiGroup.Add(uIScrollBar);
			uIScrollView.ScrollBar = uIScrollBar;
			HandleOption(7011);
		}
	}

	public void HandleOption(int id)
	{
		for (int i = 0; i < m_OptionSelBtnGroup.Count; i++)
		{
			UISelectButton uISelectButton = (UISelectButton)m_OptionSelBtnGroup[i];
			if (uISelectButton.Id != id)
			{
				uISelectButton.Set(false);
			}
			else
			{
				uISelectButton.Set(true);
			}
		}
		if (m_OptionDetailGroup != null)
		{
			m_OptionDetailGroup.Clear();
			m_OptionDetailGroup = null;
		}
		switch (id)
		{
		case 7011:
		{
			m_OptionDetailGroup = new uiGroup(m_UIManager);
			UIText uIText8 = UIUtils.BuildUIText(0, new Rect(70f, 88f, 400f, 350f), UIText.enAlignStyle.left);
			uIText8.Set("Zombie3D/Font/037-CAI978-18", "Click the button to on or off this background music.", new Color(0.81960785f, 0.5294118f, 2f / 51f, 1f));
			m_OptionDetailGroup.Add(uIText8);
			break;
		}
		case 7012:
		{
			m_OptionDetailGroup = new uiGroup(m_UIManager);
			UIText uIText9 = UIUtils.BuildUIText(0, new Rect(70f, 88f, 400f, 350f), UIText.enAlignStyle.left);
			uIText9.Set("Zombie3D/Font/037-CAI978-18", "Click the button to on or off this sound.", new Color(0.81960785f, 0.5294118f, 2f / 51f, 1f));
			m_OptionDetailGroup.Add(uIText9);
			break;
		}
		case 7013:
		{
			m_OptionDetailGroup = new uiGroup(m_UIManager);
			UIText uIText10 = UIUtils.BuildUIText(0, new Rect(70f, 88f, 400f, 350f), UIText.enAlignStyle.left);
			uIText10.Set("Zombie3D/Font/037-CAI978-18", "The camera angle is now adjustable! Tap the wrench icon on the main menu to change your settings.", new Color(0.81960785f, 0.5294118f, 2f / 51f, 1f));
			m_OptionDetailGroup.Add(uIText10);
			break;
		}
		case 7014:
			SetupLogoutConfirmDialog(true);
			break;
		case 7015:
		{
			m_OptionDetailGroup = new uiGroup(m_UIManager);
			float top = 88f;
			float width = 400f;
			if (AutoUI.IsRetain == AutoUI.RESOLUTION.LOWDEFINITION)
			{
				top = 120f;
				width = 396f;
			}
			UIText uIText7 = UIUtils.BuildUIText(0, new Rect(70f, top, width, 350f), UIText.enAlignStyle.left);
			uIText7.Set("Zombie3D/Font/037-CAI978-18", "1. You need to add friends via Game Center or Facebook account. If your Game Center or Facebook friends play this game, then you can turn their characters to your teammates. Inviting more friends into this game will give you more party options.\n2. You can also share your Game Center or Facebook account to get more friends.\n3. The more friends you invite the higher attributes you could gain.", new Color(0.81960785f, 0.5294118f, 2f / 51f, 1f));
			m_OptionDetailGroup.Add(uIText7);
			break;
		}
		case 7016:
		{
			m_OptionDetailGroup = new uiGroup(m_UIManager);
			UIText uIText6 = UIUtils.BuildUIText(0, new Rect(70f, 88f, 400f, 350f), UIText.enAlignStyle.left);
			uIText6.Set("Zombie3D/Font/037-CAI978-18", "1. At your first network connection, we will create an account for you on our server through your Game Center or Facebook account as well as upload your current character info to our server.\n2. Every time you connect to a network, we will compare your local info and online info and retain and sync the latest info.", new Color(0.81960785f, 0.5294118f, 2f / 51f, 1f));
			m_OptionDetailGroup.Add(uIText6);
			break;
		}
		case 7017:
		{
			m_OptionDetailGroup = new uiGroup(m_UIManager);
			UIText uIText5 = UIUtils.BuildUIText(0, new Rect(70f, 88f, 400f, 350f), UIText.enAlignStyle.left);
			uIText5.Set("Zombie3D/Font/037-CAI978-18", "1. You must update the game to the latest version in order to play online.\n2. More weapons, maps, armor and stages will be added soon.", new Color(0.81960785f, 0.5294118f, 2f / 51f, 1f));
			m_OptionDetailGroup.Add(uIText5);
			break;
		}
		case 7018:
		{
			m_OptionDetailGroup = new uiGroup(m_UIManager);
			UIText uIText4 = UIUtils.BuildUIText(0, new Rect(70f, 88f, 400f, 350f), UIText.enAlignStyle.left);
			uIText4.Set("Zombie3D/Font/037-CAI978-18", "Test test test test.", new Color(0.81960785f, 0.5294118f, 2f / 51f, 1f));
			m_OptionDetailGroup.Add(uIText4);
			break;
		}
		case 7023:
		{
			m_OptionDetailGroup = new uiGroup(m_UIManager);
			UIText uIText3 = UIUtils.BuildUIText(0, new Rect(70f, 88f, 400f, 350f), UIText.enAlignStyle.left);
			uIText3.Set("Zombie3D/Font/037-CAI978-18", string.Empty, new Color(0.81960785f, 0.5294118f, 2f / 51f, 1f));
			m_OptionDetailGroup.Add(uIText3);
			Application.OpenURL("http://www.trinitigame.com/forum/viewforum.php?f=63");
			break;
		}
		case 7019:
		{
			m_OptionDetailGroup = new uiGroup(m_UIManager);
			UIText uIText2 = UIUtils.BuildUIText(0, new Rect(70f, 88f, 400f, 350f), UIText.enAlignStyle.left);
			uIText2.Set("Zombie3D/Font/037-CAI978-18", "Your good rating will inspire us to release updates faster! Please rate now.", new Color(0.81960785f, 0.5294118f, 2f / 51f, 1f));
			m_OptionDetailGroup.Add(uIText2);
			Application.OpenURL("market://details?id=com.trinitigame.android.comds");
			break;
		}
		case 7020:
		{
			string url = "http://www.trinitigame.com/support?game=comds&version=2.0.2";
			Application.OpenURL(url);
			break;
		}
		case 7021:
		{
			m_OptionDetailGroup = new uiGroup(m_UIManager);
			UIText uIText = UIUtils.BuildUIText(0, new Rect(70f, 88f, 400f, 350f), UIText.enAlignStyle.left);
			uIText.Set("Zombie3D/Font/037-CAI978-18", "Test test test test.", new Color(0.81960785f, 0.5294118f, 2f / 51f, 1f));
			m_OptionDetailGroup.Add(uIText);
			break;
		}
		case 7022:
		{
			m_OptionDetailGroup = new uiGroup(m_UIManager);
			UIClickButton control = UIUtils.BuildClickButton(7024, new Rect(82f, 362f, 361f, 77f), m_MatOptionUI01, new Rect(0f, 0f, 361f, 77f), new Rect(0f, 79f, 361f, 77f), new Rect(0f, 0f, 361f, 772f), new Vector2(361f, 77f));
			m_OptionDetailGroup.Add(control);
			control = UIUtils.BuildClickButton(7025, new Rect(82f, 260f, 361f, 77f), m_MatOptionUI01, new Rect(0f, 158f, 361f, 77f), new Rect(0f, 237f, 361f, 77f), new Rect(0f, 158f, 361f, 772f), new Vector2(361f, 77f));
			m_OptionDetailGroup.Add(control);
			break;
		}
		}
	}

	public void SetupLogoutConfirmDialog(bool bShow)
	{
		if (m_DialogUIGroup != null)
		{
			m_DialogUIGroup.Clear();
			m_DialogUIGroup = null;
		}
		if (bShow)
		{
			m_DialogUIGroup = new uiGroup(m_UIManager);
			UIImage control = UIUtils.BuildImage(0, new Rect(0f, 0f, 960f, 640f), m_MatDialog01, new Rect(600f, 1f, 1f, 1f), new Vector2(960f, 640f));
			m_DialogUIGroup.Add(control);
			float left = 230f;
			float top = 170f;
			control = UIUtils.BuildImage(0, new Rect(left, top, 515f, 301f), m_MatDialog01, new Rect(0f, 378f, 515f, 301f), new Vector2(515f, 301f));
			m_DialogUIGroup.Add(control);
			UIText uIText = UIUtils.BuildUIText(0, new Rect(270f, 200f, 440f, 136f), UIText.enAlignStyle.left);
			uIText.Set("Zombie3D/Font/037-CAI978-18", "If you wish to connect to a network again, game will be reloaded. Proceed?", Constant.TextCommonColor);
			m_DialogUIGroup.Add(uIText);
			UIClickButton uIClickButton = null;
			uIClickButton = UIUtils.BuildClickButton(7026, new Rect(510f, 155f, 191f, 62f), m_MatDialog01, new Rect(640f, 0f, 191f, 62f), new Rect(832f, 0f, 191f, 62f), new Rect(640f, 0f, 191f, 62f), new Vector2(191f, 62f));
			m_DialogUIGroup.Add(uIClickButton);
			uIClickButton = UIUtils.BuildClickButton(7027, new Rect(260f, 155f, 191f, 62f), m_MatDialog01, new Rect(640f, 186f, 191f, 62f), new Rect(832f, 186f, 191f, 62f), new Rect(640f, 186f, 191f, 62f), new Vector2(191f, 62f));
			m_DialogUIGroup.Add(uIClickButton);
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
			uIClickButton = UIUtils.BuildClickButton(7028, new Rect(385f, 215f, 191f, 62f), m_MatDialog01, new Rect(640f, 0f, 191f, 62f), new Rect(832f, 0f, 191f, 62f), new Rect(640f, 0f, 191f, 62f), new Vector2(191f, 62f));
			m_DialogUI.Add(uIClickButton);
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
