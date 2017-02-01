using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

	public Animator animator;
	public HashAnimatorCharacter hashAnimator;
	public MoveToDirection3D moveToDirection3D;
	public FloorCheck floorCheck;
	public bool changeDirectionToBack = false;
	public float speed = 3f;
	public GameObject selectedObjects;
	public float timerDigFloor = 3f;
	public float digFloorDamage = 1f;
	Cube digFloorCube;
	public float timerDigWall = 3f;
	public float digWallDamage = 1f;
	Cube digWallCube;
	public float timerClimb = 3f;
	public int climbMaxHeight = 2;
	public float timerBlock = 3f;
	public float timerParachute = 3f;
	public float timerPush = 3f;
	public float pushSpeed = 3f;
	public float timerRun = 3f;
	public float runSpeed = 6f;
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
				if (action == CharacterActionsEnum.digFloor) {
					if (floorCheck.getCubeTopColliding () != null) {
						if (floorCheck.getCubeTopColliding ().getCube ().canDigThis) {
							DigFloorStart ();
							digFloorCube = floorCheck.getCubeTopColliding ().getCube ();
						}
					}
				}
			} else {
				NoAction ();
			}
		}

		if (doingSkill) {
			switch (action) {
			case CharacterActionsEnum.digFloor:
				DigFloorAction ();
				break;
			case CharacterActionsEnum.digWall:
				DigWallAction ();
				break;
			case CharacterActionsEnum.climb:
				ClimbAction ();
				break;
				/*
			case CharacterActionsEnum.block:
				BlockTimer ();
				break;
				*/
			case CharacterActionsEnum.parachute:
				ParachuteAction ();
				break;
			case CharacterActionsEnum.push:
				PushAction ();
				break;
				/*
			case CharacterActionsEnum.run:
				RunTimer ();
				break;
				*/
			default:
				NoAction ();
				break;
			}
		}
	}

	public virtual void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.tag == Tags.wall) {
			if (action == CharacterActionsEnum.digWall) {
				digWallCube = collision.gameObject.GetComponent<Cube> (); 
				DigWallStart ();
			}
			if (action == CharacterActionsEnum.climb) {
				ClimbStart (collision.gameObject.GetComponent<Cube>());
			}
			if (action == CharacterActionsEnum.push) {
				PushStart (collision.gameObject.GetComponent<Cube> ());
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
			DigFloorTimer ();
			break;
		case CharacterActionsEnum.digWall:
			DigWallTimer ();
			break;
		case CharacterActionsEnum.climb:
			ClimbTimer ();
			break;
		case CharacterActionsEnum.block:
			BlockTimer ();
			break;
		case CharacterActionsEnum.parachute:
			ParachuteTimer ();
			break;
		case CharacterActionsEnum.push:
			PushTimer ();
			break;
		case CharacterActionsEnum.run:
			RunTimer ();
			break;
		default:
			NoAction ();
			break;
		}
	}

	public virtual void DigFloorTimer(){
		timerSkillActual = timerDigFloor;
	}
	public virtual void DigFloorStart(){
		doingSkill = true;
		moveToDirection3D.speed = 0f;
		animator.SetBool (hashAnimator.digFloor, true);
	}
	public virtual void DigFloorAction(){
		if (digFloorCube != null) {
			if (digFloorCube.digMaxLife > 0) {
				digFloorCube.TakeDamage (digFloorDamage*Time.deltaTime);
			} else {
				DigFloorEnd ();
			}
		} else {
			DigFloorEnd ();
		}
	}
	public virtual void DigFloorEnd(){
		animator.SetBool (hashAnimator.digFloor, false);
		digFloorCube = null;
		NoAction ();
	}
	public virtual void DigWallTimer(){
		timerSkillActual = timerDigWall;
	}
	public virtual void DigWallStart(){
		doingSkill = true;
		moveToDirection3D.speed = 0f;
		animator.SetBool (hashAnimator.digWall, true);
	}
	public virtual void DigWallAction(){
		if (digWallCube != null) {
			if (digWallCube.digMaxLife > 0) {
				digWallCube.TakeDamage (digWallDamage*Time.deltaTime);
			} else {
				DigWallEnd ();
			}
		} else {
			DigWallEnd ();
		}
	}
	public virtual void DigWallEnd(){
		animator.SetBool (hashAnimator.digWall, false);
		digWallCube = null;
		NoAction ();
	}
	public virtual void ClimbTimer(){
		timerSkillActual = timerClimb;
	}
	public virtual void ClimbStart(Cube cubeToClimb){
		doingSkill = true;
		animator.SetBool (hashAnimator.climb, true);
	}
	public virtual void ClimbAction(){
	}
	public virtual void ClimbEnd(){
		animator.SetBool (hashAnimator.climb, false);
		NoAction ();
	}
	public virtual void BlockTimer(){
		doingSkill = true;
		timerSkillActual = timerBlock;
		moveToDirection3D.speed = 0f;
		animator.SetBool (hashAnimator.block, true);
	}
	public virtual void BlockEnd(){
		animator.SetBool (hashAnimator.block, true);
		NoAction ();
	}
	public virtual void ParachuteTimer(){
		timerSkillActual = timerParachute;
	}
	public virtual void ParachuteStart(){
		doingSkill = true;
		animator.SetBool (hashAnimator.parachute, true);
	}
	public virtual void ParachuteAction(){
		
	}
	public virtual void ParachuteEnd(){
		animator.SetBool (hashAnimator.parachute, false);
		NoAction ();
	}
	public virtual void PushTimer(){
		timerSkillActual = timerPush;
	}
	public virtual void PushStart(Cube cubeToPush){
		doingSkill = true;
		animator.SetBool (hashAnimator.push, true);
		moveToDirection3D.speed = pushSpeed;
	}
	public virtual void PushAction(){
	
	}
	public virtual void PushEnd(){
		animator.SetBool (hashAnimator.push, false);
		NoAction ();
	}
	public virtual void RunTimer(){
		doingSkill = true;
		timerSkillActual = timerRun;
		moveToDirection3D.speed = runSpeed;
		animator.SetBool (hashAnimator.run, true);
	}
	public virtual void RunEnd(){
		animator.SetBool (hashAnimator.run, false);
		NoAction ();
	}
	public virtual void NoAction(){
		moveToDirection3D.speed = speed;
		action = CharacterActionsEnum.none;
		usingSkill = false;
		doingSkill = false;
		timerSkillActual = 0f;
		timerSkillPassed = 0f;
	}

	public virtual void Select(){
		selectedObjects.SetActive (true);
	}
	public virtual void Deselect(){
		selectedObjects.SetActive (false);
	}
}
