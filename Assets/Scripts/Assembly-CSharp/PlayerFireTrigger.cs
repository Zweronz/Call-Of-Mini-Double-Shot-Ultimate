using UnityEngine;
using Zombie3D;

public class PlayerFireTrigger : MonoBehaviour
{
	public Player player;

	private bool bTrigger;

	private int triggerLateTimes;

	private void Start()
	{
	}

	private void Update()
	{
		if (bTrigger)
		{
			triggerLateTimes++;
			if (triggerLateTimes >= 2)
			{
				bTrigger = false;
				triggerLateTimes = 0;
				player.GetWeapon().Fire(Time.deltaTime);
			}
		}
	}

	public void PullTrigger()
	{
		bTrigger = true;
	}
}
