using System.Collections;
using Zombie3D;

public class AutoLoadHireFriendDataFromServer
{
	private GameState gameState;

	public static bool LoadFriendDatasOver;

	protected int m_LoadFriendDataIndex;

	private bool m_bStartLoadFriends;

	public void StartLoad()
	{
		gameState = GameApp.GetInstance().GetGameState();
		LoadFriendDatasOver = false;
		if (gameState.LoginType == GameLoginType.LoginType_Local)
		{
		}
		m_LoadFriendDataIndex = 0;
		m_bStartLoadFriends = false;
	}

	public void Update()
	{
		if (LoadFriendDatasOver)
		{
			return;
		}
		if (!m_bStartLoadFriends)
		{
			int @int = GameClient.prop.GetInt("GetHireArray_Status", -1);
			if (@int != -1 && @int == 1)
			{
				m_bStartLoadFriends = true;
				if (GameClient.HireFriendListResult.Count > 0 && m_LoadFriendDataIndex <= GameClient.HireFriendListResult.Count - 1)
				{
					m_LoadFriendDataIndex = 0;
					GameClient.HireUserInfo hireUserInfo = GameClient.HireFriendListResult[m_LoadFriendDataIndex];
					FriendUserData friendUserData = new FriendUserData();
					friendUserData.m_UUID = hireUserInfo.uuid;
					friendUserData.m_DeviceId = string.Empty;
					GameClient.GetHireFriendUserData(friendUserData);
				}
				else
				{
					LoadFriendDatasOver = true;
				}
			}
			return;
		}
		switch (GameClient.prop.GetInt("GetHireFriendUserDataStatus", 0))
		{
		case 1:
		{
			string @string = GameClient.prop.GetString("GetHireFriendUserData_Data");
			if (@string != string.Empty)
			{
				GameClient.HireUserInfo hireUserInfo3 = GameClient.HireFriendListResult[m_LoadFriendDataIndex];
				bool flag = false;
				for (int i = 0; i < this.gameState.GetHiredFriends().Count; i++)
				{
					FriendUserData key = this.gameState.GetHiredFriends()[i].Key;
					if (key.m_UUID == hireUserInfo3.uuid)
					{
						flag = true;
					}
				}
				if (!flag && hireUserInfo3.uuid == this.gameState.UUID)
				{
					flag = true;
				}
				if (!flag)
				{
					GameState gameState = new GameState();
					gameState.LoadData(@string, true);
					FriendUserData friendUserData3 = new FriendUserData();
					friendUserData3.m_Name = "MERC";
					friendUserData3.m_DeviceId = gameState.DeviceID;
					friendUserData3.m_UUID = hireUserInfo3.uuid;
					if (friendUserData3.m_UUID.Trim() == string.Empty)
					{
						GameClient.HireFriendListResult.RemoveAt(m_LoadFriendDataIndex);
					}
					else
					{
						friendUserData3.m_Exp = gameState.exp;
						friendUserData3.m_Level = gameState.Level;
						for (int j = 0; j < 2 && j < gameState.GetBattleWeapons().Count; j++)
						{
							friendUserData3.m_BattleWeapons.Add(gameState.GetBattleWeapons()[j]);
						}
						friendUserData3.m_AvatarHeadSuiteType = 0;
						friendUserData3.m_AvatarBodySuiteType = 0;
						Hashtable avatars = gameState.GetAvatars();
						foreach (Avatar key2 in avatars.Keys)
						{
							if ((bool)avatars[key2])
							{
								if (key2.AvtType == Avatar.AvatarType.Head)
								{
									friendUserData3.m_AvatarHeadSuiteType = (int)key2.SuiteType;
								}
								else if (key2.AvtType == Avatar.AvatarType.Body)
								{
									friendUserData3.m_AvatarBodySuiteType = (int)key2.SuiteType;
								}
							}
						}
						this.gameState.AddHireFriend(friendUserData3);
						m_LoadFriendDataIndex++;
					}
				}
				else
				{
					GameClient.HireFriendListResult.RemoveAt(m_LoadFriendDataIndex);
				}
			}
			if (GameClient.HireFriendListResult.Count > 0 && m_LoadFriendDataIndex <= GameClient.HireFriendListResult.Count - 1)
			{
				GameClient.HireUserInfo hireUserInfo4 = GameClient.HireFriendListResult[m_LoadFriendDataIndex];
				FriendUserData friendUserData4 = new FriendUserData();
				friendUserData4.m_UUID = hireUserInfo4.uuid;
				friendUserData4.m_DeviceId = string.Empty;
				GameClient.GetHireFriendUserData(friendUserData4);
			}
			else
			{
				LoadFriendDatasOver = true;
			}
			break;
		}
		case 2:
			GameClient.HireFriendListResult.RemoveAt(m_LoadFriendDataIndex);
			if (GameClient.HireFriendListResult.Count > 0 && m_LoadFriendDataIndex <= GameClient.HireFriendListResult.Count - 1)
			{
				GameClient.HireUserInfo hireUserInfo2 = GameClient.HireFriendListResult[m_LoadFriendDataIndex];
				FriendUserData friendUserData2 = new FriendUserData();
				friendUserData2.m_UUID = hireUserInfo2.uuid;
				friendUserData2.m_DeviceId = string.Empty;
				GameClient.GetHireFriendUserData(friendUserData2);
			}
			else
			{
				LoadFriendDatasOver = true;
			}
			break;
		}
	}
}
