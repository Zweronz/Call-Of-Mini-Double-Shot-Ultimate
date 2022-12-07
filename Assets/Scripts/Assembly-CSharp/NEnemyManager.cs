using System.Collections.Generic;
using TNetSdk;
using UnityEngine;
using Zombie3D;

public class NEnemyManager : MonoBehaviour
{
	public class NEnemyInfo
	{
		public int iMaxCount = 1;

		public int iEnemyID = -1;

		public int iEnemyType = -1;

		public List<KeyValuePair<enEnemySkillType, int>> lsSkillList = new List<KeyValuePair<enEnemySkillType, int>>();

		public int iBornPlaceID;
	}

	public class NEnemy
	{
		public Enemy enemy;

		public int isCopy;
	}

	private Dictionary<int, NEnemy> recipients = new Dictionary<int, NEnemy>();

	private List<int> m_lsDeadEnemyID = new List<int>();

	private Transform m_transSpawn;

	private int m_iEnemyCountPer = 3;

	private static NEnemyManager instance;

	public static NEnemyManager Instance
	{
		get
		{
			return instance;
		}
	}

	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		m_transSpawn = GameObject.Find("NEmenySpawn").transform;
	}

	private void Update()
	{
		if (recipients.Count <= 0)
		{
			return;
		}
		foreach (KeyValuePair<int, NEnemy> recipient in recipients)
		{
			if (recipient.Value.enemy != null)
			{
				if (recipient.Value.isCopy == 0)
				{
					recipient.Value.enemy.DoLogic(Time.deltaTime);
				}
				else
				{
					((Custom)recipient.Value.enemy).DologicExpression(Time.deltaTime);
				}
			}
		}
	}

	public void SetBrushStrange()
	{
		if (!GameSetup.Instance.RoomOwnerIsMe)
		{
			return;
		}
		SFSObject sFSObject = new SFSObject();
		SFSObject sFSObject2 = new SFSObject();
		SFSArray sFSArray = new SFSArray();
		SFSArray sFSArray2 = new SFSArray();
		int num = Random.Range(1, m_iEnemyCountPer);
		List<EnemyType> list = new List<EnemyType>();
		list.Add(EnemyType.E_BATCHER);
		list.Add(EnemyType.E_LAVA);
		list.Add(EnemyType.E_HUNTER);
		list.Add(EnemyType.E_INFECTER);
		for (int i = 0; i < num; i++)
		{
			EnemyType enemyType = list[Random.Range(0, list.Count)];
			list.Remove(enemyType);
			List<KeyValuePair<enEnemySkillType, int>> list2 = new List<KeyValuePair<enEnemySkillType, int>>();
			list2.Add(new KeyValuePair<enEnemySkillType, int>(enEnemySkillType.E_ExplodeSpore, Random.Range(1, 2)));
			list2.Add(new KeyValuePair<enEnemySkillType, int>(enEnemySkillType.E_Spikeweed, 1));
			list2.Add(new KeyValuePair<enEnemySkillType, int>(enEnemySkillType.E_GasBomb, Random.Range(1, 5)));
			list2.Add(new KeyValuePair<enEnemySkillType, int>(enEnemySkillType.E_Ionized, Random.Range(1, 4)));
			list2.Add(new KeyValuePair<enEnemySkillType, int>(enEnemySkillType.E_StingOut, Random.Range(1, 4)));
			int num2 = Random.Range(0, list2.Count - 2);
			if (num == 1)
			{
				num2 = 0;
			}
			for (int j = 0; j < num2; j++)
			{
				list2.RemoveAt(Random.Range(0, list2.Count));
			}
			NEnemyInfo nEnemyInfo = new NEnemyInfo();
			int num3 = -1;
			num3 = ((i < GameSetup.Instance.GetRoomUserList().Count) ? GameSetup.Instance.GetRoomUserList()[i].Id : GameSetup.Instance.GetRoomUserList()[i - GameSetup.Instance.GetRoomUserList().Count].Id);
			nEnemyInfo.iMaxCount = num;
			nEnemyInfo.iEnemyID = GameApp.GetInstance().GetGameState().m_eGameMode.PVE_FLOOR * 100 + i;
			nEnemyInfo.iEnemyType = (int)enemyType;
			nEnemyInfo.lsSkillList = list2;
			nEnemyInfo.iBornPlaceID = i;
			sFSArray.AddInt(nEnemyInfo.iEnemyID);
			sFSArray2.AddInt(num3);
			GameSetup.Instance.ReqSpawnEnemy(ToObject(nEnemyInfo));
		}
		sFSObject2.PutSFSArray("EIDArr", sFSArray);
		sFSObject2.PutSFSArray("EOIDArr", sFSArray2);
		sFSObject.PutInt("PVE_Floor", GameApp.GetInstance().GetGameState().m_eGameMode.PVE_FLOOR);
		sFSObject.PutSFSObject("Boss_MSG", sFSObject2);
		GameSetup.Instance.ReqSetEnemyInfoToRoomVariable(sFSObject);
	}

	public void SpawnEnemy(NEnemyInfo info)
	{
		if (recipients != null)
		{
			if (recipients.Count <= 0)
			{
				NBattleUIScript nBattleUIScript = SceneUIManager.Instance().GetSceneUIObject().GetComponent(typeof(NBattleUIScript)) as NBattleUIScript;
				nBattleUIScript.SetupBossComeInEffect();
			}
		}
		else
		{
			NBattleUIScript nBattleUIScript2 = SceneUIManager.Instance().GetSceneUIObject().GetComponent(typeof(NBattleUIScript)) as NBattleUIScript;
			nBattleUIScript2.SetupBossComeInEffect();
		}
		EnemyType iEnemyType = (EnemyType)info.iEnemyType;
		GameObject original = GameApp.GetInstance().GetGameConfig().enemy[(int)(iEnemyType - 1)];
		Enemy enemy = EnemyFactory.GetInstance().CreateEnemy(EnemyType.E_CustomBoss);
		enemy.enemyID = info.iEnemyID;
		if (enemy != null)
		{
			GameObject gameObject = (GameObject)Object.Instantiate(original, m_transSpawn.GetChild(info.iBornPlaceID).position, Quaternion.Euler(0f, 0f, 0f));
			int nextEnemyID = GameApp.GetInstance().GetGameScene().GetNextEnemyID();
			gameObject.name = "NEnemyID_" + info.iEnemyID;
			enemy.EnemyType = EnemyType.E_CustomBoss;
			enemy.Name = gameObject.name;
			switch (iEnemyType)
			{
			case EnemyType.E_INFECTER:
			{
				Material material3 = Resources.Load("Zombie3D/FBX/Materials/ZombieInfecterBoss") as Material;
				gameObject.transform.Find("Zombie_Infecter").GetComponent<Renderer>().material = material3;
				break;
			}
			case EnemyType.E_LAVA:
			{
				Material material2 = Resources.Load("Zombie3D/FBX/Materials/ZombieLavaBoss") as Material;
				gameObject.transform.Find("Zombie_Lava").GetComponent<Renderer>().material = material2;
				break;
			}
			case EnemyType.E_HUNTER:
			{
				Material material = Resources.Load("Zombie3D/FBX/Materials/ZombieHunterBoss") as Material;
				gameObject.transform.Find("Zombie_Hunter").GetComponent<Renderer>().material = material;
				break;
			}
			default:
				Debug.Log("EnemyType" + iEnemyType);
				break;
			}
			((Custom)enemy).Init(gameObject, iEnemyType, info.lsSkillList);
			enemy.SetEnemyInfoByFloorLevel(GameApp.GetInstance().GetGameState().m_eGameMode.PVE_FLOOR, info.iMaxCount, iEnemyType);
			GameApp.GetInstance().GetGameScene().GetEnemies()
				.Add(gameObject.name, enemy);
			GameApp.GetInstance().GetGameScene().ModifyEnemyNum(1);
		}
		NEnemy nEnemy = new NEnemy();
		nEnemy.isCopy = -1;
		nEnemy.enemy = enemy;
		recipients.Add(enemy.enemyID, nEnemy);
	}

	public void EnemyIsAttack(SFSObject obj)
	{
		int @int = obj.GetInt("EnemyID");
		enEnemySkillType int2 = (enEnemySkillType)obj.GetInt("ESkillType");
		List<GameObject> list = null;
		List<Vector3> list2 = null;
		if (obj.ContainsKey("ESkTarg"))
		{
			ISFSArray sFSArray = obj.GetSFSArray("ESkTarg");
			if (list == null)
			{
				list = new List<GameObject>();
			}
			for (int i = 0; i < sFSArray.Size(); i++)
			{
				if (sFSArray.GetInt(i) == GameSetup.Instance.MineUser.Id)
				{
					list.Add(PlayerManager.Instance.GetPlayerObject());
				}
				else
				{
					list.Add(PlayerManager.Instance.GetRecipient(sFSArray.GetInt(i)).PlayerObject);
				}
			}
		}
		if (obj.ContainsKey("posi"))
		{
			ISFSArray sFSArray2 = obj.GetSFSArray("posi");
			if (list2 == null)
			{
				list2 = new List<Vector3>();
			}
			for (int j = 0; j < sFSArray2.Size(); j++)
			{
				Vector3 zero = Vector3.zero;
				zero.x = sFSArray2.GetSFSArray(j).GetFloat(0);
				zero.y = sFSArray2.GetSFSArray(j).GetFloat(1);
				zero.z = sFSArray2.GetSFSArray(j).GetFloat(2);
				list2.Add(zero);
			}
		}
		if (GetNEnemyInfoByID(@int) != null)
		{
			Custom custom = (Custom)GetNEnemyInfoByID(@int).enemy;
			if (custom.GetEnemySkillImplByID(int2) != null)
			{
				switch (int2)
				{
				case enEnemySkillType.E_ExplodeSpore:
				{
					EnemySkillExplodeSpore enemySkillExplodeSpore = (EnemySkillExplodeSpore)custom.GetEnemySkillImplByID(int2);
					enemySkillExplodeSpore.CopyShoot(list[0], list2);
					break;
				}
				case enEnemySkillType.E_GasBomb:
				{
					EnemySkillGasbomb enemySkillGasbomb = (EnemySkillGasbomb)custom.GetEnemySkillImplByID(int2);
					if (list == null)
					{
						enemySkillGasbomb.CopyShoot();
					}
					else
					{
						enemySkillGasbomb.CopyShoot(list);
					}
					break;
				}
				case enEnemySkillType.E_Spikeweed:
				{
					EnemySkillSpikeweed enemySkillSpikeweed = (EnemySkillSpikeweed)custom.GetEnemySkillImplByID(int2);
					enemySkillSpikeweed.CopyShoot(list, list2);
					break;
				}
				case enEnemySkillType.E_Ionized:
				{
					EnemySkillIonized enemySkillIonized = (EnemySkillIonized)custom.GetEnemySkillImplByID(int2);
					enemySkillIonized.Shoot();
					break;
				}
				case enEnemySkillType.E_StingOut:
				{
					EnemySkillStingOut enemySkillStingOut = (EnemySkillStingOut)custom.GetEnemySkillImplByID(int2);
					enemySkillStingOut.Shoot();
					break;
				}
				}
			}
			else
			{
				Debug.Log(string.Concat("Can not find the id ", int2, " skill"));
			}
		}
		else
		{
			Debug.Log("Can not find the id " + @int + " enemy");
		}
	}

	public void ReceiveSyncHp(SFSObject table)
	{
		int @int = table.GetInt("EID");
		float @float = table.GetFloat("EHp");
		NEnemy nEnemyInfoByID = GetNEnemyInfoByID(@int);
		if (nEnemyInfoByID != null)
		{
			nEnemyInfoByID.enemy.HP = @float;
			if (table.ContainsKey("EMHp"))
			{
				float float2 = table.GetFloat("EMHp");
				nEnemyInfoByID.enemy.MAXHP = float2;
			}
		}
		if (@float <= 0f)
		{
			RemoveNEnemy(@int);
		}
	}

	public void ReceiveEnemyAnimation(SFSObject table)
	{
		int @int = table.GetInt("EID");
		string utfString = table.GetUtfString("EAnName");
		WrapMode int2 = (WrapMode)table.GetInt("EWMode");
		NEnemy nEnemyInfoByID = GetNEnemyInfoByID(@int);
		if (nEnemyInfoByID != null)
		{
			nEnemyInfoByID.enemy.Animate(utfString, int2, false);
		}
	}

	public void ReceiveNEnemyOnHitted(SFSObject table, TNetUser user)
	{
		int @int = table.GetInt("EID");
		float @float = table.GetFloat("EDam");
		NEnemy nEnemyInfoByID = Instance.GetNEnemyInfoByID(@int);
		if (nEnemyInfoByID != null && nEnemyInfoByID.isCopy <= 0)
		{
			nEnemyInfoByID.enemy.NEnemyOnHitted(user.Id, @float);
		}
	}

	public void UpdateEnemyInfo(ISFSObject obj)
	{
		ISFSArray sFSArray = obj.GetSFSArray("EIDArr");
		ISFSArray sFSArray2 = obj.GetSFSArray("EOIDArr");
		for (int i = 0; i < sFSArray2.Size(); i++)
		{
			NEnemy nEnemyInfoByID = GetNEnemyInfoByID(sFSArray.GetInt(i));
			int num = 1;
			num = ((sFSArray2.GetInt(i) != GameSetup.Instance.MineUser.Id) ? 1 : 0);
			if (nEnemyInfoByID != null && nEnemyInfoByID.isCopy != num)
			{
				if (nEnemyInfoByID.isCopy == -1 && num == 0)
				{
					GameSetup.Instance.ReqSyncEnemyHp(nEnemyInfoByID.enemy.enemyID, nEnemyInfoByID.enemy.HP, nEnemyInfoByID.enemy.MAXHP);
				}
				SetEnemyIsCopy(sFSArray.GetInt(i), num);
				SetNEnemySyncTransComp(nEnemyInfoByID);
			}
		}
	}

	public void SetNEnemySyncTransComp(NEnemy nEnemy)
	{
		if (nEnemy.isCopy == 1)
		{
			if (nEnemy.enemy.enemyObject.GetComponent(typeof(NetworkTransformSender)) != null)
			{
				Object.Destroy(nEnemy.enemy.enemyObject.GetComponent(typeof(NetworkTransformSender)));
			}
			if (nEnemy.enemy.enemyObject.GetComponent(typeof(NetworkTransformReceiver)) == null)
			{
				nEnemy.enemy.enemyObject.AddComponent(typeof(NetworkTransformReceiver));
			}
			if (nEnemy.enemy.enemyObject.GetComponent(typeof(NetworkTransformInterpolation)) == null)
			{
				nEnemy.enemy.enemyObject.AddComponent(typeof(NetworkTransformInterpolation));
			}
			return;
		}
		if (nEnemy.enemy.enemyObject.GetComponent(typeof(NetworkTransformReceiver)) != null)
		{
			Object.Destroy(nEnemy.enemy.enemyObject.GetComponent(typeof(NetworkTransformReceiver)));
		}
		if (nEnemy.enemy.enemyObject.GetComponent(typeof(NetworkTransformInterpolation)) != null)
		{
			Object.Destroy(nEnemy.enemy.enemyObject.GetComponent(typeof(NetworkTransformInterpolation)));
		}
		if (nEnemy.enemy.enemyObject.GetComponent(typeof(NetworkTransformSender)) == null)
		{
			NetworkTransformSender networkTransformSender = nEnemy.enemy.enemyObject.AddComponent(typeof(NetworkTransformSender)) as NetworkTransformSender;
			networkTransformSender.StartSendTransform();
		}
	}

	public void SetEnemyIsCopy(int enemyID, int bCopy)
	{
		if (GetNEnemyInfoByID(enemyID) != null)
		{
			GetNEnemyInfoByID(enemyID).isCopy = bCopy;
		}
	}

	public void CheckEnemyHasOwner(TNetRoom rm)
	{
		if (!rm.ContainsVariable(TNetRoomVarType.E_PVE_MSG))
		{
			return;
		}
		bool flag = false;
		SFSObject variable = rm.GetVariable(TNetRoomVarType.E_PVE_MSG);
		if (!variable.ContainsKey("Boss_MSG"))
		{
			return;
		}
		ISFSObject sFSObject = variable.GetSFSObject("Boss_MSG");
		ISFSArray sFSArray = sFSObject.GetSFSArray("EOIDArr");
		if (sFSArray == null)
		{
			return;
		}
		ISFSArray iSFSArray = new SFSArray();
		for (int i = 0; i < sFSArray.Size(); i++)
		{
			if (!GameSetup.Instance.UserIsJoinInRoom(sFSArray.GetInt(i)) || sFSArray.GetInt(i) == -1)
			{
				iSFSArray.AddInt(GameSetup.Instance.MineUser.Id);
				flag = true;
			}
			else
			{
				iSFSArray.AddInt(sFSArray.GetInt(i));
			}
		}
		sFSObject.PutSFSArray("EOIDArr", iSFSArray);
		variable.PutSFSObject("Boss_MSG", sFSObject);
		if (flag)
		{
			GameSetup.Instance.ReqSetEnemyInfoToRoomVariable(variable);
		}
	}

	public NEnemy GetNEnemyInfoByID(int enemyId)
	{
		if (recipients.ContainsKey(enemyId))
		{
			return recipients[enemyId];
		}
		return null;
	}

	public int GetNEnemyIDByObj(GameObject obj)
	{
		foreach (KeyValuePair<int, NEnemy> recipient in recipients)
		{
			if (recipient.Value.enemy.enemyObject == obj)
			{
				return recipient.Key;
			}
		}
		return -1;
	}

	public List<Enemy> GetEnemyList()
	{
		List<Enemy> list = new List<Enemy>();
		foreach (KeyValuePair<int, NEnemy> recipient in recipients)
		{
			list.Add(recipient.Value.enemy);
		}
		return list;
	}

	public void NEnemyDeadAnimationOK()
	{
		if (m_lsDeadEnemyID.Count <= 0)
		{
			return;
		}
		int num = m_lsDeadEnemyID[0];
		if (GetNEnemyInfoByID(num) != null)
		{
			if (GetNEnemyInfoByID(num).enemy.EnemyType == EnemyType.E_CustomBoss)
			{
				((Custom)GetNEnemyInfoByID(num).enemy).SkillClear();
			}
			GetNEnemyInfoByID(num).enemy.Clear();
			recipients.Remove(num);
		}
		m_lsDeadEnemyID.RemoveAt(0);
		if (recipients.Count <= 0 && GameSetup.Instance.RoomOwnerIsMe)
		{
			if (GameApp.GetInstance().GetGameState().m_eGameMode.PVE_FLOOR < 10)
			{
				GameApp.GetInstance().GetGameState().m_eGameMode.PVE_FLOOR++;
				GameSetup.Instance.ReqThisFloorOverAndUpdateMsg(GameApp.GetInstance().GetGameState().m_eGameMode.PVE_FLOOR);
				GameSetup.Instance.ReqShowFloorBalance(GameApp.GetInstance().GetGameState().m_eGameMode.PVE_FLOOR, 10f);
			}
			else
			{
				GameSetup.Instance.ReqSyncPlayerInfo(GameSetup.NPlayerDataType.E_BattleEnd, -1f);
				GameSetup.Instance.m_bIsSendBattleEndMsg = true;
			}
		}
	}

	public void RemoveNEnemy(int id)
	{
		m_lsDeadEnemyID.Add(id);
		if (GetNEnemyInfoByID(id) != null)
		{
			GetNEnemyInfoByID(id).enemy.PlayEnemyDeadEffect();
			TimeManager.Instance.Init(11, 2f, NEnemyDeadAnimationOK, null, "E_NEnemyDeadAnimation_" + GetNEnemyInfoByID(id).enemy.Name);
		}
	}

	public SFSObject ToObject(NEnemyInfo info)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("EID", info.iEnemyID);
		sFSObject.PutInt("ETy", info.iEnemyType);
		sFSObject.PutInt("EBp", info.iBornPlaceID);
		sFSObject.PutInt("EMC", info.iMaxCount);
		SFSArray sFSArray = new SFSArray();
		foreach (KeyValuePair<enEnemySkillType, int> lsSkill in info.lsSkillList)
		{
			sFSArray.AddShort((short)lsSkill.Key);
			sFSArray.AddShort((short)lsSkill.Value);
		}
		sFSObject.PutSFSArray("ESk", sFSArray);
		return sFSObject;
	}

	public NEnemyInfo FromOject(SFSObject obj)
	{
		NEnemyInfo nEnemyInfo = new NEnemyInfo();
		nEnemyInfo.iEnemyID = obj.GetInt("EID");
		nEnemyInfo.iEnemyType = obj.GetInt("ETy");
		nEnemyInfo.iBornPlaceID = obj.GetInt("EBp");
		nEnemyInfo.iMaxCount = obj.GetInt("EMC");
		ISFSArray sFSArray = obj.GetSFSArray("ESk");
		for (int i = 0; i < sFSArray.Size() / 2; i++)
		{
			nEnemyInfo.lsSkillList.Add(new KeyValuePair<enEnemySkillType, int>((enEnemySkillType)sFSArray.GetShort(2 * i), sFSArray.GetShort(2 * i + 1)));
		}
		return nEnemyInfo;
	}
}
