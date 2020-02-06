using UnityEngine.UI;
using UnityEngine;

public class InventorySlot : MonoBehaviour {
	public Image icon;
	public Button removeButton;
	public Text amountText;

	public InventoryUI parentInventory;

	protected ItemStack itemStack;
	protected Item item;

	public void AddItem(ItemStack newItem) {
		itemStack = newItem;
		item = newItem.item;

		amountText.text = newItem.currentCount.ToString();

		icon.sprite = item.itemIcon;
		icon.enabled = true;
		removeButton.interactable = true;
	}

	public void ClearSlot() {
		itemStack = null;
		item = null;

		amountText.text = string.Empty;

		icon.sprite = null;
		icon.enabled = false;
		removeButton.interactable = false;
	}

	public virtual void OnRemoveButton(bool transfer) {
		if (transfer) {
			//parentInventory.Transfer(itemStack, GameManager.instance.playerEntity.GetBehaviour<EntityInventory>().inventoryUI);
		} else {
			parentInventory.RemoveItem(itemStack);
		}
	}

	public void UseItem() {
		if (itemStack != null) {
			itemStack.UseItem();
		}
	}
}
