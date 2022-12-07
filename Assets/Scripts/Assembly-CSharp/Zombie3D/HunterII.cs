using UnityEngine;

namespace Zombie3D
{
	public class HunterII : Enemy
	{
		private float bulletFlySpeed = 3f;

		private float _attackRange = 2.857143f;

		protected string DeadAudioName = "Dead";

		protected string DeadEnvAudioName = string.Empty;

		protected string GetHitAudioName = string.Empty;

		private float _AttackAnimStartTimer = -1f;

		private float _AttackAnimStartTime = 1.2f;

		private float _HitStartTimer = -1f;

		private float _HitStartTime = 2f;

		public override void Init(GameObject gObject)
		{
			base.Init(gObject);
			Transform folderTrans = enemyTransform.Find("Audio");
			base.Audio.AddAudio(folderTrans, DeadAudioName);
			for (int i = 1; i < 4; i++)
			{
				string text = "GetHit0" + i;
				base.Audio.AddAudio(folderTrans, text);
			}
			int num = Random.Range(1, 3);
			DeadEnvAudioName = "DeadEnv0" + num;
			base.Audio.AddAudio(folderTrans, DeadEnvAudioName);
			RandomRunAnimation();
			TimerManager.GetInstance().SetTimer(21, 0.1f, true);
			TimerManager.GetInstance().SetTimer(22, 0.1f, true);
			TimerManager.GetInstance().SetTimer(22, 0.1f, true);
		}

		public override void SetBaseConfig()
		{
			base.SetBaseConfig();
			bulletFlySpeed = 3f;
			bulletFlySpeed += bulletFlySpeed * GetBulletFlySpeedFac();
		}

		protected void RandomRunAnimation()
		{
			runAnimationName = "Forward01";
		}

		public override bool AttackAnimationEnds()
		{
			if (Time.time - lastAttackTime > enemyObject.GetComponent<Animation>()["Attack01"].length)
			{
				return true;
			}
			return false;
		}

		public override void DoLogic(float deltaTime)
		{
			base.DoLogic(deltaTime);
			if (base.HP <= 0f)
			{
				return;
			}
			if (_HitStartTimer >= 0f)
			{
				_HitStartTimer += deltaTime;
			}
			if (_HitStartTimer >= _HitStartTime)
			{
				_HitStartTimer = -1f;
			}
			if (_HitStartTimer >= 0f && _HitStartTimer <= _HitStartTime)
			{
				Vector3 vector = enemyObject.transform.InverseTransformPoint(player.GetTransform().position);
				if (Mathf.Abs(vector.x) <= _attackRange && vector.z >= 0f && vector.z <= _attackRange)
				{
					HitPlayer();
				}
			}
			if (_AttackAnimStartTimer >= 0f)
			{
				_AttackAnimStartTimer += deltaTime;
			}
			if (_AttackAnimStartTimer >= _AttackAnimStartTime)
			{
				GameObject gameObject = Object.Instantiate(GameApp.GetInstance().GetGameConfig().HunterIIAttackParticles, enemyTransform.position, Quaternion.identity) as GameObject;
				Vector3 position = enemyTransform.TransformPoint(new Vector3(0f, 0f, 3f));
				gameObject.transform.position = position;
				_HitStartTimer = 0f;
				_AttackAnimStartTimer = -1f;
			}
		}

		public override void OnAttack()
		{
			base.OnAttack();
			Animate("Attack01", WrapMode.Once);
			if (_AttackAnimStartTimer == -1f)
			{
				_AttackAnimStartTimer = 0f;
			}
			enemyTransform.LookAt(target);
			lastAttackTime = Time.time;
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
			base.Audio.PlaySound(DeadEnvAudioName, true);
			base.Audio.PlaySound(DeadAudioName, true);
		}

		public void HitPlayer()
		{
			Vector3 forward = new Vector3(player.GetTransform().position.x, enemyObject.transform.position.y, player.GetTransform().position.z) - enemyObject.transform.position;
			enemyObject.transform.forward = forward;
			Animate("Attack01", WrapMode.Once);
			player.OnHit(attackDamage);
			player.SetEnemyDragSpeed(2f, -0.3f);
			Vector3 vector = new Vector3(player.GetTransform().position.x, 0f, player.GetTransform().position.z) - new Vector3(enemyObject.transform.position.x, 0f, enemyObject.transform.position.z);
			player.OnHitBack(0.3f, 1.5f, vector);
		}
	}
}
