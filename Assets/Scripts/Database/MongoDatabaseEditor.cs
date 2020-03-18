using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using MongoDB.Driver;
using MongoDB.Bson;
using Newtonsoft.Json;

[System.Serializable, CreateAssetMenu()]
public class MongoDatabaseEditor : SerializedScriptableObject {

	public static MongoDatabaseEditor Singleton;
	public MongoClient mongoConnection;
	public Dictionary<string, MongoDatabase> allDatabases;

	public void Awake() {
		Singleton = this;
	}

	public void OnEnable() {
		Singleton = this;
	}

	public void ConnectClient(string connectionString) {
		mongoConnection = new MongoClient(connectionString);
	}

	public void GetDatabase(string databaseName) {
		if (allDatabases.ContainsKey(databaseName)) {
			allDatabases.Remove(databaseName);
		}
		IMongoDatabase db = mongoConnection.GetDatabase(databaseName);
		Debug.Log(db.DatabaseNamespace.DatabaseName);
		MongoDatabase mdb = new MongoDatabase();
		mdb.database = db;
		allDatabases.Add(databaseName, mdb);
	}

	public void GetDatabaseCollections(string databaseName) {
		Debug.Log("Gettings collections");
		if (allDatabases.ContainsKey(databaseName)) {
			Debug.Log("Got database");
			MongoDatabase db = allDatabases[databaseName];
			Debug.Log(db.database.DatabaseNamespace.DatabaseName);
			List<string> collNames = new List<string>();
			using (IAsyncCursor<string> collCursor = db.database.ListCollectionNames()) {
				while (collCursor.MoveNext()) {
					foreach(var collDoc in collCursor.Current) {
						collNames.Add(collDoc);
					}
				}
			}
			foreach (string name in collNames) {
				if (db.collections.Contains(db.database.GetCollection<BsonDocument>(name))) {
					Debug.Log("Already has");
					return;
				}
				Debug.Log("Doesnt has");
				Debug.Log(db.database.GetCollection<BsonDocument>(name).CollectionNamespace.CollectionName);
				db.collections.Add(db.database.GetCollection<BsonDocument>(name));
			}
		}
	}

}

[System.Serializable]
public class MongoDatabase {
	public IMongoDatabase database;
	public List<IMongoCollection<BsonDocument>> collections;

	public MongoDatabase() {
		collections = new List<IMongoCollection<BsonDocument>>();
	}
}
