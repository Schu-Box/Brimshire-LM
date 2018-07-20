using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchManager : MonoBehaviour {

	public static AthleteMatchPanel selectedAthleteMatchPanel;
	public static OpportunityButton selectedOpportunityButton;
	public static bool actionSelected = false;
	public static Ball selectedBall;

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
	public GameObject tutorialPanel;
	public Text tutorialText;

	//Integrating these new UIs
	public GameObject homeMatchPanel;
	public GameObject awayMatchPanel;
	//NEED TO SET THESE AND ALSO THE ATHLETES MATCH PANELS AND GET RID OF OLD SHIT BRUH
	public List<AthleteMatchPanel> homeAtheteMatchPanelList;
	public List<AthleteMatchPanel> awayAthleteMatchPanelList;
	public GameObject opportunitySelectionPanel;

	[Header("Colors")]
	public Color descBaseColor;
	public Color descDecidingColor;
	public Color descDecidedColor;

	[Header("Match Field")]
	public GameObject matchFieldParent;
	public GameObject matchGridHolder;
	//public GameObject homeGoalzone;
	//public GameObject awayGoalzone;

	[Header("Gameplay UI")]
	public GameObject playIndicator;
	public GameObject movementIndicator;
	public Slider clashSlider;

	[Header("Prefabs")]
	public GameObject athleteOnFieldPrefab;
	public GameObject ballPrefab;

	[Header("Conclusion Panel")]
	public Vector3 conclusionOffPos;
	public Vector3 conclusionOnPos;
	public GameObject matchConclusionPanel;
	public Text resultText;

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
		gameController.matchHoverPanel.SetActive(false);
		
		timeText.gameObject.SetActive (false);
		vsText.gameObject.SetActive (true);
		playIndicator.SetActive (false);
		movementIndicator.SetActive (false);
		clashSlider.gameObject.SetActive(false);
		matchConclusionPanel.SetActive(false);

		matchHomeTitle.transform.parent.GetComponent<Image> ().color = match.homeTeam.teamColor;
		matchHomeTitle.text = match.homeTeam.teamName;
		matchAwayTitle.transform.parent.GetComponent<Image> ().color = match.awayTeam.teamColor;
		matchAwayTitle.text = match.awayTeam.teamName;

		matchHomeScore.text = match.homeScore.ToString ();
		matchAwayScore.text = match.awayScore.ToString ();
		matchHomeScore.transform.parent.GetComponent<Image> ().color = match.homeTeam.teamColor;
		matchAwayScore.transform.parent.GetComponent<Image> ().color = match.awayTeam.teamColor;

		opportunitySelectionPanel.SetActive(true);
		homeMatchPanel.SetActive (true);
		awayMatchPanel.SetActive (true);
		

		/*
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
		*/

		matchContinueButton.GetComponentInChildren<Text> ().text = "Start Match";
		matchContinueButton.onClick.RemoveAllListeners ();
		matchContinueButton.onClick.AddListener (() => match.StartMatch ());
		matchContinueButton.gameObject.SetActive (false);

		matchSimulateButton.GetComponentInChildren<Text> ().text = "Simulate Match";
		matchSimulateButton.onClick.RemoveAllListeners ();
		matchSimulateButton.onClick.AddListener (() => match.SimulateMatch ());

		tutorialPanel.SetActive(true);
		tutorialText.text = "Place " + currentMatch.numAthletesOnFieldPerTeam + " athletes on your half of the field.";


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

		SetRosterPanels();

		ClearOpportunityPanels();

		TeamController playerTeam = null;
		if (match.homeTeam == GameController.playerManager.GetTeam ()) {
			playerTeam = match.homeTeam;
			//matchSelectedAthletePanel.transform.position = matchHomeShield.transform.position;

			match.SetLineup (match.awayTeam);
			for (int i = 0; i < match.awayTeam.rosterList.Count; i++) {
				Athlete athlete = match.awayTeam.rosterList [i];
				if (athlete.currentFieldTile != null) {
					PlaceAthleteInGridCell (athlete, GetFieldCellObject (athlete.currentFieldTile.gridX, athlete.currentFieldTile.gridY));
				}
			}
		} else if (match.awayTeam == GameController.playerManager.GetTeam ()) {
			playerTeam = match.awayTeam;
			//matchSelectedAthletePanel.transform.position = matchAwayShield.transform.position;

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

				cell.highlightSpriter.color = GameController.playerManager.GetTeam().teamColor;
				cell.UnhighlightCell ();

				if (cell.fieldTile.isGoal) {
					if (cell.fieldTile.homeSide) {
						cell.GetComponent<SpriteRenderer> ().color = currentMatch.homeTeam.teamColor;
					} else {
						cell.GetComponent<SpriteRenderer> ().color = currentMatch.awayTeam.teamColor;
					}
				}
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

				//if (!cell.fieldTile.isGoal && !cell.fieldTile.isBoundary) {
					for (int i = 0; i < cell.centerPosition.childCount; i++) {
						deleteThisNephew.Add (cell.centerPosition.GetChild (i).gameObject);
					}
					for (int i = 0; i < cell.topPosition.childCount; i++) {
						deleteThisNephew.Add (cell.topPosition.GetChild (i).gameObject);
					}
					for (int i = 0; i < cell.rightPosition.childCount; i++) {
						deleteThisNephew.Add (cell.rightPosition.GetChild (i).gameObject);
					}
					for (int i = 0; i < cell.bottomPosition.childCount; i++) {
						deleteThisNephew.Add (cell.bottomPosition.GetChild (i).gameObject);
					}
					for (int i = 0; i < cell.leftPosition.childCount; i++) {
						deleteThisNephew.Add (cell.leftPosition.GetChild (i).gameObject);
					}

				//}
			}
		}

		for (int i = deleteThisNephew.Count - 1; i >= 0; i--) {
			GameObject mustDestroy = deleteThisNephew [i];
			deleteThisNephew.Remove (mustDestroy);
			Destroy (mustDestroy);
		}
	}

	public FieldCellController GetFieldCellObject(int rowNum, int columnNum) {
		//Debug.Log ("Getting field cell");
		return fieldGridCells [rowNum] [columnNum];
	}

	public void SetRosterPanels() {

		GameObject rosterPanelSlotHolder = homeMatchPanel.transform.GetChild(0).gameObject;
		for(int i = 0; i < rosterPanelSlotHolder.transform.childCount; i++) {
			AthleteMatchPanel athMatPan = rosterPanelSlotHolder.transform.GetChild(i).GetComponentInChildren<AthleteMatchPanel>();
			if(i < currentMatch.homeTeam.rosterList.Count) {
				athMatPan.SetAthleteMatchPanel(currentMatch.homeTeam.rosterList[i]);
				currentMatch.homeTeam.rosterList[i].athleteMatchPanel = athMatPan;
			} else {
				athMatPan.SetAthleteMatchPanel(null);
			}
		}

		rosterPanelSlotHolder = awayMatchPanel.transform.GetChild(0).gameObject;
		for(int i = 0; i < rosterPanelSlotHolder.transform.childCount; i++) {
			AthleteMatchPanel athMatPan = rosterPanelSlotHolder.transform.GetChild(i).GetComponentInChildren<AthleteMatchPanel>();
			if(i < currentMatch.awayTeam.rosterList.Count) {
				athMatPan.SetAthleteMatchPanel(currentMatch.awayTeam.rosterList[i]);
				currentMatch.awayTeam.rosterList[i].athleteMatchPanel = athMatPan;
			} else {
				athMatPan.SetAthleteMatchPanel(null);
			}
		}
	}

	public void SelectAthlete(AthleteMatchPanel athMatPan) {
		bool alreadySelected = (athMatPan == selectedAthleteMatchPanel);
		UnselectAthlete();
		athMatPan.selectionBorderImg.color = Color.black;

		if(!alreadySelected) {
			selectedAthleteMatchPanel = athMatPan;
			selectedAthleteMatchPanel.selectionBorderImg.enabled = true;

			for (int i = 0; i < fieldGridCells.Count; i++) {
				for (int j = 0; j < fieldGridCells [i].Count; j++) {
					FieldCellController cell = fieldGridCells [i] [j];
					if (!cell.fieldTile.isGoal && !cell.fieldTile.isBoundary) {
						if ((GameController.playerManager.GetTeam () == currentMatch.homeTeam && cell.fieldTile.homeSide)
						|| (GameController.playerManager.GetTeam () == currentMatch.awayTeam && !cell.fieldTile.homeSide)) {
							if (cell.fieldTile.athleteOnTile == null) {
								cell.HighlightCell ();
								cell.SetClickability(true);
							}
						}
					}
				}
			}	
		}
	}

	public void UnselectAthlete() {
		if(selectedAthleteMatchPanel != null) {
			selectedAthleteMatchPanel.selectionBorderImg.enabled = false;
			selectedAthleteMatchPanel = null;
		}

		UnhighlightAllGridCells ();
	}

	public void RemoveAthleteFromField(AthleteMatchPanel athMatPan) {
		UnhighlightAllGridCells ();

		Destroy (athMatPan.athlete.athleteOnFieldObject);

		athMatPan.athlete.GetTeam ().seasonMatchups [GameController.week].RemoveAthleteFromFieldGrid (athMatPan.athlete);

		athMatPan.SetAthleteMatchPanel(athMatPan.athlete);
		UnselectAthlete();

		CheckForGameStart(GameController.playerManager.GetTeam());
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
		UnhighlightAllGridCells ();

		selectedAthleteMatchPanel = null;

		Transform newParent = gridCell.centerPosition;

		Quaternion newQuaternion = Quaternion.identity;
		if (gridCell.fieldTile.gridX >= (fieldGridCells.Count / 2)) {
			newQuaternion.eulerAngles = new Vector3 (0, 180, 0);
		}

		GameObject placedAthleteFieldObj = Instantiate (athleteOnFieldPrefab, newParent.position, newQuaternion, newParent);
		placedAthlete.athleteOnFieldObject = placedAthleteFieldObj;

		SpriteRenderer bodySpriteRend = placedAthleteFieldObj.transform.GetChild (0).GetComponent<SpriteRenderer> ();
		bodySpriteRend.sprite = placedAthlete.bodySprite;

		SpriteRenderer jerseySpriteRend = placedAthleteFieldObj.transform.GetChild (1).GetComponent<SpriteRenderer> ();
		jerseySpriteRend.sprite = placedAthlete.jerseySprite;
		jerseySpriteRend.color = placedAthlete.GetTeam ().teamColor;

		if (placedAthlete.currentFieldTile == null) { //If the athlete is not on the field then add them to the field
			placedAthlete.GetTeam ().seasonMatchups [GameController.week].AddAthleteToFieldGrid (placedAthlete, gridCell.fieldTile); //Put the athlete in the grid data
		}

		placedAthlete.athleteMatchPanel.SetAthleteMatchPanel(placedAthlete);

		/*
		placedAthlete.athleteMatchPanel.selectionBorderImg.enabled = true;
		placedAthlete.athleteMatchPanel.selectionBorderImg.color = placedAthlete.GetTeam().teamColor;
		*/

		CheckForGameStart (GameController.playerManager.GetTeam()); //The player's team
	}

	public void CheckForGameStart(TeamController team) {
		int athletesOnFieldCount = 0;
		if (team.teamManager.currentFieldTile != null) {
			athletesOnFieldCount++;
		}
		for (int i = 0; i < team.rosterList.Count; i++) {
			if (team.rosterList [i].currentFieldTile != null) {
				athletesOnFieldCount++;
			}
		}

		if (athletesOnFieldCount == currentMatch.numAthletesOnFieldPerTeam) {
			matchContinueButton.gameObject.SetActive (true);
		} else if (athletesOnFieldCount > currentMatch.numAthletesOnFieldPerTeam) {
			Debug.Log ("You letting too many chumps into the chumpery.");
			matchContinueButton.gameObject.SetActive (false);
		} else {
			matchContinueButton.gameObject.SetActive (false);
		}
	}

	public IEnumerator AnimateStartedMatchUI() {

		Debug.Log ("Displaying Field Athlete Panel");
		tutorialPanel.SetActive(false);

		UnselectAthlete ();

		matchContinueButton.gameObject.SetActive (false);

		

		WaitForFixedUpdate waiter = new WaitForFixedUpdate();
		float step = 0f;
		while(step < 1.5f) {
			step += Time.deltaTime;

			yield return waiter;
		}

		DisplayStartedMatchUI();
	}

	public void DisplayStartedMatchUI() {
		
		vsText.gameObject.SetActive (false);
		timeText.gameObject.SetActive (true);
		timeText.text = "Time Left: " + '\n' + currentMatch.timeUnitsLeft + " ticks";

		/*
		fieldAthletesPanel.SetActive (true);
		fieldHomeAthletesGroup.GetComponent<Image> ().color = match.homeTeam.teamColor;
		fieldAwayAthletesGroup.GetComponent<Image> ().color = match.awayTeam.teamColor;
		*/

		//Partially reused from SetRosterPanels()
		GameObject rosterPanelSlotHolder = homeMatchPanel.transform.GetChild(0).gameObject;
		for(int i = 0; i < rosterPanelSlotHolder.transform.childCount; i++) {
			AthleteMatchPanel athMatPan = rosterPanelSlotHolder.transform.GetChild(i).GetComponentInChildren<AthleteMatchPanel>();
			if(i < currentMatch.homeTeam.rosterList.Count && currentMatch.homeTeam.rosterList[i].currentFieldTile != null) {
				athMatPan.SetAthleteMatchPanel(currentMatch.homeTeam.rosterList[i]);
			} else {
				if(athMatPan != null) {
					athMatPan.SetAthleteMatchPanel(null);
				}
			}
		}

		rosterPanelSlotHolder = awayMatchPanel.transform.GetChild(0).gameObject;
		for(int i = 0; i < rosterPanelSlotHolder.transform.childCount; i++) {
			AthleteMatchPanel athMatPan = rosterPanelSlotHolder.transform.GetChild(i).GetComponentInChildren<AthleteMatchPanel>();
			if(i < currentMatch.awayTeam.rosterList.Count && currentMatch.awayTeam.rosterList[i].currentFieldTile != null) {
				athMatPan.SetAthleteMatchPanel(currentMatch.awayTeam.rosterList[i]);
			} else {
				if(athMatPan != null) {
					athMatPan.SetAthleteMatchPanel(null);
				}
			}
		}

		/*
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

		for (int i = 0; i < fieldHomeAthletesGroup.transform.childCount; i++) {
			if (fieldHomeAthletesGroup.transform.GetChild (i).GetComponent<AthletePanel> ().athlete == null) {
				fieldHomeAthletesGroup.transform.GetChild (i).GetComponent<AthletePanel> ().SetAthletePanel (null);
			}
		}

		for (int i = 0; i < fieldAwayAthletesGroup.transform.childCount; i++) {
			if (fieldAwayAthletesGroup.transform.GetChild (i).GetComponent<AthletePanel> ().athlete == null) {
				fieldAwayAthletesGroup.transform.GetChild (i).GetComponent<AthletePanel> ().SetAthletePanel (null);
			}
		}
		*/

		if(currentMatch.homeTeam == GameController.playerManager.GetTeam()) {
			BeginPlayerBallPlacement();
		} else {
			currentMatch.AssignBallStarts(currentMatch.homeTeam);
			currentMatch.SetNextTurn();
		}
	}

	public void BeginPlayerBallPlacement() {
		tutorialPanel.SetActive(true);
		tutorialText.text = "Place a ball in your half of the field.";

		for (int i = 0; i < currentMatch.ballList.Count; i++) {
			if (currentMatch.ballList[i].heldByAthlete == null && currentMatch.ballList[i].looseInFieldTile == null) {
				selectedBall = currentMatch.ballList[i];
				break;
			}
		}

		if(selectedBall != null) {
			for (int i = 0; i < fieldGridCells.Count; i++) {
				for (int j = 0; j < fieldGridCells [i].Count; j++) {
					FieldCellController cell = fieldGridCells [i] [j];
					if (!cell.fieldTile.isGoal && !cell.fieldTile.isBoundary) {
						if ((GameController.playerManager.GetTeam () == currentMatch.homeTeam && cell.fieldTile.homeSide)
						|| (GameController.playerManager.GetTeam () == currentMatch.awayTeam && !cell.fieldTile.homeSide)) {
							cell.SetClickability(true);
							cell.HighlightCell ();
						}
					}
				}
			}
		} else { //If all balls have been placed
			tutorialPanel.SetActive(false);

			currentMatch.SetNextTurn();
		}
	}

	public void PlaceBallObject(Ball ball, FieldTile tile) {
		//Debug.Log("Placed ball");
		selectedBall = null;

		UnhighlightAllGridCells();

		Transform newParent = null;
		if(ball.heldByAthlete != null) {
			newParent = ball.heldByAthlete.athleteOnFieldObject.transform;
		} else if(ball.looseInFieldTile != null) {
			newParent = GetFieldCellObject(tile.gridX, tile.gridY).transform;
		} else {
			Debug.Log("Then where the fuck is this ball?");
		}

		GameObject ballObj = Instantiate(ballPrefab, newParent.position, Quaternion.identity, newParent);
		ball.ballObject = ballObj;

		BeginPlayerBallPlacement();
	}

	public void DisplayNewMatchTurn(Matchup match) {
		//Debug.Log("Displaying new match turn");

		actionSelected = false;

		//Might be a better spot for this
		HighlightAthlete(match.athleteWithTurn);

		//UpdateAthleteActionSliders ();

		for (int i = 0; i < opportunitySelectionPanel.transform.childCount; i++) {
			OpportunityButton opportunityButton = opportunitySelectionPanel.transform.GetChild (i).GetChild(0).GetComponent<OpportunityButton> ();
			if (i < match.athleteWithTurn.availableOpportunityList.Count) {
				Opportunity opp = match.athleteWithTurn.availableOpportunityList[i];

				opportunityButton.GetComponent<Image>().enabled = true;

				opportunityButton.opportunity = opp;
				opportunityButton.actionText.text = opp.name;
				opportunityButton.actionDescriptionText.text = opp.description;

				opportunityButton.GetComponent<Button> ().onClick.RemoveAllListeners ();
				if(match.athleteWithTurn.GetTeam() == GameController.playerManager.GetTeam()) {
					opportunityButton.GetComponent<Button> ().interactable = true;
					opportunityButton.GetComponent<Button> ().onClick.AddListener (() => DisplaySelectedOpportunity (currentMatch.athleteWithTurn, opportunityButton));
				} else {
					opportunityButton.GetComponent<Button> ().interactable = false;
				}
			} else {
				opportunityButton.GetComponent<Image>().enabled = false;

				opportunityButton.actionText.text = "";
				opportunityButton.actionDescriptionText.text = "";
				opportunityButton.percentageText.text = "";
				opportunityButton.timeTakenText.text = "";

				opportunityButton.GetComponent<Button> ().onClick.RemoveAllListeners ();
				opportunityButton.GetComponent<Button> ().interactable = false;
			}

			opportunityButton.specificActionPanel.SetActive(false);
		}

		DisplaySelectedOpportunity(currentMatch.athleteWithTurn, opportunitySelectionPanel.transform.GetChild(0).GetChild(0).GetComponent<OpportunityButton>());

		//This shit needs a lot of work and should probably be stored somewhere else.
		if (currentMatch.athleteWithTurn.GetTeam () != GameController.playerManager.GetTeam ()) {
			//Debug.Log ("Displaying AI Turn");

			//Action randomAction = currentMatch.GetAIAction (athlete);
			Opportunity chosenOpportunity = currentMatch.GetAIOpportunity(currentMatch.athleteWithTurn);
			OpportunityButton oppButton = null;
			for (int i = 0; i < opportunitySelectionPanel.transform.childCount; i++) {
				if (opportunitySelectionPanel.transform.GetChild (i).GetChild(0).GetComponent<OpportunityButton> ().opportunity == chosenOpportunity) {
					oppButton = opportunitySelectionPanel.transform.GetChild (i).GetChild(0).GetComponent<OpportunityButton> ();
				}
			}
			if (oppButton == null) {
				Debug.Log ("Action Button is null. Game stops here.");
			} else {
				StartCoroutine (WaitThenDisplaySelectedOpportunity (currentMatch.athleteWithTurn, oppButton));
			}
		}
	}

	public void HighlightAthlete(Athlete athlete) {
		/*
		athlete.athletePanelInMatch.bodyImg.color = Color.white;
		athlete.athletePanelInMatch.jerseyImg.color = athlete.GetTeam ().teamColor;
		*/
		athlete.athleteMatchPanel.selectionBorderImg.color = Color.black;

		playIndicator.SetActive(true); //Display an arrow
		Vector3 playPos = athlete.athleteOnFieldObject.transform.position;
		playPos.y += 0.2f;
		playIndicator.transform.position = playPos;
	}

	public void UnhighlightAthlete(Athlete athlete) {
		athlete.athleteMatchPanel.selectionBorderImg.color = athlete.GetTeam().teamColor;;
	}

	public void ClearOpportunityPanels() {
		for(int i = 0; i < opportunitySelectionPanel.transform.childCount; i++) {
			OpportunityButton opportunityButton = opportunitySelectionPanel.transform.GetChild(i).GetComponentInChildren<OpportunityButton>();

			opportunityButton.GetComponent<Image>().enabled = false;

			opportunityButton.actionText.text = "";
			opportunityButton.actionDescriptionText.text = "";

			opportunityButton.specificActionPanel.SetActive(false);
			opportunityButton.percentageText.text = "";
			opportunityButton.timeTakenText.text = "";

			opportunityButton.GetComponent<Button> ().onClick.RemoveAllListeners ();
			opportunityButton.GetComponent<Button> ().interactable = false;
		}
	}

	public void DisplayValidSelections(Opportunity opportunity) {
		Athlete athlete = currentMatch.athleteWithTurn;

		for(int i = 0; i < opportunity.possibleActions.Count; i++) {
			FieldTile tile = opportunity.possibleActions[i].fieldTile;
			FieldCellController cell = GetFieldCellObject(tile.gridX, tile.gridY);

			if(athlete.GetTeam() == GameController.playerManager.GetTeam()) {
				cell.SetClickability(true);
			}

			cell.highlightSpriter.color = athlete.GetTeam().teamColor;
			cell.HighlightCell();

			if(opportunity != selectedOpportunityButton.opportunity) {
				cell.StartHoverAnimation();
			}
		}
	}

	public void UndisplayValidSelections() {
		for (int i = 0; i < fieldGridCells.Count; i++) {
			for (int j = 0; j < fieldGridCells [i].Count; j++) {
				FieldCellController cell = fieldGridCells[i][j];
				cell.UnhighlightCell ();
				cell.StopHoverAnimation();
			}
		}
	}

	public IEnumerator WaitThenDisplaySelectedOpportunity(Athlete athlete, OpportunityButton oppButt) {
		float timeWait = 0.3f;
		float timer = 0;

		while (timer < timeWait) {
			timer += Time.deltaTime;

			yield return new WaitForFixedUpdate ();
		}

		DisplaySelectedOpportunity (athlete, oppButt);

		Action chosenAction = currentMatch.GetAIAction(athlete, oppButt.opportunity);
		StartCoroutine (WaitThenDisplayBeginAction (athlete, oppButt, chosenAction.fieldTile));
	}

	public void DisplaySelectedOpportunity(Athlete athlete, OpportunityButton oppButt) {
		if(selectedOpportunityButton != null) {
			selectedOpportunityButton.specificActionPanel.SetActive(false);
			selectedOpportunityButton.GetComponent<Button>().interactable = true;
		}

		selectedOpportunityButton = oppButt;
		
		oppButt.GetComponent<Button> ().interactable = false;
		oppButt.specificActionPanel.SetActive(true);
		UndisplaySelectedAction(oppButt);

		UndisplayValidSelections();
		DisplayValidSelections(oppButt.opportunity);
	}

	public IEnumerator WaitThenDisplayBeginAction(Athlete athlete, OpportunityButton oppButt, FieldTile tile) {
		float timeWait = 1f;
		float timer = 0;

		while (timer < timeWait) {
			timer += Time.deltaTime;

			yield return new WaitForFixedUpdate ();
		}

		DisplayBeginAction (athlete, oppButt, tile);
	}

	public void DisplaySelectedAction(OpportunityButton oppButt, FieldTile tile) {
		Action action = null;
		for(int i = 0; i < oppButt.opportunity.possibleActions.Count; i++) {
			if(oppButt.opportunity.possibleActions[i].fieldTile == tile) {
				action = oppButt.opportunity.possibleActions[i];
			}
		}
		if(action == null) {
			Debug.Log("Big time error on ya head top.");
		} else {
			oppButt.percentageText.text = action.chanceSuccess + "% chance";
			oppButt.timeTakenText.text = "in " + action.timeUnitsLeft + " ticks";
		}	
	}

	public void UndisplaySelectedAction(OpportunityButton oppButt) {
		oppButt.percentageText.text = "? % chance";
		oppButt.timeTakenText.text = "in ? ticks";
	}

	public void DisplayBeginAction(Athlete athlete, OpportunityButton oppButt, FieldTile tile) {
		//Debug.Log ("Display begin action");
		actionSelected = true;

		UnhighlightAthlete(currentMatch.athleteWithTurn);
		ClearOpportunityPanels();
		/*
		oppButt.percentageText.text = action.chanceSuccess + "% chance";
		oppButt.timeTakenText.text = "in " + action.timeUnitsLeft + " ticks";
		*/

		UnhighlightAllGridCells ();
		playIndicator.SetActive (false);

		Action action = null;
		for(int i = 0; i < oppButt.opportunity.possibleActions.Count; i++) {
			if(oppButt.opportunity.possibleActions[i].fieldTile == tile) {
				action = oppButt.opportunity.possibleActions[i];
			}
		}	

		athlete.athleteMatchPanel.actionSlider.maxValue = action.timeUnitsLeft;
		athlete.athleteMatchPanel.actionSlider.value = 0;

		athlete.athleteMatchPanel.actionSlider.GetComponentInChildren<Text>().text = action.opportunity.actionVerb + " in " + action.timeUnitsLeft + " ticks";

		StartCoroutine (AnimateBeginAction (athlete, action, tile));
	}

	public IEnumerator AnimateBeginAction(Athlete athlete, Action action, FieldTile tile) {
		Transform athleteMovingParent = null;
		WaitForFixedUpdate waiter = new WaitForFixedUpdate ();
		float step = 0f;

		switch (action.opportunity.id.ToLower ()) {

		case "dribble":
		case "move":
			FieldTile startTile = athlete.currentFieldTile;
			FieldTile endTile = tile;
			Vector3 moveEulerRotation = Vector3.zero;

			Vector3 athleteStartPos = athlete.athleteOnFieldObject.transform.position;
			FieldCellController fieldCell = GetFieldCellObject (startTile.gridX, startTile.gridY);

			movementIndicator.SetActive (true);
			movementIndicator.transform.position = fieldCell.transform.position;

			if (startTile.gridX > endTile.gridX) {
				moveEulerRotation.z = 270;

				athleteMovingParent = fieldCell.leftPosition;
			} else if (startTile.gridX < endTile.gridX) {
				moveEulerRotation.z = 90;

				athleteMovingParent = fieldCell.rightPosition;
			} else {
				if (startTile.gridY > endTile.gridY) {
					moveEulerRotation.z = 0;

					athleteMovingParent = fieldCell.bottomPosition;
				} else if (startTile.gridY < endTile.gridY) {
					moveEulerRotation.z = 180;

					athleteMovingParent = fieldCell.topPosition;
				} else {
					Debug.Log ("That ain't no movement, dawg.");
				}
			}
			movementIndicator.transform.eulerAngles = moveEulerRotation;

			SpriteRenderer spriter = movementIndicator.GetComponent<SpriteRenderer> ();
			spriter.size = new Vector2 (1f, 1f);


			while (step < 1f) {
				step += Time.deltaTime;

				spriter.size = new Vector2 (1f, Mathf.Lerp (1f, 2f, step));

				athlete.athleteOnFieldObject.transform.position = Vector3.Lerp (athleteStartPos, athleteMovingParent.position, step);

				yield return waiter;
			}

			movementIndicator.SetActive (false);

			athlete.athleteOnFieldObject.transform.SetParent (athleteMovingParent);

			break;

		case "kick":
			Debug.Log ("Animate an athlete charging up their kick.");

			while (step < 0.5f) {
				step += Time.deltaTime;

				yield return waiter;
			}
			break;

		case "wait":
			Debug.Log("Animate an athlete waiting");
			break;

		default:
			Debug.Log ("Opportunity " + action.opportunity.id + " no existo.");
			break;
		}

		currentMatch.BeginAction (athlete, action, tile);
	}

	public IEnumerator WaitThenAdvanceTime() {

		WaitForFixedUpdate waiter = new WaitForFixedUpdate();
		float timeWait = 0.5f;
		float timer = 0f;
		while (timer < timeWait) {
			timer += Time.deltaTime;

			yield return waiter;
		}
		playIndicator.SetActive(false);
		movementIndicator.SetActive (false);

		currentMatch.AdvanceTimeUnit ();

		timeText.text = "Time Left: " + '\n' + currentMatch.timeUnitsLeft + " ticks";
		UpdateAthleteActionSliders ();
	}

	public void UpdateAthleteActionSliders() {
		for (int i = 0; i < currentMatch.athletesOnField.Count; i++) {
			Athlete athlete = currentMatch.athletesOnField [i];

			if (athlete.activeAction != null) {
				//athlete.athleteMatchPanel.actionSlider.GetComponent<Image> ().color = descDecidedColor;
				athlete.athleteMatchPanel.actionSlider.GetComponentInChildren<Text>().text = athlete.activeAction.opportunity.actionVerb + " in " + athlete.activeAction.timeUnitsLeft + " ticks";

				athlete.athleteMatchPanel.actionSlider.value = athlete.athleteMatchPanel.actionSlider.maxValue - athlete.activeAction.timeUnitsLeft;
			} else {
				if (currentMatch.athleteWithTurn == athlete) {
					//athlete.athleteMatchPanel.descriptorPanel.GetComponent<Image> ().color = descDecidingColor;
					athlete.athleteMatchPanel.actionSlider.GetComponentInChildren<Text>().text = "Deciding...";
				} else {
					//athlete.athleteMatchPanel.descriptorPanel.GetComponent<Image> ().color = descBaseColor;
					athlete.athleteMatchPanel.actionSlider.GetComponentInChildren<Text>().text = "Waiting for turn...";
				}
			}
		}
	}

	public IEnumerator AnimateClash(Action action) {
		FieldTile tile = action.fieldTile;
		Athlete invader = action.athlete;
		Athlete defender = tile.athleteOnTile;
		
		FieldCellController fieldCell = GetFieldCellObject(tile.gridX, tile.gridY);

		Vector3 invaderBattlePosition = Vector3.zero;
		Vector3 defenderBattlePosition = Vector3.zero;
		if(invader.currentFieldTile.gridX > tile.gridX) {
			invaderBattlePosition = fieldCell.rightPosition.position;
			defenderBattlePosition = fieldCell.leftPosition.position;
		} else if(invader.currentFieldTile.gridX < tile.gridX) {
			invaderBattlePosition = fieldCell.leftPosition.position;
			defenderBattlePosition = fieldCell.rightPosition.position;
		} else if(invader.currentFieldTile.gridY > tile.gridY) {
			invaderBattlePosition = fieldCell.topPosition.position;
			defenderBattlePosition = fieldCell.bottomPosition.position;
		} else if(invader.currentFieldTile.gridY < tile.gridY) {
			invaderBattlePosition = fieldCell.bottomPosition.position;
			defenderBattlePosition = fieldCell.topPosition.position;
		} else {
			Debug.Log("The invader is already in this tile.");
		}
		Vector3 invaderStart = invader.athleteOnFieldObject.transform.position;
		Vector3 defenderStart = defender.athleteOnFieldObject.transform.position;

		WaitForFixedUpdate waiter = new WaitForFixedUpdate();
		float step = 0f;
		while(step < 1f) {
			step += Time.deltaTime;

			invader.athleteOnFieldObject.transform.position = Vector3.Lerp(invaderStart, invaderBattlePosition, step);
			defender.athleteOnFieldObject.transform.position = Vector3.Lerp(defenderStart, defenderBattlePosition, step);

			yield return waiter;
		}

		currentMatch.CompleteClash(action);
	}

	public IEnumerator AnimateClashCompletion(Action action, Athlete defender, int percentChanceForInvader, int randoResult) {
		Athlete invader = action.athlete;

		Transform invaderEndTransform = GetFieldCellObject(invader.currentFieldTile.gridX, invader.currentFieldTile.gridY).centerPosition;
		Transform defenderEndTransform;
		if(defender.currentFieldTile != action.fieldTile) {
			defenderEndTransform = GetFieldCellObject(defender.currentFieldTile.gridX, defender.currentFieldTile.gridY).centerPosition;
		} else {
			defenderEndTransform = defender.athleteOnFieldObject.transform.parent;
		}

		Vector3 invaderStart = invader.athleteOnFieldObject.transform.position;
		Vector3 defenderStart = defender.athleteOnFieldObject.transform.position;

		clashSlider.gameObject.SetActive(true);
		clashSlider.fillRect.GetComponent<Image>().color = invader.GetTeam().teamColor;
		clashSlider.transform.GetChild(0).GetComponent<Image>().color = defender.GetTeam().teamColor;
		clashSlider.value = (float)percentChanceForInvader / 100f;

		Slider indicatorSlider = clashSlider.transform.GetChild(3).GetComponent<Slider>();
		indicatorSlider.value = (float)randoResult / 100f;

		WaitForFixedUpdate waiter = new WaitForFixedUpdate();
		float step = 0f; 
		while(step < 2f) { //Show the clash slider and visualized result.
			step += Time.deltaTime;

			yield return waiter;
		}

		step = 0f;
		while(step < 1f) { //Show the resulting moves of the clash.
			step += Time.deltaTime;

			invader.athleteOnFieldObject.transform.position = Vector3.Lerp(invaderStart, invaderEndTransform.position, step);
			defender.athleteOnFieldObject.transform.position = Vector3.Lerp(defenderStart, defenderEndTransform.position, step);

			yield return waiter;
		}

		invader.athleteOnFieldObject.transform.SetParent(invaderEndTransform);
		defender.athleteOnFieldObject.transform.SetParent(defenderEndTransform);

		clashSlider.gameObject.SetActive(false);

		currentMatch.ConcludeAction(action);
	}

	public IEnumerator AnimateMovementAction(Action action, bool success) {
		Athlete athlete = action.athlete;

		if (success) {
			Transform newParent = GetFieldCellObject (action.fieldTile.gridX, action.fieldTile.gridY).centerPosition;
			Vector3 athleteStartPos = athlete.athleteOnFieldObject.transform.position;

			WaitForFixedUpdate waiter = new WaitForFixedUpdate ();
			float step = 0f;
			while (step < 1f) {
				step += Time.deltaTime;

				athlete.athleteOnFieldObject.transform.position = Vector3.Lerp (athleteStartPos, newParent.position, step);

				yield return waiter;
			}

			athlete.athleteOnFieldObject.transform.SetParent (newParent);
		} else {
			Debug.Log ("Insert a failed movment action here, my sir.");
		}

		currentMatch.ConcludeAction (action);
	}

	//Literally exactly the same as the movement action right now. Not great practice.
	public IEnumerator AnimateDribbleAction(Action action, bool success) {
		Athlete athlete = action.athlete;

		if (success) {
			Transform newParent = GetFieldCellObject (action.fieldTile.gridX, action.fieldTile.gridY).centerPosition;
			Vector3 athleteStartPos = athlete.athleteOnFieldObject.transform.position;

			WaitForFixedUpdate waiter = new WaitForFixedUpdate ();
			float step = 0f;
			while (step < 1f) {
				step += Time.deltaTime;

				athlete.athleteOnFieldObject.transform.position = Vector3.Lerp (athleteStartPos, newParent.position, step);

				yield return waiter;
			}

			athlete.athleteOnFieldObject.transform.SetParent (newParent);
		} else {
			Debug.Log ("Insert a failed movment action here, my sir.");
		}

		currentMatch.ConcludeAction (action);
	}

	public IEnumerator AnimateAthleteGrabBall(Ball ball, Athlete athlete) {
		Vector3 ballStartPos = ball.ballObject.transform.position;

		WaitForFixedUpdate waiter = new WaitForFixedUpdate();
		float step = 0f;
		while(step < 0.5f) {
			step += Time.deltaTime;

			ball.ballObject.transform.position = Vector3.Lerp(ballStartPos, athlete.athleteOnFieldObject.transform.position, step);

			yield return waiter;
		}

		ball.ballObject.transform.position = athlete.athleteOnFieldObject.transform.position;
		ball.ballObject.transform.SetParent(athlete.athleteOnFieldObject.transform);
	}

	public IEnumerator AnimateKickAction(Action action, bool success) {
		//This format only works if success is always true
		Athlete athlete = action.athlete;
		Ball ball = action.ballInvolved;

		//This format will cause bugs if there are multiple balls. Need to get the specific ball that was kicked.
		Transform newParent = null;
		if(ball.heldByAthlete != null) {
			newParent = ball.heldByAthlete.athleteOnFieldObject.transform;
		} else if(ball.looseInFieldTile != null) {
			newParent = GetFieldCellObject(ball.looseInFieldTile.gridX, ball.looseInFieldTile.gridY).transform;
		} else {
			Debug.Log("Major Error, dawg. YOUR BALLS need parents.");
		}
		Vector3 ballStart = ball.ballObject.transform.position;

		WaitForFixedUpdate watier = new WaitForFixedUpdate();
		float step = 0f;
		while(step < 1f) {
			step += Time.deltaTime;

			ball.ballObject.transform.position = Vector3.Lerp(ballStart, newParent.transform.position, step);

			yield return watier;
		}
		ball.ballObject.transform.SetParent(newParent);

		currentMatch.ConcludeAction(action);
	}

	/*
	public IEnumerator AnimateShootAction(Action action, bool success) {
		Athlete athlete = action.athlete;

		Ball ball = athlete.heldBall;
		Transform newParent = GetFieldCellObject (action.fieldTile.gridX, action.fieldTile.gridY).transform;

		Vector3 ballStartPos = ball.ballObject.transform.position;
		Vector3 newBallPos = newParent.position;
		if (!success) {
			if (athlete.GetTeam () == currentMatch.homeTeam) {
				newBallPos.x += 1f;
			} else {
				newBallPos.x -= 1f;
			}
		}

		WaitForFixedUpdate waiter = new WaitForFixedUpdate();
		float step = 0f;
		while (step < 1f) {
			step += Time.deltaTime;

			ball.ballObject.transform.position = Vector3.Lerp(ballStartPos, newBallPos, step);
			yield return waiter;
		}
		ball.ballObject.transform.SetParent (newParent);

		//currentMatch.MoveBallOutOfBounds (ball, action.fieldTile);
	}
	*/

	public IEnumerator AnimateGoal(Action action, Ball ball) {

		WaitForFixedUpdate waiter = new WaitForFixedUpdate();
		float step = 0f;
		while(step < 1f) {
			step += Time.deltaTime;

			yield return waiter;
		}

		UpdateScoreboard();

		step = 0f;
		while(step < 0.5f) {
			step += Time.deltaTime;

			yield return waiter;
		}

		Destroy(ball.ballObject);

		currentMatch.CompleteGoalScoring(ball, action.fieldTile);
	}

	public void UpdateScoreboard() {
		matchHomeScore.text = currentMatch.homeScore.ToString ();
		matchAwayScore.text = currentMatch.awayScore.ToString ();
	}

	public IEnumerator AnimateAthletesReturningToOriginalPositions(TeamController teamScoredOn) {

		List<Vector3> athleteStartList = new List<Vector3>();
		List<Transform> newParentList = new List<Transform>();
		for(int i = 0; i < currentMatch.athletesOnField.Count; i++) {
			Athlete athlete = currentMatch.athletesOnField[i];
			athleteStartList.Add(athlete.athleteOnFieldObject.transform.position);
			newParentList.Add(GetFieldCellObject(athlete.currentFieldTile.gridX, athlete.currentFieldTile.gridY).transform);
		}

		WaitForFixedUpdate waiter = new WaitForFixedUpdate();
		float step = 0f;
		while(step < 1f) {
			step += Time.deltaTime;

			for(int i = 0; i < currentMatch.athletesOnField.Count; i++) {
				currentMatch.athletesOnField[i].athleteOnFieldObject.transform.position = Vector3.Lerp(athleteStartList[i], newParentList[i].position, step);
			}

			yield return waiter;
		}

		for(int i = 0; i < currentMatch.athletesOnField.Count; i++) {
			currentMatch.athletesOnField[i].athleteOnFieldObject.transform.SetParent(newParentList[i]);
		}

		if(teamScoredOn == GameController.playerManager.GetTeam()) {
			BeginPlayerBallPlacement();
		} else {
			currentMatch.AssignBallStarts(teamScoredOn);
			currentMatch.SetNextTurn();
		}
	}

	/*
	public IEnumerator AnimateMoveAthleteToBoundaryThenInbound(Athlete athlete, FieldTile tile) {
		Transform newParent = GetFieldCellObject (tile.gridX, tile.gridY).transform;
		Vector3 athleteStartPos = athlete.athleteOnFieldObject.transform.position;

		WaitForFixedUpdate waiter = new WaitForFixedUpdate ();
		float step = 0f;
		while (step < 1f) {
			step += Time.deltaTime;

			athlete.athleteOnFieldObject.transform.position = Vector3.Lerp (athleteStartPos, newParent.position, step);

			yield return waiter;
		}
		athlete.athleteOnFieldObject.transform.SetParent (newParent);

		Vector3 ballStartPos = athlete.heldBall.ballObject.transform.position;
		step = 0f;
		while (step < 1f) {
			step += Time.deltaTime;

			athlete.heldBall.ballObject.transform.position = Vector3.Lerp (ballStartPos, athlete.athleteOnFieldObject.transform.position, step);

			yield return waiter;
		}
		athlete.heldBall.ballObject.transform.SetParent (athlete.athleteOnFieldObject.transform);

		//currentMatch.ResolveAllActions ();
	}
	*/

	

	public IEnumerator AnimateBallMovingToTile(Ball ball, FieldTile tile, Action lastAction) {
		Transform newParent = GetFieldCellObject (tile.gridX, tile.gridY).transform;
		Vector3 ballStartPos = ball.ballObject.transform.position;

		WaitForFixedUpdate waiter = new WaitForFixedUpdate ();
		float step = 0f;
		while (step < 1f) {
			step += Time.deltaTime;

			ball.ballObject.transform.position = Vector3.Lerp (ballStartPos, newParent.position, step);

			yield return waiter;
		}

		ball.ballObject.transform.SetParent (newParent);

		currentMatch.ConcludeAction (lastAction);
	}
		


	public void DisplayConclusionPanel() {
		timeText.text = "Full Time";

		matchConclusionPanel.SetActive(true);
		matchConclusionPanel.transform.localPosition = conclusionOffPos;

		if(GameController.playerManager.GetTeam() == currentMatch.winner) {
			resultText.text = "VICTORY!";
		} else {
			resultText.text = "Defeat...";
		}

		StartCoroutine(AnimateConclusionPanel());
	}

	public IEnumerator AnimateConclusionPanel() {

		WaitForFixedUpdate waiter = new WaitForFixedUpdate();
		float step = 0;
		while(step < 5f) {
			step += Time.deltaTime;

			matchConclusionPanel.transform.localPosition = Vector3.Lerp(conclusionOffPos, conclusionOnPos, step);

			yield return waiter;
		}
		matchConclusionPanel.transform.localPosition = conclusionOnPos;
	}

	public void UndisplayMatchupUI() {
		Debug.Log ("Calling undisplay");

		ClearField ();

		gameController.topText.transform.parent.gameObject.SetActive (true);
		matchFieldParent.SetActive (false);
		matchUIObject.SetActive (false);

		gameController.ExitMatchUI();
	}
}
