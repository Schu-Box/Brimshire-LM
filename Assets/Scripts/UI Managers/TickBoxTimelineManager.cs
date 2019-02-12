using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TickBoxTimelineManager : MonoBehaviour {

	public Vector3 startPos;
	public Vector3 endPos;
	public GameObject tickSlotPrefab;
	public GameObject tickBoxPrefab;
	public GameObject tickBoxHighlight;

	private Color tickBoxPrefabColor;
	private MatchManager matchManager;

	void Start() {
		matchManager = FindObjectOfType<MatchManager>();

		tickBoxPrefabColor = tickSlotPrefab.GetComponent<Image>().color;

		ClearTimeline();
	}

	public void ClearTimeline() {
		HideHighlight();

		for(int i = 0; i < transform.childCount; i++) {
			for(int j = transform.GetChild(i).GetChild(0).childCount - 1; j >= 0; j--) {
				Destroy(transform.GetChild(i).GetChild(0).GetChild(j).gameObject);
			}
		}

		for(int i = transform.childCount - 1; i >= 0; i--) {
			Destroy(transform.GetChild(i).gameObject);
		}

		for(int i = 0; i < 21; i++) {
			Instantiate(tickSlotPrefab, transform.localPosition, Quaternion.identity, transform);
		}
	}

	public IEnumerator ShiftWithTick(float tickTime) {

		WaitForFixedUpdate waiter = new WaitForFixedUpdate();
		float timer = 0f;
		while(timer < tickTime) {
			timer += Time.deltaTime;
			
			transform.localPosition = Vector3.Lerp(startPos, endPos, timer/tickTime);

			yield return waiter;
		}

		transform.localPosition = startPos;
		Destroy(transform.GetChild(0).gameObject);

		if(matchManager.currentMatch.timeUnitsLeft > transform.childCount - 1) {
			Instantiate(tickSlotPrefab, transform.localPosition, Quaternion.identity, transform);
		}
	}

	public void AddNewTickBox(Athlete athlete, int cooldownEndTime) {
		HideHighlight();

		if(matchManager.currentMatch.timeUnitsLeft > transform.childCount - 1) {
			GameObject newTickBox = GetNewTickBox(athlete, cooldownEndTime);
		}
	}

	public GameObject GetNewTickBox(Athlete athlete, int cooldownEndTime) {
		GameObject tickBoxHolder = transform.GetChild(cooldownEndTime).GetChild(0).gameObject;
		
		GameObject newTickBox = Instantiate(tickBoxPrefab, tickBoxHolder.transform.localPosition, Quaternion.identity, tickBoxHolder.transform);
		Vector3 newPos = tickBoxHolder.transform.localPosition;
		newPos.y -= (GetComponent<RectTransform>().rect.height * (tickBoxHolder.transform.childCount - 1)); //Subtract the height of the timeline times the number of children
		newPos.x = 0;
		newTickBox.transform.localPosition = newPos;

		newTickBox.GetComponent<Image>().color = athlete.GetTeam().teamColor;

		return newTickBox;
	}

	public void DisplayTickBoxHighlight(Athlete athlete, int cooldownEndTime) {
		if(matchManager.currentMatch.timeUnitsLeft > transform.childCount - 1) {
			tickBoxHighlight.GetComponent<Image>().enabled = true;
			tickBoxHighlight.GetComponent<Image>().color = athlete.GetTeam().teamColor;

			GameObject tickBoxHolder = transform.GetChild(cooldownEndTime).GetChild(0).gameObject;

			Vector3 newPos = tickBoxHolder.transform.position;
			tickBoxHighlight.transform.position = newPos;

			newPos = tickBoxHighlight.transform.localPosition;
			newPos.y -= (GetComponent<RectTransform>().rect.height * (tickBoxHolder.transform.childCount)); //Subtract the height of the timeline times the number of children
			tickBoxHighlight.transform.localPosition = newPos;
		}
	}

	public void HideHighlight() {
		tickBoxHighlight.GetComponent<Image>().enabled = false;
	}

	public void DeactivateNextTickBox(int slotNum) {
		GameObject nextBox = null;
		for(int i = 0; i < transform.GetChild(slotNum).GetChild(0).childCount; i++) {
			if(transform.GetChild(slotNum).GetChild(0).GetChild(i).GetComponent<Image>().color != tickBoxPrefabColor) {
				nextBox = transform.GetChild(slotNum).GetChild(0).GetChild(i).gameObject;
				break;
			}
		}

		if(nextBox != null) {
			nextBox.GetComponent<Image>().color = tickBoxPrefabColor;
		} else {
			//Debug.Log("Why you callling this then, mate?");
		}
	}
}
