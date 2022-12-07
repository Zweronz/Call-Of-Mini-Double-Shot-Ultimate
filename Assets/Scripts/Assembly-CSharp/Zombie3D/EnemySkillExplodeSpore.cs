using System;
using System.Collections.Generic;
using UnityEngine;

namespace Zombie3D
{
	public class EnemySkillExplodeSpore : EnemySkillImpl
	{
		protected uint m_iMaxCount;

		protected float m_SporeChildGenerateTimer = -1f;

		protected float m_SporeChildGenerateTime = 2f;

		protected float fSkillCDTime = 3f;

		protected float fSkillCDTimer = -2f;

		protected List<SporeChild> SporeChildren = new List<SporeChild>();

		protected GameObject targetGO;

		protected List<Vector3> lsPosition = new List<Vector3>();

		public override void Init(Enemy ownerEnemy, EnemySkill skill)
		{
			base.Init(ownerEnemy, skill);
			if (skill.Level == 1)
			{
				m_iMaxCount = 1u;
			}
			else
			{
				m_iMaxCount = 2u;
			}
			m_OwnerEnemy.SetState(Enemy.IDLE_STATE);
			fSkillCDTimer = 0f;
		}

		public override void Shoot()
		{
			base.Shoot();
			GameObject[] array = GameObject.FindGameObjectsWithTag("Player");
			List<GameObject> list = new List<GameObject>();
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] == PlayerManager.Instance.GetPlayerObject())
				{
					if (PlayerManager.Instance.GetPlayerClass().HP > 0f)
					{
						list.Add(array[i]);
					}
				}
				else if (PlayerManager.Instance.GetRecipientByObj(array[i]) != null && PlayerManager.Instance.GetRecipientByObj(array[i]).HP > 0f)
				{
					list.Add(array[i]);
				}
			}
			if (list.Count > 0)
			{
				int index = UnityEngine.Random.Range(0, list.Count);
				targetGO = list[index];
			}
			else
			{
				targetGO = PlayerManager.Instance.GetPlayerObject();
				Debug.Log("No Alive Player");
			}
			CreateSpore(targetGO);
			m_SporeChildGenerateTimer = 0f;
			List<int> list2 = new List<int>();
			if (PlayerManager.Instance.GetRecipientByObj(targetGO) != null)
			{
				list2.Add(PlayerManager.Instance.GetRecipientId(PlayerManager.Instance.GetRecipientByObj(targetGO)));
			}
			else if (PlayerManager.Instance.GetPlayerObject() == targetGO)
			{
				list2.Add(GameSetup.Instance.MineUser.Id);
			}
			GameSetup.Instance.ReqEnemySkill(m_OwnerEnemy.enemyID, GetSkill().SkillType, list2, lsPosition);
		}

		public void CopyShoot(GameObject go, List<Vector3> place)
		{
			lsPosition = place;
			for (int i = 0; i < m_iMaxCount; i++)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(gConfig.SporeChild, place[i], Quaternion.identity) as GameObject;
				lsPosition.Add(gameObject.transform.position);
				SporeChild sporeChild = gameObject.AddComponent(typeof(SporeChild)) as SporeChild;
				sporeChild.m_Type = SporeChild.SporeChildType.Common;
				sporeChild.ParentSporeSkill = this;
				sporeChild.damagePercent = m_OwnerEnemy.AttackDamage;
				sporeChild.hp = 100000f;
				SporeChildren.Add(sporeChild);
			}
		}

		public override void Dologic(float deltaTime)
		{
			base.Dologic(deltaTime);
			if (m_SporeChildGenerateTimer >= 0f)
			{
				m_SporeChildGenerateTimer += deltaTime;
				if (m_SporeChildGenerateTimer > m_SporeChildGenerateTime)
				{
					Shoot();
				}
			}
			else if (fSkillCDTimer >= 0f)
			{
				fSkillCDTimer += deltaTime;
				if (fSkillCDTimer >= fSkillCDTime)
				{
					m_SporeChildGenerateTimer = 0f;
					fSkillCDTimer = 0f;
				}
			}
		}

		public void CreateSpore(GameObject target)
		{
			lsPosition.Clear();
			for (int i = 0; i < m_iMaxCount; i++)
			{
				float num = UnityEngine.Random.Range(4f, 8f);
				float num2 = UnityEngine.Random.Range(0f, 360f);
				Vector3 position = target.transform.position;
				float x = position.x + num * Mathf.Sin(num2 * ((float)Math.PI / 180f));
				float z = position.z + num * Mathf.Cos(num2 * ((float)Math.PI / 180f));
				GameObject gameObject = UnityEngine.Object.Instantiate(gConfig.SporeChild, new Vector3(x, 10000.3f, z), Quaternion.identity) as GameObject;
				lsPosition.Add(gameObject.transform.position);
				SporeChild sporeChild = gameObject.AddComponent(typeof(SporeChild)) as SporeChild;
				sporeChild.m_Type = SporeChild.SporeChildType.Common;
				sporeChild.ParentSporeSkill = this;
				sporeChild.damagePercent = m_OwnerEnemy.AttackDamage;
				sporeChild.hp = 100000f;
				SporeChildren.Add(sporeChild);
			}
		}

		public void OnSporeChildDead(SporeChild spore_child)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(gConfig.SporeParticles04, spore_child.transform.position, Quaternion.identity) as GameObject;
			gameObject.transform.Translate(Vector3.up * 1f, Space.World);
		}

		public override void Clear()
		{
			base.Clear();
			foreach (SporeChild sporeChild in SporeChildren)
			{
				if (sporeChild != null)
				{
					sporeChild.Clear();
				}
			}
		}
	}
}
