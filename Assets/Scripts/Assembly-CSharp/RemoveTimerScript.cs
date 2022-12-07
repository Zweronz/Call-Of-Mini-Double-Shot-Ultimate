using UnityEngine;

public class RemoveTimerScript : MonoBehaviour
{
	public float life;

	protected float createdTime;

	private RemoveTimerScript_CallBack m_CallBack;

	private void Start()
	{
		createdTime = Time.time;
	}

	public void Init(RemoveTimerScript_CallBack callBack)
	{
		m_CallBack = callBack;
	}

	private void Update()
	{
		if (Time.time - createdTime > life)
		{
			if (m_CallBack != null)
			{
				m_CallBack();
			}
			Object.Destroy(base.gameObject);
		}
	}
}
