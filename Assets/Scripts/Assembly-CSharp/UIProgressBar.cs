using UnityEngine;

public class UIProgressBar : UIControlVisible
{
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

	public UIProgressBar()
	{
		CreateSprite(2);
	}

	~UIProgressBar()
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

	public void SetParam(Material mat, float percent)
	{
		m_Sprite[0] = new UISprite();
		m_Sprite[0].Position = new Vector2(m_Rect.x + m_Rect.width / 2f, m_Rect.y + m_Rect.height / 2f);
		m_Sprite[0].Size = new Vector2(m_Rect.width, m_Rect.height);
		m_Sprite[0].Material = mat;
		m_Sprite[0].TextureRect = new Rect(9f, 15f, 1f, 4f);
		m_Sprite[1] = new UISprite();
		m_Sprite[1].Position = new Vector2(m_Rect.x + m_Rect.width * percent / 2f, m_Rect.y + m_Rect.height / 2f);
		m_Sprite[1].Size = new Vector2(m_Rect.width * percent, m_Rect.height);
		m_Sprite[1].Material = mat;
		m_Sprite[1].TextureRect = new Rect(1f, 15f, 1f, 4f);
	}
}
