using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zombie3D;

public class ShopUIScript : MonoBehaviour, UIHandler
{
	public enum ShopType
	{
		Weapon = 0,
		Armor = 1,
		PowerUPS = 2,
		Bank = 3
	}

	public enum Controls
	{
		kIDLevels = 2000,
		kIDBoost = 2001,
		kIDFriends = 2002,
		kIDShop = 2003,
		kIDTChat = 2004,
		kIDOptions = 2005,
		kIDCup = 2006,
		kIDTopList = 2007,
		kIDJunjie = 2008,
		kIDJunjieClose = 2009,
		kIDGlobalBank = 2010,
		kIDWeapons = 2011,
		kIDArmor = 2012,
		kIDPowerUPS = 2013,
		kIDBank = 2014,
		kIDFilter = 2015,
		kIDBack = 2016,
		kIDPlayerRotateShowControl = 2017,
		kIDWeaponItemTryOn = 2018,
		kIDWeaponItemBuy = 2019,
		kIDWeaponItemArms = 2020,
		kIDWeaponItemDetailClose = 2021,
		kIDWeaponItemSell = 2022,
		kIDWeaponEquipped01 = 2023,
		kIDWeaponEquipped02 = 2024,
		kIDAvatarItemTryOn = 2025,
		kIDAvatarItemBuy = 2026,
		kIDAvatarItemArms = 2027,
		kIDAvatarItemDetailClose = 2028,
		kIDAvatarItemSell = 2029,
		kIDPowerUPSItemBuy = 2030,
		kIDPowerUPSItemBuyClose = 2031,
		kIDBankItemBuy = 2032,
		kIDBankItemBuyClose = 2033,
		kIDHintDialogOK = 2034,
		kIDHintDialogYes = 2035,
		kIDHintDialogNo = 2036,
		kIDGotoBankLater = 2037,
		kIDGotoBank = 2038,
		kIDGiftDialogOK = 2039,
		kIDReviewDialogOK = 2040,
		kIDReviewDialogLater = 2041,
		kIDNotificationDialogOK = 2042,
		kIDShopItemBegin = 2043,
		kIDShopItemLast = 2143,
		kIDPlayerPrograssBtn = 2144,
		kIDUnLockAccouterBtnOK = 2145,
		kIDLevelUpHortationBtnOK = 2146,
		kIDSellsDialogOK = 2147,
		kIDSellsDialogLater = 2148,
		kIDIsShareTwitter = 2149,
		kIDSharePageBack = 2150,
		kIDLast = 2151
	}

	private UIManager m_UIManager;

	private string m_ui_material_path;

	protected GameState gameState;

	protected Material m_MatCommonBg;

	protected Material m_MatShopUI;

	protected Material m_MatTransparentUI;

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

	public uiGroup m_ShareMsgGroup;

	private static bool m_bReviewShow;

	public static ShopType externalShopType;

	private UIScrollPageView m_WeaponPageView;

	private UIScrollPageView m_AvatarPageView;

	private UIScrollPageView m_PowerUPSPageView;

	private UIScrollPageView m_BankPageView;

	private UIDotScrollBar m_ShopPageViewScrollBar;

	protected PlayerUIShow playerShow;

	protected float lastUpdateTime;

	protected bool uiInited;

	protected ShopType shopType;

	protected Color m_ShopPropColor;

	protected Color m_ShopPropValueColor;

	protected Color m_ShopPropSPDPositiveValueColor;

	protected Color m_ShopPropSPDNegativeValueColor;

	protected Color m_ShopPropsAdditionColor;

	protected WeaponType weaponWantToBuy = WeaponType.Beretta_33;

	protected int avatarSelectedIndex;

	protected int powerUpsSelectedIndex;

	protected int iapSelectedIndex = -1;

	protected int exchangeSelectedIndex = -1;

	protected int m_CurBattleWeaponIndex;

	protected int m_iapBuyIndex = -1;

	private float m_GCLevelCheckTimer = -1f;

	private UIClickButton playerHpProgressBtn;

	private UIAnimationControl playerHpProgressBarBtnAnim;

	private List<int> m_lsIAPSalesIndex = new List<int>();

	private string m_strUnLockAccouterText = "New item unlocked! Visit the store.";

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
		m_MatDialog01 = LoadUIMaterial("Zombie3D/UI/Materials/Dialog01");
		Resources.UnloadUnusedAssets();
		if (SceneUIManager.Instance().GetMusicPlayerState() != SceneUIManager.MusicState_ChoosePointsAudioState && SceneUIManager.Instance().GetMusicPlayerState() != SceneUIManager.MusicState_GameStartFirstPlayState && SceneUIManager.Instance().GetMusicPlayerState() != SceneUIManager.MusicState_GameStartNotFirstPlayState)
		{
			SceneUIManager.Instance().SetMusicPlayerState(SceneUIManager.MusicState_ShopAudioState);
		}
		uiInited = true;
		m_GCLevelCheckTimer = 0f;
		m_ShopPropColor = new Color(0.81960785f, 0.5294118f, 2f / 51f, 1f);
		m_ShopPropValueColor = new Color(1f, 1f, 1f, 1f);
		m_ShopPropSPDPositiveValueColor = new Color(0f, 1f, 0f, 1f);
		m_ShopPropSPDNegativeValueColor = new Color(1f, 0f, 0f, 1f);
		m_ShopPropsAdditionColor = new Color(0.78039217f, 37f / 51f, 22f / 51f, 1f);
		shopType = ShopType.Weapon;
		if (externalShopType != 0)
		{
			shopType = externalShopType;
			externalShopType = ShopType.Weapon;
		}
		switch (shopType)
		{
		case ShopType.Weapon:
			m_MatWeaponIcons = LoadUIMaterial("Zombie3D/UI/Materials/WeaponIcons");
			GameCollectionInfoManager.Instance().GetCurrentInfo().AddUIEnterLog(GameCollectionInfo.enUIEnterIndex.ShopUI_Weapon);
			break;
		case ShopType.Armor:
			m_MatAvatarIcons = LoadUIMaterial("Zombie3D/UI/Materials/AvatarIcons");
			GameCollectionInfoManager.Instance().GetCurrentInfo().AddUIEnterLog(GameCollectionInfo.enUIEnterIndex.ShopUI_Avatar);
			break;
		case ShopType.PowerUPS:
			m_MatPowerUPSIcons = LoadUIMaterial("Zombie3D/UI/Materials/PowerUPSIcons");
			GameCollectionInfoManager.Instance().GetCurrentInfo().AddUIEnterLog(GameCollectionInfo.enUIEnterIndex.ShopUI_PowerUPS);
			break;
		case ShopType.Bank:
			m_MatBankIconUI = LoadUIMaterial("Zombie3D/UI/Materials/BankIcons");
			GameCollectionInfoManager.Instance().GetCurrentInfo().AddUIEnterLog(GameCollectionInfo.enUIEnterIndex.ShopUI_Bank);
			break;
		default:
			m_MatWeaponIcons = LoadUIMaterial("Zombie3D/UI/Materials/WeaponIcons");
			GameCollectionInfoManager.Instance().GetCurrentInfo().AddUIEnterLog(GameCollectionInfo.enUIEnterIndex.ShopUI_Weapon);
			break;
		}
		SetupShopUI(true);
		playerShow = SceneUIManager.Instance().ShowPlayerUIDDS(true);
		if (GameApp.GetInstance().GetGameState().LEVEL_UP)
		{
			SetupLevelUpHortationDialog(true);
		}
		if (gameState.m_GetFirstGift <= 0 && gameState.m_BattleCount > 0)
		{
			gameState.AddPowerUPS(ItemType.HpMiddle);
			gameState.AddPowerUPS(ItemType.HpMiddle);
			gameState.AddPowerUPS(ItemType.HpMiddle);
			gameState.AddPowerUPS(ItemType.Pacemaker);
			gameState.AddPowerUPS(ItemType.Pacemaker);
			gameState.AddPowerUPS(ItemType.StormGrenade);
			gameState.AddPowerUPS(ItemType.StormGrenade);
			gameState.m_GetFirstGift = 1;
			GameApp.GetInstance().Save();
			SetupSendGiftDialog(true);
		}
		if (!m_bReviewShow && gameState.m_ReviewCount <= 0 && gameState.m_BattleCount > 0 && gameState.m_BattleCount % 5 == 0)
		{
			gameState.m_ReviewCount = 1;
			GameApp.GetInstance().Save();
			m_bReviewShow = true;
			SetupReviewNotificationDialog(true);
		}
		if (gameState.CanShowBuffPresentation())
		{
			SetupNotificationDialogUI(true, "Weapon buffed! Check it out in the Shop.");
			gameState.ShowBuffPresentationOK();
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
		lastUpdateTime = Time.time;
		if (!(m_GCLevelCheckTimer >= 0f))
		{
			return;
		}
		m_GCLevelCheckTimer += Time.deltaTime;
		if (!(m_GCLevelCheckTimer >= 2f))
		{
			return;
		}
		m_GCLevelCheckTimer = -1f;
		if (gameState.GetPlayerLevel() >= 255)
		{
			if (gameState.IsGCArchievementLocked(35))
			{
				gameState.UnlockGCArchievement(35, "com.trinitigame.callofminibulletdudes.a36");
			}
		}
		else if (gameState.GetPlayerLevel() >= 200)
		{
			if (gameState.IsGCArchievementLocked(34))
			{
				gameState.UnlockGCArchievement(34, "com.trinitigame.callofminibulletdudes.a35");
			}
		}
		else if (gameState.GetPlayerLevel() >= 150)
		{
			if (gameState.IsGCArchievementLocked(33))
			{
				gameState.UnlockGCArchievement(33, "com.trinitigame.callofminibulletdudes.a34");
			}
		}
		else if (gameState.GetPlayerLevel() >= 100)
		{
			if (gameState.IsGCArchievementLocked(32))
			{
				gameState.UnlockGCArchievement(32, "com.trinitigame.callofminibulletdudes.a33");
			}
		}
		else if (gameState.GetPlayerLevel() >= 50)
		{
			if (gameState.IsGCArchievementLocked(31))
			{
				gameState.UnlockGCArchievement(31, "com.trinitigame.callofminibulletdudes.a32");
			}
		}
		else if (gameState.GetPlayerLevel() >= 30)
		{
			if (gameState.IsGCArchievementLocked(30))
			{
				gameState.UnlockGCArchievement(30, "com.trinitigame.callofminibulletdudes.a31");
			}
		}
		else if (gameState.GetPlayerLevel() >= 10 && gameState.IsGCArchievementLocked(29))
		{
			gameState.UnlockGCArchievement(29, "com.trinitigame.callofminibulletdudes.a30");
		}
	}

	private void LateUpdate()
	{
	}

	public void HandleEvent(UIControl control, int command, float wparam, float lparam)
	{
		Debug.LogWarning("Handle Event " + control.Id);
		if ((control.GetType() == typeof(UIClickButton) || control.GetType() == typeof(UISelectButton) || control.GetType() == typeof(UIPushButton)) && GameApp.GetInstance().GetGameState().SoundOn)
		{
			SceneUIManager.Instance().PlayClickAudio();
		}
		if (control.Id == 2000)
		{
			SceneUIManager.Instance().ShowPlayerUIDDS(false);
			SceneUIManager.Instance().ChangeSceneUI(SceneUIManager.SceneUI.ChoosePointsUI);
		}
		else if (control.Id == 2001)
		{
			SceneUIManager.Instance().ShowPlayerUIDDS(false);
			SceneUIManager.Instance().ChangeSceneUI(SceneUIManager.SceneUI.BoostUI);
		}
		else if (control.Id == 2002)
		{
			SceneUIManager.Instance().ShowPlayerUIDDS(false);
			SceneUIManager.Instance().ChangeSceneUI(SceneUIManager.SceneUI.FriendUI);
		}
		else
		{
			if (control.Id == 2003 || control.Id == 2004)
			{
				return;
			}
			if (control.Id == 2005)
			{
				SceneUIManager.Instance().ShowPlayerUIDDS(false);
				SceneUIManager.Instance().ChangeSceneUI(SceneUIManager.SceneUI.OptionUI);
			}
			else if (control.Id == 2006)
			{
				SetupNotificationDialogUI(true, "You need to log in to Game Center to proceed!");
			}
			else if (control.Id == 2007)
			{
				SetupNotificationDialogUI(true, "You need to log in to Game Center to proceed!");
			}
			else
			{
				if (control.Id == 2008)
				{
					return;
				}
				if (control.Id == 2009)
				{
					SetupJunjieUI(false);
				}
				else if (control.Id == 2017)
				{
					if (playerShow != null)
					{
						playerShow.gameObject.transform.Rotate(new Vector3(0f, (0f - wparam) / 400f * 360f, 0f));
					}
				}
				else if (control.Id == 2011)
				{
					shopType = ShopType.Weapon;
					if (shopType != 0)
					{
						GameCollectionInfoManager.Instance().GetCurrentInfo().AddUIEnterLog(GameCollectionInfo.enUIEnterIndex.ShopUI_Weapon);
					}
					SetupShopUI(true);
				}
				else if (control.Id == 2012)
				{
					shopType = ShopType.Armor;
					if (shopType != ShopType.Armor)
					{
						GameCollectionInfoManager.Instance().GetCurrentInfo().AddUIEnterLog(GameCollectionInfo.enUIEnterIndex.ShopUI_Avatar);
					}
					SetupShopUI(true);
				}
				else if (control.Id == 2013)
				{
					shopType = ShopType.PowerUPS;
					if (shopType != ShopType.PowerUPS)
					{
						GameCollectionInfoManager.Instance().GetCurrentInfo().AddUIEnterLog(GameCollectionInfo.enUIEnterIndex.ShopUI_PowerUPS);
					}
					SetupShopUI(true);
				}
				else if (control.Id == 2014 || control.Id == 2010)
				{
					shopType = ShopType.Bank;
					if (shopType != ShopType.Bank)
					{
						GameCollectionInfoManager.Instance().GetCurrentInfo().AddUIEnterLog(GameCollectionInfo.enUIEnterIndex.ShopUI_Bank);
					}
					SetupShopUI(true);
				}
				else
				{
					if (control.Id == 2015)
					{
						return;
					}
					if (control.Id == 2149)
					{
						UIClickButton uIClickButton = (UIClickButton)m_ShareMsgGroup.GetControl(2149);
						uIClickButton.Enable = false;
						gameState.ShareWithTwitter = false;
						TweetPlugin.FollowUser("@TrinitiGames");
						gameState.AddDollor(5);
						SetupAroundUI(true);
					}
					else if (control.Id == 2150)
					{
						SetupBankSocialityPage(false);
					}
					else if (control.Id >= 2043 && control.Id <= 2143)
					{
						if (shopType == ShopType.Weapon)
						{
							ArrayList weapons = ConfigManager.Instance().GetFixedConfig().weapons;
							int index = control.Id - 2043;
							int type = ((FixedConfig.WeaponCfg)weapons[index]).type;
							weaponWantToBuy = (WeaponType)type;
							SetupBuyWeaponDetail(true);
						}
						else if (shopType == ShopType.Armor)
						{
							avatarSelectedIndex = control.Id - 2043;
							SetupBuyAvatarDetail(true);
						}
						else if (shopType == ShopType.PowerUPS)
						{
							powerUpsSelectedIndex = control.Id - 2043;
							SetupBuyPowerUPSDetail(true);
						}
						else
						{
							if (shopType != ShopType.Bank)
							{
								return;
							}
							if (!gameState.m_bIAPIsInitOK)
							{
								SetupNotificationDialogUI(true, "Sorry, buy failed!");
								return;
							}
							int num = control.Id - 2043;
							iapSelectedIndex = num;
							if (iapSelectedIndex == 0)
							{
								if (gameState.NoviceGiftBagThird < gameState.m_iNovicegiftAllowBuyTimesThird)
								{
									SetupBuyBankDetail(true);
								}
							}
							else
							{
								SetupBuyBankDetail(true);
							}
						}
					}
					else if (control.Id == 2018)
					{
						SetupBuyWeaponDetail(false);
						playerShow.ChangeWeapon(weaponWantToBuy);
					}
					else if (control.Id == 2019)
					{
						ArrayList weapons2 = ConfigManager.Instance().GetFixedConfig().weapons;
						for (int i = 0; i < weapons2.Count; i++)
						{
							bool flag = false;
							FixedConfig.WeaponCfg weaponCfg = (FixedConfig.WeaponCfg)weapons2[i];
							if (weaponWantToBuy == (WeaponType)weaponCfg.type)
							{
								if (weaponCfg.priceType == "gold")
								{
									Debug.Log(gameState.GetGold() + "|" + weaponCfg.price + "|" + weaponCfg.name);
									if (gameState.GetGold() < weaponCfg.price)
									{
										SetupDonnotHaveEnoughMoneyDialog(true, "Insufficient cash! Visit the bank now to get more.");
										return;
									}
									GameApp.GetInstance().GetGameState().LoseGold(weaponCfg.price);
									GameApp.GetInstance().GetGameState().BuyWeapon(weaponWantToBuy, weaponCfg.priceType);
									GameCollectionInfoManager.Instance().GetCurrentInfo().AddTodayWeaponsBuy((int)weaponWantToBuy);
								}
								else if (weaponCfg.priceType == "dollor")
								{
									if (gameState.GetDollor() < weaponCfg.price)
									{
										SetupDonnotHaveEnoughMoneyDialog(true, "Insufficient crystals! Visit the bank now to get more.");
										return;
									}
									GameApp.GetInstance().GetGameState().LoseDollor(weaponCfg.price);
									GameApp.GetInstance().GetGameState().BuyWeapon(weaponWantToBuy, weaponCfg.priceType);
									GameCollectionInfoManager.Instance().GetCurrentInfo().AddTodayWeaponsBuy((int)weaponWantToBuy);
								}
								SetupAroundUI(true);
								SetupWeaponShopUI(true);
								List<WeaponType> weapons3 = GameApp.GetInstance().GetGameState().GetWeapons();
								for (int j = 0; j < weapons3.Count; j++)
								{
									if (weapons3[j] == weaponWantToBuy)
									{
										flag = true;
										break;
									}
								}
							}
							if (flag)
							{
								break;
							}
						}
						SetupBuyWeaponDetail(false);
						SetupBuyWeaponDetail(true);
					}
					else if (control.Id == 2022)
					{
						int price = 0;
						ArrayList weapons4 = ConfigManager.Instance().GetFixedConfig().weapons;
						for (int k = 0; k < weapons4.Count; k++)
						{
							FixedConfig.WeaponCfg weaponCfg2 = (FixedConfig.WeaponCfg)weapons4[k];
							if (weaponWantToBuy == (WeaponType)weaponCfg2.type)
							{
								if (weaponCfg2.priceType == "dollor")
								{
									price = (int)((float)weaponCfg2.price * 800f * 0.1f);
									break;
								}
								if (weaponCfg2.priceType == "gold")
								{
									price = (int)((float)weaponCfg2.price * 0.1f);
									break;
								}
								price = 0;
								Debug.Log("weapon price is wrong!!!");
								break;
							}
						}
						SetupSellsDialog(true, price);
					}
					else if (control.Id == 2020)
					{
						Hashtable weaponInfo = GameApp.GetInstance().GetGameState().GetWeaponInfo();
						List<WeaponType> battleWeapons = GameApp.GetInstance().GetGameState().GetBattleWeapons();
						if (battleWeapons.Count == 1)
						{
							if (m_CurBattleWeaponIndex == 0)
							{
								WeaponType item = battleWeapons[0];
								battleWeapons.Clear();
								battleWeapons.Add(weaponWantToBuy);
								battleWeapons.Add(item);
								List<WeaponType> battleWeapons2 = GameApp.GetInstance().GetGameState().GetBattleWeapons();
								Debug.Log(battleWeapons2.Count);
							}
							else if (m_CurBattleWeaponIndex == 1)
							{
								battleWeapons.Add(weaponWantToBuy);
							}
						}
						weaponInfo[battleWeapons[m_CurBattleWeaponIndex]] = 0;
						weaponInfo[weaponWantToBuy] = 1;
						battleWeapons[m_CurBattleWeaponIndex] = weaponWantToBuy;
						Weapon weapon = WeaponFactory.GetInstance().CreateWeapon(weaponWantToBuy);
						playerShow.ChangeWeapon(weaponWantToBuy);
						GameApp.GetInstance().Save();
						SetupWeaponShopUI(true);
						SetupBuyWeaponDetail(false);
					}
					else if (control.Id == 2023)
					{
						List<WeaponType> battleWeapons3 = GameApp.GetInstance().GetGameState().GetBattleWeapons();
						if (battleWeapons3.Count == 2)
						{
							m_CurBattleWeaponIndex = 1;
							playerShow.ChangeWeapon(battleWeapons3[m_CurBattleWeaponIndex]);
							m_uiGroup.Remove(2023);
							UIClickButton control2 = UIUtils.BuildClickButton(2024, new Rect(55f, 58f, 96f, 72f), m_MatShopUI, new Rect(278f, 941f, 96f, 72f), new Rect(276f, 939f, 100f, 76f), new Rect(278f, 941f, 96f, 72f), new Vector2(96f, 72f));
							m_uiGroup.Add(control2);
						}
						else
						{
							m_CurBattleWeaponIndex = 1;
							m_uiGroup.Remove(2023);
							UIClickButton control3 = UIUtils.BuildClickButton(2024, new Rect(55f, 58f, 96f, 72f), m_MatShopUI, new Rect(278f, 941f, 96f, 72f), new Rect(276f, 939f, 100f, 76f), new Rect(278f, 941f, 96f, 72f), new Vector2(96f, 72f));
							m_uiGroup.Add(control3);
						}
					}
					else if (control.Id == 2024)
					{
						List<WeaponType> battleWeapons4 = GameApp.GetInstance().GetGameState().GetBattleWeapons();
						m_CurBattleWeaponIndex = 0;
						playerShow.ChangeWeapon(battleWeapons4[m_CurBattleWeaponIndex]);
						m_uiGroup.Remove(2024);
						UIClickButton control4 = UIUtils.BuildClickButton(2023, new Rect(55f, 58f, 96f, 72f), m_MatShopUI, new Rect(278f, 860f, 96f, 72f), new Rect(276f, 858f, 100f, 76f), new Rect(278f, 860f, 96f, 72f), new Vector2(96f, 72f));
						m_uiGroup.Add(control4);
					}
					else if (control.Id == 2021)
					{
						SetupBuyWeaponDetail(false);
					}
					else if (control.Id == 2025)
					{
						SetupBuyAvatarDetail(false);
						FixedConfig.AvatarCfg avatarCfg = ConfigManager.Instance().GetFixedConfig().GetAvatarCfg(avatarSelectedIndex);
						playerShow.ChangeAvatar(avatarCfg.suiteType, avatarCfg.avtType);
					}
					else if (control.Id == 2026)
					{
						SetupBuyAvatarDetail(false);
						FixedConfig.AvatarCfg avatarCfg2 = ConfigManager.Instance().GetFixedConfig().GetAvatarCfg(avatarSelectedIndex);
						Debug.Log(string.Concat(avatarCfg2.suiteType, "|", avatarCfg2.avtType));
						if (avatarCfg2.priceType == "gold")
						{
							if (gameState.GetGold() < avatarCfg2.price)
							{
								SetupDonnotHaveEnoughMoneyDialog(true, "Insufficient cash! Visit the bank now to get more.");
								return;
							}
							GameApp.GetInstance().GetGameState().LoseGold(avatarCfg2.price);
							GameApp.GetInstance().GetGameState().BuyAvatar(avatarCfg2.suiteType, avatarCfg2.avtType);
							GameCollectionInfoManager.Instance().GetCurrentInfo().AddTodayAvatarBuy(avatarCfg2.suiteType, avatarCfg2.avtType);
						}
						else if (avatarCfg2.priceType == "dollor")
						{
							if (gameState.GetDollor() < avatarCfg2.price)
							{
								SetupDonnotHaveEnoughMoneyDialog(true, "Insufficient crystals! Visit the bank now to get more.");
								return;
							}
							GameApp.GetInstance().GetGameState().LoseDollor(avatarCfg2.price);
							GameApp.GetInstance().GetGameState().BuyAvatar(avatarCfg2.suiteType, avatarCfg2.avtType);
							GameCollectionInfoManager.Instance().GetCurrentInfo().AddTodayAvatarBuy(avatarCfg2.suiteType, avatarCfg2.avtType);
						}
						SetupAroundUI(true);
						SetupAvatarShopUI(true);
						SetupBuyAvatarDetail(true);
					}
					else if (control.Id == 2027)
					{
						SetupBuyAvatarDetail(false);
						FixedConfig.AvatarCfg avatarCfg3 = ConfigManager.Instance().GetFixedConfig().GetAvatarCfg(avatarSelectedIndex);
						Debug.Log("Buy Avatar - " + avatarCfg3.name);
						playerShow.ChangeAvatar(avatarCfg3.suiteType, avatarCfg3.avtType);
						GameApp.GetInstance().GetGameState().ChangeAvatar(avatarCfg3.suiteType, avatarCfg3.avtType);
						SetupAvatarShopUI(true);
					}
					else if (control.Id == 2029)
					{
						FixedConfig.AvatarCfg avatarCfg4 = ConfigManager.Instance().GetFixedConfig().GetAvatarCfg(avatarSelectedIndex);
						int num2 = 0;
						if (avatarCfg4.priceType == "dollor")
						{
							num2 = (int)((float)(avatarCfg4.price * 800) * 0.1f);
						}
						else if (avatarCfg4.priceType == "gold")
						{
							num2 = (int)((float)avatarCfg4.price * 0.1f);
						}
						else
						{
							num2 = 0;
							Debug.Log("weapon price is wrong!!!");
						}
						SetupSellsDialog(true, num2);
					}
					else if (control.Id == 2028)
					{
						SetupBuyAvatarDetail(false);
					}
					else if (control.Id == 2030)
					{
						SetupBuyPowerUPSDetail(false);
						ArrayList powerUpsCfgs = ConfigManager.Instance().GetFixedConfig().powerUpsCfgs;
						FixedConfig.PowerUPSCfg powerUPSCfg = (FixedConfig.PowerUPSCfg)powerUpsCfgs[powerUpsSelectedIndex];
						if (powerUPSCfg.priceType == "gold")
						{
							if (gameState.GetGold() < powerUPSCfg.price)
							{
								SetupDonnotHaveEnoughMoneyDialog(true, "Insufficient cash! Visit the bank now to get more.");
								return;
							}
							GameApp.GetInstance().GetGameState().LoseGold(powerUPSCfg.price);
							GameApp.GetInstance().GetGameState().BuyPowerUPS(powerUPSCfg.type);
							GameCollectionInfoManager.Instance().GetCurrentInfo().AddTodayPowerUpsBuy((int)powerUPSCfg.type, 1);
						}
						else if (powerUPSCfg.priceType == "dollor")
						{
							if (gameState.GetDollor() < powerUPSCfg.price)
							{
								SetupDonnotHaveEnoughMoneyDialog(true, "Insufficient crystals! Visit the bank now to get more.");
								return;
							}
							GameApp.GetInstance().GetGameState().LoseDollor(powerUPSCfg.price);
							GameApp.GetInstance().GetGameState().BuyPowerUPS(powerUPSCfg.type);
							GameCollectionInfoManager.Instance().GetCurrentInfo().AddTodayPowerUpsBuy((int)powerUPSCfg.type, 1);
						}
						if (powerUPSCfg.type == ItemType.Defibrilator)
						{
							for (int l = 0; l < 4; l++)
							{
								GameApp.GetInstance().GetGameState().BuyPowerUPS(powerUPSCfg.type);
								GameCollectionInfoManager.Instance().GetCurrentInfo().AddTodayPowerUpsBuy((int)powerUPSCfg.type, 1);
							}
						}
						SetupAroundUI(true);
						SetupPowerUpsShopUI(true);
					}
					else if (control.Id == 2031)
					{
						SetupBuyPowerUPSDetail(false);
					}
					else if (control.Id == 2033)
					{
						SetupBuyBankDetail(false);
					}
					else if (control.Id == 2032)
					{
						SetupBuyBankDetail(false);
						m_iapBuyIndex = iapSelectedIndex;
						FixedConfig.IAPCfg iAPCfg = ConfigManager.Instance().GetFixedConfig().GetIAPCfg(m_iapBuyIndex);
						Debug.Log("Buy IAP - " + iAPCfg.iapDollor + " | " + iAPCfg.gameGold + " | " + iAPCfg.gameDollor);
						IABAndroid.purchaseProduct(iAPCfg.iapID);
						SetupLoadingUI(true, string.Empty);
					}
					else if (control.Id == 2034)
					{
						SetupHintDialog(false, 0, 0, 0, string.Empty);
					}
					else if (control.Id == 2035)
					{
						SetupHintDialog(false, 0, 0, 0, string.Empty);
					}
					else if (control.Id == 2036)
					{
						SetupHintDialog(false, 0, 0, 0, string.Empty);
					}
					else if (control.Id == 2037)
					{
						SetupDonnotHaveEnoughMoneyDialog(false, string.Empty);
					}
					else if (control.Id == 2038)
					{
						SetupDonnotHaveEnoughMoneyDialog(false, string.Empty);
						shopType = ShopType.Bank;
						SetupShopUI(true);
					}
					else if (control.Id == 2039)
					{
						SetupSendGiftDialog(false);
					}
					else if (control.Id == 2040)
					{
						gameState.m_ReviewCount = 1;
						GameApp.GetInstance().Save();
						SetupReviewNotificationDialog(false);
						Application.OpenURL("market://details?id=com.trinitigame.android.comds");
					}
					else if (control.Id == 2041)
					{
						SetupReviewNotificationDialog(false);
					}
					else if (control.Id == 2042)
					{
						SetupNotificationDialogUI(false, string.Empty);
					}
					else if (control.Id == 2144)
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
					else if (control.Id == 2145)
					{
						SetupUnlockAccouterDialog(false);
						GameApp.GetInstance().GetGameState().LEVEL_UP = false;
					}
					else if (control.Id == 2146)
					{
						SetupLevelUpHortationDialog(false);
						SetupUnlockAccouterDialog(true);
						GameApp.GetInstance().GetGameState().LEVEL_UP = false;
					}
					else if (control.Id == 2148)
					{
						SetupSellsDialog(false);
					}
					else if (control.Id == 2147)
					{
						SetupSellsDialog(false);
						if (shopType == ShopType.Weapon)
						{
							int gold = 0;
							ArrayList weapons5 = ConfigManager.Instance().GetFixedConfig().weapons;
							for (int m = 0; m < weapons5.Count; m++)
							{
								FixedConfig.WeaponCfg weaponCfg3 = (FixedConfig.WeaponCfg)weapons5[m];
								if (weaponWantToBuy == (WeaponType)weaponCfg3.type)
								{
									if (weaponCfg3.priceType == "dollor")
									{
										gold = (int)((float)(weaponCfg3.price * 800) * 0.1f);
										break;
									}
									if (weaponCfg3.priceType == "gold")
									{
										gold = (int)((float)weaponCfg3.price * 0.1f);
										break;
									}
									gold = 0;
									Debug.Log("weapon price is wrong!!!");
									break;
								}
							}
							SellWeapon(weaponWantToBuy, gold);
						}
						else if (shopType == ShopType.Armor)
						{
							FixedConfig.AvatarCfg avatarCfg5 = ConfigManager.Instance().GetFixedConfig().GetAvatarCfg(avatarSelectedIndex);
							int num3 = 0;
							if (avatarCfg5.priceType == "dollor")
							{
								num3 = (int)((float)(avatarCfg5.price * 800) * 0.1f);
							}
							else if (avatarCfg5.priceType == "gold")
							{
								num3 = (int)((float)avatarCfg5.price * 0.1f);
							}
							else
							{
								num3 = 0;
								Debug.Log("weapon price is wrong!!!");
							}
							SellAvatar(avatarCfg5, num3);
						}
					}
					else if (control.Id != 2151)
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
			UIClickButton control3 = UIUtils.BuildClickButton(2010, new Rect(320f, 588f, 640f, 52f), m_MatDialog01, new Rect(0f, 798f, 640f, 52f), new Rect(0f, 850f, 640f, 52f), new Rect(0f, 798f, 640f, 52f), new Vector2(640f, 52f));
			m_AroundUIGroup.Add(control3);
			playerHpProgressBtn = new UIClickButton();
			playerHpProgressBtn.Id = 2144;
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
			UIClickButton control2 = UIUtils.BuildClickButton(2009, new Rect(244f, 576f, 75f, 52f), m_MatCommonBg, new Rect(1020f, 1f, 1f, 1f), new Rect(1020f, 1f, 1f, 1f), new Rect(1020f, 1f, 1f, 1f), new Vector2(75f, 52f));
			m_JunjieUIGroup.Add(control2);
			control2 = UIUtils.BuildClickButton(2009, new Rect(50f, 420f, 226f, 150f), m_MatCommonBg, new Rect(1020f, 1f, 1f, 1f), new Rect(1020f, 1f, 1f, 1f), new Rect(1020f, 1f, 1f, 1f), new Vector2(226f, 150f));
			m_JunjieUIGroup.Add(control2);
		}
	}

	public void SetupShopUI(bool bShow)
	{
		Debug.LogWarning("function SetupShopUI " + bShow + "|" + shopType);
		SetupShopCommonBarUI(bShow);
		if (shopType == ShopType.Weapon)
		{
			SetupWeaponShopUI(bShow);
		}
		else if (shopType == ShopType.Armor)
		{
			SetupAvatarShopUI(true);
		}
		else if (shopType == ShopType.PowerUPS)
		{
			SetupPowerUpsShopUI(true);
		}
		else if (shopType == ShopType.Bank)
		{
			SetupBankShopUI(true);
			RelevanceIABBasicEvent();
		}
		SetupAroundUI(true);
		SetupShopHighlightBarUI(shopType);
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
			UIClickButton control2 = UIUtils.BuildClickButton(2000, new Rect(295f, 497f, 77f, 104f), m_MatCommonBg, new Rect(1022f, 1f, 1f, 1f), new Rect(9f, 904f, 77f, 104f), new Rect(1022f, 1f, 1f, 1f), new Vector2(77f, 104f));
			m_ShopCommonBarGroup.Add(control2);
			control2 = UIUtils.BuildClickButton(2001, new Rect(372f, 497f, 80f, 104f), m_MatCommonBg, new Rect(1022f, 1f, 1f, 1f), new Rect(939f, 707f, 80f, 104f), new Rect(1022f, 1f, 1f, 1f), new Vector2(80f, 104f));
			m_ShopCommonBarGroup.Add(control2);
			control2 = UIUtils.BuildClickButton(2002, new Rect(452f, 497f, 76f, 104f), m_MatCommonBg, new Rect(1022f, 1f, 1f, 1f), new Rect(85f, 904f, 76f, 104f), new Rect(1022f, 1f, 1f, 1f), new Vector2(76f, 104f));
			m_ShopCommonBarGroup.Add(control2);
			control2 = UIUtils.BuildClickButton(2003, new Rect(528f, 497f, 80f, 104f), m_MatCommonBg, new Rect(160f, 904f, 80f, 104f), new Rect(160f, 904f, 80f, 104f), new Rect(1022f, 1f, 1f, 1f), new Vector2(80f, 104f));
			m_ShopCommonBarGroup.Add(control2);
			control2 = UIUtils.BuildClickButton(2004, new Rect(603f, 503f, 80f, 104f), m_MatCommonBg, new Rect(1022f, 1f, 1f, 1f), new Rect(834f, 700f, 80f, 104f), new Rect(1022f, 1f, 1f, 1f), new Vector2(80f, 104f));
			control2.Enable = false;
			m_ShopCommonBarGroup.Add(control2);
			control2 = UIUtils.BuildClickButton(2005, new Rect(683f, 497f, 74f, 104f), m_MatCommonBg, new Rect(1022f, 1f, 1f, 1f), new Rect(240f, 904f, 74f, 104f), new Rect(1022f, 1f, 1f, 1f), new Vector2(74f, 104f));
			m_ShopCommonBarGroup.Add(control2);
			control = UIUtils.BuildImage(0, new Rect(300f, 450f, 621f, 54f), m_MatShopUI, new Rect(0f, 0f, 621f, 54f), new Vector2(621f, 54f));
			m_ShopCommonBarGroup.Add(control);
			UIPushButton uIPushButton = null;
			uIPushButton = UIUtils.BuildPushButton(2011, new Rect(360f, 451f, 130f, 50f), m_MatShopUI, new Rect(1023f, 1f, 1f, 1f), new Rect(0f, 56f, 130f, 50f), new Rect(1023f, 1f, 1f, 1f), new Vector2(130f, 50f));
			m_ShopCommonBarGroup.Add(uIPushButton);
			if (shopType == ShopType.Weapon)
			{
				uIPushButton.Set(true);
			}
			uIPushButton = UIUtils.BuildPushButton(2012, new Rect(482f, 451f, 133f, 50f), m_MatShopUI, new Rect(1023f, 1f, 1f, 1f), new Rect(130f, 56f, 133f, 50f), new Rect(1023f, 1f, 1f, 1f), new Vector2(133f, 50f));
			m_ShopCommonBarGroup.Add(uIPushButton);
			if (shopType == ShopType.Armor)
			{
				uIPushButton.Set(true);
			}
			uIPushButton = UIUtils.BuildPushButton(2013, new Rect(610f, 451f, 189f, 50f), m_MatShopUI, new Rect(1023f, 1f, 1f, 1f), new Rect(263f, 56f, 189f, 50f), new Rect(1023f, 1f, 1f, 1f), new Vector2(189f, 50f));
			m_ShopCommonBarGroup.Add(uIPushButton);
			if (shopType == ShopType.PowerUPS)
			{
				uIPushButton.Set(true);
			}
			uIPushButton = UIUtils.BuildPushButton(2014, new Rect(795f, 451f, 126f, 50f), m_MatShopUI, new Rect(1023f, 1f, 1f, 1f), new Rect(452f, 56f, 126f, 50f), new Rect(1023f, 1f, 1f, 1f), new Vector2(126f, 50f));
			m_ShopCommonBarGroup.Add(uIPushButton);
			if (shopType == ShopType.Bank)
			{
				uIPushButton.Set(true);
			}
		}
	}

	public void SetupWeaponShopUI(bool bShow)
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
			UIMoveOuter control = UIUtils.BuildUIMoveOuter(2017, new Rect(0f, 0f, 393f, 575f), 10f, 10f);
			m_uiGroup.Add(control);
			if (m_CurBattleWeaponIndex == 0)
			{
				UIClickButton control2 = UIUtils.BuildClickButton(2023, new Rect(55f, 58f, 96f, 72f), m_MatShopUI, new Rect(278f, 860f, 96f, 72f), new Rect(276f, 858f, 100f, 76f), new Rect(278f, 860f, 96f, 72f), new Vector2(96f, 72f));
				m_uiGroup.Add(control2);
			}
			else if (m_CurBattleWeaponIndex == 1)
			{
				UIClickButton control2 = UIUtils.BuildClickButton(2024, new Rect(55f, 58f, 96f, 72f), m_MatShopUI, new Rect(278f, 941f, 96f, 72f), new Rect(276f, 939f, 100f, 76f), new Rect(278f, 941f, 96f, 72f), new Vector2(96f, 72f));
				m_uiGroup.Add(control2);
			}
			SetupWeaponPageView();
		}
	}

	public void SetupWeaponPageView()
	{
		if (m_MatWeaponIcons == null)
		{
			m_MatWeaponIcons = LoadUIMaterial("Zombie3D/UI/Materials/WeaponIcons");
		}
		UIImage uIImage = null;
		UIClickButton uIClickButton = null;
		UIText uIText = null;
		int num = 0;
		if (m_WeaponPageView != null)
		{
			num = m_WeaponPageView.PageIndex;
			m_uiGroup.Remove(m_WeaponPageView);
			m_WeaponPageView = null;
		}
		m_WeaponPageView = new UIScrollPageView();
		m_WeaponPageView.SetMoveParam(AutoUI.AutoRect(new Rect(352f, 80f, 544f, 342f)), AutoUI.AutoDistance(20f), AutoUI.AutoDistance(20f));
		m_WeaponPageView.Rect = AutoUI.AutoRect(new Rect(352f, 100f, 544f, 342f));
		m_WeaponPageView.ListOri = UIScrollPageView.ListOrientation.Horizontal;
		m_WeaponPageView.ViewSize = AutoUI.AutoSize(new Vector2(272f, 171f));
		m_WeaponPageView.ItemSpacingV = AutoUI.AutoDistance(0f);
		m_WeaponPageView.ItemSpacingH = AutoUI.AutoDistance(0f);
		m_WeaponPageView.SetClip(AutoUI.AutoRect(new Rect(350f, 80f, 550f, 342f)));
		m_WeaponPageView.Bounds = AutoUI.AutoRect(new Rect(352f, 80f, 544f, 342f));
		m_uiGroup.Add(m_WeaponPageView);
		ArrayList weapons = ConfigManager.Instance().GetFixedConfig().weapons;
		float num2 = 272f;
		float num3 = 171f;
		Rect rcMat = new Rect(773f, 15f, 150f, 112f);
		Rect rcMat2 = new Rect(621f, 112f, 150f, 112f);
		Rect rcMat3 = new Rect(621f, 0f, 150f, 112f);
		int num4 = 2;
		for (int i = 0; (float)i < (float)weapons.Count / (float)num4; i++)
		{
			UIGroupControl uIGroupControl = UIUtils.BuildUIGroupControl(0, new Rect(0f, 0f, 272f, 342f));
			int num5 = weapons.Count - i * num4;
			for (int j = 0; j < num4; j++)
			{
				int num6 = i * num4 + j;
				if (num6 >= weapons.Count)
				{
					break;
				}
				FixedConfig.WeaponCfg weaponCfg = (FixedConfig.WeaponCfg)weapons[num6];
				if (gameState == null)
				{
					gameState = GameApp.GetInstance().GetGameState();
				}
				PropsAdditionImpl propsAdditionImpl = gameState.CheckAgeing(gameState.SetPropsAdditionsID(weaponCfg.type, 2), enPropsAdditionType.E_Damage, enPropsAdditionPart.E_Weapon);
				Color color = Constant.TextCommonColor;
				float num7 = 0f + (float)(j / 2) * num2;
				float num8 = num3 - (float)(j % 2) * num3;
				uIImage = UIUtils.BuildImage(0, new Rect(num7, num8 + 10f, 265f, 153f), m_MatShopUI, new Rect(0f, 854f, 265f, 153f), new Vector2(265f, 153f));
				uIGroupControl.Add(uIImage);
				if (propsAdditionImpl != null)
				{
					uIImage = UIUtils.BuildImage(0, new Rect(num7 - 8f, num8 + 3f, 280f, 170f), m_MatShopUI, new Rect(392f, 854f, 280f, 170f), new Vector2(280f, 170f));
					uIGroupControl.Add(uIImage);
					color = new Color(1f, 1f, 1f, 1f);
				}
				Rect weaponIconTexture = GetWeaponIconTexture((WeaponType)weaponCfg.type);
				float num9 = num7 + 76f;
				float num10 = num8 + 87f;
				uIImage = UIUtils.BuildImage(0, new Rect(num9 - weaponIconTexture.width / 2f, num10 - weaponIconTexture.height / 2f, weaponIconTexture.width, weaponIconTexture.height), m_MatWeaponIcons, weaponIconTexture, new Vector2(weaponIconTexture.width, weaponIconTexture.height));
				uIGroupControl.Add(uIImage);
				float num11 = 160f;
				float num12 = 110f;
				float num13 = 200f;
				if (AutoUI.IsRetain == AutoUI.RESOLUTION.LOWDEFINITION)
				{
					num13 = 220f;
				}
				uIText = UIUtils.BuildUIText(0, new Rect(num7 + num13 - 200f, num8 + num12, 200f, 20f), UIText.enAlignStyle.right);
				uIText.Set("Zombie3D/Font/037-CAI978-13", "TIER ", m_ShopPropColor);
				uIGroupControl.Add(uIText);
				uIText = UIUtils.BuildUIText(0, new Rect(num7 + num13, num8 + num12, 200f, 20f), UIText.enAlignStyle.left);
				uIText.Set("Zombie3D/Font/037-CAI978-13", weaponCfg.mClass.ToString(), m_ShopPropValueColor);
				uIGroupControl.Add(uIText);
				uIText = UIUtils.BuildUIText(0, new Rect(num7 + num13 - 200f, num8 + num12 - 20f, 200f, 20f), UIText.enAlignStyle.right);
				uIText.Set("Zombie3D/Font/037-CAI978-13", "DMG ", m_ShopPropColor);
				uIGroupControl.Add(uIText);
				string text = weaponCfg.dmg.ToString();
				Color color2 = m_ShopPropValueColor;
				if (propsAdditionImpl != null)
				{
					text = ((int)(propsAdditionImpl.GetEffect(weaponCfg.dmg) * weaponCfg.rpm)).ToString();
					color2 = m_ShopPropsAdditionColor;
				}
				uIText = UIUtils.BuildUIText(0, new Rect(num7 + num13, num8 + num12 - 20f, 200f, 20f), UIText.enAlignStyle.left);
				uIText.Set("Zombie3D/Font/037-CAI978-13", text, color2);
				uIGroupControl.Add(uIText);
				uIText = UIUtils.BuildUIText(0, new Rect(num7 + num13 - 200f, num8 + num12 - 40f, 200f, 20f), UIText.enAlignStyle.right);
				uIText.Set("Zombie3D/Font/037-CAI978-13", "RPM ", m_ShopPropColor);
				uIGroupControl.Add(uIText);
				uIText = UIUtils.BuildUIText(0, new Rect(num7 + num13, num8 + num12 - 40f, 200f, 20f), UIText.enAlignStyle.left);
				uIText.Set("Zombie3D/Font/037-CAI978-13", string.Empty + Mathf.RoundToInt(60f / weaponCfg.rpm), m_ShopPropValueColor);
				uIGroupControl.Add(uIText);
				uIText = UIUtils.BuildUIText(0, new Rect(num7 + num13 - 200f, num8 + num12 - 60f, 200f, 20f), UIText.enAlignStyle.right);
				uIText.Set("Zombie3D/Font/037-CAI978-13", "SPD ", m_ShopPropColor);
				uIGroupControl.Add(uIText);
				uIText = UIUtils.BuildUIText(0, new Rect(num7 + num13, num8 + num12 - 60f, 200f, 20f), UIText.enAlignStyle.left);
				if (weaponCfg.spd > 0f)
				{
					uIText.Set("Zombie3D/Font/037-CAI978-13", weaponCfg.spd * 100f + "%", m_ShopPropSPDPositiveValueColor);
				}
				else
				{
					uIText.Set("Zombie3D/Font/037-CAI978-13", string.Empty + weaponCfg.spd * 100f + "%", m_ShopPropSPDNegativeValueColor);
				}
				uIGroupControl.Add(uIText);
				if (weaponCfg.bNewWeapon)
				{
					uIImage = UIUtils.BuildImage(0, new Rect(num7 + 205f, num8 + 115f, 75f, 65f), m_MatShopUI, new Rect(944f, 54f, 75f, 65f), new Vector2(75f, 65f));
					uIGroupControl.Add(uIImage);
				}
				List<WeaponType> battleWeapons = GameApp.GetInstance().GetGameState().GetBattleWeapons();
				List<WeaponType> weapons2 = GameApp.GetInstance().GetGameState().GetWeapons();
				bool flag = false;
				for (int k = 0; k < weapons2.Count; k++)
				{
					if (weaponCfg.type == (int)weapons2[k])
					{
						flag = true;
						if (battleWeapons.Contains(weapons2[k]))
						{
							uIImage = UIUtils.BuildImage(0, new Rect(num7, num8 + 30f, rcMat3.width, rcMat3.height), m_MatShopUI, rcMat3, new Vector2(rcMat3.width, rcMat3.height));
							uIGroupControl.Add(uIImage);
						}
						else
						{
							uIImage = UIUtils.BuildImage(0, new Rect(num7, num8 + 30f, rcMat2.width, rcMat2.height), m_MatShopUI, rcMat2, new Vector2(rcMat2.width, rcMat2.height));
							uIGroupControl.Add(uIImage);
						}
						uIClickButton = UIUtils.BuildClickButton(2043 + num6, new Rect(num7 + 35f, num8 + 30f, 185f, 111f), m_MatTransparentUI, new Rect(1f, 1f, 1f, 1f), new Rect(1f, 1f, 1f, 1f), new Rect(1f, 1f, 1f, 1f), new Vector2(185f, 111f));
						uIGroupControl.Add(uIClickButton);
					}
				}
				if (!flag)
				{
					if (weaponCfg.price <= 0)
					{
						uIImage = UIUtils.BuildImage(0, new Rect(num7, num8 + 35f, 151f, 112f), m_MatShopUI, new Rect(622f, 228f, 151f, 112f), new Vector2(151f, 112f));
						uIGroupControl.Add(uIImage);
					}
					if (weaponCfg.levelLimit > GameApp.GetInstance().GetGameState().GetPlayerLevel() && weaponCfg.type != 28 || weaponCfg.type == 28 && !battleWeapons.Contains(WeaponType.Ion_Cannon) && !battleWeapons.Contains(WeaponType.Ion_CannonI))
					{
						uIImage = UIUtils.BuildImage(0, new Rect(num7, num8 + 30f, rcMat.width, rcMat.height), m_MatShopUI, rcMat, new Vector2(rcMat.width, rcMat.height));
						uIGroupControl.Add(uIImage);
					}
					uIClickButton = UIUtils.BuildClickButton(2043 + num6, new Rect(num7 + 35f, num8 + 30f, 185f, 111f), m_MatTransparentUI, new Rect(1f, 1f, 1f, 1f), new Rect(1f, 1f, 1f, 1f), new Rect(1f, 1f, 1f, 1f), new Vector2(185f, 111f));
					uIGroupControl.Add(uIClickButton);
				}
				uIText = UIUtils.BuildUIText(0, new Rect(num7 + 22f, num8 + 140f, 200f, 20f), UIText.enAlignStyle.left);
				uIText.Set("Zombie3D/Font/037-CAI978-15", weaponCfg.name, color);
				uIGroupControl.Add(uIText);
				uIText = UIUtils.BuildUIText(0, new Rect(num7 + 22f, num8 + 13f, 200f, 20f), UIText.enAlignStyle.left);
				if (weaponCfg.type != 28)
				{
					uIText.Set("Zombie3D/Font/037-CAI978-15", "LV " + weaponCfg.levelLimit, color);
				}
				else
				{
					uIText.Set("Zombie3D/Font/037-CAI978-15", "Unlock", color);
				}
				uIGroupControl.Add(uIText);
				if (weaponCfg.priceType == "gold")
				{
					uIImage = UIUtils.BuildImage(0, new Rect(num7 + 150f, num8 + 12f, 35f, 21f), m_MatShopUI, new Rect(586f, 85f, 35f, 21f), new Vector2(35f, 21f));
					uIGroupControl.Add(uIImage);
				}
				else if (weaponCfg.priceType == "dollor")
				{
					uIImage = UIUtils.BuildImage(0, new Rect(num7 + 150f, num8 + 7f, 35f, 29f), m_MatShopUI, new Rect(586f, 56f, 35f, 29f), new Vector2(35f, 29f));
					uIGroupControl.Add(uIImage);
				}
				uIText = UIUtils.BuildUIText(0, new Rect(num7 + 184f, num8 + 13f, 200f, 20f), UIText.enAlignStyle.left);
				uIText.Set("Zombie3D/Font/037-CAI978-15", weaponCfg.price.ToString(), color);
				uIGroupControl.Add(uIText);
			}
			m_WeaponPageView.Add(uIGroupControl);
		}
		if (num > 0)
		{
			m_WeaponPageView.PageIndex = num;
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
		m_ShopPageViewScrollBar.SetPageCount(m_WeaponPageView.PageCount);
		m_ShopPageViewScrollBar.SetScrollBarTexture(m_MatShopUI, AutoUI.AutoRect(new Rect(597f, 107f, 11f, 11f)), m_MatShopUI, AutoUI.AutoRect(new Rect(609f, 107f, 11f, 11f)));
		m_ShopPageViewScrollBar.SetScrollPercent(0f);
		m_uiGroup.Add(m_ShopPageViewScrollBar);
		m_WeaponPageView.ScrollBar = m_ShopPageViewScrollBar;
		uIImage = UIUtils.BuildImage(0, new Rect(330f, 60f, 27f, 384f), m_MatShopUI, new Rect(0f, 106f, 27f, 384f), new Vector2(27f, 384f));
		m_uiGroup.Add(uIImage);
		uIImage = UIUtils.BuildImage(0, new Rect(882f, 60f, 45f, 388f), m_MatShopUI, new Rect(33f, 106f, 45f, 384f), new Vector2(45f, 384f));
		m_uiGroup.Add(uIImage);
	}

	public void SetupAvatarShopUI(bool bShow)
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
			UIMoveOuter control = UIUtils.BuildUIMoveOuter(2017, new Rect(0f, 0f, 393f, 575f), 10f, 10f);
			m_uiGroup.Add(control);
			SetupAvatarPageView();
		}
	}

	public void SetupAvatarPageView()
	{
		if (m_MatAvatarIcons == null)
		{
			m_MatAvatarIcons = LoadUIMaterial("Zombie3D/UI/Materials/AvatarIcons");
		}
		UIImage uIImage = null;
		UIClickButton uIClickButton = null;
		UIText uIText = null;
		int num = 0;
		if (m_AvatarPageView != null)
		{
			num = m_AvatarPageView.PageIndex;
			m_uiGroup.Remove(m_AvatarPageView);
			m_AvatarPageView = null;
		}
		m_AvatarPageView = new UIScrollPageView();
		m_AvatarPageView.SetMoveParam(AutoUI.AutoRect(new Rect(352f, 80f, 544f, 342f)), AutoUI.AutoDistance(20f), AutoUI.AutoDistance(20f));
		m_AvatarPageView.Rect = AutoUI.AutoRect(new Rect(352f, 80f, 520f, 342f));
		m_AvatarPageView.ListOri = UIScrollPageView.ListOrientation.Horizontal;
		m_AvatarPageView.ViewSize = AutoUI.AutoSize(new Vector2(272f, 171f));
		m_AvatarPageView.ItemSpacingV = AutoUI.AutoDistance(0f);
		m_AvatarPageView.ItemSpacingH = AutoUI.AutoDistance(0f);
		m_AvatarPageView.SetClip(AutoUI.AutoRect(new Rect(350f, 80f, 550f, 360f)));
		m_AvatarPageView.Bounds = AutoUI.AutoRect(new Rect(352f, 80f, 544f, 342f));
		m_uiGroup.Add(m_AvatarPageView);
		ArrayList avatarCfgs = ConfigManager.Instance().GetFixedConfig().avatarCfgs;
		float num2 = 272f;
		float num3 = 171f;
		Rect rcMat = new Rect(773f, 15f, 150f, 112f);
		Rect rcMat2 = new Rect(621f, 112f, 150f, 112f);
		Rect rcMat3 = new Rect(621f, 0f, 150f, 112f);
		int num4 = 2;
		for (int i = 0; (float)i < (float)avatarCfgs.Count / (float)num4; i++)
		{
			UIGroupControl uIGroupControl = UIUtils.BuildUIGroupControl(0, new Rect(0f, 0f, 272f, 342f));
			int num5 = avatarCfgs.Count - i * num4;
			for (int j = 0; j < num4; j++)
			{
				int num6 = i * num4 + j;
				if (num6 >= avatarCfgs.Count)
				{
					continue;
				}
				FixedConfig.AvatarCfg avatarCfg = ConfigManager.Instance().GetFixedConfig().GetAvatarCfg(num6);
				int id = gameState.SetPropsAdditionsID((int)avatarCfg.suiteType, (int)avatarCfg.avtType);
				PropsAdditionImpl propsAddition = gameState.GetPropsAddition(id);
				Color color = Constant.TextCommonColor;
				float num7 = 0f + (float)(j / 2) * num2;
				float num8 = num3 - (float)(j % 2) * num3;
				uIImage = UIUtils.BuildImage(0, new Rect(num7, num8 + 10f, 265f, 153f), m_MatShopUI, new Rect(0f, 854f, 265f, 153f), new Vector2(265f, 153f));
				uIGroupControl.Add(uIImage);
				if (propsAddition != null)
				{
					color = new Color(1f, 1f, 1f, 1f);
					uIImage = UIUtils.BuildImage(0, new Rect(num7 - 8f, num8 + 3f, 280f, 170f), m_MatShopUI, new Rect(392f, 854f, 280f, 170f), new Vector2(280f, 170f));
					uIGroupControl.Add(uIImage);
				}
				Rect avatarIconTexture = GetAvatarIconTexture(avatarCfg.suiteType, avatarCfg.avtType);
				float num9 = num7 + 76f;
				float num10 = num8 + 87f;
				uIImage = UIUtils.BuildImage(0, new Rect(num9 - avatarIconTexture.width / 2f, num10 - avatarIconTexture.height / 2f, avatarIconTexture.width, avatarIconTexture.height), m_MatAvatarIcons, avatarIconTexture, new Vector2(avatarIconTexture.width, avatarIconTexture.height));
				uIGroupControl.Add(uIImage);
				uIText = UIUtils.BuildUIText(0, new Rect(num7 + 22f, num8 + 140f, 200f, 20f), UIText.enAlignStyle.left);
				uIText.Set("Zombie3D/Font/037-CAI978-15", avatarCfg.name, color);
				uIGroupControl.Add(uIText);
				uIText = UIUtils.BuildUIText(0, new Rect(num7 + 22f, num8 + 13f, 200f, 20f), UIText.enAlignStyle.left);
				uIText.Set("Zombie3D/Font/037-CAI978-15", "LV " + avatarCfg.levelLimit, color);
				uIGroupControl.Add(uIText);
				float num11 = 165f;
				float num12 = 110f;
				float num13 = 200f;
				float num14 = num8 + num12;
				uIText = UIUtils.BuildUIText(0, new Rect(num7 + num13 - 200f, num14, 200f, 20f), UIText.enAlignStyle.right);
				uIText.Set("Zombie3D/Font/037-CAI978-13", "TIER ", m_ShopPropColor);
				uIGroupControl.Add(uIText);
				uIText = UIUtils.BuildUIText(0, new Rect(num7 + num13, num14, 200f, 20f), UIText.enAlignStyle.left);
				uIText.Set("Zombie3D/Font/037-CAI978-13", avatarCfg.m_Class.ToString(), m_ShopPropValueColor);
				uIGroupControl.Add(uIText);
				if (avatarCfg.prop.m_DefenceAdditive > 0f)
				{
					PropsAdditionImpl propsAdditionImpl = null;
					float num15 = avatarCfg.prop.m_DefenceAdditive;
					Color color2 = m_ShopPropValueColor;
					propsAdditionImpl = gameState.CheckAgeing(id, enPropsAdditionType.E_DefenceAdditive, (enPropsAdditionPart)avatarCfg.avtType);
					if (propsAdditionImpl != null)
					{
						num15 = propsAdditionImpl.GetEffect(num15);
						color2 = m_ShopPropsAdditionColor;
					}
					num14 -= 20f;
					uIText = UIUtils.BuildUIText(0, new Rect(num7 + num13 - 200f, num14, 200f, 20f), UIText.enAlignStyle.right);
					uIText.Set("Zombie3D/Font/037-CAI978-13", "DEF ", m_ShopPropColor);
					uIGroupControl.Add(uIText);
					uIText = UIUtils.BuildUIText(0, new Rect(num7 + num13, num14, 200f, 20f), UIText.enAlignStyle.left);
					uIText.Set("Zombie3D/Font/037-CAI978-13", string.Empty + num15 * 100f + "%", color2);
					uIGroupControl.Add(uIText);
				}
				if (avatarCfg.prop.m_AttackAdditive > 0f)
				{
					PropsAdditionImpl propsAdditionImpl2 = null;
					float num16 = avatarCfg.prop.m_AttackAdditive;
					Color color3 = m_ShopPropValueColor;
					propsAdditionImpl2 = gameState.CheckAgeing(id, enPropsAdditionType.E_AttackAdditive, (enPropsAdditionPart)avatarCfg.avtType);
					if (propsAdditionImpl2 != null)
					{
						num16 = propsAdditionImpl2.GetEffect(num16);
						color3 = m_ShopPropsAdditionColor;
					}
					num14 -= 20f;
					uIText = UIUtils.BuildUIText(0, new Rect(num7 + num13 - 200f, num14, 200f, 20f), UIText.enAlignStyle.right);
					uIText.Set("Zombie3D/Font/037-CAI978-13", "DMG ", m_ShopPropColor);
					uIGroupControl.Add(uIText);
					uIText = UIUtils.BuildUIText(0, new Rect(num7 + num13, num14, 200f, 20f), UIText.enAlignStyle.left);
					uIText.Set("Zombie3D/Font/037-CAI978-13", string.Empty + num16 * 100f + "%", color3);
					uIGroupControl.Add(uIText);
				}
				if (avatarCfg.prop.m_SpeedAdditive != 0f)
				{
					PropsAdditionImpl propsAdditionImpl3 = null;
					float num17 = avatarCfg.prop.m_SpeedAdditive;
					Color color4 = m_ShopPropValueColor;
					propsAdditionImpl3 = gameState.CheckAgeing(id, enPropsAdditionType.E_SpeedAdditive, (enPropsAdditionPart)avatarCfg.avtType);
					if (propsAdditionImpl3 != null)
					{
						num17 = propsAdditionImpl3.GetEffect(num17);
						color4 = m_ShopPropsAdditionColor;
					}
					num14 -= 20f;
					uIText = UIUtils.BuildUIText(0, new Rect(num7 + num13 - 200f, num14, 200f, 20f), UIText.enAlignStyle.right);
					uIText.Set("Zombie3D/Font/037-CAI978-13", "SPD ", m_ShopPropColor);
					uIGroupControl.Add(uIText);
					uIText = UIUtils.BuildUIText(0, new Rect(num7 + num13, num14, 200f, 20f), UIText.enAlignStyle.left);
					if (avatarCfg.prop.m_SpeedAdditive > 0f)
					{
						uIText.Set("Zombie3D/Font/037-CAI978-13", num17 * 100f + "%", color4);
					}
					else
					{
						uIText.Set("Zombie3D/Font/037-CAI978-13", string.Empty + avatarCfg.prop.m_SpeedAdditive * 100f + "%", m_ShopPropSPDNegativeValueColor);
					}
					uIGroupControl.Add(uIText);
				}
				if (avatarCfg.prop.m_HpAdditive > 0f)
				{
					PropsAdditionImpl propsAdditionImpl4 = null;
					float num18 = avatarCfg.prop.m_HpAdditive;
					Color color5 = m_ShopPropValueColor;
					propsAdditionImpl4 = gameState.CheckAgeing(id, enPropsAdditionType.E_HpAdditive, (enPropsAdditionPart)avatarCfg.avtType);
					if (propsAdditionImpl4 != null)
					{
						num18 = propsAdditionImpl4.GetEffect(num18);
						color5 = m_ShopPropsAdditionColor;
					}
					num14 -= 20f;
					uIText = UIUtils.BuildUIText(0, new Rect(num7 + num13 - 200f, num14, 200f, 20f), UIText.enAlignStyle.right);
					uIText.Set("Zombie3D/Font/037-CAI978-13", "HP ", m_ShopPropColor);
					uIGroupControl.Add(uIText);
					uIText = UIUtils.BuildUIText(0, new Rect(num7 + num13, num14, 200f, 20f), UIText.enAlignStyle.left);
					uIText.Set("Zombie3D/Font/037-CAI978-13", string.Empty + num18 * 100f + "%", color5);
					uIGroupControl.Add(uIText);
				}
				if (avatarCfg.prop.m_AttackSpeedAdditive > 0f)
				{
					PropsAdditionImpl propsAdditionImpl5 = null;
					float num19 = avatarCfg.prop.m_AttackSpeedAdditive;
					Color color6 = m_ShopPropValueColor;
					propsAdditionImpl5 = gameState.CheckAgeing(id, enPropsAdditionType.E_AttackSpeedAdditive, (enPropsAdditionPart)avatarCfg.avtType);
					if (propsAdditionImpl5 != null)
					{
						num19 = propsAdditionImpl5.GetEffect(num19);
						color6 = m_ShopPropsAdditionColor;
					}
					num14 -= 20f;
					uIText = UIUtils.BuildUIText(0, new Rect(num7 + num13 - 200f, num14, 200f, 20f), UIText.enAlignStyle.right);
					uIText.Set("Zombie3D/Font/037-CAI978-13", "ASPD ", m_ShopPropColor);
					uIGroupControl.Add(uIText);
					uIText = UIUtils.BuildUIText(0, new Rect(num7 + num13, num14, 200f, 20f), UIText.enAlignStyle.left);
					uIText.Set("Zombie3D/Font/037-CAI978-13", string.Empty + num19 * 100f + "%", color6);
					uIGroupControl.Add(uIText);
				}
				if (avatarCfg.prop.m_StaminaAdd > 0f)
				{
					PropsAdditionImpl propsAdditionImpl6 = null;
					float num20 = avatarCfg.prop.m_StaminaAdd;
					Color color7 = m_ShopPropValueColor;
					propsAdditionImpl6 = gameState.CheckAgeing(id, enPropsAdditionType.E_StaminaAdditive, (enPropsAdditionPart)avatarCfg.avtType);
					if (propsAdditionImpl6 != null)
					{
						num20 = propsAdditionImpl6.GetEffect(num20);
						color7 = m_ShopPropsAdditionColor;
					}
					num14 -= 20f;
					uIText = UIUtils.BuildUIText(0, new Rect(num7 + num13 - 200f, num14, 200f, 20f), UIText.enAlignStyle.right);
					uIText.Set("Zombie3D/Font/037-CAI978-13", "STA ", m_ShopPropColor);
					uIGroupControl.Add(uIText);
					uIText = UIUtils.BuildUIText(0, new Rect(num7 + num13, num14, 200f, 20f), UIText.enAlignStyle.left);
					uIText.Set("Zombie3D/Font/037-CAI978-13", string.Empty + num20 * 100f + "%", color7);
					uIGroupControl.Add(uIText);
				}
				if (avatarCfg.prop.m_ExpAdditive > 0f)
				{
					PropsAdditionImpl propsAdditionImpl7 = null;
					float num21 = avatarCfg.prop.m_ExpAdditive;
					Color color8 = m_ShopPropValueColor;
					propsAdditionImpl7 = gameState.CheckAgeing(id, enPropsAdditionType.E_ExpAdditive, (enPropsAdditionPart)avatarCfg.avtType);
					if (propsAdditionImpl7 != null)
					{
						num21 = propsAdditionImpl7.GetEffect(num21);
						color8 = m_ShopPropsAdditionColor;
					}
					num14 -= 20f;
					uIText = UIUtils.BuildUIText(0, new Rect(num7 + num13 - 200f, num14, 200f, 20f), UIText.enAlignStyle.right);
					uIText.Set("Zombie3D/Font/037-CAI978-13", "EXP ", m_ShopPropColor);
					uIGroupControl.Add(uIText);
					uIText = UIUtils.BuildUIText(0, new Rect(num7 + num13, num14, 200f, 20f), UIText.enAlignStyle.left);
					uIText.Set("Zombie3D/Font/037-CAI978-13", string.Empty + num21 * 100f + "%", color8);
					uIGroupControl.Add(uIText);
				}
				if (avatarCfg.prop.m_GoldAdditive > 0f)
				{
					PropsAdditionImpl propsAdditionImpl8 = null;
					float num22 = avatarCfg.prop.m_GoldAdditive;
					Color color9 = m_ShopPropValueColor;
					propsAdditionImpl8 = gameState.CheckAgeing(id, enPropsAdditionType.E_CashAdditive, (enPropsAdditionPart)avatarCfg.avtType);
					if (propsAdditionImpl8 != null)
					{
						num22 = propsAdditionImpl8.GetEffect(num22);
						color9 = m_ShopPropsAdditionColor;
					}
					num14 -= 20f;
					uIText = UIUtils.BuildUIText(0, new Rect(num7 + num13 - 200f, num14, 200f, 20f), UIText.enAlignStyle.right);
					uIText.Set("Zombie3D/Font/037-CAI978-13", "CASH ", m_ShopPropColor);
					uIGroupControl.Add(uIText);
					uIText = UIUtils.BuildUIText(0, new Rect(num7 + num13, num14, 200f, 20f), UIText.enAlignStyle.left);
					uIText.Set("Zombie3D/Font/037-CAI978-13", string.Empty + num22 * 100f + "%", color9);
					uIGroupControl.Add(uIText);
				}
				if (avatarCfg.bNewAvatar)
				{
					uIImage = UIUtils.BuildImage(0, new Rect(num7 + 205f, num8 + 115f, 75f, 65f), m_MatShopUI, new Rect(944f, 54f, 75f, 65f), new Vector2(75f, 65f));
					uIGroupControl.Add(uIImage);
				}
				Hashtable avatars = GameApp.GetInstance().GetGameState().GetAvatars();
				bool flag = false;
				foreach (Avatar key in avatars.Keys)
				{
					if (key.SuiteType == avatarCfg.suiteType && key.AvtType == avatarCfg.avtType)
					{
						flag = true;
						if ((bool)avatars[key])
						{
							uIImage = UIUtils.BuildImage(0, new Rect(num7, num8 + 30f, rcMat3.width, rcMat3.height), m_MatShopUI, rcMat3, new Vector2(161f, 106f));
							uIGroupControl.Add(uIImage);
						}
						else
						{
							uIImage = UIUtils.BuildImage(0, new Rect(num7, num8 + 30f, rcMat2.width, rcMat2.height), m_MatShopUI, rcMat2, new Vector2(161f, 106f));
							uIGroupControl.Add(uIImage);
						}
						uIClickButton = UIUtils.BuildClickButton(2043 + num6, new Rect(num7 + 35f, num8 + 30f, 185f, 111f), m_MatTransparentUI, new Rect(1f, 1f, 1f, 1f), new Rect(1f, 1f, 1f, 1f), new Rect(1f, 1f, 1f, 1f), new Vector2(185f, 111f));
						uIGroupControl.Add(uIClickButton);
						break;
					}
				}
				if (!flag)
				{
					if (avatarCfg.price <= 0)
					{
						uIImage = UIUtils.BuildImage(0, new Rect(num7, num8 + 35f, 151f, 112f), m_MatShopUI, new Rect(622f, 228f, 151f, 112f), new Vector2(151f, 112f));
						uIGroupControl.Add(uIImage);
					}
					if (avatarCfg.levelLimit > GameApp.GetInstance().GetGameState().GetPlayerLevel())
					{
						uIImage = UIUtils.BuildImage(0, new Rect(num7, num8 + 30f, rcMat.width, rcMat.height), m_MatShopUI, rcMat, new Vector2(rcMat.width, rcMat.height));
						uIGroupControl.Add(uIImage);
					}
					uIClickButton = UIUtils.BuildClickButton(2043 + num6, new Rect(num7 + 35f, num8 + 30f, 185f, 111f), m_MatTransparentUI, new Rect(1f, 1f, 1f, 1f), new Rect(1f, 1f, 1f, 1f), new Rect(1f, 1f, 1f, 1f), new Vector2(185f, 111f));
					uIGroupControl.Add(uIClickButton);
				}
				if (avatarCfg.priceType == "gold")
				{
					uIImage = UIUtils.BuildImage(0, new Rect(num7 + 150f, num8 + 12f, 35f, 21f), m_MatShopUI, new Rect(586f, 85f, 35f, 21f), new Vector2(35f, 21f));
					uIGroupControl.Add(uIImage);
				}
				else if (avatarCfg.priceType == "dollor")
				{
					uIImage = UIUtils.BuildImage(0, new Rect(num7 + 150f, num8 + 7f, 35f, 29f), m_MatShopUI, new Rect(586f, 56f, 35f, 29f), new Vector2(35f, 29f));
					uIGroupControl.Add(uIImage);
				}
				uIText = UIUtils.BuildUIText(0, new Rect(num7 + 184f, num8 + 13f, 200f, 20f), UIText.enAlignStyle.left);
				uIText.Set("Zombie3D/Font/037-CAI978-15", avatarCfg.price.ToString(), color);
				uIGroupControl.Add(uIText);
			}
			m_AvatarPageView.Add(uIGroupControl);
		}
		if (num > 0)
		{
			m_AvatarPageView.PageIndex = num;
		}
		if (m_ShopPageViewScrollBar != null)
		{
			m_uiGroup.Remove(m_ShopPageViewScrollBar);
			m_ShopPageViewScrollBar = null;
		}
		m_ShopPageViewScrollBar = new UIDotScrollBar();
		m_ShopPageViewScrollBar.Rect = AutoUI.AutoRect(new Rect(350f, 50f, 100f, 20f));
		m_ShopPageViewScrollBar.ScrollOri = UIDotScrollBar.ScrollOrientation.Horizontal;
		m_ShopPageViewScrollBar.DotPageWidth = AutoUI.AutoDistance(20f);
		m_ShopPageViewScrollBar.SetPageCount(m_AvatarPageView.PageCount);
		m_ShopPageViewScrollBar.SetScrollBarTexture(m_MatShopUI, AutoUI.AutoRect(new Rect(597f, 107f, 11f, 11f)), m_MatShopUI, AutoUI.AutoRect(new Rect(609f, 107f, 11f, 11f)));
		m_ShopPageViewScrollBar.SetScrollPercent(0f);
		m_uiGroup.Add(m_ShopPageViewScrollBar);
		m_AvatarPageView.ScrollBar = m_ShopPageViewScrollBar;
		uIImage = UIUtils.BuildImage(0, new Rect(330f, 60f, 27f, 384f), m_MatShopUI, new Rect(0f, 106f, 27f, 384f), new Vector2(27f, 384f));
		m_uiGroup.Add(uIImage);
		uIImage = UIUtils.BuildImage(0, new Rect(882f, 60f, 45f, 388f), m_MatShopUI, new Rect(33f, 106f, 45f, 384f), new Vector2(45f, 384f));
		m_uiGroup.Add(uIImage);
	}

	public void SetupPowerUpsShopUI(bool bShow)
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
			UIMoveOuter control = UIUtils.BuildUIMoveOuter(2017, new Rect(0f, 0f, 393f, 575f), 10f, 10f);
			m_uiGroup.Add(control);
			SetupPowerUpsPageView();
		}
	}

	public void SetupPowerUpsPageView()
	{
		if (m_MatPowerUPSIcons == null)
		{
			m_MatPowerUPSIcons = LoadUIMaterial("Zombie3D/UI/Materials/PowerUPSIcons");
		}
		UIImage uIImage = null;
		UIClickButton uIClickButton = null;
		UIText uIText = null;
		int num = 0;
		if (m_PowerUPSPageView != null)
		{
			num = m_PowerUPSPageView.PageIndex;
			m_uiGroup.Remove(m_PowerUPSPageView);
			m_PowerUPSPageView = null;
		}
		m_PowerUPSPageView = new UIScrollPageView();
		m_PowerUPSPageView.SetMoveParam(AutoUI.AutoRect(new Rect(352f, 80f, 544f, 342f)), AutoUI.AutoDistance(20f), AutoUI.AutoDistance(20f));
		m_PowerUPSPageView.Rect = AutoUI.AutoRect(new Rect(352f, 80f, 544f, 342f));
		m_PowerUPSPageView.ListOri = UIScrollPageView.ListOrientation.Horizontal;
		m_PowerUPSPageView.ViewSize = AutoUI.AutoSize(new Vector2(272f, 171f));
		m_PowerUPSPageView.ItemSpacingV = AutoUI.AutoDistance(0f);
		m_PowerUPSPageView.ItemSpacingH = AutoUI.AutoDistance(0f);
		m_PowerUPSPageView.SetClip(AutoUI.AutoRect(new Rect(350f, 80f, 550f, 360f)));
		m_PowerUPSPageView.Bounds = AutoUI.AutoRect(new Rect(352f, 80f, 544f, 342f));
		m_uiGroup.Add(m_PowerUPSPageView);
		ArrayList powerUpsCfgs = ConfigManager.Instance().GetFixedConfig().powerUpsCfgs;
		float num2 = 272f;
		float num3 = 171f;
		int num4 = 2;
		for (int i = 0; (float)i < (float)powerUpsCfgs.Count / (float)num4; i++)
		{
			UIGroupControl uIGroupControl = UIUtils.BuildUIGroupControl(0, new Rect(0f, 0f, 272f, 342f));
			int num5 = powerUpsCfgs.Count - i * num4;
			for (int j = 0; j < num4; j++)
			{
				int num6 = i * num4 + j;
				if (num6 >= powerUpsCfgs.Count)
				{
					continue;
				}
				FixedConfig.PowerUPSCfg powerUPSCfg = ConfigManager.Instance().GetFixedConfig().GetPowerUPSCfg(num6);
				float num7 = 0f + (float)(j / 2) * num2;
				float num8 = num3 - (float)(j % 2) * num3;
				uIImage = UIUtils.BuildImage(0, new Rect(num7, num8 + 10f, 265f, 153f), m_MatShopUI, new Rect(0f, 854f, 265f, 153f), new Vector2(265f, 153f));
				uIGroupControl.Add(uIImage);
				float num9 = num7 + 76f;
				float num10 = num8 + 87f;
				Rect powerUpsIconTexture = GetPowerUpsIconTexture(powerUPSCfg.type);
				uIImage = UIUtils.BuildImage(0, new Rect(num9 - powerUpsIconTexture.width / 2f, num10 - powerUpsIconTexture.height / 2f, powerUpsIconTexture.width, powerUpsIconTexture.height), m_MatPowerUPSIcons, powerUpsIconTexture, new Vector2(powerUpsIconTexture.width, powerUpsIconTexture.height));
				uIGroupControl.Add(uIImage);
				uIClickButton = UIUtils.BuildClickButton(2043 + num6, new Rect(num7 + 35f, num8 + 30f, 185f, 111f), m_MatTransparentUI, new Rect(1f, 1f, 1f, 1f), new Rect(1f, 1f, 1f, 1f), new Rect(1f, 1f, 1f, 1f), new Vector2(185f, 111f));
				uIGroupControl.Add(uIClickButton);
				uIText = UIUtils.BuildUIText(0, new Rect(num7 + 22f, num8 + 140f, 200f, 20f), UIText.enAlignStyle.left);
				uIText.Set("Zombie3D/Font/037-CAI978-15", powerUPSCfg.name, Constant.TextCommonColor);
				uIGroupControl.Add(uIText);
				if (powerUPSCfg.priceType == "gold")
				{
					uIImage = UIUtils.BuildImage(0, new Rect(num7 + 150f, num8 + 12f, 35f, 21f), m_MatShopUI, new Rect(586f, 85f, 35f, 21f), new Vector2(35f, 21f));
					uIGroupControl.Add(uIImage);
				}
				else if (powerUPSCfg.priceType == "dollor")
				{
					uIImage = UIUtils.BuildImage(0, new Rect(num7 + 150f, num8 + 7f, 35f, 29f), m_MatShopUI, new Rect(586f, 56f, 35f, 29f), new Vector2(35f, 29f));
					uIGroupControl.Add(uIImage);
				}
				uIText = UIUtils.BuildUIText(0, new Rect(num7 + 184f, num8 + 13f, 200f, 20f), UIText.enAlignStyle.left);
				uIText.Set("Zombie3D/Font/037-CAI978-15", powerUPSCfg.price.ToString(), Constant.TextCommonColor);
				uIGroupControl.Add(uIText);
				Hashtable powerUPS = GameApp.GetInstance().GetGameState().GetPowerUPS();
				if (powerUPS.ContainsKey((int)powerUPSCfg.type) && (int)powerUPS[(int)powerUPSCfg.type] > 0)
				{
					uIText = UIUtils.BuildUIText(0, new Rect(num7 + 15f, num8 + 13f, 200f, 20f), UIText.enAlignStyle.left);
					uIText.Set("Zombie3D/Font/037-CAI978-15", "OWN " + (int)powerUPS[(int)powerUPSCfg.type], Constant.TextCommonColor);
					uIGroupControl.Add(uIText);
				}
				float num11 = 140f;
				float num12 = 110f;
				float num13 = 200f;
				float num14 = num8 + num12;
				if ((float)powerUPSCfg.stamina > 0f)
				{
					num14 -= 20f;
					uIText = UIUtils.BuildUIText(0, new Rect(num7 + num13 - 200f, num14, 200f, 20f), UIText.enAlignStyle.right);
					uIText.Set("Zombie3D/Font/037-CAI978-13", "STA ", m_ShopPropColor);
					uIGroupControl.Add(uIText);
					string text = powerUPSCfg.stamina.ToString();
					if (powerUPSCfg.stamina >= 2000)
					{
						text = "Max";
					}
					uIText = UIUtils.BuildUIText(0, new Rect(num7 + num13, num14, 200f, 20f), UIText.enAlignStyle.left);
					uIText.Set("Zombie3D/Font/037-CAI978-13", string.Empty + text, m_ShopPropValueColor);
					uIGroupControl.Add(uIText);
				}
				if (powerUPSCfg.staminaSpeedAdd > 0f)
				{
					num14 -= 20f;
					uIText = UIUtils.BuildUIText(0, new Rect(num7 + num13 - 200f, num14, 200f, 20f), UIText.enAlignStyle.right);
					uIText.Set("Zombie3D/Font/037-CAI978-13", "STA ", m_ShopPropColor);
					uIGroupControl.Add(uIText);
					uIText = UIUtils.BuildUIText(0, new Rect(num7 + num13, num14, 200f, 20f), UIText.enAlignStyle.left);
					uIText.Set("Zombie3D/Font/037-CAI978-13", "+" + powerUPSCfg.staminaSpeedAdd + "/s", m_ShopPropValueColor);
					uIGroupControl.Add(uIText);
				}
				if (powerUPSCfg.hp > 0f)
				{
					num14 -= 20f;
					uIText = UIUtils.BuildUIText(0, new Rect(num7 + num13 - 200f, num14, 200f, 20f), UIText.enAlignStyle.right);
					uIText.Set("Zombie3D/Font/037-CAI978-13", "HP ", Constant.TextCommonColor);
					uIGroupControl.Add(uIText);
					uIText = UIUtils.BuildUIText(0, new Rect(num7 + num13, num14, 200f, 20f), UIText.enAlignStyle.left);
					uIText.Set("Zombie3D/Font/037-CAI978-13", string.Empty + powerUPSCfg.hp * 100f + "%", m_ShopPropValueColor);
					uIGroupControl.Add(uIText);
				}
				if (powerUPSCfg.damagePercent > 0f)
				{
					num14 -= 20f;
					uIText = UIUtils.BuildUIText(0, new Rect(num7 + num13 - 200f, num14, 200f, 20f), UIText.enAlignStyle.right);
					uIText.Set("Zombie3D/Font/037-CAI978-13", "DMG ", m_ShopPropColor);
					uIGroupControl.Add(uIText);
					uIText = UIUtils.BuildUIText(0, new Rect(num7 + num13, num14, 200f, 20f), UIText.enAlignStyle.left);
					uIText.Set("Zombie3D/Font/037-CAI978-13", string.Empty + powerUPSCfg.damagePercent * 100f + "%", m_ShopPropValueColor);
					uIGroupControl.Add(uIText);
				}
				if (powerUPSCfg.damage > 0f)
				{
					num14 -= 20f;
					uIText = UIUtils.BuildUIText(0, new Rect(num7 + num13 - 200f, num14, 200f, 20f), UIText.enAlignStyle.right);
					uIText.Set("Zombie3D/Font/037-CAI978-13", "DMG ", m_ShopPropColor);
					uIGroupControl.Add(uIText);
					uIText = UIUtils.BuildUIText(0, new Rect(num7 + num13, num14, 200f, 20f), UIText.enAlignStyle.left);
					uIText.Set("Zombie3D/Font/037-CAI978-13", string.Empty + powerUPSCfg.damage, m_ShopPropValueColor);
					uIGroupControl.Add(uIText);
				}
				if (powerUPSCfg.keepTime != 0f)
				{
					num14 -= 20f;
					uIText = UIUtils.BuildUIText(0, new Rect(num7 + num13 - 200f, num14, 200f, 20f), UIText.enAlignStyle.right);
					uIText.Set("Zombie3D/Font/037-CAI978-13", "TIME ", m_ShopPropColor);
					uIGroupControl.Add(uIText);
					string text2 = powerUPSCfg.keepTime + "S";
					if (powerUPSCfg.keepTime < 0f)
					{
						text2 = "1 STAGE";
					}
					uIText = UIUtils.BuildUIText(0, new Rect(num7 + num13, num14, 200f, 20f), UIText.enAlignStyle.left);
					uIText.Set("Zombie3D/Font/037-CAI978-13", text2, m_ShopPropValueColor);
					uIGroupControl.Add(uIText);
				}
				if (powerUPSCfg.type == ItemType.Shield)
				{
					num14 -= 20f;
					uIText = UIUtils.BuildUIText(0, new Rect(num7 + num13 - 200f, num14, 200f, 20f), UIText.enAlignStyle.right);
					uIText.Set("Zombie3D/Font/037-CAI978-13", "BLOCK ", m_ShopPropColor);
					uIGroupControl.Add(uIText);
					uIText = UIUtils.BuildUIText(0, new Rect(num7 + num13, num14, 200f, 20f), UIText.enAlignStyle.left);
					uIText.Set("Zombie3D/Font/037-CAI978-13", "150", m_ShopPropValueColor);
					uIGroupControl.Add(uIText);
				}
				if (powerUPSCfg.type == ItemType.Pacemaker)
				{
					num14 -= 20f;
					uIText = UIUtils.BuildUIText(0, new Rect(num7 + num13 - 200f, num14, 200f, 20f), UIText.enAlignStyle.right);
					uIText.Set("Zombie3D/Font/037-CAI978-13", "DEF ", m_ShopPropColor);
					uIGroupControl.Add(uIText);
					uIText = UIUtils.BuildUIText(0, new Rect(num7 + num13, num14, 200f, 20f), UIText.enAlignStyle.left);
					uIText.Set("Zombie3D/Font/037-CAI978-13", "100%", m_ShopPropValueColor);
					uIGroupControl.Add(uIText);
				}
				if (powerUPSCfg.type != ItemType.Defibrilator)
				{
				}
			}
			m_PowerUPSPageView.Add(uIGroupControl);
		}
		if (num > 0)
		{
			m_PowerUPSPageView.PageIndex = num;
		}
		if (m_ShopPageViewScrollBar != null)
		{
			m_uiGroup.Remove(m_ShopPageViewScrollBar);
		}
		m_ShopPageViewScrollBar = new UIDotScrollBar();
		m_ShopPageViewScrollBar.Rect = AutoUI.AutoRect(new Rect(530f, 50f, 100f, 20f));
		m_ShopPageViewScrollBar.ScrollOri = UIDotScrollBar.ScrollOrientation.Horizontal;
		m_ShopPageViewScrollBar.DotPageWidth = AutoUI.AutoDistance(30f);
		m_ShopPageViewScrollBar.SetPageCount(m_PowerUPSPageView.PageCount);
		m_ShopPageViewScrollBar.SetScrollBarTexture(m_MatShopUI, AutoUI.AutoRect(new Rect(597f, 107f, 11f, 11f)), m_MatShopUI, AutoUI.AutoRect(new Rect(609f, 107f, 11f, 11f)));
		m_ShopPageViewScrollBar.SetScrollPercent(0f);
		m_uiGroup.Add(m_ShopPageViewScrollBar);
		m_PowerUPSPageView.ScrollBar = m_ShopPageViewScrollBar;
		uIImage = UIUtils.BuildImage(0, new Rect(330f, 60f, 27f, 384f), m_MatShopUI, new Rect(0f, 106f, 27f, 384f), new Vector2(27f, 384f));
		m_uiGroup.Add(uIImage);
		uIImage = UIUtils.BuildImage(0, new Rect(882f, 60f, 45f, 388f), m_MatShopUI, new Rect(33f, 106f, 45f, 384f), new Vector2(45f, 384f));
		m_uiGroup.Add(uIImage);
	}

	public void SetupBankShopUI(bool bShow)
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
			UIMoveOuter control = UIUtils.BuildUIMoveOuter(2017, new Rect(0f, 0f, 393f, 575f), 10f, 10f);
			m_uiGroup.Add(control);
			SetupBankPageView();
		}
	}

	public void SetupBankPageView()
	{
		if (m_MatBankIconUI == null)
		{
			m_MatBankIconUI = LoadUIMaterial("Zombie3D/UI/Materials/BankIcons");
		}
		UIImage uIImage = null;
		UIClickButton uIClickButton = null;
		UIText uIText = null;
		int num = 0;
		if (m_BankPageView != null)
		{
			num = m_BankPageView.PageIndex;
			m_uiGroup.Remove(m_BankPageView);
			m_BankPageView = null;
		}
		m_BankPageView = new UIScrollPageView();
		m_BankPageView.SetMoveParam(AutoUI.AutoRect(new Rect(352f, 80f, 544f, 342f)), AutoUI.AutoDistance(20f), AutoUI.AutoDistance(20f));
		m_BankPageView.Rect = AutoUI.AutoRect(new Rect(352f, 80f, 544f, 342f));
		m_BankPageView.ListOri = UIScrollPageView.ListOrientation.Horizontal;
		m_BankPageView.ViewSize = AutoUI.AutoSize(new Vector2(272f, 171f));
		m_BankPageView.ItemSpacingV = AutoUI.AutoDistance(0f);
		m_BankPageView.ItemSpacingH = AutoUI.AutoDistance(0f);
		m_BankPageView.SetClip(AutoUI.AutoRect(new Rect(350f, 80f, 550f, 342f)));
		m_BankPageView.Bounds = AutoUI.AutoRect(new Rect(352f, 80f, 544f, 342f));
		m_uiGroup.Add(m_BankPageView);
		ArrayList iapCfgs = ConfigManager.Instance().GetFixedConfig().iapCfgs;
		float num2 = 272f;
		float num3 = 171f;
		int num4 = 2;
		for (int i = 0; (float)i < (float)iapCfgs.Count / (float)num4; i++)
		{
			UIGroupControl uIGroupControl = UIUtils.BuildUIGroupControl(0, new Rect(0f, 0f, 272f, 342f));
			for (int j = 0; j < num4; j++)
			{
				int num5 = i * num4 + j;
				if (num5 == 0)
				{
					FixedConfig.IAPCfg iAPCfg = ConfigManager.Instance().GetFixedConfig().GetIAPCfg(num5);
					float num6 = 0f + (float)(j / 2) * num2;
					float num7 = num3 - (float)(j % 2) * num3;
					Rect rcMat = new Rect(448f, 168f, 110f, 110f);
					uIImage = UIUtils.BuildImage(0, new Rect(num6, num7 + 10f, 265f, 153f), m_MatShopUI, new Rect(0f, 854f, 265f, 153f), new Vector2(265f, 153f));
					uIGroupControl.Add(uIImage);
					Rect rect = new Rect(448f, 168f, 110f, 110f);
					float num8 = num6 + 76f;
					float num9 = num7 + 87f;
					uIImage = UIUtils.BuildImage(0, new Rect(num8 - rect.width / 2f + 40f, num9 - rect.height / 2f, rect.width, rect.height), m_MatShopUI, rcMat, new Vector2(rect.width, rect.height));
					uIGroupControl.Add(uIImage);
					uIClickButton = UIUtils.BuildClickButton(2043 + num5, new Rect(num6 + 35f, num7 + 30f, 185f, 111f), m_MatTransparentUI, new Rect(1f, 1f, 1f, 1f), new Rect(1f, 1f, 1f, 1f), new Rect(1f, 1f, 1f, 1f), new Vector2(185f, 111f));
					uIGroupControl.Add(uIClickButton);
					uIImage = UIUtils.BuildImage(0, new Rect(num6 + 22f, num7 + 133f, 125f, 20f), m_MatShopUI, new Rect(254f, 197f, 125f, 20f), new Vector2(125f, 20f));
					uIGroupControl.Add(uIImage);
					if (gameState.NoviceGiftBagThird >= gameState.m_iNovicegiftAllowBuyTimesThird)
					{
						uIImage = UIUtils.BuildImage(0, new Rect(num6 + 32f, num7 + 30f, 154f, 122f), m_MatShopUI, new Rect(436f, 298f, 154f, 122f), new Vector2(154f, 122f));
						uIGroupControl.Add(uIImage);
					}
				}
				else if (num5 < iapCfgs.Count)
				{
					FixedConfig.IAPCfg iAPCfg2 = ConfigManager.Instance().GetFixedConfig().GetIAPCfg(num5);
					float num10 = 0f + (float)(j / 2) * num2;
					float num11 = num3 - (float)(j % 2) * num3;
					uIImage = UIUtils.BuildImage(0, new Rect(num10, num11 + 10f, 265f, 153f), m_MatShopUI, new Rect(0f, 854f, 265f, 153f), new Vector2(265f, 153f));
					uIGroupControl.Add(uIImage);
					Rect bankIconTexture = GetBankIconTexture(iAPCfg2);
					float num12 = num10 + 76f;
					float num13 = num11 + 87f;
					uIImage = UIUtils.BuildImage(0, new Rect(num12 - bankIconTexture.width / 2f, num13 - bankIconTexture.height / 2f, bankIconTexture.width, bankIconTexture.height), m_MatBankIconUI, bankIconTexture, new Vector2(bankIconTexture.width, bankIconTexture.height));
					uIGroupControl.Add(uIImage);
					Rect bankDollorTexture = GetBankDollorTexture(iAPCfg2);
					float num14 = num10 + 255f;
					float top = num11 + 20f;
					uIImage = UIUtils.BuildImage(0, new Rect(num14 - bankDollorTexture.width, top, bankDollorTexture.width, bankDollorTexture.height), m_MatBankIconUI, bankDollorTexture, new Vector2(bankDollorTexture.width, bankDollorTexture.height));
					uIImage.CatchMessage = false;
					uIGroupControl.Add(uIImage);
					uIClickButton = UIUtils.BuildClickButton(2043 + num5, new Rect(num10 + 35f, num11 + 30f, 185f, 111f), m_MatTransparentUI, new Rect(1f, 1f, 1f, 1f), new Rect(1f, 1f, 1f, 1f), new Rect(1f, 1f, 1f, 1f), new Vector2(185f, 111f));
					uIGroupControl.Add(uIClickButton);
					string text = string.Empty;
					if (iAPCfg2.gameGold > 0f)
					{
						text = "CASH";
					}
					else if (iAPCfg2.gameDollor > 0f)
					{
						text = "CRYSTAL";
					}
					uIText = UIUtils.BuildUIText(0, new Rect(num10 + 22f, num11 + 140f, 200f, 20f), UIText.enAlignStyle.left);
					uIText.Set("Zombie3D/Font/037-CAI978-15", text, Constant.TextCommonColor);
					uIGroupControl.Add(uIText);
					if (m_lsIAPSalesIndex.Contains(num5))
					{
						uIText = UIUtils.BuildUIText(0, new Rect(num10 + 150f, num11 + 140f, 200f, 20f), UIText.enAlignStyle.left);
						uIText.Set("Zombie3D/Font/037-CAI978-22", "SALE", Constant.RedGroupColor);
						uIGroupControl.Add(uIText);
					}
				}
			}
			m_BankPageView.Add(uIGroupControl);
		}
		if (m_ShopPageViewScrollBar != null)
		{
			m_uiGroup.Remove(m_ShopPageViewScrollBar);
			m_ShopPageViewScrollBar = null;
		}
		m_ShopPageViewScrollBar = new UIDotScrollBar();
		m_ShopPageViewScrollBar.Rect = AutoUI.AutoRect(new Rect(530f, 50f, 100f, 20f));
		m_ShopPageViewScrollBar.ScrollOri = UIDotScrollBar.ScrollOrientation.Horizontal;
		m_ShopPageViewScrollBar.DotPageWidth = AutoUI.AutoDistance(30f);
		m_ShopPageViewScrollBar.SetPageCount(m_BankPageView.PageCount);
		m_ShopPageViewScrollBar.SetScrollBarTexture(m_MatShopUI, AutoUI.AutoRect(new Rect(597f, 107f, 11f, 11f)), m_MatShopUI, AutoUI.AutoRect(new Rect(609f, 107f, 11f, 11f)));
		m_ShopPageViewScrollBar.SetScrollPercent(0f);
		m_uiGroup.Add(m_ShopPageViewScrollBar);
		m_BankPageView.ScrollBar = m_ShopPageViewScrollBar;
		uIImage = UIUtils.BuildImage(0, new Rect(330f, 60f, 27f, 384f), m_MatShopUI, new Rect(0f, 106f, 27f, 384f), new Vector2(27f, 384f));
		m_uiGroup.Add(uIImage);
		uIImage = UIUtils.BuildImage(0, new Rect(882f, 60f, 45f, 388f), m_MatShopUI, new Rect(33f, 106f, 45f, 384f), new Vector2(45f, 384f));
		m_uiGroup.Add(uIImage);
	}

	public void SetupBankSocialityPage(bool bShow)
	{
		if (m_ShareMsgGroup != null)
		{
			m_ShareMsgGroup.Clear();
			m_ShareMsgGroup = null;
		}
		if (!bShow)
		{
			if (playerShow.gameObject.transform.parent != null)
			{
				Camera camera = playerShow.gameObject.transform.parent.GetComponentInChildren(typeof(Camera)) as Camera;
				camera.enabled = true;
			}
			else
			{
				Debug.LogError("null playerShow parent");
			}
			return;
		}
		if (playerShow.gameObject.transform.parent != null)
		{
			Camera camera2 = playerShow.gameObject.transform.parent.GetComponentInChildren(typeof(Camera)) as Camera;
			camera2.enabled = false;
		}
		else
		{
			Debug.LogError("null playerShow parent");
		}
		m_ShareMsgGroup = new uiGroup(m_UIManager);
		Material mat = LoadUIMaterial("Zombie3D/UI/Materials/ShopUI01");
		Resources.UnloadUnusedAssets();
		Material mat2 = LoadUIMaterial("Zombie3D/UI/Materials/ChoosePointsUI");
		Resources.UnloadUnusedAssets();
		UIImage uIImage = null;
		UIClickButton uIClickButton = null;
		UIText uIText = null;
		uIImage = UIUtils.BuildImage(0, new Rect(0f, 0f, 960f, 640f), m_MatShopUI, new Rect(649f, 14f, 1f, 1f), new Vector2(1f, 1f));
		m_ShareMsgGroup.Add(uIImage);
		Rect scrRect = new Rect(55f, 64f, 849f, 512f);
		uIImage = UIUtils.BuildImage(0, scrRect, mat, new Rect(0f, 0f, scrRect.width, scrRect.height), new Vector2(scrRect.width, scrRect.height));
		m_ShareMsgGroup.Add(uIImage);
		uIText = UIUtils.BuildUIText(0, new Rect(scrRect.x + 365f, scrRect.y + 465f, 200f, 29f), UIText.enAlignStyle.left);
		uIText.Set("Zombie3D/Font/037-CAI978-27", "NOTICE", Constant.TextCommonColor);
		m_ShareMsgGroup.Add(uIText);
		uIText = UIUtils.BuildUIText(0, new Rect(scrRect.x + 220f, scrRect.y + 390f, 500f, 20f), UIText.enAlignStyle.left);
		uIText.Set("Zombie3D/Font/037-CAI978-15", "Follow Triniti on Twitter for 5 tCrystals!", Constant.TextCommonColor);
		m_ShareMsgGroup.Add(uIText);
		uIImage = UIUtils.BuildImage(0, new Rect(scrRect.x + 363f, scrRect.y + 194f, 168f, 154f), mat, new Rect(856f, 154f, 168f, 154f), new Vector2(168f, 154f));
		m_ShareMsgGroup.Add(uIImage);
		uIImage = UIUtils.BuildImage(0, new Rect(scrRect.x + 380f + 25f, scrRect.y + 145f, 35f, 29f), m_MatShopUI, new Rect(586f, 56f, 35f, 29f), new Vector2(35f, 29f));
		m_ShareMsgGroup.Add(uIImage);
		uIText = UIUtils.BuildUIText(0, new Rect(scrRect.x + 380f + 50f + 25f, scrRect.y + 142f, 200f, 29f), UIText.enAlignStyle.left);
		uIText.Set("Zombie3D/Font/037-CAI978-15", "X 5", Constant.TextCommonColor);
		m_ShareMsgGroup.Add(uIText);
		uIClickButton = UIUtils.BuildClickButton(2149, new Rect(scrRect.x + 345f, scrRect.y + 85f, 197f, 50f), mat, new Rect(0f, 564f, 197f, 50f), new Rect(0f, 514f, 197f, 50f), new Rect(0f, 614f, 197f, 50f), new Vector2(197f, 50f));
		if (MiscPlugin.IsOS5Up() && gameState.ShareWithTwitter)
		{
			uIClickButton.Enable = true;
		}
		else
		{
			uIClickButton.Enable = false;
		}
		m_ShareMsgGroup.Add(uIClickButton);
		uIClickButton = UIUtils.BuildClickButton(2150, new Rect(scrRect.x + 350f, scrRect.y - 30f, 187f, 68f), mat2, new Rect(647f, 198f, 187f, 68f), new Rect(835f, 198f, 187f, 68f), new Rect(647f, 198f, 187f, 68f), new Vector2(187f, 68f));
		m_ShareMsgGroup.Add(uIClickButton);
	}

	public void SetupShopHighlightBarUI(ShopType type)
	{
		switch (type)
		{
		}
	}

	public void SetupBuyWeaponDetail(bool bShow)
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
		if (m_MatWeaponIcons == null)
		{
			m_MatWeaponIcons = LoadUIMaterial("Zombie3D/UI/Materials/WeaponIcons");
		}
		UIImage control = UIUtils.BuildImage(0, new Rect(347f, 78f, 548f, 352f), m_MatShopUI, new Rect(598f, 120f, 1f, 1f), new Vector2(548f, 352f));
		m_uiShopItemDetail.Add(control);
		float num = 390f;
		float num2 = 92f;
		control = UIUtils.BuildImage(0, new Rect(num, num2, 483f, 357f), m_MatShopUI, new Rect(0f, 494f, 483f, 357f), new Vector2(483f, 357f));
		m_uiShopItemDetail.Add(control);
		UIClickButton uIClickButton = null;
		ArrayList weapons = ConfigManager.Instance().GetFixedConfig().weapons;
		for (int i = 0; i < weapons.Count; i++)
		{
			FixedConfig.WeaponCfg weaponCfg = (FixedConfig.WeaponCfg)weapons[i];
			if (weaponWantToBuy != (WeaponType)weaponCfg.type)
			{
				continue;
			}
			PropsAdditionImpl propsAdditionImpl = gameState.CheckAgeing(gameState.SetPropsAdditionsID(weaponCfg.type, 2), enPropsAdditionType.E_Damage, enPropsAdditionPart.E_Weapon);
			UIText uIText = UIUtils.BuildUIText(0, new Rect(num + 110f, num2 + 310f, 250f, 25f), UIText.enAlignStyle.center);
			uIText.Set("Zombie3D/Font/037-CAI978-18", weaponCfg.name, Constant.TextCommonColor);
			m_uiShopItemDetail.Add(uIText);
			Rect weaponIconTexture = GetWeaponIconTexture((WeaponType)weaponCfg.type);
			float num3 = num + 170f;
			float num4 = num2 + 200f;
			control = UIUtils.BuildImage(0, new Rect(num3 - weaponIconTexture.width / 2f, num4 - weaponIconTexture.height / 2f, weaponIconTexture.width, weaponIconTexture.height), m_MatWeaponIcons, weaponIconTexture, new Vector2(weaponIconTexture.width, weaponIconTexture.height));
			m_uiShopItemDetail.Add(control);
			float num5 = 180f;
			float num6 = 220f;
			float num7 = num5 + 200f;
			uIText = UIUtils.BuildUIText(0, new Rect(num + num5, num2 + num6, 200f, 20f), UIText.enAlignStyle.right);
			uIText.Set("Zombie3D/Font/037-CAI978-15", "TIER ", m_ShopPropColor);
			m_uiShopItemDetail.Add(uIText);
			uIText = UIUtils.BuildUIText(0, new Rect(num + num7, num2 + num6, 200f, 20f), UIText.enAlignStyle.left);
			uIText.Set("Zombie3D/Font/037-CAI978-13", weaponCfg.mClass.ToString(), m_ShopPropValueColor);
			m_uiShopItemDetail.Add(uIText);
			uIText = UIUtils.BuildUIText(0, new Rect(num + num5, num2 + num6 - 20f, 200f, 20f), UIText.enAlignStyle.right);
			uIText.Set("Zombie3D/Font/037-CAI978-15", "DMG ", m_ShopPropColor);
			m_uiShopItemDetail.Add(uIText);
			string text = weaponCfg.dmg.ToString();
			Color color = m_ShopPropValueColor;
			if (propsAdditionImpl != null)
			{
				text = ((int)(propsAdditionImpl.GetEffect(weaponCfg.dmg) * weaponCfg.rpm)).ToString();
				color = m_ShopPropsAdditionColor;
			}
			uIText = UIUtils.BuildUIText(0, new Rect(num + num7, num2 + num6 - 20f, 200f, 20f), UIText.enAlignStyle.left);
			uIText.Set("Zombie3D/Font/037-CAI978-13", text, color);
			m_uiShopItemDetail.Add(uIText);
			uIText = UIUtils.BuildUIText(0, new Rect(num + num5, num2 + num6 - 40f, 200f, 20f), UIText.enAlignStyle.right);
			uIText.Set("Zombie3D/Font/037-CAI978-15", "RPM ", m_ShopPropColor);
			m_uiShopItemDetail.Add(uIText);
			uIText = UIUtils.BuildUIText(0, new Rect(num + num7, num2 + num6 - 40f, 200f, 20f), UIText.enAlignStyle.left);
			uIText.Set("Zombie3D/Font/037-CAI978-13", Mathf.RoundToInt(60f / weaponCfg.rpm).ToString(), m_ShopPropValueColor);
			m_uiShopItemDetail.Add(uIText);
			uIText = UIUtils.BuildUIText(0, new Rect(num + num5, num2 + num6 - 60f, 200f, 20f), UIText.enAlignStyle.right);
			uIText.Set("Zombie3D/Font/037-CAI978-15", "SPD ", m_ShopPropColor);
			m_uiShopItemDetail.Add(uIText);
			uIText = UIUtils.BuildUIText(0, new Rect(num + num7, num2 + num6 - 60f, 200f, 20f), UIText.enAlignStyle.left);
			if (weaponCfg.spd > 0f)
			{
				uIText.Set("Zombie3D/Font/037-CAI978-13", weaponCfg.spd * 100f + "%", m_ShopPropSPDPositiveValueColor);
			}
			else
			{
				uIText.Set("Zombie3D/Font/037-CAI978-13", string.Empty + weaponCfg.spd * 100f + "%", m_ShopPropSPDNegativeValueColor);
			}
			m_uiShopItemDetail.Add(uIText);
			uIText = UIUtils.BuildUIText(0, new Rect(num + 43f, num2 + 40f, 400f, 70f), UIText.enAlignStyle.left);
			uIText.Set("Zombie3D/Font/037-CAI978-15", weaponCfg.introduction, Constant.TextCommonColor);
			m_uiShopItemDetail.Add(uIText);
			if (weaponCfg.priceType == "gold")
			{
				control = UIUtils.BuildImage(0, new Rect(num + 318f, num2 + 117f, 35f, 21f), m_MatShopUI, new Rect(586f, 85f, 35f, 21f), new Vector2(35f, 21f));
				control.CatchMessage = false;
				m_uiShopItemDetail.Add(control);
			}
			else if (weaponCfg.priceType == "dollor")
			{
				control = UIUtils.BuildImage(0, new Rect(num + 318f, num2 + 117f, 35f, 29f), m_MatShopUI, new Rect(586f, 56f, 35f, 29f), new Vector2(35f, 29f));
				control.CatchMessage = false;
				m_uiShopItemDetail.Add(control);
			}
			uIText = UIUtils.BuildUIText(0, new Rect(num + 360f, num2 + 120f, 200f, 20f), UIText.enAlignStyle.left);
			uIText.Set("Zombie3D/Font/037-CAI978-15", weaponCfg.price.ToString(), Constant.TextCommonColor);
			m_uiShopItemDetail.Add(uIText);
			uIClickButton = UIUtils.BuildClickButton(2021, new Rect(340f, 0f, 620f, 640f), m_MatTransparentUI, new Rect(1f, 1f, 1f, 1f), new Rect(1f, 1f, 1f, 1f), new Rect(1f, 1f, 1f, 1f), new Vector2(620f, 640f));
			m_uiShopItemDetail.Add(uIClickButton);
			Rect rcMat = new Rect(773f, 15f, 150f, 112f);
			Rect rcMat2 = new Rect(621f, 112f, 150f, 112f);
			Rect rcMat3 = new Rect(621f, 0f, 150f, 112f);
			List<WeaponType> battleWeapons = GameApp.GetInstance().GetGameState().GetBattleWeapons();
			List<WeaponType> weapons2 = GameApp.GetInstance().GetGameState().GetWeapons();
			bool flag = false;
			for (int j = 0; j < weapons2.Count; j++)
			{
				if (((FixedConfig.WeaponCfg)weapons[i]).type == (int)weapons2[j])
				{
					flag = true;
					if (!battleWeapons.Contains(weapons2[j]))
					{
						control = UIUtils.BuildImage(0, new Rect(num + 80f, num2 + 150f, rcMat2.width, rcMat2.height), m_MatShopUI, rcMat2, new Vector2(rcMat2.width, rcMat2.height));
						control.CatchMessage = false;
						m_uiShopItemDetail.Add(control);
						uIClickButton = UIUtils.BuildClickButton(2018, new Rect(num + 23f, num2 - 15f, 191f, 62f), m_MatShopUI, new Rect(833f, 136f, 191f, 62f), new Rect(833f, 198f, 191f, 62f), new Rect(833f, 136f, 191f, 62f), new Vector2(191f, 62f));
						m_uiShopItemDetail.Add(uIClickButton);
						uIClickButton = UIUtils.BuildClickButton(2020, new Rect(num + 250f, num2 - 15f, 191f, 62f), m_MatShopUI, new Rect(833f, 384f, 191f, 62f), new Rect(833f, 446f, 191f, 62f), new Rect(833f, 384f, 191f, 62f), new Vector2(191f, 62f));
						m_uiShopItemDetail.Add(uIClickButton);
						uIClickButton = UIUtils.BuildClickButton(2022, new Rect(num + 250f + 110f, num2 - 15f + 70f + 210f, 104f, 38f), m_MatShopUI, new Rect(725f, 345f, 104f, 38f), new Rect(725f, 384f, 104f, 38f), new Rect(725f, 345f, 104f, 38f), new Vector2(104f, 38f));
						m_uiShopItemDetail.Add(uIClickButton);
					}
					else
					{
						control = UIUtils.BuildImage(0, new Rect(num + 80f, num2 + 150f, rcMat3.width, rcMat3.height), m_MatShopUI, rcMat3, new Vector2(rcMat3.width, rcMat3.height));
						control.CatchMessage = false;
						m_uiShopItemDetail.Add(control);
					}
					break;
				}
			}
			if (!flag)
			{
				if (weaponCfg.bNewWeapon)
				{
					control = UIUtils.BuildImage(0, new Rect(num + 220f, num2 + 220f, 75f, 65f), m_MatShopUI, new Rect(944f, 54f, 75f, 65f), new Vector2(75f, 65f));
					m_uiShopItemDetail.Add(control);
				}
				if (weaponCfg.price <= 0)
				{
					control = UIUtils.BuildImage(0, new Rect(num + 50f, num2 + 150f, 151f, 112f), m_MatShopUI, new Rect(622f, 228f, 151f, 112f), new Vector2(151f, 112f));
					control.CatchMessage = false;
					m_uiShopItemDetail.Add(control);
				}
				if (GameApp.GetInstance().GetGameState().GetPlayerLevel() >= weaponCfg.levelLimit && weaponCfg.type != 28 || weaponCfg.type == 28 && battleWeapons.Contains(WeaponType.Ion_Cannon) && battleWeapons.Contains(WeaponType.Ion_CannonI))
				{
					uIClickButton = UIUtils.BuildClickButton(2018, new Rect(num + 23f, num2 - 15f, 191f, 62f), m_MatShopUI, new Rect(833f, 136f, 191f, 62f), new Rect(833f, 198f, 191f, 62f), new Rect(833f, 136f, 191f, 62f), new Vector2(191f, 62f));
					m_uiShopItemDetail.Add(uIClickButton);
					uIClickButton = UIUtils.BuildClickButton(2019, new Rect(num + 250f, num2 - 15f, 191f, 62f), m_MatShopUI, new Rect(833f, 260f, 191f, 62f), new Rect(833f, 322f, 191f, 62f), new Rect(833f, 260f, 191f, 62f), new Vector2(191f, 62f));
					m_uiShopItemDetail.Add(uIClickButton);
					continue;
				}
				control = UIUtils.BuildImage(0, new Rect(num + 50f, num2 + 150f, rcMat.width, rcMat.height), m_MatShopUI, rcMat, new Vector2(rcMat.width, rcMat.height));
				control.CatchMessage = false;
				m_uiShopItemDetail.Add(control);
				uIText = UIUtils.BuildUIText(0, new Rect(num + 65f, num2 + 135f, 200f, 20f), UIText.enAlignStyle.left);
				if (weaponCfg.type != 28)
				{
					uIText.Set("Zombie3D/Font/037-CAI978-15", "LV " + weaponCfg.levelLimit, Constant.TextCommonColor);
				}
				else
				{
					uIText.Set("Zombie3D/Font/037-CAI978-15", "Unlock", Constant.TextCommonColor);
				}
				m_uiShopItemDetail.Add(uIText);
				uIClickButton = UIUtils.BuildClickButton(2018, new Rect(num + 145f, num2 - 15f, 191f, 62f), m_MatShopUI, new Rect(833f, 136f, 191f, 62f), new Rect(833f, 198f, 191f, 62f), new Rect(833f, 136f, 191f, 62f), new Vector2(191f, 62f));
				m_uiShopItemDetail.Add(uIClickButton);
			}
		}
	}

	public void SetupBuyAvatarDetail(bool bShow)
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
		if (m_MatAvatarIcons == null)
		{
			m_MatAvatarIcons = LoadUIMaterial("Zombie3D/UI/Materials/AvatarIcons");
		}
		UIImage control = UIUtils.BuildImage(0, new Rect(347f, 78f, 548f, 352f), m_MatShopUI, new Rect(598f, 120f, 1f, 1f), new Vector2(548f, 352f));
		m_uiShopItemDetail.Add(control);
		float num = 390f;
		float num2 = 92f;
		control = UIUtils.BuildImage(0, new Rect(num, num2, 483f, 357f), m_MatShopUI, new Rect(0f, 494f, 483f, 357f), new Vector2(483f, 357f));
		m_uiShopItemDetail.Add(control);
		UIClickButton uIClickButton = null;
		ArrayList avatarCfgs = ConfigManager.Instance().GetFixedConfig().avatarCfgs;
		FixedConfig.AvatarCfg avatarCfg = (FixedConfig.AvatarCfg)avatarCfgs[avatarSelectedIndex];
		UIText uIText = UIUtils.BuildUIText(0, new Rect(num + 110f, num2 + 310f, 250f, 25f), UIText.enAlignStyle.center);
		uIText.Set("Zombie3D/Font/037-CAI978-18", avatarCfg.name, Constant.TextCommonColor);
		m_uiShopItemDetail.Add(uIText);
		Rect avatarIconTexture = GetAvatarIconTexture(avatarCfg.suiteType, avatarCfg.avtType);
		float num3 = num + 170f;
		float num4 = num2 + 200f;
		control = UIUtils.BuildImage(0, new Rect(num3 - avatarIconTexture.width / 2f, num4 - avatarIconTexture.height / 2f, avatarIconTexture.width, avatarIconTexture.height), m_MatAvatarIcons, avatarIconTexture, new Vector2(avatarIconTexture.width, avatarIconTexture.height));
		m_uiShopItemDetail.Add(control);
		float num5 = 300f;
		float num6 = 220f;
		float num7 = num + num5;
		float num8 = num2 + num6;
		num7 -= 120f;
		float left = num7 + 200f;
		uIText = UIUtils.BuildUIText(0, new Rect(num7, num8, 200f, 20f), UIText.enAlignStyle.right);
		uIText.Set("Zombie3D/Font/037-CAI978-15", "TIER ", Constant.TextCommonColor);
		m_uiShopItemDetail.Add(uIText);
		uIText = UIUtils.BuildUIText(0, new Rect(left, num8, 200f, 20f), UIText.enAlignStyle.left);
		uIText.Set("Zombie3D/Font/037-CAI978-13", avatarCfg.m_Class + string.Empty, m_ShopPropValueColor);
		m_uiShopItemDetail.Add(uIText);
		int id = gameState.SetPropsAdditionsID((int)avatarCfg.suiteType, (int)avatarCfg.avtType);
		if (avatarCfg.prop.m_DefenceAdditive > 0f)
		{
			PropsAdditionImpl propsAdditionImpl = null;
			float num9 = avatarCfg.prop.m_DefenceAdditive;
			Color color = m_ShopPropValueColor;
			propsAdditionImpl = gameState.CheckAgeing(id, enPropsAdditionType.E_DefenceAdditive, (enPropsAdditionPart)avatarCfg.avtType);
			if (propsAdditionImpl != null)
			{
				num9 = propsAdditionImpl.GetEffect(num9);
				color = m_ShopPropsAdditionColor;
			}
			num8 -= 20f;
			uIText = UIUtils.BuildUIText(0, new Rect(num7, num8, 200f, 20f), UIText.enAlignStyle.right);
			uIText.Set("Zombie3D/Font/037-CAI978-15", "DEF ", Constant.TextCommonColor);
			m_uiShopItemDetail.Add(uIText);
			uIText = UIUtils.BuildUIText(0, new Rect(left, num8, 200f, 20f), UIText.enAlignStyle.left);
			uIText.Set("Zombie3D/Font/037-CAI978-13", num9 * 100f + "%", color);
			m_uiShopItemDetail.Add(uIText);
		}
		if (avatarCfg.prop.m_AttackAdditive > 0f)
		{
			PropsAdditionImpl propsAdditionImpl2 = null;
			float num10 = avatarCfg.prop.m_AttackAdditive;
			Color color2 = m_ShopPropValueColor;
			propsAdditionImpl2 = gameState.CheckAgeing(id, enPropsAdditionType.E_AttackAdditive, (enPropsAdditionPart)avatarCfg.avtType);
			if (propsAdditionImpl2 != null)
			{
				num10 = propsAdditionImpl2.GetEffect(num10);
				color2 = m_ShopPropsAdditionColor;
			}
			num8 -= 20f;
			uIText = UIUtils.BuildUIText(0, new Rect(num7, num8, 200f, 20f), UIText.enAlignStyle.right);
			uIText.Set("Zombie3D/Font/037-CAI978-15", "DMG ", Constant.TextCommonColor);
			m_uiShopItemDetail.Add(uIText);
			uIText = UIUtils.BuildUIText(0, new Rect(left, num8, 200f, 20f), UIText.enAlignStyle.left);
			uIText.Set("Zombie3D/Font/037-CAI978-13", num10 * 100f + "%", color2);
			m_uiShopItemDetail.Add(uIText);
		}
		if (avatarCfg.prop.m_SpeedAdditive != 0f)
		{
			PropsAdditionImpl propsAdditionImpl3 = null;
			float num11 = avatarCfg.prop.m_SpeedAdditive;
			Color color3 = m_ShopPropValueColor;
			propsAdditionImpl3 = gameState.CheckAgeing(id, enPropsAdditionType.E_SpeedAdditive, (enPropsAdditionPart)avatarCfg.avtType);
			if (propsAdditionImpl3 != null)
			{
				num11 = propsAdditionImpl3.GetEffect(num11);
				color3 = m_ShopPropsAdditionColor;
			}
			num8 -= 20f;
			uIText = UIUtils.BuildUIText(0, new Rect(num7, num8, 200f, 20f), UIText.enAlignStyle.right);
			uIText.Set("Zombie3D/Font/037-CAI978-15", "SPD ", Constant.TextCommonColor);
			m_uiShopItemDetail.Add(uIText);
			uIText = UIUtils.BuildUIText(0, new Rect(left, num8, 200f, 20f), UIText.enAlignStyle.left);
			if (avatarCfg.prop.m_SpeedAdditive > 0f)
			{
				uIText.Set("Zombie3D/Font/037-CAI978-13", num11 * 100f + "%", color3);
			}
			else
			{
				uIText.Set("Zombie3D/Font/037-CAI978-13", string.Empty + avatarCfg.prop.m_SpeedAdditive * 100f + "%", m_ShopPropSPDNegativeValueColor);
			}
			m_uiShopItemDetail.Add(uIText);
		}
		if (avatarCfg.prop.m_HpAdditive > 0f)
		{
			PropsAdditionImpl propsAdditionImpl4 = null;
			float num12 = avatarCfg.prop.m_HpAdditive;
			Color color4 = m_ShopPropValueColor;
			propsAdditionImpl4 = gameState.CheckAgeing(id, enPropsAdditionType.E_HpAdditive, (enPropsAdditionPart)avatarCfg.avtType);
			if (propsAdditionImpl4 != null)
			{
				num12 = propsAdditionImpl4.GetEffect(num12);
				color4 = m_ShopPropsAdditionColor;
			}
			num8 -= 20f;
			uIText = UIUtils.BuildUIText(0, new Rect(num7, num8, 200f, 20f), UIText.enAlignStyle.right);
			uIText.Set("Zombie3D/Font/037-CAI978-15", "HP ", Constant.TextCommonColor);
			m_uiShopItemDetail.Add(uIText);
			uIText = UIUtils.BuildUIText(0, new Rect(left, num8, 200f, 20f), UIText.enAlignStyle.left);
			uIText.Set("Zombie3D/Font/037-CAI978-13", num12 * 100f + "%", color4);
			m_uiShopItemDetail.Add(uIText);
		}
		if (avatarCfg.prop.m_AttackSpeedAdditive > 0f)
		{
			PropsAdditionImpl propsAdditionImpl5 = null;
			float num13 = avatarCfg.prop.m_AttackSpeedAdditive;
			Color color5 = m_ShopPropValueColor;
			propsAdditionImpl5 = gameState.CheckAgeing(id, enPropsAdditionType.E_AttackSpeedAdditive, (enPropsAdditionPart)avatarCfg.avtType);
			if (propsAdditionImpl5 != null)
			{
				num13 = propsAdditionImpl5.GetEffect(num13);
				color5 = m_ShopPropsAdditionColor;
			}
			num8 -= 20f;
			uIText = UIUtils.BuildUIText(0, new Rect(num7, num8, 200f, 20f), UIText.enAlignStyle.right);
			uIText.Set("Zombie3D/Font/037-CAI978-15", "ASPD ", Constant.TextCommonColor);
			m_uiShopItemDetail.Add(uIText);
			uIText = UIUtils.BuildUIText(0, new Rect(left, num8, 200f, 20f), UIText.enAlignStyle.left);
			uIText.Set("Zombie3D/Font/037-CAI978-13", num13 * 100f + "%", color5);
			m_uiShopItemDetail.Add(uIText);
		}
		if (avatarCfg.prop.m_StaminaAdd > 0f)
		{
			PropsAdditionImpl propsAdditionImpl6 = null;
			float num14 = avatarCfg.prop.m_StaminaAdd;
			Color color6 = m_ShopPropValueColor;
			propsAdditionImpl6 = gameState.CheckAgeing(id, enPropsAdditionType.E_StaminaAdditive, (enPropsAdditionPart)avatarCfg.avtType);
			if (propsAdditionImpl6 != null)
			{
				num14 = propsAdditionImpl6.GetEffect(num14);
				color6 = m_ShopPropsAdditionColor;
			}
			num8 -= 20f;
			uIText = UIUtils.BuildUIText(0, new Rect(num7, num8, 200f, 20f), UIText.enAlignStyle.right);
			uIText.Set("Zombie3D/Font/037-CAI978-15", "STA ", Constant.TextCommonColor);
			m_uiShopItemDetail.Add(uIText);
			uIText = UIUtils.BuildUIText(0, new Rect(left, num8, 200f, 20f), UIText.enAlignStyle.left);
			uIText.Set("Zombie3D/Font/037-CAI978-13", num14 * 100f + "%", color6);
			m_uiShopItemDetail.Add(uIText);
		}
		if (avatarCfg.prop.m_ExpAdditive > 0f)
		{
			PropsAdditionImpl propsAdditionImpl7 = null;
			float num15 = avatarCfg.prop.m_ExpAdditive;
			Color color7 = m_ShopPropValueColor;
			propsAdditionImpl7 = gameState.CheckAgeing(id, enPropsAdditionType.E_ExpAdditive, (enPropsAdditionPart)avatarCfg.avtType);
			if (propsAdditionImpl7 != null)
			{
				num15 = propsAdditionImpl7.GetEffect(num15);
				color7 = m_ShopPropsAdditionColor;
			}
			num8 -= 20f;
			uIText = UIUtils.BuildUIText(0, new Rect(num7, num8, 200f, 20f), UIText.enAlignStyle.right);
			uIText.Set("Zombie3D/Font/037-CAI978-15", "EXP ", Constant.TextCommonColor);
			m_uiShopItemDetail.Add(uIText);
			uIText = UIUtils.BuildUIText(0, new Rect(left, num8, 200f, 20f), UIText.enAlignStyle.left);
			uIText.Set("Zombie3D/Font/037-CAI978-13", num15 * 100f + "%", color7);
			m_uiShopItemDetail.Add(uIText);
		}
		if (avatarCfg.prop.m_GoldAdditive > 0f)
		{
			PropsAdditionImpl propsAdditionImpl8 = null;
			float num16 = avatarCfg.prop.m_GoldAdditive;
			Color color8 = m_ShopPropValueColor;
			propsAdditionImpl8 = gameState.CheckAgeing(id, enPropsAdditionType.E_CashAdditive, (enPropsAdditionPart)avatarCfg.avtType);
			if (propsAdditionImpl8 != null)
			{
				num16 = propsAdditionImpl8.GetEffect(num16);
				color8 = m_ShopPropsAdditionColor;
			}
			num8 -= 20f;
			uIText = UIUtils.BuildUIText(0, new Rect(num7, num8, 200f, 20f), UIText.enAlignStyle.right);
			uIText.Set("Zombie3D/Font/037-CAI978-15", "CASH ", Constant.TextCommonColor);
			m_uiShopItemDetail.Add(uIText);
			uIText = UIUtils.BuildUIText(0, new Rect(left, num8, 200f, 20f), UIText.enAlignStyle.left);
			uIText.Set("Zombie3D/Font/037-CAI978-13", num16 * 100f + "%", color8);
			m_uiShopItemDetail.Add(uIText);
		}
		uIText = UIUtils.BuildUIText(0, new Rect(num + 43f, num2 + 40f, 400f, 70f), UIText.enAlignStyle.left);
		uIText.Set("Zombie3D/Font/037-CAI978-15", avatarCfg.introduction, Constant.TextCommonColor);
		m_uiShopItemDetail.Add(uIText);
		if (avatarCfg.priceType == "gold")
		{
			control = UIUtils.BuildImage(0, new Rect(num + 318f, num2 + 117f, 35f, 21f), m_MatShopUI, new Rect(586f, 85f, 35f, 21f), new Vector2(35f, 21f));
			control.CatchMessage = false;
			m_uiShopItemDetail.Add(control);
		}
		else if (avatarCfg.priceType == "dollor")
		{
			control = UIUtils.BuildImage(0, new Rect(num + 318f, num2 + 117f, 35f, 29f), m_MatShopUI, new Rect(586f, 56f, 35f, 29f), new Vector2(35f, 29f));
			control.CatchMessage = false;
			m_uiShopItemDetail.Add(control);
		}
		uIText = UIUtils.BuildUIText(0, new Rect(num + 360f, num2 + 120f, 200f, 20f), UIText.enAlignStyle.left);
		uIText.Set("Zombie3D/Font/037-CAI978-15", avatarCfg.price.ToString(), Constant.TextCommonColor);
		m_uiShopItemDetail.Add(uIText);
		uIClickButton = UIUtils.BuildClickButton(2028, new Rect(340f, 0f, 620f, 640f), m_MatTransparentUI, new Rect(1f, 1f, 1f, 1f), new Rect(1f, 1f, 1f, 1f), new Rect(1f, 1f, 1f, 1f), new Vector2(620f, 640f));
		m_uiShopItemDetail.Add(uIClickButton);
		Rect rcMat = new Rect(773f, 15f, 150f, 112f);
		Rect rcMat2 = new Rect(621f, 112f, 150f, 112f);
		Rect rcMat3 = new Rect(621f, 0f, 150f, 112f);
		Hashtable avatars = GameApp.GetInstance().GetGameState().GetAvatars();
		bool flag = false;
		foreach (Avatar key in avatars.Keys)
		{
			if (key.SuiteType == avatarCfg.suiteType && key.AvtType == avatarCfg.avtType)
			{
				flag = true;
				if ((bool)avatars[key])
				{
					control = UIUtils.BuildImage(0, new Rect(num + 80f, num2 + 150f, rcMat3.width, rcMat3.height), m_MatShopUI, rcMat3, new Vector2(rcMat3.width, rcMat3.height));
					control.CatchMessage = false;
					m_uiShopItemDetail.Add(control);
					break;
				}
				control = UIUtils.BuildImage(0, new Rect(num + 80f, num2 + 150f, rcMat2.width, rcMat2.height), m_MatShopUI, rcMat2, new Vector2(rcMat2.width, rcMat2.height));
				control.CatchMessage = false;
				m_uiShopItemDetail.Add(control);
				uIClickButton = UIUtils.BuildClickButton(2025, new Rect(num + 23f, num2 - 15f, 191f, 62f), m_MatShopUI, new Rect(833f, 136f, 191f, 62f), new Rect(833f, 198f, 191f, 62f), new Rect(833f, 136f, 191f, 62f), new Vector2(191f, 62f));
				m_uiShopItemDetail.Add(uIClickButton);
				uIClickButton = UIUtils.BuildClickButton(2027, new Rect(num + 250f, num2 - 15f, 191f, 62f), m_MatShopUI, new Rect(833f, 384f, 191f, 62f), new Rect(833f, 446f, 191f, 62f), new Rect(833f, 384f, 191f, 62f), new Vector2(191f, 62f));
				m_uiShopItemDetail.Add(uIClickButton);
				uIClickButton = UIUtils.BuildClickButton(2029, new Rect(num + 250f + 110f, num2 - 15f + 70f + 210f, 104f, 38f), m_MatShopUI, new Rect(725f, 345f, 104f, 38f), new Rect(725f, 384f, 104f, 38f), new Rect(725f, 345f, 104f, 38f), new Vector2(104f, 38f));
				m_uiShopItemDetail.Add(uIClickButton);
				break;
			}
		}
		if (!flag)
		{
			if (avatarCfg.bNewAvatar)
			{
				control = UIUtils.BuildImage(0, new Rect(num + 220f, num2 + 220f, 75f, 65f), m_MatShopUI, new Rect(944f, 54f, 75f, 65f), new Vector2(75f, 65f));
				control.CatchMessage = false;
				m_uiShopItemDetail.Add(control);
			}
			if (avatarCfg.price <= 0)
			{
				control = UIUtils.BuildImage(0, new Rect(num + 50f, num2 + 150f, 151f, 112f), m_MatShopUI, new Rect(622f, 228f, 151f, 112f), new Vector2(151f, 112f));
				control.CatchMessage = false;
				m_uiShopItemDetail.Add(control);
			}
			if (GameApp.GetInstance().GetGameState().GetPlayerLevel() >= avatarCfg.levelLimit)
			{
				uIClickButton = UIUtils.BuildClickButton(2025, new Rect(num + 23f, num2 - 15f, 191f, 62f), m_MatShopUI, new Rect(833f, 136f, 191f, 62f), new Rect(833f, 198f, 191f, 62f), new Rect(833f, 136f, 191f, 62f), new Vector2(191f, 62f));
				m_uiShopItemDetail.Add(uIClickButton);
				uIClickButton = UIUtils.BuildClickButton(2026, new Rect(num + 250f, num2 - 15f, 191f, 62f), m_MatShopUI, new Rect(833f, 260f, 191f, 62f), new Rect(833f, 322f, 191f, 62f), new Rect(833f, 260f, 191f, 62f), new Vector2(191f, 62f));
				m_uiShopItemDetail.Add(uIClickButton);
				return;
			}
			control = UIUtils.BuildImage(0, new Rect(num + 50f, num2 + 150f, rcMat.width, rcMat.height), m_MatShopUI, rcMat, new Vector2(rcMat.width, rcMat.height));
			control.CatchMessage = false;
			m_uiShopItemDetail.Add(control);
			uIText = UIUtils.BuildUIText(0, new Rect(num + 65f, num2 + 135f, 200f, 20f), UIText.enAlignStyle.left);
			uIText.Set("Zombie3D/Font/037-CAI978-15", "LV " + avatarCfg.levelLimit, Constant.TextCommonColor);
			m_uiShopItemDetail.Add(uIText);
			uIClickButton = UIUtils.BuildClickButton(2025, new Rect(num + 145f, num2 - 15f, 191f, 62f), m_MatShopUI, new Rect(833f, 136f, 191f, 62f), new Rect(833f, 198f, 191f, 62f), new Rect(833f, 136f, 191f, 62f), new Vector2(191f, 62f));
			m_uiShopItemDetail.Add(uIClickButton);
		}
	}

	public void SetupBuyPowerUPSDetail(bool bShow)
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
		if (m_MatPowerUPSIcons == null)
		{
			m_MatPowerUPSIcons = LoadUIMaterial("Zombie3D/UI/Materials/PowerUPSIcons");
		}
		UIImage control = UIUtils.BuildImage(0, new Rect(347f, 78f, 548f, 352f), m_MatShopUI, new Rect(598f, 120f, 1f, 1f), new Vector2(548f, 352f));
		m_uiShopItemDetail.Add(control);
		float num = 390f;
		float num2 = 92f;
		control = UIUtils.BuildImage(0, new Rect(num, num2, 483f, 357f), m_MatShopUI, new Rect(0f, 494f, 483f, 357f), new Vector2(483f, 357f));
		m_uiShopItemDetail.Add(control);
		FixedConfig.PowerUPSCfg powerUPSCfg = ConfigManager.Instance().GetFixedConfig().GetPowerUPSCfg(powerUpsSelectedIndex);
		float num3 = num + 170f;
		float num4 = num2 + 200f;
		Rect powerUpsIconTexture = GetPowerUpsIconTexture(powerUPSCfg.type);
		control = UIUtils.BuildImage(0, new Rect(num3 - powerUpsIconTexture.width / 2f, num4 - powerUpsIconTexture.height / 2f, powerUpsIconTexture.width, powerUpsIconTexture.height), m_MatPowerUPSIcons, powerUpsIconTexture, new Vector2(powerUpsIconTexture.width, powerUpsIconTexture.height));
		m_uiShopItemDetail.Add(control);
		UIText uIText = UIUtils.BuildUIText(0, new Rect(num + 110f, num2 + 310f, 250f, 25f), UIText.enAlignStyle.center);
		uIText.Set("Zombie3D/Font/037-CAI978-18", powerUPSCfg.name, Constant.TextCommonColor);
		m_uiShopItemDetail.Add(uIText);
		if (powerUPSCfg.priceType == "gold")
		{
			control = UIUtils.BuildImage(0, new Rect(num + 318f, num2 + 117f, 35f, 21f), m_MatShopUI, new Rect(586f, 85f, 35f, 21f), new Vector2(35f, 21f));
			m_uiShopItemDetail.Add(control);
		}
		else if (powerUPSCfg.priceType == "dollor")
		{
			control = UIUtils.BuildImage(0, new Rect(num + 318f, num2 + 117f, 35f, 29f), m_MatShopUI, new Rect(586f, 56f, 35f, 29f), new Vector2(35f, 29f));
			m_uiShopItemDetail.Add(control);
		}
		uIText = UIUtils.BuildUIText(0, new Rect(num + 360f, num2 + 120f, 200f, 20f), UIText.enAlignStyle.left);
		uIText.Set("Zombie3D/Font/037-CAI978-15", powerUPSCfg.price.ToString(), Constant.TextCommonColor);
		m_uiShopItemDetail.Add(uIText);
		Hashtable powerUPS = GameApp.GetInstance().GetGameState().GetPowerUPS();
		if (powerUPS.ContainsKey((int)powerUPSCfg.type) && (int)powerUPS[(int)powerUPSCfg.type] > 0)
		{
			uIText = UIUtils.BuildUIText(0, new Rect(num + 70f, num2 + 135f, 200f, 20f), UIText.enAlignStyle.left);
			uIText.Set("Zombie3D/Font/037-CAI978-15", "OWN " + (int)powerUPS[(int)powerUPSCfg.type], Constant.TextCommonColor);
			m_uiShopItemDetail.Add(uIText);
		}
		float num5 = 300f;
		float num6 = 220f;
		float num7 = num + num5;
		float num8 = num2 + num6;
		num7 -= 130f;
		float left = num7 + 200f;
		if ((float)powerUPSCfg.stamina > 0f)
		{
			num8 -= 20f;
			uIText = UIUtils.BuildUIText(0, new Rect(num7, num8, 200f, 20f), UIText.enAlignStyle.right);
			uIText.Set("Zombie3D/Font/037-CAI978-15", "STA ", Constant.TextCommonColor);
			m_uiShopItemDetail.Add(uIText);
			string text = powerUPSCfg.stamina.ToString();
			if (powerUPSCfg.stamina >= 2000)
			{
				text = "Max";
			}
			uIText = UIUtils.BuildUIText(0, new Rect(left, num8, 200f, 20f), UIText.enAlignStyle.left);
			uIText.Set("Zombie3D/Font/037-CAI978-13", text, m_ShopPropValueColor);
			m_uiShopItemDetail.Add(uIText);
		}
		if (powerUPSCfg.staminaSpeedAdd > 0f)
		{
			num8 -= 20f;
			uIText = UIUtils.BuildUIText(0, new Rect(num7, num8, 200f, 20f), UIText.enAlignStyle.right);
			uIText.Set("Zombie3D/Font/037-CAI978-15", "STA ", Constant.TextCommonColor);
			m_uiShopItemDetail.Add(uIText);
			uIText = UIUtils.BuildUIText(0, new Rect(left, num8, 200f, 20f), UIText.enAlignStyle.left);
			uIText.Set("Zombie3D/Font/037-CAI978-13", "+" + powerUPSCfg.staminaSpeedAdd + "/s", m_ShopPropValueColor);
			m_uiShopItemDetail.Add(uIText);
		}
		if (powerUPSCfg.hp > 0f)
		{
			num8 -= 20f;
			uIText = UIUtils.BuildUIText(0, new Rect(num7, num8, 200f, 20f), UIText.enAlignStyle.right);
			uIText.Set("Zombie3D/Font/037-CAI978-15", "HP ", Constant.TextCommonColor);
			m_uiShopItemDetail.Add(uIText);
			uIText = UIUtils.BuildUIText(0, new Rect(left, num8, 200f, 20f), UIText.enAlignStyle.left);
			uIText.Set("Zombie3D/Font/037-CAI978-13", powerUPSCfg.hp * 100f + "%", m_ShopPropValueColor);
			m_uiShopItemDetail.Add(uIText);
		}
		if (powerUPSCfg.damagePercent > 0f)
		{
			num8 -= 20f;
			uIText = UIUtils.BuildUIText(0, new Rect(num7, num8, 200f, 20f), UIText.enAlignStyle.right);
			uIText.Set("Zombie3D/Font/037-CAI978-15", "DMG ", Constant.TextCommonColor);
			m_uiShopItemDetail.Add(uIText);
			uIText = UIUtils.BuildUIText(0, new Rect(left, num8, 200f, 20f), UIText.enAlignStyle.left);
			uIText.Set("Zombie3D/Font/037-CAI978-13", powerUPSCfg.damagePercent * 100f + "%", m_ShopPropValueColor);
			m_uiShopItemDetail.Add(uIText);
		}
		if (powerUPSCfg.damage > 0f)
		{
			num8 -= 20f;
			uIText = UIUtils.BuildUIText(0, new Rect(num7, num8, 200f, 20f), UIText.enAlignStyle.right);
			uIText.Set("Zombie3D/Font/037-CAI978-15", "DMG ", Constant.TextCommonColor);
			m_uiShopItemDetail.Add(uIText);
			uIText = UIUtils.BuildUIText(0, new Rect(left, num8, 200f, 20f), UIText.enAlignStyle.left);
			uIText.Set("Zombie3D/Font/037-CAI978-13", powerUPSCfg.damage + string.Empty, m_ShopPropValueColor);
			m_uiShopItemDetail.Add(uIText);
		}
		if (powerUPSCfg.keepTime != 0f)
		{
			num8 -= 20f;
			string text2 = powerUPSCfg.keepTime + "S";
			if (powerUPSCfg.keepTime < 0f)
			{
				text2 = "1 STAGE";
			}
			uIText = UIUtils.BuildUIText(0, new Rect(num7, num8, 200f, 20f), UIText.enAlignStyle.right);
			uIText.Set("Zombie3D/Font/037-CAI978-15", "TIME ", Constant.TextCommonColor);
			m_uiShopItemDetail.Add(uIText);
			uIText = UIUtils.BuildUIText(0, new Rect(left, num8, 200f, 20f), UIText.enAlignStyle.left);
			uIText.Set("Zombie3D/Font/037-CAI978-13", text2 + string.Empty, m_ShopPropValueColor);
			m_uiShopItemDetail.Add(uIText);
		}
		if (powerUPSCfg.type == ItemType.Shield)
		{
			num8 -= 20f;
			uIText = UIUtils.BuildUIText(0, new Rect(num7, num8, 200f, 20f), UIText.enAlignStyle.right);
			uIText.Set("Zombie3D/Font/037-CAI978-15", "BLOCK ", Constant.TextCommonColor);
			m_uiShopItemDetail.Add(uIText);
			uIText = UIUtils.BuildUIText(0, new Rect(left, num8, 200f, 20f), UIText.enAlignStyle.left);
			uIText.Set("Zombie3D/Font/037-CAI978-13", "150", m_ShopPropValueColor);
			m_uiShopItemDetail.Add(uIText);
		}
		if (powerUPSCfg.type == ItemType.Pacemaker)
		{
			num8 -= 20f;
			uIText = UIUtils.BuildUIText(0, new Rect(num7, num8, 200f, 20f), UIText.enAlignStyle.right);
			uIText.Set("Zombie3D/Font/037-CAI978-15", "DEF ", Constant.TextCommonColor);
			m_uiShopItemDetail.Add(uIText);
			uIText = UIUtils.BuildUIText(0, new Rect(left, num8, 200f, 20f), UIText.enAlignStyle.left);
			uIText.Set("Zombie3D/Font/037-CAI978-13", "100%", m_ShopPropValueColor);
			m_uiShopItemDetail.Add(uIText);
		}
		if (powerUPSCfg.type == ItemType.Defibrilator)
		{
		}
		uIText = UIUtils.BuildUIText(0, new Rect(num + 43f, num2 + 40f, 400f, 70f), UIText.enAlignStyle.left);
		uIText.Set("Zombie3D/Font/037-CAI978-15", powerUPSCfg.introduction, Constant.TextCommonColor);
		m_uiShopItemDetail.Add(uIText);
		UIClickButton uIClickButton = null;
		uIClickButton = UIUtils.BuildClickButton(2031, new Rect(340f, 0f, 620f, 640f), m_MatTransparentUI, new Rect(1f, 1f, 1f, 1f), new Rect(1f, 1f, 1f, 1f), new Rect(1f, 1f, 1f, 1f), new Vector2(620f, 640f));
		m_uiShopItemDetail.Add(uIClickButton);
		uIClickButton = UIUtils.BuildClickButton(2030, new Rect(num + 145f, num2 - 15f, 191f, 62f), m_MatShopUI, new Rect(833f, 260f, 191f, 62f), new Rect(833f, 322f, 191f, 62f), new Rect(833f, 260f, 191f, 62f), new Vector2(191f, 62f));
		m_uiShopItemDetail.Add(uIClickButton);
	}

	public void SetupBuyBankDetail(bool bShow)
	{
		if (m_uiShopItemDetail != null)
		{
			m_uiShopItemDetail.Clear();
			m_uiShopItemDetail = null;
		}
		if (bShow)
		{
			m_uiShopItemDetail = new uiGroup(m_UIManager);
			if (m_MatBankIconUI == null)
			{
				m_MatBankIconUI = LoadUIMaterial("Zombie3D/UI/Materials/BankIcons");
			}
			UIImage control = UIUtils.BuildImage(0, new Rect(347f, 78f, 548f, 352f), m_MatShopUI, new Rect(598f, 120f, 1f, 1f), new Vector2(548f, 352f));
			m_uiShopItemDetail.Add(control);
			float num = 390f;
			float num2 = 92f;
			control = UIUtils.BuildImage(0, new Rect(num, num2, 483f, 357f), m_MatShopUI, new Rect(0f, 494f, 483f, 357f), new Vector2(483f, 357f));
			m_uiShopItemDetail.Add(control);
			float num3 = num + 170f;
			float num4 = num2 + 200f;
			FixedConfig.IAPCfg iAPCfg = ConfigManager.Instance().GetFixedConfig().GetIAPCfg(iapSelectedIndex);
			Rect bankIconTexture = GetBankIconTexture(iAPCfg);
			if (iapSelectedIndex == 1)
			{
				bankIconTexture = new Rect(448f, 168f, 110f, 110f);
				control = UIUtils.BuildImage(0, new Rect(num3 - bankIconTexture.width / 2f, num4 - bankIconTexture.height / 2f, bankIconTexture.width, bankIconTexture.height), m_MatShopUI, bankIconTexture, new Vector2(bankIconTexture.width, bankIconTexture.height));
				m_uiShopItemDetail.Add(control);
			}
			else
			{
				control = UIUtils.BuildImage(0, new Rect(num3 - bankIconTexture.width / 2f, num4 - bankIconTexture.height / 2f, bankIconTexture.width, bankIconTexture.height), m_MatBankIconUI, bankIconTexture, new Vector2(bankIconTexture.width, bankIconTexture.height));
				m_uiShopItemDetail.Add(control);
			}
			Rect bankDollorTexture = GetBankDollorTexture(iAPCfg);
			float num5 = num + 260f;
			float top = num2 + 146f;
			control = UIUtils.BuildImage(0, new Rect(num5 - bankDollorTexture.width, top, bankDollorTexture.width, bankDollorTexture.height), m_MatBankIconUI, bankDollorTexture, new Vector2(bankDollorTexture.width, bankDollorTexture.height));
			m_uiShopItemDetail.Add(control);
			string text = string.Empty;
			if (iAPCfg.gameGold > 0f)
			{
				text = "CASH " + iAPCfg.gameGold;
			}
			else if (iAPCfg.gameDollor > 0f)
			{
				text = "CRYSTAL " + iAPCfg.gameDollor;
			}
			UIText uIText = UIUtils.BuildUIText(0, new Rect(num + 110f, num2 + 310f, 250f, 25f), UIText.enAlignStyle.center);
			uIText.Set("Zombie3D/Font/037-CAI978-18", text, Constant.TextCommonColor);
			m_uiShopItemDetail.Add(uIText);
			uIText = UIUtils.BuildUIText(0, new Rect(num + 43f, num2 + 50f, 410f, 80f), UIText.enAlignStyle.left);
			uIText.Set("Zombie3D/Font/037-CAI978-15", iAPCfg.introduction, Constant.TextCommonColor);
			m_uiShopItemDetail.Add(uIText);
			UIClickButton control2 = UIUtils.BuildClickButton(2033, new Rect(340f, 0f, 620f, 640f), m_MatTransparentUI, new Rect(1f, 1f, 1f, 1f), new Rect(1f, 1f, 1f, 1f), new Rect(1f, 1f, 1f, 1f), new Vector2(620f, 640f));
			m_uiShopItemDetail.Add(control2);
			control2 = UIUtils.BuildClickButton(2032, new Rect(num + 145f, num2 - 15f, 191f, 62f), m_MatShopUI, new Rect(833f, 260f, 191f, 62f), new Rect(833f, 322f, 191f, 62f), new Rect(833f, 260f, 191f, 62f), new Vector2(191f, 62f));
			m_uiShopItemDetail.Add(control2);
			if (m_lsIAPSalesIndex.Contains(iapSelectedIndex))
			{
				uIText = UIUtils.BuildUIText(0, new Rect(num + 300f, num2 + 280f, 200f, 20f), UIText.enAlignStyle.left);
				uIText.Set("Zombie3D/Font/037-CAI978-22", "SALE", Constant.RedGroupColor);
				m_uiShopItemDetail.Add(uIText);
			}
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
			uIClickButton = UIUtils.BuildClickButton(2037, new Rect(num + 21f, num2 - 16f, 191f, 62f), m_MatDialog01, new Rect(640f, 186f, 191f, 62f), new Rect(832f, 186f, 191f, 62f), new Rect(640f, 186f, 191f, 62f), new Vector2(191f, 62f));
			m_uiHintDialog.Add(uIClickButton);
			uIClickButton = UIUtils.BuildClickButton(2038, new Rect(num + 280f, num2 - 16f, 191f, 62f), m_MatDialog01, new Rect(640f, 316f, 191f, 62f), new Rect(832f, 316f, 191f, 62f), new Rect(640f, 316f, 191f, 62f), new Vector2(191f, 62f));
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

	public void SetupSendGiftDialog(bool bShow)
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
			float left = 242f;
			float top = 232f;
			control = UIUtils.BuildImage(0, new Rect(left, top, 515f, 301f), m_MatDialog01, new Rect(0f, 378f, 515f, 301f), new Vector2(515f, 301f));
			m_DialogUIGroup.Add(control);
			UIText uIText = UIUtils.BuildUIText(0, new Rect(300f, 407f, 380f, 30f), UIText.enAlignStyle.center);
			uIText.Set("Zombie3D/Font/037-CAI978-18", "You've received a free gift!", Constant.TextCommonColor);
			m_DialogUIGroup.Add(uIText);
			float num = 545f;
			FixedConfig.PowerUPSCfg powerUPSCfg = ConfigManager.Instance().GetFixedConfig().GetPowerUPSCfg(ItemType.HpMiddle);
			string text = powerUPSCfg.name + "  3\n";
			UIText uIText2 = UIUtils.BuildUIText(0, new Rect(num - 200f, 367f, 200f, 25f), UIText.enAlignStyle.right);
			uIText2.Set("Zombie3D/Font/037-CAI978-18", powerUPSCfg.name, Constant.TextCommonColor);
			m_DialogUIGroup.Add(uIText2);
			uIText2 = UIUtils.BuildUIText(0, new Rect(num + 20f, 367f, 200f, 25f), UIText.enAlignStyle.left);
			uIText2.Set("Zombie3D/Font/037-CAI978-18", "  3", Constant.TextCommonColor);
			m_DialogUIGroup.Add(uIText2);
			FixedConfig.PowerUPSCfg powerUPSCfg2 = ConfigManager.Instance().GetFixedConfig().GetPowerUPSCfg(ItemType.Pacemaker);
			uIText2 = UIUtils.BuildUIText(0, new Rect(num - 200f, 337f, 200f, 25f), UIText.enAlignStyle.right);
			uIText2.Set("Zombie3D/Font/037-CAI978-18", powerUPSCfg2.name, Constant.TextCommonColor);
			m_DialogUIGroup.Add(uIText2);
			uIText2 = UIUtils.BuildUIText(0, new Rect(num + 20f, 337f, 200f, 25f), UIText.enAlignStyle.left);
			uIText2.Set("Zombie3D/Font/037-CAI978-18", "  2", Constant.TextCommonColor);
			m_DialogUIGroup.Add(uIText2);
			FixedConfig.PowerUPSCfg powerUPSCfg3 = ConfigManager.Instance().GetFixedConfig().GetPowerUPSCfg(ItemType.StormGrenade);
			uIText2 = UIUtils.BuildUIText(0, new Rect(num - 200f, 307f, 200f, 25f), UIText.enAlignStyle.right);
			uIText2.Set("Zombie3D/Font/037-CAI978-18", powerUPSCfg3.name, Constant.TextCommonColor);
			m_DialogUIGroup.Add(uIText2);
			uIText2 = UIUtils.BuildUIText(0, new Rect(num + 20f, 307f, 200f, 25f), UIText.enAlignStyle.left);
			uIText2.Set("Zombie3D/Font/037-CAI978-18", "  2", Constant.TextCommonColor);
			m_DialogUIGroup.Add(uIText2);
			UIClickButton uIClickButton = null;
			uIClickButton = UIUtils.BuildClickButton(2039, new Rect(385f, 215f, 191f, 62f), m_MatDialog01, new Rect(640f, 0f, 191f, 62f), new Rect(832f, 0f, 191f, 62f), new Rect(640f, 0f, 191f, 62f), new Vector2(191f, 62f));
			m_DialogUIGroup.Add(uIClickButton);
		}
	}

	public void SetupReviewNotificationDialog(bool bShow)
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
			UIText uIText = UIUtils.BuildUIText(0, new Rect(300f, 407f, 380f, 30f), UIText.enAlignStyle.left);
			uIText.Set("Zombie3D/Font/037-CAI978-18", "Your good rating will inspire us to release updates faster! Please rate now.", Constant.TextCommonColor);
			m_DialogNoticeUIGroup.Add(uIText);
			UIClickButton uIClickButton = null;
			uIClickButton = UIUtils.BuildClickButton(2041, new Rect(280f, 215f, 191f, 62f), m_MatDialog01, new Rect(640f, 186f, 191f, 62f), new Rect(832f, 186f, 191f, 62f), new Rect(640f, 186f, 191f, 62f), new Vector2(191f, 62f));
			m_DialogNoticeUIGroup.Add(uIClickButton);
			uIClickButton = UIUtils.BuildClickButton(2040, new Rect(504f, 215f, 191f, 62f), m_MatDialog01, new Rect(640f, 0f, 191f, 62f), new Rect(832f, 0f, 191f, 62f), new Rect(640f, 0f, 191f, 62f), new Vector2(191f, 62f));
			m_DialogNoticeUIGroup.Add(uIClickButton);
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
			uIClickButton = UIUtils.BuildClickButton(2042, new Rect(385f, 215f, 191f, 62f), m_MatDialog01, new Rect(640f, 0f, 191f, 62f), new Rect(832f, 0f, 191f, 62f), new Rect(640f, 0f, 191f, 62f), new Vector2(191f, 62f));
			m_DialogNoticeUIGroup.Add(uIClickButton);
		}
	}

	public void SetupUnlockAccouterDialog(bool bShow)
	{
		if (m_UnLockAccouterDialog != null)
		{
			m_UnLockAccouterDialog.Clear();
			m_UnLockAccouterDialog = null;
		}
		if (!bShow)
		{
			if (playerShow.gameObject.transform.parent != null)
			{
				Camera camera = playerShow.gameObject.transform.parent.GetComponentInChildren(typeof(Camera)) as Camera;
				camera.enabled = true;
			}
			else
			{
				Debug.LogError("null playerShow parent");
			}
			return;
		}
		if (playerShow.gameObject.transform.parent != null)
		{
			Camera camera2 = playerShow.gameObject.transform.parent.GetComponentInChildren(typeof(Camera)) as Camera;
			camera2.enabled = false;
		}
		else
		{
			Debug.LogError("null playerShow parent");
		}
		m_UnLockAccouterDialog = new uiGroup(m_UIManager);
		if (m_MatWeaponIcons == null)
		{
			m_MatWeaponIcons = LoadUIMaterial("Zombie3D/UI/Materials/WeaponIcons");
		}
		if (m_MatAvatarIcons == null)
		{
			m_MatAvatarIcons = LoadUIMaterial("Zombie3D/UI/Materials/AvatarIcons");
		}
		int level = GameApp.GetInstance().GetGameState().Level;
		int num = gameState.m_iPlayerLastLevel + 1;
		if (num >= level)
		{
			num = level;
		}
		int num2 = 0;
		UIImage control = UIUtils.BuildImage(0, new Rect(0f, 0f, 960f, 640f), m_MatDialog01, new Rect(600f, 1f, 1f, 1f), new Vector2(960f, 640f));
		m_UnLockAccouterDialog.Add(control);
		Material mat = LoadUIMaterial("Zombie3D/UI/Materials/ChoosePoints1UI");
		Resources.UnloadUnusedAssets();
		control = UIUtils.BuildImage(0, new Rect(99f, 57f, 771f, 450f), mat, new Rect(0f, 0f, 771f, 450f), new Vector2(771f, 450f));
		m_UnLockAccouterDialog.Add(control);
		UIText uIText = UIUtils.BuildUIText(0, new Rect(300f, 316f, 450f, 90f), UIText.enAlignStyle.left);
		uIText.Set("Zombie3D/Font/037-CAI978-18", m_strUnLockAccouterText, Constant.TextCommonColor);
		m_UnLockAccouterDialog.Add(uIText);
		UIClickButton control2 = UIUtils.BuildClickButton(2145, new Rect(400f, 40f, 191f, 62f), m_MatDialog01, new Rect(640f, 0f, 191f, 62f), new Rect(832f, 0f, 191f, 62f), new Rect(640f, 0f, 191f, 62f), new Vector2(191f, 62f));
		m_UnLockAccouterDialog.Add(control2);
		uIText = UIUtils.BuildUIText(0, new Rect(420f, 446f, 250f, 36f), UIText.enAlignStyle.left);
		uIText.Set("Zombie3D/Font/037-CAI978-27", "NOTICE", Constant.TextCommonColor);
		m_UnLockAccouterDialog.Add(uIText);
		Rect[] array = new Rect[3]
		{
			new Rect(200f, 163f, 168f, 153f),
			new Rect(403f, 163f, 168f, 153f),
			new Rect(606f, 163f, 168f, 153f)
		};
		ArrayList weapons = ConfigManager.Instance().GetFixedConfig().weapons;
		for (int num3 = weapons.Count - 1; num3 >= 0; num3--)
		{
			if (((FixedConfig.WeaponCfg)weapons[num3]).levelLimit <= level && ((FixedConfig.WeaponCfg)weapons[num3]).levelLimit >= num && num2 < 3 && num2 >= 0)
			{
				control = UIUtils.BuildImage(0, array[num2], m_MatDialog01, new Rect(647f, 775f, 168f, 153f), new Vector2(168f, 153f));
				m_UnLockAccouterDialog.Add(control);
				control = UIUtils.BuildImage(0, array[num2], m_MatWeaponIcons, GetWeaponIconTexture((WeaponType)((FixedConfig.WeaponCfg)weapons[num3]).type), new Vector2(185f, 111f));
				m_UnLockAccouterDialog.Add(control);
				num2++;
			}
		}
		bool flag = false;
		ArrayList avatarCfgs = ConfigManager.Instance().GetFixedConfig().avatarCfgs;
		for (int num4 = avatarCfgs.Count - 1; num4 >= 0; num4--)
		{
			if (((FixedConfig.AvatarCfg)avatarCfgs[num4]).levelLimit <= level && ((FixedConfig.AvatarCfg)avatarCfgs[num4]).levelLimit >= num)
			{
				if (num2 < 3 && num2 >= 0)
				{
					control = UIUtils.BuildImage(0, array[num2], m_MatDialog01, new Rect(647f, 775f, 168f, 153f), new Vector2(168f, 153f));
					m_UnLockAccouterDialog.Add(control);
					FixedConfig.AvatarCfg avatarCfg = (FixedConfig.AvatarCfg)avatarCfgs[num4];
					if (flag)
					{
						Rect avatarIconTexture = GetAvatarIconTexture(avatarCfg.suiteType, Avatar.AvatarType.Head);
						control = UIUtils.BuildImage(0, array[num2], m_MatAvatarIcons, avatarIconTexture, new Vector2(avatarIconTexture.width, avatarIconTexture.height));
					}
					else
					{
						Rect avatarIconTexture2 = GetAvatarIconTexture(avatarCfg.suiteType, Avatar.AvatarType.Body);
						control = UIUtils.BuildImage(0, array[num2], m_MatAvatarIcons, avatarIconTexture2, new Vector2(avatarIconTexture2.width, avatarIconTexture2.height));
						flag = true;
					}
					m_UnLockAccouterDialog.Add(control);
					num2++;
				}
			}
			else
			{
				flag = false;
			}
		}
		if (num2 <= 0)
		{
			SetupUnlockAccouterDialog(false);
		}
	}

	public void SetupLevelUpHortationDialog(bool bShow)
	{
		if (m_LevelUpDialog != null)
		{
			m_LevelUpDialog.Clear();
			m_LevelUpDialog = null;
		}
		if (!bShow)
		{
			if (playerShow.gameObject.transform.parent != null)
			{
				Camera camera = playerShow.gameObject.transform.parent.GetComponentInChildren(typeof(Camera)) as Camera;
				camera.enabled = true;
			}
			else
			{
				Debug.LogError("null playerShow parent");
			}
			return;
		}
		if (playerShow.gameObject.transform.parent != null)
		{
			Camera camera2 = playerShow.gameObject.transform.parent.GetComponentInChildren(typeof(Camera)) as Camera;
			camera2.enabled = false;
		}
		else
		{
			Debug.LogError("null playerShow parent");
		}
		m_LevelUpDialog = new uiGroup(m_UIManager);
		int num = 0;
		int num2 = 0;
		for (int i = gameState.m_iPlayerLastLevel + 1; i <= gameState.Level; i++)
		{
			if ((i - 1) % 5 == 0)
			{
				num2++;
				gameState.AddDollor(1);
			}
			else
			{
				num += 200 + 20 * gameState.Level;
				gameState.AddGold(200 + 20 * gameState.Level);
				GameApp.GetInstance().Save();
			}
		}
		string empty = string.Empty;
		if ((gameState.Level - 1) % 5 == 0)
		{
			empty = "Ding, you leveled! You've received a well-earned " + num2 + " tCrystal reward. Reach level ";
			empty = empty + (gameState.Level + 5) + " to get tCrystal x 1!";
		}
		else
		{
			empty = "Ding, you leveled! You've received a well-earned " + num + " cash reward. Reach level ";
			empty = empty + (5 - (gameState.Level - 1) % 5 + gameState.Level) + " to get tCrystal x 1!";
		}
		UIImage control = UIUtils.BuildImage(0, new Rect(0f, 0f, 960f, 640f), m_MatDialog01, new Rect(600f, 1f, 1f, 1f), new Vector2(960f, 640f));
		m_LevelUpDialog.Add(control);
		control = UIUtils.BuildImage(0, new Rect(215f, 167f, 515f, 301f), m_MatDialog01, new Rect(0f, 378f, 515f, 301f), new Vector2(515f, 301f));
		m_LevelUpDialog.Add(control);
		UIText uIText = UIUtils.BuildUIText(0, new Rect(260f, 230f, 400f, 135f), UIText.enAlignStyle.center);
		uIText.Set("Zombie3D/Font/037-CAI978-18", empty, Constant.TextCommonColor);
		m_LevelUpDialog.Add(uIText);
		UIClickButton control2 = UIUtils.BuildClickButton(2146, new Rect(385f, 152f, 191f, 62f), m_MatDialog01, new Rect(640f, 0f, 191f, 62f), new Rect(832f, 0f, 191f, 62f), new Rect(640f, 0f, 191f, 62f), new Vector2(191f, 62f));
		m_LevelUpDialog.Add(control2);
		SetupAroundUI(true);
	}

	public void SetupSellsDialog(bool bShow, int price = 0)
	{
		if (m_DialogSellsUIGroup != null)
		{
			m_DialogSellsUIGroup.Clear();
			m_DialogSellsUIGroup = null;
		}
		if (bShow)
		{
			string text = "Are you sure you want to sell this for ";
			text = text + price + " cash?";
			m_DialogSellsUIGroup = new uiGroup(m_UIManager);
			UIImage control = UIUtils.BuildImage(0, new Rect(0f, 0f, 960f, 640f), m_MatDialog01, new Rect(600f, 1f, 1f, 1f), new Vector2(960f, 640f));
			m_DialogSellsUIGroup.Add(control);
			float left = 242f;
			float top = 232f;
			control = UIUtils.BuildImage(0, new Rect(left, top, 515f, 301f), m_MatDialog01, new Rect(0f, 378f, 515f, 301f), new Vector2(515f, 301f));
			m_DialogSellsUIGroup.Add(control);
			UIText uIText = UIUtils.BuildUIText(0, new Rect(290f, 295f, 420f, 150f), UIText.enAlignStyle.center);
			uIText.Set("Zombie3D/Font/037-CAI978-18", text, Constant.TextCommonColor);
			m_DialogSellsUIGroup.Add(uIText);
			UIClickButton uIClickButton = null;
			uIClickButton = UIUtils.BuildClickButton(2148, new Rect(280f, 215f, 191f, 62f), m_MatDialog01, new Rect(640f, 186f, 191f, 62f), new Rect(832f, 186f, 191f, 62f), new Rect(640f, 186f, 191f, 62f), new Vector2(191f, 62f));
			m_DialogSellsUIGroup.Add(uIClickButton);
			uIClickButton = UIUtils.BuildClickButton(2147, new Rect(504f, 215f, 191f, 62f), m_MatDialog01, new Rect(640f, 0f, 191f, 62f), new Rect(832f, 0f, 191f, 62f), new Rect(640f, 0f, 191f, 62f), new Vector2(191f, 62f));
			m_DialogSellsUIGroup.Add(uIClickButton);
		}
	}

	public void SellAvatar(FixedConfig.AvatarCfg avatar_cfg, int gold)
	{
		gameState.RemoveAvatar(avatar_cfg.suiteType, avatar_cfg.avtType);
		gameState.AddGold(gold);
		SetupAroundUI(true);
		SetupAvatarShopUI(true);
		SetupBuyAvatarDetail(false);
		SetupBuyAvatarDetail(true);
	}

	public void SellWeapon(WeaponType weaponType, int gold)
	{
		gameState.RemoveWeapon(weaponWantToBuy);
		gameState.AddGold(gold);
		SetupAroundUI(true);
		SetupWeaponShopUI(true);
		SetupBuyWeaponDetail(false);
		SetupBuyWeaponDetail(true);
	}

	public void CheckIAPSales()
	{
		if (gameState.m_bCheckDataTimeOK)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(new GameObject("CheckIAPSale")) as GameObject;
			IAPSales iAPSales = gameObject.AddComponent(typeof(IAPSales)) as IAPSales;
			iAPSales.m_DownLoadErrorEvent = CheckIAPSalesError;
			iAPSales.m_DownLoadOKEvent = CheckIAPSalesOK;
		}
		else
		{
			SetupShopUI(true);
		}
	}

	public void CheckIAPSalesOK(List<IAPSalesClass> _lsIAPS, List<int> _lsSaleDays)
	{
		long nowDateSeconds = UtilsEx.getNowDateSeconds();
		DateTime dateTimeBySeconds = UtilsEx.getDateTimeBySeconds(nowDateSeconds);
		if (_lsSaleDays.Count > 0)
		{
			if (_lsSaleDays.Contains((int)dateTimeBySeconds.DayOfWeek))
			{
				m_lsIAPSalesIndex = gameState.CheckIAPSales(_lsIAPS);
			}
			else
			{
				ConfigManager.Instance().GetFixedConfig().RestIAPConfig();
			}
		}
		SetupShopUI(true);
	}

	public void CheckIAPSalesError()
	{
		SetupShopUI(true);
	}

	public static Rect GetWeaponIconTexture(WeaponType weapon_type)
	{
		switch (weapon_type)
		{
		case WeaponType.Beretta_33:
			return new Rect(0f, 0f, 185f, 111f);
		case WeaponType.GrewCar_15:
			return new Rect(185f, 0f, 185f, 111f);
		case WeaponType.UZI_E:
			return new Rect(370f, 0f, 185f, 111f);
		case WeaponType.RemingtonPipe:
			return new Rect(555f, 0f, 185f, 111f);
		case WeaponType.Springfield_9mm:
			return new Rect(740f, 0f, 185f, 111f);
		case WeaponType.Kalashnikov_II:
			return new Rect(0f, 111f, 185f, 111f);
		case WeaponType.Barrett_P90:
			return new Rect(185f, 111f, 185f, 111f);
		case WeaponType.ParkerGaussRifle:
			return new Rect(370f, 111f, 185f, 111f);
		case WeaponType.ZombieBusters:
			return new Rect(555f, 111f, 185f, 111f);
		case WeaponType.SimonovPistol:
			return new Rect(740f, 111f, 185f, 111f);
		case WeaponType.BarrettSplitIII:
			return new Rect(0f, 222f, 185f, 111f);
		case WeaponType.Tomahawk:
			return new Rect(185f, 222f, 185f, 111f);
		case WeaponType.SimonoRayRifle:
			return new Rect(370f, 222f, 185f, 111f);
		case WeaponType.Volcano:
			return new Rect(555f, 222f, 185f, 111f);
		case WeaponType.Hellfire:
			return new Rect(740f, 222f, 185f, 111f);
		case WeaponType.Nailer:
			return new Rect(0f, 333f, 185f, 111f);
		case WeaponType.NeutronRifle:
			return new Rect(185f, 333f, 185f, 111f);
		case WeaponType.BigFirework:
			return new Rect(370f, 333f, 185f, 111f);
		case WeaponType.Stormgun:
			return new Rect(555f, 333f, 185f, 111f);
		case WeaponType.Lightning:
			return new Rect(740f, 333f, 185f, 111f);
		case WeaponType.MassacreCannon:
			return new Rect(0f, 444f, 185f, 111f);
		case WeaponType.DoubleSnake:
			return new Rect(185f, 444f, 185f, 111f);
		case WeaponType.Longinus:
			return new Rect(370f, 444f, 185f, 111f);
		case WeaponType.CrossBow:
			return new Rect(134f, 573f, 137f, 84f);
		case WeaponType.Messiah:
			return new Rect(0f, 573f, 134f, 102f);
		case WeaponType.Ion_Cannon:
			return new Rect(555f, 444f, 185f, 111f);
		case WeaponType.Ion_CannonI:
			return new Rect(740f, 444f, 185f, 111f);
		case WeaponType.Ion_CannonSub:
			return new Rect(300f, 555f, 185f, 111f);
		case WeaponType.RPG_4:
			return new Rect(555f, 555f, 185f, 111f);
		default:
			return new Rect(0f, 0f, 0f, 0f);
		}
	}

	public static Rect GetAvatarIconTexture(Avatar.AvatarSuiteType avatar_suite_type, Avatar.AvatarType avatar_type)
	{
		switch (avatar_suite_type)
		{
		case Avatar.AvatarSuiteType.Driver:
			switch (avatar_type)
			{
			case Avatar.AvatarType.Head:
				return new Rect(263f, 445f, 76f, 75f);
			case Avatar.AvatarType.Body:
				return new Rect(18f, 123f, 117f, 91f);
			}
			break;
		case Avatar.AvatarSuiteType.Policeman:
			switch (avatar_type)
			{
			case Avatar.AvatarType.Head:
				return new Rect(397f, 221f, 94f, 93f);
			case Avatar.AvatarType.Body:
				return new Rect(141f, 24f, 119f, 93f);
			}
			break;
		case Avatar.AvatarSuiteType.Surgeon:
			switch (avatar_type)
			{
			case Avatar.AvatarType.Head:
				return new Rect(689f, 439f, 75f, 77f);
			case Avatar.AvatarType.Body:
				return new Rect(518f, 20f, 124f, 98f);
			}
			break;
		case Avatar.AvatarSuiteType.Cowboy:
			switch (avatar_type)
			{
			case Avatar.AvatarType.Head:
				return new Rect(207f, 325f, 127f, 92f);
			case Avatar.AvatarType.Body:
				return new Rect(771f, 223f, 117f, 92f);
			}
			break;
		case Avatar.AvatarSuiteType.Hacker:
			switch (avatar_type)
			{
			case Avatar.AvatarType.Head:
				return new Rect(782f, 334f, 84f, 84f);
			case Avatar.AvatarType.Body:
				return new Rect(519f, 120f, 118f, 91f);
			}
			break;
		case Avatar.AvatarSuiteType.Navy:
			switch (avatar_type)
			{
			case Avatar.AvatarType.Head:
				return new Rect(162f, 225f, 88f, 88f);
			case Avatar.AvatarType.Body:
				return new Rect(896f, 121f, 116f, 92f);
			}
			break;
		case Avatar.AvatarSuiteType.Ninjalong:
			switch (avatar_type)
			{
			case Avatar.AvatarType.Head:
				return new Rect(280f, 240f, 77f, 75f);
			case Avatar.AvatarType.Body:
				return new Rect(16f, 13f, 119f, 103f);
			}
			break;
		case Avatar.AvatarSuiteType.Swat:
			switch (avatar_type)
			{
			case Avatar.AvatarType.Head:
				return new Rect(561f, 439f, 99f, 83f);
			case Avatar.AvatarType.Body:
				return new Rect(645f, 24f, 119f, 92f);
			}
			break;
		case Avatar.AvatarSuiteType.MaskKnight:
			switch (avatar_type)
			{
			case Avatar.AvatarType.Head:
				return new Rect(879f, 332f, 131f, 81f);
			case Avatar.AvatarType.Body:
				return new Rect(646f, 121f, 117f, 92f);
			}
			break;
		case Avatar.AvatarSuiteType.Gentleman:
			switch (avatar_type)
			{
			case Avatar.AvatarType.Head:
				return new Rect(550f, 331f, 106f, 90f);
			case Avatar.AvatarType.Body:
				return new Rect(264f, 119f, 117f, 93f);
			}
			break;
		case Avatar.AvatarSuiteType.ZombieAssassin:
			switch (avatar_type)
			{
			case Avatar.AvatarType.Head:
				return new Rect(117f, 336f, 87f, 79f);
			case Avatar.AvatarType.Body:
				return new Rect(647f, 224f, 116f, 97f);
			}
			break;
		case Avatar.AvatarSuiteType.DeathSquads:
			switch (avatar_type)
			{
			case Avatar.AvatarType.Head:
				return new Rect(345f, 340f, 84f, 81f);
			case Avatar.AvatarType.Body:
				return new Rect(895f, 223f, 117f, 92f);
			}
			break;
		case Avatar.AvatarSuiteType.Waiter:
			switch (avatar_type)
			{
			case Avatar.AvatarType.Head:
				return new Rect(901f, 437f, 83f, 77f);
			case Avatar.AvatarType.Body:
				return new Rect(894f, 23f, 119f, 94f);
			}
			break;
		case Avatar.AvatarSuiteType.Mechanic:
			switch (avatar_type)
			{
			case Avatar.AvatarType.Head:
				return new Rect(47f, 228f, 88f, 88f);
			case Avatar.AvatarType.Body:
				return new Rect(772f, 122f, 118f, 93f);
			}
			break;
		case Avatar.AvatarSuiteType.Gladiator:
			switch (avatar_type)
			{
			case Avatar.AvatarType.Head:
				return new Rect(669f, 320f, 88f, 107f);
			case Avatar.AvatarType.Body:
				return new Rect(388f, 120f, 117f, 96f);
			}
			break;
		case Avatar.AvatarSuiteType.ViolenceFr:
			switch (avatar_type)
			{
			case Avatar.AvatarType.Head:
				return new Rect(796f, 436f, 81f, 83f);
			case Avatar.AvatarType.Body:
				return new Rect(773f, 21f, 117f, 95f);
			}
			break;
		case Avatar.AvatarSuiteType.SuperNemesis:
			switch (avatar_type)
			{
			case Avatar.AvatarType.Head:
				return new Rect(467f, 444f, 75f, 77f);
			case Avatar.AvatarType.Body:
				return new Rect(388f, 14f, 123f, 104f);
			}
			break;
		case Avatar.AvatarSuiteType.EvilClown:
			switch (avatar_type)
			{
			case Avatar.AvatarType.Head:
				return new Rect(453f, 330f, 91f, 90f);
			case Avatar.AvatarType.Body:
				return new Rect(142f, 123f, 118f, 92f);
			}
			break;
		case Avatar.AvatarSuiteType.X800:
			switch (avatar_type)
			{
			case Avatar.AvatarType.Head:
				return new Rect(22f, 337f, 76f, 76f);
			case Avatar.AvatarType.Body:
				return new Rect(519f, 222f, 117f, 93f);
			}
			break;
		case Avatar.AvatarSuiteType.ShadowAgents:
			switch (avatar_type)
			{
			case Avatar.AvatarType.Head:
				return new Rect(360f, 444f, 85f, 78f);
			case Avatar.AvatarType.Body:
				return new Rect(263f, 23f, 119f, 93f);
			}
			break;
		case Avatar.AvatarSuiteType.Pirate:
			switch (avatar_type)
			{
			case Avatar.AvatarType.Head:
				return new Rect(148f, 426f, 106f, 96f);
			case Avatar.AvatarType.Body:
				return new Rect(14f, 426f, 122f, 97f);
			}
			break;
		case Avatar.AvatarSuiteType.Eskimo:
			switch (avatar_type)
			{
			case Avatar.AvatarType.Head:
				return new Rect(641f, 524f, 110f, 98f);
			case Avatar.AvatarType.Body:
				return new Rect(755f, 526f, 113f, 90f);
			}
			break;
		case Avatar.AvatarSuiteType.Shinobi:
			switch (avatar_type)
			{
			case Avatar.AvatarType.Head:
				return new Rect(0f, 526f, 82f, 74f);
			case Avatar.AvatarType.Body:
				return new Rect(275f, 526f, 117f, 100f);
			}
			break;
		case Avatar.AvatarSuiteType.Kunoichi:
			switch (avatar_type)
			{
			case Avatar.AvatarType.Head:
				return new Rect(89f, 526f, 89f, 79f);
			case Avatar.AvatarType.Body:
				return new Rect(395f, 533f, 114f, 102f);
			}
			break;
		case Avatar.AvatarSuiteType.DemonLord:
			switch (avatar_type)
			{
			case Avatar.AvatarType.Head:
				return new Rect(185f, 524f, 81f, 82f);
			case Avatar.AvatarType.Body:
				return new Rect(510f, 526f, 122f, 116f);
			}
			break;
		case Avatar.AvatarSuiteType.Rugby:
			switch (avatar_type)
			{
			case Avatar.AvatarType.Head:
				return new Rect(122f, 653f, 98f, 92f);
			case Avatar.AvatarType.Body:
				return new Rect(0f, 653f, 122f, 106f);
			}
			break;
		case Avatar.AvatarSuiteType.VanHelsing:
			switch (avatar_type)
			{
			case Avatar.AvatarType.Head:
				return new Rect(342f, 653f, 98f, 84f);
			case Avatar.AvatarType.Body:
				return new Rect(220f, 653f, 122f, 97f);
			}
			break;
		}
		return new Rect(0f, 0f, 0f, 0f);
	}

	public static Rect GetAvatarIconHeadTexture(Avatar.AvatarSuiteType avatar_suite_type)
	{
		Avatar.AvatarType avatarType = Avatar.AvatarType.Head;
		switch (avatar_suite_type)
		{
		case Avatar.AvatarSuiteType.Driver:
			switch (avatarType)
			{
			case Avatar.AvatarType.Head:
				return new Rect(930f, 103f, 86f, 90f);
			case Avatar.AvatarType.Body:
				return new Rect(18f, 123f, 117f, 91f);
			}
			break;
		case Avatar.AvatarSuiteType.Policeman:
			switch (avatarType)
			{
			case Avatar.AvatarType.Head:
				return new Rect(827f, 2f, 100f, 102f);
			case Avatar.AvatarType.Body:
				return new Rect(141f, 24f, 119f, 93f);
			}
			break;
		case Avatar.AvatarSuiteType.Surgeon:
			switch (avatarType)
			{
			case Avatar.AvatarType.Head:
				return new Rect(196f, 198f, 80f, 84f);
			case Avatar.AvatarType.Body:
				return new Rect(518f, 20f, 124f, 98f);
			}
			break;
		case Avatar.AvatarSuiteType.Cowboy:
			switch (avatarType)
			{
			case Avatar.AvatarType.Head:
				return new Rect(2f, 2f, 140f, 96f);
			case Avatar.AvatarType.Body:
				return new Rect(771f, 223f, 117f, 92f);
			}
			break;
		case Avatar.AvatarSuiteType.Hacker:
			switch (avatarType)
			{
			case Avatar.AvatarType.Head:
				return new Rect(801f, 107f, 90f, 90f);
			case Avatar.AvatarType.Body:
				return new Rect(519f, 120f, 118f, 91f);
			}
			break;
		case Avatar.AvatarSuiteType.Navy:
			switch (avatarType)
			{
			case Avatar.AvatarType.Head:
				return new Rect(930f, 2f, 92f, 98f);
			case Avatar.AvatarType.Body:
				return new Rect(896f, 121f, 116f, 92f);
			}
			break;
		case Avatar.AvatarSuiteType.Ninjalong:
			switch (avatarType)
			{
			case Avatar.AvatarType.Head:
				return new Rect(450f, 113f, 90f, 88f);
			case Avatar.AvatarType.Body:
				return new Rect(16f, 13f, 119f, 103f);
			}
			break;
		case Avatar.AvatarSuiteType.Swat:
			switch (avatarType)
			{
			case Avatar.AvatarType.Head:
				return new Rect(145f, 93f, 102f, 88f);
			case Avatar.AvatarType.Body:
				return new Rect(645f, 24f, 119f, 92f);
			}
			break;
		case Avatar.AvatarSuiteType.MaskKnight:
			switch (avatarType)
			{
			case Avatar.AvatarType.Head:
				return new Rect(145f, 2f, 138f, 88f);
			case Avatar.AvatarType.Body:
				return new Rect(646f, 121f, 117f, 92f);
			}
			break;
		case Avatar.AvatarSuiteType.Gentleman:
			switch (avatarType)
			{
			case Avatar.AvatarType.Head:
				return new Rect(286f, 2f, 114f, 98f);
			case Avatar.AvatarType.Body:
				return new Rect(264f, 119f, 117f, 93f);
			}
			break;
		case Avatar.AvatarSuiteType.ZombieAssassin:
			switch (avatarType)
			{
			case Avatar.AvatarType.Head:
				return new Rect(609f, 103f, 94f, 84f);
			case Avatar.AvatarType.Body:
				return new Rect(647f, 224f, 116f, 97f);
			}
			break;
		case Avatar.AvatarSuiteType.DeathSquads:
			switch (avatarType)
			{
			case Avatar.AvatarType.Head:
				return new Rect(632f, 192f, 88f, 84f);
			case Avatar.AvatarType.Body:
				return new Rect(895f, 223f, 117f, 92f);
			}
			break;
		case Avatar.AvatarSuiteType.Waiter:
			switch (avatarType)
			{
			case Avatar.AvatarType.Head:
				return new Rect(103f, 184f, 90f, 84f);
			case Avatar.AvatarType.Body:
				return new Rect(894f, 23f, 119f, 94f);
			}
			break;
		case Avatar.AvatarSuiteType.Mechanic:
			switch (avatarType)
			{
			case Avatar.AvatarType.Head:
				return new Rect(351f, 103f, 96f, 92f);
			case Avatar.AvatarType.Body:
				return new Rect(772f, 122f, 118f, 93f);
			}
			break;
		case Avatar.AvatarSuiteType.Gladiator:
			switch (avatarType)
			{
			case Avatar.AvatarType.Head:
				return new Rect(516f, 2f, 90f, 108f);
			case Avatar.AvatarType.Body:
				return new Rect(388f, 120f, 117f, 96f);
			}
			break;
		case Avatar.AvatarSuiteType.ViolenceFr:
			switch (avatarType)
			{
			case Avatar.AvatarType.Head:
				return new Rect(543f, 190f, 86f, 88f);
			case Avatar.AvatarType.Body:
				return new Rect(773f, 21f, 117f, 95f);
			}
			break;
		case Avatar.AvatarSuiteType.SuperNemesis:
			switch (avatarType)
			{
			case Avatar.AvatarType.Head:
				return new Rect(362f, 198f, 80f, 84f);
			case Avatar.AvatarType.Body:
				return new Rect(388f, 14f, 123f, 104f);
			}
			break;
		case Avatar.AvatarSuiteType.EvilClown:
			switch (avatarType)
			{
			case Avatar.AvatarType.Head:
				return new Rect(718f, 2f, 106f, 98f);
			case Avatar.AvatarType.Body:
				return new Rect(142f, 123f, 118f, 92f);
			}
			break;
		case Avatar.AvatarSuiteType.X800:
			switch (avatarType)
			{
			case Avatar.AvatarType.Head:
				return new Rect(279f, 198f, 80f, 84f);
			case Avatar.AvatarType.Body:
				return new Rect(519f, 222f, 117f, 93f);
			}
			break;
		case Avatar.AvatarSuiteType.ShadowAgents:
			switch (avatarType)
			{
			case Avatar.AvatarType.Head:
				return new Rect(706f, 103f, 92f, 86f);
			case Avatar.AvatarType.Body:
				return new Rect(263f, 23f, 119f, 93f);
			}
			break;
		case Avatar.AvatarSuiteType.Pirate:
			switch (avatarType)
			{
			case Avatar.AvatarType.Head:
				return new Rect(609f, 2f, 106f, 98f);
			case Avatar.AvatarType.Body:
				return new Rect(14f, 426f, 122f, 97f);
			}
			break;
		case Avatar.AvatarSuiteType.Eskimo:
			switch (avatarType)
			{
			case Avatar.AvatarType.Head:
				return new Rect(403f, 2f, 110f, 98f);
			case Avatar.AvatarType.Body:
				return new Rect(755f, 526f, 113f, 90f);
			}
			break;
		case Avatar.AvatarSuiteType.Shinobi:
			switch (avatarType)
			{
			case Avatar.AvatarType.Head:
				return new Rect(894f, 196f, 86f, 78f);
			case Avatar.AvatarType.Body:
				return new Rect(275f, 526f, 117f, 100f);
			}
			break;
		case Avatar.AvatarSuiteType.Kunoichi:
			switch (avatarType)
			{
			case Avatar.AvatarType.Head:
				return new Rect(2f, 188f, 90f, 80f);
			case Avatar.AvatarType.Body:
				return new Rect(395f, 533f, 114f, 102f);
			}
			break;
		case Avatar.AvatarSuiteType.DemonLord:
			switch (avatarType)
			{
			case Avatar.AvatarType.Head:
				return new Rect(723f, 200f, 82f, 82f);
			case Avatar.AvatarType.Body:
				return new Rect(510f, 526f, 122f, 116f);
			}
			break;
		case Avatar.AvatarSuiteType.Rugby:
			switch (avatarType)
			{
			case Avatar.AvatarType.Head:
				return new Rect(250f, 103f, 98f, 92f);
			case Avatar.AvatarType.Body:
				return new Rect(0f, 653f, 122f, 106f);
			}
			break;
		case Avatar.AvatarSuiteType.VanHelsing:
			switch (avatarType)
			{
			case Avatar.AvatarType.Head:
				return new Rect(2f, 101f, 98f, 84f);
			case Avatar.AvatarType.Body:
				return new Rect(220f, 653f, 122f, 97f);
			}
			break;
		}
		return new Rect(0f, 0f, 0f, 0f);
	}

	public static Rect GetPowerUpsIconTexture(ItemType item_type)
	{
		switch (item_type)
		{
		case ItemType.PowerSmall:
			return new Rect(0f, 0f, 92f, 92f);
		case ItemType.PowerMiddle:
			return new Rect(91f, 0f, 92f, 92f);
		case ItemType.PowerSpecial:
			return new Rect(184f, 0f, 92f, 92f);
		case ItemType.FragGrenade:
			return new Rect(276f, 0f, 92f, 92f);
		case ItemType.StormGrenade:
			return new Rect(0f, 92f, 92f, 92f);
		case ItemType.HpSmall:
			return new Rect(91f, 92f, 92f, 92f);
		case ItemType.HpMiddle:
			return new Rect(184f, 92f, 92f, 92f);
		case ItemType.HpLarge:
			return new Rect(276f, 92f, 92f, 92f);
		case ItemType.Shield:
			return new Rect(0f, 184f, 92f, 92f);
		case ItemType.Doping:
			return new Rect(91f, 184f, 92f, 92f);
		case ItemType.NuclearCocacola:
			return new Rect(184f, 184f, 92f, 92f);
		case ItemType.Pacemaker:
			return new Rect(276f, 184f, 92f, 92f);
		case ItemType.Defibrilator:
			return new Rect(0f, 276f, 92f, 92f);
		default:
			return new Rect(0f, 0f, 0f, 0f);
		}
	}

	public Rect GetBankDollorTexture(FixedConfig.IAPCfg iap_cfg)
	{
		Rect result = new Rect(0f, 0f, 0f, 0f);
		if (iap_cfg.iapID == "com.trinitigame.callofminibulletdudes.099cents2")
		{
			result = new Rect(482f, 240f, 30f, 36f);
		}
		else if (iap_cfg.iapID == "com.trinitigame.callofminibulletdudes.099cents1")
		{
			result = new Rect(17f, 479f, 100f, 33f);
		}
		else if (iap_cfg.iapID == "com.trinitigame.callofminibulletdudes.new499cents2")
		{
			result = new Rect(454f, 279f, 58f, 36f);
		}
		else if (iap_cfg.iapID == "com.trinitigame.callofminibulletdudes.new499cents1")
		{
			result = new Rect(0f, 445f, 120f, 32f);
		}
		else if (iap_cfg.iapID == "com.trinitigame.callofminibulletdudes.new999cents2")
		{
			result = new Rect(456f, 317f, 56f, 36f);
		}
		else if (iap_cfg.iapID == "com.trinitigame.callofminibulletdudes.new999cents1")
		{
			result = new Rect(119f, 480f, 119f, 32f);
		}
		else if (iap_cfg.iapID == "com.trinitigame.callofminibulletdudes.new1999cents2")
		{
			result = new Rect(436f, 354f, 76f, 36f);
		}
		else if (iap_cfg.iapID == "com.trinitigame.callofminibulletdudes.new1999cents1")
		{
			result = new Rect(242f, 480f, 138f, 32f);
		}
		else if (iap_cfg.iapID == "com.trinitigame.callofminibulletdudes.new4999cents2")
		{
			result = new Rect(430f, 392f, 82f, 36f);
		}
		else if (iap_cfg.iapID == "com.trinitigame.callofminibulletdudes.new4999cents1")
		{
			result = new Rect(123f, 446f, 143f, 32f);
		}
		return result;
	}

	public void BuyIAPOKEvent(int iapStatus)
	{
		if (iapStatus > 0)
		{
			FixedConfig.IAPCfg iAPCfg = ConfigManager.Instance().GetFixedConfig().GetIAPCfg(m_iapBuyIndex);
			if (iAPCfg == null)
			{
				Debug.LogError("Buy IAP OK, but iap_cfg is null!!! m_iapBuyIndex is " + m_iapBuyIndex);
			}
			Debug.Log("Buy IAP OK - " + iAPCfg.iapDollor + " | " + iAPCfg.gameGold + " | " + iAPCfg.gameDollor);
			GameCollectionInfoManager.Instance().GetCurrentInfo().AddIAPInfo(iAPCfg.iapID, 1);
			if (iAPCfg.gameGold > 0f)
			{
				GameApp.GetInstance().GetGameState().AddGold(Mathf.FloorToInt(iAPCfg.gameGold));
			}
			if (iAPCfg.gameDollor > 0f)
			{
				GameApp.GetInstance().GetGameState().AddDollor(Mathf.FloorToInt(iAPCfg.gameDollor));
			}
			gameState.AddIapTimes();
			gameState.AddIapSpendDollor(iAPCfg.iapDollor);
			gameState.AddDailyCollectionInfo(0, iAPCfg.iapDollor, 0, 0);
			if (iAPCfg.iapID == "com.trinitigame.callofminibulletdudes.new099cents1")
			{
				gameState.NoviceGiftBagThird++;
				GameCollectionInfoManager.Instance().GetCurrentInfo().BuyNoviceGiftBag();
				GameApp.GetInstance().Save();
				SetupBankShopUI(true);
			}
		}
		else
		{
			Debug.Log("Buy IAP ERROR ERROR!!~~ status:" + iapStatus);
		}
		SetupLoadingUI(false, string.Empty);
		SetupAroundUI(true);
		m_iapBuyIndex = -1;
	}

	public void IAPInitTimeOut()
	{
		SetupLoadingUI(false, string.Empty);
	}

	public void RelevanceAmazonBasicEvent()
	{
		Debug.Log("RelevanceAmazonBasicEvent() - " + gameState.m_goIAPEventListener == null);
		if (gameState.m_goIAPEventListener.GetComponent(typeof(AmazonIAPEventListener)) != null)
		{
			AmazonIAPEventListener amazonIAPEventListener = gameState.m_goIAPEventListener.GetComponent(typeof(AmazonIAPEventListener)) as AmazonIAPEventListener;
			amazonIAPEventListener._itemDataRequestFailedEvent = itemDataRequestFailedEvent;
			amazonIAPEventListener._itemDataRequestFinishedEvent = itemDataRequestFinishedEvent;
			amazonIAPEventListener._purchaseFailedEvent = purchaseFailedEvent;
			amazonIAPEventListener._purchaseSuccessfulEvent = purchaseSuccessfulEvent;
			amazonIAPEventListener._purchaseUpdatesRequestFailedEvent = purchaseUpdatesRequestFailedEvent;
			amazonIAPEventListener._purchaseUpdatesRequestSuccessfulEvent = purchaseUpdatesRequestSuccessfulEvent;
			Debug.LogWarning("AmazonIAP | AmazonIAPEvent Delegate OK");
		}
		else
		{
			Debug.LogWarning("Get AmazonIAPEventListener Error");
		}
		if (!gameState.m_bIAPIsInitOK)
		{
			string[] array = new string[ConfigManager.Instance().GetFixedConfig().iapCfgs.Count];
			for (int i = 0; i < array.Length; i++)
			{
				FixedConfig.IAPCfg iAPCfg = ConfigManager.Instance().GetFixedConfig().GetIAPCfg(i);
				array[i] = iAPCfg.iapID;
			}
			AmazonIAP.initiateItemDataRequest(array);
			Debug.LogWarning("AmazonIAP | initiateItemDataRequest Count:" + array.Length);
			SetupLoadingUI(true, string.Empty);
			TimeManager.Instance.Init(1, 20f, IAPInitTimeOut, null, "Init IAP");
		}
		Debug.Log("RelevanceAmazonBasicEvent() - END");
	}

	public void RelevanceIABBasicEvent()
	{
		if (gameState.m_goIAPEventListener.GetComponent(typeof(IABAndroidEventListener)) != null)
		{
			IABAndroidEventListener iABAndroidEventListener = gameState.m_goIAPEventListener.GetComponent(typeof(IABAndroidEventListener)) as IABAndroidEventListener;
			iABAndroidEventListener._billingSupportedEvent = billingSupportedEvent;
			iABAndroidEventListener._purchaseSucceededEvent = purchaseSuccessfulEvent;
			iABAndroidEventListener._purchaseFailedEvent = purchaseFailedEvent;
			Debug.LogWarning("AmazonIAP | IABAndroidEvent Delegate OK");
		}
		else
		{
			Debug.LogWarning("Get IABAndroidEventListener Error");
		}
		if (!gameState.m_bIAPIsInitOK)
		{
			IABAndroid.init("MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAvfxRqpA+fjKm64VbNaXM6offkWUUsgCRzZlJFJrjZD5MTcX2p2/nfyOYiNDAh9qrS6hoS7MfIvPYirc38oerql/die8eIsW5JtBkeVt2te9+ZCc2BjmOr2b3g+xirbE1bkReJP5JDARHColJA7lQ/6o4J8rvv9L1rGcYynrWeSdTegeBDRkuMPQjgNArMXzkw7hITPdLXhQtBgnn62tV7zvguxKMuYoqzmXpyMsSyyAFVGDQAvI7ITKXvRR+0LL2ybjmP0+0kwLu7NL+nshBm8msjHbCqclsiOwcEkaMFk/Jgqg8B2MLeL7Ff2PJIJA023FnMfPgzNJIde0hj20j6wIDAQAB");
			Debug.LogWarning("IABAndroid | initMIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAvfxRqpA+fjKm64VbNaXM6offkWUUsgCRzZlJFJrjZD5MTcX2p2/nfyOYiNDAh9qrS6hoS7MfIvPYirc38oerql/die8eIsW5JtBkeVt2te9+ZCc2BjmOr2b3g+xirbE1bkReJP5JDARHColJA7lQ/6o4J8rvv9L1rGcYynrWeSdTegeBDRkuMPQjgNArMXzkw7hITPdLXhQtBgnn62tV7zvguxKMuYoqzmXpyMsSyyAFVGDQAvI7ITKXvRR+0LL2ybjmP0+0kwLu7NL+nshBm8msjHbCqclsiOwcEkaMFk/Jgqg8B2MLeL7Ff2PJIJA023FnMfPgzNJIde0hj20j6wIDAQAB");
			SetupLoadingUI(true, string.Empty);
			TimeManager.Instance.Init(1, 20f, IAPInitTimeOut, null, "Init IAP");
		}
	}

	public void itemDataRequestFailedEvent()
	{
		SetupLoadingUI(false, string.Empty);
		gameState.m_bIAPIsInitOK = false;
	}

	public void itemDataRequestFinishedEvent(List<string> unavailableSkus, List<AmazonItem> availableItems)
	{
		SetupLoadingUI(false, string.Empty);
		gameState.m_bIAPIsInitOK = true;
	}

	public void purchaseFailedEvent(string reason)
	{
		BuyIAPOKEvent(-1);
	}

	public void purchaseSuccessfulEvent(AmazonReceipt receipt)
	{
		BuyIAPOKEvent(1);
	}

	public void billingSupportedEvent(bool isSupported)
	{
		SetupLoadingUI(false, string.Empty);
		gameState.m_bIAPIsInitOK = isSupported;
	}

	public void purchaseSuccessfulEvent(string id)
	{
		BuyIAPOKEvent(1);
	}

	public void purchaseUpdatesRequestFailedEvent()
	{
	}

	public void purchaseUpdatesRequestSuccessfulEvent(List<string> revokedSkus, List<AmazonReceipt> receipts)
	{
	}

	public Rect GetBankIconTexture(FixedConfig.IAPCfg iap_cfg)
	{
		Rect result = new Rect(0f, 0f, 0f, 0f);
		if (iap_cfg.iapID == "com.trinitigame.callofminibulletdudes.099cents2")
		{
			result = new Rect(242f, 136f, 103f, 96f);
		}
		else if (iap_cfg.iapID == "com.trinitigame.callofminibulletdudes.099cents1")
		{
			result = new Rect(325f, 249f, 113f, 83f);
		}
		else if (iap_cfg.iapID == "com.trinitigame.callofminibulletdudes.new499cents2")
		{
			result = new Rect(205f, 243f, 120f, 91f);
		}
		else if (iap_cfg.iapID == "com.trinitigame.callofminibulletdudes.new499cents1")
		{
			result = new Rect(303f, 334f, 111f, 103f);
		}
		else if (iap_cfg.iapID == "com.trinitigame.callofminibulletdudes.new999cents2")
		{
			result = new Rect(348f, 146f, 164f, 93f);
		}
		else if (iap_cfg.iapID == "com.trinitigame.callofminibulletdudes.new999cents1")
		{
			result = new Rect(192f, 334f, 111f, 103f);
		}
		else if (iap_cfg.iapID == "com.trinitigame.callofminibulletdudes.new1999cents2")
		{
			result = new Rect(358f, 0f, 154f, 147f);
		}
		else if (iap_cfg.iapID == "com.trinitigame.callofminibulletdudes.new1999cents1")
		{
			result = new Rect(199f, 1f, 159f, 135f);
		}
		else if (iap_cfg.iapID == "com.trinitigame.callofminibulletdudes.new4999cents2")
		{
			result = new Rect(0f, 0f, 199f, 139f);
		}
		else if (iap_cfg.iapID == "com.trinitigame.callofminibulletdudes.new4999cents1")
		{
			result = new Rect(0f, 328f, 192f, 118f);
		}
		else if (iap_cfg.iapID == "com.trinitigame.callofminibulletdudes.new099cents1")
		{
			result = new Rect(242f, 136f, 103f, 96f);
		}
		else if (iap_cfg.iapID == "twitter")
		{
			result = new Rect(0f, 0f, 199f, 139f);
		}
		return result;
	}

	public static Rect[] GetJunjieIconTexture(int player_level)
	{
		Rect[] array = new Rect[3]
		{
			default(Rect),
			default(Rect),
			default(Rect)
		};
		if (player_level >= 3 && player_level < 10)
		{
			array[0] = new Rect(0f, 0f, 75f, 52f);
			array[1] = new Rect(75f, 0f, 75f, 52f);
			array[2] = new Rect(0f, 0f, 75f, 52f);
		}
		else if (player_level >= 10 && player_level < 20)
		{
			array[0] = new Rect(150f, 0f, 75f, 52f);
			array[1] = new Rect(225f, 0f, 75f, 52f);
			array[2] = new Rect(150f, 0f, 75f, 52f);
		}
		else if (player_level >= 20 && player_level < 30)
		{
			array[0] = new Rect(300f, 0f, 75f, 52f);
			array[1] = new Rect(375f, 0f, 75f, 52f);
			array[2] = new Rect(300f, 0f, 75f, 52f);
		}
		else if (player_level >= 30 && player_level < 40)
		{
			array[0] = new Rect(0f, 52f, 75f, 52f);
			array[1] = new Rect(75f, 52f, 75f, 52f);
			array[2] = new Rect(0f, 52f, 75f, 52f);
		}
		else if (player_level >= 40 && player_level < 50)
		{
			array[0] = new Rect(150f, 52f, 75f, 52f);
			array[1] = new Rect(225f, 52f, 75f, 52f);
			array[2] = new Rect(150f, 52f, 75f, 52f);
		}
		else if (player_level >= 50 && player_level < 60)
		{
			array[0] = new Rect(300f, 52f, 75f, 52f);
			array[1] = new Rect(375f, 52f, 75f, 52f);
			array[2] = new Rect(300f, 52f, 75f, 52f);
		}
		else if (player_level >= 60 && player_level < 70)
		{
			array[0] = new Rect(0f, 104f, 75f, 52f);
			array[1] = new Rect(75f, 104f, 75f, 52f);
			array[2] = new Rect(0f, 104f, 75f, 52f);
		}
		else if (player_level >= 70 && player_level < 80)
		{
			array[0] = new Rect(150f, 104f, 75f, 52f);
			array[1] = new Rect(225f, 104f, 75f, 52f);
			array[2] = new Rect(150f, 104f, 75f, 52f);
		}
		else if (player_level >= 80 && player_level < 90)
		{
			array[0] = new Rect(300f, 104f, 75f, 52f);
			array[1] = new Rect(375f, 104f, 75f, 52f);
			array[2] = new Rect(300f, 104f, 75f, 52f);
		}
		else if (player_level >= 90 && player_level < 100)
		{
			array[0] = new Rect(0f, 156f, 75f, 52f);
			array[1] = new Rect(75f, 156f, 75f, 52f);
			array[2] = new Rect(0f, 156f, 75f, 52f);
		}
		else if (player_level >= 100 && player_level < 115)
		{
			array[0] = new Rect(150f, 156f, 75f, 52f);
			array[1] = new Rect(225f, 156f, 75f, 52f);
			array[2] = new Rect(150f, 156f, 75f, 52f);
		}
		else if (player_level >= 115 && player_level < 130)
		{
			array[0] = new Rect(300f, 156f, 75f, 52f);
			array[1] = new Rect(375f, 156f, 75f, 52f);
			array[2] = new Rect(300f, 156f, 75f, 52f);
		}
		else if (player_level >= 130 && player_level < 145)
		{
			array[0] = new Rect(0f, 208f, 75f, 52f);
			array[1] = new Rect(75f, 208f, 75f, 52f);
			array[2] = new Rect(0f, 208f, 75f, 52f);
		}
		else if (player_level >= 145 && player_level < 160)
		{
			array[0] = new Rect(150f, 208f, 75f, 52f);
			array[1] = new Rect(225f, 208f, 75f, 52f);
			array[2] = new Rect(150f, 208f, 75f, 52f);
		}
		else if (player_level >= 160 && player_level < 175)
		{
			array[0] = new Rect(300f, 208f, 75f, 52f);
			array[1] = new Rect(375f, 208f, 75f, 52f);
			array[2] = new Rect(300f, 208f, 75f, 52f);
		}
		else if (player_level >= 175 && player_level < 190)
		{
			array[0] = new Rect(0f, 260f, 75f, 52f);
			array[1] = new Rect(75f, 260f, 75f, 52f);
			array[2] = new Rect(0f, 260f, 75f, 52f);
		}
		else if (player_level >= 190 && player_level < 205)
		{
			array[0] = new Rect(150f, 260f, 75f, 52f);
			array[1] = new Rect(225f, 260f, 75f, 52f);
			array[2] = new Rect(150f, 260f, 75f, 52f);
		}
		else if (player_level >= 205 && player_level < 230)
		{
			array[0] = new Rect(300f, 260f, 75f, 52f);
			array[1] = new Rect(375f, 260f, 75f, 52f);
			array[2] = new Rect(300f, 260f, 75f, 52f);
		}
		else if (player_level >= 230)
		{
			array[0] = new Rect(0f, 312f, 75f, 52f);
			array[1] = new Rect(75f, 312f, 75f, 52f);
			array[2] = new Rect(0f, 312f, 75f, 52f);
		}
		return array;
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
