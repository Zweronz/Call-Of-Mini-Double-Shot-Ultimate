using System.Collections;
using System.Collections.Generic;
using Zombie3D;
using UnityEngine;

public class CannonShieldKeep : MonoBehaviour
{
	public Weapon m_Weapon;

	private float time = 4f;

	void Update()
	{
		if (time > 0f)
		{
			time -= Time.deltaTime;
			return;
		}
		Destroy(transform.parent.gameObject);
	}

	void OnTriggerEnter()
	{
		Player OwnedPlayer = m_Weapon.GetOwnedPlayer();
		Vector3 vector2 = base.transform.position + 1.5f * base.transform.up;
		List<Enemy> list2 = new List<Enemy>();
		Hashtable enemies2 = GameApp.GetInstance().GetGameScene().GetEnemies();
		foreach (Enemy value2 in enemies2.Values)
		{
			if (!(value2.HP <= 0f) && Vector3.Distance(vector2, new Vector3(value2.GetTransform().position.x, vector2.y, value2.GetTransform().position.z)) <= 3f)
			{
				list2.Add(value2);
			}
		}
		foreach (Enemy item2 in list2)
		{
			if (item2.GetState() == Enemy.DEAD_STATE)
			{
				continue;
			}
			DamageProperty damageProperty3 = new DamageProperty();
			if (base.name.StartsWith("cannoni_"))
			{
				damageProperty3.damage = 200f;
			}
			else
			{
				damageProperty3.damage = 275f;
			}
			damageProperty3.speedFactorTime = 0.4f;
			damageProperty3.speedFactor = -0.5f;
			WeaponType WeaponType = WeaponType.Ion_CannonI;
			if (base.name.StartsWith("cannoni_"))
			{
				WeaponType = WeaponType.Ion_Cannon;
			}
			item2.OnHit(damageProperty3, WeaponType);
		}
		if (OwnedPlayer != null)
		{
			OwnedPlayer.CheckAttackBloodSuck();
		}
	}
}
