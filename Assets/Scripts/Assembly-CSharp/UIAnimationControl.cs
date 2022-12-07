using UnityEngine;

public class UIAnimationControl : UIControlVisible
{
	private float m_Interval;

	private float m_timer;

	private int m_PageNail;

	private int m_LoopCount;

	public UIAnimationEnd_CallBackEvent m_AnimationEndCallback;

	private bool m_bEnd;

	public override Rect Rect
	{
		get
		{
			return base.Rect;
		}
		set
		{
			value = AutoUIResolution.ToShiftToRight(value, 2);
			base.Rect = value;
			Vector2 position = new Vector2(value.x + value.width / 2f, value.y + value.height / 2f);
			for (int i = 0; i < m_Sprite.Length; i++)
			{
				SetSpritePosition(i, position);
				SetSpriteSize(i, new Vector2(value.width, value.height));
			}
		}
	}

	public override bool Enable
	{
		get
		{
			return base.Enable;
		}
		set
		{
			base.Enable = value;
		}
	}

	public override bool Visible
	{
		get
		{
			return base.Visible;
		}
		set
		{
			base.Visible = value;
		}
	}

	public UIAnimationControl()
	{
		m_Interval = 1f;
		m_timer = 0f;
		m_PageNail = 0;
		m_LoopCount = 1;
		m_bEnd = false;
	}

	public void SetAnimationsPageCount(int count)
	{
		CreateSprite(count);
	}

	public void SetTimeInterval(float interval)
	{
		m_Interval = interval;
	}

	public void SetLoopCount(int loop_count)
	{
		m_LoopCount = loop_count;
	}

	public void RestartTimer()
	{
		m_timer = 0f;
	}

	public void SetTexture(int page, Material material, Rect texture_rect, Vector2 size)
	{
		SetSpriteTexture(page, material, texture_rect, size);
	}

	public void SetTexture(int page, Material material, Rect texture_rect)
	{
		SetSpriteTexture(page, material, texture_rect);
	}

	public void SetColor(int page, Color color)
	{
		SetSpriteColor(page, color);
	}

	public void SetRotation(int page, float rotation)
	{
		SetSpriteRotation(page, rotation);
	}

	public void SetRotation(float rotation)
	{
		for (int i = 0; i < m_Sprite.Length; i++)
		{
			SetSpriteRotation(i, rotation);
		}
	}

	public override void Draw()
	{
		if (m_LoopCount > 0)
		{
			m_timer += Time.deltaTime;
			if (m_timer >= m_Interval)
			{
				m_timer = 0f;
				m_PageNail++;
				if (m_PageNail >= m_Sprite.Length)
				{
					m_PageNail = 0;
					m_LoopCount--;
				}
			}
			if (m_LoopCount > 0)
			{
				m_Parent.DrawSprite(m_Sprite[m_PageNail % m_Sprite.Length]);
			}
		}
		else if (!m_bEnd)
		{
			m_bEnd = true;
			if (m_AnimationEndCallback != null)
			{
				m_AnimationEndCallback(this);
			}
		}
	}

	public override bool HandleInput(UITouchInner touch)
	{
		return false;
	}
}
