using System;
using System.Collections.Generic;
using UnityEngine;

namespace Zombie3D
{
	public class EnemySkillSpikeweed : EnemySkillImpl
	{
		public enum ThornsState
		{
			E_SetPosition = 0,
			E_ZoomIn = 1,
			E_ZoomContinue = 2,
			E_ZoomOut = 3
		}

		protected uint m_iMaxCount;

		protected ThornsState thornsState;

		protected GameObject[] SporeThorns;

		protected GameObject[] SporeThornsGroundParticles;

		protected GameObject targetGO;

		protected List<Vector3> lsPosition = new List<Vector3>();

		protected float fPromptTime = 0.8f;

		protected float fSetpositionTime;

		protected bool bSetposition;

		protected float fSkillCDTime = 3f;

		protected float fSkillCDTimer = -2f;

		protected float fContinueTimer = -1f;

		protected float fContinueTime = 3f;

		protected float fLastHitTimer = Time.time;

		protected bool bSetPosition;

		protected bool bZoomIn;

		protected bool bZoomOut;

		public override void Init(Enemy ownerEnemy, EnemySkill skill)
		{
			base.Init(ownerEnemy, skill);
			m_iMaxCount = 5u;
			m_OwnerEnemy.SetState(Enemy.IDLE_STATE);
			SporeThorns = new GameObject[m_iMaxCount];
			for (int i = 0; i < SporeThorns.Length; i++)
			{
				GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(gConfig.SporeThorn, Vector3.zero, Quaternion.identity);
				gameObject.SetActiveRecursively(false);
				SporeThorns[i] = gameObject;
			}
			SporeThornsGroundParticles = new GameObject[m_iMaxCount];
			for (int j = 0; j < SporeThornsGroundParticles.Length; j++)
			{
				GameObject gameObject2 = (GameObject)UnityEngine.Object.Instantiate(gConfig.SporeParticles02, Vector3.zero, Quaternion.identity);
				gameObject2.SetActiveRecursively(false);
				SporeThornsGroundParticles[j] = gameObject2;
			}
		}

		public override void Shoot()
		{
			base.Shoot();
			fSkillCDTimer = -1f;
		}

		public void CopyShoot(List<GameObject> targetGO, List<Vector3> pos)
		{
			lsPosition.Clear();
			lsPosition = pos;
			SporeThorns[0].transform.position = targetGO[0].transform.position;
			for (int i = 1; i < SporeThorns.Length; i++)
			{
				SporeThorns[i].transform.position = pos[i - 1];
			}
			for (int j = 0; j < SporeThorns.Length && j < SporeThornsGroundParticles.Length; j++)
			{
				SporeThornsGroundParticles[j].transform.position = SporeThorns[j].transform.position;
			}
			fSkillCDTimer = -1f;
		}

		public override void Dologic(float deltaTime)
		{
			base.Dologic(deltaTime);
			if (fSkillCDTimer == -1f)
			{
				if (thornsState == ThornsState.E_SetPosition)
				{
					if (!bSetposition)
					{
						SetAttackPositions();
						ShowAttackThornGroundParticles(true);
						bSetposition = true;
						fSetpositionTime = Time.time;
					}
					if (Time.time - fSetpositionTime > fPromptTime)
					{
						thornsState = ThornsState.E_ZoomIn;
						bSetposition = false;
					}
				}
				else if (thornsState == ThornsState.E_ZoomIn)
				{
					ZoomInSporeThorns();
					ShowSporeThorns(true);
					CheckPlayersHit(m_OwnerEnemy.AttackDamage);
					fContinueTimer = 0f;
					thornsState = ThornsState.E_ZoomContinue;
				}
				else if (thornsState == ThornsState.E_ZoomContinue)
				{
					CheckPlayersHit(m_OwnerEnemy.AttackDamage);
					if (fContinueTimer >= 0f)
					{
						fContinueTimer += deltaTime;
						if (fContinueTimer >= fContinueTime)
						{
							fContinueTimer = -1f;
							thornsState = ThornsState.E_ZoomOut;
						}
					}
				}
				else if (thornsState == ThornsState.E_ZoomOut)
				{
					ZoomOutSporeThorns();
					ShowAttackThornGroundParticles(false);
					thornsState = ThornsState.E_SetPosition;
					fSkillCDTimer = 0f;
				}
			}
			else
			{
				fSkillCDTimer += deltaTime;
				if (fSkillCDTimer >= fSkillCDTime)
				{
					Shoot();
				}
			}
		}

		public override void DologicExpression(float deltaTime)
		{
			base.DologicExpression(deltaTime);
			if (fSkillCDTimer != -1f)
			{
				return;
			}
			if (thornsState == ThornsState.E_SetPosition)
			{
				if (!bSetposition)
				{
					ShowAttackThornGroundParticles(true);
					bSetposition = true;
					fSetpositionTime = Time.time;
				}
				if (Time.time - fSetpositionTime > fPromptTime)
				{
					thornsState = ThornsState.E_ZoomIn;
					bSetposition = false;
				}
			}
			else if (thornsState == ThornsState.E_ZoomIn)
			{
				ZoomInSporeThorns();
				ShowSporeThorns(true);
				CheckPlayersHit(m_OwnerEnemy.AttackDamage);
				fContinueTimer = 0f;
				thornsState = ThornsState.E_ZoomContinue;
			}
			else if (thornsState == ThornsState.E_ZoomContinue)
			{
				CheckPlayersHit(m_OwnerEnemy.AttackDamage);
				if (fContinueTimer >= 0f)
				{
					fContinueTimer += deltaTime;
					if (fContinueTimer >= fContinueTime)
					{
						fContinueTimer = -1f;
						thornsState = ThornsState.E_ZoomOut;
					}
				}
			}
			else if (thornsState == ThornsState.E_ZoomOut)
			{
				ZoomOutSporeThorns();
				ShowAttackThornGroundParticles(false);
				thornsState = ThornsState.E_SetPosition;
				fSkillCDTimer = 0f;
			}
		}

		public void SetAttackPositions()
		{
			lsPosition.Clear();
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
			SporeThorns[0].transform.position = targetGO.transform.position;
			lsPosition.Add(SporeThorns[0].transform.position);
			for (int j = 1; j < SporeThorns.Length; j++)
			{
				Vector3 position = targetGO.transform.position;
				float num = UnityEngine.Random.Range(1f, 5f);
				float num2 = UnityEngine.Random.Range(0f, 360f);
				float x = position.x + num * Mathf.Sin(num2 * ((float)Math.PI / 180f));
				float z = position.z + num * Mathf.Cos(num2 * ((float)Math.PI / 180f));
				SporeThorns[j].transform.position = new Vector3(x, position.y, z);
				lsPosition.Add(SporeThorns[j].transform.position);
			}
			for (int k = 0; k < SporeThorns.Length && k < SporeThornsGroundParticles.Length; k++)
			{
				SporeThornsGroundParticles[k].transform.position = SporeThorns[k].transform.position;
			}
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

		public void ShowAttackThornGroundParticles(bool bShow)
		{
			for (int i = 0; i < SporeThornsGroundParticles.Length; i++)
			{
				SporeThornsGroundParticles[i].SetActiveRecursively(bShow);
			}
		}

		public void ShowSporeThorns(bool bShow)
		{
			for (int i = 0; i < SporeThorns.Length; i++)
			{
				SporeThorns[i].transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
				SporeThorns[i].SetActiveRecursively(bShow);
			}
		}

		public void ZoomInSporeThorns()
		{
			for (int i = 0; i < SporeThorns.Length; i++)
			{
				SporeThorns[i].GetComponent<Animation>()["Thorn_ZoomIn"].speed = 2f;
				SporeThorns[i].GetComponent<Animation>().Play("Thorn_ZoomIn");
			}
		}

		public void ZoomOutSporeThorns()
		{
			for (int i = 0; i < SporeThorns.Length; i++)
			{
				if (SporeThorns[i].active)
				{
					SporeThorns[i].GetComponent<Animation>()["Thorn_ZoomOut"].speed = 2f;
					SporeThorns[i].GetComponent<Animation>().Play("Thorn_ZoomOut");
				}
			}
		}

		private void CheckPlayersHit(float damage)
		{
			Player player = GameApp.GetInstance().GetGameScene().GetPlayer();
			Player friendPlayer = GameApp.GetInstance().GetGameScene().GetFriendPlayer();
			for (int i = 0; i < SporeThorns.Length; i++)
			{
				if (Vector3.Distance(player.GetTransform().position, SporeThorns[i].transform.position) < 1f && Time.time - fLastHitTimer >= 0.2f)
				{
					Vector3 dir = player.GetTransform().position - SporeThorns[i].transform.position;
					player.OnHitBack(0.3f, 0.8f, dir);
					player.OnHit(damage);
					fLastHitTimer = Time.time;
				}
				if (friendPlayer != null && Vector3.Distance(friendPlayer.GetTransform().position, SporeThorns[i].transform.position) < 1f)
				{
					Vector3 vector = friendPlayer.GetTransform().position - SporeThorns[i].transform.position;
					friendPlayer.OnHit(damage);
				}
			}
		}

		public override void Clear()
		{
			base.Clear();
			GameObject[] sporeThornsGroundParticles = SporeThornsGroundParticles;
			foreach (GameObject obj in sporeThornsGroundParticles)
			{
				UnityEngine.Object.Destroy(obj);
			}
			GameObject[] sporeThorns = SporeThorns;
			foreach (GameObject obj2 in sporeThorns)
			{
				UnityEngine.Object.Destroy(obj2);
			}
		}
	}
}
