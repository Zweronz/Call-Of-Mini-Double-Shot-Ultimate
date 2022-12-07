using System.Collections.Generic;
using UnityEngine;

public class UIEffect01
{
	public class Effect01DataItem
	{
		public float time;

		public Vector2 position;
	}

	private List<Effect01DataItem> m_Datas;

	private int m_DataIndex;

	private UIManager m_UIManager;

	protected UIGroupControl m_Group;

	private float m_CurMoveStartTime;

	public UIGroupControl Group
	{
		get
		{
			return m_Group;
		}
		set
		{
			m_Group = value;
			m_UIManager.Add(m_Group);
		}
	}

	public UIEffect01(UIManager ui_manager)
	{
		m_UIManager = ui_manager;
		m_Datas = new List<Effect01DataItem>();
		m_CurMoveStartTime = Time.time;
	}

	public void AddData(Effect01DataItem data)
	{
		data.position = AutoUIResolution.ToShiftToRight(AutoUI.AutoSize(data.position), 2);
		m_Datas.Add(data);
	}

	public bool EffectOver()
	{
		if (m_DataIndex + 1 >= m_Datas.Count)
		{
			return true;
		}
		return false;
	}

	public void Clear()
	{
		if (m_Group != null)
		{
			m_UIManager.Remove(m_Group);
		}
	}

	public void Update(float deltaTime)
	{
		if (m_Group == null || m_DataIndex + 1 >= m_Datas.Count)
		{
			return;
		}
		Vector2 vector = new Vector2(m_Group.Rect.x, m_Group.Rect.y);
		Effect01DataItem effect01DataItem = m_Datas[m_DataIndex];
		Effect01DataItem effect01DataItem2 = m_Datas[m_DataIndex + 1];
		if (Time.time - m_CurMoveStartTime >= effect01DataItem2.time)
		{
			m_DataIndex++;
			if (m_DataIndex + 1 >= m_Datas.Count)
			{
				m_UIManager.Remove(m_Group);
				m_Group = null;
			}
			else
			{
				Effect01DataItem effect01DataItem3 = m_Datas[m_DataIndex + 1];
				m_CurMoveStartTime = Time.time;
			}
		}
		else
		{
			Vector2 vector2 = effect01DataItem2.position - effect01DataItem.position;
			Vector2 vector3 = effect01DataItem.position + vector2 * (Time.time - m_CurMoveStartTime) / effect01DataItem2.time;
			m_Group.Rect = new Rect(vector3.x, vector3.y, m_Group.Rect.width, m_Group.Rect.height);
		}
	}
}
