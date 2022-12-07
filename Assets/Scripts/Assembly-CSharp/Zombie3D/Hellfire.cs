using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zombie3D
{
	public class Hellfire : Weapon
	{
		protected float gunFireTimer = -1f;

		protected float gunFireShowTime = 0.03f;

		protected Vector3 bulletPosOffset = new Vector3(0.141f, -0.125f, 2.132f);

		private GameObject Bullet_Fire;

		private bool bFiring;

		private float m_FireStartTime;

		public Hellfire()
		{
			maxCapacity = 100000000;
			maxGunLoad = 100000000;
			bulletCount = maxGunLoad;
			price = 1000;
			base.IsSelectedForBattle = false;
		}

		public override WeaponType GetWeaponType()
		{
			return WeaponType.Hellfire;
		}

		public override void Init(Player owner)
		{
			base.Init(owner);
			gunfire = rightHandGun.transform.Find("gun_fire_new").gameObject;
			gunfire.GetComponent<Renderer>().enabled = false;
			Bullet_Fire = rightHandGun.transform.Find("Bullet_Hellfire/hellfire_01").gameObject;
			bFiring = false;
			Bullet_Fire.GetComponent<ParticleEmitter>().emit = false;
		}

		public override void changeReticle()
		{
		}

		public override void CreateGun()
		{
			rightHandGun = (GameObject)Object.Instantiate(gConf.weapons[(int)(GetWeaponType() - 1)], player.GetTransform().position, player.GetTransform().rotation);
		}

		public override void GunOn()
		{
			base.GunOn();
			bFiring = false;
			Bullet_Fire.GetComponent<ParticleEmitter>().emit = false;
		}

		public override void DoLogic(float deltaTime)
		{
			base.DoLogic(deltaTime);
			if (gunFireTimer >= 0f)
			{
				gunFireTimer += Time.deltaTime;
				if (gunFireTimer > gunFireShowTime)
				{
					gunfire.GetComponent<Renderer>().enabled = false;
					gunFireTimer = -1f;
				}
			}
			if (bFiring)
			{
				Bullet_Fire.GetComponent<ParticleEmitter>().emit = true;
			}
			else
			{
				Bullet_Fire.GetComponent<ParticleEmitter>().emit = false;
			}
			if (Time.time - m_FireStartTime >= 0.5f)
			{
				Bullet_Fire.GetComponent<ParticleEmitter>().emit = false;
			}
		}

		public override void Fire(float deltaTime)
		{
			bFiring = true;
			m_FireStartTime = Time.time;
			bool flag = false;
			float num = 14f;
			Fire_PVP(num);
			Hashtable enemies = GameApp.GetInstance().GetGameScene().GetEnemies();
			foreach (Enemy value in enemies.Values)
			{
				if (value.HP <= 0f || !(Vector3.Distance(rightHandGun.transform.position, value.GetTransform().position) < num))
				{
					continue;
				}
				Vector3 vector = rightHandGun.transform.InverseTransformPoint(value.GetTransform().position);
				if (Mathf.Abs(vector.x) <= 1.4f && vector.z > 0f && vector.z < num)
				{
					float thisAttack = base.Damage;
					if (!flag)
					{
						flag = true;
						thisAttack = player.GetThisAttack();
						player.CheckAttackBloodSuck();
					}
					DamageProperty damageProperty = new DamageProperty();
					damageProperty.damage = thisAttack;
					value.OnHit(damageProperty, GetWeaponType());
				}
			}
			List<JerricanScript> jerricans = GameApp.GetInstance().GetGameScene().GetJerricans();
			if (jerricans != null && jerricans.Count > 0)
			{
				for (int i = 0; i < jerricans.Count; i++)
				{
					if (jerricans[i].gameObject != null)
					{
						Vector3 vector2 = rightHandGun.transform.InverseTransformPoint(jerricans[i].transform.position);
						if (Mathf.Abs(vector2.x) <= 1.4f && vector2.z > 0f && vector2.z < num)
						{
							jerricans[i].OnHit(base.Damage);
						}
					}
				}
			}
			PathDoor[] pathDoors = GameApp.GetInstance().GetGameScene().GetPathDoors();
			for (int j = 0; j < pathDoors.Length; j++)
			{
				if (!(pathDoors[j] != null) || !(pathDoors[j].gameObject != null) || !(pathDoors[j].GetWorm() != null))
				{
					continue;
				}
				Vector3 vector3 = rightHandGun.transform.InverseTransformPoint(pathDoors[j].GetWorm().transform.position);
				if (Mathf.Abs(vector3.x) <= 1.4f && vector3.z > 0f && vector3.z < num)
				{
					WormScript component = pathDoors[j].GetWorm().GetComponent<WormScript>();
					if (component != null)
					{
						component.OnHit(base.Damage);
					}
				}
			}
			List<EnergyFeedwayScript> energyFeedways = GameApp.GetInstance().GetGameScene().GetEnergyFeedways();
			for (int k = 0; k < energyFeedways.Count; k++)
			{
				if (energyFeedways[k].gameObject != null)
				{
					Vector3 vector4 = rightHandGun.transform.InverseTransformPoint(energyFeedways[k].transform.Find("flash_01").transform.position);
					if (Mathf.Abs(vector4.x) <= 1.4f && vector4.z > 0f && vector4.z < num)
					{
						energyFeedways[k].OnHit(base.Damage);
					}
				}
			}
			audioPlayer.PlaySound("ShootAudio", true);
			lastShootTime = Time.time;
		}

		public void Fire_PVP(float attackRadius)
		{
			if ((GameApp.GetInstance().GetGameState().m_eGameMode.m_ePlayMode != GameState.NetworkGameMode.PlayMode.E_DeathMatch && GameApp.GetInstance().GetGameState().m_eGameMode.m_ePlayMode != GameState.NetworkGameMode.PlayMode.E_LastStand) || player != GameApp.GetInstance().GetGameScene().GetPlayer())
			{
				return;
			}
			bool flag = false;
			List<Player> recipientPlayerList = PlayerManager.Instance.GetRecipientPlayerList();
			foreach (Player item in recipientPlayerList)
			{
				if (item == null || item.HP <= 0f || (GameApp.GetInstance().GetGameState().m_eGameMode.m_eCooperaMode == GameState.NetworkGameMode.NetworkCooperationMode.E_Team && item.m_iNGroupID == PlayerManager.Instance.GetPlayerClass().m_iNGroupID) || !(Vector3.Distance(rightHandGun.transform.position, item.PlayerObject.transform.position) < attackRadius))
				{
					continue;
				}
				Vector3 vector = rightHandGun.transform.InverseTransformPoint(item.PlayerObject.transform.position);
				if (!(Mathf.Abs(vector.x) <= 1.4f) || !(vector.z > 0f) || !(vector.z < attackRadius))
				{
					continue;
				}
				float num = base.Damage;
				if (!flag)
				{
					flag = true;
					if (player != null)
					{
						num = player.GetThisAttack();
						player.CheckAttackBloodSuck();
					}
					else
					{
						num = 0f;
					}
				}
				DamageProperty damageProperty = new DamageProperty();
				if (player != null)
				{
					damageProperty.damage = num;
				}
				else
				{
					damageProperty.damage = 0f;
				}
				item.NPlayerOnHitted(damageProperty.damage);
			}
		}
	}
}
