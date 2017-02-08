using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSkillArrow : MonoBehaviour {

	public Character character;
	public SkillArrowType arrowType;

	public void Clicked(){
		character.BlockArrowClicked (arrowType);
	}
}

public enum SkillArrowType{
	Back,
	ClockWise,
	CounterClockWise
}