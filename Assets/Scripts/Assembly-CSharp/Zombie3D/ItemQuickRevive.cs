namespace Zombie3D
{
	public class ItemQuickRevive : NBattleShopItemImpl
	{
		private float ReceiveFac = 2f;

		public override void Init(Player player, NBattleShopItem item, int UseCount = -1)
		{
			base.Init(player, item, UseCount);
			m_PriceDollor = 1;
			m_iNumberOfUse = UseCount;
			m_iMaxNumberOfUse = 5;
			m_strIntroduce = "Halve the respawn cooldown time for the rest of the match.";
		}

		public override void Do()
		{
			if (CanDo())
			{
				base.Do();
				GameSetup.Instance.ReviveTime /= ReceiveFac;
			}
		}
	}
}
