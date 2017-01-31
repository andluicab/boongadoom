using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class EnableObjectsOnSpecificScenes : MonoBehaviour {

	public string[] scenesToEnableObject;
	public GameObject[] objectsToEnable;
	public SpriteRenderer[] spriteRenderersToHide;
	public int sortNumberToShow = 30;
	public int sortNumberToHide = 0;

	void Awake(){
		EnableEvent ();
		SceneManager.activeSceneChanged += ChangedLevel;
	}

	void ChangedLevel(Scene oldScene, Scene newScene) {
		EnableEvent ();
	}

	void EnableEvent(){
		bool enableCheck = false;
		foreach(string levelString in scenesToEnableObject){
			if(levelString == SceneManager.GetActiveScene().name){
				enableCheck = true;
			}
		}

		foreach(GameObject objectToEnable in objectsToEnable){
			objectToEnable.SetActive(enableCheck);
		}

		foreach(SpriteRenderer spriteRendererToHide in spriteRenderersToHide){
			if(enableCheck){
				spriteRendererToHide.sortingOrder = sortNumberToShow;
			}else{
				spriteRendererToHide.sortingOrder = sortNumberToHide;
			}
		}

	}
}
