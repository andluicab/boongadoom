using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorCheck : MonoBehaviour {
	public Character character;
	CubeTop cubeTopColliding;

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == Tags.floor) {
			cubeTopColliding = other.gameObject.GetComponent<CubeTop> ();
		}
	}

	public CubeTop getCubeTopColliding(){
		return cubeTopColliding;
	}
}
