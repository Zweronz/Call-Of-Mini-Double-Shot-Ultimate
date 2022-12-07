using UnityEngine;

namespace Zombie3D
{
	public class Constant
	{
		public enum ANDROIDPLATFORM
		{
			E_Amazon = 0,
			E_GooglePlayer = 1
		}

		public const string AppVersion = "2.02";

		public const string AppVersionString = "2.0.2";

		public const ANDROIDPLATFORM Android_Platform = ANDROIDPLATFORM.E_GooglePlayer;

		public const bool m_bIAPCanbuy = true;

		public const string m_strGooglePlayKey = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAvfxRqpA+fjKm64VbNaXM6offkWUUsgCRzZlJFJrjZD5MTcX2p2/nfyOYiNDAh9qrS6hoS7MfIvPYirc38oerql/die8eIsW5JtBkeVt2te9+ZCc2BjmOr2b3g+xirbE1bkReJP5JDARHColJA7lQ/6o4J8rvv9L1rGcYynrWeSdTegeBDRkuMPQjgNArMXzkw7hITPdLXhQtBgnn62tV7zvguxKMuYoqzmXpyMsSyyAFVGDQAvI7ITKXvRR+0LL2ybjmP0+0kwLu7NL+nshBm8msjHbCqclsiOwcEkaMFk/Jgqg8B2MLeL7Ff2PJIJA023FnMfPgzNJIde0hj20j6wIDAQAB";

		public const string UserDataFileName = "MyGameData";

		public const string NewVersionFlagFileName = "Version";

		public const string CollectionsFileName = "Misc";

		public const string EncryptKey = "T_Zombie_DDS_1";

		public const int MaxFriendCount = 100;

		public const float CAMERA_ZOOM_SPEED = 10f;

		public const float DEFLECTION_SPEED = 2f;

		public const float FLOORHEIGHT = 10000.1f;

		public const float SHADOWLIGHT_HEIGHT = 10000.5f;

		public const float ExplodeWeaponRadius = 4.5f;

		public const float POWERBUFFTIME = 30f;

		public const float ITEM_HIGHPOINT = 10001.3f;

		public const float ITEM_LOWPOINT = 10001.1f;

		public const float PLAYING_WALKINGSPEED_DISCOUNT_WHEN_SHOOTING = 0.8f;

		public const float ANIMATION_ENDS_PERCENT = 1f;

		public const float SPARK_MIN_DISTANCE = 2f;

		public const float MAX_WAVE_TIME = 1800f;

		public const float DOCTOR_HP_RECOVERY = 2f;

		public const float MARINE_POWER_UP = 1.2f;

		public const float SWAT_HP = 2f;

		public const float NERD_MORE_LOOT_RATE = 1.3f;

		public const float COWBOY_SPEED_UP = 1.3f;

		public const float RICHMAN_MONEY_MORE = 1.2f;

		public const int HireOutTime = 259200;

		public const int HireOutFriendTime = 86400;

		public const int SURVIVALMODE_MAPINDEX = 101;

		public const int SURVIVALMODE_MAP2INDEX = 102;

		public const int NETMODE_MAP1INDEX = 201;

		public const int NETMODE_MAP2INDEX = 202;

		public const int NETMODE_MAP3INDEX = 203;

		public const int NETMODE_MAP4INDEX = 204;

		public static Color TextCommonColor = new Color(0.81960785f, 0.5294118f, 2f / 51f, 1f);

		public static Color RedGroupColor = new Color(1f, 1f / 17f, 1f / 17f, 0.5019608f);

		public static Color BlueGroupColor = new Color(1f / 51f, 0.49803922f, 43f / 51f, 0.5019608f);
	}
}
