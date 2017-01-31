using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

	public Animator animator;
	public HashAnimatorCharacter hashAnimator;
	public MoveToDirection3D moveToDirection3D;
	public bool changeDirectionToBack = false;
	public float speed = 3f;
	public float timerDigFloor = 3f;
	public int digFloorDamage = 1;
	public float timerDigWall = 3f;
	public int digWallDamage = 1;
	public float timerClimb = 3f;
	public int climbMaxHeight = 2;
	public float timerBlock = 3f;
	public float timerParachute = 3f;
	public float timerPush = 3f;
	public float timerRun = 3f;
	bool usingSkill = false;
	bool doingSkill = false;
	float timerSkillActual = 0f;
	float timerSkillPassed = 0f;
	CharacterActionsEnum action = CharacterActionsEnum.none;

	public virtual void Awake(){
		moveToDirection3D.speed = speed;
	}

	public virtual void Update(){
		animator.SetFloat (hashAnimator.speed, moveToDirection3D.speed);
		if(usingSkill && !doingSkill) {
			if (timerSkillPassed < timerSkillActual) {
				timerSkillPassed += Time.deltaTime;
			} else {
				NoAction ();
			}
		}
	}

	public virtual void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.tag == Tags.wall) {
			if (action != CharacterActionsEnum.digWall) {
				DigWallStart (collision.gameObject.GetComponent<Cube>());
			}
			if (action != CharacterActionsEnum.climb) {
				ClimbStart (collision.gameObject.GetComponent<Cube>());
			}
			if (action != CharacterActionsEnum.push) {
				
			}

			if (action != CharacterActionsEnum.digWall && action != CharacterActionsEnum.climb && action != CharacterActionsEnum.push) {
				ChangeDirection ();
			}
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

	public virtual void StartAction(CharacterActionsEnum newAction){
		action = newAction;
		usingSkill = true;

		switch (newAction) {
		case CharacterActionsEnum.digFloor:
			DigFloor ();
			break;
		case CharacterActionsEnum.digWall:
			DigWall ();
			break;
		case CharacterActionsEnum.climb:
			Climb ();
			break;
		case CharacterActionsEnum.block:
			Block ();
			break;
		case CharacterActionsEnum.parachute:
			Parachute ();
			break;
		case CharacterActionsEnum.push:
			Push ();
			break;
		case CharacterActionsEnum.run:
			Run ();
			break;
		default:
			NoAction ();
			break;
		}
	}

	public virtual void DigFloor(){
		timerSkillActual = timerDigFloor;
	}
	public virtual void DigWall(){
		timerSkillActual = timerDigWall;
	}
	public virtual void DigWallStart(Cube cubeToDig){
		doingSkill = true;
	}
	public virtual void Climb(){
		timerSkillActual = timerClimb;
	}
	public virtual void ClimbStart(Cube cubeToClimb){
		
	}
	public virtual void Block(){
		timerSkillActual = timerBlock;
	}
	public virtual void Parachute(){
		timerSkillActual = timerParachute;
	}
	public virtual void Push(){
		timerSkillActual = timerPush;
	}
	public virtual void PushStart(Cube cubeToPush){
		doingSkill = true;
	}
	public virtual void Run(){
		doingSkill = true;
		timerSkillActual = timerRun;
		moveToDirection3D.speed = speed * 2;
	}
	public virtual void NoAction(){
		moveToDirection3D.speed = speed;
		usingSkill = false;
		doingSkill = false;
		timerSkillActual = 0f;
		timerSkillPassed = 0f;
	}
}
