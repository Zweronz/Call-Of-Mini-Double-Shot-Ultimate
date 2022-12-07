namespace Zombie3D
{
	public class Skill
	{
		public enSkillType SkillType { get; set; }

		public uint Level { get; set; }

		public Skill()
		{
			SkillType = enSkillType.FastRun;
			Level = 1u;
		}

		public Skill(enSkillType type, uint level)
		{
			SkillType = type;
			Level = level;
		}
	}
}
