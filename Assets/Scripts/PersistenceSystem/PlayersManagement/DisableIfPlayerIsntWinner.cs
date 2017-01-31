using UnityEngine;
using System.Collections;

public class DisableIfPlayerIsntWinner : MonoBehaviour {

	public int playerIndex = 0;
	public bool showOnDraw = false;

	void Start () {
		int[] scores = PersistenceData.control.getAllScores ();
		bool disable = false;

		for(int i=0; i< scores.Length; i++) {
			if (i != playerIndex) {
				if (PersistenceData.control.getScore (playerIndex) < PersistenceData.control.getScore (i)) {
					disable = true;
				}
				if (PersistenceData.control.getScore (playerIndex) == PersistenceData.control.getScore (i)) {
					if (showOnDraw) {
						disable = false;
					} else {
						disable = true;
					}
				}
			}
		}

		if (disable) {
			gameObject.SetActive (false);
		}
	}
}
