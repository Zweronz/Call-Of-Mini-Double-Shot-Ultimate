using System.Collections;
using UnityEngine;

public class UIElementGroup : UIControl, UIContainer
{
	private Rect m_DefRect;

	private ArrayList m_Controls;

	private ArrayList m_ControlsDefRect;

	public override Rect Rect
	{
		get
		{
			return base.Rect;
		}
		set
		{
			base.Rect = value;
			float num = value.width / m_DefRect.width;
			for (int i = 0; i < m_ControlsDefRect.Count; i++)
			{
				float width = ((Rect)m_ControlsDefRect[i]).width * num;
				float height = ((Rect)m_ControlsDefRect[i]).height * num;
				float left = value.x + (((Rect)m_ControlsDefRect[i]).x - m_DefRect.x) * num;
				float top = value.y + (((Rect)m_ControlsDefRect[i]).y - m_DefRect.y) * num;
				((UIControl)m_Controls[i]).Rect = new Rect(left, top, width, height);
			}
		}
	}

	public UIElementGroup(Rect defaultRc)
	{
		m_Controls = new ArrayList();
		m_ControlsDefRect = new ArrayList();
		SetDefaultRect(defaultRc);
	}

	public void SetDefaultRect(Rect rc)
	{
		m_DefRect = rc;
	}

	public void Add(UIControl control)
	{
		m_Controls.Add(control);
		m_ControlsDefRect.Add(control.Rect);
	}

	public void ClearElements()
	{
		m_Controls.Clear();
		m_ControlsDefRect.Clear();
	}

	public override void Draw()
	{
		for (int i = 0; i < m_Controls.Count; i++)
		{
			((UIControl)m_Controls[i]).Draw();
		}
	}

	public void DrawSprite(UISprite sprite)
	{
		m_Parent.DrawSprite(sprite);
	}

	public void SendEvent(UIControl control, int command, float wparam, float lparam)
	{
		m_Parent.SendEvent(control, command, wparam, lparam);
	}
}
