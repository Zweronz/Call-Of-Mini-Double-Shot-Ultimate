namespace Zombie3D
{
	public class ItemAnaestheticProjectile : NBattleShopItemImpl
	{
		private float _fTimer = 2f;

		private float _fProbability = 0.1f;

		public float GetTimer()
		{
			return _fTimer;
		}

		public float GetProbability()
		{
			return _fProbability;
		}

		public override void Init(Player player, NBattleShopItem item, int UseCount = -1)
		{
			base.Init(player, item, UseCount);
			m_PriceDollor = 1;
			_fTimer = 2f;
			_fProbability = 0.1f;
			m_iNumberOfUse = UseCount;
			m_iMaxNumberOfUse = 5;
			m_strIntroduce = "Attacks have a 10% chance to anesthetize a target for 2 seconds.";
		}

		public override void Do()
		{
			if (CanDo())
			{
				base.Do();
			}
		}
	}
}
