using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerLogButton : MonoBehaviour {

	public int playerIndex = 0;
	public GameObject checkObject;
	public goToScene sceneToGo;

	public void Clicked(string playerName, int turmaIndex, GameObject objectToHide){
		if (!PersistenceData.control.getPlayerLogged (playerIndex)) {
			PersistenceData.control.setPlayerName (playerIndex, playerName);
			PersistenceData.control.setTurmaIndex (playerIndex, turmaIndex);
			checkObject.SetActive (true);
			objectToHide.SetActive (false);
		}
		if (PersistenceData.control.getNumberOfLoggedPlayers () >= PersistenceData.control.getNumberOfActivePlayers()) {
			PersistenceData.control.setActualPlayerIndex(0);
			sceneToGo.selectScene ();
		}
	}
}
