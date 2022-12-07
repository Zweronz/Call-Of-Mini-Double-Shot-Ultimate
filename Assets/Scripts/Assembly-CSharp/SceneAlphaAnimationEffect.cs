using UnityEngine;

public class SceneAlphaAnimationEffect : MonoBehaviour
{
	private void Start()
	{
		if (!base.GetComponent<Animation>())
		{
			base.gameObject.AddComponent<Animation>();
		}
		int num = Random.Range(0, 100);
		AnimationCurve curve = ((num < 33) ? new AnimationCurve(new Keyframe(0f, 0f, 0f, 0f), new Keyframe(0.5f, 1f, 0f, 0f), new Keyframe(1f, 0f, 0f, 0f)) : ((num >= 66) ? new AnimationCurve(new Keyframe(0f, 0f, 0f, 0f), new Keyframe(0.5f, 1f, 0f, 0f), new Keyframe(0.7f, 0f, 0f, 0f)) : new AnimationCurve(new Keyframe(0f, 0f, 0f, 0f), new Keyframe(0.3f, 1f, 0f, 0f), new Keyframe(0.8f, 0f, 0f, 0f))));
		AnimationClip animationClip = new AnimationClip();
		animationClip.legacy = true;
		animationClip.SetCurve(string.Empty, typeof(Material), "_TintColor.a", curve);
		animationClip.wrapMode = WrapMode.Loop;
		base.GetComponent<Animation>().AddClip(animationClip, "Alpha");
		base.GetComponent<Animation>().Play("Alpha");
	}
}
