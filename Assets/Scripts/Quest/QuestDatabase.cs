using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(), System.Serializable]
public class QuestDatabase : SerializedScriptableObject {
	public string databaseName;
	public List<Quest> quests;
}
