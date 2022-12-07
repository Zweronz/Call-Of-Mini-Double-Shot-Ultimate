using UnityEngine;

public class KeepFlat : MonoBehaviour
{
	public Vector3 rot = new Vector3(270f, 180f, 0f);

	private void Update()
	{
		base.transform.rotation = Quaternion.Euler(rot);
	}
}
