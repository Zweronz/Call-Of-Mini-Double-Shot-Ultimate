using System.Collections.Generic;

public class Avatar
{
	public class AvatarProperty
	{
		public float m_DefenceAdditive;

		public float m_AttackAdditive;

		public float m_SpeedAdditive;

		public float m_HpAdditive;

		public float m_AttackSpeedAdditive;

		public float m_StaminaAdd;

		public float m_ExpAdditive;

		public float m_GoldAdditive;
	}

	public enum AvatarSuiteType
	{
		Driver = 0,
		Policeman = 1,
		Surgeon = 2,
		Cowboy = 3,
		Hacker = 4,
		Navy = 5,
		Ninjalong = 6,
		Swat = 7,
		MaskKnight = 8,
		Gentleman = 9,
		ZombieAssassin = 10,
		DeathSquads = 11,
		Waiter = 12,
		Mechanic = 13,
		Gladiator = 14,
		ViolenceFr = 15,
		SuperNemesis = 16,
		EvilClown = 17,
		X800 = 18,
		ShadowAgents = 19,
		Pirate = 20,
		Eskimo = 21,
		Shinobi = 22,
		Kunoichi = 23,
		DemonLord = 24,
		Rugby = 25,
		VanHelsing = 26
	}

	public enum AvatarType
	{
		Head = 0,
		Body = 1
	}

	private AvatarSuiteType m_SuiteType;

	private AvatarType m_Type;

	private List<string> m_MeshPathList;

	private List<string> m_TexturePathList;

	public List<string> MeshPathList
	{
		get
		{
			return m_MeshPathList;
		}
		set
		{
			m_MeshPathList = value;
		}
	}

	public List<string> MatPathList
	{
		get
		{
			return m_TexturePathList;
		}
		set
		{
			m_TexturePathList = value;
		}
	}

	public AvatarSuiteType SuiteType
	{
		get
		{
			return m_SuiteType;
		}
		set
		{
			m_SuiteType = value;
		}
	}

	public AvatarType AvtType
	{
		get
		{
			return m_Type;
		}
		set
		{
			m_Type = value;
		}
	}

	public Avatar(AvatarSuiteType avatar_suite_type, AvatarType avatar_type)
	{
		SuiteType = avatar_suite_type;
		AvtType = avatar_type;
		m_MeshPathList = new List<string>();
		m_TexturePathList = new List<string>();
		SetMeshAndTexture(avatar_suite_type, avatar_type);
	}

	public void SetMeshAndTexture(AvatarSuiteType avatar_suite_type, AvatarType avatar_type)
	{
		MeshPathList.Clear();
		MatPathList.Clear();
		switch (avatar_suite_type)
		{
		case AvatarSuiteType.Driver:
			switch (avatar_type)
			{
			case AvatarType.Head:
				MeshPathList.Add("Current_head");
				MatPathList.Add("Driver_head");
				break;
			case AvatarType.Body:
				MeshPathList.Add("Current_body");
				MatPathList.Add("Driver_body");
				break;
			}
			break;
		case AvatarSuiteType.Policeman:
			switch (avatar_type)
			{
			case AvatarType.Head:
				MeshPathList.Add("Current_head");
				MatPathList.Add("Policeman_head");
				MeshPathList.Add("Policeman_hat");
				MatPathList.Add("Policeman_hat");
				break;
			case AvatarType.Body:
				MeshPathList.Add("Current_body");
				MatPathList.Add("Policeman_body");
				break;
			}
			break;
		case AvatarSuiteType.Surgeon:
			switch (avatar_type)
			{
			case AvatarType.Head:
				MeshPathList.Add("Current_head");
				MatPathList.Add("Surgeon_head");
				break;
			case AvatarType.Body:
				MeshPathList.Add("Surgeon_body");
				MatPathList.Add("Surgeon_body");
				break;
			}
			break;
		case AvatarSuiteType.Cowboy:
			switch (avatar_type)
			{
			case AvatarType.Head:
				MeshPathList.Add("Current_head");
				MatPathList.Add("Cowboy_head");
				MeshPathList.Add("Cowboy_hat");
				MatPathList.Add("Cowboy_hat");
				break;
			case AvatarType.Body:
				MeshPathList.Add("Cowboy_body");
				MatPathList.Add("Cowboy_body");
				break;
			}
			break;
		case AvatarSuiteType.Hacker:
			switch (avatar_type)
			{
			case AvatarType.Head:
				MeshPathList.Add("Current_head");
				MatPathList.Add("Hacker_head");
				MeshPathList.Add("Hacker_hat");
				MatPathList.Add("Hacker_hat");
				break;
			case AvatarType.Body:
				MeshPathList.Add("Current_body");
				MatPathList.Add("Hacker_body");
				break;
			}
			break;
		case AvatarSuiteType.Navy:
			switch (avatar_type)
			{
			case AvatarType.Head:
				MeshPathList.Add("Current_head");
				MatPathList.Add("Navy_head");
				MeshPathList.Add("Navy_hat");
				MatPathList.Add("Navy_hat");
				break;
			case AvatarType.Body:
				MeshPathList.Add("Current_body");
				MatPathList.Add("Navy_body");
				break;
			}
			break;
		case AvatarSuiteType.Ninjalong:
			switch (avatar_type)
			{
			case AvatarType.Head:
				MeshPathList.Add("Current_head");
				MatPathList.Add("Ninjalong_head");
				break;
			case AvatarType.Body:
				MeshPathList.Add("Current_body");
				MatPathList.Add("Ninjalong_body");
				MeshPathList.Add("Ninjalong_equipment");
				MatPathList.Add("Ninjalong_equipment");
				break;
			}
			break;
		case AvatarSuiteType.Swat:
			switch (avatar_type)
			{
			case AvatarType.Head:
				MeshPathList.Add("Current_head");
				MatPathList.Add("Swat_head");
				MeshPathList.Add("SWAT_hat");
				MatPathList.Add("Swat_hat_Waiter_Eyeglass");
				break;
			case AvatarType.Body:
				MeshPathList.Add("Current_body");
				MatPathList.Add("Swat_body");
				break;
			}
			break;
		case AvatarSuiteType.MaskKnight:
			switch (avatar_type)
			{
			case AvatarType.Head:
				MeshPathList.Add("Current_head");
				MatPathList.Add("MaskKnight_head");
				MeshPathList.Add("MaskKnight_hat");
				MatPathList.Add("MaskKnight_hat");
				break;
			case AvatarType.Body:
				MeshPathList.Add("Current_body");
				MatPathList.Add("MaskKnight_body");
				MeshPathList.Add("MaskKnight_equipment");
				MatPathList.Add("MaskKnight_body");
				break;
			}
			break;
		case AvatarSuiteType.Gentleman:
			switch (avatar_type)
			{
			case AvatarType.Head:
				MeshPathList.Add("Current_head");
				MatPathList.Add("Gentleman_head");
				MeshPathList.Add("Gentleman_hat");
				MatPathList.Add("Gentleman_hat");
				break;
			case AvatarType.Body:
				MeshPathList.Add("Current_body");
				MatPathList.Add("Gentleman_body");
				break;
			}
			break;
		case AvatarSuiteType.ZombieAssassin:
			switch (avatar_type)
			{
			case AvatarType.Head:
				MeshPathList.Add("ZombieAssassin_head");
				MatPathList.Add("ZombieAssassin_head");
				break;
			case AvatarType.Body:
				MeshPathList.Add("Current_body");
				MatPathList.Add("ZombieAssassin_body");
				MeshPathList.Add("ZombieAssassin_equipment");
				MatPathList.Add("ZombieAssassin_equipment");
				break;
			}
			break;
		case AvatarSuiteType.DeathSquads:
			switch (avatar_type)
			{
			case AvatarType.Head:
				MeshPathList.Add("Current_head");
				MatPathList.Add("DeathSquads_head");
				MeshPathList.Add("DeathSquads_hat");
				MatPathList.Add("DeathSquads_hat");
				break;
			case AvatarType.Body:
				MeshPathList.Add("Current_body");
				MatPathList.Add("DeathSquads_body");
				break;
			}
			break;
		case AvatarSuiteType.Waiter:
			switch (avatar_type)
			{
			case AvatarType.Head:
				MeshPathList.Add("Current_head");
				MatPathList.Add("Waiter_head");
				MeshPathList.Add("Waiter_Eyeglass");
				MatPathList.Add("Swat_hat_Waiter_Eyeglass");
				break;
			case AvatarType.Body:
				MeshPathList.Add("Current_body");
				MatPathList.Add("Waiter_body");
				break;
			}
			break;
		case AvatarSuiteType.Mechanic:
			switch (avatar_type)
			{
			case AvatarType.Head:
				MeshPathList.Add("Current_head");
				MatPathList.Add("Mechanic_head");
				MeshPathList.Add("Mechanic_hat");
				MatPathList.Add("Mechanic_hat");
				break;
			case AvatarType.Body:
				MeshPathList.Add("Current_body");
				MatPathList.Add("Mechanic_body");
				break;
			}
			break;
		case AvatarSuiteType.Gladiator:
			switch (avatar_type)
			{
			case AvatarType.Head:
				MeshPathList.Add("Current_head");
				MatPathList.Add("Gladiator_head");
				MeshPathList.Add("Gladiator_hat");
				MatPathList.Add("Gladiator_hat");
				break;
			case AvatarType.Body:
				MeshPathList.Add("Current_body");
				MatPathList.Add("Gladiator_body");
				MeshPathList.Add("Gladiator_equipment");
				MatPathList.Add("Gladiator_equipment");
				break;
			}
			break;
		case AvatarSuiteType.ViolenceFr:
			switch (avatar_type)
			{
			case AvatarType.Head:
				MeshPathList.Add("Current_head");
				MatPathList.Add("ViolenceFr_head");
				MeshPathList.Add("ViolenceFr_hat");
				MatPathList.Add("ViolenceFr_hat");
				break;
			case AvatarType.Body:
				MeshPathList.Add("ViolenceFr_body");
				MatPathList.Add("ViolenceFr_body");
				break;
			}
			break;
		case AvatarSuiteType.SuperNemesis:
			switch (avatar_type)
			{
			case AvatarType.Head:
				MeshPathList.Add("SuperNemesis_head");
				MatPathList.Add("SuperNemesis_head");
				break;
			case AvatarType.Body:
				MeshPathList.Add("SuperNemesis_body");
				MatPathList.Add("SuperNemesis_body");
				break;
			}
			break;
		case AvatarSuiteType.EvilClown:
			switch (avatar_type)
			{
			case AvatarType.Head:
				MeshPathList.Add("Current_head");
				MatPathList.Add("EvilClown_head");
				MeshPathList.Add("EvilClown_hat");
				MatPathList.Add("EvilClown_hat");
				break;
			case AvatarType.Body:
				MeshPathList.Add("Current_body");
				MatPathList.Add("EvilClown_body");
				break;
			}
			break;
		case AvatarSuiteType.X800:
			switch (avatar_type)
			{
			case AvatarType.Head:
				MeshPathList.Add("Current_head");
				MatPathList.Add("X-800_head");
				break;
			case AvatarType.Body:
				MeshPathList.Add("Current_body");
				MatPathList.Add("X-800_body");
				MeshPathList.Add("X-800_equipment");
				MatPathList.Add("X-800_equipment");
				break;
			}
			break;
		case AvatarSuiteType.ShadowAgents:
			switch (avatar_type)
			{
			case AvatarType.Head:
				MeshPathList.Add("Current_head");
				MatPathList.Add("ShadowAgents_head");
				MeshPathList.Add("ShadowAgents_hat");
				MatPathList.Add("ShadowAgents_hat");
				break;
			case AvatarType.Body:
				MeshPathList.Add("Current_body");
				MatPathList.Add("ShadowAgents_body");
				break;
			}
			break;
		case AvatarSuiteType.Pirate:
			switch (avatar_type)
			{
			case AvatarType.Head:
				MeshPathList.Add("Current_head");
				MatPathList.Add("Corsair_head");
				MeshPathList.Add("Corsair_hat");
				MatPathList.Add("Corsair_hat");
				break;
			case AvatarType.Body:
				MeshPathList.Add("Corsair_body");
				MatPathList.Add("Corsair_body");
				break;
			}
			break;
		case AvatarSuiteType.Kunoichi:
			switch (avatar_type)
			{
			case AvatarType.Head:
				MeshPathList.Add("Kunoichi_head");
				MatPathList.Add("Kunoichi_head");
				break;
			case AvatarType.Body:
				MeshPathList.Add("Kunoichi_body");
				MatPathList.Add("Kunoichi_body");
				MeshPathList.Add("Kunoichi_equipment");
				MatPathList.Add("Kunoichi_equipment");
				break;
			}
			break;
		case AvatarSuiteType.Shinobi:
			switch (avatar_type)
			{
			case AvatarType.Head:
				MeshPathList.Add("Shinobi_head");
				MatPathList.Add("Shinobi_head");
				break;
			case AvatarType.Body:
				MeshPathList.Add("Shinobi_body");
				MatPathList.Add("Shinobi_body");
				MeshPathList.Add("Shinobi_equipment");
				MatPathList.Add("Shinobi_equipment");
				break;
			}
			break;
		case AvatarSuiteType.DemonLord:
			switch (avatar_type)
			{
			case AvatarType.Head:
				MeshPathList.Add("Current_head");
				MatPathList.Add("DemonLord_head");
				break;
			case AvatarType.Body:
				MeshPathList.Add("Current_body");
				MatPathList.Add("DemonLord_body");
				MeshPathList.Add("DemonLord_equipment");
				MatPathList.Add("DemonLord_equipment");
				break;
			}
			break;
		case AvatarSuiteType.Eskimo:
			switch (avatar_type)
			{
			case AvatarType.Head:
				MeshPathList.Add("Current_head");
				MatPathList.Add("Eskimo_head");
				MeshPathList.Add("Eskimo_hat");
				MatPathList.Add("Eskimo_hat");
				break;
			case AvatarType.Body:
				MeshPathList.Add("Current_body");
				MatPathList.Add("Eskimo_body");
				break;
			}
			break;
		case AvatarSuiteType.Rugby:
			switch (avatar_type)
			{
			case AvatarType.Head:
				MeshPathList.Add("Current_head");
				MatPathList.Add("Rugby_head");
				MeshPathList.Add("Rugby_hat");
				MatPathList.Add("Rugby_hat");
				break;
			case AvatarType.Body:
				MeshPathList.Add("Rugby_body");
				MatPathList.Add("Rugby_body");
				break;
			}
			break;
		case AvatarSuiteType.VanHelsing:
			switch (avatar_type)
			{
			case AvatarType.Head:
				MeshPathList.Add("Current_head");
				MatPathList.Add("Van Helsing_head");
				MeshPathList.Add("Van Helsing_hat");
				MatPathList.Add("Van Helsing_hat");
				break;
			case AvatarType.Body:
				MeshPathList.Add("Van Helsing_body");
				MatPathList.Add("Van Helsing_body");
				break;
			}
			break;
		}
	}
}
