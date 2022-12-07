using UnityEngine;

namespace Zombie3D
{
	public class Zombie : Enemy
	{
		protected float PullTriggerTimer = -1f;

		protected string DeadAudioName = string.Empty;

		protected string DeadEnvAudioName = string.Empty;

		protected string GetHitAudioName = string.Empty;

		protected void RandomRunAnimation()
		{
			runAnimationName = "Run01";
			int num = Random.Range(0, 10);
			if (num < 7)
			{
				runAnimationName = "Run01";
				return;
			}
			switch (num)
			{
			case 7:
				runAnimationName = "Forward01";
				break;
			case 8:
				runAnimationName = "Forward02";
				break;
			}
		}

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
			lastTarget = Vector3.zero;
			RandomRunAnimation();
			TimerManager.GetInstance().SetTimer(21, 0.1f, true);
			TimerManager.GetInstance().SetTimer(22, 0.1f, true);
			TimerManager.GetInstance().SetTimer(22, 0.1f, true);
		}

		public override void DoLogic(float deltaTime)
		{
			base.DoLogic(deltaTime);
			if (base.HP <= 0f || !(PullTriggerTimer >= 0f))
			{
				return;
			}
			PullTriggerTimer += deltaTime;
			if (PullTriggerTimer >= 0.3f)
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

		public void PullTrigger()
		{
			PullTriggerTimer = 0f;
		}

		public void HitPlayer()
		{
			Vector3 forward = new Vector3(player.GetTransform().position.x, enemyObject.transform.position.y, player.GetTransform().position.z) - enemyObject.transform.position;
			enemyObject.transform.forward = forward;
			Animate("Attack01", WrapMode.Once);
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
