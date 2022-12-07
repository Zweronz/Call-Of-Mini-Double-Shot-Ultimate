using UnityEngine;
using Zombie3D;

public class GamePauseUIScript : MonoBehaviour, UIHandler
{
	public enum Controls
	{
		kIDGamePause = 1000,
		kIDGameResume = 1001,
		kIDGameSurrender = 1002,
		kIDGameMusicSwitch = 1003,
		kIDGameMusicSwitchImg = 1004,
		kIDGameSFXSwitch = 1005,
		kIDGameSFXSwitchImg = 1006,
		kIDGameSurrenderYes = 1007,
		kIDGameSurrenderNo = 1008,
		kIDLast = 1009
	}

	private UIManager m_UIManager;

	private string m_ui_material_path;

	protected Material m_MatGamePauseUI;

	protected Material m_MatDialog01;

	public uiGroup m_uiGroup;

	public uiGroup m_uiHintDialog;

	protected GameScene gameScene;

	protected Player player;

	protected float lastUpdateTime;

	protected bool uiInited;

	private void Start()
	{
		OpenClickPlugin.Show(false);
		ChartBoostAndroid.showInterstitial(null);
		gameScene = GameApp.GetInstance().GetGameScene();
		player = gameScene.GetPlayer();
		m_UIManager = base.gameObject.AddComponent<UIManager>() as UIManager;
		m_UIManager.SetParameter(8, 3, false);
		m_UIManager.SetUIHandler(this);
		UIUtils.BuildIpone5Frame(m_UIManager);
		m_MatGamePauseUI = LoadUIMaterial("Zombie3D/UI/Materials/GamePauseUI");
		Resources.UnloadUnusedAssets();
		uiInited = true;
		SetupGamePauseUI(true);
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
		if (!(Time.time - lastUpdateTime < 0.001f) && uiInited)
		{
			lastUpdateTime = Time.time;
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
		if (control.Id == 1001)
		{
			Time.timeScale = 1f;
			OpenClickPlugin.Hide();
			SceneUIManager.Instance().ChangeSceneUI(SceneUIManager.SceneUI.BattleUI, false);
		}
		else if (control.Id == 1002)
		{
			SetupHintDialog(true, 0, 1007, 1008, "Quitting now will bring you straight to the battle report. Proceed?");
		}
		else if (control.Id == 1003)
		{
			GameApp.GetInstance().GetGameState().MusicOn = !GameApp.GetInstance().GetGameState().MusicOn;
			SetupGamePauseUI(true);
		}
		else if (control.Id == 1005)
		{
			GameApp.GetInstance().GetGameState().SoundOn = !GameApp.GetInstance().GetGameState().SoundOn;
			SetupGamePauseUI(true);
		}
		else if (control.Id == 1007)
		{
			SetupHintDialog(false, 0, 0, 0, string.Empty);
			gameScene.BattleEnd();
			Time.timeScale = 1f;
		}
		else if (control.Id == 1008)
		{
			SetupHintDialog(false, 0, 0, 0, string.Empty);
		}
		else if (control.Id != 1009)
		{
		}
	}

	public void SetupGamePauseUI(bool bShow)
	{
		if (m_uiGroup != null)
		{
			m_uiGroup.Clear();
			m_uiGroup = null;
		}
		if (bShow)
		{
			m_uiGroup = new uiGroup(m_UIManager);
			Vector2 rect_size = AutoUIResolution.ToLongRectOfI5(new Vector2(960f, 640f));
			UIImage uIImage = UIUtils.BuildImage(0, AutoUIResolution.ToLongRectOfI5(new Rect(0f, 0f, 960f, 640f)), m_MatGamePauseUI, new Rect(1f, 1f, 1f, 1f), rect_size);
			uIImage.Rect = new Rect(0f, 0f, rect_size.x, rect_size.y);
			m_uiGroup.Add(uIImage);
			UIClickButton control = UIUtils.BuildClickButton(1001, new Rect(320f, 400f, 300f, 76f), m_MatGamePauseUI, new Rect(0f, 0f, 300f, 76f), new Rect(301f, 0f, 300f, 76f), new Rect(0f, 0f, 300f, 76f), new Vector2(300f, 76f));
			m_uiGroup.Add(control);
			control = UIUtils.BuildClickButton(1002, new Rect(320f, 305f, 300f, 76f), m_MatGamePauseUI, new Rect(0f, 76f, 300f, 76f), new Rect(301f, 76f, 300f, 76f), new Rect(0f, 76f, 300f, 76f), new Vector2(300f, 76f));
			m_uiGroup.Add(control);
			if (GameApp.GetInstance().GetGameState().MusicOn)
			{
				control = UIUtils.BuildClickButton(1003, new Rect(320f, 210f, 300f, 76f), m_MatGamePauseUI, new Rect(0f, 154f, 300f, 76f), new Rect(301f, 154f, 300f, 76f), new Rect(0f, 154f, 300f, 76f), new Vector2(300f, 76f));
				m_uiGroup.Add(control);
			}
			else
			{
				control = UIUtils.BuildClickButton(1003, new Rect(320f, 210f, 300f, 76f), m_MatGamePauseUI, new Rect(301f, 154f, 300f, 76f), new Rect(0f, 154f, 300f, 76f), new Rect(301f, 154f, 300f, 76f), new Vector2(300f, 76f));
				m_uiGroup.Add(control);
			}
			if (GameApp.GetInstance().GetGameState().SoundOn)
			{
				control = UIUtils.BuildClickButton(1005, new Rect(320f, 114f, 300f, 76f), m_MatGamePauseUI, new Rect(0f, 230f, 300f, 76f), new Rect(301f, 230f, 300f, 76f), new Rect(0f, 230f, 300f, 76f), new Vector2(300f, 76f));
				m_uiGroup.Add(control);
			}
			else
			{
				control = UIUtils.BuildClickButton(1005, new Rect(320f, 114f, 300f, 76f), m_MatGamePauseUI, new Rect(301f, 230f, 300f, 76f), new Rect(0f, 230f, 300f, 76f), new Rect(301f, 230f, 300f, 76f), new Vector2(300f, 76f));
				m_uiGroup.Add(control);
			}
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
			if (m_MatDialog01 == null)
			{
				m_MatDialog01 = LoadUIMaterial("Zombie3D/UI/Materials/Dialog01");
			}
			UIBlock control = UIUtils.BuildBlock(0, new Rect(0f, 0f, 960f, 640f));
			m_uiHintDialog.Add(control);
			UIImage uIImage = UIUtils.BuildImage(0, new Rect(0f, 0f, 960f, 640f), m_MatDialog01, new Rect(600f, 1f, 1f, 1f), new Vector2(960f, 640f));
			float num = 215f;
			float num2 = 167f;
			uIImage = UIUtils.BuildImage(0, new Rect(num, num2, 515f, 301f), m_MatDialog01, new Rect(0f, 378f, 515f, 301f), new Vector2(515f, 301f));
			m_uiHintDialog.Add(uIImage);
			UIText uIText = UIUtils.BuildUIText(0, new Rect(num + 40f, num2 + 40f, 420f, 130f), UIText.enAlignStyle.left);
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
