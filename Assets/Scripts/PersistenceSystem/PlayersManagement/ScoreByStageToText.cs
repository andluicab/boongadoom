using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreByStageToText : MonoBehaviour {

	public int stageIndex;
	public Text text;
	bool settedText = false;

	void LateUpdate () {
		if (!settedText) {
			text.text = PersistenceData.control.getAllScores () [stageIndex].ToString ();
			settedText = true;
		}
	}
}
