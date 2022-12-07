using System.Collections.Generic;
using UnityEngine;

public class Faceout3DTextPool
{
	private GameObject m_FolderObject;

	private List<GameObject> m_Texts;

	private static Faceout3DTextPool m_Instance;

	public static Faceout3DTextPool Instance()
	{
		if (m_Instance == null)
		{
			m_Instance = new Faceout3DTextPool();
			m_Instance.Init(20);
		}
		return m_Instance;
	}

	public void Init(int initNum)
	{
		m_FolderObject = new GameObject("3DTextPool");
		m_FolderObject.transform.position = new Vector3(0f, 10000.1f, 0f);
		m_Texts = new List<GameObject>(initNum);
		for (int i = 0; i < initNum; i++)
		{
			GameObject gameObject = Object.Instantiate(Resources.Load("Zombie3D/Misc/Text3D")) as GameObject;
			gameObject.SetActiveRecursively(false);
			m_Texts.Add(gameObject);
			gameObject.transform.parent = m_FolderObject.transform;
		}
	}

	public GameObject Create3DText(Vector3 position, Quaternion rotation, string text)
	{
		Color white = Color.white;
		bool flag = false;
		for (int i = 0; i < m_Texts.Count; i++)
		{
			if (!m_Texts[i].active)
			{
				flag = true;
				m_Texts[i].SetActiveRecursively(true);
				m_Texts[i].transform.position = position;
				m_Texts[i].transform.rotation = rotation;
				Faceout3DText component = m_Texts[i].GetComponent<Faceout3DText>();
				component.Init();
				TextMesh textMesh = component.GetComponent(typeof(TextMesh)) as TextMesh;
				if (textMesh != null)
				{
					textMesh.text = text;
				}
				return m_Texts[i];
			}
		}
		if (!flag && m_Texts.Count > 0)
		{
			GameObject gameObject = Object.Instantiate(m_Texts[0]) as GameObject;
			gameObject.SetActiveRecursively(true);
			gameObject.transform.parent = m_FolderObject.transform;
			m_Texts.Add(gameObject);
			gameObject.transform.position = position;
			gameObject.transform.rotation = rotation;
			Faceout3DText component2 = gameObject.GetComponent<Faceout3DText>();
			component2.Init();
			component2.SetColor(white);
			TextMesh textMesh2 = component2.GetComponent(typeof(TextMesh)) as TextMesh;
			if (textMesh2 != null)
			{
				textMesh2.text = text;
			}
			return gameObject;
		}
		return null;
	}

	public GameObject Create3DText(Vector3 position, Quaternion rotation, string text, Color color)
	{
		bool flag = false;
		for (int i = 0; i < m_Texts.Count; i++)
		{
			if (!m_Texts[i].active)
			{
				flag = true;
				m_Texts[i].SetActiveRecursively(true);
				m_Texts[i].transform.position = position;
				m_Texts[i].transform.rotation = rotation;
				Faceout3DText component = m_Texts[i].GetComponent<Faceout3DText>();
				component.Init();
				component.SetColor(color);
				TextMesh textMesh = component.GetComponent(typeof(TextMesh)) as TextMesh;
				if (textMesh != null)
				{
					textMesh.text = text;
				}
				return m_Texts[i];
			}
		}
		if (!flag && m_Texts.Count > 0)
		{
			GameObject gameObject = Object.Instantiate(m_Texts[0]) as GameObject;
			gameObject.SetActiveRecursively(true);
			gameObject.transform.parent = m_FolderObject.transform;
			m_Texts.Add(gameObject);
			gameObject.transform.position = position;
			gameObject.transform.rotation = rotation;
			Faceout3DText component2 = gameObject.GetComponent<Faceout3DText>();
			component2.Init();
			component2.SetColor(color);
			TextMesh textMesh2 = component2.GetComponent(typeof(TextMesh)) as TextMesh;
			if (textMesh2 != null)
			{
				textMesh2.text = text;
			}
			return gameObject;
		}
		return null;
	}

	public void DoLogic()
	{
	}

	public GameObject Delete3DText(GameObject obj)
	{
		obj.SetActiveRecursively(false);
		return obj;
	}

	public void DestroyPool()
	{
		m_Texts.Clear();
		Object.Destroy(m_FolderObject);
	}
}
