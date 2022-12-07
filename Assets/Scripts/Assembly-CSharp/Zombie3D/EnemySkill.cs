namespace Zombie3D
{
	public class EnemySkill
	{
		public enEnemySkillType SkillType { get; set; }

		public uint Level { get; set; }

		public EnemySkill()
		{
			SkillType = enEnemySkillType.E_None;
			Level = 1u;
		}

		public EnemySkill(enEnemySkillType type, uint level)
		{
			SkillType = type;
			Level = level;
		}
	}
}
