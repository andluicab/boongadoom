using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillButton : MonoBehaviour {

	public CharacterActionsEnum skill;

	public void Clicked(){
		InputManager.control.setSelectedSkill (this);
	}
}
