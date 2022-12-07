using UnityEngine;

public class FPS : MonoBehaviour
{
	private int m_FrameCount;

	private float m_FrameTimer;

	private UIText m_uiFPS_white;

	private UIText m_uiFPS_black;

	public UIManager m_UIManager;

	public string fontName = "Zombie3D/Font/Arial12_bold";

	private void Start()
	{
		m_FrameCount = 0;
		m_FrameTimer = 0f;
		if (m_UIManager == null)
		{
			Debug.Log("ERROR: FPS script cannot find UIManager instance!~~");
			m_UIManager = base.gameObject.GetComponent("UIManager") as UIManager;
		}
	}

	private void Update()
	{
		m_FrameCount++;
		m_FrameTimer += Time.deltaTime;
		ShowFPS();
		if (m_UIManager == null)
		{
			m_UIManager = base.gameObject.GetComponent("UIManager") as UIManager;
		}
	}

	private void ShowFPS()
	{
		if (m_UIManager != null)
		{
			if (m_uiFPS_white == null)
			{
				m_uiFPS_white = UIUtils.BuildUIText(10000, new Rect(5f, 600f, 200f, 20f), UIText.enAlignStyle.left);
				m_uiFPS_white.Set(fontName, "FPS " + string.Format("{0:N2}", (float)m_FrameCount / m_FrameTimer) + ShowPing(), new Color(0f, 0f, 0f, 1f));
				m_UIManager.Add(m_uiFPS_white);
				m_uiFPS_black = UIUtils.BuildUIText(10000, new Rect(6f, 600f, 200f, 20f), UIText.enAlignStyle.left);
				m_uiFPS_black.Set(fontName, "FPS " + string.Format("{0:N2}", (float)m_FrameCount / m_FrameTimer) + ShowPing(), new Color(255f, 255f, 255f, 1f));
				m_UIManager.Add(m_uiFPS_black);
			}
			else
			{
				m_uiFPS_white.SetText("FPS " + string.Format("{0:N2}", (float)m_FrameCount / m_FrameTimer) + ShowPing());
				m_uiFPS_black.SetText("FPS " + string.Format("{0:N2}", (float)m_FrameCount / m_FrameTimer) + ShowPing());
			}
		}
	}

	private string ShowPing()
	{
		return string.Empty;
	}
}
