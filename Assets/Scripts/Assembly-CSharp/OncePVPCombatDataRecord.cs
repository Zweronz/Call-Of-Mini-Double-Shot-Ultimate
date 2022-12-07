using System.Collections.Generic;
using Zombie3D;

public class OncePVPCombatDataRecord
{
	public bool m_UseFastRun;

	private bool m_KMoreTenPlayerDoOnce;

	private int iOncePVPKillCount;

	private int iOncePVPDeathCount;

	private float m_FastKillTimeInterval = 10f;

	private List<KeyValuePair<int, double>> m_lsKillPlayerTime = new List<KeyValuePair<int, double>>();

	private List<int> m_lsSameKill = new List<int>();

	private void AddKillCount(int count = 1)
	{
		iOncePVPKillCount += count;
		int num = 40;
		if (iOncePVPKillCount >= num && GameApp.GetInstance().GetGameState().IsGCArchievementLocked(50))
		{
			GameApp.GetInstance().GetGameState().UnlockGCArchievement(50, "com.trinitigame.callofminibulletdudes.a51");
			GameApp.GetInstance().GetGameState().AddAchievementUI("+4 tCrystals");
			GameApp.GetInstance().GetGameState().AddDollor(4);
		}
		if (iOncePVPKillCount >= 10 && !m_KMoreTenPlayerDoOnce)
		{
			GameApp.GetInstance().GetGameState().AddKillMoreTenPlayersOnceWar();
			m_KMoreTenPlayerDoOnce = true;
		}
	}

	public void AddDeathCount(int count = 1)
	{
		iOncePVPDeathCount += count;
		int num = 20;
		if (iOncePVPDeathCount >= num && GameApp.GetInstance().GetGameState().IsGCArchievementLocked(52))
		{
			GameApp.GetInstance().GetGameState().UnlockGCArchievement(52, "com.trinitigame.callofminibulletdudes.a53");
			GameApp.GetInstance().GetGameState().AddAchievementUI("+1 tCrystals");
			GameApp.GetInstance().GetGameState().AddDollor(1);
		}
	}

	private void AddFastKillPlayer(int id, double time)
	{
		m_lsKillPlayerTime.Add(new KeyValuePair<int, double>(id, time));
		int num = 5;
		if (m_lsKillPlayerTime.Count >= num)
		{
			GameApp.GetInstance().GetGameState().AddPVPFiveKillCount();
			GameApp.GetInstance().GetGameState().AddGold(2000);
			GameApp.GetInstance().GetGameState().AddAchievementUI("Get a Mega Kill");
			GameApp.GetInstance().GetGameState().AddAchievementUI("+2000 Cash");
			return;
		}
		num = 4;
		if (m_lsKillPlayerTime.Count >= num)
		{
			GameApp.GetInstance().GetGameState().AddPVPFourKillCount();
			GameApp.GetInstance().GetGameState().AddGold(1000);
			GameApp.GetInstance().GetGameState().AddAchievementUI("Get a Quadra Kill");
			GameApp.GetInstance().GetGameState().AddAchievementUI("+1000 Cash");
			return;
		}
		num = 3;
		if (m_lsKillPlayerTime.Count >= num)
		{
			GameApp.GetInstance().GetGameState().AddPVPThreeKillCount();
			GameApp.GetInstance().GetGameState().AddGold(500);
			GameApp.GetInstance().GetGameState().AddAchievementUI("Get a Triple Kill");
			GameApp.GetInstance().GetGameState().AddAchievementUI("+500 Cash");
			return;
		}
		num = 2;
		if (m_lsKillPlayerTime.Count >= num)
		{
			GameApp.GetInstance().GetGameState().AddPVPDoubleKillCount();
			GameApp.GetInstance().GetGameState().AddGold(200);
			GameApp.GetInstance().GetGameState().AddAchievementUI("Get a Double Kill");
			GameApp.GetInstance().GetGameState().AddAchievementUI("+200 Cash");
		}
	}

	private void AddKillSamePlayer(int id)
	{
		m_lsSameKill.Add(id);
		int num = 3;
		if (m_lsSameKill.Count >= num && GameApp.GetInstance().GetGameState().IsGCArchievementLocked(55))
		{
			GameApp.GetInstance().GetGameState().UnlockGCArchievement(55, "com.trinitigame.callofminibulletdudes.a56");
			GameApp.GetInstance().GetGameState().AddAchievementUI("+2 tCrystals");
			GameApp.GetInstance().GetGameState().AddDollor(2);
		}
	}

	public void KillPlayer(int id, double time)
	{
		AddKillCount();
		if (m_lsSameKill.Count <= 0)
		{
			AddKillSamePlayer(id);
		}
		else if (m_lsSameKill.Contains(id))
		{
			AddKillSamePlayer(id);
		}
		else
		{
			m_lsSameKill.Clear();
			AddKillSamePlayer(id);
		}
		if (m_lsKillPlayerTime.Count <= 0)
		{
			AddFastKillPlayer(id, time);
		}
		else if (time - m_lsKillPlayerTime[m_lsKillPlayerTime.Count - 1].Value <= (double)(m_FastKillTimeInterval * 1000f))
		{
			AddFastKillPlayer(id, time);
		}
		else
		{
			m_lsKillPlayerTime.Clear();
		}
	}
}
