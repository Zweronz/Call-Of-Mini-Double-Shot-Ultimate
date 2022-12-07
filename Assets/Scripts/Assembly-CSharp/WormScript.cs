using UnityEngine;

public class WormScript : MonoBehaviour
{
	public float maxHp;

	public float radius = 10f;

	private float curHp;

	protected float lastUpdateTime;

	protected BloodRect bloodRect;

	private void Start()
	{
		lastUpdateTime = 0f;
		curHp = maxHp;
		base.GetComponent<Animation>().clip.wrapMode = WrapMode.Loop;
		base.GetComponent<Animation>().Play(base.GetComponent<Animation>().clip.name);
		Transform transform = base.transform.Find("BloodRect");
		if (transform != null)
		{
			bloodRect = transform.GetComponent(typeof(BloodRect)) as BloodRect;
			if (bloodRect != null)
			{
				bloodRect.SetBloodPercent(1f);
			}
		}
	}

	private void Update()
	{
		if (!(Time.time - lastUpdateTime < 0.001f))
		{
			lastUpdateTime = Time.time;
			if (bloodRect != null)
			{
				bloodRect.SetBloodPercent(Mathf.Clamp01(curHp / maxHp));
			}
		}
	}

	private void OnTriggerEnter(Collider collider)
	{
		if (collider.gameObject.layer != 23)
		{
			return;
		}
		WeaponBulletScript weaponBulletScript = collider.gameObject.GetComponent(typeof(WeaponBulletScript)) as WeaponBulletScript;
		if (weaponBulletScript != null)
		{
			curHp -= weaponBulletScript.Damage;
			if (curHp <= 0f)
			{
				Object.Destroy(base.gameObject);
			}
		}
	}

	public float GetHp()
	{
		return curHp;
	}

	public void OnHit(float damage)
	{
		curHp -= damage;
	}
}
