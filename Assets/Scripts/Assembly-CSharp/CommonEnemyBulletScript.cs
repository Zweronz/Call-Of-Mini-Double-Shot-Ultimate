using UnityEngine;
using Zombie3D;

public class CommonEnemyBulletScript : MonoBehaviour
{
	public enum BulletForwardDirection
	{
		forward = 0,
		back = 1,
		up = 2,
		down = 3,
		right = 4,
		left = 5
	}

	private EnemyType enemyType = EnemyType.E_ZOMBIE;

	private float damage = 15f;

	private float attackRange = 20f;

	private float startTime;

	private float rot;

	private float flySpeed = 2.6f;

	private Vector3 forwardDir = Vector3.forward;

	public bool bUpdateCheckHit;

	public EnemyType EnemyType
	{
		get
		{
			return enemyType;
		}
		set
		{
			enemyType = value;
		}
	}

	public float Damage
	{
		get
		{
			return damage;
		}
		set
		{
			damage = value;
		}
	}

	public float Rot
	{
		get
		{
			return rot;
		}
		set
		{
			rot = value;
		}
	}

	public float Speed
	{
		get
		{
			return flySpeed;
		}
		set
		{
			flySpeed = value;
		}
	}

	public float AttackRange
	{
		get
		{
			return attackRange;
		}
		set
		{
			attackRange = value;
		}
	}

	public Vector3 ForwardDir
	{
		set
		{
			forwardDir = value;
		}
	}

	public BulletForwardDirection ForwardDirection
	{
		set
		{
			switch (value)
			{
			case BulletForwardDirection.forward:
				forwardDir = Vector3.forward;
				break;
			case BulletForwardDirection.back:
				forwardDir = -Vector3.forward;
				break;
			case BulletForwardDirection.up:
				forwardDir = Vector3.up;
				break;
			case BulletForwardDirection.down:
				forwardDir = -Vector3.up;
				break;
			case BulletForwardDirection.right:
				forwardDir = Vector3.right;
				break;
			case BulletForwardDirection.left:
				forwardDir = -Vector3.right;
				break;
			}
		}
	}

	private void Start()
	{
		startTime = Time.time;
	}

	private void Update()
	{
		base.transform.Translate(forwardDir * flySpeed * Time.deltaTime, Space.Self);
		if (bUpdateCheckHit)
		{
			Player player = GameApp.GetInstance().GetGameScene().GetPlayer();
			float num = Vector3.Distance(player.GetTransform().position, new Vector3(base.transform.position.x, player.GetTransform().position.y, base.transform.position.z));
			if (num <= 1f)
			{
				player.OnHit(damage);
				Object.Destroy(base.gameObject);
				return;
			}
		}
		if (base.transform.position.y < 10000.01f)
		{
			DestroyBullet();
		}
		else if ((Time.time - startTime) * flySpeed > AttackRange)
		{
			DestroyBullet();
		}
		else if ((Time.time - startTime) * flySpeed > 20f)
		{
			DestroyBullet();
		}
	}

	public void DestroyBullet()
	{
		Object.Destroy(base.gameObject);
	}

	private void OnCollisionEnter(Collision collisionInfo)
	{
		Debug.Log("CommonEnemyBulletScript.OnCollisionEnter - " + collisionInfo.gameObject.name);
	}

	private void OnTriggerEnter(Collider collider)
	{
		bool flag = true;
		Transform root = collider.gameObject.transform.root;
		if (root.gameObject.layer == 8 || root.gameObject.layer == 27)
		{
			Player player = GameApp.GetInstance().GetGameScene().GetPlayer();
			Player friendPlayer = GameApp.GetInstance().GetGameScene().GetFriendPlayer();
			if (root.gameObject.GetInstanceID() == player.PlayerObject.GetInstanceID())
			{
				player.OnHit(damage);
			}
			if (friendPlayer != null && root.gameObject.GetInstanceID() == friendPlayer.PlayerObject.GetInstanceID())
			{
				friendPlayer.OnHit(damage);
			}
			if (enemyType == EnemyType.E_TRACKER || enemyType == EnemyType.E_TURRETER)
			{
				flag = false;
				Vector3 position = base.transform.position + 1.5f * base.transform.up;
				GameObject gameObject = Object.Instantiate(GameApp.GetInstance().GetGameConfig().rocketExlposion, position, Quaternion.identity) as GameObject;
			}
		}
		if (flag)
		{
			Vector3 direction = forwardDir;
			if (forwardDir == Vector3.forward)
			{
				direction = base.transform.forward;
			}
			else if (forwardDir == -Vector3.forward)
			{
				direction = -base.transform.forward;
			}
			else if (forwardDir == Vector3.up)
			{
				direction = base.transform.up;
			}
			else if (forwardDir == -Vector3.up)
			{
				direction = -base.transform.up;
			}
			else if (forwardDir == Vector3.right)
			{
				direction = base.transform.right;
			}
			else if (forwardDir == -Vector3.right)
			{
				direction = -base.transform.right;
			}
			RaycastHit hitInfo = default(RaycastHit);
			if (collider.Raycast(new Ray(base.transform.position, direction), out hitInfo, 10f))
			{
				GameObject gameObject2 = Object.Instantiate(GameApp.GetInstance().GetGameConfig().hitParticles01, hitInfo.point, Quaternion.identity) as GameObject;
				RemoveTimerScript removeTimerScript = gameObject2.AddComponent(typeof(RemoveTimerScript)) as RemoveTimerScript;
				removeTimerScript.life = 0.2f;
			}
		}
		DestroyBullet();
	}
}
