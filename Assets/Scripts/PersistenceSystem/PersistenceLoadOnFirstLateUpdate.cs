using UnityEngine;
using System.Collections;

public class PersistenceLoadOnFirstLateUpdate : MonoBehaviour {

	bool loaded = false;

	void LateUpdate () {
		if (!loaded) {
			PersistenceControllerWithName.control.Load ();
			loaded = true;
		}
	}
}
