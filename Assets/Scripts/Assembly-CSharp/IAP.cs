using UnityEngine;

public class IAP : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
		CheckPurchaseStatus();
	}

	public void PurchaseProduct(string productId, string productCount)
	{
		IAPPlugin.NowPurchaseProduct(productId, productCount);
	}

	private void CheckPurchaseStatus()
	{
		int purchaseStatus = IAPPlugin.GetPurchaseStatus();
		if (purchaseStatus != 0)
		{
			if (purchaseStatus <= 0)
			{
				Debug.Log("Purchase Product ERROR!!~~" + purchaseStatus);
			}
			Object.Destroy(base.gameObject);
		}
	}
}
