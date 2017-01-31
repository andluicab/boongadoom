using UnityEngine;
using System.Collections;

public class ActivateObjectsOnPause : MonoBehaviour {

	public GameObject[] objectsToToggle;

	public void ToggledPause(){
		if (PauseMenuController.control.pauseAndUnpause.getPaused ()) {
			ActivateTheObjects ();
		} else {
			DeactivateTheObjects ();
		}
	}

	public void ActivateTheObjects(){
		for (int i = 0; i < objectsToToggle.Length; i++) {
			objectsToToggle[i].SetActive (true);
		}
	}
	public void DeactivateTheObjects(){
		for (int i = 0; i < objectsToToggle.Length; i++) {
			objectsToToggle[i].SetActive (false);
		}
	}
}
