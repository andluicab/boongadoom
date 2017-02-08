using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorCheck : MonoBehaviour {
	public Character character;
	CubeTop cubeTopColliding;

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == Tags.floor) {
			if (cubeTopColliding == null) {
				character.EndFalling ();
			}
			cubeTopColliding = other.gameObject.GetComponent<CubeTop> ();
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.gameObject.tag == Tags.floor) {
			if (cubeTopColliding == other.gameObject.GetComponent<CubeTop> ()) {
				cubeTopColliding = null;
			}
		}

		if (cubeTopColliding == null) {
			character.StartFalling ();
		}
	}

	public CubeTop getCubeTopColliding(){
		return cubeTopColliding;
	}
}
