namespace Zombie3D
{
	public class NBattleShopItemImplFactory
	{
		public static NBattleShopItemImpl CreateNBShopItemImpl(enBattlefieldProps item_type)
		{
			NBattleShopItemImpl result = null;
			switch (item_type)
			{
			case enBattlefieldProps.E_QuickRevive:
				result = new ItemQuickRevive();
				break;
			case enBattlefieldProps.E_BestRunner:
				result = new ItemBestRunner();
				break;
			case enBattlefieldProps.E_Tenacity:
				result = new ItemTenacity();
				break;
			case enBattlefieldProps.E_AnaestheticProjectile:
				result = new ItemAnaestheticProjectile();
				break;
			case enBattlefieldProps.E_StrongWeapon:
				result = new ItemStrongWeapon();
				break;
			}
			return result;
		}
	}
}
