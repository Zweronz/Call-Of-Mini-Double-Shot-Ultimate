using System.Collections.Generic;
using UnityEngine;

namespace Zombie3D
{
	public class Swat : Enemy
	{
		private List<GameObject> bullets;

		private GameObject objGunFire;

		private Vector3 bulletPosOffset = new Vector3(-0.12f, -0.048f, -0.271f);

		private Vector3 gunFirePos = new Vector3(0.335f, 1.039f, 1f);

		private float shellsMoveSpeed = 3.8f;

		private float gunFireTime;

		private float PullTriggerTime = -1f;

		protected string DeadAudioName = string.Empty;

		protected string DeadEnvAudioName = string.Empty;

		protected string GetHitAudioName = string.Empty;

		public override void Init(GameObject gObject)
		{
			base.Init(gObject);
			Transform folderTrans = enemyTransform.Find("Audio");
			int num = Random.Range(1, 5);
			DeadAudioName = "Dead" + num;
			base.Audio.AddAudio(folderTrans, DeadAudioName);
			num = Random.Range(1, 3);
			DeadEnvAudioName = "DeadEnv0" + num;
			base.Audio.AddAudio(folderTrans, DeadEnvAudioName);
			for (int i = 1; i < 4; i++)
			{
				string text = "GetHit0" + i;
				base.Audio.AddAudio(folderTrans, text);
			}
			bullets = new List<GameObject>();
			for (int j = 0; j < 6; j++)
			{
				GameObject item = null;
				bullets.Add(item);
			}
			objGunFire = Object.Instantiate(gConfig.swatGunFire, Vector3.zero, Quaternion.identity) as GameObject;
			objGunFire.SetActiveRecursively(false);
			RandomRunAnimation();
			TimerManager.GetInstance().SetTimer(21, 0.1f, true);
			TimerManager.GetInstance().SetTimer(22, 0.1f, true);
			TimerManager.GetInstance().SetTimer(22, 0.1f, true);
		}

		public override void SetBaseConfig()
		{
			base.SetBaseConfig();
			shellsMoveSpeed = 3.8f;
			shellsMoveSpeed += shellsMoveSpeed * GetBulletFlySpeedFac();
		}

		public override bool CouldEnterAttackState()
		{
			if (base.CouldEnterAttackState())
			{
				if (enemyTransform.position.y < 10000.199f)
				{
					return true;
				}
				return false;
			}
			return false;
		}

		protected void RandomRunAnimation()
		{
			runAnimationName = "Run01";
			int num = Random.Range(0, 10);
			if (num < 5)
			{
				runAnimationName = "Run01";
			}
			else
			{
				runAnimationName = "Forward01";
			}
		}

		public override bool AttackAnimationEnds()
		{
			if (Time.time - lastAttackTime > enemyObject.GetComponent<Animation>()["Fire01"].length)
			{
				return true;
			}
			return false;
		}

		public override void DoLogic(float deltaTime)
		{
			base.DoLogic(deltaTime);
			if (!(base.HP <= 0f))
			{
				if (objGunFire != null && objGunFire.active && Time.time - gunFireTime >= 0.1f)
				{
					objGunFire.SetActiveRecursively(false);
				}
				if (PullTriggerTime >= 0f && Time.time - PullTriggerTime >= 0.35f)
				{
					Shoot();
					PullTriggerTime = -1f;
				}
			}
		}

		public override void OnAttack()
		{
			base.OnAttack();
			Animate("Fire01", WrapMode.Once);
			PullTrigger();
			lastAttackTime = Time.time;
		}

		public void PullTrigger()
		{
			PullTriggerTime = Time.time;
		}

		public void Shoot()
		{
			if (objGunFire != null)
			{
				objGunFire.SetActiveRecursively(true);
				objGunFire.transform.position = enemyObject.transform.TransformPoint(gunFirePos);
				objGunFire.transform.up = enemyObject.transform.forward;
				gunFireTime = Time.time;
			}
			enemyTransform.LookAt(target);
			for (int i = 0; i < bullets.Count; i++)
			{
				if (bullets[i] == null)
				{
					Transform transform = enemyObject.transform.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Neck/Bip01 R Clavicle/Bip01 R UpperArm/Bip01 R Forearm/Bip01 R Hand/Weapon_Dummy");
					Vector3 position = transform.TransformPoint(bulletPosOffset);
					GameObject gameObject = Object.Instantiate(gConfig.swatBullet, position, Quaternion.Euler(270f, 180f, 0f)) as GameObject;
					float y = enemyObject.transform.localEulerAngles.y;
					gameObject.transform.Rotate(Vector3.forward, y);
					CommonEnemyBulletScript commonEnemyBulletScript = gameObject.AddComponent(typeof(CommonEnemyBulletScript)) as CommonEnemyBulletScript;
					commonEnemyBulletScript.Speed = shellsMoveSpeed;
					commonEnemyBulletScript.Damage = attackDamage;
					commonEnemyBulletScript.ForwardDirection = CommonEnemyBulletScript.BulletForwardDirection.up;
					break;
				}
			}
		}

		public override void OnHit(DamageProperty dp, WeaponType weaponType)
		{
			base.OnHit(dp, weaponType);
			if (weaponType == WeaponType.Hellfire || !(base.HP > 0f))
			{
				return;
			}
			int num = Random.Range(1, 4);
			string text = "GetHit0" + num;
			switch (num)
			{
			case 1:
				if (TimerManager.GetInstance().Ready(21))
				{
					base.Audio.PlaySound(text, true);
					TimerManager.GetInstance().Do(21);
				}
				break;
			case 2:
				if (TimerManager.GetInstance().Ready(22))
				{
					base.Audio.PlaySound(text, true);
					TimerManager.GetInstance().Do(22);
				}
				break;
			case 3:
				if (TimerManager.GetInstance().Ready(22))
				{
					base.Audio.PlaySound(text, true);
					TimerManager.GetInstance().Do(22);
				}
				break;
			}
		}

		public override void OnDead()
		{
			base.OnDead();
			if (objGunFire != null)
			{
				Object.Destroy(objGunFire);
				objGunFire = null;
			}
			base.Audio.PlaySound(DeadEnvAudioName, true);
			base.Audio.PlaySound(DeadAudioName, true);
		}
	}
}
