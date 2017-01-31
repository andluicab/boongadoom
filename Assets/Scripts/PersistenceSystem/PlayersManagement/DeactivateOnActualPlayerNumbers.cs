using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DeactivateOnActualPlayerNumbers : MonoBehaviour {

	public int[] actualPlayerNumbersToDeactivate;

	void Awake () {
		foreach (int playerNumber in actualPlayerNumbersToDeactivate) {
			if (PersistenceData.control.getActualPlayerIndex () == playerNumber) {
				gameObject.SetActive (false);
			}
		}
	}
}
