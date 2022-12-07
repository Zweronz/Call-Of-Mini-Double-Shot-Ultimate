using UnityEngine;
using Zombie3D;

public class GameCenterUnlockOneViewControl : UIControl, UIHandler, UIContainer
{
	private UIImage m_ImgBg;

	private UIText m_Text;

	private float m_Timer;

	private float m_Timer1 = 0.5f;

	private float m_Timer2 = 3f;

	private float m_Timer3 = 0.5f;

	public GameCenterUnlockOneViewControl()
	{
		m_Timer = 0f;
	}

	public override void Update()
	{
		m_Timer += Time.deltaTime;
		if (m_Timer <= m_Timer1)
		{
			float num = -70f + m_Timer / m_Timer1 * 70f;
			m_ImgBg.Rect = AutoUI.AutoRect(new Rect(300f, num, 356f, 64f));
			m_Text.Rect = AutoUI.AutoRect(new Rect(375f, num + 18f, 500f, 30f));
		}
		else if (m_Timer >= m_Timer1 + m_Timer2)
		{
			if (m_Timer - (m_Timer1 + m_Timer2) < m_Timer3)
			{
				float num2 = -70f * ((m_Timer - (m_Timer1 + m_Timer2)) / m_Timer3);
				m_ImgBg.Rect = AutoUI.AutoRect(new Rect(300f, num2, 356f, 64f));
				m_Text.Rect = AutoUI.AutoRect(new Rect(375f, num2 + 18f, 500f, 30f));
			}
			else
			{
				m_ImgBg = null;
				m_Text = null;
				Enable = false;
				Visible = false;
			}
		}
	}

	public override void Draw()
	{
		if (m_ImgBg != null)
		{
			m_ImgBg.Draw();
		}
		if (m_Text != null)
		{
			m_Text.Draw();
		}
	}

	public void DrawSprite(UISprite sprite)
	{
		m_Parent.DrawSprite(sprite);
	}

	public void SendEvent(UIControl control, int command, float wparam, float lparam)
	{
	}

	public void HandleEvent(UIControl control, int command, float wparam, float lparam)
	{
	}

	public void SetupGameCenterUnlockUI(bool bShow, UIManager ui_manager, string strUnlockIntroduction)
	{
		Material mat = SceneUIManager.LoadUIMaterial("Zombie3D/UI/Materials/GameCenterUnlockUI");
		int num = 0;
		switch (strUnlockIntroduction.Contains("+") ? ((strUnlockIntroduction.Contains("tCrystals") || strUnlockIntroduction.Contains("Cash")) ? (strUnlockIntroduction.Contains("tCrystals") ? 1 : (strUnlockIntroduction.Contains("Cash") ? 2 : 0)) : 0) : 0)
		{
		case 1:
			m_ImgBg = UIUtils.BuildImage(0, new Rect(300f, -70f, 356f, 64f), mat, new Rect(0f, 64f, 356f, 64f), new Vector2(356f, 64f));
			m_ImgBg.SetParent(this);
			break;
		case 2:
			m_ImgBg = UIUtils.BuildImage(0, new Rect(300f, -70f, 356f, 64f), mat, new Rect(0f, 128f, 356f, 64f), new Vector2(356f, 64f));
			m_ImgBg.SetParent(this);
			break;
		default:
			m_ImgBg = UIUtils.BuildImage(0, new Rect(300f, -70f, 356f, 64f), mat, new Rect(0f, 0f, 356f, 64f), new Vector2(356f, 64f));
			m_ImgBg.SetParent(this);
			break;
		}
		m_Text = UIUtils.BuildUIText(0, new Rect(375f, -52f, 500f, 30f), UIText.enAlignStyle.left);
		m_Text.Set("Zombie3D/Font/037-CAI978-18", strUnlockIntroduction, Constant.TextCommonColor);
		m_Text.SetParent(this);
	}
}
