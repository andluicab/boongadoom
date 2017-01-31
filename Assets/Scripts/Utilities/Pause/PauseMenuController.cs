using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PauseMenuController : MonoBehaviour {

	public static PauseMenuController control;
	public PauseAndUnpause pauseAndUnpause;
	public PauseEvents pauseEvents;
	public UnpauseEvents unpauseEvents;

	void Awake(){
		if (control == null) {
			control = this;
			DontDestroyOnLoad (this);
			SceneManager.activeSceneChanged += control.UnpauseOnLevelLoad;
		} else {
			Destroy (gameObject);
		}
	}

	public static void Pause () {
		control.pauseAndUnpause.Pause();

		if(control.pauseEvents != null){control.pauseEvents.ExecuteEvents();}
	}

	void UnpauseOnLevelLoad(Scene previousScene, Scene newScene){
		Unpause ();
	}

	public static void Unpause(){
		control.pauseAndUnpause.Unpause();

		if(control.unpauseEvents != null){control.unpauseEvents.ExecuteEvents();}
	}
}
