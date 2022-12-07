using UnityEngine;

public class GunFireFrame : MonoBehaviour
{
	public Texture[] textures;

	public float frameTime = 0.025f;

	private float frameTimer;

	private int texIndex;

	private void Start()
	{
		frameTimer = 0f;
		texIndex = 0;
	}

	private void Update()
	{
		if (base.gameObject.GetComponent<Renderer>().enabled)
		{
			frameTimer += Time.deltaTime;
			if (frameTimer >= frameTime && textures.Length >= 2)
			{
				frameTimer = 0f;
				if (texIndex >= textures.Length - 1)
				{
					texIndex = 0;
				}
				else
				{
					texIndex++;
				}
				base.gameObject.GetComponent<Renderer>().material.mainTexture = textures[texIndex];
			}
		}
		else
		{
			frameTimer = 0f;
		}
	}
}
