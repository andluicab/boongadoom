using UnityEngine;
using System.Collections;

public class DeactivateInNumberOfPlayers : MonoBehaviour {

	public int[] numbersOfPlayersToDeactivate;

	void Start(){
		for (int i = 0; i < numbersOfPlayersToDeactivate.Length; i++) {
			if (PersistenceData.control.getNumberOfActivePlayers () == numbersOfPlayersToDeactivate[i]) {
				gameObject.SetActive (false);
			}
		}
	}

}
