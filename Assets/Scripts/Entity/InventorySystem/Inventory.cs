using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Inventory {

	protected int inventorySize;
	[SerializeField]
	protected List<ItemStack> inventory;

    public Inventory(int size) {
        inventorySize = size;
        inventory = new List<ItemStack>();
    }

    public bool AddItem(Item item) {
        foreach (ItemStack itemStack in inventory) {
            if (itemStack.item == item && itemStack.PutItem()) {
                return true;
            }
        }
        if (inventory.Count >= inventorySize) {
            Debug.Log("Not enough room");
            return false;
        }

        inventory.Add(new ItemStack(item));
        return true;
    }

    public bool AddItem(ItemStack itemStack) {
        foreach (ItemStack stack in inventory) {
            if (stack.item == itemStack.item && stack.PutItem(itemStack.currentCount)) {
                return true;
            }
        }
        if (inventory.Count >= inventorySize) {
            Debug.Log("Not enough room");
            return false;
        }

        inventory.Add(itemStack);
        return true;
    }

    public Item RemoveItem(ItemStack item) {
        Item i = item.PullItem();
        if (item.currentCount <= 0) {
            inventory.Remove(item);
        }
        return i;
    }

    public ItemStack GetItem(int index) {
        if (inventory[index] != null) {
            return inventory[index];
        } else {
            return null;
        }
    }

    public int GetInventorySize() {
        return inventorySize;
    }

    public int GetCurrentInventorySize() {
        return inventory.Count;
    }

    public List<ItemStack> GetInventory() {
        return inventory;
    }

}
