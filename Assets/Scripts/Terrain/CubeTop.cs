using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeTop : MonoBehaviour {

	Cube cube;
	public GameObject cubeOnTopObject;
	public Cube cubeOnTopScript;

	public virtual void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == Tags.wall) {
			cubeOnTopObject = other.gameObject;
			cubeOnTopScript = cubeOnTopObject.GetComponent<Cube> ();
		}
	}

	public virtual void OnTriggerExit(Collider other) {
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

	public void setCube(Cube cube){
		this.cube = cube;
	}

	public Cube getCube(){
		return cube;
	}
}
