using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.Spawning;
using MLAPI.Connection;

public class PlayerEntity : Entity {

	#region Variables im certain will stay
	public Slider playerHealthSlider;
	public List<GameObject> localObjects;

	public GameObject playerCamera;

	private Animator animator;
	private PlayerMove playerMove;
	#endregion

	public float interactionRange = 8;
	public float damage = 15;

	protected override void Start() {
		health = maxHealth;
		healthSlider.maxValue = maxHealth;
		healthSlider.value = health;
		animator = GetComponentInChildren<Animator>();
		playerMove = GetComponent<PlayerMove>();
		xpWorth = 50;

		publicCanvas.SetActive(false);

		if (!IsLocalPlayer) {
			foreach(GameObject obj in localObjects) {
				obj.SetActive(false);
			}
			publicCanvas.SetActive(true);
		} else {
			playerHealthSlider.maxValue = healthSlider.maxValue;
			playerHealthSlider.value = healthSlider.value;
		}
	}

	protected override void Update() {
		publicCanvas.transform.LookAt(SpawnManager.GetLocalPlayerObject().transform);
		publicCanvas.transform.rotation = Quaternion.Euler(0, publicCanvas.transform.rotation.eulerAngles.y, 0);

		if (!IsLocalPlayer) {
			return;
		}

		if (Input.GetKeyDown(KeyCode.Escape)) {
			if (IsHost) {
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
				NetworkingManager.Singleton.StopHost();
				UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
			} else if (IsClient) {
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
				NetworkingManager.Singleton.StopClient();
				UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
			}
		}

		if (EventSystem.current.IsPointerOverGameObject()) return;

		if (Input.GetKeyDown(KeyCode.F)) {
			Cursor.visible = !Cursor.visible;
			Cursor.lockState = Cursor.visible ? CursorLockMode.None : CursorLockMode.Locked;
		}

		if (Input.GetMouseButtonDown(0)) {
			Ray ray = GetComponentInChildren<Camera>().ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;

			if (Physics.Raycast(ray, out hit, interactionRange)) {
				if (hit.transform.GetComponent<Entity>()) {
					Attack(hit.transform.GetComponent<Entity>());
				}
			}
		}
	}

	protected override void Attack(Entity enemyEntity) {
		enemyEntity.TakeDamage(damage, false, NetworkId);
	}

	[ServerRPC(RequireOwnership = false)]
	private void PlayerDamage(float newHealth, ulong networkId) {
		InvokeClientRpcOnEveryone(UpdatePlayer, newHealth, networkId);
	}

	[ClientRPC]
	private void UpdatePlayer(float newHealth, ulong networkId) {
		health = newHealth;
		healthSlider.value = health;
		playerHealthSlider.value = health;
		if (health <= 0) {
			Die(networkId);
		}
	}

	public override void TakeDamage(float damage, bool critical, ulong networkId) {
		GameObject damageIndicator = Instantiate(GameManager.Singleton.damageCanvas, transform.position + new Vector3(0, 1 + transform.localScale.y, 0), transform.rotation);
		damageIndicator.GetComponent<NetworkedObject>().Spawn();
		damageIndicator.GetComponent<DamageIndicator>().SetText(damage, critical);
		health -= damage;
		healthSlider.value = health;
		playerHealthSlider.value = health;
		InvokeServerRpc(PlayerDamage, health, networkId);
	}

	protected override void Die(ulong killedById) {
		if (killedById == 99999) {
			Debug.LogError("Died by unknown");
		} else {
			if (SpawnManager.SpawnedObjects[killedById].GetComponent<PlayerEntity>()) {
				SpawnManager.SpawnedObjects[killedById].GetComponent<PlayerEntity>().GiveXp(xpWorth);
				InvokeServerRpc(DisconnectPlayer, OwnerClientId);
			} else {
				Debug.LogError(SpawnManager.SpawnedObjects[killedById].gameObject.name);
			}
		}
	}

	[ServerRPC]
	private void DisconnectPlayer(ulong playerId) {
		InvokeClientRpcOnClient(Died, playerId);
	}

	[ClientRPC]
	private void Died() {
		if (IsHost) {
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
			NetworkingManager.Singleton.StopHost();
			UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
		} else if (IsClient) {
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
			NetworkingManager.Singleton.StopClient();
			UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
		}
	}

	public void GiveXp(int amount) {
		currentXp += amount;
		if(level < Levels.Singleton.maxLevel) {
			if (currentXp >= Levels.Singleton.GetXpRequirement(level)) {
				currentXp -= Levels.Singleton.GetXpRequirement(level);
				LevelUp();
			}
		}
	}

	private void LevelUp() {
		level++;
	}
}
