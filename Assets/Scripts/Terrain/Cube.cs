using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour {

	public CubeTop cubeTop;

	public int GetHeight(){
		Cube cubeToCheck = cubeTop.cubeOnTopScript;
		int height = 1;

		while (cubeToCheck != null) {
			cubeToCheck = cubeToCheck.cubeTop.cubeOnTopScript;
			height++;
		}

		return height;
	}
}
