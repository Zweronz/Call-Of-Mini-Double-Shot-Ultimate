using UnityEngine;

namespace Zombie3D
{
	public class BigFirework : Weapon
	{
		protected float gunFireTimer = -1f;

		protected float gunFireShowTime = 0.05f;

		protected Vector3 bulletPos = new Vector3(0.165f, 0f, 3.865f);

		private GameObject GunFireShadowLight;

		private int subAttackBulletCount = 3;

		private float subAttackTime = 0.25f;

		private float lastSubAttackTime;

		public BigFirework()
		{
			maxCapacity = 100000000;
			maxGunLoad = 100000000;
			bulletCount = maxGunLoad;
			price = 1000;
			base.IsSelectedForBattle = false;
		}

		public override WeaponType GetWeaponType()
		{
			return WeaponType.BigFirework;
		}

		public override void Init(Player owner)
		{
			base.Init(owner);
			defaultTriggerTime = 0.05f;
			base.TriggerTime = defaultTriggerTime;
			hitForce = 20f;
			gunfire = rightHandGun.transform.Find("gun_fire_new").gameObject;
			ShowGunFire(false);
			GunFireShadowLight = rightHandGun.transform.Find("GunFire_ShadowLight").gameObject;
			GunFireShadowLight.GetComponent<Renderer>().enabled = false;
			GunFireShadowLight.AddComponent(typeof(KeepFlat));
			m_WeaponBulletPool = new WeaponBulletsPool();
			m_WeaponBulletPool.Init("BulletPool - BigFirework", gConf.weaponBullets[(int)(GetWeaponType() - 1)], 6);
			TimerManager.GetInstance().SetTimer(77, 0.1f, true);
		}

		public override void CreateGun()
		{
			rightHandGun = (GameObject)Object.Instantiate(gConf.weapons[(int)(GetWeaponType() - 1)], player.GetTransform().position, player.GetTransform().rotation);
		}

		public override void GunOn()
		{
			base.GunOn();
			subAttackBulletCount = 0;
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
			if (subAttackBulletCount > 0 && Time.time - lastSubAttackTime >= subAttackTime)
			{
				ShootOneBullet();
				lastSubAttackTime = Time.time;
				subAttackBulletCount--;
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
			subAttackBulletCount = 3;
			lastSubAttackTime = Time.time;
			lastShootTime = Time.time;
		}

		public void ShootOneBullet()
		{
			Debug.Log("ShootOneBullet" + Time.time);
			gunFireTimer = 0f;
			ShowGunFire(true);
			float y = player.GetTransform().localEulerAngles.y;
			Vector3 pos = rightHandGun.transform.TransformPoint(bulletPos);
			if (pos.y < 10000.6f)
			{
				pos = new Vector3(pos.x, 10000.6f, pos.z);
			}
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
			if (TimerManager.GetInstance().Ready(77))
			{
				audioPlayer.PlaySound("ShootAudio", true);
				TimerManager.GetInstance().Do(77);
			}
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
