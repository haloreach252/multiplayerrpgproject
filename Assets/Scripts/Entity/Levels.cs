using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
[CreateAssetMenu()]
public class Levels : ScriptableObject {

	public static Levels Singleton;

	// Starts at level 2
	public List<int> levels;
	public int maxLevel;

	public int GetXpRequirement(int level) {
		return levels[level - 1];
	}

	public int xpAmount = 0;

	[Button(Name = "Add Level")]
	public void AddLevel() {
		levels.Add(xpAmount);
	}

}
