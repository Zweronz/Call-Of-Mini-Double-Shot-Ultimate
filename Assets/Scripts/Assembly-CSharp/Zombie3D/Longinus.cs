using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zombie3D
{
	public class Longinus : Weapon
	{
		protected float gunFireTimer = -1f;

		protected float gunFireShowTime = 0.03f;

		protected Vector3 bulletPosOffset = new Vector3(0.167f, 0.45f, 1.26f);

		private GameObject GunFireShadowLight;

		private int m_iFireCount;

		public Longinus()
		{
			maxCapacity = 100000000;
			maxGunLoad = 100000000;
			bulletCount = maxGunLoad;
			base.IsSelectedForBattle = false;
		}

		public override WeaponType GetWeaponType()
		{
			return WeaponType.Longinus;
		}

		public override void Init(Player owner)
		{
			base.Init(owner);
			m_iFireCount = 0;
			defaultTriggerTime = 0f;
			base.TriggerTime = defaultTriggerTime;
			hitForce = 20f;
			gunfire = rightHandGun.transform.Find("gun_fire_new").gameObject;
			ShowGunFire(false);
			GunFireShadowLight = rightHandGun.transform.Find("GunFire_ShadowLight").gameObject;
			if (GunFireShadowLight != null)
			{
				GunFireShadowLight.GetComponent<Renderer>().enabled = false;
				GunFireShadowLight.AddComponent(typeof(KeepFlat));
			}
			m_WeaponBulletPool = new WeaponBulletsPool();
			m_WeaponBulletPool.Init("BulletPool - Longinus", gConf.weaponBullets[(int)(GetWeaponType() - 1)], 5);
			m_WeaponBulletShellsPool = new WeaponBulletsShellPool();
			m_WeaponBulletShellsPool.Init("BulletShellsPool - Longinus", gConf.BulletShell01, 1f, 5);
			m_WeaponBulletHitParticlesPool = new WeaponBulletsHitParticlePool();
			m_WeaponBulletHitParticlesPool.Init("BulletHitParticlesPool - Longinus", gConf.weaponBulletHitParticles[(int)(GetWeaponType() - 1)], 5);
			TimerManager.GetInstance().SetTimer(82, 0.1f, true);
		}

		public override void CreateGun()
		{
			rightHandGun = (GameObject)Object.Instantiate(gConf.weapons[(int)(GetWeaponType() - 1)], player.GetTransform().position, player.GetTransform().rotation);
		}

		public override void DoLogic(float deltaTime)
		{
			if (gunFireTimer >= 0f)
			{
				gunFireTimer += Time.deltaTime;
				if (gunFireTimer > gunFireShowTime)
				{
					ShowGunFire(false);
					if (GunFireShadowLight != null)
					{
						GunFireShadowLight.GetComponent<Renderer>().enabled = false;
					}
					gunFireTimer = -1f;
				}
			}
			base.DoLogic(deltaTime);
		}

		public override void Fire(float deltaTime)
		{
			if (bulletCount == 0)
			{
				player.SetState(Player.IDLE_STATE);
				StopFire();
				return;
			}
			if (Time.time - lastShootTime > attackFrenquency)
			{
				isCDing = false;
			}
			gunFireTimer = 0f;
			ShowGunFire(true);
			m_iFireCount++;
			float y = player.GetTransform().localEulerAngles.y;
			Vector3 pos = rightHandGun.transform.TransformPoint(bulletPosOffset);
			GameObject gameObject = CreateBullet(pos, Quaternion.Euler(270f, 180f, 0f));
			if (gameObject != null)
			{
				gameObject.transform.Rotate(Vector3.forward, y);
				DisactiveTimerScript disactiveTimerScript = gameObject.GetComponent<DisactiveTimerScript>();
				if (disactiveTimerScript == null)
				{
					disactiveTimerScript = gameObject.AddComponent<DisactiveTimerScript>();
				}
				disactiveTimerScript.Init();
				disactiveTimerScript.activeTime = 0.5f;
				float num = 15f;
				if (GameApp.GetInstance().GetGameState().m_eGameMode.m_ePlayMode != 0)
				{
					num = 8f;
				}
				float num2 = 1f;
				RaycastHit hitInfo = default(RaycastHit);
				if (Physics.Raycast(new Ray(gameObject.transform.position, gameObject.transform.up), out hitInfo, num, 10240))
				{
					num = Vector3.Distance(hitInfo.point, gameObject.transform.position);
				}
				num2 = num * 0.4f;
				gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x, num2, gameObject.transform.localScale.z);
				Transform transform = gameObject.transform.Find("Bullet_Longinus");
				int clipCount = transform.GetComponent<Animation>().GetClipCount();
				if (clipCount > 0)
				{
					transform.GetComponent<Animation>().Play(transform.GetComponent<Animation>().clip.name);
				}
				bool flag = false;
				Fire_PVP(num);
				Hashtable enemies = GameApp.GetInstance().GetGameScene().GetEnemies();
				foreach (Enemy value in enemies.Values)
				{
					if (value.HP <= 0f || !(Vector3.Distance(rightHandGun.transform.position, value.GetTransform().position) < num + bulletPosOffset.z))
					{
						continue;
					}
					Vector3 vector = rightHandGun.transform.InverseTransformPoint(value.GetTransform().position);
					if (Mathf.Abs(vector.x) <= 1.4f && vector.z > 0f && vector.z < num + bulletPosOffset.z)
					{
						float thisAttack = base.Damage;
						if (!flag)
						{
							flag = true;
							thisAttack = player.GetThisAttack();
							player.CheckAttackBloodSuck();
						}
						DamageProperty damageProperty = new DamageProperty();
						int num3 = m_iFireCount % 3;
						float num4 = thisAttack;
						switch (num3)
						{
						case 0:
							num4 *= 1f;
							break;
						case 1:
							num4 *= 0.5f;
							break;
						case 2:
							num4 *= 0.5f;
							break;
						default:
							num4 *= 1f;
							break;
						}
						damageProperty.damage = num4;
						value.OnHit(damageProperty, GetWeaponType());
					}
				}
				List<JerricanScript> jerricans = GameApp.GetInstance().GetGameScene().GetJerricans();
				if (jerricans != null && jerricans.Count > 0)
				{
					for (int i = 0; i < jerricans.Count; i++)
					{
						if (!(jerricans[i].gameObject != null))
						{
							continue;
						}
						Vector3 vector2 = rightHandGun.transform.InverseTransformPoint(jerricans[i].transform.position);
						if (Mathf.Abs(vector2.x) <= 1.4f && vector2.z > 0f && vector2.z < num + bulletPosOffset.z + 1f)
						{
							int num5 = m_iFireCount % 3;
							float num6 = base.Damage;
							switch (num5)
							{
							case 0:
								num6 *= 1f;
								break;
							case 1:
								num6 *= 0.5f;
								break;
							case 2:
								num6 *= 0.5f;
								break;
							default:
								num6 *= 1f;
								break;
							}
							jerricans[i].OnHit(num6);
						}
					}
				}
				PathDoor[] pathDoors = GameApp.GetInstance().GetGameScene().GetPathDoors();
				for (int j = 0; j < pathDoors.Length; j++)
				{
					if (!(pathDoors[j] != null) || !(pathDoors[j].gameObject != null) || !(pathDoors[j].GetWorm() != null))
					{
						continue;
					}
					Vector3 vector3 = rightHandGun.transform.InverseTransformPoint(pathDoors[j].GetWorm().transform.position);
					if (!(Mathf.Abs(vector3.x) <= 1.4f) || !(vector3.z > 0f) || !(vector3.z < num + bulletPosOffset.z + 1f))
					{
						continue;
					}
					WormScript component = pathDoors[j].GetWorm().GetComponent<WormScript>();
					if (component != null)
					{
						int num7 = m_iFireCount % 3;
						float num8 = base.Damage;
						switch (num7)
						{
						case 0:
							num8 *= 1f;
							break;
						case 1:
							num8 *= 0.5f;
							break;
						case 2:
							num8 *= 0.5f;
							break;
						default:
							num8 *= 1f;
							break;
						}
						component.OnHit(num8);
					}
				}
				List<EnergyFeedwayScript> energyFeedways = GameApp.GetInstance().GetGameScene().GetEnergyFeedways();
				for (int k = 0; k < energyFeedways.Count; k++)
				{
					if (!(energyFeedways[k].gameObject != null))
					{
						continue;
					}
					Vector3 vector4 = rightHandGun.transform.InverseTransformPoint(energyFeedways[k].transform.Find("flash_01").transform.position);
					if (Mathf.Abs(vector4.x) <= 1.4f && vector4.z > 0f && vector4.z < num + bulletPosOffset.z + 3f)
					{
						int num9 = m_iFireCount % 3;
						float num10 = base.Damage;
						switch (num9)
						{
						case 0:
							num10 *= 1f;
							break;
						case 1:
							num10 *= 0.5f;
							break;
						case 2:
							num10 *= 0.5f;
							break;
						default:
							num10 *= 1f;
							break;
						}
						energyFeedways[k].OnHit(num10);
					}
				}
				CreateBulletShell(true);
			}
			if (GunFireShadowLight != null)
			{
				GunFireShadowLight.transform.position = new Vector3(GunFireShadowLight.transform.position.x, 10000.5f, GunFireShadowLight.transform.position.z);
				GunFireShadowLight.transform.Rotate(Vector3.forward, y);
				GunFireShadowLight.GetComponent<Renderer>().enabled = true;
				ShadowLightFlash shadowLightFlash = GunFireShadowLight.GetComponent(typeof(ShadowLightFlash)) as ShadowLightFlash;
				if (shadowLightFlash != null)
				{
					GunFireShadowLight.GetComponent<Animation>()["Alpha"].speed = GunFireShadowLight.GetComponent<Animation>()["Alpha"].length / gunFireShowTime;
					GunFireShadowLight.GetComponent<Animation>().Play("Alpha");
				}
			}
			if (TimerManager.GetInstance().Ready(82))
			{
				audioPlayer.PlaySound("ShootAudio", true);
				TimerManager.GetInstance().Do(82);
			}
			lastShootTime = Time.time;
		}

		public void Fire_PVP(float LineLength)
		{
			if ((GameApp.GetInstance().GetGameState().m_eGameMode.m_ePlayMode != GameState.NetworkGameMode.PlayMode.E_DeathMatch && GameApp.GetInstance().GetGameState().m_eGameMode.m_ePlayMode != GameState.NetworkGameMode.PlayMode.E_LastStand) || player != GameApp.GetInstance().GetGameScene().GetPlayer())
			{
				return;
			}
			bool flag = false;
			List<Player> recipientPlayerList = PlayerManager.Instance.GetRecipientPlayerList();
			foreach (Player item in recipientPlayerList)
			{
				if (item == null || item.HP <= 0f || (GameApp.GetInstance().GetGameState().m_eGameMode.m_eCooperaMode == GameState.NetworkGameMode.NetworkCooperationMode.E_Team && item.m_iNGroupID == PlayerManager.Instance.GetPlayerClass().m_iNGroupID) || !(Vector3.Distance(rightHandGun.transform.position, item.PlayerObject.transform.position) < LineLength + bulletPosOffset.z))
				{
					continue;
				}
				Vector3 vector = rightHandGun.transform.InverseTransformPoint(item.PlayerObject.transform.position);
				if (!(Mathf.Abs(vector.x) <= 1.4f) || !(vector.z > 0f) || !(vector.z < LineLength + bulletPosOffset.z))
				{
					continue;
				}
				float num = base.Damage;
				if (!flag)
				{
					flag = true;
					if (player != null)
					{
						num = player.GetThisAttack();
						player.CheckAttackBloodSuck();
					}
					else
					{
						num = 0f;
					}
				}
				DamageProperty damageProperty = new DamageProperty();
				if (player != null)
				{
					damageProperty.damage = num;
				}
				else
				{
					damageProperty.damage = 0f;
				}
				int num2 = m_iFireCount % 3;
				float num3 = num;
				switch (num2)
				{
				case 0:
					num3 *= 1f;
					break;
				case 1:
					num3 *= 0.5f;
					break;
				case 2:
					num3 *= 0.5f;
					break;
				default:
					num3 *= 1f;
					break;
				}
				damageProperty.damage = num3;
				item.NPlayerOnHitted(damageProperty.damage);
			}
		}

		public override void ShowGunFire(bool bShow)
		{
			if (!(gunfire != null))
			{
				return;
			}
			ParticleEmitter[] componentsInChildren = gunfire.GetComponentsInChildren<ParticleEmitter>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].emit = bShow;
				if (bShow)
				{
					componentsInChildren[i].Emit();
				}
			}
		}
	}
}
