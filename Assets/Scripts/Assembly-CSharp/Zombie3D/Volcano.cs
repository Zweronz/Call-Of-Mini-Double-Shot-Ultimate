using UnityEngine;

namespace Zombie3D
{
	public class Volcano : Weapon
	{
		protected float gunFireTimer = -1f;

		protected float gunFireShowTime = 0.06f;

		protected Vector3 bulletPosOffset = new Vector3(0.145f, -0.043f, 1.6f);

		private GameObject GunFireShadowLight;

		private int shootTimes;

		public Volcano()
		{
			maxCapacity = 100000000;
			maxGunLoad = 100000000;
			bulletCount = maxGunLoad;
			price = 1000;
			base.IsSelectedForBattle = false;
		}

		public override WeaponType GetWeaponType()
		{
			return WeaponType.Volcano;
		}

		public override void Init(Player owner)
		{
			base.Init(owner);
			gunfire = rightHandGun.transform.Find("gun_fire_new").gameObject;
			ShowGunFire(false);
			GunFireShadowLight = rightHandGun.transform.Find("GunFire_ShadowLight").gameObject;
			GunFireShadowLight.GetComponent<Renderer>().enabled = false;
			GunFireShadowLight.AddComponent(typeof(KeepFlat));
			defaultTriggerTime = 0f;
			base.TriggerTime = defaultTriggerTime;
			m_WeaponBulletPool = new WeaponBulletsPool();
			m_WeaponBulletPool.Init("BulletPool - Volcano", gConf.weaponBullets[(int)(GetWeaponType() - 1)], 15);
			m_WeaponBulletShellsPool = new WeaponBulletsShellPool();
			m_WeaponBulletShellsPool.Init("BulletShellsPool - Volcano", gConf.BulletShell01, 2f, 15);
			m_WeaponBulletHitParticlesPool = new WeaponBulletsHitParticlePool();
			m_WeaponBulletHitParticlesPool.Init("BulletHitParticlesPool - Volcano", gConf.weaponBulletHitParticles[(int)(GetWeaponType() - 1)], 15);
			TimerManager.GetInstance().SetTimer(73, 0.1f, true);
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
			if (Time.time - lastShootTime > 2f && shootTimes > 0)
			{
				shootTimes = 0;
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
			shootTimes++;
			float y = player.GetTransform().localEulerAngles.y;
			Vector3 pos = rightHandGun.transform.TransformPoint(bulletPosOffset);
			float num = 0.2f;
			float f = (float)((shootTimes - 1) % 6) * 60f;
			pos += num * new Vector3(Mathf.Sin(f), Mathf.Cos(f), 0f);
			if (pos.y < 10000.6f)
			{
				pos = new Vector3(pos.x, 10000.6f, pos.z);
			}
			gunfire.transform.localPosition = new Vector3(0.17f, -0.05f, 2.035f) + 0.2f * new Vector3(Mathf.Sin(f), Mathf.Cos(f), 0f);
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
			}
			if (GunFireShadowLight != null)
			{
				GunFireShadowLight.transform.position = new Vector3(GunFireShadowLight.transform.position.x, 10000.5f, GunFireShadowLight.transform.position.z);
				GunFireShadowLight.transform.Rotate(Vector3.forward, y);
				GunFireShadowLight.GetComponent<Renderer>().enabled = true;
			}
			if (shootTimes % 3 == 0)
			{
				CreateBulletShell(true);
			}
			if (TimerManager.GetInstance().Ready(73))
			{
				audioPlayer.PlaySound("ShootAudio", true);
				TimerManager.GetInstance().Do(73);
			}
			lastShootTime = Time.time;
		}

		public override void ShowGunFire(bool bShow)
		{
			gunfire.GetComponent<Renderer>().enabled = bShow;
		}
	}
}
