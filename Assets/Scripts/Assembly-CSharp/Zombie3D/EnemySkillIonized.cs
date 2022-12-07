using UnityEngine;

namespace Zombie3D
{
	public class EnemySkillIonized : EnemySkillImpl
	{
		protected uint m_iMaxCount;

		private float bulletFlySpeed = 3.8f;

		public override void Init(Enemy ownerEnemy, EnemySkill skill)
		{
			base.Init(ownerEnemy, skill);
			bulletFlySpeed = 3f;
			bulletFlySpeed += bulletFlySpeed * m_OwnerEnemy.GetBulletFlySpeedFac();
			if (m_Skill.Level == 1)
			{
				m_iMaxCount = 1u;
			}
			else if (m_Skill.Level == 2)
			{
				m_iMaxCount = 2u;
			}
			else if (m_Skill.Level == 3)
			{
				m_iMaxCount = 5u;
			}
			else
			{
				m_iMaxCount = 5u;
			}
		}

		public override void Shoot()
		{
			base.Shoot();
			Vector3 zero = Vector3.zero;
			zero = m_OwnerEnemy.enemyObject.transform.TransformPoint(new Vector3(0f, 0.9703195f, 1.511088f));
			if (m_iMaxCount == 1)
			{
				GameObject gameObject = Object.Instantiate(gConfig.hunterBullet, zero, Quaternion.identity) as GameObject;
				gameObject.transform.eulerAngles = m_OwnerEnemy.enemyObject.transform.eulerAngles;
				CommonEnemyBulletScript commonEnemyBulletScript = gameObject.AddComponent(typeof(CommonEnemyBulletScript)) as CommonEnemyBulletScript;
				commonEnemyBulletScript.Speed = bulletFlySpeed;
				commonEnemyBulletScript.Damage = m_OwnerEnemy.AttackDamage;
				commonEnemyBulletScript.ForwardDirection = CommonEnemyBulletScript.BulletForwardDirection.forward;
			}
			else if (m_iMaxCount == 2)
			{
				if (m_iMaxCount != 0)
				{
					for (int i = 0; i < m_iMaxCount; i++)
					{
						Vector3 position = zero;
						if (i == 0)
						{
							position.x -= 0.2f;
						}
						else
						{
							position.x += 0.2f;
						}
						GameObject gameObject2 = Object.Instantiate(gConfig.hunterBullet, position, Quaternion.identity) as GameObject;
						gameObject2.transform.eulerAngles = m_OwnerEnemy.enemyObject.transform.eulerAngles;
						CommonEnemyBulletScript commonEnemyBulletScript2 = gameObject2.AddComponent(typeof(CommonEnemyBulletScript)) as CommonEnemyBulletScript;
						commonEnemyBulletScript2.Speed = bulletFlySpeed;
						commonEnemyBulletScript2.Damage = m_OwnerEnemy.AttackDamage;
						commonEnemyBulletScript2.ForwardDirection = CommonEnemyBulletScript.BulletForwardDirection.forward;
					}
				}
			}
			else if (m_iMaxCount != 0)
			{
				for (int j = 0; j < m_iMaxCount; j++)
				{
					Vector3 position2 = zero;
					GameObject gameObject3 = Object.Instantiate(gConfig.hunterBullet, position2, Quaternion.identity) as GameObject;
					gameObject3.transform.eulerAngles = m_OwnerEnemy.enemyObject.transform.eulerAngles;
					gameObject3.transform.Rotate(Vector3.up, j * 30 - 60);
					CommonEnemyBulletScript commonEnemyBulletScript3 = gameObject3.AddComponent(typeof(CommonEnemyBulletScript)) as CommonEnemyBulletScript;
					commonEnemyBulletScript3.Speed = bulletFlySpeed;
					commonEnemyBulletScript3.Damage = m_OwnerEnemy.AttackDamage;
					commonEnemyBulletScript3.ForwardDirection = CommonEnemyBulletScript.BulletForwardDirection.forward;
				}
			}
			bCanAttack = false;
		}

		public override void Dologic(float deltaTime)
		{
			base.Dologic(deltaTime);
			if (bCanAttack && Time.time - m_fLastAttackTime >= m_fCoolDown)
			{
				Shoot();
				m_fLastAttackTime = Time.time;
				GameSetup.Instance.ReqEnemySkill(m_OwnerEnemy.enemyID, GetSkill().SkillType);
			}
		}
	}
}
