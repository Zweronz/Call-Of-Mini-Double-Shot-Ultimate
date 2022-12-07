using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zombie3D;

public class DDSMap3TriggerScript : DDSTriggerScript
{
	private FixedConfig.NPCWaveCfg curWave;

	private int subWaveIndex = -1;

	private ArrayList allEnemysLeft;

	protected float spawnSpeed;

	private float subWaveSpawnTimer;

	private float subWaveDeltaSpawnTime;

	private float spawnCircleDynAngle;

	private float waveRefreshEndTimer = -1f;

	private float m_WaveRefreshTimeInterval = 20f;

	public FixedDoor[] fixedDoors;

	private Enemy m_SporeBoss;

	private float m_SpecialSporeRefreshTimer = -1f;

	private float m_SpecialSporeRefreshTime = 5f;

	public override void Init()
	{
		base.Init();
		spawnSpeed = 2f;
		m_SpecialSporeRefreshTimer = 0f;
	}

	public override void PullTrigger()
	{
		base.PullTrigger();
		int num = pointsIndex * 10;
		int wave_index = Random.Range(num - 9, num);
		PlayWave(mapIndex, pointsIndex, wave_index);
	}

	private IEnumerator Start()
	{
		yield return 0;
		Init();
	}

	private void Update()
	{
		if (m_SpecialSporeRefreshTimer < 0f && m_SporeBoss != null && !gameScene.GetEnemies().ContainsValue(m_SporeBoss))
		{
			return;
		}
		if (base.WaveRefreshEnd)
		{
			if (GameApp.GetInstance().GetGameScene().GetEnemies()
				.Count == 1 && m_SporeBoss != null && gameScene.GetEnemies().ContainsValue(m_SporeBoss) && waveRefreshEndTimer < 0f)
			{
				waveRefreshEndTimer = 0f;
			}
			if (waveRefreshEndTimer >= 0f)
			{
				waveRefreshEndTimer += Time.deltaTime;
				if (waveRefreshEndTimer > m_WaveRefreshTimeInterval)
				{
					waveRefreshEndTimer = -1f;
					Debug.Log("Next wave......................");
					base.WaveRefreshEnd = false;
					int num = pointsIndex * 10;
					int wave_index = Random.Range(num - 9, num);
					PlayWave(mapIndex, pointsIndex, wave_index);
					GameApp.GetInstance().Save();
				}
			}
		}
		else
		{
			if (!bPlaying)
			{
				return;
			}
			if (m_SpecialSporeRefreshTimer >= 0f)
			{
				m_SpecialSporeRefreshTimer += Time.deltaTime;
				if (m_SpecialSporeRefreshTimer > 5f)
				{
					m_SpecialSporeRefreshTimer = -1f;
					spawns[0].enemyType = EnemyType.E_SPORE;
					spawns[0].Spawn(1);
					m_SporeBoss = null;
					foreach (string key in gameScene.GetEnemies().Keys)
					{
						Enemy enemy = gameScene.GetEnemies()[key] as Enemy;
						if (enemy.EnemyType == EnemyType.E_SPORE)
						{
							m_SporeBoss = enemy;
							break;
						}
					}
					if (m_SporeBoss == null)
					{
						Debug.LogError("Cannot Generate a Spore!!!");
					}
				}
			}
			int num2 = 0;
			Hashtable enemies = GameApp.GetInstance().GetGameScene().GetEnemies();
			foreach (Enemy value in enemies.Values)
			{
				if (value.HP > 0f)
				{
					num2++;
				}
			}
			if (num2 < emenySpawnLimit)
			{
				subWaveSpawnTimer += Time.deltaTime;
				subWaveDeltaSpawnTime += Time.deltaTime;
				if (subWaveDeltaSpawnTime < spawnSpeed || GameApp.GetInstance().GetGameScene().GetPlayer() == null || GameApp.GetInstance().GetGameScene().PlayingState != 0)
				{
					return;
				}
				if (subWaveIndex < curWave.subWaveTimes.Length)
				{
					int num3 = curWave.subWaveTimes[subWaveIndex];
					if (subWaveSpawnTimer <= (float)num3 && allEnemysLeft.Count > 0)
					{
						subWaveDeltaSpawnTime = 0f;
						int index = Random.Range(0, allEnemysLeft.Count);
						EnemyType enemyType = EnemyType.E_ZOMBIE;
						enemyType = (EnemyType)int.Parse(allEnemysLeft[index].ToString());
						allEnemysLeft.RemoveAt(index);
						EnemySpawnScript oneEnemySpawn = GetOneEnemySpawn();
						oneEnemySpawn.enemyType = enemyType;
						oneEnemySpawn.Spawn(1);
						if (allEnemysLeft.Count == 0 && subWaveIndex >= curWave.subWaveTimes.Length - 1)
						{
							bPlaying = false;
							base.WaveRefreshEnd = true;
							Debug.Log("Wave Refresh end......................");
						}
						return;
					}
					subWaveIndex++;
					subWaveSpawnTimer = 0f;
					if (subWaveIndex < curWave.subWaveTimes.Length)
					{
						allEnemysLeft = new ArrayList();
						Hashtable hashtable = (Hashtable)curWave.enemyInfo[subWaveIndex];
						foreach (string key2 in hashtable.Keys)
						{
							int num4 = int.Parse(key2);
							int num5 = int.Parse(hashtable[key2].ToString());
							for (int i = 0; i < num5; i++)
							{
								allEnemysLeft.Add(num4);
							}
						}
						int num6 = curWave.subWaveTimes[0];
						spawnSpeed = (float)(num6 - 2) / (float)allEnemysLeft.Count;
						subWaveSpawnTimer = 0f;
						subWaveDeltaSpawnTime = 0f;
					}
					else
					{
						bPlaying = false;
						base.WaveRefreshEnd = true;
						Debug.Log("Wave Refresh end......................");
					}
				}
				else
				{
					bPlaying = false;
					base.WaveRefreshEnd = true;
					Debug.Log("Wave Refresh end......................");
				}
			}
			else
			{
				subWaveDeltaSpawnTime += Time.deltaTime;
			}
		}
	}

	private void OnDrawGizmos()
	{
	}

	public override void PlayWave(int map_index, int points_index, int wave_index)
	{
		Debug.Log("PlayWave() - " + map_index + "|" + points_index + "|" + wave_index);
		base.PlayWave(map_index, points_index, wave_index);
		curWave = ConfigManager.Instance().GetFixedConfig().GetNPCWaveConfig(map_index, points_index, wave_index);
		if (curWave != null)
		{
			bPlaying = true;
			subWaveIndex = 0;
			allEnemysLeft = new ArrayList();
			Hashtable hashtable = (Hashtable)curWave.enemyInfo[0];
			foreach (string key in hashtable.Keys)
			{
				int num = int.Parse(key);
				int num2 = int.Parse(hashtable[key].ToString());
				for (int i = 0; i < num2; i++)
				{
					allEnemysLeft.Add(num);
				}
			}
			int num3 = curWave.subWaveTimes[0];
			spawnSpeed = (float)(num3 - 2) / (float)allEnemysLeft.Count;
			subWaveSpawnTimer = 0f;
			subWaveDeltaSpawnTime = 0f;
		}
		else
		{
			Debug.LogError("Get wave config ERROR!!!");
		}
		gameScene.StartWave();
	}

	public EnemySpawnScript GetOneEnemySpawn()
	{
		EnemySpawnScript enemySpawnScript = null;
		List<EnemySpawnScript> list = new List<EnemySpawnScript>();
		for (int i = 0; i < spawns.Length; i++)
		{
			float num = Vector3.Angle((spawns[i].transform.position - player.GetTransform().position).normalized, player.GetTransform().forward.normalized);
			if (num < 180f)
			{
				float num2 = Vector3.Distance(spawns[i].transform.position, player.GetTransform().position);
				if (num2 > 15f && num2 <= 30f)
				{
					list.Add(spawns[i]);
				}
			}
		}
		if (list.Count < 1)
		{
			Debug.LogError("ERROR: Cannot find an EnemySpawnScript!!! " + player.GetTransform().position);
			for (int j = 0; j < spawns.Length; j++)
			{
				float num3 = Vector3.Angle((spawns[j].transform.position - player.GetTransform().position).normalized, player.GetTransform().forward.normalized);
				float num4 = Vector3.Distance(spawns[j].transform.position, player.GetTransform().position);
				if (num4 > 15f)
				{
					list.Add(spawns[j]);
				}
			}
			if (list.Count > 0)
			{
				enemySpawnScript = list[Random.Range(0, list.Count)];
			}
		}
		else
		{
			int index = Random.Range(0, list.Count);
			enemySpawnScript = list[index];
		}
		if (enemySpawnScript == null)
		{
			list.Clear();
			for (int k = 0; k < spawns.Length; k++)
			{
				float num5 = Vector3.Angle((spawns[k].transform.position - player.GetTransform().position).normalized, player.GetTransform().forward.normalized);
				float num6 = Vector3.Distance(spawns[k].transform.position, player.GetTransform().position);
				if (num6 > 15f)
				{
					list.Add(spawns[k]);
				}
			}
			if (list.Count > 0)
			{
				int index2 = Random.Range(0, list.Count);
				enemySpawnScript = list[index2];
			}
			else
			{
				int num7 = Random.Range(0, spawns.Length);
				enemySpawnScript = spawns[num7];
			}
		}
		return enemySpawnScript;
	}
}
