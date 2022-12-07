using System.Collections;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

public class CubeAppData
{
	public class TaskBase
	{
		protected struct Key_In
		{
			public string key;
		}

		protected struct Key2_In
		{
			public string key2;
		}

		protected struct Key2Value_In
		{
			public string key2;

			public string value;
		}

		protected struct KeyKey2Value_Out
		{
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
			public string key;

			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
			public string key2;

			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 10240)]
			public string value;
		}

		protected int m_TaskId;

		protected int m_Status;

		protected TaskBase()
		{
			m_TaskId = -1;
			m_Status = -1;
		}

		[DllImport("Cube")]
		protected static extern int CubeAppData_SetAppData(string action_id, string user_id, string project, string key, Key2Value_In[] item, int count);

		[DllImport("Cube")]
		protected static extern int CubeAppData_SetAppDataResult(int task_id, ref int code);

		[DllImport("Cube")]
		protected static extern int CubeAppData_GetAppData(string action_id, string user_id, string project, Key_In[] key, int count);

		[DllImport("Cube")]
		protected static extern int CubeAppData_GetAppDataResult(int task_id, ref int code, [Out] KeyKey2Value_Out[] item, int buf_count, ref int read_count);

		[DllImport("Cube")]
		protected static extern int CubeAppData_DeleteAppData(string action_id, string user_id, string project, Key_In[] key, int count);

		[DllImport("Cube")]
		protected static extern int CubeAppData_DeleteAppDataResult(int task_id, ref int code);

		[DllImport("Cube")]
		protected static extern int CubeAppData_SetAppDataItem(string action_id, string user_id, string project, string key, Key2Value_In[] item, int count);

		[DllImport("Cube")]
		protected static extern int CubeAppData_SetAppDataItemResult(int task_id, ref int code);

		[DllImport("Cube")]
		protected static extern int CubeAppData_GetAppDataItem(string action_id, string user_id, string project, string key, Key2_In[] key2, int count);

		[DllImport("Cube")]
		protected static extern int CubeAppData_GetAppDataItemResult(int task_id, ref int code, [Out] KeyKey2Value_Out[] item, int buf_count, ref int read_count);

		[DllImport("Cube")]
		protected static extern int CubeAppData_DeleteAppDataItem(string action_id, string user_id, string project, string key, Key2_In[] key2, int count);

		[DllImport("Cube")]
		protected static extern int CubeAppData_DeleteAppDataItemResult(int task_id, ref int code);

		[DllImport("Cube")]
		protected static extern int CubeAppData_SendJson(string action_id, string json);

		[DllImport("Cube")]
		protected static extern int CubeAppData_SendJsonResult(int task_id, [Out][MarshalAs(UnmanagedType.LPStr)] StringBuilder json);

		[DllImport("Cube")]
		protected static extern int CubeAccount_SendJsonToPlatform(string action_id, string json);

		[DllImport("Cube")]
		protected static extern int CubeAccount_SendJsonToPlatformResult(int task_id, [Out][MarshalAs(UnmanagedType.LPStr)] StringBuilder json);

		[DllImport("Cube")]
		protected static extern void CubeAccount_CancelTask(int task_id);

		[DllImport("Cube")]
		protected static extern void CubeAppData_CancelTask(int task_id);
	}

	public class SetAppData : TaskBase
	{
		private int m_Code;

		public int Code
		{
			get
			{
				return m_Code;
			}
		}

		public SetAppData()
		{
			m_Code = -1;
		}

		public bool Start(string action_id, string user_id, string project, string key, Hashtable data)
		{
			if (m_TaskId >= 0)
			{
				return false;
			}
			Key2Value_In[] array = new Key2Value_In[data.Count];
			int num = 0;
			foreach (DictionaryEntry datum in data)
			{
				array[num].key2 = datum.Key.ToString();
				array[num].value = datum.Value.ToString();
				num++;
			}
			m_TaskId = TaskBase.CubeAppData_SetAppData(action_id, user_id, project, key, array, num);
			if (m_TaskId < 0)
			{
				return false;
			}
			m_Status = 0;
			m_Code = -1;
			return true;
		}

		public void Stop()
		{
			if (m_TaskId >= 0)
			{
				TaskBase.CubeAppData_CancelTask(m_TaskId);
			}
			m_TaskId = -1;
			m_Status = -1;
		}

		public int Status()
		{
			if (m_TaskId < 0)
			{
				return -1;
			}
			if (m_Status != 0)
			{
				return m_Status;
			}
			int code = -1;
			m_Status = TaskBase.CubeAppData_SetAppDataResult(m_TaskId, ref code);
			if (m_Status > 0)
			{
				m_Code = code;
			}
			return m_Status;
		}
	}

	public class GetAppData : TaskBase
	{
		private int m_MaxItemCount = 64;

		private int m_Code;

		private Hashtable m_Data;

		public int MaxItemCount
		{
			set
			{
				m_MaxItemCount = value;
			}
		}

		public int Code
		{
			get
			{
				return m_Code;
			}
		}

		public GetAppData()
		{
			m_Code = -1;
			m_Data = new Hashtable();
		}

		public bool Start(string action_id, string user_id, string project, string[] key)
		{
			if (m_TaskId >= 0)
			{
				return false;
			}
			Key_In[] array = new Key_In[key.Length];
			for (int i = 0; i < key.Length; i++)
			{
				array[i].key = key[i];
			}
			m_TaskId = TaskBase.CubeAppData_GetAppData(action_id, user_id, project, array, key.Length);
			if (m_TaskId < 0)
			{
				return false;
			}
			m_Status = 0;
			m_Code = -1;
			m_Data.Clear();
			return true;
		}

		public void Stop()
		{
			if (m_TaskId >= 0)
			{
				TaskBase.CubeAppData_CancelTask(m_TaskId);
			}
			m_TaskId = -1;
			m_Status = -1;
		}

		public int Status()
		{
			if (m_TaskId < 0)
			{
				return -1;
			}
			if (m_Status != 0)
			{
				return m_Status;
			}
			int code = -1;
			KeyKey2Value_Out[] array = new KeyKey2Value_Out[m_MaxItemCount];
			int read_count = 0;
			m_Status = TaskBase.CubeAppData_GetAppDataResult(m_TaskId, ref code, array, m_MaxItemCount, ref read_count);
			if (m_Status > 0)
			{
				m_Code = code;
				for (int i = 0; i < read_count; i++)
				{
					string key = array[i].key;
					if (m_Data.Contains(key))
					{
						Hashtable hashtable = (Hashtable)m_Data[key];
						hashtable.Add(array[i].key2, array[i].value);
					}
					else
					{
						Hashtable hashtable2 = new Hashtable();
						hashtable2.Add(array[i].key2, array[i].value);
						m_Data.Add(key, hashtable2);
					}
				}
			}
			return m_Status;
		}

		public Hashtable GetData(string key)
		{
			if (m_Data.Contains(key))
			{
				return (Hashtable)m_Data[key];
			}
			return null;
		}
	}

	public class DeleteAppData : TaskBase
	{
		private int m_Code;

		public int Code
		{
			get
			{
				return m_Code;
			}
		}

		public DeleteAppData()
		{
			m_Code = -1;
		}

		public bool Start(string action_id, string user_id, string project, string[] key)
		{
			if (m_TaskId >= 0)
			{
				return false;
			}
			Key_In[] array = new Key_In[key.Length];
			for (int i = 0; i < key.Length; i++)
			{
				array[i].key = key[i];
			}
			m_TaskId = TaskBase.CubeAppData_DeleteAppData(action_id, user_id, project, array, key.Length);
			if (m_TaskId < 0)
			{
				return false;
			}
			m_Status = 0;
			m_Code = -1;
			return true;
		}

		public void Stop()
		{
			if (m_TaskId >= 0)
			{
				TaskBase.CubeAppData_CancelTask(m_TaskId);
			}
			m_TaskId = -1;
			m_Status = -1;
		}

		public int Status()
		{
			if (m_TaskId < 0)
			{
				return -1;
			}
			if (m_Status != 0)
			{
				return m_Status;
			}
			int code = -1;
			m_Status = TaskBase.CubeAppData_DeleteAppDataResult(m_TaskId, ref code);
			if (m_Status > 0)
			{
				m_Code = code;
			}
			return m_Status;
		}
	}

	public class SetAppDataItem : TaskBase
	{
		private int m_Code;

		public int Code
		{
			get
			{
				return m_Code;
			}
		}

		public SetAppDataItem()
		{
			m_Code = -1;
		}

		public bool Start(string action_id, string user_id, string project, string key, Hashtable data)
		{
			if (m_TaskId >= 0)
			{
				return false;
			}
			Key2Value_In[] array = new Key2Value_In[data.Count];
			int num = 0;
			foreach (DictionaryEntry datum in data)
			{
				array[num].key2 = datum.Key.ToString();
				array[num].value = datum.Value.ToString();
				num++;
			}
			m_TaskId = TaskBase.CubeAppData_SetAppDataItem(action_id, user_id, project, key, array, num);
			if (m_TaskId < 0)
			{
				return false;
			}
			m_Status = 0;
			m_Code = -1;
			return true;
		}

		public void Stop()
		{
			if (m_TaskId >= 0)
			{
				TaskBase.CubeAppData_CancelTask(m_TaskId);
			}
			m_TaskId = -1;
			m_Status = -1;
		}

		public int Status()
		{
			if (m_TaskId < 0)
			{
				return -1;
			}
			if (m_Status != 0)
			{
				return m_Status;
			}
			int code = -1;
			m_Status = TaskBase.CubeAppData_SetAppDataItemResult(m_TaskId, ref code);
			if (m_Status > 0)
			{
				m_Code = code;
			}
			return m_Status;
		}
	}

	public class GetAppDataItem : TaskBase
	{
		private int m_MaxItemCount = 64;

		private int m_Code;

		private Hashtable m_Data;

		public int MaxItemCount
		{
			set
			{
				m_MaxItemCount = value;
			}
		}

		public int Code
		{
			get
			{
				return m_Code;
			}
		}

		public GetAppDataItem()
		{
			m_Code = -1;
			m_Data = new Hashtable();
		}

		public bool Start(string action_id, string user_id, string project, string key, string[] key2)
		{
			if (m_TaskId >= 0)
			{
				return false;
			}
			Key2_In[] array = new Key2_In[key2.Length];
			for (int i = 0; i < key2.Length; i++)
			{
				array[i].key2 = key2[i];
			}
			m_TaskId = TaskBase.CubeAppData_GetAppDataItem(action_id, user_id, project, key, array, key2.Length);
			if (m_TaskId < 0)
			{
				return false;
			}
			m_Status = 0;
			m_Code = -1;
			return true;
		}

		public void Stop()
		{
			if (m_TaskId >= 0)
			{
				TaskBase.CubeAppData_CancelTask(m_TaskId);
			}
			m_TaskId = -1;
			m_Status = -1;
		}

		public int Status()
		{
			if (m_TaskId < 0)
			{
				return -1;
			}
			if (m_Status != 0)
			{
				return m_Status;
			}
			int code = -1;
			KeyKey2Value_Out[] array = new KeyKey2Value_Out[m_MaxItemCount];
			int read_count = 0;
			m_Status = TaskBase.CubeAppData_GetAppDataItemResult(m_TaskId, ref code, array, m_MaxItemCount, ref read_count);
			if (m_Status > 0)
			{
				m_Code = code;
				for (int i = 0; i < read_count; i++)
				{
					string key = array[i].key;
					if (m_Data.Contains(key))
					{
						Hashtable hashtable = (Hashtable)m_Data[key];
						hashtable.Add(array[i].key2, array[i].value);
					}
					else
					{
						Hashtable hashtable2 = new Hashtable();
						hashtable2.Add(array[i].key2, array[i].value);
						m_Data.Add(key, hashtable2);
					}
				}
			}
			return m_Status;
		}

		public Hashtable GetData(string key)
		{
			if (m_Data.Contains(key))
			{
				return (Hashtable)m_Data[key];
			}
			return null;
		}
	}

	public class DeleteAppDataItem : TaskBase
	{
		private int m_Code;

		public int Code
		{
			get
			{
				return m_Code;
			}
		}

		public DeleteAppDataItem()
		{
			m_Code = -1;
		}

		public bool Start(string action_id, string user_id, string project, string key, string[] key2)
		{
			if (m_TaskId >= 0)
			{
				return false;
			}
			Key2_In[] array = new Key2_In[key2.Length];
			for (int i = 0; i < key2.Length; i++)
			{
				array[i].key2 = key2[i];
			}
			m_TaskId = TaskBase.CubeAppData_DeleteAppDataItem(action_id, user_id, project, key, array, key2.Length);
			if (m_TaskId < 0)
			{
				return false;
			}
			m_Status = 0;
			m_Code = -1;
			return true;
		}

		public void Stop()
		{
			if (m_TaskId >= 0)
			{
				TaskBase.CubeAppData_CancelTask(m_TaskId);
			}
			m_TaskId = -1;
			m_Status = -1;
		}

		public int Status()
		{
			if (m_TaskId < 0)
			{
				return -1;
			}
			if (m_Status != 0)
			{
				return m_Status;
			}
			int code = -1;
			m_Status = TaskBase.CubeAppData_DeleteAppDataItemResult(m_TaskId, ref code);
			if (m_Status > 0)
			{
				m_Code = code;
			}
			return m_Status;
		}
	}

	public class AppDataSendJson : TaskBase
	{
		private int m_Code;

		private string m_Data = string.Empty;

		public int Code
		{
			get
			{
				return m_Code;
			}
		}

		public AppDataSendJson()
		{
			m_Code = -1;
			m_Data = new string('\0', 10240);
		}

		public bool Start(string action_id, string json)
		{
			if (m_TaskId >= 0)
			{
				return false;
			}
			m_TaskId = TaskBase.CubeAppData_SendJson(action_id, json);
			if (m_TaskId < 0)
			{
				return false;
			}
			m_Status = 0;
			m_Code = -1;
			return true;
		}

		public void Stop()
		{
			if (m_TaskId >= 0)
			{
				TaskBase.CubeAppData_CancelTask(m_TaskId);
			}
			m_TaskId = -1;
			m_Status = -1;
		}

		public int Status()
		{
			if (m_TaskId < 0)
			{
				return -1;
			}
			if (m_Status != 0)
			{
				return m_Status;
			}
			int code = 0;
			StringBuilder stringBuilder = new StringBuilder(10240);
			m_Status = TaskBase.CubeAppData_SendJsonResult(m_TaskId, stringBuilder);
			m_Data = stringBuilder.ToString();
			if (m_Status > 0)
			{
				m_Code = code;
			}
			return m_Status;
		}

		public string GetData()
		{
			return m_Data;
		}
	}

	public class AccountDataSendJsonToPlatform : TaskBase
	{
		private int m_Code;

		private string m_Data = string.Empty;

		public int Code
		{
			get
			{
				return m_Code;
			}
		}

		public AccountDataSendJsonToPlatform()
		{
			m_Code = -1;
			m_Data = new string('\0', 10240);
		}

		public bool Start(string action_id, string json)
		{
			if (m_TaskId >= 0)
			{
				return false;
			}
			m_TaskId = TaskBase.CubeAccount_SendJsonToPlatform(action_id, json);
			if (m_TaskId < 0)
			{
				Debug.Log("Cannnnnnnnnnnnnnnnnnnnnnnnnnnnnnn");
				return false;
			}
			m_Status = 0;
			m_Code = -1;
			return true;
		}

		public void Stop()
		{
			if (m_TaskId >= 0)
			{
				TaskBase.CubeAccount_CancelTask(m_TaskId);
			}
			m_TaskId = -1;
			m_Status = -1;
		}

		public int Status()
		{
			if (m_TaskId < 0)
			{
				return -1;
			}
			if (m_Status != 0)
			{
				return m_Status;
			}
			int code = 0;
			StringBuilder stringBuilder = new StringBuilder(11240);
			m_Status = TaskBase.CubeAccount_SendJsonToPlatformResult(m_TaskId, stringBuilder);
			m_Data = stringBuilder.ToString();
			if (m_Status > 0)
			{
				m_Code = code;
			}
			return m_Status;
		}

		public string GetData()
		{
			return m_Data;
		}
	}
}
