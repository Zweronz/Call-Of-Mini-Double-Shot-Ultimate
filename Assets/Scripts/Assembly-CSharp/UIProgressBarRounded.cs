using UnityEngine;

public class UIProgressBarRounded : UIControlVisible
{
	private float _percent;

	private int m_SpriteCount = 4;

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

	public UIProgressBarRounded()
	{
		CreateSprite(4);
	}

	~UIProgressBarRounded()
	{
	}

	public override void Draw()
	{
		if (m_Sprite != null)
		{
			for (int i = 0; i < m_Sprite.Length && i < m_SpriteCount; i++)
			{
				m_Parent.DrawSprite(m_Sprite[i]);
			}
		}
	}

	public void SetPercent(float percent)
	{
		_percent = Mathf.Clamp01(percent);
	}

	public void SetParam(Material matBackground, Rect texBgRect, Material matProgressBar, Rect texProgressTailRect, Rect texProgressBarRect, Rect texProgressHeadRect, float percent)
	{
		float num = Rect.width * percent;
		if (num <= texProgressTailRect.width)
		{
			m_Sprite[0].Position = new Vector2(Rect.x + Rect.width / 2f, Rect.y + Rect.height / 2f);
			m_Sprite[0].Size = new Vector2(Rect.width, Rect.height);
			m_Sprite[0].Material = matBackground;
			m_Sprite[0].TextureRect = AutoUI.AutoRect(texBgRect);
			m_Sprite[1].Position = new Vector2(Rect.x + texProgressTailRect.width / 2f, Rect.y + Rect.height / 2f);
			m_Sprite[1].Size = new Vector2(texProgressTailRect.width * (num / texProgressTailRect.width), Rect.height);
			m_Sprite[1].Material = matProgressBar;
			m_Sprite[1].TextureRect = texProgressTailRect;
			m_SpriteCount = 2;
		}
		else if (num <= texProgressTailRect.width + texProgressHeadRect.width)
		{
			m_Sprite[0].Position = new Vector2(Rect.x + Rect.width / 2f, Rect.y + Rect.height / 2f);
			m_Sprite[0].Size = new Vector2(Rect.width, Rect.height);
			m_Sprite[0].Material = matBackground;
			m_Sprite[0].TextureRect = texBgRect;
			m_Sprite[1].Position = new Vector2(Rect.x + texProgressTailRect.width / 2f, Rect.y + Rect.height / 2f);
			m_Sprite[1].Size = new Vector2(texProgressTailRect.width, Rect.height);
			m_Sprite[1].Material = matProgressBar;
			m_Sprite[1].TextureRect = texProgressTailRect;
			m_Sprite[2].Position = new Vector2(Rect.x + texProgressTailRect.width + (num - texProgressTailRect.width) / 2f, Rect.y + Rect.height / 2f);
			m_Sprite[2].Size = new Vector2(num - texProgressTailRect.width, Rect.height);
			m_Sprite[2].Material = matProgressBar;
			m_Sprite[2].TextureRect = texProgressHeadRect;
			m_SpriteCount = 3;
		}
		else
		{
			m_Sprite[0].Position = new Vector2(Rect.x + Rect.width / 2f, Rect.y + Rect.height / 2f);
			m_Sprite[0].Size = new Vector2(Rect.width, Rect.height);
			m_Sprite[0].Material = matBackground;
			m_Sprite[0].TextureRect = texBgRect;
			m_Sprite[1].Position = new Vector2(Rect.x + texProgressTailRect.width / 2f, Rect.y + Rect.height / 2f);
			m_Sprite[1].Size = new Vector2(texProgressTailRect.width, Rect.height);
			m_Sprite[1].Material = matProgressBar;
			m_Sprite[1].TextureRect = texProgressTailRect;
			m_Sprite[2].Position = new Vector2(Rect.x + texProgressTailRect.width + (num - texProgressTailRect.width - texProgressHeadRect.width) / 2f, Rect.y + Rect.height / 2f);
			m_Sprite[2].Size = new Vector2(num - texProgressTailRect.width - texProgressHeadRect.width + 4f, Rect.height);
			m_Sprite[2].Material = matProgressBar;
			m_Sprite[2].TextureRect = texProgressBarRect;
			m_Sprite[3].Position = new Vector2(Rect.x + (num - texProgressHeadRect.width) + texProgressHeadRect.width / 2f, Rect.y + Rect.height / 2f);
			m_Sprite[3].Size = new Vector2(texProgressTailRect.width, Rect.height);
			m_Sprite[3].Material = matProgressBar;
			m_Sprite[3].TextureRect = texProgressHeadRect;
			m_SpriteCount = 4;
		}
	}
}
