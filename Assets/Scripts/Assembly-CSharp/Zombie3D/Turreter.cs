using UnityEngine;

namespace Zombie3D
{
	public class Turreter : Enemy
	{
		private GameObject turreterCannon;

		private bool bAttacked;

		private float turreterDeltaAngle;

		private float shellsMoveSpeed = 4f;

		private Renderer objRenderer;

		public override void Init(GameObject gObject)
		{
			base.Init(gObject);
			Transform folderTrans = enemyTransform.Find("Audio");
			base.Audio.AddAudio(folderTrans, "Dead");
			base.Audio.AddAudio(folderTrans, "Hit01");
			base.Audio.AddAudio(folderTrans, "Hit02");
			base.Audio.AddAudio(folderTrans, "Hit03");
			turreterCannon = gObject.transform.Find("Root_Bone/Pelvis/Spine/Cannon/Zombie_Turreter_Cannon_Parent/Zombie_Turreter_Cannon").gameObject;
			objRenderer = enemyObject.transform.Find("Zombie_Turreter").GetComponent<Renderer>();
			RandomRunAnimation();
			animation[runAnimationName].speed = 2f;
		}

		public override void SetBaseConfig()
		{
			base.SetBaseConfig();
			shellsMoveSpeed = 4f;
			shellsMoveSpeed += shellsMoveSpeed * GetBulletFlySpeedFac();
		}

		protected void RandomRunAnimation()
		{
			runAnimationName = "Forward01";
		}

		public override void DoLogic(float deltaTime)
		{
			base.DoLogic(deltaTime);
			if (base.HP <= 0f)
			{
				return;
			}
			if (objRenderer != null && objRenderer.isVisible)
			{
				enemyObject.transform.LookAt(player.GetTransform().position);
			}
			if (Time.time - lastAttackTime >= attackFrequency && objRenderer != null && objRenderer.isVisible)
			{
				OnAttack();
			}
			float num = 0.5f;
			if (bAttacked && Time.time - lastAttackTime >= num && turreterDeltaAngle < 30f)
			{
				turreterDeltaAngle += (attackFrequency - 0.5f) / 30f * deltaTime;
				turreterCannon.transform.Rotate(Vector3.up, (attackFrequency - num) * 30f * deltaTime);
				if (turreterDeltaAngle >= 30f)
				{
					bAttacked = false;
					turreterDeltaAngle = 0f;
				}
			}
		}

		public override void OnAttack()
		{
			if (objRenderer != null && objRenderer.isVisible)
			{
				base.OnAttack();
				Animate("Attack01", WrapMode.Loop);
				turreterCannon.GetComponent<Animation>()["Attack01"].wrapMode = WrapMode.Once;
				turreterCannon.GetComponent<Animation>().Play("Attack01");
				Shoot();
				bAttacked = true;
				lastAttackTime = Time.time;
			}
		}

		public void Shoot()
		{
			float y = turreterCannon.transform.localEulerAngles.y;
			float num = 0f;
			for (int i = 0; i < 4; i++)
			{
				num = (float)i * 90f;
				GameObject gameObject = Object.Instantiate(gConfig.turreterGunFire, turreterCannon.transform.position, Quaternion.Euler(270f, 180f, 0f)) as GameObject;
				gameObject.transform.Rotate(Vector3.up * enemyObject.transform.eulerAngles.y, Space.World);
				gameObject.transform.Rotate(Vector3.up * (y + num), Space.World);
				gameObject.transform.Translate(Vector3.up * 1.25f, Space.Self);
				RemoveTimerScript removeTimerScript = gameObject.AddComponent(typeof(RemoveTimerScript)) as RemoveTimerScript;
				removeTimerScript.life = 0.2f;
				GameObject gameObject2 = Object.Instantiate(gConfig.turreterCannonShells, turreterCannon.transform.position, Quaternion.Euler(0f, 0f, 0f)) as GameObject;
				gameObject2.transform.Rotate(Vector3.up * enemyObject.transform.eulerAngles.y, Space.World);
				gameObject2.transform.Rotate(Vector3.up * (y + num), Space.World);
				gameObject2.transform.Translate(Vector3.forward * 1.25f);
				CommonEnemyBulletScript commonEnemyBulletScript = gameObject2.AddComponent(typeof(CommonEnemyBulletScript)) as CommonEnemyBulletScript;
				commonEnemyBulletScript.Speed = shellsMoveSpeed;
				commonEnemyBulletScript.Damage = attackDamage;
				commonEnemyBulletScript.ForwardDirection = CommonEnemyBulletScript.BulletForwardDirection.forward;
				commonEnemyBulletScript.EnemyType = enemyType;
			}
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
