using UnityEngine;

public class AutoUI
{
	public enum RESOLUTION
	{
		NONE = -1,
		HIGHDEFINITION = 0,
		LOWDEFINITION = 1
	}

	public static RESOLUTION IsRetain;

	private static uint _highDefinitionWidth = 960u;

	private static uint _highDefinitionHeight = 640u;

	private static uint _lowDefinitionWidth = 480u;

	private static uint _lowDefinitionHeight = 320u;

	public static void Init()
	{
		if (Screen.width == _highDefinitionWidth && Screen.height == _highDefinitionHeight)
		{
			IsRetain = RESOLUTION.HIGHDEFINITION;
		}
		else if (Screen.width == _lowDefinitionWidth && Screen.height == _lowDefinitionHeight)
		{
			IsRetain = RESOLUTION.LOWDEFINITION;
		}
	}

	public static Rect AutoRect(Rect rect)
	{
		if (IsRetain != 0)
		{
			if (IsRetain == RESOLUTION.LOWDEFINITION)
			{
				rect = new Rect(rect.left / 2f, rect.top / 2f, rect.width / 2f, rect.height / 2f);
			}
			else
			{
				Debug.Log("Error no option" + Screen.width + "|" + Screen.height);
			}
		}
		return rect;
	}

	public static Vector2 AutoSize(Vector2 vec)
	{
		if (IsRetain != 0)
		{
			if (IsRetain == RESOLUTION.LOWDEFINITION)
			{
				vec = new Vector2(vec.x / 2f, vec.y / 2f);
			}
			else
			{
				Debug.Log("Error no option" + Screen.width + "|" + Screen.height);
			}
		}
		return vec;
	}

	public static float AutoDistance(float dis)
	{
		if (IsRetain != 0)
		{
			if (IsRetain == RESOLUTION.LOWDEFINITION)
			{
				dis /= 2f;
			}
			else
			{
				Debug.Log("Error no option" + Screen.width + "|" + Screen.height);
			}
		}
		return dis;
	}
}
