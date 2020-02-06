using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item : ScriptableObject {

	public long itemId;
	public string itemName;
	public string itemDescription;
	public Sprite itemIcon;

	public int price;

	public int stackSize;

	public EntityStats statRequirement;
	public int levelRequirement;

	public virtual void Use() { }

}
