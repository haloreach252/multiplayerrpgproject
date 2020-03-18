using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;

public class InventoryUI : NetworkedBehaviour {

	public GameObject inventoryUIPrefab;
	private InventoryParent inventoryParent;

	private InventorySlot[] slots;

	private Inventory inventory;
	public bool isOpen;

	private void Start() {
		isOpen = false;
	}

	public void SetupUI(Inventory inventory) {
		this.inventory = inventory;

		// Creates the inventory UI prefab and gets the inventoryParent component of it
		GameObject uiPrefab = Instantiate(inventoryUIPrefab, transform);
		inventoryParent = uiPrefab.GetComponent<InventoryParent>();
		inventoryParent.parentUI = this;

		// Gets the amount of slots to generate, then finds the amount of rows for dynamic inventory size. Calculates based off of columns (default 6)
		int slotAmount = inventory.GetInventorySize();
		int rows = Mathf.CeilToInt(slotAmount / 6.0f);
		if (rows < 1) rows = 1;

		Vector2 panelSize = new Vector2(645, (rows * 90) + ((rows + 1) * 15));
		inventoryParent.GetComponent<RectTransform>().sizeDelta = panelSize;
		inventoryParent.inventoryPanel.GetComponent<RectTransform>().sizeDelta = panelSize;
		inventoryParent.closeButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(panelSize.x / 2, panelSize.y / 2);

		for (int i = 0; i < slotAmount; i++) {
			Instantiate(inventoryParent.slotPrefab, inventoryParent.inventoryPanel.transform);
		}
		slots = inventoryParent.inventoryPanel.GetComponentsInChildren<InventorySlot>();

		foreach (InventorySlot slot in slots) {
			slot.parentInventory = this;
		}

		inventoryParent.gameObject.SetActive(isOpen);

		UpdateUi();
	}

	public void ToggleUI() {
		isOpen = !isOpen;
		inventoryParent.gameObject.SetActive(isOpen);
	}

	public bool Transfer(ItemStack item, InventoryUI ui) {
		bool playerAdded = ui.Add(item.item);
		if (playerAdded) {
			inventory.RemoveItem(item);
			UpdateUi();
			return true;
		} else {
			return false;
		}
	}

	public bool Add(Item item) {
		bool a = inventory.AddItem(item);
		UpdateUi();
		return a;
	}

	public bool RemoveItem(ItemStack item) {
		inventory.RemoveItem(item);
		UpdateUi();
		if (item.currentCount == 0) {
			return true;
		}
		return false;
	}

	public void UpdateUi() {
		for (int i = 0; i < slots.Length; i++) {
			if (i < inventory.GetCurrentInventorySize()) {
				slots[i].AddItem(inventory.GetItem(i));
			} else {
				slots[i].ClearSlot();
			}
		}
	}

}
