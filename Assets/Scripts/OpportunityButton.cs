using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OpportunityButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

	public Opportunity opportunity;
	public Text actionText;
	public Text actionDescriptionText;
	public GameObject specificActionPanel;
	public Text percentageText;
	public Text timeTakenText;

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
			matchManager.DisplayValidSelections (opportunity);
		}
	}
	#endregion

	#region IPointerExitHandler implementation
	public void OnPointerExit (PointerEventData eventData) {
		if (GetComponent<Button> ().interactable) {
			matchManager.UndisplayValidSelections ();

			if(MatchManager.selectedOpportunityButton != null) {
				matchManager.DisplayValidSelections(MatchManager.selectedOpportunityButton.opportunity);
			}
		}
	}
	#endregion
}
