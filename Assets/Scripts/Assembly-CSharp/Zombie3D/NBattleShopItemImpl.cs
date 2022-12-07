using UnityEngine;

namespace Zombie3D
{
	public class NBattleShopItemImpl
	{
		protected Player m_Player;

		protected NBattleShopItem m_NBShopItem;

		protected int m_iNumberOfUse = -1;

		protected int m_iMaxNumberOfUse = 5;

		protected int m_PriceGold;

		protected int m_PriceDollor;

		protected string m_strIntroduce = string.Empty;

		public int PriceGold
		{
			get
			{
				return m_PriceGold;
			}
		}

		public int PriceDollor
		{
			get
			{
				return m_PriceDollor;
			}
		}

		public string Introduc
		{
			get
			{
				return m_strIntroduce;
			}
			set
			{
				m_strIntroduce = value;
			}
		}

		public int NumberOfUse
		{
			get
			{
				return m_iNumberOfUse;
			}
		}

		public NBattleShopItem GetItem()
		{
			return m_NBShopItem;
		}

		public bool CanBuy()
		{
			if (GameApp.GetInstance().GetGameState().GetGold() >= m_PriceGold && GameApp.GetInstance().GetGameState().GetDollor() >= m_PriceDollor)
			{
				return true;
			}
			return false;
		}

		public virtual void Init(Player player, NBattleShopItem item, int UseCount)
		{
			m_Player = player;
			m_NBShopItem = item;
		}

		public virtual void Buy()
		{
			if (m_PriceDollor > 0)
			{
				GameApp.GetInstance().GetGameState().LoseDollor(m_PriceDollor);
			}
			if (m_PriceGold > 0)
			{
				GameApp.GetInstance().GetGameState().LoseGold(m_PriceGold);
			}
			m_iNumberOfUse = m_iMaxNumberOfUse;
			if (GameApp.GetInstance().GetGameState().m_eGameMode.m_PlaersNBattleBuff.ContainsKey(m_NBShopItem.BattlefieldProps))
			{
				GameApp.GetInstance().GetGameState().m_eGameMode.m_PlaersNBattleBuff[m_NBShopItem.BattlefieldProps] = m_iNumberOfUse;
			}
			else
			{
				GameApp.GetInstance().GetGameState().m_eGameMode.m_PlaersNBattleBuff.Add(m_NBShopItem.BattlefieldProps, m_iNumberOfUse);
			}
			GameApp.GetInstance().Save();
		}

		public virtual bool CanDo()
		{
			bool result = false;
			Debug.LogWarning(string.Concat(GetItem().BattlefieldProps, " | ", m_iNumberOfUse));
			if (m_iNumberOfUse > 0)
			{
				m_iNumberOfUse--;
				result = true;
				Debug.Log(string.Concat("Use Minus", GetItem().BattlefieldProps, " | ", m_iNumberOfUse));
			}
			else
			{
				m_iNumberOfUse = -1;
				Debug.Log("Error. 无法使用，没有使用次数");
			}
			if (GameApp.GetInstance().GetGameState().m_eGameMode.m_PlaersNBattleBuff.ContainsKey(m_NBShopItem.BattlefieldProps))
			{
				GameApp.GetInstance().GetGameState().m_eGameMode.m_PlaersNBattleBuff[m_NBShopItem.BattlefieldProps] = m_iNumberOfUse;
			}
			else
			{
				GameApp.GetInstance().GetGameState().m_eGameMode.m_PlaersNBattleBuff.Add(m_NBShopItem.BattlefieldProps, m_iNumberOfUse);
			}
			GameApp.GetInstance().Save();
			return result;
		}

		public virtual void Do()
		{
		}
	}
}
