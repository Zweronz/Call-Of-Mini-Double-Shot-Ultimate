public class FBWrapper
{
	public static int Status()
	{
		return 2;
	}

	public static int BuddyStatus()
	{
		return -1;
	}

	public static int StatusForPermission()
	{
		return -1;
	}

	public static int StatusCheckPermission()
	{
		return -1;
	}

	public static void ResumeSession()
	{
	}

	public static void Login()
	{
	}

	public static void LoginTimeout()
	{
	}

	public static void Logout()
	{
	}

	public static void GetInfo(ref string id, ref string name)
	{
		id = "myid";
		name = "myname";
	}

	public static int GetFriendCount()
	{
		return 5;
	}

	public static void GetFriendInfo(int index, ref string id, ref string name)
	{
		id = "friendid:" + index;
		name = "friendname:" + index;
	}

	public static void PublishFeedDialog(string title)
	{
	}

	public static void PushFeedNoDialog(string message, string attachment)
	{
	}

	public static void AskPermissionDialog(string permission)
	{
	}

	public static void FBCheckPermission(string permission)
	{
	}
}
