using UnityEngine;

public class UIProgressBarProgressive : UIControlVisible
{
	private Rect rcBgTex;

	private Rect rcFgTex;

	private float _percent;

	public new Rect Rect
	{
		get
		{
			return base.Rect;
		}
		set
		{
			base.Rect = value;
		}
	}

	public UIProgressBarProgressive()
	{
		CreateSprite(2);
	}

	~UIProgressBarProgressive()
	{
	}

	public override void Draw()
	{
		if (m_Sprite != null)
		{
			for (int i = 0; i < m_Sprite.Length; i++)
			{
				m_Parent.DrawSprite(m_Sprite[i]);
			}
		}
	}

	public void SetParam(Material mat_bg, Material mat_fg, Rect rcBg_Tex, Rect rcFg_Tex, float percent)
	{
		m_Sprite[0] = new UISprite();
		m_Sprite[0].Position = new Vector2(m_Rect.x + m_Rect.width / 2f, m_Rect.y + m_Rect.height / 2f);
		m_Sprite[0].Size = new Vector2(m_Rect.width, m_Rect.height);
		m_Sprite[0].Material = mat_bg;
		m_Sprite[0].TextureRect = rcBg_Tex;
		m_Sprite[1] = new UISprite();
		m_Sprite[1].Position = new Vector2(m_Rect.x + m_Rect.width * percent / 2f, m_Rect.y + m_Rect.height / 2f);
		m_Sprite[1].Size = new Vector2(m_Rect.width * percent, m_Rect.height);
		m_Sprite[1].Material = mat_fg;
		m_Sprite[1].TextureRect = new Rect(rcFg_Tex.x, rcFg_Tex.y, rcFg_Tex.width * percent, rcFg_Tex.height);
		rcBgTex = rcBg_Tex;
		rcFgTex = rcFg_Tex;
		_percent = percent;
	}

	public void SetProgressPercent(float percent)
	{
		_percent = percent;
		m_Sprite[1].Position = new Vector2(m_Rect.x + m_Rect.width * percent / 2f, m_Rect.y + m_Rect.height / 2f);
		m_Sprite[1].Size = new Vector2(m_Rect.width * percent, m_Rect.height);
		m_Sprite[1].TextureRect = new Rect(rcFgTex.x, rcFgTex.y, rcFgTex.width * percent, rcFgTex.height);
	}
}
