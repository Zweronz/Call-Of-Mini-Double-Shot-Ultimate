using System.Collections;
using UnityEngine;

namespace Zombie3D
{
	public class SkillBuildCannon : SkillImpl
	{
		private GameObject m_CannonObj;

		private WeaponBulletsPool m_CannonBulletPool;

		private GameObject m_CannonObjGunFire;

		private Enemy m_CurTarget;

		private float m_CannonDamage = 10f;

		private float m_CannonAttackFrequence = 1f;

		private float m_LastAttackTimer;

		private float m_LastFireTimer = -1f;

		public override void Init(Player player, Skill skill)
		{
			base.Init(player, skill);
			float value = 10f + (float)(skill.Level - 1) * 10f;
			m_CannonDamage = Mathf.Clamp(value, 10f, 50f);
			m_CannonAttackFrequence = 0.2f;
			base.SkillStanimaLoseSpeed = 5f;
			m_LastAttackTimer = 0f;
			GameObject gameObject = Object.Instantiate(Resources.Load("Zombie3D/Misc/SkillCannonEffect_Born"), m_Player.GetTransform().position, m_Player.GetTransform().rotation) as GameObject;
			RemoveTimerScript removeTimerScript = gameObject.AddComponent<RemoveTimerScript>();
			removeTimerScript.life = 1f;
			m_CannonObj = Object.Instantiate(Resources.Load("Zombie3D/Misc/SkillCannon"), m_Player.GetTransform().position, m_Player.GetTransform().rotation) as GameObject;
			m_CannonObjGunFire = m_CannonObj.transform.Find("GunFire").gameObject;
			m_CannonObjGunFire.SetActiveRecursively(false);
			m_CannonBulletPool = new WeaponBulletsPool();
			m_CannonBulletPool.Init("SkillCannonBulletPool", Resources.Load("Zombie3D/Misc/SkillCannonBullet") as GameObject, 5);
		}

		public override void Update(float deltaTime)
		{
			base.Update(deltaTime);
			m_CannonBulletPool.DoLogic();
			m_LastAttackTimer += deltaTime;
			if (m_CurTarget != null && m_CurTarget.HP > 0f)
			{
				if (m_LastAttackTimer > m_CannonAttackFrequence)
				{
					m_LastAttackTimer = 0f;
					m_CannonObj.transform.LookAt(new Vector3(m_CurTarget.GetTransform().position.x, m_CannonObj.transform.position.y, m_CurTarget.GetTransform().position.z));
					m_CannonObj.GetComponent<Animation>().Play(m_CannonObj.GetComponent<Animation>().clip.name);
					m_CannonObjGunFire.SetActiveRecursively(true);
					m_LastFireTimer = 0f;
					Vector3 position = m_CannonObj.transform.TransformPoint(new Vector3(0f, 1.15f, 2.4f));
					GameObject gameObject = m_CannonBulletPool.CreateBullet(position, Quaternion.Euler(new Vector3(270f, m_CannonObj.transform.eulerAngles.y, 0f)));
					SkillCannonBulletScript component = gameObject.GetComponent<SkillCannonBulletScript>();
					if (component != null)
					{
						component.AttackRange = 7.5f;
						component.Damage = m_CannonDamage;
						component.Speed = 15f;
						component.Init();
					}
				}
			}
			else
			{
				FindNextEnemyTarget();
			}
			if (m_LastFireTimer >= 0f)
			{
				m_LastFireTimer += deltaTime;
				if (m_LastFireTimer > 0.5f)
				{
					m_LastFireTimer = -1f;
					m_CannonObjGunFire.SetActiveRecursively(false);
				}
			}
			if (m_Player.Stamina <= 0f)
			{
				m_Player.TerminateActiveSkill();
			}
		}

		public override void Stop()
		{
			if (m_CannonObj != null)
			{
				GameObject gameObject = Object.Instantiate(Resources.Load("Zombie3D/Misc/SkillCannonEffect_Born"), m_CannonObj.transform.position, m_CannonObj.transform.rotation) as GameObject;
				RemoveTimerScript removeTimerScript = gameObject.AddComponent<RemoveTimerScript>();
				removeTimerScript.life = 1f;
				Object.Destroy(m_CannonObj);
			}
			m_CannonBulletPool.DestroyPool();
		}

		private void FindNextEnemyTarget()
		{
			m_CurTarget = null;
			float num = 7.5f;
			Hashtable enemies = GameApp.GetInstance().GetGameScene().GetEnemies();
			foreach (Enemy value in enemies.Values)
			{
				if (value != null && (m_CannonObj.transform.position - value.GetTransform().position).sqrMagnitude < num * num && value.HP > 0f)
				{
					m_CurTarget = value;
					break;
				}
			}
			if (m_CurTarget == null)
			{
				Debug.Log("Not Find Target!!!");
			}
			else
			{
				Debug.Log("Find Target!!!");
			}
		}
	}
}
