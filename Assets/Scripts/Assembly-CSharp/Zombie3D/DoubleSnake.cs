using UnityEngine;

namespace Zombie3D
{
	public class DoubleSnake : Weapon
	{
		protected float gunFireTimer = -1f;

		protected float gunFireShowTime = 0.03f;

		protected Vector3 bulletPosOffset_Front = new Vector3(0.1f, 0.386f, 2.518f);

		protected Vector3 bulletPosOffset_Back = new Vector3(0.1f, 0.386f, -2.518f);

		private GameObject GunFireShadowLight_Front;

		private GameObject GunFireShadowLight_Back;

		public DoubleSnake()
		{
			maxCapacity = 100000000;
			maxGunLoad = 100000000;
			bulletCount = maxGunLoad;
			base.IsSelectedForBattle = false;
		}

		public override WeaponType GetWeaponType()
		{
			return WeaponType.DoubleSnake;
		}

		public override void Init(Player owner)
		{
			base.Init(owner);
			defaultTriggerTime = 0f;
			base.TriggerTime = defaultTriggerTime;
			hitForce = 20f;
			gunfire = rightHandGun.transform.Find("gun_fire_new").gameObject;
			ShowGunFire(false);
			GunFireShadowLight_Front = rightHandGun.transform.Find("GunFire_ShadowLight/GunFire_ShadowLight_Front").gameObject;
			if (GunFireShadowLight_Front != null)
			{
				GunFireShadowLight_Front.GetComponent<Renderer>().enabled = false;
				GunFireShadowLight_Front.AddComponent(typeof(KeepFlat));
			}
			GunFireShadowLight_Back = rightHandGun.transform.Find("GunFire_ShadowLight/GunFire_ShadowLight_Back").gameObject;
			if (GunFireShadowLight_Back != null)
			{
				GunFireShadowLight_Back.GetComponent<Renderer>().enabled = false;
				GunFireShadowLight_Back.AddComponent(typeof(KeepFlat));
			}
			m_WeaponBulletPool = new WeaponBulletsPool();
			m_WeaponBulletPool.Init("BulletPool - DoubleSnake", gConf.weaponBullets[(int)(GetWeaponType() - 1)], 5);
			m_WeaponBulletShellsPool = new WeaponBulletsShellPool();
			m_WeaponBulletShellsPool.Init("BulletShellsPool - DoubleSnake", gConf.BulletShell01, 1f, 5);
			m_WeaponBulletHitParticlesPool = new WeaponBulletsHitParticlePool();
			m_WeaponBulletHitParticlesPool.Init("BulletHitParticlesPool - DoubleSnake", gConf.weaponBulletHitParticles[(int)(GetWeaponType() - 1)], 5);
			TimerManager.GetInstance().SetTimer(81, 0.1f, true);
		}

		public override void CreateGun()
		{
			rightHandGun = (GameObject)Object.Instantiate(gConf.weapons[(int)(GetWeaponType() - 1)], player.GetTransform().position, player.GetTransform().rotation);
		}

		public override void DoLogic(float deltaTime)
		{
			if (gunFireTimer >= 0f)
			{
				gunFireTimer += Time.deltaTime;
				if (gunFireTimer > gunFireShowTime)
				{
					ShowGunFire(false);
					if (GunFireShadowLight_Front != null)
					{
						GunFireShadowLight_Front.GetComponent<Renderer>().enabled = false;
					}
					if (GunFireShadowLight_Back != null)
					{
						GunFireShadowLight_Back.GetComponent<Renderer>().enabled = false;
					}
					gunFireTimer = -1f;
				}
			}
			base.DoLogic(deltaTime);
		}

		public override void Fire(float deltaTime)
		{
			if (bulletCount == 0)
			{
				player.SetState(Player.IDLE_STATE);
				StopFire();
				return;
			}
			if (Time.time - lastShootTime > attackFrenquency)
			{
				isCDing = false;
			}
			gunFireTimer = 0f;
			ShowGunFire(true);
			float y = player.GetTransform().localEulerAngles.y;
			Vector3 pos = rightHandGun.transform.TransformPoint(bulletPosOffset_Front);
			GameObject gameObject = CreateBullet(pos, Quaternion.Euler(270f, 180f, 0f));
			if (gameObject != null)
			{
				gameObject.transform.Rotate(Vector3.forward, y);
				WeaponBulletScript component = gameObject.GetComponent<WeaponBulletScript>();
				component.m_Weapon = this;
				component.Damage = player.Attack;
				component.Speed = 27f;
				component.Rot = y;
				component.Init();
				CreateBulletShell(true);
			}
			Vector3 pos2 = rightHandGun.transform.TransformPoint(bulletPosOffset_Back);
			GameObject gameObject2 = CreateBullet(pos2, Quaternion.Euler(270f, 180f, 0f));
			if (gameObject2 != null)
			{
				gameObject2.transform.Rotate(Vector3.forward, y + 180f);
				WeaponBulletScript component2 = gameObject2.GetComponent<WeaponBulletScript>();
				component2.m_Weapon = this;
				component2.Damage = player.Attack;
				component2.Speed = 27f;
				component2.Rot = y;
				component2.Init();
				CreateBulletShell(true);
			}
			if (GunFireShadowLight_Front != null)
			{
				GunFireShadowLight_Front.transform.position = new Vector3(GunFireShadowLight_Front.transform.position.x, 10000.5f, GunFireShadowLight_Front.transform.position.z);
				GunFireShadowLight_Front.transform.Rotate(Vector3.forward, y);
				GunFireShadowLight_Front.GetComponent<Renderer>().enabled = true;
				ShadowLightFlash shadowLightFlash = GunFireShadowLight_Front.GetComponent(typeof(ShadowLightFlash)) as ShadowLightFlash;
				if (shadowLightFlash != null)
				{
					GunFireShadowLight_Front.GetComponent<Animation>()["Alpha"].speed = GunFireShadowLight_Front.GetComponent<Animation>()["Alpha"].length / gunFireShowTime;
					GunFireShadowLight_Front.GetComponent<Animation>().Play("Alpha");
				}
			}
			if (TimerManager.GetInstance().Ready(81))
			{
				audioPlayer.PlaySound("ShootAudio", true);
				TimerManager.GetInstance().Do(81);
			}
			lastShootTime = Time.time;
		}

		public override void ShowGunFire(bool bShow)
		{
			Renderer[] componentsInChildren = gunfire.gameObject.GetComponentsInChildren<Renderer>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].enabled = bShow;
			}
		}
	}
}
