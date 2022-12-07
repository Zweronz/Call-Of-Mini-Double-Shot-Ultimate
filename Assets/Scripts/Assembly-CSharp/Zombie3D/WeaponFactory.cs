using UnityEngine;

namespace Zombie3D
{
	public class WeaponFactory
	{
		protected static WeaponFactory instance;

		public static WeaponFactory GetInstance()
		{
			if (instance == null)
			{
				instance = new WeaponFactory();
			}
			return instance;
		}

		public Weapon CreateWeapon(WeaponType wType)
		{
			Debug.Log("CreateWeapon: " + wType);
			Weapon result = null;
			switch (wType)
			{
			case WeaponType.Beretta_33:
				result = new Beretta_33();
				break;
			case WeaponType.GrewCar_15:
				result = new GrewCar_15();
				break;
			case WeaponType.UZI_E:
				result = new UZI_E();
				break;
			case WeaponType.RemingtonPipe:
				result = new RemingtonPipe();
				break;
			case WeaponType.Springfield_9mm:
				result = new Springfield_9mm();
				break;
			case WeaponType.Kalashnikov_II:
				result = new Kalashnikov_II();
				break;
			case WeaponType.Barrett_P90:
				result = new Barrett_P90();
				break;
			case WeaponType.ParkerGaussRifle:
				result = new ParkerGaussRifle();
				break;
			case WeaponType.ZombieBusters:
				result = new ZombieBusters();
				break;
			case WeaponType.SimonovPistol:
				result = new SimonovPistol();
				break;
			case WeaponType.BarrettSplitIII:
				result = new BarrettSplitIII();
				break;
			case WeaponType.Tomahawk:
				result = new Tomahawk();
				break;
			case WeaponType.SimonoRayRifle:
				result = new SimonoRayRifle();
				break;
			case WeaponType.Volcano:
				result = new Volcano();
				break;
			case WeaponType.Hellfire:
				result = new Hellfire();
				break;
			case WeaponType.Nailer:
				result = new Nailer();
				break;
			case WeaponType.NeutronRifle:
				result = new NeutronRifle();
				break;
			case WeaponType.BigFirework:
				result = new BigFirework();
				break;
			case WeaponType.Stormgun:
				result = new Stormgun();
				break;
			case WeaponType.Lightning:
				result = new Lightning();
				break;
			case WeaponType.MassacreCannon:
				result = new MassacreCannon();
				break;
			case WeaponType.DoubleSnake:
				result = new DoubleSnake();
				break;
			case WeaponType.Longinus:
				result = new Longinus();
				break;
			case WeaponType.CrossBow:
				result = new CrossBow();
				break;
			case WeaponType.Messiah:
				result = new Messiah();
				break;
			case WeaponType.Ion_Cannon:
				result = new CannonI();
				break;
			case WeaponType.Ion_CannonI:
				result = new CannonII();
				break;
			case WeaponType.Ion_CannonSub:
				result = new CannonSub();
				break;
			}
			return result;
		}

		public GameObject CreateWeaponModel(string weaponName, Vector3 pos, Quaternion rotation)
		{
			return null;
		}

		public GameObject CreateWeapon2DModel(string weaponName, Vector3 pos, Quaternion rotation)
		{
			return null;
		}

		public GameObject CreateWeapon2DModelMask(string weaponName, Vector3 pos, Quaternion rotation)
		{
			return null;
		}
	}
}
