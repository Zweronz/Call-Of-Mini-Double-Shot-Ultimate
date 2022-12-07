using Zombie3D;

public class GameStartFirstPlayState : MusicPlayerState
{
	public override void OnEnter()
	{
		MusicManager.Instance().PlayMusic(MusicManager.MusicType.Music_CartoonAudio);
	}

	public override void Update()
	{
	}

	public override void OnExit()
	{
	}
}
