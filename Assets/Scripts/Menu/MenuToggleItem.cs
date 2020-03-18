using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable, CreateAssetMenu()]
public class MenuToggleItem : ScriptableObject {

	public GameObject[] objectsToEnable;
	public GameObject[] objectsToDisable;

	public void Toggle() {
		foreach (GameObject go in objectsToEnable) {
			go.SetActive(true);
		}
		foreach (GameObject go in objectsToDisable) {
			go.SetActive(false);
		}
	}

}
