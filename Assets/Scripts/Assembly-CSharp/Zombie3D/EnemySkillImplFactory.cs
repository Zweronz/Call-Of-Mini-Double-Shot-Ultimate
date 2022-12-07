namespace Zombie3D
{
	public class EnemySkillImplFactory
	{
		public static EnemySkillImpl CreateSkillImpl(enEnemySkillType skill_type)
		{
			EnemySkillImpl result = null;
			switch (skill_type)
			{
			case enEnemySkillType.E_ExplodeSpore:
				result = new EnemySkillExplodeSpore();
				break;
			case enEnemySkillType.E_Spikeweed:
				result = new EnemySkillSpikeweed();
				break;
			case enEnemySkillType.E_GasBomb:
				result = new EnemySkillGasbomb();
				break;
			case enEnemySkillType.E_Ionized:
				result = new EnemySkillIonized();
				break;
			case enEnemySkillType.E_StingOut:
				result = new EnemySkillStingOut();
				break;
			}
			return result;
		}
	}
}
