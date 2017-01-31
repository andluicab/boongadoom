using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AddChosenPlayerScoreToText : MonoBehaviour {

	public Text text;
	public int playerIndex = 0;

	void Start () {
		text.text += PersistenceData.control.getScore( playerIndex ).ToString();
	}
}
