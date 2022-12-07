using UnityEngine;

namespace Zombie3D
{
	public class Laser : Enemy
	{
		private GameObject rayObj;

		protected bool bIsPreparing;

		private bool bIsAttacking;

		private bool bIsStable;

		private float animReadyFire01Time;

		private float animFireTime;

		private float preparingAnimTimer;

		private float animFireTimer;

		private Vector3 startPosOffset = new Vector3(0.6f, 1.5f, 5f);

		private float preparingTimer;

		private bool bPlayingPrepareAnim;

		private float stableTimer;

		private float zoomOutTimer;

		private GameObject objLine;

		private bool lineTexAlphaZoomIn;

		private float lineTexAlphaZoomInTimer;

		private bool lineTexAlphaZoomOut;

		private float lineTexAlphaZoomOutTimer;

		private float lastHitPlayerTime;

		private float lastHitFriendPlayerTime;

		private float m_LineLength = 20f;

		protected string GetHitAudioName = string.Empty;

		public override void Init(GameObject gObject)
		{
			base.Init(gObject);
			Transform folderTrans = enemyTransform.Find("Audio");
			base.Audio.AddAudio(folderTrans, "DeadEnv");
			base.Audio.AddAudio(folderTrans, "Dead");
			for (int i = 1; i < 4; i++)
			{
				string text = "GetHit0" + i;
				base.Audio.AddAudio(folderTrans, text);
			}
			runAnimationName = "Forward01";
			rayObj = Object.Instantiate(gConfig.zombieLaserLine, Vector3.zero, Quaternion.identity) as GameObject;
			rayObj.SetActiveRecursively(false);
			animReadyFire01Time = gObject.GetComponent<Animation>()["ReadyFire01"].length;
			animFireTime = gObject.GetComponent<Animation>()["Fire01"].length;
			TimerManager.GetInstance().SetTimer(21, 0.1f, true);
			TimerManager.GetInstance().SetTimer(22, 0.1f, true);
			TimerManager.GetInstance().SetTimer(22, 0.1f, true);
		}

		public override void DoLogic(float deltaTime)
		{
			base.DoLogic(deltaTime);
			if (base.HP <= 0f)
			{
				return;
			}
			if (bPlayingPrepareAnim)
			{
				preparingAnimTimer += deltaTime;
				if (preparingAnimTimer >= animReadyFire01Time)
				{
					enemyObject.GetComponent<Animation>()["Fire01"].wrapMode = WrapMode.Loop;
					enemyObject.GetComponent<Animation>().CrossFade("Fire01");
					rayObj.transform.position = enemyObject.transform.TransformPoint(startPosOffset);
					rayObj.transform.forward = enemyObject.transform.forward;
					rayObj.SetActiveRecursively(true);
					rayObj.transform.Find("laserhand attack_02").gameObject.SetActiveRecursively(false);
					objLine = rayObj.transform.Find("laserhand attack_03").gameObject;
					BoxCollider boxCollider = objLine.GetComponent(typeof(BoxCollider)) as BoxCollider;
					boxCollider.enabled = false;
					objLine.transform.localScale = new Vector3(0.01f, 1f, 0.2f);
					objLine.GetComponent<Renderer>().material.SetTexture("_MainTex", gConfig.zombieLaserLineTex1);
					Color color = objLine.GetComponent<Renderer>().material.GetColor("_TintColor");
					objLine.GetComponent<Renderer>().material.SetColor("_TintColor", new Color(color.r, color.g, color.b, 0.1f));
					lineTexAlphaZoomIn = true;
					preparingAnimTimer = 0f;
					bPlayingPrepareAnim = false;
					bIsPreparing = true;
					preparingTimer = 0f;
					lineTexAlphaZoomOut = false;
				}
			}
			if (bIsPreparing)
			{
				preparingTimer += deltaTime;
				if (preparingTimer >= 1.2f)
				{
					RaycastHit hitInfo = default(RaycastHit);
					if (Physics.Raycast(new Ray(rayObj.transform.position, rayObj.transform.forward), out hitInfo, 100f, 10240))
					{
						rayObj.SetActiveRecursively(true);
						objLine.GetComponent<Renderer>().material.SetTexture("_MainTex", gConfig.zombieLaserLineTex2);
						m_LineLength = Vector3.Distance(hitInfo.point, rayObj.transform.position) - 0.72f;
						float y = m_LineLength / 8.8f * 0.2f;
						objLine.transform.localScale = new Vector3(0.05f, y, 0.2f);
						BoxCollider boxCollider2 = objLine.GetComponent(typeof(BoxCollider)) as BoxCollider;
						boxCollider2.enabled = true;
						GameObject gameObject = rayObj.transform.Find("laserhand attack_02").gameObject;
						gameObject.SetActiveRecursively(true);
						gameObject.transform.position = hitInfo.point;
						gameObject.transform.Translate(0f, -0.43f, 0f);
					}
					else
					{
						rayObj.SetActiveRecursively(true);
						objLine.GetComponent<Renderer>().material.SetTexture("_MainTex", gConfig.zombieLaserLineTex2);
						m_LineLength = 30f;
						float y2 = m_LineLength / 8.8f * 0.2f;
						objLine.transform.localScale = new Vector3(0.05f, y2, 0.2f);
						BoxCollider boxCollider3 = objLine.GetComponent(typeof(BoxCollider)) as BoxCollider;
						boxCollider3.enabled = true;
						GameObject gameObject2 = rayObj.transform.Find("laserhand attack_02").gameObject;
						gameObject2.SetActiveRecursively(true);
						gameObject2.transform.position = rayObj.transform.position + rayObj.transform.forward * m_LineLength;
						gameObject2.transform.Translate(0f, -0.43f, 0f);
					}
					lineTexAlphaZoomIn = false;
					bIsStable = true;
					stableTimer = 0f;
					enemyObject.GetComponent<Animation>()["FireIdle01"].wrapMode = WrapMode.Loop;
					enemyObject.GetComponent<Animation>().CrossFade("FireIdle01");
					bIsPreparing = false;
				}
			}
			if (bIsStable)
			{
				stableTimer += deltaTime;
				if (stableTimer >= 2f)
				{
					lineTexAlphaZoomOut = true;
					if (objLine != null)
					{
						objLine.GetComponent<Renderer>().material.SetTexture("_MainTex", gConfig.zombieLaserLineTex1);
					}
					bIsStable = false;
					stableTimer = 0f;
				}
			}
			if (lineTexAlphaZoomIn && objLine != null)
			{
				Color color2 = objLine.GetComponent<Renderer>().material.GetColor("_TintColor");
				float a = color2.a;
				a = Mathf.Lerp(a, 1f, deltaTime / 2.5f);
				objLine.GetComponent<Renderer>().material.SetColor("_TintColor", new Color(color2.r, color2.g, color2.b, a));
				float x = objLine.transform.localScale.x;
				x = Mathf.Lerp(x, 0.05f, deltaTime / 2.5f);
				float y3 = objLine.transform.localScale.y;
				objLine.transform.localScale = new Vector3(x, y3, 0.2f);
			}
			if (lineTexAlphaZoomOut)
			{
				zoomOutTimer += deltaTime;
				if (zoomOutTimer <= 0.5f)
				{
					if (objLine != null)
					{
						Color color3 = objLine.GetComponent<Renderer>().material.GetColor("_TintColor");
						float a2 = color3.a;
						a2 = Mathf.Lerp(a2, 0.1f, deltaTime / 0.5f);
						objLine.GetComponent<Renderer>().material.SetColor("_TintColor", new Color(color3.r, color3.g, color3.b, a2));
						float x2 = objLine.transform.localScale.x;
						x2 = Mathf.Lerp(x2, 0.005f, deltaTime / 0.5f);
						float y4 = objLine.transform.localScale.y;
						objLine.transform.localScale = new Vector3(x2, y4, 0.2f);
					}
				}
				else
				{
					rayObj.SetActiveRecursively(false);
					lineTexAlphaZoomOut = false;
					bIsAttacking = false;
					enemyObject.GetComponent<Animation>()["Idle01"].wrapMode = WrapMode.Loop;
					enemyObject.GetComponent<Animation>().CrossFade("Idle01");
				}
			}
			if (objLine != null && !lineTexAlphaZoomIn && !lineTexAlphaZoomOut)
			{
				float x3 = 0.0475f + 0.05f * (0.05f * Mathf.Sin(Time.time * 50f));
				float y5 = objLine.transform.localScale.y;
				objLine.transform.localScale = new Vector3(x3, y5, 0.2f);
			}
			if (!(objLine != null) || !objLine.active)
			{
				return;
			}
			BoxCollider boxCollider4 = objLine.GetComponent(typeof(BoxCollider)) as BoxCollider;
			if (!boxCollider4.enabled)
			{
				return;
			}
			if (Time.time - lastHitPlayerTime > 0.4f)
			{
				Vector3 vector = rayObj.transform.InverseTransformPoint(player.GetTransform().position);
				if (vector.z > -1f && vector.z < m_LineLength && Mathf.Abs(vector.x) < 1f)
				{
					lastHitPlayerTime = Time.time;
					player.OnHit(attackDamage);
					if (vector.x > 0f)
					{
						player.OnHitBack(0.4f, 1f, rayObj.transform.right);
					}
					else
					{
						player.OnHitBack(0.4f, 1f, -rayObj.transform.right);
					}
				}
			}
			if (!((double)(Time.time - lastHitFriendPlayerTime) > 0.4))
			{
				return;
			}
			Vector3 vector2 = rayObj.transform.InverseTransformPoint(player.GetTransform().position);
			if (vector2.z > -1f && vector2.z < m_LineLength && Mathf.Abs(vector2.x) < 1f)
			{
				lastHitFriendPlayerTime = Time.time;
				FriendPlayer.OnHit(attackDamage);
				if (vector2.x > 0f)
				{
					FriendPlayer.OnHitBack(0.4f, 1f, rayObj.transform.right);
				}
				else
				{
					FriendPlayer.OnHitBack(0.4f, 1f, -rayObj.transform.right);
				}
			}
		}

		public override void OnAttack()
		{
			base.OnAttack();
			enemyObject.transform.LookAt(player.GetTransform().position);
			enemyObject.GetComponent<Animation>()["ReadyFire01"].wrapMode = WrapMode.Once;
			enemyObject.GetComponent<Animation>().Play("ReadyFire01");
			bPlayingPrepareAnim = true;
			preparingAnimTimer = 0f;
			bIsAttacking = true;
			lastAttackTime = Time.time;
		}

		public override void OnHit(DamageProperty dp, WeaponType weaponType)
		{
			base.OnHit(dp, weaponType);
			if (weaponType == WeaponType.Hellfire || !(base.HP > 0f))
			{
				return;
			}
			int num = Random.Range(1, 4);
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
			base.Audio.PlaySound("DeadEnv", true);
			base.Audio.PlaySound("Dead", true);
			Object.Destroy(rayObj);
		}

		public override bool AttackAnimationEnds()
		{
			return !bIsAttacking;
		}
	}
}
