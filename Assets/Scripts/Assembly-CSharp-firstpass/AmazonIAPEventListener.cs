using System.Collections.Generic;
using UnityEngine;

public class AmazonIAPEventListener : MonoBehaviour
{
	public AmazonIAP_itemDataRequestFailedEvent _itemDataRequestFailedEvent;

	public AmazonIAP_itemDataRequestFinishedEvent _itemDataRequestFinishedEvent;

	public AmazonIAP_purchaseFailedEvent _purchaseFailedEvent;

	public AmazonIAP_purchaseSuccessfulEvent _purchaseSuccessfulEvent;

	public AmazonIAP_purchaseUpdatesRequestFailedEvent _purchaseUpdatesRequestFailedEvent;

	public AmazonIAP_purchaseUpdatesRequestSuccessfulEvent _purchaseUpdatesRequestSuccessfulEvent;

	private void OnEnable()
	{
		AmazonIAPManager.itemDataRequestFailedEvent += itemDataRequestFailedEvent;
		AmazonIAPManager.itemDataRequestFinishedEvent += itemDataRequestFinishedEvent;
		AmazonIAPManager.purchaseFailedEvent += purchaseFailedEvent;
		AmazonIAPManager.purchaseSuccessfulEvent += purchaseSuccessfulEvent;
		AmazonIAPManager.purchaseUpdatesRequestFailedEvent += purchaseUpdatesRequestFailedEvent;
		AmazonIAPManager.purchaseUpdatesRequestSuccessfulEvent += purchaseUpdatesRequestSuccessfulEvent;
		AmazonIAPManager.onSdkAvailableEvent += onSdkAvailableEvent;
		AmazonIAPManager.onGetUserIdResponseEvent += onGetUserIdResponseEvent;
	}

	private void OnDisable()
	{
		AmazonIAPManager.itemDataRequestFailedEvent -= itemDataRequestFailedEvent;
		AmazonIAPManager.itemDataRequestFinishedEvent -= itemDataRequestFinishedEvent;
		AmazonIAPManager.purchaseFailedEvent -= purchaseFailedEvent;
		AmazonIAPManager.purchaseSuccessfulEvent -= purchaseSuccessfulEvent;
		AmazonIAPManager.purchaseUpdatesRequestFailedEvent -= purchaseUpdatesRequestFailedEvent;
		AmazonIAPManager.purchaseUpdatesRequestSuccessfulEvent -= purchaseUpdatesRequestSuccessfulEvent;
		AmazonIAPManager.onSdkAvailableEvent -= onSdkAvailableEvent;
		AmazonIAPManager.onGetUserIdResponseEvent -= onGetUserIdResponseEvent;
	}

	private void itemDataRequestFailedEvent()
	{
		Debug.Log("itemDataRequestFailedEvent");
		if (_itemDataRequestFailedEvent != null)
		{
			_itemDataRequestFailedEvent();
		}
	}

	private void itemDataRequestFinishedEvent(List<string> unavailableSkus, List<AmazonItem> availableItems)
	{
		Debug.Log("itemDataRequestFinishedEvent. unavailable skus: " + unavailableSkus.Count + ", avaiable items: " + availableItems.Count);
		if (_itemDataRequestFinishedEvent != null)
		{
			_itemDataRequestFinishedEvent(unavailableSkus, availableItems);
		}
	}

	private void purchaseFailedEvent(string reason)
	{
		Debug.Log("purchaseFailedEvent: " + reason);
		if (_purchaseFailedEvent != null)
		{
			_purchaseFailedEvent(reason);
		}
	}

	private void purchaseSuccessfulEvent(AmazonReceipt receipt)
	{
		Debug.Log("purchaseSuccessfulEvent: " + receipt);
		if (_purchaseSuccessfulEvent != null)
		{
			_purchaseSuccessfulEvent(receipt);
		}
	}

	private void purchaseUpdatesRequestFailedEvent()
	{
		Debug.Log("purchaseUpdatesRequestFailedEvent");
		if (_purchaseUpdatesRequestFailedEvent != null)
		{
			_purchaseUpdatesRequestFailedEvent();
		}
	}

	private void purchaseUpdatesRequestSuccessfulEvent(List<string> revokedSkus, List<AmazonReceipt> receipts)
	{
		Debug.Log("purchaseUpdatesRequestSuccessfulEvent. revoked skus: " + revokedSkus.Count);
		foreach (AmazonReceipt receipt in receipts)
		{
			Debug.Log(receipt);
		}
		if (_purchaseUpdatesRequestSuccessfulEvent != null)
		{
			_purchaseUpdatesRequestSuccessfulEvent(revokedSkus, receipts);
		}
	}

	private void onSdkAvailableEvent(bool isTestMode)
	{
		Debug.Log("onSdkAvailableEvent. isTestMode: " + isTestMode);
	}

	private void onGetUserIdResponseEvent(string userId)
	{
		Debug.Log("onGetUserIdResponseEvent: " + userId);
	}
}
