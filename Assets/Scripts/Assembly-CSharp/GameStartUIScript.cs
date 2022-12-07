using System.Collections;
using UnityEngine;

public class GameStartUIScript : MonoBehaviour, UIHandler
{
	public enum Controls
	{
		kIDStart = 1000,
		kIDImgBtn = 1001,
		kIDLast = 1002
	}

	private UIManager m_UIManager;

	private string m_ui_material_path;

	private static bool m_PluginsInitiated;

	protected Material m_MatLoginBg;

	public uiGroup m_uiGroup;

	private UIImage m_WheelAnimLeft;

	private UIImage m_WheelAnimRight;

	protected float lastUpdateTime;

	protected bool uiInited;

	private bool m_bChangeSceneToNext;

	private float m_Timer = -1f;

	private float m_ShowTime = 3f;

	private void Awake()
	{
		XAdManagerWrapper.SetVideoAdUrl("http://itunes.apple.com/us/app/isniper-3d-arctic-warfare/id533741523?mt=8");
		XAdManagerWrapper.ShowVideoAdLocal();
		if (!m_PluginsInitiated)
		{
			m_PluginsInitiated = true;
			TapjoyPlugin.RequestConnect("00264b65-2c88-4a33-9b96-f46d42e58974", "U5pwowtc9qV5d97XhbMx");
		}
	}

	private IEnumerator Start()
	{
		yield return 0;
		m_UIManager = base.gameObject.AddComponent<UIManager>() as UIManager;
		m_UIManager.SetParameter(8, 3, false);
		m_UIManager.SetUIHandler(this);
		UIUtils.BuildIpone5Frame(m_UIManager);
		m_MatLoginBg = LoadUIMaterial("Zombie3D/UI/Materials/GameLoginBgUI");
		Resources.UnloadUnusedAssets();
		uiInited = true;
		SetupGameStartUI(true);
		m_Timer = 0f;
		XAdManagerWrapper.SetImageAdUrl("http://itunes.apple.com/us/app/isniper-3d-arctic-warfare/id533741523?mt=8");
		XAdManagerWrapper.ShowImageAd();
		OpenClickPlugin.Show(true);
		ChartBoostAndroid.showInterstitial(null);
	}

	private void Update()
	{
		if (!m_bChangeSceneToNext && !(Time.time - lastUpdateTime < 0.001f) && uiInited)
		{
			float num = Time.time - lastUpdateTime;
			lastUpdateTime = Time.time;
			if (m_WheelAnimLeft != null)
			{
				m_WheelAnimLeft.SetRotation(Time.time);
			}
			if (m_WheelAnimRight != null)
			{
				m_WheelAnimRight.SetRotation(0f - Time.time);
			}
			m_Timer += num;
			if (m_Timer >= m_ShowTime)
			{
				m_bChangeSceneToNext = true;
				SceneUIManager.Instance().ChangeSceneUI(SceneUIManager.SceneUI.LoginUI);
				XAdManagerWrapper.HideImageAd();
				OpenClickPlugin.Hide();
			}
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
		if (control.Id == 1001)
		{
			Application.OpenURL("http://itunes.apple.com/us/app/isniper-3d-arctic-warfare/id533741523?mt=8");
		}
		else if (control.Id != 1002)
		{
		}
	}

	public void SetupGameStartUI(bool bShow)
	{
		if (m_uiGroup != null)
		{
			m_uiGroup.Clear();
			m_uiGroup = null;
		}
		if (bShow)
		{
			m_uiGroup = new uiGroup(m_UIManager);
			UIImage control = UIUtils.BuildImage(0, new Rect(0f, 0f, 960f, 640f), m_MatLoginBg, new Rect(0f, 0f, 960f, 640f), new Vector2(960f, 640f));
			m_uiGroup.Add(control);
			m_WheelAnimLeft = UIUtils.BuildImage(0, new Rect(95f, 50f, 138f, 138f), m_MatLoginBg, new Rect(777f, 642f, 138f, 138f), new Vector2(138f, 138f));
			m_uiGroup.Add(m_WheelAnimLeft);
			m_WheelAnimRight = UIUtils.BuildImage(0, new Rect(735f, 50f, 138f, 138f), m_MatLoginBg, new Rect(777f, 642f, 138f, 138f), new Vector2(138f, 138f));
			m_uiGroup.Add(m_WheelAnimRight);
			control = UIUtils.BuildImage(0, new Rect(111f, 42f, 775f, 205f), m_MatLoginBg, new Rect(0f, 642f, 775f, 205f), new Vector2(775f, 205f));
			m_uiGroup.Add(control);
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
