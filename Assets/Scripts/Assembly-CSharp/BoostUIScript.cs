using System;
using System.Collections.Generic;
using UnityEngine;
using Zombie3D;

public class BoostUIScript : MonoBehaviour, UIHandler
{
	public enum BoostType
	{
		Skill = 0
	}

	public enum Controls
	{
		kIDLevels = 11000,
		kIDBoost = 11001,
		kIDFriends = 11002,
		kIDShop = 11003,
		kIDTChat = 11004,
		kIDOptions = 11005,
		kIDCup = 11006,
		kIDTopList = 11007,
		kIDJunjie = 11008,
		kIDJunjieClose = 11009,
		kIDGlobalBank = 11010,
		kIDSkills = 11011,
		kIDWeapons = 11012,
		kIDArmor = 11013,
		kIDPowerUPS = 11014,
		kIDBank = 11015,
		kIDFilter = 11016,
		kIDBack = 11017,
		kIDPlayerRotateShowControl = 11018,
		kIDSkillItemDetailClose = 11019,
		kIDSkillLearn = 11020,
		kIDSkillUpgrade = 11021,
		kIDSkillActive = 11022,
		kIDSkillLearnOK = 11023,
		kIDSkillLearnLater = 11024,
		kIDSkillUpgradeOK = 11025,
		kIDSkillUpgradeLater = 11026,
		kIDWeaponItemTryOn = 11027,
		kIDWeaponItemBuy = 11028,
		kIDWeaponItemArms = 11029,
		kIDWeaponItemDetailClose = 11030,
		kIDWeaponItemSell = 11031,
		kIDWeaponEquipped01 = 11032,
		kIDWeaponEquipped02 = 11033,
		kIDAvatarItemTryOn = 11034,
		kIDAvatarItemBuy = 11035,
		kIDAvatarItemArms = 11036,
		kIDAvatarItemDetailClose = 11037,
		kIDAvatarItemSell = 11038,
		kIDPowerUPSItemBuy = 11039,
		kIDPowerUPSItemBuyClose = 11040,
		kIDBankItemBuy = 11041,
		kIDBankItemBuyClose = 11042,
		kIDHintDialogOK = 11043,
		kIDHintDialogYes = 11044,
		kIDHintDialogNo = 11045,
		kIDGotoBankLater = 11046,
		kIDGotoBank = 11047,
		kIDGiftDialogOK = 11048,
		kIDReviewDialogOK = 11049,
		kIDReviewDialogLater = 11050,
		kIDNotificationDialogOK = 11051,
		kIDBoostItemBegin = 11052,
		kIDBoostItemLast = 11152,
		kIDPlayerPrograssBtn = 11153,
		kIDUnLockAccouterBtnOK = 11154,
		kIDLevelUpHortationBtnOK = 11155,
		kIDSellsDialogOK = 11156,
		kIDSellsDialogLater = 11157,
		kIDLast = 11158
	}

	private UIManager m_UIManager;

	private string m_ui_material_path;

	protected GameState gameState;

	protected Material m_MatCommonBg;

	protected Material m_MatShopUI;

	protected Material m_MatTransparentUI;

	protected Material m_MatSkillIcons;

	protected Material m_MatWeaponIcons;

	protected Material m_MatAvatarIcons;

	protected Material m_MatPowerUPSIcons;

	protected Material m_MatBankIconUI;

	protected Material m_MatDialog01;

	public uiGroup m_uiGroup;

	public uiGroup m_DialogNoticeUIGroup;

	public uiGroup m_DialogUIGroup;

	public uiGroup m_ShopCommonBarGroup;

	public uiGroup m_AroundUIGroup;

	public uiGroup m_JunjieUIGroup;

	public uiGroup m_uiShopItemDetail;

	public uiGroup m_uiHintDialog;

	public uiGroup m_LoadingUI;

	public uiGroup m_UnLockAccouterDialog;

	public uiGroup m_LevelUpDialog;

	public uiGroup m_DialogSellsUIGroup;

	private static bool m_bReviewShow;

	public static BoostType externalShopType;

	private UIScrollPageView m_BoostSkillPageView;

	private UIDotScrollBar m_ShopPageViewScrollBar;

	protected PlayerUIShow playerShow;

	protected float lastUpdateTime;

	protected bool uiInited;

	protected BoostType m_BoostType;

	protected enSkillType m_SelectedSkillType = enSkillType.FastRun;

	protected Color m_ShopPropColor;

	protected Color m_ShopPropValueColor;

	protected Color m_ShopPropSPDPositiveValueColor;

	protected Color m_ShopPropSPDNegativeValueColor;

	protected Skill m_SkillSelected;

	protected int avatarSelectedIndex;

	protected int powerUpsSelectedIndex;

	protected int iapSelectedIndex = -1;

	protected int exchangeSelectedIndex = -1;

	private UIClickButton playerHpProgressBtn;

	private UIAnimationControl playerHpProgressBarBtnAnim;

	private int _maxSkillLevel = 10;

	private void Start()
	{
		OpenClickPlugin.Hide();
		m_UIManager = base.gameObject.AddComponent<UIManager>() as UIManager;
		m_UIManager.SetParameter(8, 3, false);
		m_UIManager.SetUIHandler(this);
		UIUtils.BuildIpone5Frame(m_UIManager);
		gameState = GameApp.GetInstance().GetGameState();
		m_MatCommonBg = LoadUIMaterial("Zombie3D/UI/Materials/CommonBgUI");
		m_MatShopUI = LoadUIMaterial("Zombie3D/UI/Materials/ShopUI");
		m_MatTransparentUI = LoadUIMaterial("Zombie3D/UI/Materials/TransparentUI");
		m_MatSkillIcons = LoadUIMaterial("Zombie3D/UI/Materials/SkillIcons");
		m_MatWeaponIcons = LoadUIMaterial("Zombie3D/UI/Materials/WeaponIcons");
		m_MatAvatarIcons = LoadUIMaterial("Zombie3D/UI/Materials/AvatarIcons");
		m_MatPowerUPSIcons = LoadUIMaterial("Zombie3D/UI/Materials/PowerUPSIcons");
		m_MatBankIconUI = LoadUIMaterial("Zombie3D/UI/Materials/BankIcons");
		m_MatDialog01 = LoadUIMaterial("Zombie3D/UI/Materials/Dialog01");
		Resources.UnloadUnusedAssets();
		uiInited = true;
		m_ShopPropColor = new Color(0.81960785f, 0.5294118f, 2f / 51f, 1f);
		m_ShopPropValueColor = new Color(1f, 1f, 1f, 1f);
		m_ShopPropSPDPositiveValueColor = new Color(0f, 1f, 0f, 1f);
		m_ShopPropSPDNegativeValueColor = new Color(1f, 0f, 0f, 1f);
		playerShow = SceneUIManager.Instance().ShowPlayerUIDDS(true);
		m_BoostType = BoostType.Skill;
		SetupBoostUI(true);
		GameCollectionInfoManager.Instance().GetCurrentInfo().AddUIEnterLog(GameCollectionInfo.enUIEnterIndex.BoostUI_Skill);
	}

	private void Update()
	{
		if (!(Time.time - lastUpdateTime < 0.001f) && uiInited)
		{
			lastUpdateTime = Time.time;
		}
	}

	private void LateUpdate()
	{
		UITouchInner[] array = (Application.isMobilePlatform) ? iPhoneInputMgr.MockTouches() : WindowsInputMgr.MockTouches();
		foreach (UITouchInner touch in array)
		{
			if (!(m_UIManager != null) || m_UIManager.HandleInput(touch))
			{
			}
		}
	}

	public void HandleEvent(UIControl control, int command, float wparam, float lparam)
	{
		if ((control.GetType() == typeof(UIClickButton) || control.GetType() == typeof(UISelectButton) || control.GetType() == typeof(UIPushButton)) && GameApp.GetInstance().GetGameState().SoundOn)
		{
			SceneUIManager.Instance().PlayClickAudio();
		}
		if (control.Id == 11000)
		{
			playerShow = SceneUIManager.Instance().ShowPlayerUIDDS(false);
			SceneUIManager.Instance().ChangeSceneUI(SceneUIManager.SceneUI.ChoosePointsUI);
		}
		else
		{
			if (control.Id == 11001)
			{
				return;
			}
			if (control.Id == 11002)
			{
				playerShow = SceneUIManager.Instance().ShowPlayerUIDDS(false);
				SceneUIManager.Instance().ChangeSceneUI(SceneUIManager.SceneUI.FriendUI);
			}
			else if (control.Id == 11003)
			{
				playerShow = SceneUIManager.Instance().ShowPlayerUIDDS(false);
				SceneUIManager.Instance().ChangeSceneUI(SceneUIManager.SceneUI.ShopUI);
			}
			else
			{
				if (control.Id == 11004)
				{
					return;
				}
				if (control.Id == 11005)
				{
					playerShow = SceneUIManager.Instance().ShowPlayerUIDDS(false);
					SceneUIManager.Instance().ChangeSceneUI(SceneUIManager.SceneUI.OptionUI);
				}
				else if (control.Id == 11006)
				{
					SetupNotificationDialogUI(true, "You need to log in to Game Center to proceed!");
				}
				else if (control.Id == 11007)
				{
					SetupNotificationDialogUI(true, "You need to log in to Game Center to proceed!");
				}
				else
				{
					if (control.Id == 11008)
					{
						return;
					}
					if (control.Id == 11009)
					{
						SetupJunjieUI(false);
					}
					else if (control.Id == 11018)
					{
						if (playerShow != null)
						{
							playerShow.gameObject.transform.Rotate(new Vector3(0f, (0f - wparam) / 400f * 360f, 0f));
						}
					}
					else if (control.Id == 11011)
					{
						m_BoostType = BoostType.Skill;
						SetupBoostUI(true);
					}
					else if (control.Id >= 11052 && control.Id <= 11152)
					{
						if (m_BoostType == BoostType.Skill)
						{
							int selectedSkillType = control.Id - 11052;
							m_SelectedSkillType = (enSkillType)selectedSkillType;
							SetupSkillDetail(true);
						}
					}
					else if (control.Id == 11020)
					{
						float gold = 0f;
						float dollor = 0f;
						GetSkillUpgradePrice(m_SelectedSkillType, 1, ref gold, ref dollor);
						string dialog_content = "Upgrade this skill for " + gold + " cash and " + dollor + " tCrystals?";
						SetupHintDialog(true, -1, 11023, 11024, dialog_content);
					}
					else if (control.Id == 11023)
					{
						SetupHintDialog(false, -1, -1, -1, string.Empty);
						float gold2 = 0f;
						float dollor2 = 0f;
						GetSkillUpgradePrice(m_SelectedSkillType, 1, ref gold2, ref dollor2);
						if ((float)gameState.gold >= gold2 && (float)gameState.dollor >= dollor2)
						{
							gameState.LoseGold(Mathf.FloorToInt(gold2));
							gameState.LoseDollor(Mathf.FloorToInt(dollor2));
							Skill skill = new Skill(m_SelectedSkillType, 1u);
							gameState.AddSkill(skill);
							SetupSkillPageView();
							SetupSkillDetail(true);
							SetupAroundUI(true);
						}
						else
						{
							SetupDonnotHaveEnoughMoneyDialog(true, "Insufficient cash/crystals! Visit the bank now to get more.");
						}
					}
					else if (control.Id == 11024)
					{
						SetupHintDialog(false, -1, -1, -1, string.Empty);
					}
					else if (control.Id == 11021)
					{
						Skill skill2 = null;
						List<Skill> playerSkilles = gameState.GetPlayerSkilles();
						foreach (Skill item in playerSkilles)
						{
							if (item.SkillType == m_SelectedSkillType)
							{
								skill2 = item;
								break;
							}
						}
						if (skill2 != null)
						{
							float gold3 = 0f;
							float dollor3 = 0f;
							GetSkillUpgradePrice(m_SelectedSkillType, (int)(skill2.Level + 1), ref gold3, ref dollor3);
							string dialog_content2 = "Upgrade this skill for " + gold3 + " cash and " + dollor3 + " tCrystals?";
							SetupHintDialog(true, -1, 11025, 11026, dialog_content2);
						}
					}
					else if (control.Id == 11025)
					{
						SetupHintDialog(false, -1, -1, -1, string.Empty);
						Skill skill3 = null;
						List<Skill> playerSkilles2 = gameState.GetPlayerSkilles();
						foreach (Skill item2 in playerSkilles2)
						{
							if (item2.SkillType == m_SelectedSkillType)
							{
								skill3 = item2;
								break;
							}
						}
						if (skill3 != null)
						{
							float gold4 = 0f;
							float dollor4 = 0f;
							GetSkillUpgradePrice(m_SelectedSkillType, (int)(skill3.Level + 1), ref gold4, ref dollor4);
							if ((float)gameState.gold >= gold4 && (float)gameState.dollor >= dollor4)
							{
								gameState.LoseGold(Mathf.FloorToInt(gold4));
								gameState.LoseDollor(Mathf.FloorToInt(dollor4));
								gameState.UpdateSkill(skill3.SkillType, 1u);
								SetupSkillPageView();
								SetupSkillDetail(true);
								SetupAroundUI(true);
							}
							else
							{
								SetupDonnotHaveEnoughMoneyDialog(true, "Insufficient cash/crystals! Visit the bank now to get more.");
							}
						}
					}
					else if (control.Id == 11026)
					{
						SetupHintDialog(false, -1, -1, -1, string.Empty);
					}
					else if (control.Id == 11022)
					{
						gameState.m_CurSkillType = m_SelectedSkillType;
						GameApp.GetInstance().Save();
						SetupSkillPageView();
						SetupSkillDetail(false);
					}
					else if (control.Id == 11019)
					{
						SetupSkillDetail(false);
					}
					else if (control.Id == 11043)
					{
						SetupHintDialog(false, 0, 0, 0, string.Empty);
					}
					else if (control.Id == 11044)
					{
						SetupHintDialog(false, 0, 0, 0, string.Empty);
					}
					else if (control.Id == 11045)
					{
						SetupHintDialog(false, 0, 0, 0, string.Empty);
					}
					else if (control.Id == 11046)
					{
						SetupDonnotHaveEnoughMoneyDialog(false, string.Empty);
					}
					else if (control.Id == 11047)
					{
						SetupDonnotHaveEnoughMoneyDialog(false, string.Empty);
						playerShow = SceneUIManager.Instance().ShowPlayerUIDDS(false);
						ShopUIScript.externalShopType = ShopUIScript.ShopType.Bank;
						SceneUIManager.Instance().ChangeSceneUI(SceneUIManager.SceneUI.ShopUI);
					}
					else if (control.Id == 11051)
					{
						SetupNotificationDialogUI(false, string.Empty);
					}
					else if (control.Id == 11153)
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
					else if (control.Id == 11010)
					{
						playerShow = SceneUIManager.Instance().ShowPlayerUIDDS(false);
						ShopUIScript.externalShopType = ShopUIScript.ShopType.Bank;
						SceneUIManager.Instance().ChangeSceneUI(SceneUIManager.SceneUI.ShopUI);
					}
					else if (control.Id != 11158)
					{
					}
				}
			}
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
			control.CatchMessage = false;
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
			UIClickButton control3 = UIUtils.BuildClickButton(11010, new Rect(320f, 588f, 640f, 52f), m_MatDialog01, new Rect(0f, 798f, 640f, 52f), new Rect(0f, 850f, 640f, 52f), new Rect(0f, 798f, 640f, 52f), new Vector2(640f, 52f));
			m_AroundUIGroup.Add(control3);
			playerHpProgressBtn = new UIClickButton();
			playerHpProgressBtn.Id = 11153;
			playerHpProgressBtn.Rect = AutoUIResolution.ToShiftToRight(AutoUI.AutoRect(new Rect(0f, 564f, 101f, 76f)), 2);
			m_AroundUIGroup.Add(playerHpProgressBtn);
			if (GameApp.GetInstance().GetGameState().GetNeedShowLevelupAnimation())
			{
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
			UIClickButton control2 = UIUtils.BuildClickButton(11009, new Rect(244f, 576f, 75f, 52f), m_MatCommonBg, new Rect(1020f, 1f, 1f, 1f), new Rect(1020f, 1f, 1f, 1f), new Rect(1020f, 1f, 1f, 1f), new Vector2(75f, 52f));
			m_JunjieUIGroup.Add(control2);
			control2 = UIUtils.BuildClickButton(11009, new Rect(50f, 420f, 226f, 150f), m_MatCommonBg, new Rect(1020f, 1f, 1f, 1f), new Rect(1020f, 1f, 1f, 1f), new Rect(1020f, 1f, 1f, 1f), new Vector2(226f, 150f));
			m_JunjieUIGroup.Add(control2);
		}
	}

	public void SetupBoostUI(bool bShow)
	{
		SetupShopCommonBarUI(bShow);
		if (m_BoostType == BoostType.Skill)
		{
			SetupBoostSkillUI(bShow);
		}
		SetupAroundUI(true);
	}

	public void SetupShopCommonBarUI(bool bShow)
	{
		if (m_ShopCommonBarGroup != null)
		{
			m_ShopCommonBarGroup.Clear();
			m_ShopCommonBarGroup = null;
		}
		if (bShow)
		{
			m_ShopCommonBarGroup = new uiGroup(m_UIManager);
			UIImage control = UIUtils.BuildImage(0, new Rect(0f, 0f, 960f, 640f), m_MatCommonBg, new Rect(0f, 0f, 960f, 640f), new Vector2(960f, 640f));
			m_ShopCommonBarGroup.Add(control);
			control = UIUtils.BuildImage(0, new Rect(295f, 497f, 457f, 104f), m_MatCommonBg, new Rect(9f, 800f, 457f, 104f), new Vector2(457f, 104f));
			m_ShopCommonBarGroup.Add(control);
			UIClickButton control2 = UIUtils.BuildClickButton(11000, new Rect(295f, 497f, 77f, 104f), m_MatCommonBg, new Rect(1022f, 1f, 1f, 1f), new Rect(9f, 904f, 77f, 104f), new Rect(1022f, 1f, 1f, 1f), new Vector2(77f, 104f));
			m_ShopCommonBarGroup.Add(control2);
			control2 = UIUtils.BuildClickButton(11001, new Rect(372f, 497f, 80f, 104f), m_MatCommonBg, new Rect(939f, 707f, 80f, 104f), new Rect(939f, 707f, 80f, 104f), new Rect(1022f, 1f, 1f, 1f), new Vector2(80f, 104f));
			m_ShopCommonBarGroup.Add(control2);
			control2 = UIUtils.BuildClickButton(11002, new Rect(452f, 497f, 76f, 104f), m_MatCommonBg, new Rect(1022f, 1f, 1f, 1f), new Rect(85f, 904f, 76f, 104f), new Rect(1022f, 1f, 1f, 1f), new Vector2(76f, 104f));
			m_ShopCommonBarGroup.Add(control2);
			control2 = UIUtils.BuildClickButton(11003, new Rect(528f, 497f, 80f, 104f), m_MatCommonBg, new Rect(1022f, 1f, 1f, 1f), new Rect(160f, 904f, 80f, 104f), new Rect(1022f, 1f, 1f, 1f), new Vector2(80f, 104f));
			m_ShopCommonBarGroup.Add(control2);
			control2 = UIUtils.BuildClickButton(11004, new Rect(603f, 503f, 80f, 104f), m_MatCommonBg, new Rect(1022f, 1f, 1f, 1f), new Rect(834f, 700f, 80f, 104f), new Rect(1022f, 1f, 1f, 1f), new Vector2(80f, 104f));
			control2.Enable = false;
			m_ShopCommonBarGroup.Add(control2);
			control2 = UIUtils.BuildClickButton(11005, new Rect(683f, 497f, 74f, 104f), m_MatCommonBg, new Rect(1022f, 1f, 1f, 1f), new Rect(240f, 904f, 74f, 104f), new Rect(1022f, 1f, 1f, 1f), new Vector2(74f, 104f));
			m_ShopCommonBarGroup.Add(control2);
			control = UIUtils.BuildImage(0, new Rect(300f, 445f, 621f, 54f), m_MatShopUI, new Rect(87f, 438f, 621f, 54f), new Vector2(621f, 54f));
			m_ShopCommonBarGroup.Add(control);
			UIPushButton uIPushButton = null;
			uIPushButton = UIUtils.BuildPushButton(11011, new Rect(357f, 445f, 130f, 50f), m_MatShopUI, new Rect(255f, 108f, 134f, 54f), new Rect(386f, 108f, 134f, 54f), new Rect(255f, 108f, 134f, 54f), new Vector2(134f, 54f));
			m_ShopCommonBarGroup.Add(uIPushButton);
			if (m_BoostType == BoostType.Skill)
			{
				uIPushButton.Set(true);
			}
		}
	}

	public void SetupBoostSkillUI(bool bShow)
	{
		if (m_uiGroup != null)
		{
			m_uiGroup.Clear();
			m_uiGroup = null;
		}
		if (bShow)
		{
			m_uiGroup = new uiGroup(m_UIManager);
			UIImage uIImage = null;
			uIImage = UIUtils.BuildImage(0, new Rect(340f, 70f, 560f, 363f), m_MatShopUI, new Rect(30f, 110f, 1f, 1f), new Vector2(560f, 363f));
			m_uiGroup.Add(uIImage);
			UIMoveOuter control = UIUtils.BuildUIMoveOuter(11018, new Rect(0f, 0f, 393f, 575f), 10f, 10f);
			m_uiGroup.Add(control);
			SetupSkillPageView();
		}
	}

	public void SetupSkillPageView()
	{
		UIImage uIImage = null;
		UIClickButton uIClickButton = null;
		UIText uIText = null;
		int num = 0;
		if (m_BoostSkillPageView != null)
		{
			num = m_BoostSkillPageView.PageIndex;
			m_uiGroup.Remove(m_BoostSkillPageView);
			m_BoostSkillPageView = null;
		}
		m_BoostSkillPageView = new UIScrollPageView();
		m_BoostSkillPageView.SetMoveParam(AutoUI.AutoRect(new Rect(352f, 80f, 544f, 342f)), AutoUI.AutoDistance(20f), AutoUI.AutoDistance(20f));
		m_BoostSkillPageView.Rect = AutoUI.AutoRect(new Rect(352f, 100f, 544f, 342f));
		m_BoostSkillPageView.ListOri = UIScrollPageView.ListOrientation.Horizontal;
		m_BoostSkillPageView.ViewSize = AutoUI.AutoSize(new Vector2(272f, 171f));
		m_BoostSkillPageView.ItemSpacingV = AutoUI.AutoDistance(0f);
		m_BoostSkillPageView.ItemSpacingH = AutoUI.AutoDistance(0f);
		m_BoostSkillPageView.SetClip(AutoUI.AutoRect(new Rect(350f, 80f, 550f, 342f)));
		m_BoostSkillPageView.Bounds = AutoUI.AutoRect(new Rect(352f, 80f, 544f, 342f));
		m_uiGroup.Add(m_BoostSkillPageView);
		float num2 = 272f;
		float num3 = 171f;
		Rect rect = new Rect(773f, 15f, 150f, 112f);
		Rect rect2 = new Rect(621f, 112f, 150f, 112f);
		Rect rect3 = new Rect(621f, 0f, 150f, 112f);
		int num4 = 2;
		int num5 = 9;
		for (int i = 0; (float)i < (float)num5 / (float)num4; i++)
		{
			UIGroupControl uIGroupControl = UIUtils.BuildUIGroupControl(0, new Rect(0f, 0f, 272f, 342f));
			for (int j = 0; j < num4; j++)
			{
				int num6 = i * num4 + j + 1;
				if (num6 > num5)
				{
					break;
				}
				enSkillType enSkillType = (enSkillType)num6;
				Skill skill = null;
				List<Skill> playerSkilles = gameState.GetPlayerSkilles();
				foreach (Skill item in playerSkilles)
				{
					if (item.SkillType == enSkillType)
					{
						skill = item;
						break;
					}
				}
				if (skill == null)
				{
					skill = new Skill(enSkillType, 0u);
				}
				float num7 = 0f + (float)(j / 2) * num2;
				float num8 = num3 - (float)(j % 2) * num3;
				uIImage = UIUtils.BuildImage(0, new Rect(num7, num8 + 10f, 265f, 153f), m_MatShopUI, new Rect(0f, 854f, 265f, 153f), new Vector2(265f, 153f));
				uIGroupControl.Add(uIImage);
				Rect skillIconTexture = GetSkillIconTexture(enSkillType);
				float num9 = num7 + 76f + 50f;
				float num10 = num8 + 87f;
				uIImage = UIUtils.BuildImage(0, new Rect(num9 - skillIconTexture.width / 2f, num10 - skillIconTexture.height / 2f, skillIconTexture.width, skillIconTexture.height), m_MatSkillIcons, skillIconTexture, new Vector2(skillIconTexture.width, skillIconTexture.height));
				uIGroupControl.Add(uIImage);
				if (gameState.m_CurSkillType == skill.SkillType)
				{
					Rect rcMat = new Rect(90f, 109f, 151f, 112f);
					uIImage = UIUtils.BuildImage(0, new Rect(num9 - rcMat.width / 2f, num10 - rcMat.height / 2f, rcMat.width, rcMat.height), m_MatShopUI, rcMat, new Vector2(rcMat.width, rcMat.height));
					uIGroupControl.Add(uIImage);
				}
				float num11 = 160f;
				float num12 = 110f;
				float num13 = 200f;
				if (AutoUI.IsRetain == AutoUI.RESOLUTION.LOWDEFINITION)
				{
					num13 = 220f;
				}
				string text = string.Empty;
				switch (enSkillType)
				{
				case enSkillType.FastRun:
					text = "Cardio";
					break;
				case enSkillType.BuildCannon:
					text = "Bullet Buddy";
					break;
				case enSkillType.ThrowGrenade:
					text = "Fire in the Hole!";
					break;
				case enSkillType.CoverMe:
					text = "Cover Me!";
					break;
				case enSkillType.DoubleTeam:
					text = "Double Team";
					break;
				case enSkillType.KillShot:
					text = "Kill Shot";
					break;
				case enSkillType.FancyFootwork:
					text = "Fancy Footwork";
					break;
				case enSkillType.HailMary:
					text = "Hail Mary";
					break;
				case enSkillType.MachoMan:
					text = "Macho Man";
					break;
				}
				uIText = UIUtils.BuildUIText(0, new Rect(num7 + 22f, num8 + 140f, 200f, 20f), UIText.enAlignStyle.left);
				uIText.Set("Zombie3D/Font/037-CAI978-15", text, Constant.TextCommonColor);
				uIGroupControl.Add(uIText);
				uIText = UIUtils.BuildUIText(0, new Rect(num7 + 200f, num8 + 10f, 200f, 20f), UIText.enAlignStyle.left);
				uIText.Set("Zombie3D/Font/037-CAI978-15", "LV " + skill.Level, Constant.TextCommonColor);
				uIGroupControl.Add(uIText);
				uIClickButton = UIUtils.BuildClickButton((int)(11052 + enSkillType), new Rect(num7 + 35f, num8 + 30f, 185f, 111f), m_MatTransparentUI, new Rect(1f, 1f, 1f, 1f), new Rect(1f, 1f, 1f, 1f), new Rect(1f, 1f, 1f, 1f), new Vector2(185f, 111f));
				uIGroupControl.Add(uIClickButton);
			}
			m_BoostSkillPageView.Add(uIGroupControl);
		}
		if (num > 0)
		{
			m_BoostSkillPageView.PageIndex = num;
		}
		if (m_ShopPageViewScrollBar != null)
		{
			m_uiGroup.Remove(m_ShopPageViewScrollBar);
			m_ShopPageViewScrollBar = null;
		}
		m_ShopPageViewScrollBar = new UIDotScrollBar();
		m_ShopPageViewScrollBar.Rect = AutoUI.AutoRect(new Rect(450f, 50f, 100f, 20f));
		m_ShopPageViewScrollBar.ScrollOri = UIDotScrollBar.ScrollOrientation.Horizontal;
		m_ShopPageViewScrollBar.DotPageWidth = AutoUI.AutoDistance(30f);
		m_ShopPageViewScrollBar.SetPageCount(m_BoostSkillPageView.PageCount);
		m_ShopPageViewScrollBar.SetScrollBarTexture(m_MatShopUI, AutoUI.AutoRect(new Rect(597f, 107f, 11f, 11f)), m_MatShopUI, AutoUI.AutoRect(new Rect(609f, 107f, 11f, 11f)));
		m_ShopPageViewScrollBar.SetScrollPercent(0f);
		m_uiGroup.Add(m_ShopPageViewScrollBar);
		m_BoostSkillPageView.ScrollBar = m_ShopPageViewScrollBar;
		uIImage = UIUtils.BuildImage(0, new Rect(330f, 60f, 27f, 384f), m_MatShopUI, new Rect(0f, 106f, 27f, 384f), new Vector2(27f, 384f));
		m_uiGroup.Add(uIImage);
		uIImage = UIUtils.BuildImage(0, new Rect(882f, 60f, 45f, 388f), m_MatShopUI, new Rect(33f, 106f, 45f, 384f), new Vector2(45f, 384f));
		m_uiGroup.Add(uIImage);
	}

	public void SetupSkillDetail(bool bShow)
	{
		if (m_uiShopItemDetail != null)
		{
			m_uiShopItemDetail.Clear();
			m_uiShopItemDetail = null;
		}
		if (!bShow)
		{
			return;
		}
		m_uiShopItemDetail = new uiGroup(m_UIManager);
		UIImage control = UIUtils.BuildImage(0, new Rect(347f, 78f, 548f, 352f), m_MatShopUI, new Rect(598f, 120f, 1f, 1f), new Vector2(548f, 352f));
		m_uiShopItemDetail.Add(control);
		float num = 390f;
		float num2 = 92f;
		control = UIUtils.BuildImage(0, new Rect(num, num2, 483f, 357f), m_MatShopUI, new Rect(0f, 494f, 483f, 357f), new Vector2(483f, 357f));
		m_uiShopItemDetail.Add(control);
		UIClickButton uIClickButton = null;
		UIText uIText = null;
		Skill skill = null;
		List<Skill> playerSkilles = gameState.GetPlayerSkilles();
		foreach (Skill item in playerSkilles)
		{
			if (item.SkillType == m_SelectedSkillType)
			{
				skill = item;
				break;
			}
		}
		if (skill == null)
		{
			skill = new Skill(m_SelectedSkillType, 0u);
		}
		string text = string.Empty;
		string text2 = string.Empty;
		switch (skill.SkillType)
		{
		case enSkillType.FastRun:
		{
			text = "Cardio";
			text2 = "Increases movement speed for a short time.";
			float num32 = 0.1f;
			float num33 = 5f;
			if (skill.Level != 0)
			{
				num32 = 0.1f + (float)(skill.Level - 1) * 0.05f;
				num33 = 4f;
			}
			float num34 = 180f;
			float num35 = 220f;
			float num36 = num34 + 200f;
			uIText = UIUtils.BuildUIText(0, new Rect(num + num34, num2 + num35, 200f, 20f), UIText.enAlignStyle.right);
			uIText.Set("Zombie3D/Font/037-CAI978-15", "SPD ", m_ShopPropColor);
			m_uiShopItemDetail.Add(uIText);
			uIText = UIUtils.BuildUIText(0, new Rect(num + num36, num2 + num35, 200f, 20f), UIText.enAlignStyle.left);
			uIText.Set("Zombie3D/Font/037-CAI978-13", num32 * 100f + "%", m_ShopPropSPDPositiveValueColor);
			m_uiShopItemDetail.Add(uIText);
			break;
		}
		case enSkillType.BuildCannon:
		{
			text = "Bullet Buddy";
			text2 = "Deploys an automated turret for extra firepower.";
			float num6 = 10f;
			float num7 = 0.2f;
			float num8 = 5f;
			if (skill.Level != 0)
			{
				num6 = 10f + (float)(skill.Level - 1) * 10f;
			}
			float num9 = 220f;
			float num10 = 220f;
			float num11 = num9 + 200f;
			uIText = UIUtils.BuildUIText(0, new Rect(num + num9, num2 + num10, 200f, 20f), UIText.enAlignStyle.right);
			uIText.Set("Zombie3D/Font/037-CAI978-15", "TURRET DMG ", m_ShopPropColor);
			m_uiShopItemDetail.Add(uIText);
			uIText = UIUtils.BuildUIText(0, new Rect(num + num11, num2 + num10, 200f, 20f), UIText.enAlignStyle.left);
			uIText.Set("Zombie3D/Font/037-CAI978-13", num6.ToString(), m_ShopPropValueColor);
			m_uiShopItemDetail.Add(uIText);
			uIText = UIUtils.BuildUIText(0, new Rect(num + num9, num2 + num10 - 20f, 200f, 20f), UIText.enAlignStyle.right);
			uIText.Set("Zombie3D/Font/037-CAI978-15", "TURRET RPM ", m_ShopPropColor);
			m_uiShopItemDetail.Add(uIText);
			uIText = UIUtils.BuildUIText(0, new Rect(num + num11, num2 + num10 - 20f, 200f, 20f), UIText.enAlignStyle.left);
			uIText.Set("Zombie3D/Font/037-CAI978-13", Mathf.FloorToInt(60f / num7).ToString(), m_ShopPropValueColor);
			m_uiShopItemDetail.Add(uIText);
			break;
		}
		case enSkillType.ThrowGrenade:
		{
			text = "Fire in the Hole!";
			text2 = "Launches a grenade.";
			float num37 = 35f;
			if (skill.Level != 0)
			{
				switch (skill.Level)
				{
				case 1u:
					num37 = 35f;
					break;
				case 2u:
					num37 = 70f;
					break;
				case 3u:
					num37 = 110f;
					break;
				case 4u:
					num37 = 155f;
					break;
				case 5u:
					num37 = 205f;
					break;
				case 6u:
					num37 = 260f;
					break;
				case 7u:
					num37 = 320f;
					break;
				case 8u:
					num37 = 385f;
					break;
				case 9u:
					num37 = 455f;
					break;
				case 10u:
					num37 = 530f;
					break;
				default:
					num37 = 35f;
					break;
				}
			}
			num37 *= 2f;
			float num38 = 180f;
			float num39 = 220f;
			float num40 = num38 + 200f;
			uIText = UIUtils.BuildUIText(0, new Rect(num + num38, num2 + num39, 200f, 20f), UIText.enAlignStyle.right);
			uIText.Set("Zombie3D/Font/037-CAI978-15", "DMG ", m_ShopPropColor);
			m_uiShopItemDetail.Add(uIText);
			uIText = UIUtils.BuildUIText(0, new Rect(num + num40, num2 + num39, 200f, 20f), UIText.enAlignStyle.left);
			uIText.Set("Zombie3D/Font/037-CAI978-13", num37.ToString(), m_ShopPropValueColor);
			m_uiShopItemDetail.Add(uIText);
			break;
		}
		case enSkillType.CoverMe:
		{
			text = "Cover Me!";
			text2 = "Makes your sidekick shoot behind you.";
			float num24 = 0.1f;
			if (skill.Level != 0)
			{
				num24 = 0.1f + (float)(skill.Level - 1) * 0.1f;
			}
			float num25 = 200f;
			float num26 = 220f;
			float num27 = num25 + 200f;
			uIText = UIUtils.BuildUIText(0, new Rect(num + num25, num2 + num26, 200f, 20f), UIText.enAlignStyle.right);
			uIText.Set("Zombie3D/Font/037-CAI978-15", "ALLY DMG ", m_ShopPropColor);
			m_uiShopItemDetail.Add(uIText);
			uIText = UIUtils.BuildUIText(0, new Rect(num + num27, num2 + num26, 200f, 20f), UIText.enAlignStyle.left);
			uIText.Set("Zombie3D/Font/037-CAI978-13", "+" + Mathf.FloorToInt(num24 * 100f) + "%", m_ShopPropSPDPositiveValueColor);
			m_uiShopItemDetail.Add(uIText);
			break;
		}
		case enSkillType.DoubleTeam:
		{
			text = "Double Team";
			text2 = "Makes your sidekick shoot where you shoot.";
			float num20 = 0.1f;
			if (skill.Level != 0)
			{
				num20 = 0.1f + (float)(skill.Level - 1) * 0.1f;
			}
			float num21 = 200f;
			float num22 = 220f;
			float num23 = num21 + 200f;
			uIText = UIUtils.BuildUIText(0, new Rect(num + num21, num2 + num22, 200f, 20f), UIText.enAlignStyle.right);
			uIText.Set("Zombie3D/Font/037-CAI978-15", "ALLY DMG ", m_ShopPropColor);
			m_uiShopItemDetail.Add(uIText);
			uIText = UIUtils.BuildUIText(0, new Rect(num + num23, num2 + num22, 200f, 20f), UIText.enAlignStyle.left);
			uIText.Set("Zombie3D/Font/037-CAI978-13", "+" + Mathf.FloorToInt(num20 * 100f) + "%", m_ShopPropSPDPositiveValueColor);
			m_uiShopItemDetail.Add(uIText);
			break;
		}
		case enSkillType.KillShot:
		{
			text = "Kill Shot";
			text2 = "Gives every shot a chance to do double damage.";
			float num12 = 0.01f;
			if (skill.Level != 0)
			{
				num12 = 0.01f;
				switch (skill.Level)
				{
				case 1u:
					num12 = 0.01f;
					break;
				case 2u:
					num12 = 0.02f;
					break;
				case 3u:
					num12 = 0.04f;
					break;
				case 4u:
					num12 = 0.07f;
					break;
				case 5u:
					num12 = 0.1f;
					break;
				case 6u:
					num12 = 0.11f;
					break;
				case 7u:
					num12 = 0.12f;
					break;
				case 8u:
					num12 = 0.13f;
					break;
				case 9u:
					num12 = 0.14f;
					break;
				case 10u:
					num12 = 0.15f;
					break;
				default:
					num12 = 0.1f;
					break;
				}
			}
			float num13 = 200f;
			float num14 = 220f;
			float num15 = num13 + 200f;
			uIText = UIUtils.BuildUIText(0, new Rect(num + num13, num2 + num14, 200f, 20f), UIText.enAlignStyle.right);
			uIText.Set("Zombie3D/Font/037-CAI978-15", "CHANCE ", m_ShopPropColor);
			m_uiShopItemDetail.Add(uIText);
			uIText = UIUtils.BuildUIText(0, new Rect(num + num15, num2 + num14, 200f, 20f), UIText.enAlignStyle.left);
			uIText.Set("Zombie3D/Font/037-CAI978-13", Mathf.FloorToInt(num12 * 100f) + "%", m_ShopPropValueColor);
			m_uiShopItemDetail.Add(uIText);
			break;
		}
		case enSkillType.FancyFootwork:
		{
			text = "Fancy Footwork";
			text2 = "Gives you a chance to avoid incoming damage.";
			float num28 = 0.03f;
			if (skill.Level != 0)
			{
				num28 = 0.03f;
				switch (skill.Level)
				{
				case 1u:
					num28 = 0.03f;
					break;
				case 2u:
					num28 = 0.07f;
					break;
				case 3u:
					num28 = 0.12f;
					break;
				case 4u:
					num28 = 0.18f;
					break;
				case 5u:
					num28 = 0.25f;
					break;
				case 6u:
					num28 = 0.27f;
					break;
				case 7u:
					num28 = 0.29f;
					break;
				case 8u:
					num28 = 0.31f;
					break;
				case 9u:
					num28 = 0.33f;
					break;
				case 10u:
					num28 = 0.35f;
					break;
				default:
					num28 = 0.03f;
					break;
				}
			}
			float num29 = 200f;
			float num30 = 220f;
			float num31 = num29 + 200f;
			uIText = UIUtils.BuildUIText(0, new Rect(num + num29, num2 + num30, 200f, 20f), UIText.enAlignStyle.right);
			uIText.Set("Zombie3D/Font/037-CAI978-15", "CHANCE ", m_ShopPropColor);
			m_uiShopItemDetail.Add(uIText);
			uIText = UIUtils.BuildUIText(0, new Rect(num + num31, num2 + num30, 200f, 20f), UIText.enAlignStyle.left);
			uIText.Set("Zombie3D/Font/037-CAI978-13", Mathf.FloorToInt(num28 * 100f) + "%", m_ShopPropValueColor);
			m_uiShopItemDetail.Add(uIText);
			break;
		}
		case enSkillType.HailMary:
		{
			text = "Hail Mary";
			text2 = "Gives each successful attack a chance to restore a little HP.";
			float num16 = 0.01f;
			if (skill.Level != 0)
			{
				num16 = 0.01f;
				switch (skill.Level)
				{
				case 1u:
					num16 = 0.01f;
					break;
				case 2u:
					num16 = 0.02f;
					break;
				case 3u:
					num16 = 0.04f;
					break;
				case 4u:
					num16 = 0.07f;
					break;
				case 5u:
					num16 = 0.1f;
					break;
				case 6u:
					num16 = 0.11f;
					break;
				case 7u:
					num16 = 0.12f;
					break;
				case 8u:
					num16 = 0.13f;
					break;
				case 9u:
					num16 = 0.14f;
					break;
				case 10u:
					num16 = 0.15f;
					break;
				default:
					num16 = 0.01f;
					break;
				}
			}
			float num17 = 200f;
			float num18 = 220f;
			float num19 = num17 + 200f;
			uIText = UIUtils.BuildUIText(0, new Rect(num + num17, num2 + num18, 200f, 20f), UIText.enAlignStyle.right);
			uIText.Set("Zombie3D/Font/037-CAI978-15", "CHANCE ", m_ShopPropColor);
			m_uiShopItemDetail.Add(uIText);
			uIText = UIUtils.BuildUIText(0, new Rect(num + num19, num2 + num18, 200f, 20f), UIText.enAlignStyle.left);
			uIText.Set("Zombie3D/Font/037-CAI978-13", Mathf.FloorToInt(num16 * 100f) + "%", m_ShopPropValueColor);
			m_uiShopItemDetail.Add(uIText);
			break;
		}
		case enSkillType.MachoMan:
		{
			text = "Macho Man";
			text2 = "Increases your max stamina.";
			float f = 10f;
			if (skill.Level != 0)
			{
				f = 10f + (float)(skill.Level - 1) * 10f;
			}
			float num3 = 200f;
			float num4 = 220f;
			float num5 = num3 + 200f;
			uIText = UIUtils.BuildUIText(0, new Rect(num + num3, num2 + num4, 200f, 20f), UIText.enAlignStyle.right);
			uIText.Set("Zombie3D/Font/037-CAI978-15", "STA ", m_ShopPropColor);
			m_uiShopItemDetail.Add(uIText);
			uIText = UIUtils.BuildUIText(0, new Rect(num + num5, num2 + num4, 200f, 20f), UIText.enAlignStyle.left);
			uIText.Set("Zombie3D/Font/037-CAI978-13", "+" + Mathf.FloorToInt(f), m_ShopPropSPDPositiveValueColor);
			m_uiShopItemDetail.Add(uIText);
			break;
		}
		}
		Rect skillIconTexture = GetSkillIconTexture(skill.SkillType);
		float num41 = num + 170f;
		float num42 = num2 + 200f;
		control = UIUtils.BuildImage(0, new Rect(num41 - skillIconTexture.width / 2f, num42 - skillIconTexture.height / 2f, skillIconTexture.width, skillIconTexture.height), m_MatSkillIcons, skillIconTexture, new Vector2(skillIconTexture.width, skillIconTexture.height));
		m_uiShopItemDetail.Add(control);
		if (gameState.m_CurSkillType == skill.SkillType)
		{
			Rect rcMat = new Rect(90f, 109f, 151f, 112f);
			control = UIUtils.BuildImage(0, new Rect(num41 - rcMat.width / 2f, num42 - rcMat.height / 2f, rcMat.width, rcMat.height), m_MatShopUI, rcMat, new Vector2(rcMat.width, rcMat.height));
			m_uiShopItemDetail.Add(control);
		}
		if (skill.Level < _maxSkillLevel)
		{
			float gold = 0f;
			float dollor = 0f;
			GetSkillUpgradePrice(m_SelectedSkillType, (int)(skill.Level + 1), ref gold, ref dollor);
			if (gold >= 0f)
			{
				control = UIUtils.BuildImage(0, new Rect(num + 318f, num2 + 117f + 30f, 35f, 21f), m_MatShopUI, new Rect(586f, 85f, 35f, 21f), new Vector2(35f, 21f));
				control.CatchMessage = false;
				m_uiShopItemDetail.Add(control);
				uIText = UIUtils.BuildUIText(0, new Rect(num + 360f, num2 + 120f + 30f, 200f, 20f), UIText.enAlignStyle.left);
				uIText.Set("Zombie3D/Font/037-CAI978-15", gold.ToString(), Constant.TextCommonColor);
				m_uiShopItemDetail.Add(uIText);
			}
			if (dollor >= 0f)
			{
				control = UIUtils.BuildImage(0, new Rect(num + 318f, num2 + 117f, 35f, 29f), m_MatShopUI, new Rect(586f, 56f, 35f, 29f), new Vector2(35f, 29f));
				control.CatchMessage = false;
				m_uiShopItemDetail.Add(control);
				uIText = UIUtils.BuildUIText(0, new Rect(num + 360f, num2 + 120f, 200f, 20f), UIText.enAlignStyle.left);
				uIText.Set("Zombie3D/Font/037-CAI978-15", dollor.ToString(), Constant.TextCommonColor);
				m_uiShopItemDetail.Add(uIText);
			}
		}
		uIText = UIUtils.BuildUIText(0, new Rect(num + 110f, num2 + 310f, 250f, 25f), UIText.enAlignStyle.center);
		uIText.Set("Zombie3D/Font/037-CAI978-18", text, Constant.TextCommonColor);
		m_uiShopItemDetail.Add(uIText);
		uIText = UIUtils.BuildUIText(0, new Rect(num + 43f, num2 + 40f, 400f, 70f), UIText.enAlignStyle.left);
		uIText.Set("Zombie3D/Font/037-CAI978-18", text2, Constant.TextCommonColor);
		m_uiShopItemDetail.Add(uIText);
		string text3 = skill.Level.ToString();
		if (skill.Level == 0)
		{
			text3 = "1";
		}
		uIText = UIUtils.BuildUIText(0, new Rect(num + 115f, num2 + 130f, 250f, 25f), UIText.enAlignStyle.center);
		uIText.Set("Zombie3D/Font/037-CAI978-15", "LV " + text3, Constant.TextCommonColor);
		m_uiShopItemDetail.Add(uIText);
		uIClickButton = UIUtils.BuildClickButton(11019, new Rect(340f, 0f, 620f, 640f), m_MatTransparentUI, new Rect(1f, 1f, 1f, 1f), new Rect(1f, 1f, 1f, 1f), new Rect(1f, 1f, 1f, 1f), new Vector2(620f, 640f));
		m_uiShopItemDetail.Add(uIClickButton);
		if (skill.Level < 1)
		{
			uIClickButton = UIUtils.BuildClickButton(11020, new Rect(542f, 77f, 191f, 63f), m_MatShopUI, new Rect(833f, 633f, 191f, 63f), new Rect(833f, 696f, 191f, 63f), new Rect(833f, 633f, 191f, 63f), new Vector2(191f, 63f));
			m_uiShopItemDetail.Add(uIClickButton);
		}
		else if (skill.Level < _maxSkillLevel)
		{
			if (skill.SkillType == enSkillType.KillShot || skill.SkillType == enSkillType.FancyFootwork || skill.SkillType == enSkillType.HailMary || skill.SkillType == enSkillType.MachoMan)
			{
				uIClickButton = UIUtils.BuildClickButton(11021, new Rect(542f, 77f, 191f, 63f), m_MatShopUI, new Rect(833f, 759f, 191f, 63f), new Rect(833f, 822f, 191f, 63f), new Rect(833f, 759f, 191f, 63f), new Vector2(191f, 63f));
				m_uiShopItemDetail.Add(uIClickButton);
				return;
			}
			uIClickButton = UIUtils.BuildClickButton(11021, new Rect(422f, 77f, 191f, 63f), m_MatShopUI, new Rect(833f, 759f, 191f, 63f), new Rect(833f, 822f, 191f, 63f), new Rect(833f, 759f, 191f, 63f), new Vector2(191f, 63f));
			m_uiShopItemDetail.Add(uIClickButton);
			uIClickButton = UIUtils.BuildClickButton(11022, new Rect(622f, 77f, 191f, 63f), m_MatShopUI, new Rect(833f, 885f, 191f, 63f), new Rect(833f, 948f, 191f, 63f), new Rect(833f, 885f, 191f, 63f), new Vector2(191f, 63f));
			m_uiShopItemDetail.Add(uIClickButton);
		}
		else if (skill.SkillType != enSkillType.KillShot && skill.SkillType != enSkillType.FancyFootwork && skill.SkillType != enSkillType.HailMary && skill.SkillType != enSkillType.MachoMan)
		{
			uIClickButton = UIUtils.BuildClickButton(11022, new Rect(542f, 77f, 191f, 63f), m_MatShopUI, new Rect(833f, 885f, 191f, 63f), new Rect(833f, 948f, 191f, 63f), new Rect(833f, 885f, 191f, 63f), new Vector2(191f, 63f));
			m_uiShopItemDetail.Add(uIClickButton);
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
			UIImage control = UIUtils.BuildImage(0, new Rect(347f, 78f, 548f, 352f), m_MatShopUI, new Rect(598f, 120f, 1f, 1f), new Vector2(548f, 352f));
			m_uiHintDialog.Add(control);
			float num = 360f;
			float num2 = 107f;
			control = UIUtils.BuildImage(0, new Rect(num, num2, 515f, 301f), m_MatDialog01, new Rect(0f, 378f, 515f, 301f), new Vector2(515f, 301f));
			m_uiHintDialog.Add(control);
			UIText uIText = UIUtils.BuildUIText(0, new Rect(num + 40f, num2 + 40f, 420f, 173f), UIText.enAlignStyle.left);
			uIText.Set("Zombie3D/Font/037-CAI978-18", dialog_content, Constant.TextCommonColor);
			m_uiHintDialog.Add(uIText);
			UIClickButton uIClickButton = null;
			uIClickButton = UIUtils.BuildClickButton(11046, new Rect(num + 21f, num2 - 16f, 191f, 62f), m_MatDialog01, new Rect(640f, 186f, 191f, 62f), new Rect(832f, 186f, 191f, 62f), new Rect(640f, 186f, 191f, 62f), new Vector2(191f, 62f));
			m_uiHintDialog.Add(uIClickButton);
			uIClickButton = UIUtils.BuildClickButton(11047, new Rect(num + 280f, num2 - 16f, 191f, 62f), m_MatDialog01, new Rect(640f, 316f, 191f, 62f), new Rect(832f, 316f, 191f, 62f), new Rect(640f, 316f, 191f, 62f), new Vector2(191f, 62f));
			m_uiHintDialog.Add(uIClickButton);
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
			UIImage control = UIUtils.BuildImage(0, new Rect(0f, 0f, 960f, 640f), m_MatDialog01, new Rect(600f, 1f, 1f, 1f), new Vector2(960f, 640f));
			m_uiHintDialog.Add(control);
			control = UIUtils.BuildImage(0, new Rect(347f, 78f, 548f, 352f), m_MatShopUI, new Rect(598f, 120f, 1f, 1f), new Vector2(548f, 352f));
			m_uiHintDialog.Add(control);
			float num = 360f;
			float num2 = 107f;
			control = UIUtils.BuildImage(0, new Rect(num, num2, 515f, 301f), m_MatDialog01, new Rect(0f, 378f, 515f, 301f), new Vector2(515f, 301f));
			m_uiHintDialog.Add(control);
			UIText uIText = UIUtils.BuildUIText(0, new Rect(num + 40f, num2 + 40f, 420f, 173f), UIText.enAlignStyle.left);
			uIText.Set("Zombie3D/Font/037-CAI978-18", dialog_content, Constant.TextCommonColor);
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

	public void SetupLoadingUI(bool bShow, string content = "")
	{
		if (m_LoadingUI != null)
		{
			m_LoadingUI.Clear();
			m_LoadingUI = null;
		}
		if (bShow)
		{
			m_LoadingUI = new uiGroup(m_UIManager);
			UIImage control = UIUtils.BuildImage(0, new Rect(0f, 0f, 960f, 640f), m_MatShopUI, new Rect(600f, 120f, 1f, 1f), new Vector2(960f, 640f));
			m_LoadingUI.Add(control);
			Material material = LoadUIMaterial("Zombie3D/UI/Materials/MacOSLoadingUI");
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
			if (content != string.Empty)
			{
				UIText uIText = UIUtils.BuildUIText(0, new Rect(450f, 250f, 250f, 40f), UIText.enAlignStyle.left);
				uIText.Set("Zombie3D/Font/037-CAI978-15", content, Color.yellow);
				m_LoadingUI.Add(uIText);
			}
		}
	}

	public void SetupNotificationDialogUI(bool bShow, string strContent)
	{
		if (m_DialogNoticeUIGroup != null)
		{
			m_DialogNoticeUIGroup.Clear();
			m_DialogNoticeUIGroup = null;
		}
		if (bShow)
		{
			m_DialogNoticeUIGroup = new uiGroup(m_UIManager);
			UIImage control = UIUtils.BuildImage(0, new Rect(0f, 0f, 960f, 640f), m_MatDialog01, new Rect(600f, 1f, 1f, 1f), new Vector2(960f, 640f));
			m_DialogNoticeUIGroup.Add(control);
			float left = 242f;
			float top = 232f;
			control = UIUtils.BuildImage(0, new Rect(left, top, 515f, 301f), m_MatDialog01, new Rect(0f, 378f, 515f, 301f), new Vector2(515f, 301f));
			m_DialogNoticeUIGroup.Add(control);
			UIText uIText = UIUtils.BuildUIText(0, new Rect(290f, 295f, 420f, 150f), UIText.enAlignStyle.left);
			uIText.Set("Zombie3D/Font/037-CAI978-18", strContent, Constant.TextCommonColor);
			m_DialogNoticeUIGroup.Add(uIText);
			UIClickButton uIClickButton = null;
			uIClickButton = UIUtils.BuildClickButton(11051, new Rect(385f, 215f, 191f, 62f), m_MatDialog01, new Rect(640f, 0f, 191f, 62f), new Rect(832f, 0f, 191f, 62f), new Rect(640f, 0f, 191f, 62f), new Vector2(191f, 62f));
			m_DialogNoticeUIGroup.Add(uIClickButton);
		}
	}

	public static Rect GetSkillIconTexture(enSkillType skill_type)
	{
		Rect result = default(Rect);
		switch (skill_type)
		{
		case enSkillType.FastRun:
			result = new Rect(0f, 0f, 110f, 100f);
			break;
		case enSkillType.BuildCannon:
			result = new Rect(110f, 0f, 110f, 100f);
			break;
		case enSkillType.ThrowGrenade:
			result = new Rect(220f, 0f, 110f, 100f);
			break;
		case enSkillType.CoverMe:
			result = new Rect(330f, 0f, 110f, 100f);
			break;
		case enSkillType.DoubleTeam:
			result = new Rect(0f, 100f, 110f, 100f);
			break;
		case enSkillType.KillShot:
			result = new Rect(110f, 100f, 110f, 100f);
			break;
		case enSkillType.FancyFootwork:
			result = new Rect(220f, 100f, 110f, 100f);
			break;
		case enSkillType.HailMary:
			result = new Rect(330f, 100f, 110f, 100f);
			break;
		case enSkillType.MachoMan:
			result = new Rect(0f, 200f, 110f, 100f);
			break;
		}
		return result;
	}

	public void GetSkillUpgradePrice(enSkillType skill_type, int level, ref float gold, ref float dollor)
	{
		switch (skill_type)
		{
		case enSkillType.FastRun:
			switch (level)
			{
			case 1:
				gold = 0f;
				dollor = 0f;
				break;
			case 2:
				gold = 10000f;
				dollor = 0f;
				break;
			case 3:
				gold = 15000f;
				dollor = 0f;
				break;
			case 4:
				gold = 20000f;
				dollor = 0f;
				break;
			case 5:
				gold = 25000f;
				dollor = 0f;
				break;
			case 6:
				gold = 30000f;
				dollor = 0f;
				break;
			case 7:
				gold = 35000f;
				dollor = 0f;
				break;
			case 8:
				gold = 40000f;
				dollor = 0f;
				break;
			case 9:
				gold = 45000f;
				dollor = 0f;
				break;
			case 10:
				gold = 50000f;
				dollor = 0f;
				break;
			}
			break;
		case enSkillType.BuildCannon:
			switch (level)
			{
			case 1:
				gold = 5000f;
				dollor = 0f;
				break;
			case 2:
				gold = 10000f;
				dollor = 0f;
				break;
			case 3:
				gold = 15000f;
				dollor = 0f;
				break;
			case 4:
				gold = 20000f;
				dollor = 0f;
				break;
			case 5:
				gold = 25000f;
				dollor = 0f;
				break;
			case 6:
				gold = 30000f;
				dollor = 0f;
				break;
			case 7:
				gold = 35000f;
				dollor = 0f;
				break;
			case 8:
				gold = 40000f;
				dollor = 0f;
				break;
			case 9:
				gold = 45000f;
				dollor = 0f;
				break;
			case 10:
				gold = 50000f;
				dollor = 0f;
				break;
			}
			break;
		case enSkillType.ThrowGrenade:
			switch (level)
			{
			case 1:
				gold = 5000f;
				dollor = 0f;
				break;
			case 2:
				gold = 10000f;
				dollor = 0f;
				break;
			case 3:
				gold = 15000f;
				dollor = 0f;
				break;
			case 4:
				gold = 20000f;
				dollor = 0f;
				break;
			case 5:
				gold = 25000f;
				dollor = 0f;
				break;
			case 6:
				gold = 30000f;
				dollor = 0f;
				break;
			case 7:
				gold = 35000f;
				dollor = 0f;
				break;
			case 8:
				gold = 40000f;
				dollor = 0f;
				break;
			case 9:
				gold = 45000f;
				dollor = 0f;
				break;
			case 10:
				gold = 50000f;
				dollor = 0f;
				break;
			}
			break;
		case enSkillType.CoverMe:
			switch (level)
			{
			case 1:
				gold = 5000f;
				dollor = 0f;
				break;
			case 2:
				gold = 10000f;
				dollor = 0f;
				break;
			case 3:
				gold = 15000f;
				dollor = 0f;
				break;
			case 4:
				gold = 20000f;
				dollor = 0f;
				break;
			case 5:
				gold = 25000f;
				dollor = 0f;
				break;
			case 6:
				gold = 30000f;
				dollor = 0f;
				break;
			case 7:
				gold = 35000f;
				dollor = 0f;
				break;
			case 8:
				gold = 40000f;
				dollor = 0f;
				break;
			case 9:
				gold = 45000f;
				dollor = 0f;
				break;
			case 10:
				gold = 50000f;
				dollor = 0f;
				break;
			}
			break;
		case enSkillType.DoubleTeam:
			switch (level)
			{
			case 1:
				gold = 5000f;
				dollor = 0f;
				break;
			case 2:
				gold = 10000f;
				dollor = 0f;
				break;
			case 3:
				gold = 15000f;
				dollor = 0f;
				break;
			case 4:
				gold = 20000f;
				dollor = 0f;
				break;
			case 5:
				gold = 25000f;
				dollor = 0f;
				break;
			case 6:
				gold = 30000f;
				dollor = 0f;
				break;
			case 7:
				gold = 35000f;
				dollor = 0f;
				break;
			case 8:
				gold = 40000f;
				dollor = 0f;
				break;
			case 9:
				gold = 45000f;
				dollor = 0f;
				break;
			case 10:
				gold = 50000f;
				dollor = 0f;
				break;
			}
			break;
		case enSkillType.KillShot:
			switch (level)
			{
			case 1:
				gold = 10000f;
				dollor = 0f;
				break;
			case 2:
				gold = 15000f;
				dollor = 0f;
				break;
			case 3:
				gold = 20000f;
				dollor = 0f;
				break;
			case 4:
				gold = 25000f;
				dollor = 0f;
				break;
			case 5:
				gold = 30000f;
				dollor = 0f;
				break;
			case 6:
				gold = 35000f;
				dollor = 0f;
				break;
			case 7:
				gold = 40000f;
				dollor = 0f;
				break;
			case 8:
				gold = 45000f;
				dollor = 0f;
				break;
			case 9:
				gold = 50000f;
				dollor = 0f;
				break;
			case 10:
				gold = 55000f;
				dollor = 0f;
				break;
			}
			break;
		case enSkillType.FancyFootwork:
			switch (level)
			{
			case 1:
				gold = 10000f;
				dollor = 0f;
				break;
			case 2:
				gold = 15000f;
				dollor = 0f;
				break;
			case 3:
				gold = 20000f;
				dollor = 0f;
				break;
			case 4:
				gold = 25000f;
				dollor = 0f;
				break;
			case 5:
				gold = 30000f;
				dollor = 0f;
				break;
			case 6:
				gold = 35000f;
				dollor = 0f;
				break;
			case 7:
				gold = 40000f;
				dollor = 0f;
				break;
			case 8:
				gold = 45000f;
				dollor = 0f;
				break;
			case 9:
				gold = 50000f;
				dollor = 0f;
				break;
			case 10:
				gold = 55000f;
				dollor = 0f;
				break;
			}
			break;
		case enSkillType.HailMary:
			switch (level)
			{
			case 1:
				gold = 10000f;
				dollor = 0f;
				break;
			case 2:
				gold = 15000f;
				dollor = 0f;
				break;
			case 3:
				gold = 20000f;
				dollor = 0f;
				break;
			case 4:
				gold = 25000f;
				dollor = 0f;
				break;
			case 5:
				gold = 30000f;
				dollor = 0f;
				break;
			case 6:
				gold = 35000f;
				dollor = 0f;
				break;
			case 7:
				gold = 40000f;
				dollor = 0f;
				break;
			case 8:
				gold = 45000f;
				dollor = 0f;
				break;
			case 9:
				gold = 50000f;
				dollor = 0f;
				break;
			case 10:
				gold = 55000f;
				dollor = 0f;
				break;
			}
			break;
		case enSkillType.MachoMan:
			switch (level)
			{
			case 1:
				gold = 10000f;
				dollor = 0f;
				break;
			case 2:
				gold = 15000f;
				dollor = 0f;
				break;
			case 3:
				gold = 20000f;
				dollor = 0f;
				break;
			case 4:
				gold = 25000f;
				dollor = 0f;
				break;
			case 5:
				gold = 30000f;
				dollor = 0f;
				break;
			case 6:
				gold = 35000f;
				dollor = 0f;
				break;
			case 7:
				gold = 40000f;
				dollor = 0f;
				break;
			case 8:
				gold = 45000f;
				dollor = 0f;
				break;
			case 9:
				gold = 50000f;
				dollor = 0f;
				break;
			case 10:
				gold = 55000f;
				dollor = 0f;
				break;
			}
			break;
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
