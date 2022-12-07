using UnityEngine;
using Zombie3D;

public class SkillCannonBulletScript : MonoBehaviour
{
	private float m_StartTime;

	private float m_Speed = 2f;

	private float m_AttackRange = 7.5f;

	private float m_Damage = 10f;

	public float Speed
	{
		get
		{
			return m_Speed;
		}
		set
		{
			m_Speed = value;
		}
	}

	public float AttackRange
	{
		get
		{
			return m_AttackRange;
		}
		set
		{
			m_AttackRange = value;
		}
	}

	public float Damage
	{
		get
		{
			return m_Damage;
		}
		set
		{
			m_Damage = value;
		}
	}

	public void Init()
	{
		m_StartTime = Time.time;
		base.GetComponent<Animation>()[base.GetComponent<Animation>().clip.name].wrapMode = WrapMode.Loop;
		base.GetComponent<Animation>().Play(base.GetComponent<Animation>().clip.name);
	}

	private void Start()
	{
	}

	private void Update()
	{
		base.transform.Translate(-Vector3.up * m_Speed * Time.deltaTime, Space.Self);
		if ((Time.time - m_StartTime) * m_Speed > AttackRange)
		{
			DestroyBullet();
		}
	}

	private void OnTriggerEnter(Collider collider)
	{
		GameObject gameObject = Object.Instantiate(Resources.Load("Zombie3D/Misc/SkillCannonBulletHitParticle"), base.gameObject.transform.position, base.gameObject.transform.rotation) as GameObject;
		DestroyBullet();
		Transform root = collider.gameObject.transform.root;
		if (root.gameObject.name.StartsWith("E_"))
		{
			Enemy enemyByID = GameApp.GetInstance().GetGameScene().GetEnemyByID(root.gameObject.name);
			if (enemyByID != null && enemyByID.HP > 0f)
			{
				DamageProperty damageProperty = new DamageProperty();
				damageProperty.damage = Damage;
				enemyByID.OnHit(damageProperty, WeaponType.NoGun);
			}
		}
	}

	private void DestroyBullet()
	{
		base.gameObject.SetActiveRecursively(false);
	}
}
