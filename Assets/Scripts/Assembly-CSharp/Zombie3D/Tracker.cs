using UnityEngine;

namespace Zombie3D
{
	public class Tracker : Enemy
	{
		private bool bIsPrepareAttacking;

		private bool bIsAttacking;

		private float shellsMoveSpeed = 4.2f;

		private float shellsMaxDistance = 20f;

		protected int OneAttackShootTimes;

		protected float OneAttackShootInterval = 1.5f;

		protected float shootBeginTime;

		protected float shootAnimPrepareTime;

		protected bool bShooted;

		public override void Init(GameObject gObject)
		{
			base.Init(gObject);
			Transform folderTrans = enemyTransform.Find("Audio");
			base.Audio.AddAudio(folderTrans, "Dead");
			base.Audio.AddAudio(folderTrans, "Hit01");
			base.Audio.AddAudio(folderTrans, "Hit02");
			base.Audio.AddAudio(folderTrans, "Hit03");
			runAnimationName = "Forward01";
			shootAnimPrepareTime = animation["Fire01"].clip.length * 0.75f;
		}

		public override void SetBaseConfig()
		{
			base.SetBaseConfig();
			shellsMoveSpeed = 4.2f;
			shellsMoveSpeed += shellsMoveSpeed * GetBulletFlySpeedFac();
		}

		public override void DoLogic(float deltaTime)
		{
			base.DoLogic(deltaTime);
			if (base.HP <= 0f)
			{
				return;
			}
			if (bIsPrepareAttacking && !animation.IsPlaying("PrepareFire01"))
			{
				bIsPrepareAttacking = false;
				bIsAttacking = true;
				OneAttackShootTimes = 3;
			}
			if (!bIsAttacking)
			{
				return;
			}
			if (OneAttackShootTimes > 0)
			{
				if (Time.time - shootBeginTime >= OneAttackShootInterval)
				{
					if (OneAttackShootTimes == 3)
					{
						enemyObject.transform.LookAt(player.GetTransform().position);
					}
					Animate("Fire01", WrapMode.Loop);
					shootBeginTime = Time.time;
					bShooted = false;
				}
				else if (Time.time - shootBeginTime >= animation["Fire01"].clip.length && !animation.IsPlaying("Idle02"))
				{
					Animate("Idle02", WrapMode.Loop);
				}
				else if (Time.time - shootBeginTime >= shootAnimPrepareTime && !bShooted)
				{
					Shoot();
					OneAttackShootTimes--;
					bShooted = true;
				}
			}
			else
			{
				bIsAttacking = false;
				string animationName = "Idle02";
				int num = Random.Range(0, 10);
				if (num < 5)
				{
					animationName = "Idle02";
				}
				Animate(animationName, WrapMode.Loop);
			}
		}

		public override void OnAttack()
		{
			base.OnAttack();
			Animate("ReadyFire01", WrapMode.Once);
			bIsPrepareAttacking = true;
			bIsAttacking = false;
			lastAttackTime = Time.time;
		}

		public void Shoot()
		{
			float num = 0f;
			GameObject gameObject = Object.Instantiate(gConfig.trackerGunFire, enemyObject.transform.position, Quaternion.Euler(270f, 180f, 0f)) as GameObject;
			gameObject.transform.Rotate(Vector3.up * enemyObject.transform.eulerAngles.y, Space.World);
			gameObject.transform.Rotate(Vector3.up * num, Space.World);
			gameObject.transform.Translate(0f, 2.45f, 1.75f, Space.Self);
			Vector3 position = new Vector3(enemyObject.transform.position.x, 10001.8f, enemyObject.transform.position.z);
			position += 1f * enemyObject.transform.forward;
			GameObject gameObject2 = Object.Instantiate(gConfig.trackerShells, position, Quaternion.Euler(0f, 0f, 0f)) as GameObject;
			gameObject2.transform.Rotate(Vector3.up * enemyObject.transform.eulerAngles.y, Space.World);
			gameObject2.transform.Rotate(Vector3.up * num, Space.World);
			gameObject2.transform.Translate(Vector3.forward * 1f);
			CommonEnemyBulletScript commonEnemyBulletScript = gameObject2.AddComponent(typeof(CommonEnemyBulletScript)) as CommonEnemyBulletScript;
			commonEnemyBulletScript.Speed = shellsMoveSpeed;
			commonEnemyBulletScript.Damage = attackDamage;
			commonEnemyBulletScript.ForwardDirection = CommonEnemyBulletScript.BulletForwardDirection.forward;
			commonEnemyBulletScript.EnemyType = enemyType;
		}

		public override bool AttackAnimationEnds()
		{
			return !bIsAttacking;
		}

		public override void OnHit(DamageProperty dp, WeaponType weaponType)
		{
			base.OnHit(dp, weaponType);
			if (weaponType != WeaponType.Hellfire && base.HP > 0f)
			{
				switch (Random.Range(0, 3))
				{
				case 0:
					base.Audio.PlaySound("Hit01", true);
					break;
				case 1:
					base.Audio.PlaySound("Hit02", true);
					break;
				case 2:
					base.Audio.PlaySound("Hit03", true);
					break;
				}
			}
		}

		public override void OnDead()
		{
			base.OnDead();
			base.Audio.PlaySound("Dead", true);
		}
	}
}
