using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseBtn : MonoBehaviour {

	public void Clicked(){
		if (!PauseMenuController.control.pauseAndUnpause.getPaused ()) {
			PauseMenuController.Pause ();
		} else {
			PauseMenuController.Unpause ();
		}
	}
}
