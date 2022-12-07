using UnityEngine;

public class ShadowLightFlash : MonoBehaviour
{
	public float MinAlpha;

	public float MaxAlpha = 1f;

	public float FrameTime = 0.1f;

	private void Start()
	{
		if (!base.GetComponent<Animation>())
		{
			base.gameObject.AddComponent<Animation>();
		}
		AnimationCurve curve = new AnimationCurve(new Keyframe(0f, MinAlpha, 0f, 0f), new Keyframe(FrameTime / 2f, MaxAlpha, 0f, 0f), new Keyframe(FrameTime, MinAlpha, 0f, 0f));
		AnimationClip animationClip = new AnimationClip();
		animationClip.legacy = true;
		animationClip.SetCurve(string.Empty, typeof(Material), "_TintColor.a", curve);
		animationClip.wrapMode = WrapMode.Loop;
		base.GetComponent<Animation>().AddClip(animationClip, "Alpha");
		base.GetComponent<Animation>().Play("Alpha");
	}
}
