using System;
using System.Collections;
using UnityEngine;

namespace Zombie3D
{
	public class Lightning : Weapon
	{
		protected float gunFireTimer = -1f;

		protected float gunFireShowTime = 0.03f;

		protected Vector3 bulletPosOffset = new Vector3(0f, 0.345f, 1.412f);

		protected bool bRightGun = true;

		protected GameObject leftHandGunfire;

		protected GameObject rightHandGunfire;

		private GameObject leftHandGunFireShadowLight;

		private GameObject rightHandGunFireShadowLight;

		public Lightning()
		{
			maxCapacity = 100000000;
			maxGunLoad = 100000000;
			BulletCount = maxGunLoad;
			price = 1000;
			base.IsSelectedForBattle = false;
		}

		public override WeaponType GetWeaponType()
		{
			return WeaponType.Lightning;
		}

		public override void Init(Player owner)
		{
			base.Init(owner);
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
			m_WeaponBulletPool.Init("BulletPool - Lightning", gConf.weaponBullets[(int)(GetWeaponType() - 1)], 10);
			m_WeaponBulletShellsPool = new WeaponBulletsShellPool();
			m_WeaponBulletShellsPool.Init("BulletShellsPool - Lightning", gConf.BulletShell03, 0.5f, 10);
			m_WeaponBulletHitParticlesPool = new WeaponBulletsHitParticlePool();
			m_WeaponBulletHitParticlesPool.Init("BulletHitParticlesPool - Lightning", gConf.weaponBulletHitParticles[(int)(GetWeaponType() - 1)], 10);
			TimerManager.GetInstance().SetTimer(79, 0.1f, true);
		}

		public override void CreateGun()
		{
			leftHandGun = (GameObject)UnityEngine.Object.Instantiate(gConf.weapons[(int)(GetWeaponType() - 1)], player.GetTransform().position, player.GetTransform().rotation);
			rightHandGun = (GameObject)UnityEngine.Object.Instantiate(gConf.weapons[(int)(GetWeaponType() - 1)], player.GetTransform().position, player.GetTransform().rotation);
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
			float num = 100f;
			float num2 = 0f;
			Enemy enemy = null;
			Hashtable enemies = GameApp.GetInstance().GetGameScene().GetEnemies();
			foreach (Enemy value in enemies.Values)
			{
				if (!(value.HP <= 0f))
				{
					Vector3 vector = player.GetTransform().InverseTransformPoint(value.GetPosition());
					num2 = ((!(vector.z >= 0f)) ? ((Mathf.Atan2(vector.z, vector.x) + (float)Math.PI * 2f) / (float)Math.PI * 180f) : (Mathf.Atan2(vector.z, vector.x) / (float)Math.PI * 180f));
					if (num2 >= 60f && num2 <= 120f && num > value.SqrDistanceFromPlayer && value.SqrDistanceFromPlayer > 2f)
					{
						num = value.SqrDistanceFromPlayer;
						enemy = value;
					}
				}
			}
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
					component.Speed = 25f;
					component.Rot = y;
					component.Init();
					if (enemy != null)
					{
						component.AutoTraceEnemy = enemy;
					}
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
				GameObject gameObject2 = CreateBullet(pos2, Quaternion.Euler(270f, 180f, 0f));
				if (gameObject2 != null)
				{
					gameObject2.transform.Rotate(Vector3.forward, y);
					WeaponBulletScript component2 = gameObject2.GetComponent<WeaponBulletScript>();
					component2.m_Weapon = this;
					component2.Damage = player.Attack;
					component2.Speed = 25f;
					component2.Rot = y;
					component2.Init();
					if (enemy != null)
					{
						component2.AutoTraceEnemy = enemy;
					}
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
			if (TimerManager.GetInstance().Ready(79))
			{
				audioPlayer.PlaySound("ShootAudio", true);
				TimerManager.GetInstance().Do(79);
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
