using UnityEngine;
using System.Collections;

public class ResetIsPlayerActiveOnStart : MonoBehaviour {
	void Start () {
		PersistenceData.control.resetAllIsPlayersActive ();
	}
}
