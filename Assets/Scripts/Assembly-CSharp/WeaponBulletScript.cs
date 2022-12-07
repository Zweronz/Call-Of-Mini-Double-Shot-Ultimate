using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zombie3D;

public class WeaponBulletScript : MonoBehaviour
{
	public GameObject BulletHitPrefab;

	public GameObject ShadowLightPrefab;

	private GameObject ShadowLight;

	public Vector3 ShadowPosOffset = Vector3.zero;

	public float flashFrameTime = -1f;

	public Weapon m_Weapon;

	private WeaponType weaponType;

	private float damage = 15f;

	private float attackRange = 15f;

	private float startTime;

	private float rot;

	private float flySpeed = 2.6f;

	public Vector3 shadowLightScale = Vector3.one;

	public float shadowLightFlashMaxAlpha = 1f;

	public bool showShadowLight = true;

	private Enemy autoTraceEnemy;

	private bool hitAndDestroy = true;

	public bool isMassacreCannonSubBullet;

	public float initAngel;

	private int m_trailCounter = -1;

	private float m_TrailRendererTime;

	public Player OwnedPlayer { get; set; }

	public WeaponType WeaponType
	{
		get
		{
			return m_Weapon.GetWeaponType();
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

	public Enemy AutoTraceEnemy
	{
		get
		{
			return autoTraceEnemy;
		}
		set
		{
			autoTraceEnemy = value;
		}
	}

	public bool HitAndDestroy
	{
		get
		{
			return hitAndDestroy;
		}
		set
		{
			hitAndDestroy = value;
		}
	}

	public void Init()
	{
		startTime = Time.time;
		if (m_Weapon != null)
		{
			OwnedPlayer = m_Weapon.GetOwnedPlayer();
		}
		if (GameApp.GetInstance().GetGameState().m_eGameMode.m_ePlayMode != 0)
		{
			ShadowLightPrefab = null;
			attackRange = 8f;
		}
		if (showShadowLight && ShadowLightPrefab != null)
		{
			Vector3 position = new Vector3(base.transform.position.x, 10000.2f, base.transform.position.z);
			if (ShadowLight == null)
			{
				ShadowLight = Object.Instantiate(ShadowLightPrefab, position, Quaternion.Euler(270f, 90f, 0f)) as GameObject;
			}
			ShadowLight.transform.position = position;
			ShadowLight.transform.rotation = Quaternion.Euler(270f, 90f, 0f);
			ShadowLightFlash shadowLightFlash = ShadowLight.GetComponent(typeof(ShadowLightFlash)) as ShadowLightFlash;
			if (shadowLightFlash != null)
			{
				shadowLightFlash.transform.localScale = shadowLightScale;
				shadowLightFlash.MaxAlpha = shadowLightFlashMaxAlpha;
				if (flashFrameTime > 0f)
				{
					shadowLightFlash.FrameTime = flashFrameTime;
				}
				ShadowLight.transform.Rotate(Vector3.forward, Rot);
			}
			ShadowLight.active = true;
		}
		m_trailCounter = -1;
		TrailRenderer component = base.gameObject.GetComponent<TrailRenderer>();
		if (component != null)
		{
			component.time = 0f;
			m_trailCounter = 0;
		}
	}

	private void Awake()
	{
		HitAndDestroy = true;
		TrailRenderer component = base.gameObject.GetComponent<TrailRenderer>();
		if (component != null)
		{
			m_TrailRendererTime = component.time;
		}
	}

	private void Start()
	{
	}

	private void Update()
	{
		base.transform.Translate(Vector3.up * flySpeed * Time.deltaTime, Space.Self);
		if (ShadowLight != null)
		{
			Vector3 position = new Vector3(base.transform.position.x, 10000.05f, base.transform.position.z);
			position += base.transform.right * ShadowPosOffset.x;
			position += base.transform.up * ShadowPosOffset.y;
			position += base.transform.forward * ShadowPosOffset.z;
			ShadowLight.transform.position = position;
		}
		if (base.transform.position.y < 10000.1f)
		{
			DestroyBullet();
		}
		else if ((Time.time - startTime) * flySpeed > AttackRange)
		{
			DestroyBullet();
		}
		else
		{
			if (m_trailCounter < 0)
			{
				return;
			}
			m_trailCounter++;
			if (m_trailCounter >= 1)
			{
				TrailRenderer component = base.gameObject.GetComponent<TrailRenderer>();
				if (component != null)
				{
					component.time = m_TrailRendererTime;
					m_trailCounter = -1;
				}
			}
		}
	}

	public void DestroyBullet(bool bDestoyGameObject = false)
	{
		if (bDestoyGameObject)
		{
			if (ShadowLight != null)
			{
				Object.Destroy(ShadowLight);
			}
			Object.Destroy(base.gameObject);
			return;
		}
		TrailRenderer component = base.gameObject.GetComponent<TrailRenderer>();
		if (component != null)
		{
			component.time = 0f;
		}
		if (m_Weapon != null)
		{
			m_Weapon.DeleteBullet(base.gameObject);
		}
		if (ShadowLight != null)
		{
			ShadowLight.active = false;
		}
	}

	private void OnCollisionEnter(Collision collisionInfo)
	{
		Debug.LogError("OnCollisionEnter - " + collisionInfo.gameObject.name);
	}

	private void OnTriggerEnter(Collider collider)
	{
		if ((isMassacreCannonSubBullet && collider.transform.root.gameObject.layer != 9 && collider.transform.root.gameObject.layer != 27) || (collider.transform.root.gameObject.layer == 27 && (collider.transform.root.gameObject == PlayerManager.Instance.GetPlayerObject() || (PlayerManager.Instance.GetRecipientByObj(collider.transform.root.gameObject) != null && OwnedPlayer == PlayerManager.Instance.GetRecipientByObj(collider.transform.root.gameObject)))) || collider.gameObject.name == "protect_cover")
		{
			return;
		}
		if (HitAndDestroy)
		{
			DestroyBullet();
		}
		else if (collider.transform.root.gameObject.layer != 9)
		{
			DestroyBullet();
		}
		if (WeaponType == WeaponType.Messiah)
		{
			Vector3 vector = base.transform.position + 1.5f * base.transform.up;
			m_Weapon.CreateBulletHitParticle(base.transform.position);
			float num = 4.5f;
			if (GameApp.GetInstance().GetGameState().m_eGameMode.m_ePlayMode == GameState.NetworkGameMode.PlayMode.E_DeathMatch || GameApp.GetInstance().GetGameState().m_eGameMode.m_ePlayMode == GameState.NetworkGameMode.PlayMode.E_LastStand)
			{
				AOEBullet_PVPModeTrigger(vector, collider);
			}
			else
			{
				List<Enemy> list = new List<Enemy>();
				Hashtable enemies = GameApp.GetInstance().GetGameScene().GetEnemies();
				foreach (Enemy value in enemies.Values)
				{
					if (value.HP <= 0f)
					{
						continue;
					}
					if (value.Name == collider.transform.root.gameObject.name)
					{
						DamageProperty damageProperty = new DamageProperty();
						damageProperty.damage = Damage;
						if (GameApp.GetInstance().GetGameState().m_eGameMode.m_ePlayMode == GameState.NetworkGameMode.PlayMode.E_PVE_BossRush)
						{
							if (m_Weapon == PlayerManager.Instance.GetPlayerClass().GetWeapon())
							{
								GameSetup.Instance.ReqHitNEnemy(value.enemyID, damageProperty.damage);
							}
						}
						else
						{
							value.OnHit(damageProperty, WeaponType);
						}
					}
					else if (Vector3.Distance(vector, new Vector3(value.GetTransform().position.x, vector.y, value.GetTransform().position.z)) <= num)
					{
						list.Add(value);
					}
				}
				float num2 = 10f;
				bool flag = false;
				foreach (Enemy item in list)
				{
					if (item.GetState() == Enemy.DEAD_STATE)
					{
						continue;
					}
					DamageProperty damageProperty2 = new DamageProperty();
					damageProperty2.damage = num2;
					if (GameApp.GetInstance().GetGameState().m_eGameMode.m_ePlayMode == GameState.NetworkGameMode.PlayMode.E_PVE_BossRush)
					{
						if (m_Weapon == PlayerManager.Instance.GetPlayerClass().GetWeapon())
						{
							GameSetup.Instance.ReqHitNEnemy(item.enemyID, damageProperty2.damage);
						}
					}
					else
					{
						item.OnHit(damageProperty2, WeaponType);
					}
					if (!flag)
					{
						flag = true;
						if (OwnedPlayer != null)
						{
							OwnedPlayer.CheckAttackBloodSuck();
						}
					}
				}
			}
			List<JerricanScript> jerricans = GameApp.GetInstance().GetGameScene().GetJerricans();
			if (jerricans != null && jerricans.Count > 0)
			{
				for (int i = 0; i < jerricans.Count; i++)
				{
					if (jerricans[i].gameObject != null && Vector3.Distance(vector, jerricans[i].transform.position) <= num)
					{
						jerricans[i].OnHit(Damage);
					}
				}
			}
			PathDoor[] pathDoors = GameApp.GetInstance().GetGameScene().GetPathDoors();
			for (int j = 0; j < pathDoors.Length; j++)
			{
				if (pathDoors[j] != null && pathDoors[j].gameObject != null && pathDoors[j].GetWorm() != null && pathDoors[j].gameObject != null && Vector3.Distance(vector, pathDoors[j].transform.position) <= num)
				{
					WormScript component = pathDoors[j].GetWorm().GetComponent<WormScript>();
					if (component != null)
					{
						component.OnHit(Damage);
					}
				}
			}
			return;
		}
		if (WeaponType == WeaponType.Tomahawk || WeaponType == WeaponType.BigFirework || WeaponType == WeaponType.Ion_Cannon || WeaponType == WeaponType.Ion_CannonI)
		{
			Vector3 vector2 = base.transform.position + 1.5f * base.transform.up;
			int splashThing = 0;
			if (WeaponType == WeaponType.Ion_Cannon)
			{
				splashThing = 25;
			}
			else
			{
				splashThing = 26;
			}
			GameObject gameObject = (WeaponType == WeaponType.Ion_Cannon || WeaponType == WeaponType.Ion_CannonI) ? Object.Instantiate(GameApp.GetInstance().GetGameConfig().weaponBulletHitParticles[splashThing], vector2, Quaternion.identity) as GameObject : Object.Instantiate(GameApp.GetInstance().GetGameConfig().rocketExlposion, vector2, Quaternion.identity) as GameObject;
			if (GameApp.GetInstance().GetGameState().SoundOn)
			{
				if (gameObject.GetComponent<AudioSource>() != null)
				{
					gameObject.GetComponent<AudioSource>().mute = false;
					gameObject.GetComponent<AudioSource>().Play();
				}
			}
			else if (gameObject.GetComponent<AudioSource>() != null)
			{
				gameObject.GetComponent<AudioSource>().mute = true;
			}
			float num3 = 4.5f;
			if (GameApp.GetInstance().GetGameState().m_eGameMode.m_ePlayMode == GameState.NetworkGameMode.PlayMode.E_DeathMatch || GameApp.GetInstance().GetGameState().m_eGameMode.m_ePlayMode == GameState.NetworkGameMode.PlayMode.E_LastStand)
			{
				AOEBullet_PVPModeTrigger(vector2, collider);
			}
			else
			{
				List<Enemy> list2 = new List<Enemy>();
				Hashtable enemies2 = GameApp.GetInstance().GetGameScene().GetEnemies();
				bool gotAnEnemy = false;
				foreach (Enemy value2 in enemies2.Values)
				{
					if (!(value2.HP <= 0f) && Vector3.Distance(vector2, new Vector3(value2.GetTransform().position.x, vector2.y, value2.GetTransform().position.z)) <= num3)
					{
						list2.Add(value2);
						gotAnEnemy = true;
					}
				}
				if (!gotAnEnemy && WeaponType == WeaponType.Ion_Cannon && collider.gameObject.name != "cannoni_shield_plus_keep_pfb")
				{
					GameObject shieldObject = Object.Instantiate(Resources.Load<GameObject>("zombie3d/effect/cannoni_shield_plus_keep_pfb"), vector2, base.transform.rotation);
					shieldObject.transform.rotation = OwnedPlayer.PlayerObject.transform.rotation;
					shieldObject.GetComponentInChildren<CannonShieldKeep>().m_Weapon = m_Weapon;
				}
				if (!gotAnEnemy && WeaponType == WeaponType.Ion_CannonI && collider.gameObject.name != "cannonii_shield_plus_keep_pfb")
				{
					GameObject shieldObject2 = Object.Instantiate(Resources.Load<GameObject>("zombie3d/effect/cannonii_shield_plus_keep_pfb"), vector2, base.transform.rotation);
					shieldObject2.transform.rotation = OwnedPlayer.PlayerObject.transform.rotation;
					shieldObject2.GetComponentInChildren<CannonShieldKeep>().m_Weapon = m_Weapon;
				}
				float thisAttack = Damage;
				if (OwnedPlayer != null && !isMassacreCannonSubBullet)
				{
					thisAttack = OwnedPlayer.GetThisAttack();
				}
				bool flag2 = false;
				bool flag3 = false;
				foreach (Enemy item2 in list2)
				{
					if (item2.GetState() == Enemy.DEAD_STATE)
					{
						continue;
					}
					DamageProperty damageProperty3 = new DamageProperty();
					damageProperty3.damage = thisAttack;
					damageProperty3.speedFactorTime = 0.4f;
					damageProperty3.speedFactor = -0.5f;
					if (GameApp.GetInstance().GetGameState().m_eGameMode.m_ePlayMode == GameState.NetworkGameMode.PlayMode.E_PVE_BossRush)
					{
						if (m_Weapon == PlayerManager.Instance.GetPlayerClass().GetWeapon())
						{
							GameSetup.Instance.ReqHitNEnemy(item2.enemyID, damageProperty3.damage);
						}
					}
					else
					{
						item2.OnHit(damageProperty3, WeaponType);
					}
					if (!flag3)
					{
						flag3 = true;
					}
					if (!flag2)
					{
						flag2 = true;
					}
				}
				if (OwnedPlayer != null && !isMassacreCannonSubBullet && flag2)
				{
					OwnedPlayer.CheckAttackBloodSuck();
				}
				if (this.gameObject.name.Contains("SubCannon"))
				{
					new WeaponBulletsPool().DeleteBullet(base.gameObject);
				}
			}
			List<JerricanScript> jerricans2 = GameApp.GetInstance().GetGameScene().GetJerricans();
			if (jerricans2 != null && jerricans2.Count > 0)
			{
				for (int k = 0; k < jerricans2.Count; k++)
				{
					if (jerricans2[k].gameObject != null && Vector3.Distance(vector2, jerricans2[k].transform.position) <= num3)
					{
						jerricans2[k].OnHit(Damage);
					}
				}
			}
			PathDoor[] pathDoors2 = GameApp.GetInstance().GetGameScene().GetPathDoors();
			for (int l = 0; l < pathDoors2.Length; l++)
			{
				if (pathDoors2[l] != null && pathDoors2[l].gameObject != null && pathDoors2[l].GetWorm() != null && pathDoors2[l].gameObject != null && Vector3.Distance(vector2, pathDoors2[l].transform.position) <= num3)
				{
					WormScript component2 = pathDoors2[l].GetWorm().GetComponent<WormScript>();
					if (component2 != null)
					{
						component2.OnHit(Damage);
					}
				}
			}
			return;
		}
		RaycastHit hitInfo = default(RaycastHit);
		bool flag4 = collider.Raycast(new Ray(base.transform.position, base.transform.up), out hitInfo, 10f);
		if (flag4)
		{
			m_Weapon.CreateBulletHitParticle(hitInfo.point);
		}
		else
		{
			m_Weapon.CreateBulletHitParticle(base.transform.position);
		}
		if (GameApp.GetInstance().GetGameState().m_eGameMode.m_ePlayMode == GameState.NetworkGameMode.PlayMode.E_DeathMatch || GameApp.GetInstance().GetGameState().m_eGameMode.m_ePlayMode == GameState.NetworkGameMode.PlayMode.E_LastStand)
		{
			NormalBullet_PVPModeTrigger(collider);
		}
		Transform root = collider.gameObject.transform.root;
		if (root.gameObject.name.StartsWith("E_") || root.gameObject.name.StartsWith("NEnemyID_"))
		{
			Enemy enemyByID = GameApp.GetInstance().GetGameScene().GetEnemyByID(root.gameObject.name);
			if (enemyByID != null && enemyByID.GetState() != Enemy.DEAD_STATE)
			{
				float thisAttack2 = Damage;
				if (OwnedPlayer != null && !isMassacreCannonSubBullet)
				{
					thisAttack2 = OwnedPlayer.GetThisAttack();
					OwnedPlayer.CheckAttackBloodSuck();
				}
				if (WeaponType == WeaponType.RemingtonPipe || WeaponType == WeaponType.ParkerGaussRifle || WeaponType == WeaponType.ZombieBusters || WeaponType == WeaponType.Tomahawk || WeaponType == WeaponType.Ion_Cannon || WeaponType == WeaponType.Ion_CannonI)
				{
					DamageProperty damageProperty4 = new DamageProperty();
					damageProperty4.damage = thisAttack2;
					damageProperty4.speedFactorTime = 0.4f;
					damageProperty4.speedFactor = -0.5f;
					if (GameApp.GetInstance().GetGameState().m_eGameMode.m_ePlayMode == GameState.NetworkGameMode.PlayMode.E_PVE_BossRush)
					{
						if (m_Weapon == PlayerManager.Instance.GetPlayerClass().GetWeapon())
						{
							GameSetup.Instance.ReqHitNEnemy(enemyByID.enemyID, damageProperty4.damage);
						}
					}
					else
					{
						enemyByID.OnHit(damageProperty4, WeaponType);
					}
				}
				else
				{
					DamageProperty damageProperty5 = new DamageProperty();
					damageProperty5.damage = thisAttack2;
					if (GameApp.GetInstance().GetGameState().m_eGameMode.m_ePlayMode == GameState.NetworkGameMode.PlayMode.E_PVE_BossRush)
					{
						if (m_Weapon == PlayerManager.Instance.GetPlayerClass().GetWeapon())
						{
							GameSetup.Instance.ReqHitNEnemy(enemyByID.enemyID, damageProperty5.damage);
						}
					}
					else
					{
						enemyByID.OnHit(damageProperty5, WeaponType);
					}
				}
			}
		}
		if (WeaponType != WeaponType.MassacreCannon || isMassacreCannonSubBullet)
		{
			return;
		}
		float y = base.transform.localEulerAngles.y;
		Vector3 pos = base.transform.position;
		if (flag4)
		{
			pos = hitInfo.point;
		}
		if (m_Weapon == null)
		{
			return;
		}
		for (int m = 0; m < 6; m++)
		{
			GameObject gameObject2 = m_Weapon.CreateBullet(pos, Quaternion.Euler(270f, 180f, 0f));
			if (gameObject2 != null)
			{
				gameObject2.transform.Rotate(Vector3.forward, y + 60f * (float)m + Random.Range(-30f, 30f));
				gameObject2.transform.Translate(Vector3.up * 2f);
				float num4 = 1f;
				gameObject2.transform.localScale = new Vector3(0.08030832f * num4, 0.1348395f * num4, 0.4631814f * num4);
				WeaponBulletScript component3 = gameObject2.GetComponent<WeaponBulletScript>();
				component3.m_Weapon = m_Weapon;
				component3.Damage = damage * 0.17f;
				component3.Speed = 12f;
				component3.Rot = y + (60f * (float)m + (float)Random.Range(0, 30));
				component3.flashFrameTime = 0.2f;
				component3.isMassacreCannonSubBullet = true;
				component3.Init();
				ParticleEmitter particleEmitter = component3.BulletHitPrefab.transform.GetChild(0).gameObject.GetComponent<ParticleEmitter>();
				particleEmitter.minSize = 2f;
				particleEmitter.maxSize = 2f;
			}
		}
	}

	private void AOEBullet_PVPModeTrigger(Vector3 hitPos, Collider collider)
	{
		if (OwnedPlayer != GameApp.GetInstance().GetGameScene().GetPlayer())
		{
			return;
		}
		float num = 4.5f;
		List<Player> list = new List<Player>();
		List<Player> recipientPlayerList = PlayerManager.Instance.GetRecipientPlayerList();
		foreach (Player item in recipientPlayerList)
		{
			if (item != null && !(item.HP <= 0f) && (GameApp.GetInstance().GetGameState().m_eGameMode.m_eCooperaMode != 0 || item.m_iNGroupID != PlayerManager.Instance.GetPlayerClass().m_iNGroupID))
			{
				if (item.PlayerObject == collider.transform.root.gameObject)
				{
					DamageProperty damageProperty = new DamageProperty();
					damageProperty.damage = Damage;
					item.NPlayerOnHitted(damageProperty.damage);
				}
				else if (Vector3.Distance(hitPos, new Vector3(item.PlayerObject.transform.position.x, hitPos.y, item.PlayerObject.transform.position.z)) <= num)
				{
					list.Add(item);
				}
			}
		}
		float num2 = 10f;
		if (OwnedPlayer != null && !isMassacreCannonSubBullet)
		{
			num2 = OwnedPlayer.GetThisAttack();
		}
		bool flag = false;
		bool flag2 = false;
		foreach (Player item2 in list)
		{
			if (item2.HP > 0f)
			{
				DamageProperty damageProperty2 = new DamageProperty();
				damageProperty2.damage = num2;
				damageProperty2.speedFactorTime = 0.4f;
				damageProperty2.speedFactor = -0.5f;
				item2.NPlayerOnHitted(damageProperty2.damage);
				if (!flag2)
				{
					flag2 = true;
				}
				if (!flag)
				{
					flag = true;
				}
			}
		}
		if (OwnedPlayer != null && !isMassacreCannonSubBullet && flag)
		{
			OwnedPlayer.CheckAttackBloodSuck();
		}
	}

	private void NormalBullet_PVPModeTrigger(Collider collider)
	{
		Transform root = collider.gameObject.transform.root;
		if (root.gameObject.layer != 27)
		{
			return;
		}
		Player recipientByObj = PlayerManager.Instance.GetRecipientByObj(root.gameObject);
		if (recipientByObj != null && OwnedPlayer == GameApp.GetInstance().GetGameScene().GetPlayer() && (GameApp.GetInstance().GetGameState().m_eGameMode.m_eCooperaMode != 0 || recipientByObj.m_iNGroupID != PlayerManager.Instance.GetPlayerClass().m_iNGroupID) && recipientByObj.HP > 0f)
		{
			float thisAttack = Damage;
			if (OwnedPlayer != null && !isMassacreCannonSubBullet)
			{
				thisAttack = OwnedPlayer.GetThisAttack();
				OwnedPlayer.CheckAttackBloodSuck();
			}
			if (WeaponType == WeaponType.RemingtonPipe || WeaponType == WeaponType.ParkerGaussRifle || WeaponType == WeaponType.ZombieBusters || WeaponType == WeaponType.Tomahawk)
			{
				DamageProperty damageProperty = new DamageProperty();
				damageProperty.damage = thisAttack;
				damageProperty.speedFactorTime = 0.4f;
				damageProperty.speedFactor = -0.5f;
				recipientByObj.NPlayerOnHitted(damageProperty.damage);
			}
			else
			{
				DamageProperty damageProperty2 = new DamageProperty();
				damageProperty2.damage = thisAttack;
				recipientByObj.NPlayerOnHitted(damageProperty2.damage);
			}
		}
	}
}
