using UnityEngine;

public class iPhoneInputMgr
{
	private static UITouchInner[] touches = new UITouchInner[0];

	private static UITouchInner[] touches0 = new UITouchInner[0];

	private static UITouchInner[] touches1 = new UITouchInner[1];

	private static UITouchInner[] touches2 = new UITouchInner[2];

	public static float m_fOffectX = 0f;

	public static float m_fOffectY = 0f;

	public static float m_fScreenToRatio = 1f;

	public static UITouchInner[] MockTouches()
	{
		float fOffectX = m_fOffectX;
		float fOffectY = m_fOffectY;
		if (Input.touches.Length == 0)
		{
			return touches0;
		}
		if (Input.touches.Length == 1)
		{
			int num = 0;
			Touch[] array = Input.touches;
			for (int i = 0; i < array.Length; i++)
			{
				Touch touch = array[i];
				touches1[num].deltaPosition = new Vector2(touch.deltaPosition.x * m_fScreenToRatio, touch.deltaPosition.y * m_fScreenToRatio);
				touches1[num].deltaTime = touch.deltaTime;
				touches1[num].fingerId = touch.fingerId;
				touches1[num].phase = touch.phase;
				touches1[num].position = new Vector2(touch.position.x * m_fScreenToRatio + fOffectX, touch.position.y * m_fScreenToRatio + fOffectY);
				touches1[num].tapCount = touch.tapCount;
				Debug.LogWarning(string.Concat("|touch.position|", touch.position, "|touches1[i].position|", touches1[num].position));
				num++;
			}
			return touches1;
		}
		if (Input.touches.Length == 2)
		{
			int num2 = 0;
			Touch[] array2 = Input.touches;
			for (int j = 0; j < array2.Length; j++)
			{
				Touch touch2 = array2[j];
				touches2[num2].deltaPosition = new Vector2(touch2.deltaPosition.x * m_fScreenToRatio, touch2.deltaPosition.y * m_fScreenToRatio);
				touches2[num2].deltaTime = touch2.deltaTime;
				touches2[num2].fingerId = touch2.fingerId;
				touches2[num2].phase = touch2.phase;
				touches2[num2].position = new Vector2(touch2.position.x * m_fScreenToRatio + fOffectX, touch2.position.y * m_fScreenToRatio + fOffectY);
				touches2[num2].tapCount = touch2.tapCount;
				Debug.LogWarning(string.Concat("|touch.position|", touch2.position, "|touches2[i].position|", touches2[num2].position));
				num2++;
			}
			return touches2;
		}
		touches = new UITouchInner[Input.touches.Length];
		int num3 = 0;
		Touch[] array3 = Input.touches;
		for (int k = 0; k < array3.Length; k++)
		{
			Touch touch3 = array3[k];
			touches[num3].deltaPosition = new Vector2(touch3.deltaPosition.x * m_fScreenToRatio, touch3.deltaPosition.y * m_fScreenToRatio);
			touches[num3].deltaTime = touch3.deltaTime;
			touches[num3].fingerId = touch3.fingerId;
			touches[num3].phase = touch3.phase;
			touches[num3].position = new Vector2(touch3.position.x * m_fScreenToRatio + fOffectX, touch3.position.y * m_fScreenToRatio + fOffectY);
			touches[num3].tapCount = touch3.tapCount;
			Debug.LogWarning(string.Concat("|touch.position|", touch3.position, "|touches[i].position|", touches[num3].position));
			num3++;
		}
		return touches;
	}
}
