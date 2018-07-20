using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FieldCellController : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {

	/*
	public bool isGoal = false;
	public bool isBoundary = false;
	*/

	public FieldTile fieldTile;

	[HideInInspector] public bool canClick = false;

	public GameObject fieldHighlightObject;
	public Transform centerPosition;
	public Transform topPosition;
	public Transform rightPosition;
	public Transform bottomPosition;
	public Transform leftPosition;

	//private GameController gameController;
	private MatchManager matchManager;
	//private SpriteRenderer spriteRenderer;

	[HideInInspector] public SpriteRenderer highlightSpriter;
	private Vector2 highlightStartSize;

	private Coroutine hoverCoroutine;

	void Awake() {
		//gameController = FindObjectOfType<GameController> ();
		matchManager = FindObjectOfType<MatchManager> ();
		//spriteRenderer = GetComponent<SpriteRenderer> ();

		highlightSpriter = fieldHighlightObject.GetComponent<SpriteRenderer> ();
		highlightStartSize = highlightSpriter.size;
	}

	#region IPointerClickHandler implementation
	public void OnPointerClick (PointerEventData eventData) {
		if (canClick) {
			if (MatchManager.selectedAthleteMatchPanel != null) {
				matchManager.PlaceAthleteInGridCell (MatchManager.selectedAthleteMatchPanel.athlete, this);
			} else if(MatchManager.selectedBall != null) {
				matchManager.currentMatch.PlaceBallOnTile(MatchManager.selectedBall, fieldTile);
			} else if (MatchManager.selectedOpportunityButton != null) {
				matchManager.DisplayBeginAction (matchManager.currentMatch.athleteWithTurn, MatchManager.selectedOpportunityButton, fieldTile);
			}
		}
	}
	#endregion

	#region IPointerEnterHandler implementation
	public void OnPointerEnter (PointerEventData eventData) {
		if (canClick) {
			StartHoverAnimation();

			if(MatchManager.selectedOpportunityButton != null) {
				matchManager.DisplaySelectedAction(MatchManager.selectedOpportunityButton, fieldTile);
			}
		}
	}
	#endregion

	#region IPointerExitHandler implementation
	public void OnPointerExit (PointerEventData eventData) {
		if (hoverCoroutine != null) {
			StopHoverAnimation();
		}

		if(MatchManager.selectedOpportunityButton != null && !MatchManager.actionSelected) {
			matchManager.UndisplaySelectedAction(MatchManager.selectedOpportunityButton);
		}
	}
	#endregion

	public void SetClickability(bool clickable) {
		canClick = clickable;
	}

	public void HighlightCell() {
		fieldHighlightObject.SetActive (true);
	}

	public void UnhighlightCell() {
		SetClickability(false);
		fieldHighlightObject.SetActive (false);
	}

	public void StartHoverAnimation() {
		hoverCoroutine = StartCoroutine(HoverAnimation());
	}
	public IEnumerator HoverAnimation() {

		float xSize = highlightStartSize.x * 0.75f;
		float ySize = highlightStartSize.y * 0.75f;
		Vector2 smallestSize = new Vector2 (xSize, ySize);
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

	public void StopHoverAnimation() {
		if(hoverCoroutine != null) {
			StopCoroutine(hoverCoroutine);
			hoverCoroutine = null;
			highlightSpriter.size = highlightStartSize;
		}
	}

	/*
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
	*/
}
