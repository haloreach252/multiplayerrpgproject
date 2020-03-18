using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public class QuestTask {
	public enum QUEST_TYPE {
		KILL, FETCH, GATHER, DELIVER
	}
	public QUEST_TYPE questType;
	public string taskName;
	public string taskDescription;

	public QuestTaskItem[] questTaskItems;
}

[System.Serializable]
public class QuestTaskItem {
	public int gottenAmount;
	public int requiredAmount;
	public string taskItemName;
}
