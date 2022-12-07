using System;
using System.Collections.Generic;
using TNetSdk;
using UnityEngine;
using Zombie3D;

public class NExchangeUIScript : MonoBehaviour, UIHandler
{
	public enum Controls
	{
		kIDLevels = 6000,
		kIDFriends = 6001,
		kIDShop = 6002,
		kIDOptions = 6003,
		kIDCup = 6004,
		kIDTopList = 6005,
		kIDGlobalBank = 6006,
		kIDExchangeUINext = 6007,
		kIDBattleReportListView = 6008,
		kIDExchangeUpgrade3Days = 6009,
		kIDExchangeUpgrade7Days = 6010,
		kIDExchangeUpgrade20Days = 6011,
		kIDExchangeUpgradeNotHaveEnoughCrystalOK = 6012,
		kIDExchangeUpgrade3DaysYes = 6013,
		kIDExchangeUpgrade7DaysYes = 6014,
		kIDExchangeUpgrade20DaysYes = 6015,
		kIDExchangeUpgradeNo = 6016,
		kIDLast = 6017
	}

	private UIManager m_UIManager;

	private string m_ui_material_path;

	protected Material m_MatCommonBg;

	protected Material m_MatNExchangeUI;

	protected Material m_MatDialog01;

	public uiGroup m_uiGroup;

	public uiGroup m_uiHintDialog;

	public uiGroup m_uiTweetUI;

	protected GameState gameState;

	protected float lastUpdateTime;

	private bool m_bExchange3xLoot;

	private UIScrollView m_FriendsPageView;

	private UIScrollBar m_FriendsPageViewScrollBar;

	private string m_strTweetText = string.Empty;

	private bool m_bTweetSend;

	private bool m_bExchangeCollected;

	private void Start()
	{
		Time.timeScale = 1f;
		lastUpdateTime = Time.time;
		gameState = GameApp.GetInstance().GetGameState();
		m_UIManager = base.gameObject.AddComponent<UIManager>() as UIManager;
		m_UIManager.SetParameter(8, 3, false);
		m_UIManager.SetUIHandler(this);
		UIUtils.BuildIpone5Frame(m_UIManager);
		m_MatCommonBg = LoadUIMaterial("Zombie3D/UI/Materials/CommonBgUI");
		m_MatNExchangeUI = LoadUIMaterial("Zombie3D/UI/Materials/NExchangeUI");
		m_MatDialog01 = LoadUIMaterial("Zombie3D/UI/Materials/Dialog01");
		Resources.UnloadUnusedAssets();
		gameState.m_BattleCount++;
		if (gameState.m_eGameMode.m_ePlayMode == GameState.NetworkGameMode.PlayMode.E_PVE_BossRush)
		{
			SetupPVEExchangeUI(true);
		}
		else
		{
			SetupExchangeUI(true);
		}
		SetupAroundUI(true, m_uiGroup);
		GameCollectionInfoManager.Instance().GetCurrentInfo().AddGameTime((int)gameState.m_BattleTime);
		GameCollectionInfoManager.Instance().GetCurrentInfo().AddUIEnterLog(GameCollectionInfo.enUIEnterIndex.ExchangeUI);
		gameState.AddDailyCollectionInfo(0, 0f, 0, (int)gameState.m_BattleTime);
		gameState.SaveExchangeInfo();
		gameState.m_iPlayGameTimes++;
		if (gameState.m_iPlayGameTimes % 5 == 0)
		{
			OpenClickPlugin.Show(true);
			ChartBoostAndroid.showInterstitial(null);
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
		if (!(Time.time - lastUpdateTime < 0.001f))
		{
			float num = Time.time - lastUpdateTime;
			lastUpdateTime = Time.time;
		}
	}

	public void HandleEvent(UIControl control, int command, float wparam, float lparam)
	{
		if ((control.GetType() == typeof(UIClickButton) || control.GetType() == typeof(UISelectButton)) && GameApp.GetInstance().GetGameState().SoundOn)
		{
			SceneUIManager.Instance().PlayClickAudio();
		}
		if (control.Id == 6007)
		{
			if (SmartFoxConnection.IsInitialized)
			{
				SmartFoxConnection.Connection.Send(new LeaveRoomRequest());
			}
			GameSetup.Instance.UnsubscribeDelegates();
			SmartFoxConnection.DisConnect();
			if (gameState.m_eGameMode.m_ePlayMode == GameState.NetworkGameMode.PlayMode.E_PVE_BossRush)
			{
				SceneUIManager.Instance().ChangeSceneUI(SceneUIManager.SceneUI.ChoosePointsUI);
			}
			else
			{
				SceneUIManager.Instance().ChangeSceneUI(SceneUIManager.SceneUI.NNetworkUI);
			}
		}
		else if (control.Id == 6009)
		{
			if (m_uiHintDialog == null)
			{
				if (gameState.HaveEnoughDollor(5))
				{
					SetupHintDialog(true, 0, 6013, 6016, "Do you want to spend 5 Crystals to get pumped up for 72 hours?");
				}
				else
				{
					SetupHintDialog(true, 6016, 0, 0, "Insufficient crystals! Visit the bank now to get more.");
				}
			}
		}
		else if (control.Id == 6010)
		{
			if (m_uiHintDialog == null)
			{
				if (gameState.HaveEnoughDollor(8))
				{
					SetupHintDialog(true, 0, 6014, 6016, "Do you want to spend 8 Crystals to get pumped up for 192 hours?");
				}
				else
				{
					SetupHintDialog(true, 6016, 0, 0, "Insufficient crystals! Visit the bank now to get more.");
				}
			}
		}
		else if (control.Id == 6011)
		{
			if (m_uiHintDialog == null)
			{
				if (gameState.HaveEnoughDollor(15))
				{
					SetupHintDialog(true, 0, 6015, 6016, "Do you want to spend 15 Crystals to get pumped up for 480 hours?");
				}
				else
				{
					SetupHintDialog(true, 6016, 0, 0, "Insufficient crystals! Visit the bank now to get more.");
				}
			}
		}
		else if (control.Id == 6013)
		{
			gameState.AddExchangeUpgradeTime(259199L);
			gameState.LoseDollor(5);
			GameApp.GetInstance().Save();
			SetupHintDialog(false, 0, 0, 0, string.Empty);
			SetupExchangeUI(true);
		}
		else if (control.Id == 6014)
		{
			gameState.AddExchangeUpgradeTime(604799L);
			gameState.LoseDollor(8);
			GameApp.GetInstance().Save();
			SetupHintDialog(false, 0, 0, 0, string.Empty);
			SetupExchangeUI(true);
		}
		else if (control.Id == 6015)
		{
			gameState.AddExchangeUpgradeTime(1727999L);
			gameState.LoseDollor(15);
			GameApp.GetInstance().Save();
			SetupHintDialog(false, 0, 0, 0, string.Empty);
			SetupExchangeUI(true);
		}
		else if (control.Id == 6016)
		{
			SetupHintDialog(false, 0, 0, 0, string.Empty);
		}
		else if (control.Id != 6006 && control.Id != 6017)
		{
		}
	}

	public void SetupAroundUI(bool bShow, uiGroup group)
	{
		UIImage control = UIUtils.BuildImage(0, new Rect(0f, 572f, 960f, 68f), m_MatCommonBg, new Rect(0f, 640f, 960f, 68f), new Vector2(960f, 68f));
		group.Add(control);
		control = UIUtils.BuildImage(0, new Rect(0f, 0f, 62f, 572f), m_MatCommonBg, new Rect(960f, 0f, 62f, 572f), new Vector2(62f, 572f));
		group.Add(control);
		control = UIUtils.BuildImage(0, new Rect(630f, 242f, 572f, 88f), m_MatCommonBg, new Rect(0f, 710f, 572f, 88f), new Vector2(572f, 88f));
		control.Rect = AutoUIResolution.ToShiftToRight(AutoUI.AutoRect(new Rect(872f, 0f, 88f, 572f)), 2);
		control.SetRotation((float)Math.PI / 2f);
		group.Add(control);
		float playerExpNextLevelPercent = GameApp.GetInstance().GetGameState().GetPlayerExpNextLevelPercent();
		UIProgressBarProgressive control2 = UIUtils.BuildUIProgressBarRounded(0, new Rect(71f, 593f, 174f, 20f), m_MatCommonBg, m_MatCommonBg, new Rect(398f, 708f, 174f, 20f), new Rect(572f, 708f, 174f, 20f), playerExpNextLevelPercent);
		group.Add(control2);
		UIText uIText = UIUtils.BuildUIText(0, new Rect(5f, 600f, 145f, 30f), UIText.enAlignStyle.left);
		uIText.Set("Zombie3D/Font/037-CAI978-22", "LV " + GameApp.GetInstance().GetGameState().GetPlayerLevel(), new Color(0.81960785f, 0.5294118f, 2f / 51f, 1f));
		group.Add(uIText);
		control = UIUtils.BuildImage(0, new Rect(341f, 602f, 50f, 32f), m_MatCommonBg, new Rect(967f, 591f, 50f, 32f), new Vector2(50f, 32f));
		group.Add(control);
		uIText = UIUtils.BuildUIText(0, new Rect(394f, 602f, 145f, 30f), UIText.enAlignStyle.left);
		uIText.Set("Zombie3D/Font/037-CAI978-22", GameApp.GetInstance().GetGameState().gold.ToString(), new Color(0.81960785f, 0.5294118f, 2f / 51f, 1f));
		group.Add(uIText);
		control = UIUtils.BuildImage(0, new Rect(579f, 602f, 50f, 36f), m_MatCommonBg, new Rect(967f, 631f, 50f, 36f), new Vector2(50f, 36f));
		group.Add(control);
		uIText = UIUtils.BuildUIText(0, new Rect(627f, 602f, 145f, 30f), UIText.enAlignStyle.left);
		uIText.Set("Zombie3D/Font/037-CAI978-22", GameApp.GetInstance().GetGameState().dollor.ToString(), new Color(0.81960785f, 0.5294118f, 2f / 51f, 1f));
		group.Add(uIText);
	}

	public void SetupExchangeUI(bool bShow)
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
		Material material = LoadUIMaterial("Zombie3D/UI/Materials/ExchangeUI");
		Material material2 = LoadUIMaterial("Zombie3D/UI/Materials/FriendsUI");
		Material mat = LoadUIMaterial("Zombie3D/UI/Materials/AvatarIconsHead");
		Material mat2 = LoadUIMaterial("Zombie3D/UI/Materials/PowerUPSIcons");
		Resources.UnloadUnusedAssets();
		m_uiGroup = new uiGroup(m_UIManager);
		UIClickButton uIClickButton = null;
		UIImage uIImage = null;
		UIText uIText = null;
		uIImage = UIUtils.BuildImage(0, new Rect(0f, 0f, 960f, 640f), m_MatCommonBg, new Rect(0f, 0f, 960f, 640f), new Vector2(960f, 640f));
		m_uiGroup.Add(uIImage);
		SetupAroundUI(true, m_uiGroup);
		uIImage = UIUtils.BuildImage(0, new Rect(50f, 90f, 864f, 410f), m_MatNExchangeUI, new Rect(2f, 2f, 864f, 410f), new Vector2(864f, 410f));
		m_uiGroup.Add(uIImage);
		Rect[] array = new Rect[4]
		{
			new Rect(187f, 435f, 130f, 30f),
			new Rect(323f, 435f, 110f, 30f),
			new Rect(436f, 435f, 120f, 30f),
			new Rect(573f, 435f, 250f, 30f)
		};
		string[] array2 = new string[4] { "NAME", "KILLS", "DEATHS", "REWARD" };
		for (int i = 0; i < array.Length; i++)
		{
			uIText = UIUtils.BuildUIText(0, array[i], UIText.enAlignStyle.left);
			uIText.Set("Zombie3D/Font/037-CAI978-18", array2[i], Constant.TextCommonColor);
			m_uiGroup.Add(uIText);
		}
		if (m_FriendsPageViewScrollBar != null)
		{
			m_uiGroup.Remove(m_FriendsPageViewScrollBar);
		}
		m_FriendsPageViewScrollBar = new UIScrollBar();
		m_FriendsPageViewScrollBar.ScrollOri = UIScrollBar.ScrollOrientation.Vertical;
		m_FriendsPageViewScrollBar.Rect = AutoUI.AutoRect(new Rect(860f, 150f, 20f, 274f));
		m_FriendsPageViewScrollBar.SetScrollBarTexture(material2, AutoUI.AutoRect(new Rect(564f, 0f, 20f, 274f)), material2, AutoUI.AutoRect(new Rect(564f, 274f, 20f, 86f)));
		m_FriendsPageViewScrollBar.SetSliderSize(AutoUI.AutoSize(new Vector2(20f, 86f)));
		m_FriendsPageViewScrollBar.SetScrollPercent(0f);
		m_uiGroup.Add(m_FriendsPageViewScrollBar);
		float num = 0f;
		int num2 = 0;
		if (m_FriendsPageView != null)
		{
			num = m_FriendsPageView.ScrollPosV;
			num2 = m_FriendsPageView.GetControlsCount();
			m_uiGroup.Remove(m_FriendsPageView);
		}
		m_FriendsPageView = new UIScrollView();
		m_FriendsPageView.SetMoveParam(AutoUI.AutoRect(new Rect(71f, 124f, 800f, 260f)), AutoUI.AutoDistance(20f), AutoUI.AutoDistance(20f));
		m_FriendsPageView.Id = 6008;
		m_FriendsPageView.Rect = AutoUI.AutoRect(new Rect(71f, 124f, 754f, 260f));
		m_FriendsPageView.ScrollOri = UIScrollView.ScrollOrientation.Vertical;
		m_FriendsPageView.ListOri = UIScrollView.ListOrientation.Vertical;
		m_FriendsPageView.ItemSpacingV = AutoUI.AutoDistance(4f);
		m_FriendsPageView.ItemSpacingH = AutoUI.AutoDistance(0f);
		m_FriendsPageView.SetClip(AutoUI.AutoRect(new Rect(71f, 125f, 800f, 300f)));
		m_FriendsPageView.Bounds = AutoUI.AutoRect(new Rect(71f, 125f, 800f, 300f));
		m_FriendsPageView.ScrollBar = m_FriendsPageViewScrollBar;
		m_uiGroup.Add(m_FriendsPageView);
		Rect[] array3 = new Rect[6]
		{
			new Rect(2f, 415f, 754f, 66f),
			new Rect(2f, 484f, 754f, 66f),
			new Rect(2f, 553f, 754f, 66f),
			new Rect(2f, 622f, 754f, 66f),
			new Rect(2f, 691f, 754f, 66f),
			new Rect(2f, 760f, 754f, 66f)
		};
		int num3 = 0;
		foreach (int item in PlayerManager.Instance.SortPlaersStatistics())
		{
			KeyValuePair<int, GameState.NetworkGameMode.NetworkPlayerStatistics> keyValuePair = new KeyValuePair<int, GameState.NetworkGameMode.NetworkPlayerStatistics>(item, gameState.GetPlayerStatistics(item));
			UIGroupControl uIGroupControl = UIUtils.BuildUIGroupControl(0, new Rect(0f, 0f, 800f, 70f));
			if (keyValuePair.Key == GameSetup.Instance.MineUser.Id)
			{
				uIImage = UIUtils.BuildImage(0, new Rect(18f, -2f, 758f, 67f), m_MatNExchangeUI, new Rect(15f, 937f, 758f, 67f), new Vector2(758f, 67f));
				uIGroupControl.Add(uIImage);
				if (gameState.m_eGameMode.m_eCooperaMode == GameState.NetworkGameMode.NetworkCooperationMode.E_Simple)
				{
					uIImage = UIUtils.BuildImage(0, new Rect(20f, 0f, 754f, 65f), m_MatNExchangeUI, array3[1], new Vector2(754f, 65f));
				}
				else if (keyValuePair.Value.m_iNGroup == 1)
				{
					uIImage = UIUtils.BuildImage(0, new Rect(20f, 0f, 754f, 65f), m_MatNExchangeUI, array3[2], new Vector2(754f, 65f));
				}
				else if (keyValuePair.Value.m_iNGroup == 2)
				{
					uIImage = UIUtils.BuildImage(0, new Rect(20f, 0f, 754f, 65f), m_MatNExchangeUI, array3[3], new Vector2(754f, 65f));
				}
				AddReward(keyValuePair.Value.m_lsReward);
				gameState.AddGold(keyValuePair.Value.m_iKillCount * 100);
			}
			else if (gameState.m_eGameMode.m_eCooperaMode == GameState.NetworkGameMode.NetworkCooperationMode.E_Simple)
			{
				uIImage = UIUtils.BuildImage(0, new Rect(20f, 0f, 754f, 65f), m_MatNExchangeUI, array3[0], new Vector2(754f, 65f));
			}
			else if (keyValuePair.Value.m_iNGroup == 1)
			{
				uIImage = UIUtils.BuildImage(0, new Rect(20f, 0f, 754f, 65f), m_MatNExchangeUI, array3[5], new Vector2(754f, 65f));
			}
			else if (keyValuePair.Value.m_iNGroup == 2)
			{
				uIImage = UIUtils.BuildImage(0, new Rect(20f, 0f, 754f, 65f), m_MatNExchangeUI, array3[4], new Vector2(754f, 65f));
			}
			uIGroupControl.Add(uIImage);
			bool bIsOline = keyValuePair.Value.m_bIsOline;
			float num4 = 70f;
			Vector2 vector = new Vector2(63f, 38f);
			Rect avatarIconHeadTexture = ShopUIScript.GetAvatarIconHeadTexture((Avatar.AvatarSuiteType)keyValuePair.Value.m_iHeadAvatarID);
			Vector2 rect_size = new Vector2(70f, 70f);
			if (avatarIconHeadTexture.width > avatarIconHeadTexture.height)
			{
				if (avatarIconHeadTexture.width > rect_size.x)
				{
					rect_size = new Vector2(rect_size.x, rect_size.x / avatarIconHeadTexture.width * avatarIconHeadTexture.height);
				}
			}
			else if (avatarIconHeadTexture.height > rect_size.y)
			{
				rect_size = new Vector2(rect_size.y / avatarIconHeadTexture.height * avatarIconHeadTexture.width, rect_size.y);
			}
			uIImage = UIUtils.BuildImage(0, new Rect(vector.x - rect_size.x / 2f, vector.y - rect_size.y / 2f, rect_size.x, rect_size.y), mat, avatarIconHeadTexture, rect_size);
			uIImage.CatchMessage = false;
			uIGroupControl.Add(uIImage);
			uIText = UIUtils.BuildUIText(0, new Rect(array[0].x - num4, 25f, array[0].width, 25f), UIText.enAlignStyle.left);
			uIText.Set("Zombie3D/Font/037-CAI978-18", gameState.GetNName(keyValuePair.Value.m_strName), Color.white);
			uIGroupControl.Add(uIText);
			uIText = UIUtils.BuildUIText(0, new Rect(array[1].x - num4, 25f, array[1].width, 25f), UIText.enAlignStyle.left);
			uIText.Set("Zombie3D/Font/037-CAI978-18", keyValuePair.Value.m_iKillCount.ToString(), Color.white);
			uIGroupControl.Add(uIText);
			uIText = UIUtils.BuildUIText(0, new Rect(array[2].x - num4, 25f, array[2].width, 25f), UIText.enAlignStyle.left);
			uIText.Set("Zombie3D/Font/037-CAI978-18", keyValuePair.Value.m_iDeathCount.ToString(), Color.white);
			uIGroupControl.Add(uIText);
			int num5 = 0;
			if (bIsOline)
			{
				if (keyValuePair.Value.m_bIsBestKiller)
				{
				}
				if (keyValuePair.Value.m_bIsWinner)
				{
				}
				if (keyValuePair.Value.m_lsReward.Count > 0 || keyValuePair.Value.m_iKillCount > 0)
				{
					num5 = 0;
					foreach (KeyValuePair<int, int> item2 in keyValuePair.Value.m_lsReward)
					{
						Debug.LogWarning(item2.Key + "|" + item2.Value);
						float num6 = array[3].x - num4 + 70f * (float)num5;
						if (item2.Key >= 0)
						{
							Rect powerUpsIconTexture = ShopUIScript.GetPowerUpsIconTexture((ItemType)item2.Key);
							uIImage = UIUtils.BuildImage(0, new Rect(num6, 14f, 45f, 45f), mat2, powerUpsIconTexture, new Vector2(45f, 45f));
							uIGroupControl.Add(uIImage);
						}
						else
						{
							Rect rcMat = new Rect(869f, 2f, 45f, 45f);
							uIImage = UIUtils.BuildImage(0, new Rect(num6, 4f, rcMat.width, rcMat.height), m_MatNExchangeUI, rcMat, new Vector2(rcMat.width, rcMat.height));
							uIGroupControl.Add(uIImage);
						}
						uIText = UIUtils.BuildUIText(0, new Rect(num6 + 45f, 45f, 25f, 25f), UIText.enAlignStyle.left);
						uIText.Set("Zombie3D/Font/037-CAI978-18", " X" + item2.Value, Color.white);
						uIGroupControl.Add(uIText);
						num5++;
					}
				}
			}
			else
			{
				float left = array[3].x + 110f;
				Rect rcMat2 = new Rect(882f, 48f, 66f, 35f);
				uIImage = UIUtils.BuildImage(0, new Rect(left, 15f, rcMat2.width, rcMat2.height), m_MatNExchangeUI, rcMat2, new Vector2(rcMat2.width, rcMat2.height));
				uIGroupControl.Add(uIImage);
			}
			if (keyValuePair.Value.m_iKillCount > 0 && bIsOline)
			{
				float num7 = array[3].x - num4 + 70f * (float)num5;
				Rect rcMat3 = new Rect(914f, 2f, 45f, 45f);
				uIImage = UIUtils.BuildImage(0, new Rect(num7, 4f, rcMat3.width, rcMat3.height), m_MatNExchangeUI, rcMat3, new Vector2(rcMat3.width, rcMat3.height));
				uIGroupControl.Add(uIImage);
				uIText = UIUtils.BuildUIText(0, new Rect(num7 + 45f, 45f, 25f, 25f), UIText.enAlignStyle.left);
				uIText.Set("Zombie3D/Font/037-CAI978-18", " X" + keyValuePair.Value.m_iKillCount * 100, Color.white);
				uIGroupControl.Add(uIText);
				num5++;
			}
			m_FriendsPageView.Add(uIGroupControl);
			num3++;
		}
		if (num > 0f)
		{
			float num8 = num * (float)num2 / (float)m_FriendsPageView.GetControlsCount();
			m_FriendsPageView.ScrollPosV = num8;
			m_FriendsPageViewScrollBar.SetScrollPercent(num8);
		}
		uIImage = UIUtils.BuildImage(0, new Rect(60f, 500f, 275f, 38f), m_MatNExchangeUI, new Rect(2f, 829f, 276f, 38f), new Vector2(276f, 38f));
		m_uiGroup.Add(uIImage);
		uIClickButton = UIUtils.BuildClickButton(6007, new Rect(690f, 20f, 188f, 68f), m_MatNExchangeUI, new Rect(759f, 415f, 188f, 68f), new Rect(759f, 486f, 188f, 68f), new Rect(759f, 415f, 188f, 68f), new Vector2(188f, 68f));
		m_uiGroup.Add(uIClickButton);
	}

	public void SetupPVEExchangeUI(bool bShow)
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
		Material material = LoadUIMaterial("Zombie3D/UI/Materials/ExchangeUI");
		Material material2 = LoadUIMaterial("Zombie3D/UI/Materials/FriendsUI");
		Material mat = LoadUIMaterial("Zombie3D/UI/Materials/AvatarIconsHead");
		Material mat2 = LoadUIMaterial("Zombie3D/UI/Materials/PowerUPSIcons");
		Resources.UnloadUnusedAssets();
		m_uiGroup = new uiGroup(m_UIManager);
		UIClickButton uIClickButton = null;
		UIImage uIImage = null;
		UIText uIText = null;
		uIImage = UIUtils.BuildImage(0, new Rect(0f, 0f, 960f, 640f), m_MatCommonBg, new Rect(0f, 0f, 960f, 640f), new Vector2(960f, 640f));
		m_uiGroup.Add(uIImage);
		SetupAroundUI(true, m_uiGroup);
		uIImage = UIUtils.BuildImage(0, new Rect(50f, 90f, 864f, 410f), m_MatNExchangeUI, new Rect(2f, 2f, 864f, 410f), new Vector2(864f, 410f));
		m_uiGroup.Add(uIImage);
		Rect[] array = new Rect[3]
		{
			new Rect(187f, 435f, 130f, 30f),
			new Rect(323f, 435f, 230f, 30f),
			new Rect(573f, 435f, 250f, 30f)
		};
		string[] array2 = new string[3] { "NAME", "DAMAGE TAKEN", "REWARD" };
		for (int i = 0; i < array.Length; i++)
		{
			uIText = UIUtils.BuildUIText(0, array[i], UIText.enAlignStyle.left);
			uIText.Set("Zombie3D/Font/037-CAI978-18", array2[i], Constant.TextCommonColor);
			m_uiGroup.Add(uIText);
		}
		if (m_FriendsPageViewScrollBar != null)
		{
			m_uiGroup.Remove(m_FriendsPageViewScrollBar);
		}
		m_FriendsPageViewScrollBar = new UIScrollBar();
		m_FriendsPageViewScrollBar.ScrollOri = UIScrollBar.ScrollOrientation.Vertical;
		m_FriendsPageViewScrollBar.Rect = AutoUI.AutoRect(new Rect(860f, 150f, 20f, 274f));
		m_FriendsPageViewScrollBar.SetScrollBarTexture(material2, AutoUI.AutoRect(new Rect(564f, 0f, 20f, 274f)), material2, AutoUI.AutoRect(new Rect(564f, 274f, 20f, 86f)));
		m_FriendsPageViewScrollBar.SetSliderSize(AutoUI.AutoSize(new Vector2(20f, 86f)));
		m_FriendsPageViewScrollBar.SetScrollPercent(0f);
		m_uiGroup.Add(m_FriendsPageViewScrollBar);
		float num = 0f;
		int num2 = 0;
		if (m_FriendsPageView != null)
		{
			num = m_FriendsPageView.ScrollPosV;
			num2 = m_FriendsPageView.GetControlsCount();
			m_uiGroup.Remove(m_FriendsPageView);
		}
		m_FriendsPageView = new UIScrollView();
		m_FriendsPageView.SetMoveParam(AutoUI.AutoRect(new Rect(71f, 124f, 800f, 260f)), AutoUI.AutoDistance(20f), AutoUI.AutoDistance(20f));
		m_FriendsPageView.Id = 6008;
		m_FriendsPageView.Rect = AutoUI.AutoRect(new Rect(71f, 124f, 754f, 260f));
		m_FriendsPageView.ScrollOri = UIScrollView.ScrollOrientation.Vertical;
		m_FriendsPageView.ListOri = UIScrollView.ListOrientation.Vertical;
		m_FriendsPageView.ItemSpacingV = AutoUI.AutoDistance(4f);
		m_FriendsPageView.ItemSpacingH = AutoUI.AutoDistance(0f);
		m_FriendsPageView.SetClip(AutoUI.AutoRect(new Rect(71f, 125f, 800f, 300f)));
		m_FriendsPageView.Bounds = AutoUI.AutoRect(new Rect(71f, 125f, 800f, 300f));
		m_FriendsPageView.ScrollBar = m_FriendsPageViewScrollBar;
		m_uiGroup.Add(m_FriendsPageView);
		Rect[] array3 = new Rect[6]
		{
			new Rect(2f, 415f, 754f, 66f),
			new Rect(2f, 484f, 754f, 66f),
			new Rect(2f, 553f, 754f, 66f),
			new Rect(2f, 622f, 754f, 66f),
			new Rect(2f, 691f, 754f, 66f),
			new Rect(2f, 760f, 754f, 66f)
		};
		int num3 = 0;
		foreach (int item in PlayerManager.Instance.SortPlaersStatistics())
		{
			KeyValuePair<int, GameState.NetworkGameMode.NetworkPlayerStatistics> keyValuePair = new KeyValuePair<int, GameState.NetworkGameMode.NetworkPlayerStatistics>(item, gameState.GetPlayerStatistics(item));
			UIGroupControl uIGroupControl = UIUtils.BuildUIGroupControl(0, new Rect(0f, 0f, 800f, 70f));
			if (keyValuePair.Key == GameSetup.Instance.MineUser.Id)
			{
				uIImage = UIUtils.BuildImage(0, new Rect(18f, -2f, 758f, 67f), m_MatNExchangeUI, new Rect(15f, 937f, 758f, 67f), new Vector2(758f, 67f));
				uIGroupControl.Add(uIImage);
				if (gameState.m_eGameMode.m_eCooperaMode == GameState.NetworkGameMode.NetworkCooperationMode.E_Simple)
				{
					uIImage = UIUtils.BuildImage(0, new Rect(20f, 0f, 754f, 65f), m_MatNExchangeUI, array3[1], new Vector2(754f, 65f));
				}
				else if (keyValuePair.Value.m_iNGroup == 1)
				{
					uIImage = UIUtils.BuildImage(0, new Rect(20f, 0f, 754f, 65f), m_MatNExchangeUI, array3[2], new Vector2(754f, 65f));
				}
				else if (keyValuePair.Value.m_iNGroup == 2)
				{
					uIImage = UIUtils.BuildImage(0, new Rect(20f, 0f, 754f, 65f), m_MatNExchangeUI, array3[3], new Vector2(754f, 65f));
				}
				if (keyValuePair.Value.LsRewardContainsKey(101).Value > 0)
				{
					gameState.AddGold(keyValuePair.Value.LsRewardContainsKey(101).Value);
				}
				if (keyValuePair.Value.LsRewardContainsKey(102).Value > 0)
				{
					gameState.AddGold(keyValuePair.Value.LsRewardContainsKey(102).Value);
				}
			}
			else if (gameState.m_eGameMode.m_eCooperaMode == GameState.NetworkGameMode.NetworkCooperationMode.E_Simple)
			{
				uIImage = UIUtils.BuildImage(0, new Rect(20f, 0f, 754f, 65f), m_MatNExchangeUI, array3[0], new Vector2(754f, 65f));
			}
			else if (keyValuePair.Value.m_iNGroup == 1)
			{
				uIImage = UIUtils.BuildImage(0, new Rect(20f, 0f, 754f, 65f), m_MatNExchangeUI, array3[5], new Vector2(754f, 65f));
			}
			else if (keyValuePair.Value.m_iNGroup == 2)
			{
				uIImage = UIUtils.BuildImage(0, new Rect(20f, 0f, 754f, 65f), m_MatNExchangeUI, array3[4], new Vector2(754f, 65f));
			}
			uIGroupControl.Add(uIImage);
			bool bIsOline = keyValuePair.Value.m_bIsOline;
			float num4 = 70f;
			Vector2 vector = new Vector2(63f, 38f);
			Rect avatarIconHeadTexture = ShopUIScript.GetAvatarIconHeadTexture((Avatar.AvatarSuiteType)keyValuePair.Value.m_iHeadAvatarID);
			Vector2 rect_size = new Vector2(70f, 70f);
			if (avatarIconHeadTexture.width > avatarIconHeadTexture.height)
			{
				if (avatarIconHeadTexture.width > rect_size.x)
				{
					rect_size = new Vector2(rect_size.x, rect_size.x / avatarIconHeadTexture.width * avatarIconHeadTexture.height);
				}
			}
			else if (avatarIconHeadTexture.height > rect_size.y)
			{
				rect_size = new Vector2(rect_size.y / avatarIconHeadTexture.height * avatarIconHeadTexture.width, rect_size.y);
			}
			uIImage = UIUtils.BuildImage(0, new Rect(vector.x - rect_size.x / 2f, vector.y - rect_size.y / 2f, rect_size.x, rect_size.y), mat, avatarIconHeadTexture, rect_size);
			uIImage.CatchMessage = false;
			uIGroupControl.Add(uIImage);
			uIText = UIUtils.BuildUIText(0, new Rect(array[0].x - num4, 25f, array[0].width, 25f), UIText.enAlignStyle.left);
			uIText.Set("Zombie3D/Font/037-CAI978-18", gameState.GetNName(keyValuePair.Value.m_strName), Color.white);
			uIGroupControl.Add(uIText);
			uIText = UIUtils.BuildUIText(0, new Rect(array[1].x - num4, 25f, array[1].width, 25f), UIText.enAlignStyle.left);
			uIText.Set("Zombie3D/Font/037-CAI978-18", keyValuePair.Value.m_iMyDamage.ToString(), Color.white);
			uIGroupControl.Add(uIText);
			int num5 = 0;
			if (bIsOline)
			{
				if (keyValuePair.Value.m_bIsBestKiller)
				{
				}
				if (keyValuePair.Value.m_bIsWinner)
				{
				}
				if (keyValuePair.Value.m_lsReward.Count > 0)
				{
					num5 = 0;
					foreach (KeyValuePair<int, int> item2 in keyValuePair.Value.m_lsReward)
					{
						float num6 = array[2].x - num4 + 100f * (float)num5;
						if (item2.Key >= 1000)
						{
							KeyValuePair<int, int> worAIDByPropsID = gameState.GetWorAIDByPropsID(item2.Key - 1000);
							if (worAIDByPropsID.Key == 0)
							{
								Material mat3 = LoadUIMaterial("Zombie3D/UI/Materials/NAvatarIcons");
								Resources.UnloadUnusedAssets();
								Rect avatarIconTexture = ShopUIScript.GetAvatarIconTexture((Avatar.AvatarSuiteType)worAIDByPropsID.Value, Avatar.AvatarType.Head);
								Rect rcMat = new Rect(avatarIconTexture.x / 2f, avatarIconTexture.y / 2f, avatarIconTexture.width / 2f, avatarIconTexture.height / 2f);
								uIImage = UIUtils.BuildImage(0, new Rect(num6, 11f, rcMat.width / 2f, rcMat.width / 2f), mat3, rcMat, new Vector2(rcMat.width / 2f, rcMat.width / 2f));
								uIGroupControl.Add(uIImage);
								Debug.LogWarning("0");
							}
							else if (worAIDByPropsID.Key == 1)
							{
								Material mat4 = LoadUIMaterial("Zombie3D/UI/Materials/NAvatarIcons");
								Resources.UnloadUnusedAssets();
								Rect avatarIconTexture2 = ShopUIScript.GetAvatarIconTexture((Avatar.AvatarSuiteType)worAIDByPropsID.Value, Avatar.AvatarType.Body);
								Rect rcMat2 = new Rect(avatarIconTexture2.x / 2f, avatarIconTexture2.y / 2f, avatarIconTexture2.width / 2f, avatarIconTexture2.height / 2f);
								uIImage = UIUtils.BuildImage(0, new Rect(num6, 8f, rcMat2.width / 2f, rcMat2.width / 2f), mat4, rcMat2, new Vector2(rcMat2.width / 2f, rcMat2.width / 2f));
								uIGroupControl.Add(uIImage);
								Debug.LogWarning("1");
							}
							else if (worAIDByPropsID.Key == 2)
							{
								Material mat5 = LoadUIMaterial("Zombie3D/UI/Materials/WeaponIcons");
								Resources.UnloadUnusedAssets();
								Rect weaponIconTexture = ShopUIScript.GetWeaponIconTexture((WeaponType)worAIDByPropsID.Value);
								uIImage = UIUtils.BuildImage(0, new Rect(num6, 8f, weaponIconTexture.width / 4f, weaponIconTexture.height / 4f), mat5, weaponIconTexture, new Vector2(weaponIconTexture.width / 4f, weaponIconTexture.height / 4f));
								uIGroupControl.Add(uIImage);
								Debug.LogWarning("2");
							}
							else
							{
								Debug.Log("Wrong");
							}
							uIText = UIUtils.BuildUIText(0, new Rect(num6 + 45f, 45f, 25f, 25f), UIText.enAlignStyle.left);
							uIText.Set("Zombie3D/Font/037-CAI978-18", " X" + item2.Value, Color.white);
							uIGroupControl.Add(uIText);
						}
						else
						{
							if (item2.Key < 0)
							{
								Rect rcMat3 = new Rect(869f, 2f, 45f, 45f);
								uIImage = UIUtils.BuildImage(0, new Rect(num6, 4f, rcMat3.width, rcMat3.height), m_MatNExchangeUI, rcMat3, new Vector2(rcMat3.width, rcMat3.height));
								uIGroupControl.Add(uIImage);
							}
							else if (item2.Key == 101)
							{
								Rect rcMat4 = new Rect(914f, 2f, 45f, 45f);
								uIImage = UIUtils.BuildImage(0, new Rect(num6, 4f, rcMat4.width, rcMat4.height), m_MatNExchangeUI, rcMat4, new Vector2(rcMat4.width, rcMat4.height));
								uIGroupControl.Add(uIImage);
							}
							else if (item2.Key == 102)
							{
								uIImage = UIUtils.BuildImage(rcMat: new Rect(964f, 0f, 60f, 43f), id: 0, scrRect: new Rect(num6, 4f, 45f, 45f), mat: m_MatNExchangeUI, rect_size: new Vector2(45f, 45f));
								uIGroupControl.Add(uIImage);
							}
							else
							{
								Rect powerUpsIconTexture = ShopUIScript.GetPowerUpsIconTexture((ItemType)item2.Key);
								uIImage = UIUtils.BuildImage(0, new Rect(num6, 14f, 45f, 45f), mat2, powerUpsIconTexture, new Vector2(45f, 45f));
								uIGroupControl.Add(uIImage);
							}
							uIText = UIUtils.BuildUIText(0, new Rect(num6 + 45f, 45f, 25f, 25f), UIText.enAlignStyle.left);
							uIText.Set("Zombie3D/Font/037-CAI978-18", " X" + item2.Value, Color.white);
							uIGroupControl.Add(uIText);
						}
						num5++;
					}
				}
			}
			else
			{
				float left = array[2].x + 110f;
				Rect rcMat6 = new Rect(882f, 48f, 66f, 35f);
				uIImage = UIUtils.BuildImage(0, new Rect(left, 15f, rcMat6.width, rcMat6.height), m_MatNExchangeUI, rcMat6, new Vector2(rcMat6.width, rcMat6.height));
				uIGroupControl.Add(uIImage);
			}
			m_FriendsPageView.Add(uIGroupControl);
			num3++;
		}
		if (num > 0f)
		{
			float num7 = num * (float)num2 / (float)m_FriendsPageView.GetControlsCount();
			m_FriendsPageView.ScrollPosV = num7;
			m_FriendsPageViewScrollBar.SetScrollPercent(num7);
		}
		uIImage = UIUtils.BuildImage(0, new Rect(60f, 500f, 275f, 38f), m_MatNExchangeUI, new Rect(2f, 829f, 276f, 38f), new Vector2(276f, 38f));
		m_uiGroup.Add(uIImage);
		uIClickButton = UIUtils.BuildClickButton(6007, new Rect(690f, 20f, 188f, 68f), m_MatNExchangeUI, new Rect(759f, 415f, 188f, 68f), new Rect(759f, 486f, 188f, 68f), new Rect(759f, 415f, 188f, 68f), new Vector2(188f, 68f));
		m_uiGroup.Add(uIClickButton);
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
			Material mat = LoadUIMaterial("Zombie3D/UI/Materials/Dialog01");
			UIImage control = UIUtils.BuildImage(0, new Rect(0f, 0f, 960f, 640f), mat, new Rect(600f, 1f, 1f, 1f), new Vector2(960f, 640f));
			m_uiHintDialog.Add(control);
			float num = 215f;
			float num2 = 167f;
			control = UIUtils.BuildImage(0, new Rect(num, num2, 515f, 301f), mat, new Rect(0f, 378f, 515f, 301f), new Vector2(515f, 301f));
			m_uiHintDialog.Add(control);
			UIText uIText = UIUtils.BuildUIText(0, new Rect(num + 40f, num2 + 40f, 420f, 173f), UIText.enAlignStyle.left);
			uIText.Set("Zombie3D/Font/037-CAI978-18", dialog_content, Constant.TextCommonColor);
			m_uiHintDialog.Add(uIText);
			UIClickButton uIClickButton = null;
			if (okId > 0)
			{
				uIClickButton = UIUtils.BuildClickButton(okId, new Rect(num + 154f, num2 - 16f, 191f, 62f), mat, new Rect(640f, 0f, 191f, 62f), new Rect(832f, 0f, 191f, 62f), new Rect(640f, 0f, 191f, 62f), new Vector2(191f, 62f));
				m_uiHintDialog.Add(uIClickButton);
			}
			if (noId > 0)
			{
				uIClickButton = UIUtils.BuildClickButton(noId, new Rect(num + 21f, num2 - 16f, 191f, 62f), mat, new Rect(640f, 124f, 191f, 62f), new Rect(832f, 124f, 191f, 62f), new Rect(640f, 124f, 191f, 62f), new Vector2(191f, 62f));
				m_uiHintDialog.Add(uIClickButton);
			}
			if (yesId > 0)
			{
				uIClickButton = UIUtils.BuildClickButton(yesId, new Rect(num + 280f, num2 - 16f, 191f, 62f), mat, new Rect(640f, 62f, 191f, 62f), new Rect(832f, 62f, 191f, 62f), new Rect(640f, 62f, 191f, 62f), new Vector2(191f, 62f));
				m_uiHintDialog.Add(uIClickButton);
			}
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
				gameState.BuyPowerUPS((ItemType)ls[i].Key);
			}
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
