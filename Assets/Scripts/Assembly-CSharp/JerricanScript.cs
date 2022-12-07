using System.Collections;
using UnityEngine;
using Zombie3D;

public class JerricanScript : MonoBehaviour
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
		GameObject gameObject = base.transform.Find("BloodRect").gameObject;
		if (gameObject != null)
		{
			bloodRect = gameObject.GetComponent(typeof(BloodRect)) as BloodRect;
			if (bloodRect != null)
			{
				bloodRect.SetBloodPercent(1f);
			}
		}
	}

	private void Update()
	{
		if (Time.time - lastUpdateTime < 0.001f)
		{
			return;
		}
		lastUpdateTime = Time.time;
		bloodRect.SetBloodPercent(Mathf.Clamp01(curHp / maxHp));
		if (!(curHp <= 0f))
		{
			return;
		}
		GameObject gameObject = Object.Instantiate(GameApp.GetInstance().GetGameConfig().Exlposion02, base.transform.position + Vector3.up * 1.5f, Quaternion.identity) as GameObject;
		if (GameApp.GetInstance().GetGameState().SoundOn)
		{
			GameObject gameObject2 = Object.Instantiate(GameApp.GetInstance().GetGameConfig().JerricanExplodeAudio, base.transform.position, Quaternion.identity) as GameObject;
			RemoveTimerScript removeTimerScript = gameObject2.AddComponent(typeof(RemoveTimerScript)) as RemoveTimerScript;
			removeTimerScript.life = gameObject2.GetComponent<AudioSource>().clip.length + 0.1f;
			gameObject2.GetComponent<AudioSource>().Play();
		}
		GameObject gameObject3 = Object.Instantiate(GameApp.GetInstance().GetGameConfig().JerricanExplodeFloor, new Vector3(base.transform.position.x, 10000.05f, base.transform.position.z), Quaternion.Euler(270f, 0f, 0f)) as GameObject;
		Object.Destroy(base.gameObject);
		((TopWatchingCameraScript)GameApp.GetInstance().GetGameScene().GetCamera()).ShowExplodeEffect();
		GameApp.GetInstance().GetGameScene().GetJerricans()
			.Remove(this);
		Hashtable enemies = GameApp.GetInstance().GetGameScene().GetEnemies();
		foreach (Enemy value in enemies.Values)
		{
			if (value != null && (base.transform.position - value.GetTransform().position).sqrMagnitude < radius * radius)
			{
				DamageProperty damageProperty = new DamageProperty();
				damageProperty.damage = 320f;
				value.OnHit(damageProperty, WeaponType.NoGun);
			}
		}
	}

	private void OnTriggerEnter(Collider collider)
	{
		if (collider.gameObject.layer == 23)
		{
			WeaponBulletScript weaponBulletScript = collider.gameObject.GetComponent(typeof(WeaponBulletScript)) as WeaponBulletScript;
			if (weaponBulletScript != null)
			{
				curHp -= weaponBulletScript.Damage;
			}
		}
	}

	public void OnHit(float damage)
	{
		curHp -= damage;
	}
}
