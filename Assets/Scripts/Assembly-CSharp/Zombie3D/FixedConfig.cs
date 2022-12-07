using System.Collections;
using UnityEngine;

namespace Zombie3D
{
	public class FixedConfig
	{
		public class NPCWaveCfg
		{
			public int mapIndex;

			public int pointsIndex;

			public int waveIndex;

			public ArrayList enemyInfo;

			public int[] subWaveTimes;
		}

		public class WeaponCfg
		{
			public int type;

			public string name = string.Empty;

			public int mClass;

			public int dmg;

			public float rpm;

			public float spd;

			public int levelLimit = 1;

			public string priceType = "gold";

			public int price = 1;

			public string introduction = string.Empty;

			public bool bNewWeapon;
		}

		public class EnemyCfg
		{
			public int type;

			public string name = string.Empty;

			public int hp;

			public float attack;

			public float walkSpeed = 1f;

			public float attackFrequency = 1f;

			public float attackRange = 1f;

			public int lootCash;

			public int lootExp;
		}

		public class PowerUPSCfg
		{
			public ItemType type;

			public string name;

			public string priceType = "gold";

			public int price = 1;

			public string introduction = "introduction";

			public int stamina;

			public float hp;

			public float keepTime;

			public float damagePercent;

			public float damage;

			public float attackAdd;

			public bool bNew;

			public float staminaSpeedAdd;

			public bool bNewPowerUps;
		}

		public class AvatarCfg
		{
			public Avatar.AvatarSuiteType suiteType;

			public Avatar.AvatarType avtType;

			public string name;

			public int m_Class;

			public string priceType;

			public int price;

			public int levelLimit;

			public Avatar.AvatarProperty prop;

			public string introduction;

			public bool bNewAvatar;
		}

		public class IAPCfg
		{
			public string iapID;

			public float iapDollor;

			public float gameGold;

			public float gameDollor;

			public string introduction;
		}

		public class GameCenterArchievementsCfg
		{
			public string id;

			public string name;

			public string introduction;

			public int score;
		}

		public string m_FilePath = string.Empty;

		public ArrayList waves;

		public ArrayList weapons;

		public Hashtable enemyCfgs;

		public ArrayList powerUpsCfgs;

		public ArrayList avatarCfgs;

		public ArrayList iapCfgs;

		public ArrayList GCArchievementCfgs;

		private bool m_bIapHasChanged;

		public void LoadFixedConfig()
		{
			LoadWavesConfig();
			LoadWeaponConfigData();
			LoadEnemyConfigData();
			LoadPowerUPSConfigData();
			LoadAvatarConfigData();
			LoadIAPConfigData();
			LoadGCArchievementData();
		}

		protected void LoadWavesConfig()
		{
			waves = new ArrayList();
			TextAsset textAsset = (TextAsset)Resources.Load("Zombie3D/Config/waves", typeof(TextAsset));
			if (textAsset == null)
			{
				Debug.Log("ConfigManager.LoadFixedConfig.LoadWaveConfig ERROR!!! waves.txt file not exist!!");
				return;
			}
			string text = textAsset.text;
			string[] array = text.Split('\r', '\n');
			LoadMap1Config(text);
			LoadMap2Config(text);
			LoadMap3Config(text);
			LoadMap4Config(text);
			LoadSurvivalModeMapConfig(text);
			LoadMap5Config(text);
			LoadMap6Config(text);
			LoadMap7Config(text);
			LoadSurvivalModeMap2Config(text);
		}

		public void LoadMap1Config(string text)
		{
			int num = 1;
			string[] array = text.Split('\r', '\n');
			ArrayList arrayList = new ArrayList();
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] == null || array[i].Length < 2)
				{
					continue;
				}
				NPCWaveCfg nPCWaveCfg = new NPCWaveCfg();
				string text2 = array[i].Trim();
				string[] array2 = text2.Split('\t');
				if (array2 == null || array2.Length < 2)
				{
					continue;
				}
				nPCWaveCfg.mapIndex = int.Parse(array2[0].Trim());
				if (nPCWaveCfg.mapIndex != num)
				{
					continue;
				}
				nPCWaveCfg.pointsIndex = int.Parse(array2[1].Trim());
				nPCWaveCfg.waveIndex = int.Parse(array2[2].Trim());
				nPCWaveCfg.enemyInfo = new ArrayList();
				string[] array3 = array2[3].Split(';');
				for (int j = 0; j < array3.Length; j++)
				{
					Hashtable hashtable = new Hashtable();
					string[] array4 = array3[j].Split(',');
					for (int k = 0; k < array4.Length; k++)
					{
						string[] array5 = array4[k].Trim().Split('-');
						if (array5.Length != 2)
						{
							Debug.LogError("Config ERROE!!! - waves.txt, subwave enemy type and number not a pair! - " + nPCWaveCfg.mapIndex + "-" + nPCWaveCfg.pointsIndex + "-" + nPCWaveCfg.waveIndex);
						}
						else
						{
							hashtable[array5[0].Trim()] = array5[1].Trim();
						}
					}
					nPCWaveCfg.enemyInfo.Add(hashtable);
				}
				string[] array6 = array2[4].Split(';');
				if (array3.Length != array6.Length)
				{
					Debug.Log("ERROR: - " + nPCWaveCfg.mapIndex + "|" + nPCWaveCfg.pointsIndex + "|" + nPCWaveCfg.waveIndex);
				}
				int num2 = Mathf.Min(array6.Length, array3.Length);
				nPCWaveCfg.subWaveTimes = new int[num2];
				for (int l = 0; l < array6.Length && l < array3.Length; l++)
				{
					nPCWaveCfg.subWaveTimes[l] = int.Parse(array6[l].Trim());
				}
				int maxPointsOfMap = GetMaxPointsOfMap(num);
				int waveIndex = nPCWaveCfg.waveIndex;
				if (waveIndex <= maxPointsOfMap)
				{
					NPCWaveCfg nPCWaveCfg2 = new NPCWaveCfg();
					nPCWaveCfg2.mapIndex = num;
					nPCWaveCfg2.pointsIndex = nPCWaveCfg.waveIndex;
					int maxWavesOfPoints = GetMaxWavesOfPoints(num, nPCWaveCfg2.pointsIndex);
					nPCWaveCfg2.waveIndex = maxWavesOfPoints;
					nPCWaveCfg2.enemyInfo = new ArrayList();
					string[] array7 = array2[5].Split(';');
					for (int m = 0; m < array7.Length; m++)
					{
						Hashtable hashtable2 = new Hashtable();
						string[] array8 = array7[m].Split(',');
						for (int n = 0; n < array8.Length; n++)
						{
							string[] array9 = array8[n].Trim().Split('-');
							if (array9.Length != 2)
							{
								Debug.LogError("Config ERROE!!! - waves.txt, subwave enemy type and number not a pair! - " + nPCWaveCfg2.mapIndex + "-" + nPCWaveCfg2.pointsIndex + "-" + nPCWaveCfg2.waveIndex);
							}
							else
							{
								hashtable2[array9[0].Trim()] = array9[1].Trim();
							}
						}
						nPCWaveCfg2.enemyInfo.Add(hashtable2);
					}
					string[] array10 = array2[6].Split(';');
					if (array7.Length != array10.Length)
					{
						Debug.Log("ERROR: - " + nPCWaveCfg.mapIndex + "|" + nPCWaveCfg.pointsIndex + "|" + nPCWaveCfg.waveIndex);
					}
					nPCWaveCfg2.subWaveTimes = new int[array10.Length];
					for (int num3 = 0; num3 < array10.Length && num3 < array7.Length; num3++)
					{
						nPCWaveCfg2.subWaveTimes[num3] = int.Parse(array10[num3].Trim());
					}
					waves.Add(nPCWaveCfg2);
				}
				arrayList.Add(nPCWaveCfg);
			}
			int maxPointsOfMap2 = GetMaxPointsOfMap(num);
			for (int num4 = 1; num4 < maxPointsOfMap2 + 1; num4++)
			{
				int maxWavesOfPoints2 = GetMaxWavesOfPoints(num, num4);
				for (int num5 = 1; num5 < maxWavesOfPoints2; num5++)
				{
					NPCWaveCfg nPCWaveCfg3 = new NPCWaveCfg();
					for (int num6 = 0; num6 < arrayList.Count; num6++)
					{
						NPCWaveCfg nPCWaveCfg4 = (NPCWaveCfg)arrayList[num6];
						if (nPCWaveCfg4.mapIndex == num && nPCWaveCfg4.pointsIndex == 1 && nPCWaveCfg4.waveIndex == num4 + num5 - 1)
						{
							nPCWaveCfg3 = nPCWaveCfg4;
							break;
						}
					}
					if (nPCWaveCfg3 == null)
					{
						continue;
					}
					NPCWaveCfg nPCWaveCfg5 = new NPCWaveCfg();
					nPCWaveCfg5.mapIndex = num;
					nPCWaveCfg5.pointsIndex = num4;
					nPCWaveCfg5.waveIndex = num5;
					nPCWaveCfg5.enemyInfo = new ArrayList();
					for (int num7 = 0; num7 < nPCWaveCfg3.enemyInfo.Count; num7++)
					{
						Hashtable hashtable3 = new Hashtable();
						Hashtable hashtable4 = (Hashtable)nPCWaveCfg3.enemyInfo[num7];
						foreach (string key in hashtable4.Keys)
						{
							int num8 = int.Parse(hashtable4[key].ToString());
							hashtable3[key] = Mathf.FloorToInt((float)num8 * (1f + (float)(num4 - 1) * 0.024f)).ToString();
						}
						nPCWaveCfg5.enemyInfo.Add(hashtable3);
					}
					nPCWaveCfg5.subWaveTimes = new int[nPCWaveCfg3.subWaveTimes.Length];
					nPCWaveCfg3.subWaveTimes.CopyTo(nPCWaveCfg5.subWaveTimes, 0);
					waves.Add(nPCWaveCfg5);
				}
			}
		}

		public void LoadMap2Config(string text)
		{
			int num = 2;
			string[] array = text.Split('\r', '\n');
			ArrayList arrayList = new ArrayList();
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] == null || array[i].Length < 2)
				{
					continue;
				}
				NPCWaveCfg nPCWaveCfg = new NPCWaveCfg();
				string text2 = array[i].Trim();
				string[] array2 = text2.Split('\t');
				if (array2 == null || array2.Length < 2)
				{
					continue;
				}
				nPCWaveCfg.mapIndex = int.Parse(array2[0].Trim());
				if (nPCWaveCfg.mapIndex != num)
				{
					continue;
				}
				nPCWaveCfg.pointsIndex = int.Parse(array2[1].Trim());
				nPCWaveCfg.waveIndex = int.Parse(array2[2].Trim());
				nPCWaveCfg.enemyInfo = new ArrayList();
				string[] array3 = array2[3].Split(';');
				for (int j = 0; j < array3.Length; j++)
				{
					Hashtable hashtable = new Hashtable();
					string[] array4 = array3[j].Split(',');
					for (int k = 0; k < array4.Length; k++)
					{
						string[] array5 = array4[k].Trim().Split('-');
						if (array5.Length != 2)
						{
							Debug.LogError("Config ERROE!!! - waves.txt, subwave enemy type and number not a pair! - " + nPCWaveCfg.mapIndex + "-" + nPCWaveCfg.pointsIndex + "-" + nPCWaveCfg.waveIndex);
						}
						else
						{
							hashtable[array5[0].Trim()] = array5[1].Trim();
						}
					}
					nPCWaveCfg.enemyInfo.Add(hashtable);
				}
				string[] array6 = array2[4].Split(';');
				if (array3.Length != array6.Length)
				{
					Debug.Log("ERROR: - " + nPCWaveCfg.mapIndex + "|" + nPCWaveCfg.pointsIndex + "|" + nPCWaveCfg.waveIndex);
				}
				int num2 = Mathf.Min(array6.Length, array3.Length);
				nPCWaveCfg.subWaveTimes = new int[num2];
				for (int l = 0; l < array6.Length && l < array3.Length; l++)
				{
					nPCWaveCfg.subWaveTimes[l] = int.Parse(array6[l].Trim());
				}
				int maxPointsOfMap = GetMaxPointsOfMap(num);
				int waveIndex = nPCWaveCfg.waveIndex;
				if (waveIndex <= maxPointsOfMap)
				{
					NPCWaveCfg nPCWaveCfg2 = new NPCWaveCfg();
					nPCWaveCfg2.mapIndex = num;
					nPCWaveCfg2.pointsIndex = nPCWaveCfg.waveIndex;
					int maxWavesOfPoints = GetMaxWavesOfPoints(num, nPCWaveCfg2.pointsIndex);
					nPCWaveCfg2.waveIndex = maxWavesOfPoints;
					nPCWaveCfg2.enemyInfo = new ArrayList();
					string[] array7 = array2[5].Split(';');
					for (int m = 0; m < array7.Length; m++)
					{
						Hashtable hashtable2 = new Hashtable();
						string[] array8 = array7[m].Split(',');
						for (int n = 0; n < array8.Length; n++)
						{
							string[] array9 = array8[n].Trim().Split('-');
							if (array9.Length != 2)
							{
								Debug.LogError("Config ERROE!!! - waves.txt, subwave enemy type and number not a pair! - " + nPCWaveCfg2.mapIndex + "-" + nPCWaveCfg2.pointsIndex + "-" + nPCWaveCfg2.waveIndex);
							}
							else
							{
								hashtable2[array9[0].Trim()] = array9[1].Trim();
							}
						}
						nPCWaveCfg2.enemyInfo.Add(hashtable2);
					}
					string[] array10 = array2[6].Split(';');
					if (array7.Length != array10.Length)
					{
						Debug.Log("ERROR: - " + nPCWaveCfg.mapIndex + "|" + nPCWaveCfg.pointsIndex + "|" + nPCWaveCfg.waveIndex);
					}
					nPCWaveCfg2.subWaveTimes = new int[array10.Length];
					for (int num3 = 0; num3 < array10.Length && num3 < array7.Length; num3++)
					{
						nPCWaveCfg2.subWaveTimes[num3] = int.Parse(array10[num3].Trim());
					}
					waves.Add(nPCWaveCfg2);
				}
				arrayList.Add(nPCWaveCfg);
			}
			int maxPointsOfMap2 = GetMaxPointsOfMap(num);
			for (int num4 = 1; num4 < maxPointsOfMap2 + 1; num4++)
			{
				int maxWavesOfPoints2 = GetMaxWavesOfPoints(num, num4);
				for (int num5 = 1; num5 < maxWavesOfPoints2; num5++)
				{
					NPCWaveCfg nPCWaveCfg3 = new NPCWaveCfg();
					for (int num6 = 0; num6 < arrayList.Count; num6++)
					{
						NPCWaveCfg nPCWaveCfg4 = (NPCWaveCfg)arrayList[num6];
						if (nPCWaveCfg4.mapIndex == num && nPCWaveCfg4.pointsIndex == 1 && nPCWaveCfg4.waveIndex == num4 + num5 - 1)
						{
							nPCWaveCfg3 = nPCWaveCfg4;
							break;
						}
					}
					if (nPCWaveCfg3 == null)
					{
						continue;
					}
					NPCWaveCfg nPCWaveCfg5 = new NPCWaveCfg();
					nPCWaveCfg5.mapIndex = num;
					nPCWaveCfg5.pointsIndex = num4;
					nPCWaveCfg5.waveIndex = num5;
					nPCWaveCfg5.enemyInfo = new ArrayList();
					for (int num7 = 0; num7 < nPCWaveCfg3.enemyInfo.Count; num7++)
					{
						Hashtable hashtable3 = new Hashtable();
						Hashtable hashtable4 = (Hashtable)nPCWaveCfg3.enemyInfo[num7];
						foreach (string key in hashtable4.Keys)
						{
							int num8 = int.Parse(hashtable4[key].ToString());
							hashtable3[key] = Mathf.FloorToInt((float)num8 * (1f + (float)(num4 - 1) * 0.024f)).ToString();
						}
						nPCWaveCfg5.enemyInfo.Add(hashtable3);
					}
					nPCWaveCfg5.subWaveTimes = new int[nPCWaveCfg3.subWaveTimes.Length];
					nPCWaveCfg3.subWaveTimes.CopyTo(nPCWaveCfg5.subWaveTimes, 0);
					waves.Add(nPCWaveCfg5);
				}
			}
		}

		public void LoadMap3Config(string text)
		{
			int num = 3;
			string[] array = text.Split('\r', '\n');
			ArrayList arrayList = new ArrayList();
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] == null || array[i].Length < 2)
				{
					continue;
				}
				NPCWaveCfg nPCWaveCfg = new NPCWaveCfg();
				string text2 = array[i].Trim();
				string[] array2 = text2.Split('\t');
				if (array2 == null || array2.Length < 2)
				{
					continue;
				}
				nPCWaveCfg.mapIndex = int.Parse(array2[0].Trim());
				if (nPCWaveCfg.mapIndex != num)
				{
					continue;
				}
				nPCWaveCfg.pointsIndex = int.Parse(array2[1].Trim());
				nPCWaveCfg.waveIndex = int.Parse(array2[2].Trim());
				nPCWaveCfg.enemyInfo = new ArrayList();
				string[] array3 = array2[3].Split(';');
				for (int j = 0; j < array3.Length; j++)
				{
					Hashtable hashtable = new Hashtable();
					string[] array4 = array3[j].Split(',');
					for (int k = 0; k < array4.Length; k++)
					{
						string[] array5 = array4[k].Trim().Split('-');
						if (array5.Length != 2)
						{
							Debug.LogError("Config ERROE!!! - waves.txt, subwave enemy type and number not a pair! - " + nPCWaveCfg.mapIndex + "-" + nPCWaveCfg.pointsIndex + "-" + nPCWaveCfg.waveIndex);
						}
						else
						{
							hashtable[array5[0].Trim()] = array5[1].Trim();
						}
					}
					nPCWaveCfg.enemyInfo.Add(hashtable);
				}
				string[] array6 = array2[4].Split(';');
				if (array3.Length != array6.Length)
				{
					Debug.Log("ERROR: - " + nPCWaveCfg.mapIndex + "|" + nPCWaveCfg.pointsIndex + "|" + nPCWaveCfg.waveIndex);
				}
				int num2 = Mathf.Min(array6.Length, array3.Length);
				nPCWaveCfg.subWaveTimes = new int[num2];
				for (int l = 0; l < array6.Length && l < array3.Length; l++)
				{
					nPCWaveCfg.subWaveTimes[l] = int.Parse(array6[l].Trim());
				}
				arrayList.Add(nPCWaveCfg);
			}
			for (int m = 1; m < 6; m++)
			{
				int num3 = m;
				int num4 = 50;
				for (int n = 1; n < num4 + 1; n++)
				{
					NPCWaveCfg nPCWaveCfg2 = new NPCWaveCfg();
					for (int num5 = 0; num5 < arrayList.Count; num5++)
					{
						NPCWaveCfg nPCWaveCfg3 = (NPCWaveCfg)arrayList[num5];
						if (nPCWaveCfg3.mapIndex == num && nPCWaveCfg3.pointsIndex == 1 && nPCWaveCfg3.waveIndex == n)
						{
							nPCWaveCfg2 = nPCWaveCfg3;
							break;
						}
					}
					if (nPCWaveCfg2 == null)
					{
						continue;
					}
					NPCWaveCfg nPCWaveCfg4 = new NPCWaveCfg();
					nPCWaveCfg4.mapIndex = num;
					nPCWaveCfg4.pointsIndex = m;
					nPCWaveCfg4.waveIndex = n;
					nPCWaveCfg4.enemyInfo = new ArrayList();
					nPCWaveCfg4.enemyInfo = new ArrayList();
					for (int num6 = 0; num6 < nPCWaveCfg2.enemyInfo.Count; num6++)
					{
						Hashtable hashtable2 = new Hashtable();
						Hashtable hashtable3 = (Hashtable)nPCWaveCfg2.enemyInfo[num6];
						foreach (string key in hashtable3.Keys)
						{
							int num7 = int.Parse(hashtable3[key].ToString());
							num7 = num7;
							hashtable2[key] = num7.ToString();
						}
						nPCWaveCfg4.enemyInfo.Add(hashtable2);
					}
					nPCWaveCfg4.subWaveTimes = new int[nPCWaveCfg2.subWaveTimes.Length];
					nPCWaveCfg2.subWaveTimes.CopyTo(nPCWaveCfg4.subWaveTimes, 0);
					waves.Add(nPCWaveCfg4);
				}
			}
		}

		public void LoadMap4Config(string text)
		{
			int num = 4;
			string[] array = text.Split('\r', '\n');
			ArrayList arrayList = new ArrayList();
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] == null || array[i].Length < 2)
				{
					continue;
				}
				NPCWaveCfg nPCWaveCfg = new NPCWaveCfg();
				string text2 = array[i].Trim();
				string[] array2 = text2.Split('\t');
				if (array2 == null || array2.Length < 2)
				{
					continue;
				}
				nPCWaveCfg.mapIndex = int.Parse(array2[0].Trim());
				if (nPCWaveCfg.mapIndex != num)
				{
					continue;
				}
				nPCWaveCfg.pointsIndex = int.Parse(array2[1].Trim());
				nPCWaveCfg.waveIndex = int.Parse(array2[2].Trim());
				nPCWaveCfg.enemyInfo = new ArrayList();
				string[] array3 = array2[3].Split(';');
				for (int j = 0; j < array3.Length; j++)
				{
					Hashtable hashtable = new Hashtable();
					string[] array4 = array3[j].Split(',');
					for (int k = 0; k < array4.Length; k++)
					{
						string[] array5 = array4[k].Trim().Split('-');
						if (array5.Length != 2)
						{
							Debug.LogError("Config ERROE!!! - waves.txt, subwave enemy type and number not a pair! - " + nPCWaveCfg.mapIndex + "-" + nPCWaveCfg.pointsIndex + "-" + nPCWaveCfg.waveIndex);
						}
						else
						{
							hashtable[array5[0].Trim()] = array5[1].Trim();
						}
					}
					nPCWaveCfg.enemyInfo.Add(hashtable);
				}
				string[] array6 = array2[4].Split(';');
				if (array3.Length != array6.Length)
				{
					Debug.Log("ERROR: - " + nPCWaveCfg.mapIndex + "|" + nPCWaveCfg.pointsIndex + "|" + nPCWaveCfg.waveIndex);
				}
				int num2 = Mathf.Min(array6.Length, array3.Length);
				nPCWaveCfg.subWaveTimes = new int[num2];
				for (int l = 0; l < array6.Length && l < array3.Length; l++)
				{
					nPCWaveCfg.subWaveTimes[l] = int.Parse(array6[l].Trim());
				}
				arrayList.Add(nPCWaveCfg);
			}
			for (int m = 1; m < 6; m++)
			{
				int num3 = m;
				int num4 = 50;
				for (int n = 1; n < num4 + 1; n++)
				{
					NPCWaveCfg nPCWaveCfg2 = new NPCWaveCfg();
					for (int num5 = 0; num5 < arrayList.Count; num5++)
					{
						NPCWaveCfg nPCWaveCfg3 = (NPCWaveCfg)arrayList[num5];
						if (nPCWaveCfg3.mapIndex == num && nPCWaveCfg3.pointsIndex == 1 && nPCWaveCfg3.waveIndex == n)
						{
							nPCWaveCfg2 = nPCWaveCfg3;
							break;
						}
					}
					if (nPCWaveCfg2 == null)
					{
						continue;
					}
					NPCWaveCfg nPCWaveCfg4 = new NPCWaveCfg();
					nPCWaveCfg4.mapIndex = num;
					nPCWaveCfg4.pointsIndex = m;
					nPCWaveCfg4.waveIndex = n;
					nPCWaveCfg4.enemyInfo = new ArrayList();
					nPCWaveCfg4.enemyInfo = new ArrayList();
					for (int num6 = 0; num6 < nPCWaveCfg2.enemyInfo.Count; num6++)
					{
						Hashtable hashtable2 = new Hashtable();
						Hashtable hashtable3 = (Hashtable)nPCWaveCfg2.enemyInfo[num6];
						foreach (string key in hashtable3.Keys)
						{
							int num7 = int.Parse(hashtable3[key].ToString());
							num7 = num7;
							hashtable2[key] = num7.ToString();
						}
						nPCWaveCfg4.enemyInfo.Add(hashtable2);
					}
					nPCWaveCfg4.subWaveTimes = new int[nPCWaveCfg2.subWaveTimes.Length];
					nPCWaveCfg2.subWaveTimes.CopyTo(nPCWaveCfg4.subWaveTimes, 0);
					waves.Add(nPCWaveCfg4);
				}
			}
		}

		public void LoadSurvivalModeMapConfig(string text)
		{
			int num = 101;
			string[] array = text.Split('\r', '\n');
			ArrayList arrayList = new ArrayList();
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] == null || array[i].Length < 2)
				{
					continue;
				}
				NPCWaveCfg nPCWaveCfg = new NPCWaveCfg();
				string text2 = array[i].Trim();
				string[] array2 = text2.Split('\t');
				if (array2 == null || array2.Length < 2)
				{
					continue;
				}
				nPCWaveCfg.mapIndex = int.Parse(array2[0].Trim());
				if (nPCWaveCfg.mapIndex != num)
				{
					continue;
				}
				nPCWaveCfg.pointsIndex = int.Parse(array2[1].Trim());
				nPCWaveCfg.waveIndex = int.Parse(array2[2].Trim());
				nPCWaveCfg.enemyInfo = new ArrayList();
				string[] array3 = array2[3].Split(';');
				for (int j = 0; j < array3.Length; j++)
				{
					Hashtable hashtable = new Hashtable();
					string[] array4 = array3[j].Split(',');
					for (int k = 0; k < array4.Length; k++)
					{
						string[] array5 = array4[k].Trim().Split('-');
						if (array5.Length != 2)
						{
							Debug.LogError("Config ERROE!!! - waves.txt, subwave enemy type and number not a pair! - " + nPCWaveCfg.mapIndex + "-" + nPCWaveCfg.pointsIndex + "-" + nPCWaveCfg.waveIndex);
						}
						else
						{
							hashtable[array5[0].Trim()] = array5[1].Trim();
						}
					}
					nPCWaveCfg.enemyInfo.Add(hashtable);
				}
				string[] array6 = array2[4].Split(';');
				if (array3.Length != array6.Length)
				{
					Debug.Log("ERROR: - " + nPCWaveCfg.mapIndex + "|" + nPCWaveCfg.pointsIndex + "|" + nPCWaveCfg.waveIndex);
				}
				int num2 = Mathf.Min(array6.Length, array3.Length);
				nPCWaveCfg.subWaveTimes = new int[num2];
				for (int l = 0; l < array6.Length && l < array3.Length; l++)
				{
					nPCWaveCfg.subWaveTimes[l] = int.Parse(array6[l].Trim());
				}
				arrayList.Add(nPCWaveCfg);
			}
			for (int m = 1; m < 11; m++)
			{
				int num3 = 50;
				for (int n = 1; n < num3 + 1; n++)
				{
					NPCWaveCfg nPCWaveCfg2 = new NPCWaveCfg();
					for (int num4 = 0; num4 < arrayList.Count; num4++)
					{
						NPCWaveCfg nPCWaveCfg3 = (NPCWaveCfg)arrayList[num4];
						if (nPCWaveCfg3.mapIndex == num && nPCWaveCfg3.pointsIndex == 1 && nPCWaveCfg3.waveIndex == n)
						{
							nPCWaveCfg2 = nPCWaveCfg3;
							break;
						}
					}
					if (nPCWaveCfg2 == null)
					{
						continue;
					}
					NPCWaveCfg nPCWaveCfg4 = new NPCWaveCfg();
					nPCWaveCfg4.mapIndex = num;
					nPCWaveCfg4.pointsIndex = m;
					nPCWaveCfg4.waveIndex = n;
					nPCWaveCfg4.enemyInfo = new ArrayList();
					nPCWaveCfg4.enemyInfo = new ArrayList();
					for (int num5 = 0; num5 < nPCWaveCfg2.enemyInfo.Count; num5++)
					{
						Hashtable hashtable2 = new Hashtable();
						Hashtable hashtable3 = (Hashtable)nPCWaveCfg2.enemyInfo[num5];
						foreach (string key in hashtable3.Keys)
						{
							int num6 = int.Parse(hashtable3[key].ToString());
							hashtable2[key] = Mathf.FloorToInt((float)num6 * 1.1f).ToString();
						}
						nPCWaveCfg4.enemyInfo.Add(hashtable2);
					}
					nPCWaveCfg4.subWaveTimes = new int[nPCWaveCfg2.subWaveTimes.Length];
					nPCWaveCfg2.subWaveTimes.CopyTo(nPCWaveCfg4.subWaveTimes, 0);
					waves.Add(nPCWaveCfg4);
				}
			}
		}

		public void LoadSurvivalModeMap2Config(string text)
		{
			int num = 102;
			string[] array = text.Split('\r', '\n');
			ArrayList arrayList = new ArrayList();
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] == null || array[i].Length < 2)
				{
					continue;
				}
				NPCWaveCfg nPCWaveCfg = new NPCWaveCfg();
				string text2 = array[i].Trim();
				string[] array2 = text2.Split('\t');
				if (array2 == null || array2.Length < 2)
				{
					continue;
				}
				nPCWaveCfg.mapIndex = int.Parse(array2[0].Trim());
				if (nPCWaveCfg.mapIndex != num)
				{
					continue;
				}
				nPCWaveCfg.pointsIndex = int.Parse(array2[1].Trim());
				nPCWaveCfg.waveIndex = int.Parse(array2[2].Trim());
				nPCWaveCfg.enemyInfo = new ArrayList();
				string[] array3 = array2[3].Split(';');
				for (int j = 0; j < array3.Length; j++)
				{
					Hashtable hashtable = new Hashtable();
					string[] array4 = array3[j].Split(',');
					for (int k = 0; k < array4.Length; k++)
					{
						string[] array5 = array4[k].Trim().Split('-');
						if (array5.Length != 2)
						{
							Debug.LogError("Config ERROE!!! - waves.txt, subwave enemy type and number not a pair! - " + nPCWaveCfg.mapIndex + "-" + nPCWaveCfg.pointsIndex + "-" + nPCWaveCfg.waveIndex);
						}
						else
						{
							hashtable[array5[0].Trim()] = array5[1].Trim();
						}
					}
					nPCWaveCfg.enemyInfo.Add(hashtable);
				}
				string[] array6 = array2[4].Split(';');
				if (array3.Length != array6.Length)
				{
					Debug.Log("ERROR: - " + nPCWaveCfg.mapIndex + "|" + nPCWaveCfg.pointsIndex + "|" + nPCWaveCfg.waveIndex);
				}
				int num2 = Mathf.Min(array6.Length, array3.Length);
				nPCWaveCfg.subWaveTimes = new int[num2];
				for (int l = 0; l < array6.Length && l < array3.Length; l++)
				{
					nPCWaveCfg.subWaveTimes[l] = int.Parse(array6[l].Trim());
				}
				arrayList.Add(nPCWaveCfg);
			}
			for (int m = 1; m < 11; m++)
			{
				int num3 = 50;
				for (int n = 1; n < num3 + 1; n++)
				{
					NPCWaveCfg nPCWaveCfg2 = new NPCWaveCfg();
					for (int num4 = 0; num4 < arrayList.Count; num4++)
					{
						NPCWaveCfg nPCWaveCfg3 = (NPCWaveCfg)arrayList[num4];
						if (nPCWaveCfg3.mapIndex == num && nPCWaveCfg3.pointsIndex == 1 && nPCWaveCfg3.waveIndex == n)
						{
							nPCWaveCfg2 = nPCWaveCfg3;
							break;
						}
					}
					if (nPCWaveCfg2 == null)
					{
						continue;
					}
					NPCWaveCfg nPCWaveCfg4 = new NPCWaveCfg();
					nPCWaveCfg4.mapIndex = num;
					nPCWaveCfg4.pointsIndex = m;
					nPCWaveCfg4.waveIndex = n;
					nPCWaveCfg4.enemyInfo = new ArrayList();
					nPCWaveCfg4.enemyInfo = new ArrayList();
					for (int num5 = 0; num5 < nPCWaveCfg2.enemyInfo.Count; num5++)
					{
						Hashtable hashtable2 = new Hashtable();
						Hashtable hashtable3 = (Hashtable)nPCWaveCfg2.enemyInfo[num5];
						foreach (string key in hashtable3.Keys)
						{
							int num6 = int.Parse(hashtable3[key].ToString());
							hashtable2[key] = Mathf.FloorToInt((float)num6 * 1.1f).ToString();
						}
						nPCWaveCfg4.enemyInfo.Add(hashtable2);
					}
					nPCWaveCfg4.subWaveTimes = new int[nPCWaveCfg2.subWaveTimes.Length];
					nPCWaveCfg2.subWaveTimes.CopyTo(nPCWaveCfg4.subWaveTimes, 0);
					waves.Add(nPCWaveCfg4);
				}
			}
		}

		public void LoadMap5Config(string text)
		{
			int num = 5;
			string[] array = text.Split('\r', '\n');
			ArrayList arrayList = new ArrayList();
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] == null || array[i].Length < 2)
				{
					continue;
				}
				NPCWaveCfg nPCWaveCfg = new NPCWaveCfg();
				string text2 = array[i].Trim();
				string[] array2 = text2.Split('\t');
				if (array2 == null || array2.Length < 2)
				{
					continue;
				}
				nPCWaveCfg.mapIndex = int.Parse(array2[0].Trim());
				if (nPCWaveCfg.mapIndex != num)
				{
					continue;
				}
				nPCWaveCfg.pointsIndex = int.Parse(array2[1].Trim());
				nPCWaveCfg.waveIndex = int.Parse(array2[2].Trim());
				nPCWaveCfg.enemyInfo = new ArrayList();
				string[] array3 = array2[3].Split(';');
				for (int j = 0; j < array3.Length; j++)
				{
					Hashtable hashtable = new Hashtable();
					string[] array4 = array3[j].Split(',');
					for (int k = 0; k < array4.Length; k++)
					{
						string[] array5 = array4[k].Trim().Split('-');
						if (array5.Length != 2)
						{
							Debug.LogError("Config ERROE!!! - waves.txt, subwave enemy type and number not a pair! - " + nPCWaveCfg.mapIndex + "-" + nPCWaveCfg.pointsIndex + "-" + nPCWaveCfg.waveIndex);
						}
						else
						{
							hashtable[array5[0].Trim()] = array5[1].Trim();
						}
					}
					nPCWaveCfg.enemyInfo.Add(hashtable);
				}
				string[] array6 = array2[4].Split(';');
				if (array3.Length != array6.Length)
				{
					Debug.Log("ERROR: - " + nPCWaveCfg.mapIndex + "|" + nPCWaveCfg.pointsIndex + "|" + nPCWaveCfg.waveIndex);
				}
				int num2 = Mathf.Min(array6.Length, array3.Length);
				nPCWaveCfg.subWaveTimes = new int[num2];
				for (int l = 0; l < array6.Length && l < array3.Length; l++)
				{
					nPCWaveCfg.subWaveTimes[l] = int.Parse(array6[l].Trim());
				}
				arrayList.Add(nPCWaveCfg);
			}
			for (int m = 1; m < 11; m++)
			{
				int num3 = 50;
				for (int n = 1; n < num3 + 1; n++)
				{
					NPCWaveCfg nPCWaveCfg2 = new NPCWaveCfg();
					for (int num4 = 0; num4 < arrayList.Count; num4++)
					{
						NPCWaveCfg nPCWaveCfg3 = (NPCWaveCfg)arrayList[num4];
						if (nPCWaveCfg3.mapIndex == num && nPCWaveCfg3.pointsIndex == 1 && nPCWaveCfg3.waveIndex == n)
						{
							nPCWaveCfg2 = nPCWaveCfg3;
							break;
						}
					}
					if (nPCWaveCfg2 == null)
					{
						continue;
					}
					NPCWaveCfg nPCWaveCfg4 = new NPCWaveCfg();
					nPCWaveCfg4.mapIndex = num;
					nPCWaveCfg4.pointsIndex = m;
					nPCWaveCfg4.waveIndex = n;
					nPCWaveCfg4.enemyInfo = new ArrayList();
					nPCWaveCfg4.enemyInfo = new ArrayList();
					for (int num5 = 0; num5 < nPCWaveCfg2.enemyInfo.Count; num5++)
					{
						Hashtable hashtable2 = new Hashtable();
						Hashtable hashtable3 = (Hashtable)nPCWaveCfg2.enemyInfo[num5];
						foreach (string key in hashtable3.Keys)
						{
							int num6 = int.Parse(hashtable3[key].ToString());
							num6 = num6;
							hashtable2[key] = num6.ToString();
						}
						nPCWaveCfg4.enemyInfo.Add(hashtable2);
					}
					nPCWaveCfg4.subWaveTimes = new int[nPCWaveCfg2.subWaveTimes.Length];
					nPCWaveCfg2.subWaveTimes.CopyTo(nPCWaveCfg4.subWaveTimes, 0);
					waves.Add(nPCWaveCfg4);
				}
			}
		}

		public void LoadMap6Config(string text)
		{
			int num = 6;
			string[] array = text.Split('\r', '\n');
			ArrayList arrayList = new ArrayList();
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] == null || array[i].Length < 2)
				{
					continue;
				}
				NPCWaveCfg nPCWaveCfg = new NPCWaveCfg();
				string text2 = array[i].Trim();
				string[] array2 = text2.Split('\t');
				if (array2 == null || array2.Length < 2)
				{
					continue;
				}
				nPCWaveCfg.mapIndex = int.Parse(array2[0].Trim());
				if (nPCWaveCfg.mapIndex != num)
				{
					continue;
				}
				nPCWaveCfg.pointsIndex = int.Parse(array2[1].Trim());
				nPCWaveCfg.waveIndex = int.Parse(array2[2].Trim());
				nPCWaveCfg.enemyInfo = new ArrayList();
				string[] array3 = array2[3].Split(';');
				for (int j = 0; j < array3.Length; j++)
				{
					Hashtable hashtable = new Hashtable();
					string[] array4 = array3[j].Split(',');
					for (int k = 0; k < array4.Length; k++)
					{
						string[] array5 = array4[k].Trim().Split('-');
						if (array5.Length != 2)
						{
							Debug.LogError("Config ERROE!!! - waves.txt, subwave enemy type and number not a pair! - " + nPCWaveCfg.mapIndex + "-" + nPCWaveCfg.pointsIndex + "-" + nPCWaveCfg.waveIndex);
						}
						else
						{
							hashtable[array5[0].Trim()] = array5[1].Trim();
						}
					}
					nPCWaveCfg.enemyInfo.Add(hashtable);
				}
				string[] array6 = array2[4].Split(';');
				if (array3.Length != array6.Length)
				{
					Debug.Log("ERROR: - " + nPCWaveCfg.mapIndex + "|" + nPCWaveCfg.pointsIndex + "|" + nPCWaveCfg.waveIndex);
				}
				int num2 = Mathf.Min(array6.Length, array3.Length);
				nPCWaveCfg.subWaveTimes = new int[num2];
				for (int l = 0; l < array6.Length && l < array3.Length; l++)
				{
					nPCWaveCfg.subWaveTimes[l] = int.Parse(array6[l].Trim());
				}
				int maxPointsOfMap = GetMaxPointsOfMap(num);
				int waveIndex = nPCWaveCfg.waveIndex;
				if (waveIndex <= maxPointsOfMap)
				{
					NPCWaveCfg nPCWaveCfg2 = new NPCWaveCfg();
					nPCWaveCfg2.mapIndex = num;
					nPCWaveCfg2.pointsIndex = nPCWaveCfg.waveIndex;
					int maxWavesOfPoints = GetMaxWavesOfPoints(num, nPCWaveCfg2.pointsIndex);
					nPCWaveCfg2.waveIndex = maxWavesOfPoints;
					nPCWaveCfg2.enemyInfo = new ArrayList();
					string[] array7 = array2[5].Split(';');
					for (int m = 0; m < array7.Length; m++)
					{
						Hashtable hashtable2 = new Hashtable();
						string[] array8 = array7[m].Split(',');
						for (int n = 0; n < array8.Length; n++)
						{
							string[] array9 = array8[n].Trim().Split('-');
							if (array9.Length != 2)
							{
								Debug.LogError("Config ERROE!!! - waves.txt, subwave enemy type and number not a pair! - " + nPCWaveCfg2.mapIndex + "-" + nPCWaveCfg2.pointsIndex + "-" + nPCWaveCfg2.waveIndex);
							}
							else
							{
								hashtable2[array9[0].Trim()] = array9[1].Trim();
							}
						}
						nPCWaveCfg2.enemyInfo.Add(hashtable2);
					}
					string[] array10 = array2[6].Split(';');
					if (array7.Length != array10.Length)
					{
						Debug.Log("ERROR: - " + nPCWaveCfg.mapIndex + "|" + nPCWaveCfg.pointsIndex + "|" + nPCWaveCfg.waveIndex);
					}
					nPCWaveCfg2.subWaveTimes = new int[array10.Length];
					for (int num3 = 0; num3 < array10.Length && num3 < array7.Length; num3++)
					{
						nPCWaveCfg2.subWaveTimes[num3] = int.Parse(array10[num3].Trim());
					}
					waves.Add(nPCWaveCfg2);
				}
				arrayList.Add(nPCWaveCfg);
			}
			int maxPointsOfMap2 = GetMaxPointsOfMap(num);
			for (int num4 = 1; num4 < maxPointsOfMap2 + 1; num4++)
			{
				int maxWavesOfPoints2 = GetMaxWavesOfPoints(num, num4);
				for (int num5 = 1; num5 < maxWavesOfPoints2; num5++)
				{
					NPCWaveCfg nPCWaveCfg3 = new NPCWaveCfg();
					for (int num6 = 0; num6 < arrayList.Count; num6++)
					{
						NPCWaveCfg nPCWaveCfg4 = (NPCWaveCfg)arrayList[num6];
						if (nPCWaveCfg4.mapIndex == num && nPCWaveCfg4.pointsIndex == 1 && nPCWaveCfg4.waveIndex == num4 + num5 - 1)
						{
							nPCWaveCfg3 = nPCWaveCfg4;
							break;
						}
					}
					if (nPCWaveCfg3 == null)
					{
						continue;
					}
					NPCWaveCfg nPCWaveCfg5 = new NPCWaveCfg();
					nPCWaveCfg5.mapIndex = num;
					nPCWaveCfg5.pointsIndex = num4;
					nPCWaveCfg5.waveIndex = num5;
					nPCWaveCfg5.enemyInfo = new ArrayList();
					for (int num7 = 0; num7 < nPCWaveCfg3.enemyInfo.Count; num7++)
					{
						Hashtable hashtable3 = new Hashtable();
						Hashtable hashtable4 = (Hashtable)nPCWaveCfg3.enemyInfo[num7];
						foreach (string key in hashtable4.Keys)
						{
							int num8 = int.Parse(hashtable4[key].ToString());
							hashtable3[key] = Mathf.FloorToInt((float)num8 * (1f + (float)(num4 - 1) * 0.024f)).ToString();
						}
						nPCWaveCfg5.enemyInfo.Add(hashtable3);
					}
					nPCWaveCfg5.subWaveTimes = new int[nPCWaveCfg3.subWaveTimes.Length];
					nPCWaveCfg3.subWaveTimes.CopyTo(nPCWaveCfg5.subWaveTimes, 0);
					waves.Add(nPCWaveCfg5);
				}
			}
		}

		public void LoadMap7Config(string text)
		{
			int num = 7;
			string[] array = text.Split('\r', '\n');
			ArrayList arrayList = new ArrayList();
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] == null || array[i].Length < 2)
				{
					continue;
				}
				NPCWaveCfg nPCWaveCfg = new NPCWaveCfg();
				string text2 = array[i].Trim();
				string[] array2 = text2.Split('\t');
				if (array2 == null || array2.Length < 2)
				{
					continue;
				}
				nPCWaveCfg.mapIndex = int.Parse(array2[0].Trim());
				if (nPCWaveCfg.mapIndex != num)
				{
					continue;
				}
				nPCWaveCfg.pointsIndex = int.Parse(array2[1].Trim());
				nPCWaveCfg.waveIndex = int.Parse(array2[2].Trim());
				nPCWaveCfg.enemyInfo = new ArrayList();
				string[] array3 = array2[3].Split(';');
				for (int j = 0; j < array3.Length; j++)
				{
					Hashtable hashtable = new Hashtable();
					string[] array4 = array3[j].Split(',');
					for (int k = 0; k < array4.Length; k++)
					{
						string[] array5 = array4[k].Trim().Split('-');
						if (array5.Length != 2)
						{
							Debug.LogError("Config ERROE!!! - waves.txt, subwave enemy type and number not a pair! - " + nPCWaveCfg.mapIndex + "-" + nPCWaveCfg.pointsIndex + "-" + nPCWaveCfg.waveIndex);
						}
						else
						{
							hashtable[array5[0].Trim()] = array5[1].Trim();
						}
					}
					nPCWaveCfg.enemyInfo.Add(hashtable);
				}
				string[] array6 = array2[4].Split(';');
				if (array3.Length != array6.Length)
				{
					Debug.Log("ERROR: - " + nPCWaveCfg.mapIndex + "|" + nPCWaveCfg.pointsIndex + "|" + nPCWaveCfg.waveIndex);
				}
				int num2 = Mathf.Min(array6.Length, array3.Length);
				nPCWaveCfg.subWaveTimes = new int[num2];
				for (int l = 0; l < array6.Length && l < array3.Length; l++)
				{
					nPCWaveCfg.subWaveTimes[l] = int.Parse(array6[l].Trim());
				}
				int maxPointsOfMap = GetMaxPointsOfMap(num);
				int waveIndex = nPCWaveCfg.waveIndex;
				if (waveIndex <= maxPointsOfMap)
				{
					NPCWaveCfg nPCWaveCfg2 = new NPCWaveCfg();
					nPCWaveCfg2.mapIndex = num;
					nPCWaveCfg2.pointsIndex = nPCWaveCfg.waveIndex;
					int maxWavesOfPoints = GetMaxWavesOfPoints(num, nPCWaveCfg2.pointsIndex);
					nPCWaveCfg2.waveIndex = maxWavesOfPoints;
					nPCWaveCfg2.enemyInfo = new ArrayList();
					string[] array7 = array2[5].Split(';');
					for (int m = 0; m < array7.Length; m++)
					{
						Hashtable hashtable2 = new Hashtable();
						string[] array8 = array7[m].Split(',');
						for (int n = 0; n < array8.Length; n++)
						{
							string[] array9 = array8[n].Trim().Split('-');
							if (array9.Length != 2)
							{
								Debug.LogError("Config ERROE!!! - waves.txt, subwave enemy type and number not a pair! - " + nPCWaveCfg2.mapIndex + "-" + nPCWaveCfg2.pointsIndex + "-" + nPCWaveCfg2.waveIndex);
							}
							else
							{
								hashtable2[array9[0].Trim()] = array9[1].Trim();
							}
						}
						nPCWaveCfg2.enemyInfo.Add(hashtable2);
					}
					string[] array10 = array2[6].Split(';');
					if (array7.Length != array10.Length)
					{
						Debug.Log("ERROR: - " + nPCWaveCfg.mapIndex + "|" + nPCWaveCfg.pointsIndex + "|" + nPCWaveCfg.waveIndex);
					}
					nPCWaveCfg2.subWaveTimes = new int[array10.Length];
					for (int num3 = 0; num3 < array10.Length && num3 < array7.Length; num3++)
					{
						nPCWaveCfg2.subWaveTimes[num3] = int.Parse(array10[num3].Trim());
					}
					waves.Add(nPCWaveCfg2);
				}
				arrayList.Add(nPCWaveCfg);
			}
			int maxPointsOfMap2 = GetMaxPointsOfMap(num);
			for (int num4 = 1; num4 < maxPointsOfMap2 + 1; num4++)
			{
				int maxWavesOfPoints2 = GetMaxWavesOfPoints(num, num4);
				for (int num5 = 1; num5 < maxWavesOfPoints2; num5++)
				{
					NPCWaveCfg nPCWaveCfg3 = new NPCWaveCfg();
					for (int num6 = 0; num6 < arrayList.Count; num6++)
					{
						NPCWaveCfg nPCWaveCfg4 = (NPCWaveCfg)arrayList[num6];
						if (nPCWaveCfg4.mapIndex == num && nPCWaveCfg4.pointsIndex == 1 && nPCWaveCfg4.waveIndex == num4 + num5 - 1)
						{
							nPCWaveCfg3 = nPCWaveCfg4;
							break;
						}
					}
					if (nPCWaveCfg3 == null)
					{
						continue;
					}
					NPCWaveCfg nPCWaveCfg5 = new NPCWaveCfg();
					nPCWaveCfg5.mapIndex = num;
					nPCWaveCfg5.pointsIndex = num4;
					nPCWaveCfg5.waveIndex = num5;
					nPCWaveCfg5.enemyInfo = new ArrayList();
					try
					{
						for (int num7 = 0; num7 < nPCWaveCfg3.enemyInfo.Count; num7++)
						{
							Hashtable hashtable3 = new Hashtable();
							Hashtable hashtable4 = (Hashtable)nPCWaveCfg3.enemyInfo[num7];
							foreach (string key in hashtable4.Keys)
							{
								int num8 = int.Parse(hashtable4[key].ToString());
								hashtable3[key] = Mathf.FloorToInt((float)num8 * (1f + (float)(num4 - 1) * 0.024f)).ToString();
							}
							nPCWaveCfg5.enemyInfo.Add(hashtable3);
						}
						nPCWaveCfg5.subWaveTimes = new int[nPCWaveCfg3.subWaveTimes.Length];
						nPCWaveCfg3.subWaveTimes.CopyTo(nPCWaveCfg5.subWaveTimes, 0);
						waves.Add(nPCWaveCfg5);
					}
					catch
					{
					}
				}
			}
		}

		public int GetMaxPointsOfMap(int mapIndex)
		{
			int result = 70;
			switch (mapIndex)
			{
			case 3:
				result = 5;
				break;
			case 4:
				result = 5;
				break;
			case 5:
				result = 5;
				break;
			}
			return result;
		}

		public int GetMaxWavesOfPoints(int mapIndex, int pointsIndex = 1)
		{
			int result = 5;
			switch (mapIndex)
			{
			case 1:
				result = 3;
				break;
			case 2:
				result = 3;
				break;
			case 6:
				result = 3;
				break;
			case 7:
				result = 7;
				break;
			}
			return result;
		}

		public NPCWaveCfg GetNPCWaveConfig(int mapIndex, int pointsIndex, int waveIndex)
		{
			for (int i = 0; i < waves.Count; i++)
			{
				NPCWaveCfg nPCWaveCfg = (NPCWaveCfg)waves[i];
				if (nPCWaveCfg.mapIndex == mapIndex && nPCWaveCfg.pointsIndex == pointsIndex && nPCWaveCfg.waveIndex == waveIndex)
				{
					return nPCWaveCfg;
				}
			}
			return null;
		}

		public void LoadWeaponConfigData()
		{
			LoadDefaultWeaponConfigData();
		}

		public void SetDefaultWeaponConfigData()
		{
			weapons = new ArrayList();
			WeaponCfg weaponCfg = new WeaponCfg();
			weaponCfg.type = 1;
			weaponCfg.name = "Beretta 33";
			weaponCfg.mClass = 1;
			weaponCfg.dmg = 1;
			weaponCfg.rpm = 1f;
			weaponCfg.spd = 0.1f;
			weaponCfg.levelLimit = 0;
			weaponCfg.priceType = "gold";
			weaponCfg.price = 1000;
			weapons.Add(weaponCfg);
			weaponCfg = new WeaponCfg();
			weaponCfg.type = 2;
			weaponCfg.name = "GrewCar_15";
			weaponCfg.mClass = 1;
			weaponCfg.dmg = 1;
			weaponCfg.rpm = 1f;
			weaponCfg.spd = 0.1f;
			weaponCfg.levelLimit = 1;
			weaponCfg.priceType = "gold";
			weaponCfg.price = 1000;
			weapons.Add(weaponCfg);
			weaponCfg = new WeaponCfg();
			weaponCfg.type = 3;
			weaponCfg.name = "UZI_E";
			weaponCfg.mClass = 1;
			weaponCfg.dmg = 1;
			weaponCfg.rpm = 1f;
			weaponCfg.spd = 0.1f;
			weaponCfg.levelLimit = 1;
			weaponCfg.priceType = "gold";
			weaponCfg.price = 1000;
			weapons.Add(weaponCfg);
			weaponCfg = new WeaponCfg();
			weaponCfg.type = 4;
			weaponCfg.name = "RemingtonPipe";
			weaponCfg.mClass = 1;
			weaponCfg.dmg = 1;
			weaponCfg.rpm = 1f;
			weaponCfg.spd = 0.1f;
			weaponCfg.levelLimit = 1;
			weaponCfg.priceType = "gold";
			weaponCfg.price = 1000;
			weapons.Add(weaponCfg);
			weaponCfg = new WeaponCfg();
			weaponCfg.type = 5;
			weaponCfg.name = "Springfield_9mm";
			weaponCfg.mClass = 1;
			weaponCfg.dmg = 1;
			weaponCfg.rpm = 1f;
			weaponCfg.spd = 0.1f;
			weaponCfg.levelLimit = 1;
			weaponCfg.priceType = "gold";
			weaponCfg.price = 1000;
			weapons.Add(weaponCfg);
			weaponCfg = new WeaponCfg();
			weaponCfg.type = 6;
			weaponCfg.name = "Kalashnikov_II";
			weaponCfg.mClass = 1;
			weaponCfg.dmg = 1;
			weaponCfg.rpm = 1f;
			weaponCfg.spd = 0.1f;
			weaponCfg.levelLimit = 1;
			weaponCfg.priceType = "gold";
			weaponCfg.price = 1000;
			weapons.Add(weaponCfg);
			weaponCfg = new WeaponCfg();
			weaponCfg.type = 7;
			weaponCfg.name = "Barrett_P90";
			weaponCfg.mClass = 1;
			weaponCfg.dmg = 1;
			weaponCfg.rpm = 1f;
			weaponCfg.spd = 0.1f;
			weaponCfg.levelLimit = 1;
			weaponCfg.priceType = "gold";
			weaponCfg.price = 1000;
			weapons.Add(weaponCfg);
			weaponCfg = new WeaponCfg();
			weaponCfg.type = 8;
			weaponCfg.name = "ParkerGaussRifle";
			weaponCfg.mClass = 1;
			weaponCfg.dmg = 1;
			weaponCfg.rpm = 1f;
			weaponCfg.spd = 0.1f;
			weaponCfg.levelLimit = 1;
			weaponCfg.priceType = "gold";
			weaponCfg.price = 1000;
			weapons.Add(weaponCfg);
			weaponCfg = new WeaponCfg();
			weaponCfg.type = 9;
			weaponCfg.name = "ZombieBusters";
			weaponCfg.mClass = 1;
			weaponCfg.dmg = 1;
			weaponCfg.rpm = 1f;
			weaponCfg.spd = 0.1f;
			weaponCfg.levelLimit = 1;
			weaponCfg.priceType = "gold";
			weaponCfg.price = 1000;
			weapons.Add(weaponCfg);
			weaponCfg = new WeaponCfg();
			weaponCfg.type = 10;
			weaponCfg.name = "SimonovPistol";
			weaponCfg.mClass = 1;
			weaponCfg.dmg = 1;
			weaponCfg.rpm = 1f;
			weaponCfg.spd = 0.1f;
			weaponCfg.levelLimit = 1;
			weaponCfg.priceType = "gold";
			weaponCfg.price = 1000;
			weapons.Add(weaponCfg);
			weaponCfg = new WeaponCfg();
			weaponCfg.type = 11;
			weaponCfg.name = "BarrettSplitIII";
			weaponCfg.mClass = 1;
			weaponCfg.dmg = 1;
			weaponCfg.rpm = 1f;
			weaponCfg.spd = 0.1f;
			weaponCfg.levelLimit = 1;
			weaponCfg.priceType = "gold";
			weaponCfg.price = 1000;
			weapons.Add(weaponCfg);
			weaponCfg = new WeaponCfg();
			weaponCfg.type = 12;
			weaponCfg.name = "Tomahawk";
			weaponCfg.mClass = 1;
			weaponCfg.dmg = 1;
			weaponCfg.rpm = 1f;
			weaponCfg.spd = 0.1f;
			weaponCfg.levelLimit = 1;
			weaponCfg.priceType = "gold";
			weaponCfg.price = 1000;
			weapons.Add(weaponCfg);
			weaponCfg = new WeaponCfg();
			weaponCfg.type = 13;
			weaponCfg.name = "SimonoRayRifle";
			weaponCfg.mClass = 1;
			weaponCfg.dmg = 1;
			weaponCfg.rpm = 1f;
			weaponCfg.spd = 0.1f;
			weaponCfg.levelLimit = 1;
			weaponCfg.priceType = "gold";
			weaponCfg.price = 1000;
			weapons.Add(weaponCfg);
			weaponCfg = new WeaponCfg();
			weaponCfg.type = 14;
			weaponCfg.name = "Volcano";
			weaponCfg.mClass = 1;
			weaponCfg.dmg = 1;
			weaponCfg.rpm = 1f;
			weaponCfg.spd = 0.1f;
			weaponCfg.levelLimit = 1;
			weaponCfg.priceType = "gold";
			weaponCfg.price = 1000;
			weapons.Add(weaponCfg);
			weaponCfg = new WeaponCfg();
			weaponCfg.type = 15;
			weaponCfg.name = "Hellfire";
			weaponCfg.mClass = 1;
			weaponCfg.dmg = 1;
			weaponCfg.rpm = 1f;
			weaponCfg.spd = 0.1f;
			weaponCfg.levelLimit = 1;
			weaponCfg.priceType = "gold";
			weaponCfg.price = 1000;
			weapons.Add(weaponCfg);
			weaponCfg = new WeaponCfg();
			weaponCfg.type = 16;
			weaponCfg.name = "Nailer";
			weaponCfg.mClass = 1;
			weaponCfg.dmg = 1;
			weaponCfg.rpm = 1f;
			weaponCfg.spd = 0.1f;
			weaponCfg.levelLimit = 1;
			weaponCfg.priceType = "gold";
			weaponCfg.price = 1000;
			weapons.Add(weaponCfg);
			weaponCfg = new WeaponCfg();
			weaponCfg.type = 17;
			weaponCfg.name = "NeutronRifle";
			weaponCfg.mClass = 1;
			weaponCfg.dmg = 1;
			weaponCfg.rpm = 1f;
			weaponCfg.spd = 0.1f;
			weaponCfg.levelLimit = 1;
			weaponCfg.priceType = "gold";
			weaponCfg.price = 1000;
			weapons.Add(weaponCfg);
			weaponCfg = new WeaponCfg();
			weaponCfg.type = 18;
			weaponCfg.name = "BigFirework";
			weaponCfg.mClass = 1;
			weaponCfg.dmg = 1;
			weaponCfg.rpm = 1f;
			weaponCfg.spd = 0.1f;
			weaponCfg.levelLimit = 1;
			weaponCfg.priceType = "gold";
			weaponCfg.price = 1000;
			weapons.Add(weaponCfg);
			weaponCfg = new WeaponCfg();
			weaponCfg.type = 19;
			weaponCfg.name = "Stormgun";
			weaponCfg.mClass = 1;
			weaponCfg.dmg = 1;
			weaponCfg.rpm = 1f;
			weaponCfg.spd = 0.1f;
			weaponCfg.levelLimit = 1;
			weaponCfg.priceType = "gold";
			weaponCfg.price = 1000;
			weapons.Add(weaponCfg);
			weaponCfg = new WeaponCfg();
			weaponCfg.type = 20;
			weaponCfg.name = "Lightning";
			weaponCfg.mClass = 1;
			weaponCfg.dmg = 1;
			weaponCfg.rpm = 1f;
			weaponCfg.spd = 0.1f;
			weaponCfg.levelLimit = 1;
			weaponCfg.priceType = "gold";
			weaponCfg.price = 1000;
			weapons.Add(weaponCfg);
			weaponCfg = new WeaponCfg();
			weaponCfg.type = 21;
			weaponCfg.name = "MassacreCannon";
			weaponCfg.mClass = 1;
			weaponCfg.dmg = 1;
			weaponCfg.rpm = 1f;
			weaponCfg.spd = 0.1f;
			weaponCfg.levelLimit = 1;
			weaponCfg.priceType = "gold";
			weaponCfg.price = 1000;
			weapons.Add(weaponCfg);
		}

		public WeaponCfg GetWeaponCfg(int index)
		{
			return (WeaponCfg)weapons[index];
		}

		public WeaponCfg GetWeaponCfg(WeaponType weapon_type)
		{
			for (int i = 0; i < weapons.Count; i++)
			{
				WeaponCfg weaponCfg = (WeaponCfg)weapons[i];
				if (weaponCfg.type == (int)weapon_type)
				{
					return weaponCfg;
				}
			}
			return null;
		}

		public void LoadDefaultWeaponConfigData()
		{
			weapons = new ArrayList();
			TextAsset textAsset = (TextAsset)Resources.Load("Zombie3D/Config/WeaponConfig", typeof(TextAsset));
			if (textAsset == null)
			{
				Debug.Log("ConfigManager.LoadFixedConfig.LoadDefaultWeaponConfigData ERROR!!! WeaponConfig.txt file not exist!!");
				return;
			}
			string text = textAsset.text;
			string[] array = text.Split('\r', '\n');
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] != null && array[i].Trim().Length != 0)
				{
					string[] array2 = array[i].Split('\t');
					if (array2.Length >= 9)
					{
						WeaponCfg weaponCfg = new WeaponCfg();
						weaponCfg.type = int.Parse(array2[0].Trim());
						weaponCfg.name = array2[1].Trim();
						weaponCfg.mClass = int.Parse(array2[2].Trim());
						weaponCfg.dmg = int.Parse(array2[3].Trim());
						weaponCfg.rpm = float.Parse(array2[4].Trim());
						weaponCfg.spd = float.Parse(array2[5].Trim());
						weaponCfg.levelLimit = int.Parse(array2[6].Trim());
						weaponCfg.priceType = array2[7].Trim();
						weaponCfg.price = int.Parse(array2[8].Trim());
						weaponCfg.introduction = array2[9].Trim();
						weaponCfg.bNewWeapon = int.Parse(array2[10].Trim()) == 1;
						weapons.Add(weaponCfg);
					}
				}
			}
		}

		public EnemyCfg GetEnemyCfg(int enemy_type)
		{
			return (EnemyCfg)enemyCfgs[enemy_type];
		}

		public void LoadEnemyConfigData()
		{
			enemyCfgs = new Hashtable();
			TextAsset textAsset = (TextAsset)Resources.Load("Zombie3D/Config/EnemyConfig", typeof(TextAsset));
			if (textAsset == null)
			{
				Debug.Log("ConfigManager.LoadFixedConfig.LoadEnemyConfigDate ERROR!!! EnemyConfig.txt file not exist!!");
				return;
			}
			string text = textAsset.text;
			string[] array = text.Split('\r', '\n');
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] != null && array[i].Trim().Length != 0)
				{
					string[] array2 = array[i].Split('\t');
					if (array2.Length >= 9)
					{
						EnemyCfg enemyCfg = new EnemyCfg();
						enemyCfg.type = int.Parse(array2[0].Trim());
						enemyCfg.name = array2[1].Trim();
						enemyCfg.hp = int.Parse(array2[2].Trim());
						enemyCfg.attack = float.Parse(array2[3].Trim());
						enemyCfg.walkSpeed = float.Parse(array2[4].Trim());
						enemyCfg.attackFrequency = float.Parse(array2[5].Trim());
						enemyCfg.attackRange = float.Parse(array2[6].Trim());
						enemyCfg.lootCash = int.Parse(array2[7].Trim());
						enemyCfg.lootExp = int.Parse(array2[8].Trim());
						enemyCfgs[enemyCfg.type] = enemyCfg;
					}
				}
			}
		}

		public PowerUPSCfg GetPowerUPSCfg(int index)
		{
			return (PowerUPSCfg)powerUpsCfgs[index];
		}

		public PowerUPSCfg GetPowerUPSCfg(ItemType type)
		{
			for (int i = 0; i < powerUpsCfgs.Count; i++)
			{
				PowerUPSCfg powerUPSCfg = (PowerUPSCfg)powerUpsCfgs[i];
				if (powerUPSCfg.type == type)
				{
					return powerUPSCfg;
				}
			}
			return null;
		}

		public void LoadPowerUPSConfigData()
		{
			powerUpsCfgs = new ArrayList();
			TextAsset textAsset = (TextAsset)Resources.Load("Zombie3D/Config/PowerUPSConfig", typeof(TextAsset));
			if (textAsset == null)
			{
				Debug.Log("ConfigManager.LoadFixedConfig.LoadDefaultPowerUPSConfigData ERROR!!! PowerUPSConfig.txt file not exist!!");
				return;
			}
			string text = textAsset.text;
			string[] array = text.Split('\r', '\n');
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] != null && array[i].Trim().Length != 0)
				{
					string[] array2 = array[i].Split('\t');
					if (array2.Length >= 4)
					{
						PowerUPSCfg powerUPSCfg = new PowerUPSCfg();
						powerUPSCfg.type = (ItemType)(int.Parse(array2[0].Trim()) - 1);
						powerUPSCfg.name = array2[1].Trim();
						powerUPSCfg.priceType = array2[2].Trim();
						powerUPSCfg.price = int.Parse(array2[3].Trim());
						powerUPSCfg.introduction = array2[4].Trim();
						powerUPSCfg.stamina = int.Parse(array2[5].Trim());
						powerUPSCfg.hp = float.Parse(array2[6].Trim());
						powerUPSCfg.keepTime = float.Parse(array2[7].Trim());
						powerUPSCfg.damagePercent = float.Parse(array2[8].Trim());
						powerUPSCfg.damage = float.Parse(array2[9].Trim());
						powerUPSCfg.attackAdd = float.Parse(array2[10].Trim());
						powerUPSCfg.bNew = ((int.Parse(array2[11].Trim()) != 0) ? true : false);
						powerUPSCfg.staminaSpeedAdd = float.Parse(array2[12].Trim());
						powerUpsCfgs.Add(powerUPSCfg);
					}
				}
			}
		}

		public AvatarCfg GetAvatarCfg(int index)
		{
			return (AvatarCfg)avatarCfgs[index];
		}

		public AvatarCfg GetAvatarCfg(Avatar.AvatarSuiteType avatar_suite_type, Avatar.AvatarType avatar_type)
		{
			AvatarCfg result = null;
			for (int i = 0; i < avatarCfgs.Count; i++)
			{
				AvatarCfg avatarCfg = (AvatarCfg)avatarCfgs[i];
				if (avatarCfg.suiteType == avatar_suite_type && avatarCfg.avtType == avatar_type)
				{
					result = avatarCfg;
				}
			}
			return result;
		}

		public void LoadAvatarConfigData()
		{
			avatarCfgs = new ArrayList();
			TextAsset textAsset = (TextAsset)Resources.Load("Zombie3D/Config/AvatarConfig", typeof(TextAsset));
			if (textAsset == null)
			{
				Debug.Log("ConfigManager.LoadFixedConfig.LoadAvatarConfigData ERROR!!! AvatarConfig.txt file not exist!!");
				return;
			}
			string text = textAsset.text;
			string[] array = text.Split('\r', '\n');
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] != null && array[i].Trim().Length != 0)
				{
					string[] array2 = array[i].Split('\t');
					if (array2.Length >= 4)
					{
						AvatarCfg avatarCfg = new AvatarCfg();
						avatarCfg.suiteType = (Avatar.AvatarSuiteType)(int.Parse(array2[0].Trim()) - 1);
						avatarCfg.name = array2[1].Trim();
						avatarCfg.m_Class = int.Parse(array2[2].Trim());
						avatarCfg.prop = new Avatar.AvatarProperty();
						avatarCfg.prop.m_AttackAdditive = float.Parse(array2[3].Trim());
						avatarCfg.prop.m_DefenceAdditive = float.Parse(array2[4].Trim());
						avatarCfg.prop.m_SpeedAdditive = float.Parse(array2[5].Trim());
						avatarCfg.prop.m_HpAdditive = float.Parse(array2[6].Trim());
						avatarCfg.prop.m_AttackSpeedAdditive = float.Parse(array2[7].Trim());
						avatarCfg.prop.m_StaminaAdd = float.Parse(array2[8].Trim());
						avatarCfg.prop.m_ExpAdditive = float.Parse(array2[9].Trim());
						avatarCfg.prop.m_GoldAdditive = float.Parse(array2[10].Trim());
						avatarCfg.priceType = array2[11].Trim();
						avatarCfg.price = int.Parse(array2[12].Trim());
						avatarCfg.levelLimit = int.Parse(array2[13].Trim());
						avatarCfg.avtType = ((!(array2[14].Trim() == "head")) ? Avatar.AvatarType.Body : Avatar.AvatarType.Head);
						avatarCfg.introduction = array2[15].Trim();
						avatarCfg.bNewAvatar = int.Parse(array2[16].Trim()) == 1;
						avatarCfgs.Add(avatarCfg);
					}
				}
			}
		}

		public IAPCfg GetIAPCfg(int index)
		{
			return (IAPCfg)iapCfgs[index];
		}

		public void LoadIAPConfigData()
		{
			iapCfgs = new ArrayList();
			IAPCfg iAPCfg = new IAPCfg();
			iAPCfg.iapID = "com.trinitigame.callofminibulletdudes.new099cents1";
			iAPCfg.iapDollor = 0.99f;
			iAPCfg.gameGold = 32000f;
			iAPCfg.gameDollor = 30f;
			iAPCfg.introduction = "Get 30 tCrystals and 32K cash for cheap! (Limited to one purchase per user).";
			iapCfgs.Add(iAPCfg);
			iAPCfg = new IAPCfg();
			iAPCfg.iapID = "com.trinitigame.callofminibulletdudes.new4999cents2";
			iAPCfg.iapDollor = 49.99f;
			iAPCfg.gameGold = 0f;
			iAPCfg.gameDollor = 450f;
			iAPCfg.introduction = "450 tCrystals: The best value. Get any equipment you desire, personalize your character however you want, and dominate your enemies like never before.";
			iapCfgs.Add(iAPCfg);
			iAPCfg = new IAPCfg();
			iAPCfg.iapID = "com.trinitigame.callofminibulletdudes.new4999cents1";
			iAPCfg.iapDollor = 49.99f;
			iAPCfg.gameGold = 455000f;
			iAPCfg.gameDollor = 0f;
			iAPCfg.introduction = "455,000 Cash: An unbeatable value. Never worry about farming cash for equipment or consumables again.";
			iapCfgs.Add(iAPCfg);
			iAPCfg = new IAPCfg();
			iAPCfg.iapID = "com.trinitigame.callofminibulletdudes.new1999cents2";
			iAPCfg.iapDollor = 19.99f;
			iAPCfg.gameGold = 0f;
			iAPCfg.gameDollor = 160f;
			iAPCfg.introduction = "160 tCrystals: The most elite rare equipment in the game can be within your grasp for a super value.";
			iapCfgs.Add(iAPCfg);
			iAPCfg = new IAPCfg();
			iAPCfg.iapID = "com.trinitigame.callofminibulletdudes.new1999cents1";
			iAPCfg.iapDollor = 19.99f;
			iAPCfg.gameGold = 164000f;
			iAPCfg.gameDollor = 0f;
			iAPCfg.introduction = "164,000 Cash: This deal is a steal, and with this much money you can snatch up the strongest equipment and consumables like candy.";
			iapCfgs.Add(iAPCfg);
			iAPCfg = new IAPCfg();
			iAPCfg.iapID = "com.trinitigame.callofminibulletdudes.new999cents2";
			iAPCfg.iapDollor = 9.99f;
			iAPCfg.gameGold = 0f;
			iAPCfg.gameDollor = 70f;
			iAPCfg.introduction = "70 tCrystals: This super value pack will really open up your options of mid to high range rare equipment and leave some tCrystals to spare for powerful consumables on the side.";
			iapCfgs.Add(iAPCfg);
			iAPCfg = new IAPCfg();
			iAPCfg.iapID = "com.trinitigame.callofminibulletdudes.new999cents1";
			iAPCfg.iapDollor = 9.99f;
			iAPCfg.gameGold = 73000f;
			iAPCfg.gameDollor = 0f;
			iAPCfg.introduction = "73,000 Cash: Make yourself wealthy, you won't regret it at this value. Buy whatever you want in the store without breaking the bank.";
			iapCfgs.Add(iAPCfg);
			iAPCfg = new IAPCfg();
			iAPCfg.iapID = "com.trinitigame.callofminibulletdudes.new499cents2";
			iAPCfg.iapDollor = 4.99f;
			iAPCfg.gameGold = 0f;
			iAPCfg.gameDollor = 30f;
			iAPCfg.introduction = "30 tCrystals: With a pretty stack of crystals like this, you could shop to your heart's content and get a unique leg up on competition.";
			iapCfgs.Add(iAPCfg);
			iAPCfg = new IAPCfg();
			iAPCfg.iapID = "com.trinitigame.callofminibulletdudes.new499cents1";
			iAPCfg.iapDollor = 4.99f;
			iAPCfg.gameGold = 32000f;
			iAPCfg.gameDollor = 0f;
			iAPCfg.introduction = "32,000 Cash: The in-game store is your oyster; take your pick of consumables and mid-range equipment.";
			iapCfgs.Add(iAPCfg);
			iAPCfg = new IAPCfg();
			iAPCfg.iapID = "com.trinitigame.callofminibulletdudes.099cents2";
			iAPCfg.iapDollor = 0.99f;
			iAPCfg.gameGold = 0f;
			iAPCfg.gameDollor = 5f;
			iAPCfg.introduction = "5 tCrystals: Get your first taste of the rare and wonderful equipment available for VIP customers with crystal.";
			iapCfgs.Add(iAPCfg);
			iAPCfg = new IAPCfg();
			iAPCfg.iapID = "com.trinitigame.callofminibulletdudes.099cents1";
			iAPCfg.iapDollor = 0.99f;
			iAPCfg.gameGold = 5000f;
			iAPCfg.gameDollor = 0f;
			iAPCfg.introduction = "5,000 Cash: Get things rolling with a cash injection; buy that early level equipment and some consumables right away!";
			iapCfgs.Add(iAPCfg);
			m_bIapHasChanged = false;
		}

		public int FindIAPConfigData(string _IAPId)
		{
			int result = -1;
			for (int i = 0; i < iapCfgs.Count; i++)
			{
				if (GetIAPCfg(i).iapID == _IAPId)
				{
					result = i;
					break;
				}
			}
			return result;
		}

		public void RemoveIAPConfigData(int _index)
		{
			iapCfgs.RemoveAt(_index);
		}

		public void AddIAPConfigData(IAPCfg cfg, int _index = -1)
		{
			if (_index >= 0)
			{
				iapCfgs.Insert(_index, cfg);
			}
			else
			{
				iapCfgs.Add(cfg);
			}
			m_bIapHasChanged = true;
		}

		public void RestIAPConfig()
		{
			if (iapCfgs != null && m_bIapHasChanged)
			{
				iapCfgs.Clear();
				LoadIAPConfigData();
			}
		}

		public GameCenterArchievementsCfg GetGCArchievementCfg(string id)
		{
			for (int i = 0; i < GCArchievementCfgs.Count; i++)
			{
				GameCenterArchievementsCfg gameCenterArchievementsCfg = (GameCenterArchievementsCfg)GCArchievementCfgs[i];
				if (gameCenterArchievementsCfg.id == id)
				{
					return gameCenterArchievementsCfg;
				}
			}
			if (GCArchievementCfgs.Count > 0)
			{
				return (GameCenterArchievementsCfg)GCArchievementCfgs[0];
			}
			return null;
		}

		public void LoadGCArchievementData()
		{
			GCArchievementCfgs = new ArrayList();
			TextAsset textAsset = (TextAsset)Resources.Load("Zombie3D/Config/GCArchievementConfig", typeof(TextAsset));
			if (textAsset == null)
			{
				Debug.Log("ConfigManager.LoadFixedConfig.LoadGCArchievementData ERROR!!! GCArchievementConfig.txt file not exist!!");
				return;
			}
			string text = textAsset.text;
			string[] array = text.Split('\r', '\n');
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] != null && array[i].Trim().Length != 0)
				{
					string[] array2 = array[i].Split('\t');
					if (array2.Length >= 4)
					{
						GameCenterArchievementsCfg gameCenterArchievementsCfg = new GameCenterArchievementsCfg();
						gameCenterArchievementsCfg.id = array2[0];
						gameCenterArchievementsCfg.name = array2[1];
						gameCenterArchievementsCfg.introduction = array2[2];
						gameCenterArchievementsCfg.score = int.Parse(array2[3]);
						GCArchievementCfgs.Add(gameCenterArchievementsCfg);
					}
				}
			}
		}

		public string GetRankName(int player_level)
		{
			string result = string.Empty;
			if (player_level >= 3 && player_level < 10)
			{
				result = "Private E-1";
			}
			else if (player_level >= 10 && player_level < 20)
			{
				result = "Private E-2";
			}
			else if (player_level >= 20 && player_level < 30)
			{
				result = "Private First Class";
			}
			else if (player_level >= 30 && player_level < 40)
			{
				result = "Specialist";
			}
			else if (player_level >= 40 && player_level < 50)
			{
				result = "Sergeant";
			}
			else if (player_level >= 50 && player_level < 60)
			{
				result = "Staff Sergeant";
			}
			else if (player_level >= 60 && player_level < 70)
			{
				result = "Sergeant First Class";
			}
			else if (player_level >= 70 && player_level < 80)
			{
				result = "Master Sergeant";
			}
			else if (player_level >= 80 && player_level < 90)
			{
				result = "Sergeant Major";
			}
			else if (player_level >= 90 && player_level < 100)
			{
				result = "Command Sergeant Major";
			}
			else if (player_level >= 100 && player_level < 115)
			{
				result = "Warrant Officer";
			}
			else if (player_level >= 115 && player_level < 130)
			{
				result = "Chief Warrant Officer 2";
			}
			else if (player_level >= 130 && player_level < 145)
			{
				result = "Chief Warrant Officer 3";
			}
			else if (player_level >= 145 && player_level < 160)
			{
				result = "Chief Warrant Officer 4";
			}
			else if (player_level >= 160 && player_level < 175)
			{
				result = "Chief Warrant Officer 5";
			}
			else if (player_level >= 175 && player_level < 190)
			{
				result = "Second Lieutenant";
			}
			else if (player_level >= 190 && player_level < 205)
			{
				result = "First Lieutenant";
			}
			else if (player_level >= 205 && player_level < 230)
			{
				result = "Captain";
			}
			else if (player_level >= 230)
			{
				result = "Major";
			}
			return result;
		}

		public int GetNextRankLevel(int player_level)
		{
			int result = 3;
			if (player_level < 3)
			{
				result = 3;
			}
			else if (player_level >= 3 && player_level < 10)
			{
				result = 10;
			}
			else if (player_level >= 10 && player_level < 20)
			{
				result = 20;
			}
			else if (player_level >= 20 && player_level < 30)
			{
				result = 30;
			}
			else if (player_level >= 30 && player_level < 40)
			{
				result = 40;
			}
			else if (player_level >= 40 && player_level < 50)
			{
				result = 50;
			}
			else if (player_level >= 50 && player_level < 60)
			{
				result = 60;
			}
			else if (player_level >= 60 && player_level < 70)
			{
				result = 70;
			}
			else if (player_level >= 70 && player_level < 80)
			{
				result = 80;
			}
			else if (player_level >= 80 && player_level < 90)
			{
				result = 90;
			}
			else if (player_level >= 90 && player_level < 100)
			{
				result = 100;
			}
			else if (player_level >= 100 && player_level < 115)
			{
				result = 115;
			}
			else if (player_level >= 115 && player_level < 130)
			{
				result = 130;
			}
			else if (player_level >= 130 && player_level < 145)
			{
				result = 145;
			}
			else if (player_level >= 145 && player_level < 160)
			{
				result = 160;
			}
			else if (player_level >= 160 && player_level < 175)
			{
				result = 175;
			}
			else if (player_level >= 175 && player_level < 190)
			{
				result = 190;
			}
			else if (player_level >= 190 && player_level < 205)
			{
				result = 205;
			}
			else if (player_level >= 205 && player_level < 230)
			{
				result = 230;
			}
			else if (player_level >= 230)
			{
				result = 1000;
			}
			return result;
		}
	}
}
