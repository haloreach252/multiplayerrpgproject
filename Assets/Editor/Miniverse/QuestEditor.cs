using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector.Editor;
using Sirenix.OdinInspector;
using Sirenix.Serialization.Editor;
using Sirenix.Serialization;
using MongoDB.Driver;
using MongoDB.Bson;
using Newtonsoft.Json;

public class QuestEditor : OdinEditorWindow {
	#region Database Init
	[SerializeField]
	private MongoClient client;
	private IMongoDatabase questDatabase;
	private string connectionString = "mongodb+srv://RPGEditor:Fr34kylinx55%25@miniverserpg-drot6.mongodb.net/test?retryWrites=true&w=majority";

	[MenuItem("Miniverse/Quest Editor")]
	private static void OpenWindow() {
		GetWindow<QuestEditor>().Show();
	}

	protected override void OnEnable() {
		base.OnEnable();
		client = new MongoClient(connectionString);
		questDatabase = client.GetDatabase("quests");
	}

	protected override void OnDestroy() {
		base.OnDestroy();
	}
	#endregion

	public List<QuestDatabase> databases;
	public string questRegionName = "startZone";

	#region Create Quests
	[TabGroup("Create Quest", GroupName = "QuestCreator")]
	public Quest questToCreate;

	[TabGroup("Create Quest", GroupName = "QuestCreator"), Button("Create Quest")]
	public void CreateQuest() {
		IMongoCollection<BsonDocument> collectionToAddTo = questDatabase.GetCollection<BsonDocument>(questRegionName);
		List<BsonDocument> docs = collectionToAddTo.Find(new BsonDocument()).ToList();
		List<Quest> tempQuests = new List<Quest>();
		foreach (var doc in docs) {
			Quest q = QuestUtilities.GetQuest(doc.ToJson());
			tempQuests.Add(q);
		}
		questToCreate.localId = tempQuests[tempQuests.Count - 1].localId + 1;
		string questToJson = JsonConvert.SerializeObject(questToCreate);
		BsonDocument docToAdd = BsonDocument.Parse(questToJson);
		collectionToAddTo.InsertOne(docToAdd);
	}
	#endregion

	#region Delete Quests
	[TabGroup("Delete Quest", GroupName = "QuestCreator")]
	public bool deleteAll;
	[TabGroup("Delete Quest", GroupName = "QuestCreator")]
	public int questIdToDelete;

	[TabGroup("Delete Quest", GroupName = "QuestCreator"), Button("Delete Quest")]
	private void DeleteQuest() {
	}
	#endregion
}

public static class QuestUtilities {
	public static string ValidJson(string brokenJson) {
		var result = Regex.Match(brokenJson, @"ObjectId\(([^\)]*)\)").Value;
		var id = result.Replace("ObjectId(", string.Empty).Replace(")", string.Empty);
		var validJson = brokenJson.Replace(result, id);
		return validJson;
	}

	public static Quest GetQuest(string brokenJson) {
		return JsonConvert.DeserializeObject<Quest>(ValidJson(brokenJson));
	}

	public static void CreateQuestDatabase(string dbName, IMongoCollection<BsonDocument> collection) {
		List<Quest> tempQuests = new List<Quest>();
		List<BsonDocument> docs = collection.Find(new BsonDocument()).ToList();
		foreach (var doc in docs) {
			Quest q = GetQuest(doc.ToJson());
			tempQuests.Add(q);
		}
		if (AssetDatabase.LoadAssetAtPath<QuestDatabase>($"Assets/Quests/Databases/{dbName}QuestDatabase.asset")) {
			QuestDatabase qDb = AssetDatabase.LoadAssetAtPath<QuestDatabase>($"Assets/Quests/Databases/{dbName}QuestDatabase.asset");
			qDb.quests = tempQuests;
			AssetDatabase.SaveAssets();
			EditorUtility.FocusProjectWindow();
		} else {
			QuestDatabase qDb = ScriptableObject.CreateInstance<QuestDatabase>();
			qDb.quests = tempQuests;
			qDb.databaseName = dbName;
			AssetDatabase.CreateAsset(qDb, $"Assets/Quests/Databases/{dbName}QuestDatabase.asset");
			AssetDatabase.SaveAssets();
			EditorUtility.FocusProjectWindow();
		}
	}
}