using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FieldCellController : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {

	public int columnNum;
	public int rowNum;

	public bool canSelect = false;

	private GameController gameController;
	private FieldGridManager fieldGridManager;

	void Start() {
		gameController = FindObjectOfType<GameController> ();
		fieldGridManager = FindObjectOfType<FieldGridManager> ();
	}

	#region IPointerClickHandler implementation
	public void OnPointerClick (PointerEventData eventData) {
		if (canSelect) {
			if (GameController.selectedAthleteForPlacement != null) {
				Debug.Log ("Selected Position");

				gameController.PlaceAthleteInGridCell (GameController.selectedAthleteForPlacement, this);
			}
		}
	}
	#endregion

	#region IPointerEnterHandler implementation
	public void OnPointerEnter (PointerEventData eventData) {
		if (canSelect) {
			if (GameController.selectedAthleteForPlacement != null) {
				Debug.Log ("Highlighting");
				//fieldGridManager.HighlightGridCell (this.gameObject);

				GetComponent<SpriteRenderer> ().sortingOrder = 2;
			}
		}
	}
	#endregion

	#region IPointerExitHandler implementation
	public void OnPointerExit (PointerEventData eventData) {
		if (gameController.matchFog.activeSelf == true) {
			GetComponent<SpriteRenderer> ().sortingOrder = 1;
		}
	}
	#endregion
}
