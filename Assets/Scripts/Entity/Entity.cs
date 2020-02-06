using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;

public class Entity : NetworkedBehaviour {

	protected virtual void Start() {
		if (!IsLocalPlayer) {
			return;
		}
	}

}
