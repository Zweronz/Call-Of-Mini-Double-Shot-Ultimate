using System.Collections;
using UnityEngine;

public class UIViewMoveable : UIControl, UIContainer
{
	public enum ViewMoveDirection
	{
		Horizontal = 0,
		Vertical = 1,
		Horizontal_Vertical = 2
	}

	private UIMoveOuter m_Move;

	public ViewMoveDirection m_MoveDirection = ViewMoveDirection.Horizontal_Vertical;

	private ArrayList m_Controls;

	public Rect m_MoveBound = new Rect(0f, 0f, 960f, 640f);

	protected int m_FingerId;

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
				((UIControl)m_Controls[num - 1]).Visible = value;
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
				((UIControl)m_Controls[num - 1]).Enable = value;
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
			base.Rect = value;
		}
	}

	public ArrayList Controls
	{
		get
		{
			return m_Controls;
		}
	}

	public UIViewMoveable(Rect rcMove, float moveMinX, float moveMinY)
	{
		m_Controls = new ArrayList();
		m_Move = new UIMoveOuter();
		m_Move.Rect = rcMove;
		m_Move.MinX = moveMinX;
		m_Move.MinY = moveMinY;
		m_Move.SetParent(this);
		m_FingerId = -1;
	}

	~UIViewMoveable()
	{
		Clear();
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

	public void Remove(UIControl control)
	{
		m_Controls.Remove(control);
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

	public void MovePosition(Vector2 delta_pos)
	{
		if (Rect.width < m_MoveBound.width || Rect.height < m_MoveBound.height)
		{
			Debug.LogError("UIViewMoveable : Rect must Larger than MoveBound !!!");
		}
		Vector2 vector = delta_pos;
		if (Rect.xMin + vector.x > m_MoveBound.xMin)
		{
			vector.x = m_MoveBound.xMin - Rect.xMin;
		}
		if (Rect.yMin + vector.y > m_MoveBound.yMin)
		{
			vector.y = m_MoveBound.yMin - Rect.yMin;
		}
		if (Rect.xMax + vector.x < m_MoveBound.xMax)
		{
			vector.x = m_MoveBound.xMax - Rect.xMax;
		}
		if (Rect.yMax + vector.y < m_MoveBound.yMax)
		{
			vector.y = m_MoveBound.yMax - Rect.yMax;
		}
		Rect = new Rect(Rect.x + vector.x, Rect.y + vector.y, Rect.width, Rect.height);
		for (int i = 0; i < m_Controls.Count; i++)
		{
			float left = ((UIControl)m_Controls[i]).Rect.x + vector.x;
			float top = ((UIControl)m_Controls[i]).Rect.y + vector.y;
			((UIControl)m_Controls[i]).Rect = new Rect(left, top, ((UIControl)m_Controls[i]).Rect.width, ((UIControl)m_Controls[i]).Rect.height);
		}
	}

	public virtual void HandleMoveBegin()
	{
	}

	public virtual void HandleMoveEnd()
	{
	}

	public void Clear()
	{
		m_Controls.Clear();
	}

	public override void SetClip(Rect clip_rect)
	{
		base.SetClip(clip_rect);
		for (int i = 0; i < m_Controls.Count; i++)
		{
			UIControl uIControl = (UIControl)m_Controls[i];
			uIControl.SetClip(clip_rect);
		}
	}

	public override void Draw()
	{
		for (int i = 0; i < m_Controls.Count; i++)
		{
			UIControl uIControl = (UIControl)m_Controls[i];
			uIControl.Update();
			if (uIControl.Visible && Visible)
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
		if (control == m_Move)
		{
			switch (command)
			{
			case 0:
				HandleMoveBegin();
				break;
			case 1:
			{
				Vector2 zero = Vector2.zero;
				if (m_MoveDirection == ViewMoveDirection.Horizontal_Vertical)
				{
					zero.x = wparam;
					zero.y = lparam;
				}
				else if (m_MoveDirection == ViewMoveDirection.Horizontal)
				{
					zero.x = wparam;
					zero.y = 0f;
				}
				else if (m_MoveDirection == ViewMoveDirection.Vertical)
				{
					zero.x = 0f;
					zero.y = lparam;
				}
				MovePosition(zero);
				break;
			}
			case 2:
				HandleMoveEnd();
				break;
			}
		}
		else if (!m_Move.IsMoving())
		{
			m_Parent.SendEvent(control, command, wparam, lparam);
		}
	}

	public override bool HandleInput(UITouchInner touch)
	{
		if (touch.phase == TouchPhase.Began)
		{
			if (m_Move.Rect.Contains(touch.position))
			{
				m_FingerId = touch.fingerId;
				if (m_Move.HandleInput(touch))
				{
					return true;
				}
				for (int num = m_Controls.Count - 1; num >= 0; num--)
				{
					UIControl uIControl = (UIControl)m_Controls[num];
					if (uIControl.Enable && Enable && uIControl.HandleInput(touch))
					{
						return true;
					}
				}
				return true;
			}
			return false;
		}
		if (touch.fingerId == m_FingerId)
		{
			if (touch.phase == TouchPhase.Moved)
			{
				if (m_Move.Rect.Contains(touch.position))
				{
				}
				if (m_Move.HandleInput(touch))
				{
					for (int num2 = m_Controls.Count - 1; num2 >= 0; num2--)
					{
						UIControl uIControl2 = (UIControl)m_Controls[num2];
						if (uIControl2.GetType().Equals(typeof(UIClickButton)) && uIControl2.Enable && Enable)
						{
							((UIClickButton)uIControl2).Reset();
						}
					}
					return true;
				}
				for (int num3 = m_Controls.Count - 1; num3 >= 0; num3--)
				{
					UIControl uIControl3 = (UIControl)m_Controls[num3];
					if (uIControl3.Enable && Enable && uIControl3.HandleInput(touch))
					{
						return true;
					}
				}
			}
			else if (touch.phase == TouchPhase.Ended)
			{
				m_FingerId = -1;
				if (m_Move.HandleInput(touch))
				{
				}
				for (int num4 = m_Controls.Count - 1; num4 >= 0; num4--)
				{
					UIControl uIControl4 = (UIControl)m_Controls[num4];
					if (uIControl4.Enable && Enable && uIControl4.HandleInput(touch))
					{
						return true;
					}
				}
			}
			return true;
		}
		return false;
	}
}
