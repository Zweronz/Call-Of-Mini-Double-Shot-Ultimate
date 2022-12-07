using UnityEngine;

namespace Zombie3D
{
	public class SkillFastRun : SkillImpl
	{
		protected float m_SpeedAdd = 0.1f;

		public float SpeedAdd
		{
			get
			{
				return m_SpeedAdd;
			}
			set
			{
				m_SpeedAdd = value;
			}
		}

		public override void Init(Player player, Skill skill)
		{
			base.Init(player, skill);
			float value = 0.1f + (float)(skill.Level - 1) * 0.05f;
			SpeedAdd = Mathf.Clamp(value, 0.1f, 1f);
			base.SkillStanimaLoseSpeed = 5f;
		}

		public override void Update(float deltaTime)
		{
			base.Update(deltaTime);
			if (m_Player.Stamina <= 0f)
			{
				m_Player.TerminateActiveSkill();
			}
		}

		public override void Stop()
		{
		}
	}
}
