using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillButton : MonoBehaviour {

	public CharacterActionsEnum skill;
	public bool selected = false;

	public void Clicked(){
		if (InputManager.control.getSelectedSkill() != this) {
			InputManager.control.setSelectedSkill (this);
			Selected ();
		} else {
			InputManager.control.setSelectedSkill (null);
			Deselected ();
		}
	}

	public void Selected(){
		selected = true;
	}
	public void Deselected(){
		selected = false;
	}
}
