using UnityEngine;

namespace Zombie3D
{
	public class Spider : Enemy
	{
		public static SpiderCatchingState SPIDER_CATCHING_STATE = new SpiderCatchingState();

		public bool bStop;

		public float timer;

		private float MoveTimes = 2.5f;

		private float StopTimes = 3f;

		protected GameObject LightCircle;

		protected float InvincibleRange = 3f;

		public override void Init(GameObject gObject)
		{
			base.Init(gObject);
			Transform folderTrans = enemyTransform.Find("Audio");
			base.Audio.AddAudio(folderTrans, "Dead");
			base.Audio.AddAudio(folderTrans, "Hit01");
			base.Audio.AddAudio(folderTrans, "Hit02");
			base.Audio.AddAudio(folderTrans, "Hit03");
			LightCircle = enemyObject.transform.Find("LightCircle").gameObject;
			LightCircle.SetActiveRecursively(true);
			attackRange = 1.5f;
			bStop = false;
			timer = Time.time;
			runAnimationName = "Forward01";
		}

		public override void DoLogic(float deltaTime)
		{
			base.DoLogic(deltaTime);
			if (base.HP <= 0f)
			{
				return;
			}
			if (LightCircle.active)
			{
				LightCircle.transform.LookAt(Camera.main.transform.position);
			}
			if (bStop)
			{
				if (Time.time - timer >= StopTimes)
				{
					enemyObject.transform.Find("Zombie_Spider").gameObject.GetComponent<Renderer>().material.SetColor("_TintColor", new Color(1f, 1f, 1f, 1f));
					LightCircle.SetActiveRecursively(false);
					bStop = false;
					timer = Time.time;
				}
				return;
			}
			if (LightCircle.active)
			{
				LightCircle.SetActiveRecursively(false);
			}
			if (Time.time - timer >= MoveTimes && base.SqrDistanceFromPlayer > InvincibleRange * InvincibleRange)
			{
				enemyObject.transform.Find("Zombie_Spider").gameObject.GetComponent<Renderer>().material.SetColor("_TintColor", new Color(0.45f, 1f, 0.7f, 1f));
				LightCircle.SetActiveRecursively(true);
				bStop = true;
				timer = Time.time;
			}
		}

		public override void OnAttack()
		{
			base.OnAttack();
			Animate("Attack01", WrapMode.Once);
			player.OnHit(attackDamage);
			Vector3 vector = player.GetTransform().position - enemyObject.transform.position;
			player.OnHitBack(0.4f, 1.5f, vector);
			lastAttackTime = Time.time;
		}

		public override void OnHit(DamageProperty dp, WeaponType weaponType)
		{
			if (!LightCircle.active)
			{
				base.OnHit(dp, weaponType);
			}
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
			if (LightCircle.active)
			{
				LightCircle.SetActiveRecursively(false);
			}
		}

		public override EnemyState EnterSpecialState(float deltaTime)
		{
			return SPIDER_CATCHING_STATE;
		}
	}
}
