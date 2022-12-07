using UnityEngine;

public class DisactiveTimerScript : MonoBehaviour
{
	public float activeTime;

	protected float createdTime;

	private void Start()
	{
		Init();
	}

	public void Init()
	{
		createdTime = Time.time;
	}

	private void Update()
	{
		if (Time.time - createdTime > activeTime)
		{
			base.gameObject.SetActiveRecursively(false);
		}
	}
}
