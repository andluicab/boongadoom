using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;
//#if UNITY_WP8
//To use the base64 on the player prefs, because serialization doesnt work on windows phone
using System.Text;
//#else
//To serialize needs these 2 libraries and the "System" library
/*
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
*/
//#endif


public class PersistenceControllerWithName : MonoBehaviour {

	//Static so you can access the instance in any other class calling PersistenceController.control
	//This is a singleton, so there is only one object of this class instanced.
	//It make sure there is only one on the Awake()
	public static PersistenceControllerWithName control;

	private const int numberOfStages = 26;

	private string achievements;
	private int[] maxPointsHighScore = new int[numberOfStages];

	//#if UNITY_WP8
	private string achievementsKeyToPrefs = "aktp";
	private string[] maxPointsHighScoreKeyToPrefs = new string[numberOfStages];
	//private const string maxPointsHighScoreKeyToPrefs2 = "mphs2";

	//#else
	/*
	public string fileName = "persistentdata.dat";
	private string completeFilePath;
	*/
	//#endif
	

	void Awake() {
		//DontDestroyOnLoad make the object available even when you change the scene.
		//The problem is that you must enter a scene that have the object first
		//The solution is to have one copy of the object on each scene, but when you change scenes
		//the object from previous scene is there too. So you make sure you Destroy this if there is another 
		//object from the class instanced.
		//It can be verified because the variable "control" is static and is unique for every object on the class.
		//So you can verify if any object of the class is in the control static variable.
		if(control == null){
			DontDestroyOnLoad(gameObject);
			control = this;
			control.Init ();
			control.Load ();
		}else if(control != this){
			Destroy (gameObject);
		}

		//#if !UNITY_WP8
		/*
		completeFilePath = Application.persistentDataPath + "/"+fileName;
		*/
		//#endif
	}

	void Init(){
		for (int i = 0; i < maxPointsHighScoreKeyToPrefs.Length; i++) {
			maxPointsHighScoreKeyToPrefs [i] = "mphs" + (i).ToString ();
		}
	}


	//Application.persistentDataPath is an automatic path unity makes to store data.
	//It works even on mobile, it doesnt work only in webplayer (because webplayer dont have anywhere to write files)
	public void Save(){
		//#if UNITY_WP8
		if (achievements != null) {
			PlayerPrefs.SetString (achievementsKeyToPrefs, EncodeString (achievements));
		}
		for (int i = 0; i < maxPointsHighScore.Length; i++) {
			if(maxPointsHighScore[i] > DecodeInt( PlayerPrefs.GetString(maxPointsHighScoreKeyToPrefs[i]) )){
				PlayerPrefs.SetString( maxPointsHighScoreKeyToPrefs[i], EncodeInt(maxPointsHighScore[i]) );
			}
		}


		//#else
		/*
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(completeFilePath);

		PersistentData data = new PersistentData();

		data.maxPointsHighScore = maxPointsHighScore;
		data.maxPointsHighScore2 = maxPointsHighScore2;
		data.maxPointsHighScore3 = maxPointsHighScore3;

		bf.Serialize(file, data);
		file.Close();
		*/
		//#endif
	}

	public void Load(){
		//#if UNITY_WP8
		if(PlayerPrefs.GetString(achievementsKeyToPrefs) != "" && PlayerPrefs.GetString(achievementsKeyToPrefs) != null){
			achievements = DecodeString(PlayerPrefs.GetString(achievementsKeyToPrefs));
		}
		for (int i = 0; i < maxPointsHighScore.Length; i++) {
			if (PlayerPrefs.GetString (maxPointsHighScoreKeyToPrefs[i]) != "" && PlayerPrefs.GetString (maxPointsHighScoreKeyToPrefs[i]) != null) {
				maxPointsHighScore[i] = DecodeInt (PlayerPrefs.GetString (maxPointsHighScoreKeyToPrefs[i]));
			}
		}
		//#else
		/*
		if(File.Exists(completeFilePath)){
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(completeFilePath, FileMode.Open);

			PersistentData data = (PersistentData)bf.Deserialize(file);
			file.Close();

			maxPointsHighScore = data.maxPointsHighScore;
			maxPointsHighScore2 = data.maxPointsHighScore2;
			maxPointsHighScore3 = data.maxPointsHighScore3;
		}
		*/
		//#endif
	}

	//#if UNITY_WP8
	string EncodeInt(int intToEncode){
		byte[] bytesToEncode = Encoding.UTF8.GetBytes (intToEncode.ToString());
		string encodedText = Convert.ToBase64String (bytesToEncode);
		if (encodedText != null) {
			return encodedText;
		} else {
			return 0.ToString();
		}
	}

	int DecodeInt(string textToDecode){
		byte[] decodedBytes = Convert.FromBase64String (textToDecode);
		//the next line didnt worked with only decodedBytes on windows phone
		string decodedText = Encoding.UTF8.GetString (decodedBytes, 0, decodedBytes.Length);
		if (decodedText != null && decodedText != "") {
			return int.Parse (decodedText);
		} else {
			return 0;
		}
	}

	string EncodeString(string stringToEncode){
		byte[] bytesToEncode = Encoding.UTF8.GetBytes (stringToEncode);
		string encodedText = Convert.ToBase64String (bytesToEncode);
		if (encodedText != null) {
			return encodedText;
		} else {
			return "";
		}
	}

	string DecodeString(string textToDecode){
		byte[] decodedBytes = Convert.FromBase64String (textToDecode);
		//the next line didnt worked with only decodedBytes on windows phone
		string decodedText = Encoding.UTF8.GetString (decodedBytes, 0, decodedBytes.Length);
		if (decodedText != null) {
			return decodedText;
		} else {
			return "";
		}
	}
	//#endif

	public void addAchievement(string achievementName){
		if (achievementName != "" && achievementName != null) {

			if (control.achievements == null) {
				control.achievements = "";
			}

			if(!control.achievements.Contains(achievementName + ";")){
				control.achievements += (achievementName + ";");
				Save ();
			}
		}
	}

	public List<String> getAchievements(){
		List<String> achievementsList = new List<String> ();
		string[] achievementsArray = achievements.Split (';');

		foreach(string achievementString in achievementsArray){
			if (achievementString != "") {
				achievementsList.Add (achievementString);
			}
		}

		return achievementsList;
	}

	public String getAchievementsText(){
		if (achievements != null) {
			return achievements;
		} else {
			return "";
		}
	}

	public bool setHighScore(int index, int score){
		if (maxPointsHighScore.Length > index) {
			if (maxPointsHighScore [index] < score) {
				maxPointsHighScore [index] = score;
				Save ();
				return true;
			}
		}
		return false;
	}

	public int getHighScore(int index){
		if (maxPointsHighScore.Length > index) {
			return maxPointsHighScore [index];
		} else {
			return 0;
		}
	}

}

//#if !UNITY_WP8
//To serialize we need a class with only the variables that we will serialize and with [Serializable] before the class name
//Serializing a class with methods can cause problems (something to do with monodevelop compiling or something like that)
/*
[Serializable]
class PersistentData{
	public int maxPointsHighScore;
	public int maxPointsHighScore2;
	public int maxPointsHighScore3;
}
*/
//#endif