using System.Collections;
using Zombie3D;

public class AutoLoadFriendDataFromServer
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
			LoadFriendDatasOver = true;
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
			int @int = GameClient.prop.GetInt("GetUserFriendListInServer_Status", -1);
			if (@int != -1 && @int == 1)
			{
				m_bStartLoadFriends = true;
				if (GameClient.friendListResult.Count > 0 && m_LoadFriendDataIndex <= GameClient.friendListResult.Count - 1)
				{
					m_LoadFriendDataIndex = 0;
					GameClient.Bindingbuddy bindingbuddy = GameClient.friendListResult[m_LoadFriendDataIndex];
					FriendUserData friendUserData = new FriendUserData();
					friendUserData.m_UUID = bindingbuddy.uuid;
					friendUserData.m_DeviceId = string.Empty;
					GameClient.GetFriendUserData(friendUserData);
				}
				else
				{
					LoadFriendDatasOver = true;
				}
			}
			return;
		}
		switch (GameClient.prop.GetInt("GetFriendUserDataStatus", 0))
		{
		case 1:
		{
			string @string = GameClient.prop.GetString("GetFriendUserData_Data");
			if (@string != string.Empty)
			{
				GameClient.Bindingbuddy bindingbuddy3 = GameClient.friendListResult[m_LoadFriendDataIndex];
				GameState gameState = new GameState();
				gameState.LoadData(@string, true);
				FriendUserData friendUserData3 = new FriendUserData();
				friendUserData3.m_Name = "John";
				if (this.gameState.LoginType == GameLoginType.LoginType_Facebook)
				{
					string facebookName = gameState.FacebookName;
					string text = string.Empty;
					for (int i = 0; i < facebookName.Length; i++)
					{
						text = ((facebookName[i] <= '\u007f') ? (text + facebookName[i]) : (text + "*"));
					}
					friendUserData3.m_Name = text;
				}
				else if (this.gameState.LoginType == GameLoginType.LoginType_GameCenter)
				{
					string gameCenterName = gameState.GameCenterName;
					string text2 = string.Empty;
					for (int j = 0; j < gameCenterName.Length; j++)
					{
						text2 = ((gameCenterName[j] <= '\u007f') ? (text2 + gameCenterName[j]) : (text2 + "*"));
					}
					friendUserData3.m_Name = text2;
				}
				friendUserData3.m_DeviceId = gameState.DeviceID;
				friendUserData3.m_UUID = bindingbuddy3.uuid;
				friendUserData3.m_Exp = gameState.exp;
				friendUserData3.m_Level = gameState.Level;
				for (int k = 0; k < 2 && k < gameState.GetBattleWeapons().Count; k++)
				{
					friendUserData3.m_BattleWeapons.Add(gameState.GetBattleWeapons()[k]);
				}
				friendUserData3.m_AvatarHeadSuiteType = 0;
				friendUserData3.m_AvatarBodySuiteType = 0;
				Hashtable avatars = gameState.GetAvatars();
				foreach (Avatar key in avatars.Keys)
				{
					if ((bool)avatars[key])
					{
						if (key.AvtType == Avatar.AvatarType.Head)
						{
							friendUserData3.m_AvatarHeadSuiteType = (int)key.SuiteType;
						}
						else if (key.AvtType == Avatar.AvatarType.Body)
						{
							friendUserData3.m_AvatarBodySuiteType = (int)key.SuiteType;
						}
					}
				}
				this.gameState.AddFriend(friendUserData3);
			}
			m_LoadFriendDataIndex++;
			if (GameClient.friendListResult.Count > 0 && m_LoadFriendDataIndex <= GameClient.friendListResult.Count - 1)
			{
				GameClient.Bindingbuddy bindingbuddy4 = GameClient.friendListResult[m_LoadFriendDataIndex];
				FriendUserData friendUserData4 = new FriendUserData();
				friendUserData4.m_UUID = bindingbuddy4.uuid;
				friendUserData4.m_DeviceId = string.Empty;
				GameClient.GetFriendUserData(friendUserData4);
			}
			else
			{
				LoadFriendDatasOver = true;
			}
			break;
		}
		case 2:
			m_LoadFriendDataIndex++;
			if (GameClient.friendListResult.Count > 0 && m_LoadFriendDataIndex <= GameClient.friendListResult.Count - 1)
			{
				GameClient.Bindingbuddy bindingbuddy2 = GameClient.friendListResult[m_LoadFriendDataIndex];
				FriendUserData friendUserData2 = new FriendUserData();
				friendUserData2.m_UUID = bindingbuddy2.uuid;
				friendUserData2.m_DeviceId = string.Empty;
				GameClient.GetFriendUserData(friendUserData2);
			}
			else
			{
				LoadFriendDatasOver = true;
			}
			break;
		}
	}
}
