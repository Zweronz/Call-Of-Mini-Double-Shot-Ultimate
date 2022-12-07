using UnityEngine;

public class SceneUIManager : MonoBehaviour
{
	public enum SceneUI
	{
		LoginUI = 1000,
		ShopUI = 2000,
		BattleUI = 3000,
		GamePauseUI = 4000,
		ChoosePointsUI = 5000,
		FriendUI = 6000,
		OptionUI = 7000,
		ExchangeUI = 8000,
		GameStartUI = 9000,
		FriendsHireUI = 10000,
		BoostUI = 11000,
		NExchangUI = 12000,
		NBattleUI = 13000,
		NNetworkUI = 14000
	}

	public static MusicPlayerState MusicState_GameStartFirstPlayState = new GameStartFirstPlayState();

	public static MusicPlayerState MusicState_GameStartNotFirstPlayState = new GameStartNotFirstPlayState();

	public static MusicPlayerState MusicState_BattleAudioState = new BattleAudioState();

	public static MusicPlayerState MusicState_ChoosePointsAudioState = new ChoosePointsAudioState();

	public static MusicPlayerState MusicState_ShopAudioState = new ShopAudioState();

	public static MusicPlayerState MusicState_PerfectWaveAudioState = new BattlePerfectWaveAudioState();

	public static MusicPlayerState MusicState_ExchangeUIAudioState = new ExchangeUIAudioState();

	protected static SceneUIManager m_Instance = null;

	protected GameObject m_SceneUI;

	protected AudioSource m_UIClickAudio;

	protected FadeAnimationScript m_UIFade;

	protected bool m_bFadeIn;

	protected bool m_bFadeingIn;

	protected bool m_bFadeOut;

	protected bool m_bFadeingOut;

	protected Color m_FadeInStartColor = new Color(0f, 0f, 0f, 0f);

	protected Color m_FadeInEndColor = new Color(0f, 0f, 0f, 1f);

	protected Color m_FadeOutStartColor = new Color(0f, 0f, 0f, 1f);

	protected Color m_FadeOutEndColor = new Color(0f, 0f, 0f, 0f);

	protected SceneUI m_NextSceneUI = SceneUI.ShopUI;

	private PlayerUIShow m_PlayerUIShow;

	private float m_GCCleanStartTime = -1f;

	protected MusicPlayerState m_MusicPlayerState;

	protected AutoLoadFriendDataFromServer m_LoadFriendFromServer;

	protected AutoLoadHireFriendDataFromServer m_LoadHireFriendFromServer;

	public static SceneUIManager Instance()
	{
		if (m_Instance == null)
		{
			GameObject gameObject = new GameObject("SceneUIManager");
			m_Instance = gameObject.AddComponent(typeof(SceneUIManager)) as SceneUIManager;
		}
		return m_Instance;
	}

	private void Awake()
	{
		GameObject gameObject = Object.Instantiate(Resources.Load("Zombie3D/Misc/PlayerUIShow")) as GameObject;
		gameObject.transform.position = new Vector3(0f, -10000f, 0f);
		m_PlayerUIShow = gameObject.transform.Find("Player").GetComponent(typeof(PlayerUIShow)) as PlayerUIShow;
		Object.DontDestroyOnLoad(gameObject);
		ShowPlayerUIDDS(false);
		Object.DontDestroyOnLoad(base.gameObject);
		GameObject gameObject2 = Object.Instantiate(Resources.Load("Zombie3D/UI/Screen_UIFadeOut", typeof(GameObject)), new Vector3(0f, 1000f, 0f), Quaternion.identity) as GameObject;
		if (gameObject2 != null)
		{
			gameObject2.transform.parent = base.transform;
			m_UIFade = gameObject2.GetComponent(typeof(FadeAnimationScript)) as FadeAnimationScript;
		}
		else
		{
			Debug.LogError("Cannot Find Screen UI FadeOut GameObject!!!");
		}
		m_UIClickAudio = GameObject.Find("UIClickAudio").GetComponent(typeof(AudioSource)) as AudioSource;
		Object.DontDestroyOnLoad(m_UIClickAudio);
		m_bFadeIn = false;
		m_bFadeOut = false;
	}

	public PlayerUIShow ShowPlayerUIDDS(bool bShow)
	{
		if (m_PlayerUIShow != null)
		{
			m_PlayerUIShow.ShowPlayer(bShow);
			return m_PlayerUIShow;
		}
		return null;
	}

	private void Update()
	{
		if (m_UIFade != null)
		{
			if (m_bFadeIn)
			{
				m_UIFade.enableAlphaAnimation = true;
				m_UIFade.StartFade(m_FadeInStartColor, m_FadeInEndColor, 0.5f);
				m_bFadeIn = false;
				m_bFadeingIn = true;
			}
			else if (m_bFadeOut)
			{
				m_UIFade.enableAlphaAnimation = true;
				m_UIFade.StartFade(m_FadeOutStartColor, m_FadeOutEndColor, 0.5f);
				m_bFadeOut = false;
				m_bFadeingOut = true;
			}
			if (m_bFadeingOut && m_UIFade.FadeOutComplete())
			{
				m_UIFade.enableAlphaAnimation = false;
				m_bFadeingOut = false;
			}
			if (m_bFadeingIn && m_UIFade.FadeInComplete())
			{
				Object.Destroy(m_SceneUI);
				Resources.UnloadUnusedAssets();
				m_GCCleanStartTime = Time.time;
				m_bFadeOut = true;
				m_bFadeingIn = false;
			}
		}
		if (m_GCCleanStartTime >= 0f && Time.time - m_GCCleanStartTime > 0.5f)
		{
			ChangeNextSceneUI();
			m_GCCleanStartTime = -1f;
		}
		if (m_MusicPlayerState != null)
		{
			m_MusicPlayerState.Update();
		}
		if (m_LoadFriendFromServer != null)
		{
			if (AutoLoadFriendDataFromServer.LoadFriendDatasOver)
			{
				m_LoadFriendFromServer = null;
			}
			else
			{
				m_LoadFriendFromServer.Update();
			}
		}
		if (m_LoadHireFriendFromServer != null)
		{
			if (AutoLoadHireFriendDataFromServer.LoadFriendDatasOver)
			{
				m_LoadHireFriendFromServer = null;
			}
			else
			{
				m_LoadHireFriendFromServer.Update();
			}
		}
		int activeSignal = MiscPlugin.GetActiveSignal();
		if (activeSignal > 0)
		{
			GameCollectionInfoManager.Instance().GetCurrentInfo().AddGameActiveTimes(activeSignal);
		}
	}

	private void OnApplicationFocus(bool bFocus)
	{
	}

	public GameObject GetSceneUIObject()
	{
		return m_SceneUI;
	}

	public SceneUI GetSceneUIMode()
	{
		return m_NextSceneUI;
	}

	public void PlayClickAudio()
	{
		if (m_UIClickAudio != null)
		{
			m_UIClickAudio.transform.position = Camera.main.transform.position + Camera.main.transform.up * -1f;
			m_UIClickAudio.Play();
		}
	}

	public void ChangeSceneUI(SceneUI scene_ui_id, bool bFadeOut = true)
	{
		Debug.Log("ChangeSceneUI - " + scene_ui_id);
		m_NextSceneUI = scene_ui_id;
		if (bFadeOut)
		{
			m_bFadeIn = true;
			m_bFadeOut = false;
			m_bFadeingIn = false;
			m_bFadeingOut = false;
			UIManager uIManager = null;
			if (m_SceneUI != null)
			{
				uIManager = m_SceneUI.GetComponent(typeof(UIManager)) as UIManager;
				if (uIManager != null)
				{
					UIBlock uIBlock = new UIBlock();
					uIBlock.Rect = new Rect(0f, 0f, 960f, 640f);
					uIManager.Add(uIBlock);
				}
			}
		}
		else
		{
			ChangeNextSceneUI();
		}
	}

	private void ChangeNextSceneUI()
	{
		Debug.Log("ChangeNextSceneUI - " + m_NextSceneUI);
		if (m_NextSceneUI == SceneUI.LoginUI)
		{
			Object.Destroy(m_SceneUI);
			m_SceneUI = new GameObject("GUI_LoginUI", typeof(GameLoginUIScript));
			Object.DontDestroyOnLoad(m_SceneUI);
		}
		else if (m_NextSceneUI == SceneUI.GamePauseUI)
		{
			Object.Destroy(m_SceneUI);
			m_SceneUI = new GameObject("GUI_GamePauseUI", typeof(GamePauseUIScript));
			Object.DontDestroyOnLoad(m_SceneUI);
		}
		else if (m_NextSceneUI == SceneUI.BattleUI)
		{
			Object.Destroy(m_SceneUI);
			m_SceneUI = new GameObject("GUI_BattleUI", typeof(BattleUIScript));
			Object.DontDestroyOnLoad(m_SceneUI);
		}
		else if (m_NextSceneUI == SceneUI.ShopUI)
		{
			Object.Destroy(m_SceneUI);
			m_SceneUI = new GameObject("GUI_ShopUI", typeof(ShopUIScript));
			Object.DontDestroyOnLoad(m_SceneUI);
		}
		else if (m_NextSceneUI == SceneUI.ChoosePointsUI)
		{
			Object.Destroy(m_SceneUI);
			m_SceneUI = new GameObject("GUI_ChoosePointsUI", typeof(ChoosePointsUIScript));
			Object.DontDestroyOnLoad(m_SceneUI);
		}
		else if (m_NextSceneUI == SceneUI.FriendUI)
		{
			Object.Destroy(m_SceneUI);
			m_SceneUI = new GameObject("GUI_FriendsUI", typeof(FriendsUIScript));
			Object.DontDestroyOnLoad(m_SceneUI);
		}
		else if (m_NextSceneUI == SceneUI.OptionUI)
		{
			Object.Destroy(m_SceneUI);
			m_SceneUI = new GameObject("GUI_OptionUI", typeof(OptionUIScript));
			Object.DontDestroyOnLoad(m_SceneUI);
		}
		else if (m_NextSceneUI == SceneUI.ExchangeUI)
		{
			Object.Destroy(m_SceneUI);
			m_SceneUI = new GameObject("GUI_ExchangeUI", typeof(ExchangeUIScript));
			Object.DontDestroyOnLoad(m_SceneUI);
		}
		else if (m_NextSceneUI == SceneUI.GameStartUI)
		{
			Object.Destroy(m_SceneUI);
			m_SceneUI = new GameObject("GUI_GameStartUI", typeof(GameStartUIScript));
			Object.DontDestroyOnLoad(m_SceneUI);
		}
		else if (m_NextSceneUI == SceneUI.FriendsHireUI)
		{
			Object.Destroy(m_SceneUI);
			m_SceneUI = new GameObject("GUI_FriendsHireUI", typeof(FriendsHireUIScript));
			Object.DontDestroyOnLoad(m_SceneUI);
		}
		else if (m_NextSceneUI == SceneUI.BoostUI)
		{
			Object.Destroy(m_SceneUI);
			m_SceneUI = new GameObject("GUI_BoostUII", typeof(BoostUIScript));
			Object.DontDestroyOnLoad(m_SceneUI);
		}
		else if (m_NextSceneUI == SceneUI.NExchangUI)
		{
			Object.Destroy(m_SceneUI);
			m_SceneUI = new GameObject("GUI_NExchangUI", typeof(NExchangeUIScript));
			Object.DontDestroyOnLoad(m_SceneUI);
		}
		else if (m_NextSceneUI == SceneUI.NBattleUI)
		{
			Object.Destroy(m_SceneUI);
			m_SceneUI = new GameObject("GUI_NBattleUI", typeof(NBattleUIScript));
			Object.DontDestroyOnLoad(m_SceneUI);
		}
		else if (m_NextSceneUI == SceneUI.NNetworkUI)
		{
			Object.Destroy(m_SceneUI);
			m_SceneUI = new GameObject("GUI_NNetworkUI", typeof(NNetworkUIScript));
			Object.DontDestroyOnLoad(m_SceneUI);
		}
		Instance().SetupGameCenterUnlockListInfo();
		Resources.UnloadUnusedAssets();
	}

	public void SetMusicPlayerState(MusicPlayerState state)
	{
		if (m_MusicPlayerState != state)
		{
			if (m_MusicPlayerState != null)
			{
				m_MusicPlayerState.OnExit();
				m_MusicPlayerState = null;
			}
			m_MusicPlayerState = state;
			m_MusicPlayerState.OnEnter();
		}
	}

	public MusicPlayerState GetMusicPlayerState()
	{
		return m_MusicPlayerState;
	}

	public void ResetMusicPlayerState()
	{
		m_MusicPlayerState = null;
	}

	public void LoadFriendFromServer()
	{
		m_LoadFriendFromServer = null;
		m_LoadFriendFromServer = new AutoLoadFriendDataFromServer();
		m_LoadFriendFromServer.StartLoad();
	}

	public void LoadHireFriendFromServer()
	{
		m_LoadHireFriendFromServer = null;
		m_LoadHireFriendFromServer = new AutoLoadHireFriendDataFromServer();
		m_LoadHireFriendFromServer.StartLoad();
	}

	public void SetupGameCenterUnlockListInfo()
	{
		if (m_SceneUI.GetComponent(typeof(GameCenterMsgManager)) as GameCenterMsgManager == null)
		{
			m_SceneUI.AddComponent(typeof(GameCenterMsgManager));
		}
	}

	public void SetupGameCenterUnlockUI(string strContent)
	{
		UIManager component = m_SceneUI.GetComponent<UIManager>();
		if (component != null)
		{
			GameCenterUnlockOneViewControl gameCenterUnlockOneViewControl = new GameCenterUnlockOneViewControl();
			gameCenterUnlockOneViewControl.SetupGameCenterUnlockUI(true, component, strContent);
			component.Add(gameCenterUnlockOneViewControl);
		}
	}

	public static Material LoadUIMaterial(string name)
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
