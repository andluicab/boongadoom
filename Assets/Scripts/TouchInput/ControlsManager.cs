using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

//[RequireComponent (typeof (CharactersManager))]
[RequireComponent (typeof (CameraManager))]
public class ControlsManager : MonoBehaviour {

	//CharactersManager charactersManager;
	CameraManager cameraManager;
	bool pinching = false;
	public float dragThresholdTime = 0.05f;
	float dragTimePassed = 0f;
	bool touchStarted = false;
	bool touchMoved = false;

	//CharactersOnMissionList charactersOnMissionList;
	bool buttonsOnLeft = false;

	float screenWidth = (float)Screen.width/100f;
	//multiply by screen proportion to make the screen move equally even using a percentage
	float screenHeight = ((float)Screen.height*((float)Screen.width/(float)Screen.height))/100f;

	Rect clickableArea;
	public Button[] allButtons;
	//CharacterSelectButton[] characterSelectButtons;
	Rect[] buttonsRects;

	bool gamePaused = false;
	GameObject[] objectsToShowOnPause;

	float anchorMinLeft = 0f;
	float anchorMaxLeft = 0f;

	float anchorMinRight = 0f;
	float anchorMaxRight = 0f;


	public Texture mytexture;

	//
	Text text;
	//

	void Start () {
		//objectsToShowOnPause = GameObject.FindGameObjectsWithTag(Tags.pauseObject);
		setObjectsToShowOnPause(false);

		//charactersManager = GetComponent<CharactersManager>();
		cameraManager = GetComponent<CameraManager>();

		RectTransform rectTransformOneButton = allButtons[0].GetComponent<RectTransform>();
		anchorMinRight = rectTransformOneButton.anchorMin.x;
		anchorMaxRight = rectTransformOneButton.anchorMax.x;
		anchorMinLeft = 0f;
		anchorMaxLeft = anchorMaxRight - anchorMinRight;


		//charactersOnMissionList = GameObject.FindGameObjectWithTag(Tags.charactersOnMissionList).GetComponent<CharactersOnMissionList>();
		//buttonsOnLeft = charactersOnMissionList.getButtonsOnLeft();
		if(buttonsOnLeft){
			//setButtonsToLeft();
		}
		else{
			//setButtonsToRight();
		}

		setPause(false);


		//
		text = GameObject.FindGameObjectWithTag("debugtext").GetComponent<Text>();
		//
	}
	
	void Update () {

#if !UNITY_IOS
//####MOBILE exit button. IOS don't have an exit button
		if(Input.GetKeyDown(KeyCode.Escape)){
			setPause(!gamePaused);
		}
#endif

#if UNITY_ANDROID || UNITY_WP8 || UNITY_IOS
//####MOBILE
		if(Input.touchCount == 1 && !pinching){
			if(clickableArea.Contains(Input.GetTouch(0).position)){
				if (Input.GetTouch(0).phase == TouchPhase.Began) {
					touchStarted = true;
				}

				if(touchStarted){
					dragTimePassed += Time.deltaTime;
				}
				
				//move camera mobile
				if (Input.GetTouch(0).phase == TouchPhase.Moved) {
					touchMoved = true;
					/*if(dragTimePassed >= dragThresholdTime){*/
						cameraManager.DragCamera( DragMovement(Input.GetTouch(0)) );
					/*}*/
				}

				//move character mobile after threshold time
				/*
				if (touchStarted && !touchMoved && dragTimePassed >= dragThresholdTime) {
					charactersManager.CheckClickedPoint(Input.GetTouch(0).position);
				}
				*/

				//move character mobile
				if (Input.GetTouch(0).phase == TouchPhase.Ended && !touchMoved /*&& dragTimePassed < dragThresholdTime*/) {
				//	charactersManager.CheckClickedPoint(Input.GetTouch(0).position);
				}

			}else{
				//start selecting buttons
				if (Input.GetTouch(0).phase == TouchPhase.Began) {
				//	CharacterSelectButton buttonTouched = CheckButtonsTouched(Input.GetTouch(0).position);
				/*	if(buttonTouched != null){
							buttonTouched.DeselectAllCharacters();
							buttonTouched.SelectCharacterWithoutDeselectingOthers();
					}*/
				}

				//drag buttons
				if (Input.GetTouch(0).phase == TouchPhase.Moved) {
					//CharacterSelectButton buttonTouched = CheckButtonsTouched(Input.GetTouch(0).position);
					/*
					if(buttonTouched != null){
						if(!buttonTouched.selected){
							buttonTouched.SelectCharacterWithoutDeselectingOthers();
						}
					}
					*/
				}


			}
		}
		//Zoom mobile
		if (Input.touchCount == 2){
			if(clickableArea.Contains(Input.GetTouch(0).position) && clickableArea.Contains(Input.GetTouch(1).position)){
				pinching = true;
				// Store both touches.
				Touch touchZero = Input.GetTouch(0);
				Touch touchOne = Input.GetTouch(1);
				
				cameraManager.ZoomCamera( PinchMovement(touchZero, touchOne) );
			}
		}
		if(Input.touchCount == 0){
			pinching = false;

			//Drag variables
			dragTimePassed = 0f;
			touchStarted = false;
			touchMoved = false;
		}
#endif
#if UNITY_EDITOR || (!UNITY_ANDROID && !UNITY_WP8 && !UNITY_IOS)
		//####DESKTOP
		//move character desktop
		if(Input.GetAxis("Horizontal") != 0f || Input.GetAxis("Vertical") != 0f){
			cameraManager.DragCamera(new Vector2(-Input.GetAxis("Horizontal"), -Input.GetAxis("Vertical")));
		}
		if(clickableArea.Contains(Input.mousePosition)){
			if (Input.GetMouseButtonDown(0)) {
				//charactersManager.CheckClickedPoint(Input.mousePosition);
			}
			
			if(Input.GetMouseButton(0)){
				dragTimePassed += Time.deltaTime;
			}
			
			if(Input.GetMouseButtonUp(0)){
				dragTimePassed = 0f;
			}
		}
#endif
	}

	float GetWidthFromRectTransformUI(RectTransform rectTransform){
		//the rect is a percentage. the maximum number, 1, represents 100% from the screen width
		//so we multiply the screen width by the rect to get the real width of the button
		float rectWidth = rectTransform.anchorMax.x - rectTransform.anchorMin.x;
		rectWidth *= Screen.width;

		return rectWidth;
	}

	float GetHeightFromRectTransformUI(RectTransform rectTransform){
		//the rect is a percentage. the maximum number, 1, represents 100% from the screen width
		//so we multiply the screen width by the rect to get the real width of the button
		float rectHeight = rectTransform.anchorMax.y - rectTransform.anchorMin.y;
		rectHeight *= Screen.height;
		
		return rectHeight;
	}

	Rect GetRectFromRectTransformUI(RectTransform rectTransform){
		return new Rect(rectTransform.anchorMin.x*Screen.width,rectTransform.anchorMin.y*Screen.height, GetWidthFromRectTransformUI(rectTransform), GetHeightFromRectTransformUI(rectTransform));
	}
	/*
	CharacterSelectButton CheckButtonsTouched(Vector2 position){
		for(int i=0; i<buttonsRects.Length; i++){
			if(buttonsRects[i].Contains(position)){
				return characterSelectButtons[i];
			}
		}
		return null;
	}
*/
	Vector2 DragMovement(Touch touch){

		float screenAdjuster = 1f;
		
		if(touch.deltaTime != 0){
			screenAdjuster = Time.deltaTime / touch.deltaTime;
		}else{
			screenAdjuster = 1f;
		}

		Vector2 touchPosition = new Vector2 (touch.position.x/screenWidth, touch.position.y/screenHeight);
		Vector2 touchOldPosition = touch.position - (touch.deltaPosition  * screenAdjuster); //new Vector2 (touch.deltaPosition.x/screenWidth, touch.deltaPosition.y/screenHeight);
		touchOldPosition = new Vector2(touchOldPosition.x/screenWidth, touchOldPosition.y/screenHeight);


		Vector2 currentDrag = touchPosition - touchOldPosition;

		//currentDrag = new Vector2(currentDrag.x, currentDrag.y);

		//
		text.text = touch.position.ToString() + "\ntouchDelta:" + touch.deltaPosition.ToString() + "\nscreenAdjuster" + screenAdjuster + "\ntouchDelta*screenAdjuster" + (touch.deltaPosition*screenAdjuster).ToString() +"\ncurrent drag:"+currentDrag.ToString()+ "\npositionTouch" + touchPosition.ToString() + "\nold:" + touchOldPosition.ToString();
		//

		return currentDrag  /* *screenAdjuster */;
	}
	
	float PinchMovement(Touch touchZero, Touch touchOne){
		float screenAdjuster = 1f;
		
		if(touchZero.deltaTime != 0){
			screenAdjuster = Time.deltaTime / touchZero.deltaTime;
		}else{
			screenAdjuster = 1f;
		}

		// Find the position in the previous frame of each touch.
		Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
		touchZeroPrevPos = new Vector2(touchZeroPrevPos.x/screenWidth, touchZeroPrevPos.y/screenHeight);
		Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;
		touchOnePrevPos = new Vector2(touchOnePrevPos.x/screenWidth, touchOnePrevPos.y/screenHeight);

		Vector2 touchZeroPosition = new Vector2(touchZero.position.x/screenWidth, touchZero.position.y/screenHeight);
		Vector2 touchOnePosition= new Vector2(touchOne.position.x/screenWidth, touchOne.position.y/screenHeight);

		// Find the magnitude of the vector (the distance) between the touches in each frame.
		float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
		float touchDeltaMag = (touchZeroPosition - touchOnePosition).magnitude;
		
		// Find the difference in the distances between each frame.
		float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

		return deltaMagnitudeDiff  /* *screenAdjuster*/;
	}
	/*
	public void setButtonsToLeft(){
		//get the width from the button, and set the clickable area to remove the buttons width
		RectTransform rectTransform = allButtons[0].GetComponent<RectTransform>();
		float buttonWidth = GetWidthFromRectTransformUI(rectTransform);
		clickableArea = new Rect(buttonWidth,0,Screen.width - buttonWidth, Screen.height);

		characterSelectButtons = new CharacterSelectButton[allButtons.Length];
		buttonsRects = new Rect[allButtons.Length];
		for(int i=0; i<allButtons.Length;i++){
			characterSelectButtons[i] = allButtons[i].GetComponent<CharacterSelectButton>();

			RectTransform buttonRectTransform = allButtons[i].GetComponent<RectTransform>();
			buttonRectTransform.anchorMin = new Vector2(anchorMinLeft, buttonRectTransform.anchorMin.y);
			buttonRectTransform.anchorMax = new Vector2(anchorMaxLeft, buttonRectTransform.anchorMax.y);

			buttonsRects[i] = GetRectFromRectTransformUI( buttonRectTransform );
			characterSelectButtons[i].setButtonOnLeft(true);
		}
	}
	public void setButtonsToRight(){
		//get the width from the button, and set the clickable area to remove the buttons width
		RectTransform rectTransform = allButtons[0].GetComponent<RectTransform>();
		float buttonWidth = GetWidthFromRectTransformUI(rectTransform);
		clickableArea = new Rect(0,0,Screen.width - buttonWidth, Screen.height);
		
		characterSelectButtons = new CharacterSelectButton[allButtons.Length];
		buttonsRects = new Rect[allButtons.Length];
		for(int i=0; i<allButtons.Length;i++){
			characterSelectButtons[i] = allButtons[i].GetComponent<CharacterSelectButton>();

			RectTransform buttonRectTransform = allButtons[i].GetComponent<RectTransform>();
			buttonRectTransform.anchorMin = new Vector2(anchorMinRight, buttonRectTransform.anchorMin.y);
			buttonRectTransform.anchorMax = new Vector2(anchorMaxRight, buttonRectTransform.anchorMax.y);

			buttonsRects[i] = GetRectFromRectTransformUI( buttonRectTransform );
			characterSelectButtons[i].setButtonOnLeft(false);
		}
	}
	*/
	public void setPause(bool pause){
		if(pause){
			gamePaused = true;
			setObjectsToShowOnPause(true);
			Time.timeScale = 0f;
		}else{
			gamePaused = false;
			setObjectsToShowOnPause(false);
			Time.timeScale = 1f;
		}
	}

	public void StageEndPause(){
		gamePaused = true;
		Time.timeScale = 0f;
	}

	void setObjectsToShowOnPause(bool show){
		foreach(GameObject objectToSet in objectsToShowOnPause){
			objectToSet.SetActive(show);
		}
	}

	public bool getGamePaused(){
		return gamePaused;
	}
}
