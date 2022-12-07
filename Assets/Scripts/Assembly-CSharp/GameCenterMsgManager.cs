using UnityEngine;
using Zombie3D;

public class GameCenterMsgManager : MonoBehaviour
{
	public float m_timer = -1f;

	public float m_time = 4f;

	private int m_id = -1;

	private bool m_bOneSuitOfGC;

	private int m_iOneSuitOfGC;

	private AudioSource m_asAudiosource;

	private void Update()
	{
		if (GameApp.GetInstance().GetGameState().m_lsGameCenterMsg.Count >= 0 && m_timer == -1f)
		{
			int iD = GetID();
			if (iD < 0)
			{
				return;
			}
			m_id = iD;
			GameApp.GetInstance().GetGameState().AchievementUI(GameApp.GetInstance().GetGameState().m_lsGameCenterMsg[m_id]);
			int num = 0;
			if (GameApp.GetInstance().GetGameState().m_lsGameCenterMsg[m_id].Contains("Double"))
			{
				num = 2;
				m_bOneSuitOfGC = true;
			}
			else if (GameApp.GetInstance().GetGameState().m_lsGameCenterMsg[m_id].Contains("Triple"))
			{
				num = 3;
				m_bOneSuitOfGC = true;
			}
			else if (GameApp.GetInstance().GetGameState().m_lsGameCenterMsg[m_id].Contains("Quadra"))
			{
				num = 4;
				m_bOneSuitOfGC = true;
			}
			else if (GameApp.GetInstance().GetGameState().m_lsGameCenterMsg[m_id].Contains("Mega"))
			{
				num = 5;
				m_bOneSuitOfGC = true;
			}
			else
			{
				if (m_iOneSuitOfGC <= 0)
				{
					m_bOneSuitOfGC = false;
				}
				num = 0;
			}
			if (m_bOneSuitOfGC)
			{
				m_iOneSuitOfGC++;
				if (m_iOneSuitOfGC == 1)
				{
					NBattleUIScript nBattleUIScript = SceneUIManager.Instance().GetSceneUIObject().GetComponent(typeof(NBattleUIScript)) as NBattleUIScript;
					if (nBattleUIScript != null)
					{
						nBattleUIScript.SetupFastKillMsgInfo(true, num);
					}
				}
			}
			if (GameApp.GetInstance().GetGameState().SoundOn)
			{
				string empty = string.Empty;
				switch (num)
				{
				case 2:
					empty = "DoublekillAudio";
					break;
				case 3:
					empty = "TriplekillAudio";
					break;
				case 4:
					empty = "QuadrakillAudio";
					break;
				case 5:
					empty = "MegakillAudio";
					break;
				default:
					empty = string.Empty;
					break;
				}
				if (empty != string.Empty)
				{
					GameObject gameObject = Object.Instantiate(Resources.Load("Zombie3D/Audio/Network/" + empty)) as GameObject;
					if (gameObject != null)
					{
						gameObject.transform.position = PlayerManager.Instance.GetPlayerObject().transform.position;
						RemoveTimerScript removeTimerScript = gameObject.AddComponent<RemoveTimerScript>();
						if (m_asAudiosource != null && m_asAudiosource.isPlaying)
						{
							m_asAudiosource.Stop();
						}
						m_asAudiosource = gameObject.GetComponent<AudioSource>();
						removeTimerScript.life = m_asAudiosource.clip.length + 0.1f;
						m_asAudiosource.loop = false;
						m_asAudiosource.Play();
					}
				}
			}
			GameApp.GetInstance().GetGameState().m_lsGameCenterMsg.RemoveAt(m_id);
			m_timer = 0f;
		}
		if (!(m_timer >= 0f))
		{
			return;
		}
		m_timer += Time.deltaTime;
		if (!(m_timer >= m_time))
		{
			return;
		}
		m_timer = -1f;
		if (m_bOneSuitOfGC && m_iOneSuitOfGC == 2)
		{
			NBattleUIScript nBattleUIScript2 = SceneUIManager.Instance().GetSceneUIObject().GetComponent(typeof(NBattleUIScript)) as NBattleUIScript;
			if (nBattleUIScript2 != null)
			{
				nBattleUIScript2.SetupFastKillMsgInfo(false);
			}
			m_bOneSuitOfGC = false;
			m_iOneSuitOfGC = 0;
		}
	}

	private int GetID()
	{
		if (GameApp.GetInstance().GetGameState().m_lsGameCenterMsg.Count >= 1)
		{
			int num = GameApp.GetInstance().GetGameState().m_lsGameCenterMsg.Count - 1;
			return 0;
		}
		return -1;
	}
}
