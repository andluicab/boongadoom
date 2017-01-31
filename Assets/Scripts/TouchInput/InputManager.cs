using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InputManager : MonoBehaviour {

	public static InputManager control;
	public InputButtonWithSlide[] buttonsWithSlide;
	//public Rect clickableArea;
	Character selectedChar;
	SkillButton selectedSkill;
	bool pinching = false;
	public float dragThresholdTime = 0.05f;
	public float pinchThreshold = 1f;
	public float twoFingersThreshold = 0.2f;
	float startDistancePinch = 0f;
	public float mouseScrollSpeed = 10f;
	bool mouseRotating = false;
	bool mouseDragging = false;
	public CameraManager cameraManager;
	public Vector3 mouseLastPosition = new Vector3 (999, 999, 999);
	Vector3 mouseNullPosition = new Vector3 (999, 999, 999);
	float dragTimePassed = 0f;
	bool touchStarted = false;
	bool touchMoved = false;


	float screenWidth = (float)Screen.width/100f;
	//multiply by screen proportion to make the screen move equally even using a percentage
	//float screenHeight = ((float)Screen.height*((float)Screen.width/(float)Screen.height))/100f;
	float screenHeight = (float)Screen.height/100f;

	void Awake(){
		if (control == null) {
			control = this;
		} else {
			Destroy (gameObject);
		}
	}

	void Update () {
		GetInputs ();
	}

	public void RecalculateAllButtons(float rotationZ){
		foreach (InputButtonWithSlide buttonWithSlide in buttonsWithSlide) {
			buttonWithSlide.Recalculate (rotationZ);
		}
	}

	void GetInputs(){
		#if UNITY_WEBGL || UNITY_STANDALONE || UNITY_EDITOR
		if(!Input.touchSupported){
			if (Input.GetMouseButton (0)) {
				AnyButtonsUnpress ();

				for (int j = 0; j < buttonsWithSlide.Length; j++) {
					if (buttonsWithSlide [j].ButtonArea.Contains (Input.mousePosition)) {
						buttonsWithSlide [j].AnyButtonPressed = true;
						if (!buttonsWithSlide [j].ButtonPressed) {
							buttonsWithSlide [j].ButtonPressed = true;
							buttonsWithSlide [j].StartPress ();
						}
					}
				}

			} else {
				AnyButtonsUnpress ();
			}

			if(Input.GetAxis(Buttons.mouseScrollWheel) != 0){
				float zoomToAdd = Input.GetAxis(Buttons.mouseScrollWheel) * mouseScrollSpeed;
				cameraManager.ZoomCamera( zoomToAdd );
			}

			if(Input.GetButtonDown(Buttons.fire1)){
					//mouseLastPosition = Input.mousePosition;
			}

			if(Input.GetButtonDown(Buttons.fire2)){
				if(!mouseDragging){
					mouseRotating = true;
					mouseLastPosition = Input.mousePosition;
				}
			}

			if(Input.GetButton(Buttons.fire2)){
				if(mouseLastPosition != mouseNullPosition && !mouseDragging && mouseRotating){
					cameraManager.RotateCamera( ScreenPositionPercentage(Input.mousePosition) - ScreenPositionPercentage(mouseLastPosition));
					mouseLastPosition = Input.mousePosition;
				}
			}
			if(Input.GetButtonUp(Buttons.fire2)){
				mouseRotating = false;
				mouseLastPosition = mouseNullPosition;
			}

			if(Input.GetButtonDown(Buttons.fire3)){
				if(!mouseRotating){
					mouseDragging = true;
					mouseLastPosition = Input.mousePosition;
				}
			}
			if(Input.GetButton(Buttons.fire3)){
				if(mouseLastPosition != mouseNullPosition && mouseDragging && !mouseRotating){
					cameraManager.DragCamera( ScreenPositionPercentage(Input.mousePosition) - ScreenPositionPercentage(mouseLastPosition));
					mouseLastPosition = Input.mousePosition;
				}
			}
			if(Input.GetButtonUp(Buttons.fire3)){
				mouseDragging = false;
				mouseLastPosition = mouseNullPosition;
			}
		}
		#endif
		#if UNITY_ANDROID || UNITY_IOS
		if(Input.touchCount > 0){
			AnyButtonsUnpress();
			if(/*clickableArea.Contains(Input.GetTouch(0).position) && */!pinching){
				if (Input.GetTouch(0).phase == TouchPhase.Began) {
					touchStarted = true;
				}

				if(touchStarted){
					dragTimePassed += Time.deltaTime;
				}

				//move camera mobile
				if (Input.GetTouch(0).phase == TouchPhase.Moved && Input.touchCount == 1 && Input.touchCount != 2 && !pinching) {

					touchMoved = true;
					/*if(dragTimePassed >= dragThresholdTime){*/
					cameraManager.RotateCamera( DragMovement(Input.GetTouch(0)) );
					/*}*/
				}
			}

			Touch[] touches = Input.touches;

			for(int i=0; i<touches.Length; i++ ){
				for (int j = 0; j < buttonsWithSlide.Length; j++) {
					if(buttonsWithSlide[j].ButtonArea.Contains(touches[i].position)){
					buttonsWithSlide [j].AnyButtonPressed = true;
						if(!buttonsWithSlide [j].ButtonPressed){
							buttonsWithSlide [j].ButtonPressed = true;
							buttonsWithSlide [j].StartPress ();
						}
					}
				}
			}
			if (Input.touchCount == 2){
				//if(clickableArea.Contains(Input.GetTouch(0).position) && clickableArea.Contains(Input.GetTouch(1).position)){
								
					// Store both touches.
					Touch touchZero = Input.GetTouch(0);
					Touch touchOne = Input.GetTouch(1);

					float pinchValue = PinchMovement(touchZero, touchOne);
					float distancePoints = Vector2.Distance(
						ScreenPositionPercentage(touchZero.position), ScreenPositionPercentage(touchOne.position)
					);

					if(!pinching){
						startDistancePinch = distancePoints;
						pinching = true;
					}

					float pinchCompare = 0f;

					if(startDistancePinch > distancePoints){
						pinchCompare = startDistancePinch - distancePoints;
					}else{
						pinchCompare = distancePoints - startDistancePinch;
					}


					if(pinchThreshold < pinchCompare){
						cameraManager.ZoomCamera( pinchValue );
						startDistancePinch = distancePoints;
					}else{
						if(twoFingersThreshold < DragMovement(Input.GetTouch(0)).magnitude){
							cameraManager.DragCamera( DragMovement(Input.GetTouch(0)) );
							startDistancePinch = distancePoints;
						}
					}

					
				//}
			}
		}
		if(Input.touchCount == 0){
			AnyButtonsUnpress();

			//if(Input.touchCount == 0){
			pinching = false;

			//Drag variables
			dragTimePassed = 0f;
			startDistancePinch = 0f;
			touchStarted = false;
			touchMoved = false;
			//}
		}
		#endif
		ReleaseUnusedButtons ();
	}

	Vector2 DragMovement(Touch touch){

		Vector2 touchPosition = new Vector2 (touch.position.x/screenWidth, touch.position.y/screenHeight);

		Vector2 touchOldPosition = touchPosition - new Vector2 (touch.deltaPosition.x/screenWidth, touch.deltaPosition.y/screenHeight); /*(touch.deltaPosition  * screenAdjuster);*/ 
		//touchOldPosition = new Vector2(touchOldPosition.x/screenWidth, touchOldPosition.y/screenHeight);


		Vector2 currentDrag = touchPosition - touchOldPosition;
		//currentDrag = new Vector2(currentDrag.x, currentDrag.y);

		//
//		text.text = touch.position.ToString() + "\ntouchDelta:" + touch.deltaPosition.ToString() + "\nscreenAdjuster" + screenAdjuster + "\ntouchDelta*screenAdjuster" + (touch.deltaPosition*screenAdjuster).ToString() +"\ncurrent drag:"+currentDrag.ToString()+ "\npositionTouch" + touchPosition.ToString() + "\nold:" + touchOldPosition.ToString();
		//

		return currentDrag  /* *screenAdjuster */;
	}

	float PinchMovement(Touch touchZero, Touch touchOne){

		// Find the position in the previous frame of each touch.
		Vector2 touchZeroPrevPos = 
			new Vector2(touchZero.position.x/screenWidth, touchZero.position.y/screenHeight) - new Vector2(touchZero.deltaPosition.x/screenWidth, touchZero.deltaPosition.y/screenHeight);

		Vector2 touchOnePrevPos = 
			new Vector2(touchOne.position.x/screenWidth, touchOne.position.y/screenHeight) - new Vector2(touchOne.deltaPosition.x/screenWidth, touchOne.deltaPosition.y/screenHeight);


		Vector2 touchZeroPosition = new Vector2(touchZero.position.x/screenWidth, touchZero.position.y/screenHeight);
		Vector2 touchOnePosition= new Vector2(touchOne.position.x/screenWidth, touchOne.position.y/screenHeight);

		// Find the magnitude of the vector (the distance) between the touches in each frame.
		float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
		float touchDeltaMag = (touchZeroPosition - touchOnePosition).magnitude;

		// Find the difference in the distances between each frame.
		float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

		return deltaMagnitudeDiff  /* *screenAdjuster*/;
	}

	Vector2 ScreenPositionPercentage(Vector2 screenPosition){
		Vector2 screenPositionPercentage = new Vector2 (screenPosition.x / screenWidth, screenPosition.y / screenHeight);
		return screenPositionPercentage;
	}

	void AnyButtonsUnpress(){
		for (int i = 0; i < buttonsWithSlide.Length; i++) {
			buttonsWithSlide [i].AnyButtonPressed = false;
		}
	}

	void ReleaseUnusedButtons(){
		for (int i = 0; i < buttonsWithSlide.Length; i++) {
			if (!buttonsWithSlide [i].AnyButtonPressed && buttonsWithSlide [i].ButtonPressed) {
				buttonsWithSlide [i].ButtonPressed = buttonsWithSlide [i].AnyButtonPressed;
				buttonsWithSlide [i].EndPress ();
			}
		}
	}

	Rect GetRectFromRectTransformUI(RectTransform rectTransform){
		return new Rect(rectTransform.anchorMin.x*Screen.width,rectTransform.anchorMin.y*Screen.height, GetWidthFromRectTransformUI(rectTransform), GetHeightFromRectTransformUI(rectTransform));
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

	public void setSelectedChar(Character selectedChar){
		this.selectedChar = selectedChar;
	}

	public void setSelectedSkill(SkillButton selectedSkill){
		this.selectedSkill = selectedSkill;
	}

	public void DeselectChar(){
		setSelectedChar (null);
	}

	public void DeselectSkill(){
		setSelectedSkill (null);
	}
}
