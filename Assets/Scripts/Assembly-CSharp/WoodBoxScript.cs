using UnityEngine;
using Zombie3D;

public class WoodBoxScript : MonoBehaviour
{
	public float hp = 10f;

	protected GameConfigScript gConf;

	protected Transform boxTransform;

	private void Start()
	{
		boxTransform = base.gameObject.transform;
	}

	private void Update()
	{
	}

	public void OnHit(float damage)
	{
		gConf = GameApp.GetInstance().GetGameConfig();
		hp -= damage;
		if (hp <= 0f)
		{
			Object.Destroy(base.gameObject);
			Object.Instantiate(gConf.woodExplode, base.transform.position, Quaternion.identity);
			SendMessage("OnLoot");
		}
	}
}
