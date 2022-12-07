using System.Collections;
using System.IO;
using UnityEngine;

namespace Zombie3D
{
	public class UserConfig
	{
		public float m_Proficiency;

		public ArrayList m_OldProficiencyArray;

		public int m_AchievementWinCount;

		public int m_AchievementLoseCount;

		public string m_SavePath = string.Empty;

		public UserConfig()
		{
			m_Proficiency = 0f;
			m_OldProficiencyArray = new ArrayList();
			m_SavePath = Utils.SavePath() + "/YooKendo/UserData.txt";
		}

		public void LoadUserConfig()
		{
			string textFileData = ConfigManager.GetTextFileData(m_SavePath);
			string[] array = textFileData.Split('\r', '\n');
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] == null)
				{
					continue;
				}
				string[] array2 = array[i].Split('\t');
				if (array2 == null || array2.Length < 2)
				{
					continue;
				}
				switch (array2[0])
				{
				case "Proficiency":
					if (array2.Length == 2)
					{
						m_Proficiency = float.Parse(array2[1].Trim());
					}
					break;
				case "Achievement":
					if (array2.Length == 2)
					{
						string[] array4 = array2[1].Split(';');
						m_AchievementWinCount = int.Parse(array4[0].Trim());
						m_AchievementLoseCount = int.Parse(array4[1].Trim());
					}
					break;
				case "ProficiencyArray":
				{
					m_OldProficiencyArray.Clear();
					string[] array3 = array2[1].Split(';');
					for (int j = 0; j < array3.Length; j++)
					{
						m_OldProficiencyArray.Add(float.Parse(array3[j].Trim()));
					}
					break;
				}
				}
			}
		}

		public void Save()
		{
			string empty = string.Empty;
			string text = empty;
			empty = text + "Proficiency\t" + Mathf.FloorToInt(m_Proficiency) + "\n";
			text = empty;
			empty = text + "Achievement\t" + m_AchievementWinCount + ";" + m_AchievementLoseCount + "\n";
			if (m_OldProficiencyArray.Count > 0)
			{
				empty += "ProficiencyArray\t";
				for (int i = 0; i < m_OldProficiencyArray.Count; i++)
				{
					empty += Mathf.FloorToInt(float.Parse(m_OldProficiencyArray[i].ToString()));
					if (i < m_OldProficiencyArray.Count - 1)
					{
						empty += ";";
					}
				}
			}
			StreamWriter streamWriter = new StreamWriter(m_SavePath, false);
			streamWriter.Write(empty);
			streamWriter.Flush();
			streamWriter.Close();
		}
	}
}
