using System;
using UnityEngine;

namespace Zombie3D
{
	public class Spore : Enemy
	{
		protected const float baseHp = 10000f;

		protected Vector3 bornPos = new Vector3(5.67f, 10000f, -3.81f);

		protected Vector3 smallScale = new Vector3(0.01f, 0.01f, 0.01f);

		protected Vector3 normalScale = new Vector3(0.85f, 0.85f, 0.85f);

		protected GameObject[] SporeThorns;

		protected SporeChild[] SporeChildren;

		protected bool bSetPosition;

		protected float attackPrepareTime = 0.6f;

		protected float attackPrepareBeginTime;

		protected float attack_frequence = 3f;

		protected bool bCheckAttack;

		protected float checkAttackBeginTime;

		protected bool bZoomOutThorn;

		protected bool bCanAttack;

		protected bool bDead;

		protected bool bZoomInSpore;

		protected bool bZoomOutSpore;

		protected float preBornStartTime;

		protected GameObject commonParticle;

		protected GameObject bornParticle;

		protected GameObject sporeDeadParticle;

		protected GameObject[] SporeThornsGroundParticles;

		protected float m_SporeChildGenerateTimer = -1f;

		protected float m_SporeChildGenerateTime = 2f;

		protected string DeadAudioName = string.Empty;

		public override void Init(GameObject gObject)
		{
			base.Init(gObject);
			Transform folderTrans = enemyTransform.Find("Audio");
			DeadAudioName = "Dead";
			base.Audio.AddAudio(folderTrans, DeadAudioName);
			for (int i = 1; i < 4; i++)
			{
				string text = "GetHit0" + i;
				base.Audio.AddAudio(folderTrans, text);
			}
			detectionRange = 0f;
			attackRange = 0f;
			bornPos = player.GetTransform().position;
			enemyObject.transform.Rotate(Vector3.up, 180f);
			enemyObject.transform.position = bornPos;
			SetState(Enemy.IDLE_STATE);
			enemyObject.transform.localScale = smallScale;
			SporeChildren = new SporeChild[3];
			for (int j = 0; j < SporeChildren.Length; j++)
			{
				SporeChildren[j] = null;
			}
			SporeThorns = new GameObject[5];
			for (int k = 0; k < SporeThorns.Length; k++)
			{
				GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(gConfig.SporeThorn, Vector3.zero, Quaternion.identity);
				gameObject.SetActiveRecursively(false);
				SporeThorns[k] = gameObject;
			}
			SporeThornsGroundParticles = new GameObject[5];
			for (int l = 0; l < SporeThornsGroundParticles.Length; l++)
			{
				GameObject gameObject2 = (GameObject)UnityEngine.Object.Instantiate(gConfig.SporeParticles02, Vector3.zero, Quaternion.identity);
				gameObject2.SetActiveRecursively(false);
				SporeThornsGroundParticles[l] = gameObject2;
			}
			attackPrepareBeginTime = Time.time;
			lastAttackTime = Time.time;
			preBornStartTime = Time.time;
			bornParticle = UnityEngine.Object.Instantiate(gConfig.SporeParticles01, new Vector3(enemyObject.transform.position.x, 10000.399f, enemyObject.transform.position.z), Quaternion.identity) as GameObject;
			bCanAttack = true;
			TimerManager.GetInstance().SetTimer(21, 0.1f, true);
			TimerManager.GetInstance().SetTimer(22, 0.1f, true);
			TimerManager.GetInstance().SetTimer(22, 0.1f, true);
		}

		public override void DoLogic(float deltaTime)
		{
			if (base.HP <= 0f && !bDead)
			{
				bDead = true;
				bZoomOutSpore = true;
				bZoomInSpore = false;
				if (commonParticle != null)
				{
					UnityEngine.Object.Destroy(commonParticle);
				}
				GameObject obj = UnityEngine.Object.Instantiate(GameApp.GetInstance().GetGameConfig().SporeParticles08, enemyObject.transform.position, Quaternion.identity) as GameObject;
				UnityEngine.Object.Destroy(obj, 1f);
				bCanAttack = false;
				for (int i = 0; i < SporeChildren.Length; i++)
				{
					if (SporeChildren[i] != null && SporeChildren[i].gameObject != null)
					{
						UnityEngine.Object.Destroy(SporeChildren[i].gameObject);
					}
				}
				for (int j = 0; j < SporeThorns.Length; j++)
				{
					UnityEngine.Object.Destroy(SporeThorns[j]);
				}
				for (int k = 0; k < SporeThornsGroundParticles.Length; k++)
				{
					UnityEngine.Object.Destroy(SporeThornsGroundParticles[k]);
				}
			}
			base.DoLogic(deltaTime);
			if (preBornStartTime >= 0f && Time.time - preBornStartTime >= 0.2f && Time.time - preBornStartTime >= 1f)
			{
				if (bornParticle != null)
				{
					UnityEngine.Object.Destroy(bornParticle);
					bornParticle = null;
					GameObject obj2 = UnityEngine.Object.Instantiate(GameApp.GetInstance().GetGameConfig().SporeParticles06, enemyObject.transform.position, Quaternion.identity) as GameObject;
					UnityEngine.Object.Destroy(obj2, 1f);
				}
				else if (Time.time - preBornStartTime >= 1.5f)
				{
					preBornStartTime = -1f;
					if (!bZoomInSpore)
					{
						if (Vector3.Distance(player.GetTransform().position, bornPos) < 3f)
						{
							Vector3 vector = player.GetRespawnTransform().TransformDirection(player.GetTransform().position - bornPos);
							player.OnHitBack(0.3f, 2f, vector);
						}
						if (Vector3.Distance(FriendPlayer.GetTransform().position, bornPos) < 3f)
						{
							Vector3 vector2 = FriendPlayer.GetRespawnTransform().TransformDirection(FriendPlayer.GetTransform().position - bornPos);
							FriendPlayer.OnHitBack(0.3f, 2f, vector2);
						}
						bZoomInSpore = true;
						bZoomOutSpore = false;
					}
				}
			}
			if (bZoomInSpore)
			{
				if (enemyObject.active && Vector3.Distance(enemyObject.transform.localScale, normalScale) >= 0.05f)
				{
					enemyObject.transform.localScale = Vector3.Lerp(enemyObject.transform.localScale, normalScale, deltaTime * 4f);
				}
				else
				{
					bZoomInSpore = false;
					m_SporeChildGenerateTimer = 0f;
					enemyObject.transform.localScale = normalScale;
					commonParticle = (GameObject)UnityEngine.Object.Instantiate(gConfig.SporeParticles10, enemyObject.transform.position, Quaternion.identity);
					commonParticle.transform.Translate(Vector3.up * 1.7f, Space.World);
				}
			}
			else if (bZoomOutSpore)
			{
				if (enemyObject.active && Vector3.Distance(enemyObject.transform.localScale, smallScale) >= 0.05f)
				{
					enemyObject.transform.localScale = Vector3.Lerp(enemyObject.transform.localScale, smallScale, deltaTime * 4f);
				}
				else
				{
					bZoomOutSpore = false;
					enemyObject.SetActiveRecursively(false);
				}
			}
			if (m_SporeChildGenerateTimer >= 0f)
			{
				m_SporeChildGenerateTimer += deltaTime;
				if (m_SporeChildGenerateTimer > m_SporeChildGenerateTime && player.HP > 0f)
				{
					for (int l = 0; l < 2; l++)
					{
						float num = UnityEngine.Random.Range(4f, 8f);
						float num2 = UnityEngine.Random.Range(0f, 360f);
						Vector3 position = player.GetTransform().position;
						float x = position.x + num * Mathf.Sin(num2 * ((float)Math.PI / 180f));
						float z = position.z + num * Mathf.Cos(num2 * ((float)Math.PI / 180f));
						GameObject gameObject = UnityEngine.Object.Instantiate(gConfig.SporeChild, new Vector3(x, 10000.3f, z), Quaternion.identity) as GameObject;
						SporeChild sporeChild = gameObject.AddComponent(typeof(SporeChild)) as SporeChild;
						sporeChild.m_Type = SporeChild.SporeChildType.Common;
						sporeChild.ParentSpore = this;
						sporeChild.damagePercent = attackDamage;
						sporeChild.hp = 100000f;
					}
					m_SporeChildGenerateTimer = 0f;
				}
			}
			if (!bCanAttack)
			{
				return;
			}
			if (Time.time - lastAttackTime >= attack_frequence)
			{
				if (!bSetPosition)
				{
					SetAttackPositions();
					ShowAttackThornGroundParticles(true);
					attackPrepareBeginTime = Time.time;
					bSetPosition = true;
				}
				if (Time.time - attackPrepareBeginTime >= attackPrepareTime)
				{
					ShowSporeThorns(true);
					ZoomInSporeThorns();
					bCheckAttack = false;
					checkAttackBeginTime = Time.time;
					bZoomOutThorn = false;
					lastAttackTime = Time.time;
					bSetPosition = false;
				}
			}
			else if (Time.time - lastAttackTime >= 1.5f)
			{
				if (!bZoomOutThorn)
				{
					ZoomOutSporeThorns();
					ShowAttackThornGroundParticles(false);
					bZoomOutThorn = true;
				}
			}
			else
			{
				if (!(Time.time - checkAttackBeginTime >= 0.3f) || bCheckAttack || !(player.HP > 0f))
				{
					return;
				}
				for (int m = 0; m < SporeThorns.Length; m++)
				{
					if (GameApp.GetInstance().GetGameState().SoundOn)
					{
						GameObject gameObject2 = SporeThorns[m].transform.Find("Audio/Born").gameObject;
						if (gameObject2 != null)
						{
							AudioSource audioSource = gameObject2.GetComponent<AudioSource>();
							if (audioSource != null)
							{
								audioSource.loop = false;
								audioSource.Play();
							}
						}
					}
					if (Vector3.Distance(player.GetTransform().position, SporeThorns[m].transform.position) < 1f)
					{
						Vector3 vector3 = player.GetTransform().position - bornPos;
						player.OnHitBack(0.3f, 0.8f, vector3);
						player.OnHit(player.GetMaxHp() * (attackDamage * 0.33f));
					}
					if (Vector3.Distance(FriendPlayer.GetTransform().position, SporeThorns[m].transform.position) < 1f)
					{
						Vector3 vector4 = FriendPlayer.GetTransform().position - bornPos;
						FriendPlayer.OnHitBack(0.3f, 0.8f, vector4);
						FriendPlayer.OnHit(FriendPlayer.GetMaxHp() * (attackDamage * 0.33f));
					}
				}
				bCheckAttack = true;
			}
		}

		public override void OnAttack()
		{
			base.OnAttack();
			Animate("Attack01", WrapMode.Once);
			lastAttackTime = Time.time;
		}

		public override void OnHit(DamageProperty dp, WeaponType weaponType)
		{
			base.OnHit(dp, weaponType);
			if (weaponType == WeaponType.Hellfire || !(base.HP > 0f))
			{
				return;
			}
			int num = UnityEngine.Random.Range(1, 4);
			string text = "GetHit0" + num;
			switch (num)
			{
			case 1:
				if (TimerManager.GetInstance().Ready(21))
				{
					base.Audio.PlaySound(text, true);
					TimerManager.GetInstance().Do(21);
				}
				break;
			case 2:
				if (TimerManager.GetInstance().Ready(22))
				{
					base.Audio.PlaySound(text, true);
					TimerManager.GetInstance().Do(22);
				}
				break;
			case 3:
				if (TimerManager.GetInstance().Ready(22))
				{
					base.Audio.PlaySound(text, true);
					TimerManager.GetInstance().Do(22);
				}
				break;
			}
		}

		public override void OnDead()
		{
			base.OnDead();
			base.Audio.PlaySound(DeadAudioName, true);
			GameObject obj = UnityEngine.Object.Instantiate(GameApp.GetInstance().GetGameConfig().SporeParticles08, enemyObject.transform.position, Quaternion.identity) as GameObject;
			UnityEngine.Object.Destroy(obj, 1f);
			if (enemyObject.active)
			{
				bZoomOutSpore = true;
				bZoomInSpore = false;
			}
		}

		public void SetAttackPositions()
		{
			SporeThorns[0].transform.position = player.GetTransform().position;
			for (int i = 1; i < SporeThorns.Length; i++)
			{
				Vector3 position = player.GetTransform().position;
				float num = UnityEngine.Random.Range(1f, 5f);
				float num2 = UnityEngine.Random.Range(0f, 360f);
				float x = position.x + num * Mathf.Sin(num2 * ((float)Math.PI / 180f));
				float z = position.z + num * Mathf.Cos(num2 * ((float)Math.PI / 180f));
				SporeThorns[i].transform.position = new Vector3(x, position.y, z);
			}
			for (int j = 0; j < SporeThorns.Length && j < SporeThornsGroundParticles.Length; j++)
			{
				SporeThornsGroundParticles[j].transform.position = SporeThorns[j].transform.position;
			}
		}

		public void ShowAttackThornGroundParticles(bool bShow)
		{
			for (int i = 0; i < SporeThornsGroundParticles.Length; i++)
			{
				SporeThornsGroundParticles[i].SetActiveRecursively(bShow);
			}
		}

		public void ShowSporeThorns(bool bShow)
		{
			for (int i = 0; i < SporeThorns.Length; i++)
			{
				SporeThorns[i].transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
				SporeThorns[i].SetActiveRecursively(bShow);
			}
		}

		public void ZoomInSporeThorns()
		{
			for (int i = 0; i < SporeThorns.Length; i++)
			{
				SporeThorns[i].GetComponent<Animation>()["Thorn_ZoomIn"].speed = 2f;
				SporeThorns[i].GetComponent<Animation>().Play("Thorn_ZoomIn");
			}
		}

		public void ZoomOutSporeThorns()
		{
			for (int i = 0; i < SporeThorns.Length; i++)
			{
				if (SporeThorns[i].active)
				{
					SporeThorns[i].GetComponent<Animation>()["Thorn_ZoomOut"].speed = 2f;
					SporeThorns[i].GetComponent<Animation>().Play("Thorn_ZoomOut");
				}
			}
		}

		public void OnSporeChildDead(SporeChild spore_child)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(gConfig.SporeParticles04, spore_child.transform.position, Quaternion.identity) as GameObject;
			gameObject.transform.Translate(Vector3.up * 1f, Space.World);
			for (int i = 0; i < SporeChildren.Length; i++)
			{
				if (SporeChildren[i] == spore_child)
				{
					SporeChildren[i] = null;
				}
			}
		}
	}
}
