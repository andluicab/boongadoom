using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InputButtonWithSlide : MonoBehaviour {

	public RectTransform buttonRect;
	public Image buttonImage;

	Rect buttonArea;
	public Rect ButtonArea{ get{ return buttonArea; }}

	bool buttonPressed = false;
	public bool ButtonPressed{ get{ return buttonPressed; } set{ buttonPressed = value; }}

	bool anyButtonPressed = false;
	public bool AnyButtonPressed{ get{ return anyButtonPressed; } set{ anyButtonPressed = value; }}

	public Sprite buttonSprite;
	public Sprite buttonPressedSprite;

	void Awake () {
		buttonPressed = false;
		anyButtonPressed = false;

		buttonArea = GetRectFromRectTransformUI(buttonRect);
	}

	public void Recalculate(float rotationZ){
		buttonPressed = false;
		anyButtonPressed = false;

		if(rotationZ == 0){
			buttonArea = GetRectFromRectTransformUI(buttonRect);
		}else{
			buttonArea = GetRectFromRectTransformUIReverse(buttonRect);
		}
	}

	Rect GetRectFromRectTransformUI(RectTransform rectTransform){
		return new Rect(rectTransform.anchorMin.x*Screen.width,rectTransform.anchorMin.y*Screen.height, GetWidthFromRectTransformUI(rectTransform), GetHeightFromRectTransformUI(rectTransform));
	}

	Rect GetRectFromRectTransformUIReverse(RectTransform rectTransform){
		return new Rect((1-rectTransform.anchorMax.x)*Screen.width,(1-rectTransform.anchorMax.y)*Screen.height,GetWidthFromRectTransformUI(rectTransform), GetHeightFromRectTransformUI(rectTransform));
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

	public virtual void StartPress(){
		/*
		if (buttonImage != null) {
			if(buttonPressed != null){
				buttonImage.sprite = buttonPressedSprite;
			}
		}
		*/
	}
	public virtual void EndPress(){
		/*
		if (buttonImage != null) {
			if(buttonSprite != null){
				buttonImage.sprite = buttonSprite;
			}
		}
		*/
	}
}
