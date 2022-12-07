using System.Collections.Generic;
using UnityEngine;

namespace Zombie3D
{
	public class EnemySkillGasbomb : EnemySkillImpl
	{
		protected uint m_iMaxCount;

		private float bulletFlySpeed = 3.8f;

		private List<GameObject> _playerGOs = new List<GameObject>();

		public override void Init(Enemy ownerEnemy, EnemySkill skill)
		{
			base.Init(ownerEnemy, skill);
			bulletFlySpeed = 3.8f;
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
				m_iMaxCount = 3u;
			}
			else if (m_Skill.Level == 4)
			{
				m_iMaxCount = 8u;
			}
			else
			{
				m_iMaxCount = 8u;
			}
		}

		public override void Shoot()
		{
			base.Shoot();
			Vector3 zero = Vector3.zero;
			zero = m_OwnerEnemy.enemyObject.transform.TransformPoint(new Vector3(0.7255474f, 1.768321f, 2.331216f));
			if (m_iMaxCount > 4)
			{
				for (int i = 0; i < m_iMaxCount; i++)
				{
					GameObject gameObject = Object.Instantiate(gConfig.infecterBullet, zero, Quaternion.identity) as GameObject;
					gameObject.transform.Rotate(Vector3.up, i * 45);
					CommonEnemyBulletScript commonEnemyBulletScript = gameObject.AddComponent(typeof(CommonEnemyBulletScript)) as CommonEnemyBulletScript;
					commonEnemyBulletScript.Speed = bulletFlySpeed;
					commonEnemyBulletScript.Damage = m_OwnerEnemy.AttackDamage;
					commonEnemyBulletScript.ForwardDirection = CommonEnemyBulletScript.BulletForwardDirection.forward;
					commonEnemyBulletScript.bUpdateCheckHit = true;
				}
			}
			else
			{
				_playerGOs.Clear();
				GameObject[] array = GameObject.FindGameObjectsWithTag("Player");
				for (int j = 0; j < m_iMaxCount; j++)
				{
					int num = Random.Range(0, array.Length);
					_playerGOs.Add(array[num]);
					GameObject gameObject2 = Object.Instantiate(gConfig.infecterBullet, zero, Quaternion.LookRotation(array[num].transform.position)) as GameObject;
					InfecterBulletScript infecterBulletScript = gameObject2.AddComponent<InfecterBulletScript>();
					infecterBulletScript.Init(array[num]);
					infecterBulletScript.damage = m_OwnerEnemy.AttackDamage;
					infecterBulletScript.flySpeed = bulletFlySpeed;
				}
			}
			bCanAttack = false;
		}

		public void CopyShoot(List<GameObject> lsTarget = null)
		{
			base.Shoot();
			Vector3 zero = Vector3.zero;
			zero = m_OwnerEnemy.enemyObject.transform.TransformPoint(new Vector3(0.7255474f, 1.768321f, 2.331216f));
			if (m_iMaxCount > 4)
			{
				for (int i = 0; i < m_iMaxCount; i++)
				{
					GameObject gameObject = Object.Instantiate(gConfig.infecterBullet, zero, Quaternion.identity) as GameObject;
					gameObject.transform.Rotate(Vector3.up, i * 45);
					CommonEnemyBulletScript commonEnemyBulletScript = gameObject.AddComponent(typeof(CommonEnemyBulletScript)) as CommonEnemyBulletScript;
					commonEnemyBulletScript.Speed = bulletFlySpeed;
					commonEnemyBulletScript.Damage = m_OwnerEnemy.AttackDamage;
					commonEnemyBulletScript.ForwardDirection = CommonEnemyBulletScript.BulletForwardDirection.forward;
				}
			}
			else
			{
				for (int j = 0; j < m_iMaxCount; j++)
				{
					if (lsTarget == null)
					{
						lsTarget = new List<GameObject>();
					}
					if (lsTarget[j] == null)
					{
						lsTarget.Add(PlayerManager.Instance.GetPlayerObject());
					}
					GameObject gameObject2 = Object.Instantiate(gConfig.infecterBullet, zero, Quaternion.LookRotation(lsTarget[j].transform.position)) as GameObject;
					InfecterBulletScript infecterBulletScript = gameObject2.AddComponent<InfecterBulletScript>();
					infecterBulletScript.Init(lsTarget[j]);
					infecterBulletScript.damage = m_OwnerEnemy.AttackDamage;
					infecterBulletScript.flySpeed = bulletFlySpeed;
				}
			}
			bCanAttack = false;
		}

		public override void Dologic(float deltaTime)
		{
			base.Dologic(deltaTime);
			if (!bCanAttack || !(Time.time - m_fLastAttackTime >= m_fCoolDown))
			{
				return;
			}
			Shoot();
			m_fLastAttackTime = Time.time;
			List<int> list = new List<int>();
			foreach (GameObject playerGO in _playerGOs)
			{
				if (PlayerManager.Instance.GetRecipientByObj(playerGO) != null)
				{
					list.Add(PlayerManager.Instance.GetRecipientId(PlayerManager.Instance.GetRecipientByObj(playerGO)));
				}
				else if (PlayerManager.Instance.GetPlayerObject() == playerGO)
				{
					list.Add(GameSetup.Instance.MineUser.Id);
				}
			}
			GameSetup.Instance.ReqEnemySkill(m_OwnerEnemy.enemyID, GetSkill().SkillType, list);
		}
	}
}
