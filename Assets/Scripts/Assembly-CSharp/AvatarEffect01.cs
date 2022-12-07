using UnityEngine;

public class AvatarEffect01 : MonoBehaviour
{
	public Color m_StartColor;

	public Color m_EndColor;

	public float m_AnimPeriod = 1f;

	private void Start()
	{
		if (!base.GetComponent<Animation>())
		{
			base.gameObject.AddComponent<Animation>();
		}
		AnimationCurve curve = new AnimationCurve(new Keyframe(0f, m_StartColor.r, 0f, 0f), new Keyframe(m_AnimPeriod / 2f, m_EndColor.r, 0f, 0f), new Keyframe(m_AnimPeriod, m_StartColor.r, 0f, 0f));
		AnimationCurve curve2 = new AnimationCurve(new Keyframe(0f, m_StartColor.g, 0f, 0f), new Keyframe(m_AnimPeriod / 2f, m_EndColor.g, 0f, 0f), new Keyframe(m_AnimPeriod, m_StartColor.g, 0f, 0f));
		AnimationCurve curve3 = new AnimationCurve(new Keyframe(0f, m_StartColor.b, 0f, 0f), new Keyframe(m_AnimPeriod / 2f, m_EndColor.b, 0f, 0f), new Keyframe(m_AnimPeriod, m_StartColor.b, 0f, 0f));
		AnimationCurve curve4 = new AnimationCurve(new Keyframe(0f, m_StartColor.a, 0f, 0f), new Keyframe(m_AnimPeriod / 2f, m_EndColor.a, 0f, 0f), new Keyframe(m_AnimPeriod, m_StartColor.a, 0f, 0f));
		AnimationClip animationClip = new AnimationClip();
		animationClip.legacy = true;
		animationClip.SetCurve(string.Empty, typeof(Material), "_Color.r", curve);
		animationClip.SetCurve(string.Empty, typeof(Material), "_Color.g", curve2);
		animationClip.SetCurve(string.Empty, typeof(Material), "_Color.b", curve3);
		animationClip.SetCurve(string.Empty, typeof(Material), "_Color.a", curve4);
		animationClip.wrapMode = WrapMode.Once;
		base.GetComponent<Animation>().AddClip(animationClip, "LightMapColorAnimation");
		base.GetComponent<Animation>()["LightMapColorAnimation"].layer = 2;
		base.GetComponent<Animation>()["LightMapColorAnimation"].wrapMode = WrapMode.Loop;
		base.GetComponent<Animation>().Play("LightMapColorAnimation");
	}

	private void Update()
	{
	}
}
