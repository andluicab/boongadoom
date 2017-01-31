using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PersistenceData : MonoBehaviour {

	public static PersistenceData control;
	const int maxNumberOfPlayers = 2;
	public static int numberOfStages = 26;
	int actualPlayerIndex = 0;
	bool[] isPlayerActive = new bool[maxNumberOfPlayers];
	string[] playerName = new string[maxNumberOfPlayers];
	bool[] playerLogged = new bool[maxNumberOfPlayers];
	int[] difficultyLevel = new int[maxNumberOfPlayers];
	int[] turmaIndex = new int[maxNumberOfPlayers];
	int[] score = new int[maxNumberOfPlayers];
	int[] highScoresOnStages = new int[numberOfStages];
	float[] scorePercentage = new float[maxNumberOfPlayers];
	string[] achievements = new string[maxNumberOfPlayers];

	void Awake () {
		if(control == null){
			DontDestroyOnLoad(this);
			control = this;
		}else{
			Destroy(gameObject);
		}
	}

	void Start(){
		LoadAchievements ();
		LoadHighScores ();
	}

	public void setScorePercentage(int playerIndex, float percentage){
		if (scorePercentage.Length > playerIndex) {
			scorePercentage [playerIndex] = percentage;
		}
	}

	public float getScorePercentage(int playerIndex){
		if (scorePercentage.Length > playerIndex) {
			return scorePercentage[playerIndex];
		}
		return 0f;
	}

	public void setTurmaIndex(int playerIndex,int turma){
		control.turmaIndex[playerIndex] = turma;
	}
	public int getTurmaIndex(int playerIndex){
		return control.turmaIndex[playerIndex];
	}

	public int getActualPlayerIndex(){
		return actualPlayerIndex;
	}

	public void setActualPlayerIndex(int actualPlayerIndex){
		control.actualPlayerIndex = actualPlayerIndex;
	}

	public void setIsPlayerActive(int playerIndex, bool isActive){
		control.isPlayerActive [playerIndex] = isActive;
	}
	public int getNumberOfActivePlayers(){
		int players = 0;
		for (int i = 0; i < control.isPlayerActive.Length; i++) {
			if(control.isPlayerActive [i]){
				players++;
			}
		}
		return players;
	}
	public void resetAllIsPlayersActive(){
		for (int i = 0; i < control.isPlayerActive.Length; i++) {
			control.isPlayerActive [i] = false;
		}
	}

	public int getNextActivePlayer(){
		for (int i = control.actualPlayerIndex+1; i < control.isPlayerActive.Length; i++) {
			if (control.isPlayerActive [i]) {
				return i;
			}
		}
		return 999;
	}

	public bool setPlayerName(int playerIndex,string playerName){
		bool repeatedPlayerName = false;
		for (int i = 0; i < playerIndex; i++) {
			if (control.playerName [i] == playerName) {
				repeatedPlayerName = true;
			}
		}
		if (playerName == "" || playerName == "Anônimo") {
			repeatedPlayerName = false;
		}
		if (!repeatedPlayerName) {
			control.playerName [playerIndex] = playerName;
			control.score [playerIndex] = 0;
			control.playerLogged [playerIndex] = true;
			return true;
		} else {
			return false;
		}
	}
	public bool getPlayerLogged(int playerIndex){
		return control.playerLogged[playerIndex];
	}
	public int getNumberOfLoggedPlayers(){
		int players = 0;
		for (int i = 0; i < control.isPlayerActive.Length; i++) {
			if(control.playerLogged [i]){
				players++;
			}
		}
		return players;
	}
	public string getPlayerName(int playerIndex){
		return control.playerName[playerIndex];
	}
	public void ResetPlayerName(int playerIndex){
		control.playerName [playerIndex] = "";
		control.playerLogged [playerIndex] = false;
	}
	public void ResetAllPlayersName(){
		for (int i=0; i<control.playerName.Length;i++) {
			control.playerName[i] = "";
			control.playerLogged [i] = false;
		}
	}

	public void ResetAllPlayers(){
		for (int i=0; i<control.playerName.Length;i++) {
			control.playerName[i] = "";
			control.playerLogged [i] = false;
		}
		for(int i=0; i<control.score.Length; i++) {
			control.score[i] = 0;
		}
		for(int i=0; i<control.achievements.Length; i++) {
			control.achievements[i] = "";
		}
		for (int i=0; i< control.difficultyLevel.Length; i++) {
			control.difficultyLevel[i] = 0;
		}
		setActualPlayerIndex (0);
	}

	public void setDifficultyLevel(int playerIndex,int difficultyLevel){
		control.difficultyLevel[playerIndex] = difficultyLevel;
	}
	public int getDifficultyLevel(int playerIndex){
		return control.difficultyLevel[playerIndex];
	}

	public void setAchievements(int playerIndex, string achievementName){
		if (achievementName != "" && achievements.Length > playerIndex) {
			if(!control.achievements[playerIndex].Contains(achievementName + ";")){
				control.achievements[playerIndex] += (achievementName + ";");
			}
		}
	}

	public void setScore(int playerIndex,int score){
		control.score[playerIndex] = score;
	}

	public int getScore(int playerIndex){
		if (control.score.Length > playerIndex) {
			return control.score [playerIndex];
		}
		return 0;
	}

	public int[] getAllScores(){
		return score;
	}

	public string[] getAchievementsActual(int playerIndex){
		if (control.achievements.Length > playerIndex) {
			return control.achievements [playerIndex].Split (';');
		}
		return new string[0];
	} 

	public int getMaxNumberOfPlayers(){
		return maxNumberOfPlayers;
	}


	//FUNCTIONS THAT SAVE AND LOAD START
	//WILL BE CHANGED IF API IS USED

	public void LoadAchievements(){
		PersistenceControllerWithName.control.Load ();
		UpdateAchievements (0);
		UpdateAchievements (1);
		achievements [0] = PersistenceControllerWithName.control.getAchievementsText ();
	}

	public void LoadHighScores(){
		PersistenceControllerWithName.control.Load ();
		for(int i=0; i< highScoresOnStages.Length; i++){
			highScoresOnStages [i] = PersistenceControllerWithName.control.getHighScore (i);
		}
	}

	public void UpdateAchievements(int playerIndex){
		if (achievements.Length > playerIndex) {
			if (achievements [playerIndex] != null) {
				string[] achievementsToAdd = achievements [playerIndex].Split (';');
				foreach (string achievementToAdd in achievementsToAdd) {
					if (achievementToAdd != "" && achievementToAdd != null) {
						PersistenceControllerWithName.control.addAchievement (achievementToAdd);
					}
				}
				PersistenceControllerWithName.control.Save ();
			}
		}
	}

	public void UpdateScore(int playerIndex){
		if (control.score.Length > playerIndex){
			PersistenceControllerWithName.control.setHighScore (control.difficultyLevel [playerIndex], control.score[playerIndex]);
			PersistenceControllerWithName.control.Save ();
		}
	}

	//FUNCTIONS THAT SAVE AND LOAD END

}
