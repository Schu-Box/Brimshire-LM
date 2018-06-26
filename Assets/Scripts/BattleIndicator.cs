using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BattleIndicator : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {

	public Matchup match;

	private GameController gameController;

	void Start() {
		gameController = FindObjectOfType<GameController> ();
	}

	#region IPointerClickHandler implementation
	public void OnPointerClick (PointerEventData eventData) {
		Debug.Log ("CLICKED");
	}
	#endregion

	#region IPointerEnterHandler implementation
	public void OnPointerEnter (PointerEventData eventData) {
		if (GameController.canHoverMatches) {
			gameController.DisplayMatchHoverPanel (match);
		}

	}
	#endregion

	#region IPointerExitHandler implementation
	public void OnPointerExit (PointerEventData eventData) {
		gameController.matchHoverPanel.SetActive (false);
	}
	#endregion

}
