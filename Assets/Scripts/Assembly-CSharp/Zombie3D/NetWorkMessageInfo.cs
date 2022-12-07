namespace Zombie3D
{
	public class NetWorkMessageInfo
	{
		public enum E_CalculagraphID
		{
			E_BuyIAP = 0,
			E_IAPInit = 1,
			E_Connected = 2,
			E_Login = 3,
			E_JoinRoom = 4,
			E_SubscribeGroup = 5,
			E_RoomCanBackTime = 6,
			E_Respawn = 7,
			E_BattleMsgAll = 8,
			E_LoseConnect = 9,
			E_FloorBalance = 10,
			E_NEnemyDeadAnimation = 11
		}

		public enum E_NetCMD
		{
			E_Kick = 0,
			E_StartGame = 1,
			E_TestRoomOwner = 2,
			E_SpawnPlayer = 3,
			E_SpawnIsOK = 4,
			E_GameBegin = 5,
			E_PlayerChangeWeapon = 6,
			E_SyncAnimation = 7,
			E_SyncTransform = 8,
			E_SyncPlyaerDataInfo = 9,
			E_RescuePlayer = 10,
			E_UnRescuePlayer = 11,
			E_StruckInformation = 12,
			E_SpawnEnemy = 13,
			E_SyncEnemyAnimation = 14,
			E_EnemyUseSkillAttack = 15,
			E_SyncEnemyHP = 16,
			E_SyncEnemyHit = 17,
			E_PVEFloorBalance = 18
		}

		public const string NameDistinguish = "[Ma!cA@d#dres]";

		public const string KillMsgDivision = "[De!ath#Msg%]";

		public const string GroupID_DeathMatch = "DeatchMatch";

		public const string GroupID_LastStand = "LastStand";
	}
}
