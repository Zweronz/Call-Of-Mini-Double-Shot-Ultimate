using Zombie3D;

public class ShopAudioState : MusicPlayerState
{
	public override void OnEnter()
	{
		MusicManager.Instance().PlayMusic(MusicManager.MusicType.Music_ShopUIAudio);
	}

	public override void Update()
	{
	}

	public override void OnExit()
	{
	}
}
