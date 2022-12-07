using UnityEngine;

namespace Zombie3D
{
	public class Barrett_P90 : Weapon
	{
		protected float gunFireTimer = -1f;

		protected float gunFireShowTime = 0.03f;

		protected Vector3 bulletPosOffset = new Vector3(-0.016f, 0.088f, 2.19f);

		protected bool bRightGun = true;

		protected GameObject leftHandGunfire;

		protected GameObject rightHandGunfire;

		private GameObject leftHandGunFireShadowLight;

		private GameObject rightHandGunFireShadowLight;

		public Barrett_P90()
		{
			maxCapacity = 100000000;
			maxGunLoad = 100000000;
			BulletCount = maxGunLoad;
			price = 1000;
			base.IsSelectedForBattle = false;
		}

		public override WeaponType GetWeaponType()
		{
			return WeaponType.Barrett_P90;
		}

		public override void Init(Player owner)
		{
			base.Init(owner);
			hitForce = 20f;
			leftHandGunfire = leftHandGun.transform.Find("gun_fire_new").gameObject;
			rightHandGunfire = rightHandGun.transform.Find("gun_fire_new").gameObject;
			bRightGun = false;
			ShowGunFire(false);
			bRightGun = true;
			ShowGunFire(false);
			if (GameApp.GetInstance().GetGameState().m_eGameMode.m_ePlayMode != 0)
			{
				leftHandGunFireShadowLight = leftHandGun.transform.Find("GunFire_ShadowLight").gameObject;
				leftHandGunFireShadowLight.GetComponent<Renderer>().enabled = false;
				rightHandGunFireShadowLight = rightHandGun.transform.Find("GunFire_ShadowLight").gameObject;
				rightHandGunFireShadowLight.GetComponent<Renderer>().enabled = false;
				leftHandGunFireShadowLight.AddComponent(typeof(KeepFlat));
				rightHandGunFireShadowLight.AddComponent(typeof(KeepFlat));
			}
			m_WeaponBulletPool = new WeaponBulletsPool();
			m_WeaponBulletPool.Init("BulletPool - Barrett_P90", gConf.weaponBullets[(int)(GetWeaponType() - 1)], 5);
			m_WeaponBulletShellsPool = new WeaponBulletsShellPool();
			m_WeaponBulletShellsPool.Init("BulletShellsPool - Barrett_P90", gConf.BulletShell01, 1f, 5);
			m_WeaponBulletHitParticlesPool = new WeaponBulletsHitParticlePool();
			m_WeaponBulletHitParticlesPool.Init("BulletHitParticlesPool - Barrett_P90", gConf.weaponBulletHitParticles[(int)(GetWeaponType() - 1)], 5);
			TimerManager.GetInstance().SetTimer(66, 0.1f, true);
		}

		public override void CreateGun()
		{
			leftHandGun = (GameObject)Object.Instantiate(gConf.weapons[(int)(GetWeaponType() - 1)], player.GetTransform().position, player.GetTransform().rotation);
			rightHandGun = (GameObject)Object.Instantiate(gConf.weapons[(int)(GetWeaponType() - 1)], player.GetTransform().position, player.GetTransform().rotation);
		}

		public override void BindGunAndFire()
		{
			leftHandGun.transform.parent = twoHandLeftWeaponBoneTrans;
			leftHandGun.transform.localPosition = Vector3.zero;
			leftHandGun.transform.localRotation = Quaternion.identity;
			rightHandGun.transform.parent = twoHandRightWeaponBoneTrans;
			rightHandGun.transform.localPosition = Vector3.zero;
			rightHandGun.transform.localRotation = Quaternion.identity;
		}

		public override void DoLogic(float deltaTime)
		{
			if (gunFireTimer >= 0f)
			{
				gunFireTimer += Time.deltaTime;
				if (gunFireTimer > gunFireShowTime)
				{
					ShowGunFire(false);
					if (leftHandGunFireShadowLight != null)
					{
						leftHandGunFireShadowLight.GetComponent<Renderer>().enabled = false;
					}
					if (rightHandGunFireShadowLight != null)
					{
						rightHandGunFireShadowLight.GetComponent<Renderer>().enabled = false;
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
			bRightGun = !bRightGun;
			gunFireTimer = 0f;
			float y = player.GetTransform().localEulerAngles.y;
			if (!bRightGun)
			{
				Vector3 position = leftHandGun.transform.TransformPoint(bulletPosOffset);
				GameObject gameObject = m_WeaponBulletPool.CreateBullet(position, Quaternion.Euler(270f, 180f, 0f));
				if (gameObject != null)
				{
					gameObject.transform.Rotate(Vector3.forward, y);
					WeaponBulletScript component = gameObject.GetComponent<WeaponBulletScript>();
					component.m_Weapon = this;
					component.Damage = player.Attack;
					component.Speed = 27f;
					component.Rot = y;
					component.Init();
					CreateBulletShell(false);
				}
				ShowGunFire(true);
				if (leftHandGunFireShadowLight != null)
				{
					leftHandGunFireShadowLight.transform.position = new Vector3(leftHandGunFireShadowLight.transform.position.x, 10000.5f, leftHandGunFireShadowLight.transform.position.z);
					leftHandGunFireShadowLight.transform.Rotate(Vector3.forward, y);
					leftHandGunFireShadowLight.GetComponent<Renderer>().enabled = true;
					ShadowLightFlash shadowLightFlash = leftHandGunFireShadowLight.GetComponent(typeof(ShadowLightFlash)) as ShadowLightFlash;
					if (shadowLightFlash != null)
					{
						leftHandGunFireShadowLight.GetComponent<Animation>()["Alpha"].speed = leftHandGunFireShadowLight.GetComponent<Animation>()["Alpha"].length / gunFireShowTime;
						leftHandGunFireShadowLight.GetComponent<Animation>().Play("Alpha");
					}
				}
			}
			else
			{
				Vector3 pos = rightHandGun.transform.TransformPoint(bulletPosOffset);
				GameObject gameObject2 = CreateBullet(pos, Quaternion.Euler(270f, 180f, 0f));
				if (gameObject2 != null)
				{
					gameObject2.transform.Rotate(Vector3.forward, y);
					WeaponBulletScript component2 = gameObject2.GetComponent<WeaponBulletScript>();
					component2.m_Weapon = this;
					component2.Damage = player.Attack;
					component2.Speed = 27f;
					component2.Rot = y;
					component2.Init();
					CreateBulletShell(true);
				}
				ShowGunFire(true);
				if (rightHandGunFireShadowLight != null)
				{
					rightHandGunFireShadowLight.transform.position = new Vector3(rightHandGunFireShadowLight.transform.position.x, 10000.5f, rightHandGunFireShadowLight.transform.position.z);
					rightHandGunFireShadowLight.transform.Rotate(Vector3.forward, y);
					rightHandGunFireShadowLight.GetComponent<Renderer>().enabled = true;
					ShadowLightFlash shadowLightFlash2 = rightHandGunFireShadowLight.GetComponent(typeof(ShadowLightFlash)) as ShadowLightFlash;
					if (shadowLightFlash2 != null)
					{
						rightHandGunFireShadowLight.GetComponent<Animation>()["Alpha"].speed = rightHandGunFireShadowLight.GetComponent<Animation>()["Alpha"].length / gunFireShowTime;
						rightHandGunFireShadowLight.GetComponent<Animation>().Play("Alpha");
					}
				}
			}
			if (TimerManager.GetInstance().Ready(66))
			{
				audioPlayer.PlaySound("ShootAudio", true);
				TimerManager.GetInstance().Do(66);
			}
			lastShootTime = Time.time;
		}

		public override void ShowGunFire(bool bShow)
		{
			if (bRightGun)
			{
				rightHandGunfire.GetComponent<Renderer>().enabled = bShow;
			}
			else
			{
				leftHandGunfire.GetComponent<Renderer>().enabled = bShow;
			}
		}
	}
}
