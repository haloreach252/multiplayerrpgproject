using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public class Quest {
	public int localId;
	public int regionId;
	public string questName;
	public string questDescription;
	public string questgiverName;

	public QuestTask[] tasks;
}
