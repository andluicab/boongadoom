using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HashAnimatorCharacter : MonoBehaviour {

	public int speed;
	public int attacking;
	public int direction;
	public int isDead;
	public int skill;


	void Awake() {
		speed = Animator.StringToHash("Speed");
		attacking = Animator.StringToHash("Attacking");
		direction = Animator.StringToHash("Direction");
		isDead = Animator.StringToHash("IsDead");
		skill = Animator.StringToHash ("Skill");
	}
}
