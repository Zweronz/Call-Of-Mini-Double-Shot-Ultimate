using System.Collections;
using System.Collections.Generic;
using Zombie3D;

public class NGroupManager
{
	private List<ArrayList> m_GroupList = new List<ArrayList>();

	private int m_iModeIndex;

	private List<string> m_NeedSubscribeGroups = new List<string>();

	public float m_fCountDownTimer = -1f;

	private float m_fCountDownTime = 20f;

	public List<string> SubscribeGroups
	{
		get
		{
			return m_NeedSubscribeGroups;
		}
	}

	public NGroupManager()
	{
		ArrayList item = new ArrayList();
		m_GroupList.Add(item);
		item = null;
		item = new ArrayList();
		m_GroupList.Add(item);
		item = null;
		item = new ArrayList();
		m_GroupList.Add(item);
		item = null;
		item = new ArrayList();
		m_GroupList.Add(item);
	}

	public void Init(GameState.NetworkGameMode.PlayMode playerMode, GameState.NetworkGameMode.NetworkCooperationMode cooperationMode, int score, float maxCountDownTime)
	{
		int num = 0;
		num = ((score < 1 || score >= 501) ? ((score >= 501 && score < 1501) ? 1 : ((score < 1501 || score >= 2501) ? 3 : 2)) : 0);
		m_NeedSubscribeGroups.Clear();
		switch (playerMode)
		{
		case GameState.NetworkGameMode.PlayMode.E_LastStand:
			switch (cooperationMode)
			{
			case GameState.NetworkGameMode.NetworkCooperationMode.E_Team:
				m_iModeIndex = 0;
				break;
			case GameState.NetworkGameMode.NetworkCooperationMode.E_Simple:
				m_iModeIndex = 1;
				break;
			}
			break;
		case GameState.NetworkGameMode.PlayMode.E_DeathMatch:
			switch (cooperationMode)
			{
			case GameState.NetworkGameMode.NetworkCooperationMode.E_Team:
				m_iModeIndex = 2;
				break;
			case GameState.NetworkGameMode.NetworkCooperationMode.E_Simple:
				m_iModeIndex = 3;
				break;
			}
			break;
		}
		string item = (string)m_GroupList[m_iModeIndex][num];
		m_NeedSubscribeGroups.Add(item);
		m_fCountDownTime = maxCountDownTime;
	}

	public void Dologic(float detalTime)
	{
		if (m_fCountDownTimer >= 0f)
		{
			m_fCountDownTimer += detalTime;
			if (m_fCountDownTimer >= m_fCountDownTime)
			{
				ReCalcuSubscribeGroups();
				StopCountDown();
			}
		}
	}

	public void ReCalcuSubscribeGroups()
	{
		m_NeedSubscribeGroups.Clear();
		foreach (string item in m_GroupList[m_iModeIndex])
		{
			m_NeedSubscribeGroups.Add(item);
		}
	}

	public void StopCountDown()
	{
		m_fCountDownTimer = -1f;
	}
}
