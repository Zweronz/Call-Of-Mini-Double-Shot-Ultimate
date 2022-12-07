using UnityEngine;

public class ColorAnimationScript : MonoBehaviour
{
	public float m_AnimPeriod = 1f;

	public Color m_StartColor;

	public Color m_EndColor;

	private void Start()
	{
		SetColorAnimation();
	}

	public void SetColorAnimation()
	{
		if (base.GetComponent<Renderer>().enabled)
		{
			string text = "_TintColor";
			if (base.GetComponent<Renderer>().material.HasProperty("_TintColor"))
			{
				text = "_TintColor";
				m_StartColor = base.GetComponent<Renderer>().material.GetColor(text);
			}
			else if (base.GetComponent<Renderer>().material.HasProperty("_Color"))
			{
				text = "_Color";
				m_StartColor = base.GetComponent<Renderer>().material.GetColor(text);
			}
			else if (base.GetComponent<Renderer>().material.HasProperty("_texBase"))
			{
				text = "_texBase";
				m_StartColor = base.GetComponent<Renderer>().material.GetColor(text);
			}
			else
			{
				Debug.LogError("ColorAnimationScript.SetColorAnimation() ERROR!!!");
			}
			if (!base.GetComponent<Animation>())
			{
				base.gameObject.AddComponent<Animation>();
			}
			AnimationCurve curve = new AnimationCurve(new Keyframe(0f, m_StartColor.r, 0f, 0f), new Keyframe(m_AnimPeriod / 2f, m_EndColor.r, 0f, 0f), new Keyframe(m_AnimPeriod, m_StartColor.r, 0f, 0f));
			AnimationCurve curve2 = new AnimationCurve(new Keyframe(0f, m_StartColor.g, 0f, 0f), new Keyframe(m_AnimPeriod / 2f, m_EndColor.g, 0f, 0f), new Keyframe(m_AnimPeriod, m_StartColor.g, 0f, 0f));
			AnimationCurve curve3 = new AnimationCurve(new Keyframe(0f, m_StartColor.b, 0f, 0f), new Keyframe(m_AnimPeriod / 2f, m_EndColor.b, 0f, 0f), new Keyframe(m_AnimPeriod, m_StartColor.b, 0f, 0f));
			AnimationClip animationClip = new AnimationClip();
			animationClip.legacy = true;
			animationClip.SetCurve(string.Empty, typeof(Material), text + ".r", curve);
			animationClip.SetCurve(string.Empty, typeof(Material), text + ".g", curve2);
			animationClip.SetCurve(string.Empty, typeof(Material), text + ".b", curve3);
			animationClip.wrapMode = WrapMode.Once;
			base.GetComponent<Animation>().AddClip(animationClip, "ColorAnimation");
		}
		else
		{
			Debug.LogError("ERROR: - " + base.gameObject.transform.root.name + " | " + base.gameObject.name + " 's Renderer is not enabled!!!");
		}
	}

	public void PlayColorAnimation()
	{
		if (base.GetComponent<Animation>()["ColorAnimation"] != null)
		{
			base.GetComponent<Animation>()["ColorAnimation"].wrapMode = WrapMode.Loop;
			base.GetComponent<Animation>().Play("ColorAnimation");
		}
	}

	public void StopColorAnimation()
	{
		if (base.GetComponent<Animation>()["ColorAnimation"] != null)
		{
			base.GetComponent<Animation>().Stop("ColorAnimation");
		}
	}
}
