using UnityEngine;

namespace Zombie3D
{
	public class SkillImpl
	{
		protected Player m_Player;

		protected Skill m_Skill;

		private float m_SkillStaminaLoseSpeed;

		public float SkillStanimaLoseSpeed
		{
			get
			{
				return m_SkillStaminaLoseSpeed;
			}
			set
			{
				m_SkillStaminaLoseSpeed = value;
			}
		}

		public Skill GetSkill()
		{
			return m_Skill;
		}

		public virtual bool CheckStart()
		{
			return true;
		}

		public virtual void Init(Player player, Skill skill)
		{
			m_Player = player;
			m_Skill = skill;
		}

		public virtual void Update(float deltaTime)
		{
			if (SkillStanimaLoseSpeed > 0f)
			{
				m_Player.Stamina -= SkillStanimaLoseSpeed * deltaTime;
				m_Player.Stamina = Mathf.Clamp(m_Player.Stamina, 0f, m_Player.GetMaxStamina());
			}
		}

		public virtual void Stop()
		{
		}
	}
}
