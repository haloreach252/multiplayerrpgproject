using UnityEngine;
using UnityEngine.UI;

using MLAPI;
using MLAPI.SceneManagement;

using WordPressPCL.Models;

using System.Collections.Generic;

using Newtonsoft.Json;

using Sirenix.OdinInspector;

public class MainMenuController : MonoBehaviour {

	public Text buildNumberTextS;
	public static Text buildNumberText;

	public GameObject menuCamera;
	public Dropdown playerMaterialDropdown;

	private User localUser;

	public List<GameObject> AllSections;

	[Header("Login References")]
	[Space(2)]
	public InputField loginUsernameField;
	public InputField loginPasswordField;
	public Toggle rememberMeToggle;
	public GameObject loginSectionGameObject;
	public GameObject mainSectionGameObject;

	[Space(5)]
	[Header("Register References")]
	[Space(2)]
	public GameObject registerSectionGameObject;
	public InputField registerUsernameField;
	public InputField registerPasswordField;
	public InputField registerEmailField;

	[Space(6)]
	[Header("Host Game References")]
	[Space(2)]
	public InputField hostPortField;
	public InputField hostPasswordField;

	[Space(6)]
	[Header("Join Game Reference")]
	[Space(2)]
	public InputField joinIpField;
	public InputField joinPortField;
	public InputField joinPasswordField;

	[Button("Debug text")]
	private void GetText() {
		buildNumberText = buildNumberTextS;
	}

	private void OnValidate() {
		buildNumberText = buildNumberTextS;
	}

	private void Start() {
		menuCamera.SetActive(true);
		NetworkingManager.Singleton.GetComponent<ServerCallbacks>().ClearCallbacks();
		if (NetworkingManager.Singleton.GetComponent<ConnectionData>().playerUser != null) {
			mainSectionGameObject.SetActive(true);
			loginSectionGameObject.SetActive(false);
		} else {
			if (!string.IsNullOrEmpty(PlayerPrefs.GetString("PlayerUsername")) && !string.IsNullOrEmpty(PlayerPrefs.GetString("PlayerPassword"))) {
				string username = PlayerPrefs.GetString("PlayerUsername");
				string password = PlayerPrefs.GetString("PlayerPassword");

				loginUsernameField.text = username;
				loginPasswordField.text = password;
			}
		}
	}

	private void Update() {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			Application.Quit();
		}
	}

	#region User system
	public void Login() {
		if(loginUsernameField.text == string.Empty || loginPasswordField.text == string.Empty) {
			Debug.LogError("A field is empty, exiting");
			return;
		}

		string loginUser = loginUsernameField.text;
		string loginPass = loginPasswordField.text;
		NetworkLogin.Login(loginUser, loginPass, this);
	}

	public void Login(User user, string username, string password) {
		if (NetworkLogin.loginSuccess) {
			if (rememberMeToggle.isOn) {
				PlayerPrefs.SetString("PlayerUsername", username);
				PlayerPrefs.SetString("PlayerPassword", password);
			}
			localUser = user;
			mainSectionGameObject.SetActive(true);
			loginSectionGameObject.SetActive(false);
			SetConnectionData(NetworkingManager.Singleton.GetComponent<ConnectionData>());
		} else {
			// Todo: Make this more user friendly
			Debug.LogError("Authentication failed, try again");
		}
	}

	public void RegisterButton() {
		if(string.IsNullOrEmpty(registerUsernameField.text) || string.IsNullOrEmpty(registerPasswordField.text) || string.IsNullOrEmpty(registerEmailField.text)) {
			Debug.LogError("A field is missing, exiting");
			return;
		}

		string registerUser = registerUsernameField.text;
		string registerPass = registerPasswordField.text;
		string registerEmail = registerEmailField.text;

		NetworkLogin.Register(registerUser, registerPass, registerEmail, this);
	}

	public void Register(string username, string password) {
		registerSectionGameObject.SetActive(false);
		loginSectionGameObject.SetActive(true);
		NetworkLogin.Login(username, password, this);
	}

	public void Logout() {
		NetworkLogin.loginSuccess = false;
		NetworkLogin.user = null;
		localUser = null;
		NetworkingManager.Singleton.GetComponent<ConnectionData>().playerUser = null;
		foreach(GameObject go in AllSections) {
			go.SetActive(false);
		}
		loginSectionGameObject.SetActive(true);
	}
	#endregion

	public void OpenUI(GameObject open) {
		open.SetActive(true);
	}

	public void CloseUI(GameObject close) {
		close.SetActive(false);
	}

	public void HostGame() {
		NetworkingManager.Singleton.GetComponent<ConnectionData>().playerTexture = GetPlayerTexture();

		menuCamera.SetActive(false);
		NetworkingManager.Singleton.NetworkConfig.ConnectionApproval = true;

		if (hostPasswordField.text != string.Empty) {
			SetHostPassword(hostPasswordField.text);
		}

		//if(hostWhitelistField)

		if(hostPortField.text != string.Empty) {
			NetworkingManager.Singleton.GetComponent<EnetTransport.EnetTransport>().Port = (ushort)int.Parse(hostPortField.text);
		}

		NetworkingManager.Singleton.StartHost();
		NetworkSceneManager.SwitchScene("RPGWorld");
	}

	public void Join() {
		NetworkingManager.Singleton.GetComponent<ConnectionData>().playerTexture = GetPlayerTexture();

		NetworkingManager.Singleton.GetComponent<EnetTransport.EnetTransport>().Address = joinIpField.text;
		NetworkingManager.Singleton.GetComponent<EnetTransport.EnetTransport>().Port = (ushort)int.Parse(joinPortField.text);
		NetworkingManager.Singleton.NetworkConfig.ConnectionApproval = true;

		ConnectionApprovalData data = new ConnectionApprovalData();
		data.username = localUser.UserName;

		if (joinPasswordField.text != string.Empty) {
			data.password = joinPasswordField.text;
			menuCamera.SetActive(false);
		}

		NetworkingManager.Singleton.NetworkConfig.ConnectionData = System.Text.Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(data));

		NetworkingManager.Singleton.StartClient();
	}

	private void SetConnectionData(ConnectionData data) {
		NetworkingManager.Singleton.GetComponent<ServerCallbacks>().AddCallbacks();
		data.playerUser = localUser;
	}

	private void SetHostPassword(string password) {
		NetworkingManager.Singleton.GetComponent<ServerCallbacks>().SetPassword(password);
	}

	private void SetHostWhitelist(List<string> usernames) {
		NetworkingManager.Singleton.GetComponent<ServerCallbacks>().SetWhitelist(usernames);
	}

	public int GetPlayerTexture() {
		return playerMaterialDropdown.value;
	}

	public void ExitGame() {
		Application.Quit();
	}
}
