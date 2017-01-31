using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PersistenceAchievementsHighScoreToText : MonoBehaviour {

	public int stageIndex = 0;
	public Text text;
	bool changed = false;

	void LateUpdate(){
		if (!changed) {
			text.text += PersistenceControllerWithAchievements.control.getHighScore (stageIndex);
			changed = true;
		}
	}
}
