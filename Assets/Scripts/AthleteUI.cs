using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AthleteUI : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {

	//public GameObject parentObject;

	public static bool disabledInteraction = false;
	public static bool lineupSelection = false;

	public Athlete athlete;
	public Text nameText;
	public Text forceText;
	public Text resilienceText;
	public Text agilityText;
	public Text tacticsText;

	public bool draftee = false;

	private Image img;
	private GameController gameController;

	void Start() {
		img = GetComponent<Image> ();
		gameController = FindObjectOfType<GameController> ();
	}

	#region IPointerClickHandler implementation
	public void OnPointerClick (PointerEventData eventData) {

		/*
		if (draftee) {
			if (GameController.playerTeam.GetDrafting () && athlete != null) {
				Debug.Log ("DRAFTING");

				gameController.SelectDraftee (this.athlete);
			}
		}
		*/

		/*
		//Really fucked all this shit up good
		if (!disabledInteraction && athlete.GetTeam () == GameController.displayTeam) {
			if (lineupSelection) { //Need to add a length condition too
				Matchup match = GameController.playerTeam.seasonMatchups [GameController.week];
				if (match.homeTeam == GameController.playerTeam) {
					gameController.AddToActiveLineup (match, athlete);
				} else { //Then the player is the away team
					gameController.AddToActiveLineup (match, athlete);
				}
			} else {
				Debug.Log (draftee);

				if (draftee) { //If this athlete is a draftee
					Debug.Log("DRAFTABLE");

					if (GameController.playerTeam.GetDrafting ()) {
						Debug.Log ("YAP");

						gameController.SelectDraftee (this.athlete);
					}

					if (GameController.playerTeam.GetDrafting () && athlete != null) {
						Debug.Log ("DRAFTING");

						gameController.SelectDraftee (this.athlete);
					}
				} else {
					
					Debug.Log (athlete.name);
					for (int i = 0; i < athlete.GetTeam ().rosterList.Count; i++) {
						if (athlete == athlete.GetTeam ().rosterList [i]) {
							gameController.ExpandAthleteDetails (i);
						}
					}

				}
			}
		} else { //You be clicking on the opponent UI

		}
		*/
	}
	#endregion

	#region IPointerEnterHandler implementation
	public void OnPointerEnter (PointerEventData eventData) {
		//Debug.Log ("ENTER");
		/*
		if (!disabledInteraction) {
			if (athlete != null) {
				img.enabled = true;

				gameController.DisplayAthleteDetails (athlete);
			}
		}
		*/
	}
	#endregion

	#region IPointerExitHandler implementation
	public void OnPointerExit (PointerEventData eventData) {
		/*
		if (!disabledInteraction) {
			if (athlete != null) {
				if (!athlete.onActiveRoster) {
					img.enabled = false;
				}

				gameController.UndisplayAthleteDetails ();
			}
		}
		*/
	}
	#endregion
}
