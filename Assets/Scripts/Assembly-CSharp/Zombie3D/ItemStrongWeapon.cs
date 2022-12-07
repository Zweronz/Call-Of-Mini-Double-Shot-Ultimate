using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zombie3D
{
	public class ItemStrongWeapon : NBattleShopItemImpl
	{
		private List<WeaponType> shopItemList = new List<WeaponType>();

		private int _StrongWeaponClass = 7;

		private int _rd = -1;

		public override void Init(Player player, NBattleShopItem item, int UseCount = -1)
		{
			base.Init(player, item, UseCount);
			m_iNumberOfUse = -1;
			m_iMaxNumberOfUse = 1;
			m_PriceGold = 2000;
			m_strIntroduce = "Gain use of a powerful weapon in weapon slot 1 for the rest of the match.";
			ArrayList weapons = ConfigManager.Instance().GetFixedConfig().weapons;
			for (int i = 0; i < weapons.Count; i++)
			{
				FixedConfig.WeaponCfg weaponCfg = (FixedConfig.WeaponCfg)weapons[i];
				if (weaponCfg.mClass == _StrongWeaponClass)
				{
					shopItemList.Add((WeaponType)weaponCfg.type);
				}
			}
			foreach (WeaponType weapon in player.WeaponList)
			{
				FixedConfig.WeaponCfg weaponCfg2 = ConfigManager.Instance().GetFixedConfig().GetWeaponCfg(weapon);
				if (weaponCfg2.mClass == _StrongWeaponClass)
				{
					shopItemList.Remove(weapon);
				}
			}
			_rd = Random.Range(0, shopItemList.Count);
		}

		public WeaponType GetWeaponType()
		{
			if (_rd >= 0)
			{
				return shopItemList[_rd];
			}
			return WeaponType.NoGun;
		}

		public override void Buy()
		{
			base.Buy();
		}

		public override void Do()
		{
			if (CanDo())
			{
				base.Do();
				if (_rd >= 0)
				{
					m_Player.ModificationWeaponList(GetWeaponType(), true);
				}
			}
		}
	}
}
