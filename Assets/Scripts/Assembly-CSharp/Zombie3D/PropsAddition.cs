namespace Zombie3D
{
	public class PropsAddition
	{
		public enPropsAdditionType PropsType { get; set; }

		public enPropsAdditionPart PropsPart { get; set; }

		public uint Level { get; set; }

		public PropsAddition()
		{
			PropsType = enPropsAdditionType.E_None;
			PropsPart = enPropsAdditionPart.E_Last;
			Level = 1u;
		}

		public PropsAddition(enPropsAdditionType type, enPropsAdditionPart part, uint level)
		{
			PropsType = type;
			PropsPart = part;
			Level = level;
		}
	}
}
