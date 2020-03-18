using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MLAPI;
using MLAPI.Messaging;

public class PlayerCreation : NetworkedBehaviour {

	public GameObject playerPrefab;

	public GameObject buttonPrefab;
	public GameObject buttonPanel;

	private void Start() {
		if (!IsLocalPlayer) {
			GetComponentInChildren<Canvas>().gameObject.SetActive(false);
			GetComponentInChildren<Camera>().gameObject.SetActive(false);
			return;
		}
	}

	public override void NetworkStart() {
		if (!IsLocalPlayer || IsHost || IsServer) {
			return;
		}

		SetupSpawnPoints();
	}

	public void SetupSpawnPoints() {
		List<SpawnPoint> spawnPoints = GameManager.Singleton.GetSpawnPoints();
		foreach (SpawnPoint point in spawnPoints) {
			GameObject button = Instantiate(buttonPrefab, buttonPanel.transform);
			button.name = "Button" + point.spawnPointName;
			button.GetComponent<Button>().onClick.AddListener(() => SetSpawnPosition(spawnPoints.IndexOf(point)));
			button.GetComponentInChildren<Text>().text = point.spawnPointName;
			button.GetComponent<Button>().interactable = point.isOpen;
		}
	}

	public void SetSpawnPosition(int posIndex) {
		InvokeServerRpc(CreatePlayer, OwnerClientId, posIndex);
	}

	private void TestingMode() {
		InvokeServerRpc(CreatePlayer, OwnerClientId, 0);
	}

	[ServerRPC]
	public void CreatePlayer(ulong clientId, int spawnPosIndex) {
		SpawnPoint spawnPoint = GameManager.Singleton.GetSpawnPoint(spawnPosIndex);
		if(spawnPoint == null) {
			spawnPoint = GameManager.Singleton.GetSpawnPoint(0);
		}
		if(spawnPoint.isOpen) {
			GameObject player = Instantiate(playerPrefab, spawnPoint.location.position, spawnPoint.location.rotation);
			player.GetComponent<NetworkedObject>().SpawnAsPlayerObject(clientId);
			InvokeClientRpcOnClient(SetupPlayer, clientId, player);
			Destroy(gameObject);
		}
	}

	[ClientRPC]
	private void SetupPlayer(GameObject player) {
		//PlayerCharacter c = player.GetComponent<PlayerCharacter>();
		//c.characterData = NetworkingManager.Singleton.GetComponent<ConnectionData>().characterData;
		//c.Setup();
		/*
		player.GetComponent<PlayerUsername>().username = NetworkingManager.Singleton.GetComponent<ConnectionData>().playerUser.Name;
		player.gameObject.name = player.GetComponent<PlayerUsername>().username;
		player.GetComponent<PlayerUsername>().UsernameUpdate();
		player.GetComponent<PlayerVisuals>().playerTexture = NetworkingManager.Singleton.GetComponent<ConnectionData>().playerTexture;
		player.GetComponent<PlayerVisuals>().ColorUpdate();*/
	}
}
