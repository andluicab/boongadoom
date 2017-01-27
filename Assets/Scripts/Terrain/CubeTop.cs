using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeTop : MonoBehaviour {

	public GameObject cubeOnTopObject;
	public Cube cubeOnTopScript;

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == Tags.wall) {
			cubeOnTopObject = other.gameObject;
			cubeOnTopScript = cubeOnTopObject.GetComponent<Cube> ();
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.gameObject.tag == Tags.wall) {
			if (other.gameObject == cubeOnTopObject) {
				cubeOnTopObject = null;
				cubeOnTopScript = null;
			}
		}
	}

	public bool HasCubeOnTop(){
		return cubeOnTopObject != null;
	}
}
