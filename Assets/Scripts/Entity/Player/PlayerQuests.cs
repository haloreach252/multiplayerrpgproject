using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;

public class PlayerQuests : NetworkedBehaviour {

	public List<Quest> playerQuests;

	[ClientRPC]
	private void ProgressQuestTask(QuestTaskEvent e) {

	}

}