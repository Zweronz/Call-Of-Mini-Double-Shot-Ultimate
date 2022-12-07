using UnityEngine;

namespace Zombie3D
{
	public class RemingtonPipe : Weapon
	{
		protected float gunFireTimer = -1f;

		protected float gunFireShowTime = 0.05f;

		protected Vector3 bulletPosOffset = new Vector3(0.11f, 0.214f, 2f);

		private GameObject GunFireShadowLight;

		public RemingtonPipe()
		{
			maxCapacity = 100000000;
			maxGunLoad = 100000000;
			bulletCount = maxGunLoad;
			price = 1000;
			base.IsSelectedForBattle = false;
		}

		public override WeaponType GetWeaponType()
		{
			return WeaponType.RemingtonPipe;
		}

		public override void Init(Player owner)
		{
			base.Init(owner);
			gunfire = rightHandGun.transform.Find("gun_fire_new").gameObject;
			GunFireShadowLight = rightHandGun.transform.Find("GunFire_ShadowLight").gameObject;
			GunFireShadowLight.GetComponent<Renderer>().enabled = false;
			GunFireShadowLight.AddComponent(typeof(KeepFlat));
			m_WeaponBulletPool = new WeaponBulletsPool();
			m_WeaponBulletPool.Init("BulletPool - RemingtonPipe", gConf.weaponBullets[(int)(GetWeaponType() - 1)], 4);
			m_WeaponBulletShellsPool = new WeaponBulletsShellPool();
			m_WeaponBulletShellsPool.Init("BulletShellsPool - RemingtonPipe", gConf.BulletShell02, 1f, 4);
			m_WeaponBulletHitParticlesPool = new WeaponBulletsHitParticlePool();
			m_WeaponBulletHitParticlesPool.Init("BulletHitParticlesPool - RemingtonPipe", gConf.weaponBulletHitParticles[(int)(GetWeaponType() - 1)], 4);
			TimerManager.GetInstance().SetTimer(63, 0.1f, true);
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
			if (gunFireTimer >= 0f)
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
			gunFireTimer = 0f;
			ShowGunFire(true);
			float y = player.GetTransform().localEulerAngles.y;
			Vector3 pos = rightHandGun.transform.TransformPoint(bulletPosOffset);
			for (int i = 0; i < 4; i++)
			{
				float num = y - 15f + (float)(i * 10);
				GameObject gameObject = CreateBullet(pos, Quaternion.Euler(270f, 180f, 0f));
				if (gameObject != null)
				{
					gameObject.transform.Rotate(Vector3.forward, num);
					gameObject.transform.Translate(Vector3.up * 0.5f);
					WeaponBulletScript component = gameObject.GetComponent<WeaponBulletScript>();
					component.m_Weapon = this;
					component.AttackRange = 3f;
					component.Damage = player.Attack;
					component.Speed = 27f;
					component.Rot = num;
					component.Init();
					if (gameObject.GetComponent<Renderer>() != null && Random.Range(1, 10) < 5)
					{
						Color color = gameObject.GetComponent<Renderer>().material.GetColor("_TintColor");
						Color color2 = new Color(color.r, color.g, color.b, Random.RandomRange(0.1f, 0.3f));
						gameObject.GetComponent<Renderer>().material.SetColor("_TintColor", color2);
					}
				}
			}
			CreateBulletShell(true);
			if (GunFireShadowLight != null)
			{
				GunFireShadowLight.transform.position = new Vector3(GunFireShadowLight.transform.position.x, 10000.8f, GunFireShadowLight.transform.position.z);
				GunFireShadowLight.transform.Rotate(Vector3.forward, y);
				GunFireShadowLight.GetComponent<Renderer>().enabled = true;
				ShadowLightFlash shadowLightFlash = GunFireShadowLight.GetComponent(typeof(ShadowLightFlash)) as ShadowLightFlash;
				if (shadowLightFlash != null)
				{
					GunFireShadowLight.GetComponent<Animation>()["Alpha"].speed = 1f;
					GunFireShadowLight.GetComponent<Animation>()["Alpha"].wrapMode = WrapMode.Once;
					GunFireShadowLight.GetComponent<Animation>().Play("Alpha");
				}
			}
			if (TimerManager.GetInstance().Ready(63))
			{
				audioPlayer.PlaySound("ShootAudio", true);
				TimerManager.GetInstance().Do(63);
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
