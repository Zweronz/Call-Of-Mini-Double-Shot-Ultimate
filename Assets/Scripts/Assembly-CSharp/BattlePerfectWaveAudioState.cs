using UnityEngine;
using Zombie3D;

public class BattlePerfectWaveAudioState : MusicPlayerState
{
	private float perfectWaveAudioStartTime = -1f;

	private float perfectWaveAudioLength = -1f;

	public override void OnEnter()
	{
		MusicManager.Instance().PlayMusic(MusicManager.MusicType.Music_PerfectWaveAudio, false);
		perfectWaveAudioStartTime = Time.time;
		perfectWaveAudioLength = MusicManager.Instance().CurMusicLength;
		MusicManager.Instance().m_MusicObj.GetComponent<AudioSource>().volume = 1f;
	}

	public override void Update()
	{
		if (Time.time - perfectWaveAudioStartTime < perfectWaveAudioLength / 2f)
		{
			if (MusicManager.Instance().m_MusicObj != null)
			{
				float volume = MusicManager.Instance().m_MusicObj.GetComponent<AudioSource>().volume;
				MusicManager.Instance().m_MusicObj.GetComponent<AudioSource>().volume = Mathf.Lerp(volume, 0.3f, 0.03f);
			}
		}
		else if (Time.time - perfectWaveAudioStartTime <= perfectWaveAudioLength && MusicManager.Instance().m_MusicObj != null)
		{
			float volume2 = MusicManager.Instance().m_MusicObj.GetComponent<AudioSource>().volume;
			MusicManager.Instance().m_MusicObj.GetComponent<AudioSource>().volume = Mathf.Lerp(volume2, 1f, 0.03f);
		}
	}

	public override void OnExit()
	{
	}
}
