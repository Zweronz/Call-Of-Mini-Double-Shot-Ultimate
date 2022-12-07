using System.Collections.Generic;
using UnityEngine;

internal class UIScrollPageView : UIControl, UIContainer
{
	public enum ListOrientation
	{
		Horizontal = 0,
		Vertical = 1
	}

	protected const float m_reboundSpeed = 1f;

	protected const float m_overscrollAllowance = 0.5f;

	protected const float m_scrollDecelCoef = 0.4f;

	protected const float m_lowPassKernelWidthInSeconds = 0.03f;

	protected const float m_scrollDeltaUpdateInterval = 0.0166f;

	protected const float m_lowPassFilterFactor = 83f / 150f;

	private UIMoveOuter m_Move;

	private List<UIControl> m_Controls = new List<UIControl>();

	private IScrollBar m_ScrollBar;

	private Rect m_bounds;

	private Vector2 m_viewSize = default(Vector2);

	private List<float> m_posList = new List<float>();

	private int m_posIndex;

	private ListOrientation m_listOri;

	private Vector2 m_contentExtent;

	private Vector2 m_posOrigin = default(Vector2);

	private float m_itemSpacingH;

	private float m_itemSpacingV;

	private float m_scrollPosH;

	private float m_scrollPosV;

	private float m_scrollDeltaH;

	private float m_scrollDeltaV;

	private bool m_isScrolling;

	private bool m_noTouch = true;

	private float m_scrollInertiaH;

	private float m_scrollInertiaV;

	protected float m_scrollMaxH;

	protected float m_scrollMaxV;

	private float m_lastTime;

	private float m_timeDelta;

	public IScrollBar ScrollBar
	{
		get
		{
			return m_ScrollBar;
		}
		set
		{
			m_ScrollBar = value;
		}
	}

	public Rect Bounds
	{
		get
		{
			return m_bounds;
		}
		set
		{
			Rect bounds = AutoUIResolution.ToShiftToRight(value, 2);
			m_bounds = bounds;
		}
	}

	public Vector2 ViewSize
	{
		get
		{
			return m_viewSize;
		}
		set
		{
			m_viewSize = value;
		}
	}

	public int PageCount
	{
		get
		{
			return m_posList.Count;
		}
	}

	public int PageIndex
	{
		get
		{
			return m_posIndex;
		}
		set
		{
			m_posIndex = Mathf.Clamp(value, 0, m_posList.Count - 1);
			switch (m_listOri)
			{
			case ListOrientation.Horizontal:
				ScrollListTo_InternalH(m_posList[m_posIndex]);
				break;
			case ListOrientation.Vertical:
				ScrollListTo_InternalV(m_posList[m_posIndex]);
				break;
			}
			UpdateControlPos();
		}
	}

	public ListOrientation ListOri
	{
		get
		{
			return m_listOri;
		}
		set
		{
			m_listOri = value;
		}
	}

	public float ItemSpacingH
	{
		get
		{
			return m_itemSpacingH;
		}
		set
		{
			m_itemSpacingH = value;
		}
	}

	public float ItemSpacingV
	{
		get
		{
			return m_itemSpacingV;
		}
		set
		{
			m_itemSpacingV = value;
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
			for (int num = m_Controls.Count; num > 0; num--)
			{
				m_Controls[num - 1].Visible = value;
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
			for (int num = m_Controls.Count; num > 0; num--)
			{
				m_Controls[num - 1].Enable = value;
			}
		}
	}

	public override Rect Rect
	{
		get
		{
			return base.Rect;
		}
		set
		{
			Rect rect = Rect;
			int count = m_Controls.Count;
			for (int i = 0; i < count; i++)
			{
				UIControl uIControl = m_Controls[i];
				Rect rect2 = uIControl.Rect;
				float num = (rect2.x - rect.x) / rect.width;
				float num2 = (rect2.y - rect.y) / rect.height;
				rect2.x = value.x + m_Rect.width * num;
				rect2.y = value.y + m_Rect.height * num2;
				float num3 = value.width / rect.width;
				float num4 = value.height / rect.height;
				rect2.width *= num3;
				rect2.height *= num4;
				uIControl.Rect = rect2;
			}
			Rect rect3 = AutoUIResolution.ToShiftToRight(value, 2);
			base.Rect = rect3;
		}
	}

	public void SetMoveParam(Rect rcMove, float moveMinX, float moveMinY)
	{
		m_Move = new UIMoveOuter();
		m_Move.Rect = AutoUIResolution.ToShiftToRight(rcMove, 2);
		m_Move.MinX = moveMinX;
		m_Move.MinY = moveMinY;
		m_Move.SetParent(this);
	}

	public void Add(UIControl control)
	{
		control.SetParent(this);
		if (m_Controls == null)
		{
			m_Controls = new List<UIControl>();
		}
		m_Controls.Add(control);
		if (m_listOri == ListOrientation.Horizontal)
		{
			m_contentExtent.x += control.Rect.width + m_itemSpacingH;
			float b = control.Rect.height + m_itemSpacingV;
			m_contentExtent.y = Mathf.Max(m_contentExtent.y, b);
			float num = 0f;
			m_posList.Clear();
			for (; num < m_contentExtent.x; num += m_viewSize.x)
			{
				m_posList.Add(num / (m_contentExtent.x - m_bounds.width));
			}
			m_scrollMaxH = m_bounds.width / num * 0.5f;
		}
		else if (m_listOri == ListOrientation.Vertical)
		{
			m_contentExtent.y += control.Rect.height + m_itemSpacingV;
			float b2 = control.Rect.width + m_itemSpacingH;
			m_contentExtent.x = Mathf.Max(m_contentExtent.x, b2);
			float num2 = 0f;
			m_posList.Clear();
			for (; num2 < m_contentExtent.y; num2 += m_viewSize.y)
			{
				m_posList.Add(num2 / (m_contentExtent.y - m_bounds.height));
			}
			m_scrollMaxV = m_bounds.height / num2 * 0.5f;
		}
		UpdateControlPos();
		if (m_Clip)
		{
			control.SetClip(m_ClipRect);
		}
	}

	public void Remove(UIControl control)
	{
		m_Controls.Remove(control);
	}

	public override void SetClip(Rect clip_rect)
	{
		clip_rect = AutoUIResolution.ToShiftToRight(clip_rect, 2);
		base.SetClip(clip_rect);
		for (int i = 0; i < m_Controls.Count; i++)
		{
			UIControl uIControl = m_Controls[i];
			uIControl.SetClip(clip_rect);
		}
	}

	public override void Draw()
	{
		for (int i = 0; i < m_Controls.Count; i++)
		{
			UIControl uIControl = m_Controls[i];
			uIControl.Update();
			if (uIControl.Visible && Visible)
			{
				uIControl.Draw();
			}
		}
	}

	public override bool HandleInput(UITouchInner touch)
	{
		if (m_Move.HandleInput(touch))
		{
			for (int num = m_Controls.Count - 1; num >= 0; num--)
			{
				UIControl uIControl = m_Controls[num];
				if (uIControl.GetType().Equals(typeof(UIClickButton)))
				{
					if (uIControl.Enable && Enable)
					{
						((UIClickButton)uIControl).Reset();
					}
				}
				else if (uIControl.GetType().Equals(typeof(UISelectButton)))
				{
					if (uIControl.Enable && Enable)
					{
						((UISelectButton)uIControl).Reset();
					}
				}
				else if (uIControl.GetType().Equals(typeof(UIGroupControl)))
				{
					UIGroupControl uIGroupControl = (UIGroupControl)uIControl;
					for (int num2 = uIGroupControl.Controls.Count - 1; num2 >= 0; num2--)
					{
						UIControl uIControl2 = (UIControl)uIGroupControl.Controls[num2];
						if (uIControl2.GetType().Equals(typeof(UIClickButton)))
						{
							if (uIControl2.Enable && Enable)
							{
								((UIClickButton)uIControl2).Reset();
							}
						}
						else if (uIControl2.GetType().Equals(typeof(UISelectButton)) && uIControl2.Enable && Enable)
						{
							((UISelectButton)uIControl2).Reset();
						}
					}
				}
			}
			return true;
		}
		for (int num3 = m_Controls.Count - 1; num3 >= 0; num3--)
		{
			UIControl uIControl3 = m_Controls[num3];
			if (uIControl3.Enable && Enable && uIControl3.HandleInput(touch))
			{
				return true;
			}
		}
		return false;
	}

	public void DrawSprite(UISprite sprite)
	{
		m_Parent.DrawSprite(sprite);
	}

	public void SendEvent(UIControl control, int command, float wparam, float lparam)
	{
		if (control == m_Move)
		{
			switch (command)
			{
			case 1:
				ScrollDragged(new Vector2(wparam, lparam));
				break;
			case 2:
				PointerReleased();
				break;
			}
		}
		else
		{
			m_Parent.SendEvent(control, command, wparam, lparam);
		}
	}

	public override void Update()
	{
		base.Update();
		m_timeDelta = Time.realtimeSinceStartup - m_lastTime;
		m_lastTime = Time.realtimeSinceStartup;
		if (m_isScrolling && m_noTouch)
		{
			float num = m_posList[m_posIndex];
			if (m_listOri == ListOrientation.Horizontal)
			{
				m_scrollDeltaH -= m_scrollDeltaH * 0.4f * (m_timeDelta / 0.166f);
				if (m_scrollPosH < num)
				{
					m_scrollPosH -= (m_scrollPosH - num) * 1f * (m_timeDelta / 0.166f);
					m_scrollDeltaH *= Mathf.Clamp01(1f + (m_scrollPosH - num) / m_scrollMaxH);
				}
				else if (m_scrollPosH > num)
				{
					m_scrollPosH -= (m_scrollPosH - num) * 1f * (m_timeDelta / 0.166f);
					m_scrollDeltaH *= Mathf.Clamp01(1f - (m_scrollPosH - num) / m_scrollMaxH);
				}
				if (Mathf.Abs(m_scrollDeltaH) < 0.0001f)
				{
					m_scrollDeltaH = 0f;
					if (m_scrollPosH > num - 0.0001f && m_scrollPosH < num + 0.0001f)
					{
						m_scrollPosH = num;
					}
				}
				ScrollListTo_InternalH(m_scrollPosH + m_scrollDeltaH);
				if (m_scrollPosH >= num - 0.001f && m_scrollPosH <= num + 0.001f && m_scrollDeltaH == 0f)
				{
					m_isScrolling = false;
				}
			}
			else
			{
				m_scrollDeltaV -= m_scrollDeltaV * 0.4f * (m_timeDelta / 0.166f);
				if (m_scrollPosV < num)
				{
					m_scrollPosV -= (m_scrollPosV - num) * 1f * (m_timeDelta / 0.166f);
					m_scrollDeltaV *= Mathf.Clamp01(1f + (m_scrollPosV - num) / m_scrollMaxV);
				}
				else if (m_scrollPosV > num)
				{
					m_scrollPosV -= (m_scrollPosV - num) * 1f * (m_timeDelta / 0.166f);
					m_scrollDeltaV *= Mathf.Clamp01(1f - (m_scrollPosV - num) / m_scrollMaxV);
				}
				if (Mathf.Abs(m_scrollDeltaV) < 0.0001f)
				{
					m_scrollDeltaV = 0f;
					if (m_scrollPosV > num - 0.0001f && m_scrollPosV < num + 0.0001f)
					{
						m_scrollPosV = num;
					}
				}
				ScrollListTo_InternalV(m_scrollPosV + m_scrollDeltaV);
				if (m_scrollPosV >= num - 0.001f && m_scrollPosV <= num + 0.001f && m_scrollDeltaV == 0f)
				{
					m_isScrolling = false;
				}
			}
			UpdateControlPos();
		}
		else if (m_listOri == ListOrientation.Horizontal)
		{
			m_scrollInertiaH = Mathf.Lerp(m_scrollInertiaH, m_scrollDeltaH, 83f / 150f);
		}
		else
		{
			m_scrollInertiaV = Mathf.Lerp(m_scrollInertiaV, m_scrollDeltaV, 83f / 150f);
		}
		if (ScrollBar != null)
		{
			if (PageCount <= 1)
			{
				ScrollBar.SetScrollPercent(0f);
			}
			else if (PageCount > 1)
			{
				ScrollBar.SetScrollPercent(Mathf.Clamp01((float)PageIndex / (float)(PageCount - 1)));
			}
		}
	}

	private void PointerReleased()
	{
		m_noTouch = true;
		float num = 0f;
		num = ((m_listOri != 0) ? m_scrollPosV : m_scrollPosH);
		m_posIndex = 0;
		int count = m_posList.Count;
		if (num <= m_posList[0])
		{
			m_posIndex = 0;
		}
		else if (num >= m_posList[count - 1])
		{
			m_posIndex = count - 1;
		}
		else
		{
			for (int i = 0; i < count - 1; i++)
			{
				float num2 = m_posList[i];
				float num3 = m_posList[i + 1];
				if (num >= num2 && num < num3)
				{
					if (num >= (num2 + num3) * 0.5f)
					{
						m_posIndex = i + 1;
					}
					else
					{
						m_posIndex = i;
					}
					break;
				}
			}
		}
		if (m_listOri == ListOrientation.Horizontal && (m_scrollInertiaH > 0.001f || m_scrollInertiaH < -0.001f))
		{
			if (m_scrollInertiaH > 0f)
			{
				m_posIndex = Mathf.Min(m_posIndex + 1, m_posList.Count - 1);
			}
			else if (m_scrollInertiaH < 0f)
			{
				m_posIndex = Mathf.Max(m_posIndex - 1, 0);
			}
			m_scrollDeltaH = m_scrollInertiaH;
			m_scrollInertiaH = 0f;
		}
		else if (m_listOri == ListOrientation.Vertical && (m_scrollInertiaV > 0.001f || m_scrollInertiaV < -0.001f))
		{
			if (m_scrollInertiaV < 0f)
			{
				m_posIndex = Mathf.Min(m_posIndex + 1, m_posList.Count - 1);
			}
			else if (m_scrollInertiaV > 0f)
			{
				m_posIndex = Mathf.Max(m_posIndex - 1, 0);
			}
			m_scrollDeltaV = m_scrollInertiaV;
			m_scrollInertiaV = 0f;
		}
	}

	private void ScrollDragged(Vector2 deltaPos)
	{
		if (m_listOri == ListOrientation.Horizontal)
		{
			m_scrollDeltaH = (0f - deltaPos.x) / (m_contentExtent.x - m_bounds.width);
			float num = m_scrollPosH + m_scrollDeltaH;
			float num2 = m_posList[m_posList.Count - 1];
			if (num > num2)
			{
				m_scrollDeltaH *= Mathf.Clamp01(1f - (num - num2) / m_scrollMaxH);
			}
			else if (num < 0f)
			{
				m_scrollDeltaH *= Mathf.Clamp01(1f + num / m_scrollMaxH);
			}
			ScrollListTo_InternalH(m_scrollPosH + m_scrollDeltaH);
		}
		else
		{
			m_scrollDeltaV = deltaPos.y / (m_contentExtent.y - m_bounds.height);
			float num3 = m_scrollPosV + m_scrollDeltaV;
			float num4 = m_posList[m_posList.Count - 1];
			if (num3 > num4)
			{
				m_scrollDeltaV *= Mathf.Clamp01(1f - (num3 - num4) / m_scrollMaxV);
			}
			else if (num3 < 0f)
			{
				m_scrollDeltaV *= Mathf.Clamp01(1f + num3 / m_scrollMaxV);
			}
			ScrollListTo_InternalV(m_scrollPosV + m_scrollDeltaV);
		}
		UpdateControlPos();
		m_noTouch = false;
		m_isScrolling = true;
	}

	private void ScrollListTo_InternalH(float pos)
	{
		if (!float.IsNaN(pos))
		{
			float num = m_contentExtent.x - m_bounds.width;
			m_posOrigin.x = Mathf.Clamp(num, 0f, num) * (0f - pos);
			m_scrollPosH = pos;
		}
	}

	private void ScrollListTo_InternalV(float pos)
	{
		if (!float.IsNaN(pos))
		{
			float num = m_contentExtent.y - m_bounds.height;
			m_posOrigin.y = Mathf.Clamp(num, 0f, num) * (0f - pos);
			m_scrollPosV = pos;
		}
	}

	public void ScrollListToH(float pos)
	{
		m_scrollInertiaH = 0f;
		m_scrollDeltaH = 0f;
		ScrollListTo_InternalH(pos);
		UpdateControlPos();
	}

	public void ScrollListToV(float pos)
	{
		m_scrollInertiaV = 0f;
		m_scrollDeltaV = 0f;
		ScrollListTo_InternalV(pos);
		UpdateControlPos();
	}

	private void UpdateControlPos()
	{
		int count = m_Controls.Count;
		for (int i = 0; i < count; i++)
		{
			UIControl uIControl = m_Controls[i];
			Rect rect = uIControl.Rect;
			if (m_listOri == ListOrientation.Horizontal)
			{
				rect.x = m_bounds.x + m_posOrigin.x + (float)i * (rect.width + m_itemSpacingH);
				rect.y = m_bounds.yMax - rect.height - m_posOrigin.y;
			}
			else if (m_listOri == ListOrientation.Vertical)
			{
				rect.x = m_bounds.x + m_posOrigin.x;
				rect.y = m_bounds.yMax - m_posOrigin.y - (float)i * (rect.height + m_itemSpacingV) - rect.height;
			}
			if (rect.xMax > m_ClipRect.xMin && rect.xMin < m_ClipRect.xMax && rect.yMax > m_ClipRect.yMin && rect.yMin < m_ClipRect.yMax)
			{
				uIControl.Rect = rect;
				uIControl.Visible = true;
				uIControl.Enable = true;
			}
			else
			{
				uIControl.Visible = false;
				uIControl.Enable = false;
			}
		}
	}
}
