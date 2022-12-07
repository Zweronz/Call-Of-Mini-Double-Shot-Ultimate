using System.Collections;
using UnityEngine;
using Zombie3D;

public class TriggerScript : MonoBehaviour
{
	protected Transform triggerTransform;

	protected Player player;

	protected bool triggered;

	protected bool alreadyMaxSpawned;

	protected bool hasSecondarySpawns;

	public Transform SecondPosition;

	public int minEnemy;

	public int maxSpawn;

	protected int currentEnemyNum;

	protected int spawnedNum;

	public float radius;

	public EnemySpawnScript[] spawns = new EnemySpawnScript[5];

	public EnemySpawnScript[] secondarySpawns = new EnemySpawnScript[5];

	protected GameScene gameScene;

	protected float lastUpdateTime = -1000f;

	public bool AlreadyMaxSpawned
	{
		get
		{
			return alreadyMaxSpawned;
		}
	}

	private IEnumerator Start()
	{
		yield return 0;
		triggerTransform = base.gameObject.transform;
		triggered = false;
		EnemySpawnScript[] array = spawns;
		foreach (EnemySpawnScript es in array)
		{
			if (es != null)
			{
				es.TriggerBelongsto = this;
			}
		}
		hasSecondarySpawns = false;
		EnemySpawnScript[] array2 = secondarySpawns;
		foreach (EnemySpawnScript es2 in array2)
		{
			if (es2 != null)
			{
				es2.TriggerBelongsto = this;
				hasSecondarySpawns = true;
			}
		}
		alreadyMaxSpawned = false;
		GameApp.GetInstance().GetGameScene().AddTrigger(this);
		gameScene = GameApp.GetInstance().GetGameScene();
	}

	private void Update()
	{
		if (Time.time - lastUpdateTime < 1f || triggerTransform == null)
		{
			return;
		}
		lastUpdateTime = Time.time;
		if (!triggered)
		{
			player = gameScene.GetPlayer();
			bool flag = false;
			if (SecondPosition != null && (player.GetTransform().position - SecondPosition.position).sqrMagnitude <= radius * radius)
			{
				flag = true;
			}
			if (!((player.GetTransform().position - triggerTransform.position).sqrMagnitude <= radius * radius) && !flag)
			{
				return;
			}
			EnemySpawnScript[] array = spawns;
			foreach (EnemySpawnScript enemySpawnScript in array)
			{
				if (enemySpawnScript != null)
				{
					enemySpawnScript.Spawn(1);
					currentEnemyNum++;
					spawnedNum++;
				}
			}
			triggered = true;
		}
		else if (spawnedNum < maxSpawn && hasSecondarySpawns)
		{
			if (currentEnemyNum > minEnemy)
			{
				return;
			}
			EnemySpawnScript[] array2 = secondarySpawns;
			foreach (EnemySpawnScript enemySpawnScript2 in array2)
			{
				if (enemySpawnScript2 != null)
				{
					enemySpawnScript2.Spawn(1);
					currentEnemyNum++;
					spawnedNum++;
				}
			}
		}
		else
		{
			alreadyMaxSpawned = true;
		}
	}

	public void EnemyKilled()
	{
		currentEnemyNum--;
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawSphere(base.transform.position, 0.3f);
		Gizmos.color = Color.white;
		Gizmos.DrawWireSphere(base.transform.position, radius);
		if (SecondPosition != null)
		{
			Gizmos.DrawWireSphere(SecondPosition.position, radius);
		}
		if (gameScene == null)
		{
			return;
		}
		if (gameScene.GetEnemies() != null)
		{
			foreach (Enemy value in gameScene.GetEnemies().Values)
			{
				if (value.LastTarget != Vector3.zero)
				{
					Gizmos.color = Color.blue;
					Gizmos.DrawSphere(value.LastTarget, 0.3f);
					Gizmos.DrawLine(value.ray.origin, value.rayhit.point);
				}
			}
		}
		Vector3[] path = gameScene.GetPath();
		if (path != null && path.Length > 0)
		{
			Vector3 from = path[path.Length - 1];
			Vector3[] array = path;
			foreach (Vector3 vector in array)
			{
				Gizmos.color = Color.white;
				Gizmos.DrawSphere(vector, 0.1f);
				Gizmos.DrawLine(from, vector);
				from = vector;
			}
		}
	}
}
