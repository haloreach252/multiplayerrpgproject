using UnityEngine.UI;
using UnityEngine;
using MLAPI;

public class InventorySlot : NetworkedBehaviour {
	public Image icon;
	public Button removeButton;
	public Text amountText;

	public InventoryUI parentInventory;
	public PlayerInventory playerInventory;

	protected ItemStack itemStack;
	protected Item item;

	public void AddItem(ItemStack newItem) {
		itemStack = newItem;
		item = newItem.item;

		GetComponent<Image>().color = Color.white;

		amountText.text = newItem.currentCount.ToString();

		icon.sprite = item.itemIcon;
		icon.enabled = true;
		removeButton.interactable = true;
	}

	public void ClearSlot() {
		itemStack = null;
		item = null;

		GetComponent<Image>().color = Color.gray;

		amountText.text = string.Empty;

		icon.sprite = null;
		icon.enabled = false;
		removeButton.interactable = false;
	}

	public virtual void OnRemoveButton(bool transfer) {
		Debug.LogError("Clicked remove button");
	}

	public void UseItem() {
		if (itemStack != null) {
			itemStack.UseItem();
		}
	}
}
