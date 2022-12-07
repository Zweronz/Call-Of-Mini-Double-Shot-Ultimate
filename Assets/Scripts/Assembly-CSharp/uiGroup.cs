using System.Collections;
using UnityEngine;

public class uiGroup
{
	private UIManager m_UIManager;

	private ArrayList m_Controls;

	private bool m_Visible;

	private bool m_Enable;

	private Vector2 m_Position;

	public bool Visible
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
				Debug.Log("YooKendo_uiGroup::Visible " + ((UIControl)m_Controls[num - 1]).Id + " " + value);
				((UIControl)m_Controls[num - 1]).Visible = value;
			}
		}
	}

	public bool Enable
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
				Debug.Log("YooKendo_uiGroup::Enable " + ((UIControl)m_Controls[num - 1]).Id + " " + value);
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

	public uiGroup(UIManager ui_manager)
	{
		m_Controls = new ArrayList();
		m_UIManager = ui_manager;
		m_Visible = true;
		m_Enable = true;
		m_Position = new Vector2(0f, 0f);
	}

	~uiGroup()
	{
		Clear();
	}

	public void SetUIManager(UIManager ui_manager)
	{
		m_UIManager = ui_manager;
	}

	public void SetClip(Rect rc_clip)
	{
		for (int num = m_Controls.Count; num > 0; num--)
		{
			((UIControl)m_Controls[num - 1]).SetClip(rc_clip);
		}
	}

	public void Add(UIControl control)
	{
		m_UIManager.Add(control);
		m_Controls.Add(control);
	}

	public void Remove(UIControl control)
	{
		m_Controls.Remove(control);
		m_UIManager.Remove(control);
	}

	public void Remove(int controlId)
	{
		for (int num = m_Controls.Count; num > 0; num--)
		{
			if (((UIControl)m_Controls[num - 1]).Id == controlId)
			{
				m_UIManager.Remove((UIControl)m_Controls[num - 1]);
				m_Controls.Remove((UIControl)m_Controls[num - 1]);
				break;
			}
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
		for (int i = 0; i < m_Controls.Count; i++)
		{
			m_UIManager.Remove((UIControl)m_Controls[i]);
		}
		m_Controls.Clear();
	}
}
