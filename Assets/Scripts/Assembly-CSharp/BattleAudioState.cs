using UnityEngine;
using Zombie3D;

public class BattleAudioState : MusicPlayerState
{
	private int mapIndex = 1;

	public override void OnEnter()
	{
		if (GameApp.GetInstance().GetGameState().m_eGameMode.m_ePlayMode == GameState.NetworkGameMode.PlayMode.E_Console)
		{
			mapIndex = GameApp.GetInstance().GetGameScene().DDSTrigger.MapIndex;
		}
		else
		{
			int map_index = 101;
			int points_index = 1;
			int wave_index = 1;
			GameApp.GetInstance().GetGameState().GetGameTriggerInfo(ref map_index, ref points_index, ref wave_index);
			mapIndex = map_index;
			int[] array = new int[3] { 1, 3, 13 };
			MusicManager.Instance().PlayMusic((MusicManager.MusicType)array[Random.Range(0, array.Length)]);
		}
		if (mapIndex == 1 || mapIndex == 101 || mapIndex == 7)
		{
			MusicManager.Instance().PlayMusic(MusicManager.MusicType.Music_Map01_01Audio);
		}
		else if (mapIndex == 2 || mapIndex == 102)
		{
			MusicManager.Instance().PlayMusic(MusicManager.MusicType.Music_Map02_01Audio);
		}
		else if (mapIndex == 6 || mapIndex == 5)
		{
			MusicManager.Instance().PlayMusic(MusicManager.MusicType.Music_Map03_01Audio);
		}
		else if (mapIndex == 3 || mapIndex == 4)
		{
			MusicManager.Instance().PlayMusic(MusicManager.MusicType.Music_BossBgMusic);
		}
	}

	public override void Update()
	{
	}

	public override void OnExit()
	{
	}
}
