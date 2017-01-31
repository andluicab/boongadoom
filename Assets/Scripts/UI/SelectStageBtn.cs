using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectStageBtn : MonoBehaviour {

	public Button btn;
	public string stageSceneName = "01";
	public goToScene goToSceneScript;

	void Start(){
		if (!PersistenceControllerWithAchievements.control.hasOpenedStage (stageSceneName)) {
			btn.interactable = false;
		}
	}

	public void Clicked(){
		goToSceneScript.selectScene ();
	}
}
