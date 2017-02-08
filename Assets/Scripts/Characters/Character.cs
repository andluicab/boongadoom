using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

	public Animator animator;
	public HashAnimatorCharacter hashAnimator;
	public MoveToDirection3D moveToDirection3D;
	Directions3D directionBeforeVertical;
	public FloorCheck floorCheck;
	public CubeWallCheck wallCheckTop;
	public CubeWallCheck wallCheckDown;
	public bool changeDirectionToBack = false;
	public float speed = 3f;
	public GameObject characterClickArea;
	public GameObject selectedObjects;
	public float timerDigFloor = 3f;
	public float digFloorDamage = 1f;
	Cube digFloorCube;
	public float timerDigWall = 3f;
	public float digWallDamage = 1f;
	Cube digWallCubeTop;
	Cube digWallCubeDown;
	public float timerClimb = 3f;
	public int climbMaxHeight = 2;
	public float climbSpeed = 1f;
	CubeTop lastBlockTopClimb;
	bool isFalling = false;
	public float fallSpeed = 5f;
	public float maxFallToLive = 3.5f;
	float yFallStart = 0;
	public float timerBlock = 3f;
	public BlockSkill blockSkill;
	public bool chooseBlockDirection = true;
	public GameObject blockArrows;
	Vector3 blockTargetPosition;
	public float timerParachute = 3f;
	public float parachuteSpeed = 2f;
	public float parachuteAngle = 0.25f;
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

		if (isFalling) {
			if (usingSkill && action == CharacterActionsEnum.parachute) {
				ParachuteStart ();
			}
			if (!usingSkill && action != CharacterActionsEnum.climb && action != CharacterActionsEnum.parachute) {
				NoAction ();
			}
		}

		if(usingSkill && !doingSkill) {
			if (action == CharacterActionsEnum.block) {
				if (Vector3.Distance (blockTargetPosition, transform.position) < 0.1f) {
					doingSkill = true;
				}
			} else {
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
			case CharacterActionsEnum.block:
				break;
			case CharacterActionsEnum.parachute:
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
				if(wallCheckTop.getCubeColliding() != null){
					digWallCubeTop = wallCheckTop.getCubeColliding();
				}
				if(wallCheckDown.getCubeColliding() != null){
					digWallCubeDown = wallCheckDown.getCubeColliding();
				}
				DigWallStart ();
			}
			if (action == CharacterActionsEnum.climb) {
				ClimbStart (collision.gameObject.GetComponent<Cube>());
			}
			if (action == CharacterActionsEnum.push) {
				PushStart (collision.gameObject.GetComponent<Cube> ());
			}

			if (action != CharacterActionsEnum.digWall && action != CharacterActionsEnum.climb && action != CharacterActionsEnum.push) {
				if (wallCheckTop.getCubeColliding () != null && wallCheckDown.getCubeColliding () != null) {
					ChangeDirection ();
				}
			}
		}
	}

	public virtual void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == Tags.characterBlocking) {
			BlockSkill otherBlockSkill = other.gameObject.GetComponent<BlockSkill> ();
			if (otherBlockSkill != blockSkill) {
				if (otherBlockSkill.character.chooseBlockDirection) {
					Directions3D newDirection = otherBlockSkill.direction;
					if (newDirection != Directions3D.top && newDirection != Directions3D.bottom) {
						ChangeDirection (newDirection);
					} else {
						ChangeDirection (DirectionsChanges3D.GetOpositeDirection (moveToDirection3D.direction));
					}
				} else {
					ChangeDirection ();
				}
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

	public virtual void ChangeDirection(Directions3D directionToGo){
		moveToDirection3D.ChangeDirection (directionToGo);

		transform.position = new Vector3 (Mathf.Round (transform.position.x), transform.position.y, Mathf.Round (transform.position.z));
	}

	public virtual void StartAction(CharacterActionsEnum newAction){
		if (usingSkill) {
			StopAllActionAnimations ();
			NoAction ();
		}
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
		if (digWallCubeTop != null) {
			if (digWallCubeTop.digMaxLife > 0) {
				digWallCubeTop.TakeDamage (digWallDamage * Time.deltaTime);
			} else {
				digWallCubeTop = null;
			}
		}
		if (digWallCubeDown != null) {
			if (digWallCubeDown.digMaxLife > 0) {
				digWallCubeDown.TakeDamage (digWallDamage * Time.deltaTime);
			} else {
				digWallCubeDown = null;
			}
		}
		if(digWallCubeTop == null && digWallCubeDown == null) {
			DigWallEnd ();
		}
	}
	public virtual void DigWallEnd(){
		animator.SetBool (hashAnimator.digWall, false);
		NoAction ();
	}
	public virtual void ClimbTimer(){
		timerSkillActual = timerClimb;
	}
	public virtual void ClimbStart(Cube cubeToClimb){
		int wallHeight = 0;
		if (wallCheckDown.getCubeColliding () != null) {
			wallHeight = wallCheckDown.getCubeColliding ().GetHeight ();
		} else {
			if (wallCheckTop.getCubeColliding () != null) {
				wallHeight = wallCheckDown.getCubeColliding ().GetHeight () + 1;
			}
		}
		if (wallHeight > 0 && wallHeight <= climbMaxHeight) {
			doingSkill = true;
			if (moveToDirection3D.direction != Directions3D.top && moveToDirection3D.direction!= Directions3D.bottom) {
				directionBeforeVertical = moveToDirection3D.direction;
			}
			moveToDirection3D.speed = climbSpeed;
			moveToDirection3D.ChangeDirection (Directions3D.top);
			animator.SetBool (hashAnimator.climb, true);
		} else {
			NoAction ();
		}
	}
	public virtual void ClimbAction(){
		if (wallCheckDown.getCubeColliding () != null && lastBlockTopClimb == null) {
			if (wallCheckDown.getCubeColliding ().GetHeight () == 1) {
				lastBlockTopClimb = wallCheckDown.getCubeColliding ().cubeTop;
			}
		}
		if (lastBlockTopClimb != null && floorCheck.transform.position.y >= lastBlockTopClimb.transform.position.y) {
			ClimbEnd ();
		}
	}
	public virtual void ClimbEnd(){
		animator.SetBool (hashAnimator.climb, false);
		transform.position = new Vector3 (Mathf.Round (transform.position.x), Mathf.Round (lastBlockTopClimb.transform.position.y)+1.5f, Mathf.Round (transform.position.z));
		lastBlockTopClimb = null;
		NoAction ();
	}

	public virtual void StartFalling(){
		if (!usingSkill && action != CharacterActionsEnum.climb && action != CharacterActionsEnum.parachute) {
			NoAction ();
			isFalling = true;
			if (moveToDirection3D.direction != Directions3D.top && moveToDirection3D.direction != Directions3D.bottom) {
				directionBeforeVertical = moveToDirection3D.direction;
			}
			yFallStart = transform.position.y;
			moveToDirection3D.ChangeDirection (Directions3D.bottom);
			moveToDirection3D.speed = fallSpeed;
			animator.SetBool (hashAnimator.falling, true);
		}

		if (usingSkill && action == CharacterActionsEnum.parachute) {
			ParachuteStart ();
		}
	}

	public virtual void EndFalling(){
		if (isFalling) {
			isFalling = false;
			if (Mathf.Abs (transform.position.y - yFallStart) < maxFallToLive || action == CharacterActionsEnum.parachute) {
				isFalling = false;
				moveToDirection3D.ChangeDirection (directionBeforeVertical);
				if (action == CharacterActionsEnum.parachute) {
					ParachuteEnd ();
				}
				NoAction ();
				animator.SetBool (hashAnimator.falling, false);
			} else {
				Die ();
			}
		}
	}

	public virtual bool getIsFalling(){
		return isFalling;
	}

	public virtual void BlockTimer(){
		blockArrows.SetActive (true);
	}

	public virtual void BlockArrowClicked(SkillArrowType arrowClicked){
		usingSkill = true;
		blockTargetPosition = new Vector3 (Mathf.Round (wallCheckDown.transform.position.x), transform.position.y, Mathf.Round (wallCheckDown.transform.position.z));

		BlockStart(arrowClicked);
	}

	public virtual void BlockStart(SkillArrowType arrowClicked){
		doingSkill = true;

		timerSkillActual = timerBlock;
		moveToDirection3D.speed = 0f;
		animator.SetBool (hashAnimator.block, true);

		blockArrows.SetActive (false);

		blockSkill.gameObject.SetActive (true);
		if (arrowClicked == SkillArrowType.Back) {
			blockSkill.GoBack ();
		}
		if (arrowClicked == SkillArrowType.ClockWise) {
			blockSkill.ClockWise ();
		}
		if (arrowClicked == SkillArrowType.CounterClockWise) {
			blockSkill.CounterClockWise ();
		}
		transform.position = blockTargetPosition;
	}
	public virtual void BlockEnd(){
		animator.SetBool (hashAnimator.block, false);
		NoAction ();
	}

	public virtual void ParachuteTimer(){
		timerSkillActual = timerParachute;
	}
	public virtual void ParachuteStart(){
		doingSkill = true;
		moveToDirection3D.speed = parachuteSpeed;
		directionBeforeVertical = moveToDirection3D.direction;
		moveToDirection3D.ChangeDirection (new Vector3(0,-(1*(1-parachuteAngle)),1*parachuteAngle));
		animator.SetBool (hashAnimator.parachute, true);
	}
	public virtual void ParachuteEnd(){
		animator.SetBool (hashAnimator.parachute, false);
		doingSkill = false;
		moveToDirection3D.ChangeDirection(directionBeforeVertical);
		moveToDirection3D.speed = speed;
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
		if (!isFalling) {
			if (moveToDirection3D.direction == Directions3D.top || moveToDirection3D.direction == Directions3D.bottom) {
				moveToDirection3D.ChangeDirection (directionBeforeVertical);
			}
			moveToDirection3D.speed = speed;
			action = CharacterActionsEnum.none;
			usingSkill = false;
			doingSkill = false;
			timerSkillActual = 0f;
			timerSkillPassed = 0f;

			blockArrows.SetActive (false);
		}
	}

	public virtual void Die(){
		NoAction();
		animator.SetBool (hashAnimator.isDead, true);
		moveToDirection3D.speed = 0f;
	}

	public virtual void StopAllActionAnimations(){
		animator.SetBool (hashAnimator.digFloor, false);
		animator.SetBool (hashAnimator.digWall, false);
		animator.SetBool (hashAnimator.climb, false);
		animator.SetBool (hashAnimator.block, false);
		animator.SetBool (hashAnimator.parachute, false);
		animator.SetBool (hashAnimator.push, false);
		animator.SetBool (hashAnimator.run, false);
	}

	public virtual void Select(){
		selectedObjects.SetActive (true);
		characterClickArea.SetActive (false);
	}
	public virtual void Deselect(){
		selectedObjects.SetActive (false);

		blockArrows.SetActive (false);

		if (action == CharacterActionsEnum.block && !usingSkill && !doingSkill) {
			NoAction ();
		}

		characterClickArea.SetActive (true);
	}
}
