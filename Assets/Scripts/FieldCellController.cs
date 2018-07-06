using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FieldCellController : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {
	
	public FieldTile fieldTile;

	[HideInInspector] public bool canClick = false;

	public GameObject fieldHighlightObject;
	public Transform centerPositions;
	public Transform topPositions;
	public Transform rightPositions;
	public Transform bottomPositions;
	public Transform leftPositions;

	//private GameController gameController;
	private MatchManager matchManager;
	//private SpriteRenderer spriteRenderer;

	[HideInInspector] public SpriteRenderer highlightSpriter;
	private Vector2 highlightStartSize;

	private Coroutine hoverCoroutine;

	void Start() {
		//gameController = FindObjectOfType<GameController> ();
		matchManager = FindObjectOfType<MatchManager> ();
		//spriteRenderer = GetComponent<SpriteRenderer> ();

		highlightSpriter = fieldHighlightObject.GetComponent<SpriteRenderer> ();
		highlightStartSize = highlightSpriter.size;
	}

	#region IPointerClickHandler implementation
	public void OnPointerClick (PointerEventData eventData) {
		if (canClick) {
			if (MatchManager.selectedAthleteForPlacement != null) {
				Debug.Log ("Selected Position");
				matchManager.PlaceAthleteInGridCell (MatchManager.selectedAthleteForPlacement, this);
			} else if (MatchManager.selectedAction != null) {
				//UnhighlightCell ();
				matchManager.DisplayBeginAction (matchManager.currentMatch.athleteWithTurn, MatchManager.selectedAction, fieldTile);
			}
		}
	}
	#endregion

	#region IPointerEnterHandler implementation
	public void OnPointerEnter (PointerEventData eventData) {
		if (canClick) {
			hoverCoroutine = StartCoroutine (StartHoverAnimation ());
		}
	}
	#endregion

	#region IPointerExitHandler implementation
	public void OnPointerExit (PointerEventData eventData) {
		if (hoverCoroutine != null) {
			StopCoroutine (hoverCoroutine);
			hoverCoroutine = null;
			highlightSpriter.size = highlightStartSize;
		}
	}
	#endregion

	public void HighlightCell() {
		//Debug.Log ("Highlighting");

		canClick = true;
		fieldHighlightObject.SetActive (true);
	}

	public void UnhighlightCell() {
		canClick = false;
		fieldHighlightObject.SetActive (false);
	}

	public IEnumerator StartHoverAnimation() {
		Vector2 smallestSize = new Vector2 (3.6f, 3.6f);
		float speed = 2f;
		float step = 0f;
		bool increasingStep = true;

		WaitForFixedUpdate waiter = new WaitForFixedUpdate ();
		while(true) {
			if (increasingStep) {
				step += (Time.deltaTime * speed);
			} else {
				step -= (Time.deltaTime * speed);
			}

			Vector2 newSize = Vector2.Lerp (highlightStartSize, smallestSize, step);
			highlightSpriter.size = newSize;

			if (step >= 1) {
				increasingStep = false;
			} else if (step <= 0) {
				increasingStep = true;
			}

			yield return waiter;
		}
	}

	public Transform GetNextAvailablePosition(string positionID) {
		Transform groupChecked = null;

		switch (positionID.ToLower()) {
		case "center":
			groupChecked = centerPositions;
			break;

		case "top":
			groupChecked = topPositions;
			break;

		case "right":
			groupChecked = rightPositions;
			break;

		case "bottom":
			groupChecked = bottomPositions;
			break;

		case "left":
			groupChecked = leftPositions;
			break;

		default:
			Debug.Log ("That's not a valid position.");
			break;
		}
		for (int i = 0; i < groupChecked.transform.childCount; i++) {
			if (groupChecked.transform.GetChild (i).childCount == 0) {
				return groupChecked.transform.GetChild (i);
			}
		}

		Debug.Log ("You fudged it. GG. The slot is completely filled.");
		return null;
	}
}
