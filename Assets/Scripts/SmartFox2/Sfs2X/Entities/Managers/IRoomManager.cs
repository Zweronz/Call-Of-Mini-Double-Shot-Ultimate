using System.Collections.Generic;

namespace Sfs2X.Entities.Managers
{
	public interface IRoomManager
	{
		List<Room> GetRoomListFromGroup(string groupId);
	}
}
