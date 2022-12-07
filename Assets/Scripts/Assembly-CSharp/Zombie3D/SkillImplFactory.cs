namespace Zombie3D
{
	public class SkillImplFactory
	{
		public static SkillImpl CreateSkillImpl(enSkillType skill_type)
		{
			SkillImpl result = null;
			switch (skill_type)
			{
			case enSkillType.FastRun:
				result = new SkillFastRun();
				break;
			case enSkillType.BuildCannon:
				result = new SkillBuildCannon();
				break;
			case enSkillType.ThrowGrenade:
				result = new SkillThrowGrenade();
				break;
			case enSkillType.CoverMe:
				result = new SkillCoverMe();
				break;
			case enSkillType.DoubleTeam:
				result = new SkillDoubleTeam();
				break;
			case enSkillType.KillShot:
				result = new SkillKillShot();
				break;
			case enSkillType.FancyFootwork:
				result = new SkillFancyFootwork();
				break;
			case enSkillType.HailMary:
				result = new SkillHailMary();
				break;
			case enSkillType.MachoMan:
				result = new SkillMachoMan();
				break;
			}
			return result;
		}
	}
}
