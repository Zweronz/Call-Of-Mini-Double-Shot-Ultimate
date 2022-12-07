using System;
using System.Collections;
using UnityEngine;

public class UICartoonAnimControl : UIControl, UIHandler, UIContainer
{
	private enum enControls
	{
		kIDLast = 0
	}

	private UIHandler m_UIHandler;

	private ArrayList m_Controls;

	public bool PlayEnd;

	protected FadeAnimationScript m_UIFade;

	protected bool m_bFadeIn;

	protected bool m_bFadeingIn;

	protected bool m_bFadeOut;

	protected bool m_bFadeingOut;

	protected Color m_FadeInStartColor = new Color(0f, 0f, 0f, 0f);

	protected Color m_FadeInEndColor = new Color(0f, 0f, 0f, 1f);

	protected Color m_FadeOutStartColor = new Color(0f, 0f, 0f, 1f);

	protected Color m_FadeOutEndColor = new Color(0f, 0f, 0f, 0f);

	private Material m_MatTransparentUI;

	private Material m_MatBlackAlphaChangeUI;

	private Material m_MatCartoonAnim01UI;

	private Material m_MatCartoonAnim03UI;

	private Material m_MatCartoonAnim04UI;

	private Material m_MatCartoonAnim05UI;

	private Material m_MatCartoonAnim06UI;

	private Material m_MatCartoonAnim07UI;

	private int m_PageIndex = 1;

	private UIImage m_PageBlackBg;

	private UIClickButton m_SkipBtn;

	private float m_LastSkipTime;

	private float m_SkipTimer = 1f;

	private UIImage m_Page1Bg;

	private UIImage m_Page1Item01;

	private float m_Page1Item01_Timer = -1f;

	private float m_Page1Item01_Time = 2f;

	private UIAnimationControl m_Page1BlackColorAnim;

	private float m_Page1BlackColorAnim_Timer = -1f;

	private float m_Page1BlackColorAnim_Time = 0.6f;

	private UIImage m_Page1ItemBgBlack;

	private UIImage m_Page1Item02Fg;

	private float m_Page1Item02Fg_Timer = -1f;

	private float m_Page1Item02Fg_Time = 0.3f;

	private UIImage m_Page1Item02;

	private float m_Page1Item02_Timer = -1f;

	private float m_Page1Item02_Time = 1f;

	private UIImage m_Page1Item03Fg;

	private float m_Page1Item03Fg_Timer = -1f;

	private float m_Page1Item03Fg_Time = 0.3f;

	private UIImage m_Page1Item03;

	private float m_Page1Item03_Timer = -1f;

	private float m_Page1Item03_Time = 1f;

	private UIImage m_Page1Item04Fg;

	private float m_Page1Item04Fg_Timer = -1f;

	private float m_Page1Item04Fg_Time = 0.3f;

	private UIImage m_Page1Item04;

	private float m_Page1Item04_Timer = -1f;

	private float m_Page1Item04_Time = 0.5f;

	private float m_Page1ItemLast_Timer = -1f;

	private float m_Page1ItemLast_Time = 1.5f;

	private UIImage m_Page2Bg;

	private UIImage m_Page2Item01;

	private float m_Page2Item01_Timer = -1f;

	private float m_Page2Item01_Time = 4f;

	private UIAnimationControl m_Page2BlackColorAnim;

	private float m_Page2BlackColorAnim_Timer = -1f;

	private float m_Page2BlackColorAnim_Time = 4f;

	private UIImage m_Page2ItemBgBlack;

	private UIImage m_Page2Item02;

	private float m_Page2Item02_Timer = -1f;

	private float m_Page2Item02_Time = 2f;

	private UIImage m_Page2Item03Fg;

	private float m_Page2Item03Fg_Timer = -1f;

	private float m_Page2Item03Fg_Time = 0.5f;

	private UIImage m_Page2Item03;

	private float m_Page2Item03_Timer = -1f;

	private float m_Page2Item03_Time = 0.5f;

	private UIImage m_Page2Item04Fg;

	private float m_Page2Item04Fg_Timer = -1f;

	private float m_Page2Item04Fg_Time = 0.3f;

	private UIImage m_Page2Item04;

	private float m_Page2Item04_Timer = -1f;

	private float m_Page2Item04_Time = 0.5f;

	private UIImage m_Page2Item04_01;

	private float m_Page2Item04_01_Timer = -1f;

	private float m_Page2Item04_01_Time = 0.5f;

	private UIImage m_Page2Item05;

	private float m_Page2Item05_Timer = -1f;

	private float m_Page2Item05_Time = 0.5f;

	private UIImage m_Page2Item05Fg;

	private float m_Page2Item05Fg_Timer = -1f;

	private float m_Page2Item05Fg_Time = 0.5f;

	private UIImage m_Page2Item06;

	private float m_Page2Item06_Timer = -1f;

	private float m_Page2Item06_Time = 1f;

	private UIImage m_Page2Item07;

	private float m_Page2Item07_Timer = -1f;

	private float m_Page2Item07_Time = 1f;

	private UIImage m_Page2Item08;

	private float m_Page2Item08_Timer = -1f;

	private float m_Page2Item08_Time = 2f;

	private UIImage m_Page2Item09;

	private float m_Page2Item09_Timer = -1f;

	private float m_Page2Item09_Time = 0.3f;

	private UIImage m_Page2Item09Fg;

	private float m_Page2Item09Fg_Timer = -1f;

	private float m_Page2Item09Fg_Time = 0.5f;

	private UIImage m_Page2Item10;

	private float m_Page2Item10_Timer = -1f;

	private float m_Page2Item10_Time = 2f;

	private UIImage m_Page2Item11;

	private float m_Page2Item11_Timer = -1f;

	private float m_Page2Item11_Time = 1f;

	private UIImage m_Page2Item12;

	private float m_Page2Item12_Timer = -1f;

	private float m_Page2Item12_Time = 1.5f;

	private UIImage m_Page2Item12Fg;

	private float m_Page2Item12Fg_Timer = -1f;

	private float m_Page2Item12Fg_Time = 0.3f;

	private UIAnimationControl m_Page2BlackColorAnim2;

	private float m_Page2BlackColorAnim2_Timer = -1f;

	private float m_Page2BlackColorAnim2_Time = 0.5f;

	private UIImage m_Page2Item13;

	private float m_Page2Item13_Timer = -1f;

	private float m_Page2Item13_Time = 0.3f;

	private float m_Page2ItemLast_Timer = -1f;

	private float m_Page2ItemLast_Time = 1.5f;

	private UIImage m_Page3Bg;

	private UIImage m_Page3Item01;

	private float m_Page3Item01_Timer = -1f;

	private float m_Page3Item01_Time = 4f;

	private UIImage m_Page3Item01Fg;

	private float m_Page3Item01Fg_Timer = -1f;

	private float m_Page3Item01Fg_Time = 0.6f;

	private UIImage m_Page3Item02;

	private float m_Page3Item02_Timer = -1f;

	private float m_Page3Item02_Time = 0.3f;

	private UIImage m_Page3Item03;

	private float m_Page3Item03_Timer = -1f;

	private float m_Page3Item03_Time = 0.6f;

	private float m_Page3Item03_Time2 = 0.7f;

	private float m_Page3Item03_Time3 = 1f;

	private float m_Page3Item03_Time4 = 1.4f;

	private UIImage m_Page3Item03Fg;

	private float m_Page3Item03Fg_Timer = -1f;

	private float m_Page3Item03Fg_Time = 0.6f;

	private UIImage m_Page3ItemBgBlack;

	private UIImage m_Page3Item04;

	private float m_Page3Item04Col = 0.7529f;

	private int m_Page3Item04Index = -1;

	private float m_Page3Item04_Timer = -1f;

	private float m_Page3Item04_Time = 0.4f;

	private UIImage m_Page3Item05;

	private float m_Page3Item05_Timer = -1f;

	private float m_Page3Item05_Time = 0.3f;

	private UIImage m_Page3Item06;

	private float m_Page3Item06_Timer = -1f;

	private float m_Page3Item06_Time = 0.8f;

	private int m_Page3Item06_Index;

	private int m_Page3Item06_RoatSpeed = 5;

	private UIImage m_Page3ItemBgBlack2;

	private UIImage m_Page3Item07;

	private float m_Page3Item07_Timer = -1f;

	private float m_Page3Item07_Time = 0.2f;

	private float m_Page3Item07_Time2 = 1.5f;

	private int m_Page3Item07_Index;

	private int m_Page3Item07_MoveSpeed = 5;

	private UIImage m_Page3Item08;

	private float m_Page3Item08_Timer = -1f;

	private float m_Page3Item08_Time = 0.2f;

	private UIImage m_Page3Item09;

	private float m_Page3Item09_Timer = -1f;

	private float m_Page3Item09_Time = 0.1f;

	private float m_Page3Item09_Time2 = 0.2f;

	private float m_Page3Item09_Time3 = 0.3f;

	private float m_Page3Item09_Time4 = 0.4f;

	private UIImage m_Page3ItemBgBlack3;

	private UIImage m_Page3Item10;

	private float m_Page3Item10_Timer = -1f;

	private float m_Page3Item10_Time = 0.2f;

	private float m_Page3Item10_Time2 = 0.5f;

	private UIImage m_Page3Item11;

	private float m_Page3Item11_Timer = -1f;

	private float m_Page3Item11_Time = 0.3f;

	private float m_Page3ItemLast_Timer = -1f;

	private float m_Page3ItemLast_Time = 1.5f;

	private UIImage m_Page4Bg;

	private UIImage m_Page4Item01;

	private float m_Page4Item01_Timer = -1f;

	private float m_Page4Item01_Time = 1.3f;

	private float m_Page4Item01_Time2 = 0.4f;

	private UIImage m_Page4Item01Fg;

	private float m_Page4Item01Fg_Timer = -1f;

	private float m_Page4Item01Fg_Time = 0.4f;

	private UIImage m_Page4Item02;

	private float m_Page4Item02_Timer = -1f;

	private float m_Page4Item02_Time = 0.1f;

	private float m_Page4Item02_Time2 = 0.2f;

	private Color m_Page4Item03_Color = new Color(0.47843f, 0.4392156f, 0.46274f, 1f);

	private UIImage m_Page4Item03;

	private float m_Page4Item03_Index;

	private float m_Page4Item03_MoveSpeed = 5f;

	private float m_Page4Item03_Timer = -1f;

	private float m_Page4Item03_Time = 0.3f;

	private UIImage m_Page4Item03Fg;

	private float m_Page4Item03Fg_Timer = -1f;

	private float m_Page4Item03Fg_Time = 0.3f;

	private float m_Page4Item03Fg_Time2 = 0.6f;

	private UIImage m_Page4Item04;

	private float m_Page4Item04_Timer = -1f;

	private float m_Page4Item04_Time_1 = 0.1f;

	private float m_Page4Item04_Time = 0.2f;

	private UIImage m_Page4Item05;

	private float m_Page4Item05_MoveSpeed;

	private float m_Page4Item05_Timer = -1f;

	private float m_Page4Item05_Time = 0.4f;

	private UIImage m_Page4Item05Fg;

	private float m_Page4Item05Fg_Timer = -1f;

	private float m_Page4Item05Fg_Time = 0.3f;

	private UIImage m_Page4Item06;

	private float m_Page4Item06_Timer = -1f;

	private float m_Page4Item06_Time = 0.2f;

	private float m_Page4Item06_Time2 = 3f;

	private UIImage m_Page4Item07;

	private float m_Page4Item07_Timer = -1f;

	private float m_Page4Item07_Time_0 = 0.1f;

	private float m_Page4Item07_Time = 0.3f;

	private float m_Page4Item07_Time2 = 0.4f;

	private UIAnimationControl m_Page4BlackColorAnim;

	private float m_Page4BlackColorAnim_Timer = -1f;

	private float m_Page4BlackColorAnim_Time = 4f;

	private UIImage m_Page4ItemBgBlack;

	private float m_Page4ItemBgBlack_Timer = -1f;

	private float m_Page4ItemBgBlack_Time = 4f;

	public override bool Visible
	{
		get
		{
			return m_Visible;
		}
		set
		{
			m_Visible = value;
			for (int num = m_Controls.Count; num > 0; num--)
			{
				((UIControl)m_Controls[num - 1]).Visible = value;
			}
		}
	}

	public override bool Enable
	{
		get
		{
			return m_Enable;
		}
		set
		{
			m_Enable = value;
			for (int num = m_Controls.Count; num > 0; num--)
			{
				((UIControl)m_Controls[num - 1]).Enable = value;
			}
		}
	}

	public ArrayList Controls
	{
		get
		{
			return m_Controls;
		}
	}

	public UICartoonAnimControl()
	{
		m_Controls = new ArrayList();
		m_UIHandler = this;
		PlayEnd = false;
		m_MatTransparentUI = LoadUIMaterial("Zombie3D/UI/Materials/TransparentUI");
		m_MatBlackAlphaChangeUI = LoadUIMaterial("Zombie3D/UI/Materials/BlackAlphaChangeUI");
		m_PageIndex = 1;
		BeginPage();
	}

	public void InitDatas()
	{
	}

	public void Add(UIControl control)
	{
		control.SetParent(this);
		m_Controls.Add(control);
		if (m_Clip)
		{
			control.SetClip(m_ClipRect);
		}
	}

	public UIControl GetControl(int controlId)
	{
		for (int i = 0; i < m_Controls.Count; i++)
		{
			if (((UIControl)m_Controls[i]).Id == controlId)
			{
				return (UIControl)m_Controls[i];
			}
		}
		return null;
	}

	public void Clear()
	{
		m_Controls.Clear();
	}

	public override void Update()
	{
		if (m_PageIndex == 1)
		{
			if (m_Page1Item01_Timer >= 0f)
			{
				m_Page1Item01_Timer += Time.deltaTime;
				if (m_Page1Item01_Timer <= m_Page1Item01_Time)
				{
					m_Page1Item01.Rect = AutoUI.AutoRect(new Rect(0f, -30f * (m_Page1Item01_Time - m_Page1Item01_Timer), 960f, 640f));
				}
				else
				{
					m_Page1Item01_Timer = -1f;
					m_Page1BlackColorAnim = new UIAnimationControl();
					m_Page1BlackColorAnim.Id = 0;
					m_Page1BlackColorAnim.SetAnimationsPageCount(10);
					m_Page1BlackColorAnim.Rect = AutoUI.AutoRect(new Rect(0f, 0f, 960f, 640f));
					m_Page1BlackColorAnim.SetTexture(0, m_MatBlackAlphaChangeUI, AutoUI.AutoRect(new Rect(1f, 1f, 1f, 1f)), AutoUI.AutoSize(new Vector2(960f, 640f)));
					m_Page1BlackColorAnim.SetTexture(1, m_MatBlackAlphaChangeUI, AutoUI.AutoRect(new Rect(5f, 1f, 1f, 1f)), AutoUI.AutoSize(new Vector2(960f, 640f)));
					m_Page1BlackColorAnim.SetTexture(2, m_MatBlackAlphaChangeUI, AutoUI.AutoRect(new Rect(9f, 1f, 1f, 1f)), AutoUI.AutoSize(new Vector2(960f, 640f)));
					m_Page1BlackColorAnim.SetTexture(3, m_MatBlackAlphaChangeUI, AutoUI.AutoRect(new Rect(13f, 1f, 1f, 1f)), AutoUI.AutoSize(new Vector2(960f, 640f)));
					m_Page1BlackColorAnim.SetTexture(4, m_MatBlackAlphaChangeUI, AutoUI.AutoRect(new Rect(17f, 1f, 1f, 1f)), AutoUI.AutoSize(new Vector2(960f, 640f)));
					m_Page1BlackColorAnim.SetTexture(5, m_MatBlackAlphaChangeUI, AutoUI.AutoRect(new Rect(21f, 1f, 1f, 1f)), AutoUI.AutoSize(new Vector2(960f, 640f)));
					m_Page1BlackColorAnim.SetTexture(6, m_MatBlackAlphaChangeUI, AutoUI.AutoRect(new Rect(25f, 1f, 1f, 1f)), AutoUI.AutoSize(new Vector2(960f, 640f)));
					m_Page1BlackColorAnim.SetTexture(7, m_MatBlackAlphaChangeUI, AutoUI.AutoRect(new Rect(29f, 1f, 1f, 1f)), AutoUI.AutoSize(new Vector2(960f, 640f)));
					m_Page1BlackColorAnim.SetTexture(8, m_MatBlackAlphaChangeUI, AutoUI.AutoRect(new Rect(29f, 1f, 1f, 1f)), AutoUI.AutoSize(new Vector2(960f, 640f)));
					m_Page1BlackColorAnim.SetTexture(9, m_MatBlackAlphaChangeUI, AutoUI.AutoRect(new Rect(29f, 1f, 1f, 1f)), AutoUI.AutoSize(new Vector2(960f, 640f)));
					m_Page1BlackColorAnim.SetTimeInterval(0.07f);
					m_Page1BlackColorAnim.SetLoopCount(1);
					Add(m_Page1BlackColorAnim);
					if (m_Page1BlackColorAnim_Timer < 0f)
					{
						m_Page1BlackColorAnim_Timer = 0f;
					}
				}
			}
			if (m_Page1BlackColorAnim_Timer >= 0f)
			{
				m_Page1BlackColorAnim_Timer += Time.deltaTime;
				if (m_Page1BlackColorAnim_Timer >= m_Page1BlackColorAnim_Time)
				{
					m_Page1BlackColorAnim_Timer = -1f;
					m_Page1BlackColorAnim.Enable = false;
					m_Page1BlackColorAnim.Visible = false;
					if (m_Page1Item02Fg_Timer < 0f)
					{
						m_Page1ItemBgBlack = UIUtils.BuildImage(0, new Rect(0f, 0f, 960f, 640f), m_MatBlackAlphaChangeUI, new Rect(29f, 1f, 1f, 1f), new Vector2(960f, 640f));
						Add(m_Page1ItemBgBlack);
						m_Page1Item02 = UIUtils.BuildImage(0, new Rect(-232f, 316f, 232f, 290f), m_MatCartoonAnim07UI, new Rect(0f, 586f, 253f, 318f), new Vector2(232f, 290f));
						Add(m_Page1Item02);
						m_Page1Item02Fg = UIUtils.BuildImage(0, new Rect(-232f, 316f, 232f, 290f), m_MatCartoonAnim01UI, new Rect(0f, 654f, 232f, 290f), new Vector2(232f, 290f));
						Add(m_Page1Item02Fg);
						m_Page1Item02Fg_Timer = 0f;
					}
				}
			}
			if (m_Page1Item02Fg_Timer >= 0f)
			{
				m_Page1Item02Fg_Timer += Time.deltaTime;
				if (m_Page1Item02Fg_Timer <= m_Page1Item02Fg_Time)
				{
					m_Page1Item02Fg.Rect = AutoUI.AutoRect(new Rect(-232f + m_Page1Item02Fg_Timer / m_Page1Item02Fg_Time * 274f, 316f, 232f, 290f));
					m_Page1Item02.Rect = AutoUI.AutoRect(new Rect(-232f + m_Page1Item02Fg_Timer / m_Page1Item02Fg_Time * 274f, 316f, 232f, 290f));
				}
				else
				{
					m_Page1Item02Fg.Rect = AutoUI.AutoRect(new Rect(42f, 316f, 232f, 290f));
					m_Page1Item02.Rect = AutoUI.AutoRect(new Rect(42f, 316f, 232f, 290f));
					m_Page1Item02Fg_Timer = -1f;
					m_Page1Item02.SetClip(m_Page1Item02Fg.Rect);
					if (m_Page1Item02_Timer < 0f)
					{
						m_Page1Item02_Timer = 0f;
					}
				}
			}
			if (m_Page1Item02_Timer >= 0f)
			{
				m_Page1Item02_Timer += Time.deltaTime;
				if (m_Page1Item02_Timer <= m_Page1Item02_Time)
				{
					float num = 0f + m_Page1Item02_Timer / m_Page1Item02_Time * 10f;
					float num2 = 586f + m_Page1Item02_Timer / m_Page1Item02_Time * 14f;
					m_Page1Item02.SetTexture(m_MatCartoonAnim07UI, AutoUI.AutoRect(new Rect(0f + m_Page1Item02_Timer / m_Page1Item02_Time * 10f, 586f + m_Page1Item02_Timer / m_Page1Item02_Time * 14f, 253f - m_Page1Item02_Timer / m_Page1Item02_Time * 20f, 318f - m_Page1Item02_Timer / m_Page1Item02_Time * 28f)), AutoUI.AutoSize(new Vector2(232f, 290f)));
				}
				else
				{
					m_Page1Item02_Timer = -1f;
					if (m_Page1Item03Fg_Timer < 0f)
					{
						m_Page1Item03 = UIUtils.BuildImage(0, new Rect(960f, 393f, 414f, 212f), m_MatCartoonAnim07UI, new Rect(580f, 307f, 414f, 228f), new Vector2(414f, 212f));
						Add(m_Page1Item03);
						m_Page1Item03Fg = UIUtils.BuildImage(0, new Rect(960f, 393f, 414f, 212f), m_MatCartoonAnim01UI, new Rect(503f, 654f, 414f, 212f), new Vector2(414f, 212f));
						Add(m_Page1Item03Fg);
						m_Page1Item03Fg_Timer = 0f;
					}
				}
			}
			if (m_Page1Item03Fg_Timer >= 0f)
			{
				m_Page1Item03Fg_Timer += Time.deltaTime;
				if (m_Page1Item03Fg_Timer <= m_Page1Item03Fg_Time)
				{
					m_Page1Item03Fg.Rect = AutoUI.AutoRect(new Rect(960f - m_Page1Item03Fg_Timer / m_Page1Item03Fg_Time * 670f, 393f, 414f, 212f));
					m_Page1Item03.Rect = AutoUI.AutoRect(new Rect(960f - m_Page1Item03Fg_Timer / m_Page1Item03Fg_Time * 670f, 393f, 414f, 212f));
				}
				else
				{
					m_Page1Item03Fg.Rect = AutoUI.AutoRect(new Rect(290f, 393f, 414f, 212f));
					m_Page1Item03.Rect = AutoUI.AutoRect(new Rect(290f, 393f, 414f, 212f));
					m_Page1Item03Fg_Timer = -1f;
					if (m_Page1Item03_Timer < 0f)
					{
						m_Page1Item03_Timer = 0f;
					}
				}
			}
			if (m_Page1Item03_Timer >= 0f)
			{
				m_Page1Item03_Timer += Time.deltaTime;
				if (m_Page1Item03_Timer <= m_Page1Item03_Time)
				{
					m_Page1Item03.SetTexture(m_MatCartoonAnim07UI, AutoUI.AutoRect(new Rect(580f + m_Page1Item03_Timer / m_Page1Item03_Time * 30f, 307f, 414f, 228f)), AutoUI.AutoSize(new Vector2(414f, 212f)));
				}
				else
				{
					m_Page1Item03_Timer = -1f;
					if (m_Page1Item04Fg_Timer < 0f)
					{
						m_Page1Item04 = UIUtils.BuildImage(0, new Rect(960f, 393f, 226f, 212f), m_MatCartoonAnim07UI, new Rect(793f, 537f, 231f, 217f), new Vector2(226f, 212f));
						Add(m_Page1Item04);
						m_Page1Item04Fg = UIUtils.BuildImage(0, new Rect(960f, 393f, 226f, 212f), m_MatCartoonAnim01UI, new Rect(254f, 654f, 226f, 212f), new Vector2(226f, 212f));
						Add(m_Page1Item04Fg);
						m_Page1Item04Fg_Timer = 0f;
					}
				}
			}
			if (m_Page1Item04Fg_Timer >= 0f)
			{
				m_Page1Item04Fg_Timer += Time.deltaTime;
				if (m_Page1Item04Fg_Timer <= m_Page1Item04Fg_Time)
				{
					m_Page1Item04Fg.Rect = AutoUI.AutoRect(new Rect(960f - m_Page1Item04Fg_Timer / m_Page1Item04Fg_Time * 243f, 393f, 226f, 212f));
					m_Page1Item04.Rect = AutoUI.AutoRect(new Rect(960f - m_Page1Item04Fg_Timer / m_Page1Item04Fg_Time * 243f, 393f, 226f, 212f));
				}
				else
				{
					m_Page1Item04Fg.Rect = AutoUI.AutoRect(new Rect(717f, 393f, 226f, 212f));
					m_Page1Item04.Rect = AutoUI.AutoRect(new Rect(717f, 393f, 226f, 212f));
					m_Page1Item04Fg_Timer = -1f;
					m_Page1Item04.SetClip(m_Page1Item04Fg.Rect);
					if (m_Page1Item04_Timer < 0f)
					{
						m_Page1Item04_Timer = 0f;
					}
				}
			}
			if (m_Page1Item04_Timer >= 0f)
			{
				m_Page1Item04_Timer += Time.deltaTime;
				if (m_Page1Item04_Timer <= m_Page1Item04_Time)
				{
					float num3 = 793f + m_Page1Item04_Timer / m_Page1Item04_Time * 3f;
					float num4 = 586f + m_Page1Item04_Timer / m_Page1Item04_Time * 3f;
					m_Page1Item04.SetTexture(m_MatCartoonAnim07UI, AutoUI.AutoRect(new Rect(793f + m_Page1Item04_Timer / m_Page1Item04_Time * 3f, 537f + m_Page1Item04_Timer / m_Page1Item04_Time * 3f, 231f - m_Page1Item04_Timer / m_Page1Item04_Time * 6f, 217f - m_Page1Item04_Timer / m_Page1Item04_Time * 6f)), AutoUI.AutoSize(new Vector2(226f, 212f)));
				}
				else
				{
					m_Page1Item04_Timer = -1f;
					m_Page1ItemLast_Timer = 0f;
				}
			}
			if (m_Page1ItemLast_Timer >= 0f)
			{
				m_Page1ItemLast_Timer += Time.deltaTime;
				if (m_Page1ItemLast_Timer > m_Page1ItemLast_Time && m_PageIndex == 1)
				{
					m_PageIndex = 2;
					BeginFadeIn();
				}
			}
		}
		else if (m_PageIndex == 2)
		{
			if (m_Page2Item01_Timer >= 0f)
			{
				m_Page2Item01_Timer += Time.deltaTime;
				if (m_Page2Item01_Timer <= m_Page2Item01_Time)
				{
					m_Page2Item01.Rect = AutoUI.AutoRect(new Rect(0f, -27f + m_Page2Item01_Timer / m_Page2Item01_Time * 27f, 492f, 667f));
					if (m_Page2Item01_Timer > 1f && m_Page2Item02_Timer == -1f)
					{
						m_Page2Item02 = UIUtils.BuildImage(0, new Rect(250f, 200f, 160f, 200f), m_MatCartoonAnim03UI, new Rect(847f, 824f, 160f, 200f), new Vector2(160f, 200f));
						Add(m_Page2Item02);
						m_Page2Item02_Timer = 0f;
					}
					if (m_Page2Item01_Timer > m_Page2Item01_Time * 0.5f && m_Page2Item03Fg_Timer == -1f)
					{
						m_Page2Item03 = UIUtils.BuildImage(0, new Rect(960f, 358f, 176f, 256f), m_MatCartoonAnim03UI, new Rect(658f, 753f, 187f, 271f), new Vector2(176f, 256f));
						Add(m_Page2Item03);
						m_Page2Item03Fg = UIUtils.BuildImage(0, new Rect(960f, 358f, 176f, 256f), m_MatCartoonAnim03UI, new Rect(847f, 568f, 176f, 256f), new Vector2(176f, 256f));
						Add(m_Page2Item03Fg);
						m_Page2Item03Fg_Timer = 0f;
					}
				}
				else
				{
					m_Page2Item01_Timer = -1f;
					m_Page2BlackColorAnim = new UIAnimationControl();
					m_Page2BlackColorAnim.Id = 0;
					m_Page2BlackColorAnim.SetAnimationsPageCount(10);
					m_Page2BlackColorAnim.Rect = AutoUI.AutoRect(new Rect(0f, 0f, 492f, 640f));
					m_Page2BlackColorAnim.SetTexture(0, m_MatBlackAlphaChangeUI, AutoUI.AutoRect(new Rect(1f, 1f, 1f, 1f)), AutoUI.AutoSize(new Vector2(492f, 640f)));
					m_Page2BlackColorAnim.SetTexture(1, m_MatBlackAlphaChangeUI, AutoUI.AutoRect(new Rect(5f, 1f, 1f, 1f)), AutoUI.AutoSize(new Vector2(492f, 640f)));
					m_Page2BlackColorAnim.SetTexture(2, m_MatBlackAlphaChangeUI, AutoUI.AutoRect(new Rect(9f, 1f, 1f, 1f)), AutoUI.AutoSize(new Vector2(492f, 640f)));
					m_Page2BlackColorAnim.SetTexture(3, m_MatBlackAlphaChangeUI, AutoUI.AutoRect(new Rect(13f, 1f, 1f, 1f)), AutoUI.AutoSize(new Vector2(492f, 640f)));
					m_Page2BlackColorAnim.SetTexture(4, m_MatBlackAlphaChangeUI, AutoUI.AutoRect(new Rect(17f, 1f, 1f, 1f)), AutoUI.AutoSize(new Vector2(492f, 640f)));
					m_Page2BlackColorAnim.SetTexture(5, m_MatBlackAlphaChangeUI, AutoUI.AutoRect(new Rect(21f, 1f, 1f, 1f)), AutoUI.AutoSize(new Vector2(492f, 640f)));
					m_Page2BlackColorAnim.SetTexture(6, m_MatBlackAlphaChangeUI, AutoUI.AutoRect(new Rect(25f, 1f, 1f, 1f)), AutoUI.AutoSize(new Vector2(492f, 640f)));
					m_Page2BlackColorAnim.SetTexture(7, m_MatBlackAlphaChangeUI, AutoUI.AutoRect(new Rect(29f, 1f, 1f, 1f)), AutoUI.AutoSize(new Vector2(492f, 640f)));
					m_Page2BlackColorAnim.SetTexture(8, m_MatBlackAlphaChangeUI, AutoUI.AutoRect(new Rect(29f, 1f, 1f, 1f)), AutoUI.AutoSize(new Vector2(492f, 640f)));
					m_Page2BlackColorAnim.SetTexture(9, m_MatBlackAlphaChangeUI, AutoUI.AutoRect(new Rect(29f, 1f, 1f, 1f)), AutoUI.AutoSize(new Vector2(492f, 640f)));
					m_Page2BlackColorAnim.SetTimeInterval(0.08f);
					m_Page2BlackColorAnim.SetLoopCount(1);
					Add(m_Page2BlackColorAnim);
					if (m_Page2BlackColorAnim_Timer < 0f)
					{
						m_Page2BlackColorAnim_Timer = 0f;
					}
				}
			}
			if (m_Page2Item02_Timer >= 0f)
			{
				m_Page2Item02_Timer += Time.deltaTime;
				if (m_Page2Item02_Timer <= m_Page2Item02_Time)
				{
					if (m_Page2Item02_Timer < m_Page2Item02_Time * 0.33f)
					{
						float a = Mathf.Clamp01(m_Page2Item02_Timer / (m_Page2Item02_Time * 0.33f));
						m_Page2Item02.SetColor(new Color(1f, 1f, 1f, a));
					}
					else if (m_Page2Item02_Timer > m_Page2Item02_Time * 0.7f)
					{
						float a2 = Mathf.Clamp01((m_Page2Item02_Time - m_Page2Item02_Timer) / (m_Page2Item02_Time * 0.33f));
						m_Page2Item02.SetColor(new Color(1f, 1f, 1f, a2));
					}
				}
				else
				{
					m_Page2Item02_Timer = -2f;
					m_Page2Item02.Visible = false;
				}
			}
			if (m_Page2BlackColorAnim_Timer >= 0f)
			{
				m_Page2BlackColorAnim_Timer += Time.deltaTime;
				if (m_Page2BlackColorAnim_Timer >= 0.8f)
				{
					m_Page2BlackColorAnim_Timer = -1f;
					m_Page2BlackColorAnim.Enable = false;
					m_Page2BlackColorAnim.Visible = false;
					m_Page2ItemBgBlack = UIUtils.BuildImage(0, new Rect(0f, 0f, 492f, 640f), m_MatBlackAlphaChangeUI, new Rect(29f, 1f, 1f, 1f), new Vector2(492f, 640f));
					Add(m_Page2ItemBgBlack);
				}
			}
			if (m_Page2Item03Fg_Timer >= 0f)
			{
				m_Page2Item03Fg_Timer += Time.deltaTime;
				if (m_Page2Item03Fg_Timer <= m_Page2Item03Fg_Time)
				{
					m_Page2Item03Fg.Rect = AutoUI.AutoRect(new Rect(960f - m_Page2Item03Fg_Timer / m_Page2Item03Fg_Time * 447f, 358f, 176f, 256f));
					m_Page2Item03.Rect = AutoUI.AutoRect(new Rect(960f - m_Page2Item03Fg_Timer / m_Page2Item03Fg_Time * 447f, 358f, 176f, 256f));
				}
				else
				{
					m_Page2Item03Fg.Rect = AutoUI.AutoRect(new Rect(513f, 358f, 176f, 256f));
					m_Page2Item03.Rect = AutoUI.AutoRect(new Rect(513f, 358f, 176f, 256f));
					m_Page2Item03Fg_Timer = -2f;
					m_Page2Item03_Timer = 0f;
				}
			}
			if (m_Page2Item03_Timer >= 0f)
			{
				m_Page2Item03_Timer += Time.deltaTime;
				if (m_Page2Item03_Timer <= m_Page2Item03_Time)
				{
					if (m_Page2Item04_Timer < 0f)
					{
						m_Page2Item04 = UIUtils.BuildImage(0, new Rect(520f, 544f, 70f, 82f), m_MatCartoonAnim03UI, new Rect(943f, 0f, 70f, 82f), new Vector2(70f, 82f));
						Add(m_Page2Item04);
						m_Page2Item04_Timer = 0f;
					}
					float num5 = 847f + m_Page2Item03_Timer / m_Page2Item03_Time * 5f;
					float num6 = 824f + m_Page2Item03_Timer / m_Page2Item03_Time * 8f;
					m_Page2Item03.SetTexture(m_MatCartoonAnim03UI, AutoUI.AutoRect(new Rect(658f + m_Page2Item03_Timer / m_Page2Item03_Time * 5f, 753f + m_Page2Item03_Timer / m_Page2Item03_Time * 8f, 187f - m_Page2Item03_Timer / m_Page2Item03_Time * 10f, 271f - m_Page2Item03_Timer / m_Page2Item03_Time * 16f)), AutoUI.AutoSize(new Vector2(176f, 256f)));
				}
				else
				{
					m_Page2Item03_Timer = -1f;
				}
			}
			if (m_Page2Item04_Timer >= 0f)
			{
				m_Page2Item04_Timer += Time.deltaTime;
				if (m_Page2Item04_Timer < m_Page2Item04_Time)
				{
					float num7 = 1f;
					num7 = ((!(m_Page2Item04_Timer < m_Page2Item04_Time * 2f / 3f)) ? (1f - 0.3f * (m_Page2Item04_Timer - m_Page2Item04_Time * 2f / 3f) / (m_Page2Item04_Time * 1f / 3f)) : (1f * m_Page2Item04_Timer / (m_Page2Item04_Time * 2f / 3f)));
					Vector2 vec = new Vector2(70f * num7, 82f * num7);
					m_Page2Item04.Rect = AutoUI.AutoRect(new Rect(555f - vec.x / 2f, 544f, vec.x, vec.y));
					m_Page2Item04.SetTextureSize(AutoUI.AutoSize(vec));
					if (m_Page2Item04_Timer > m_Page2Item04_Time / 2f && m_Page2Item04_01_Timer < 0f)
					{
						m_Page2Item04_01 = UIUtils.BuildImage(0, new Rect(585f, 522f, 94f, 94f), m_MatCartoonAnim03UI, new Rect(755f, 314f, 94f, 94f), new Vector2(94f, 94f));
						Add(m_Page2Item04_01);
						m_Page2Item04_01_Timer = 0f;
					}
				}
				else
				{
					m_Page2Item04_Timer = -1f;
					m_Page2Item05 = UIUtils.BuildImage(0, new Rect(960f, 358f, 176f, 256f), m_MatCartoonAnim03UI, new Rect(468f, 751f, 176f, 256f), new Vector2(176f, 256f));
					Add(m_Page2Item05);
					m_Page2Item05Fg = UIUtils.BuildImage(0, new Rect(960f, 358f, 176f, 256f), m_MatCartoonAnim03UI, new Rect(847f, 568f, 176f, 256f), new Vector2(176f, 256f));
					Add(m_Page2Item05Fg);
					m_Page2Item05Fg_Timer = 0f;
				}
			}
			if (m_Page2Item04_01_Timer >= 0f)
			{
				m_Page2Item04_01_Timer += Time.deltaTime;
				if (m_Page2Item04_01_Timer <= m_Page2Item04_01_Time)
				{
					float num8 = 1f;
					num8 = ((!(m_Page2Item04_01_Timer < m_Page2Item04_01_Time * 2f / 3f)) ? (1f - 0.3f * (m_Page2Item04_01_Timer - m_Page2Item04_01_Time * 2f / 3f) / (m_Page2Item04_01_Time * 1f / 3f)) : (1f * m_Page2Item04_01_Timer / (m_Page2Item04_01_Time * 2f / 3f)));
					Vector2 vec2 = new Vector2(94f * num8, 94f * num8);
					m_Page2Item04_01.Rect = AutoUI.AutoRect(new Rect(595f, 530f, vec2.x, vec2.y));
					m_Page2Item04_01.SetTextureSize(AutoUI.AutoSize(vec2));
				}
				else
				{
					m_Page2Item04_01_Timer = -1f;
				}
			}
			if (m_Page2Item05Fg_Timer >= 0f)
			{
				m_Page2Item05Fg_Timer += Time.deltaTime;
				if (m_Page2Item05Fg_Timer <= m_Page2Item05Fg_Time)
				{
					m_Page2Item05Fg.Rect = AutoUI.AutoRect(new Rect(960f - m_Page2Item05Fg_Timer / m_Page2Item05Fg_Time * 260f, 358f, 176f, 256f));
					m_Page2Item05.Rect = AutoUI.AutoRect(new Rect(960f - m_Page2Item05Fg_Timer / m_Page2Item05Fg_Time * 260f, 358f, 176f, 256f));
				}
				else
				{
					m_Page2Item05Fg.Rect = AutoUI.AutoRect(new Rect(700f, 358f, 176f, 256f));
					m_Page2Item05.Rect = AutoUI.AutoRect(new Rect(700f, 358f, 176f, 256f));
					m_Page2Item05Fg_Timer = -1f;
					m_Page2Item05_Timer = 0f;
				}
			}
			if (m_Page2Item05_Timer >= 0f)
			{
				m_Page2Item05_Timer += Time.deltaTime;
				if (m_Page2Item05_Timer <= m_Page2Item05_Time)
				{
					m_Page2Item05.SetTexture(m_MatCartoonAnim03UI, AutoUI.AutoRect(new Rect(468f + m_Page2Item05_Timer / m_Page2Item05_Time * 12f, 751f, 176f, 256f)));
				}
				else
				{
					m_Page2Item05_Timer = -1f;
					m_Page2Item06 = UIUtils.BuildImage(0, new Rect(844f, 503f, 127f, 62f), m_MatCartoonAnim03UI, new Rect(896f, 504f, 127f, 62f), new Vector2(127f, 62f));
					Add(m_Page2Item06);
					m_Page2Item06_Timer = 0f;
				}
			}
			if (m_Page2Item06_Timer >= 0f)
			{
				m_Page2Item06_Timer += Time.deltaTime;
				if (m_Page2Item06_Timer <= m_Page2Item06_Time)
				{
					float[] array = new float[8] { 0f, -2.5f, -5f, -2.5f, 0f, 2.5f, 5f, 2.5f };
					int num9 = Mathf.FloorToInt(m_Page2Item06_Timer / m_Page2Item06_Time * 50f) % array.Length;
					m_Page2Item06.Rect = AutoUI.AutoRect(new Rect(844f, 503f + array[num9], 127f, 62f));
					if (m_Page2Item06_Timer >= m_Page2Item06_Time * 1f / 2f)
					{
						float a3 = (m_Page2Item06_Time - m_Page2Item06_Timer) / m_Page2Item06_Time;
						m_Page2Item06.SetColor(new Color(1f, 1f, 1f, a3));
					}
				}
				else
				{
					m_Page2Item06_Timer = -1f;
					m_Page2Item06.SetColor(new Color(1f, 1f, 1f, 0f));
					m_Page2Item07 = UIUtils.BuildImage(0, new Rect(813f, 390f, 127f, 62f), m_MatCartoonAnim03UI, new Rect(896f, 504f, 127f, 62f), new Vector2(127f, 62f));
					Add(m_Page2Item07);
					m_Page2Item07_Timer = 0f;
				}
			}
			if (m_Page2Item07_Timer >= 0f)
			{
				m_Page2Item07_Timer += Time.deltaTime;
				if (m_Page2Item07_Timer <= m_Page2Item07_Time)
				{
					float[] array2 = new float[8] { 0f, -2.5f, -5f, -2.5f, 0f, 2.5f, 5f, 2.5f };
					int num10 = Mathf.FloorToInt(m_Page2Item07_Timer / m_Page2Item07_Time * 50f) % array2.Length;
					m_Page2Item07.Rect = AutoUI.AutoRect(new Rect(844f, 390f + array2[num10], 127f, 62f));
					if (m_Page2Item07_Timer >= m_Page2Item07_Time * 1f / 2f)
					{
						float a4 = (m_Page2Item07_Time - m_Page2Item07_Timer) / m_Page2Item07_Time;
						m_Page2Item07.SetColor(new Color(1f, 1f, 1f, a4));
					}
				}
				else
				{
					m_Page2Item07_Timer = -1f;
					m_Page2Item07.SetColor(new Color(1f, 1f, 1f, 0f));
					m_Page2Item09 = UIUtils.BuildImage(0, new Rect(513f, 38f, 446f, 312f), m_MatCartoonAnim03UI, new Rect(0f, 668f, 446f, 312f), new Vector2(446f, 312f));
					Add(m_Page2Item09);
					m_Page2Item10 = UIUtils.BuildImage(0, new Rect(706f, 268f, 38f, 38f), m_MatCartoonAnim03UI, new Rect(849f, 314f, 38f, 38f), new Vector2(38f, 38f));
					Add(m_Page2Item10);
					m_Page2Item09Fg = UIUtils.BuildImage(0, new Rect(513f, 38f, 446f, 312f), m_MatCartoonAnim03UI, new Rect(495f, 0f, 446f, 312f), new Vector2(446f, 312f));
					Add(m_Page2Item09Fg);
					m_Page2Item09Fg_Timer = 0f;
					m_Page2Item08 = UIUtils.BuildImage(0, new Rect(770f, 250f, 127f, 62f), m_MatCartoonAnim03UI, new Rect(896f, 504f, 127f, 62f), new Vector2(127f, 62f));
					Add(m_Page2Item08);
					m_Page2Item08_Timer = 0f;
				}
			}
			if (m_Page2Item08_Timer >= 0f)
			{
				m_Page2Item08_Timer += Time.deltaTime;
				if (m_Page2Item08_Timer <= m_Page2Item08_Time)
				{
					float[] array3 = new float[8] { 0f, -2.5f, -5f, -2.5f, 0f, 2.5f, 5f, 2.5f };
					int num11 = Mathf.FloorToInt(m_Page2Item08_Timer / m_Page2Item08_Time * 80f) % array3.Length;
					m_Page2Item08.Rect = AutoUI.AutoRect(new Rect(770f, 250f + array3[num11], 127f, 62f));
					if (m_Page2Item08_Timer >= m_Page2Item08_Time * 2f / 3f)
					{
						float a5 = (m_Page2Item08_Time - m_Page2Item08_Timer) / m_Page2Item08_Time;
						m_Page2Item08.SetColor(new Color(1f, 1f, 1f, a5));
					}
				}
				else
				{
					m_Page2Item08_Timer = -1f;
					m_Page2Item08.SetColor(new Color(1f, 1f, 1f, 0f));
				}
			}
			if (m_Page2Item10 != null && m_Page2Item10.Visible)
			{
				float a6 = Time.time % 0.33f * 3f;
				m_Page2Item10.SetColor(new Color(1f, 1f, 1f, a6));
			}
			if (m_Page2Item09Fg_Timer >= 0f)
			{
				m_Page2Item09Fg_Timer += Time.deltaTime;
				if (m_Page2Item09Fg_Timer <= m_Page2Item09_Time)
				{
					m_Page2Item09.Rect = AutoUI.AutoRect(new Rect(960f - m_Page2Item09Fg_Timer / m_Page2Item09_Time * 447f, 38f, 446f, 312f));
					m_Page2Item10.Rect = AutoUI.AutoRect(new Rect(960f - m_Page2Item09Fg_Timer / m_Page2Item09_Time * 245f, 268f, 38f, 38f));
					m_Page2Item09Fg.Rect = AutoUI.AutoRect(new Rect(960f - m_Page2Item09Fg_Timer / m_Page2Item09_Time * 447f, 38f, 446f, 312f));
				}
				else
				{
					m_Page2Item09Fg_Timer = -1f;
					m_Page2Item09.Rect = AutoUI.AutoRect(new Rect(513f, 38f, 446f, 312f));
					m_Page2Item10.Rect = AutoUI.AutoRect(new Rect(715f, 268f, 38f, 38f));
					m_Page2Item09Fg.Rect = AutoUI.AutoRect(new Rect(513f, 38f, 446f, 312f));
					m_Page2Item10_Timer = 0f;
					m_Page2Item11 = UIUtils.BuildImage(0, new Rect(530f, 210f, 115f, 156f), m_MatCartoonAnim03UI, new Rect(905f, 314f, 115f, 156f), new Vector2(115f, 156f));
					Add(m_Page2Item11);
					m_Page2Item11_Timer = 0f;
				}
			}
			if (m_Page2Item10_Timer >= 0f)
			{
				m_Page2Item10_Timer += Time.deltaTime;
				if (m_Page2Item10_Timer <= m_Page2Item10_Time)
				{
					m_Page2Item09.SetTexture(m_MatCartoonAnim03UI, AutoUI.AutoRect(new Rect(0f + m_Page2Item10_Timer / m_Page2Item10_Time * 20f, 668f, 446f, 312f)));
					m_Page2Item10.Rect = AutoUI.AutoRect(new Rect(715f - m_Page2Item10_Timer / m_Page2Item10_Time * 15f, 268f, 38f, 38f));
				}
				else
				{
					m_Page2Item10_Timer = -1f;
					m_Page2Item10.Rect = AutoUI.AutoRect(new Rect(700f, 268f, 38f, 38f));
				}
			}
			if (m_Page2Item11_Timer >= 0f)
			{
				m_Page2Item11_Timer += Time.deltaTime;
				if (m_Page2Item11_Timer <= m_Page2Item11_Time)
				{
					Vector2 vector = new Vector2(587f, 288f);
					if (m_Page2Item11_Timer <= m_Page2Item11_Time * 1f / 6f)
					{
						float num12 = m_Page2Item11_Timer / (m_Page2Item11_Time * 1f / 6f) + 0.7f;
						m_Page2Item11.Rect = AutoUI.AutoRect(new Rect(vector.x - num12 * 115f / 2f, vector.y - num12 * 156f / 2f, num12 * 115f, num12 * 156f));
						m_Page2Item11.SetTextureSize(AutoUI.AutoSize(new Vector2(num12 * 115f, num12 * 156f)));
					}
					else if (m_Page2Item11_Timer <= m_Page2Item11_Time * 2f / 6f)
					{
						float num13 = (m_Page2Item11_Time * 2f / 6f - m_Page2Item11_Timer) / (m_Page2Item11_Time * 1f / 6f) + 0.7f;
						m_Page2Item11.Rect = AutoUI.AutoRect(new Rect(vector.x - num13 * 115f / 2f, vector.y - num13 * 156f / 2f, num13 * 115f, num13 * 156f));
						m_Page2Item11.SetTextureSize(AutoUI.AutoSize(new Vector2(num13 * 115f, num13 * 156f)));
					}
				}
				else
				{
					m_Page2Item11_Timer = -1f;
					m_Page2BlackColorAnim2 = new UIAnimationControl();
					m_Page2BlackColorAnim2.Id = 0;
					m_Page2BlackColorAnim2.SetAnimationsPageCount(10);
					m_Page2BlackColorAnim2.Rect = AutoUI.AutoRect(new Rect(0f, 0f, 960f, 640f));
					m_Page2BlackColorAnim2.SetTexture(0, m_MatBlackAlphaChangeUI, AutoUI.AutoRect(new Rect(1f, 1f, 1f, 1f)), AutoUI.AutoSize(new Vector2(960f, 640f)));
					m_Page2BlackColorAnim2.SetTexture(1, m_MatBlackAlphaChangeUI, AutoUI.AutoRect(new Rect(5f, 1f, 1f, 1f)), AutoUI.AutoSize(new Vector2(960f, 640f)));
					m_Page2BlackColorAnim2.SetTexture(2, m_MatBlackAlphaChangeUI, AutoUI.AutoRect(new Rect(9f, 1f, 1f, 1f)), AutoUI.AutoSize(new Vector2(960f, 640f)));
					m_Page2BlackColorAnim2.SetTexture(3, m_MatBlackAlphaChangeUI, AutoUI.AutoRect(new Rect(13f, 1f, 1f, 1f)), AutoUI.AutoSize(new Vector2(960f, 640f)));
					m_Page2BlackColorAnim2.SetTexture(4, m_MatBlackAlphaChangeUI, AutoUI.AutoRect(new Rect(17f, 1f, 1f, 1f)), AutoUI.AutoSize(new Vector2(960f, 640f)));
					m_Page2BlackColorAnim2.SetTexture(5, m_MatBlackAlphaChangeUI, AutoUI.AutoRect(new Rect(21f, 1f, 1f, 1f)), AutoUI.AutoSize(new Vector2(960f, 640f)));
					m_Page2BlackColorAnim2.SetTexture(6, m_MatBlackAlphaChangeUI, AutoUI.AutoRect(new Rect(25f, 1f, 1f, 1f)), AutoUI.AutoSize(new Vector2(960f, 640f)));
					m_Page2BlackColorAnim2.SetTexture(7, m_MatBlackAlphaChangeUI, AutoUI.AutoRect(new Rect(29f, 1f, 1f, 1f)), AutoUI.AutoSize(new Vector2(960f, 640f)));
					m_Page2BlackColorAnim2.SetTexture(8, m_MatBlackAlphaChangeUI, AutoUI.AutoRect(new Rect(29f, 1f, 1f, 1f)), AutoUI.AutoSize(new Vector2(960f, 640f)));
					m_Page2BlackColorAnim2.SetTexture(9, m_MatBlackAlphaChangeUI, AutoUI.AutoRect(new Rect(29f, 1f, 1f, 1f)), AutoUI.AutoSize(new Vector2(960f, 640f)));
					m_Page2BlackColorAnim2.SetTimeInterval(m_Page2BlackColorAnim2_Time / 10f);
					m_Page2BlackColorAnim2.SetLoopCount(1);
					Add(m_Page2BlackColorAnim2);
					m_Page2Item13 = UIUtils.BuildImage(0, new Rect(0f, 0f, 960f, 640f), m_MatBlackAlphaChangeUI, new Rect(29f, 1f, 1f, 1f), new Vector2(960f, 640f));
					m_Page2Item13.Visible = false;
					Add(m_Page2Item13);
					m_Page2BlackColorAnim2_Timer = 0f;
					m_Page2Item12 = UIUtils.BuildImage(0, new Rect(770f, 15f, 254f, 189f), m_MatCartoonAnim03UI, new Rect(495f, 535f, 315f, 214f), new Vector2(254f, 189f));
					Add(m_Page2Item12);
					m_Page2Item12Fg = UIUtils.BuildImage(0, new Rect(770f, 15f, 254f, 189f), m_MatCartoonAnim03UI, new Rect(497f, 314f, 254f, 189f), new Vector2(254f, 189f));
					Add(m_Page2Item12Fg);
					m_Page2Item12Fg_Timer = 0f;
				}
			}
			if (m_Page2BlackColorAnim2_Timer >= 0f)
			{
				m_Page2BlackColorAnim2_Timer += Time.deltaTime;
				if (!(m_Page2BlackColorAnim2_Timer <= m_Page2BlackColorAnim2_Time * 0.8f))
				{
					m_Page2BlackColorAnim2.Visible = false;
					m_Page2Item13.Visible = true;
					m_Page2BlackColorAnim2_Timer = -1f;
				}
			}
			if (m_Page2Item12Fg_Timer >= 0f)
			{
				m_Page2Item12Fg_Timer += Time.deltaTime;
				if (m_Page2Item12Fg_Timer <= m_Page2Item12Fg_Time)
				{
					m_Page2Item12.Rect = AutoUI.AutoRect(new Rect(960f - m_Page2Item12Fg_Timer / m_Page2Item12Fg_Time * 190f, 15f, 254f, 189f));
					m_Page2Item12Fg.Rect = AutoUI.AutoRect(new Rect(960f - m_Page2Item12Fg_Timer / m_Page2Item12Fg_Time * 190f, 15f, 254f, 189f));
				}
				else
				{
					m_Page2Item12Fg_Timer = -1f;
					m_Page2Item12.Rect = AutoUI.AutoRect(new Rect(770f, 15f, 254f, 189f));
					m_Page2Item12Fg.Rect = AutoUI.AutoRect(new Rect(770f, 15f, 254f, 189f));
					m_Page2Item12_Timer = 0f;
				}
			}
			if (m_Page2Item12_Timer >= 0f)
			{
				m_Page2Item12_Timer += Time.deltaTime;
				if (m_Page2Item12_Timer <= m_Page2Item12_Time)
				{
					m_Page2Item12.SetTexture(m_MatCartoonAnim03UI, AutoUI.AutoRect(new Rect(495f + m_Page2Item12_Timer / m_Page2Item12_Time * 30f, 535f + m_Page2Item12_Timer / m_Page2Item12_Time * 12f, 315f - m_Page2Item12_Timer / m_Page2Item12_Time * 60f, 214f - m_Page2Item12_Timer / m_Page2Item12_Time * 24f)), AutoUI.AutoSize(new Vector2(254f, 189f)));
				}
				else
				{
					m_Page2Item12_Timer = -1f;
					m_Page2ItemLast_Timer = 0f;
				}
			}
			if (m_Page2ItemLast_Timer >= 0f)
			{
				m_Page2ItemLast_Timer += Time.deltaTime;
				if (m_Page2ItemLast_Timer > m_Page2ItemLast_Time)
				{
					m_Page2ItemLast_Timer = -1f;
					if (m_PageIndex == 2)
					{
						m_PageIndex = 3;
						BeginFadeIn();
					}
				}
			}
		}
		else if (m_PageIndex == 3)
		{
			if (m_Page3Item01Fg_Timer >= 0f)
			{
				m_Page3Item01Fg_Timer += Time.deltaTime;
				if (m_Page3Item01Fg_Timer <= m_Page3Item01Fg_Time)
				{
					m_Page3Item01Fg.Rect = AutoUI.AutoRect(new Rect(-170f + m_Page3Item01Fg_Timer / m_Page3Item01Fg_Time * 190f, 276f, 230f, 338f));
					m_Page3Item01.Rect = AutoUI.AutoRect(new Rect(-170f + m_Page3Item01Fg_Timer / m_Page3Item01Fg_Time * 190f, 276f, 230f, 338f));
				}
				else
				{
					m_Page3Item01Fg.Rect = AutoUI.AutoRect(new Rect(20f, 276f, 230f, 338f));
					m_Page3Item01.Rect = AutoUI.AutoRect(new Rect(20f, 276f, 230f, 338f));
					m_Page3Item01Fg_Timer = -1f;
					m_Page3Item02 = UIUtils.BuildImage(0, new Rect(82f, 467f, 179f, 171f), m_MatCartoonAnim05UI, new Rect(0f, 767f, 179f, 171f), new Vector2(179f, 171f));
					Add(m_Page3Item02);
					m_Page3Item02_Timer = 0f;
				}
			}
			if (m_Page3Item02_Timer >= 0f)
			{
				m_Page3Item02_Timer += Time.deltaTime;
				if (m_Page3Item02_Timer <= m_Page3Item02_Time)
				{
					float a7 = m_Page3Item02_Timer / m_Page3Item02_Time;
					m_Page3Item02.SetColor(new Color(1f, 1f, 1f, a7));
				}
				else
				{
					m_Page3Item02.SetColor(new Color(1f, 1f, 1f, 1f));
					m_Page3Item02_Timer = -1f;
					m_Page3Item03 = UIUtils.BuildImage(0, new Rect(-58f, 276f, 58f, 338f), m_MatCartoonAnim05UI, new Rect(645f, 599f, 58f, 368f), new Vector2(58f, 338f));
					Add(m_Page3Item03);
					m_Page3Item03Fg = UIUtils.BuildImage(0, new Rect(-58f, 276f, 58f, 338f), m_MatCartoonAnim05UI, new Rect(706f, 599f, 58f, 338f), new Vector2(58f, 338f));
					Add(m_Page3Item03Fg);
					m_Page3Item03Fg_Timer = 0f;
				}
			}
			if (m_Page3Item03Fg_Timer >= 0f)
			{
				m_Page3Item03Fg_Timer += Time.deltaTime;
				if (m_Page3Item03Fg_Timer <= m_Page3Item03Fg_Time)
				{
					m_Page3Item03Fg.Rect = AutoUI.AutoRect(new Rect(-58f + m_Page3Item03Fg_Timer / m_Page3Item03Fg_Time * 317f, 276f, 58f, 338f));
					m_Page3Item03.Rect = AutoUI.AutoRect(new Rect(-58f + m_Page3Item03Fg_Timer / m_Page3Item03Fg_Time * 317f, 276f, 58f, 338f));
				}
				else
				{
					m_Page3Item03Fg.Rect = AutoUI.AutoRect(new Rect(259f, 276f, 58f, 338f));
					m_Page3Item03.Rect = AutoUI.AutoRect(new Rect(259f, 276f, 58f, 338f));
					m_Page3Item03Fg_Timer = -1f;
					m_Page3Item03_Timer = 0f;
				}
			}
			if (m_Page3Item03_Timer >= 0f)
			{
				m_Page3Item03_Timer += Time.deltaTime;
				if (m_Page3Item03_Timer <= m_Page3Item03_Time)
				{
					m_Page3Item03.SetTexture(m_MatCartoonAnim05UI, AutoUI.AutoRect(new Rect(645f, 599f + m_Page3Item03_Timer / m_Page3Item03_Time * 15f, 58f, 368f - m_Page3Item03_Timer / m_Page3Item03_Time * 15f)), AutoUI.AutoSize(new Vector2(58f, 338f)));
				}
				else
				{
					m_Page3Item03.SetTexture(m_MatCartoonAnim05UI, AutoUI.AutoRect(new Rect(645f, 614f, 58f, 353f)), AutoUI.AutoSize(new Vector2(58f, 338f)));
					if (m_Page3Item03_Timer <= m_Page3Item03_Time2)
					{
						float a8 = 1f - m_Page3Item03_Timer / m_Page3Item03_Time2;
						m_Page3Item02.SetColor(new Color(1f, 1f, 1f, a8));
					}
					else if (m_Page3Item03_Timer <= m_Page3Item03_Time3)
					{
						float a9 = 1f - m_Page3Item03_Timer / m_Page3Item03_Time3;
						m_Page3Item01.SetColor(new Color(1f, 1f, 1f, a9));
						m_Page3Item01Fg.SetColor(new Color(1f, 1f, 1f, a9));
					}
					else if (m_Page3Item03_Timer <= m_Page3Item03_Time4)
					{
						float a10 = 1f - m_Page3Item03_Timer / m_Page3Item03_Time4;
						m_Page3Item03.SetColor(new Color(1f, 1f, 1f, a10));
						m_Page3Item03Fg.SetColor(new Color(1f, 1f, 1f, a10));
					}
					else
					{
						m_Page3Item03_Timer = -1f;
						m_Page3ItemBgBlack = UIUtils.BuildImage(0, new Rect(0f, 0f, 960f, 640f), m_MatBlackAlphaChangeUI, new Rect(41f, 1f, 1f, 1f), new Vector2(960f, 640f));
						Add(m_Page3ItemBgBlack);
						m_Page3Item04 = UIUtils.BuildImage(0, new Rect(94f, 101f, 645f, 501f), m_MatCartoonAnim04UI, new Rect(0f, 0f, 743f, 577f), new Vector2(645f, 501f));
						Add(m_Page3Item04);
						m_Page3Item04_Timer = 0f;
						m_Page3Item04Index = 0;
					}
				}
			}
			if (m_Page3Item04_Timer >= 0f)
			{
				m_Page3Item04_Timer += Time.deltaTime;
				if (m_Page3Item04_Timer < m_Page3Item04_Time)
				{
					float num14 = m_Page3Item04_Timer / m_Page3Item04_Time;
					if (num14 <= 0.8f)
					{
						m_Page3Item04.SetColor(new Color(m_Page3Item04Col * (0.5f + 0.5f * num14 / 0.8f), 0f, 0f, 1f));
						m_Page3Item04.SetTexture(m_MatCartoonAnim04UI, AutoUI.AutoRect(new Rect(0f + num14 * 49f, 0f + num14 * 38f, 743f - num14 * 49f * 2f, 577f - num14 * 38f * 2f)), AutoUI.AutoSize(new Vector2(645f, 501f)));
					}
					else
					{
						m_Page3Item04.SetColor(new Color(1f, 1f, 1f, 1f));
						m_Page3Item04.SetTexture(m_MatCartoonAnim04UI, AutoUI.AutoRect(new Rect(0f + num14 * 49f, 0f + num14 * 38f, 743f - num14 * 49f * 2f, 577f - num14 * 38f * 2f)), AutoUI.AutoSize(new Vector2(645f, 501f)));
					}
				}
				else
				{
					m_Page3Item04.SetColor(new Color(1f, 1f, 1f, 1f));
					m_Page3Item04.SetTexture(m_MatCartoonAnim04UI, AutoUI.AutoRect(new Rect(0f, 0f, 743f, 577f)), AutoUI.AutoSize(new Vector2(645f, 501f)));
					m_Page3Item04_Timer = 0f;
					switch (m_Page3Item04Index)
					{
					case 0:
						m_Page3Item05_Timer = 0f;
						break;
					case 1:
						m_Page3Item06_Timer = 0f;
						m_Page3Item04Index = -1;
						break;
					}
				}
			}
			if (m_Page3Item05_Timer >= 0f)
			{
				if (m_Page3Item05 == null)
				{
					m_Page3Item05 = UIUtils.BuildImage(0, new Rect(1139f, 274f, 179f, 334f), m_MatCartoonAnim05UI, new Rect(800f, 189f, 179f, 334f), new Vector2(179f, 334f));
					Add(m_Page3Item05);
					m_Page3Item04Index = 1;
				}
				m_Page3Item05_Timer += Time.deltaTime;
				if (m_Page3Item05_Timer <= m_Page3Item05_Time)
				{
					m_Page3Item05.Rect = AutoUI.AutoRect(new Rect(1139f - m_Page3Item05_Timer / m_Page3Item05_Time * 390f, 274f, 179f, 334f));
				}
				else
				{
					m_Page3Item05.Rect = AutoUI.AutoRect(new Rect(774f, 274f, 179f, 334f));
					m_Page3Item05_Timer = -1f;
				}
			}
			if (m_Page3Item06_Timer >= 0f)
			{
				m_Page3Item06_Timer += Time.deltaTime;
				if (m_Page3Item06 == null)
				{
					m_Page3Item06 = UIUtils.BuildImage(0, new Rect(685f, 438f, 137f, 172f), m_MatCartoonAnim05UI, new Rect(535f, 0f, 194f, 258f), new Vector2(137f, 172f));
					Add(m_Page3Item06);
				}
				if (m_Page3Item06_Timer <= m_Page3Item06_Time * 0.1f)
				{
					m_Page3Item06.SetTexture(m_MatCartoonAnim05UI, AutoUI.AutoRect(new Rect(535f, 0f, 194f, 258f)), AutoUI.AutoSize(new Vector2(194f, 258f)));
				}
				else if (m_Page3Item06_Timer <= m_Page3Item06_Time * 0.75f)
				{
					float[] array4 = new float[4] { 30f, 0f, -30f, 0f };
					int num15 = m_Page3Item06_Index % (array4.Length * m_Page3Item06_RoatSpeed);
					num15 /= m_Page3Item06_RoatSpeed;
					int num16 = num15;
					m_Page3Item06_Index++;
					m_Page3Item06.SetRotation(array4[num16] * ((float)Math.PI / 180f));
				}
				else if (m_Page3Item06_Timer <= m_Page3Item06_Time * 1f)
				{
					float a11 = 1f - (m_Page3Item06_Timer - m_Page3Item06_Time * 0.75f) / (m_Page3Item06_Time - m_Page3Item06_Time * 0.75f);
					m_Page3Item04.SetColor(new Color(1f, 1f, 1f, a11));
					m_Page3Item05.SetColor(new Color(1f, 1f, 1f, a11));
					m_Page3Item06.SetColor(new Color(1f, 1f, 1f, a11));
				}
				else
				{
					m_Page3Item06_Timer = -1f;
					m_Page3Item06_Index = 0;
					if (m_Page3ItemBgBlack2 == null)
					{
						m_Page3ItemBgBlack2 = UIUtils.BuildImage(0, new Rect(0f, 0f, 960f, 640f), m_MatBlackAlphaChangeUI, new Rect(41f, 1f, 1f, 1f), new Vector2(960f, 640f));
						Add(m_Page3ItemBgBlack2);
					}
					m_Page3Item04_Timer = -1f;
					m_Page3Item07_Timer = 0f;
				}
			}
			if (m_Page3Item07_Timer >= 0f)
			{
				m_Page3Item07_Timer += Time.deltaTime;
				if (!(m_Page3Item07_Timer <= m_Page3Item07_Time))
				{
					if (m_Page3Item07_Timer <= m_Page3Item07_Time2)
					{
						if (m_Page3Item07 == null)
						{
							m_Page3Item07 = UIUtils.BuildImage(0, new Rect(171f, 84f, 536f, 264f), m_MatCartoonAnim05UI, new Rect(0f, 0f, 536f, 264f), new Vector2(536f, 264f));
							Add(m_Page3Item07);
						}
						Vector2[] array5 = new Vector2[13]
						{
							new Vector2(166f, 90f),
							new Vector2(160f, 92f),
							new Vector2(154f, 89f),
							new Vector2(149f, 91f),
							new Vector2(141f, 97f),
							new Vector2(137f, 95f),
							new Vector2(129f, 99f),
							new Vector2(124f, 104f),
							new Vector2(120f, 102f),
							new Vector2(114f, 104f),
							new Vector2(108f, 109f),
							new Vector2(103f, 106f),
							new Vector2(95f, 110f)
						};
						int num17 = m_Page3Item07_Index % (array5.Length * m_Page3Item07_MoveSpeed);
						num17 /= m_Page3Item07_MoveSpeed;
						int num18 = num17;
						if (m_Page3Item07_Index < array5.Length * m_Page3Item07_MoveSpeed)
						{
							m_Page3Item07_Index++;
						}
						else
						{
							m_Page3Item07_Index = array5.Length * m_Page3Item07_MoveSpeed;
							num18 = (num17 = 12);
						}
						float x = array5[num18].x;
						float y = array5[num18].y;
						m_Page3Item07.Rect = AutoUI.AutoRect(new Rect(x, y, 536f, 264f));
					}
					else
					{
						m_Page3Item07_Index = 0;
						m_Page3Item07_Timer = -1f;
						m_Page3Item07.Rect = AutoUI.AutoRect(new Rect(95f, 112f, 536f, 264f));
						m_Page3Item07.SetColor(new Color(1f, 1f, 1f, 0.7f));
						if (m_Page3Item08 == null)
						{
							m_Page3Item08 = UIUtils.BuildImage(0, new Rect(503f, 207f, 214f, 197f), m_MatCartoonAnim04UI, new Rect(750f, 680f, 252f, 223f), new Vector2(214f, 197f));
							Add(m_Page3Item08);
						}
						m_Page3Item08_Timer = 0f;
					}
				}
			}
			if (m_Page3Item08_Timer >= 0f)
			{
				m_Page3Item08_Timer += Time.deltaTime;
				if (m_Page3Item08_Timer <= m_Page3Item08_Time)
				{
					m_Page3Item07.SetColor(new Color(1f, 1f, 1f, 0f));
					float num19 = m_Page3Item08_Timer / m_Page3Item08_Time * 80f;
					float num20 = m_Page3Item08_Timer / m_Page3Item08_Time * 80f;
					m_Page3Item08.SetTexture(m_MatCartoonAnim04UI, AutoUI.AutoRect(new Rect(750f, 680f, 252f, 223f)), AutoUI.AutoSize(new Vector2(214f + num19, 197f + num20)));
				}
				else
				{
					m_Page3Item08_Timer = -1f;
					m_Page3Item09_Timer = 0f;
				}
			}
			if (m_Page3Item09_Timer >= 0f)
			{
				m_Page3Item09_Timer += Time.deltaTime;
				if (m_Page3Item09_Timer <= m_Page3Item09_Time)
				{
					float num21 = 1f - m_Page3Item09_Timer / m_Page3Item09_Time - 0.4f;
					if (num21 <= 0f)
					{
						num21 = 0f;
					}
					m_Page3Item08.SetColor(new Color(1f, 1f, 1f, num21));
				}
				else if (m_Page3Item09_Timer <= m_Page3Item09_Time2)
				{
					if (m_Page3Item09 == null)
					{
						m_Page3Item08.SetColor(new Color(1f, 1f, 1f, 0f));
						m_Page3Item09 = UIUtils.BuildImage(0, new Rect(805f, 83f, 149f, 134f), m_MatCartoonAnim05UI, new Rect(803f, 525f, 195f, 160f), new Vector2(149f, 134f));
						Add(m_Page3Item09);
					}
				}
				else if (m_Page3Item09_Timer <= m_Page3Item09_Time3)
				{
					m_Page3Item08.SetColor(new Color(1f, 1f, 1f, 0f));
					float num22 = m_Page3Item09_Timer / m_Page3Item09_Time3 * 50f;
					float num23 = m_Page3Item09_Timer / m_Page3Item09_Time3 * 30f;
					m_Page3Item09.SetTexture(m_MatCartoonAnim05UI, AutoUI.AutoRect(new Rect(803f, 525f, 195f, 160f)), AutoUI.AutoSize(new Vector2(149f + num22, 134f + num23)));
				}
				else if (m_Page3Item09_Timer <= m_Page3Item09_Time4)
				{
					float num24 = m_Page3Item09_Timer / m_Page3Item09_Time4 * 50f;
					float num25 = m_Page3Item09_Timer / m_Page3Item09_Time4 * 30f;
					m_Page3Item09.SetTexture(m_MatCartoonAnim05UI, AutoUI.AutoRect(new Rect(803f, 525f, 195f, 160f)), AutoUI.AutoSize(new Vector2(199f - num24, 164f - num25)));
				}
				else
				{
					m_Page3Item09.SetColor(new Color(1f, 1f, 1f, 0f));
					m_Page3Item09_Timer = -1f;
					if (m_Page3ItemBgBlack3 == null)
					{
						m_Page3ItemBgBlack3 = UIUtils.BuildImage(0, new Rect(0f, 0f, 960f, 640f), m_MatBlackAlphaChangeUI, new Rect(41f, 1f, 1f, 1f), new Vector2(960f, 640f));
						Add(m_Page3ItemBgBlack3);
					}
					m_Page3Item10_Timer = 0f;
				}
			}
			if (m_Page3Item10_Timer >= 0f)
			{
				m_Page3Item10_Timer += Time.deltaTime;
				if (!(m_Page3Item10_Timer <= m_Page3Item10_Time))
				{
					if (m_Page3Item10_Timer <= m_Page3Item10_Time2)
					{
						if (m_Page3Item10 == null)
						{
							m_Page3Item10 = UIUtils.BuildImage(0, new Rect(95f, 0f, 727f, 413f), m_MatCartoonAnim04UI, new Rect(0f, 610f, 727f, 413f), new Vector2(727f, 413f));
							Add(m_Page3Item10);
						}
						float num26 = (m_Page3Item10_Timer - m_Page3Item10_Time) / (m_Page3Item10_Time2 - m_Page3Item10_Time) + 0.2f;
						if (num26 >= 1f)
						{
							num26 = 1f;
						}
						m_Page3Item10.SetColor(new Color(1f, 1f, 1f, num26));
					}
					else
					{
						m_Page3Item10_Timer = -1f;
						m_Page3Item11_Timer = 0f;
					}
				}
			}
			if (m_Page3Item11_Timer >= 0f)
			{
				m_Page3Item11_Timer += Time.deltaTime;
				if (m_Page3Item11_Timer <= m_Page3Item11_Time)
				{
					if (m_Page3Item11 == null)
					{
						m_Page3Item11 = UIUtils.BuildImage(0, new Rect(762f, 0f, 227f, 190f), m_MatCartoonAnim05UI, new Rect(750f, 0f, 227f, 190f), new Vector2(227f, 190f));
						Add(m_Page3Item11);
					}
					float num27 = m_Page3Item11_Timer / m_Page3Item11_Time + 0.2f;
					if (num27 < 0f)
					{
						num27 = 0f;
					}
					m_Page3Item11.SetColor(new Color(1f, 1f, 1f, num27));
				}
				else
				{
					m_Page3Item11_Timer = -1f;
					m_Page3ItemLast_Timer = 0f;
				}
			}
			if (m_Page3ItemLast_Timer >= 0f)
			{
				m_Page3ItemLast_Timer += Time.deltaTime;
				if (m_Page3ItemLast_Timer > m_Page3ItemLast_Time)
				{
					m_Page3ItemLast_Timer = -1f;
					if (m_PageIndex == 3)
					{
						m_PageIndex = 4;
						BeginFadeIn();
					}
				}
			}
		}
		else if (m_PageIndex == 4)
		{
			if (m_Page4Item01Fg_Timer >= 0f)
			{
				m_Page4Item01Fg_Timer += Time.deltaTime;
				if (m_Page4Item01Fg_Timer <= m_Page4Item01Fg_Time)
				{
					m_Page4Item01Fg.Rect = AutoUI.AutoRect(new Rect(-490f + m_Page4Item01Fg_Timer / m_Page4Item01Fg_Time * 525f, 365f, 490f, 230f));
					m_Page4Item01.Rect = AutoUI.AutoRect(new Rect(-490f + m_Page4Item01Fg_Timer / m_Page4Item01Fg_Time * 525f, 365f, 490f, 230f));
				}
				else
				{
					m_Page4Item01Fg.Rect = AutoUI.AutoRect(new Rect(35f, 365f, 490f, 230f));
					m_Page4Item01.Rect = AutoUI.AutoRect(new Rect(35f, 365f, 490f, 230f));
					m_Page4Item01Fg_Timer = -1f;
					m_Page4Item01_Timer = 0f;
				}
			}
			if (m_Page4Item01_Timer >= 0f)
			{
				m_Page4Item01_Timer += Time.deltaTime;
				if (m_Page4Item01_Timer < m_Page4Item01_Time)
				{
					float left = m_Page4Item01_Timer / m_Page4Item01_Time * 39f;
					m_Page4Item01.SetTexture(m_MatCartoonAnim07UI, AutoUI.AutoRect(new Rect(left, 0f, 490f, 230f)), AutoUI.AutoSize(new Vector2(490f, 230f)));
				}
				else
				{
					m_Page4Item01_Timer = -1f;
				}
				if (m_Page4Item01_Timer >= m_Page4Item01_Time2 && m_Page4Item02 == null)
				{
					m_Page4Item02 = UIUtils.BuildImage(0, new Rect(444f, 492f, 82f, 133f), m_MatCartoonAnim07UI, new Rect(287f, 245f, 106f, 181f), new Vector2(78f, 122f));
					Add(m_Page4Item02);
					m_Page4Item02_Timer = 0f;
				}
			}
			if (m_Page4Item02_Timer >= 0f)
			{
				m_Page4Item02_Timer += Time.deltaTime;
				if (!(m_Page4Item02_Timer <= m_Page4Item02_Time))
				{
					if (m_Page4Item02_Timer <= m_Page4Item02_Time2)
					{
						m_Page4Item02.SetTexture(m_MatCartoonAnim07UI, AutoUI.AutoRect(new Rect(287f, 245f, 106f, 181f)), AutoUI.AutoSize(new Vector2(106f, 181f)));
					}
					else
					{
						m_Page4Item02_Timer = -1f;
						m_Page4Item03Fg_Timer = 0f;
					}
				}
			}
			if (m_Page4Item03Fg_Timer >= 0f)
			{
				if (m_Page4Item02 != null)
				{
					m_Page4Item02.SetTexture(m_MatCartoonAnim07UI, AutoUI.AutoRect(new Rect(287f, 245f, 106f, 181f)), AutoUI.AutoSize(new Vector2(78f, 122f)));
				}
				m_Page4Item03Fg_Timer += Time.deltaTime;
				if (m_Page4Item03Fg_Timer <= m_Page4Item03Fg_Time)
				{
					if (m_Page4Item03Fg == null)
					{
						m_Page4Item03 = UIUtils.BuildImage(0, new Rect(-218f, 28f, 218f, 304f), m_MatCartoonAnim07UI, new Rect(558f, 0f, 218f, 304f), new Vector2(218f, 304f));
						Add(m_Page4Item03);
						m_Page4Item03Fg = UIUtils.BuildImage(0, new Rect(-218f, 28f, 218f, 304f), m_MatCartoonAnim07UI, new Rect(772f, 0f, 218f, 304f), new Vector2(218f, 304f));
						Add(m_Page4Item03Fg);
					}
					m_Page4Item03Fg.Rect = AutoUI.AutoRect(new Rect(-218f + m_Page4Item03Fg_Timer / m_Page4Item03Fg_Time * 253f, 28f, 218f, 304f));
					m_Page4Item03.Rect = AutoUI.AutoRect(new Rect(-218f + m_Page4Item03Fg_Timer / m_Page4Item03Fg_Time * 253f, 28f, 218f, 304f));
				}
				else if (m_Page4Item03Fg_Timer <= m_Page4Item03Fg_Time2)
				{
					m_Page4Item03Fg.Rect = AutoUI.AutoRect(new Rect(35f, 28f, 218f, 304f));
					m_Page4Item03.Rect = AutoUI.AutoRect(new Rect(35f, 28f, 218f, 304f));
					m_Page4Item02.SetColor(new Color(1f, 1f, 1f, 0f));
					float num28 = (m_Page4Item03Fg_Timer - m_Page3Item04_Time) / (m_Page4Item03Fg_Time2 - m_Page3Item04_Time);
					m_Page4Item03.SetColor(new Color(m_Page3Item04Col * (0.5f + 0.5f * num28 / 0.8f), 0f, 0f, 1f));
				}
				else
				{
					m_Page4Item03Fg.Rect = AutoUI.AutoRect(new Rect(35f, 28f, 218f, 304f));
					m_Page4Item03.Rect = AutoUI.AutoRect(new Rect(35f, 28f, 218f, 304f));
					m_Page4Item02.SetColor(new Color(1f, 1f, 1f, 0f));
					m_Page4Item03_Index = 0f;
					m_Page4Item03Fg_Timer = -1f;
					if (m_Page4Item04 == null)
					{
						m_Page4Item03.SetColor(new Color(1f, 1f, 1f, 1f));
						m_Page4Item04 = UIUtils.BuildImage(0, new Rect(-9f, 230f, 219f, 184f), m_MatCartoonAnim06UI, new Rect(491f, 723f, 327f, 266f), new Vector2(219f, 184f));
						Add(m_Page4Item04);
					}
					m_Page4Item04_Timer = 0f;
				}
			}
			if (m_Page4Item04_Timer >= 0f)
			{
				m_Page4Item04_Timer += Time.deltaTime;
				if (m_Page4Item04_Timer <= m_Page4Item04_Time_1)
				{
					m_Page4Item04.SetTexture(m_MatCartoonAnim06UI, AutoUI.AutoRect(new Rect(491f, 723f, 327f, 266f)), AutoUI.AutoSize(new Vector2(273f, 225f)));
				}
				else if (m_Page4Item04_Timer <= m_Page4Item04_Time)
				{
					m_Page4Item04.SetTexture(m_MatCartoonAnim06UI, AutoUI.AutoRect(new Rect(491f, 723f, 327f, 266f)), AutoUI.AutoSize(new Vector2(327f, 266f)));
				}
				else if (m_Page4Item04_Timer <= m_Page4Item04_Time + 0.5f)
				{
					m_Page4Item04.SetTexture(m_MatCartoonAnim06UI, AutoUI.AutoRect(new Rect(491f, 723f, 327f, 266f)), AutoUI.AutoSize(new Vector2(219f, 184f)));
					m_Page4Item04.SetColor(new Color(1f, 1f, 1f, (m_Page4Item04_Time + 0.5f - m_Page4Item04_Timer) / 0.5f));
				}
				else
				{
					m_Page4Item04.Visible = false;
					m_Page4Item04_Timer = -1f;
					m_Page4Item05Fg_Timer = 0f;
				}
			}
			if (m_Page4Item05Fg_Timer >= 0f)
			{
				m_Page4Item05Fg_Timer += Time.deltaTime;
				if (m_Page4Item05Fg == null)
				{
					m_Page4Item05 = UIUtils.BuildImage(0, new Rect(-130f, 28f, 130f, 304f), m_MatCartoonAnim07UI, new Rect(12f, 262f, 130f, 304f), new Vector2(130f, 304f));
					Add(m_Page4Item05);
					m_Page4Item05Fg = UIUtils.BuildImage(0, new Rect(-130f, 28f, 130f, 304f), m_MatCartoonAnim07UI, new Rect(156f, 245f, 130f, 304f), new Vector2(130f, 304f));
					Add(m_Page4Item05Fg);
				}
				if (m_Page4Item05Fg_Timer <= m_Page4Item05Fg_Time)
				{
					m_Page4Item05Fg.Rect = AutoUI.AutoRect(new Rect(-130f + m_Page4Item05Fg_Timer / m_Page4Item05Fg_Time * 420f, 28f, 130f, 304f));
					m_Page4Item05.Rect = AutoUI.AutoRect(new Rect(-130f + m_Page4Item05Fg_Timer / m_Page4Item05Fg_Time * 420f, 28f, 130f, 304f));
				}
				else
				{
					m_Page4Item05Fg.Rect = AutoUI.AutoRect(new Rect(290f, 28f, 130f, 304f));
					m_Page4Item05.Rect = AutoUI.AutoRect(new Rect(290f, 28f, 130f, 304f));
					m_Page4Item05Fg_Timer = -1f;
					m_Page4Item05_Timer = 0f;
				}
			}
			if (m_Page4Item05_Timer >= 0f)
			{
				m_Page4Item05_Timer += Time.deltaTime;
				if (m_Page4Item05_Timer <= m_Page4Item05_Time)
				{
					if (m_Page4Item05_MoveSpeed < 20f)
					{
						m_Page4Item05_MoveSpeed += 1f;
					}
					float num29 = m_Page4Item05_Timer / m_Page4Item05_Time;
					int num30 = Mathf.FloorToInt(m_Page4Item05_MoveSpeed);
					m_Page4Item05.SetTexture(m_MatCartoonAnim07UI, AutoUI.AutoRect(new Rect(25f - 25f * num29, 279f - 34f * num29, 130f + 25f * num29, 304f + 34f * num29)), AutoUI.AutoSize(new Vector2(130f, 304f)));
				}
				else
				{
					m_Page4Item05.SetTexture(m_MatCartoonAnim07UI, AutoUI.AutoRect(new Rect(0f, 245f, 155f, 334f)), AutoUI.AutoSize(new Vector2(130f, 304f)));
					m_Page4Item05_Timer = -1f;
					m_Page4Item05_MoveSpeed = 0f;
					if (m_Page4Item06 != null)
					{
						m_Page4Item06.SetColor(new Color(1f, 1f, 1f, 1f));
					}
					m_Page4Item06_Timer = 0f;
				}
			}
			if (m_Page4Item06_Timer >= 0f)
			{
				m_Page4Item06_Timer += Time.deltaTime;
				if (m_Page4Item06_Timer <= m_Page4Item06_Time2)
				{
					m_Page4Item06.SetTexture(m_MatCartoonAnim06UI, AutoUI.AutoRect(new Rect(0f + m_Page4Item06_Timer / m_Page4Item06_Time2 * 32f, 0f + m_Page4Item06_Timer / m_Page4Item06_Time2 * 31f, 1023f - m_Page4Item06_Timer / m_Page4Item06_Time2 * 32f * 2f, 721f - m_Page4Item06_Timer / m_Page4Item06_Time2 * 31f * 2f)), AutoUI.AutoSize(new Vector2(960f, 640f)));
				}
				else
				{
					m_Page4Item06_Timer = -1f;
				}
				if (!(m_Page4Item06_Timer <= m_Page4Item06_Time) && m_Page4Item07 == null)
				{
					m_Page4Item07_Timer = 0f;
					m_Page4Item07 = UIUtils.BuildImage(0, new Rect(528f, 365f, 127f, 151f), m_MatCartoonAnim06UI, new Rect(818f, 723f, 160f, 218f), new Vector2(127f, 151f));
					Add(m_Page4Item07);
				}
			}
			if (m_Page4Item07_Timer >= 0f)
			{
				m_Page4Item07_Timer += Time.deltaTime;
				if (!(m_Page4Item07_Timer <= m_Page4Item07_Time_0))
				{
					if (m_Page4Item07_Timer <= m_Page4Item07_Time)
					{
						m_Page4Item07.SetTexture(m_MatCartoonAnim06UI, AutoUI.AutoRect(new Rect(818f, 723f, 160f, 218f)), AutoUI.AutoSize(new Vector2(160f, 218f)));
					}
					else if (m_Page4Item07_Timer <= m_Page4Item07_Time2)
					{
						m_Page4Item07.SetTexture(m_MatCartoonAnim06UI, AutoUI.AutoRect(new Rect(818f, 723f, 160f, 218f)), AutoUI.AutoSize(new Vector2(127f, 151f)));
					}
					else
					{
						m_Page4BlackColorAnim = new UIAnimationControl();
						m_Page4BlackColorAnim.Id = 0;
						m_Page4BlackColorAnim.SetAnimationsPageCount(10);
						m_Page4BlackColorAnim.Rect = AutoUI.AutoRect(new Rect(0f, 0f, 960f, 640f));
						m_Page4BlackColorAnim.SetTexture(0, m_MatBlackAlphaChangeUI, AutoUI.AutoRect(new Rect(1f, 1f, 1f, 1f)), AutoUI.AutoSize(new Vector2(960f, 640f)));
						m_Page4BlackColorAnim.SetTexture(1, m_MatBlackAlphaChangeUI, AutoUI.AutoRect(new Rect(5f, 1f, 1f, 1f)), AutoUI.AutoSize(new Vector2(960f, 640f)));
						m_Page4BlackColorAnim.SetTexture(2, m_MatBlackAlphaChangeUI, AutoUI.AutoRect(new Rect(9f, 1f, 1f, 1f)), AutoUI.AutoSize(new Vector2(960f, 640f)));
						m_Page4BlackColorAnim.SetTexture(3, m_MatBlackAlphaChangeUI, AutoUI.AutoRect(new Rect(13f, 1f, 1f, 1f)), AutoUI.AutoSize(new Vector2(960f, 640f)));
						m_Page4BlackColorAnim.SetTexture(4, m_MatBlackAlphaChangeUI, AutoUI.AutoRect(new Rect(17f, 1f, 1f, 1f)), AutoUI.AutoSize(new Vector2(960f, 640f)));
						m_Page4BlackColorAnim.SetTexture(5, m_MatBlackAlphaChangeUI, AutoUI.AutoRect(new Rect(21f, 1f, 1f, 1f)), AutoUI.AutoSize(new Vector2(960f, 640f)));
						m_Page4BlackColorAnim.SetTexture(6, m_MatBlackAlphaChangeUI, AutoUI.AutoRect(new Rect(21f, 1f, 1f, 1f)), AutoUI.AutoSize(new Vector2(960f, 640f)));
						m_Page4BlackColorAnim.SetTexture(7, m_MatBlackAlphaChangeUI, AutoUI.AutoRect(new Rect(21f, 1f, 1f, 1f)), AutoUI.AutoSize(new Vector2(960f, 640f)));
						m_Page4BlackColorAnim.SetTexture(8, m_MatBlackAlphaChangeUI, AutoUI.AutoRect(new Rect(21f, 1f, 1f, 1f)), AutoUI.AutoSize(new Vector2(960f, 640f)));
						m_Page4BlackColorAnim.SetTexture(9, m_MatBlackAlphaChangeUI, AutoUI.AutoRect(new Rect(21f, 1f, 1f, 1f)), AutoUI.AutoSize(new Vector2(960f, 640f)));
						m_Page4BlackColorAnim.SetTimeInterval(0.08f);
						m_Page4BlackColorAnim.SetLoopCount(1);
						Add(m_Page4BlackColorAnim);
						m_Page4Item07.SetColor(new Color(1f, 1f, 1f, 0f));
						m_Page4Item07_Timer = -1f;
						m_Page4ItemBgBlack_Timer = 0f;
					}
				}
			}
			if (m_Page4ItemBgBlack_Timer >= 0f)
			{
				m_Page4ItemBgBlack_Timer += Time.deltaTime;
				if (m_Page4ItemBgBlack_Timer <= m_Page4ItemBgBlack_Time)
				{
					if (m_Page4ItemBgBlack == null)
					{
						m_Page4BlackColorAnim.Visible = false;
						m_Page4ItemBgBlack = UIUtils.BuildImage(0, new Rect(0f, 0f, 960f, 640f), m_MatBlackAlphaChangeUI, new Rect(21f, 1f, 1f, 1f), new Vector2(960f, 640f));
						Add(m_Page4ItemBgBlack);
					}
				}
				else
				{
					PlayEnd = true;
				}
			}
		}
		if (m_UIFade != null)
		{
			if (m_bFadeIn)
			{
				m_UIFade.enableAlphaAnimation = true;
				m_UIFade.StartFade(m_FadeInStartColor, m_FadeInEndColor, 1f);
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
			if (m_bFadeingIn && m_UIFade.FadeInComplete() && m_PageIndex <= 4)
			{
				BeginPage();
				m_bFadeOut = true;
				m_bFadeingIn = false;
			}
		}
	}

	public override void Draw()
	{
		for (int i = 0; i < m_Controls.Count; i++)
		{
			UIControl uIControl = (UIControl)m_Controls[i];
			uIControl.Update();
			if (uIControl.Visible)
			{
				uIControl.Draw();
			}
		}
	}

	public void DrawSprite(UISprite sprite)
	{
		m_Parent.DrawSprite(sprite);
	}

	public void SendEvent(UIControl control, int command, float wparam, float lparam)
	{
		if (m_UIHandler != null)
		{
			m_UIHandler.HandleEvent(control, command, wparam, lparam);
		}
		else
		{
			m_Parent.SendEvent(this, command, wparam, lparam);
		}
	}

	public override bool HandleInput(UITouchInner touch)
	{
		if (m_SkipBtn != null)
		{
			bool flag = m_SkipBtn.HandleInput(touch);
		}
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

	public void HandleEvent(UIControl control, int command, float wparam, float lparam)
	{
		if (control == m_SkipBtn && Time.time - m_LastSkipTime >= m_SkipTimer)
		{
			m_LastSkipTime = Time.time;
			HandleSkip();
		}
	}

	private void BeginPage()
	{
		Clear();
		m_Page1Bg = null;
		m_Page1Item01 = null;
		m_Page1BlackColorAnim = null;
		m_Page1ItemBgBlack = null;
		m_Page1Item02Fg = null;
		m_Page1Item02 = null;
		m_Page1Item03Fg = null;
		m_Page1Item03 = null;
		m_Page1Item04Fg = null;
		m_Page1Item04 = null;
		m_Page2Bg = null;
		m_Page2Item01 = null;
		m_Page2BlackColorAnim = null;
		m_Page2ItemBgBlack = null;
		m_Page2Item02 = null;
		m_Page2Item03Fg = null;
		m_Page2Item03 = null;
		m_Page2Item04Fg = null;
		m_Page2Item04 = null;
		m_Page2Item04_01 = null;
		m_Page2Item05 = null;
		m_Page2Item05Fg = null;
		m_Page2Item06 = null;
		m_Page2Item07 = null;
		m_Page2Item08 = null;
		m_Page2Item09 = null;
		m_Page2Item09Fg = null;
		m_Page2Item10 = null;
		m_Page2Item11 = null;
		m_Page2Item12 = null;
		m_Page2Item12Fg = null;
		m_Page3Bg = null;
		m_Page3Item01 = null;
		m_Page3Item01Fg = null;
		m_Page3Item02 = null;
		m_Page3Item03 = null;
		m_Page3Item03Fg = null;
		m_Page3ItemBgBlack = null;
		m_Page3Item04 = null;
		m_Page3Item05 = null;
		m_Page3Item06 = null;
		m_Page3Item06_Index = 0;
		m_Page3ItemBgBlack2 = null;
		m_Page3Item07 = null;
		m_Page3Item07_Index = 0;
		m_Page3Item08 = null;
		m_Page3Item09 = null;
		m_Page3ItemBgBlack3 = null;
		m_Page3Item10 = null;
		m_Page3Item11 = null;
		m_Page4Bg = null;
		m_Page4Item01 = null;
		m_Page4Item01Fg = null;
		m_Page4Item02 = null;
		m_Page4Item03 = null;
		m_Page4Item03Fg = null;
		m_Page4Item04 = null;
		m_Page4Item05 = null;
		m_Page4Item05Fg = null;
		m_Page4Item06 = null;
		m_Page4Item07 = null;
		m_PageBlackBg = UIUtils.BuildImage(0, new Rect(0f, 0f, 960f, 640f), m_MatBlackAlphaChangeUI, new Rect(41f, 1f, 1f, 1f), new Vector2(960f, 640f));
		m_PageBlackBg.CatchMessage = true;
		Add(m_PageBlackBg);
		if (m_SkipBtn == null)
		{
			m_SkipBtn = UIUtils.BuildClickButton(0, new Rect(0f, 0f, 960f, 640f), m_MatTransparentUI, new Rect(1f, 1f, 1f, 1f), new Rect(1f, 1f, 1f, 1f), new Rect(1f, 1f, 1f, 1f), new Vector2(960f, 640f));
			m_SkipBtn.SetParent(this);
		}
		if (m_PageIndex == 1)
		{
			m_MatCartoonAnim01UI = LoadUIMaterial("Zombie3D/UI/Materials/CartoonAnim01UI");
			m_MatCartoonAnim07UI = LoadUIMaterial("Zombie3D/UI/Materials/CartoonAnim07UI");
			m_Page1Bg = UIUtils.BuildImage(0, new Rect(0f, 0f, 960f, 640f), m_MatBlackAlphaChangeUI, new Rect(41f, 1f, 1f, 1f), new Vector2(960f, 640f));
			Add(m_Page1Bg);
			m_Page1Item01 = UIUtils.BuildImage(0, new Rect(0f, -30f, 960f, 640f), m_MatCartoonAnim01UI, new Rect(0f, 0f, 960f, 640f), new Vector2(960f, 640f));
			Add(m_Page1Item01);
			m_Page1Item01_Timer = 0f;
		}
		else if (m_PageIndex == 2)
		{
			m_MatCartoonAnim01UI = null;
			m_MatCartoonAnim07UI = null;
			m_MatCartoonAnim03UI = LoadUIMaterial("Zombie3D/UI/Materials/CartoonAnim03UI");
			m_Page2Bg = UIUtils.BuildImage(0, new Rect(0f, 0f, 960f, 640f), m_MatBlackAlphaChangeUI, new Rect(41f, 1f, 1f, 1f), new Vector2(960f, 640f));
			Add(m_Page2Bg);
			m_Page2Item01 = UIUtils.BuildImage(0, new Rect(0f, -27f, 492f, 667f), m_MatCartoonAnim03UI, new Rect(0f, 0f, 492f, 667f), new Vector2(492f, 667f));
			Add(m_Page2Item01);
			m_Page2BlackColorAnim = new UIAnimationControl();
			m_Page2BlackColorAnim.Id = 0;
			m_Page2BlackColorAnim.SetAnimationsPageCount(8);
			m_Page2BlackColorAnim.Rect = AutoUI.AutoRect(new Rect(0f, 0f, 960f, 640f));
			m_Page2BlackColorAnim.SetTexture(7, m_MatBlackAlphaChangeUI, AutoUI.AutoRect(new Rect(1f, 1f, 1f, 1f)), AutoUI.AutoSize(new Vector2(960f, 640f)));
			m_Page2BlackColorAnim.SetTexture(6, m_MatBlackAlphaChangeUI, AutoUI.AutoRect(new Rect(5f, 1f, 1f, 1f)), AutoUI.AutoSize(new Vector2(960f, 640f)));
			m_Page2BlackColorAnim.SetTexture(5, m_MatBlackAlphaChangeUI, AutoUI.AutoRect(new Rect(9f, 1f, 1f, 1f)), AutoUI.AutoSize(new Vector2(960f, 640f)));
			m_Page2BlackColorAnim.SetTexture(4, m_MatBlackAlphaChangeUI, AutoUI.AutoRect(new Rect(13f, 1f, 1f, 1f)), AutoUI.AutoSize(new Vector2(960f, 640f)));
			m_Page2BlackColorAnim.SetTexture(3, m_MatBlackAlphaChangeUI, AutoUI.AutoRect(new Rect(17f, 1f, 1f, 1f)), AutoUI.AutoSize(new Vector2(960f, 640f)));
			m_Page2BlackColorAnim.SetTexture(2, m_MatBlackAlphaChangeUI, AutoUI.AutoRect(new Rect(21f, 1f, 1f, 1f)), AutoUI.AutoSize(new Vector2(960f, 640f)));
			m_Page2BlackColorAnim.SetTexture(1, m_MatBlackAlphaChangeUI, AutoUI.AutoRect(new Rect(25f, 1f, 1f, 1f)), AutoUI.AutoSize(new Vector2(960f, 640f)));
			m_Page2BlackColorAnim.SetTexture(0, m_MatBlackAlphaChangeUI, AutoUI.AutoRect(new Rect(29f, 1f, 1f, 1f)), AutoUI.AutoSize(new Vector2(960f, 640f)));
			m_Page2BlackColorAnim.SetTimeInterval(0.08f);
			m_Page2BlackColorAnim.SetLoopCount(1);
			Add(m_Page2BlackColorAnim);
			m_Page2Item01_Timer = 0f;
		}
		else if (m_PageIndex == 3)
		{
			m_MatCartoonAnim03UI = null;
			m_MatCartoonAnim04UI = LoadUIMaterial("Zombie3D/UI/Materials/CartoonAnim04UI");
			m_MatCartoonAnim05UI = LoadUIMaterial("Zombie3D/UI/Materials/CartoonAnim05UI");
			m_Page3Bg = UIUtils.BuildImage(0, new Rect(0f, 0f, 960f, 640f), m_MatBlackAlphaChangeUI, new Rect(41f, 1f, 1f, 1f), new Vector2(960f, 640f));
			Add(m_Page3Bg);
			m_Page3Item01 = UIUtils.BuildImage(0, new Rect(-170f, 276f, 230f, 338f), m_MatCartoonAnim04UI, new Rect(745f, 0f, 230f, 338f), new Vector2(230f, 338f));
			Add(m_Page3Item01);
			m_Page3Item01Fg = UIUtils.BuildImage(0, new Rect(-170f, 276f, 230f, 338f), m_MatCartoonAnim04UI, new Rect(745f, 335f, 230f, 338f), new Vector2(230f, 338f));
			Add(m_Page3Item01Fg);
			m_Page3Item01Fg_Timer = 0f;
		}
		else if (m_PageIndex == 4)
		{
			m_MatCartoonAnim04UI = null;
			m_MatCartoonAnim05UI = null;
			m_MatCartoonAnim06UI = LoadUIMaterial("Zombie3D/UI/Materials/CartoonAnim06UI");
			m_MatCartoonAnim07UI = LoadUIMaterial("Zombie3D/UI/Materials/CartoonAnim07UI");
			m_Page4Bg = UIUtils.BuildImage(0, new Rect(0f, 0f, 960f, 640f), m_MatBlackAlphaChangeUI, new Rect(41f, 1f, 1f, 1f), new Vector2(960f, 640f));
			Add(m_Page4Bg);
			m_Page4Item06 = UIUtils.BuildImage(0, new Rect(0f, 0f, 960f, 640f), m_MatCartoonAnim06UI, new Rect(0f, 0f, 1023f, 721f), new Vector2(960f, 640f));
			Add(m_Page4Item06);
			m_Page4Item06.SetColor(new Color(1f, 1f, 1f, 0f));
			m_Page4Item01 = UIUtils.BuildImage(0, new Rect(-490f, 365f, 490f, 230f), m_MatCartoonAnim07UI, new Rect(0f, 0f, 490f, 230f), new Vector2(490f, 230f));
			Add(m_Page4Item01);
			m_Page4Item01Fg = UIUtils.BuildImage(0, new Rect(-490f, 365f, 490f, 230f), m_MatCartoonAnim06UI, new Rect(0f, 722f, 490f, 230f), new Vector2(490f, 230f));
			Add(m_Page4Item01Fg);
			m_Page4Item01Fg_Timer = 0f;
		}
		else
		{
			m_MatCartoonAnim06UI = null;
			m_MatCartoonAnim07UI = null;
		}
		Resources.UnloadUnusedAssets();
	}

	public void BeginFadeIn()
	{
		GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load("Zombie3D/UI/Screen_UIFadeOut", typeof(GameObject)), new Vector3(0f, 1000f, 0f), Quaternion.identity) as GameObject;
		if (gameObject != null)
		{
			m_UIFade = gameObject.GetComponent(typeof(FadeAnimationScript)) as FadeAnimationScript;
		}
		else
		{
			Debug.LogError("Cannot Find Screen UI FadeOut GameObject!!!");
		}
		m_bFadeIn = true;
		m_bFadeOut = false;
		m_bFadeingIn = false;
		m_bFadeingOut = false;
	}

	private void HandleSkip()
	{
		if (m_PageIndex == 1)
		{
			if (m_Page1Item01_Timer >= 0f)
			{
				m_Page1BlackColorAnim_Timer = -1f;
				m_Page1Item01_Timer = m_Page1Item01_Time;
			}
			else if (m_Page1BlackColorAnim_Timer >= 0f)
			{
				m_Page1Item02Fg_Timer = -1f;
				m_Page1BlackColorAnim_Timer = m_Page1BlackColorAnim_Time;
			}
			else if (m_Page1Item02Fg_Timer >= 0f)
			{
				m_Page1Item02_Timer = -1f;
				m_Page1Item02Fg_Timer = m_Page1Item02Fg_Time;
			}
			else if (m_Page1Item02_Timer >= 0f)
			{
				m_Page1Item03Fg_Timer = -1f;
				m_Page1Item02_Timer = m_Page1Item02_Time;
			}
			else if (m_Page1Item03Fg_Timer >= 0f)
			{
				m_Page1Item03_Timer = -1f;
				m_Page1Item03Fg_Timer = m_Page1Item03Fg_Time;
			}
			else if (m_Page1Item03_Timer >= 0f)
			{
				m_Page1Item04Fg_Timer = -1f;
				m_Page1Item03_Timer = m_Page1Item03_Time;
			}
			else if (m_Page1Item04Fg_Timer >= 0f)
			{
				m_Page1Item04_Timer = -1f;
				m_Page1Item04Fg_Timer = m_Page1Item04Fg_Time;
			}
			else if (m_Page1Item04_Timer >= 0f)
			{
				m_Page1Item04_Timer = m_Page1Item04_Time;
			}
		}
		else if (m_PageIndex == 2)
		{
			if (m_Page2Item01_Timer >= 0f)
			{
				m_Page2Item02_Timer = m_Page2Item02_Time + 1f;
				if (m_Page2Item02 == null)
				{
					m_Page2Item02 = UIUtils.BuildImage(0, new Rect(250f, 200f, 160f, 200f), m_MatCartoonAnim03UI, new Rect(847f, 824f, 160f, 200f), new Vector2(160f, 200f));
					Add(m_Page2Item02);
				}
				m_Page2Item03Fg_Timer = m_Page2Item03Fg_Time;
				if (m_Page2Item03 == null || m_Page2Item03Fg == null)
				{
					m_Page2Item03 = UIUtils.BuildImage(0, new Rect(960f, 358f, 176f, 256f), m_MatCartoonAnim03UI, new Rect(658f, 753f, 187f, 271f), new Vector2(176f, 256f));
					Add(m_Page2Item03);
					m_Page2Item03Fg = UIUtils.BuildImage(0, new Rect(960f, 358f, 176f, 256f), m_MatCartoonAnim03UI, new Rect(847f, 568f, 176f, 256f), new Vector2(176f, 256f));
					Add(m_Page2Item03Fg);
				}
				m_Page2Item01_Timer = m_Page2Item01_Time;
			}
			if (m_Page2BlackColorAnim_Timer >= 0f)
			{
				m_Page2BlackColorAnim_Timer = 0.8f;
				return;
			}
			if (m_Page2Item02_Timer >= 0f)
			{
				m_Page2Item02_Timer = -1f;
				return;
			}
			if (m_Page2Item03Fg_Timer >= 0f)
			{
				m_Page2Item03Fg_Timer = m_Page2Item03Fg_Time;
				return;
			}
			if (m_Page2Item03_Timer >= 0f)
			{
				m_Page2Item04_Timer = 0f;
				m_Page2Item03_Timer = m_Page2Item03_Time;
				return;
			}
			if (m_Page2Item04_Timer >= 0f)
			{
				m_Page2Item04_Timer = m_Page2Item04_Time;
				return;
			}
			if (m_Page2Item04_01_Timer >= 0f)
			{
				m_Page2Item04_01_Timer = -1f;
			}
			if (m_Page2Item05_Timer >= 0f)
			{
				m_Page2Item05_Timer = m_Page2Item05_Time;
			}
			else if (m_Page2Item05Fg_Timer >= 0f)
			{
				m_Page2Item05Fg_Timer = m_Page2Item05Fg_Time;
			}
			else if (m_Page2Item06_Timer >= 0f)
			{
				m_Page2Item06_Timer = m_Page2Item06_Time;
			}
			else if (m_Page2Item07_Timer >= 0f)
			{
				m_Page2Item07_Timer = m_Page2Item07_Time;
			}
			else if (m_Page2Item08_Timer >= 0f)
			{
				m_Page2Item08_Timer = m_Page2Item08_Time;
			}
			else if (m_Page2Item09Fg_Timer >= 0f)
			{
				m_Page2Item09Fg_Timer = m_Page2Item09_Time;
			}
			else if (m_Page2Item10_Timer >= 0f)
			{
				m_Page2Item10_Timer = m_Page2Item10_Time;
			}
			else if (m_Page2Item11_Timer >= 0f)
			{
				m_Page2Item11_Timer = m_Page2Item11_Time;
			}
			else if (m_Page2Item12_Timer >= 0f)
			{
				m_Page2Item12_Timer = m_Page2Item12_Time;
			}
			else if (m_Page2Item12Fg_Timer >= 0f)
			{
				m_Page2Item12Fg_Timer = m_Page2Item12Fg_Time;
			}
			else if (m_Page2BlackColorAnim2_Timer >= 0f)
			{
				m_Page2BlackColorAnim2_Timer = m_Page2BlackColorAnim2_Time;
			}
		}
		else if (m_PageIndex == 3)
		{
			if (m_Page3Item01Fg_Timer >= 0f)
			{
				m_Page3Item01Fg_Timer = m_Page3Item01Fg_Time;
			}
			else if (m_Page3Item02_Timer >= 0f)
			{
				m_Page3Item02_Timer = m_Page3Item02_Time;
			}
			else if (m_Page3Item03Fg_Timer >= 0f)
			{
				m_Page3Item03Fg_Timer = m_Page3Item03Fg_Time;
			}
			else if (m_Page3Item03_Timer >= 0f)
			{
				m_Page3Item03_Timer = m_Page3Item03_Time4;
			}
			else if (m_Page3Item05_Timer >= 0f)
			{
				m_Page3Item05_Timer = m_Page3Item05_Time;
			}
			else if (m_Page3Item06_Timer >= 0f)
			{
				if (m_Page3Item06_Time < m_Page3Item06_Time * 0.75f)
				{
					m_Page3Item06_Timer = m_Page3Item06_Time * 0.75f;
				}
				if (m_Page3Item06_Timer < m_Page3Item06_Time * 1f)
				{
					m_Page3Item06_Timer = m_Page3Item06_Time * 1f;
				}
			}
			else if (m_Page3Item07_Timer >= 0f)
			{
				if (m_Page3Item07 == null)
				{
					m_Page3Item07 = UIUtils.BuildImage(0, new Rect(171f, 84f, 536f, 264f), m_MatCartoonAnim05UI, new Rect(0f, 0f, 536f, 264f), new Vector2(536f, 264f));
					Add(m_Page3Item07);
				}
				m_Page3Item07_Timer = m_Page3Item07_Time2;
			}
			else if (m_Page3Item08_Timer >= 0f)
			{
				m_Page3Item08_Timer = m_Page3Item08_Time;
			}
			else if (m_Page3Item09_Timer >= 0f)
			{
				if (m_Page3Item09_Timer < m_Page3Item09_Time2)
				{
					m_Page3Item09_Timer = m_Page3Item09_Time2;
				}
				if (m_Page3Item09_Timer < m_Page3Item09_Time4)
				{
					m_Page3Item09_Timer = m_Page3Item09_Time4;
				}
			}
			else if (m_Page3Item10_Timer >= 0f)
			{
				m_Page3Item10_Timer = m_Page3Item10_Time2;
			}
			else if (m_Page3Item11_Timer >= 0f)
			{
				m_Page3Item11_Timer = m_Page3Item11_Time;
			}
		}
		else
		{
			if (m_PageIndex != 4)
			{
				return;
			}
			if (m_Page4Item01Fg_Timer >= 0f)
			{
				m_Page4Item01Fg_Timer = m_Page4Item01Fg_Timer;
				m_Page4Item01_Timer = m_Page4Item01_Time2;
			}
			else if (m_Page4Item02_Timer >= 0f)
			{
				m_Page4Item02_Timer = m_Page4Item02_Time2;
			}
			else if (m_Page4Item03Fg_Timer >= 0f)
			{
				m_Page4Item03Fg_Timer = m_Page4Item03Fg_Time2;
			}
			else if (m_Page4Item04_Timer >= 0f)
			{
				m_Page4Item04_Timer = m_Page4Item04_Time;
			}
			else if (m_Page4Item05Fg_Timer >= 0f)
			{
				m_Page4Item05Fg_Timer = m_Page4Item05Fg_Time;
				m_Page4Item05_Timer = m_Page4Item05_Time;
			}
			else if (m_Page4Item06_Timer >= 0f)
			{
				m_Page4Item06_Timer = m_Page4Item06_Time2;
				if (m_Page4Item07 == null)
				{
					m_Page4Item07_Timer = 0f;
					m_Page4Item07 = UIUtils.BuildImage(0, new Rect(528f, 365f, 127f, 151f), m_MatCartoonAnim06UI, new Rect(818f, 723f, 160f, 218f), new Vector2(127f, 151f));
					Add(m_Page4Item07);
				}
			}
			else if (m_Page4Item07_Timer >= 0f)
			{
				m_Page4Item07_Timer = m_Page4Item07_Time2;
				m_Page4ItemBgBlack_Timer = m_Page4ItemBgBlack_Time;
			}
			else if (!(m_Page4ItemBgBlack_Timer >= 0f))
			{
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
