using UnityEngine;

namespace Zombie3D
{
	public class Tomahawk : Weapon
	{
		protected float gunFireTimer = -1f;

		protected float gunFireShowTime = 0.03f;

		protected Vector3 bulletPosOffset = new Vector3(0.162f, 0.538f, 1.541f);

		public Tomahawk()
		{
			maxCapacity = 100000000;
			maxGunLoad = 100000000;
			bulletCount = maxGunLoad;
			price = 1000;
			base.IsSelectedForBattle = false;
		}

		public override WeaponType GetWeaponType()
		{
			return WeaponType.Tomahawk;
		}

		public override void Init(Player owner)
		{
			base.Init(owner);
			defaultTriggerTime = 0.2f;
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
			m_WeaponBulletPool = new WeaponBulletsPool();
			m_WeaponBulletPool.Init("BulletPool - Tomahawk", gConf.weaponBullets[(int)(GetWeaponType() - 1)], 3);
			TimerManager.GetInstance().SetTimer(71, 0.1f, true);
		}

		public override void CreateGun()
		{
			rightHandGun = (GameObject)Object.Instantiate(gConf.weapons[(int)(GetWeaponType() - 1)], player.GetTransform().position, player.GetTransform().rotation);
		}

		public override void GunOn()
		{
			base.GunOn();
			ShowGunFire(false);
		}

		public override void DoLogic(float deltaTime)
		{
			if (gunfire != null && gunFireTimer >= 0f)
			{
				gunFireTimer += Time.deltaTime;
				if (gunFireTimer > gunFireShowTime)
				{
					ShowGunFire(false);
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
			if (gunfire != null)
			{
				ShowGunFire(true);
				gunFireTimer = 0f;
			}
			float y = player.GetTransform().localEulerAngles.y;
			Vector3 pos = rightHandGun.transform.TransformPoint(bulletPosOffset);
			GameObject gameObject = CreateBullet(pos, Quaternion.Euler(270f, 180f, 0f));
			if (gameObject != null)
			{
				gameObject.transform.Rotate(Vector3.forward, y);
				WeaponBulletScript component = gameObject.GetComponent<WeaponBulletScript>();
				component.m_Weapon = this;
				component.Damage = player.Attack;
				component.Speed = 15f;
				component.Rot = y;
				component.Init();
			}
			if (TimerManager.GetInstance().Ready(71))
			{
				audioPlayer.PlaySound("ShootAudio", true);
				TimerManager.GetInstance().Do(71);
			}
			lastShootTime = Time.time;
		}

		public override void ShowGunFire(bool bShow)
		{
			if (!(gunfire != null))
			{
				return;
			}
			ParticleEmitter[] componentsInChildren = gunfire.GetComponentsInChildren<ParticleEmitter>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].emit = bShow;
				if (bShow)
				{
					componentsInChildren[i].Emit();
				}
			}
		}
	}
}
