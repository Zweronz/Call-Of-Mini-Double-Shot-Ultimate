namespace Zombie3D
{
	public class EnemyFactory
	{
		protected static EnemyFactory instance;

		public static EnemyFactory GetInstance()
		{
			if (instance == null)
			{
				instance = new EnemyFactory();
			}
			return instance;
		}

		public Enemy CreateEnemy(EnemyType enemyType)
		{
			Enemy enemy = null;
			switch (enemyType)
			{
			case EnemyType.E_ZOMBIE:
				enemy = new Zombie();
				break;
			case EnemyType.E_BOOMER:
				enemy = new Boomer();
				break;
			case EnemyType.E_SWAT:
				enemy = new Swat();
				break;
			case EnemyType.E_LAVA:
				enemy = new Lava();
				break;
			case EnemyType.E_INFECTER:
				enemy = new Infecter();
				break;
			case EnemyType.E_SPIDER:
				enemy = new Spider();
				break;
			case EnemyType.E_HUNTER:
				enemy = new Hunter();
				break;
			case EnemyType.E_LASER:
				enemy = new Laser();
				break;
			case EnemyType.E_BATCHER:
				enemy = new Batcher();
				break;
			case EnemyType.E_TRACKER:
				enemy = new Tracker();
				break;
			case EnemyType.E_TURRETER:
				enemy = new Turreter();
				break;
			case EnemyType.E_SPORE:
				enemy = new Spore();
				break;
			case EnemyType.E_VAMPIREDOG:
				enemy = new VampireDog();
				break;
			case EnemyType.E_HUNTER_II:
				enemy = new HunterII();
				break;
			case EnemyType.E_SPORE_II:
				enemy = new SporeII();
				break;
			case EnemyType.E_CustomBoss:
				enemy = new Custom();
				break;
			default:
				enemy = new Zombie();
				break;
			}
			enemy.EnemyType = enemyType;
			return enemy;
		}
	}
}
