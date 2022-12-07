using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zombie3D;

public class EnergyFeedwayScript : MonoBehaviour
{
	public float maxHp = 7000f;

	public float radius = 10f;

	public List<GameObject> Energy_Laser;

	protected BloodRect bloodRect;

	protected float lastUpdateTime;

	private float curHp;

	private GameState _gameState;

	private IEnumerator Start()
	{
		yield return 0;
	}

	public void Init()
	{
		lastUpdateTime = 0f;
		_gameState = GameApp.GetInstance().GetGameState();
		maxHp = CalcMaxBlood();
		curHp = maxHp;
		GameObject gameObject = base.transform.Find("BloodRect").gameObject;
		if (gameObject != null)
		{
			bloodRect = gameObject.GetComponent(typeof(BloodRect)) as BloodRect;
			if (bloodRect != null)
			{
				bloodRect.SetBloodPercent(1f);
			}
		}
	}

	private void Update()
	{
		if (Time.time - lastUpdateTime < 0.001f)
		{
			return;
		}
		lastUpdateTime = Time.time;
		if (bloodRect != null)
		{
			bloodRect.SetBloodPercent(Mathf.Clamp01(curHp / maxHp));
		}
		if (curHp <= 0f)
		{
			GameObject gameObject = null;
			gameObject = Object.Instantiate(GameApp.GetInstance().GetGameConfig().Exlposion03, base.transform.Find("machine10").position + base.transform.up * 0.5f, Quaternion.identity) as GameObject;
			if (GameApp.GetInstance().GetGameState().SoundOn && gameObject.GetComponent<AudioSource>() != null)
			{
				gameObject.GetComponent<AudioSource>().Play();
			}
			gameObject = Object.Instantiate(Resources.Load("Zombie3D/Misc/EnergyFeedwayExp", typeof(GameObject)), base.transform.Find("Ball_Colider").position + base.transform.up * 0.5f, base.transform.Find("Ball_Colider").rotation) as GameObject;
			if (GameApp.GetInstance().GetGameState().SoundOn && gameObject.GetComponent<AudioSource>() != null)
			{
				gameObject.GetComponent<AudioSource>().Play();
			}
			Object.Destroy(base.gameObject);
			GameApp.GetInstance().GetGameScene().unlockNextBallPlaces(this);
		}
	}

	private void OnTriggerEnter(Collider collider)
	{
		if (collider.gameObject.layer == 23)
		{
			WeaponBulletScript weaponBulletScript = collider.gameObject.GetComponent(typeof(WeaponBulletScript)) as WeaponBulletScript;
			if (weaponBulletScript != null)
			{
				OnHit(weaponBulletScript.Damage);
			}
		}
	}

	public void OnHit(float damage)
	{
		curHp -= damage;
	}

	public float CalcMaxBlood()
	{
		float num = 7000f;
		int map_index = 1;
		int points_index = 1;
		int wave_index = 1;
		_gameState.GetGameTriggerInfo(ref map_index, ref points_index, ref wave_index);
		int num2 = points_index;
		switch (points_index)
		{
		case 1:
			num2 = 2;
			break;
		case 2:
			num2 = 5;
			break;
		}
		float num3 = (float)(num2 - 1) * 0.6f;
		return num * (1f + num3);
	}

	public void AddBlood()
	{
		float num = 1000f;
		int map_index = 1;
		int points_index = 1;
		int wave_index = 1;
		_gameState.GetGameTriggerInfo(ref map_index, ref points_index, ref wave_index);
		int num2 = points_index;
		switch (points_index)
		{
		case 1:
			num2 = 2;
			break;
		case 2:
			num2 = 5;
			break;
		}
		float num3 = (float)(num2 - 1) * 0.6f;
		num *= 1f + num3;
		curHp += num;
		maxHp += num;
	}
}
