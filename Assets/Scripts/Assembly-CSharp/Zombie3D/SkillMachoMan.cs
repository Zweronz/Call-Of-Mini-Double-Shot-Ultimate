using UnityEngine;

namespace Zombie3D
{
	public class SkillMachoMan : SkillImpl
	{
		private GameObject m_GrenadeObj;

		public float m_MaxStaminaAdd = 10f;

		public override void Init(Player player, Skill skill)
		{
			base.Init(player, skill);
			m_MaxStaminaAdd = 10f + (float)(m_Skill.Level - 1) * 10f;
		}

		public override void Update(float deltaTime)
		{
			base.Update(deltaTime);
		}
	}
}
