using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableCube : Cube {

	public MoveToDirection3D moveToDirection;
	Character characterPushing;
	bool isPushing = false;

	public void StartPushing(Character character){
		characterPushing = character;
	}

	public void EndPushing(){
		if (characterPushing != null) {
			Debug.Log ("collided");
			characterPushing.PushEnd ();

			moveToDirection.StopMoving ();

			characterPushing = null;
		}
	}

	public void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.tag == Tags.wall) {
			EndPushing ();
		}
	}
}
