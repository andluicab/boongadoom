using UnityEngine;
using System.Collections;

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InputDragAndDropManager : MonoBehaviour {

	GameObject objectBeingDragged;

	void Update () {
		#if UNITY_EDITOR || UNITY_WEBGL
		if (Input.GetMouseButtonDown (0)) {
			StartDrag(Input.mousePosition);
		}
		if (Input.GetMouseButton (0)) {
			if(objectBeingDragged != null){
				MoveDrag(Input.mousePosition);
			}

			if(objectBeingDragged != null){
				MoveDrag(Input.mousePosition);
			}

			if(objectBeingDragged != null){
				if(!objectBeingDragged.activeInHierarchy){
					EndDrag();
				}
			}
		}
		if(Input.GetMouseButtonUp(0)){
			EndDrag();
		}


		#else
		if(Input.touchCount > 0){
			if(Input.touches[0].phase == TouchPhase.Began){
				StartDrag(Input.touches[0].position);
			}
			if(Input.touches[0].phase == TouchPhase.Moved && objectBeingDragged != null){
				MoveDrag(Input.touches[0].position);
			}
			if(Input.touches[0].phase == TouchPhase.Moved && objectBeingDragged != null){
				MoveDrag(Input.touches[0].position);
			}
			if(Input.touches[0].phase == TouchPhase.Ended){
				EndDrag();
			}
			if(objectBeingDragged != null){
				if(!objectBeingDragged.activeInHierarchy){
					EndDrag();
				}
			}
			/*
			Touch[] touches = Input.touches;
			for(int i=0; i<touches.Length; i++ ){
				
			}
			*/
		}else{
			//EndDrag();
		}
		#endif
	}

	public void EndDrag(){
		objectBeingDragged = null;
	}

	public void MoveDrag(Vector3 newPos){
		Vector2 positionToGo = Camera.main.ScreenToWorldPoint (newPos);
		objectBeingDragged.transform.position = positionToGo;
	}

	public void StartDrag(Vector3 point){
		//CODE WITH ONLY ONE RAYCASTHIT
		Ray ray = Camera.main.ScreenPointToRay(point);
		RaycastHit2D hit = Physics2D.Raycast (ray.origin, ray.direction);


		if (hit != null){
			if (hit.collider != null) {
				//the hit.collider gets the object directly clicked. If you use only the hit, you will get the parent
				switch (hit.collider.transform.gameObject.tag) {

				case Tags.card:
					objectBeingDragged = hit.collider.transform.gameObject;

					Vector2 positionToGo = Camera.main.ScreenToWorldPoint (point);
					objectBeingDragged.transform.position = positionToGo;

					break;
				default:
					objectBeingDragged = null;
					break;
				}
			}
		}


		/*multiple raycasthit
		// Construct a ray from the current touch coordinates
		Ray ray = Camera.main.ScreenPointToRay(point);

		// Move to hit
		RaycastHit[] allHits = Physics.RaycastAll(ray);
		List<RaycastHit> closestHit = new List<RaycastHit>();

		bool containsPriority = false;

		if(allHits.Length > 0){
			for(int i=0; i<allHits.Length;i++){
				if( i==0 ){
					if(!closestHit.Contains(allHits[i])){
						closestHit.Add(allHits[i]);
						containsPriority = CheckRaycastHitPriority(allHits[i]) || containsPriority;
					}
				}

				if( CheckRaycastHitPriority(allHits[i]) || !containsPriority ){
					if(!closestHit.Contains(allHits[i])){
						closestHit.Add(allHits[i]);
						containsPriority = CheckRaycastHitPriority(allHits[i]) || containsPriority;
					}
				}
			}

			if(!containsPriority && !HasElementWithTag(closestHit, Tags.wall)){
				MoveAllSelected(closestHit[0].point);
			}

			if(containsPriority && HasElementWithTag(closestHit, Tags.enemyClickArea)){
				closestHit = RemoveElementsWithoutTag(closestHit, Tags.enemyClickArea);
				if (closestHit.Count > 0){
					for(int i=0; i<closestHit.Count; i++){
						AttackWithAllSelected(closestHit[i].collider.transform.parent.gameObject);
					}
				}
			}
			*/

		/*
		if(containsPriority && HasElementWithTag(closestHit, Tags.playerClickArea)){
			closestHit = RemoveElementsWithoutTag(closestHit, Tags.playerClickArea);
			if (closestHit.Count > 0){

				closestHit[0].collider.transform.parent.GetComponent<CharacterMovement>().CharacterClickedDeselectAllButtons();
				RemoveAllFromSelected();

				for(int i=0; i<closestHit.Count; i++){
					closestHit[i].collider.transform.parent.GetComponent<CharacterMovement>().CharacterClickedWithoutDeselectingOthers();
				}
			}
		}
		*/
	}


}

