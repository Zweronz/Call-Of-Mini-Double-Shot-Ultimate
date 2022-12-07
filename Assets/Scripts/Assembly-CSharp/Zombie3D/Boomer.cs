using UnityEngine;

namespace Zombie3D
{
	public class Boomer : Enemy
	{
		public static EnemyState EXPLODE_STATE = new BoomerExplodeState();

		public static EnemyState PAUSE_STATE = new BoomerPauseState();

		protected GameObject fire;

		protected Vector3 targetPosition;

		protected GameObject explodeObject;

		protected float attackDetecDis = 5f;

		protected float runSpeedFast = 3.5f;

		protected float speedUpTimer = -1f;

		public override void Init(GameObject gObject)
		{
			base.Init(gObject);
			lastTarget = Vector3.zero;
			runAnimationName = "Run";
			explodeObject = gConfig.boomerExplosion;
			fire = gObject.transform.Find("Fire").gameObject;
			fire.SetActiveRecursively(false);
			runSpeedFast = runSpeed * 1.2f;
		}

		public override void DoLogic(float deltaTime)
		{
			base.DoLogic(deltaTime);
			if (base.HP <= 0f || !(runSpeed < runSpeedFast))
			{
				return;
			}
			if (base.SqrDistanceFromPlayer < attackDetecDis * attackDetecDis && speedUpTimer < 0f)
			{
				speedUpTimer = 0f;
				SetState(PAUSE_STATE);
				fire.SetActiveRecursively(true);
			}
			if (speedUpTimer >= 0f)
			{
				speedUpTimer += deltaTime;
				if (speedUpTimer >= 0.4f)
				{
					hp = 1f;
					runSpeed = runSpeedFast;
					animation[runAnimationName].speed = 1.5f;
					speedUpTimer = -1f;
					SetState(Enemy.CATCHING_STATE);
				}
			}
		}

		public void Explode()
		{
			Vector3 position = new Vector3(enemyObject.transform.position.x, enemyObject.transform.position.y + 1.5f, enemyObject.transform.position.z);
			GameObject gameObject = Object.Instantiate(gConfig.boomerBurst, position, Quaternion.identity) as GameObject;
			player.OnHit(attackDamage);
			GameApp.GetInstance().GetGameState().AddGCBoomerAttackTimes();
			Vector3 vector = player.GetTransform().position - enemyObject.transform.position;
			player.OnHitBack(0.4f, 1f, vector);
		}

		public override void OnDead()
		{
			GameObject gameObject = Object.Instantiate(explodeObject, enemyTransform.position, Quaternion.identity) as GameObject;
			if (GameApp.GetInstance().GetGameState().SoundOn)
			{
				Transform transform = gameObject.transform.Find("Audio/Dead");
				if (transform != null && transform.gameObject != null)
				{
					AudioSource component = transform.GetComponent<AudioSource>();
					if (component != null)
					{
						component.Play();
					}
				}
			}
			criticalAttacked = true;
			base.OnDead();
		}

		public override void OnAttack()
		{
			base.OnAttack();
			DamageProperty damageProperty = new DamageProperty();
			damageProperty.damage = hp;
			Explode();
			((TopWatchingCameraScript)GameApp.GetInstance().GetGameScene().GetCamera()).ShowExplodeEffect(false);
			criticalAttacked = true;
			OnHit(damageProperty, WeaponType.NoGun);
		}

		public override EnemyState EnterSpecialState(float deltaTime)
		{
			return null;
		}
	}
}
