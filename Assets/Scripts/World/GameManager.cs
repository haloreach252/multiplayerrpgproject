using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public static GameManager instance;

	public ItemDatabase itemDatabase;
	//public LevelProgression levels;
	public List<PlayerEntity> players;
	public List<Entity> entities;

	/// <summary>
	/// This region has references that only need 1 reference project wide.
	/// </summary>
	public GameObject damageCanvas;

	private void Awake() {
		if (instance) {
			return;
		}

		instance = this;
		entities = new List<Entity>();
		players = new List<PlayerEntity>();
	}

}
