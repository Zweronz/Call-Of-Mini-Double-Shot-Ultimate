using UnityEngine;

namespace Zombie3D
{
	public class Infecter : Enemy
	{
		private GameObject bullets;

		private Vector3 bulletsBeginPos;

		private float bulletFlySpeed = 3.8f;

		protected string GetHitAudioName = string.Empty;

		private float PullTriggerTime = -1f;

		public override void Init(GameObject gObject)
		{
			base.Init(gObject);
			Transform folderTrans = enemyTransform.Find("Audio");
			base.Audio.AddAudio(folderTrans, "DeadEnv");
			base.Audio.AddAudio(folderTrans, "Dead");
			for (int i = 1; i < 4; i++)
			{
				string text = "GetHit0" + i;
				base.Audio.AddAudio(folderTrans, text);
			}
			RandomRunAnimation();
			TimerManager.GetInstance().SetTimer(21, 0.1f, true);
			TimerManager.GetInstance().SetTimer(22, 0.1f, true);
			TimerManager.GetInstance().SetTimer(22, 0.1f, true);
		}

		public override void SetBaseConfig()
		{
			base.SetBaseConfig();
			bulletFlySpeed = 3.8f;
			bulletFlySpeed += bulletFlySpeed * GetBulletFlySpeedFac();
		}

		protected void RandomRunAnimation()
		{
			runAnimationName = "Forward01";
		}

		public override void DoLogic(float deltaTime)
		{
			base.DoLogic(deltaTime);
			if (!(base.HP <= 0f) && PullTriggerTime >= 0f && Time.time - PullTriggerTime >= 0.35f)
			{
				Shoot();
				PullTriggerTime = -1f;
			}
		}

		public void PullTrigger()
		{
			PullTriggerTime = Time.time;
		}

		public override void OnAttack()
		{
			base.OnAttack();
			enemyObject.transform.LookAt(player.GetTransform().position);
			if (base.SqrDistanceFromPlayer < base.AttackRange * base.AttackRange)
			{
				Animate("Attack_LongRange01", WrapMode.Once);
				PullTrigger();
				lastAttackTime = Time.time;
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
			base.Audio.PlaySound("DeadEnv", true);
			base.Audio.PlaySound("Dead", true);
		}

		public override bool AttackAnimationEnds()
		{
			string text = "Attack_LongRange01";
			if (base.SqrDistanceFromPlayer < base.AttackRange * base.AttackRange)
			{
				text = "Attack_LongRange01";
			}
			if (Time.time - lastAttackTime > enemyObject.GetComponent<Animation>()[text].length)
			{
				return true;
			}
			return false;
		}

		public void Shoot()
		{
			Vector3 position = enemyObject.transform.TransformPoint(new Vector3(0.7255474f, 1.768321f, 2.331216f));
			GameObject gameObject = Object.Instantiate(gConfig.infecterBullet, position, Quaternion.LookRotation(player.GetTransform().position)) as GameObject;
			InfecterBulletScript infecterBulletScript = gameObject.AddComponent<InfecterBulletScript>();
			infecterBulletScript.damage = attackDamage;
			infecterBulletScript.flySpeed = bulletFlySpeed;
		}
	}
}
