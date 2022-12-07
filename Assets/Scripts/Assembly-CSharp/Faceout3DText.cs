using UnityEngine;

public class Faceout3DText : MonoBehaviour
{
	private MeshRenderer mrender;

	private Color mcolor;

	private float alpha = 1f;

	private float alphaFadeSpeed = 0.1f;

	private float upSpeed = 3f;

	public void Init()
	{
		alpha = 1f;
	}

	public void SetColor(Color color)
	{
		mcolor = color;
	}

	private void Awake()
	{
		mrender = base.gameObject.GetComponent("MeshRenderer") as MeshRenderer;
		SetColor(mrender.material.color);
	}

	private void Update()
	{
		if (alpha <= 0.05f)
		{
			Faceout3DTextPool.Instance().Delete3DText(base.gameObject);
			return;
		}
		alpha -= alphaFadeSpeed;
		mcolor = new Color(mcolor.r, mcolor.g, mcolor.b, alpha);
		mrender.material.color = mcolor;
		base.transform.position += Vector3.up * upSpeed * Time.deltaTime;
	}
}
