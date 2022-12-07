using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zombie3D;

public class DDSWaveTriggerScript : DDSTriggerScript
{
	private FixedConfig.NPCWaveCfg curWave;

	private int subWaveIndex = -1;

	protected List<EnemySpawnScript> dynSpawns;

	private float m_DynSpawnsLastTime;

	private ArrayList allEnemysLeft;

	protected float spawnSpeed;

	private float subWaveSpawnTimer;

	private float subWaveDeltaSpawnTime;

	private float spawnCircleDynAngle;

	private float waveRefreshEndTimer = -1f;

	public FixedDoor[] fixedDoors;

	private float m_LastSpawnsInScreenTime;

	private float m_NextSpawnsInScreenTime = 1f;

	private int m_curWaveTotalCount = 1;

	private int m_CurWaveGenCount;

	private int m_CurWaveExternEnemiesCount;

	public override void Init()
	{
		base.Init();
		spawnSpeed = 2f;
		base.AllEnemiesOfCurWave = 1;
		base.GenEnemiesCountOfCurWave = 0;
		base.GenExternEnemiesCountOfCurWave = 0;
	}

	public override void PullTrigger()
	{
		base.PullTrigger();
		PlayWave(mapIndex, pointsIndex, waveIndex);
		m_DynSpawnsLastTime = Time.time;
		m_LastSpawnsInScreenTime = Time.time;
		base.AllEnemiesOfCurWave = CalcCurPointsTotalEnemies();
	}

	private IEnumerator Start()
	{
		yield return 0;
		Init();
		RefreshDynSpawns();
	}

	private void Update()
	{
		if (base.WaveRefreshEnd)
		{
			if (GameApp.GetInstance().GetGameScene().GetEnemies()
				.Count == 0 && waveRefreshEndTimer < 0f)
			{
				waveRefreshEndTimer = 0f;
			}
			if (!(waveRefreshEndTimer >= 0f))
			{
				return;
			}
			waveRefreshEndTimer += Time.deltaTime;
			if (waveRefreshEndTimer > 4f)
			{
				waveRefreshEndTimer = -1f;
				Debug.Log("Next wave......................");
				base.WaveRefreshEnd = false;
				int maxWavesOfPoints = ConfigManager.Instance().GetFixedConfig().GetMaxWavesOfPoints(base.MapIndex, base.PointsIndex);
				if (base.WaveIndex < maxWavesOfPoints)
				{
					base.WaveIndex++;
					PlayWave(base.MapIndex, base.PointsIndex, base.WaveIndex);
				}
				else
				{
					gameScene.BattleEnd();
					base.WaveRefreshEnd = false;
					bPlaying = false;
				}
				GameApp.GetInstance().Save();
			}
		}
		else
		{
			if (!bPlaying)
			{
				return;
			}
			if (Time.time - m_DynSpawnsLastTime > 5f)
			{
				RefreshDynSpawns();
			}
			int num = 0;
			Hashtable enemies = GameApp.GetInstance().GetGameScene().GetEnemies();
			foreach (Enemy value2 in enemies.Values)
			{
				if (value2.HP > 0f)
				{
					num++;
				}
			}
			if (num < emenySpawnLimit)
			{
				subWaveSpawnTimer += Time.deltaTime;
				subWaveDeltaSpawnTime += Time.deltaTime;
				if (subWaveDeltaSpawnTime < spawnSpeed || GameApp.GetInstance().GetGameScene().GetPlayer() == null || GameApp.GetInstance().GetGameScene().PlayingState != 0)
				{
					return;
				}
				if (subWaveIndex < curWave.subWaveTimes.Length)
				{
					int num2 = curWave.subWaveTimes[subWaveIndex];
					if (allEnemysLeft.Count > 0)
					{
						subWaveDeltaSpawnTime = 0f;
						float value = (float)(m_curWaveTotalCount - m_CurWaveGenCount) / (float)m_curWaveTotalCount;
						value = Mathf.Clamp01(value);
						if (value > 0.1f && Time.time - m_LastSpawnsInScreenTime > m_NextSpawnsInScreenTime)
						{
							m_LastSpawnsInScreenTime = Time.time;
							m_NextSpawnsInScreenTime = Random.Range(0.7f, 1.3f);
							EnemySpawnScript oneEnemySpawnInScreen = GetOneEnemySpawnInScreen();
							if (oneEnemySpawnInScreen != null)
							{
								EnemyType enemyType = EnemyType.E_ZOMBIE;
								oneEnemySpawnInScreen.enemyType = enemyType;
								oneEnemySpawnInScreen.Spawn(1);
								GameObject gameObject = Object.Instantiate(Resources.Load("Zombie3D/Misc/GraveBornRock"), new Vector3(oneEnemySpawnInScreen.transform.position.x, 10000.199f, oneEnemySpawnInScreen.transform.position.z), Quaternion.identity) as GameObject;
								base.GenExternEnemiesCountOfCurWave++;
							}
						}
						int index = Random.Range(0, allEnemysLeft.Count);
						EnemyType enemyType2 = EnemyType.E_ZOMBIE;
						enemyType2 = (EnemyType)int.Parse(allEnemysLeft[index].ToString());
						EnemySpawnScript oneEnemySpawn = GetOneEnemySpawn();
						oneEnemySpawn.enemyType = enemyType2;
						oneEnemySpawn.Spawn(1);
						allEnemysLeft.RemoveAt(index);
						base.GenEnemiesCountOfCurWave++;
						m_CurWaveGenCount++;
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
						foreach (string key in hashtable.Keys)
						{
							int num3 = int.Parse(key);
							int num4 = int.Parse(hashtable[key].ToString());
							for (int i = 0; i < num4; i++)
							{
								allEnemysLeft.Add(num3);
							}
						}
						int num5 = curWave.subWaveTimes[0];
						spawnSpeed = (float)(num5 - 2) / (float)allEnemysLeft.Count;
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
		m_curWaveTotalCount = CalcCurWaveTotalEnemies();
		m_CurWaveGenCount = 0;
		m_CurWaveExternEnemiesCount = 0;
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

	private void RefreshDynSpawns()
	{
		dynSpawns = new List<EnemySpawnScript>();
		float num = 18f;
		for (int i = 0; i < spawns.Length; i++)
		{
			Vector2 vector = new Vector2(spawns[i].transform.position.x, spawns[i].transform.position.z);
			Vector2 vector2 = new Vector2(player.GetTransform().position.x, player.GetTransform().position.z);
			Vector2 vector3 = vector - vector2;
			float sqrMagnitude = (vector - vector2).sqrMagnitude;
			if (sqrMagnitude < num * num)
			{
				spawns[i].m_GizmosColor = Color.yellow;
				dynSpawns.Add(spawns[i]);
			}
			else
			{
				spawns[i].m_GizmosColor = Color.red;
			}
		}
		if (dynSpawns.Count < 5)
		{
			float num2 = 40f;
			for (int j = 0; j < spawns.Length; j++)
			{
				Vector2 vector4 = new Vector2(spawns[j].transform.position.x, spawns[j].transform.position.z);
				Vector2 vector5 = new Vector2(player.GetTransform().position.x, player.GetTransform().position.z);
				Vector2 vector6 = vector4 - vector5;
				float sqrMagnitude2 = (vector4 - vector5).sqrMagnitude;
				if (sqrMagnitude2 < num2 * num2)
				{
					spawns[j].m_GizmosColor = Color.yellow;
					dynSpawns.Add(spawns[j]);
				}
				else
				{
					spawns[j].m_GizmosColor = Color.red;
				}
			}
		}
		m_DynSpawnsLastTime = Time.time;
	}

	public EnemySpawnScript GetOneEnemySpawn()
	{
		EnemySpawnScript enemySpawnScript = null;
		List<EnemySpawnScript> list = new List<EnemySpawnScript>();
		for (int i = 0; i < dynSpawns.Count; i++)
		{
			Vector2 vector = new Vector2(dynSpawns[i].transform.position.x, dynSpawns[i].transform.position.z);
			Vector2 vector2 = new Vector2(player.GetTransform().position.x, player.GetTransform().position.z);
			Vector2 normalized = (vector - vector2).normalized;
			Vector2 vector3 = new Vector2(player.GetTransform().forward.x, player.GetTransform().forward.z);
			float num = Vector2.Angle(normalized, vector3.normalized);
			float sqrMagnitude = (vector - vector2).sqrMagnitude;
			if (sqrMagnitude > 225f && sqrMagnitude < 400f)
			{
				list.Add(dynSpawns[i]);
			}
		}
		if (list.Count < 1)
		{
			for (int j = 0; j < dynSpawns.Count; j++)
			{
				Vector2 vector4 = new Vector2(dynSpawns[j].transform.position.x, dynSpawns[j].transform.position.z);
				Vector2 vector5 = new Vector2(player.GetTransform().position.x, player.GetTransform().position.z);
				float num2 = 15f;
				float sqrMagnitude2 = (vector4 - vector5).sqrMagnitude;
				if (sqrMagnitude2 > num2 * num2)
				{
					list.Add(dynSpawns[j]);
				}
			}
			if (list.Count > 0)
			{
				int index = Random.Range(0, list.Count);
				enemySpawnScript = list[index];
			}
			else
			{
				int index2 = Random.Range(0, dynSpawns.Count);
				enemySpawnScript = dynSpawns[index2];
			}
		}
		else
		{
			int index3 = Random.Range(0, list.Count);
			enemySpawnScript = list[index3];
		}
		if (enemySpawnScript == null)
		{
			list.Clear();
			for (int k = 0; k < dynSpawns.Count; k++)
			{
				Vector2 vector6 = new Vector2(dynSpawns[k].transform.position.x, dynSpawns[k].transform.position.z);
				Vector2 vector7 = new Vector2(player.GetTransform().position.x, player.GetTransform().position.z);
				float num3 = 15f;
				float sqrMagnitude3 = (vector6 - vector7).sqrMagnitude;
				if (sqrMagnitude3 > num3 * num3)
				{
					list.Add(dynSpawns[k]);
				}
			}
			if (list.Count > 0)
			{
				int index4 = Random.Range(0, list.Count);
				enemySpawnScript = list[index4];
			}
			else
			{
				int index5 = Random.Range(0, dynSpawns.Count);
				enemySpawnScript = dynSpawns[index5];
			}
		}
		return enemySpawnScript;
	}

	private int CalcCurWaveTotalEnemies()
	{
		int num = 0;
		curWave = ConfigManager.Instance().GetFixedConfig().GetNPCWaveConfig(base.MapIndex, base.PointsIndex, base.WaveIndex);
		for (int i = 0; i < curWave.subWaveTimes.Length; i++)
		{
			Hashtable hashtable = (Hashtable)curWave.enemyInfo[i];
			foreach (string key in hashtable.Keys)
			{
				int num2 = int.Parse(key);
				int num3 = int.Parse(hashtable[key].ToString());
				for (int j = 0; j < num3; j++)
				{
					num++;
				}
			}
		}
		if (num == 0)
		{
			num = 1;
		}
		return num;
	}

	private int CalcCurPointsTotalEnemies()
	{
		int num = 0;
		int maxWavesOfPoints = ConfigManager.Instance().GetFixedConfig().GetMaxWavesOfPoints(mapIndex, pointsIndex);
		for (int i = 1; i < maxWavesOfPoints + 1; i++)
		{
			FixedConfig.NPCWaveCfg nPCWaveConfig = ConfigManager.Instance().GetFixedConfig().GetNPCWaveConfig(mapIndex, pointsIndex, i);
			if (nPCWaveConfig == null)
			{
				continue;
			}
			for (int j = 0; j < nPCWaveConfig.subWaveTimes.Length; j++)
			{
				Hashtable hashtable = (Hashtable)nPCWaveConfig.enemyInfo[j];
				foreach (string key in hashtable.Keys)
				{
					int num2 = int.Parse(key);
					int num3 = int.Parse(hashtable[key].ToString());
					for (int k = 0; k < num3; k++)
					{
						num++;
					}
				}
			}
		}
		if (num == 0)
		{
			num = 1;
		}
		return num;
	}

	public EnemySpawnScript GetOneEnemySpawnInScreen()
	{
		EnemySpawnScript result = null;
		float num = 4f;
		float num2 = 8f;
		List<EnemySpawnScript> list = new List<EnemySpawnScript>();
		for (int i = 0; i < dynSpawns.Count; i++)
		{
			Vector2 vector = new Vector2(dynSpawns[i].transform.position.x, dynSpawns[i].transform.position.z);
			Vector2 vector2 = new Vector2(player.GetTransform().position.x, player.GetTransform().position.z);
			Vector2 vector3 = vector - vector2;
			float sqrMagnitude = (vector - vector2).sqrMagnitude;
			if (sqrMagnitude > num * num && sqrMagnitude < num2 * num2)
			{
				list.Add(dynSpawns[i]);
			}
		}
		if (list.Count > 0)
		{
			result = list[Random.Range(0, list.Count)];
		}
		return result;
	}
}
