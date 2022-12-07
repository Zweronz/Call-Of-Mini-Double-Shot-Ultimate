using System;
using UnityEngine;

public class UIMacOSLoading : UIControlVisible
{
	private float m_Timer;

	public override Rect Rect
	{
		get
		{
			return base.Rect;
		}
		set
		{
			base.Rect = value;
			Vector2 position = new Vector2(value.x + value.width / 2f, value.y + value.height / 2f);
			SetSpritePosition(0, position);
			SetSpriteSize(0, new Vector2(value.width, value.height));
		}
	}

	public UIMacOSLoading()
	{
		CreateSprite(1);
		m_Timer = 0f;
	}

	public void SetTexture(Material material, Rect texture_rect, Vector2 size)
	{
		SetSpriteTexture(0, material, texture_rect, size);
	}

	public override void Draw()
	{
		m_Parent.DrawSprite(m_Sprite[0]);
	}

	public override void Update()
	{
		base.Update();
		m_Timer += Time.deltaTime;
		float num = m_Timer * 360f;
		SetSpriteRotation(0, num * ((float)Math.PI / 180f));
	}

	public override bool HandleInput(UITouchInner touch)
	{
		return false;
	}
}
