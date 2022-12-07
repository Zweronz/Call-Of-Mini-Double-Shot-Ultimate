using UnityEngine;

namespace Zombie3D
{
	public abstract class Weapon
	{
		protected GameObject hitParticles;

		protected GameObject projectile;

		protected Camera cameraComponent;

		protected Transform cameraTransform;

		protected BaseCameraScript gameCamera;

		protected GameObject gunfire;

		protected WeaponBulletsPool m_WeaponBulletPool;

		protected WeaponBulletsShellPool m_WeaponBulletShellsPool;

		protected WeaponBulletsHitParticlePool m_WeaponBulletHitParticlesPool;

		protected GameObject leftHandGun;

		public GameObject rightHandGun;

		protected Transform oneHandWeaponBoneTrans;

		protected Transform twoHandLeftWeaponBoneTrans;

		protected Transform twoHandRightWeaponBoneTrans;

		protected GameConfigScript gConf;

		protected GameParametersScript gParam;

		protected AudioPlayer audioPlayer;

		protected GameScene gameScene;

		protected Player player;

		protected Vector3 aimTarget;

		protected bool isCDing;

		protected float hitForce;

		protected float range;

		protected float lastShootTime;

		protected int maxCapacity;

		protected int maxGunLoad;

		protected int capacity;

		protected int bulletCount;

		protected float maxDeflection;

		protected Vector2 deflection;

		protected float damage;

		protected float attackFrenquency;

		protected float accuracy;

		protected float speedDrag;

		protected Vector3 lastHitPosition;

		protected int price;

		protected float TriggerTimer;

		protected float defaultTriggerTime;

		protected bool bBulletShellHitFloorAudioPlayed = true;

		protected float bulletShellHitFloorAudioCalcStartTime;

		protected float bulletShellHitFloorAudioTime = 1f;

		protected bool bReloaded = true;

		protected float reloadCalcStartTime;

		protected float reloadAudioTime = 5f;

		public int DamageLevel { get; set; }

		public int FrequencyLevel { get; set; }

		public int AccuracyLevel { get; set; }

		public bool Trigger { get; set; }

		public float TriggerTime { get; set; }

		public bool IsSelectedForBattle { get; set; }

		public bool Exist { get; set; }

		public string Info
		{
			get
			{
				return Name;
			}
		}

		public string Name { get; set; }

		public int Price
		{
			get
			{
				return price;
			}
		}

		public float Accuracy
		{
			get
			{
				return accuracy;
			}
			set
			{
				accuracy = value;
			}
		}

		public int MaxGunLoad
		{
			get
			{
				return maxGunLoad;
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

		public float AttackFrequency
		{
			get
			{
				return attackFrenquency;
			}
			set
			{
				attackFrenquency = value;
			}
		}

		public virtual int BulletCount
		{
			get
			{
				return 0;
			}
			set
			{
				bulletCount = value;
			}
		}

		public int Capacity
		{
			get
			{
				return capacity;
			}
		}

		public Vector2 Deflection
		{
			get
			{
				return deflection;
			}
		}

		public Weapon()
		{
			defaultTriggerTime = 0.1f;
			TriggerTime = defaultTriggerTime;
		}

		public abstract void Fire(float deltaTime);

		public virtual void StopFire()
		{
		}

		public abstract WeaponType GetWeaponType();

		public virtual void changeReticle()
		{
		}

		public virtual void DoLogic(float deltaTime)
		{
			if (Trigger)
			{
				TriggerTimer += deltaTime;
				if (TriggerTimer >= TriggerTime)
				{
					Fire(deltaTime);
					SetDefaultWeaponTriggerTime();
					Trigger = false;
					TriggerTimer = 0f;
					bBulletShellHitFloorAudioPlayed = false;
					bulletShellHitFloorAudioCalcStartTime = Time.time;
					bReloaded = false;
					reloadCalcStartTime = Time.time;
				}
			}
			if (!bBulletShellHitFloorAudioPlayed && Time.time - bulletShellHitFloorAudioCalcStartTime >= bulletShellHitFloorAudioTime)
			{
				bBulletShellHitFloorAudioPlayed = true;
				bulletShellHitFloorAudioCalcStartTime = Time.time;
			}
			if (!bReloaded && Time.time - reloadCalcStartTime >= reloadAudioTime)
			{
				bReloaded = true;
				reloadCalcStartTime = Time.time;
			}
			if (m_WeaponBulletHitParticlesPool != null)
			{
				m_WeaponBulletHitParticlesPool.DoLogic();
			}
			if (m_WeaponBulletShellsPool != null)
			{
				m_WeaponBulletShellsPool.DoLogic();
			}
		}

		public virtual void LoadConfig()
		{
			FixedConfig.WeaponCfg weaponCfg = ConfigManager.Instance().GetFixedConfig().GetWeaponCfg(GetWeaponType());
			damage = weaponCfg.dmg;
			attackFrenquency = weaponCfg.rpm;
			speedDrag = weaponCfg.spd;
			if (GameApp.GetInstance().GetGameState().m_eGameMode.m_ePlayMode != 0)
			{
				attackFrenquency *= 1.3f;
				float num = damage / attackFrenquency;
				float num2 = 0.5f;
				if (num < 700f)
				{
					damage = (700f - num) * num2 + num;
				}
				else
				{
					damage = num - (num - 700f) * num2;
				}
			}
			defaultTriggerTime = attackFrenquency / 3f;
			if (defaultTriggerTime > 0.3f)
			{
				defaultTriggerTime = 0.3f;
			}
			else if (defaultTriggerTime < 0.1f)
			{
				defaultTriggerTime = 0.03f;
			}
			TriggerTime = defaultTriggerTime;
		}

		public float GetSpeedDrag()
		{
			return speedDrag;
		}

		public void Upgrade(float power, float frequency, float accur)
		{
			if (power != 0f)
			{
				damage += power;
				int num = (int)(damage * 100f);
				damage = (float)((double)num * 1.0) / 100f;
				DamageLevel++;
			}
			if (frequency != 0f)
			{
				attackFrenquency -= frequency;
				int num = (int)(attackFrenquency * 100f);
				attackFrenquency = (float)((double)num * 1.0) / 100f;
				FrequencyLevel++;
			}
			if (accur != 0f)
			{
				accuracy += accur;
				int num = (int)(accuracy * 100f);
				accuracy = (float)((double)num * 1.0) / 100f;
				AccuracyLevel++;
			}
		}

		public float GetNextLevelDamage()
		{
			float num = 1f;
			int num2 = (int)(num * 100f);
			return (float)((double)num2 * 1.0) / 100f;
		}

		public int GetDamageUpgradePrice()
		{
			int damageLevel = DamageLevel;
			float num = 10f;
			float num2 = 1f;
			float num3 = num;
			for (int i = 0; i < damageLevel; i++)
			{
				num3 *= 1f + num2;
			}
			return (int)num3 / 100 * 100;
		}

		public int GetFrequencyUpgradePrice()
		{
			int frequencyLevel = FrequencyLevel;
			float num = 10f;
			float num2 = 1f;
			float num3 = num;
			for (int i = 0; i < frequencyLevel; i++)
			{
				num3 *= 1f + num2;
			}
			return (int)num3 / 100 * 100;
		}

		public int GetAccuracyUpgradePrice()
		{
			int accuracyLevel = AccuracyLevel;
			float num = 10f;
			float num2 = 1f;
			float num3 = num;
			for (int i = 0; i < accuracyLevel; i++)
			{
				num3 *= 1f + num2;
			}
			return (int)num3 / 100 * 100;
		}

		public float GetNextLevelFrequency()
		{
			float num = 1f;
			int num2 = (int)(num * 100f);
			return (float)((double)num2 * 1.0) / 100f;
		}

		public float GetNextLevelAccuracy()
		{
			float num = 1f;
			int num2 = (int)(num * 100f);
			return (float)((double)num2 * 1.0) / 100f;
		}

		public float GetLastShootTime()
		{
			return lastShootTime;
		}

		public Player GetOwnedPlayer()
		{
			return player;
		}

		public void SetOwnedPlayer(Player newPlayer)
		{
			player = newPlayer;
		}

		public virtual void Init(Player owner)
		{
			gameScene = GameApp.GetInstance().GetGameScene();
			gConf = GameApp.GetInstance().GetGameConfig();
			gParam = GameApp.GetInstance().GetGameScene().GetGameParameters();
			gameCamera = gameScene.GetCamera();
			cameraComponent = gameCamera.GetComponent<Camera>();
			cameraTransform = gameCamera.CameraTransform;
			player = owner;
			LoadConfig();
			aimTarget = default(Vector3);
			hitParticles = gConf.hitParticles01;
			projectile = gConf.projectile;
			hitForce = 0f;
			oneHandWeaponBoneTrans = player.GetTransform().Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Neck/Bip01 R Clavicle/Bip01 R UpperArm/Bip01 R Forearm/Bip01 R Hand/Weapon_Dummy/OneHandWeaponRoot");
			twoHandLeftWeaponBoneTrans = player.GetTransform().Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Neck/Bip01 L Clavicle/Bip01 L UpperArm/Bip01 L Forearm/Bip01 L Hand/Weapon_Bone_L/TwoHandLeftWeaponRoot");
			twoHandRightWeaponBoneTrans = player.GetTransform().Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Neck/Bip01 R Clavicle/Bip01 R UpperArm/Bip01 R Forearm/Bip01 R Hand/Weapon_Bone_R/TwoHandRightWeaponRoot");
			CreateGun();
			audioPlayer = new AudioPlayer();
			Transform transform = rightHandGun.transform.Find("Audio");
			if (transform != null)
			{
				if (transform.Find("ShootAudio") != null)
				{
					transform.Find("ShootAudio").gameObject.GetComponent<AudioSource>().playOnAwake = false;
					audioPlayer.AddAudio(transform, "ShootAudio");
				}
				if (transform.Find("Reload") != null)
				{
					transform.Find("Reload").gameObject.GetComponent<AudioSource>().playOnAwake = false;
					audioPlayer.AddAudio(transform, "Reload");
				}
			}
		}

		public abstract void CreateGun();

		public virtual void FireUpdate(float deltaTime)
		{
		}

		public virtual void AutoAim(float deltaTime)
		{
		}

		public virtual void BindGunAndFire()
		{
			rightHandGun.transform.localRotation = player.GetTransform().rotation;
			rightHandGun.transform.parent = oneHandWeaponBoneTrans;
			rightHandGun.transform.localPosition = Vector3.zero;
			rightHandGun.transform.localRotation = Quaternion.identity;
		}

		public void DeleteBullet(GameObject bulletObj)
		{
			if (m_WeaponBulletPool != null)
			{
				m_WeaponBulletPool.DeleteBullet(bulletObj);
			}
		}

		public void DeleteBulletHitParticle(GameObject bulletHitParticleObj)
		{
			if (m_WeaponBulletHitParticlesPool != null)
			{
				m_WeaponBulletHitParticlesPool.DeleteBullet(bulletHitParticleObj);
			}
		}

		public virtual void GetBullet()
		{
			capacity += maxGunLoad;
			capacity = Mathf.Clamp(capacity, 0, maxCapacity);
		}

		public void AddBullets(int num)
		{
			BulletCount += num;
		}

		public virtual void MaxBullet()
		{
			BulletCount = maxGunLoad;
		}

		public virtual bool HaveBullets()
		{
			return true;
		}

		public void PullTrigger()
		{
			if (!Trigger)
			{
				Trigger = true;
				TriggerTimer = 0f;
			}
		}

		public virtual bool CouldMakeNextShoot()
		{
			float num = attackFrenquency;
			if (player.WeaponNameEnd == "_Two")
			{
				num = 2f * attackFrenquency;
			}
			float num2 = attackFrenquency * (1f - player.AttackSpeedAdditive);
			if (!Trigger && (Time.time - lastShootTime >= num2 || Mathf.Abs(Time.time - lastShootTime - num2) <= 0.01f))
			{
				return true;
			}
			return false;
		}

		public virtual bool CouldPlayNextShootAnim()
		{
			bool result = false;
			if (player.WeaponNameEnd == "_Two")
			{
				if (Time.time - lastShootTime >= 2f * attackFrenquency)
				{
					result = true;
				}
			}
			else
			{
				result = true;
			}
			return result;
		}

		public virtual void GunOn()
		{
			BindGunAndFire();
			ShowGunFire(false);
		}

		public virtual void GunOff()
		{
			Object.Destroy(leftHandGun);
			Object.Destroy(rightHandGun);
			if (m_WeaponBulletPool != null)
			{
				m_WeaponBulletPool.DestroyPool();
			}
			if (m_WeaponBulletHitParticlesPool != null)
			{
				m_WeaponBulletHitParticlesPool.DestroyPool();
			}
			if (m_WeaponBulletShellsPool != null)
			{
				m_WeaponBulletShellsPool.DestroyPool();
			}
			StopFire();
		}

		public virtual void ShowGunFire(bool bShow)
		{
		}

		public virtual void SetDefaultWeaponTriggerTime()
		{
			TriggerTime = defaultTriggerTime;
		}

		public void SetWeaponTriggerTime(float trigger_time)
		{
			TriggerTime = trigger_time;
		}

		public virtual GameObject CreateBullet(Vector3 pos, Quaternion rotation)
		{
			return m_WeaponBulletPool.CreateBullet(pos, rotation);
		}

		public virtual GameObject CreateSubBullet(Vector3 pos, Quaternion rotation, GameObject obj)
		{
			return m_WeaponBulletPool.CreateSubBullet(pos, rotation, obj);
		}

		public virtual GameObject CreateBulletHitParticle(Vector3 pos)
		{
			if (m_WeaponBulletHitParticlesPool != null)
			{
				return m_WeaponBulletHitParticlesPool.CreateBulletHitParticle(pos);
			}
			return null;
		}

		public void CreateBulletShell(bool bRightHandBulletShell)
		{
			if (m_WeaponBulletShellsPool != null && GameApp.GetInstance().GetGameState().m_eGameMode.m_ePlayMode == GameState.NetworkGameMode.PlayMode.E_Console)
			{
				float y = player.GetTransform().localEulerAngles.y;
				y += Random.Range(-1f, 1f) * 30f;
				float num = 2f * Random.Range(0.8f, 1.4f);
				float y2 = 5f * Random.Range(0.8f, 1.4f);
				if (bRightHandBulletShell)
				{
					GameObject gameObject = m_WeaponBulletShellsPool.CreateBulletShell(rightHandGun.transform.position);
					gameObject.transform.rotation = Quaternion.identity;
					gameObject.transform.Rotate(Vector3.up, y);
					Vector3 direction = new Vector3(num, y2, 0f);
					Vector3 force = gameObject.transform.TransformDirection(direction);
					gameObject.GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
				}
				else
				{
					GameObject gameObject2 = m_WeaponBulletShellsPool.CreateBulletShell(leftHandGun.transform.position);
					gameObject2.transform.rotation = Quaternion.identity;
					gameObject2.transform.Rotate(Vector3.up, y);
					Vector3 direction2 = new Vector3(0f - num, y2, 0f);
					Vector3 force2 = gameObject2.transform.TransformDirection(direction2);
					gameObject2.GetComponent<Rigidbody>().AddForce(force2, ForceMode.Impulse);
				}
			}
		}

		public void Clear()
		{
			if (m_WeaponBulletHitParticlesPool != null)
			{
				m_WeaponBulletHitParticlesPool.DestroyPool();
			}
			if (m_WeaponBulletPool != null)
			{
				m_WeaponBulletPool.DestroyPool();
			}
			if (m_WeaponBulletShellsPool != null)
			{
				m_WeaponBulletShellsPool.DestroyPool();
			}
		}

		public void PlayBulletShellHitFloorAudio()
		{
			if (GameApp.GetInstance().GetGameState().SoundOn)
			{
				int num = Random.Range(0, 10);
				if (num < 5)
				{
					gConf.BulletShellHitFloorAudio01.Play();
				}
				else
				{
					gConf.BulletShellHitFloorAudio02.Play();
				}
			}
		}

		public void PlayReloadAudio()
		{
			audioPlayer.PlaySound("Reload");
		}

		public static GameObject GetWeaponPrefab(WeaponType wType)
		{
			string path = string.Empty;
			switch (wType)
			{
			case WeaponType.Beretta_33:
				path = "Zombie3D/Weapons/Beretta_33";
				break;
			case WeaponType.GrewCar_15:
				path = "Zombie3D/Weapons/GrewCar_15";
				break;
			case WeaponType.UZI_E:
				path = "Zombie3D/Weapons/UZI_E";
				break;
			case WeaponType.RemingtonPipe:
				path = "Zombie3D/Weapons/RemingtonPipe";
				break;
			case WeaponType.Springfield_9mm:
				path = "Zombie3D/Weapons/Springfield_9mm";
				break;
			case WeaponType.Kalashnikov_II:
				path = "Zombie3D/Weapons/Kalashnikov_II";
				break;
			case WeaponType.Barrett_P90:
				path = "Zombie3D/Weapons/Barrett_P90";
				break;
			case WeaponType.ParkerGaussRifle:
				path = "Zombie3D/Weapons/ParkerGaussRifle";
				break;
			case WeaponType.ZombieBusters:
				path = "Zombie3D/Weapons/ZombieBusters";
				break;
			case WeaponType.SimonovPistol:
				path = "Zombie3D/Weapons/SimonovPistol";
				break;
			case WeaponType.BarrettSplitIII:
				path = "Zombie3D/Weapons/BarrettSplitIII";
				break;
			case WeaponType.Tomahawk:
				path = "Zombie3D/Weapons/Tomahawk";
				break;
			case WeaponType.SimonoRayRifle:
				path = "Zombie3D/Weapons/SimonoRayRifle";
				break;
			case WeaponType.Volcano:
				path = "Zombie3D/Weapons/Volcano";
				break;
			case WeaponType.Hellfire:
				path = "Zombie3D/Weapons/Hellfire";
				break;
			case WeaponType.Nailer:
				path = "Zombie3D/Weapons/Nailer";
				break;
			case WeaponType.NeutronRifle:
				path = "Zombie3D/Weapons/NeutronRifle";
				break;
			case WeaponType.BigFirework:
				path = "Zombie3D/Weapons/BigFirework";
				break;
			case WeaponType.Stormgun:
				path = "Zombie3D/Weapons/Stormgun";
				break;
			case WeaponType.Lightning:
				path = "Zombie3D/Weapons/Lightning";
				break;
			case WeaponType.MassacreCannon:
				path = "Zombie3D/Weapons/MassacreCannon";
				break;
			case WeaponType.DoubleSnake:
				path = "Zombie3D/Weapons/DoubleSnake";
				break;
			case WeaponType.Longinus:
				path = "Zombie3D/Weapons/Longinus";
				break;
			case WeaponType.CrossBow:
				path = "Zombie3D/Weapons/CrossBow";
				break;
			case WeaponType.Messiah:
				path = "Zombie3D/Weapons/Messiah";
				break;
			case WeaponType.Ion_Cannon:
				path = "Zombie3D/Weapons/cannoni";
				break;
			case WeaponType.Ion_CannonI:
				path = "Zombie3D/Weapons/cannonii";
				break;
			}
			return Resources.Load(path, typeof(GameObject)) as GameObject;
		}
	}
}
