using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Zombie3D
{
	public class FriendUserData
	{
		public string m_DeviceId = string.Empty;

		public string m_UUID = string.Empty;

		public string m_FacebookID = string.Empty;

		public string m_GameCenterID = string.Empty;

		public int m_Exp;

		public int m_Level = 1;

		public int m_ExternExp;

		public int m_AvatarHeadSuiteType;

		public int m_AvatarBodySuiteType;

		public List<WeaponType> m_BattleWeapons;

		public string m_Name;

		public FriendUserData()
		{
			m_BattleWeapons = new List<WeaponType>();
		}

		public void LoadFriendUserData(string input_data)
		{
			//Discarded unreachable code: IL_0048
			string text = input_data;
			try
			{
				byte[] data = Convert.FromBase64String(input_data);
				string playerDataEncryptKey = MiscPlugin.GetPlayerDataEncryptKey("ME_2_@_YOU_DD");
				byte[] bytes = XXTEAUtils.Decrypt(data, Encoding.ASCII.GetBytes(playerDataEncryptKey));
				text = Encoding.UTF8.GetString(bytes);
			}
			catch (Exception)
			{
				Debug.Log("ERROR: - FriendUserData.LoadFriendUserData() - Exception01");
				return;
			}
			if (!(text != string.Empty))
			{
				return;
			}
			string[] array = text.Split('\r', '\n');
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] == null)
				{
					continue;
				}
				string[] array2 = array[i].Split('\t');
				if (array2.Length < 2)
				{
					continue;
				}
				switch (array2[0])
				{
				case "DeviceID":
					m_DeviceId = array2[1];
					break;
				case "UUID":
					m_UUID = array2[1];
					break;
				case "FacebookID":
					m_FacebookID = array2[1];
					break;
				case "GameCenterID":
					m_GameCenterID = array2[1];
					break;
				case "Level":
					m_Level = int.Parse(array2[1]);
					break;
				case "AvatarHeadSuiteType":
					m_AvatarHeadSuiteType = int.Parse(array2[1]);
					break;
				case "AvatarBodySuiteType":
					m_AvatarBodySuiteType = int.Parse(array2[1]);
					break;
				case "BattleWeapons":
					m_BattleWeapons = new List<WeaponType>();
					if (array2[1] != string.Empty)
					{
						string[] array3 = array2[1].Split(',');
						for (int j = 0; j < array3.Length; j++)
						{
							m_BattleWeapons.Add((WeaponType)int.Parse(array3[j]));
						}
						if (m_BattleWeapons.Count == 0)
						{
							m_BattleWeapons.Add(WeaponType.Beretta_33);
						}
					}
					break;
				case "Name":
					m_Name = array2[1];
					break;
				}
			}
		}

		public string DataToString()
		{
			string empty = string.Empty;
			empty = empty + "DeviceID\t" + m_DeviceId;
			empty += "\n";
			empty = empty + "UUID\t" + m_UUID;
			empty += "\n";
			empty = empty + "FacebookID\t" + m_FacebookID;
			empty += "\n";
			empty = empty + "GameCenterID\t" + m_GameCenterID;
			empty += "\n";
			empty = empty + "Level\t" + m_Level;
			empty += "\n";
			empty = empty + "AvatarHeadSuiteType\t" + m_AvatarHeadSuiteType;
			empty += "\n";
			empty = empty + "AvatarBodySuiteType\t" + m_AvatarBodySuiteType;
			empty += "\n";
			empty = empty + "Name\t" + m_Name;
			empty += "\n";
			if (m_BattleWeapons.Count > 0)
			{
				empty += "BattleWeapons\t";
				for (int i = 0; i < m_BattleWeapons.Count; i++)
				{
					empty += (int)m_BattleWeapons[i];
					if (i < m_BattleWeapons.Count - 1)
					{
						empty += ",";
					}
				}
				empty += "\n";
			}
			string playerDataEncryptKey = MiscPlugin.GetPlayerDataEncryptKey("ME_2_@_YOU_DD");
			byte[] inArray = XXTEAUtils.Encrypt(Encoding.UTF8.GetBytes(empty), Encoding.ASCII.GetBytes(playerDataEncryptKey));
			return Convert.ToBase64String(inArray);
		}
	}
}
