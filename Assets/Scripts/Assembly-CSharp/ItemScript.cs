using UnityEngine;
using Zombie3D;

public class ItemScript : MonoBehaviour
{
	public ItemType itemType;

	private bool moveUp;

	public float rotationSpeed = 45f;

	public bool enableUpandDown = true;

	protected float deltaTime;

	private void Start()
	{
	}

	private void Update()
	{
		deltaTime += Time.deltaTime;
		if (deltaTime < 0.03f)
		{
			return;
		}
		base.transform.Rotate(0f, rotationSpeed * deltaTime, 0f);
		if (enableUpandDown)
		{
			if (!moveUp)
			{
				float num = Mathf.MoveTowards(base.transform.position.y, 10001.1f, 0.2f * deltaTime);
				base.transform.position = new Vector3(base.transform.position.x, num, base.transform.position.z);
				if (num == 10001.1f)
				{
					moveUp = true;
				}
			}
			else
			{
				float num2 = Mathf.MoveTowards(base.transform.position.y, 10001.3f, 0.2f * deltaTime);
				base.transform.position = new Vector3(base.transform.position.x, num2, base.transform.position.z);
				if (num2 == 10001.3f)
				{
					moveUp = false;
				}
			}
		}
		deltaTime = 0f;
	}

	private void OnTriggerEnter(Collider c)
	{
		Player player = GameApp.GetInstance().GetGameScene().GetPlayer();
		player.OnPickUp(itemType);
		Object.Destroy(base.gameObject);
	}
}
