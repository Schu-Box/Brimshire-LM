using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//Get rid of this
/*
public class GoalZoneController : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {

	[HideInInspector] public bool canClick = false;

	private SpriteRenderer spriteRenderer;

	private Vector2 highlightStartSize;
	private MatchManager matchManager;
	private Coroutine hoverCoroutine;

	void Start() {
		//gameController = FindObjectOfType<GameController> ();
		matchManager = FindObjectOfType<MatchManager> ();
		//spriteRenderer = GetComponent<SpriteRenderer> ();


	}

	void Awake() {
		spriteRenderer = GetComponent<SpriteRenderer> ();
		highlightStartSize = spriteRenderer.size;

		UnhighlightGoal ();
	}


	#region IPointerClickHandler implementation
	public void OnPointerClick (PointerEventData eventData) {
		if (canClick) {
			matchManager.DisplayBeginAction (matchManager.currentMatch.athleteWithTurn, MatchManager.selectedAction, null);
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
			spriteRenderer.size = highlightStartSize;
		}
	}
	#endregion

	public void HighlightGoal() {
		//Debug.Log ("Highlighting");

		canClick = true;
		spriteRenderer.enabled = true;
	}

	public void UnhighlightGoal() {
		canClick = false;
		spriteRenderer.enabled = false;
	}

	public IEnumerator StartHoverAnimation() {
		Vector2 smallestSize = new Vector2 (0.6f, 3.4f);
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
			spriteRenderer.size = newSize;

			if (step >= 1) {
				increasingStep = false;
			} else if (step <= 0) {
				increasingStep = true;
			}

			yield return waiter;
		}
	}
}
*/
