using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSkill : MonoBehaviour {

	public Character character;
	public Directions3D direction;

	public void ClockWise(){
		direction = DirectionsChanges3D.Get90ClockwiseDirection (character.moveToDirection3D.direction);
	}

	public void CounterClockWise(){
		direction = DirectionsChanges3D.Get90CounterClockwiseDirection (character.moveToDirection3D.direction);
	}

	public void GoBack(){
		direction = Directions3D.top;
	}

}
