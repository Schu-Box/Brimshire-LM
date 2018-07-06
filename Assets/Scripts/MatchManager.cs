using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchManager : MonoBehaviour {

	public static Athlete selectedAthleteForPlacement;
	public static Action selectedAction;

	[Header("Match UI")]
	public GameObject matchUIObject;
	public Text matchHomeTitle;
	public Text matchAwayTitle;
	public Text matchHomeScore;
	public Text matchAwayScore;
	public Text vsText;
	public Text timeText;
	public Button matchContinueButton;
	public Button matchSimulateButton;
	public Image matchHomeShield;
	public Image matchAwayShield;
	public GameObject matchManagerSelectionPanel;
	public GameObject matchAthleteSelectionGroup;
	public AthletePanel matchSelectedAthletePanel;

	public GameObject fieldAthletesPanel;
	public GameObject fieldHomeAthletesGroup;
	public GameObject fieldAwayAthletesGroup;
	public GameObject homeMatchPanel;
	public GameObject awayMatchPanel;

	[Header("Colors")]
	public Color descBaseColor;
	public Color descDecidingColor;
	public Color descDecidedColor;

	[Header("Match Field")]
	public GameObject matchFieldParent;
	public GameObject matchGridHolder;
	public GameObject homeGoalzone;
	public GameObject awayGoalzone;
	//public GameObject matchFog;
	//public GameObject athleteHolder;
	public GameObject playIndicator;
	public GameObject movementIndicator;

	public GameObject athleteOnFieldPrefab;

	public List<List<FieldCellController>> fieldGridCells = new List<List<FieldCellController>> ();

	public Matchup currentMatch;

	private GameController gameController;

	void Awake() {
		gameController = FindObjectOfType<GameController> ();
	}

	public void DisplayMatchUI (Matchup match) {
		Debug.Log ("Displaying Match UI");
		currentMatch = match;

		GameController.movementPaused = true;
		GameController.canHoverMatches = false;

		//Everything just pops up right now but eventually there should be an unravelling animation to bring it up
		matchUIObject.SetActive (true);
		matchFieldParent.SetActive (true);

		gameController.topText.transform.parent.gameObject.SetActive (false);
		//matchFog.SetActive (false);
		matchSelectedAthletePanel.gameObject.SetActive (false);
		fieldAthletesPanel.SetActive (false);
		homeMatchPanel.GetComponent<Image> ().color = match.homeTeam.teamColor;
		awayMatchPanel.GetComponent<Image> ().color = match.awayTeam.teamColor;
		homeMatchPanel.SetActive (false);
		awayMatchPanel.SetActive (false);
		timeText.gameObject.SetActive (false);
		vsText.gameObject.SetActive (true);
		playIndicator.SetActive (false);
		movementIndicator.SetActive (false);

		matchHomeTitle.transform.parent.GetComponent<Image> ().color = match.homeTeam.teamColor;
		matchHomeTitle.text = match.homeTeam.teamName;
		matchAwayTitle.transform.parent.GetComponent<Image> ().color = match.awayTeam.teamColor;
		matchAwayTitle.text = match.awayTeam.teamName;

		matchHomeScore.text = match.homeScore.ToString ();
		matchAwayScore.text = match.awayScore.ToString ();
		matchHomeScore.transform.parent.GetComponent<Image> ().color = match.homeTeam.teamColor;
		matchAwayScore.transform.parent.GetComponent<Image> ().color = match.awayTeam.teamColor;

		Image homeSecondaryImage = matchHomeShield.transform.GetChild (0).GetComponent<Image> ();
		homeSecondaryImage.sprite = match.homeTeam.logoSecondarySprite;
		homeSecondaryImage.color = match.homeTeam.teamColor;
		Image homePrimaryImage = matchHomeShield.transform.GetChild (1).GetComponent<Image> ();
		homePrimaryImage.sprite = match.homeTeam.logoPrimarySprite;
		homePrimaryImage.color = match.homeTeam.GetCity ().GetCounty ().GetCapitalCity ().GetTeamOfCity ().teamColor;
		Image awaySecondaryImage = matchAwayShield.transform.GetChild (0).GetComponent<Image> ();
		awaySecondaryImage.sprite = match.awayTeam.logoSecondarySprite;
		awaySecondaryImage.color = match.awayTeam.teamColor;
		Image awayPrimaryImage = matchAwayShield.transform.GetChild (1).GetComponent<Image> ();
		awayPrimaryImage.sprite = match.awayTeam.logoPrimarySprite;
		awayPrimaryImage.color = match.awayTeam.GetCity ().GetCounty ().GetCapitalCity ().GetTeamOfCity ().teamColor;

		homeGoalzone.GetComponent<SpriteRenderer> ().color = match.homeTeam.teamColor;
		awayGoalzone.GetComponent<SpriteRenderer> ().color = match.awayTeam.teamColor;

		matchContinueButton.GetComponentInChildren<Text> ().text = "Start Match";
		matchContinueButton.onClick.RemoveAllListeners ();
		matchContinueButton.onClick.AddListener (() => match.StartMatch ());
		matchContinueButton.gameObject.SetActive (false);

		matchSimulateButton.GetComponentInChildren<Text> ().text = "Simulate Match";
		matchSimulateButton.onClick.RemoveAllListeners ();
		matchSimulateButton.onClick.AddListener (() => match.SimulateMatch ());

		Vector3 matchPosition = match.battleMarker.transform.position;
		matchFieldParent.transform.position = matchPosition;
		matchUIObject.transform.position = matchPosition;

		//Zoom the camera onto the matchUIObject
		float camZ = Camera.main.transform.position.z;
		Vector3 newCamPos = Camera.main.transform.position;
		newCamPos = matchPosition;
		newCamPos.z = camZ;
		StartCoroutine (gameController.ZoomAndShiftCameraTo (newCamPos, 1.12f, 2f));


		SetMatchField ();

		TeamController playerTeam = null;
		if (match.homeTeam == GameController.playerManager.GetTeam ()) {
			playerTeam = match.homeTeam;
			matchSelectedAthletePanel.transform.position = matchHomeShield.transform.position;

			match.SetLineup (match.awayTeam);
			for (int i = 0; i < match.awayTeam.rosterList.Count; i++) {
				Athlete athlete = match.awayTeam.rosterList [i];
				if (athlete.currentFieldTile != null) {
					PlaceAthleteInGridCell (athlete, GetFieldCellObject (athlete.currentFieldTile.gridX, athlete.currentFieldTile.gridY));
				}
			}
		} else if (match.awayTeam == GameController.playerManager.GetTeam ()) {
			playerTeam = match.awayTeam;
			matchSelectedAthletePanel.transform.position = matchAwayShield.transform.position;

			match.SetLineup (match.homeTeam);
			for (int i = 0; i < match.homeTeam.rosterList.Count; i++) {
				Athlete athlete = match.homeTeam.rosterList [i];
				if (athlete.currentFieldTile != null) {
					PlaceAthleteInGridCell (athlete, GetFieldCellObject (athlete.currentFieldTile.gridX, athlete.currentFieldTile.gridY));
				}
			}
		} else {
			Debug.Log ("HUGE ERROR or maybe you spectating");
		}

		matchAthleteSelectionGroup.transform.parent.gameObject.SetActive (true);
		matchAthleteSelectionGroup.transform.parent.GetComponent<Image> ().color = playerTeam.teamColor;
		UpdateRosterSelectionPanel (playerTeam);
	}

	public void SetMatchField() {
		//Eventually this should be generated here instead of pulled from the transform's children

		fieldGridCells = new List<List<FieldCellController>> ();
		for (int i = 0; i < matchGridHolder.transform.childCount; i++) {
			fieldGridCells.Add (new List<FieldCellController> ());

			for(int j = 0; j < matchGridHolder.transform.GetChild(i).childCount; j++) {
				fieldGridCells[i].Add (matchGridHolder.transform.GetChild (i).GetChild (j).GetComponent<FieldCellController> ());
				FieldCellController cell = fieldGridCells [i] [j];
				cell.fieldTile = currentMatch.fieldGrid [i] [j];
				cell.UnhighlightCell ();

				//TERRIBLE LAZY IMPLEMENTATION ALERT
				cell.highlightSpriter.color = GameController.playerManager.GetTeam ().teamColor;
			}
		}

		ClearField ();
	}

	public void ClearField() {
		Debug.Log ("Clearing Field");
		List<GameObject> deleteThisNephew = new List<GameObject> ();

		for (int x = 0; x < fieldGridCells.Count; x++) {
			for (int y = 0; y < fieldGridCells [x].Count; y++) {
				FieldCellController cell = fieldGridCells [x] [y];

				for (int i = 0; i < cell.centerPositions.childCount; i++) {
					for (int j = 0; j < cell.centerPositions.GetChild (i).childCount; j++) {
						deleteThisNephew.Add (cell.centerPositions.GetChild (i).GetChild (j).gameObject);
					}
				}

				for (int i = 0; i < cell.topPositions.childCount; i++) {
					for (int j = 0; j < cell.topPositions.GetChild (i).childCount; j++) {
						deleteThisNephew.Add (cell.topPositions.GetChild (i).GetChild (j).gameObject);
					}
				}

				for (int i = 0; i < cell.rightPositions.childCount; i++) {
					for (int j = 0; j < cell.rightPositions.GetChild (i).childCount; j++) {
						deleteThisNephew.Add (cell.rightPositions.GetChild (i).GetChild (j).gameObject);
					}
				}

				for (int i = 0; i < cell.bottomPositions.childCount; i++) {
					for (int j = 0; j < cell.bottomPositions.GetChild (i).childCount; j++)
						deleteThisNephew.Add (cell.bottomPositions.GetChild (i).GetChild (j).gameObject);
				}

				for (int i = 0; i < cell.leftPositions.childCount; i++) {
					for (int j = 0; j < cell.leftPositions.GetChild (i).childCount; j++) {
						deleteThisNephew.Add (cell.leftPositions.GetChild (i).GetChild (j).gameObject);
					}
				}
			}
		}

		for (int i = deleteThisNephew.Count - 1; i >= 0; i--) {
			Destroy (deleteThisNephew [i].gameObject);
			deleteThisNephew.Remove (deleteThisNephew [i]);
		}
	}

	public FieldCellController GetFieldCellObject(int rowNum, int columnNum) {
		//Debug.Log ("Getting field cell");
		return fieldGridCells [rowNum] [columnNum];
	}

	public void SelectAthlete(Athlete athlete) {
		selectedAthleteForPlacement = athlete;

		//matchAthleteSelectionGroup.transform.parent.gameObject.SetActive (false);

		matchSelectedAthletePanel.gameObject.SetActive (true);
		matchSelectedAthletePanel.SetAthletePanel (selectedAthleteForPlacement);
		matchSelectedAthletePanel.button.onClick.RemoveAllListeners ();
		matchSelectedAthletePanel.button.onClick.AddListener (() => UnselectAthlete ());

		//matchFog.SetActive (true);
		for (int i = 0; i < fieldGridCells.Count; i++) {
			for (int j = 0; j < fieldGridCells [i].Count; j++) {
				FieldCellController cell = fieldGridCells [i] [j];
				if ((GameController.playerManager.GetTeam () == currentMatch.homeTeam && cell.fieldTile.homeSide)
				   || (GameController.playerManager.GetTeam () == currentMatch.awayTeam && !cell.fieldTile.homeSide)) {

					cell.HighlightCell ();
				}
			}
		}
	}

	public void UnselectAthlete() {
		//RemoveFog ();

		selectedAthleteForPlacement = null;

		matchSelectedAthletePanel.gameObject.SetActive (false);

		//matchAthleteSelectionGroup.transform.parent.gameObject.SetActive (true);

		UnhighlightAllGridCells ();
	}

	public void RemoveAthleteFromField(Athlete athlete) {
		selectedAthleteForPlacement = null;
		matchSelectedAthletePanel.gameObject.SetActive (false);
		UnhighlightAllGridCells ();

		Destroy (athlete.athleteOnFieldObject);

		athlete.GetTeam ().seasonMatchups [GameController.week].RemoveAthleteFromFieldGrid (athlete);

		UpdateRosterSelectionPanel (athlete.GetTeam ());
	}

	public void UnhighlightAllGridCells() {
		for (int i = 0; i < fieldGridCells.Count; i++) {
			for (int j = 0; j < fieldGridCells [i].Count; j++) {
				FieldCellController cell = fieldGridCells [i] [j];
				cell.UnhighlightCell ();
			}
		}
	}

	public void PlaceAthleteInGridCell(Athlete placedAthlete, FieldCellController gridCell) {
		Debug.Log ("Placing Athlete");

		selectedAthleteForPlacement = null;
		matchSelectedAthletePanel.gameObject.SetActive (false);

		UnhighlightAllGridCells ();

		Transform newParent = gridCell.GetNextAvailablePosition ("center");

		Quaternion newQuaternion = Quaternion.identity;
		if (gridCell.fieldTile.gridX >= (fieldGridCells.Count / 2)) {
			newQuaternion.eulerAngles = new Vector3 (0, 180, 0);
		}

		GameObject placedAthleteFieldObj = Instantiate (athleteOnFieldPrefab, newParent.position, newQuaternion, newParent);
		placedAthlete.athleteOnFieldObject = placedAthleteFieldObj;

		SpriteRenderer jerseySpriteRend = placedAthleteFieldObj.transform.GetChild (1).GetComponent<SpriteRenderer> ();
		jerseySpriteRend.sprite = placedAthlete.jerseySprite;
		jerseySpriteRend.color = placedAthlete.GetTeam ().teamColor;

		SpriteRenderer bodySpriteRend = placedAthleteFieldObj.transform.GetChild (0).GetComponent<SpriteRenderer> ();
		bodySpriteRend.sprite = placedAthlete.bodySprite;



		if (placedAthlete.currentFieldTile == null) { //If the athlete is not on the field then add them to the field
			placedAthlete.GetTeam ().seasonMatchups [GameController.week].AddAthleteToFieldGrid (placedAthlete, gridCell.fieldTile.gridX, gridCell.fieldTile.gridY); //Put the athlete in the grid data
		}

		UpdateRosterSelectionPanel (GameController.playerManager.GetTeam()); //The player's team

		//placedAthleteFieldObj.transform.localPosition = Vector3.zero;
	}

	public void UpdateRosterSelectionPanel(TeamController team) {
		matchManagerSelectionPanel.GetComponent<AthletePanel> ().SetAthletePanel (team.teamManager);

		for (int i = 0; i < matchAthleteSelectionGroup.transform.childCount; i++) {
			AthletePanel athletePanel = matchAthleteSelectionGroup.transform.GetChild (i).GetComponent<AthletePanel> ();
			if (i < team.rosterList.Count) {
				athletePanel.SetAthletePanel (team.rosterList [i]);
			} else {
				athletePanel.SetAthletePanel (null);
			}
		}

		int athletesOnFieldCount = 0;
		if (team.teamManager.currentFieldTile != null) {
			athletesOnFieldCount++;
		}
		for (int i = 0; i < team.rosterList.Count; i++) {
			if (team.rosterList [i].currentFieldTile != null) {
				athletesOnFieldCount++;
			}
		}

		if (athletesOnFieldCount == 3) {
			matchContinueButton.gameObject.SetActive (true);
		} else if (athletesOnFieldCount > 3) {
			Debug.Log ("You letting too many chumps into the chumpery.");
			matchContinueButton.gameObject.SetActive (false);
		} else {
			matchContinueButton.gameObject.SetActive (false);
		}
	}

	public void DisplayStartedMatchUI(Matchup match) {
		Debug.Log ("Displaying Field Athlete Panel");

		matchContinueButton.gameObject.SetActive (false);
		vsText.gameObject.SetActive (false);
		timeText.gameObject.SetActive (true);
		timeText.text = "Time Left: " + currentMatch.timeUnitsLeft + " seconds";

		fieldAthletesPanel.SetActive (true);
		fieldHomeAthletesGroup.GetComponent<Image> ().color = match.homeTeam.teamColor;
		fieldAwayAthletesGroup.GetComponent<Image> ().color = match.awayTeam.teamColor;

		int homeAthletesDisplayed = 0;
		int awayAthletesDisplayed = 0;
		for (int i = 0; i < match.athletesOnField.Count; i++) { //Set the Athlete Panels in the match for each athlete
			GameObject athPan;
			if (match.athletesOnField [i].GetTeam () == match.homeTeam) {
				athPan = fieldHomeAthletesGroup.transform.GetChild (homeAthletesDisplayed).gameObject;
				homeAthletesDisplayed++;
			} else {
				athPan = fieldAwayAthletesGroup.transform.GetChild (awayAthletesDisplayed).gameObject;
				awayAthletesDisplayed++;
			}
			athPan.GetComponent<AthletePanel> ().SetAthletePanel (match.athletesOnField [i]);
			match.athletesOnField [i].athletePanelInMatch = athPan.GetComponent<AthletePanel> ();

			athPan.GetComponent<AthletePanel>().descriptorPanel.SetActive (true);
		}

		UnhighlightAllAthletes ();
	}

	public void DisplayNewMatchTurn(Matchup match) {
		Athlete athlete = match.athleteWithTurn;

		HighlightAthleteWithTurn (athlete);

		UpdateAthleteDescriptorPanels ();

		GameObject actionButtonsHolder;
		if (athlete.GetTeam () == match.homeTeam) {
			homeMatchPanel.SetActive (true);  
			awayMatchPanel.SetActive (false);

			actionButtonsHolder = homeMatchPanel.transform.GetChild (0).gameObject;
		} else { //Assumes that the athlete belongs to the away team. SHOULD always be correct.
			awayMatchPanel.SetActive (true);
			homeMatchPanel.SetActive (false);

			actionButtonsHolder = awayMatchPanel.transform.GetChild (0).gameObject;
		}

		for (int i = 0; i < actionButtonsHolder.transform.childCount; i++) {
			ActionButton actionButton = actionButtonsHolder.transform.GetChild (i).GetComponent<ActionButton> ();
			if (i < athlete.availableActionList.Count) {
				Action act = athlete.availableActionList [i];

				actionButton.action = act;
				actionButton.actionText.text = act.opportunity.name;
				actionButton.actionDescriptionText.text = act.opportunity.description;
				actionButton.percentageText.text = act.opportunity.basePercentChanceSuccess + "% chance";
				actionButton.timeTakenText.text = "in " + act.timeUnitsLeft + " seconds";

				actionButton.GetComponent<Button> ().interactable = true;
				actionButton.GetComponent<Button> ().onClick.RemoveAllListeners ();
				actionButton.GetComponent<Button> ().onClick.AddListener (() => DisplaySelectedAction (athlete, actionButton));
			} else {
				actionButton.actionText.text = "";
				actionButton.actionDescriptionText.text = "";
				actionButton.percentageText.text = "";
				actionButton.timeTakenText.text = "";

				actionButton.GetComponent<Button> ().onClick.RemoveAllListeners ();
				actionButton.GetComponent<Button> ().interactable = false;
			}
		}
			
		//This shit needs a lot of work and should probably be stored somewhere else.
		if (athlete.GetTeam () != GameController.playerManager.GetTeam ()) {
			Debug.Log ("Displaying AI Turn");

			Action randomAction = currentMatch.GetAIAction (athlete);
			ActionButton actButton = null;
			for (int i = 0; i < actionButtonsHolder.transform.childCount; i++) {
				if (actionButtonsHolder.transform.GetChild (i).GetComponent<ActionButton> ().action == randomAction) {
					actButton = actionButtonsHolder.transform.GetChild (i).GetComponent<ActionButton> ();
				}
			}
			if (actButton == null) {
				Debug.Log ("Action Button is null. Game stops here.");
			} else {
				StartCoroutine (WaitThenDisplaySelectedAction (athlete, actButton));
			}
		}
	}

	public void HighlightAthleteWithTurn(Athlete athlete) {
		athlete.athletePanelInMatch.bodyImg.color = Color.white;
		athlete.athletePanelInMatch.jerseyImg.color = athlete.GetTeam ().teamColor;

		playIndicator.SetActive(true); //Display an arrow
		Vector3 playPos = athlete.athleteOnFieldObject.transform.position;
		playPos.y += 0.2f;
		playIndicator.transform.position = playPos;
	}

	public void UnhighlightAllAthletes() {
		for (int i = 0; i < fieldHomeAthletesGroup.transform.childCount; i++) {
			AthletePanel athPan = fieldHomeAthletesGroup.transform.GetChild (i).GetComponent<AthletePanel> ();
			athPan.bodyImg.color = Color.white;
			athPan.jerseyImg.color = athPan.athlete.GetTeam ().teamColor;
		}
		for (int i = 0; i < fieldAwayAthletesGroup.transform.childCount; i++) {
			AthletePanel athPan = fieldAwayAthletesGroup.transform.GetChild (i).GetComponent<AthletePanel> ();
			athPan.bodyImg.color = Color.white;
			athPan.jerseyImg.color = athPan.athlete.GetTeam ().teamColor;
		}

		for (int i = 0; i < currentMatch.athletesOnField.Count; i++) {
			Athlete athlete = currentMatch.athletesOnField [i];
			athlete.athleteOnFieldObject.GetComponent<SpriteRenderer> ().sortingLayerName = "Match Athlete";
			for (int j = 0; j < athlete.athleteOnFieldObject.transform.childCount; j++) {
				athlete.athleteOnFieldObject.transform.GetChild (j).GetComponent<SpriteRenderer> ().sortingLayerName = "Match Athlete";
			}
		}
	}

	public IEnumerator WaitThenDisplaySelectedAction(Athlete athlete, ActionButton actButt) {
		float timeWait = 0.5f;
		float timer = 0;

		while (timer < timeWait) {
			timer += Time.deltaTime;

			yield return new WaitForFixedUpdate ();
		}

		DisplaySelectedAction (athlete, actButt);
		StartCoroutine (WaitThenDisplayBeginAction (athlete, actButt.action, actButt.action.fieldTile));
	}

	public void DisplaySelectedAction(Athlete athlete, ActionButton actButt) {
		selectedAction = actButt.action;
		//currentMatch.SelectOpportunity (athlete, oppButt.opportunity);

		//Add listener to cancel opportunity
		actButt.GetComponent<Button> ().interactable = false;
	}

	public IEnumerator WaitThenDisplayBeginAction(Athlete athlete, Action action, FieldTile tile) {
		float timeWait = 1f;
		float timer = 0;

		while (timer < timeWait) {
			timer += Time.deltaTime;

			yield return new WaitForFixedUpdate ();
		}

		DisplayBeginAction (athlete, action, tile);
	}

	public void DisplayBeginAction(Athlete athlete, Action action, FieldTile tile) {
		//Debug.Log ("Attempting opportunity");
		UnhighlightAllGridCells ();
		playIndicator.SetActive (false);

		action.fieldTile = tile;

		StartCoroutine (AnimateBeginAction (athlete, action));

	}

	public IEnumerator AnimateBeginAction(Athlete athlete, Action action) {
		Transform athleteMovingParent = null;

		switch (action.opportunity.id.ToLower ()) {
		case "move":
			FieldTile startTile = athlete.currentFieldTile;
			FieldTile endTile = action.fieldTile;
			Vector3 moveEulerRotation = Vector3.zero;

			Vector3 athleteStartPos = athlete.athleteOnFieldObject.transform.position;
			FieldCellController fieldCell = GetFieldCellObject (startTile.gridX, startTile.gridY);

			movementIndicator.SetActive (true);
			movementIndicator.transform.position = fieldCell.transform.position;

			if (startTile.gridX > endTile.gridX) {
				moveEulerRotation.z = 270;

				athleteMovingParent = fieldCell.GetNextAvailablePosition ("left");
			} else if (startTile.gridX < endTile.gridX) {
				moveEulerRotation.z = 90;

				athleteMovingParent = fieldCell.GetNextAvailablePosition ("right");
			} else {
				if (startTile.gridY > endTile.gridY) {
					moveEulerRotation.z = 180;

					athleteMovingParent = fieldCell.GetNextAvailablePosition ("top");
				} else if (startTile.gridY < endTile.gridY) {
					moveEulerRotation.z = 0;

					athleteMovingParent = fieldCell.GetNextAvailablePosition ("bottom");
				} else {
					Debug.Log ("That ain't no movement, dawg.");
				}
			}
			movementIndicator.transform.eulerAngles = moveEulerRotation;

			SpriteRenderer spriter = movementIndicator.GetComponent<SpriteRenderer> ();
			spriter.size = new Vector2 (1f, 1f);

			WaitForFixedUpdate waiter = new WaitForFixedUpdate ();
			float step = 0f;
			while (step < 1f) {
				step += Time.deltaTime;

				spriter.size = new Vector2 (1f, Mathf.Lerp (1f, 2.5f, step));

				athlete.athleteOnFieldObject.transform.position = Vector3.Lerp (athleteStartPos, athleteMovingParent.position, step);

				yield return waiter;
			}

			movementIndicator.SetActive (false);

			break;

		default:
			Debug.Log ("Opportunity " + action.opportunity.id + " no existo.");
			break;
		}

		athlete.athleteOnFieldObject.transform.SetParent (athleteMovingParent);

		currentMatch.BeginAction (athlete, action);
	}

	public IEnumerator WaitThenAdvanceTime() {
		//RemoveFog ();

		homeMatchPanel.SetActive (false);  
		awayMatchPanel.SetActive (false);

		float timeWait = 0.5f;
		float timer = 0;

		while (timer < timeWait) {
			timer += Time.deltaTime;

			yield return new WaitForFixedUpdate ();
		}

		playIndicator.SetActive(false);
		movementIndicator.SetActive (false);

		currentMatch.AdvanceTimeUnit ();

		timeText.text = "Time Left: " + currentMatch.timeUnitsLeft + " seconds";

		UpdateAthleteDescriptorPanels ();
	}

	public void UpdateAthleteDescriptorPanels() {
		for (int i = 0; i < currentMatch.athletesOnField.Count; i++) {
			Athlete athlete = currentMatch.athletesOnField [i];
			if (athlete.activeAction != null) {
				athlete.athletePanelInMatch.descriptorPanel.GetComponent<Image> ().color = descDecidedColor;
				athlete.athletePanelInMatch.descriptorText.text = athlete.activeAction.opportunity.actionVerb + " in " + athlete.activeAction.timeUnitsLeft + " seconds.";
			} else {
				if (currentMatch.athleteWithTurn == athlete) {
					athlete.athletePanelInMatch.descriptorPanel.GetComponent<Image> ().color = descDecidingColor;
					athlete.athletePanelInMatch.descriptorText.text = "Deciding...";
				} else {
					athlete.athletePanelInMatch.descriptorPanel.GetComponent<Image> ().color = descBaseColor;
					athlete.athletePanelInMatch.descriptorText.text = "Waiting for turn...";
				}
			}
		}
	}

	public IEnumerator AnimateSuccessfulMovement(Action action) {
		Athlete athlete = action.athlete;

		Transform newParent = GetFieldCellObject (action.fieldTile.gridX, action.fieldTile.gridY).GetNextAvailablePosition ("center");
		Vector3 athleteStartPos = athlete.athleteOnFieldObject.transform.position;

		WaitForFixedUpdate waiter = new WaitForFixedUpdate ();
		float step = 0f;
		while (step < 1f) {
			step += Time.deltaTime;

			athlete.athleteOnFieldObject.transform.position = Vector3.Lerp (athleteStartPos, newParent.position, step);

			yield return waiter;
		}

		athlete.athleteOnFieldObject.transform.SetParent (newParent);

		currentMatch.CompleteSuccessfulMovement (action);
	}

	/*
	public IEnumerator MoveAthlete(GameObject obj, Vector3 newPosition, float timeTaken) {
		Vector3 startPosition = obj.transform.localPosition;

		float timer = 0;
		float step = 0;

		while (timer < timeTaken) {
			timer += Time.deltaTime;
			step = timer / timeTaken;

			obj.transform.localPosition = Vector3.Lerp (startPosition, newPosition, step);

			yield return new WaitForFixedUpdate ();
		}

		obj.transform.localPosition = newPosition;
	}
	*/
		


	public void UndisplayMatchupUI(Matchup match) {
		Debug.Log ("Calling undisplay");

		ClearField ();

		gameController.topText.transform.parent.gameObject.SetActive (true);
		matchFieldParent.SetActive (false);
		matchUIObject.SetActive (false);

		StartCoroutine (gameController.ZoomAndShiftCameraTo (gameController.cameraStartPosition, gameController.cameraFullSize, 2f));
		//These should probably come after the camera has been unzoomed but isn't a big deal for now.
		GameController.movementPaused = false;
		GameController.canHoverMatches = true;

		gameController.StartTeamMovement ();
	}
}
