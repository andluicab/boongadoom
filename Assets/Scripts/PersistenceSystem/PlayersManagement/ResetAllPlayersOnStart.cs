using UnityEngine;
using System.Collections;

public class ResetAllPlayersOnStart : MonoBehaviour {

	void Start(){
		PersistenceData.control.ResetAllPlayers ();
	}
}
