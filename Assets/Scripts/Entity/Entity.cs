using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.Spawning;

public class Entity : NetworkedBehaviour {

	public GameObject publicCanvas;
	public Slider healthSlider;

	public float maxHealth = 100;
	protected float health;

	public int level = 1;
	public int xpWorth = 1;
	protected int currentXp = 0;

	protected virtual void Start() {
		health = maxHealth;
		healthSlider.maxValue = maxHealth;
		healthSlider.value = health;
	}

	protected virtual void Update() {
		
	}

	protected virtual void Die(ulong killedById) {
		if(killedById == 99999) {
			Destroy(gameObject);
		} else {
			if (SpawnManager.GetPlayerObject(killedById)) {
				SpawnManager.GetPlayerObject(killedById).GetComponent<PlayerEntity>().GiveXp(xpWorth);
				Debug.LogError("Killed by player: " + SpawnManager.GetPlayerObject(killedById).GetComponent<PlayerUsername>().username);
			}
			
			Destroy(gameObject);
		}
	}

	protected virtual void Attack(Entity enemyEntity) {

	}

	public virtual void TakeDamage(float damage, bool critical, ulong networkId) {
		GameObject damageIndicator = Instantiate(GameManager.Singleton.damageCanvas, transform.position + new Vector3(0, 1 + transform.localScale.y, 0), transform.rotation);
		damageIndicator.GetComponent<NetworkedObject>().Spawn();
		damageIndicator.GetComponent<DamageIndicator>().SetText(damage, critical);
		health -= damage;
		healthSlider.value = health;
		if (health <= 0) {
			Die(networkId);
		}
	}

	public void Heal(float heal) {
		health += heal;
		if(health > maxHealth) {
			health = maxHealth;
		}
		healthSlider.value = health;
	}

}
