using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HashAnimatorCharacter : MonoBehaviour {

	public int speed;
	public int attacking;
	public int direction;
	public int isDead;
	public int skill;

	public int digFloor;
	public int digWall;
	public int climb;
	public int block;
	public int parachute;
	public int push;
	public int run;


	void Awake() {
		speed = Animator.StringToHash("Speed");
		direction = Animator.StringToHash("Direction");
		isDead = Animator.StringToHash("IsDead");
		skill = Animator.StringToHash ("Skill");

		digFloor = Animator.StringToHash ("DigFloor");
		digWall = Animator.StringToHash ("DigWall");
		climb = Animator.StringToHash ("Climb");
		block = Animator.StringToHash ("Block");
		parachute = Animator.StringToHash ("Parachute");
		push = Animator.StringToHash ("Push");
		run = Animator.StringToHash ("Run");
	}
}
