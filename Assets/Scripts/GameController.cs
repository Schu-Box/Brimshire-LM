//Author: Dan Schumacher - Don't steal my shit
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {
	[Header("New Canvas")]

	public GameObject textPrefab;

	[Header("Top Banner")]
	public Button exitButton;
	public Button leagueViewButton;
	public Text topText;
	public Button advanceButton;

	[Header("County Panel")]
	public GameObject countyPanel;
	public Text countyNameText;
	public Text capitalCityNameText;
	public Text countyPopulationText;
	public Text countyPrestigeText;
	public Text countyGoldText;

	[Header("City Panel")]
	public GameObject cityPanel;
	public Image countyColorImage;
	public Text cityNameText;
	public Text cityPopulationText;
	public Text cityPrestigeText;
	public Text cityGoldText;
	public Text cityTeamName;
	public Text cityManagerName;

	[Header("Team Detail Panel")]
	public GameObject teamDetailPanel;
	public Text teamDetailNameText;
	public Text teamDescriptionText;
	public Image teamShieldImage;
	public Image teamLogoPrimaryImage;
	public Image teamLogoSecondaryImage;
	public Text teamEstablishedYearText;
	public Text ownerNameText;
	public List<GameObject> ownerValuesList = new List<GameObject> ();

	public Text teamGoldText;
	public Text teamPrestigeText;

	public GameObject schedulePanel;

	public GameObject managerAthletePanel;
	public List<GameObject> athletePanelsList = new List<GameObject>();

	[Header("Athlete Detail Panel")]
	public GameObject bridgePanel; //Should only be temporary and replaced with bridges connected to athleteDetailPanel eventually
	public GameObject athleteDetailPanel;
	public List<Button> athleteOptionButtons = new List<Button> ();

	public Text ageText;
	public Text raceText;
	public Text originText;
	public Text experienceAsProText;
	public Text experienceWithTeamText;

	public GameObject attributesGroupObj;
	public GameObject seasonStatisticsGroupObj;
	public GameObject careerStatisticsGroupObj;

	[Header("Owner Message Panel")]
	public GameObject ownerMessagePanel;
	public Text ownerDeclarationText;
	public Text ownerBodyText;
	public Text ownerSignatureText;

	public List<GameObject> ownerExpectationsObjectList;
	public List<GameObject> ownerObjectivesObjectList;

	[Header("Match Hover Panel")]
	public GameObject matchHoverPanel;
	public Text matchHoverHomeTeamText;
	public Text matchHoverAwayTeamText;
	public Text matchScoreText;
	public Image matchHomeColorPanel;
	public Image matchAwayColorPanel;

	[Header("Match UI")]
	public GameObject matchUIObject;
	public Text matchHomeTitle;
	public Text matchAwayTitle;
	public Text matchHomeScore;
	public Text matchAwayScore;
	public Button matchContinueButton;
	public Button matchSimulateButton;
	public Image matchHomeShield;
	public Image matchAwayShield;
	public GameObject matchManagerSelectionPanel;
	public GameObject matchAthleteSelectionGroup;
	public GameObject matchSelectedAthlete;

	[Header("Match Field")]
	public GameObject matchFieldObject;
	public GameObject homeGoalzone;
	public GameObject awayGoalzone;
	public GameObject matchFog;

	public GameObject athleteOnFieldPrefab;

	private FieldGridManager fieldGridManager;

	[Header("League Panel")]
	public GameObject leaguePanel;
	public GameObject leagueStandingsHolder;
	public GameObject leagueTeamUIPrefab;
	public GameObject leaguePreviousChampionTeamUI;

	//End of UI

	[Header("Non UI")]
	public GameObject fogLayer;
	public List<GameObject> countyObjects = new List<GameObject> ();
	public GameObject battleIndicator;

	[Header("Races")]
	public List<Sprite> raceSpriteList;
	public List<Sprite> raceJerseyList;

	public List<Race> races = new List<Race> ();

	public static bool canInteractWithMap = true;
	public static bool canHoverMatches = true;
	public static bool movementPaused = false;

	public static CountyController focusedCounty;

	public static Athlete playerManager;
	public static TeamController displayTeam;
	public static Athlete selectedAthleteForPlacement; //This is a bit dubious

	//public static Athlete displayAthlete;
	public static int week = -1;

	private float cameraFullSize;
	private Vector3 cameraStartPosition;
	private bool seasonScheduleDisplayed;
	/*
	private bool athleteInfoExpanded = false;
	private int athleteExpandedInt;
	*/

	public League brimshireLeague;

	void Start () {
		cameraFullSize = Camera.main.orthographicSize;
		cameraStartPosition = Camera.main.transform.position;

		fieldGridManager = FindObjectOfType<FieldGridManager> ();

		exitButton.gameObject.SetActive (false);
		advanceButton.gameObject.SetActive (false);
		countyPanel.SetActive (false);
		cityPanel.SetActive (false);

		athleteDetailPanel.SetActive (false);
		bridgePanel.SetActive (false);
		teamDetailPanel.SetActive (false);
		ownerMessagePanel.SetActive (false);
		matchHoverPanel.SetActive (false);
		matchFieldObject.SetActive (false);
		matchUIObject.SetActive (false);
		leaguePanel.SetActive (false);

		fogLayer.SetActive (false);

		races.Add (new Race ("Chooken", raceSpriteList [0], raceJerseyList [0]));
		races.Add (new Race ("Laguna", raceSpriteList [1], raceJerseyList [1]));
		races.Add (new Race ("Garump", raceSpriteList [2], raceJerseyList [2]));

		brimshireLeague = new League (new List<TeamController> ());
		brimshireLeague.goldRewardPerWin = 10;
		brimshireLeague.prestigeRewardPerWin = 10;

		for (int i = 0; i < countyObjects.Count; i++) { //Initalize every county, which intializes every city, which intialize every team
			countyObjects [i].GetComponent<CountyController> ().InitializeCounty ();
		}

		brimshireLeague.SetSeason ();
		brimshireLeague.SetTeamStandings ();

		leagueViewButton.onClick.AddListener (() => DisplayLeaguePanel (brimshireLeague));
	}

	#region Displaying Counties/Cities

	public void DisplayCountyPanel(CountyController county) {
		countyPanel.SetActive (true);

		Vector3 countyHoverPos = county.transform.position;
		if (county.transform.position.y < 0) {
			countyHoverPos.y += 3.1f;
		} else {
			countyHoverPos.y -= 3.1f;
		}
		countyPanel.transform.position = countyHoverPos;

		countyPanel.GetComponent<Image> ().color = county.countyColor;

		countyNameText.text = county.countyName;
		capitalCityNameText.text = "Capital City of " + county.GetCapitalCity ().cityName;

		countyPopulationText.text = county.GetTotalPopulation ().ToString ();
		countyPrestigeText.text = county.GetTotalPrestige ().ToString();
		countyGoldText.text = county.GetTotalGold ().ToString();

		//countyPanel.GetComponent<UIController> ().MoveToDisplay ();

	}

	public void UndisplayCountyPanel() {
		//countyPanel.GetComponent<UIController> ().MoveAndDisable ();
		countyPanel.SetActive(false);
	}

	public void FocusCounty(CountyController county) {
		if (focusedCounty != null && focusedCounty != county) { //If there was a previously focused county then disable all its cities hoverability
			focusedCounty.GetComponent<SpriteRenderer>().sortingLayerName = "World";

			for (int i = 0; i < focusedCounty.cityObjects.Count; i++) {
				focusedCounty.cityObjects [i].GetComponent<SpriteRenderer> ().enabled = false;
				focusedCounty.cityObjects [i].GetComponent<Collider2D> ().enabled = false;
			}
		}
		focusedCounty = county; //Set the selected county as the focused county

		StartCoroutine (ZoomAndShiftCameraTo (county.countyCamFocalPos, county.countyZoomSize, 0.5f));

		/*
		Camera.main.transform.localPosition = county.countyCamFocalPos;
		Camera.main.orthographicSize = county.countyZoomSize;
		*/

		county.GetComponent<SpriteRenderer> ().sortingLayerName = "Focal County";
		fogLayer.SetActive (true);

		UndisplayCountyPanel ();

		for (int i = 0; i < countyObjects.Count; i++) { //For all counties, make them non-hoverable
			countyObjects [i].GetComponent<CountyController> ().hoverable = false;
		}

		for (int i = 0; i < county.cityObjects.Count; i++) { //For all cities in this county, make them hoverable and enable sprite
			county.cityObjects [i].GetComponent<SpriteRenderer>().enabled = true;
			county.cityObjects [i].GetComponent<Collider2D>().enabled = true;
		}

		UpdateTopPanel ();
	}

	public void ExitCountyToWorldView() {
		StartCoroutine (ZoomAndShiftCameraTo (cameraStartPosition, cameraFullSize, 0.8f));
		/*
		Camera.main.orthographicSize = cameraFullSize;
		Camera.main.transform.position = cameraStartPosition;
		*/

		fogLayer.SetActive (false);

		ownerMessagePanel.SetActive (false);
		teamDetailPanel.SetActive (false); //Technically don't need this here but doesn't hurt

		if (focusedCounty != null) {
			focusedCounty.GetComponent<SpriteRenderer> ().sortingLayerName = "World";
			for (int i = 0; i < focusedCounty.cityObjects.Count; i++) {
				focusedCounty.cityObjects [i].GetComponent<SpriteRenderer> ().enabled = false;
				focusedCounty.cityObjects [i].GetComponent<Collider2D> ().enabled = false;
			}
			focusedCounty = null;
		}

		for (int i = 0; i < countyObjects.Count; i++) {
			countyObjects [i].GetComponent<CountyController> ().hoverable = true;
		}

		UpdateTopPanel ();
	}

	public void DisplayCityPanel(CityController city) {
		cityPanel.SetActive (true);

		Vector3 cityHoverPos = city.transform.position;
		if (city.transform.position.y < Camera.main.transform.position.y) {
			cityHoverPos.y += 0.6f;
		} else {
			cityHoverPos.y -= 0.6f;
		}
		cityPanel.transform.position = cityHoverPos;

		countyColorImage.color = city.GetCounty ().countyColor;

		cityPanel.GetComponent<Image> ().color = city.GetTeamOfCity ().teamColor;

		cityNameText.text = city.cityName;

		cityPopulationText.text = city.GetCityPopulation ().ToString ();
		cityPrestigeText.text = city.GetTeamOfCity ().prestige.ToString ();
		cityGoldText.text = city.GetTeamOfCity ().gold.ToString ();

		string cityTeamNameString = "Home of ";
		if (city.GetTeamOfCity ().thePronoun == true) {
			cityTeamNameString += "the ";
		}
		cityTeamNameString += city.GetTeamOfCity ().teamName;

		cityTeamName.text = cityTeamNameString;
		cityManagerName.text = "Managed by " + city.GetTeamOfCity ().teamManager.name;
	}

	#endregion

	#region Displaying Team
	public void DisplayTeamDetailPanel(TeamController team) {
		displayTeam = team;

		canInteractWithMap = false;

		teamDetailPanel.SetActive (true);

		Vector3 originalPanelPosition = teamDetailPanel.transform.localPosition;
		originalPanelPosition.y = -64;

		Vector3 offsetPanelPosition = originalPanelPosition;
		offsetPanelPosition.y = -1550;
		teamDetailPanel.transform.localPosition = offsetPanelPosition;

		StartCoroutine (MoveObjectFromCurrentPositionTo (teamDetailPanel, originalPanelPosition, 0.5f));

		teamDetailNameText.text = team.teamName;
		if (week > 0) {
			teamDetailNameText.text += " ( " + team.winsThisSeason + " - " + team.losesThisSeason + " )";
		}

		teamLogoPrimaryImage.sprite = team.logoPrimarySprite;
		teamLogoSecondaryImage.sprite = team.logoSecondarySprite;
		teamLogoPrimaryImage.color = team.GetCity ().GetCounty ().GetCapitalCity ().GetTeamOfCity ().teamColor; //The capital team's color
		teamLogoSecondaryImage.color = team.teamColor;

		teamEstablishedYearText.text = "Established " + team.yearEstablished + " in " + team.GetCity ().cityName;

		teamDescriptionText.text = team.teamDescription;

		teamGoldText.text = "Gold: " + team.gold;
		teamPrestigeText.text = "Prestige: " + team.prestige;

		team.gameObject.GetComponent<SpriteRenderer> ().enabled = false;
		cityPanel.SetActive (false);

		UpdateTopPanel ();

		ownerNameText.text = team.owner.name;
		for (int i = 0; i < ownerValuesList.Count; i++) {
			for (int j = 1; j < 5; j++) {
				if (team.owner.ownerValues [i].valueValue >= j - 1) {
					ownerValuesList [i].transform.GetChild (j).GetComponent<Image> ().enabled = true;
				} else {
					ownerValuesList [i].transform.GetChild (j).GetComponent<Image> ().enabled = false;
				}
			}
		}
			
		Text managerNameText = managerAthletePanel.transform.GetChild (0).GetComponent<Text> ();
		Image managerImage = managerAthletePanel.transform.GetChild (1).GetComponent<Image> ();
		Image managerJerseyImage = managerAthletePanel.transform.GetChild (2).GetComponent<Image> ();
		Button managerButton = managerAthletePanel.GetComponent<Button> ();
		Text managerPositionText = managerAthletePanel.transform.GetChild (3).GetComponent<Text> ();

		managerNameText.text = team.teamManager.name;
		managerImage.sprite = team.teamManager.bodySprite;
		managerJerseyImage.sprite = team.teamManager.jerseySprite;
		managerJerseyImage.color = team.teamColor;

		managerButton.onClick.RemoveAllListeners ();
		managerButton.onClick.AddListener (() => DisplayAthletePanel (managerAthletePanel, team.teamManager));

		managerPositionText.text = team.teamManager.positionName;

		//Set the athlete panels
		for (int i = 0; i < athletePanelsList.Count; i++) {
			GameObject athletePanel = athletePanelsList [i];

			Athlete athleteOnPanel = null;
			if (i < team.rosterList.Count) {
				athleteOnPanel = team.rosterList [i];
			}

			Text athleteNameText = athletePanel.transform.GetChild (0).GetComponent<Text> ();
			Image athleteImage = athletePanel.transform.GetChild (1).GetComponent<Image> ();
			Image athleteJerseyImage = athletePanel.transform.GetChild (2).GetComponent<Image> ();
			Button athleteButton = athletePanel.transform.GetComponent<Button> ();
			Text athletePositionText = athletePanel.transform.GetChild (3).GetComponent<Text> ();

			if (athleteOnPanel != null) {

				athleteNameText.enabled = true;
				athleteImage.enabled = true;
				athleteJerseyImage.enabled = true;
				athleteButton.enabled = true;
				athletePositionText.enabled = true;

				athleteNameText.text = athleteOnPanel.name;

				//Should have the image and jersey stored directly on the athletes as opposed to pulling from their race class
				athleteImage.sprite = athleteOnPanel.bodySprite;
				athleteJerseyImage.sprite = athleteOnPanel.jerseySprite;

				athleteJerseyImage.color = team.teamColor;

				athleteButton.onClick.RemoveAllListeners ();
				athleteButton.onClick.AddListener (() => DisplayAthletePanel (athletePanel, athleteOnPanel));

				athletePositionText.text = athleteOnPanel.positionName;
			} else {
				athleteNameText.enabled = false;
				athleteImage.enabled = false;
				athleteJerseyImage.enabled = false;
				athletePositionText.enabled = false;

				//athleteButton.enabled = false;

				athleteButton.onClick.RemoveAllListeners ();
				athleteButton.onClick.AddListener (() => UndisplayAthletePanel ());
			}
		}

		bridgePanel.SetActive (false);
		athleteDetailPanel.SetActive (false);

		for (int i = schedulePanel.transform.childCount - 1; i >= 0; i--) {
			Destroy (schedulePanel.transform.GetChild (i).gameObject);
		}

		for (int i = 0; i < team.seasonMatchups.Count; i++) {
			GameObject newText = Instantiate (textPrefab, schedulePanel.transform.localPosition, Quaternion.identity, schedulePanel.transform);
			string newString;
			Color backColor = newText.GetComponentInChildren<Image> ().color;

			if (team.seasonMatchups [i] == null) {
				newString = "Bye" + '\n' + "Week";
			} else {
				if (team.seasonMatchups [i].homeTeam == team) {
					newString = "vs" + '\n' + team.seasonMatchups [i].awayTeam.GetCity ().cityName;
					backColor = team.seasonMatchups [i].awayTeam.teamColor;
				} else { //This team will be playing away
					newString = "@" + '\n' + team.seasonMatchups [i].homeTeam.GetCity ().cityName;
					backColor = team.seasonMatchups [i].homeTeam.teamColor;
				}

				if (team.seasonMatchups [i].winner != null) { //If the game is over
					if (team.seasonMatchups [i].winner == team) {
						newString += '\n' + "Won ";

						if (team.seasonMatchups [i].homeTeam == team) {
							newString += team.seasonMatchups [i].homeScore.ToString () + " - " + team.seasonMatchups [i].awayScore.ToString ();
						} else { //They're away
							newString += team.seasonMatchups [i].awayScore.ToString () + " - " + team.seasonMatchups [i].homeScore.ToString ();
						}
					} else {
						newString += '\n' + "Lost ";

						if (team.seasonMatchups [i].homeTeam == team) {
							newString += team.seasonMatchups [i].homeScore.ToString () + " - " + team.seasonMatchups [i].awayScore.ToString ();
						} else { //They're away
							newString += team.seasonMatchups [i].awayScore.ToString () + " - " + team.seasonMatchups [i].homeScore.ToString ();
						}
					}
				}
			}

			newText.GetComponentInChildren<Text> ().text = newString;
			newText.GetComponentInChildren<Image> ().color = backColor;
		}
	}

	public void DisplayAthletePanel(GameObject athletePan, Athlete athlete) {

		if (bridgePanel.activeSelf == true && bridgePanel.transform.position == athletePan.transform.position) { //Check to see if it's already that athlete in a dumb way
			UndisplayAthletePanel();
		} else {
			Debug.Log ("Displaying: " + athlete.name);

			Vector3 newTeamDetailPosition = teamDetailPanel.transform.localPosition;
			newTeamDetailPosition.y = 1090; //This will only work for the optimal resolution. Needs to use anchor points to determine the correct location
			StartCoroutine(MoveObjectFromCurrentPositionTo(teamDetailPanel, newTeamDetailPosition, 0.5f));

			bridgePanel.SetActive (true);
			athleteDetailPanel.SetActive (true);
			bridgePanel.transform.position = athletePan.transform.position;

			raceText.text = athlete.race.raceName;
			originText.text = athlete.originCountyName;
			ageText.text = athlete.age + " seasons old";
			experienceAsProText.text = "Pro Athlete for " + athlete.seasonsPlayed + " seasons";
			experienceWithTeamText.text = "With Team for " + athlete.seasonsWithTeam + " seasons";

			for (int i = attributesGroupObj.transform.childCount - 1; i >= 0; i--) {
				Destroy (attributesGroupObj.transform.GetChild (i).gameObject);
			}
				
			for (int i = 0; i < athlete.attributeList.Count; i++) {
				GameObject newAttributeLabel = Instantiate (textPrefab, attributesGroupObj.transform.position, Quaternion.identity, attributesGroupObj.transform);
				newAttributeLabel.GetComponentInChildren<Text> ().text = athlete.attributeList [i].attributeName + '\n' + athlete.attributeList [i].value;
				newAttributeLabel.GetComponentInChildren<Image> ().color = athlete.GetTeam ().teamColor;
			}

			for (int i = seasonStatisticsGroupObj.transform.childCount - 1; i >= 0; i--) {
				Destroy (seasonStatisticsGroupObj.transform.GetChild (i).gameObject);
			}

			for (int i = 0; i < athlete.statisticList.Count; i++) {
				GameObject newStatisticLabel = Instantiate (textPrefab, seasonStatisticsGroupObj.transform.position, Quaternion.identity, seasonStatisticsGroupObj.transform);
				newStatisticLabel.GetComponentInChildren<Text> ().text = athlete.statisticList [i].statName + '\n' + athlete.statisticList [i].value;
				newStatisticLabel.GetComponentInChildren<Image> ().color = athlete.GetTeam ().teamColor;
			}

			for (int i = careerStatisticsGroupObj.transform.childCount - 1; i >= 0; i--) {
				Destroy (careerStatisticsGroupObj.transform.GetChild (i).gameObject);
			}

			for (int i = 0; i < athlete.statisticList.Count; i++) {
				GameObject newStatisticLabel = Instantiate (textPrefab, careerStatisticsGroupObj.transform.position, Quaternion.identity, careerStatisticsGroupObj.transform);
				newStatisticLabel.GetComponentInChildren<Text> ().text = athlete.statisticList [i].statName + '\n' + athlete.statisticList [i].value;
				newStatisticLabel.GetComponentInChildren<Image> ().color = athlete.GetTeam ().teamColor;
			}

			//Athlete Option Buttons
			int activeOptionButtons = 0;

			if (athlete.manager && playerManager == null) {
				activeOptionButtons++;

				athleteOptionButtons [0].GetComponent<Button> ().interactable = true;
				athleteOptionButtons [0].GetComponentInChildren<Text> ().text = "Select Manager";

				athleteOptionButtons [0].onClick.RemoveAllListeners ();
				athleteOptionButtons [0].onClick.AddListener (() => SelectPlayerManager (athlete));
			} else {
				athleteOptionButtons [0].GetComponent<Button> ().interactable = false;
				athleteOptionButtons [0].GetComponentInChildren<Text> ().text = "";
			}

			for (int i = activeOptionButtons; i < athleteOptionButtons.Count; i++) {
				athleteOptionButtons [i].GetComponent<Button> ().interactable = false;
				athleteOptionButtons [i].GetComponentInChildren<Text> ().text = "";
			}
		}
	}

	public void UndisplayAthletePanel() {
		bridgePanel.SetActive (false);
		athleteDetailPanel.SetActive (false);

		Vector3 originalTeamDetailPanelPosition = teamDetailPanel.transform.localPosition;
		originalTeamDetailPanelPosition.y = -64;
		StartCoroutine (MoveObjectFromCurrentPositionTo (teamDetailPanel, originalTeamDetailPanelPosition, 0.5f));

		for (int i = 0; i < athleteOptionButtons.Count; i++) {
			athleteOptionButtons [i].GetComponent<Image> ().enabled = false;
			athleteOptionButtons [i].GetComponentInChildren<Text> ().enabled = false;
		}
	}

	#endregion

	//Throw this in with a back button function
	public void ExitTeamDetailToCountyView() {
		displayTeam = null;

		canInteractWithMap = true;

		teamDetailPanel.SetActive (false);

		FocusCounty (focusedCounty);
	}

	public void SelectPlayerManager(Athlete manager) {
		Debug.Log(manager.name + " is the player now.");
		playerManager = manager;

		teamDetailPanel.SetActive (false);

		ownerMessagePanel.SetActive (true);

		Owner owner = manager.GetTeam().owner;

		string declarationString = "Dear " + manager.name + ",";
		ownerDeclarationText.text = declarationString;

		string teamString = "";
		if (manager.GetTeam ().thePronoun) {
			teamString += "the ";
		}
		teamString += manager.GetTeam ().teamName;

		string bodyString = owner.GetWelcomeMessage ();
		ownerBodyText.text = bodyString;

		string signatureString = "Best of luck. Make me proud," + '\n' + owner.name + '\n' + "Owner of " + teamString;
		ownerSignatureText.text = signatureString;

		UpdateTopPanel ();

		advanceButton.gameObject.SetActive (true);
		advanceButton.GetComponentInChildren<Text> ().text = "Begin Season " + (brimshireLeague.seasonsPlayed + 1);
		advanceButton.onClick.RemoveAllListeners ();
		advanceButton.onClick.AddListener (() => StartSeason ());

		topText.text = "Begin the season.";

		//Expectations and Objectives - Currently only for the player
		owner.AssignExpectations (manager);

		UpdateObjectivesPanel ();
	}

	public void UpdateObjectivesPanel() {
		Owner owner = playerManager.GetTeam ().owner;

		//Expectations
		for (int i = 0; i < ownerExpectationsObjectList.Count && i < owner.expectationsList.Count; i++) {
			ownerExpectationsObjectList [i].GetComponent<Image> ().enabled = true;
			ownerExpectationsObjectList [i].transform.GetChild (0).GetComponent<Text> ().text = owner.expectationsList [i].objectiveString;
			ownerExpectationsObjectList [i].transform.GetChild (1).GetComponent<Text> ().text = owner.expectationsList [i].rewardString;
		}
		for (int i = owner.expectationsList.Count; i < ownerExpectationsObjectList.Count; i++) { //Clear the remaining expectation slots
			ownerExpectationsObjectList [i].GetComponent<Image> ().enabled = false;
			ownerExpectationsObjectList [i].transform.GetChild (0).GetComponent<Text> ().text = "";
			ownerExpectationsObjectList [i].transform.GetChild (1).GetComponent<Text> ().text = "";
		}

		//Bonus Objectives
		for (int i = 0; i < ownerObjectivesObjectList.Count && i < owner.bonusObjectivesList.Count; i++) {
			ownerObjectivesObjectList [i].GetComponent<Image> ().enabled = true;
			ownerObjectivesObjectList [i].transform.GetChild (0).GetComponent<Text> ().text = owner.bonusObjectivesList [i].objectiveString;
			ownerObjectivesObjectList [i].transform.GetChild (1).GetComponent<Text> ().text = owner.bonusObjectivesList [i].rewardString;
		}
		for (int i = owner.bonusObjectivesList.Count; i < ownerObjectivesObjectList.Count; i++) { //Clear the remaining expectation slots
			ownerObjectivesObjectList [i].GetComponent<Image> ().enabled = false;
			ownerObjectivesObjectList [i].transform.GetChild (0).GetComponent<Text> ().text = "";
			ownerObjectivesObjectList [i].transform.GetChild (1).GetComponent<Text> ().text = "";
		}
	}

	//Need to replace these two functions to be more flexible.

	public void ExitMessageToTeam() {
		ownerMessagePanel.SetActive (false);

		DisplayTeamDetailPanel (displayTeam);
	}

	//Should get rid of this clunky ass function. Keep everything in their own functions.
	public void UpdateTopPanel() {
		if (ownerMessagePanel.activeSelf == true) {
			topText.text = "Read the Message";

			exitButton.gameObject.SetActive (true);
			exitButton.GetComponentInChildren<Text> ().text = "Back to Team";
			exitButton.onClick.RemoveAllListeners ();
			exitButton.onClick.AddListener (() => ExitMessageToTeam ());
		} else if (teamDetailPanel.activeSelf == true) {
			//if player hasn't selected a manager
			if (playerManager == null) {
				topText.text = "Become the manager of this team or exit to choose another.";
			}

			exitButton.onClick.RemoveAllListeners ();
			exitButton.onClick.AddListener (() => ExitTeamDetailToCountyView ());
			exitButton.GetComponentInChildren<Text> ().text = "Back to County";
		} else if (focusedCounty != null) {
			if (playerManager == null) {
				topText.text = "Select a city."; //If the player hasn't selected a team tell them to select a team
			}

			exitButton.gameObject.SetActive (true);
			exitButton.onClick.RemoveAllListeners ();
			exitButton.onClick.AddListener (() => ExitCountyToWorldView ());
			exitButton.GetComponentInChildren<Text> ().text = "Back to Kingdom";
		} else { //World view
			exitButton.gameObject.SetActive (false);
			if (playerManager == null) {
				topText.text = "Select a county.";
			}
		}
	}

	public void StartSeason() {
		Debug.Log ("Starting Season " + (brimshireLeague.seasonsPlayed + 1));

		week = -1; //This is the one I should use
		/*
		//DEBUG ONLY START
		if (brimshireLeague.seasonsPlayed < 1) {
			Debug.Log ("Debug Mode");
			week = 7;
		}
		//DEBUG END
		*/
		AdvanceWeek ();
	}

	#region Advancing Week and Setting Matches

	public void AdvanceWeek() {
		week++;
		Debug.Log ("Advancing to Week " + (week + 1));

		canInteractWithMap = false;

		leagueViewButton.gameObject.SetActive (false);
		topText.text = "";

		ExitCountyToWorldView ();
		UndisplayLeaguePanel ();

		if (week < brimshireLeague.regularSeasonLength) { //During the regular season
			for (int i = 0; i < countyObjects.Count; i++) {
				for (int j = 0; j < countyObjects [i].GetComponent<CountyController> ().cityObjects.Count; j++) {
					countyObjects [i].GetComponent<CountyController> ().cityObjects [j].GetComponent<SpriteRenderer> ().enabled = true;
					countyObjects [i].GetComponent<CountyController> ().cityObjects [j].GetComponent<Collider2D> ().enabled = true;
				}
			}

			//dateText.text = "Week " + (week + 1); //Unused

			advanceButton.gameObject.SetActive (false);

			for (int i = 0; i < brimshireLeague.weeklyListOfMatchupsForSeason [week].Count; i++) {
				TeamController homeTeam = brimshireLeague.weeklyListOfMatchupsForSeason [week] [i].homeTeam;
				TeamController awayTeam = brimshireLeague.weeklyListOfMatchupsForSeason [week] [i].awayTeam;
				homeTeam.GetComponent<SpriteRenderer> ().enabled = true;
				awayTeam.GetComponent<SpriteRenderer> ().enabled = true;

				if (homeTeam.GetCityLocation () != (Vector2)homeTeam.transform.position) { //If the home team is not at home, then move them there
					StartCoroutine (TeamMovement (homeTeam, homeTeam.GetCityLocation ()));
				} else {
					brimshireLeague.weeklyListOfMatchupsForSeason [week] [i].firstTeamPresent = true; //Else set firstTeamPresent to true
				}

				if (homeTeam.GetCityLocation () != (Vector2)awayTeam.transform.position) { //If the away team is not at the home city, then move them there
					StartCoroutine (TeamMovement (awayTeam, homeTeam.GetCityLocation ()));
				} else {
					brimshireLeague.weeklyListOfMatchupsForSeason [week] [i].firstTeamPresent = true;
				}
			}

			for (int c = 0; c < countyObjects.Count; c++) {
				//Do the same shit for each county league so they'll play any matches they have. (Should they have matches weekly)?

				League cLeague = countyObjects [c].GetComponent<CountyController> ().countyLeague;

				for (int m = 0; m < cLeague.weeklyListOfMatchupsForSeason [week].Count; m++) {
					//Could create a function for this shiz to slightly reduce code redundancy

					TeamController homeTeam = cLeague.weeklyListOfMatchupsForSeason [week] [m].homeTeam;
					TeamController awayTeam = cLeague.weeklyListOfMatchupsForSeason [week] [m].awayTeam;
					homeTeam.GetComponent<SpriteRenderer> ().enabled = true;
					awayTeam.GetComponent<SpriteRenderer> ().enabled = true;

					if (homeTeam.GetCityLocation () != (Vector2)homeTeam.transform.position) { //If the home team is not at home, then move them there
						StartCoroutine (TeamMovement (homeTeam, homeTeam.GetCityLocation ()));
					} else {
						cLeague.weeklyListOfMatchupsForSeason [week] [m].firstTeamPresent = true; //Else set firstTeamPresent to true
					}

					if (homeTeam.GetCityLocation () != (Vector2)awayTeam.transform.position) { //If the away team is not at the home city, then move them there
						StartCoroutine (TeamMovement (awayTeam, homeTeam.GetCityLocation ()));
					} else {
						cLeague.weeklyListOfMatchupsForSeason [week] [m].firstTeamPresent = true;
					}
				}
			}
		}
	}

	IEnumerator TeamMovement(TeamController team, Vector2 destination) {
		bool areWeThereYet = false;
		float speed = 1;

		while (!areWeThereYet) {

			if(!movementPaused) {

				float step = speed * Time.deltaTime;

				team.transform.position = Vector2.MoveTowards (team.transform.position, destination, step);

				if (team == playerManager.GetTeam()) { //If this team is the player's then have the camera follow along, This will need to be conditional
					Vector3 newCamPosition = team.transform.position;
					newCamPosition.z = cameraStartPosition.z;
					Camera.main.transform.position = newCamPosition;
				}

				if (team.transform.position == (Vector3)destination) {
					areWeThereYet = true;
				}
			}

			yield return null;
		}

		if (team.seasonMatchups [week].winner == null && team.seasonMatchups [week].firstTeamPresent == true) { //If the game has not already been played and the first team is present
			movementPaused = true;

			SetupMatch (team.seasonMatchups [week]);

			if (playerManager.GetTeam() == team.seasonMatchups [week].homeTeam || playerManager.GetTeam() == team.seasonMatchups [week].awayTeam) { //If the team is owned by the player, display match panel
				DisplayMatchUI (team.seasonMatchups[week]);
			}

		} else { //Otherwise, set this bool to true so that games can only be played when both teams are present
			team.seasonMatchups [week].firstTeamPresent = true;
		}

		yield return null;
	}

	public void SetupMatch(Matchup match) {
		match.homeTeam.opponentThisWeek = match.awayTeam;
		match.awayTeam.opponentThisWeek = match.homeTeam;

		match.matchStrings = new List<MatchText> (); //Resets the matchupStrings list
		//match.AddAndUpdateMatchFeed(new MatchText(match.GenerateWelcomeString(), Color.black, true));

		//Instantiate a battle marker for this matchup and change the colors to represent the teams playing
		Vector2 battleSpawn = match.homeTeam.transform.position;
		battleSpawn.y = battleSpawn.y - 0.2f;
		match.battleMarker = Instantiate (battleIndicator, battleSpawn, Quaternion.identity);
		SpriteRenderer[] battlerSprites = new SpriteRenderer[3];
		battlerSprites = match.battleMarker.GetComponentsInChildren<SpriteRenderer> ();

		match.homeTeamMarker = battlerSprites [1];
		match.awayTeamMarker = battlerSprites [2];

		match.awayTeamMarker.color = match.awayTeam.teamColor;
		match.homeTeamMarker.color = match.homeTeam.teamColor;

		match.battleMarker.GetComponent<BattleIndicator> ().match = match;



		//Everything below here is up for refactoring
		/*
		for (int i = 0; i < brimshireLeague.numFieldPositions; i++) {
			match.fieldPositions.Add (new FieldPosition ());
			match.fieldPositions [i].fieldPositionObj = fieldPositionsList [i];
		}
		*/

		//SetActiveLineup (match);

		if (match.homeTeam != playerManager.GetTeam () && match.awayTeam != playerManager.GetTeam ()) {
			match.SimulateMatch ();
		}
	}

	#endregion

	public void DisplayMatchHoverPanel(Matchup match) {
		matchHoverPanel.SetActive (true);

		Vector3 matchHoverPos = match.battleMarker.transform.position;
		if (match.battleMarker.transform.position.y > 0) {
			matchHoverPos.y -= 1.9f;
		} else {
			matchHoverPos.y += 2.3f; //This number is really low because BattleIndicators are currently instantiated as a free object, not dependent on the canvas or anything
		}
		matchHoverPanel.transform.position = matchHoverPos;

		matchHoverHomeTeamText.text = match.homeTeam.teamName;
		matchHoverAwayTeamText.text = match.awayTeam.teamName;

		matchScoreText.text = match.homeScore + " - " + match.awayScore;

		matchHomeColorPanel.color = match.homeTeam.teamColor;
		matchAwayColorPanel.color = match.awayTeam.teamColor;
	}

	#region Universal Tool Functions

	public IEnumerator ZoomAndShiftCameraTo(Vector3 destination, float cameraEndSize, float timeTaken) {
		Vector3 cameraStartPos = Camera.main.transform.position;
		float cameraStartSize = Camera.main.orthographicSize;

		float timer = 0;
		float step = 0;

		while (timer < timeTaken) {
			timer += Time.deltaTime;
			step = timer / timeTaken;

			Camera.main.transform.position = Vector3.Lerp (cameraStartPos, destination, step);
			Camera.main.orthographicSize = Mathf.Lerp (cameraStartSize, cameraEndSize, step);

			yield return new WaitForFixedUpdate ();
		}

		Camera.main.transform.position = destination;
		Camera.main.orthographicSize = cameraEndSize;
	}

	public IEnumerator MoveObjectFromCurrentPositionTo(GameObject obj, Vector3 newPosition, float timeTaken) {
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

	public void SetAthletePanel(GameObject athletePanel, Athlete athlete) { //This function greatly reduces code redundancy since it's an operation performed in team display and match lineup selection
		Text nameText = athletePanel.transform.GetChild (0).GetComponent<Text> ();
		Image body = athletePanel.transform.GetChild (1).GetComponent<Image> ();
		Image jersey = athletePanel.transform.GetChild (2).GetComponent<Image> ();
		Text positionText = athletePanel.transform.GetChild (3).GetComponent<Text> ();

		athletePanel.GetComponent<Button> ().onClick.RemoveAllListeners ();

		if (athlete != null) {
			nameText.text = athlete.name;

			if (!athlete.onField) {
				body.enabled = true;
				jersey.enabled = true;

				body.sprite = athlete.bodySprite;
				jersey.sprite = athlete.jerseySprite;
				jersey.color = athlete.GetTeam ().teamColor;

				//If it's in match display, else add a listener for athlete details
				athletePanel.GetComponent<Button> ().onClick.AddListener (() => SelectAthlete (athlete));
			} else {
				body.enabled = false;
				jersey.enabled = false;
			}

			positionText.text = athlete.positionName;
		} else {
			nameText.text = "";
			body.enabled = false;
			jersey.enabled = false;
			positionText.text = "";
		}
	}

	#endregion

	#region Displaying Matches for Players

	public void DisplayMatchUI (Matchup match) {
		Debug.Log ("Displaying Match UI");

		movementPaused = true;
		canHoverMatches = false;

		//Everything just pops up right now but eventually there should be an unravelling animation to bring it up
		matchUIObject.SetActive (true);
		matchFieldObject.SetActive (true);
		matchFog.SetActive (false);

		matchSelectedAthlete.GetComponent<Button> ().onClick.RemoveAllListeners ();
		matchSelectedAthlete.GetComponent<Button> ().onClick.AddListener (() => UnselectAthlete ());
		matchSelectedAthlete.SetActive (false);

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

		topText.transform.parent.gameObject.SetActive (false);

		matchContinueButton.GetComponentInChildren<Text> ().text = "Start Match";
		matchContinueButton.onClick.RemoveAllListeners ();
		matchContinueButton.onClick.AddListener (() => match.StartMatch ());
		matchContinueButton.gameObject.SetActive (false);

		matchSimulateButton.GetComponentInChildren<Text> ().text = "Simulate Match";
		matchSimulateButton.onClick.RemoveAllListeners ();
		matchSimulateButton.onClick.AddListener (() => match.SimulateMatch ());

		Vector3 matchPosition = match.battleMarker.transform.position;

		matchFieldObject.transform.position = matchPosition;
		matchUIObject.transform.position = matchPosition;

		//Zoom the camera onto the matchUIObject
		float camZ = Camera.main.transform.position.z;
		Vector3 newCamPos = Camera.main.transform.position;

		newCamPos = matchPosition;
		newCamPos.z = camZ;

		StartCoroutine (ZoomAndShiftCameraTo (newCamPos, 1.12f, 2f));

		TeamController playerTeam = null;
		if (match.homeTeam == playerManager.GetTeam ()) {
			playerTeam = match.homeTeam;
			matchSelectedAthlete.transform.position = matchHomeShield.transform.position;

			fieldGridManager.SetLineupPlacementCells (true);
		} else if (match.awayTeam == playerManager.GetTeam ()) {
			playerTeam = match.awayTeam;
			matchSelectedAthlete.transform.position = matchAwayShield.transform.position;

			fieldGridManager.SetLineupPlacementCells (false);
		} else {
			Debug.Log ("HUGE ERROR or maybe you spectating");
		}

		matchAthleteSelectionGroup.transform.parent.gameObject.SetActive (true);
		matchAthleteSelectionGroup.transform.parent.GetComponent<Image> ().color = playerTeam.teamColor;

		fieldGridManager.ClearFieldCells ();

		UpdateRosterSelectionPanel (playerTeam);
	}

	public void UpdateRosterSelectionPanel(TeamController team) {
		SetAthletePanel (matchManagerSelectionPanel, team.teamManager);

		for (int i = 0; i < matchAthleteSelectionGroup.transform.childCount; i++) {
			GameObject athletePanel = matchAthleteSelectionGroup.transform.GetChild (i).gameObject;
			if (i < team.rosterList.Count) {
				SetAthletePanel (athletePanel, team.rosterList [i]);
			} else {
				SetAthletePanel (athletePanel, null);
			}
		}
	}

	public void UndisplayMatchupUI(Matchup match) {
		matchFieldObject.SetActive (false);
		matchUIObject.SetActive (false);

		topText.transform.parent.gameObject.SetActive (true);

		StartCoroutine (ZoomAndShiftCameraTo (cameraStartPosition, cameraFullSize, 2f));

		//These should probably come after the camera has been unzoomed but isn't a big deal for now.
		movementPaused = false;
		canHoverMatches = true;
	}

	public void SelectAthlete(Athlete athlete) {
		selectedAthleteForPlacement = athlete;

		matchAthleteSelectionGroup.transform.parent.gameObject.SetActive (false);

		matchSelectedAthlete.SetActive (true);

		SetAthletePanel (matchSelectedAthlete, selectedAthleteForPlacement);
		/*
		matchSelectedAthlete.transform.GetChild (0).GetComponent<Text> ().text = athlete.name;
		matchSelectedAthlete.transform.GetChild (1).GetComponent<Image> ().sprite = athlete.bodySprite;
		matchSelectedAthlete.transform.GetChild (2).GetComponent<Image> ().sprite = athlete.jerseySprite;
		matchSelectedAthlete.transform.GetChild (2).GetComponent<Image> ().color = athlete.GetTeam ().teamColor;
		matchSelectedAthlete.transform.GetChild (3).GetComponent<Text> ().text = athlete.positionName;
		*/

		matchFog.SetActive (true);
	}

	public void UnselectAthlete() {
		selectedAthleteForPlacement = null;

		matchSelectedAthlete.SetActive (false);

		matchAthleteSelectionGroup.transform.parent.gameObject.SetActive (true);
	}

	public void PlaceAthleteInGridCell(GameObject gridCell) {
		Debug.Log ("Placing Athlete");

		fieldGridManager.RemoveFog ();

		GameObject placedAthleteFieldObj = Instantiate (athleteOnFieldPrefab, gridCell.transform.position, Quaternion.identity, gridCell.transform);

		placedAthleteFieldObj.GetComponent<SpriteRenderer> ().sprite = selectedAthleteForPlacement.bodySprite;

		SpriteRenderer jerseySpriteRend = placedAthleteFieldObj.transform.GetChild (0).GetComponent<SpriteRenderer> ();
		jerseySpriteRend.sprite = selectedAthleteForPlacement.jerseySprite;
		jerseySpriteRend.color = selectedAthleteForPlacement.GetTeam ().teamColor;

		selectedAthleteForPlacement.onField = true;

		selectedAthleteForPlacement = null;
		matchSelectedAthlete.SetActive (false);
	
		TeamController checkedTeam = playerManager.GetTeam ();

		int athletesOnFieldCount = 0;
		if (checkedTeam.teamManager.onField == true) {
			athletesOnFieldCount++;
		}
		for (int i = 0; i < checkedTeam.rosterList.Count; i++) {
			if (checkedTeam.rosterList [i].onField == true) {
				athletesOnFieldCount++;
			}
		}

		if (athletesOnFieldCount == 3) {
			matchContinueButton.gameObject.SetActive (true);
		} else if (athletesOnFieldCount > 3) {
			Debug.Log ("You letting too many chumps into the chumpery.");
		} else {
			matchAthleteSelectionGroup.transform.parent.gameObject.SetActive (true);
			UpdateRosterSelectionPanel (playerManager.GetTeam()); //The player's team
		}
	}

	#endregion

	#region Concluding Week

	public void LastMatchCompleted() {
		Debug.Log ("All matches complete.");

		advanceButton.gameObject.SetActive (true);
		advanceButton.GetComponentInChildren<Text> ().text = "Conclude Week " + (week + 1);
		advanceButton.onClick.RemoveAllListeners ();
		advanceButton.onClick.AddListener (() => ConcludeWeek ());
	}

	public void ConcludeWeek() {
		Debug.Log ("Concluding Week");

		canInteractWithMap = false;

		brimshireLeague.SetTeamStandings ();

		leagueViewButton.gameObject.SetActive (true);

		for (int i = 0; i < brimshireLeague.weeklyListOfMatchupsForSeason [week].Count; i++) { //Clear the battleMarkers from the previous week
			Destroy (brimshireLeague.weeklyListOfMatchupsForSeason [week] [i].battleMarker);
		}

		for (int c = 0; c < countyObjects.Count; c++) {
			League countyLeague = countyObjects [c].GetComponent<CountyController> ().countyLeague;

			for (int m = 0; m < countyLeague.weeklyListOfMatchupsForSeason [week].Count; m++) {
				Destroy (countyLeague.weeklyListOfMatchupsForSeason [week] [m].battleMarker);
			}
		}

		for(int i = 0; i < countyObjects.Count; i++) {
			for (int j = 0; j < countyObjects [i].GetComponent<CountyController> ().cityObjects.Count; j++) { //Disable the sprite of each city across the kingdom
				countyObjects [i].GetComponent<CountyController> ().cityObjects [j].GetComponent<SpriteRenderer> ().enabled = false;
				countyObjects [i].GetComponent<CountyController> ().cityObjects [j].GetComponent<Collider2D> ().enabled = false;
				countyObjects [i].GetComponent<CountyController> ().cityObjects [j].GetComponent<CityController> ().GetTeamOfCity ().GetComponent<SpriteRenderer> ().enabled = false;
			
				/*
				TeamController team = countyObjects [i].GetComponent<CountyController> ().cityObjects [j].GetComponent<CityController> ().GetTeamOfCity ();
				if (team.transform.position != (Vector3)team.GetCityLocation ()) { //If the team is away from home then send them home
					team.transform.position = (Vector3)team.GetCityLocation();
				}
				*/
			}

			countyObjects [i].GetComponent<CountyController> ().IncreaseCountyPopulationByWeek ();
		}

		advanceButton.onClick.RemoveAllListeners ();
		if (week + 1 < brimshireLeague.regularSeasonLength ) { //If the next week is during the regular season
			advanceButton.gameObject.GetComponentInChildren<Text> ().text = "Advance to Week " + (week + 2); //Week is +1 for programming reasons and +1 away from current week
			advanceButton.onClick.AddListener (() => AdvanceWeek ());
		} else { //If the regular season is over
			if (brimshireLeague.numPostseasonRounds > 0) { //If there is a postseason
				advanceButton.gameObject.GetComponentInChildren<Text> ().text = "Begin Postseason";
				advanceButton.onClick.AddListener (() => BeginPostseason ());
			} else { //The season is over
				advanceButton.gameObject.GetComponentInChildren<Text> ().text = "Conclude Season " + (brimshireLeague.seasonsPlayed + 1);
				advanceButton.onClick.AddListener (() => ConcludeSeason ());
			}
		}
		advanceButton.gameObject.SetActive (false);

		ownerMessagePanel.SetActive (true);

		exitButton.gameObject.SetActive (true);
		exitButton.onClick.RemoveAllListeners ();
		exitButton.onClick.AddListener (() => CloseOwnerMessage ());
		exitButton.GetComponentInChildren<Text> ().text = "Close Message";

		Athlete manager = playerManager;
		Owner owner = playerManager.GetTeam ().owner;

		string declarationString = "Dear " + manager.name + ",";
		ownerDeclarationText.text = declarationString;

		string teamString = "";
		if (manager.GetTeam ().thePronoun) {
			teamString += "the ";
		}
		teamString += manager.GetTeam ().teamName;

		string bodyString = owner.GetRegularSeasonMessage ();
		ownerBodyText.text = bodyString;

		string signatureString = "Regards," + '\n' + owner.name + '\n' + "Owner of " + teamString;
		ownerSignatureText.text = signatureString;

		CompleteBonusObjectives (owner.GetCompletedBonusObjectives (manager));
	}

	#endregion

	//Might be some way to combine these two functions
	public void CompleteBonusObjectives(List<Objective> completedBonusObjectives) {
		for (int i = 0; i < ownerObjectivesObjectList.Count; i++) {
			for (int j = 0; j < completedBonusObjectives.Count; j++) {

				Objective completedObjective = completedBonusObjectives [j];

				if (completedObjective.objectiveString == ownerObjectivesObjectList [i].transform.GetChild (0).GetComponent<Text> ().text) {
					//ownerObjectivesObjectList [i].GetComponent<Image> ().enabled = false;
					//ownerObjectivesObjectList [i].transform.GetChild (0).GetComponent<Text> ().text = "Completed";
					ownerObjectivesObjectList [i].transform.GetChild (1).GetComponent<Text> ().text = "Completed";

					switch (completedObjective.rewardID) {
					case "gold":
						playerManager.GetTeam ().gold += completedObjective.rewardValue;
						break;
					}
				}
			}
		}
	}

	public void CompleteExpectations(List<Objective> completedExpectations) {
		for (int i = 0; i < ownerExpectationsObjectList.Count; i++) {
			for (int j = 0; j < completedExpectations.Count; j++) {
				Objective completedExpectation = completedExpectations [j];

				if (completedExpectation.objectiveString == ownerExpectationsObjectList [i].transform.GetChild (0).GetComponent<Text> ().text) {
					ownerExpectationsObjectList [i].transform.GetChild (1).GetComponent<Text> ().text = "Completed";

					switch (completedExpectation.rewardID) {
					case "gold":
						playerManager.GetTeam ().gold += completedExpectation.rewardValue;
						break;
					}
				}
			}
		}
	}

	public void CloseOwnerMessage() {
		canInteractWithMap = true;

		ownerMessagePanel.SetActive(false);

		exitButton.gameObject.SetActive (false);

		advanceButton.gameObject.SetActive (true);
	}

	public void BeginPostseason() {
		Debug.Log ("Postseason");

		advanceButton.GetComponentInChildren<Text> ().text = "Begin Offseason";
		advanceButton.onClick.RemoveAllListeners ();
		advanceButton.onClick.AddListener (() => ConcludeSeason ());
	}

	private void ConcludeSeason() {
		brimshireLeague.seasonsPlayed++;

		brimshireLeague.DetermineChampion();

		CompleteExpectations (playerManager.GetTeam ().owner.GetCompletedExpectations (playerManager));

		//Determine county league champions and what happens to them

		//playerManager.GetTeam().owner.

		BeginOffseason ();
	}

	private void BeginOffseason() {
		Debug.Log ("Offseason");

		canInteractWithMap = false;
		ExitCountyToWorldView ();

		for (int i = 0; i < countyObjects.Count; i++) {
			countyObjects [i].GetComponent<CountyController> ().countyLeague.seasonsPlayed++;

			for (int j = 0; j < countyObjects [i].GetComponent<CountyController> ().cityObjects.Count; j++) {
				TeamController team = countyObjects [i].GetComponent<CountyController> ().cityObjects [j].GetComponent<CityController> ().GetTeamOfCity ();
				if (team.transform.position != (Vector3)team.GetCityLocation ()) { //If the team is away from home then send them home
					team.transform.position = (Vector3)team.GetCityLocation ();
				}
			}
		}

		ownerMessagePanel.SetActive(true);
		ownerBodyText.text = playerManager.GetTeam ().owner.GetEndMessage ();

		exitButton.gameObject.SetActive (true);
		exitButton.onClick.RemoveAllListeners ();
		exitButton.onClick.AddListener (() => CloseOwnerMessage ());
		exitButton.GetComponentInChildren<Text> ().text = "Close Message";

		advanceButton.onClick.RemoveAllListeners ();
		if (true) { //Replace this with some bool checking to see if there's a draft
			advanceButton.GetComponentInChildren<Text> ().text = "Advance to Preseason";
			advanceButton.onClick.AddListener (() => BeginPreseason ());
		} else {
			advanceButton.GetComponentInChildren<Text> ().text = "Begin Draft";
			//league.SetupDraft ();
		}
		advanceButton.gameObject.SetActive (false);
	}

	private void BeginPreseason() {
		Debug.Log ("Preseason");

		canInteractWithMap = false;
		ExitCountyToWorldView ();

		brimshireLeague.SetSeason ();

		for (int i = 0; i < countyObjects.Count; i++) {
			countyObjects [i].GetComponent<CountyController> ().countyLeague.SetSeason ();
		}

		ownerMessagePanel.SetActive (true);
		ownerBodyText.text = playerManager.GetTeam ().owner.GetPreseasonMessage ();

		playerManager.GetTeam ().owner.AssignExpectations (playerManager);
		UpdateObjectivesPanel ();

		exitButton.gameObject.SetActive (true);
		exitButton.onClick.RemoveAllListeners ();
		exitButton.onClick.AddListener (() => CloseOwnerMessage ());
		exitButton.GetComponentInChildren<Text> ().text = "Close Message";

		advanceButton.GetComponentInChildren<Text> ().text = "Begin Season " + (brimshireLeague.seasonsPlayed + 1);
		advanceButton.onClick.RemoveAllListeners ();
		advanceButton.onClick.AddListener (() => StartSeason ());
		advanceButton.gameObject.SetActive (false);
	}

	#region League Panel Display
	public void DisplayLeaguePanel(League league) {
		leaguePanel.SetActive (true);

		for (int i = leagueStandingsHolder.transform.childCount - 1; i >= 0; i--) {
			Destroy (leagueStandingsHolder.transform.GetChild (i).gameObject);
		}

		/*
		while (leagueStandingsHolder.transform.childCount > 0) { //Clears all children
			Destroy (leagueStandingsHolder.transform.GetChild (0));
		}
		*/

		for (int i = 0; i < league.teamStandings.Length; i++) {
			GameObject newStanding = Instantiate (leagueTeamUIPrefab, leagueStandingsHolder.transform.position, Quaternion.identity, leagueStandingsHolder.transform);

			newStanding.GetComponent<Image> ().color = league.teamStandings [i].teamColor;
			newStanding.transform.GetChild(0).GetComponent<Text> ().text = league.teamStandings [i].teamName;
			newStanding.transform.GetChild (1).GetComponent<Text> ().text = league.teamStandings [i].winsThisSeason + " - " + league.teamStandings [i].losesThisSeason;
			newStanding.transform.GetChild (2).GetComponent<Text> ().text = (i + 1).ToString ();
		}

		leagueViewButton.onClick.RemoveAllListeners ();
		leagueViewButton.onClick.AddListener (() => UndisplayLeaguePanel ());

		if (league.seasonsPlayed > 0) {
			TeamController previousChampion = league.leagueChampions [league.seasonsPlayed - 1];
			leaguePreviousChampionTeamUI.GetComponent<Image> ().color = previousChampion.teamColor;
			leaguePreviousChampionTeamUI.transform.GetChild (0).GetComponent<Text> ().text = previousChampion.teamName;
			leaguePreviousChampionTeamUI.transform.GetChild (1).GetComponent<Text> ().text = previousChampion.winsThisSeason + " - " + previousChampion.losesThisSeason;
			leaguePreviousChampionTeamUI.transform.GetChild (2).GetComponent<Text> ().text = "";
		}
	}

	public void UndisplayLeaguePanel() {
		leaguePanel.SetActive (false);

		leagueViewButton.onClick.RemoveAllListeners ();
		leagueViewButton.onClick.AddListener (() => DisplayLeaguePanel (brimshireLeague));
	}
	#endregion








	/*
	public void SetActiveLineup(Matchup match) {
		TeamController home = match.homeTeam;
		TeamController away = match.awayTeam;

		AthleteUI.disabledInteraction = false;
		AthleteUI.lineupSelection = true;

		for (int i = 0; i < league.activeAthletesPerTeam; i++) {
			//Placeholder currently always adds the first 3 athletes on an AI's roster
			if (playerTeam != home) {
				AddToActiveLineup (match, home.rosterList [i]); 
				home.rosterList [i].onActiveRoster = true;
			}

			if (playerTeam != away) {
				AddToActiveLineup (match, away.rosterList [i]); 
				away.rosterList [i].onActiveRoster = true;
			}
		}
	}

	public void AddToActiveLineup(Matchup match, Athlete athlete) {
		bool alreadyOnTeam = false;

		if (athlete.GetTeam () == match.homeTeam) {
			for (int i = 0; i < match.activeHomeAthletes.Count; i++) {
				if (match.activeHomeAthletes [i] == athlete) {
					alreadyOnTeam = true;
				}
			}

			if (alreadyOnTeam) {
				match.activeHomeAthletes.Remove (athlete);
				athlete.onActiveRoster = false;

				if (matchUI.activeSelf == true) {
					athlete.athUI.GetComponent<Image> ().enabled = false;
					Destroy (athlete.fieldUI);
				}

				if (matchInteractionButton.gameObject.activeSelf == true) {
					matchInteractionButton.gameObject.SetActive (false);
					matchNotificationPanel.SetActive (true);
				}
			} else {
				if (match.activeHomeAthletes.Count < league.activeAthletesPerTeam) {
					match.activeHomeAthletes.Add (athlete);
					athlete.onActiveRoster = true;

					if (matchUI.activeSelf == true) {
						athlete.athUI.GetComponent<Image> ().enabled = true;

						//Instantiate a field UI element in the first position
						//Should make this into a function
						GameObject athleteFieldUI = Instantiate (athleteFieldUIPrefab, Vector3.zero, Quaternion.identity, fieldPositionsList [0].transform);
						athleteFieldUI.GetComponent<Image> ().overrideSprite = athlete.race.raceSprite;
						athleteFieldUI.transform.GetChild(0).GetComponent<Image> ().overrideSprite = athlete.race.raceJersey;
						athleteFieldUI.transform.GetChild (0).GetComponent<Image> ().color = athlete.GetTeam ().teamColor;
						athlete.fieldUI = athleteFieldUI;
						athlete.fieldPosition = 0;

						if (match.activeHomeAthletes.Count == league.activeAthletesPerTeam && match.homeTeam == playerTeam) {
							matchInteractionButton.gameObject.SetActive (true);
							matchNotificationPanel.SetActive (false);
						}
					}
				}
			}
		} else {
			for (int i = 0; i < match.activeAwayAthletes.Count; i++) {
				if (match.activeAwayAthletes [i] == athlete) {
					alreadyOnTeam = true;
				}
			}

			if (alreadyOnTeam) {
				match.activeAwayAthletes.Remove (athlete);
				athlete.onActiveRoster = false;

				if (matchUI.activeSelf == true) {
					athlete.athUI.GetComponent<Image> ().enabled = false;
					Destroy (athlete.fieldUI);
				}

				if (matchInteractionButton.gameObject.activeSelf == true) {
					matchInteractionButton.gameObject.SetActive (false);
					matchNotificationPanel.SetActive (true);
				}
			} else {
				if (match.activeAwayAthletes.Count < league.activeAthletesPerTeam) {
					match.activeAwayAthletes.Add (athlete);
					athlete.onActiveRoster = true;

					if (matchUI.activeSelf == true) {
						athlete.athUI.GetComponent<Image> ().enabled = true;

						GameObject athleteFieldUI = Instantiate (athleteFieldUIPrefab, Vector3.zero, Quaternion.identity, fieldPositionsList [fieldPositionsList.Count - 1].transform);
						athleteFieldUI.GetComponent<Image> ().overrideSprite = athlete.race.raceSprite;
						athleteFieldUI.transform.GetChild(0).GetComponent<Image> ().overrideSprite = athlete.race.raceJersey;
						athleteFieldUI.transform.GetChild (0).GetComponent<Image> ().color = athlete.GetTeam ().teamColor;
						athleteFieldUI.transform.localRotation = Quaternion.Euler (0, 180, 0);
						athlete.fieldUI = athleteFieldUI;
						athlete.fieldPosition = fieldPositionsList.Count - 1;

						if (match.activeAwayAthletes.Count == league.activeAthletesPerTeam && match.awayTeam == playerTeam) {
							matchInteractionButton.gameObject.SetActive (true);
							matchNotificationPanel.SetActive (false);
						}
					}
				}
			}
		}
	}
	*/

	/*
	public void StartWaitBeforeNextAttempt(Matchup match, Athlete athlete, Opportunity opp) {
		if (athlete.GetTeam () == GameController.playerTeam) {

			for (int i = 0; i < opportunityButtons.Length; i++) { //Resets all opportunityButtons
				opportunityButtons [i].gameObject.SetActive (true);
				opportunityButtons [i].onClick.RemoveAllListeners ();
			}
			opportunityPanel.SetActive (false);
		}

		if (matchUI.activeSelf == true) {
			StartCoroutine (WaitBeforeNextAttempt (match, athlete, opp));
		} else {
			match.AttemptOpportunity (athlete, opp);
		}
	}


	private IEnumerator WaitBeforeNextAttempt(Matchup match, Athlete athlete, Opportunity opp) {
		yield return new WaitForSeconds (1);
		match.AttemptOpportunity (athlete, opp);
	}

	public void InstantiateMatchNotification(Athlete athlete, string notifier) {

		Debug.Log ("Notey");

		GameObject obj = Instantiate (matchNotificationObj, FindObjectOfType<Canvas>().transform);

		matchNotificationObjList.Add (obj);

		obj.transform.position = athlete.fieldUI.transform.position + new Vector3(0, 0.8f);

		obj.GetComponent<Text> ().color = athlete.GetTeam ().teamColor;
		obj.GetComponent<Text>().text = notifier;

	}

	public void DestroyMatchNotifications() {
		for (int i = 0; i < matchNotificationObjList.Count; i++) {
			Destroy (matchNotificationObjList [i]);
		}

		matchNotificationObjList = new List<GameObject> ();
	}
	*/

	/*

	public void DisplayAthleteDetails(Athlete athlete) {
		//athleteDetailPanel.SetActive (true);
		//athleteDetailPanel.GetComponent<Image> ().color = athlete.GetTeam ().teamColor;

		athleteImage.overrideSprite = athlete.race.raceSprite;
		athleteJersey.overrideSprite = athlete.race.raceJersey;

		athleteNameText.text = athlete.name;
		if (athlete.GetTeam () != null) {
			athleteJersey.color = athlete.GetTeam ().teamColor;
		}

		athleteRaceText.text = "Race: " + athlete.race.raceName;
		athleteAgeText.text = "Age: " + athlete.age;
		athleteOriginText.text = "Origin: " + athlete.originCountyName;

		athleteSeasonsPlayedText.text = "Seasons: " + athlete.seasonsPlayed;
		athleteMatchesPlayedText.text = "Matches: " + athlete.matchesPlayed;
		athleteWLText.text = "W: " + athlete.wins + "    L: " + athlete.losses;
		athleteScoresText.text = "Scores: " + athlete.scores;
		athleteAssistsText.text = "Assists: " + athlete.assists;
		athleteBoardsText.text = "Boards: " + athlete.rebounds;
		athleteTacklesText.text = "Tackles: " + athlete.tackles;
		athleteStealsText.text = "Steals: " + athlete.steals;
		athleteSavesText.text = "Saves: " + athlete.saves;
		athleteTurnoversText.text = "Turnovers: " + athlete.turnovers;
	}

	public void UndisplayAthleteDetails() {
		//athleteDetailPanel.SetActive (false);
	}

	//Currently unused

	public void ActivateRenameAthlete(AthleteUI athUI) {
		if (renameAthleteInputField.gameObject.activeSelf == false) {
			renameAthleteInputField.gameObject.SetActive(true);
			Debug.Log (athUI.athlete.name);
			athUI.nameText.text = "";
		} else {
			if (renameAthleteInputField.text != "Enter Name" && renameAthleteInputField.text != "") {
				displayAthlete.name = renameAthleteInputField.text;
				//UpdateNamesOnTeamPanel ();
			}
				
			renameAthleteInputField.gameObject.SetActive (false);
		}
	}

*/


	/*
	private int CheckHeadToHead(TeamController team1, TeamController team2) {
		int headToHeadRecordComparison = 0;

		for (int i = 0; i < league.matchupArrayForSeason.Length; i++) {
			for(int j = 0; j < league.matchupArrayForSeason[i].Length; j++) {
				if ((league.matchupArrayForSeason [i] [j].homeTeam == team1 && league.matchupArrayForSeason [i] [j].awayTeam == team2) 
					|| (league.matchupArrayForSeason [i] [j].homeTeam == team2 && league.matchupArrayForSeason [i] [j].awayTeam == team1)) {
					Matchup headToHeadMatch = league.matchupArrayForSeason [i] [j];
					if (headToHeadMatch.winner == team1) { //Team1 won
						headToHeadRecordComparison++;
					} else if (headToHeadMatch.winner == team2) { //Team2 won
						headToHeadRecordComparison--;
					} //Else it hasn't been played or is some kind of tie
				}
			}
		}

		return headToHeadRecordComparison;
	}

	private int CheckHomeRecord(TeamController team) {
		int homeWins = 0;

		for (int i = 0; i < team.seasonMatchups.Count; i++) {
			if (team.seasonMatchups [i].winner != null) { //If the game has already been played
				if (team.seasonMatchups [i].homeTeam == team && team.seasonMatchups[i].winner == team) {
					homeWins++;
				}
			}
		}

		return homeWins;
	}
	*/

	/*
	private void ConcludePostseason() {
		league.postseasonChampion = league.weeklyListOfMatchupsForSeason [league.weeklyListOfMatchupsForSeason.Count - 1] [0].winner;

		advanceWeekButton.gameObject.SetActive (true);

		draftConfirmationButton.gameObject.SetActive (true);
		messageText.text = "Congratulations to the " + league.postseasonChampion.teamName + "!";
		advanceWeekText.text = "Begin Offseason";
		advanceWeekButton.onClick.RemoveAllListeners ();
		advanceWeekButton.onClick.AddListener (() => BeginOffseason ()); 
	}
		
	public void SelectDraftee(Athlete athlete) {
		drafteeInfoPanel.SetActive (true);
		drafteeNameText.text = athlete.name;
		//GameController.disabledCountySelection = true;
		draftConfirmationButton.gameObject.SetActive (true);
		draftConfirmationButton.onClick.RemoveAllListeners ();
		draftConfirmationButton.onClick.AddListener (() => ConfirmDraftChoice (athlete, GameController.playerTeam));


	}

	public void ConfirmDraftChoice(Athlete athlete, TeamController team) {
		Debug.Log ("Added " + athlete.name + " to team " + team.teamName);

		//Remove them from the draft board
		athlete.athUI.nameText.text = "";
		athlete.athUI.forceText.text = "";
		athlete.athUI.resilienceText.text = "";
		athlete.athUI.agilityText.text = "";
		athlete.athUI.tacticsText.text = "";
		athlete.athUI.transform.SetSiblingIndex(league.draftClassSize); //Sets the UI gameObject at the last slot of the hierarchy

		athlete.athUI.GetComponent<Image>().enabled = false;
		draftConfirmationButton.onClick.RemoveAllListeners ();
		draftConfirmationButton.gameObject.SetActive (false);
		//disabledCountySelection = false;
		drafteeInfoPanel.SetActive (false);

		team.GetDraftOrderUI ().draftedAthleteText.text = athlete.name;

		team.SetDrafting (false);

		league.RemoveFromDraftList (athlete);

		athlete.athUI.athlete = null;
		athlete.athUI = null;

		team.AddAthleteToRoster(athlete); //Add this player to the team's roster

		league.ContinueDraft ();
	}

	public IEnumerator WaitThenDraftStep(League leg) {
		yield return new WaitForSeconds (1);
		leg.DraftStep ();
	}

	*/
}