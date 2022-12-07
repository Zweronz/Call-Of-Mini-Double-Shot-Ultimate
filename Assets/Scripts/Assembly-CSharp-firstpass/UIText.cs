using System.Collections;
using UnityEngine;

public class UIText : UIControlVisible
{
	public enum enAlignStyle
	{
		left = 0,
		center = 1,
		right = 2
	}

	private string m_Text;

	private Font m_Font;

	private float m_LineSpacing = 1f;

	private float m_CharacterSpacing = 1f;

	private Color m_Color = Color.black;

	private bool m_bIsAutoLine = true;

	private enAlignStyle m_AlignStyle;

	public Font Font
	{
		get
		{
			return m_Font;
		}
		set
		{
			m_Font = value;
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
			UpdateText();
		}
	}

	public float CharacterSpacing
	{
		get
		{
			return m_CharacterSpacing;
		}
		set
		{
			m_CharacterSpacing = value;
		}
	}

	public float LineSpacing
	{
		get
		{
			return m_LineSpacing;
		}
		set
		{
			m_LineSpacing = value;
		}
	}

	public enAlignStyle AlignStyle
	{
		get
		{
			return m_AlignStyle;
		}
		set
		{
			m_AlignStyle = value;
		}
	}

	public bool AutoLine
	{
		get
		{
			return m_bIsAutoLine;
		}
		set
		{
			m_bIsAutoLine = value;
		}
	}

	~UIText()
	{
	}

	public override void SetClip(Rect clip_rect)
	{
		base.SetClip(clip_rect);
		UpdateText();
	}

	public void Set(string font, string text, Color color)
	{
		if (AutoUI.IsRetain == AutoUI.RESOLUTION.LOWDEFINITION)
		{
			switch (font)
			{
			case "Zombie3D/Font/037-CAI978-10":
				font = "Zombie3D/Font/037-CAI978-7";
				break;
			case "Zombie3D/Font/037-CAI978-13":
				font = "Zombie3D/Font/037-CAI978-7";
				break;
			case "Zombie3D/Font/037-CAI978-15":
				font = "Zombie3D/Font/037-CAI978-7";
				break;
			case "Zombie3D/Font/037-CAI978-18":
				font = "Zombie3D/Font/037-CAI978-9";
				break;
			case "Zombie3D/Font/037-CAI978-22":
				font = "Zombie3D/Font/037-CAI978-11";
				break;
			case "Zombie3D/Font/037-CAI978-27":
				font = "Zombie3D/Font/037-CAI978-13_1";
				break;
			case "Zombie3D/Font/037-CAI978-spec1":
				font = "Zombie3D/Font/037-CAI978-spec1_LOW";
				break;
			}
		}
		m_Font = mgrFont.Instance().getFont(font);
		m_Color = color;
		m_Text = text;
		UpdateText();
	}

	public void SetColor(Color clr)
	{
		m_Color = clr;
		UpdateText();
	}

	public void SetFont(string name)
	{
		m_Font = mgrFont.Instance().getFont(name);
		UpdateText();
	}

	public void SetText(string text)
	{
		m_Text = text;
		UpdateText();
	}

	public string GetText()
	{
		return m_Text;
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

	private void UpdateText()
	{
		m_Sprite = null;
		if (m_Font == null || m_Text == null || m_Text.Length <= 0)
		{
			return;
		}
		ArrayList arrayList = new ArrayList();
		ArrayList arrayList2 = new ArrayList();
		string[] array = m_Text.Split('\n');
		if (m_bIsAutoLine)
		{
			for (int i = 0; i < array.Length; i++)
			{
				ArrayList arrayList3 = new ArrayList();
				string[] array2 = array[i].Split(' ');
				string text = string.Empty;
				float num = 0f;
				for (int j = 0; j < array2.Length; j++)
				{
					float textWidth = m_Font.GetTextWidth(array2[j], CharacterSpacing);
					if (num + textWidth <= Rect.width)
					{
						text += array2[j];
						num += textWidth;
					}
					else
					{
						text.Trim();
						if (string.Empty != text)
						{
							arrayList3.Add(text);
						}
						text = array2[j];
						num = textWidth;
					}
					text += " ";
					num += CharacterSpacing;
					num += m_Font.GetTextWidth(" ");
				}
				text.Trim();
				if (string.Empty != text)
				{
					arrayList3.Add(text);
				}
				for (int k = 0; k < arrayList3.Count; k++)
				{
					arrayList2.Add(arrayList3[k]);
				}
			}
		}
		else
		{
			for (int l = 0; l < array.Length; l++)
			{
				arrayList2.Add(array[l]);
			}
		}
		float num2 = (float)m_Font.CellHeight + LineSpacing;
		int num3 = m_Font.TextureWidth / m_Font.CellWidth;
		for (int m = 0; m < arrayList2.Count; m++)
		{
			float num4 = 0f;
			for (int n = 0; n < ((string)arrayList2[m]).Length; n++)
			{
				char c = ((string)arrayList2[m])[n];
				float num5 = m_Font.getCharWidth(c);
				int num6 = c - 32;
				int num7 = num6 % num3;
				int num8 = num6 / num3;
				float left = num7 * m_Font.CellWidth;
				float top = num8 * m_Font.CellHeight;
				UISprite uISprite = new UISprite();
				uISprite.Position = new Vector2(m_Rect.x + num4 + (float)(m_Font.CellWidth / 2), m_Rect.y + m_Rect.height - (float)(m + 1) * num2 + (float)(m_Font.CellHeight / 2));
				uISprite.Size = new Vector2(m_Font.CellWidth, m_Font.CellHeight);
				uISprite.Material = m_Font.getTexture();
				uISprite.TextureRect = new Rect(left, top, m_Font.CellWidth, m_Font.CellHeight);
				uISprite.Color = m_Color;
				if (m_Clip)
				{
					uISprite.SetClip(m_ClipRect);
				}
				arrayList.Add(uISprite);
				num4 += num5 + CharacterSpacing;
			}
		}
		if (AlignStyle == enAlignStyle.center)
		{
			int num9 = 0;
			for (int num10 = 0; num10 < arrayList2.Count; num10++)
			{
				string text2 = (string)arrayList2[num10];
				float num11 = m_Font.GetTextWidth(text2, CharacterSpacing);
				float num12 = (Rect.width - num11) / 2f;
				for (int num13 = 0; num13 < text2.Length; num13++)
				{
					((UISprite)arrayList[num13 + num9]).Position = new Vector2(((UISprite)arrayList[num13 + num9]).Position.x + num12, ((UISprite)arrayList[num13 + num9]).Position.y);
				}
				num9 += text2.Length;
			}
		}
		else if (AlignStyle == enAlignStyle.right)
		{
			int num14 = 0;
			for (int num15 = 0; num15 < arrayList2.Count; num15++)
			{
				string text3 = (string)arrayList2[num15];
				float num16 = m_Font.GetTextWidth(text3, CharacterSpacing);
				float num17 = Rect.width - num16;
				for (int num18 = 0; num18 < text3.Length; num18++)
				{
					((UISprite)arrayList[num18 + num14]).Position = new Vector2(((UISprite)arrayList[num18 + num14]).Position.x + num17, ((UISprite)arrayList[num18 + num14]).Position.y);
				}
				num14 += text3.Length;
			}
		}
		m_Sprite = new UISprite[arrayList.Count];
		for (int num19 = 0; num19 < arrayList.Count; num19++)
		{
			m_Sprite[num19] = (UISprite)arrayList[num19];
		}
	}
}
