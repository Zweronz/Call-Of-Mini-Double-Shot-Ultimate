using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonShieldOn : MonoBehaviour
{
	private bool doneTheInstantiate;
	void Update ()
	{
		if (!GetComponentInChildren<ParticleSystem>().isPlaying && !doneTheInstantiate)
		{
			Instantiate(Resources.Load<GameObject>("zombie3d/effect/cannoni_shield_plus_keep_pfb"), base.transform.position, base.transform.rotation);
			doneTheInstantiate = true;
		}
	}
}
