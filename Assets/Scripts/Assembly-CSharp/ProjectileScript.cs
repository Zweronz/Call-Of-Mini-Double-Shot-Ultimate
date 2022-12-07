using UnityEngine;
using Zombie3D;

[AddComponentMenu("TPS/ProjectileScript")]
public class ProjectileScript : MonoBehaviour
{
	protected GameObject resObject;

	protected GameObject explodeObject;

	protected GameObject smallExplodeObject;

	protected GameObject laserHitObject;

	public Transform targetTransform;

	protected Transform proTransform;

	protected WeaponType gunType;

	protected GameConfigScript gConf;

	public Vector3 dir;

	public float hitForce;

	public float explodeRadius;

	public float flySpeed;

	public Vector3 speed;

	public float life = 2f;

	public float damage;

	protected float createdTime;

	protected float lastTriggerTime;

	protected float gravity = 16f;

	protected float downSpeed;

	protected float deltaTime;

	public WeaponType GunType
	{
		set
		{
			gunType = value;
		}
	}

	public void Start()
	{
		gConf = GameApp.GetInstance().GetGameConfig();
		resObject = gConf.projectile;
		explodeObject = gConf.rocketExlposion;
		proTransform = base.transform;
		laserHitObject = gConf.laserHit;
		createdTime = Time.time;
	}

	public void Update()
	{
		deltaTime += Time.deltaTime;
		if (!(deltaTime < 0.03f))
		{
			proTransform.Translate(flySpeed * dir * deltaTime, Space.World);
			if (Time.time - createdTime > life)
			{
				Object.DestroyObject(base.gameObject);
			}
			deltaTime = 0f;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		GameScene gameScene = GameApp.GetInstance().GetGameScene();
		Player player = gameScene.GetPlayer();
	}
}
