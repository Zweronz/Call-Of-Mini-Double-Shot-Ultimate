using UnityEngine;

public class SceneUIScript : MonoBehaviour
{
	protected bool increase = true;

	protected float frames;

	protected float updateInterval = 2f;

	protected float timeLeft;

	protected string fpsStr;

	protected float accum;

	protected int count;

	protected Rect[] buttonRect;

	private void Start()
	{
		timeLeft = updateInterval;
		buttonRect = new Rect[4];
		buttonRect[0] = new Rect(0.8f * (float)Screen.width, 0.05f * (float)Screen.height, 0.16f * (float)Screen.width, 0.07f * (float)Screen.height);
		buttonRect[1] = new Rect(0.4f * (float)Screen.width, 0.75f * (float)Screen.height, 0.24f * (float)Screen.width, 0.08f * (float)Screen.height);
		buttonRect[2] = new Rect(0.4f * (float)Screen.width, 0.75f * (float)Screen.height, 0.14f * (float)Screen.width, 0.14f * (float)Screen.height);
		buttonRect[3] = new Rect(0.4f * (float)Screen.width, 0.75f * (float)Screen.height, 0.14f * (float)Screen.width, 0.14f * (float)Screen.height);
	}

	private void Update()
	{
		timeLeft -= Time.deltaTime;
		accum += Time.timeScale / Time.deltaTime;
		frames += 1f;
		if (timeLeft <= 0f)
		{
			fpsStr = "FPS:" + accum / frames;
			frames = 0f;
			accum = 0f;
			timeLeft = updateInterval;
		}
	}
}
