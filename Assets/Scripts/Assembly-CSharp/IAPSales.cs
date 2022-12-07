using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Zombie3D;

public class IAPSales : MonoBehaviour
{
	private string url = "http://account.trinitigame.com/game/CoMDSIAP/CoMDSIAP.txt";

	public HandlerEvent_IAPSaleCheck m_DownLoadErrorEvent;

	public HandlerEvent_IAPSaleCheckOK m_DownLoadOKEvent;

	private List<IAPSalesClass> lsIAPS = new List<IAPSalesClass>();

	private List<int> lsSaleDates = new List<int>();

	public IEnumerator Start()
	{
		WWW www = new WWW(url);
		yield return www;
		if (www.error != null)
		{
			Debug.Log(www.error);
			if (m_DownLoadErrorEvent != null)
			{
				m_DownLoadErrorEvent();
			}
		}
		LoadData(www.text);
		if (m_DownLoadOKEvent != null)
		{
			m_DownLoadOKEvent(lsIAPS, lsSaleDates);
		}
	}

	private void LoadData(string input)
	{
		string text = string.Empty;
		try
		{
			text = Decrypt(input);
		}
		catch (Exception)
		{
		}
		Debug.LogWarning("|" + text + "|");
		if (!(text != string.Empty))
		{
			return;
		}
		string[] array = text.Split('\r', '\n');
		IAPSalesClass iAPSalesClass = null;
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i] == null)
			{
				continue;
			}
			try
			{
				if (array[i] == "[-]")
				{
					iAPSalesClass = new IAPSalesClass();
					iAPSalesClass._iapCfg = new FixedConfig.IAPCfg();
					continue;
				}
				string[] array2 = array[i].Split('\t');
				if (array2.Length < 2 || iAPSalesClass == null)
				{
					continue;
				}
				switch (array2[0])
				{
				case "SaleDates":
				{
					string[] array3 = array2[1].Split(',');
					string[] array4 = array3;
					foreach (string s in array4)
					{
						lsSaleDates.Add(int.Parse(s));
					}
					break;
				}
				case "CID":
					iAPSalesClass._iapID = array2[1];
					break;
				case "IAPID":
					iAPSalesClass._iapCfg.iapID = array2[1];
					break;
				case "IAPDOLLOR":
					iAPSalesClass._iapCfg.iapDollor = float.Parse(array2[1]);
					break;
				case "GAMEGOLD":
					iAPSalesClass._iapCfg.gameGold = float.Parse(array2[1]);
					break;
				case "GAMEDOLLOR":
					iAPSalesClass._iapCfg.gameDollor = float.Parse(array2[1]);
					break;
				case "INTRODUCTION":
					iAPSalesClass._iapCfg.introduction = array2[1];
					lsIAPS.Add(iAPSalesClass);
					break;
				}
			}
			catch
			{
				Debug.Log("IAPSALE.LoadData() Exception!!!");
			}
		}
	}

	private string Decrypt(string input_data)
	{
		string empty = string.Empty;
		byte[] data = Convert.FromBase64String(input_data);
		string playerDataEncryptKey = MiscPlugin.GetPlayerDataEncryptKey("ME_2_@_YOU_DD");
		byte[] bytes = XXTEAUtils.Decrypt(data, Encoding.ASCII.GetBytes(playerDataEncryptKey));
		return Encoding.UTF8.GetString(bytes);
	}
}
