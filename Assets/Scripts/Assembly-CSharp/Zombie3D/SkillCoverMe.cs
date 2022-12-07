using UnityEngine;

namespace Zombie3D
{
	public class SkillCoverMe : SkillImpl
	{
		private GameObject m_GrenadeObj;

		public float m_FriendAttackAdd = 0.1f;

		public override void Init(Player player, Skill skill)
		{
			base.Init(player, skill);
			base.SkillStanimaLoseSpeed = 7f;
			m_FriendAttackAdd = 0.1f + (float)(m_Skill.Level - 1) * 0.1f;
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
