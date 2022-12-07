using UnityEngine;

namespace Zombie3D
{
	public class CannonSub : Weapon
	{
		protected float gunFireTimer = -1f;

		protected float gunFireShowTime = 0.03f;

		protected Vector3 bulletPosOffset = new Vector3(0.162f, 0.538f, 1.541f);

		protected bool bRightGun = true;

		protected GameObject leftHandGunfire;

		protected GameObject rightHandGunfire;

		private GameObject leftHandGunFireShadowLight;

		private GameObject rightHandGunFireShadowLight;

		private CannonI myCannon1;

		private CannonII myCannon2;

		protected GameObject bullet1;

		protected GameObject bullet2;

		public CannonSub()
		{
			maxCapacity = 100000000;
			maxGunLoad = 100000000;
			bulletCount = maxGunLoad;
			price = 1000;
			base.IsSelectedForBattle = false;
		}

		public override WeaponType GetWeaponType()
		{
			return WeaponType.Ion_CannonSub;
		}

		public override void Init(Player owner)
		{
			base.Init(owner);
			defaultTriggerTime = 0f;
			base.TriggerTime = defaultTriggerTime;
			leftHandGunfire = leftHandGun.transform.Find("gun_fire_new").gameObject;
			rightHandGunfire = rightHandGun.transform.Find("gun_fire_new").gameObject;
			bRightGun = false;
			ShowGunFire(false);
			bRightGun = true;
			ShowGunFire(false);
			leftHandGunFireShadowLight = leftHandGun.transform.Find("GunFire_ShadowLight").gameObject;
			leftHandGunFireShadowLight.GetComponent<Renderer>().enabled = false;
			rightHandGunFireShadowLight = rightHandGun.transform.Find("GunFire_ShadowLight").gameObject;
			rightHandGunFireShadowLight.GetComponent<Renderer>().enabled = false;
			leftHandGunFireShadowLight.AddComponent(typeof(KeepFlat));
			rightHandGunFireShadowLight.AddComponent(typeof(KeepFlat));
			m_WeaponBulletPool = new WeaponBulletsPool();
			bullet1 = gConf.weaponBullets[(int)(25)];
			bullet2 = gConf.weaponBullets[(int)(26)];
			m_WeaponBulletPool.Init("BulletPool - CannonSub1", bullet1, 3);
			m_WeaponBulletPool.Init("BulletPool - CannonSub2", bullet2, 3);
			myCannon1 = new CannonI();
			myCannon2 = new CannonII();
			myCannon1.SetOwnedPlayer(base.GetOwnedPlayer());
			myCannon2.SetOwnedPlayer(base.GetOwnedPlayer());
			TimerManager.GetInstance().SetTimer(60, 0.1f, true);
		}

		public override void CreateGun()
		{
			leftHandGun = (GameObject)Object.Instantiate(gConf.weapons[(int)(25)], player.GetTransform().position, player.GetTransform().rotation);
			rightHandGun = (GameObject)Object.Instantiate(gConf.weapons[(int)(26)], player.GetTransform().position, player.GetTransform().rotation);
		}

		public override void BindGunAndFire()
		{
			leftHandGun.transform.parent = twoHandLeftWeaponBoneTrans;
			leftHandGun.transform.localPosition = new Vector3(-0.162f, -0.213f, -0.243f);
			leftHandGun.transform.localRotation = Quaternion.identity;
			rightHandGun.transform.parent = twoHandRightWeaponBoneTrans;
			rightHandGun.transform.localPosition = new Vector3(-0.068f, -0.154f, -0.181f);
			rightHandGun.transform.localRotation = Quaternion.identity;
		}

		public override void GunOn()
		{
			base.GunOn();
			BindGunAndFire();
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
				Vector3 pos = leftHandGun.transform.TransformPoint(bulletPosOffset);
				GameObject gameObject = CreateSubBullet(pos, Quaternion.Euler(270f, 180f, 0f), bullet1);
				if (gameObject != null)
				{
					gameObject.transform.Rotate(Vector3.forward, y);
					WeaponBulletScript component = gameObject.GetComponent<WeaponBulletScript>();
					gameObject.name = gameObject.name + "SubCannon";
					component.HitAndDestroy = true;
					component.m_Weapon = myCannon1;
					component.Damage = 650f;
					component.Speed = 30f;
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
				Vector3 pos2 = rightHandGun.transform.TransformPoint(bulletPosOffset);
				GameObject gameObject2 = CreateSubBullet(pos2, Quaternion.Euler(270f, 180f, 0f), bullet2);
				if (gameObject2 != null)
				{
					gameObject2.transform.Rotate(Vector3.forward, y);
					WeaponBulletScript component2 = gameObject2.GetComponent<WeaponBulletScript>();
					gameObject2.name = gameObject2.name + "SubCannon";
					component2.HitAndDestroy = true;
					component2.m_Weapon = myCannon2;
					component2.Damage = 500f;
					component2.Speed = 30f;
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
			if (TimerManager.GetInstance().Ready(60))
			{
				audioPlayer.PlaySound("ShootAudio", true);
				TimerManager.GetInstance().Do(60);
			}
			lastShootTime = Time.time;
		}

		public override void ShowGunFire(bool bShow)
		{
			ParticleSystem[] componentsInChildren = (bRightGun) ? rightHandGunfire.GetComponentsInChildren<ParticleSystem>() : leftHandGunfire.GetComponentsInChildren<ParticleSystem>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				if (bShow)
				{
					componentsInChildren[i].Play();
				}
			}
		}
	}
}
