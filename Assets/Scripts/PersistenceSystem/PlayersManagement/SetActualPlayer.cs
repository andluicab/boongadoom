using UnityEngine;
using System.Collections;

public class SetActualPlayer : MonoBehaviour {

	public int actualPlayer = 0;

	public void SetTheActualPlayer () {
		PersistenceData.control.setActualPlayerIndex (actualPlayer);
	}
}
