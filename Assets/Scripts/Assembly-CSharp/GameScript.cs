using UnityEngine;
using Zombie3D;

public class GameScript : MonoBehaviour
{
	protected float lastUpdateTime;

	protected float deltaTime;

	private void Start()
	{
		GameApp.GetInstance().Init();
		lastUpdateTime = Time.time;
	}

	private void Update()
	{
		deltaTime += Time.deltaTime;
		if (deltaTime >= 0.001f)
		{
			GameApp.GetInstance().Loop(deltaTime);
			deltaTime = 0f;
		}
	}
}
