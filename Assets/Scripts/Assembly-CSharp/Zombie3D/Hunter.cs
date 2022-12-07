using UnityEngine;

namespace Zombie3D
{
	public class Hunter : Enemy
	{
		private float bulletFlySpeed = 3f;

		private float zoomInAttackRange = 8f;

		protected string DeadAudioName = "Dead";

		protected string DeadEnvAudioName = string.Empty;

		protected string GetHitAudioName = string.Empty;

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
			Debug.Log("bulletFlySpeed - " + bulletFlySpeed);
			bulletFlySpeed += bulletFlySpeed * GetBulletFlySpeedFac();
			Debug.Log("bulletFlySpeed - " + bulletFlySpeed);
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
			runAnimationName = "Forward01";
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
			if (!(base.HP <= 0f) && attackRange == zoomInAttackRange && Vector3.Distance(enemyObject.transform.position, player.GetTransform().position) > zoomInAttackRange)
			{
				float num = hp;
				SetBaseConfig();
				hp = num;
			}
		}

		public override void OnAttack()
		{
			base.OnAttack();
			attackRange = zoomInAttackRange;
			Animate("Fire01", WrapMode.Once);
			enemyTransform.LookAt(target);
			Vector3 position = enemyObject.transform.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Dummy_weapon_center/Dummy_weapon_L3/Dummy_weapon_L2/Dummy_weapon_L1").position;
			GameObject gameObject = Object.Instantiate(gConfig.hunterBullet, position, Quaternion.identity) as GameObject;
			CommonEnemyBulletScript commonEnemyBulletScript = gameObject.AddComponent(typeof(CommonEnemyBulletScript)) as CommonEnemyBulletScript;
			commonEnemyBulletScript.Speed = bulletFlySpeed;
			commonEnemyBulletScript.Damage = attackDamage;
			commonEnemyBulletScript.ForwardDirection = CommonEnemyBulletScript.BulletForwardDirection.forward;
			Vector3 forward = player.GetTransform().position - enemyObject.transform.position;
			gameObject.transform.forward = forward;
			Vector3 position2 = enemyObject.transform.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Dummy_weapon_center/Dummy_weapon_R3/Dummy_weapon_R2/Dummy_weapon_R1").position;
			GameObject gameObject2 = Object.Instantiate(gConfig.hunterBullet, position2, Quaternion.identity) as GameObject;
			CommonEnemyBulletScript commonEnemyBulletScript2 = gameObject2.AddComponent(typeof(CommonEnemyBulletScript)) as CommonEnemyBulletScript;
			commonEnemyBulletScript2.Speed = bulletFlySpeed;
			commonEnemyBulletScript2.Damage = attackDamage;
			commonEnemyBulletScript.ForwardDirection = CommonEnemyBulletScript.BulletForwardDirection.forward;
			Vector3 forward2 = player.GetTransform().position - enemyObject.transform.position;
			gameObject2.transform.forward = forward2;
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
	}
}
