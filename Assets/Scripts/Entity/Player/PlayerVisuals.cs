 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.Connection;

[System.Serializable]
public class PlayerVisuals : NetworkedBehaviour {

    public GameObject visualObject;
	/*
	private void Start() {
		InvokeServerRpc(ReloadColors);
	}
	/*
	public void ColorUpdate() {
		//visualObject.GetComponent<Renderer>().material.mainTexture = skin.characterTexture;
		//InvokeServerRpc(SendColorToServer, skin);
	}

	[ServerRPC]
	private void ReloadColors() {
		foreach (NetworkedClient client in NetworkingManager.Singleton.ConnectedClientsList) {
			client.PlayerObject.GetComponent<PlayerVisuals>().ColorUpdate(skin);
		}
	}
	/*
	[ServerRPC(RequireOwnership = true)]
	private void SendColorToServer(CharacterSkin color) {
		InvokeClientRpcOnEveryone(SetColorClient, color);
	}

	[ClientRPC]
	private void SetColorClient(CharacterSkin color) {
		visualObject.GetComponent<Renderer>().material.mainTexture = color.characterTexture;
	}*/

}
