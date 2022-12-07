using System.IO;

namespace Zombie3D
{
	public class ConfigManager
	{
		private static ConfigManager m_Instance;

		private FixedConfig m_FixedConfig;

		private UserConfig m_UserConfig;

		public static ConfigManager Instance()
		{
			if (m_Instance == null)
			{
				m_Instance = new ConfigManager();
				m_Instance.Init();
			}
			return m_Instance;
		}

		public void Init()
		{
			m_FixedConfig = new FixedConfig();
			m_FixedConfig.LoadFixedConfig();
		}

		public FixedConfig GetFixedConfig()
		{
			return m_FixedConfig;
		}

		public UserConfig GetUserConfig()
		{
			return m_UserConfig;
		}

		public void SaveUserConfig()
		{
			m_UserConfig.Save();
		}

		public static string GetTextFileData(string fullFileName)
		{
			string result = string.Empty;
			StreamReader streamReader = null;
			try
			{
				streamReader = new StreamReader(fullFileName);
				result = streamReader.ReadToEnd();
				return result;
			}
			catch
			{
				return result;
			}
			finally
			{
				if (streamReader != null)
				{
					streamReader.Close();
				}
			}
		}
	}
}
