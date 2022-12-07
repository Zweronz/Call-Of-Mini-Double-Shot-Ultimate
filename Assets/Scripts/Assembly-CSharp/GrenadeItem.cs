using System.Collections;
using UnityEngine;
using Zombie3D;

public class GrenadeItem : MonoBehaviour
{
	protected float startTime;

	public float explodeTime = 4f;

	public float radius = 10f;

	public float damage = 20f;

	private Player player;

	public GameObject explodeObj;

	private void Start()
	{
		startTime = Time.time;
		player = GameApp.GetInstance().GetGameScene().GetPlayer();
	}

	private void Update()
	{
		if (!(Time.time - startTime > explodeTime))
		{
			return;
		}
		Hashtable enemies = GameApp.GetInstance().GetGameScene().GetEnemies();
		foreach (Enemy value in enemies.Values)
		{
			if (value != null && (base.transform.position - value.GetTransform().position).sqrMagnitude < radius * radius)
			{
				DamageProperty damageProperty = new DamageProperty();
				damageProperty.damage = damage;
				value.OnHit(damageProperty, WeaponType.NoGun);
			}
		}
		if (explodeObj != null)
		{
			GameObject gameObject = Object.Instantiate(explodeObj, base.transform.position + Vector3.up * 1f, Quaternion.identity) as GameObject;
			if (GameApp.GetInstance().GetGameState().SoundOn && gameObject.GetComponent<AudioSource>() != null)
			{
				gameObject.GetComponent<AudioSource>().Play();
			}
		}
		Object.Destroy(base.gameObject);
		((TopWatchingCameraScript)GameApp.GetInstance().GetGameScene().GetCamera()).ShowExplodeEffect();
		if (player.EnemyTarget == base.transform && player.PlayerObject != null)
		{
			player.EnemyTarget = player.PlayerObject.transform;
		}
	}
}
