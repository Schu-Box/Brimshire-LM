using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ActionButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

	public Action action;
	public Text actionText;
	public Text actionDescriptionText;
	public Text percentageText;
	public Text timeTakenText;

	//public string opportunityQuote = '"' + "I can do this, coach!" + '"';
	//private string oppTextBeforeEnter;

	/*
	#region IPointerClickHandler implementation
	public void OnPointerClick (PointerEventData eventData) {
		
	}
	#endregion
	*/

	private MatchManager matchManager;

	void Start() {
		matchManager = FindObjectOfType<MatchManager> ();
	}

	#region IPointerEnterHandler implementation
	public void OnPointerEnter (PointerEventData eventData) {
		if (GetComponent<Button> ().interactable) {
			DisplayValidSelections ();
		}
		/*
		oppTextBeforeEnter = opportunityDescriptionText.text;
		opportunityDescriptionText.text = opportunityQuote;
		*/
	}
	#endregion

	#region IPointerExitHandler implementation
	public void OnPointerExit (PointerEventData eventData) {
		if (GetComponent<Button> ().interactable) {
			UndisplayValidSelections ();
		}
		//opportunityDescriptionText.text = oppTextBeforeEnter;
	}
	#endregion

	public void DisplayValidSelections() {
		//Debug.Log (action.athlete.name);

		switch (action.opportunity.id.ToLower ()) {
		case "move":
			FieldTile currentFieldTile = action.athlete.currentFieldTile;

			for (int i = 0; i < matchManager.fieldGridCells.Count; i++) {
				for (int j = 0; j < matchManager.fieldGridCells [i].Count; j++) {
					for (int f = 0; f < matchManager.fieldGridCells[i][j].fieldTile.neighborList.Count; f++) {
						if (matchManager.fieldGridCells [i] [j].fieldTile.neighborList [f] == currentFieldTile) {
							matchManager.fieldGridCells [i] [j].HighlightCell ();
						}
					}
				}
			}
			break;
		default:
			Debug.Log ("That action is a figment of your imagination.");
			break;
		}
	}

	public void UndisplayValidSelections() {
		for (int i = 0; i < matchManager.fieldGridCells.Count; i++) {
			for (int j = 0; j < matchManager.fieldGridCells [i].Count; j++) {
				if (matchManager.fieldGridCells [i] [j].fieldTile != action.athlete.currentFieldTile) {
					matchManager.fieldGridCells [i] [j].UnhighlightCell ();
				}
			}
		}
	}
}
