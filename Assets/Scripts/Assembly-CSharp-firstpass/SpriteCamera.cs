using UnityEngine;

public class SpriteCamera : MonoBehaviour
{
	private int m_Layer;

	private Transform m_Transform;

	private Camera m_Camera;

	private Rect m_Range = new Rect(-1f, -1f, 1f, 1f);

	public void Initialize(int layer)
	{
		base.transform.position = Vector3.zero;
		base.transform.rotation = Quaternion.identity;
		base.transform.localScale = Vector3.one;
		m_Camera = (Camera)base.gameObject.AddComponent(typeof(Camera));
		m_Layer = layer;
		m_Camera.clearFlags = CameraClearFlags.Nothing;
		m_Camera.backgroundColor = Color.white;
		m_Camera.nearClipPlane = -1f;
		m_Camera.farClipPlane = 1f;
		m_Camera.orthographic = true;
		m_Camera.cullingMask = 1 << m_Layer;
	}

	public void SetClear(bool clear)
	{
		if (clear)
		{
			m_Camera.clearFlags = CameraClearFlags.Color;
		}
		else
		{
			m_Camera.clearFlags = CameraClearFlags.Nothing;
		}
	}

	public void SetDepth(float depth)
	{
		m_Camera.depth = depth;
	}

	public void SetViewport(Rect range)
	{
		base.transform.position = new Vector3((range.xMin + range.xMax + 1f) / 2f, (range.yMin + range.yMax - 1f) / 2f, 0f);
		m_Camera.pixelRect = new Rect(0f, 0f, Screen.width, Screen.height);
		m_Camera.aspect = range.width / range.height;
		m_Camera.orthographicSize = range.height / 2f;
		m_Range = range;
	}

	public Vector2 ScreenToWorld(Vector2 point)
	{
		return new Vector2(m_Range.x + point.x / (float)Screen.width * m_Range.width, m_Range.y + point.y / (float)Screen.height * m_Range.height);
	}
}
