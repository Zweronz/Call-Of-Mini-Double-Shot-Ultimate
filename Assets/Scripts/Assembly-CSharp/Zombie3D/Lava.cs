using UnityEngine;

namespace Zombie3D
{
	public class Lava : Enemy
	{
		private LavaAttackState LAVA_ATTACK_STATE = new LavaAttackState();

		private GameObject fireObj;

		private float attackBeginTime;

		private float attackHpTimer;

		private bool isAttacking;

		private bool isPreAttack;

		private float preAttackBeginTime;

		private float fireAttackRange = 5.5f;

		private float fireOnceTime = 1f;

		private float preAnimationSpeed = 0.5f;

		public override void Init(GameObject gObject)
		{
			base.Init(gObject);
			Transform folderTrans = enemyTransform.Find("Audio");
			base.Audio.AddAudio(folderTrans, "Dead");
			base.Audio.AddAudio(folderTrans, "Hit01");
			base.Audio.AddAudio(folderTrans, "Hit02");
			base.Audio.AddAudio(folderTrans, "Hit03");
			fireObj = gObject.transform.Find("LavaFireEffect").gameObject;
			fireObj.GetComponent<ParticleEmitter>().emit = false;
			RandomRunAnimation();
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
			if (player.HP <= 0f)
			{
				Animate("Idle01", WrapMode.Loop);
				fireObj.GetComponent<ParticleEmitter>().emit = false;
				fireObj.GetComponent<ParticleEmitter>().ClearParticles();
				return;
			}
			if (isPreAttack && Time.time - preAttackBeginTime >= enemyObject.GetComponent<Animation>()["AttackIdle01"].length / preAnimationSpeed)
			{
				isPreAttack = false;
				Attack();
			}
			if (isAttacking)
			{
				attackHpTimer += deltaTime;
				if (attackHpTimer >= 0.2f && base.SqrDistanceFromPlayer <= fireAttackRange * fireAttackRange)
				{
					CheckPlayersHit(attackDamage);
					attackHpTimer = 0f;
				}
				if (Time.time - attackBeginTime >= fireOnceTime)
				{
					isAttacking = false;
					fireObj.GetComponent<ParticleEmitter>().emit = false;
					SetState(Enemy.IDLE_STATE);
				}
			}
		}

		public override void OnAttack()
		{
			SetState(LAVA_ATTACK_STATE);
			if (!isAttacking)
			{
				lastAttackTime = Time.time;
				PreAttack();
			}
		}

		public void PreAttack()
		{
			isPreAttack = true;
			preAttackBeginTime = Time.time;
			enemyObject.transform.LookAt(player.GetTransform().position);
			enemyObject.GetComponent<Animation>()["AttackIdle01"].speed = preAnimationSpeed;
			Animate("AttackIdle01", WrapMode.Once);
		}

		public void Attack()
		{
			Debug.Log("~~~Attack time - " + Time.time);
			base.OnAttack();
			CheckPlayersHit(attackDamage);
			Animate("Attack01", WrapMode.Loop);
			fireObj.GetComponent<ParticleEmitter>().emit = true;
			fireObj.GetComponent<ParticleEmitter>().ClearParticles();
			fireObj.GetComponent<ParticleEmitter>().Emit();
			attackBeginTime = Time.time;
			attackHpTimer = 0f;
			isAttacking = true;
		}

		public void AfterAttack()
		{
		}

		private void CheckPlayersHit(float damage)
		{
			Vector3 vector = enemyObject.transform.InverseTransformPoint(player.PlayerObject.transform.position);
			if (Mathf.Abs(vector.x) <= 0.6f && vector.z * enemyObject.transform.localScale.z >= 0.3f && vector.z * enemyObject.transform.localScale.z <= fireAttackRange)
			{
				Debug.Log("Lave Damage - " + damage + "|" + player.HP + "|" + Time.time + "|" + Time.frameCount);
				player.OnHit(damage);
			}
			Vector3 vector2 = enemyObject.transform.InverseTransformPoint(FriendPlayer.GetTransform().position);
			if (Mathf.Abs(vector2.x) <= 0.6f && vector2.z * enemyObject.transform.localScale.z >= 0.3f && vector2.z * enemyObject.transform.localScale.z <= fireAttackRange)
			{
				FriendPlayer.OnHit(damage);
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
			if (fireObj != null)
			{
				fireObj.GetComponent<ParticleEmitter>().emit = false;
			}
			base.Audio.PlaySound("Dead", true);
		}
	}
}
