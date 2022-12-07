using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zombie3D;

public class PlayerUIShow : MonoBehaviour
{
	protected List<List<GameObject>> m_WeaponObjs;

	protected Avatar m_AvatarHead;

	protected Avatar m_AvatarBody;

	protected List<GameObject> m_AvatarEffects;

	protected string m_WeaponAnimSuffix = string.Empty;

	private void Awake()
	{
		m_WeaponObjs = new List<List<GameObject>>();
		for (int i = 1; i <= 28; i++)
		{
			WeaponType weaponType = (WeaponType)i;
			SetWeaponAnimSuffix(weaponType);
			List<GameObject> list = new List<GameObject>();
			if (m_WeaponAnimSuffix == "_Two")
			{
				GameObject gameObject = Object.Instantiate(Weapon.GetWeaponPrefab((i == 28) ? (WeaponType)26 : weaponType), base.gameObject.transform.position, base.gameObject.transform.rotation) as GameObject;
				gameObject.transform.parent = base.gameObject.transform.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Neck/Bip01 L Clavicle/Bip01 L UpperArm/Bip01 L Forearm/Bip01 L Hand/Weapon_Bone_L");
				if (i == 28)
				{
					gameObject.transform.position = new Vector3(gameObject.transform.position.x - -0.091f, gameObject.transform.position.y - 0.16f, gameObject.transform.position.z - -0.134f);
				}
				Transform transform = gameObject.transform.Find("gun_fire_new");
				if (transform != null)
				{
					Renderer[] componentsInChildren = transform.gameObject.GetComponentsInChildren<Renderer>();
					for (int j = 0; j < componentsInChildren.Length; j++)
					{
						componentsInChildren[j].enabled = false;
					}
				}
				Transform transform2 = gameObject.transform.Find("GunFire_ShadowLight");
				if (transform2 != null)
				{
					if (transform2.gameObject.GetComponent<Renderer>() != null)
					{
						transform2.gameObject.GetComponent<Renderer>().enabled = false;
					}
					Renderer[] componentsInChildren2 = transform2.gameObject.GetComponentsInChildren<Renderer>();
					for (int k = 0; k < componentsInChildren2.Length; k++)
					{
						componentsInChildren2[k].enabled = false;
					}
				}
				list.Add(gameObject);
				gameObject.SetActiveRecursively(false);
				GameObject gameObject2 = Object.Instantiate(Weapon.GetWeaponPrefab((i == 28) ? (WeaponType)27 : weaponType), base.gameObject.transform.position, base.gameObject.transform.rotation) as GameObject;
				gameObject2.transform.parent = base.gameObject.transform.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Neck/Bip01 R Clavicle/Bip01 R UpperArm/Bip01 R Forearm/Bip01 R Hand/Weapon_Bone_R");
				if (i == 28)
				{
					gameObject2.transform.position = new Vector3(gameObject2.transform.position.x - -0.091f, gameObject2.transform.position.y - 0.134f, gameObject2.transform.position.z - -0.114f);
				}
				Transform transform3 = gameObject2.transform.Find("gun_fire_new");
				if (transform3 != null)
				{
					Renderer[] componentsInChildren3 = transform3.gameObject.GetComponentsInChildren<Renderer>();
					for (int l = 0; l < componentsInChildren3.Length; l++)
					{
						componentsInChildren3[l].enabled = false;
					}
				}
				Transform transform4 = gameObject2.transform.Find("GunFire_ShadowLight");
				if (transform2 != null)
				{
					if (transform2.gameObject.GetComponent<Renderer>() != null)
					{
						transform2.gameObject.GetComponent<Renderer>().enabled = false;
					}
					Renderer[] componentsInChildren4 = transform2.gameObject.GetComponentsInChildren<Renderer>();
					for (int m = 0; m < componentsInChildren4.Length; m++)
					{
						componentsInChildren4[m].enabled = false;
					}
				}
				list.Add(gameObject2);
				gameObject2.SetActiveRecursively(false);
			}
			else
			{
				GameObject gameObject3 = Object.Instantiate(Weapon.GetWeaponPrefab(weaponType), base.gameObject.transform.position, base.gameObject.transform.rotation) as GameObject;
				gameObject3.transform.parent = base.gameObject.transform.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Neck/Bip01 R Clavicle/Bip01 R UpperArm/Bip01 R Forearm/Bip01 R Hand/Weapon_Dummy");
				Transform transform5 = gameObject3.transform.Find("gun_fire_new");
				if (transform5 != null)
				{
					Renderer[] componentsInChildren5 = transform5.gameObject.GetComponentsInChildren<Renderer>();
					for (int n = 0; n < componentsInChildren5.Length; n++)
					{
						componentsInChildren5[n].enabled = false;
					}
				}
				Transform transform6 = gameObject3.transform.Find("GunFire_ShadowLight");
				if (transform6 != null)
				{
					if (transform6.gameObject.GetComponent<Renderer>() != null)
					{
						transform6.gameObject.GetComponent<Renderer>().enabled = false;
					}
					Renderer[] componentsInChildren6 = transform6.gameObject.GetComponentsInChildren<Renderer>();
					for (int num = 0; num < componentsInChildren6.Length; num++)
					{
						componentsInChildren6[num].enabled = false;
					}
				}
				list.Add(gameObject3);
				gameObject3.SetActiveRecursively(false);
			}
			m_WeaponObjs.Add(list);
		}
		ChangeWeapon(WeaponType.Beretta_33);
		m_AvatarHead = new Avatar(Avatar.AvatarSuiteType.Cowboy, Avatar.AvatarType.Head);
		m_AvatarBody = new Avatar(Avatar.AvatarSuiteType.Cowboy, Avatar.AvatarType.Body);
		LoadAvatar();
		SetAvatarEffect();
	}

	public void ShowPlayer(bool bShow)
	{
		GameObject gameObject = base.transform.root.Find("Camera").gameObject;
		Camera camera = gameObject.GetComponent<Camera>();
		camera.enabled = bShow;
		if (!bShow)
		{
			return;
		}
		ChangeWeapon(GameApp.GetInstance().GetGameState().GetBattleWeapons()[0]);
		Hashtable avatars = GameApp.GetInstance().GetGameState().GetAvatars();
		foreach (Avatar key in avatars.Keys)
		{
			if ((bool)avatars[key])
			{
				if (key.AvtType == Avatar.AvatarType.Head)
				{
					m_AvatarHead = new Avatar(key.SuiteType, key.AvtType);
				}
				else if (key.AvtType == Avatar.AvatarType.Body)
				{
					m_AvatarBody = new Avatar(key.SuiteType, key.AvtType);
				}
			}
		}
		LoadAvatar();
		SetAvatarEffect();
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void SetWeaponAnimSuffix(WeaponType weapon_type)
	{
		m_WeaponAnimSuffix = Player.GetWeaponAnimSuffix(weapon_type);
	}

	public void ChangeWeapon(WeaponType weapon_type)
	{
		SetWeaponAnimSuffix(weapon_type);
		for (int i = 0; i < m_WeaponObjs.Count; i++)
		{
			if (weapon_type == (WeaponType)(i + 1))
			{
				List<GameObject> list = m_WeaponObjs[i];
				for (int j = 0; j < list.Count; j++)
				{
					Transform transform = list[j].transform.Find("gun_fire_new");
					if (transform != null)
					{
						Renderer[] componentsInChildren = transform.gameObject.GetComponentsInChildren<Renderer>();
						for (int k = 0; k < componentsInChildren.Length; k++)
						{
							componentsInChildren[k].enabled = false;
						}
					}
					Transform transform2 = list[j].transform.Find("GunFire_ShadowLight");
					if (transform2 != null)
					{
						if (transform2.gameObject.GetComponent<Renderer>() != null)
						{
							transform2.gameObject.GetComponent<Renderer>().enabled = false;
						}
						Renderer[] componentsInChildren2 = transform2.gameObject.GetComponentsInChildren<Renderer>();
						for (int l = 0; l < componentsInChildren2.Length; l++)
						{
							componentsInChildren2[l].enabled = false;
						}
					}
					list[j].SetActiveRecursively(true);
					if (weapon_type == WeaponType.Hellfire)
					{
						Transform transform3 = list[j].transform.Find("Bullet_Hellfire");
						if (transform3 != null && transform3.gameObject != null)
						{
							transform3.gameObject.SetActiveRecursively(false);
						}
					}
					if (weapon_type == WeaponType.Messiah)
					{
						Transform transform4 = list[j].transform.Find("messaih_01");
						if (transform4 != null && transform4.gameObject != null)
						{
							transform4.gameObject.SetActiveRecursively(false);
						}
					}
				}
			}
			else
			{
				List<GameObject> list2 = m_WeaponObjs[i];
				for (int m = 0; m < list2.Count; m++)
				{
					list2[m].SetActiveRecursively(false);
				}
			}
		}
		string text = "Idle01" + m_WeaponAnimSuffix;
		base.gameObject.GetComponent<Animation>()[text].wrapMode = WrapMode.Loop;
		base.gameObject.GetComponent<Animation>().Play(text);
	}

	public void ChangeAvatar(Avatar.AvatarSuiteType suite_type, Avatar.AvatarType avt_type)
	{
		switch (avt_type)
		{
		case Avatar.AvatarType.Head:
			m_AvatarHead = new Avatar(suite_type, avt_type);
			break;
		case Avatar.AvatarType.Body:
			m_AvatarBody = new Avatar(suite_type, avt_type);
			break;
		}
		LoadAvatar();
		SetAvatarEffect();
	}

	protected void LoadAvatar()
	{
		Renderer[] componentsInChildren = base.gameObject.GetComponentsInChildren<Renderer>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (componentsInChildren[i].transform.parent.gameObject == base.gameObject && componentsInChildren[i].transform.name != "shadow")
			{
				componentsInChildren[i].enabled = false;
			}
		}
		for (int j = 0; j < componentsInChildren.Length; j++)
		{
			if (!(componentsInChildren[j].transform.parent.gameObject == base.gameObject) || !(componentsInChildren[j].transform.name != "shadow"))
			{
				continue;
			}
			for (int k = 0; k < m_AvatarHead.MeshPathList.Count; k++)
			{
				if (componentsInChildren[j].gameObject.name == m_AvatarHead.MeshPathList[k])
				{
					componentsInChildren[j].enabled = true;
					componentsInChildren[j].material = Resources.Load("Zombie3D/Avatar/" + m_AvatarHead.MatPathList[k]) as Material;
				}
			}
			for (int l = 0; l < m_AvatarBody.MeshPathList.Count; l++)
			{
				if (componentsInChildren[j].gameObject.name == m_AvatarBody.MeshPathList[l])
				{
					componentsInChildren[j].enabled = true;
					componentsInChildren[j].material = Resources.Load("Zombie3D/Avatar/" + m_AvatarBody.MatPathList[l]) as Material;
				}
			}
		}
	}

	public void SetAvatarEffect()
	{
		if (m_AvatarEffects != null)
		{
			for (int i = 0; i < m_AvatarEffects.Count; i++)
			{
				Object.Destroy(m_AvatarEffects[i]);
			}
			m_AvatarEffects.Clear();
		}
		m_AvatarEffects = new List<GameObject>();
		if (m_AvatarBody.SuiteType == Avatar.AvatarSuiteType.Gladiator)
		{
			GameObject gameObject = Object.Instantiate(Resources.Load("Zombie3D/AvatarEffect/Gladiator_body"), base.transform.position, base.transform.rotation) as GameObject;
			gameObject.transform.parent = base.transform;
			gameObject.transform.localPosition = new Vector3(0f, 0.2f, 0f);
			m_AvatarEffects.Add(gameObject);
		}
		if (m_AvatarBody.SuiteType == Avatar.AvatarSuiteType.Hacker)
		{
			GameObject gameObject2 = Object.Instantiate(Resources.Load("Zombie3D/AvatarEffect/Hacker_body"), base.transform.position, base.transform.rotation) as GameObject;
			gameObject2.transform.parent = base.transform;
			m_AvatarEffects.Add(gameObject2);
		}
		if (m_AvatarHead.SuiteType == Avatar.AvatarSuiteType.X800)
		{
			GameObject gameObject3 = Object.Instantiate(Resources.Load("Zombie3D/AvatarEffect/X800_head"), base.transform.position, base.transform.rotation) as GameObject;
			Transform transform = base.transform.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1");
			if (transform != null)
			{
				gameObject3.transform.parent = transform;
				gameObject3.transform.localRotation = Quaternion.Euler(270f, 90f, 0f);
				gameObject3.transform.localPosition = new Vector3(0.548f, -0.058f, 0f);
				m_AvatarEffects.Add(gameObject3);
			}
		}
		if (m_AvatarBody.SuiteType == Avatar.AvatarSuiteType.X800)
		{
			GameObject gameObject4 = Object.Instantiate(Resources.Load("Zombie3D/AvatarEffect/X800_body"), base.transform.position, base.transform.rotation) as GameObject;
			Transform transform2 = base.transform.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1");
			if (transform2 != null)
			{
				gameObject4.AddComponent(typeof(AvatarEffect02));
				gameObject4.transform.parent = transform2;
				gameObject4.transform.localRotation = Quaternion.Euler(270f, 90f, 0f);
				gameObject4.transform.localPosition = new Vector3(0.548f, -0.058f, 0f);
				m_AvatarEffects.Add(gameObject4);
			}
		}
		if (m_AvatarHead.SuiteType == Avatar.AvatarSuiteType.ViolenceFr)
		{
			GameObject gameObject5 = Object.Instantiate(Resources.Load("Zombie3D/AvatarEffect/ViolenceFr_head"), base.transform.position, base.transform.rotation) as GameObject;
			gameObject5.transform.parent = base.transform;
			Transform transform3 = base.transform.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Neck/Bip01 Head/Bip01 HeadNub");
			if (transform3 != null)
			{
				gameObject5.transform.parent = transform3;
				gameObject5.transform.localRotation = Quaternion.Euler(270f, 90.56f, 0f);
				gameObject5.transform.localPosition = new Vector3(0.525f, -0.222f, 0.086f);
				Transform transform4 = gameObject5.transform.Find("ViolenceFr_01");
				transform4.GetComponent<Animation>().clip.wrapMode = WrapMode.Loop;
				transform4.GetComponent<Animation>().Play(transform4.GetComponent<Animation>().clip.name);
				m_AvatarEffects.Add(gameObject5);
			}
		}
		if (m_AvatarHead.SuiteType == Avatar.AvatarSuiteType.Ninjalong)
		{
			GameObject gameObject6 = Object.Instantiate(Resources.Load("Zombie3D/AvatarEffect/NinjalongEffect_head"), base.transform.position, base.transform.rotation) as GameObject;
			Transform transform5 = base.transform.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Neck/Bip01 Head/Bip01 HeadNub");
			if (transform5 != null)
			{
				gameObject6.transform.parent = transform5;
				gameObject6.transform.localPosition = new Vector3(1.472f, 0.033f, -0.06f);
				m_AvatarEffects.Add(gameObject6);
			}
		}
		if (m_AvatarBody.SuiteType == Avatar.AvatarSuiteType.Ninjalong)
		{
			GameObject gameObject7 = Object.Instantiate(Resources.Load("Zombie3D/AvatarEffect/NinjalongEffect_body"), base.transform.position, base.transform.rotation) as GameObject;
			gameObject7.transform.parent = base.transform;
			m_AvatarEffects.Add(gameObject7);
		}
		if (m_AvatarHead.SuiteType == Avatar.AvatarSuiteType.SuperNemesis)
		{
			Renderer[] componentsInChildren = base.gameObject.GetComponentsInChildren<Renderer>();
			for (int j = 0; j < componentsInChildren.Length; j++)
			{
				if (!(componentsInChildren[j].transform.parent.gameObject == base.gameObject) || !(componentsInChildren[j].transform.name != "shadow"))
				{
					continue;
				}
				for (int k = 0; k < m_AvatarHead.MeshPathList.Count; k++)
				{
					if (componentsInChildren[j].gameObject.name == m_AvatarHead.MeshPathList[k])
					{
						AvatarEffect01 avatarEffect = componentsInChildren[j].gameObject.AddComponent<AvatarEffect01>();
						if (avatarEffect != null)
						{
							avatarEffect.m_AnimPeriod = 1f;
							avatarEffect.m_StartColor = new Color(1f, 1f, 1f, 0f);
							avatarEffect.m_EndColor = new Color(1f, 1f, 1f, 1f);
						}
					}
				}
			}
		}
		if (m_AvatarBody.SuiteType == Avatar.AvatarSuiteType.SuperNemesis || m_AvatarBody.SuiteType == Avatar.AvatarSuiteType.DemonLord)
		{
			Renderer[] componentsInChildren2 = base.gameObject.GetComponentsInChildren<Renderer>();
			for (int l = 0; l < componentsInChildren2.Length; l++)
			{
				if (!(componentsInChildren2[l].transform.parent.gameObject == base.gameObject) || !(componentsInChildren2[l].transform.name != "shadow") || !(componentsInChildren2[l].transform.parent.gameObject == base.gameObject) || !(componentsInChildren2[l].transform.name != "shadow"))
				{
					continue;
				}
				for (int m = 0; m < m_AvatarBody.MeshPathList.Count; m++)
				{
					if (componentsInChildren2[l].gameObject.name == m_AvatarBody.MeshPathList[m])
					{
						AvatarEffect01 avatarEffect2 = componentsInChildren2[l].gameObject.AddComponent<AvatarEffect01>();
						if (avatarEffect2 != null)
						{
							avatarEffect2.m_AnimPeriod = 1f;
							avatarEffect2.m_StartColor = new Color(1f, 1f, 1f, 0f);
							avatarEffect2.m_EndColor = new Color(1f, 1f, 1f, 1f);
						}
					}
				}
			}
		}
		if (m_AvatarBody.SuiteType == Avatar.AvatarSuiteType.Shinobi && m_AvatarBody.AvtType == Avatar.AvatarType.Body)
		{
			GameObject gameObject8 = Object.Instantiate(Resources.Load("Zombie3D/AvatarEffect/Shinobi_body"), base.transform.position, base.transform.rotation) as GameObject;
			gameObject8.transform.parent = base.transform;
			gameObject8.transform.localPosition = new Vector3(0f, 0.2f, 0f);
			m_AvatarEffects.Add(gameObject8);
		}
		if (m_AvatarBody.SuiteType == Avatar.AvatarSuiteType.Kunoichi && m_AvatarBody.AvtType == Avatar.AvatarType.Body)
		{
			GameObject gameObject9 = Object.Instantiate(Resources.Load("Zombie3D/AvatarEffect/Kunoichi_body"), base.transform.position, base.transform.rotation) as GameObject;
			gameObject9.transform.parent = base.transform;
			gameObject9.transform.localPosition = new Vector3(0f, 0.2f, 0f);
			m_AvatarEffects.Add(gameObject9);
		}
		if (m_AvatarBody.SuiteType == Avatar.AvatarSuiteType.Eskimo && m_AvatarBody.AvtType == Avatar.AvatarType.Body)
		{
			GameObject gameObject10 = Object.Instantiate(Resources.Load("Zombie3D/AvatarEffect/Eskimo_body"), base.transform.position, base.transform.rotation) as GameObject;
			gameObject10.transform.parent = base.transform;
			gameObject10.transform.localPosition = new Vector3(0f, 0.2f, 0f);
			m_AvatarEffects.Add(gameObject10);
		}
		if (m_AvatarBody.SuiteType != Avatar.AvatarSuiteType.DemonLord || m_AvatarBody.AvtType != Avatar.AvatarType.Body)
		{
			return;
		}
		Renderer[] componentsInChildren3 = base.gameObject.GetComponentsInChildren<Renderer>();
		for (int n = 0; n < componentsInChildren3.Length; n++)
		{
			if (!(componentsInChildren3[n].transform.parent.gameObject == base.gameObject) || !(componentsInChildren3[n].transform.name != "shadow") || !(componentsInChildren3[n].transform.parent.gameObject == base.gameObject) || !(componentsInChildren3[n].transform.name != "shadow"))
			{
				continue;
			}
			bool flag = false;
			for (int num = 0; num < m_AvatarBody.MeshPathList.Count; num++)
			{
				if (componentsInChildren3[n].gameObject.name == m_AvatarBody.MeshPathList[num] && !flag)
				{
					GameObject gameObject11 = new GameObject();
					gameObject11.name = "DemonLord_Fire";
					gameObject11.transform.parent = base.gameObject.transform;
					gameObject11.transform.localPosition = Vector3.zero;
					AvatarEffect03 avatarEffect3 = gameObject11.AddComponent<AvatarEffect03>();
					if (avatarEffect3 != null)
					{
						avatarEffect3.genTimeInterval = 0.3f;
					}
					m_AvatarEffects.Add(gameObject11);
				}
			}
		}
	}
}
