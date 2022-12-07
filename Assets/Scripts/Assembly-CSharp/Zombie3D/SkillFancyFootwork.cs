using UnityEngine;

namespace Zombie3D
{
	public class SkillFancyFootwork : SkillImpl
	{
		private GameObject m_GrenadeObj;

		public float m_FancyPercent = 0.01f;

		public override void Init(Player player, Skill skill)
		{
			base.Init(player, skill);
			m_FancyPercent = 0.03f;
			switch (skill.Level)
			{
			case 1u:
				m_FancyPercent = 0.03f;
				break;
			case 2u:
				m_FancyPercent = 0.07f;
				break;
			case 3u:
				m_FancyPercent = 0.12f;
				break;
			case 4u:
				m_FancyPercent = 0.18f;
				break;
			case 5u:
				m_FancyPercent = 0.25f;
				break;
			case 6u:
				m_FancyPercent = 0.27f;
				break;
			case 7u:
				m_FancyPercent = 0.29f;
				break;
			case 8u:
				m_FancyPercent = 0.31f;
				break;
			case 9u:
				m_FancyPercent = 0.33f;
				break;
			case 10u:
				m_FancyPercent = 0.35f;
				break;
			default:
				m_FancyPercent = 0.03f;
				break;
			}
		}

		public override void Update(float deltaTime)
		{
			base.Update(deltaTime);
		}
	}
}
