using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AddChosenPlayerNameAndScoreToText : MonoBehaviour {

	public Text text;
	public int playerIndex = 0;
	public bool showName = true;
	public bool showScore = false;

	void Start () {
		if (playerIndex < PersistenceData.control.getNumberOfActivePlayers ()) {
			string playerName = PersistenceData.control.getPlayerName (playerIndex);

			if (showName) {
				if (playerName != "" && playerName != null && playerName != "Anônimo") {
					text.text += playerName;
				} else {
					text.text = "PLAYER " + (playerIndex + 1).ToString ();
				}
				if (showScore) {
					text.text += " - " + PersistenceData.control.getScore (playerIndex).ToString ();
				}
			} else {
				if (showScore) {
					text.text = PersistenceData.control.getScore (playerIndex).ToString ();
				}
			}
		}
	}

}
