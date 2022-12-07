using Zombie3D;

public class ChoosePointsAudioState : MusicPlayerState
{
	public override void OnEnter()
	{
		MusicManager.Instance().PlayMusic(MusicManager.MusicType.Music_ChoosePointsUIAudio);
	}

	public override void Update()
	{
	}

	public override void OnExit()
	{
	}
}
