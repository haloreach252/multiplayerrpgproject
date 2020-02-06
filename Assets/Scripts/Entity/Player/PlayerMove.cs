using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;

public class PlayerMove : NetworkedBehaviour {

	public float moveSpeed = 5;
	public float lookSpeed = 25;

	public float lookMin = -25;
	public float lookMax = 40;

	/// <summary>
	/// This is how much you have to move your mouse to look up/down
	/// </summary>
	public float lookDeadzone = 0.4f;

	public GameObject cameraRotate;

	private void Start() {
		if (!IsLocalPlayer) {
			GetComponentInChildren<Camera>().gameObject.SetActive(false);
			return;
		}

		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	private void Update() {
		if (!IsLocalPlayer) {
			return;
		}

		float vertical = Input.GetAxis("Vertical");
		float horizontal = Input.GetAxis("Horizontal");

		transform.Translate(new Vector3(horizontal, 0, vertical) * moveSpeed * Time.deltaTime);
	}

	private void LateUpdate() {
		if (!IsLocalPlayer) {
			return;
		}

		float mouseX = Input.GetAxis("Mouse X") * lookSpeed;
		float mouseY = Input.GetAxis("Mouse Y");
		if((mouseY < lookDeadzone && mouseY > 0) || (mouseY > -lookDeadzone && mouseY < 0)) {
			mouseY = 0;
		} else {
			mouseY = mouseY * lookSpeed * -1;
		}

		transform.Rotate(0, mouseX, 0);
		cameraRotate.transform.Rotate(mouseY, 0, 0);
		float rotX = cameraRotate.transform.localRotation.eulerAngles.x;
		if (rotX > 180) rotX -= 360;
		rotX = Mathf.Clamp(rotX, lookMin, lookMax);
		cameraRotate.transform.localRotation = Quaternion.Euler(rotX, 0, 0);
	}

}
