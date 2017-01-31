using UnityEngine;
using System.Collections;

public class LoadPersistenceDataOnStart : MonoBehaviour {

	void Start () {
		PersistenceData.control.LoadAchievements ();
		PersistenceData.control.LoadHighScores ();
	}
}
