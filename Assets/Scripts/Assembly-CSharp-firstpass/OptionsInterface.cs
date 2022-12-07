public class OptionsInterface
{
	private static bool m_bOpenMusic;

	private static bool m_bOpenSound;

	private static bool m_bRevertYAris;

	private static string m_filename;

	static OptionsInterface()
	{
		m_bOpenMusic = true;
		m_bOpenSound = true;
		m_bRevertYAris = false;
		m_filename = "/Options.dat";
	}

	public static bool IsOpenMusic()
	{
		return m_bOpenMusic;
	}

	public static void SetMusic(bool bVal)
	{
		m_bOpenMusic = bVal;
		Save();
	}

	public static bool IsOpenSound()
	{
		return m_bOpenSound;
	}

	public static void SetSound(bool bVal)
	{
		m_bOpenSound = bVal;
		Save();
	}

	public static bool IsRevertYAris()
	{
		return m_bRevertYAris;
	}

	public static void SetRevertYAris(bool bVal)
	{
		m_bRevertYAris = bVal;
		Save();
	}

	public static void Load()
	{
		string content = string.Empty;
		Utils.FileGetString(m_filename, ref content);
		string[] array = content.Split('\n');
		foreach (string text in array)
		{
			int num = text.IndexOf(':');
			if (num >= 0)
			{
				string text2 = text.Substring(0, num);
				string s = text.Substring(num + 1);
				switch (text2)
				{
				case "music":
				{
					int num4 = int.Parse(s);
					m_bOpenMusic = num4 == 1;
					break;
				}
				case "sound":
				{
					int num3 = int.Parse(s);
					m_bOpenSound = num3 == 1;
					break;
				}
				case "Yaris":
				{
					int num2 = int.Parse(s);
					m_bRevertYAris = num2 == 1;
					break;
				}
				}
			}
		}
	}

	public static void Save()
	{
		string empty = string.Empty;
		string text = empty;
		empty = text + "music:" + (m_bOpenMusic ? 1 : 0) + "\n";
		text = empty;
		empty = text + "sound:" + (m_bOpenSound ? 1 : 0) + "\n";
		text = empty;
		empty = text + "Yaris:" + (m_bRevertYAris ? 1 : 0) + "\n";
		Utils.FileSaveString(m_filename, empty);
	}
}
