using System.Collections.Generic;
using UnityEngine;

namespace Zombie3D
{
	public abstract class Enemy
	{
		public static bool m_bShowDebugInfo = false;

		public static EnemyState GRAVEBORN_STATE = new GraveBornState();

		public static EnemyState IDLE_STATE = new IdleState();

		public static EnemyState CATCHING_STATE = new CatchingState();

		public static EnemyState GOTHIT_STATE = new GotHitState();

		public static EnemyState PATROL_STATE = new PatrolState();

		public static EnemyState ATTACK_STATE = new AttackState();

		public static EnemyState DEAD_STATE = new DeadState();

		public static EnemyState FORCEIDLE_STATE = new ForceIdleState();

		public GameObject enemyObject;

		public int enemyID = -1;

		protected Transform enemyTransform;

		protected Animation animation;

		protected Rigidbody rigidbody;

		protected Transform aimedTransform;

		protected Transform target;

		protected Vector3 spawnCenter;

		protected Vector3 patrolTarget;

		protected EnemySpawnScript spawn;

		protected SceneUIScript sceneGUI;

		protected Collider collider;

		protected GameParametersScript gParam;

		protected GameConfigScript gConfig;

		protected EnemyType enemyType;

		protected Vector3 lastTarget;

		protected GameScene gameScene;

		protected Player player;

		public Player FriendPlayer;

		protected CharacterController controller;

		protected Vector3 dir;

		protected AudioPlayer audio;

		protected List<Vector4> pathPoints;

		protected BloodRect bloodRect;

		protected float hp;

		protected float maxHp;

		protected float runSpeed;

		protected float runSpeedFactor;

		protected float runSpeedFactorTime = -1f;

		protected float runSpeedFactorTimer = -1f;

		protected bool beWokeUp;

		protected float deadTime;

		public float lootExp;

		protected int lootCash;

		protected string name;

		protected bool visible;

		protected ObjectPool hitBloodObjectPool;

		protected bool moveWithCharacterController;

		protected float lastUpdateTime;

		protected float lastPathFindingTime;

		protected float lastSearchPathTime;

		protected EnemyState state;

		public float attackRange;

		public float attackRangeNear;

		public float detectionRange;

		protected float minRange;

		protected float attackFrequency;

		protected float attackDamage;

		protected float idlePeriod = 1.5f;

		protected float aiRadius = 100f;

		protected float gotHitTime;

		protected float idleStartTime;

		public float lastAttackTime = -100f;

		protected float lookAroundStartTime;

		protected int nextPoint = -1;

		protected string runAnimationName = "Run01";

		protected GameObject targetObj;

		protected float onhitRate = 100f;

		public bool criticalAttacked;

		protected Vector3[] path;

		public Ray ray;

		public RaycastHit rayhit;

		public float lastStateTime;

		private string lastAnimationName = string.Empty;

		public float AttackDamage
		{
			get
			{
				return attackDamage;
			}
		}

		public AudioPlayer Audio
		{
			get
			{
				return audio;
			}
		}

		public string RunAnimationName
		{
			get
			{
				return runAnimationName;
			}
		}

		public bool MoveWithCharacterController
		{
			get
			{
				return moveWithCharacterController;
			}
			set
			{
				moveWithCharacterController = value;
			}
		}

		public string Name
		{
			get
			{
				return name;
			}
			set
			{
				name = value;
			}
		}

		public float HP
		{
			get
			{
				return hp;
			}
			set
			{
				hp = value;
			}
		}

		public float MAXHP
		{
			get
			{
				return maxHp;
			}
			set
			{
				maxHp = value;
			}
		}

		public float DetectionRange
		{
			get
			{
				return detectionRange;
			}
		}

		public float AttackRange
		{
			get
			{
				return attackRange;
			}
		}

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

		public EnemySpawnScript Spawn
		{
			set
			{
				spawn = value;
			}
		}

		public Vector3 LastTarget
		{
			get
			{
				return lastTarget;
			}
		}

		public float SqrDistanceFromPlayer
		{
			get
			{
				return (target.position - enemyTransform.position).sqrMagnitude;
			}
		}

		public Transform GetTransform()
		{
			return enemyTransform;
		}

		public bool CouldMakeNextAttack()
		{
			if (Time.time - lastAttackTime >= attackFrequency)
			{
				return true;
			}
			return false;
		}

		public virtual bool CouldEnterAttackState()
		{
			if (SqrDistanceFromPlayer < AttackRange * AttackRange)
			{
				return true;
			}
			return false;
		}

		public bool IsPlayingAnimation(string name)
		{
			return animation.IsPlaying(name);
		}

		public bool IsAnimationPlayedPercentage(string aniName, float percentage)
		{
			if (animation[aniName].time >= animation[aniName].clip.length * percentage)
			{
				return true;
			}
			return false;
		}

		public virtual bool AttackAnimationEnds()
		{
			if (Time.time - lastAttackTime > enemyObject.GetComponent<Animation>()["Attack01"].length)
			{
				return true;
			}
			return false;
		}

		public bool GotHitAnimationEnds()
		{
			if (Time.time - gotHitTime >= animation["Damage01"].clip.length)
			{
				return true;
			}
			return false;
		}

		public void Animate(string animationName, WrapMode wrapMode, bool bNeedReq = true)
		{
			if (GameApp.GetInstance().GetGameState().m_eGameMode.m_ePlayMode == GameState.NetworkGameMode.PlayMode.E_PVE_BossRush && animationName != lastAnimationName && bNeedReq)
			{
				GameSetup.Instance.ReqSyncEnemyAnimation(enemyID, animationName, wrapMode);
			}
			animation[animationName].wrapMode = wrapMode;
			if (!animation.IsPlaying("Damage01"))
			{
				if (wrapMode == WrapMode.Loop || (!animation.IsPlaying(animationName) && animationName != "Damage01"))
				{
					animation.CrossFade(animationName);
					return;
				}
				animation.Stop();
				animation.CrossFade(animationName, 0.1f);
			}
		}

		public void SetInGrave(bool inGrave)
		{
			if (inGrave)
			{
				SetState(GRAVEBORN_STATE);
				enemyTransform.Translate(Vector3.down * 2f);
				enemyObject.layer = 0;
				enemyTransform.GetComponent<Rigidbody>().useGravity = false;
			}
			else
			{
				enemyObject.layer = 9;
				enemyTransform.GetComponent<Rigidbody>().useGravity = true;
			}
		}

		public bool MoveFromGrave(float deltaTime)
		{
			enemyTransform.Translate(Vector3.up * deltaTime * 2f);
			if (enemyTransform.position.y >= 10000.1f)
			{
				return true;
			}
			return false;
		}

		public virtual void Init(GameObject gObject)
		{
			gameScene = GameApp.GetInstance().GetGameScene();
			player = gameScene.GetPlayer();
			FriendPlayer = gameScene.GetFriendPlayer();
			enemyObject = gObject;
			enemyTransform = enemyObject.transform;
			animation = enemyObject.GetComponent<Animation>();
			aimedTransform = enemyTransform.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Neck/Bip01 Head");
			rigidbody = enemyObject.GetComponent<Rigidbody>();
			collider = enemyObject.transform.GetComponent<Collider>();
			pathPoints = new List<Vector4>(40);
			Transform transform = enemyTransform.Find("BloodRect");
			if (transform != null)
			{
				GameObject gameObject = transform.gameObject;
				if (gameObject != null)
				{
					bloodRect = gameObject.GetComponent(typeof(BloodRect)) as BloodRect;
					if (bloodRect != null)
					{
						bloodRect.SetBloodPercent(1f);
					}
				}
			}
			gConfig = GameApp.GetInstance().GetGameConfig();
			gParam = GameApp.GetInstance().GetGameScene().GetGameParameters();
			controller = enemyObject.GetComponent<Collider>() as CharacterController;
			detectionRange = 150f;
			criticalAttacked = false;
			spawnCenter = enemyTransform.position;
			target = GameApp.GetInstance().GetGameScene().GetPlayer()
				.EnemyTarget;
			audio = new AudioPlayer();
			if (enemyType != EnemyType.E_LAVA || enemyType != EnemyType.E_INFECTER || enemyType != EnemyType.E_LASER || enemyType != EnemyType.E_BATCHER || enemyType != EnemyType.E_TRACKER || enemyType != EnemyType.E_TURRETER)
			{
				if (TimerManager.GetInstance().Ready(93) && GameApp.GetInstance().GetGameState().SoundOn)
				{
					string text = "Zombie3D/Audio/RealPersonSound/CorpseAppear/CorpseAppear_";
					text += Random.Range(0, 3);
					AudioManager.PlayMusicOnce(text, enemyTransform);
					TimerManager.GetInstance().Do(93);
				}
			}
			else if (TimerManager.GetInstance().Ready(94) && GameApp.GetInstance().GetGameState().SoundOn)
			{
				string strPath = "Zombie3D/Audio/RealPersonSound/BigCorpseAppear/BigCorpseAppear_0";
				AudioManager.PlayMusicOnce(strPath, enemyTransform);
				TimerManager.GetInstance().Do(94);
			}
			hitBloodObjectPool = GameApp.GetInstance().GetGameScene().HitBloodObjectPool;
			animation.wrapMode = WrapMode.Loop;
			animation.Play("Idle01");
			state = IDLE_STATE;
			lastUpdateTime = Time.time;
			lastPathFindingTime = Time.time;
			idleStartTime = -2f;
			SetBaseConfig();
			path = GameApp.GetInstance().GetGameScene().GetPath();
		}

		public virtual void UpdateTarget(Transform targetTrans)
		{
			target = targetTrans;
		}

		public virtual void UpdateTarget(GameObject targetGO)
		{
			targetObj = targetGO;
		}

		public virtual void UpdateTarget(GameObject targetGO, Transform targetTrans)
		{
			targetObj = targetGO;
			target = targetTrans;
		}

		public virtual void SetBaseConfig()
		{
			FixedConfig.EnemyCfg enemyCfg = ConfigManager.Instance().GetFixedConfig().GetEnemyCfg((int)EnemyType);
			hp = enemyCfg.hp;
			attackDamage = enemyCfg.attack;
			attackRange = enemyCfg.attackRange;
			attackFrequency = enemyCfg.attackFrequency;
			runSpeed = enemyCfg.walkSpeed;
			lootCash = enemyCfg.lootCash;
			lootExp = enemyCfg.lootExp;
			int num = Random.Range(-2, 2);
			attackFrequency = enemyCfg.attackFrequency * (1f + (float)num / 10f);
			if (GameApp.GetInstance().GetGameState().m_eGameMode.m_ePlayMode == GameState.NetworkGameMode.PlayMode.E_PVE_BossRush)
			{
				maxHp = hp;
				return;
			}
			int num2 = 1;
			if (gameScene.DDSTrigger != null)
			{
				num2 = gameScene.DDSTrigger.PointsIndex;
			}
			if (GameApp.GetInstance().GetGameState().m_bIsSurvivalMode)
			{
				int battleWaves = GameApp.GetInstance().GetGameState().m_BattleWaves;
				float num3 = 0f;
				WeaponType weaponType = player.GetWeapon().GetWeaponType();
				FixedConfig.WeaponCfg weaponCfg = ConfigManager.Instance().GetFixedConfig().GetWeaponCfg(weaponType);
				int mClass = weaponCfg.mClass;
				num3 += (float)mClass * 2f;
				num3 += 10f;
				Mathf.Clamp(num3, 10f, 10000f);
				hp *= 2.5f;
				lootCash *= 2;
				lootExp *= 2f;
				runSpeed *= 1.3f;
				hp *= 1f + 0.02f * num3;
				attackDamage *= 1f + 0.015f * num3;
				attackFrequency *= 1f - 0.006f * num3;
				runSpeed *= 1f + 0.006f * num3;
				hp += hp * 0.02f * (float)battleWaves;
				attackDamage += attackDamage * 0.015f * (float)battleWaves;
				attackFrequency *= 1f - 0.006f * (float)battleWaves;
				runSpeed += runSpeed * 0.006f * (float)battleWaves;
				lootCash += (int)((float)lootCash * 0.03f * 0.5f * (float)battleWaves);
				lootExp += (int)(lootExp * 0.03f * (float)battleWaves);
			}
			else
			{
				if (gameScene.DDSTrigger.MapIndex == 1 && gameScene.DDSTrigger.PointsIndex > 1)
				{
					hp *= 1f + (float)((gameScene.DDSTrigger.PointsIndex - 1) / 10) * 0.2f;
					runSpeed *= 1f + (float)((gameScene.DDSTrigger.PointsIndex - 1) / 10) * 0.1f;
					attackFrequency *= 1f + (float)((gameScene.DDSTrigger.PointsIndex - 1) / 10) * 0.05f;
					attackDamage *= 1f + (float)((gameScene.DDSTrigger.PointsIndex - 1) / 10) * 0.32f;
					hp += hp * 0.02f * (float)(num2 - 1);
					attackDamage += attackDamage * 0.015f * (float)(num2 - 1);
					attackFrequency *= 1f - 0.006f * (float)(num2 - 1);
					runSpeed += runSpeed * 0.006f * (float)(num2 - 1);
					lootCash += (int)((float)lootCash * 0.03f * (float)(num2 - 1));
					lootExp += (int)(lootExp * 0.03f * (float)(num2 - 1));
				}
				if (gameScene.DDSTrigger.MapIndex == 2 && gameScene.DDSTrigger.PointsIndex > 1)
				{
					float num4 = gameScene.DDSTrigger.PointsIndex;
					float num5 = (num4 - 1f) / 2f;
					num4 = 10f + num5;
					hp *= 1f + (num4 - 1f) / 10f * 0.2f;
					runSpeed *= 1f + (num4 - 1f) / 10f * 0.1f;
					attackFrequency *= 1f + (num4 - 1f) / 10f * 0.05f;
					attackDamage *= 1f + (num4 - 1f) / 10f * 0.32f;
					hp += hp * 0.02f * 0.8f * (num4 - 1f);
					attackDamage += attackDamage * 0.015f * 0.8f * (num4 - 1f);
					attackFrequency *= 1f - 0.0048f * (num4 - 1f);
					runSpeed += runSpeed * 0.006f * 0.8f * (num4 - 1f);
					lootCash += (int)((float)lootCash * 0.03f * 0.8f * (num4 - 1f));
					lootExp += (int)(lootExp * 0.03f * 0.8f * (num4 - 1f));
				}
				if (gameScene.DDSTrigger.MapIndex == 3 && gameScene.DDSTrigger.PointsIndex > 1)
				{
					int num6 = gameScene.DDSTrigger.PointsIndex;
					switch (num2)
					{
					case 1:
						num6 = 2;
						break;
					case 2:
						num6 = 5;
						break;
					}
					hp += hp * 0.8f * (float)(num6 - 1);
					attackDamage += attackDamage * 0.3f * (float)(num6 - 1);
					runSpeed += runSpeed * 0.15f * (float)(num6 - 1);
					lootCash += (int)((float)lootCash * 0.3f * (float)(num6 - 1));
					lootExp += (int)(lootExp * 0.3f * (float)(num6 - 1));
				}
				if (gameScene.DDSTrigger.MapIndex == 4 && gameScene.DDSTrigger.PointsIndex > 1)
				{
					int num7 = gameScene.DDSTrigger.PointsIndex;
					switch (num2)
					{
					case 1:
						num7 = 2;
						break;
					case 2:
						num7 = 5;
						break;
					}
					hp += hp * 0.8f * (float)(num7 - 1);
					attackDamage += attackDamage * 0.3f * (float)(num7 - 1);
					runSpeed += runSpeed * 0.15f * (float)(num7 - 1);
					lootCash += (int)((float)lootCash * 0.3f * (float)(num7 - 1));
					lootExp += (int)(lootExp * 0.3f * (float)(num7 - 1));
				}
				if (gameScene.DDSTrigger.MapIndex == 5 && gameScene.DDSTrigger.PointsIndex > 1)
				{
					int num8 = gameScene.DDSTrigger.PointsIndex;
					switch (num2)
					{
					case 1:
						num8 = 2;
						break;
					case 2:
						num8 = 5;
						break;
					}
					hp += hp * 0.8f * (float)(num8 - 1);
					attackDamage += attackDamage * 0.3f * (float)(num8 - 1);
					runSpeed += runSpeed * 0.15f * (float)(num8 - 1);
					lootCash += (int)((float)lootCash * 0.3f * (float)(num8 - 1));
					lootExp += (int)(lootExp * 0.3f * (float)(num8 - 1));
				}
				if (gameScene.DDSTrigger.MapIndex == 6 && gameScene.DDSTrigger.PointsIndex > 1)
				{
					float num9 = gameScene.DDSTrigger.PointsIndex;
					float num10 = (num9 - 1f) / 2f;
					num9 = 25f + num10;
					hp *= 1f + (num9 - 1f) / 10f * 0.2f;
					runSpeed *= 1f + (num9 - 1f) / 10f * 0.1f;
					attackFrequency *= 1f + (num9 - 1f) / 10f * 0.05f;
					attackDamage *= 1f + (num9 - 1f) / 10f * 0.32f;
					hp += hp * 0.02f * 0.5f * (num9 - 1f);
					attackDamage += attackDamage * 0.015f * 0.5f * (num9 - 1f);
					attackFrequency *= 1f - 0.003f * (num9 - 1f);
					runSpeed += runSpeed * 0.006f * 0.5f * (num9 - 1f);
					lootCash += (int)((float)lootCash * 0.03f * 0.5f * (num9 - 1f));
					lootExp += (int)(lootExp * 0.03f * 0.5f * (num9 - 1f));
				}

				if (gameScene.DDSTrigger.MapIndex == 7 && gameScene.DDSTrigger.PointsIndex > 1)
				{
					float num9 = gameScene.DDSTrigger.PointsIndex;
					float num10 = (num9 - 1f) / 2f;
					num9 = 25f + num10;
					hp *= 1f + (num9 - 1f) / 10f * 0.2f;
					runSpeed *= 1f + (num9 - 1f) / 10f * 0.1f;
					attackFrequency *= 1f + (num9 - 1f) / 10f * 0.05f;
					attackDamage *= 1f + (num9 - 1f) / 10f * 0.32f;
					hp += hp * 0.02f * 0.5f * (num9 - 1f);
					attackDamage += attackDamage * 0.015f * 0.5f * (num9 - 1f);
					attackFrequency *= 1f - 0.003f * (num9 - 1f);
					runSpeed += runSpeed * 0.006f * 0.5f * (num9 - 1f);
					lootCash += (int)((float)lootCash * 0.03f * 0.5f * (num9 - 1f));
					lootExp += (int)(lootExp * 0.03f * 0.5f * (num9 - 1f));
				}
			}
			maxHp = hp;
		}

		public void SetEnemyInfo(float _hp, float _attackDamage, float _attackRange, float _attackFrequency, float _runspeed, int _lootCash, float _lootExp)
		{
			hp = _hp;
			attackDamage = _attackDamage;
			attackRange = _attackRange;
			attackFrequency = _attackFrequency;
			runSpeed = _runspeed;
			lootCash = _lootCash;
			lootExp = _lootExp;
			maxHp = hp;
		}

		public void SetEnemyInfoByFloorLevel(int level, int maxEnemyCount, EnemyType type)
		{
			hp = (1f + (float)level * 0.2f) * hp / (float)maxEnemyCount;
			maxHp = hp;
		}

		public float GetBulletFlySpeedFac()
		{
			float num = 0f;
			if (GameApp.GetInstance().GetGameState().m_bIsSurvivalMode)
			{
				uint survivalModeBattledMapCount = GameApp.GetInstance().GetGameState().m_SurvivalModeBattledMapCount;
				if (survivalModeBattledMapCount > 1)
				{
					num += 0.2f;
					num += (float)survivalModeBattledMapCount * 0.01f;
				}
			}
			if (gameScene.DDSTrigger.MapIndex == 1 && gameScene.DDSTrigger.PointsIndex > 1)
			{
				num += (float)(gameScene.DDSTrigger.PointsIndex / 10) * 0.1f;
				num += 0.012f * (float)(gameScene.DDSTrigger.PointsIndex - 1);
			}
			if (gameScene.DDSTrigger.MapIndex == 2 && gameScene.DDSTrigger.PointsIndex > 1)
			{
				num += (float)(gameScene.DDSTrigger.PointsIndex / 10) * 0.1f;
				num += 0.012f * (float)(gameScene.DDSTrigger.PointsIndex - 1);
			}
			if (gameScene.DDSTrigger.MapIndex == 3 && gameScene.DDSTrigger.PointsIndex > 1)
			{
				num += 0.01f * (float)(gameScene.DDSTrigger.PointsIndex - 1);
			}
			if (gameScene.DDSTrigger.MapIndex == 4 && gameScene.DDSTrigger.PointsIndex > 1)
			{
				num += 0.01f * (float)(gameScene.DDSTrigger.PointsIndex - 1);
			}
			if (gameScene.DDSTrigger.MapIndex == 5 && gameScene.DDSTrigger.PointsIndex > 1)
			{
				num += 0.012f * (float)(gameScene.DDSTrigger.PointsIndex - 1);
			}
			if (gameScene.DDSTrigger.MapIndex == 6 && gameScene.DDSTrigger.PointsIndex > 1)
			{
				num += 0.012f * (float)(gameScene.DDSTrigger.PointsIndex - 1);
			}
			if (gameScene.DDSTrigger.MapIndex == 7 && gameScene.DDSTrigger.PointsIndex > 1)
			{
				num += 0.012f * (float)(gameScene.DDSTrigger.PointsIndex - 1);
			}
			return num;
		}

		public void SetState(EnemyState newState)
		{
			if (newState != state)
			{
			}
			state = newState;
		}

		public EnemyState GetState()
		{
			return state;
		}

		public virtual void CheckHit()
		{
		}

		public Transform GetAimedTransform()
		{
			return aimedTransform;
		}

		public Vector3 GetPosition()
		{
			return enemyTransform.position;
		}

		public Collider GetCollider()
		{
			return collider;
		}

		public void NEnemyOnHitted(int userID, float damage)
		{
			HP -= damage;
			GameSetup.Instance.ReqSyncEnemyHp(enemyID, hp, maxHp);
			if (hp <= 0f)
			{
				NEnemyManager.Instance.RemoveNEnemy(enemyID);
			}
		}

		public virtual void OnHit(DamageProperty dp, WeaponType weaponType)
		{
			if (state != GRAVEBORN_STATE)
			{
				beWokeUp = true;
				hp -= dp.damage;
				if (runSpeedFactorTimer < 0f && dp.speedFactorTime > 0f)
				{
					runSpeedFactorTime = dp.speedFactorTime;
					runSpeedFactorTimer = 0f;
					runSpeedFactor += dp.speedFactor;
				}
				Faceout3DTextPool.Instance().Create3DText(enemyObject.transform.position + Vector3.up * 1.5f, Quaternion.Euler(55f, 0f, 0f), "-" + Mathf.FloorToInt(dp.damage * Random.Range(0.75f, 1.1f)));
			}
		}

		public virtual void OnAttack()
		{
		}

		public virtual void PlayDeadEffects()
		{
			if ((bool)enemyObject)
			{
				PlayBloodEffect();
				Clear();
			}
		}

		public virtual void PlayBloodEffect()
		{
			if ((bool)enemyObject)
			{
				GameObject deadBlood = gConfig.deadBlood;
				Object.Instantiate(deadBlood, enemyTransform.position + new Vector3(0f, 0.5f, 0f), Quaternion.Euler(0f, 0f, 0f));
			}
		}

		public virtual void PlayBodyExlodeEffect()
		{
			if (!enemyObject)
			{
			}
		}

		public virtual void FindPath()
		{
			if (target == null || target.gameObject == null)
			{
				return;
			}
			Vector3 position = target.position;
			if (!(Time.time - lastPathFindingTime > 0.5f / runSpeed))
			{
				return;
			}
			lastPathFindingTime = Time.time;
			position.y = enemyTransform.position.y;
			if (lastTarget == Vector3.zero)
			{
				lastTarget = target.position;
			}
			ray = new Ray(enemyTransform.position + new Vector3(0f, 1f, 0f), position - (enemyTransform.position + new Vector3(0f, 0f, 0f)));
			if (target.gameObject.tag == "Player")
			{
				if (Physics.Raycast(ray, out rayhit, 100f, 134293760))
				{
					if (m_bShowDebugInfo)
					{
						Debug.Log(enemyObject.name + " - " + rayhit.collider.gameObject.transform.root.name + "|");
					}
					if (rayhit.collider.gameObject.tag == "Player")
					{
						lastTarget = position;
						pathPoints.Clear();
					}
					else if (pathPoints.Count <= 1)
					{
						Vector4[] array = Pathfinding.Instance().FindPath(enemyTransform.position, target.position);
						if (array != null)
						{
							pathPoints.Clear();
							for (int i = 0; i < array.Length; i++)
							{
								pathPoints.Add(array[i]);
							}
							Vector4 vector = pathPoints[0];
							lastTarget = new Vector3(vector.x, vector.y, vector.z);
						}
					}
					else
					{
						Vector4 vector2 = pathPoints[0];
						lastTarget = new Vector3(vector2.x, vector2.y, vector2.z);
					}
				}
				if ((enemyTransform.position - lastTarget).sqrMagnitude < 1f)
				{
					if (pathPoints.Count <= 0)
					{
						Vector4[] array2 = Pathfinding.Instance().FindPath(enemyTransform.position, target.position);
						if (array2 != null)
						{
							for (int j = 0; j < array2.Length; j++)
							{
								pathPoints.Add(array2[j]);
							}
							Vector4 vector3 = pathPoints[0];
							lastTarget = new Vector3(vector3.x, vector3.y, vector3.z);
						}
					}
					else
					{
						pathPoints.RemoveAt(0);
						if (pathPoints.Count > 0)
						{
							Vector4 vector4 = pathPoints[0];
							lastTarget = new Vector3(vector4.x, vector4.y, vector4.z);
						}
					}
				}
				enemyTransform.LookAt(new Vector3(lastTarget.x, enemyTransform.position.y, lastTarget.z));
				dir = (lastTarget - enemyTransform.position).normalized;
			}
			else
			{
				Debug.Log(target.gameObject.name);
			}
		}

		public virtual void Patrol(float deltaTime)
		{
		}

		public void RemoveDeadBodyTimer()
		{
			if (GameApp.GetInstance().GetGameState().m_eGameMode.m_ePlayMode == GameState.NetworkGameMode.PlayMode.E_Console && Time.time - deadTime > 2f)
			{
				Clear();
			}
		}

		public void PlayEnemyDeadEffect()
		{
			if (criticalAttacked)
			{
				PlayDeadEffects();
			}
			else
			{
				if ((bool)animation)
				{
					AnimationState animationState = animation["Death01"];
					if (animationState != null)
					{
						animation["Death01"].wrapMode = WrapMode.ClampForever;
						animation["Death01"].speed = 1f;
						animation.CrossFade("Death01");
					}
					else
					{
						Debug.Log(string.Concat(enemyObject.name, " | ", EnemyType, " Donnot have Death1 animation"));
					}
				}
				if ((bool)enemyObject)
				{
					enemyObject.layer = 18;
				}
				PlayBloodEffect();
			}
			if (bloodRect != null)
			{
				bloodRect.gameObject.SetActiveRecursively(false);
			}
			if (TimerManager.GetInstance().Ready(92) && GameApp.GetInstance().GetGameState().SoundOn)
			{
				string text = "Zombie3D/Audio/RealPersonSound/KillCorpse/KillCorpse_";
				text += Random.Range(0, 6);
				AudioManager.PlayMusicOnce(text, enemyTransform);
				TimerManager.GetInstance().Do(92);
			}
		}

		public virtual void OnDead()
		{
			deadTime = Time.time;
			GameApp.GetInstance().GetGameState().IncreaseBattleGold(lootCash);
			GameApp.GetInstance().GetGameState().IncreaseBattleExp(lootExp);
			gameScene.IncreaseKills(enemyType);
			gameScene.ModifyEnemyNum(-1);
			if (criticalAttacked)
			{
				PlayDeadEffects();
			}
			else
			{
				if ((bool)animation)
				{
					AnimationState animationState = animation["Death01"];
					if (animationState != null)
					{
						animation["Death01"].wrapMode = WrapMode.ClampForever;
						animation["Death01"].speed = 1f;
						animation.CrossFade("Death01");
					}
					else
					{
						Debug.Log(string.Concat(enemyObject.name, " | ", EnemyType, " Donnot have Death1 animation"));
					}
				}
				if ((bool)enemyObject)
				{
					enemyObject.layer = 18;
				}
				PlayBloodEffect();
			}
			if (bloodRect != null)
			{
				bloodRect.gameObject.SetActiveRecursively(false);
			}
			if (TimerManager.GetInstance().Ready(92) && GameApp.GetInstance().GetGameState().SoundOn)
			{
				string text = "Zombie3D/Audio/RealPersonSound/KillCorpse/KillCorpse_";
				text += Random.Range(0, 6);
				AudioManager.PlayMusicOnce(text, enemyTransform);
				TimerManager.GetInstance().Do(92);
			}
		}

		public virtual bool OnSpecialState(float deltaTime)
		{
			return false;
		}

		public virtual EnemyState EnterSpecialState(float deltaTime)
		{
			return null;
		}

		public virtual void DoMove(float deltaTime)
		{
			float num = runSpeed * (1f + runSpeedFactor);
			enemyTransform.Translate(dir * num * deltaTime, Space.World);
		}

		public float GetSqrDistanceFromPlayer()
		{
			return (enemyTransform.position - player.GetTransform().position).sqrMagnitude;
		}

		public void Clear()
		{
			gameScene.GetEnemies().Remove(enemyObject.name);
			Object.Destroy(enemyObject);
		}

		public virtual void DoLogic(float deltaTime)
		{
			state.NextState(this, deltaTime, player);
			RemoveExceptionPositionEnemy();
			if (runSpeedFactorTimer >= 0f && runSpeedFactorTimer < runSpeedFactorTime)
			{
				runSpeedFactorTimer += deltaTime;
				if (runSpeedFactorTimer >= runSpeedFactorTime)
				{
					runSpeedFactorTimer = -1f;
					runSpeedFactorTime = -1f;
					runSpeedFactor = 0f;
				}
			}
			if (bloodRect != null)
			{
				bloodRect.SetBloodPercent(HP / maxHp);
			}
			if (!m_bShowDebugInfo)
			{
			}
		}

		protected void RemoveExceptionPositionEnemy()
		{
			if (enemyTransform.position.y < 9900.1f)
			{
				Debug.Log("ENEMY:::RemoveException!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
				DamageProperty damageProperty = new DamageProperty();
				damageProperty.damage = HP;
				criticalAttacked = false;
				OnHit(damageProperty, WeaponType.NoGun);
			}
		}
	}
}
