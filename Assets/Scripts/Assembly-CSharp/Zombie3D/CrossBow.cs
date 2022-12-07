using UnityEngine;

namespace Zombie3D
{
	public class CrossBow : Weapon
	{
		protected float gunFireTimer = -1f;

		protected float gunFireShowTime = 0.03f;

		protected Vector3 bulletPosOffset = new Vector3(0f, 0.2669076f, 0.9085897f);

		protected bool bRightGun = true;

		protected GameObject leftHandGunfire;

		protected GameObject rightHandGunfire;

		private GameObject leftHandGunFireShadowLight;

		private GameObject rightHandGunFireShadowLight;

		public CrossBow()
		{
			maxCapacity = 100000000;
			maxGunLoad = 100000000;
			BulletCount = maxGunLoad;
			price = 1000;
			base.IsSelectedForBattle = false;
		}

		public override WeaponType GetWeaponType()
		{
			return WeaponType.CrossBow;
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
			leftHandGunFireShadowLight = leftHandGun.transform.Find("GunFire_ShadowLight").gameObject;
			leftHandGunFireShadowLight.GetComponent<Renderer>().enabled = false;
			rightHandGunFireShadowLight = rightHandGun.transform.Find("GunFire_ShadowLight").gameObject;
			rightHandGunFireShadowLight.GetComponent<Renderer>().enabled = false;
			leftHandGunFireShadowLight.AddComponent(typeof(KeepFlat));
			rightHandGunFireShadowLight.AddComponent(typeof(KeepFlat));
			m_WeaponBulletPool = new WeaponBulletsPool();
			m_WeaponBulletPool.Init("BulletPool - CrossBow", gConf.weaponBullets[(int)(GetWeaponType() - 1)], 4);
			m_WeaponBulletShellsPool = new WeaponBulletsShellPool();
			m_WeaponBulletShellsPool.Init("BulletShellsPool - CrossBow", gConf.BulletShell01, 1f, 4);
			m_WeaponBulletHitParticlesPool = new WeaponBulletsHitParticlePool();
			m_WeaponBulletHitParticlesPool.Init("BulletHitParticlesPool - CrossBow", gConf.weaponBulletHitParticles[(int)(GetWeaponType() - 1)], 4);
			TimerManager.GetInstance().SetTimer(60, 0.1f, true);
		}

		public override void CreateGun()
		{
			leftHandGun = (GameObject)Object.Instantiate(gConf.weapons[(int)(GetWeaponType() - 1)], player.GetTransform().position, player.GetTransform().rotation);
			rightHandGun = (GameObject)Object.Instantiate(gConf.weapons[(int)(GetWeaponType() - 1)], player.GetTransform().position, player.GetTransform().rotation);
		}

		public override void BindGunAndFire()
		{
			Debug.Log(leftHandGun.transform.root.name + " | " + twoHandLeftWeaponBoneTrans.name);
			leftHandGun.transform.parent = twoHandLeftWeaponBoneTrans;
			Debug.Log(leftHandGun.transform.root.name);
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
				Vector3 pos = leftHandGun.transform.TransformPoint(bulletPosOffset);
				GameObject gameObject = CreateBullet(pos, Quaternion.Euler(270f, 180f, 0f));
				GameObject gameObject2 = CreateBullet(pos, Quaternion.Euler(270f, 180f, 0f));
				if (gameObject != null)
				{
					gameObject.transform.Rotate(Vector3.forward, y - 15f);
					gameObject.transform.Translate(Vector3.up * 2f, Space.Self);
					WeaponBulletScript component = gameObject.GetComponent<WeaponBulletScript>();
					component.m_Weapon = this;
					component.Damage = player.Attack;
					component.Speed = 15f;
					component.Rot = y;
					component.Init();
					gameObject2.transform.Rotate(Vector3.forward, y + 15f);
					gameObject2.transform.Translate(Vector3.up * 2f, Space.Self);
					WeaponBulletScript component2 = gameObject2.GetComponent<WeaponBulletScript>();
					component2.m_Weapon = this;
					component2.Damage = player.Attack;
					component2.Speed = 15f;
					component2.Rot = y;
					component2.Init();
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
				GameObject gameObject3 = CreateBullet(pos2, Quaternion.Euler(270f, 180f, 0f));
				GameObject gameObject4 = CreateBullet(pos2, Quaternion.Euler(270f, 180f, 0f));
				if (gameObject3 != null)
				{
					gameObject3.transform.Rotate(Vector3.forward, y - 15f);
					gameObject3.transform.Translate(Vector3.up, Space.Self);
					WeaponBulletScript component3 = gameObject3.GetComponent<WeaponBulletScript>();
					component3.m_Weapon = this;
					component3.Damage = player.Attack;
					component3.Speed = 15f;
					component3.Rot = y;
					component3.Init();
					gameObject4.transform.Rotate(Vector3.forward, y + 15f);
					gameObject4.transform.Translate(Vector3.up, Space.Self);
					WeaponBulletScript component4 = gameObject4.GetComponent<WeaponBulletScript>();
					component4.m_Weapon = this;
					component4.Damage = player.Attack;
					component4.Speed = 15f;
					component4.Rot = y;
					component4.Init();
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
			if (!bRightGun)
			{
				ParticleEmitter[] componentsInChildren = leftHandGunfire.GetComponentsInChildren<ParticleEmitter>();
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					componentsInChildren[i].emit = bShow;
					if (bShow)
					{
						componentsInChildren[i].Emit();
					}
				}
				return;
			}
			ParticleEmitter[] componentsInChildren2 = rightHandGunfire.GetComponentsInChildren<ParticleEmitter>();
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
