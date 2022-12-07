using UnityEngine;

namespace Zombie3D
{
	public class GrewCar_15 : Weapon
	{
		protected float gunFireTimer = -1f;

		protected float gunFireShowTime = 0.03f;

		protected Vector3 bulletPosOffset = new Vector3(0.1f, 0.386f, 2.518f);

		private GameObject GunFireShadowLight;

		public GrewCar_15()
		{
			maxCapacity = 100000000;
			maxGunLoad = 100000000;
			bulletCount = maxGunLoad;
			base.IsSelectedForBattle = false;
		}

		public override WeaponType GetWeaponType()
		{
			return WeaponType.GrewCar_15;
		}

		public override void Init(Player owner)
		{
			base.Init(owner);
			defaultTriggerTime = 0f;
			base.TriggerTime = defaultTriggerTime;
			hitForce = 20f;
			gunfire = rightHandGun.transform.Find("gun_fire_new").gameObject;
			ShowGunFire(false);
			GunFireShadowLight = rightHandGun.transform.Find("GunFire_ShadowLight").gameObject;
			if (GunFireShadowLight != null)
			{
				GunFireShadowLight.GetComponent<Renderer>().enabled = false;
				GunFireShadowLight.AddComponent(typeof(KeepFlat));
			}
			m_WeaponBulletPool = new WeaponBulletsPool();
			m_WeaponBulletPool.Init("BulletPool - GrewCar_15", gConf.weaponBullets[(int)(GetWeaponType() - 1)], 5);
			m_WeaponBulletShellsPool = new WeaponBulletsShellPool();
			m_WeaponBulletShellsPool.Init("BulletShellsPool - GrewCar_15", gConf.BulletShell01, 1f, 5);
			m_WeaponBulletHitParticlesPool = new WeaponBulletsHitParticlePool();
			m_WeaponBulletHitParticlesPool.Init("BulletHitParticlesPool - GrewCar_15", gConf.weaponBulletHitParticles[(int)(GetWeaponType() - 1)], 5);
			TimerManager.GetInstance().SetTimer(61, 0.1f, true);
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
					if (GunFireShadowLight != null)
					{
						GunFireShadowLight.GetComponent<Renderer>().enabled = false;
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
			Vector3 pos = rightHandGun.transform.TransformPoint(bulletPosOffset);
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
			if (GunFireShadowLight != null)
			{
				GunFireShadowLight.transform.position = new Vector3(GunFireShadowLight.transform.position.x, 10000.5f, GunFireShadowLight.transform.position.z);
				GunFireShadowLight.transform.Rotate(Vector3.forward, y);
				GunFireShadowLight.GetComponent<Renderer>().enabled = true;
				ShadowLightFlash shadowLightFlash = GunFireShadowLight.GetComponent(typeof(ShadowLightFlash)) as ShadowLightFlash;
				if (shadowLightFlash != null)
				{
					GunFireShadowLight.GetComponent<Animation>()["Alpha"].speed = GunFireShadowLight.GetComponent<Animation>()["Alpha"].length / gunFireShowTime;
					GunFireShadowLight.GetComponent<Animation>().Play("Alpha");
				}
			}
			if (TimerManager.GetInstance().Ready(61))
			{
				audioPlayer.PlaySound("ShootAudio", true);
				TimerManager.GetInstance().Do(61);
			}
			lastShootTime = Time.time;
		}

		public override void ShowGunFire(bool bShow)
		{
			gunfire.GetComponent<Renderer>().enabled = bShow;
		}
	}
}
