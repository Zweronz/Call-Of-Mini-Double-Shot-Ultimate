namespace Zombie3D
{
	public class EnemySkillImpl
	{
		protected Enemy m_OwnerEnemy;

		protected EnemySkill m_Skill;

		public bool bCanAttack;

		protected float m_fCoolDown = 3.4f;

		protected float m_fLastAttackTime;

		protected GameConfigScript gConfig;

		public EnemySkill GetSkill()
		{
			return m_Skill;
		}

		public virtual void Init(Enemy ownerEnemy, EnemySkill skill)
		{
			m_OwnerEnemy = ownerEnemy;
			m_Skill = skill;
			gConfig = GameApp.GetInstance().GetGameConfig();
		}

		public virtual void Shoot()
		{
		}

		public virtual void Dologic(float deltaTime)
		{
		}

		public virtual void DologicExpression(float deltaTime)
		{
		}

		public virtual void Clear()
		{
		}
	}
}
