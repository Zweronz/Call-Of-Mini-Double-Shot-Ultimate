using UnityEngine;

namespace Zombie3D
{
	public class Batcher : Enemy
	{
		protected Collider axCollider;

		protected Batcher_EffectTrail m_Trail;

		protected bool bCheckHit;

		protected float attackAnimBeginTime;

		protected float checkHitTimeAfterAnim;

		protected float checkHitOverTimeAfterAnim;

		protected float lastHitPlayerTime = -1f;

		protected float lastHitFriendPlayerTime = -1f;

		protected float trailTimer = -1f;

		private float trailStartFrame = 20f;

		private float trailEndFrame = 33f;

		protected string DeadAudioName = "Dead";

		protected string GetHitAudioName = string.Empty;

		public override void Init(GameObject gObject)
		{
			base.Init(gObject);
			Transform folderTrans = enemyTransform.Find("Audio");
			base.Audio.AddAudio(folderTrans, "DeadEnv");
			base.Audio.AddAudio(folderTrans, DeadAudioName);
			for (int i = 1; i < 4; i++)
			{
				string text = "GetHit0" + i;
				base.Audio.AddAudio(folderTrans, text);
			}
			axCollider = enemyTransform.Find("Bip01/Bip01 Prop1").gameObject.GetComponent<Collider>();
			m_Trail = enemyTransform.Find("Bip01/Bip01 Prop1/BatcherAxTail").gameObject.GetComponent(typeof(Batcher_EffectTrail)) as Batcher_EffectTrail;
			m_Trail.m_bCollectPoints = false;
			m_Trail.m_lastTime = enemyObject.GetComponent<Animation>()["Attack01"].length * ((trailEndFrame - trailStartFrame) / 62f);
			checkHitTimeAfterAnim = animation["Attack01"].clip.length * 0.32258064f;
			checkHitOverTimeAfterAnim = animation["Attack01"].clip.length * (33f / 62f);
			runAnimationName = "Forward01";
			TimerManager.GetInstance().SetTimer(21, 0.1f, true);
			TimerManager.GetInstance().SetTimer(22, 0.1f, true);
			TimerManager.GetInstance().SetTimer(22, 0.1f, true);
		}

		public override void DoLogic(float deltaTime)
		{
			base.DoLogic(deltaTime);
			if (base.HP <= 0f)
			{
				return;
			}
			if (bCheckHit && Time.time - attackAnimBeginTime >= checkHitTimeAfterAnim)
			{
				if (Time.time - attackAnimBeginTime < checkHitOverTimeAfterAnim)
				{
					if (Time.time - lastHitPlayerTime > 0.5f && axCollider.bounds.Intersects(player.GetCollider().bounds))
					{
						Debug.Log("Hit Player  " + Time.time);
						lastHitPlayerTime = Time.time;
						HitPlayer(player);
					}
					if (Time.time - lastHitFriendPlayerTime > 0.5f && axCollider.bounds.Intersects(FriendPlayer.GetCollider().bounds))
					{
						lastHitFriendPlayerTime = Time.time;
						HitPlayer(FriendPlayer);
					}
				}
				else
				{
					bCheckHit = false;
				}
			}
			if (!(trailTimer >= 0f))
			{
				return;
			}
			trailTimer += deltaTime;
			float length = enemyObject.GetComponent<Animation>()["Attack01"].length;
			if (trailTimer < length * (trailEndFrame / 62f))
			{
				if (!m_Trail.m_bCollectPoints && trailTimer >= length * (trailStartFrame / 62f))
				{
					m_Trail.m_bCollectPoints = true;
				}
			}
			else
			{
				m_Trail.m_bCollectPoints = false;
				trailTimer = -1f;
			}
		}

		public override void OnAttack()
		{
			base.OnAttack();
			enemyObject.transform.LookAt(player.GetTransform().position);
			Animate("Attack01", WrapMode.Once);
			trailTimer = 0f;
			bCheckHit = true;
			attackAnimBeginTime = Time.time;
			lastAttackTime = Time.time;
		}

		public override void OnDead()
		{
			base.OnDead();
			base.Audio.PlaySound("DeadEnv", true);
			base.Audio.PlaySound(DeadAudioName, true);
			Object.Destroy(m_Trail.TrailObject);
		}

		public override void OnHit(DamageProperty dp, WeaponType weaponType)
		{
			criticalAttacked = false;
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

		public void HitPlayer(Player player_hit)
		{
			player_hit.OnHit(attackDamage);
			Vector3 vector = new Vector3(player_hit.GetTransform().position.x, 0f, player_hit.GetTransform().position.z) - new Vector3(enemyObject.transform.position.x, 0f, enemyObject.transform.position.z);
			player_hit.OnHitBack(0.3f, 1f, vector);
		}
	}
}
