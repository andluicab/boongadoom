﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AddActualPlayerNameToText : MonoBehaviour {

	public Text text;

	void Start () {
		string playerName = PersistenceData.control.getPlayerName( PersistenceData.control.getActualPlayerIndex () );
		if (playerName != "" && playerName != null && playerName != "Anônimo") {
			text.text += playerName;
		} else {
			text.text += "PLAYER " + (PersistenceData.control.getActualPlayerIndex () + 1).ToString();
		}
	}
}
