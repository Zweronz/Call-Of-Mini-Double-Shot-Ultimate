using System.Collections.Generic;
using UnityEngine;
using Zombie3D;

public class Custom : Enemy
{
	private float fLastUpdateTargetTime;

	private List<EnemySkillImpl> lsEnemyskill = new List<EnemySkillImpl>();

	private EnemyType m_enemyType = EnemyType.E_ZOMBIE;

	public void Init(GameObject gObject, EnemyType enemyType, List<KeyValuePair<enEnemySkillType, int>> skillID)
	{
		m_enemyType = enemyType;
		base.Init(gObject);
		switch (enemyType)
		{
		case EnemyType.E_LAVA:
			runAnimationName = "Forward01";
			enemyObject.transform.Find("LavaFireEffect").gameObject.SetActiveRecursively(false);
			break;
		case EnemyType.E_INFECTER:
			runAnimationName = "Forward01";
			break;
		case EnemyType.E_HUNTER:
			runAnimationName = "Forward01";
			break;
		case EnemyType.E_BATCHER:
		{
			runAnimationName = "Forward01";
			Batcher_EffectTrail batcher_EffectTrail = gObject.transform.Find("Bip01/Bip01 Prop1/BatcherAxTail").GetComponent(typeof(Batcher_EffectTrail)) as Batcher_EffectTrail;
			batcher_EffectTrail.enabled = false;
			break;
		}
		}
		foreach (KeyValuePair<enEnemySkillType, int> item in skillID)
		{
			EnemySkillImpl enemySkillImpl = EnemySkillImplFactory.CreateSkillImpl(item.Key);
			enemySkillImpl.Init(this, new EnemySkill(item.Key, (uint)item.Value));
			lsEnemyskill.Add(enemySkillImpl);
		}
	}

	public override void DoLogic(float deltaTime)
	{
		base.DoLogic(deltaTime);
		if (Time.time - fLastUpdateTargetTime >= 3f)
		{
			List<GameObject> list = new List<GameObject>();
			if (GameApp.GetInstance().GetGameState().m_eGameMode.m_ePlayMode != 0)
			{
				foreach (Player recipientPlayer in PlayerManager.Instance.GetRecipientPlayerList())
				{
					list.Add(recipientPlayer.PlayerObject);
				}
			}
			list.Add(player.PlayerObject);
			GameObject gameObject = TargetConfirmed(list);
			UpdateTarget(gameObject, gameObject.transform);
			fLastUpdateTargetTime = Time.time;
		}
		foreach (EnemySkillImpl item in lsEnemyskill)
		{
			item.Dologic(deltaTime);
		}
	}

	public void DologicExpression(float deltaTime)
	{
		foreach (EnemySkillImpl item in lsEnemyskill)
		{
			item.DologicExpression(deltaTime);
		}
		if (bloodRect != null)
		{
			bloodRect.SetBloodPercent(base.HP / maxHp);
		}
	}

	public override void OnAttack()
	{
		base.OnAttack();
		List<EnemySkillImpl> list = new List<EnemySkillImpl>();
		foreach (EnemySkillImpl item in lsEnemyskill)
		{
			if (item.GetSkill().SkillType != enEnemySkillType.E_Spikeweed || item.GetSkill().SkillType != enEnemySkillType.E_ExplodeSpore)
			{
				list.Add(item);
			}
		}
		if (list.Count > 0)
		{
			int index = Random.Range(0, list.Count);
			EnemySkillImpl enemySkillImpl = list[index];
			enemySkillImpl.bCanAttack = true;
		}
		else
		{
			Debug.LogWarning(enemyObject.name + "No Other Skill");
		}
		if (m_enemyType == EnemyType.E_LAVA)
		{
			Animate("Attack01", WrapMode.Loop);
		}
		else if (m_enemyType == EnemyType.E_INFECTER)
		{
			Animate("Attack_LongRange01", WrapMode.Once);
		}
		else if (m_enemyType == EnemyType.E_HUNTER)
		{
			Animate("Fire01", WrapMode.Once);
		}
		else if (m_enemyType == EnemyType.E_BATCHER)
		{
			Animate("Attack01", WrapMode.Once);
		}
		else
		{
			Debug.LogWarning("Wrong m_enemyType " + m_enemyType);
		}
		lastAttackTime = Time.time;
	}

	public override bool AttackAnimationEnds()
	{
		if (m_enemyType == EnemyType.E_INFECTER)
		{
			string text = "Attack_LongRange01";
			if (base.SqrDistanceFromPlayer < base.AttackRange * base.AttackRange)
			{
				text = "Attack_LongRange01";
			}
			if (Time.time - lastAttackTime > enemyObject.GetComponent<Animation>()[text].length)
			{
				return true;
			}
			return false;
		}
		if (m_enemyType == EnemyType.E_LAVA)
		{
			return base.AttackAnimationEnds();
		}
		if (m_enemyType == EnemyType.E_HUNTER)
		{
			if (Time.time - lastAttackTime > enemyObject.GetComponent<Animation>()["Fire01"].length)
			{
				return true;
			}
			return false;
		}
		if (m_enemyType == EnemyType.E_BATCHER)
		{
			return base.AttackAnimationEnds();
		}
		return base.AttackAnimationEnds();
	}

	public GameObject TargetConfirmed(List<GameObject> lsGO)
	{
		float num = 9999f;
		GameObject gameObject = null;
		foreach (GameObject item in lsGO)
		{
			if (item == PlayerManager.Instance.GetPlayerObject())
			{
				if (PlayerManager.Instance.GetPlayerClass().HP <= 0f)
				{
					continue;
				}
			}
			else if (PlayerManager.Instance.GetRecipientByObj(item).HP <= 0f)
			{
				continue;
			}
			float sqrMagnitude = (item.transform.position - enemyTransform.position).sqrMagnitude;
			if (sqrMagnitude < num)
			{
				gameObject = item;
				num = sqrMagnitude;
			}
		}
		if (gameObject == null)
		{
			gameObject = PlayerManager.Instance.GetPlayerObject();
			Debug.LogWarning("Target Player = null");
		}
		return gameObject;
	}

	public EnemySkillImpl GetEnemySkillImplByID(enEnemySkillType type)
	{
		foreach (EnemySkillImpl item in lsEnemyskill)
		{
			if (item.GetSkill().SkillType == type)
			{
				return item;
			}
		}
		return null;
	}

	public void SkillClear()
	{
		foreach (EnemySkillImpl item in lsEnemyskill)
		{
			item.Clear();
		}
	}
}
