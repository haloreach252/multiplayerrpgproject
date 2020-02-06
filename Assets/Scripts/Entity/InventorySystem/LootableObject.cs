using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootableObject : MonoBehaviour {
	public GameObject interactionObject;

	public int amountMin = 1;
	public int amountMax = 2;

	public LootTable lootTable;

	[SerializeField]
	private Inventory inventory;
	public InventoryUI inventoryUI;

	private void Start() {
		inventoryUI = GetComponent<InventoryUI>();
		inventory = new Inventory(lootTable.amountMax + 1);

		// TODO: Make rarity actually have an effect
		int amount = Random.Range(lootTable.amountMin, lootTable.amountMax);
		for (int i = 0; i < amount; i++) {
			int randIndex = Random.Range(0, lootTable.loot.Count);
			inventory.AddItem(lootTable.loot[randIndex].item);
		}

		inventoryUI.SetupUI(inventory);
	}

	private void OnMouseEnter() {
		interactionObject.SetActive(true);
	}

	private void OnMouseExit() {
		interactionObject.SetActive(false);
	}
}
