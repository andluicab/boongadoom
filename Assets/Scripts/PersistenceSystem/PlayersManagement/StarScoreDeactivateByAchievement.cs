using UnityEngine;
using System.Collections;

public class StarScoreDeactivateByAchievement : MonoBehaviour {

	public string stageCode;
	public string starNumber;
	bool checkedThePersistence = false;

	void LateUpdate(){
		if (!checkedThePersistence) {
			string achievementCode = stageCode + starNumber;

			bool deactivate = true;

			string[] achievementsGot = PersistenceData.control.getAchievementsActual (0);
			foreach (string achievementGot in achievementsGot) {
				if (achievementGot == achievementCode) {
					deactivate = false;
				}
			}

			if (deactivate) {
				gameObject.SetActive (false);
			}

			checkedThePersistence = true;
		}
	}

}
