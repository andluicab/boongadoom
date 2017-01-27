using UnityEngine;
using System.Collections;

public class MoveToDirection3D : MonoBehaviour {

	public float speed = 1f;
	public bool moveOnAwake = true;
	public Directions3D direction;
	Vector3 directionToMoveVector;
	bool isMoving = false;
	public bool lookAt = true;

	void Awake(){
		isMoving = moveOnAwake;
		ChangeDirection(direction);
	}

	void Update () {
		if(isMoving){
			transform.Translate(directionToMoveVector * Time.deltaTime * speed);
		}
	}

	public void StopMoving(){
		isMoving = false;
	}

	public void StartMoving(){
		isMoving = true;
	}

	public void ChangeDirection(Directions3D direction){
		this.direction = direction;
		if (!lookAt) {
			directionToMoveVector = DirectionsVectors3D.directionVector3D [direction];
		}

		if (lookAt) {
			directionToMoveVector = DirectionsVectors3D.directionVector3D [Directions3D.north];
			transform.rotation = Quaternion.Euler ( new Vector3 (transform.rotation.x, DirectionsVectors3D.DirectionYRotation[direction], transform.rotation.z) );
		}
	}
}
