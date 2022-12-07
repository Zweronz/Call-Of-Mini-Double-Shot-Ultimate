using Zombie3D;

public class ExchangeUIAudioState : MusicPlayerState
{
	public override void OnEnter()
	{
		MusicManager.Instance().PlayMusic(MusicManager.MusicType.Music_ExchangeUIAudio);
	}

	public override void Update()
	{
	}

	public override void OnExit()
	{
	}
}
