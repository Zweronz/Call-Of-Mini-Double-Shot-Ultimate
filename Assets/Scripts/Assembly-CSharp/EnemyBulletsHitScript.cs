using UnityEngine;

public class EnemyBulletsHitScript : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnCollisionEnter(Collision collision)
	{
		Object.Destroy(base.gameObject);
	}
}
