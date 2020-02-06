using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using MLAPI;
using MLAPI.SceneManagement;

public class PlayerEntity : Entity {

	public Renderer rend;

	public Material deadMaterial;
	public Material livingMaterial;

	public float interactionRange = 5;
	public float damage = 15;

	public float health;
	public float maxHealth = 500;

	private Animator animator;
	private PlayerMove playerMove;

	override
	protected void Start() {
		health = maxHealth;
		animator = GetComponentInChildren<Animator>();
		playerMove = GetComponent<PlayerMove>();
	}

	private void Update() {
		if (!IsLocalPlayer) {
			return;
		}

		if (!EventSystem.current.IsPointerOverGameObject()) {
			if (Input.GetMouseButtonDown(0)) {
				Ray ray = GetComponentInChildren<Camera>().ScreenPointToRay(Input.mousePosition);
				RaycastHit hit;

				if (Physics.Raycast(ray, out hit, interactionRange, 0)) {
					Debug.Log(hit.transform.name);
					PlayerEntity player = hit.transform.GetComponent<PlayerEntity>();
					if (player != null) {
						player.TakeDamage(damage, false);
					}
				}
			}

			if (Input.GetMouseButtonDown(1)) {
				Ray ray = GetComponentInChildren<Camera>().ScreenPointToRay(Input.mousePosition);
				RaycastHit hit;

				if (Physics.Raycast(ray, out hit, interactionRange, 0)) {
					Debug.Log(hit.transform.name);
					PlayerEntity player = hit.transform.GetComponent<PlayerEntity>();
					if (player != null) {
						player.Heal(damage);
					}
				}
			}
		}

		if (Input.GetKeyDown(KeyCode.Escape)) {
			if (IsHost) {
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
				NetworkSceneManager.SwitchScene("MainMenu");
				NetworkingManager.Singleton.StopHost();
			} else if (IsClient) {
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
				NetworkingManager.Singleton.StopClient();
				UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
			}
		}
	}

	public void TakeDamage(float damage, bool critical) {
		GameObject damageIndicator = Instantiate(GameManager.instance.damageCanvas, transform);
		damageIndicator.transform.Translate(-1, 2, -1);
		damageIndicator.GetComponent<DamageIndicator>().SetText(damage, critical);
		health -= damage;
		if(health <= 0) {
			rend.material = deadMaterial;
		}
	}

	public void Heal(float heal) {
		health += heal;
		if(health > 0) {
			rend.material = livingMaterial;
		}
	}

}
