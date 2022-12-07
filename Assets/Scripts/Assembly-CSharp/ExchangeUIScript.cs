using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zombie3D;

public class ExchangeUIScript : MonoBehaviour, UIHandler
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
		kIDEnemiesInfoScroll = 6007,
		kIDExchangeGold = 6008,
		kIDExchangeExp = 6009,
		kIDExchange3xLoot = 6010,
		kIDExchangeTweet = 6011,
		kIDExchangeTweenBack = 6012,
		kIDExchangeTweenSend = 6013,
		kIDExchangeCollect = 6014,
		kIDExchangeUINext = 6015,
		kIDExchangeUpgrade3Days = 6016,
		kIDExchangeUpgrade7Days = 6017,
		kIDExchangeUpgrade20Days = 6018,
		kIDExchangeUpgradeNotHaveEnoughCrystalOK = 6019,
		kIDExchangeUpgrade3DaysYes = 6020,
		kIDExchangeUpgrade7DaysYes = 6021,
		kIDExchangeUpgrade20DaysYes = 6022,
		kIDExchangeUpgradeNo = 6023,
		kIDDialogBattleStarOK = 6024,
		kIDLast = 6025
	}

	private UIManager m_UIManager;

	private string m_ui_material_path;

	protected Material m_MatCommonBg;

	protected Material m_MatExchangeUI;

	protected Material m_MatExchangeUI01;

	protected Material m_MatExchangeUI02;

	protected Material m_MatExchangeAnim1UI;

	protected Material m_MatDialog01;

	public uiGroup m_uiGroup;

	public uiGroup m_LevelUpEffectGroup;

	public uiGroup m_uiHintDialog;

	public uiGroup m_uiTweetUI;

	private AudioSource m_ScrollEnemyInfoAudio;

	private UIImageScroller m_KillInfoScrollBar;

	private UIText m_CurKilledEnemyInfoName;

	private UIText m_CurKilledEnemyInfoNum;

	private UIText m_CurKilledEnemyInfo;

	private List<int> m_KilledEnemyOrder;

	public UIEffect01 m_LevelUpEffect01;

	public UIEffect01 m_LevelUpEffect02;

	public UIEffect01 m_LevelUpEffect03;

	public UIImage m_LevelUpRankShow;

	public UIImage m_StarLeft;

	public UIImage m_StarRight;

	public UIImage m_LevelNum1;

	public UIImage m_LevelNum2;

	public UIImage m_LevelNum3;

	public float m_LevelUpRankShowTimer;

	public UIImage m_ContratulationsImg;

	public UIImage m_LineImg;

	public UIImage m_GrayBgImg;

	protected GameState gameState;

	protected float lastUpdateTime;

	protected bool uiInited;

	protected bool m_bLeavingSceneUI;

	private UIEffect01 m_EffectGoldMove02;

	private UIEffect01 m_EffectExpMove02;

	private UIBlock m_LevelUpEffectBlk;

	private float m_GoldEffectOverTimer = -1f;

	private bool m_bGoldEffectOver;

	private float m_ExpEffectOverTimer = -1f;

	private bool m_bExpEffectOver;

	private int m_LastLevel = 1000;

	private int m_NextRankLevel = 1000;

	private int m_FrendeExternExp;

	public uiGroup m_BattleStarDialog;

	private float m_BattleStarTimer = -1f;

	private float m_BattleStarTimeInterval = 0.85f;

	private UIImage m_BattleStarImg1;

	private UIImage m_BattleStarImg2;

	private UIImage m_BattleStarImg3;

	private UIText m_BattleStarText1;

	private UIText m_BattleStarText2;

	private UIText m_BattleStarText3;

	private UIAnimationControl m_BattleStarAnimation;

	private float m_CurExchangeExpPercent = 1f;

	private float m_CurExchangeExpPercentTarget = 1f;

	private float m_CurExchangeGoldPercent = 1f;

	private float m_CurExchangeGoldPercentTarget = 1f;

	private UIText m_CurExchangeGoldPercentShow;

	private UIText m_CurExchangeExpPercentShow;

	private bool m_bExchange3xLoot;

	private string m_strTweetText = string.Empty;

	private bool m_bTweetSend;

	private bool m_bExchangeCollected;

	private void Start()
	{
		Time.timeScale = 1f;
		m_BattleStarTimer = -1f;
		lastUpdateTime = Time.time;
		OpenClickPlugin.Show(false);
		gameState = GameApp.GetInstance().GetGameState();
		m_UIManager = base.gameObject.AddComponent<UIManager>() as UIManager;
		m_UIManager.SetParameter(8, 3, false);
		m_UIManager.SetUIHandler(this);
		UIUtils.BuildIpone5Frame(m_UIManager);
		m_MatCommonBg = LoadUIMaterial("Zombie3D/UI/Materials/CommonBgUI");
		m_MatExchangeUI = LoadUIMaterial("Zombie3D/UI/Materials/ExchangeUI");
		m_MatExchangeUI01 = LoadUIMaterial("Zombie3D/UI/Materials/ExchangeUI01");
		m_MatExchangeUI02 = LoadUIMaterial("Zombie3D/UI/Materials/ExchangeUI02");
		m_MatExchangeAnim1UI = LoadUIMaterial("Zombie3D/UI/Materials/ExchangeAnim1UI");
		m_MatDialog01 = LoadUIMaterial("Zombie3D/UI/Materials/Dialog01");
		GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load("Zombie3D/Audio/ExchangeScroll")) as GameObject;
		if (gameObject != null)
		{
			gameObject.transform.position = Camera.main.transform.position;
			m_ScrollEnemyInfoAudio = gameObject.GetComponent<AudioSource>();
		}
		Resources.UnloadUnusedAssets();
		SceneUIManager.Instance().SetMusicPlayerState(SceneUIManager.MusicState_ExchangeUIAudioState);
		uiInited = true;
		gameState.m_BattleCount++;
		m_FrendeExternExp = Mathf.FloorToInt(gameState.m_BattleExp * 0.03f);
		m_CurExchangeExpPercent = gameState.m_BattleExpExchangePercent;
		m_CurExchangeExpPercentTarget = m_CurExchangeExpPercent;
		m_CurExchangeGoldPercent = gameState.m_BattleGoldExchangePercent;
		m_CurExchangeGoldPercentTarget = m_CurExchangeGoldPercent;
		m_LastLevel = gameState.GetPlayerLevel();
		gameState.m_iPlayerLastLevel = m_LastLevel;
		m_NextRankLevel = ConfigManager.Instance().GetFixedConfig().GetNextRankLevel(m_LastLevel);
		Debug.Log(gameState.m_bExchanged + "|" + gameState.m_bExchangeGold + "|" + gameState.m_bExchangeExp);
		GameApp.GetInstance().GetGameState().LEVEL_UP = false;
		gameState.AddExchangeUpgradeTime(0L);
		gameState.m_bExchanged = false;
		m_bExchangeCollected = false;
		SetupExchangeReportUI(true);
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
		if (m_bLeavingSceneUI || Time.time - lastUpdateTime < 0.001f || !uiInited)
		{
			return;
		}
		float num = Time.time - lastUpdateTime;
		lastUpdateTime = Time.time;
		if (m_EffectGoldMove02 != null)
		{
			m_EffectGoldMove02.Update(num);
			if (m_EffectGoldMove02.EffectOver())
			{
				SetupExchangeUI(true);
				if (GameApp.GetInstance().GetGameState().SoundOn)
				{
					GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load("Zombie3D/Audio/ExchangeGetCash")) as GameObject;
					if (gameObject != null)
					{
						gameObject.transform.position = Camera.main.transform.position;
						RemoveTimerScript removeTimerScript = gameObject.AddComponent<RemoveTimerScript>();
						removeTimerScript.life = 2f;
						AudioSource audioSource = gameObject.GetComponent<AudioSource>();
						if (audioSource != null)
						{
							audioSource.loop = false;
							audioSource.Play();
						}
					}
				}
				m_EffectGoldMove02.Clear();
				m_EffectGoldMove02 = null;
				UIAnimationControl uIAnimationControl = new UIAnimationControl();
				uIAnimationControl.Id = 0;
				uIAnimationControl.SetAnimationsPageCount(6);
				uIAnimationControl.Rect = AutoUI.AutoRect(new Rect(380f, 490f, 221f, 226f));
				uIAnimationControl.SetTexture(0, m_MatExchangeAnim1UI, AutoUI.AutoRect(new Rect(221f, 113f, 110f, 113f)), AutoUI.AutoSize(new Vector2(221f, 226f)));
				uIAnimationControl.SetTexture(1, m_MatExchangeAnim1UI, AutoUI.AutoRect(new Rect(331f, 113f, 110f, 113f)), AutoUI.AutoSize(new Vector2(221f, 226f)));
				uIAnimationControl.SetTexture(2, m_MatExchangeAnim1UI, AutoUI.AutoRect(new Rect(0f, 226f, 110f, 113f)), AutoUI.AutoSize(new Vector2(221f, 226f)));
				uIAnimationControl.SetTexture(3, m_MatExchangeAnim1UI, AutoUI.AutoRect(new Rect(110f, 226f, 110f, 113f)), AutoUI.AutoSize(new Vector2(221f, 226f)));
				uIAnimationControl.SetTexture(4, m_MatExchangeAnim1UI, AutoUI.AutoRect(new Rect(221f, 226f, 110f, 113f)), AutoUI.AutoSize(new Vector2(221f, 226f)));
				uIAnimationControl.SetTexture(5, m_MatExchangeAnim1UI, AutoUI.AutoRect(new Rect(331f, 226f, 110f, 113f)), AutoUI.AutoSize(new Vector2(221f, 226f)));
				uIAnimationControl.SetTimeInterval(0.1f);
				uIAnimationControl.SetLoopCount(1);
				m_UIManager.Add(uIAnimationControl);
				m_GoldEffectOverTimer = 0f;
			}
		}
		if (m_GoldEffectOverTimer >= 0f)
		{
			m_GoldEffectOverTimer += num;
			if (m_GoldEffectOverTimer > 1f)
			{
				m_GoldEffectOverTimer = -1f;
				m_bGoldEffectOver = true;
			}
		}
		if (m_EffectExpMove02 != null)
		{
			m_EffectExpMove02.Update(num);
			if (m_EffectExpMove02.EffectOver())
			{
				SetupExchangeUI(true);
				if (GameApp.GetInstance().GetGameState().SoundOn)
				{
					GameObject gameObject2 = UnityEngine.Object.Instantiate(Resources.Load("Zombie3D/Audio/ExchangeGetExp")) as GameObject;
					if (gameObject2 != null)
					{
						gameObject2.transform.position = Camera.main.transform.position;
						RemoveTimerScript removeTimerScript2 = gameObject2.AddComponent<RemoveTimerScript>();
						removeTimerScript2.life = 2f;
						AudioSource audioSource2 = gameObject2.GetComponent<AudioSource>();
						if (audioSource2 != null)
						{
							audioSource2.loop = false;
							audioSource2.Play();
						}
					}
				}
				m_EffectExpMove02.Clear();
				m_EffectExpMove02 = null;
				UIAnimationControl uIAnimationControl2 = new UIAnimationControl();
				uIAnimationControl2.Id = 0;
				uIAnimationControl2.SetAnimationsPageCount(6);
				uIAnimationControl2.Rect = AutoUI.AutoRect(new Rect(70f, 490f, 221f, 226f));
				uIAnimationControl2.SetTexture(0, m_MatExchangeAnim1UI, AutoUI.AutoRect(new Rect(0f, 0f, 110f, 113f)), AutoUI.AutoSize(new Vector2(221f, 226f)));
				uIAnimationControl2.SetTexture(1, m_MatExchangeAnim1UI, AutoUI.AutoRect(new Rect(110f, 0f, 110f, 113f)), AutoUI.AutoSize(new Vector2(221f, 226f)));
				uIAnimationControl2.SetTexture(2, m_MatExchangeAnim1UI, AutoUI.AutoRect(new Rect(221f, 0f, 110f, 113f)), AutoUI.AutoSize(new Vector2(221f, 226f)));
				uIAnimationControl2.SetTexture(3, m_MatExchangeAnim1UI, AutoUI.AutoRect(new Rect(331f, 0f, 110f, 113f)), AutoUI.AutoSize(new Vector2(221f, 226f)));
				uIAnimationControl2.SetTexture(4, m_MatExchangeAnim1UI, AutoUI.AutoRect(new Rect(0f, 113f, 110f, 113f)), AutoUI.AutoSize(new Vector2(221f, 226f)));
				uIAnimationControl2.SetTexture(5, m_MatExchangeAnim1UI, AutoUI.AutoRect(new Rect(110f, 113f, 110f, 113f)), AutoUI.AutoSize(new Vector2(221f, 226f)));
				uIAnimationControl2.SetTimeInterval(0.1f);
				uIAnimationControl2.SetLoopCount(1);
				m_UIManager.Add(uIAnimationControl2);
				m_ExpEffectOverTimer = 0f;
			}
		}
		if (m_ExpEffectOverTimer >= 0f)
		{
			m_ExpEffectOverTimer += num;
			if (m_ExpEffectOverTimer > 1f)
			{
				m_ExpEffectOverTimer = -1f;
				bool flag = false;
				if (gameState.GetPlayerLevel() > m_LastLevel)
				{
					m_bExpEffectOver = false;
					flag = true;
					SetupLevelUpEffectUI(true);
					GameApp.GetInstance().GetGameState().LEVEL_UP = true;
					GameApp.GetInstance().GetGameState().SetNeedShowLevelupAnimation(1);
					GameApp.GetInstance().Save();
					if (GameApp.GetInstance().GetGameState().SoundOn)
					{
						GameObject gameObject3 = UnityEngine.Object.Instantiate(Resources.Load("Zombie3D/Audio/LevelUpAudio")) as GameObject;
						if (gameObject3 != null)
						{
							gameObject3.transform.position = Camera.main.transform.position;
							RemoveTimerScript removeTimerScript3 = gameObject3.AddComponent<RemoveTimerScript>();
							removeTimerScript3.life = 2f;
							AudioSource audioSource3 = gameObject3.GetComponent<AudioSource>();
							if (audioSource3 != null)
							{
								audioSource3.loop = false;
								audioSource3.Play();
							}
						}
					}
				}
				else
				{
					m_bExpEffectOver = true;
				}
			}
		}
		if (m_LevelUpEffect01 != null)
		{
			m_LevelUpEffect01.Update(num);
			m_LevelUpEffect02.Update(num);
			m_LevelUpEffect03.Update(num);
			m_LevelUpRankShowTimer += num;
			if (m_LevelUpRankShowTimer < 0.2f)
			{
				float a = 0f;
				if (m_LevelUpRankShow != null)
				{
					m_LevelUpRankShow.SetColor(new Color(1f, 1f, 1f, a));
				}
				m_StarLeft.SetColor(new Color(1f, 1f, 1f, a));
				m_StarRight.SetColor(new Color(1f, 1f, 1f, a));
				if (m_LevelNum1 != null)
				{
					m_LevelNum1.SetColor(new Color(1f, 1f, 1f, a));
				}
				if (m_LevelNum2 != null)
				{
					m_LevelNum2.SetColor(new Color(1f, 1f, 1f, a));
				}
				if (m_LevelNum3 != null)
				{
					m_LevelNum3.SetColor(new Color(1f, 1f, 1f, a));
				}
			}
			else if (m_LevelUpRankShowTimer < 1.6f)
			{
				float a2 = Mathf.Clamp01(m_LevelUpRankShowTimer - 1.2f);
				if (m_LevelUpRankShow != null)
				{
					m_LevelUpRankShow.SetColor(new Color(1f, 1f, 1f, a2));
				}
				m_StarLeft.SetColor(new Color(1f, 1f, 1f, a2));
				m_StarRight.SetColor(new Color(1f, 1f, 1f, a2));
				if (m_LevelNum1 != null)
				{
					m_LevelNum1.SetColor(new Color(1f, 1f, 1f, a2));
				}
				if (m_LevelNum2 != null)
				{
					m_LevelNum2.SetColor(new Color(1f, 1f, 1f, a2));
				}
				if (m_LevelNum3 != null)
				{
					m_LevelNum3.SetColor(new Color(1f, 1f, 1f, a2));
				}
			}
			else
			{
				float a3 = Mathf.Clamp01((3.3f - m_LevelUpRankShowTimer) * 2f);
				if (m_LevelUpRankShow != null)
				{
					m_LevelUpRankShow.SetColor(new Color(1f, 1f, 1f, a3));
				}
				if (m_StarLeft != null)
				{
					m_StarLeft.SetColor(new Color(1f, 1f, 1f, a3));
				}
				if (m_StarRight != null)
				{
					m_StarRight.SetColor(new Color(1f, 1f, 1f, a3));
				}
				if (m_LevelNum1 != null)
				{
					m_LevelNum1.SetColor(new Color(1f, 1f, 1f, a3));
				}
				if (m_LevelNum2 != null)
				{
					m_LevelNum2.SetColor(new Color(1f, 1f, 1f, a3));
				}
				if (m_LevelNum3 != null)
				{
					m_LevelNum3.SetColor(new Color(1f, 1f, 1f, a3));
				}
				if (m_GrayBgImg != null)
				{
					m_GrayBgImg.SetColor(new Color(1f, 1f, 1f, a3));
				}
				if (m_ContratulationsImg != null)
				{
					m_ContratulationsImg.SetColor(new Color(1f, 1f, 1f, a3));
				}
				if (m_LineImg != null)
				{
					m_LineImg.SetColor(new Color(1f, 1f, 1f, a3));
				}
			}
			if (m_LevelUpEffect01.EffectOver())
			{
				m_LevelUpRankShowTimer = 0f;
				m_LevelUpEffect01.Clear();
				m_LevelUpEffect01 = null;
				m_LevelUpEffect02.Clear();
				m_LevelUpEffect02 = null;
				m_UIManager.Remove(m_LevelUpEffectBlk);
				m_UIManager.Remove(m_LevelUpRankShow);
				m_LevelUpRankShow = null;
				m_UIManager.Remove(m_StarLeft);
				m_StarLeft = null;
				m_UIManager.Remove(m_StarRight);
				m_StarRight = null;
				m_UIManager.Remove(m_LevelNum1);
				m_LevelNum1 = null;
				m_UIManager.Remove(m_LevelNum2);
				m_LevelNum2 = null;
				m_UIManager.Remove(m_LevelNum3);
				m_LevelNum3 = null;
				m_bExpEffectOver = true;
			}
		}
		if (m_bGoldEffectOver && m_bExpEffectOver && m_uiHintDialog == null)
		{
			UIBlock uIBlock = new UIBlock();
			uIBlock.Rect = new Rect(0f, 0f, 960f, 640f);
			m_UIManager.Add(uIBlock);
			m_bLeavingSceneUI = true;
			if (gameState.m_bGameLoginExchange)
			{
				SceneUIManager.Instance().ChangeSceneUI(SceneUIManager.SceneUI.ChoosePointsUI);
			}
			else
			{
				SceneUIManager.Instance().ChangeSceneUI(SceneUIManager.SceneUI.ShopUI);
			}
		}
		if (m_BattleStarTimer >= 0f)
		{
			m_BattleStarTimer += num;
			if (m_BattleStarTimer <= m_BattleStarTimeInterval * 1f + 0.15f && m_BattleStarTimer > m_BattleStarTimeInterval * 1f && 1 <= gameState.GetBattleStar())
			{
				if (m_BattleStarImg1 == null)
				{
					return;
				}
				if (!m_BattleStarImg1.Visible)
				{
					m_BattleStarImg1.Visible = true;
					m_BattleStarText1.Visible = true;
					m_BattleStarText2.Visible = false;
					m_BattleStarText3.Visible = false;
					if (GameApp.GetInstance().GetGameState().SoundOn)
					{
						AudioSource audioSource4 = null;
						GameObject gameObject4 = UnityEngine.Object.Instantiate(Resources.Load("Zombie3D/Audio/RankUpAudio")) as GameObject;
						if (gameObject4 != null)
						{
							gameObject4.transform.position = base.transform.position;
							RemoveTimerScript removeTimerScript4 = gameObject4.AddComponent<RemoveTimerScript>();
							if (audioSource4 != null && audioSource4.isPlaying)
							{
								audioSource4.Stop();
							}
							audioSource4 = gameObject4.GetComponent<AudioSource>();
							removeTimerScript4.life = audioSource4.clip.length + 0.1f;
							audioSource4.loop = false;
							audioSource4.Play();
						}
					}
					if (gameState.GetBattleStar() >= 2)
					{
						if (m_BattleStarAnimation != null)
						{
							if (m_uiGroup != null)
							{
								m_uiGroup.Remove(m_BattleStarAnimation);
							}
							m_BattleStarAnimation = null;
							m_BattleStarAnimation = new UIAnimationControl();
						}
						m_BattleStarAnimation.Id = 0;
						m_BattleStarAnimation.SetAnimationsPageCount(5);
						m_BattleStarAnimation.Rect = AutoUI.AutoRect(new Rect(570f, 113f, 238f, 235f));
						m_BattleStarAnimation.SetTexture(0, m_MatExchangeUI01, AutoUI.AutoRect(new Rect(0f, 0f, 119f, 117f)), AutoUI.AutoSize(new Vector2(238f, 235f)));
						m_BattleStarAnimation.SetTexture(1, m_MatExchangeUI01, AutoUI.AutoRect(new Rect(119f, 0f, 119f, 117f)), AutoUI.AutoSize(new Vector2(238f, 235f)));
						m_BattleStarAnimation.SetTexture(2, m_MatExchangeUI01, AutoUI.AutoRect(new Rect(238f, 0f, 119f, 117f)), AutoUI.AutoSize(new Vector2(238f, 235f)));
						m_BattleStarAnimation.SetTexture(3, m_MatExchangeUI01, AutoUI.AutoRect(new Rect(0f, 117f, 119f, 117f)), AutoUI.AutoSize(new Vector2(238f, 235f)));
						m_BattleStarAnimation.SetTexture(4, m_MatExchangeUI01, AutoUI.AutoRect(new Rect(119f, 117f, 119f, 117f)), AutoUI.AutoSize(new Vector2(238f, 235f)));
						m_BattleStarAnimation.SetTimeInterval(0.2f);
						m_BattleStarAnimation.SetLoopCount(1);
						m_BattleStarAnimation.Visible = true;
						m_BattleStarAnimation.Enable = true;
						if (m_uiGroup != null)
						{
							m_uiGroup.Add(m_BattleStarAnimation);
						}
					}
				}
			}
			if (m_BattleStarTimer < m_BattleStarTimeInterval * 2f + 0.15f && m_BattleStarTimer > m_BattleStarTimeInterval * 2f && 2 <= gameState.GetBattleStar())
			{
				if (m_BattleStarImg2 == null)
				{
					return;
				}
				if (!m_BattleStarImg2.Visible)
				{
					m_BattleStarImg2.Visible = true;
					m_BattleStarText1.Visible = false;
					m_BattleStarText2.Visible = true;
					m_BattleStarText3.Visible = false;
					if (GameApp.GetInstance().GetGameState().SoundOn)
					{
						AudioSource audioSource5 = null;
						GameObject gameObject5 = UnityEngine.Object.Instantiate(Resources.Load("Zombie3D/Audio/RankUpAudio")) as GameObject;
						if (gameObject5 != null)
						{
							gameObject5.transform.position = base.transform.position;
							RemoveTimerScript removeTimerScript5 = gameObject5.AddComponent<RemoveTimerScript>();
							if (audioSource5 != null && audioSource5.isPlaying)
							{
								audioSource5.Stop();
							}
							audioSource5 = gameObject5.GetComponent<AudioSource>();
							removeTimerScript5.life = audioSource5.clip.length + 0.1f;
							audioSource5.loop = false;
							audioSource5.Play();
						}
					}
					if (gameState.GetBattleStar() >= 3)
					{
						if (m_BattleStarAnimation != null)
						{
							if (m_uiGroup != null)
							{
								m_uiGroup.Remove(m_BattleStarAnimation);
							}
							m_BattleStarAnimation = null;
							m_BattleStarAnimation = new UIAnimationControl();
						}
						m_BattleStarAnimation.Id = 0;
						m_BattleStarAnimation.SetAnimationsPageCount(5);
						m_BattleStarAnimation.Rect = AutoUI.AutoRect(new Rect(698f, 113f, 238f, 235f));
						m_BattleStarAnimation.SetTexture(0, m_MatExchangeUI01, AutoUI.AutoRect(new Rect(0f, 0f, 119f, 117f)), AutoUI.AutoSize(new Vector2(238f, 235f)));
						m_BattleStarAnimation.SetTexture(1, m_MatExchangeUI01, AutoUI.AutoRect(new Rect(119f, 0f, 119f, 117f)), AutoUI.AutoSize(new Vector2(238f, 235f)));
						m_BattleStarAnimation.SetTexture(2, m_MatExchangeUI01, AutoUI.AutoRect(new Rect(238f, 0f, 119f, 117f)), AutoUI.AutoSize(new Vector2(238f, 235f)));
						m_BattleStarAnimation.SetTexture(3, m_MatExchangeUI01, AutoUI.AutoRect(new Rect(0f, 117f, 119f, 117f)), AutoUI.AutoSize(new Vector2(238f, 235f)));
						m_BattleStarAnimation.SetTexture(4, m_MatExchangeUI01, AutoUI.AutoRect(new Rect(119f, 117f, 119f, 117f)), AutoUI.AutoSize(new Vector2(238f, 235f)));
						m_BattleStarAnimation.SetTimeInterval(0.2f);
						m_BattleStarAnimation.SetLoopCount(1);
						m_BattleStarAnimation.Visible = true;
						m_BattleStarAnimation.Enable = true;
						if (m_uiGroup != null)
						{
							m_uiGroup.Add(m_BattleStarAnimation);
						}
					}
				}
			}
			if (m_BattleStarTimer < m_BattleStarTimeInterval * 3f + 0.15f && m_BattleStarTimer > m_BattleStarTimeInterval * 3f && 3 <= gameState.GetBattleStar())
			{
				if (m_BattleStarImg3 == null)
				{
					return;
				}
				if (!m_BattleStarImg3.Visible)
				{
					m_BattleStarImg3.Visible = true;
					m_BattleStarText1.Visible = false;
					m_BattleStarText2.Visible = false;
					m_BattleStarText3.Visible = true;
					if (GameApp.GetInstance().GetGameState().SoundOn)
					{
						AudioSource audioSource6 = null;
						GameObject gameObject6 = UnityEngine.Object.Instantiate(Resources.Load("Zombie3D/Audio/RankUpAudio")) as GameObject;
						if (gameObject6 != null)
						{
							gameObject6.transform.position = base.transform.position;
							RemoveTimerScript removeTimerScript6 = gameObject6.AddComponent<RemoveTimerScript>();
							if (audioSource6 != null && audioSource6.isPlaying)
							{
								audioSource6.Stop();
							}
							audioSource6 = gameObject6.GetComponent<AudioSource>();
							removeTimerScript6.life = audioSource6.clip.length + 0.1f;
							audioSource6.loop = false;
							audioSource6.Play();
						}
					}
					m_BattleStarAnimation.Visible = false;
					m_BattleStarAnimation.Enable = false;
				}
			}
			if (m_BattleStarTimer > m_BattleStarTimeInterval * (float)(gameState.GetBattleStar() + 1))
			{
				m_BattleStarTimer = -1f;
				m_BattleStarText1.Visible = false;
				m_BattleStarText2.Visible = false;
				m_BattleStarText3.Visible = false;
				m_BattleStarAnimation.Visible = false;
				m_BattleStarAnimation.Enable = false;
				if (gameState.GetBattleStar() >= 2)
				{
					SetupBattleStarDialog(true);
				}
			}
		}
		if (m_CurExchangeGoldPercent < m_CurExchangeGoldPercentTarget)
		{
			m_CurExchangeGoldPercent = Mathf.MoveTowards(m_CurExchangeGoldPercent, m_CurExchangeGoldPercentTarget, 0.5f);
			string text = "x" + Mathf.FloorToInt(m_CurExchangeGoldPercent * 100f) + "%";
			m_CurExchangeGoldPercentShow.SetText(text);
		}
		if (m_CurExchangeExpPercent < m_CurExchangeExpPercentTarget)
		{
			m_CurExchangeExpPercent = Mathf.MoveTowards(m_CurExchangeExpPercent, m_CurExchangeExpPercentTarget, 0.5f);
			string text2 = "x" + Mathf.FloorToInt(m_CurExchangeExpPercent * 100f) + "%";
			m_CurExchangeExpPercentShow.SetText(text2);
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
		if (control.Id == 6007)
		{
			if (command != 0)
			{
				return;
			}
			int index = Mathf.FloorToInt(wparam);
			if (m_KilledEnemyOrder.Count > 0)
			{
				FixedConfig.EnemyCfg enemyCfg = ConfigManager.Instance().GetFixedConfig().GetEnemyCfg(m_KilledEnemyOrder[index]);
				int num = (int)gameState.m_KilledEnemyInfo[m_KilledEnemyOrder[index]];
				if (m_CurKilledEnemyInfoName != null)
				{
					m_CurKilledEnemyInfoName.SetText(enemyCfg.name);
					m_CurKilledEnemyInfoNum.SetText(num.ToString());
				}
			}
			if (gameState.SoundOn && m_ScrollEnemyInfoAudio != null)
			{
				m_ScrollEnemyInfoAudio.loop = false;
				m_ScrollEnemyInfoAudio.Play();
			}
		}
		else if (control.Id == 6014)
		{
			if (m_bExchangeCollected)
			{
				return;
			}
			m_bExchangeCollected = true;
			if (m_bExchange3xLoot)
			{
				gameState.LoseDollor(1);
			}
			gameState.ExchangeGold(m_CurExchangeGoldPercentTarget);
			gameState.ExchangeExp(m_CurExchangeExpPercentTarget);
			gameState.m_bExchanged = true;
			gameState.InitExchangeInfo();
			gameState.SaveExchangeInfo();
			SetupExchangeUI(true);
			SetupGoldMove02Effect();
			SetupExpMove02Effect();
			if (m_FrendeExternExp > 0 && gameState.m_SelectFriendIndex > 0)
			{
				ArrayList friends = gameState.GetFriends();
				FriendUserData friendUserData = friends[gameState.m_SelectFriendIndex] as FriendUserData;
				if (friendUserData.m_UUID != string.Empty)
				{
					GameClient.SetFriendUserExternExp(m_FrendeExternExp, friendUserData.m_UUID, friendUserData.m_DeviceId);
				}
			}
		}
		else if (control.Id == 6010)
		{
			m_bExchange3xLoot = true;
			m_CurExchangeExpPercentTarget += 3f;
			m_CurExchangeGoldPercentTarget += 3f;
			SetupExchangeUI(true);
		}
		else if (control.Id == 6011)
		{
			SetupTweetUI(true);
		}
		else if (control.Id == 6013)
		{
			if (!m_bTweetSend)
			{
				m_bTweetSend = true;
				m_CurExchangeExpPercentTarget += 0.2f;
				m_CurExchangeGoldPercentTarget += 0.2f;
				SetupTweetUI(false);
				SetupExchangeUI(true);
				string tweetText = GetTweetText(true);
				tweetText += "#callofmini #doubleshot";
				TweetPlugin.SendMsg(tweetText);
			}
		}
		else if (control.Id == 6012)
		{
			SetupTweetUI(false);
		}
		else if (control.Id == 6015)
		{
			SetupExchangeUI(true);
			m_BattleStarTimer = -1f;
		}
		else if (control.Id == 6016)
		{
			if (m_uiHintDialog == null)
			{
				if (gameState.HaveEnoughDollor(5))
				{
					SetupHintDialog(true, 0, 6020, 6023, "Do you want to spend 5 Crystals to get pumped up for 72 hours?");
				}
				else
				{
					SetupHintDialog(true, 6023, 0, 0, "Insufficient crystals! Visit the bank now to get more.");
				}
			}
		}
		else if (control.Id == 6017)
		{
			if (m_uiHintDialog == null)
			{
				if (gameState.HaveEnoughDollor(8))
				{
					SetupHintDialog(true, 0, 6021, 6023, "Do you want to spend 8 Crystals to get pumped up for 192 hours?");
				}
				else
				{
					SetupHintDialog(true, 6023, 0, 0, "Insufficient crystals! Visit the bank now to get more.");
				}
			}
		}
		else if (control.Id == 6018)
		{
			if (m_uiHintDialog == null)
			{
				if (gameState.HaveEnoughDollor(15))
				{
					SetupHintDialog(true, 0, 6022, 6023, "Do you want to spend 15 Crystals to get pumped up for 480 hours?");
				}
				else
				{
					SetupHintDialog(true, 6023, 0, 0, "Insufficient crystals! Visit the bank now to get more.");
				}
			}
		}
		else if (control.Id == 6020)
		{
			gameState.AddExchangeUpgradeTime(259199L);
			gameState.LoseDollor(5);
			GameApp.GetInstance().Save();
			SetupHintDialog(false, 0, 0, 0, string.Empty);
			SetupExchangeUI(true);
		}
		else if (control.Id == 6021)
		{
			gameState.AddExchangeUpgradeTime(604799L);
			gameState.LoseDollor(8);
			GameApp.GetInstance().Save();
			SetupHintDialog(false, 0, 0, 0, string.Empty);
			SetupExchangeUI(true);
		}
		else if (control.Id == 6022)
		{
			gameState.AddExchangeUpgradeTime(1727999L);
			gameState.LoseDollor(15);
			GameApp.GetInstance().Save();
			SetupHintDialog(false, 0, 0, 0, string.Empty);
			SetupExchangeUI(true);
		}
		else if (control.Id == 6023)
		{
			SetupHintDialog(false, 0, 0, 0, string.Empty);
		}
		else if (control.Id == 6024)
		{
			SetupBattleStarDialog(false);
			if (m_uiGroup != null)
			{
				UIClickButton uIClickButton = (UIClickButton)m_uiGroup.GetControl(6015);
				if (uIClickButton != null)
				{
					uIClickButton.Visible = true;
				}
			}
		}
		else if (control.Id != 6006 && control.Id != 6025)
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
		if (bShow)
		{
			OpenClickPlugin.Hide();
			m_uiGroup = new uiGroup(m_UIManager);
			UIClickButton uIClickButton = null;
			UIImage uIImage = null;
			UIText uIText = null;
			uIImage = UIUtils.BuildImage(0, new Rect(0f, 0f, 960f, 640f), m_MatCommonBg, new Rect(0f, 0f, 960f, 640f), new Vector2(960f, 640f));
			m_uiGroup.Add(uIImage);
			SetupAroundUI(true, m_uiGroup);
			uIImage = UIUtils.BuildImage(0, new Rect(54f, 147f, 850f, 363f), m_MatExchangeUI02, new Rect(0f, 0f, 850f, 363f), new Vector2(850f, 363f));
			m_uiGroup.Add(uIImage);
			uIImage = UIUtils.BuildImage(0, new Rect(115f, 407f, 94f, 79f), m_MatExchangeUI, new Rect(911f, 88f, 94f, 79f), new Vector2(94f, 79f));
			m_uiGroup.Add(uIImage);
			uIImage = UIUtils.BuildImage(0, new Rect(487f, 413f, 95f, 75f), m_MatExchangeUI, new Rect(911f, 167f, 95f, 75f), new Vector2(95f, 75f));
			m_uiGroup.Add(uIImage);
			string text = Mathf.FloorToInt(gameState.m_BattleGold).ToString();
			float num = 0f;
			if (text.Length <= 3)
			{
				num = 20f;
			}
			uIText = UIUtils.BuildUIText(0, new Rect(190f + num, 437f, 200f, 25f), UIText.enAlignStyle.left);
			uIText.Set("Zombie3D/Font/037-CAI978-18", text, Color.white);
			m_uiGroup.Add(uIText);
			float textWidth = mgrFont.Instance().getFont("Zombie3D/Font/037-CAI978-18").GetTextWidth(text);
			float left = uIText.Rect.x + textWidth + 5f;
			if (AutoUI.IsRetain == AutoUI.RESOLUTION.LOWDEFINITION)
			{
				left = (95f + textWidth + 20f) * 2f;
			}
			string text2 = "x" + m_CurExchangeGoldPercent * 100f + "%";
			m_CurExchangeGoldPercentShow = UIUtils.BuildUIText(0, new Rect(left, 425f, 200f, 40f), UIText.enAlignStyle.left);
			m_CurExchangeGoldPercentShow.Set("Zombie3D/Font/037-CAI978-27", text2, Constant.TextCommonColor);
			m_uiGroup.Add(m_CurExchangeGoldPercentShow);
			string text3 = Mathf.FloorToInt(gameState.m_BattleExp).ToString();
			float num2 = 0f;
			if (text3.Length <= 3)
			{
				num2 = 20f;
			}
			uIText = UIUtils.BuildUIText(0, new Rect(570f + num2, 437f, 200f, 25f), UIText.enAlignStyle.left);
			uIText.Set("Zombie3D/Font/037-CAI978-18", text3, Color.white);
			m_uiGroup.Add(uIText);
			float textWidth2 = mgrFont.Instance().getFont("Zombie3D/Font/037-CAI978-18").GetTextWidth(text3);
			float left2 = uIText.Rect.x + textWidth2 + 5f;
			if (AutoUI.IsRetain == AutoUI.RESOLUTION.LOWDEFINITION)
			{
				left2 = (285f + textWidth2 + 20f) * 2f;
			}
			string text4 = "x" + m_CurExchangeExpPercent * 100f + "%";
			m_CurExchangeExpPercentShow = UIUtils.BuildUIText(0, new Rect(left2, 425f, 200f, 40f), UIText.enAlignStyle.left);
			m_CurExchangeExpPercentShow.Set("Zombie3D/Font/037-CAI978-27", text4, Constant.TextCommonColor);
			m_uiGroup.Add(m_CurExchangeExpPercentShow);
			uIText = UIUtils.BuildUIText(0, new Rect(320f, 365f, 200f, 25f), UIText.enAlignStyle.left);
			uIText.Set("Zombie3D/Font/037-CAI978-18", "1", Constant.TextCommonColor);
			m_uiGroup.Add(uIText);
			uIImage = UIUtils.BuildImage(0, new Rect(333f, 360f, 50f, 36f), m_MatCommonBg, new Rect(967f, 631f, 50f, 36f), new Vector2(50f, 36f));
			m_uiGroup.Add(uIImage);
			uIClickButton = UIUtils.BuildClickButton(6010, new Rect(380f, 352f, 196f, 53f), m_MatExchangeUI02, new Rect(659f, 363f, 196f, 53f), new Rect(659f, 416f, 196f, 53f), new Rect(659f, 740f, 196f, 53f), new Vector2(196f, 53f));
			if (gameState.dollor < 1 || m_bExchange3xLoot)
			{
				uIClickButton.Enable = false;
			}
			m_uiGroup.Add(uIClickButton);
			uIImage = UIUtils.BuildImage(0, new Rect(204f, 178f, 531f, 167f), m_MatExchangeUI02, new Rect(0f, 767f, 531f, 167f), new Vector2(531f, 167f));
			m_uiGroup.Add(uIImage);
			uIImage = UIUtils.BuildImage(0, new Rect(300f, 207f, 82f, 58f), m_MatExchangeUI02, new Rect(855f, 0f, 82f, 58f), new Vector2(82f, 58f));
			m_uiGroup.Add(uIImage);
			uIClickButton = UIUtils.BuildClickButton(6011, new Rect(380f, 214f, 196f, 53f), m_MatExchangeUI02, new Rect(659f, 577f, 196f, 53f), new Rect(659f, 628f, 196f, 53f), new Rect(659f, 684f, 196f, 53f), new Vector2(196f, 53f));
			if (!MiscPlugin.IsOS5Up() || m_bTweetSend)
			{
				uIClickButton.Enable = false;
			}
			m_uiGroup.Add(uIClickButton);
			Rect scrRect = new Rect(280f, 290f, 600f, 25f);
			string empty = string.Empty;
			if (GameApp.GetInstance().GetGameState().GetBattleStar() == 3)
			{
				empty = "Tweet your score for bonus Loot!";
			}
			else
			{
				empty = "Get a 3-star rating to tweet for bonuses!";
				scrRect = new Rect(250f, 290f, 600f, 25f);
				uIClickButton.Enable = false;
			}
			uIText = UIUtils.BuildUIText(0, scrRect, UIText.enAlignStyle.left);
			uIText.Set("Zombie3D/Font/037-CAI978-18", empty, Constant.TextCommonColor);
			m_uiGroup.Add(uIText);
			uIClickButton = UIUtils.BuildClickButton(6014, new Rect(380f, 75f, 196f, 53f), m_MatExchangeUI02, new Rect(659f, 469f, 196f, 53f), new Rect(659f, 523f, 196f, 53f), new Rect(659f, 469f, 196f, 53f), new Vector2(196f, 53f));
			m_uiGroup.Add(uIClickButton);
		}
	}

	public void SetupExchangeReportUI(bool bShow)
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
		UIClickButton uIClickButton = null;
		UIImage uIImage = null;
		UIText uIText = null;
		uIImage = UIUtils.BuildImage(0, new Rect(0f, 0f, 960f, 640f), m_MatCommonBg, new Rect(0f, 0f, 960f, 640f), new Vector2(960f, 640f));
		m_uiGroup.Add(uIImage);
		SetupAroundUI(true, m_uiGroup);
		uIImage = UIUtils.BuildImage(0, new Rect(48f, 525f, 373f, 34f), m_MatExchangeUI, new Rect(0f, 990f, 373f, 34f), new Vector2(373f, 34f));
		m_uiGroup.Add(uIImage);
		uIImage = UIUtils.BuildImage(0, new Rect(32f, 178f, 45f, 225f), m_MatExchangeUI, new Rect(600f, 799f, 45f, 225f), new Vector2(45f, 225f));
		m_uiGroup.Add(uIImage);
		uIImage = UIUtils.BuildImage(0, new Rect(413f, 178f, 45f, 225f), m_MatExchangeUI, new Rect(646f, 799f, 45f, 225f), new Vector2(45f, 225f));
		m_uiGroup.Add(uIImage);
		SetupExchangeBattleInfoUI(m_uiGroup);
		m_KillInfoScrollBar = new UIImageScroller(AutoUI.AutoRect(new Rect(20f, 20f, 450f, 550f)), AutoUI.AutoRect(new Rect(50f, 87f, 379f, 415f)), 1, AutoUI.AutoSize(new Vector2(379f, 198f)), ScrollerDir.Vertical, true);
		m_KillInfoScrollBar.Id = 6007;
		m_KillInfoScrollBar.SetImageSpacing(AutoUI.AutoSize(new Vector2(0f, 50f)));
		m_KillInfoScrollBar.SetUIHandler(m_KillInfoScrollBar);
		Material mat = LoadUIMaterial("Zombie3D/UI/Materials/ExchangeZombieIconsUI01");
		Material mat2 = LoadUIMaterial("Zombie3D/UI/Materials/ExchangeZombieIconsUI02");
		m_KilledEnemyOrder = new List<int>();
		foreach (int key in gameState.m_KilledEnemyInfo.Keys)
		{
			EnemyType enemy_type = (EnemyType)key;
			m_KilledEnemyOrder.Add(key);
			int texture_index = 1;
			Rect zombieIconTex = GetZombieIconTex(enemy_type, gameState.m_BattleMapId, ref texture_index);
			switch (texture_index)
			{
			case 1:
				uIImage = UIUtils.BuildImage(0, new Rect(0f, 0f, zombieIconTex.width, zombieIconTex.height), mat, zombieIconTex, new Vector2(zombieIconTex.width, zombieIconTex.height));
				m_KillInfoScrollBar.Add(uIImage);
				break;
			case 2:
				uIImage = UIUtils.BuildImage(0, new Rect(0f, 0f, zombieIconTex.width, zombieIconTex.height), mat2, zombieIconTex, new Vector2(zombieIconTex.width, zombieIconTex.height));
				m_KillInfoScrollBar.Add(uIImage);
				break;
			}
		}
		m_KillInfoScrollBar.EnableScroll();
		m_KillInfoScrollBar.SetMaskImage(m_MatExchangeUI, new Rect(0f, 0f, 1f, 1f), new Vector2(500f, 500f));
		m_KillInfoScrollBar.Show();
		m_uiGroup.Add(m_KillInfoScrollBar);
		if (m_KilledEnemyOrder.Count > 0)
		{
			FixedConfig.EnemyCfg enemyCfg = ConfigManager.Instance().GetFixedConfig().GetEnemyCfg(m_KilledEnemyOrder[0]);
			m_CurKilledEnemyInfoName = UIUtils.BuildUIText(0, new Rect(200f, 290f, 300f, 30f), UIText.enAlignStyle.center);
			m_CurKilledEnemyInfoName.Set("Zombie3D/Font/037-CAI978-18", enemyCfg.name, new Color(46f / 51f, 0.6392157f, 0.16078432f, 1f));
			m_uiGroup.Add(m_CurKilledEnemyInfoName);
			int num2 = (int)gameState.m_KilledEnemyInfo[m_KilledEnemyOrder[0]];
			Rect scrRect = new Rect(223f, 245f, 200f, 44f);
			m_CurKilledEnemyInfoNum = UIUtils.BuildUIText(0, scrRect, UIText.enAlignStyle.right);
			m_CurKilledEnemyInfoNum.Set("Zombie3D/Font/037-CAI978-27", num2.ToString(), Color.red);
			m_uiGroup.Add(m_CurKilledEnemyInfoNum);
			float textWidth = mgrFont.Instance().getFont("Zombie3D/Font/037-CAI978-27").GetTextWidth(num2.ToString());
			float num3 = 5f;
			if (AutoUI.IsRetain == AutoUI.RESOLUTION.LOWDEFINITION)
			{
				num3 = 30f;
			}
			m_CurKilledEnemyInfo = UIUtils.BuildUIText(0, new Rect(423f - textWidth - 200f - num3, 258f, 200f, 25f), UIText.enAlignStyle.right);
			m_CurKilledEnemyInfo.Set("Zombie3D/Font/037-CAI978-18", "KILLED: ", new Color(46f / 51f, 0.6392157f, 0.16078432f, 1f));
			m_uiGroup.Add(m_CurKilledEnemyInfo);
		}
		if (m_FrendeExternExp > 0 && gameState.m_SelectFriendIndex > 0)
		{
			gameState.AddGCPlayWithNetFriendTimes();
		}
		uIClickButton = UIUtils.BuildClickButton(6015, new Rect(690f, 60f, 188f, 68f), m_MatExchangeUI, new Rect(386f, 888f, 188f, 68f), new Rect(386f, 956f, 188f, 68f), new Rect(386f, 888f, 188f, 68f), new Vector2(188f, 68f));
		if (GameApp.GetInstance().GetGameState().GetBattleStar() > 1)
		{
			uIClickButton.Visible = false;
		}
		m_uiGroup.Add(uIClickButton);
	}

	public void SetupExchangeBattleInfoUI(uiGroup group)
	{
		string text = "BATTLE TIME: ";
		int num = 30;
		UIText uIText = UIUtils.BuildUIText(0, new Rect(520f, 380 + num, 200f, 25f), UIText.enAlignStyle.left);
		uIText.Set("Zombie3D/Font/037-CAI978-18", text, new Color(46f / 51f, 0.6392157f, 0.16078432f, 1f));
		group.Add(uIText);
		float textWidth = mgrFont.Instance().getFont("Zombie3D/Font/037-CAI978-18").GetTextWidth(text);
		Debug.Log("SetupExchangeBattleInfoUI - " + textWidth);
		string text2 = "00:00:00";
		text2 = UtilsEx.TimeToStr_HMS((long)gameState.m_BattleTime);
		Rect scrRect = new Rect(520f + textWidth + 15f, 380 + num, 200f, 25f);
		if (AutoUI.IsRetain == AutoUI.RESOLUTION.LOWDEFINITION)
		{
			scrRect = new Rect((260f + textWidth + 20f) * 2f, 380 + num, 200f, 25f);
		}
		uIText = UIUtils.BuildUIText(0, scrRect, UIText.enAlignStyle.left);
		uIText.Set("Zombie3D/Font/037-CAI978-18", text2.ToString(), Color.white);
		group.Add(uIText);
		uIText = UIUtils.BuildUIText(0, new Rect(520f, 350 + num, 400f, 25f), UIText.enAlignStyle.left);
		uIText.Set("Zombie3D/Font/037-CAI978-18", "TARGETS DESTROYED: ", new Color(46f / 51f, 0.6392157f, 0.16078432f, 1f));
		group.Add(uIText);
		float textWidth2 = mgrFont.Instance().getFont("Zombie3D/Font/037-CAI978-18").GetTextWidth("TARGETS DESTROYED: ");
		Rect scrRect2 = new Rect(520f + textWidth2 + 20f, 350 + num, 200f, 25f);
		if (AutoUI.IsRetain == AutoUI.RESOLUTION.LOWDEFINITION)
		{
			scrRect2 = new Rect((260f + textWidth2 + 20f) * 2f, 350 + num, 200f, 25f);
		}
		uIText = UIUtils.BuildUIText(0, scrRect2, UIText.enAlignStyle.left);
		uIText.Set("Zombie3D/Font/037-CAI978-18", gameState.Killed.ToString(), Color.white);
		group.Add(uIText);
		if (gameState.m_BattleMapId != 3 && gameState.m_BattleMapId != 4 && gameState.m_BattleMapId != 5)
		{
			uIText = UIUtils.BuildUIText(0, new Rect(520f, 290 + num, 330f, 25f), UIText.enAlignStyle.left);
			uIText.Set("Zombie3D/Font/037-CAI978-18", gameState.m_BattleWaves.ToString(), Color.white);
			group.Add(uIText);
			float textWidth3 = mgrFont.Instance().getFont("Zombie3D/Font/037-CAI978-18").GetTextWidth(gameState.m_BattleWaves.ToString());
			uIText = UIUtils.BuildUIText(0, new Rect(520f + textWidth3 + 10f, 290 + num, 400f, 25f), UIText.enAlignStyle.left);
			uIText.Set("Zombie3D/Font/037-CAI978-18", " WAVES CLEARED!", new Color(46f / 51f, 0.6392157f, 0.16078432f, 1f));
			group.Add(uIText);
			uIText = UIUtils.BuildUIText(0, new Rect(520f, 260 + num, 400f, 25f), UIText.enAlignStyle.left);
			uIText.Set("Zombie3D/Font/037-CAI978-18", "PERFECT WAVES: ", new Color(46f / 51f, 0.6392157f, 0.16078432f, 1f));
			group.Add(uIText);
			float textWidth4 = mgrFont.Instance().getFont("Zombie3D/Font/037-CAI978-18").GetTextWidth("PERFECT WAVES: ");
			Rect scrRect3 = new Rect(520f + textWidth4 + 20f, 260 + num, 200f, 25f);
			if (AutoUI.IsRetain == AutoUI.RESOLUTION.LOWDEFINITION)
			{
				scrRect3 = new Rect((260f + textWidth4 + 20f) * 2f, 260 + num, 200f, 25f);
			}
			uIText = UIUtils.BuildUIText(0, scrRect3, UIText.enAlignStyle.left);
			uIText.Set("Zombie3D/Font/037-CAI978-18", gameState.m_BattlePerfectWaves.ToString(), Color.white);
			group.Add(uIText);
		}
		if (gameState.m_bIsSurvivalMode || !gameState.m_IsPassStage)
		{
			return;
		}
		Rect scrRect4 = new Rect(490f, 130 + num, 385f, 134f);
		UIImage control = UIUtils.BuildImage(0, scrRect4, m_MatExchangeUI, new Rect(0f, 393f, 385f, 134f), new Vector2(385f, 134f));
		group.Add(control);
		if (GameApp.GetInstance().GetGameState().GetBattleStar() <= 0)
		{
			return;
		}
		m_BattleStarTimer = 0f;
		m_BattleStarImg1 = UIUtils.BuildImage(0, new Rect(506f, 154 + num, 98f, 93f), m_MatExchangeUI, new Rect(417f, 392f, 98f, 93f), new Vector2(98f, 93f));
		group.Add(m_BattleStarImg1);
		m_BattleStarImg1.Visible = false;
		m_BattleStarText1 = UIUtils.BuildUIText(0, new Rect(600f, 100 + num, 200f, 30f), UIText.enAlignStyle.left);
		m_BattleStarText1.Set("Zombie3D/Font/037-CAI978-18", "Stage complete", Constant.TextCommonColor);
		group.Add(m_BattleStarText1);
		m_BattleStarText1.Visible = false;
		m_BattleStarImg2 = UIUtils.BuildImage(0, new Rect(634f, 154 + num, 98f, 93f), m_MatExchangeUI, new Rect(417f, 392f, 98f, 93f), new Vector2(98f, 93f));
		group.Add(m_BattleStarImg2);
		m_BattleStarImg2.Visible = false;
		m_BattleStarText2 = UIUtils.BuildUIText(0, new Rect(600f, 100 + num, 200f, 30f), UIText.enAlignStyle.left);
		if (GameApp.GetInstance().GetGameState().GetBattleStar() == 2)
		{
			if (gameState.m_IsFastPassBattle)
			{
				m_BattleStarText2.Set("Zombie3D/Font/037-CAI978-18", "Speed Run", Constant.TextCommonColor);
			}
			else if (gameState.m_IsNoBruisePassBattle)
			{
				m_BattleStarText2.Set("Zombie3D/Font/037-CAI978-18", "Untouched", Constant.TextCommonColor);
			}
		}
		else if (GameApp.GetInstance().GetGameState().GetBattleStar() > 2)
		{
			m_BattleStarText2.Set("Zombie3D/Font/037-CAI978-18", "Speed Run", Constant.TextCommonColor);
		}
		group.Add(m_BattleStarText2);
		m_BattleStarText2.Visible = false;
		m_BattleStarImg3 = UIUtils.BuildImage(0, new Rect(762f, 154 + num, 98f, 93f), m_MatExchangeUI, new Rect(417f, 392f, 98f, 93f), new Vector2(98f, 93f));
		group.Add(m_BattleStarImg3);
		m_BattleStarImg3.Visible = false;
		m_BattleStarText3 = UIUtils.BuildUIText(0, new Rect(600f, 100 + num, 200f, 30f), UIText.enAlignStyle.left);
		m_BattleStarText3.Set("Zombie3D/Font/037-CAI978-18", "Untouched", Constant.TextCommonColor);
		group.Add(m_BattleStarText3);
		m_BattleStarText3.Visible = false;
		m_BattleStarAnimation = new UIAnimationControl();
		m_BattleStarAnimation.Id = 0;
		m_BattleStarAnimation.SetAnimationsPageCount(5);
		m_BattleStarAnimation.Rect = AutoUI.AutoRect(new Rect(442f, 83 + num, 238f, 235f));
		m_BattleStarAnimation.SetTexture(0, m_MatExchangeUI01, AutoUI.AutoRect(new Rect(0f, 0f, 119f, 117f)), AutoUI.AutoSize(new Vector2(238f, 235f)));
		m_BattleStarAnimation.SetTexture(1, m_MatExchangeUI01, AutoUI.AutoRect(new Rect(119f, 0f, 119f, 117f)), AutoUI.AutoSize(new Vector2(238f, 235f)));
		m_BattleStarAnimation.SetTexture(2, m_MatExchangeUI01, AutoUI.AutoRect(new Rect(238f, 0f, 119f, 117f)), AutoUI.AutoSize(new Vector2(238f, 235f)));
		m_BattleStarAnimation.SetTexture(3, m_MatExchangeUI01, AutoUI.AutoRect(new Rect(0f, 117f, 119f, 117f)), AutoUI.AutoSize(new Vector2(238f, 235f)));
		m_BattleStarAnimation.SetTexture(4, m_MatExchangeUI01, AutoUI.AutoRect(new Rect(119f, 117f, 119f, 117f)), AutoUI.AutoSize(new Vector2(238f, 235f)));
		m_BattleStarAnimation.SetTimeInterval(0.2f);
		m_BattleStarAnimation.SetLoopCount(1);
		m_BattleStarAnimation.Visible = true;
		m_BattleStarAnimation.Enable = true;
		group.Add(m_BattleStarAnimation);
	}

	public void SetupRankUpEffectUI(bool bShow)
	{
		if (m_LevelUpEffect01 != null)
		{
			m_LevelUpEffect01.Clear();
			m_LevelUpEffect01 = null;
		}
		m_LevelUpEffectBlk = new UIBlock();
		m_LevelUpEffectBlk.Rect = new Rect(0f, 0f, 960f, 640f);
		m_UIManager.Add(m_LevelUpEffectBlk);
		m_LevelUpEffect01 = new UIEffect01(m_UIManager);
		UIEffect01.Effect01DataItem effect01DataItem = null;
		effect01DataItem = new UIEffect01.Effect01DataItem();
		effect01DataItem.position = new Vector2(945f, 320f);
		effect01DataItem.time = 0.5f;
		m_LevelUpEffect01.AddData(effect01DataItem);
		effect01DataItem = new UIEffect01.Effect01DataItem();
		effect01DataItem.position = new Vector2(285f, 320f);
		effect01DataItem.time = 0.6f;
		m_LevelUpEffect01.AddData(effect01DataItem);
		effect01DataItem = new UIEffect01.Effect01DataItem();
		effect01DataItem.position = new Vector2(285f, 320f);
		effect01DataItem.time = 0.1f;
		m_LevelUpEffect01.AddData(effect01DataItem);
		effect01DataItem = new UIEffect01.Effect01DataItem();
		effect01DataItem.position = new Vector2(285f, 320f);
		effect01DataItem.time = 2.5f;
		m_LevelUpEffect01.AddData(effect01DataItem);
		UIGroupControl uIGroupControl = UIUtils.BuildUIGroupControl(0, new Rect(0f, 320f, 500f, 500f));
		m_ContratulationsImg = UIUtils.BuildImage(0, new Rect(0f, 320f, 388f, 64f), m_MatExchangeUI, new Rect(636f, 565f, 388f, 64f), new Vector2(388f, 64f));
		uIGroupControl.Add(m_ContratulationsImg);
		m_LevelUpEffect01.Group = uIGroupControl;
		m_LevelUpEffect01.Update(Time.deltaTime);
		if (m_LevelUpEffect02 != null)
		{
			m_LevelUpEffect02.Clear();
			m_LevelUpEffect02 = null;
		}
		m_LevelUpEffect02 = new UIEffect01(m_UIManager);
		UIEffect01.Effect01DataItem effect01DataItem2 = null;
		effect01DataItem2 = new UIEffect01.Effect01DataItem();
		effect01DataItem2.position = new Vector2(845f, 305f);
		effect01DataItem2.time = 0.5f;
		m_LevelUpEffect02.AddData(effect01DataItem2);
		effect01DataItem2 = new UIEffect01.Effect01DataItem();
		effect01DataItem2.position = new Vector2(0f, 305f);
		effect01DataItem2.time = 0.6f;
		m_LevelUpEffect02.AddData(effect01DataItem2);
		effect01DataItem2 = new UIEffect01.Effect01DataItem();
		effect01DataItem2.position = new Vector2(0f, 305f);
		effect01DataItem2.time = 3f;
		m_LevelUpEffect02.AddData(effect01DataItem2);
		UIGroupControl uIGroupControl2 = UIUtils.BuildUIGroupControl(0, new Rect(0f, 305f, 500f, 500f));
		m_LineImg = UIUtils.BuildImage(0, new Rect(0f, 305f, 960f, 10f), m_MatExchangeUI, new Rect(0f, 784f, 960f, 10f), new Vector2(960f, 10f));
		uIGroupControl2.Add(m_LineImg);
		m_LevelUpEffect02.Group = uIGroupControl2;
		m_LevelUpEffect02.Update(Time.deltaTime);
		if (m_LevelUpEffect03 != null)
		{
			m_LevelUpEffect03.Clear();
			m_LevelUpEffect03 = null;
		}
		Rect rankNameTex = GetRankNameTex();
		m_LevelUpEffect03 = new UIEffect01(m_UIManager);
		UIEffect01.Effect01DataItem effect01DataItem3 = null;
		effect01DataItem3 = new UIEffect01.Effect01DataItem();
		effect01DataItem3.position = new Vector2(-1000f, 305f);
		effect01DataItem3.time = 0.1f;
		m_LevelUpEffect03.AddData(effect01DataItem3);
		effect01DataItem3 = new UIEffect01.Effect01DataItem();
		effect01DataItem3.position = new Vector2(480f - rankNameTex.width / 2f - 20f - 36f - 270f, 305f);
		effect01DataItem3.time = 0.6f;
		m_LevelUpEffect03.AddData(effect01DataItem3);
		effect01DataItem3 = new UIEffect01.Effect01DataItem();
		effect01DataItem3.position = new Vector2(480f - rankNameTex.width / 2f - 20f - 36f - 270f, 305f);
		effect01DataItem3.time = 3f;
		m_LevelUpEffect03.AddData(effect01DataItem3);
		UIGroupControl uIGroupControl3 = UIUtils.BuildUIGroupControl(0, new Rect(0f, 305f, 500f, 500f));
		m_LevelUpRankShow = UIUtils.BuildImage(0, new Rect(480f - rankNameTex.width / 2f, 263f, rankNameTex.width, rankNameTex.height), m_MatExchangeUI, rankNameTex, new Vector2(rankNameTex.width, rankNameTex.height));
		uIGroupControl3.Add(m_LevelUpRankShow);
		m_StarLeft = UIUtils.BuildImage(0, new Rect(480f - rankNameTex.width / 2f - 20f - 36f, 262f, 36f, 36f), m_MatExchangeUI, new Rect(950f, 242f, 36f, 36f), new Vector2(36f, 36f));
		uIGroupControl3.Add(m_StarLeft);
		m_StarRight = UIUtils.BuildImage(0, new Rect(480f + rankNameTex.width / 2f + 20f, 262f, 36f, 36f), m_MatExchangeUI, new Rect(950f, 242f, 36f, 36f), new Vector2(36f, 36f));
		uIGroupControl3.Add(m_StarRight);
		m_LevelUpEffect03.Group = uIGroupControl3;
		m_LevelUpEffect03.Update(Time.deltaTime);
	}

	public void SetupLevelUpEffectUI(bool bShow)
	{
		if (m_LevelUpEffect01 != null)
		{
			m_LevelUpEffect01.Clear();
			m_LevelUpEffect01 = null;
		}
		m_LevelUpEffectBlk = new UIBlock();
		m_LevelUpEffectBlk.Rect = new Rect(0f, 0f, 960f, 640f);
		m_UIManager.Add(m_LevelUpEffectBlk);
		m_LevelUpEffect01 = new UIEffect01(m_UIManager);
		UIEffect01.Effect01DataItem effect01DataItem = null;
		effect01DataItem = new UIEffect01.Effect01DataItem();
		effect01DataItem.position = new Vector2(945f, 320f);
		effect01DataItem.time = 0.5f;
		m_LevelUpEffect01.AddData(effect01DataItem);
		effect01DataItem = new UIEffect01.Effect01DataItem();
		effect01DataItem.position = new Vector2(320f, 320f);
		effect01DataItem.time = 0.6f;
		m_LevelUpEffect01.AddData(effect01DataItem);
		effect01DataItem = new UIEffect01.Effect01DataItem();
		effect01DataItem.position = new Vector2(320f, 320f);
		effect01DataItem.time = 0.1f;
		m_LevelUpEffect01.AddData(effect01DataItem);
		effect01DataItem = new UIEffect01.Effect01DataItem();
		effect01DataItem.position = new Vector2(320f, 320f);
		effect01DataItem.time = 2.5f;
		m_LevelUpEffect01.AddData(effect01DataItem);
		UIGroupControl uIGroupControl = UIUtils.BuildUIGroupControl(0, new Rect(0f, 320f, 500f, 500f));
		m_ContratulationsImg = UIUtils.BuildImage(0, new Rect(0f, 320f, 323f, 52f), m_MatExchangeUI, new Rect(701f, 972f, 323f, 52f), new Vector2(323f, 52f));
		uIGroupControl.Add(m_ContratulationsImg);
		m_LevelUpEffect01.Group = uIGroupControl;
		m_LevelUpEffect01.Update(Time.deltaTime);
		if (m_LevelUpEffect02 != null)
		{
			m_LevelUpEffect02.Clear();
			m_LevelUpEffect02 = null;
		}
		m_LevelUpEffect02 = new UIEffect01(m_UIManager);
		UIEffect01.Effect01DataItem effect01DataItem2 = null;
		effect01DataItem2 = new UIEffect01.Effect01DataItem();
		effect01DataItem2.position = new Vector2(845f, 305f);
		effect01DataItem2.time = 0.5f;
		m_LevelUpEffect02.AddData(effect01DataItem2);
		effect01DataItem2 = new UIEffect01.Effect01DataItem();
		effect01DataItem2.position = new Vector2(0f, 305f);
		effect01DataItem2.time = 0.6f;
		m_LevelUpEffect02.AddData(effect01DataItem2);
		effect01DataItem2 = new UIEffect01.Effect01DataItem();
		effect01DataItem2.position = new Vector2(0f, 305f);
		effect01DataItem2.time = 3f;
		m_LevelUpEffect02.AddData(effect01DataItem2);
		UIGroupControl uIGroupControl2 = UIUtils.BuildUIGroupControl(0, new Rect(0f, 305f, 500f, 500f));
		m_LineImg = UIUtils.BuildImage(0, new Rect(0f, 305f, 960f, 10f), m_MatExchangeUI, new Rect(0f, 769f, 960f, 10f), new Vector2(960f, 10f));
		uIGroupControl2.Add(m_LineImg);
		m_LevelUpEffect02.Group = uIGroupControl2;
		m_LevelUpEffect02.Update(Time.deltaTime);
		UIGroupControl uIGroupControl3 = UIUtils.BuildUIGroupControl(0, new Rect(0f, 305f, 500f, 500f));
		m_LevelNum1 = null;
		m_LevelNum2 = null;
		m_LevelNum3 = null;
		float num = 50f;
		int playerLevel = gameState.GetPlayerLevel();
		string text = playerLevel.ToString();
		if (text.Length == 1)
		{
			Rect waveShowFontTexRect = GetWaveShowFontTexRect(playerLevel);
			m_LevelNum1 = UIUtils.BuildImage(0, new Rect(480f - waveShowFontTexRect.width / 2f, 255f, waveShowFontTexRect.width, waveShowFontTexRect.height), m_MatExchangeUI, waveShowFontTexRect, new Vector2(waveShowFontTexRect.width, waveShowFontTexRect.height));
			uIGroupControl3.Add(m_LevelNum1);
			num = waveShowFontTexRect.width;
		}
		else if (text.Length == 2)
		{
			int num2 = int.Parse(text[0].ToString());
			Rect waveShowFontTexRect2 = GetWaveShowFontTexRect(num2);
			int num3 = int.Parse(text[1].ToString());
			Rect waveShowFontTexRect3 = GetWaveShowFontTexRect(num3);
			num = waveShowFontTexRect2.width + waveShowFontTexRect3.width;
			Rect scrRect = new Rect(480f - (waveShowFontTexRect2.width + waveShowFontTexRect3.width) / 2f, 255f, waveShowFontTexRect2.width, waveShowFontTexRect2.height);
			m_LevelNum1 = UIUtils.BuildImage(0, scrRect, m_MatExchangeUI, waveShowFontTexRect2, new Vector2(waveShowFontTexRect2.width, waveShowFontTexRect2.height));
			uIGroupControl3.Add(m_LevelNum1);
			m_LevelNum2 = UIUtils.BuildImage(0, new Rect(scrRect.x + scrRect.width, 255f, waveShowFontTexRect3.width, waveShowFontTexRect3.height), m_MatExchangeUI, waveShowFontTexRect3, new Vector2(waveShowFontTexRect3.width, waveShowFontTexRect3.height));
			uIGroupControl3.Add(m_LevelNum2);
		}
		else if (text.Length == 3)
		{
			int num4 = int.Parse(text[0].ToString());
			Rect waveShowFontTexRect4 = GetWaveShowFontTexRect(num4);
			int num5 = int.Parse(text[1].ToString());
			Rect waveShowFontTexRect5 = GetWaveShowFontTexRect(num5);
			int num6 = int.Parse(text[2].ToString());
			Rect waveShowFontTexRect6 = GetWaveShowFontTexRect(num6);
			num = waveShowFontTexRect4.width + waveShowFontTexRect5.width + waveShowFontTexRect6.width;
			Rect scrRect2 = new Rect(480f - (waveShowFontTexRect4.width + waveShowFontTexRect5.width + waveShowFontTexRect6.width) / 2f, 255f, waveShowFontTexRect4.width, waveShowFontTexRect4.height);
			m_LevelNum1 = UIUtils.BuildImage(0, scrRect2, m_MatExchangeUI, waveShowFontTexRect4, new Vector2(waveShowFontTexRect4.width, waveShowFontTexRect4.height));
			uIGroupControl3.Add(m_LevelNum1);
			Rect scrRect3 = new Rect(scrRect2.x + scrRect2.width, 255f, waveShowFontTexRect5.width, waveShowFontTexRect5.height);
			m_LevelNum2 = UIUtils.BuildImage(0, scrRect3, m_MatExchangeUI, waveShowFontTexRect5, new Vector2(waveShowFontTexRect5.width, waveShowFontTexRect5.height));
			uIGroupControl3.Add(m_LevelNum2);
			Rect scrRect4 = new Rect(scrRect2.x + scrRect2.width + scrRect3.width, 255f, waveShowFontTexRect6.width, waveShowFontTexRect6.height);
			m_LevelNum3 = UIUtils.BuildImage(0, scrRect4, m_MatExchangeUI, waveShowFontTexRect6, new Vector2(waveShowFontTexRect6.width, waveShowFontTexRect6.height));
			uIGroupControl3.Add(m_LevelNum3);
		}
		m_StarLeft = UIUtils.BuildImage(0, new Rect(480f - num / 2f - 20f - 36f, 262f, 36f, 36f), m_MatExchangeUI, new Rect(911f, 242f, 36f, 36f), new Vector2(36f, 36f));
		uIGroupControl3.Add(m_StarLeft);
		m_StarRight = UIUtils.BuildImage(0, new Rect(480f + num / 2f + 20f, 262f, 36f, 36f), m_MatExchangeUI, new Rect(911f, 242f, 36f, 36f), new Vector2(36f, 36f));
		uIGroupControl3.Add(m_StarRight);
		m_LevelUpEffect03 = new UIEffect01(m_UIManager);
		UIEffect01.Effect01DataItem effect01DataItem3 = null;
		effect01DataItem3 = new UIEffect01.Effect01DataItem();
		effect01DataItem3.position = new Vector2(-1000f, 305f);
		effect01DataItem3.time = 0.1f;
		m_LevelUpEffect03.AddData(effect01DataItem3);
		effect01DataItem3 = new UIEffect01.Effect01DataItem();
		effect01DataItem3.position = new Vector2(480f - num / 2f - 20f - 36f - 390f, 305f);
		effect01DataItem3.time = 0.6f;
		m_LevelUpEffect03.AddData(effect01DataItem3);
		effect01DataItem3 = new UIEffect01.Effect01DataItem();
		effect01DataItem3.position = new Vector2(480f - num / 2f - 20f - 36f - 390f, 305f);
		effect01DataItem3.time = 3f;
		m_LevelUpEffect03.AddData(effect01DataItem3);
		m_LevelUpEffect03.Group = uIGroupControl3;
		m_LevelUpEffect03.Update(Time.deltaTime);
	}

	public void SetupGoldMove02Effect()
	{
		m_EffectGoldMove02 = new UIEffect01(m_UIManager);
		UIEffect01.Effect01DataItem effect01DataItem = null;
		UIGroupControl uIGroupControl = UIUtils.BuildUIGroupControl(0, new Rect(0f, 0f, 960f, 640f));
		effect01DataItem = new UIEffect01.Effect01DataItem();
		effect01DataItem.position = new Vector2(36f, 413f);
		effect01DataItem.time = 0.6f;
		m_EffectGoldMove02.AddData(effect01DataItem);
		effect01DataItem = new UIEffect01.Effect01DataItem();
		effect01DataItem.position = new Vector2(413f, 587f);
		effect01DataItem.time = 0.6f;
		m_EffectGoldMove02.AddData(effect01DataItem);
		uIGroupControl.Rect = AutoUI.AutoRect(new Rect(520f, 400f, 94f, 79f));
		UIImage control = UIUtils.BuildImage(0, new Rect(520f, 400f, 94f, 79f), m_MatExchangeUI, new Rect(911f, 88f, 94f, 79f), new Vector2(94f, 79f));
		uIGroupControl.Add(control);
		m_EffectGoldMove02.Group = uIGroupControl;
		m_EffectGoldMove02.Update(Time.deltaTime);
	}

	public void SetupExpMove02Effect()
	{
		m_EffectExpMove02 = new UIEffect01(m_UIManager);
		UIEffect01.Effect01DataItem effect01DataItem = null;
		UIGroupControl uIGroupControl = UIUtils.BuildUIGroupControl(0, new Rect(0f, 0f, 960f, 640f));
		effect01DataItem = new UIEffect01.Effect01DataItem();
		effect01DataItem.position = new Vector2(484f, 413f);
		effect01DataItem.time = 0.6f;
		m_EffectExpMove02.AddData(effect01DataItem);
		effect01DataItem = new UIEffect01.Effect01DataItem();
		effect01DataItem.position = new Vector2(90f, 587f);
		effect01DataItem.time = 0.6f;
		m_EffectExpMove02.AddData(effect01DataItem);
		uIGroupControl.Rect = AutoUI.AutoRect(new Rect(520f, 132f, 95f, 75f));
		UIImage control = UIUtils.BuildImage(0, new Rect(520f, 132f, 95f, 75f), m_MatExchangeUI, new Rect(911f, 167f, 95f, 75f), new Vector2(95f, 75f));
		uIGroupControl.Add(control);
		m_EffectExpMove02.Group = uIGroupControl;
		m_EffectExpMove02.Update(Time.deltaTime);
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

	public Rect GetZombieIconTex(EnemyType enemy_type, int map_index, ref int texture_index)
	{
		Rect result = default(Rect);
		switch (enemy_type)
		{
		case EnemyType.E_ZOMBIE:
			texture_index = 1;
			result = new Rect(0f, 0f, 379f, 198f);
			if (map_index == 2 || map_index == 102)
			{
				result = new Rect(379f, 0f, 379f, 198f);
			}
			break;
		case EnemyType.E_BOOMER:
			texture_index = 1;
			result = new Rect(0f, 198f, 379f, 198f);
			if (map_index == 2 || map_index == 102)
			{
				result = new Rect(379f, 198f, 379f, 198f);
			}
			break;
		case EnemyType.E_SWAT:
			texture_index = 1;
			result = new Rect(0f, 396f, 379f, 198f);
			if (map_index == 2 || map_index == 102)
			{
				result = new Rect(379f, 396f, 379f, 198f);
			}
			break;
		case EnemyType.E_LAVA:
			texture_index = 1;
			result = new Rect(0f, 594f, 379f, 198f);
			break;
		case EnemyType.E_INFECTER:
			texture_index = 1;
			result = new Rect(379f, 594f, 379f, 198f);
			break;
		case EnemyType.E_SPIDER:
			texture_index = 1;
			result = new Rect(0f, 792f, 379f, 198f);
			break;
		case EnemyType.E_HUNTER:
			texture_index = 1;
			result = new Rect(379f, 792f, 379f, 198f);
			break;
		case EnemyType.E_LASER:
			texture_index = 2;
			result = new Rect(0f, 0f, 379f, 198f);
			break;
		case EnemyType.E_BATCHER:
			texture_index = 2;
			result = new Rect(379f, 0f, 379f, 198f);
			break;
		case EnemyType.E_TRACKER:
			texture_index = 2;
			result = new Rect(0f, 198f, 379f, 198f);
			break;
		case EnemyType.E_TURRETER:
			texture_index = 2;
			result = new Rect(379f, 198f, 379f, 198f);
			break;
		case EnemyType.E_SPORE:
			texture_index = 2;
			result = new Rect(0f, 396f, 379f, 198f);
			break;
		case EnemyType.E_VAMPIREDOG:
			texture_index = 2;
			result = new Rect(379f, 594f, 379f, 198f);
			break;
		case EnemyType.E_HUNTER_II:
			texture_index = 2;
			result = new Rect(379f, 396f, 379f, 198f);
			break;
		case EnemyType.E_SPORE_II:
			texture_index = 2;
			result = new Rect(0f, 594f, 379f, 198f);
			break;
		}
		return result;
	}

	public Rect GetRankNameTex()
	{
		Rect result = default(Rect);
		int playerLevel = gameState.GetPlayerLevel();
		if (playerLevel >= 3 && playerLevel < 10)
		{
			result = new Rect(0f, 393f, 209f, 32f);
		}
		else if (playerLevel >= 10 && playerLevel < 20)
		{
			result = new Rect(0f, 427f, 209f, 32f);
		}
		else if (playerLevel >= 20 && playerLevel < 30)
		{
			result = new Rect(0f, 461f, 352f, 32f);
		}
		else if (playerLevel >= 30 && playerLevel < 40)
		{
			result = new Rect(0f, 495f, 188f, 32f);
		}
		else if (playerLevel >= 40 && playerLevel < 50)
		{
			result = new Rect(0f, 529f, 185f, 32f);
		}
		else if (playerLevel >= 50 && playerLevel < 60)
		{
			result = new Rect(0f, 563f, 294f, 32f);
		}
		else if (playerLevel >= 60 && playerLevel < 70)
		{
			result = new Rect(0f, 597f, 391f, 32f);
		}
		else if (playerLevel >= 70 && playerLevel < 80)
		{
			result = new Rect(0f, 631f, 329f, 32f);
		}
		else if (playerLevel >= 80 && playerLevel < 90)
		{
			result = new Rect(0f, 665f, 316f, 32f);
		}
		else if (playerLevel >= 90 && playerLevel < 100)
		{
			result = new Rect(0f, 699f, 519f, 32f);
		}
		else if (playerLevel >= 100 && playerLevel < 115)
		{
			result = new Rect(0f, 733f, 332f, 32f);
		}
		else if (playerLevel >= 115 && playerLevel < 130)
		{
			result = new Rect(521f, 698f, 471f, 32f);
		}
		else if (playerLevel >= 130 && playerLevel < 145)
		{
			result = new Rect(213f, 393f, 470f, 32f);
		}
		else if (playerLevel >= 145 && playerLevel < 160)
		{
			result = new Rect(213f, 427f, 472f, 32f);
		}
		else if (playerLevel >= 160 && playerLevel < 175)
		{
			result = new Rect(338f, 733f, 470f, 32f);
		}
		else if (playerLevel >= 175 && playerLevel < 190)
		{
			result = new Rect(194f, 495f, 373f, 32f);
		}
		else if (playerLevel >= 190 && playerLevel < 205)
		{
			result = new Rect(194f, 529f, 315f, 32f);
		}
		else if (playerLevel >= 205 && playerLevel < 230)
		{
			result = new Rect(298f, 563f, 153f, 32f);
		}
		else if (playerLevel >= 230)
		{
			result = new Rect(360f, 461f, 129f, 32f);
		}
		return result;
	}

	public Rect GetWaveShowFontTexRect(int num)
	{
		switch (num)
		{
		case 0:
			return new Rect(413f, 638f, 46f, 49f);
		case 1:
			return new Rect(482f, 638f, 34f, 49f);
		case 2:
			return new Rect(535f, 638f, 50f, 49f);
		case 3:
			return new Rect(598f, 638f, 49f, 49f);
		case 4:
			return new Rect(658f, 638f, 50f, 49f);
		case 5:
			return new Rect(720f, 638f, 50f, 49f);
		case 6:
			return new Rect(785f, 638f, 50f, 49f);
		case 7:
			return new Rect(849f, 638f, 49f, 49f);
		case 8:
			return new Rect(909f, 638f, 48f, 49f);
		case 9:
			return new Rect(970f, 638f, 50f, 49f);
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

	public void SetupBattleStarDialog(bool bShow)
	{
		if (m_BattleStarDialog != null)
		{
			m_BattleStarDialog.Clear();
			m_BattleStarDialog = null;
		}
		if (bShow)
		{
			m_BattleStarDialog = new uiGroup(m_UIManager);
			Resources.UnloadUnusedAssets();
			UIImage control = UIUtils.BuildImage(0, new Rect(0f, 0f, 960f, 640f), m_MatDialog01, new Rect(600f, 1f, 1f, 1f), new Vector2(960f, 640f));
			m_BattleStarDialog.Add(control);
			control = UIUtils.BuildImage(0, new Rect(215f, 167f, 515f, 301f), m_MatDialog01, new Rect(0f, 378f, 515f, 301f), new Vector2(515f, 301f));
			m_BattleStarDialog.Add(control);
			string empty = string.Empty;
			empty = ((gameState.GetBattleStar() == 2) ? "Your 2 star rating gives 10% extra rewards!" : ((gameState.GetBattleStar() != 3) ? string.Empty : "Your 3 star rating gives 20% extra rewards!"));
			UIText uIText = UIUtils.BuildUIText(0, new Rect(280f, 230f, 420f, 150f), UIText.enAlignStyle.left);
			uIText.Set("Zombie3D/Font/037-CAI978-18", empty, Constant.TextCommonColor);
			m_BattleStarDialog.Add(uIText);
			UIClickButton control2 = UIUtils.BuildClickButton(6024, new Rect(385f, 152f, 191f, 62f), m_MatDialog01, new Rect(640f, 0f, 191f, 62f), new Rect(832f, 0f, 191f, 62f), new Rect(640f, 0f, 191f, 62f), new Vector2(191f, 62f));
			m_BattleStarDialog.Add(control2);
		}
	}

	public void SetupTweetUI(bool bShow)
	{
		if (m_uiTweetUI != null)
		{
			m_uiTweetUI.Clear();
			m_uiTweetUI = null;
		}
		if (bShow)
		{
			m_uiTweetUI = new uiGroup(m_UIManager);
			UIImage control = UIUtils.BuildImage(0, new Rect(0f, 0f, 960f, 640f), m_MatDialog01, new Rect(600f, 1f, 1f, 1f), new Vector2(960f, 640f));
			m_uiTweetUI.Add(control);
			control = UIUtils.BuildImage(0, new Rect(180f, 162f, 660f, 404f), m_MatExchangeUI02, new Rect(0f, 363f, 660f, 404f), new Vector2(660f, 404f));
			m_uiTweetUI.Add(control);
			m_strTweetText = GetTweetText();
			UIText uIText = UIUtils.BuildUIText(0, new Rect(298f, 285f, 480f, 190f), UIText.enAlignStyle.left);
			uIText.Set("Zombie3D/Font/037-CAI978-22", m_strTweetText, Constant.TextCommonColor);
			m_uiTweetUI.Add(uIText);
			UIClickButton control2 = UIUtils.BuildClickButton(6012, new Rect(256f, 190f, 191f, 62f), m_MatExchangeUI02, new Rect(1f, 938f, 191f, 62f), new Rect(197f, 938f, 191f, 62f), new Rect(1f, 938f, 191f, 62f), new Vector2(191f, 62f));
			m_uiTweetUI.Add(control2);
			control2 = UIUtils.BuildClickButton(6013, new Rect(618f, 190f, 191f, 62f), m_MatExchangeUI02, new Rect(659f, 796f, 191f, 62f), new Rect(659f, 858f, 191f, 62f), new Rect(659f, 796f, 191f, 62f), new Vector2(191f, 62f));
			m_uiTweetUI.Add(control2);
		}
	}

	public string GetTweetText(bool bNameUtf = false)
	{
		string empty = string.Empty;
		empty = empty + "3 Stars in " + Mathf.FloorToInt(gameState.m_BattleTime) + " seconds! ";
		string text = string.Empty;
		string gameCenterName = gameState.GameCenterName;
		if (!bNameUtf)
		{
			for (int i = 0; i < gameCenterName.Length; i++)
			{
				text = ((gameCenterName[i] <= '\u007f') ? (text + gameCenterName[i]) : (text + "*"));
			}
		}
		if (text != string.Empty)
		{
			empty = empty + text + ", ";
		}
		empty = empty + "Level " + gameState.GetPlayerLevel() + " ";
		Hashtable avatars = GameApp.GetInstance().GetGameState().GetAvatars();
		bool flag = false;
		foreach (Avatar key in avatars.Keys)
		{
			if (key.AvtType == Avatar.AvatarType.Head)
			{
				FixedConfig.AvatarCfg avatarCfg = ConfigManager.Instance().GetFixedConfig().GetAvatarCfg(key.SuiteType, key.AvtType);
				empty = empty + avatarCfg.name + ". ";
				break;
			}
		}
		FixedConfig.WeaponCfg weaponCfg = ConfigManager.Instance().GetFixedConfig().GetWeaponCfg(gameState.GetBattleWeapons()[0]);
		return empty + "Weapon of choice: " + weaponCfg.name + ". ";
	}
}
