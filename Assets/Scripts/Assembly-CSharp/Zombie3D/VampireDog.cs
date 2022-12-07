using UnityEngine;

namespace Zombie3D
{
	public class VampireDog : Enemy
	{
		public float m_IsJumpOnPlayer;

		private float _fBeginJumpOnDir = 2.5f;

		protected float PullTriggerTimer = -1f;

		public float GetBeginJumpOnDir()
		{
			return _fBeginJumpOnDir;
		}

		public override void Init(GameObject gObject)
		{
			base.Init(gObject);
			m_IsJumpOnPlayer = -1f;
		}

		public override void DoLogic(float deltaTime)
		{
			base.DoLogic(deltaTime);
			if (m_IsJumpOnPlayer >= 0f && AttackAnimationEnds())
			{
				m_IsJumpOnPlayer = -1f;
			}
			if (!(PullTriggerTimer >= 0f))
			{
				return;
			}
			PullTriggerTimer += deltaTime;
			if (PullTriggerTimer >= 0.4f)
			{
				if (base.SqrDistanceFromPlayer < base.AttackRange * base.AttackRange)
				{
					HitPlayer();
				}
				PullTriggerTimer = -1f;
			}
		}

		public override void OnAttack()
		{
			base.OnAttack();
			Animate("Attack01", WrapMode.Once);
			PullTrigger();
			lastAttackTime = Time.time;
			m_IsJumpOnPlayer = 0f;
		}

		public override void OnHit(DamageProperty dp, WeaponType weaponType)
		{
			base.OnHit(dp, weaponType);
		}

		public override void OnDead()
		{
			base.OnDead();
		}

		public void PullTrigger()
		{
			PullTriggerTimer = 0f;
		}

		public void HitPlayer()
		{
			Vector3 forward = new Vector3(player.GetTransform().position.x, enemyObject.transform.position.y, player.GetTransform().position.z) - enemyObject.transform.position;
			enemyObject.transform.forward = forward;
			player.OnHit(attackDamage);
			Vector3 vector = new Vector3(player.GetTransform().position.x, 0f, player.GetTransform().position.z) - new Vector3(enemyObject.transform.position.x, 0f, enemyObject.transform.position.z);
			player.OnHitBack(0.3f, 1.5f, vector);
			string text = "Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Neck/Bip01 R Clavicle/Bip01 R UpperArm/Bip01 R Forearm/Bip01 R Hand";
			Vector3 position = enemyObject.transform.Find(text).position;
			GameObject gameObject = Object.Instantiate(GameApp.GetInstance().GetGameConfig().hitParticles01, position, Quaternion.identity) as GameObject;
			RemoveTimerScript removeTimerScript = gameObject.AddComponent(typeof(RemoveTimerScript)) as RemoveTimerScript;
			removeTimerScript.life = 0.2f;
		}
	}
}
