using System.Collections;
using UnityEngine;

public class mgrFont
{
	protected static mgrFont _mgrFontInstance;

	protected Hashtable _fonts;

	public static mgrFont Instance()
	{
		if (_mgrFontInstance == null)
		{
			_mgrFontInstance = new mgrFont();
		}
		return _mgrFontInstance;
	}

	public Font getFont(string fontName)
	{
		if (_fonts == null)
		{
			_fonts = new Hashtable();
		}
		if (AutoUI.IsRetain == AutoUI.RESOLUTION.LOWDEFINITION)
		{
			switch (fontName)
			{
			case "Zombie3D/Font/037-CAI978-10":
				fontName = "Zombie3D/Font/037-CAI978-7";
				break;
			case "Zombie3D/Font/037-CAI978-13":
				fontName = "Zombie3D/Font/037-CAI978-7";
				break;
			case "Zombie3D/Font/037-CAI978-15":
				fontName = "Zombie3D/Font/037-CAI978-7";
				break;
			case "Zombie3D/Font/037-CAI978-18":
				fontName = "Zombie3D/Font/037-CAI978-9";
				break;
			case "Zombie3D/Font/037-CAI978-22":
				fontName = "Zombie3D/Font/037-CAI978-11";
				break;
			case "Zombie3D/Font/037-CAI978-27":
				fontName = "Zombie3D/Font/037-CAI978-13_1";
				break;
			case "Zombie3D/Font/037-CAI978-spec1":
				fontName = "Zombie3D/Font/037-CAI978-spec1_LOW";
				break;
			}
		}
		if (_fonts.Contains(fontName))
		{
			return (Font)_fonts[fontName];
		}
		Font font = new Font();
		font._Material = Resources.Load(fontName + "_M") as Material;
		if (null == font._Material)
		{
			Debug.Log("Cannot find text_matertial : " + fontName);
			return null;
		}
		TextAsset textAsset = Resources.Load(fontName + "_cfg") as TextAsset;
		if (null != textAsset && textAsset.text != null)
		{
			string[] array = textAsset.text.Split('\n');
			string[] array2 = array[0].Split(' ');
			font.TextureWidth = int.Parse(array2[0]);
			font.TextureHeight = int.Parse(array2[1]);
			font.CellWidth = int.Parse(array2[2]);
			font.CellHeight = int.Parse(array2[3]);
			font.OffsetX = int.Parse(array2[4]);
			font.OffsetY = int.Parse(array2[5]);
			string[] array3 = array[1].Split(' ');
			for (int i = 0; i < array3.Length; i++)
			{
				font._widths.Add(float.Parse(array3[i]));
			}
			_fonts[fontName] = font;
			return font;
		}
		Debug.Log("Cannot find font text file : " + fontName);
		return null;
	}
}