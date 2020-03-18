using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class ItemDatabase : ScriptableObject {

	public static ItemDatabase Singleton;

	public List<Item> items;
	public List<LootTable> lootTables;

	public Item GetRandomItem() {
		return items[Random.Range(0, items.Count)];
	}

	public Item GetItemById(string itemId) {
		for (int i = 0; i < items.Count; i++) {
			if(items[i].itemId == itemId) {
				return items[i];
			}
		}
		return null;
	}

}