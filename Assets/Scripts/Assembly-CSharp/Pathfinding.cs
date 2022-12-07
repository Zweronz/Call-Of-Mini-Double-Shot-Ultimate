using UnityEngine;

public class Pathfinding
{
	private static Pathfinding m_instance;

	private PathfindingImpl m_impl;

	public static Pathfinding Instance()
	{
		if (m_instance == null)
		{
			m_instance = new Pathfinding();
		}
		return m_instance;
	}

	public void SetImpl(PathfindingImpl impl)
	{
		m_impl = impl;
	}

	public Vector4[] FindPath(Vector3 begin, Vector3 end)
	{
		if (m_impl == null)
		{
			return null;
		}
		return m_impl.FindPath(begin, end);
	}
}
