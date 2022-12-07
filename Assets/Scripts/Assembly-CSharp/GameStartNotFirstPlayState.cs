using Zombie3D;

public class GameStartNotFirstPlayState : MusicPlayerState
{
	public override void OnEnter()
	{
		MusicManager.Instance().PlayMusic(MusicManager.MusicType.Music_TopicAudio01);
	}

	public override void Update()
	{
	}

	public override void OnExit()
	{
	}
}
