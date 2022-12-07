using UnityEngine;

public class RunCallback : MonoBehaviour
{
	private object serviceObj;

	private object otherParam;

	private WorkDone stopMethod;

	private WorkDone2 stopMethod2;

	private WorkDone callback;

	private WorkDone2 callback2;

	private float runTime;

	private float timeOut = 20f;

	private float totalTime;

	private float callbackTime;

	private bool bDontDestroy = true;

	public object ServiceObj
	{
		get
		{
			return serviceObj;
		}
		set
		{
			serviceObj = value;
		}
	}

	public object OtherParam
	{
		get
		{
			return otherParam;
		}
		set
		{
			otherParam = value;
		}
	}

	public WorkDone StopMethod
	{
		set
		{
			stopMethod = value;
		}
	}

	public WorkDone2 StopMethod2
	{
		set
		{
			stopMethod2 = value;
		}
	}

	public WorkDone Callback
	{
		set
		{
			callback = value;
		}
	}

	public WorkDone2 Callback2
	{
		set
		{
			callback2 = value;
		}
	}

	public float TimeOut
	{
		set
		{
			timeOut = value;
		}
	}

	public float CallbackTime
	{
		set
		{
			callbackTime = value;
		}
	}

	public bool DontDestroy
	{
		get
		{
			return bDontDestroy;
		}
		set
		{
			bDontDestroy = value;
		}
	}

	private void Awake()
	{
	}

	private void Start()
	{
		if (bDontDestroy)
		{
			Object.DontDestroyOnLoad(this);
		}
	}

	private void Update()
	{
		if (serviceObj == null)
		{
			return;
		}
		runTime += Time.deltaTime;
		totalTime += Time.deltaTime;
		if (totalTime >= timeOut && stopMethod != null)
		{
			totalTime = 0f;
			StopNow();
		}
		else if (callbackTime == 0f)
		{
			runTime = 0f;
			if (callback != null)
			{
				callback(serviceObj);
			}
			else if (callback2 != null)
			{
				callback2(serviceObj, otherParam);
			}
		}
		else if (runTime / callbackTime >= 1f)
		{
			runTime = 0f;
			if (callback != null)
			{
				callback(serviceObj);
			}
			else if (callback2 != null)
			{
				callback2(serviceObj, otherParam);
			}
		}
	}

	public void ClearRunTime()
	{
		runTime = 0f;
		totalTime = 0f;
	}

	public void StopNow()
	{
		if (stopMethod != null)
		{
			stopMethod(serviceObj);
		}
		else if (stopMethod2 != null)
		{
			stopMethod2(serviceObj, otherParam);
		}
		CallBackManager.Instance().RemoveCallBack(base.gameObject.name);
	}
}
