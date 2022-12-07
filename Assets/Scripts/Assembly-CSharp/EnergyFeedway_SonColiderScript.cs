using System.Collections;
using UnityEngine;
using Zombie3D;

public class EnergyFeedway_SonColiderScript : MonoBehaviour
{
	private GameState _gameState;

	private IEnumerator Start()
	{
		yield return 0;
	}

	public void Init()
	{
		_gameState = GameApp.GetInstance().GetGameState();
	}

	private void OnTriggerEnter(Collider collider)
	{
		if (collider.gameObject.layer == 23)
		{
			WeaponBulletScript weaponBulletScript = collider.gameObject.GetComponent(typeof(WeaponBulletScript)) as WeaponBulletScript;
			if (weaponBulletScript != null)
			{
				(base.transform.parent.gameObject.GetComponent(typeof(EnergyFeedwayScript)) as EnergyFeedwayScript).OnHit(weaponBulletScript.Damage);
			}
		}
	}
}
