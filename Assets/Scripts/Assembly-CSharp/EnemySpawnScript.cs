using UnityEngine;
using Zombie3D;

public class EnemySpawnScript : MonoBehaviour
{
	public EnemyType enemyType;

	public bool isKilled;

	public bool m_bFixedDoor;

	protected float lastSpawnTime;

	protected TriggerScript triggerBelongsto;

	protected static GameObject enemyFolder;

	protected bool disable;

	public Color m_GizmosColor = Color.red;

	public TriggerScript TriggerBelongsto
	{
		set
		{
			triggerBelongsto = value;
		}
	}

	private void Start()
	{
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = m_GizmosColor;
		Gizmos.DrawSphere(base.transform.position, 0.3f);
	}

	public void Spawn(int spawnNum)
	{
		if (GameApp.GetInstance().GetGameScene() == null || disable)
		{
			return;
		}
		GameObject original = GameApp.GetInstance().GetGameConfig().enemy[(int)(enemyType - 1)];
		for (int i = 0; i < spawnNum; i++)
		{
			Enemy enemy = EnemyFactory.GetInstance().CreateEnemy(enemyType);
			if (enemy != null)
			{
				GameObject gameObject = (GameObject)Object.Instantiate(original, base.transform.position, Quaternion.Euler(0f, 0f, 0f));
				gameObject.name = "E_" + GameApp.GetInstance().GetGameScene().GetNextEnemyID();
				enemy.EnemyType = enemyType;
				enemy.Init(gameObject);
				enemy.Spawn = this;
				enemy.Name = gameObject.name;
				GameApp.GetInstance().GetGameScene().GetEnemies()
					.Add(gameObject.name, enemy);
				GameApp.GetInstance().GetGameScene().ModifyEnemyNum(1);
			}
		}
		lastSpawnTime = Time.time;
	}

	private void OnResetSpawnTrigger()
	{
		if (triggerBelongsto != null)
		{
			triggerBelongsto.EnemyKilled();
		}
		isKilled = true;
	}
}
