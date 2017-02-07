using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour {

	public CubeTop cubeTop;
	public Collider colliderFloor;
	public Collider colliderWall;
	public bool canDigThis = true;
	public float digMaxLife = 10f;

	public virtual void Awake(){
		cubeTop.setCube (this);
	}

	public virtual int GetHeight(){
		Cube cubeToCheck = cubeTop.cubeOnTopScript;
		int height = 1;

		while (cubeToCheck != null) {
			cubeToCheck = cubeToCheck.cubeTop.cubeOnTopScript;
			height++;
		}

		return height;
	}

	public virtual void TakeDamage(float damage){
		if (damage < digMaxLife) {
			digMaxLife -= damage;
		} else {
			digMaxLife = 0;
			CubeLifeZero ();
		}
	}

	public virtual void CubeLifeZero(){
		colliderFloor.enabled = false;
		colliderWall.enabled = false;
	}
}
