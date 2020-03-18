using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Connection;
using MLAPI.Spawning;
using Newtonsoft.Json;

public class ServerCallbacks : MonoBehaviour {

	private string password;
	private List<string> usernames = new List<string>();

	public void SetPassword(string password) {
		this.password = password;
		NetworkingManager.Singleton.NetworkConfig.ConnectionApproval = true;
	}

	public void SetWhitelist(List<string> usernames) {
		this.usernames = usernames;
	}

	public void AddCallbacks() {
		NetworkingManager.Singleton.OnClientDisconnectCallback += ClientDisconnected;
		NetworkingManager.Singleton.OnClientConnectedCallback += ClientConnected;
		NetworkingManager.Singleton.ConnectionApprovalCallback += ClientApproval;
	}

	public void ClearCallbacks() {
		NetworkingManager.Singleton.OnClientDisconnectCallback -= ClientDisconnected;
		NetworkingManager.Singleton.OnClientConnectedCallback -= ClientConnected;
		NetworkingManager.Singleton.ConnectionApprovalCallback -= ClientApproval;
	}

	private void ClientDisconnected(ulong clientId) {
		if (clientId == NetworkingManager.Singleton.LocalClientId) {
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
			UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
		}
	}

	private void ClientConnected(ulong clientId) {
	}

	// TODO: Player authentication and player whitelist
	private void ClientApproval(byte[] connectionData, ulong clientId, NetworkingManager.ConnectionApprovedDelegate callback) {
		bool passwordApprove = false;
		bool usernameApprove = false;

		ConnectionApprovalData data = JsonConvert.DeserializeObject<ConnectionApprovalData>(System.Text.Encoding.ASCII.GetString(connectionData));

		if(password == string.Empty) {
			passwordApprove = true;
		} else {
			if(data.password == password) {
				passwordApprove = true;
			} else {
				passwordApprove = false;
			}
		}

		if(usernames.Count > 0) {
			if (usernames.Contains(data.username)) {
				usernameApprove = true;
			} else {
				usernameApprove = false;
			}
		} else {
			usernameApprove = true;
		}

		bool approve = passwordApprove && usernameApprove ? true : false;
		ulong? prefabHash = SpawnManager.GetPrefabHashFromGenerator("PlayerStart");

		callback(approve, prefabHash, approve, null, null);
	}
}

[System.Serializable]
public class ConnectionApprovalData {
	public string username;
	public string password;
}
