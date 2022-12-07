using UnityEngine;

public class AvatarEffect03 : MonoBehaviour
{
	private float keepTime = 1.5f;

	public float genTimeInterval = 1f;

	private float lastGenTime;

	private GameObject m_DemonLordFire;

	private void Start()
	{
		m_DemonLordFire = Resources.Load("Zombie3D/AvatarEffect/DemonLord_RoadFire") as GameObject;
	}

	private void Update()
	{
		if (Time.time - lastGenTime > genTimeInterval)
		{
			Vector3 position = base.transform.position;
			float f = Random.Range(0f, 360f);
			float num = 0.4f;
			GameObject gameObject = Object.Instantiate(position: new Vector3(position.x + num * Mathf.Sin(f), position.y, position.z + num * Mathf.Cos(f)), original: m_DemonLordFire, rotation: Quaternion.identity) as GameObject;
			gameObject.transform.parent = base.transform;
			RemoveTimerScript removeTimerScript = gameObject.AddComponent<RemoveTimerScript>();
			removeTimerScript.life = keepTime;
			lastGenTime = Time.time;
		}
	}
}
