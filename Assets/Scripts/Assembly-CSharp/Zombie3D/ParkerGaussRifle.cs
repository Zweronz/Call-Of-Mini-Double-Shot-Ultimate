using UnityEngine;

namespace Zombie3D
{
	public class ParkerGaussRifle : Weapon
	{
		protected float gunFireTimer = -1f;

		protected float gunFireShowTime = 0.05f;

		protected Vector3 bulletPosOffset = new Vector3(0.166f, 0.225f, 3.51f);

		private GameObject GunFireShadowLight;

		public ParkerGaussRifle()
		{
			maxCapacity = 100000000;
			maxGunLoad = 100000000;
			bulletCount = maxGunLoad;
			price = 1000;
			base.IsSelectedForBattle = false;
		}

		public override WeaponType GetWeaponType()
		{
			return WeaponType.ParkerGaussRifle;
		}

		public override void Init(Player owner)
		{
			base.Init(owner);
			defaultTriggerTime = 0.1f;
			base.TriggerTime = defaultTriggerTime;
			Transform transform = rightHandGun.transform.Find("gun_fire_new");
			if (transform != null)
			{
				gunfire = rightHandGun.transform.Find("gun_fire_new").gameObject;
			}
			if (gunfire != null)
			{
				ShowGunFire(false);
			}
			GunFireShadowLight = rightHandGun.transform.Find("GunFire_ShadowLight").gameObject;
			GunFireShadowLight.GetComponent<Renderer>().enabled = false;
			GunFireShadowLight.AddComponent(typeof(KeepFlat));
			m_WeaponBulletPool = new WeaponBulletsPool();
			m_WeaponBulletPool.Init("BulletPool - ParkerGaussRifle", gConf.weaponBullets[(int)(GetWeaponType() - 1)], 2);
			m_WeaponBulletShellsPool = new WeaponBulletsShellPool();
			m_WeaponBulletShellsPool.Init("BulletShellsPool - ParkerGaussRifle", gConf.BulletShell01, 2f, 2);
			m_WeaponBulletHitParticlesPool = new WeaponBulletsHitParticlePool();
			m_WeaponBulletHitParticlesPool.Init("BulletHitParticlesPool - ParkerGaussRifle", gConf.weaponBulletHitParticles[(int)(GetWeaponType() - 1)], 2);
			TimerManager.GetInstance().SetTimer(67, 0.1f, true);
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
					if (gunfire != null)
					{
						ShowGunFire(false);
					}
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
			if (gunfire != null)
			{
				ShowGunFire(true);
			}
			float y = player.GetTransform().localEulerAngles.y;
			Vector3 pos = rightHandGun.transform.TransformPoint(bulletPosOffset);
			GameObject gameObject = CreateBullet(pos, Quaternion.Euler(270f, 180f, 0f));
			if (gameObject != null)
			{
				gameObject.transform.Rotate(Vector3.forward, y);
				WeaponBulletScript component = gameObject.GetComponent<WeaponBulletScript>();
				component.m_Weapon = this;
				component.HitAndDestroy = false;
				component.AttackRange = 20f;
				component.Damage = player.Attack;
				component.Speed = 45f;
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
					GunFireShadowLight.GetComponent<Animation>()["Alpha"].wrapMode = WrapMode.Once;
					GunFireShadowLight.GetComponent<Animation>().Play("Alpha");
				}
			}
			if (TimerManager.GetInstance().Ready(67))
			{
				audioPlayer.PlaySound("ShootAudio", true);
				TimerManager.GetInstance().Do(67);
			}
			lastShootTime = Time.time;
		}

		public override void ShowGunFire(bool bShow)
		{
			gunfire.GetComponent<Renderer>().enabled = bShow;
		}
	}
}
