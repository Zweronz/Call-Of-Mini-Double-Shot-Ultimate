namespace Zombie3D
{
	public class ItemBestRunner : NBattleShopItemImpl
	{
		private float SpeedConsume = 2.5f;

		public override void Init(Player player, NBattleShopItem item, int UseCount = -1)
		{
			base.Init(player, item, UseCount);
			m_PriceDollor = 1;
			m_iNumberOfUse = UseCount;
			m_iMaxNumberOfUse = 5;
			m_strIntroduce = "Halve stamina consumption for the rest of the match.";
		}

		public override void Do()
		{
			if (CanDo())
			{
				base.Do();
				m_Player.m_SpeedUpConsume = SpeedConsume;
			}
		}
	}
}
