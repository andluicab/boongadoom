using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

	public Animator animator;
	public HashAnimatorCharacter hashAnimator;
	public MoveToDirection3D moveToDirection3D;
	public bool changeDirectionToBack = false;

	public virtual void Update(){
		animator.SetFloat (hashAnimator.speed, moveToDirection3D.speed);
	}

	public virtual void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.tag == Tags.wall) {
			ChangeDirection ();
		}
	}

	public virtual void ChangeDirection(){
		if (changeDirectionToBack) {
			moveToDirection3D.ChangeDirection (DirectionsChanges3D.RandomDirectionGrounded (moveToDirection3D.direction));
		} else {
			moveToDirection3D.ChangeDirection (DirectionsChanges3D.RandomDirectionGrounded90Degrees (moveToDirection3D.direction));
		}

		transform.position = new Vector3 (Mathf.Round (transform.position.x), transform.position.y, Mathf.Round (transform.position.z));
	}
}
