using UnityEngine;

namespace Zombie3D
{
	public class MassacreCannon : Weapon
	{
		protected float gunFireTimer = -1f;

		protected float gunFireShowTime = 0.03f;

		protected GameObject gunfireFlash;

		protected float gunAnimTimer = -1f;

		protected Vector3 bulletPosOffset = new Vector3(0.107f, 0.412f, 2.957f);

		private GameObject GunFireShadowLight;

		public MassacreCannon()
		{
			maxCapacity = 100000000;
			maxGunLoad = 100000000;
			BulletCount = maxGunLoad;
			price = 1000;
			base.IsSelectedForBattle = false;
		}

		public override WeaponType GetWeaponType()
		{
			return WeaponType.MassacreCannon;
		}

		public override void Init(Player owner)
		{
			base.Init(owner);
			gunfire = rightHandGun.transform.Find("gun_fire_new").gameObject;
			ShowGunFire(false);
			gunfireFlash = rightHandGun.transform.Find("MassacreCannonFlash").gameObject;
			gunfireFlash.GetComponent<ParticleEmitter>().emit = false;
			GunFireShadowLight = rightHandGun.transform.Find("GunFire_ShadowLight").gameObject;
			GunFireShadowLight.GetComponent<Renderer>().enabled = false;
			GunFireShadowLight.AddComponent(typeof(KeepFlat));
			m_WeaponBulletPool = new WeaponBulletsPool();
			m_WeaponBulletPool.Init("BulletPool - MassacreCannon", gConf.weaponBullets[(int)(GetWeaponType() - 1)], 30);
			m_WeaponBulletShellsPool = new WeaponBulletsShellPool();
			m_WeaponBulletShellsPool.Init("BulletShellsPool - Lightning", gConf.BulletShell03, 1f, 10);
			m_WeaponBulletHitParticlesPool = new WeaponBulletsHitParticlePool();
			m_WeaponBulletHitParticlesPool.Init("BulletHitParticlesPool - MassacreCannon", gConf.weaponBulletHitParticles[(int)(GetWeaponType() - 1)], 30);
			TimerManager.GetInstance().SetTimer(80, 0.1f, true);
		}

		public override void CreateGun()
		{
			rightHandGun = (GameObject)Object.Instantiate(gConf.weapons[(int)(GetWeaponType() - 1)], player.GetTransform().position, player.GetTransform().rotation);
		}

		public override void GunOn()
		{
			base.GunOn();
			rightHandGun.GetComponent<Animation>()[rightHandGun.GetComponent<Animation>().clip.name].wrapMode = WrapMode.Loop;
			gunfireFlash.GetComponent<ParticleEmitter>().emit = true;
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
			if (gunAnimTimer >= 0f)
			{
				gunAnimTimer += deltaTime;
				if (gunAnimTimer > 2f)
				{
					rightHandGun.GetComponent<Animation>().Stop(rightHandGun.GetComponent<Animation>().clip.name);
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
			gunAnimTimer = 0f;
			rightHandGun.GetComponent<Animation>().CrossFade(rightHandGun.GetComponent<Animation>().clip.name);
			float y = player.GetTransform().localEulerAngles.y;
			Vector3 pos = rightHandGun.transform.TransformPoint(bulletPosOffset);
			GameObject gameObject = CreateBullet(pos, Quaternion.Euler(270f, 180f, 0f));
			if (gameObject != null)
			{
				gameObject.transform.Rotate(Vector3.forward, y);
				float num = 1.8f;
				gameObject.transform.localScale = new Vector3(0.08030832f * num, 0.1348395f * num, 0.4631814f * num);
				WeaponBulletScript component = gameObject.GetComponent<WeaponBulletScript>();
				component.m_Weapon = this;
				component.Damage = player.Attack;
				component.Speed = 12f;
				component.Rot = y;
				component.isMassacreCannonSubBullet = false;
				component.Init();
				component.flashFrameTime = 0.2f;
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
			if (TimerManager.GetInstance().Ready(80))
			{
				audioPlayer.PlaySound("ShootAudio", true);
				TimerManager.GetInstance().Do(80);
			}
			lastShootTime = Time.time;
		}

		public override void ShowGunFire(bool bShow)
		{
			gunfire.GetComponent<Renderer>().enabled = bShow;
		}
	}
}
