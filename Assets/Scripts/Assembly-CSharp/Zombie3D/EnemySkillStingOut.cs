using System;
using UnityEngine;

namespace Zombie3D
{
	public class EnemySkillStingOut : EnemySkillImpl
	{
		private float fireAttackRange = 9f;

		private float firingTimer = -1f;

		private float firingTime = 3f;

		private GameObject m_firingGO;

		private GameObject m_firingGOPrefab;

		private bool bNeedRoating;

		private float lastHitTime;

		public override void Init(Enemy ownerEnemy, EnemySkill skill)
		{
			base.Init(ownerEnemy, skill);
			if (skill.Level == 1)
			{
				m_firingGOPrefab = Resources.Load("Zombie3D/Effect/ShortFiring") as GameObject;
				fireAttackRange = 9f;
				firingTime = 3f;
			}
			else
			{
				m_firingGOPrefab = Resources.Load("Zombie3D/Effect/LongFiring") as GameObject;
				fireAttackRange = 16f;
				if (skill.Level == 2)
				{
					firingTime = 3f;
				}
				else
				{
					firingTime = 2f;
					bNeedRoating = true;
				}
			}
			lastHitTime = Time.time;
			Resources.UnloadUnusedAssets();
			Vector3 position = m_OwnerEnemy.enemyObject.transform.Find("FiringPos").position;
			m_firingGO = UnityEngine.Object.Instantiate(m_firingGOPrefab, position, Quaternion.identity) as GameObject;
			m_firingGO.transform.parent = m_OwnerEnemy.GetTransform();
			m_firingGO.SetActiveRecursively(false);
		}

		public override void Shoot()
		{
			base.Shoot();
			if (firingTimer == -1f)
			{
				firingTimer = 0f;
				m_firingGO.SetActiveRecursively(true);
				CheckPlayersHit(m_OwnerEnemy.AttackDamage);
				bCanAttack = false;
			}
		}

		public override void Dologic(float deltaTime)
		{
			base.Dologic(deltaTime);
			if (firingTimer >= 0f)
			{
				firingTimer += deltaTime;
				if (bNeedRoating)
				{
					m_OwnerEnemy.enemyObject.transform.RotateAround(Vector3.up, (float)Math.PI * 2f / firingTime * deltaTime);
				}
				if (m_OwnerEnemy.SqrDistanceFromPlayer <= fireAttackRange * fireAttackRange)
				{
					CheckPlayersHit(m_OwnerEnemy.AttackDamage);
				}
				if (m_OwnerEnemy.GetState() == Enemy.CATCHING_STATE)
				{
					firingTimer = -1f;
					m_firingGO.SetActiveRecursively(false);
					m_OwnerEnemy.SetState(Enemy.IDLE_STATE);
					bCanAttack = false;
				}
				if (firingTimer >= firingTime || m_OwnerEnemy.HP <= 0f)
				{
					firingTimer = -1f;
					m_firingGO.SetActiveRecursively(false);
					m_OwnerEnemy.SetState(Enemy.IDLE_STATE);
					bCanAttack = false;
				}
			}
			if (bCanAttack && Time.time - m_fLastAttackTime >= m_fCoolDown)
			{
				Shoot();
				m_fLastAttackTime = Time.time;
				GameSetup.Instance.ReqEnemySkill(m_OwnerEnemy.enemyID, GetSkill().SkillType);
			}
		}

		public override void DologicExpression(float deltaTime)
		{
			base.DologicExpression(deltaTime);
			Dologic(deltaTime);
		}

		private void CheckPlayersHit(float damage)
		{
			if (Time.time - lastHitTime <= 0.2f)
			{
				return;
			}
			Player player = GameApp.GetInstance().GetGameScene().GetPlayer();
			if (player != null)
			{
				Vector3 vector = m_OwnerEnemy.enemyObject.transform.InverseTransformPoint(player.PlayerObject.transform.position);
				if (Mathf.Abs(vector.x) <= 0.6f && vector.z * m_OwnerEnemy.enemyObject.transform.localScale.z >= 0.3f && vector.z * m_OwnerEnemy.enemyObject.transform.localScale.z <= fireAttackRange)
				{
					player.OnHit(damage);
					lastHitTime = Time.time;
				}
			}
			if (GameApp.GetInstance().GetGameState().m_eGameMode.m_ePlayMode == GameState.NetworkGameMode.PlayMode.E_Console)
			{
				Player friendPlayer = GameApp.GetInstance().GetGameScene().GetFriendPlayer();
				Vector3 vector2 = m_OwnerEnemy.enemyObject.transform.InverseTransformPoint(friendPlayer.GetTransform().position);
				if (Mathf.Abs(vector2.x) <= 0.6f && vector2.z * m_OwnerEnemy.enemyObject.transform.localScale.z >= 0.3f && vector2.z * m_OwnerEnemy.enemyObject.transform.localScale.z <= fireAttackRange)
				{
					friendPlayer.OnHit(damage);
				}
			}
		}
	}
}
