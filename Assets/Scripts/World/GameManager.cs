using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using MLAPI;
using MLAPI.Spawning;
using MLAPI.Connection;
using MLAPI.Messaging;

[System.Serializable]
public class GameManager : NetworkedBehaviour {

	public static GameManager Singleton;

	public ItemDatabase itemDatabase;
	public Levels levels;

	public GameObject damageCanvas;

	public List<SpawnPoint> spawnPoints;

	private void Awake() {
		if (Singleton) {
			Destroy(gameObject);
			return;
		}

		Singleton = this;
		ItemDatabase.Singleton = itemDatabase;
		Levels.Singleton = levels;
	}

	private void Start() {
		foreach (NetworkedClient client in NetworkingManager.Singleton.ConnectedClientsList) {
			client.PlayerObject.GetComponent<PlayerCreation>().SetupSpawnPoints();
		}
	}

	public SpawnPoint GetSpawnPoint(int index) {
		return spawnPoints[index];
	}

	public List<SpawnPoint> GetSpawnPoints() {
		return spawnPoints;
	}
}