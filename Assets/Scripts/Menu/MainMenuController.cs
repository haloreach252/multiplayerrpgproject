using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MLAPI;
using MLAPI.Spawning;
using MLAPI.SceneManagement;
using MLAPI.Transports.UNET;

public class MainMenuController : MonoBehaviour {

	public InputField ipAddressField;
	public InputField portField;

	public void HostGame() {
		NetworkingManager.Singleton.StartHost();
		NetworkSceneManager.SwitchScene("RPGWorld");
	}

	public void JoinGame() {
		NetworkingManager.Singleton.GetComponent<UnetTransport>().ConnectAddress = ipAddressField.text;
		NetworkingManager.Singleton.GetComponent<UnetTransport>().ConnectPort = int.Parse(portField.text);

		NetworkingManager.Singleton.StartClient();
	}

}
