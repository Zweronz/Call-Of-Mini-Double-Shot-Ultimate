using UnityEngine;

namespace Zombie3D
{
	public class Nailer : Weapon
	{
		protected float gunFireTimer = -1f;

		protected float gunFireShowTime = 0.03f;

		protected Vector3 bulletPosOffset = new Vector3(0f, 0.352f, 1.41f);

		protected bool bRightGun = true;

		protected GameObject leftHandGunfire;

		protected GameObject rightHandGunfire;

		private GameObject leftHandGunFireShadowLight;

		private GameObject rightHandGunFireShadowLight;

		public Nailer()
		{
			maxCapacity = 100000000;
			maxGunLoad = 100000000;
			BulletCount = maxGunLoad;
			price = 1000;
			base.IsSelectedForBattle = false;
		}

		public override WeaponType GetWeaponType()
		{
			return WeaponType.Nailer;
		}

		public override void Init(Player owner)
		{
			base.Init(owner);
			base.TriggerTime = 0f;
			hitForce = 20f;
			leftHandGunfire = leftHandGun.transform.Find("gun_fire_new").gameObject;
			rightHandGunfire = rightHandGun.transform.Find("gun_fire_new").gameObject;
			bRightGun = false;
			ShowGunFire(false);
			bRightGun = true;
			ShowGunFire(false);
			m_WeaponBulletPool = new WeaponBulletsPool();
			m_WeaponBulletPool.Init("BulletPool - Nailer", gConf.weaponBullets[(int)(GetWeaponType() - 1)], 6);
			m_WeaponBulletHitParticlesPool = new WeaponBulletsHitParticlePool();
			m_WeaponBulletHitParticlesPool.Init("BulletHitParticlesPool - Nailer", gConf.weaponBulletHitParticles[(int)(GetWeaponType() - 1)], 6);
			TimerManager.GetInstance().SetTimer(75, 0.1f, true);
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

		public override void GunOn()
		{
			base.GunOn();
			bRightGun = false;
			ShowGunFire(false);
			bRightGun = true;
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
			bRightGun = !bRightGun;
			gunFireTimer = 0f;
			float y = player.GetTransform().localEulerAngles.y;
			if (!bRightGun)
			{
				Vector3 pos = leftHandGun.transform.TransformPoint(bulletPosOffset);
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
				Vector3 pos2 = rightHandGun.transform.TransformPoint(bulletPosOffset);
				GameObject gameObject2 = CreateBullet(pos2, Quaternion.Euler(270f, 180f, 0f));
				if (gameObject2 != null)
				{
					gameObject2.transform.Rotate(Vector3.forward, y);
					WeaponBulletScript component2 = gameObject2.GetComponent<WeaponBulletScript>();
					component2.m_Weapon = this;
					component2.Damage = player.Attack;
					component2.Speed = 27f;
					component2.Rot = y;
					component2.Init();
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
			if (TimerManager.GetInstance().Ready(75))
			{
				audioPlayer.PlaySound("ShootAudio", true);
				TimerManager.GetInstance().Do(75);
			}
			lastShootTime = Time.time;
		}

		public override void ShowGunFire(bool bShow)
		{
			if (bRightGun)
			{
				if (!(rightHandGunfire != null))
				{
					return;
				}
				ParticleEmitter[] componentsInChildren = rightHandGunfire.GetComponentsInChildren<ParticleEmitter>();
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					componentsInChildren[i].emit = bShow;
					if (bShow)
					{
						componentsInChildren[i].Emit();
					}
				}
			}
			else
			{
				if (!(leftHandGunfire != null))
				{
					return;
				}
				ParticleEmitter[] componentsInChildren2 = leftHandGunfire.GetComponentsInChildren<ParticleEmitter>();
				for (int j = 0; j < componentsInChildren2.Length; j++)
				{
					componentsInChildren2[j].emit = bShow;
					if (bShow)
					{
						componentsInChildren2[j].Emit();
					}
				}
			}
		}
	}
}
