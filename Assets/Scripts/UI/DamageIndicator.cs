using UnityEngine;
using UnityEngine.UI;
using MLAPI;
using MLAPI.Spawning;

public class DamageIndicator : NetworkedBehaviour {

	public Text damageText;
	private float timer;

	private void Start() {
		timer = 1.5f;
		Destroy(gameObject, timer);
	}

	private void Update() {
		transform.LookAt(SpawnManager.GetLocalPlayerObject().transform);
		transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
		damageText.transform.Translate(0, 0.01f, 0);
	}

	public void SetText(float damage, bool critical) {
		damageText.color = critical ? Color.red : Color.black;
		damageText.text = damage.ToString();
	}

}
