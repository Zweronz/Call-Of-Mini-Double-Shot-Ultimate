using UnityEngine;
using Zombie3D;

public class CopBombScript : MonoBehaviour
{
	protected float startTime;

	public float explodeTime = 4f;

	public float radius = 5f;

	public float damage = 20f;

	public Vector3 speed;

	private void Start()
	{
		startTime = Time.time;
	}

	private void Update()
	{
		if (Time.time - startTime > explodeTime)
		{
			Player player = GameApp.GetInstance().GetGameScene().GetPlayer();
			if ((base.transform.position - player.GetTransform().position).sqrMagnitude < radius * radius)
			{
				player.OnHit(damage);
			}
			Object.Instantiate(GameApp.GetInstance().GetGameConfig().rocketExlposion, base.transform.position, Quaternion.identity);
			Object.Destroy(base.gameObject);
		}
	}
}
