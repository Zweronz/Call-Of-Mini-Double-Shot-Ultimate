using UnityEngine;
using Zombie3D;

internal class LootManagerScript : MonoBehaviour
{
	public float dropRate = 1f;

	public ItemType[] itemTables = new ItemType[10];

	public float[] rateTables = new float[10];

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void SpawnItem(ItemType itemType)
	{
		GameConfigScript gameConfig = GameApp.GetInstance().GetGameConfig();
	}

	public void OnLoot()
	{
		float value = Random.value;
		if (!(value < dropRate))
		{
			return;
		}
		value = Random.value;
		float num = 0f;
		for (int i = 0; i < itemTables.Length; i++)
		{
			if (rateTables[i] > 0f && value <= num + rateTables[i])
			{
				SpawnItem(itemTables[i]);
				break;
			}
			num += rateTables[i];
		}
	}
}
