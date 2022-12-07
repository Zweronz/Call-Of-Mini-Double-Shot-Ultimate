using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using LitJson;
using UnityEngine;
using Zombie3D;

public class GameCollectionInfo
{
	public enum enUIEnterIndex
	{
		ChoosePointsUI = 10,
		ShopUI_Weapon = 21,
		ShopUI_Avatar = 22,
		ShopUI_PowerUPS = 23,
		ShopUI_Bank = 24,
		BoostUI_Skill = 31,
		TeamUI_Friend = 41,
		TeamUI_MERC = 42,
		MERC_UI = 50,
		OptionUI = 60,
		BattleUI_Pause = 71,
		BattleUI_Item = 72,
		BattleUI_Dead = 73,
		ExchangeUI = 80
	}

	private int m_CurDays;

	private bool m_bCrackedMachine;

	private string m_FirstLoginTime = string.Empty;

	private GameState m_GameState;

	private int m_Level = 1;

	private int m_TotalExp;

	private int m_TodayExp;

	private int m_TotalGameTime;

	private int m_TodayGameTime;

	private int m_GameActiveTime;

	private int m_TotalGoldGet;

	private int m_CurGold;

	private int m_TodayGoldGet;

	private int m_TodayGoldLose;

	private int m_TotalTCrystalGet;

	private int m_CurTCrystal;

	private int m_TodayTCrystalGet;

	private int m_TodayTCrystalLose;

	private Hashtable m_TodayIapInfo = new Hashtable();

	private List<WeaponType> m_CurWeaponsHave = new List<WeaponType>();

	private List<int> m_TodayWeaponsBuy = new List<int>();

	private Hashtable m_CurAvatarHave = new Hashtable();

	private List<Avatar> m_TodayAvatarBuy = new List<Avatar>();

	private Hashtable m_CurPowerUPSHave = new Hashtable();

	private Hashtable m_TodayPowerUPSBuy = new Hashtable();

	private Hashtable m_TodayPowerUPSUsed = new Hashtable();

	private Hashtable m_UIEnterInfo = new Hashtable();

	private enUIEnterIndex m_UILastId;

	private int m_HireoutSelfTimes;

	private int m_HireOtherTimes;

	private Hashtable m_LastGamePointsInfo = new Hashtable();

	private Hashtable m_LH1Info = new Hashtable();

	private Hashtable m_LH2Info = new Hashtable();

	private Hashtable m_LH3Info = new Hashtable();

	private Hashtable m_CH1Info = new Hashtable();

	private Hashtable m_CH2Info = new Hashtable();

	private Hashtable m_CH3Info = new Hashtable();

	private Hashtable m_NP1Info = new Hashtable();

	private Hashtable m_NP2Info = new Hashtable();

	private Hashtable m_LH1PointsInfo = new Hashtable();

	private Hashtable m_LH2PointsInfo = new Hashtable();

	private Hashtable m_CH1PointsInfo = new Hashtable();

	private Hashtable m_CH2PointsInfo = new Hashtable();

	private Hashtable m_NP1PointsInfo = new Hashtable();

	private Hashtable m_NP2PointsInfo = new Hashtable();

	private int m_bIsBuyNoviceGiftBag;

	private Dictionary<string, int> m_NetPlayTimes = new Dictionary<string, int>();

	public GameCollectionInfo()
	{
		m_CurDays = new TimeSpan(DateTime.Now.Ticks).Days;
		m_bCrackedMachine = UtilsEx.OSIsJailBreak();
		m_GameState = GameApp.GetInstance().GetGameState();
		m_FirstLoginTime = m_GameState.FirstLoginTime;
		m_Level = m_GameState.Level;
		m_CurGold = m_GameState.gold;
		m_CurTCrystal = m_GameState.dollor;
		m_TotalExp = m_GameState.TotalExp;
		m_TotalGameTime = m_GameState.TotalGameTime;
		m_TotalGoldGet = m_GameState.TotalGoldGet;
		m_TotalTCrystalGet = m_GameState.TotalDollorGet;
		m_HireoutSelfTimes = 0;
		m_HireOtherTimes = 0;
		m_CurWeaponsHave = m_GameState.GetWeapons();
		m_bIsBuyNoviceGiftBag = m_GameState.NoviceGiftBagThird;
	}

	private void Reset()
	{
	}

	public void LoadFromFile(string file_path)
	{
		//Discarded unreachable code: IL_0096
		string text = string.Empty;
		StreamReader streamReader = null;
		try
		{
			streamReader = new StreamReader(file_path);
			text = streamReader.ReadToEnd();
		}
		catch
		{
			Debug.Log("ERROR - GameCollectionInfo LoadFromFile()!!! - " + file_path);
		}
		finally
		{
			if (streamReader != null)
			{
				streamReader.Close();
			}
		}
		if (text.Length <= 0)
		{
			return;
		}
		string text2 = text;
		try
		{
			byte[] data = Convert.FromBase64String(text);
			string playerDataEncryptKey = MiscPlugin.GetPlayerDataEncryptKey("ME_2_@_YOU_DD");
			byte[] bytes = XXTEAUtils.Decrypt(data, Encoding.ASCII.GetBytes(playerDataEncryptKey));
			text2 = Encoding.UTF8.GetString(bytes);
		}
		catch (Exception)
		{
			Debug.LogError("GameCollectionInfo.LoadFromFile() ERROR!!!");
			return;
		}
		if (!(text2 != string.Empty))
		{
			return;
		}
		string[] array = text2.Split('\r', '\n');
		for (int i = 0; i < array.Length; i++)
		{
			if (string.IsNullOrEmpty(array[i]))
			{
				continue;
			}
			try
			{
				string[] array2 = array[i].Split('\t');
				if (array2.Length < 2)
				{
					continue;
				}
				string text3 = array2[0];
				switch (text3)
				{
				case "Cracked":
				{
					string text4 = array2[1].Trim();
					m_bCrackedMachine = false;
					if (text4 == "True")
					{
						m_bCrackedMachine = true;
					}
					else if (text4 == "False")
					{
						m_bCrackedMachine = false;
					}
					else
					{
						m_bCrackedMachine = ((int.Parse(text4) != 0) ? true : false);
					}
					break;
				}
				case "Level":
					m_Level = int.Parse(array2[1]);
					break;
				case "Gold":
					m_CurGold = int.Parse(array2[1]);
					break;
				case "Crystal":
					m_CurTCrystal = int.Parse(array2[1]);
					break;
				case "FirstLoginTime":
					m_FirstLoginTime = array2[1];
					break;
				case "TotalExp":
					m_TotalExp = int.Parse(array2[1]);
					break;
				case "TotalGameTime":
					m_TotalGameTime = int.Parse(array2[1]);
					break;
				case "TotalGoldGet":
					m_TotalGoldGet = int.Parse(array2[1]);
					break;
				case "TotalDollorGet":
					m_TotalTCrystalGet = int.Parse(array2[1]);
					break;
				case "HireoutSelfTimes":
					m_HireoutSelfTimes = int.Parse(array2[1]);
					break;
				case "HireOtherTimes":
					m_HireOtherTimes = int.Parse(array2[1]);
					break;
				case "TodayExp":
					m_TodayExp = int.Parse(array2[1]);
					break;
				case "TodayGameTime":
					m_TodayGameTime = int.Parse(array2[1]);
					break;
				case "GameActiveTime":
					m_GameActiveTime = int.Parse(array2[1]);
					break;
				case "TodayGoldGet":
					m_TodayGoldGet = int.Parse(array2[1]);
					break;
				case "TodayGoldLose":
					m_TodayGoldLose = int.Parse(array2[1]);
					break;
				case "TodayTCrystalGet":
					m_TodayTCrystalGet = int.Parse(array2[1]);
					break;
				case "TodayTCrystalLose":
					m_TodayTCrystalLose = int.Parse(array2[1]);
					break;
				case "TodayIapInfo":
				{
					m_TodayIapInfo = new Hashtable();
					string[] array5 = array2[1].Split(';');
					for (int k = 0; k < array5.Length; k++)
					{
						string[] array6 = array5[k].Split(',');
						if (array6.Length > 1)
						{
							m_TodayIapInfo[array6[0]] = int.Parse(array6[1]);
						}
					}
					break;
				}
				case "WeaponList":
				{
					m_CurWeaponsHave = new List<WeaponType>();
					string[] array55 = array2[1].Split(';');
					for (int num55 = 0; num55 < array55.Length; num55++)
					{
						WeaponType item2 = (WeaponType)int.Parse(array55[num55]);
						m_CurWeaponsHave.Add(item2);
					}
					break;
				}
				case "TodayWeaponsBuy":
				{
					m_TodayWeaponsBuy = new List<int>();
					string[] array51 = array2[1].Split(';');
					for (int num51 = 0; num51 < array51.Length; num51++)
					{
						m_TodayWeaponsBuy.Add(int.Parse(array51[i]));
					}
					break;
				}
				case "CurAvatarHave":
				{
					m_CurAvatarHave = new Hashtable();
					string[] array49 = array2[1].Split(';');
					for (int num50 = 0; num50 < array49.Length; num50++)
					{
						string[] array50 = array49[num50].Split(',');
						if (array50.Length > 1)
						{
							Avatar key2 = new Avatar((Avatar.AvatarSuiteType)int.Parse(array50[0]), (Avatar.AvatarType)int.Parse(array50[1]));
							m_CurAvatarHave[key2] = false;
						}
					}
					break;
				}
				case "TodayAvatarBuy":
				{
					m_TodayAvatarBuy = new List<Avatar>();
					string[] array42 = array2[1].Split(';');
					for (int num43 = 0; num43 < array42.Length; num43++)
					{
						string[] array43 = array42[num43].Split(',');
						if (array43.Length > 1)
						{
							Avatar item = new Avatar((Avatar.AvatarSuiteType)int.Parse(array43[0]), (Avatar.AvatarType)int.Parse(array43[1]));
							m_TodayAvatarBuy.Add(item);
						}
					}
					break;
				}
				case "PowerUPSList":
				{
					m_CurPowerUPSHave = new Hashtable();
					string[] array44 = array2[1].Split(';');
					for (int num44 = 0; num44 < array44.Length; num44++)
					{
						string[] array45 = array44[num44].Split(',');
						if (array45.Length == 2)
						{
							int num45 = 1;
							int num46 = 1;
							try
							{
								num45 = int.Parse(array45[0]);
								num46 = int.Parse(array45[1]);
							}
							catch
							{
								Debug.Log("PowerUPSList Error!");
							}
							m_CurPowerUPSHave[num45] = num46;
						}
						else
						{
							Debug.LogError("ERROE: PowerUPS info wrong!!!");
						}
					}
					break;
				}
				case "TodayPowerUPSBuy":
				{
					m_TodayPowerUPSBuy = new Hashtable();
					string[] array40 = array2[1].Split(';');
					for (int num40 = 0; num40 < array40.Length; num40++)
					{
						string[] array41 = array40[num40].Split(',');
						if (array41.Length == 2)
						{
							int num41 = 1;
							int num42 = 1;
							try
							{
								num41 = int.Parse(array41[0]);
								num42 = int.Parse(array41[1]);
							}
							catch
							{
								Debug.Log("TodayPowerUPSBuy Error!");
							}
							m_TodayPowerUPSBuy[num41] = num42;
						}
						else
						{
							Debug.LogError("ERROE: PowerUPS info wrong!!!");
						}
					}
					break;
				}
				case "TodayPowerUPSUsed":
				{
					m_TodayPowerUPSUsed = new Hashtable();
					string[] array35 = array2[1].Split(';');
					for (int num34 = 0; num34 < array35.Length; num34++)
					{
						string[] array36 = array35[num34].Split(',');
						if (array36.Length == 2)
						{
							int num35 = 1;
							int num36 = 1;
							try
							{
								num35 = int.Parse(array36[0]);
								num36 = int.Parse(array36[1]);
							}
							catch
							{
								Debug.Log("TodayPowerUPSUsed Error!");
							}
							m_TodayPowerUPSUsed[num35] = num36;
						}
						else
						{
							Debug.LogError("ERROE: PowerUPS info wrong!!!");
						}
					}
					break;
				}
				case "UIEnterInfo":
				{
					m_UIEnterInfo = new Hashtable();
					string[] array33 = array2[1].Split(';');
					for (int num31 = 0; num31 < array33.Length; num31++)
					{
						string[] array34 = array33[num31].Split(',');
						if (array34.Length == 2)
						{
							int num32 = 1;
							int num33 = 1;
							try
							{
								num32 = int.Parse(array34[0]);
								num33 = int.Parse(array34[1]);
							}
							catch
							{
								Debug.Log("TodayPowerUPSUsed Error!");
							}
							m_UIEnterInfo[num32.ToString()] = num33;
						}
						else
						{
							Debug.LogError("ERROE: PowerUPS info wrong!!!");
						}
					}
					break;
				}
				case "UILastId":
					m_UILastId = (enUIEnterIndex)int.Parse(array2[1]);
					break;
				case "LH1Info":
				{
					m_LH1Info = new Hashtable();
					string[] array28 = array2[1].Split(';');
					for (int num25 = 0; num25 < array28.Length; num25++)
					{
						string[] array29 = array28[num25].Split(',');
						if (array29.Length == 2)
						{
							int num26 = 1;
							int num27 = 1;
							try
							{
								num26 = int.Parse(array29[0]);
								num27 = int.Parse(array29[1]);
							}
							catch
							{
								Debug.Log("LH1Info Error!");
							}
							m_LH1Info[num26] = num27;
						}
					}
					break;
				}
				case "LH2Info":
				{
					m_LH2Info = new Hashtable();
					string[] array26 = array2[1].Split(';');
					for (int num22 = 0; num22 < array26.Length; num22++)
					{
						string[] array27 = array26[num22].Split(',');
						if (array27.Length == 2)
						{
							int num23 = 1;
							int num24 = 1;
							try
							{
								num23 = int.Parse(array27[0]);
								num24 = int.Parse(array27[1]);
							}
							catch
							{
								Debug.Log("LH2Info Error!");
							}
							m_LH2Info[num23] = num24;
						}
					}
					break;
				}
				case "LH3Info":
				{
					m_LH3Info = new Hashtable();
					string[] array19 = array2[1].Split(';');
					for (int num13 = 0; num13 < array19.Length; num13++)
					{
						string[] array20 = array19[num13].Split(',');
						if (array20.Length == 2)
						{
							int num14 = 1;
							int num15 = 1;
							try
							{
								num14 = int.Parse(array20[0]);
								num15 = int.Parse(array20[1]);
							}
							catch
							{
								Debug.Log("LH3Info Error!");
							}
							m_LH3Info[num14] = num15;
						}
					}
					break;
				}
				case "CH1Info":
				{
					m_CH1Info = new Hashtable();
					string[] array21 = array2[1].Split(';');
					for (int num16 = 0; num16 < array21.Length; num16++)
					{
						string[] array22 = array21[num16].Split(',');
						if (array22.Length == 2)
						{
							int num17 = 1;
							int num18 = 1;
							try
							{
								num17 = int.Parse(array22[0]);
								num18 = int.Parse(array22[1]);
							}
							catch
							{
								Debug.Log("CH1Info Error!");
							}
							m_CH1Info[num17] = num18;
						}
					}
					break;
				}
				case "CH2Info":
				{
					m_CH2Info = new Hashtable();
					string[] array17 = array2[1].Split(';');
					for (int num10 = 0; num10 < array17.Length; num10++)
					{
						string[] array18 = array17[num10].Split(',');
						if (array18.Length == 2)
						{
							int num11 = 1;
							int num12 = 1;
							try
							{
								num11 = int.Parse(array18[0]);
								num12 = int.Parse(array18[1]);
							}
							catch
							{
								Debug.Log("CH2Info Error!");
							}
							m_CH2Info[num11] = num12;
						}
					}
					break;
				}
				case "CH3Info":
				{
					m_CH3Info = new Hashtable();
					string[] array11 = array2[1].Split(';');
					for (int n = 0; n < array11.Length; n++)
					{
						string[] array12 = array11[n].Split(',');
						if (array12.Length == 2)
						{
							int num5 = 1;
							int num6 = 1;
							try
							{
								num5 = int.Parse(array12[0]);
								num6 = int.Parse(array12[1]);
							}
							catch
							{
								Debug.Log("CH3Info Error!");
							}
							m_CH3Info[num5] = num6;
						}
					}
					break;
				}
				case "NP1Info":
				{
					m_NP1Info = new Hashtable();
					string[] array9 = array2[1].Split(';');
					for (int m = 0; m < array9.Length; m++)
					{
						string[] array10 = array9[m].Split(',');
						if (array10.Length == 2)
						{
							int num3 = 1;
							int num4 = 1;
							try
							{
								num3 = int.Parse(array10[0]);
								num4 = int.Parse(array10[1]);
							}
							catch
							{
								Debug.Log("NP1Info Error!");
							}
							m_NP1Info[num3] = num4;
						}
					}
					break;
				}
				case "NP2Info":
				{
					m_NP2Info = new Hashtable();
					string[] array7 = array2[1].Split(';');
					for (int l = 0; l < array7.Length; l++)
					{
						string[] array8 = array7[l].Split(',');
						if (array8.Length == 2)
						{
							int num = 1;
							int num2 = 1;
							try
							{
								num = int.Parse(array8[0]);
								num2 = int.Parse(array8[1]);
							}
							catch
							{
								Debug.Log("NP2Info Error!");
							}
							m_NP2Info[num] = num2;
						}
					}
					break;
				}
				case "LH1PointsInfo":
				{
					m_LH1PointsInfo = new Hashtable();
					string[] array52 = array2[1].Split('\t');
					if (array52.Length > 0)
					{
						m_LH1PointsInfo["maxPoints"] = int.Parse(array52[0]);
					}
					Hashtable hashtable6 = new Hashtable();
					if (array52.Length > 1)
					{
						string[] array53 = array52[1].Split(';');
						for (int num52 = 0; num52 < array53.Length; num52++)
						{
							string[] array54 = array53[num52].Split(',');
							if (array54.Length == 2)
							{
								int num53 = int.Parse(array54[0]);
								int num54 = int.Parse(array54[1]);
								hashtable6[num53] = num54;
							}
						}
					}
					m_LH1PointsInfo["PointsInfo"] = hashtable6;
					break;
				}
				case "LH2PointsInfo":
				{
					m_LH2PointsInfo = new Hashtable();
					string[] array46 = array2[1].Split('\t');
					if (array46.Length > 0)
					{
						m_LH2PointsInfo["maxPoints"] = int.Parse(array46[0]);
					}
					Hashtable hashtable5 = new Hashtable();
					if (array46.Length > 1)
					{
						string[] array47 = array46[1].Split(';');
						for (int num47 = 0; num47 < array47.Length; num47++)
						{
							string[] array48 = array47[num47].Split(',');
							if (array48.Length == 2)
							{
								int num48 = int.Parse(array48[0]);
								int num49 = int.Parse(array48[1]);
								hashtable5[num48] = num49;
							}
						}
					}
					m_LH2PointsInfo["PointsInfo"] = hashtable5;
					break;
				}
				case "CH1PointsInfo":
				{
					m_CH1PointsInfo = new Hashtable();
					string[] array37 = array2[1].Split('\t');
					if (array37.Length > 0)
					{
						m_CH1PointsInfo["maxPoints"] = int.Parse(array37[0]);
					}
					Hashtable hashtable4 = new Hashtable();
					if (array37.Length > 1)
					{
						string[] array38 = array37[1].Split(';');
						for (int num37 = 0; num37 < array38.Length; num37++)
						{
							string[] array39 = array38[num37].Split(',');
							if (array39.Length == 2)
							{
								int num38 = int.Parse(array39[0]);
								int num39 = int.Parse(array39[1]);
								hashtable4[num38] = num39;
							}
						}
					}
					m_CH1PointsInfo["PointsInfo"] = hashtable4;
					break;
				}
				case "Ch2PointsInfo":
				{
					m_CH2PointsInfo = new Hashtable();
					string[] array30 = array2[1].Split('\t');
					if (array30.Length > 0)
					{
						m_CH2PointsInfo["maxPoints"] = int.Parse(array30[0]);
					}
					Hashtable hashtable3 = new Hashtable();
					if (array30.Length > 1)
					{
						string[] array31 = array30[1].Split(';');
						for (int num28 = 0; num28 < array31.Length; num28++)
						{
							string[] array32 = array31[num28].Split(',');
							if (array32.Length == 2)
							{
								int num29 = int.Parse(array32[0]);
								int num30 = int.Parse(array32[1]);
								hashtable3[num29] = num30;
							}
						}
					}
					m_CH2PointsInfo["PointsInfo"] = hashtable3;
					break;
				}
				case "NP1PointsInfo":
				{
					m_NP1PointsInfo = new Hashtable();
					string[] array23 = array2[1].Split('\t');
					if (array23.Length > 0)
					{
						m_NP1PointsInfo["maxPoints"] = int.Parse(array23[0]);
					}
					Hashtable hashtable2 = new Hashtable();
					if (array23.Length > 1)
					{
						string[] array24 = array23[1].Split(';');
						for (int num19 = 0; num19 < array24.Length; num19++)
						{
							string[] array25 = array24[num19].Split(',');
							if (array25.Length == 2)
							{
								int num20 = int.Parse(array25[0]);
								int num21 = int.Parse(array25[1]);
								hashtable2[num20] = num21;
							}
						}
					}
					m_NP1PointsInfo["PointsInfo"] = hashtable2;
					break;
				}
				case "NP2PointsInfo":
				{
					m_NP2PointsInfo = new Hashtable();
					string[] array14 = array2[1].Split('\t');
					if (array14.Length > 0)
					{
						m_NP2PointsInfo["maxPoints"] = int.Parse(array14[0]);
					}
					Hashtable hashtable = new Hashtable();
					if (array14.Length > 1)
					{
						string[] array15 = array14[1].Split(';');
						for (int num7 = 0; num7 < array15.Length; num7++)
						{
							string[] array16 = array15[num7].Split(',');
							if (array16.Length == 2)
							{
								int num8 = int.Parse(array16[0]);
								int num9 = int.Parse(array16[1]);
								hashtable[num8] = num9;
							}
						}
					}
					m_NP2PointsInfo["PointsInfo"] = hashtable;
					break;
				}
				case "LastGamePointsInfo":
				{
					m_LastGamePointsInfo = new Hashtable();
					string[] array13 = array2[1].Split(',');
					if (array13.Length == 3)
					{
						m_LastGamePointsInfo["mapIndex"] = int.Parse(array13[0]);
						m_LastGamePointsInfo["pointIndex"] = int.Parse(array13[1]);
						m_LastGamePointsInfo["state"] = int.Parse(array13[2]);
					}
					break;
				}
				case "BuyNoviceGiftBag":
				{
					string text5 = array2[1].Trim();
					m_bIsBuyNoviceGiftBag = 0;
					if (text5 == "True")
					{
						m_bIsBuyNoviceGiftBag = 1;
					}
					else if (text5 == "False")
					{
						m_bIsBuyNoviceGiftBag = 0;
					}
					else
					{
						m_bIsBuyNoviceGiftBag = int.Parse(text5);
					}
					break;
				}
				case "NetPlayTimes":
				{
					if (m_NetPlayTimes == null)
					{
						m_NetPlayTimes = new Dictionary<string, int>();
					}
					else
					{
						m_NetPlayTimes.Clear();
					}
					string[] array3 = array2[1].Split(';');
					for (int j = 0; j < array3.Length; j++)
					{
						if (array3[j].Length >= 2)
						{
							string[] array4 = array3[j].Split(',');
							string key = array4[0];
							int value = int.Parse(array4[1]);
							m_NetPlayTimes.Add(key, value);
						}
					}
					break;
				}
				default:
					if (!(text3 == string.Empty))
					{
					}
					break;
				}
			}
			catch
			{
				string[] array56 = array[i].Split('\t');
				if (array56.Length >= 2)
				{
					string text6 = array56[0];
					Debug.Log(i + " - " + text6 + " |" + array56[1] + "|");
				}
				Debug.Log("GameCollectionInfo.LoadFromFile() Exception!!!");
			}
		}
	}

	public void Save()
	{
		string empty = string.Empty;
		empty = empty + "Cracked\t" + (m_bCrackedMachine ? 1 : 0) + "\n";
		m_FirstLoginTime = m_GameState.FirstLoginTime;
		m_Level = m_GameState.Level;
		m_CurGold = m_GameState.gold;
		m_CurTCrystal = m_GameState.dollor;
		m_TotalExp = m_GameState.TotalExp;
		m_TotalGameTime = m_GameState.TotalGameTime;
		m_TotalGoldGet = m_GameState.TotalGoldGet;
		m_TotalTCrystalGet = m_GameState.TotalDollorGet;
		string text = empty;
		empty = text + "Level\t" + m_Level + "\n";
		text = empty;
		empty = text + "Gold\t" + m_CurGold + "\n";
		text = empty;
		empty = text + "Crystal\t" + m_CurTCrystal + "\n";
		empty = empty + "FirstLoginTime\t" + m_FirstLoginTime + "\n";
		text = empty;
		empty = text + "TotalExp\t" + m_TotalExp + "\n";
		text = empty;
		empty = text + "TotalGameTime\t" + m_TotalGameTime + "\n";
		text = empty;
		empty = text + "TotalGoldGet\t" + m_TotalGoldGet + "\n";
		text = empty;
		empty = text + "TotalDollorGet\t" + m_TotalTCrystalGet + "\n";
		text = empty;
		empty = text + "HireoutSelfTimes\t" + m_HireoutSelfTimes + "\n";
		text = empty;
		empty = text + "HireOtherTimes\t" + m_HireOtherTimes + "\n";
		text = empty;
		empty = text + "TodayExp\t" + m_TodayExp + "\n";
		text = empty;
		empty = text + "TodayGameTime\t" + m_TodayGameTime + "\n";
		text = empty;
		empty = text + "GameActiveTime\t" + m_GameActiveTime + "\n";
		text = empty;
		empty = text + "TodayGoldGet\t" + m_TodayGoldGet + "\n";
		text = empty;
		empty = text + "TodayGoldLose\t" + m_TodayGoldLose + "\n";
		text = empty;
		empty = text + "TodayTCrystalGet\t" + m_TodayTCrystalGet + "\n";
		text = empty;
		empty = text + "TodayTCrystalLose\t" + m_TodayTCrystalLose + "\n";
		if (m_TodayIapInfo.Count > 0)
		{
			empty += "TodayIapInfo\t";
			int num = 0;
			foreach (string key in m_TodayIapInfo.Keys)
			{
				empty = empty + key + "," + (int)m_TodayIapInfo[key];
				if (num < m_TodayIapInfo.Count - 1)
				{
					empty += ";";
				}
				num++;
			}
			empty += "\n";
		}
		if (m_CurWeaponsHave.Count > 0)
		{
			empty += "WeaponList\t";
			for (int i = 0; i < m_CurWeaponsHave.Count; i++)
			{
				empty += (int)m_CurWeaponsHave[i];
				if (i < m_CurWeaponsHave.Count - 1)
				{
					empty += ";";
				}
			}
			empty += "\n";
		}
		if (m_TodayWeaponsBuy.Count > 0)
		{
			empty += "TodayWeaponsBuy\t";
			for (int j = 0; j < m_TodayWeaponsBuy.Count; j++)
			{
				empty += m_TodayWeaponsBuy[j];
				if (j < m_TodayWeaponsBuy.Count - 1)
				{
					empty += ";";
				}
			}
			empty += "\n";
		}
		m_CurAvatarHave = m_GameState.GetAvatars();
		if (m_CurAvatarHave.Count > 0)
		{
			empty += "CurAvatarHave\t";
			int num2 = 0;
			foreach (Avatar key2 in m_CurAvatarHave.Keys)
			{
				text = empty;
				empty = text + (int)key2.SuiteType + "," + (int)key2.AvtType;
				if (num2 < m_CurAvatarHave.Keys.Count - 1)
				{
					empty += ";";
				}
				num2++;
			}
			empty += "\n";
		}
		if (m_TodayAvatarBuy.Count > 0)
		{
			empty += "TodayAvatarBuy\t";
			for (int k = 0; k < m_TodayAvatarBuy.Count; k++)
			{
				text = empty;
				empty = text + (int)m_TodayAvatarBuy[k].SuiteType + "," + (int)m_TodayAvatarBuy[k].AvtType;
				if (k < m_TodayAvatarBuy.Count - 1)
				{
					empty += ";";
				}
			}
			empty += "\n";
		}
		m_CurPowerUPSHave = m_GameState.GetPowerUPS();
		if (m_CurPowerUPSHave.Count > 0)
		{
			empty += "PowerUPSList\t";
			int num3 = 0;
			foreach (int key3 in m_CurPowerUPSHave.Keys)
			{
				text = empty;
				empty = text + key3 + "," + (int)m_CurPowerUPSHave[key3];
				if (num3 < m_CurPowerUPSHave.Keys.Count - 1)
				{
					empty += ";";
				}
				num3++;
			}
			empty += "\n";
		}
		if (m_TodayPowerUPSBuy.Count > 0)
		{
			empty += "TodayPowerUPSBuy\t";
			int num5 = 0;
			foreach (int key4 in m_TodayPowerUPSBuy.Keys)
			{
				text = empty;
				empty = text + key4 + "," + (int)m_TodayPowerUPSBuy[key4];
				if (num5 < m_TodayPowerUPSBuy.Keys.Count - 1)
				{
					empty += ";";
				}
				num5++;
			}
			empty += "\n";
		}
		if (m_TodayPowerUPSUsed.Count > 0)
		{
			empty += "TodayPowerUPSUsed\t";
			int num7 = 0;
			foreach (int key5 in m_TodayPowerUPSUsed.Keys)
			{
				text = empty;
				empty = text + key5 + "," + (int)m_TodayPowerUPSUsed[key5];
				if (num7 < m_TodayPowerUPSUsed.Keys.Count - 1)
				{
					empty += ";";
				}
				num7++;
			}
			empty += "\n";
		}
		if (m_UIEnterInfo.Count > 0)
		{
			empty += "UIEnterInfo\t";
			int num9 = 0;
			foreach (string key6 in m_UIEnterInfo.Keys)
			{
				text = empty;
				empty = text + key6 + "," + (int)m_UIEnterInfo[key6];
				if (num9 < m_UIEnterInfo.Keys.Count - 1)
				{
					empty += ";";
				}
				num9++;
			}
			empty += "\n";
		}
		string text4 = empty;
		int uILastId = (int)m_UILastId;
		empty = text4 + "UILastId\t" + uILastId + "\n";
		if (m_LH1Info.Count > 0)
		{
			empty += "LH1Info\t";
			int num10 = 0;
			foreach (int key7 in m_LH1Info.Keys)
			{
				text = empty;
				empty = text + key7 + "," + (int)m_LH1Info[key7];
				if (num10 < m_LH1Info.Keys.Count - 1)
				{
					empty += ";";
				}
			}
			empty += "\n";
		}
		if (m_LH2Info.Count > 0)
		{
			empty += "LH2Info\t";
			int num12 = 0;
			foreach (int key8 in m_LH2Info.Keys)
			{
				text = empty;
				empty = text + key8 + "," + (int)m_LH2Info[key8];
				if (num12 < m_LH2Info.Keys.Count - 1)
				{
					empty += ";";
				}
			}
			empty += "\n";
		}
		if (m_LH3Info.Count > 0)
		{
			empty += "LH3Info\t";
			int num14 = 0;
			foreach (int key9 in m_LH3Info.Keys)
			{
				text = empty;
				empty = text + key9 + "," + (int)m_LH3Info[key9];
				if (num14 < m_LH3Info.Keys.Count - 1)
				{
					empty += ";";
				}
			}
			empty += "\n";
		}
		if (m_CH1Info.Count > 0)
		{
			empty += "CH1Info\t";
			int num16 = 0;
			foreach (int key10 in m_CH1Info.Keys)
			{
				text = empty;
				empty = text + key10 + "," + (int)m_CH1Info[key10];
				if (num16 < m_CH1Info.Keys.Count - 1)
				{
					empty += ";";
				}
			}
			empty += "\n";
		}
		if (m_CH2Info.Count > 0)
		{
			empty += "CH2Info\t";
			int num18 = 0;
			foreach (int key11 in m_CH2Info.Keys)
			{
				text = empty;
				empty = text + key11 + "," + (int)m_CH2Info[key11];
				if (num18 < m_CH2Info.Keys.Count - 1)
				{
					empty += ";";
				}
			}
			empty += "\n";
		}
		if (m_CH3Info.Count > 0)
		{
			empty += "CH3Info\t";
			int num20 = 0;
			foreach (int key12 in m_CH3Info.Keys)
			{
				text = empty;
				empty = text + key12 + "," + (int)m_CH3Info[key12];
				if (num20 < m_CH3Info.Keys.Count - 1)
				{
					empty += ";";
				}
			}
			empty += "\n";
		}
		if (m_NP1Info.Count > 0)
		{
			empty += "NP1Info\t";
			int num22 = 0;
			foreach (int key13 in m_NP1Info.Keys)
			{
				text = empty;
				empty = text + key13 + "," + (int)m_NP1Info[key13];
				if (num22 < m_NP1Info.Keys.Count - 1)
				{
					empty += ";";
				}
			}
			empty += "\n";
		}
		if (m_NP2Info.Count > 0)
		{
			empty += "NP2Info\t";
			int num24 = 0;
			foreach (int key14 in m_NP2Info.Keys)
			{
				text = empty;
				empty = text + key14 + "," + (int)m_NP2Info[key14];
				if (num24 < m_NP2Info.Keys.Count - 1)
				{
					empty += ";";
				}
			}
			empty += "\n";
		}
		if (m_LH1PointsInfo.Count > 0)
		{
			empty += "LH1PointsInfo\t";
			int num26 = 1;
			if (m_LH1PointsInfo.ContainsKey("maxPoints"))
			{
				num26 = (int)m_LH1PointsInfo["maxPoints"];
			}
			Hashtable hashtable = new Hashtable();
			if (m_LH1PointsInfo.ContainsKey("PointsInfo"))
			{
				hashtable = (Hashtable)m_LH1PointsInfo["PointsInfo"];
			}
			string text5 = string.Empty;
			if (hashtable.Count > 0)
			{
				int num27 = 0;
				foreach (int key15 in hashtable.Keys)
				{
					text = text5;
					text5 = text + key15 + "," + (int)hashtable[key15];
					if (num27 < hashtable.Keys.Count - 1)
					{
						text5 += ";";
					}
				}
			}
			empty = empty + num26 + "\t" + text5;
		}
		if (m_LH2PointsInfo.Count > 0)
		{
			empty += "LH2PointsInfo\t";
			int num29 = 1;
			if (m_LH2PointsInfo.ContainsKey("maxPoints"))
			{
				num29 = (int)m_LH2PointsInfo["maxPoints"];
			}
			Hashtable hashtable2 = new Hashtable();
			if (m_LH2PointsInfo.ContainsKey("PointsInfo"))
			{
				hashtable2 = (Hashtable)m_LH2PointsInfo["PointsInfo"];
			}
			string text6 = string.Empty;
			if (hashtable2.Count > 0)
			{
				int num30 = 0;
				foreach (int key16 in hashtable2.Keys)
				{
					text = text6;
					text6 = text + key16 + "," + (int)hashtable2[key16];
					if (num30 < hashtable2.Keys.Count - 1)
					{
						text6 += ";";
					}
				}
			}
			empty = empty + num29 + "\t" + text6;
		}
		if (m_CH1PointsInfo.Count > 0)
		{
			empty += "CH1PointsInfo\t";
			int num32 = 1;
			if (m_CH1PointsInfo.ContainsKey("maxPoints"))
			{
				num32 = (int)m_CH1PointsInfo["maxPoints"];
			}
			Hashtable hashtable3 = new Hashtable();
			if (m_CH1PointsInfo.ContainsKey("PointsInfo"))
			{
				hashtable3 = (Hashtable)m_CH1PointsInfo["PointsInfo"];
			}
			string text7 = string.Empty;
			if (hashtable3.Count > 0)
			{
				int num33 = 0;
				foreach (int key17 in hashtable3.Keys)
				{
					text = text7;
					text7 = text + key17 + "," + (int)hashtable3[key17];
					if (num33 < hashtable3.Keys.Count - 1)
					{
						text7 += ";";
					}
				}
			}
			empty = empty + num32 + "\t" + text7;
		}
		if (m_CH2PointsInfo.Count > 0)
		{
			empty += "CH2PointsInfo\t";
			int num35 = 1;
			if (m_CH2PointsInfo.ContainsKey("maxPoints"))
			{
				num35 = (int)m_CH2PointsInfo["maxPoints"];
			}
			Hashtable hashtable4 = new Hashtable();
			if (m_CH2PointsInfo.ContainsKey("PointsInfo"))
			{
				hashtable4 = (Hashtable)m_CH2PointsInfo["PointsInfo"];
			}
			string text8 = string.Empty;
			if (hashtable4.Count > 0)
			{
				int num36 = 0;
				foreach (int key18 in hashtable4.Keys)
				{
					text = text8;
					text8 = text + key18 + "," + (int)hashtable4[key18];
					if (num36 < hashtable4.Keys.Count - 1)
					{
						text8 += ";";
					}
				}
			}
			empty = empty + num35 + "\t" + text8;
		}
		if (m_NP1PointsInfo.Count > 0)
		{
			empty += "NP1PointsInfo\t";
			int num38 = 1;
			if (m_NP1PointsInfo.ContainsKey("maxPoints"))
			{
				num38 = (int)m_NP1PointsInfo["maxPoints"];
			}
			Hashtable hashtable5 = new Hashtable();
			if (m_NP1PointsInfo.ContainsKey("PointsInfo"))
			{
				hashtable5 = (Hashtable)m_NP1PointsInfo["PointsInfo"];
			}
			string text9 = string.Empty;
			if (hashtable5.Count > 0)
			{
				int num39 = 0;
				foreach (int key19 in hashtable5.Keys)
				{
					text = text9;
					text9 = text + key19 + "," + (int)hashtable5[key19];
					if (num39 < hashtable5.Keys.Count - 1)
					{
						text9 += ";";
					}
				}
			}
			empty = empty + num38 + "\t" + text9;
		}
		if (m_NP2PointsInfo.Count > 0)
		{
			empty += "NP2PointsInfo\t";
			int num41 = 1;
			if (m_NP2PointsInfo.ContainsKey("maxPoints"))
			{
				num41 = (int)m_NP2PointsInfo["maxPoints"];
			}
			Hashtable hashtable6 = new Hashtable();
			if (m_NP2PointsInfo.ContainsKey("PointsInfo"))
			{
				hashtable6 = (Hashtable)m_NP2PointsInfo["PointsInfo"];
			}
			string text10 = string.Empty;
			if (hashtable6.Count > 0)
			{
				int num42 = 0;
				foreach (int key20 in hashtable6.Keys)
				{
					text = text10;
					text10 = text + key20 + "," + (int)hashtable6[key20];
					if (num42 < hashtable6.Keys.Count - 1)
					{
						text10 += ";";
					}
				}
			}
			empty = empty + num41 + "\t" + text10;
		}
		if (m_LastGamePointsInfo.Count == 3)
		{
			empty += "LastGamePointsInfo\t";
			text = empty;
			empty = text + (int)m_LastGamePointsInfo["mapIndex"] + "," + (int)m_LastGamePointsInfo["pointIndex"] + "," + (int)m_LastGamePointsInfo["state"];
			empty += "\n";
		}
		int bIsBuyNoviceGiftBag = m_bIsBuyNoviceGiftBag;
		empty = empty + "BuyNoviceGiftBag\t" + bIsBuyNoviceGiftBag + "\n";
		if (m_NetPlayTimes.Count >= 0)
		{
			empty += "NetPlayTimes\t";
			string text11 = string.Empty;
			int num44 = 0;
			foreach (KeyValuePair<string, int> netPlayTime in m_NetPlayTimes)
			{
				text = text11;
				text11 = text + netPlayTime.Key + "," + netPlayTime.Value;
				if (num44 < m_NetPlayTimes.Count - 1)
				{
					text11 += ";";
				}
				num44++;
			}
			empty = empty + text11 + "\n";
		}
		string value = empty;
		try
		{
			string playerDataEncryptKey = MiscPlugin.GetPlayerDataEncryptKey("ME_2_@_YOU_DD");
			byte[] inArray = XXTEAUtils.Encrypt(Encoding.UTF8.GetBytes(empty), Encoding.ASCII.GetBytes(playerDataEncryptKey));
			value = Convert.ToBase64String(inArray);
		}
		catch (Exception)
		{
			Debug.Log("GameCollectionInfo.Save() ERROR!!!");
		}
		string text12 = Utils.SavePath();
		string path = text12 + "/" + m_CurDays;
		StreamWriter streamWriter = new StreamWriter(path, false);
		streamWriter.Write(value);
		streamWriter.Flush();
		streamWriter.Close();
	}

	public string ToJsonString()
	{
		string text = string.Empty;
		try
		{
			Hashtable hashtable = new Hashtable();
			hashtable["Cracked"] = (m_bCrackedMachine ? 1 : 0).ToString();
			hashtable["Level"] = m_Level.ToString();
			hashtable["Gold"] = m_CurGold.ToString();
			hashtable["Crystal"] = m_CurTCrystal.ToString();
			hashtable["FirstLoginTime"] = m_FirstLoginTime.ToString();
			hashtable["TotalExp"] = m_TotalExp.ToString();
			hashtable["TotalGameTime"] = m_TotalGameTime.ToString();
			hashtable["TotalGoldGet"] = m_TotalGoldGet.ToString();
			hashtable["TotalDollorGet"] = m_TotalTCrystalGet.ToString();
			hashtable["HireoutSelfTimes"] = m_HireoutSelfTimes.ToString();
			hashtable["HireOtherTimes"] = m_HireOtherTimes.ToString();
			hashtable["TodayExp"] = m_TodayExp.ToString();
			hashtable["TodayGameTime"] = m_TodayGameTime.ToString();
			hashtable["GameActiveTime"] = m_GameActiveTime.ToString();
			hashtable["TodayGoldGet"] = m_TodayGoldGet.ToString();
			hashtable["TodayGoldLose"] = m_TodayGoldLose.ToString();
			hashtable["TodayTCrystalGet"] = m_TodayTCrystalGet.ToString();
			hashtable["TodayTCrystalLose"] = m_TodayTCrystalLose.ToString();
			string value = JsonMapper.ToJson(m_TodayIapInfo);
			hashtable["TodayIapInfo"] = value;
			string text2 = string.Empty;
			if (m_CurWeaponsHave.Count > 0)
			{
				for (int i = 0; i < m_CurWeaponsHave.Count; i++)
				{
					text2 += (int)m_CurWeaponsHave[i];
					if (i < m_CurWeaponsHave.Count - 1)
					{
						text2 += ";";
					}
				}
				hashtable["WeaponList"] = text2;
			}
			string text3 = string.Empty;
			if (m_TodayWeaponsBuy.Count > 0)
			{
				for (int j = 0; j < m_TodayWeaponsBuy.Count; j++)
				{
					text3 += m_TodayWeaponsBuy[j];
					if (j < m_TodayWeaponsBuy.Count - 1)
					{
						text3 += ";";
					}
				}
				hashtable["TodayWeaponsBuy"] = text3;
			}
			string text4 = string.Empty;
			if (m_CurAvatarHave.Count > 0)
			{
				int num = 0;
				foreach (Avatar key9 in m_CurAvatarHave.Keys)
				{
					string text5 = text4;
					text4 = text5 + (int)key9.SuiteType + "," + (int)key9.AvtType;
					if (num < m_CurAvatarHave.Keys.Count - 1)
					{
						text4 += ";";
					}
					num++;
				}
				hashtable["CurAvatarHave"] = text4;
			}
			string text6 = string.Empty;
			if (m_TodayAvatarBuy.Count > 0)
			{
				for (int k = 0; k < m_TodayAvatarBuy.Count; k++)
				{
					string text5 = text6;
					text6 = text5 + (int)m_TodayAvatarBuy[k].SuiteType + "," + (int)m_TodayAvatarBuy[k].AvtType;
					if (k < m_TodayAvatarBuy.Count - 1)
					{
						text6 += ";";
					}
				}
				hashtable["TodayAvatarBuy"] = text6;
			}
			string text7 = string.Empty;
			if (m_CurPowerUPSHave.Count > 0)
			{
				int num2 = 0;
				foreach (int key10 in m_CurPowerUPSHave.Keys)
				{
					string text5 = text7;
					text7 = text5 + key10 + "," + (int)m_CurPowerUPSHave[key10];
					if (num2 < m_CurPowerUPSHave.Keys.Count - 1)
					{
						text7 += ";";
					}
					num2++;
				}
				hashtable["PowerUPSList"] = text7;
			}
			string text8 = string.Empty;
			if (m_TodayPowerUPSBuy.Count > 0)
			{
				int num4 = 0;
				foreach (int key11 in m_TodayPowerUPSBuy.Keys)
				{
					string text5 = text8;
					text8 = text5 + key11 + "," + (int)m_TodayPowerUPSBuy[key11];
					if (num4 < m_TodayPowerUPSBuy.Keys.Count - 1)
					{
						text8 += ";";
					}
					num4++;
				}
				hashtable["TodayPowerUPSBuy"] = text8;
			}
			string text9 = string.Empty;
			if (m_TodayPowerUPSUsed.Count > 0)
			{
				int num6 = 0;
				foreach (int key12 in m_TodayPowerUPSUsed.Keys)
				{
					string text5 = text9;
					text9 = text5 + key12 + "," + (int)m_TodayPowerUPSUsed[key12];
					if (num6 < m_TodayPowerUPSUsed.Keys.Count - 1)
					{
						text9 += ";";
					}
					num6++;
				}
				hashtable["TodayPowerUPSUsed"] = text9;
			}
			if (m_UIEnterInfo.Count > 0)
			{
				string value2 = JsonMapper.ToJson(m_UIEnterInfo);
				hashtable["UIEnterInfo"] = value2;
			}
			int uILastId = (int)m_UILastId;
			hashtable["UILastId"] = uILastId.ToString();
			if (m_LH1Info.Count > 0)
			{
				Hashtable hashtable2 = new Hashtable();
				foreach (int key13 in m_LH1Info.Keys)
				{
					string key = key13.ToString();
					int num9 = (int)m_LH1Info[key13];
					hashtable2[key] = num9;
				}
				string value3 = JsonMapper.ToJson(hashtable2);
				hashtable["LH1Info"] = value3;
			}
			if (m_LH2Info.Count > 0)
			{
				Hashtable hashtable3 = new Hashtable();
				foreach (int key14 in m_LH2Info.Keys)
				{
					string key2 = key14.ToString();
					int num11 = (int)m_LH2Info[key14];
					hashtable3[key2] = num11;
				}
				string value4 = JsonMapper.ToJson(hashtable3);
				hashtable["LH2Info"] = value4;
			}
			if (m_LH3Info.Count > 0)
			{
				Hashtable hashtable4 = new Hashtable();
				foreach (int key15 in m_LH3Info.Keys)
				{
					string key3 = key15.ToString();
					int num13 = (int)m_LH3Info[key15];
					hashtable4[key3] = num13;
				}
				string value5 = JsonMapper.ToJson(hashtable4);
				hashtable["LH3Info"] = value5;
			}
			if (m_CH1Info.Count > 0)
			{
				Hashtable hashtable5 = new Hashtable();
				foreach (int key16 in m_CH1Info.Keys)
				{
					string key4 = key16.ToString();
					int num15 = (int)m_CH1Info[key16];
					hashtable5[key4] = num15;
				}
				string value6 = JsonMapper.ToJson(hashtable5);
				hashtable["CH1Info"] = value6;
			}
			if (m_CH2Info.Count > 0)
			{
				Hashtable hashtable6 = new Hashtable();
				foreach (int key17 in m_CH2Info.Keys)
				{
					string key5 = key17.ToString();
					int num17 = (int)m_CH2Info[key17];
					hashtable6[key5] = num17;
				}
				string value7 = JsonMapper.ToJson(hashtable6);
				hashtable["CH2Info"] = value7;
			}
			if (m_CH3Info.Count > 0)
			{
				Hashtable hashtable7 = new Hashtable();
				foreach (int key18 in m_CH3Info.Keys)
				{
					string key6 = key18.ToString();
					int num19 = (int)m_CH3Info[key18];
					hashtable7[key6] = num19;
				}
				string value8 = JsonMapper.ToJson(hashtable7);
				hashtable["CH3Info"] = value8;
			}
			if (m_NP1Info.Count > 0)
			{
				Hashtable hashtable8 = new Hashtable();
				foreach (int key19 in m_NP1Info.Keys)
				{
					string key7 = key19.ToString();
					int num21 = (int)m_NP1Info[key19];
					hashtable8[key7] = num21;
				}
				string value9 = JsonMapper.ToJson(hashtable8);
				hashtable["NP1Info"] = value9;
			}
			if (m_NP2Info.Count > 0)
			{
				Hashtable hashtable9 = new Hashtable();
				foreach (int key20 in m_NP2Info.Keys)
				{
					string key8 = key20.ToString();
					int num23 = (int)m_NP2Info[key20];
					hashtable9[key8] = num23;
				}
				string value10 = JsonMapper.ToJson(m_NP2Info);
				hashtable["NP2Info"] = value10;
			}
			if (m_LH1PointsInfo.Count > 0)
			{
				int num24 = 1;
				if (m_LH1PointsInfo.ContainsKey("maxPoints"))
				{
					num24 = (int)m_LH1PointsInfo["maxPoints"];
				}
				Hashtable hashtable10 = new Hashtable();
				if (m_LH1PointsInfo.ContainsKey("PointsInfo"))
				{
					hashtable10 = (Hashtable)m_LH1PointsInfo["PointsInfo"];
				}
				string text10 = string.Empty;
				if (hashtable10.Count > 0)
				{
					int num25 = 0;
					foreach (int key21 in hashtable10.Keys)
					{
						string text5 = text10;
						text10 = text5 + key21 + "," + (int)hashtable10[key21];
						if (num25 < hashtable10.Keys.Count - 1)
						{
							text10 += ";";
						}
					}
				}
				string value11 = num24 + "\t" + text10;
				hashtable["LH1PointsInfo"] = value11;
			}
			if (m_LH2PointsInfo.Count > 0)
			{
				int num27 = 1;
				if (m_LH2PointsInfo.ContainsKey("maxPoints"))
				{
					num27 = (int)m_LH2PointsInfo["maxPoints"];
				}
				Hashtable hashtable11 = new Hashtable();
				if (m_LH2PointsInfo.ContainsKey("PointsInfo"))
				{
					hashtable11 = (Hashtable)m_LH2PointsInfo["PointsInfo"];
				}
				string text11 = string.Empty;
				if (hashtable11.Count > 0)
				{
					int num28 = 0;
					foreach (int key22 in hashtable11.Keys)
					{
						string text5 = text11;
						text11 = text5 + key22 + "," + (int)hashtable11[key22];
						if (num28 < hashtable11.Keys.Count - 1)
						{
							text11 += ";";
						}
					}
				}
				string value12 = num27 + "\t" + text11;
				hashtable["LH2PointsInfo"] = value12;
			}
			if (m_CH1PointsInfo.Count > 0)
			{
				int num30 = 1;
				if (m_CH1PointsInfo.ContainsKey("maxPoints"))
				{
					num30 = (int)m_CH1PointsInfo["maxPoints"];
				}
				Hashtable hashtable12 = new Hashtable();
				if (m_CH1PointsInfo.ContainsKey("PointsInfo"))
				{
					hashtable12 = (Hashtable)m_CH1PointsInfo["PointsInfo"];
				}
				string text12 = string.Empty;
				if (hashtable12.Count > 0)
				{
					int num31 = 0;
					foreach (int key23 in hashtable12.Keys)
					{
						string text5 = text12;
						text12 = text5 + key23 + "," + (int)hashtable12[key23];
						if (num31 < hashtable12.Keys.Count - 1)
						{
							text12 += ";";
						}
					}
				}
				string value13 = num30 + "\t" + text12;
				hashtable["CH1PointsInfo"] = value13;
			}
			if (m_CH2PointsInfo.Count > 0)
			{
				int num33 = 1;
				if (m_CH2PointsInfo.ContainsKey("maxPoints"))
				{
					num33 = (int)m_CH2PointsInfo["maxPoints"];
				}
				Hashtable hashtable13 = new Hashtable();
				if (m_CH2PointsInfo.ContainsKey("PointsInfo"))
				{
					hashtable13 = (Hashtable)m_CH2PointsInfo["PointsInfo"];
				}
				string text13 = string.Empty;
				if (hashtable13.Count > 0)
				{
					int num34 = 0;
					foreach (int key24 in hashtable13.Keys)
					{
						string text5 = text13;
						text13 = text5 + key24 + "," + (int)hashtable13[key24];
						if (num34 < hashtable13.Keys.Count - 1)
						{
							text13 += ";";
						}
					}
				}
				string value14 = num33 + "\t" + text13;
				hashtable["CH2PointsInfo"] = value14;
			}
			if (m_NP1PointsInfo.Count > 0)
			{
				int num36 = 1;
				if (m_NP1PointsInfo.ContainsKey("maxPoints"))
				{
					num36 = (int)m_NP1PointsInfo["maxPoints"];
				}
				Hashtable hashtable14 = new Hashtable();
				if (m_NP1PointsInfo.ContainsKey("PointsInfo"))
				{
					hashtable14 = (Hashtable)m_NP1PointsInfo["PointsInfo"];
				}
				string text14 = string.Empty;
				if (hashtable14.Count > 0)
				{
					int num37 = 0;
					foreach (int key25 in hashtable14.Keys)
					{
						string text5 = text14;
						text14 = text5 + key25 + "," + (int)hashtable14[key25];
						if (num37 < hashtable14.Keys.Count - 1)
						{
							text14 += ";";
						}
					}
				}
				string value15 = num36 + "\t" + text14;
				hashtable["NP1PointsInfo"] = value15;
			}
			if (m_NP2PointsInfo.Count > 0)
			{
				int num39 = 1;
				if (m_NP2PointsInfo.ContainsKey("maxPoints"))
				{
					num39 = (int)m_NP2PointsInfo["maxPoints"];
				}
				Hashtable hashtable15 = new Hashtable();
				if (m_NP2PointsInfo.ContainsKey("PointsInfo"))
				{
					hashtable15 = (Hashtable)m_NP2PointsInfo["PointsInfo"];
				}
				string text15 = string.Empty;
				if (hashtable15.Count > 0)
				{
					int num40 = 0;
					foreach (int key26 in hashtable15.Keys)
					{
						string text5 = text15;
						text15 = text5 + key26 + "," + (int)hashtable15[key26];
						if (num40 < hashtable15.Keys.Count - 1)
						{
							text15 += ";";
						}
					}
				}
				string value16 = num39 + "\t" + text15;
				hashtable["NP2PointsInfo"] = value16;
			}
			int bIsBuyNoviceGiftBag = m_bIsBuyNoviceGiftBag;
			hashtable["BuyNoviceGiftBag"] = bIsBuyNoviceGiftBag;
			if (m_NetPlayTimes.Count >= 0)
			{
				string text16 = string.Empty;
				int num42 = 0;
				foreach (KeyValuePair<string, int> netPlayTime in m_NetPlayTimes)
				{
					string text5 = text16;
					text16 = text5 + netPlayTime.Key + "," + netPlayTime.Value;
					if (num42 < m_NetPlayTimes.Count - 1)
					{
						text16 += ";";
					}
					num42++;
				}
				hashtable["NetPlayTimes"] = text16;
			}
			text = JsonMapper.ToJson(hashtable);
			Debug.Log("GameCollectionInfo - ||" + text);
			return text;
		}
		catch (Exception)
		{
			Debug.Log("GameCollectionInfo.ToJsonString() - Exception!!!");
			return text;
		}
	}

	public void AddExp(int experience)
	{
		m_TodayExp += experience;
	}

	public void AddGameTime(int time)
	{
		m_TodayGameTime += time;
	}

	public void AddGameActiveTimes(int times)
	{
		m_GameActiveTime += times;
		Save();
	}

	public void AddGold(int gold)
	{
		m_TodayGoldGet += gold;
		Save();
	}

	public void LoseGold(int gold)
	{
		m_TodayGoldLose += gold;
		Save();
	}

	public void AddDollor(int dollor)
	{
		m_TodayTCrystalGet += dollor;
		Save();
	}

	public void LoseDollor(int dollor)
	{
		m_TodayTCrystalLose += dollor;
		Save();
	}

	public void AddIAPInfo(string id, int count)
	{
		Debug.Log("AddIAPInfo() " + id + " | " + count);
		int num = count;
		if (m_TodayIapInfo.ContainsKey(id))
		{
			num = (int)m_TodayIapInfo[id];
			num += count;
		}
		m_TodayIapInfo[id] = num;
		Save();
	}

	public void AddTodayWeaponsBuy(int id)
	{
		if (!m_TodayWeaponsBuy.Contains(id))
		{
			m_TodayWeaponsBuy.Add(id);
			Save();
		}
	}

	public void AddTodayAvatarBuy(Avatar.AvatarSuiteType suite_type, Avatar.AvatarType avt_type)
	{
		bool flag = false;
		for (int i = 0; i < m_TodayAvatarBuy.Count; i++)
		{
			Avatar avatar = m_TodayAvatarBuy[i];
			if (avatar.SuiteType == suite_type && avatar.AvtType == avt_type)
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			Avatar item = new Avatar(suite_type, avt_type);
			m_TodayAvatarBuy.Add(item);
			Save();
		}
	}

	public void AddTodayPowerUpsBuy(int id, int count)
	{
		int num = count;
		if (m_TodayPowerUPSBuy.ContainsKey(id))
		{
			num = (int)m_TodayPowerUPSBuy[id];
			num += count;
		}
		m_TodayPowerUPSBuy[id] = num;
		Save();
	}

	public void AddTodayPowerUpsUsed(int id, int count)
	{
		int num = count;
		if (m_TodayPowerUPSUsed.ContainsKey(id))
		{
			num = (int)m_TodayPowerUPSUsed[id];
			num += count;
		}
		m_TodayPowerUPSUsed[id] = num;
		Save();
	}

	public void AddUIEnterLog(enUIEnterIndex id)
	{
		int num = 1;
		int num2 = (int)id;
		string key = num2.ToString();
		if (m_UIEnterInfo.ContainsKey(key))
		{
			num = (int)m_UIEnterInfo[key];
			num++;
		}
		m_UIEnterInfo[key] = num;
		m_UILastId = id;
		Save();
	}

	public void AddHireSelfTimes(int count = 1)
	{
		m_HireoutSelfTimes += count;
		Save();
	}

	public void AddHireOtherTimes(int count = 1)
	{
		m_HireOtherTimes += count;
		Save();
	}

	public void UpdatePointsInfo(int map_index, int points_index, int wave_index)
	{
		switch (map_index)
		{
		case 1:
		{
			int num9 = points_index;
			if (m_LH1PointsInfo.ContainsKey("maxPoints"))
			{
				num9 = (int)m_LH1PointsInfo["maxPoints"];
				if (num9 < points_index)
				{
					num9 = points_index;
				}
			}
			m_LH1PointsInfo["maxPoints"] = num9;
			Hashtable hashtable5 = new Hashtable();
			if (m_LH1PointsInfo.ContainsKey("PointsInfo"))
			{
				hashtable5 = (Hashtable)m_LH1PointsInfo["PointsInfo"];
			}
			int num10 = 1;
			if (hashtable5.ContainsKey(points_index))
			{
				num10 = (int)hashtable5[points_index];
				num10++;
			}
			hashtable5[points_index] = num10;
			break;
		}
		case 3:
		{
			int num7 = points_index;
			if (m_LH2PointsInfo.ContainsKey("maxPoints"))
			{
				num7 = (int)m_LH2PointsInfo["maxPoints"];
				if (num7 < points_index)
				{
					num7 = points_index;
				}
			}
			m_LH2PointsInfo["maxPoints"] = num7;
			Hashtable hashtable4 = new Hashtable();
			if (m_LH2PointsInfo.ContainsKey("PointsInfo"))
			{
				hashtable4 = (Hashtable)m_LH2PointsInfo["PointsInfo"];
			}
			int num8 = 1;
			if (hashtable4.ContainsKey(points_index))
			{
				num8 = (int)hashtable4[points_index];
				num8++;
			}
			hashtable4[points_index] = num8;
			break;
		}
		case 2:
		{
			int num11 = points_index;
			if (m_CH1PointsInfo.ContainsKey("maxPoints"))
			{
				num11 = (int)m_CH1PointsInfo["maxPoints"];
				if (num11 < points_index)
				{
					num11 = points_index;
				}
			}
			m_CH1PointsInfo["maxPoints"] = num11;
			Hashtable hashtable6 = new Hashtable();
			if (m_CH1PointsInfo.ContainsKey("PointsInfo"))
			{
				hashtable6 = (Hashtable)m_CH1PointsInfo["PointsInfo"];
			}
			int num12 = 1;
			if (hashtable6.ContainsKey(points_index))
			{
				num12 = (int)hashtable6[points_index];
				num12++;
			}
			hashtable6[points_index] = num12;
			break;
		}
		case 4:
		{
			int num3 = points_index;
			if (m_CH2PointsInfo.ContainsKey("maxPoints"))
			{
				num3 = (int)m_CH2PointsInfo["maxPoints"];
				if (num3 < points_index)
				{
					num3 = points_index;
				}
			}
			m_CH2PointsInfo["maxPoints"] = num3;
			Hashtable hashtable2 = new Hashtable();
			if (m_CH2PointsInfo.ContainsKey("PointsInfo"))
			{
				hashtable2 = (Hashtable)m_CH2PointsInfo["PointsInfo"];
			}
			int num4 = 1;
			if (hashtable2.ContainsKey(points_index))
			{
				num4 = (int)hashtable2[points_index];
				num4++;
			}
			hashtable2[points_index] = num4;
			break;
		}
		case 6:
		{
			int num5 = points_index;
			if (m_NP1PointsInfo.ContainsKey("maxPoints"))
			{
				num5 = (int)m_NP1PointsInfo["maxPoints"];
				if (num5 < points_index)
				{
					num5 = points_index;
				}
			}
			m_NP1PointsInfo["maxPoints"] = num5;
			Hashtable hashtable3 = new Hashtable();
			if (m_NP1PointsInfo.ContainsKey("PointsInfo"))
			{
				hashtable3 = (Hashtable)m_NP1PointsInfo["PointsInfo"];
			}
			int num6 = 1;
			if (hashtable3.ContainsKey(points_index))
			{
				num6 = (int)hashtable3[points_index];
				num6++;
			}
			hashtable3[points_index] = num6;
			break;
		}
		case 7:
		{
			int num5 = points_index;
			if (m_NP1PointsInfo.ContainsKey("maxPoints"))
			{
				num5 = (int)m_NP1PointsInfo["maxPoints"];
				if (num5 < points_index)
				{
					num5 = points_index;
				}
			}
			m_NP1PointsInfo["maxPoints"] = num5;
			Hashtable hashtable3 = new Hashtable();
			if (m_NP1PointsInfo.ContainsKey("PointsInfo"))
			{
				hashtable3 = (Hashtable)m_NP1PointsInfo["PointsInfo"];
			}
			int num6 = 1;
			if (hashtable3.ContainsKey(points_index))
			{
				num6 = (int)hashtable3[points_index];
				num6++;
			}
			hashtable3[points_index] = num6;
			break;
		}
		case 5:
		{
			int num = points_index;
			if (m_NP2PointsInfo.ContainsKey("maxPoints"))
			{
				num = (int)m_NP2PointsInfo["maxPoints"];
				if (num < points_index)
				{
					num = points_index;
				}
			}
			m_NP2PointsInfo["maxPoints"] = num;
			Hashtable hashtable = new Hashtable();
			if (m_NP2PointsInfo.ContainsKey("PointsInfo"))
			{
				hashtable = (Hashtable)m_NP2PointsInfo["PointsInfo"];
			}
			int num2 = 1;
			if (hashtable.ContainsKey(points_index))
			{
				num2 = (int)hashtable[points_index];
				num2++;
			}
			hashtable[points_index] = num2;
			break;
		}
		}
		Save();
	}

	public void UpdateDeadInfo(int map_index, int points_index, int wave_index)
	{
		switch (map_index)
		{
		case 1:
		{
			int num5 = 1;
			if (m_LH1Info.ContainsKey(points_index))
			{
				num5 = (int)m_LH1Info[points_index] + 1;
			}
			m_LH1Info[points_index] = num5;
			break;
		}
		case 3:
		{
			int num8 = 1;
			if (m_LH2Info.ContainsKey(points_index))
			{
				num8 = (int)m_LH2Info[points_index] + 1;
			}
			m_LH2Info[points_index] = num8;
			break;
		}
		case 101:
		{
			int num6 = 1;
			if (m_LH3Info.ContainsKey(points_index))
			{
				num6 = (int)m_LH3Info[points_index] + 1;
			}
			m_LH3Info[points_index] = num6;
			break;
		}
		case 2:
		{
			int num3 = 1;
			if (m_CH1Info.ContainsKey(points_index))
			{
				num3 = (int)m_CH1Info[points_index] + 1;
			}
			m_CH1Info[points_index] = num3;
			break;
		}
		case 4:
		{
			int num2 = 1;
			if (m_CH2Info.ContainsKey(points_index))
			{
				num2 = (int)m_CH2Info[points_index] + 1;
			}
			m_CH2Info[points_index] = num2;
			break;
		}
		case 102:
		{
			int num7 = 1;
			if (m_CH3Info.ContainsKey(points_index))
			{
				num7 = (int)m_CH3Info[points_index] + 1;
			}
			m_CH3Info[points_index] = num7;
			break;
		}
		case 6:
		{
			int num4 = 1;
			if (m_NP1Info.ContainsKey(points_index))
			{
				num4 = (int)m_NP1Info[points_index] + 1;
			}
			m_NP1Info[points_index] = num4;
			break;
		}
		case 7:
		{
			int num4 = 1;
			if (m_NP1Info.ContainsKey(points_index))
			{
				num4 = (int)m_NP1Info[points_index] + 1;
			}
			m_NP1Info[points_index] = num4;
			break;
		}
		case 5:
		{
			int num = 1;
			if (m_NP2Info.ContainsKey(points_index))
			{
				num = (int)m_NP2Info[points_index] + 1;
			}
			m_NP2Info[points_index] = num;
			break;
		}
		}
		Save();
	}

	public void SetLastGamePointsInfo(int map_index, int points_index, int state)
	{
		m_LastGamePointsInfo["mapIndex"] = map_index;
		m_LastGamePointsInfo["pointIndex"] = points_index;
		m_LastGamePointsInfo["state"] = state;
		Save();
	}

	public void BuyNoviceGiftBag()
	{
		m_bIsBuyNoviceGiftBag++;
		Save();
	}

	public void AddPlayNetTimes(GameState.NetworkGameMode.PlayMode mode, GameState.NetworkGameMode.NetworkCooperationMode coop)
	{
		string text = string.Empty;
		switch (mode)
		{
		case GameState.NetworkGameMode.PlayMode.E_LastStand:
			text = "LastStand_";
			text = ((coop != 0) ? (text + "Solo") : (text + "Team"));
			break;
		case GameState.NetworkGameMode.PlayMode.E_DeathMatch:
			text = "DeathMatch_";
			text = ((coop != 0) ? (text + "Solo") : (text + "Team"));
			break;
		}
		if (m_NetPlayTimes.ContainsKey(text))
		{
			Dictionary<string, int> netPlayTimes;
			Dictionary<string, int> dictionary = (netPlayTimes = m_NetPlayTimes);
			string key;
			string key2 = (key = text);
			int num = netPlayTimes[key];
			dictionary[key2] = num + 1;
		}
		else
		{
			m_NetPlayTimes.Add(text, 1);
		}
		Save();
	}
}
