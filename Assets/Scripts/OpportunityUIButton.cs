using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OpportunityUIButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

	public Opportunity opportunity;
	public Text opportunityText;
	public Text opportunityDescriptionText;
	public Text percentageText;

	public string opportunityQuote = '"' + "I can do this, coach!" + '"';

	private string oppTextBeforeEnter;

	/*
	#region IPointerClickHandler implementation
	public void OnPointerClick (PointerEventData eventData) {
		
	}
	#endregion
	*/

	#region IPointerEnterHandler implementation
	public void OnPointerEnter (PointerEventData eventData) {
		oppTextBeforeEnter = opportunityDescriptionText.text;
		opportunityDescriptionText.text = opportunityQuote;
	}
	#endregion

	#region IPointerExitHandler implementation
	public void OnPointerExit (PointerEventData eventData) {
		opportunityDescriptionText.text = oppTextBeforeEnter;
	}
	#endregion
}
