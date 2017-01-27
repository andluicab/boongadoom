using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum Directions3D{
	north,
	east,
	south,
	west,
	top,
	bottom
}

public static class DirectionsVectors3D{
	public static Dictionary<Directions3D, Vector3> directionVector3D = new Dictionary<Directions3D, Vector3>(){
		{ Directions3D.north, Vector3.forward },
		{ Directions3D.east, Vector3.right },
		{ Directions3D.south, Vector3.back },
		{ Directions3D.west, Vector3.left },
		{ Directions3D.top, Vector3.up },
		{ Directions3D.bottom, Vector3.down }
	};
	public static Dictionary<Directions3D, float> DirectionYRotation = new Dictionary<Directions3D, float>(){
		{ Directions3D.north, 0f },
		{ Directions3D.east, 90f },
		{ Directions3D.south, 180f },
		{ Directions3D.west, 270f },
		{ Directions3D.top, 0f },
		{ Directions3D.bottom, 0f }
	};
	public static Dictionary<Directions3D, bool> DirectionIsGrounded = new Dictionary<Directions3D, bool>(){
		{ Directions3D.north, true },
		{ Directions3D.east, true },
		{ Directions3D.south, true },
		{ Directions3D.west, true },
		{ Directions3D.top, false },
		{ Directions3D.bottom, false }
	};
}
