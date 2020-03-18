using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.Connection;

[System.Serializable]
public class PlayerUsername : NetworkedBehaviour {

	public string username;
	public Text usernameDisplay;
	public Text playerUsernameText;

	private void Start() {
		InvokeServerRpc(ReloadUsernames);
	}

	public void UsernameUpdate(string username) {
		this.username = username;
		gameObject.name = username;
		usernameDisplay.text = username;
		playerUsernameText.text = username;
		InvokeServerRpc(SendUsernameToServer, username);
	}

	[ServerRPC]
	private void ReloadUsernames() {
		foreach (NetworkedClient client in NetworkingManager.Singleton.ConnectedClientsList) {
			client.PlayerObject.GetComponent<PlayerUsername>().UsernameUpdate(username);
		}
	}

	[ServerRPC(RequireOwnership = true)]
	private void SendUsernameToServer(string username) {
		InvokeClientRpcOnEveryone(SetUsernameClient, username);
	}

	[ClientRPC]
	private void SetUsernameClient(string username) {
		this.username = username;
		gameObject.name = username;
		playerUsernameText.text = username;
		usernameDisplay.text = username;
	}

}
