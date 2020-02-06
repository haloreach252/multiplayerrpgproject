using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryParent : MonoBehaviour {
	public GameObject inventoryPanel;
	public GameObject closeButton;
	public GameObject slotPrefab;

	public InventoryUI parentUI;

	public void CloseInventory() {
		parentUI.ToggleUI();
	}
}
