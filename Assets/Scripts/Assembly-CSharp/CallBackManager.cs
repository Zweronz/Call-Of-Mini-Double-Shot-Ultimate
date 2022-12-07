using UnityEngine;

public class CallBackManager
{
	protected PropUtils m_Props = new PropUtils();

	protected GameObject m_CallBackManagerObj;

	private static CallBackManager s_intance;

	public CallBackManager()
	{
		m_CallBackManagerObj = new GameObject();
		m_CallBackManagerObj.name = "CallBackManager";
		m_CallBackManagerObj.transform.position = Vector3.zero;
		Object.DontDestroyOnLoad(m_CallBackManagerObj);
	}

	public static CallBackManager Instance()
	{
		if (s_intance == null)
		{
			s_intance = new CallBackManager();
		}
		return s_intance;
	}

	public RunCallback CreateCallBack(string objName, WorkDone callbackMethod, WorkDone stopMethod, object m_service)
	{
		if (m_Props.GetObject(objName) != null)
		{
			return null;
		}
		GameObject gameObject = new GameObject(objName);
		gameObject.transform.parent = m_CallBackManagerObj.transform;
		m_Props.SetProp(objName, gameObject);
		RunCallback runCallback = gameObject.AddComponent<RunCallback>();
		runCallback.Callback = callbackMethod;
		runCallback.Callback2 = null;
		runCallback.StopMethod = stopMethod;
		runCallback.StopMethod2 = null;
		runCallback.ServiceObj = m_service;
		runCallback.OtherParam = null;
		return runCallback;
	}

	public RunCallback CreateCallBack2(string objName, WorkDone2 callbackMethod, WorkDone2 stopMethod, object m_service, object other_param)
	{
		if (m_Props.GetObject(objName) != null)
		{
			return null;
		}
		GameObject gameObject = new GameObject(objName);
		gameObject.transform.parent = m_CallBackManagerObj.transform;
		m_Props.SetProp(objName, gameObject);
		RunCallback runCallback = gameObject.AddComponent<RunCallback>() as RunCallback;
		runCallback.Callback = null;
		runCallback.Callback2 = callbackMethod;
		runCallback.StopMethod = null;
		runCallback.StopMethod2 = stopMethod;
		runCallback.ServiceObj = m_service;
		runCallback.OtherParam = other_param;
		return runCallback;
	}

	public static void Working(WorkDone callBack, object paramObject)
	{
		callBack(paramObject);
	}

	public static void Working2(WorkDone2 callBack, object paramObject, object param2)
	{
		callBack(paramObject, param2);
	}

	public RunCallback GetCallBack(string callback_obj_name)
	{
		GameObject gameObject = m_Props.GetGameObject(callback_obj_name);
		if (gameObject != null && (bool)gameObject.GetComponent(typeof(RunCallback)))
		{
			return gameObject.GetComponent(typeof(RunCallback)) as RunCallback;
		}
		return null;
	}

	public void RemoveCallBack(string objName)
	{
		object @object = m_Props.GetObject(objName);
		if (@object != null)
		{
			m_Props.RemoveProp(objName);
			Object.Destroy((GameObject)@object);
		}
	}
}
