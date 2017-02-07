using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillButton : MonoBehaviour {

	public CharacterActionsEnum skill;
	public bool selected = false;
	public ImageUISpriteSwap spriteSwapUI;

	public void Clicked(){
		if (InputManager.control.getSelectedSkill() != this) {
			if (!selected) {
				Selected ();
				InputManager.control.setSelectedSkill (this);
			}
		} else {
			Deselected ();
			InputManager.control.setSelectedSkill (null);
		}
	}

	public void Selected(){
		selected = true;
		spriteSwapUI.SwapToSprite (1);
	}
	public void Deselected(){
		selected = false;
		spriteSwapUI.SwapToSprite (0);
	}
}
