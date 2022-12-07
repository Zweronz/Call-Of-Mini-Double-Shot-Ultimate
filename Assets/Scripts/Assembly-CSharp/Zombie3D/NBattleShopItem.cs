namespace Zombie3D
{
	public class NBattleShopItem
	{
		public enBattlefieldProps BattlefieldProps { get; set; }

		public NBattleShopItem()
		{
			BattlefieldProps = enBattlefieldProps.E_QuickRevive;
		}

		public NBattleShopItem(enBattlefieldProps type)
		{
			BattlefieldProps = type;
		}
	}
}
