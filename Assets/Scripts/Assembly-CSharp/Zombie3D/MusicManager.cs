using UnityEngine;

namespace Zombie3D
{
	public class MusicManager
	{
		public enum MusicType
		{
			Music_No = 0,
			Music_Map01_01Audio = 1,
			Music_Map01_01_Loop_Audio = 2,
			Music_Map02_01Audio = 3,
			Music_Map02_01_Loop_Audio = 4,
			Music_PerfectWaveAudio = 5,
			Music_ExchangeUIAudio = 6,
			Music_GameFirstBegin = 7,
			Music_ShopUIAudio = 8,
			Music_CartoonAudio = 9,
			Music_ChoosePointsUIAudio = 10,
			Music_TopicAudio01 = 11,
			Music_TopicAudio02 = 12,
			Music_Map03_01Audio = 13,
			Music_BossBgMusic = 14
		}

		public GameObject m_MusicObj;

		private static MusicManager instance;

		public MusicType PlayingMusicType { get; set; }

		public float CurMusicLength { get; set; }

		public static MusicManager Instance()
		{
			if (instance == null)
			{
				instance = new MusicManager();
			}
			return instance;
		}

		public void PlayMusic(MusicType music_type, bool bLoop = true)
		{
			PlayingMusicType = music_type;
			string musicPath = GetMusicPath(music_type);
			Object.Destroy(m_MusicObj);
			m_MusicObj = null;
			GameObject gameObject = Resources.Load(musicPath, typeof(GameObject)) as GameObject;
			if (gameObject != null)
			{
				m_MusicObj = Object.Instantiate(gameObject, Vector3.zero, Quaternion.identity) as GameObject;
				if (m_MusicObj != null)
				{
					m_MusicObj.transform.parent = Camera.main.transform;
					m_MusicObj.transform.localPosition = new Vector3(0f, 0f, 2f);
					m_MusicObj.GetComponent<AudioSource>().loop = bLoop;
					m_MusicObj.GetComponent<AudioSource>().Play();
					CurMusicLength = m_MusicObj.GetComponent<AudioSource>().clip.length;
				}
				else
				{
					Debug.Log(string.Concat("ERROR: MusicManager - ", music_type, "|", musicPath));
				}
			}
			else
			{
				Debug.LogError("ERROR: MusicManager - Donnot have audio: " + musicPath);
			}
			if (!GameApp.GetInstance().GetGameState().MusicOn && m_MusicObj != null)
			{
				m_MusicObj.GetComponent<AudioSource>().Pause();
			}
		}

		public void ChangeMusicOption()
		{
			if (m_MusicObj != null)
			{
				if (GameApp.GetInstance().GetGameState().MusicOn)
				{
					m_MusicObj.GetComponent<AudioSource>().Play();
				}
				else
				{
					m_MusicObj.GetComponent<AudioSource>().Pause();
				}
			}
		}

		public string GetMusicPath(MusicType music_type)
		{
			string result = "Zombie3D/Audio/GameFirstBegin";
			switch (music_type)
			{
			case MusicType.Music_ExchangeUIAudio:
				result = "Zombie3D/Audio/ExchangeUIAudio";
				break;
			case MusicType.Music_Map01_01Audio:
				result = "Zombie3D/Audio/Map01_01";
				break;
			case MusicType.Music_Map01_01_Loop_Audio:
				result = "Zombie3D/Audio/Map01_01_Loop_Audio";
				break;
			case MusicType.Music_Map02_01Audio:
				result = "Zombie3D/Audio/Map02_01";
				break;
			case MusicType.Music_Map02_01_Loop_Audio:
				result = "Zombie3D/Audio/Map02_01_Loop_Audio";
				break;
			case MusicType.Music_PerfectWaveAudio:
				result = "Zombie3D/Audio/PerfectWaveAudio";
				break;
			case MusicType.Music_GameFirstBegin:
				result = "Zombie3D/Audio/GameFirstBegin";
				break;
			case MusicType.Music_ShopUIAudio:
				result = "Zombie3D/Audio/ShopUIAudio";
				break;
			case MusicType.Music_CartoonAudio:
				result = "Zombie3D/Audio/CartoonAudio";
				break;
			case MusicType.Music_ChoosePointsUIAudio:
				result = "Zombie3D/Audio/ChoosePointsUIAudio";
				break;
			case MusicType.Music_TopicAudio01:
				result = "Zombie3D/Audio/TopicAudio01";
				break;
			case MusicType.Music_TopicAudio02:
				result = "Zombie3D/Audio/TopicAudio02";
				break;
			case MusicType.Music_Map03_01Audio:
				result = "Zombie3D/Audio/Map03_01";
				break;
			case MusicType.Music_BossBgMusic:
				result = "Zombie3D/Audio/BossBgMusic";
				break;
			}
			return result;
		}
	}
}
