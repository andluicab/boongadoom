using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeWallCheck : MonoBehaviour {

	public Character character;
	Cube cubeColliding;

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == Tags.wall) {
			cubeColliding = other.gameObject.GetComponent<Cube> ();
		}
	}

	void OnTriggerExit(Collider other){
		if (other.gameObject.tag == Tags.wall) {
			if (cubeColliding == other.gameObject.GetComponent<Cube> ()) {
				cubeColliding = null;
			}
		}
	}

	public Cube getCubeColliding(){
		return cubeColliding;
	}
}
