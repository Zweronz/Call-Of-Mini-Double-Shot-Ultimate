namespace Zombie3D
{
	public class ItemTenacity : NBattleShopItemImpl
	{
		private float _AddHpLimit = 500f;

		public override void Init(Player player, NBattleShopItem item, int UseCount = -1)
		{
			base.Init(player, item, UseCount);
			m_PriceDollor = 1;
			m_iNumberOfUse = UseCount;
			m_iMaxNumberOfUse = 5;
			m_strIntroduce = "+500 HP for the rest of the match.";
		}

		public override void Do()
		{
			if (CanDo())
			{
				base.Do();
				m_Player.m_nHpLimit = _AddHpLimit;
				m_Player.CalcMaxHp();
			}
		}
	}
}
