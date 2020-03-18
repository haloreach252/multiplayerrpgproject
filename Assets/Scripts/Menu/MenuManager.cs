using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MLAPI;

public class MenuManager : MonoBehaviour {

	public InputField hostPasswordField;

	public InputField joinIpField;
	public InputField joinPortField;
	public InputField joinPasswordField;

	public void NavButtonClick(MenuToggleItem item) {
		item.Toggle();
	}

	public void JoinGame() {

	}

	public void HostGame() {

	}

}