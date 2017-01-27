using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CameraManager : MonoBehaviour {

	public Camera cameraToUse;
	public GameObject lookAtObject;
	public GameObject rotationContainer;
	public float zoomSpeed = 0.5f;
	public float minZoom = 4f;
	public float maxZoom = 20f;
	public float dragSpeed = 0.3f;
	public float rotateSpeed = 0.3f;
	public bool blockRotateX = true;
	float dragSpeedWithZoom = 0f;
	public float orthographicSizeCalibrated = 8.4f;
	public float dragMinX;
	public float dragMaxX;
	public float dragMinY;
	public float dragMaxY;

	//
	Text text;
	//

	public void Awake(){

		//
		//text = GameObject.FindGameObjectWithTag("debugtext2").GetComponent<Text>();
		//

		if (cameraToUse == null) {
			cameraToUse = Camera.main;
		}

		if (cameraToUse.orthographic) {
			//zoomMedium = (minZoom+maxZoom)/2;
			//ChangeDragSpeedWithZoom (cameraToUse.orthographicSize);
			dragSpeedWithZoom = dragSpeed;
		} else {
			dragSpeedWithZoom = dragSpeed;
		}

	}


	public void DragCamera(Vector2 drag){
		//
		//text.text = drag.ToString() + "\ndpi: " + Screen.dpi + "\nspeed:" + dragSpeedWithZoom + "\nspeed zoom:" + zoomSpeed;
		//
		lookAtObject.transform.localPosition =
			new Vector3(Mathf.Clamp(lookAtObject.transform.localPosition.x - (drag.x * dragSpeedWithZoom), dragMinX, dragMaxX),
				Mathf.Clamp(lookAtObject.transform.transform.localPosition.y - (drag.y * dragSpeedWithZoom), dragMinY, dragMaxY), 
				lookAtObject.transform.localPosition.z );
	}

	public void RotateCamera(Vector2 drag){
		//
		//text.text = drag.ToString() + "\ndpi: " + Screen.dpi + "\nspeed:" + dragSpeedWithZoom + "\nspeed zoom:" + zoomSpeed;
		//
		if (!blockRotateX) {
			rotationContainer.transform.rotation = Quaternion.Euler (
				new Vector3 (rotationContainer.transform.rotation.eulerAngles.x - (drag.y * rotateSpeed),
					rotationContainer.transform.transform.rotation.eulerAngles.y - (drag.x * rotateSpeed), 
					rotationContainer.transform.rotation.eulerAngles.z)
			);
		} else {
			rotationContainer.transform.rotation = Quaternion.Euler (
				new Vector3 (rotationContainer.transform.rotation.eulerAngles.x,
					rotationContainer.transform.transform.rotation.eulerAngles.y - (drag.x * rotateSpeed), 
					rotationContainer.transform.rotation.eulerAngles.z)
			);
		}
	}

	public void ZoomCamera(float deltaMagnitudeDiff){	
		// If the camera is orthographic...
		if (cameraToUse.orthographic)
		{
			// ... change the orthographic size based on the change in distance between the touches.
			// Make sure the orthographic size is within the min and maxzoom.
			cameraToUse.orthographicSize = Mathf.Clamp(cameraToUse.orthographicSize + (deltaMagnitudeDiff * zoomSpeed), minZoom, maxZoom);

			//ChangeDragSpeedWithZoom(cameraToUse.orthographicSize);
		}
		else
		{
			// ... change the z axis of the camera
			// Make sure the z is within the min and maxzoom.
			cameraToUse.transform.localPosition = 
				new Vector3(cameraToUse.transform.localPosition.x, 
				            cameraToUse.transform.localPosition.y, 
				            Mathf.Clamp(cameraToUse.transform.localPosition.z + (deltaMagnitudeDiff * zoomSpeed), minZoom, maxZoom));
		}


	}

	void ChangeDragSpeedWithZoom(float zoomOrthographicSize){
		//dragSpeedWithZoom = dragSpeed * zoomOrthographicSize/orthographicSizeCalibrated;
	}
}
