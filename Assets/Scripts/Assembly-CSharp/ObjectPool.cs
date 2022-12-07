using UnityEngine;

public class ObjectPool
{
	private GameObject folderObject;

	protected GameObject[] objects;

	protected Transform[] transforms;

	protected float[] createdTime;

	protected int poolSize;

	protected float life;

	protected bool hasAnimation;

	protected bool hasParticleEmitter;

	public void Init(string poolName, GameObject prefab, int initNum, float life)
	{
		poolSize = initNum;
		objects = new GameObject[initNum];
		transforms = new Transform[initNum];
		createdTime = new float[initNum];
		this.life = life;
		folderObject = new GameObject(poolName);
		for (int i = 0; i < initNum; i++)
		{
			objects[i] = Object.Instantiate(prefab) as GameObject;
			transforms[i] = objects[i].transform;
			objects[i].SetActiveRecursively(false);
			objects[i].transform.parent = folderObject.transform;
			if (objects[i].GetComponent<Animation>() != null)
			{
				hasAnimation = true;
			}
			if (objects[i].GetComponent<ParticleEmitter>() != null)
			{
				hasParticleEmitter = true;
			}
		}
	}

	public GameObject CreateObject(Vector3 position, Vector3 rotation)
	{
		for (int i = 0; i < poolSize; i++)
		{
			if (!objects[i].active)
			{
				objects[i].SetActiveRecursively(true);
				transforms[i].position = position;
				objects[i].transform.rotation = Quaternion.LookRotation(rotation);
				if (hasAnimation)
				{
					objects[i].GetComponent<Animation>().Play();
				}
				if (hasParticleEmitter)
				{
					ParticleEmitter particleEmitter = objects[i].GetComponent<ParticleEmitter>();
					particleEmitter.emit = true;
					particleEmitter.Emit();
				}
				createdTime[i] = Time.time;
				return objects[i];
			}
		}
		return null;
	}

	public void DoLogic()
	{
		for (int i = 0; i < poolSize; i++)
		{
			if (objects[i].active && Time.time - createdTime[i] > life)
			{
				objects[i].SetActiveRecursively(false);
				transforms[i].position = Vector3.zero;
				if (hasParticleEmitter)
				{
					objects[i].GetComponent<ParticleEmitter>().emit = false;
				}
			}
		}
	}

	public GameObject DeleteObject(GameObject obj)
	{
		obj.SetActiveRecursively(false);
		return obj;
	}

	public void DestroyPool()
	{
		Object.Destroy(folderObject);
		for (int i = 0; i < objects.Length; i++)
		{
			objects[i] = null;
		}
	}
}
