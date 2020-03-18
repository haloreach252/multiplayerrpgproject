using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MLAPI;

public class PlayerInventory : NetworkedBehaviour {

	[Header("References")]
	[Space(2)]
	public GameObject inventoryCanvas;
	public GameObject backgroundPanel;
	//public GameObject closeButton;
	public GameObject slotPrefab;

	[Space(5)]

	public float slotSize = 100;
	public int columns = 3;
	public bool isOpen = false;

	private InventorySlot[] slots;
	private Inventory inventory;

	private void Start() {
		if (!IsLocalPlayer) {
			inventoryCanvas.SetActive(false);
			return;
		}

		isOpen = false;
		SetupUI();
	}

	private void SetupUI() {
		if (!IsLocalPlayer) {
			return;
		}

		inventory = new Inventory(8);

		int slotAmount = inventory.GetInventorySize();
		int rows = Mathf.CeilToInt(slotAmount / (float)columns);
		if (rows < 1) {
			rows = 1;
		}

		Vector2 panelSize = new Vector2((columns * slotSize) + ((columns + 1) * 15), (rows * slotSize) + ((rows + 1) * 15));
		inventoryCanvas.GetComponent<RectTransform>().sizeDelta = panelSize;
		backgroundPanel.GetComponent<RectTransform>().sizeDelta = panelSize;
		//closeButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(panelSize.x / 2, panelSize.y / 2);

		for (int i = 0; i < slotAmount; i++) {
			Instantiate(slotPrefab, backgroundPanel.transform);
		}
		slots = backgroundPanel.GetComponentsInChildren<InventorySlot>();

		foreach (InventorySlot slot in slots) {
			slot.playerInventory = this;
			//slot.parentInventory = this;
		}

		inventoryCanvas.SetActive(isOpen);

		UpdateUI();
	}

	private void UpdateUI() {
		for (int i = 0; i < slots.Length; i++) {
			if(i < inventory.GetCurrentInventorySize()) {
				slots[i].AddItem(inventory.GetItem(i));
			} else {
				slots[i].ClearSlot();
			}
		}
	}

	private void Update() {
		if (!IsLocalPlayer) {
			return;
		}

		if (Input.GetKeyDown(KeyCode.E)) {
			ToggleUI();
		}
	}

	public bool Transfer(ItemStack itemStack, InventoryUI ui) {
		bool playerAdded = ui.Add(itemStack.item);
		if (playerAdded) {
			inventory.RemoveItem(itemStack);
			UpdateUI();
			return true;
		} else {
			return false;
		}
	}

	public bool Add(Item item) {
		bool added = inventory.AddItem(item);
		UpdateUI();
		return added;
	}

	public bool RemoveItem(ItemStack item) {
		inventory.RemoveItem(item);
		UpdateUI();
		if(item.currentCount <= 0) {
			return true;
		}
		return false;
	}

	private void ToggleUI() {
		if (!IsLocalPlayer) {
			return;
		}

		isOpen = !isOpen;
		inventoryCanvas.SetActive(isOpen);
	}

	public void CloseInventory() {
		if (!IsLocalPlayer) {
			return;
		}

		ToggleUI();
	}

}
