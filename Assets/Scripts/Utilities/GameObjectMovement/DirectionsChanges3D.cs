using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DirectionsChanges3D : MonoBehaviour {

	static Directions3D direction;
	static System.Array arrayDirections;
	static List<Directions3D> arrayDirectionsGrounded = new List<Directions3D>();

	public static Directions3D RandomDirection(){
		if (arrayDirections == null) {
			arrayDirections = System.Enum.GetValues (typeof(Directions3D));
		}
		if (arrayDirections.Length > 0) {
			direction = (Directions3D)arrayDirections.GetValue( Random.Range (0, arrayDirections.Length) );
		}

		return direction;
	}

	public static Directions3D RandomDirectionGrounded(){
		if (arrayDirections == null) {
			arrayDirections = System.Enum.GetValues (typeof(Directions3D));
			for (int i = 0; i < arrayDirections.Length; i++) {
				Directions3D directionToCheck = (Directions3D)arrayDirections.GetValue(i);
				if (DirectionsVectors3D.DirectionIsGrounded[directionToCheck]) {
					arrayDirectionsGrounded.Add (directionToCheck);
				}
			}
		}
		if (arrayDirectionsGrounded.Count > 0) {
			direction = arrayDirectionsGrounded[Random.Range (0, arrayDirectionsGrounded.Count)];
		}

		return direction;
	}

	public static Directions3D RandomDirectionGrounded(Directions3D directionToExclude){
		List<Directions3D> directionsToExclude = new List<Directions3D>();
		directionsToExclude.Add(directionToExclude);

		direction = RandomDirectionGrounded(directionsToExclude);

		return direction;
	}

	public static Directions3D RandomDirectionGrounded(List<Directions3D> directionsToExclude){
		if (arrayDirections == null) {
			arrayDirections = System.Enum.GetValues (typeof(Directions3D));
			for (int i = 0; i < arrayDirections.Length; i++) {
				Directions3D directionToCheck = (Directions3D)arrayDirections.GetValue(i);
				if (DirectionsVectors3D.DirectionIsGrounded[directionToCheck]) {
					arrayDirectionsGrounded.Add (directionToCheck);
				}
			}
		}

		List<Directions3D> directionsNoExcluded = new List<Directions3D> ();
		for (int i = 0; i < arrayDirectionsGrounded.Count; i++) {
			if (!directionsToExclude.Contains(arrayDirectionsGrounded[i])) {
				directionsNoExcluded.Add (arrayDirectionsGrounded[i]);
			}
		}
		if (directionsNoExcluded.Count > 0) {
			direction = directionsNoExcluded[Random.Range (0, directionsNoExcluded.Count)];
		}

		return direction;
	}

	public static Directions3D RandomDirectionGrounded90Degrees(Directions3D directionToUse){
		if (Random.Range (0, 2) == 0) {
			direction = Get90ClockwiseDirection(directionToUse);
		} else {
			direction = Get90CounterClockwiseDirection (directionToUse);
		}

		return direction;
	}

	public static Vector3 DirectionVector3(Vector3 origin, Vector3 destiny){
		return (destiny - origin).normalized;
	}

	public static Directions3D GetOpositeDirection(Directions3D direction){
		switch(direction){
			case Directions3D.north:
				return Directions3D.south;
				break;
			case Directions3D.east:
				return Directions3D.west;
				break;
			case Directions3D.south:
				return Directions3D.north;
				break;
			case Directions3D.west:
				return Directions3D.east;
				break;
			case Directions3D.top:
				return Directions3D.bottom;
				break;
			case Directions3D.bottom:
				return Directions3D.top;
				break;
			default:
				return Directions3D.north;
			break;
		}
	}

	public static Directions3D Get90ClockwiseDirection(Directions3D direction){
		switch(direction){
		case Directions3D.north:
			return Directions3D.east;
			break;
		case Directions3D.east:
			return Directions3D.south;
			break;
		case Directions3D.south:
			return Directions3D.west;
			break;
		case Directions3D.west:
			return Directions3D.north;
			break;
		case Directions3D.top:
			return Directions3D.east;
			break;
		case Directions3D.bottom:
			return Directions3D.west;
			break;
		default:
			return Directions3D.north;
			break;
		}
	}

	public static Directions3D Get90CounterClockwiseDirection(Directions3D direction){
		switch(direction){
		case Directions3D.north:
			return Directions3D.west;
			break;
		case Directions3D.east:
			return Directions3D.north;
			break;
		case Directions3D.south:
			return Directions3D.east;
			break;
		case Directions3D.west:
			return Directions3D.south;
			break;
		case Directions3D.top:
			return Directions3D.west;
			break;
		case Directions3D.bottom:
			return Directions3D.east;
			break;
		default:
			return Directions3D.north;
			break;
		}
	}
}
