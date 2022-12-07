using UnityEngine;

public class AutoUIResolution
{
	private static uint _iphone5DefinitionWidth = 1136u;

	private static uint _iphone5DefinitionHeight = 640u;

	public static bool bIsIphoneResolution;

	public static void Init()
	{
		if (Screen.width == _iphone5DefinitionWidth && Screen.height == _iphone5DefinitionHeight)
		{
			bIsIphoneResolution = true;
		}
		else
		{
			bIsIphoneResolution = false;
		}
	}

	public static Rect ToShiftToRight(Rect srcRect, int type)
	{
		if (!bIsIphoneResolution || type == 0)
		{
			return srcRect;
		}
		return srcRect;
	}

	public static Vector2 ToShiftToRight(Vector2 _position, int type)
	{
		if (!bIsIphoneResolution || type == 0)
		{
			return _position;
		}
		Vector2 result = _position;
		switch (type)
		{
		case 1:
			result.x += _iphone5DefinitionWidth - 960;
			break;
		case 2:
			result.x += (_iphone5DefinitionWidth - 960) / 2u;
			break;
		}
		return result;
	}

	public static Rect ToShiftToRight_Recursive(Rect srcRect, int type)
	{
		if (!bIsIphoneResolution || type == 0)
		{
			return srcRect;
		}
		Rect result = srcRect;
		switch (type)
		{
		case 1:
			result.x -= _iphone5DefinitionWidth - 960;
			break;
		case 2:
			result.x -= (_iphone5DefinitionWidth - 960) / 2u;
			break;
		}
		return result;
	}

	public static Rect ToLongRectOfI5(Rect rect)
	{
		if (!bIsIphoneResolution)
		{
			return rect;
		}
		return new Rect(rect.x, rect.y, _iphone5DefinitionWidth, _iphone5DefinitionHeight);
	}

	public static Vector2 ToLongRectOfI5(Vector2 rect)
	{
		if (!bIsIphoneResolution)
		{
			return rect;
		}
		return new Vector2(_iphone5DefinitionWidth, _iphone5DefinitionHeight);
	}
}
