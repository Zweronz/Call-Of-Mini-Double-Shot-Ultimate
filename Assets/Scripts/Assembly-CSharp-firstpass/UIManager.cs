using System.Collections;
using UnityEngine;

public class UIManager : MonoBehaviour, UIContainer
{
	public int LAYER;

	public int DEPTH;

	public bool CLEAR;

	private UIMesh m_UIMesh;

	private SpriteCamera m_SpriteCamera;

	private UIHandler m_UIHandler;

	private ArrayList m_Controls;

	private bool m_bCenterForiPad;

	private Vector2 m_ScreenOffset;

	public UIManager()
	{
		m_UIMesh = null;
		m_SpriteCamera = null;
		m_UIHandler = null;
		m_Controls = new ArrayList();
		m_bCenterForiPad = true;
		m_ScreenOffset = Vector2.zero;
		AutoUIResolution.Init();
		AutoUI.Init();
	}

	public void SetViewPortInCenter(bool bCenter)
	{
		m_bCenterForiPad = bCenter;
	}

	public void SetUIHandler(UIHandler ui_handler)
	{
		m_UIHandler = ui_handler;
	}

	public void Add(UIControl control)
	{
		m_Controls.Add(control);
		control.SetParent(this);
	}

	public void Remove(UIControl control)
	{
		m_Controls.Remove(control);
	}

	public void RemoveAll()
	{
		m_Controls.Clear();
	}

	public bool HandleInput(UITouchInner touch)
	{
		touch.position -= m_ScreenOffset;
		for (int num = m_Controls.Count - 1; num >= 0; num--)
		{
			UIControl uIControl = (UIControl)m_Controls[num];
			if (uIControl.Enable && uIControl.HandleInput(touch))
			{
				return true;
			}
		}
		return false;
	}

	public void Awake()
	{
	}

	public void SetParameter(int layer, int depth, bool clear)
	{
		LAYER = layer;
		DEPTH = depth;
		CLEAR = clear;
	}

	public void Start()
	{
		Initialize();
		InitializeSpriteMesh();
		InitializeSpriteCamera();
	}

	public void LateUpdate()
	{
		m_UIMesh.RemoveAll();
		for (int i = 0; i < m_Controls.Count; i++)
		{
			UIControl uIControl = (UIControl)m_Controls[i];
			uIControl.Update();
			if (uIControl.Visible)
			{
				uIControl.Draw();
			}
		}
		m_UIMesh.DoLateUpdate();
	}

	public void DrawSprite(UISprite sprite)
	{
		m_UIMesh.Add(sprite);
	}

	public void SendEvent(UIControl control, int command, float wparam, float lparam)
	{
		if (m_UIHandler != null)
		{
			m_UIHandler.HandleEvent(control, command, wparam, lparam);
		}
	}

	private void Initialize()
	{
		base.transform.position = Vector3.zero;
		base.transform.rotation = Quaternion.identity;
		base.transform.localScale = Vector3.one;
	}

	private void InitializeSpriteMesh()
	{
		GameObject gameObject = new GameObject("UIMesh");
		gameObject.transform.parent = base.gameObject.transform;
		m_UIMesh = (UIMesh)gameObject.AddComponent(typeof(UIMesh));
		m_UIMesh.Initialize(LAYER);
	}

	private void CameraMiddle()
	{
	}

	private void InitializeSpriteCamera()
	{
		GameObject gameObject = new GameObject("SpriteCamera");
		gameObject.transform.parent = base.gameObject.transform;
		m_SpriteCamera = (SpriteCamera)gameObject.AddComponent(typeof(SpriteCamera));
		m_SpriteCamera.Initialize(LAYER);
		m_SpriteCamera.SetClear(CLEAR);
		m_SpriteCamera.SetDepth(DEPTH);
		if (m_bCenterForiPad)
		{
			float num = (float)Screen.width / (float)Screen.height;
			float num2 = 1.5f;
			float num3 = 0f;
			float num4 = 0f;
			if (num > num2)
			{
				num3 = 640f / (float)Screen.height * (float)Screen.width - 960f;
				num4 = 0f - num3 / 2f;
				m_SpriteCamera.SetViewport(new Rect(num4, 0f, 640f / (float)Screen.height * (float)Screen.width, 640f));
				if (Application.isMobilePlatform)
				{
					iPhoneInputMgr.m_fOffectX = num4;
					iPhoneInputMgr.m_fScreenToRatio = 640f / (float)Screen.height;
				}
				else
				{
					WindowsInputMgr.m_fOffectX = num4;
					WindowsInputMgr.m_fScreenToRatio = 640f / (float)Screen.height;
				}
			}
			else if (num == num2)
			{
				m_SpriteCamera.SetViewport(new Rect(0f, 0f, 960f, 640f));
				if (Application.isMobilePlatform)
				{
					iPhoneInputMgr.m_fOffectX = 0f;
					iPhoneInputMgr.m_fOffectY = 0f;
				}
				else
				{
					WindowsInputMgr.m_fOffectX = 0f;
					WindowsInputMgr.m_fOffectY = 0f;
				}
			}
			else
			{
				num3 = 960f / (float)Screen.width * (float)Screen.height - 640f;
				num4 = 0f - num3 / 2f;
				m_SpriteCamera.SetViewport(new Rect(0f, num4, 960f, 960f / (float)Screen.width * (float)Screen.height));
				if (Application.isMobilePlatform)
				{
					iPhoneInputMgr.m_fOffectY = num4;
					iPhoneInputMgr.m_fScreenToRatio = 960f / (float)Screen.width;
				}
				else
				{
					WindowsInputMgr.m_fOffectY = num4;
					WindowsInputMgr.m_fScreenToRatio = 960f / (float)Screen.width;
				}
			}
		}
		else
		{
			m_SpriteCamera.SetViewport(new Rect(0f, 0f, Screen.width, Screen.height));
			if (Application.isMobilePlatform)
			{
				iPhoneInputMgr.m_fOffectX = 0f;
				iPhoneInputMgr.m_fOffectY = 0f;
				iPhoneInputMgr.m_fScreenToRatio = 1f;
			}
			else
			{
				WindowsInputMgr.m_fOffectX = 0f;
				WindowsInputMgr.m_fOffectY = 0f;
				WindowsInputMgr.m_fScreenToRatio = 1f;
			}
		}
	}
}
