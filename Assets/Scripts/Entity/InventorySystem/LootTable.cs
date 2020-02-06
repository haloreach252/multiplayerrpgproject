using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class LootTable : ScriptableObject {
	public List<ItemDrop> loot;
	public int amountMin = 1;
	public int amountMax = 3;
}

[System.Serializable]
public class ItemDrop {
	public Item item;
	public float percentChance;

	public ItemDrop(Item item, float percentChance) {
		this.item = item;
		this.percentChance = percentChance;
	}
}