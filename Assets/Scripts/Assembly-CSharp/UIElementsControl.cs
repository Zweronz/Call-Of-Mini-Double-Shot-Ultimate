using System.Collections;
using UnityEngine;

public class UIElementsControl : UIControl, UIContainer
{
	public enum Command
	{
		Begin = 0,
		Move = 1,
		End = 2,
		Select = 3
	}

	private ArrayList m_ElementsRectList = new ArrayList();

	private UIControl[] m_Controls = new UIControl[7];

	private int[] m_DrawOrder = new int[7];

	private bool m_AutoMove;

	private int m_AutoMoveDir;

	private int m_AutoMoveCounter;

	private int m_AutoMoveCounterMax = 100;

	private int m_orderNail;

	private UIControl[] m_BuyControlGroup = new UIControl[4];

	protected int m_FingerId;

	protected Vector2 m_TouchPosition;

	protected float m_MinX;

	protected float m_MinY;

	public float MinX
	{
		get
		{
			return m_MinX;
		}
		set
		{
			m_MinX = value;
		}
	}

	public float MinY
	{
		get
		{
			return m_MinY;
		}
		set
		{
			m_MinY = value;
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

	public UIElementsControl(UIManager uimanager)
	{
		SetParent(uimanager);
		m_FingerId = -1;
		m_TouchPosition = new Vector2(0f, 0f);
		m_DrawOrder[0] = 0;
		m_DrawOrder[1] = 1;
		m_DrawOrder[2] = 2;
		m_DrawOrder[3] = 3;
		m_DrawOrder[4] = 4;
		m_DrawOrder[5] = 5;
		m_DrawOrder[6] = 6;
	}

	public void Clear()
	{
		for (int i = 0; i < m_Controls.Length; i++)
		{
			((UIElementGroup)m_Controls[i]).ClearElements();
		}
	}

	public void init(int page)
	{
		if (m_ElementsRectList.Count <= 0)
		{
			m_ElementsRectList.Add(new Rect(Rect.x + 17f, Rect.y + 77f, 20f, 20f));
			m_ElementsRectList.Add(new Rect(Rect.x + 38f, Rect.y + 44f, 94f, 94f));
			m_ElementsRectList.Add(new Rect(Rect.x + 86f, Rect.y + 26f, 124f, 124f));
			m_ElementsRectList.Add(new Rect(Rect.x + 155f, Rect.y, 170f, 170f));
			m_ElementsRectList.Add(new Rect(Rect.x + 269f, Rect.y + 26f, 124f, 124f));
			m_ElementsRectList.Add(new Rect(Rect.x + 348f, Rect.y + 44f, 94f, 94f));
			m_ElementsRectList.Add(new Rect(Rect.x + 443f, Rect.y + 77f, 20f, 20f));
		}
	}

	public override void Draw()
	{
		if (m_AutoMove)
		{
			bool flag = false;
			for (int i = 0; i < 7; i++)
			{
				Rect rect = new Rect(m_Controls[i].Rect);
				if (m_AutoMoveDir < 0)
				{
					int index = m_DrawOrder[i];
					int num = m_DrawOrder[i] - 1;
					if (num < 0)
					{
						num = 6;
					}
					if (m_AutoMoveCounter < m_AutoMoveCounterMax)
					{
						rect.x = ((Rect)m_ElementsRectList[index]).x + (float)(Mathf.Abs(m_AutoMoveCounter) / m_AutoMoveCounterMax) * (((Rect)m_ElementsRectList[num]).x - ((Rect)m_ElementsRectList[index]).x);
						rect.y = ((Rect)m_ElementsRectList[index]).y + (float)(Mathf.Abs(m_AutoMoveCounter) / m_AutoMoveCounterMax) * (((Rect)m_ElementsRectList[num]).y - ((Rect)m_ElementsRectList[index]).y);
						rect.width = ((Rect)m_ElementsRectList[index]).width + (float)(Mathf.Abs(m_AutoMoveCounter) / m_AutoMoveCounterMax) * (((Rect)m_ElementsRectList[num]).width - ((Rect)m_ElementsRectList[index]).width);
						rect.height = ((Rect)m_ElementsRectList[index]).height + (float)(Mathf.Abs(m_AutoMoveCounter) / m_AutoMoveCounterMax) * (((Rect)m_ElementsRectList[num]).height - ((Rect)m_ElementsRectList[index]).height);
						if (rect.x <= ((Rect)m_ElementsRectList[num]).x)
						{
							rect.x = ((Rect)m_ElementsRectList[num]).x;
						}
						if (rect.x > m_Controls[i].Rect.x)
						{
							rect = new Rect(m_Controls[i].Rect);
						}
					}
				}
				else if (m_AutoMoveDir == 0)
				{
					int index2 = m_DrawOrder[i];
					int index3 = m_DrawOrder[i];
					if (m_AutoMoveCounter < m_AutoMoveCounterMax)
					{
						rect.x = ((Rect)m_ElementsRectList[index2]).x + (float)(Mathf.Abs(m_AutoMoveCounter) / m_AutoMoveCounterMax) * (((Rect)m_ElementsRectList[index3]).x - ((Rect)m_ElementsRectList[index2]).x);
						rect.y = ((Rect)m_ElementsRectList[index2]).y + (float)(Mathf.Abs(m_AutoMoveCounter) / m_AutoMoveCounterMax) * (((Rect)m_ElementsRectList[index3]).y - ((Rect)m_ElementsRectList[index2]).y);
						rect.width = ((Rect)m_ElementsRectList[index2]).width + (float)(Mathf.Abs(m_AutoMoveCounter) / m_AutoMoveCounterMax) * (((Rect)m_ElementsRectList[index3]).width - ((Rect)m_ElementsRectList[index2]).width);
						rect.height = ((Rect)m_ElementsRectList[index2]).height + (float)(Mathf.Abs(m_AutoMoveCounter) / m_AutoMoveCounterMax) * (((Rect)m_ElementsRectList[index3]).height - ((Rect)m_ElementsRectList[index2]).height);
						if (rect.x >= ((Rect)m_ElementsRectList[index3]).x)
						{
							rect.x = ((Rect)m_ElementsRectList[index3]).x;
						}
					}
				}
				else if (m_AutoMoveDir > 0)
				{
					int index4 = m_DrawOrder[i];
					int num2 = m_DrawOrder[i] + 1;
					if (num2 > 6)
					{
						num2 = 0;
					}
					if (m_AutoMoveCounter < m_AutoMoveCounterMax)
					{
						rect.x = ((Rect)m_ElementsRectList[index4]).x + (float)(Mathf.Abs(m_AutoMoveCounter) / m_AutoMoveCounterMax) * (((Rect)m_ElementsRectList[num2]).x - ((Rect)m_ElementsRectList[index4]).x);
						rect.y = ((Rect)m_ElementsRectList[index4]).y + (float)(Mathf.Abs(m_AutoMoveCounter) / m_AutoMoveCounterMax) * (((Rect)m_ElementsRectList[num2]).y - ((Rect)m_ElementsRectList[index4]).y);
						rect.width = ((Rect)m_ElementsRectList[index4]).width + (float)(Mathf.Abs(m_AutoMoveCounter) / m_AutoMoveCounterMax) * (((Rect)m_ElementsRectList[num2]).width - ((Rect)m_ElementsRectList[index4]).width);
						rect.height = ((Rect)m_ElementsRectList[index4]).height + (float)(Mathf.Abs(m_AutoMoveCounter) / m_AutoMoveCounterMax) * (((Rect)m_ElementsRectList[num2]).height - ((Rect)m_ElementsRectList[index4]).height);
						if (rect.x >= ((Rect)m_ElementsRectList[num2]).x)
						{
							rect.x = ((Rect)m_ElementsRectList[num2]).x;
						}
						if (rect.x < m_Controls[i].Rect.x)
						{
							rect = new Rect(m_Controls[i].Rect);
						}
					}
				}
				m_Controls[i].Rect = rect;
			}
			if (flag)
			{
				m_AutoMove = false;
			}
			if (++m_AutoMoveCounter >= m_AutoMoveCounterMax)
			{
				m_AutoMoveCounter = 0;
				m_AutoMove = false;
			}
		}
		int[] array = new int[7] { 0, 6, 1, 5, 2, 4, 3 };
		for (int j = 0; j < array.Length; j++)
		{
			for (int k = 0; k < m_DrawOrder.Length; k++)
			{
				if (m_DrawOrder[k] == array[j])
				{
					m_Controls[k].Draw();
				}
			}
		}
		for (int l = 0; l < m_BuyControlGroup.Length; l++)
		{
			m_BuyControlGroup[l].Draw();
		}
	}

	public override bool HandleInput(UITouchInner touch)
	{
		if (touch.phase == TouchPhase.Began)
		{
			if (PtInRect(touch.position))
			{
				m_FingerId = touch.fingerId;
				m_TouchPosition = touch.position;
				m_AutoMove = false;
				return true;
			}
			return false;
		}
		if (touch.fingerId != m_FingerId)
		{
			return false;
		}
		if (!PtInRect(touch.position))
		{
			return false;
		}
		if (touch.phase == TouchPhase.Moved)
		{
			float num = touch.position.x - m_TouchPosition.x;
			float num2 = touch.position.y - m_TouchPosition.y;
			for (int i = 0; i < 7; i++)
			{
				Rect rect = new Rect(m_Controls[i].Rect);
				if (num < 0f)
				{
					int index = m_DrawOrder[i];
					int num3 = m_DrawOrder[i] - 1;
					if (num3 < 0)
					{
						num3 = 6;
					}
					if (Mathf.Abs(num) <= 100f)
					{
						rect.x = ((Rect)m_ElementsRectList[index]).x + Mathf.Abs(num) / 100f * (((Rect)m_ElementsRectList[num3]).x - ((Rect)m_ElementsRectList[index]).x);
						rect.y = ((Rect)m_ElementsRectList[index]).y + Mathf.Abs(num) / 100f * (((Rect)m_ElementsRectList[num3]).y - ((Rect)m_ElementsRectList[index]).y);
						rect.width = ((Rect)m_ElementsRectList[index]).width + Mathf.Abs(num) / 100f * (((Rect)m_ElementsRectList[num3]).width - ((Rect)m_ElementsRectList[index]).width);
						rect.height = ((Rect)m_ElementsRectList[index]).height + Mathf.Abs(num) / 100f * (((Rect)m_ElementsRectList[num3]).height - ((Rect)m_ElementsRectList[index]).height);
						if (rect.x <= ((Rect)m_ElementsRectList[num3]).x)
						{
							rect.x = ((Rect)m_ElementsRectList[num3]).x;
						}
					}
				}
				else
				{
					int index2 = m_DrawOrder[i];
					int num4 = m_DrawOrder[i] + 1;
					if (num4 > 6)
					{
						num4 = 0;
					}
					if (Mathf.Abs(num) <= 100f)
					{
						rect.x = ((Rect)m_ElementsRectList[index2]).x + Mathf.Abs(num) / 100f * (((Rect)m_ElementsRectList[num4]).x - ((Rect)m_ElementsRectList[index2]).x);
						rect.y = ((Rect)m_ElementsRectList[index2]).y + Mathf.Abs(num) / 100f * (((Rect)m_ElementsRectList[num4]).y - ((Rect)m_ElementsRectList[index2]).y);
						rect.width = ((Rect)m_ElementsRectList[index2]).width + Mathf.Abs(num) / 100f * (((Rect)m_ElementsRectList[num4]).width - ((Rect)m_ElementsRectList[index2]).width);
						rect.height = ((Rect)m_ElementsRectList[index2]).height + Mathf.Abs(num) / 100f * (((Rect)m_ElementsRectList[num4]).height - ((Rect)m_ElementsRectList[index2]).height);
						if (rect.x >= ((Rect)m_ElementsRectList[num4]).x)
						{
							rect.x = ((Rect)m_ElementsRectList[num4]).x;
						}
					}
				}
				m_Controls[i].Rect = rect;
			}
			return true;
		}
		if (touch.phase == TouchPhase.Ended)
		{
			m_AutoMoveCounter = 0;
			float num5 = touch.position.x - m_TouchPosition.x;
			if (Mathf.Abs(num5) > 10f)
			{
				m_AutoMove = true;
				if (num5 < 0f)
				{
					m_AutoMoveDir = -1;
					m_orderNail++;
					for (int j = 0; j < 7; j++)
					{
						int num6 = m_DrawOrder[j] - 1;
						if (num6 < 0)
						{
							num6 = 6;
							int num7 = m_orderNail + 6;
						}
						m_DrawOrder[j] = num6;
					}
				}
				else
				{
					m_AutoMoveDir = 1;
					m_orderNail--;
					for (int k = 0; k < 7; k++)
					{
						int num8 = m_DrawOrder[k] + 1;
						if (num8 > 6)
						{
							num8 = 0;
							int orderNail = m_orderNail;
						}
						m_DrawOrder[k] = num8;
					}
				}
			}
			else
			{
				m_AutoMove = true;
				m_AutoMoveDir = 0;
				if (Mathf.Abs(num5) < 5f && m_TouchPosition.x > Rect.x + 155f && m_TouchPosition.x < Rect.x + 155f + 170f && m_TouchPosition.y > Rect.y && m_TouchPosition.y < Rect.y + 170f)
				{
					m_Parent.SendEvent(this, 3, 0f, 0f);
				}
			}
			m_FingerId = -1;
			m_TouchPosition = new Vector2(0f, 0f);
			if (m_AutoMove)
			{
				return true;
			}
			return false;
		}
		return false;
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
