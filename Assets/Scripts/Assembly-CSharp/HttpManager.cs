using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Text;
using UnityEngine;

public class HttpManager : MonoBehaviour
{
	private class Session
	{
		private const int BUFFERSIZE = 2048;

		private HttpManager m_HttpMgr;

		private byte[] m_RequestData;

		private HttpWebRequest m_Request;

		private OnResponseDelegate m_onResponse;

		private OnRequestTimeoutDelegate m_onTimeout;

		public StringBuilder m_ResponseData;

		public HttpWebResponse m_Response;

		public float m_fTimeout;

		public int m_iSessionId;

		public string m_strParam;

		public bool m_bCompleted;

		private byte[] m_ReadBuffer = new byte[2048];

		public Session(HttpManager httpMgr, int id, string method, string url, string request, string param, float fTimeout, OnResponseDelegate onResponse, OnRequestTimeoutDelegate onTimeout)
		{
			m_HttpMgr = httpMgr;
			m_iSessionId = id;
			m_fTimeout = fTimeout;
			m_strParam = param;
			m_RequestData = Encoding.UTF8.GetBytes(request);
			m_bCompleted = false;
			m_ResponseData = new StringBuilder(string.Empty);
			m_Response = null;
			m_Request = (HttpWebRequest)WebRequest.Create(url);
			m_Request.Method = method;
			m_Request.KeepAlive = false;
			m_onResponse = onResponse;
			m_onTimeout = onTimeout;
		}

		public void Start()
		{
			if (m_RequestData != null && m_RequestData.Length > 0)
			{
				m_Request.ContentLength = m_RequestData.Length;
				m_Request.ContentType = "application/Json";
				m_Request.BeginGetRequestStream(ConnectedCallback, null);
			}
			m_Request.BeginGetResponse(RespCallback, null);
		}

		public void Stop()
		{
			if (m_Request != null)
			{
				m_Request.Abort();
			}
			if (m_Response != null)
			{
				m_Response.Close();
			}
			m_Request = null;
			m_Response = null;
		}

		public void Update(float deltaTime)
		{
			if (!m_bCompleted && !(m_fTimeout <= 0f))
			{
				m_fTimeout -= deltaTime;
				if (m_fTimeout <= 0f)
				{
					m_bCompleted = true;
				}
			}
		}

		private void ConnectedCallback(IAsyncResult ar)
		{
			Stream stream = m_Request.EndGetRequestStream(ar);
			stream.Write(m_RequestData, 0, m_RequestData.Length);
			stream.Flush();
			stream.Close();
		}

		private void RespCallback(IAsyncResult ar)
		{
			m_Response = (HttpWebResponse)m_Request.EndGetResponse(ar);
			Debug.Log("RespCallback - " + m_Response.StatusCode);
			m_Response.GetResponseStream().BeginRead(m_ReadBuffer, 0, 2048, ReadCallBack, null);
		}

		private void ReadCallBack(IAsyncResult ar)
		{
			Debug.Log("ReadCallBack - " + m_Response.StatusCode);
			int num = m_Response.GetResponseStream().EndRead(ar);
			if (num > 0)
			{
				m_ResponseData.Append(Encoding.UTF8.GetString(m_ReadBuffer, 0, num));
				m_Response.GetResponseStream().BeginRead(m_ReadBuffer, 0, 2048, ReadCallBack, null);
			}
			else
			{
				m_Response.GetResponseStream().Close();
				m_bCompleted = true;
			}
		}

		public void Callback()
		{
			if (m_Response != null)
			{
				if (m_Response.StatusCode == HttpStatusCode.OK)
				{
					if (m_onResponse != null)
					{
						m_onResponse(m_iSessionId, m_strParam, 0, m_ResponseData.ToString());
					}
				}
				else if (m_onResponse != null)
				{
					m_onResponse(m_iSessionId, m_strParam, (int)m_Response.StatusCode, m_ResponseData.ToString());
				}
			}
			else if (m_onTimeout != null)
			{
				m_onTimeout(m_iSessionId, m_strParam);
			}
		}
	}

	public delegate void OnResponseDelegate(int task_id, string param, int code, string response);

	public delegate void OnRequestTimeoutDelegate(int task_id, string param);

	private string m_url;

	private int m_iCounter;

	private Hashtable m_mapSessions = new Hashtable();

	public static HttpManager CreateInstance(string name)
	{
		GameObject gameObject = new GameObject(name);
		UnityEngine.Object.DontDestroyOnLoad(gameObject);
		return gameObject.AddComponent<HttpManager>() as HttpManager;
	}

	public static HttpManager Instance(string name)
	{
		GameObject gameObject = GameObject.Find(name);
		if (gameObject == null)
		{
			return CreateInstance(name);
		}
		return gameObject.GetComponent("HttpManager") as HttpManager;
	}

	public static HttpManager Instance()
	{
		return Instance("HttpManager");
	}

	public void SetUrl(string url)
	{
		m_url = url;
	}

	public int SendRequest(string url, string request, string param, float timeout, OnResponseDelegate onResponse, OnRequestTimeoutDelegate onTimeout)
	{
		int num = GenerateId();
		Debug.Log("Url:" + url);
		Debug.Log("Request:" + request);
		Session session = new Session(this, num, "POST", url, request, param, timeout, onResponse, onTimeout);
		session.Start();
		m_mapSessions.Add(num, session);
		return num;
	}

	public void CancelRequest(int id)
	{
		if (m_mapSessions.ContainsKey(id))
		{
			Session session = m_mapSessions[id] as Session;
			session.Stop();
			m_mapSessions.Remove(id);
		}
	}

	public int Request(string url, float timeout, OnResponseDelegate onResponse, OnRequestTimeoutDelegate onTimeout)
	{
		int num = GenerateId();
		Session session = new Session(this, num, "GET", url, string.Empty, string.Empty, timeout, onResponse, onTimeout);
		session.Start();
		m_mapSessions.Add(num, session);
		return num;
	}

	private int GenerateId()
	{
		do
		{
			m_iCounter++;
			if (m_iCounter <= 0)
			{
				m_iCounter = 1;
			}
		}
		while (m_mapSessions.ContainsKey(m_iCounter));
		return m_iCounter;
	}

	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	public void Update()
	{
		if (m_mapSessions.Count <= 0)
		{
			return;
		}
		ArrayList arrayList = new ArrayList();
		foreach (DictionaryEntry mapSession in m_mapSessions)
		{
			Session session = mapSession.Value as Session;
			session.Update(Time.deltaTime);
			if (session.m_bCompleted)
			{
				arrayList.Add(session);
			}
		}
		for (int i = 0; i < arrayList.Count; i++)
		{
			Session session2 = (Session)arrayList[i];
			session2.Callback();
			session2.Stop();
			m_mapSessions.Remove(session2.m_iSessionId);
		}
	}

	private void OnDestroy()
	{
		foreach (DictionaryEntry mapSession in m_mapSessions)
		{
			Session session = mapSession.Value as Session;
			session.Stop();
			session.Callback();
		}
		m_mapSessions.Clear();
	}
}
