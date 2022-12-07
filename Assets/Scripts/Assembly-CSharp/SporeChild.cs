using UnityEngine;
using Zombie3D;

public class SporeChild : MonoBehaviour
{
	public enum SporeChildType
	{
		Common = 0,
		Special = 1
	}

	public SporeChildType m_Type;

	public Spore ParentSpore;

	public SporeII ParentSporeII;

	public EnemySkillExplodeSpore ParentSporeSkill;

	public float hp = 10f;

	public float damagePercent = 10f;

	private GameObject m_Circle;

	private bool bZoomIn;

	private bool bZoomOut;

	private float m_ExplodeTimer = -1f;

	private float m_ExplodeTime = 3.5f;

	private void Start()
	{
		base.GetComponent<Animation>()["Idle01"].wrapMode = WrapMode.Loop;
		base.GetComponent<Animation>().CrossFade("Idle01");
		if (GameApp.GetInstance().GetGameState().SoundOn)
		{
			GameObject gameObject = base.transform.Find("Audio/Born").gameObject;
			if (gameObject != null)
			{
				AudioSource audioSource = gameObject.GetComponent<AudioSource>();
				if (audioSource != null)
				{
					audioSource.loop = false;
					audioSource.Play();
				}
			}
		}
		bZoomIn = true;
		base.gameObject.transform.localScale = new Vector3(0.02f, 0.02f, 0.02f);
		m_Circle = Object.Instantiate(GameApp.GetInstance().GetGameConfig().SporeParticles11, base.transform.position, Quaternion.identity) as GameObject;
		if (m_Circle.transform.position.y < 10000.159f)
		{
			m_Circle.transform.position = new Vector3(base.transform.position.x, 10000.3f, base.transform.position.z);
		}
	}

	private void Update()
	{
		if (m_Type == SporeChildType.Common)
		{
			if (hp <= 0f)
			{
				if (!bZoomOut)
				{
					Dead();
					return;
				}
				base.gameObject.transform.localScale = Vector3.Lerp(base.gameObject.transform.localScale, Vector3.zero, Time.deltaTime * 10f);
				if (base.gameObject.transform.localScale.x <= 0.1f)
				{
					Object.Destroy(base.gameObject);
					Object.Destroy(m_Circle);
				}
				return;
			}
			if (bZoomIn)
			{
				base.gameObject.transform.localScale = Vector3.Lerp(base.gameObject.transform.localScale, new Vector3(1f, 1f, 1f), Time.deltaTime * 10f);
				if (base.gameObject.transform.localScale.x >= 0.8f)
				{
					m_ExplodeTimer = 0f;
					bZoomIn = false;
					base.gameObject.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
				}
			}
			if (m_ExplodeTimer >= 0f)
			{
				m_ExplodeTimer += Time.deltaTime;
				if (m_ExplodeTimer > m_ExplodeTime)
				{
					m_ExplodeTimer = -1f;
					bZoomOut = true;
					if (m_Circle != null)
					{
						Object.Destroy(m_Circle);
					}
					GameObject obj = Object.Instantiate(GameApp.GetInstance().GetGameConfig().SporeParticles07, base.transform.position, Quaternion.identity) as GameObject;
					Object.Destroy(obj, 1f);
					Player player = GameApp.GetInstance().GetGameScene().GetPlayer();
					if (player != null && player.HP > 0f && Vector3.Distance(player.GetTransform().position, base.transform.position) < 5f)
					{
						player.OnHitBack(0.3f, 0.8f, player.GetTransform().position - base.transform.position);
						float damage = player.GetMaxHp() * damagePercent + 10f;
						player.OnHit(damage);
					}
					Player friendPlayer = GameApp.GetInstance().GetGameScene().GetFriendPlayer();
					if (friendPlayer != null && friendPlayer.HP > 0f && Vector3.Distance(friendPlayer.GetTransform().position, base.transform.position) < 5f)
					{
						friendPlayer.OnHitBack(0.3f, 0.8f, friendPlayer.GetTransform().position - base.transform.position);
						float damage2 = friendPlayer.GetMaxHp() * damagePercent + 10f;
						friendPlayer.OnHit(damage2);
					}
				}
			}
			if (bZoomOut)
			{
				base.gameObject.transform.localScale = Vector3.Lerp(base.gameObject.transform.localScale, Vector3.zero, Time.deltaTime * 10f);
				if (base.gameObject.transform.localScale.x <= 0.1f)
				{
					Object.Destroy(base.gameObject);
					Object.Destroy(m_Circle);
				}
			}
		}
		else if (hp <= 0f)
		{
			if (!bZoomOut)
			{
				Dead();
				return;
			}
			base.gameObject.transform.localScale = Vector3.Lerp(base.gameObject.transform.localScale, Vector3.zero, Time.deltaTime * 10f);
			if (base.gameObject.transform.localScale.x <= 0.1f)
			{
				Object.Destroy(base.gameObject);
				Object.Destroy(m_Circle);
			}
		}
		else if (bZoomIn)
		{
			base.gameObject.transform.localScale = Vector3.Lerp(base.gameObject.transform.localScale, new Vector3(1f, 1f, 1f), Time.deltaTime * 10f);
			if (base.gameObject.transform.localScale.x >= 0.8f)
			{
				bZoomIn = false;
				base.gameObject.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
			}
		}
	}

	private void OnCollisionEnter(Collision collisionInfo)
	{
		if (collisionInfo.gameObject.layer == 23)
		{
			WeaponBulletScript weaponBulletScript = collisionInfo.gameObject.GetComponent(typeof(WeaponBulletScript)) as WeaponBulletScript;
			if (weaponBulletScript != null)
			{
				hp -= weaponBulletScript.Damage;
			}
			if (hp <= 0f)
			{
				Dead();
			}
		}
	}

	public void Dead()
	{
		if (ParentSpore != null)
		{
			ParentSpore.OnSporeChildDead(this);
		}
		if (ParentSporeII != null)
		{
			ParentSporeII.OnSporeChildDead(this);
		}
		if (ParentSporeSkill != null)
		{
			ParentSporeSkill.OnSporeChildDead(this);
		}
		if (GameApp.GetInstance().GetGameState().SoundOn)
		{
			GameObject gameObject = base.transform.Find("Audio/Dead").gameObject;
			if (gameObject != null)
			{
				AudioSource audioSource = gameObject.GetComponent<AudioSource>();
				if (audioSource != null)
				{
					audioSource.loop = false;
					audioSource.Play();
				}
			}
		}
		bZoomOut = true;
	}

	public void Clear()
	{
		if (base.gameObject != null)
		{
			Object.Destroy(base.gameObject);
		}
		if (m_Circle != null)
		{
			Object.Destroy(m_Circle);
		}
	}
}
