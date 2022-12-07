using UnityEngine;
using Zombie3D;

public class DDSTriggerScript : MonoBehaviour
{
	protected Player player;

	protected bool bPlaying;

	protected int mapIndex;

	protected int pointsIndex;

	protected int waveIndex;

	protected int subWaveNum;

	public int emenySpawnLimit = 15;

	public GameObject m_EnemySpawnRoot;

	protected EnemySpawnScript[] spawns;

	protected GameScene gameScene;

	protected float lastUpdateTime = -1000f;

	private bool bWaveRefreshEnd;

	protected int m_AllEnemiesOfCurWave = 1;

	protected int m_GenEnemiesCountOfCurWave;

	protected int m_GenExternEnemiesCountOfCurWave;

	public bool Playing
	{
		get
		{
			return bPlaying;
		}
		set
		{
			bPlaying = value;
		}
	}

	public bool WaveRefreshEnd
	{
		get
		{
			return bWaveRefreshEnd;
		}
		set
		{
			bWaveRefreshEnd = value;
		}
	}

	public int MapIndex
	{
		get
		{
			return mapIndex;
		}
		set
		{
			mapIndex = value;
		}
	}

	public int PointsIndex
	{
		get
		{
			return pointsIndex;
		}
		set
		{
			pointsIndex = value;
		}
	}

	public int WaveIndex
	{
		get
		{
			return waveIndex;
		}
		set
		{
			waveIndex = value;
		}
	}

	public int AllEnemiesOfCurWave
	{
		get
		{
			return m_AllEnemiesOfCurWave;
		}
		set
		{
			m_AllEnemiesOfCurWave = value;
		}
	}

	public int GenEnemiesCountOfCurWave
	{
		get
		{
			return m_GenEnemiesCountOfCurWave;
		}
		set
		{
			m_GenEnemiesCountOfCurWave = value;
		}
	}

	public int GenExternEnemiesCountOfCurWave
	{
		get
		{
			return m_GenExternEnemiesCountOfCurWave;
		}
		set
		{
			m_GenExternEnemiesCountOfCurWave = value;
		}
	}

	public virtual void Init()
	{
		gameScene = GameApp.GetInstance().GetGameScene();
		GameApp.GetInstance().GetGameScene().DDSTrigger = this;
		GameApp.GetInstance().GetGameState().GetGameTriggerInfo(ref mapIndex, ref pointsIndex, ref waveIndex);
		player = GameApp.GetInstance().GetGameScene().GetPlayer();
		if (null != m_EnemySpawnRoot)
		{
			spawns = m_EnemySpawnRoot.GetComponentsInChildren<EnemySpawnScript>();
			Algorithem<EnemySpawnScript>.RandomSort(spawns);
		}
		bPlaying = false;
		bWaveRefreshEnd = false;
	}

	private void Update()
	{
	}

	private void OnDrawGizmos()
	{
	}

	public virtual void PullTrigger()
	{
		bPlaying = true;
	}

	public void StopRefreshEnemies()
	{
		bPlaying = false;
	}

	public virtual void PlayWave(int map_index, int points_index, int wave_index)
	{
		MapIndex = map_index;
		PointsIndex = points_index;
		WaveIndex = wave_index;
	}
}
