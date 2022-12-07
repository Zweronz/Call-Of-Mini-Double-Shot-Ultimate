using UnityEngine;

namespace Zombie3D
{
	public class SkillHailMary : SkillImpl
	{
		private GameObject m_GrenadeObj;

		public float m_HailMaryHp = 5f;

		public float m_HailMaryHpPercent = 0.01f;

		public override void Init(Player player, Skill skill)
		{
			base.Init(player, skill);
			m_HailMaryHp = 5f;
			m_HailMaryHpPercent = 0.01f;
			switch (skill.Level)
			{
			case 1u:
				m_HailMaryHpPercent = 0.01f;
				break;
			case 2u:
				m_HailMaryHpPercent = 0.02f;
				break;
			case 3u:
				m_HailMaryHpPercent = 0.04f;
				break;
			case 4u:
				m_HailMaryHpPercent = 0.07f;
				break;
			case 5u:
				m_HailMaryHpPercent = 0.1f;
				break;
			case 6u:
				m_HailMaryHpPercent = 0.11f;
				break;
			case 7u:
				m_HailMaryHpPercent = 0.12f;
				break;
			case 8u:
				m_HailMaryHpPercent = 0.13f;
				break;
			case 9u:
				m_HailMaryHpPercent = 0.14f;
				break;
			case 10u:
				m_HailMaryHpPercent = 0.15f;
				break;
			default:
				m_HailMaryHpPercent = 0.01f;
				break;
			}
		}

		public override void Update(float deltaTime)
		{
			base.Update(deltaTime);
		}
	}
}
