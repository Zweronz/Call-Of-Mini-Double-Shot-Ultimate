using UnityEngine;

public class BloodRect : MonoBehaviour
{
	private float m_BloodRectPercent = 1f;

	private Transform m_RedBar;

	private void Start()
	{
		m_RedBar = base.transform.Find("blood_01");
	}

	private void Update()
	{
		Vector3 lhs = -Camera.main.transform.forward;
		Vector3 normalized = Vector3.Cross(lhs, Vector3.up).normalized;
		Vector3 normalized2 = Vector3.Cross(lhs, normalized).normalized;
		base.transform.up = normalized2;
		m_RedBar.localScale = new Vector3(1f * m_BloodRectPercent, 1f, 1f);
	}

	public void SetBloodPercent(float percent)
	{
		if (percent > 1f)
		{
			Debug.LogError("BloodRect: " + percent);
		}
		m_BloodRectPercent = Mathf.Clamp01(percent);
	}
}
