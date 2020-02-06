using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageIndicator : MonoBehaviour {

	public Text damageText;
	private float timer;

	private void Start() {
		timer = 5f;
	}

	private void Update() {
		timer -= Time.deltaTime;
		if(timer <= 0) {
			Destroy(gameObject);
		}

		damageText.transform.Translate(0, 0.01f, 0);
	}

	public void SetText(float damage, bool critical) {
		if (critical) {
			damageText.color = Color.red;
		} else {
			damageText.color = Color.black;
		}

		damageText.text = damage.ToString();
	}

}
