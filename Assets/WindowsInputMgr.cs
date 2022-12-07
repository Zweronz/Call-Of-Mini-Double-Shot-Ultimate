using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowsInputMgr : MonoBehaviour
{
	public static float m_fOffectX = 0f;

	public static float m_fOffectY = 0f;

	public static float m_fScreenToRatio = 1f;

	public static UITouchInner[] MockTouches()
	{
		float fOffectX = m_fOffectX;
		float fOffectY = m_fOffectY;
		UITouchInner[] touches = new UITouchInner[1];
		foreach(Touch touch in InputHelper.GetTouches())
		{
			touches[0].deltaPosition = new Vector2(touch.deltaPosition.x * m_fScreenToRatio, touch.deltaPosition.y * m_fScreenToRatio);
			touches[0].deltaTime = touch.deltaTime;
			touches[0].fingerId = touch.fingerId;
			touches[0].phase = touch.phase;
			touches[0].position = new Vector2(touch.position.x * m_fScreenToRatio + fOffectX, touch.position.y * m_fScreenToRatio + fOffectY);
			touches[0].tapCount = 1;
			return touches;
		}
		return touches;
	}
}