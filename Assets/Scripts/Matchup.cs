﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchText {
	public string matchString;
	public Color matchTextColor = Color.black;
	public bool disabledOutline = false;

	public MatchText(string stringy, Color colory) {
		matchString = stringy;
		matchTextColor = colory;
	}

	public MatchText(string stringy, Color colory, bool disably) {
		matchString = stringy;
		matchTextColor = colory;
		disabledOutline = disably;
	}
}

public class Opportunity {
	public string id;
	public string name;
	public string description;
	public string actionVerb;

	public int baseTimeTaken;
	public int basePercentChanceSuccess;

	public int tileRange;

	public List<Action> possibleActions = new List<Action>();

	public Opportunity(string identifier, string officialName, string descriptionOfOpportunity, string verb, int baseTime, int baseChance, int range) {
		id = identifier;
		name = officialName;
		description = descriptionOfOpportunity;
		actionVerb = verb;

		baseTimeTaken = baseTime;
		basePercentChanceSuccess = baseChance;

		tileRange = range;
	}
}

public class Action {
	public Athlete athlete;
	public Opportunity opportunity;

	public int timeUnitsLeft = 0;
	public int chanceSuccess = 0;
	public int randomResult = -1;
	public bool success = false;

	public FieldTile fieldTile;
	public Ball ballInvolved = null;

	public Action(Athlete a, Opportunity opp, FieldTile tile) {
		athlete = a;
		opportunity = opp;
		fieldTile = tile;
	}
}

public class FieldTile {
	public int gridX = -1;
	public int gridY = -1;
	public List<FieldTile> neighborList = new List<FieldTile> ();
	public bool homeSide;

	public bool isGoal = false;
	//public bool isBoundary = false;

	//public List<Athlete> athletesOnTile = new List<Athlete> ();
	public Athlete athleteOnTile;
	public Ball ballLooseInTile;
}

public class Ball {
	public FieldTile looseInFieldTile = null;
	public Athlete heldByAthlete = null;

	public GameObject ballObject = null;

	public bool scored = false;
}

public class Matchup {

	public GameController gameController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();
	public MatchManager matchManager = GameObject.FindGameObjectWithTag ("MatchManager").GetComponent<MatchManager> ();

	public League league;

	public int weekIndex;
	public TeamController homeTeam;
	public TeamController awayTeam;

	public Matchup(League l, int week, TeamController home, TeamController away) {
		weekIndex = week;
		homeTeam = home;
		awayTeam = away;

		league = l;
	}

	public bool firstTeamPresent = false;
	//Before Match ^

	public int xFieldTiles = 6;
	public int yFieldTiles = 3;
	public List<List<FieldTile>> fieldGrid = new List<List<FieldTile>> ();

	//During Match v
	public int numAthletesOnFieldPerTeam = 2;
	public int numBalls = 1;

	public List<Athlete> athletesOnField = new List<Athlete> ();
	public Athlete athleteWithTurn = null;

	public int timeUnitsLeft = 100;

	//public List<Action> actionExecutionOrder = new List<Action> ();
	public List<Ball> ballList = new List<Ball> ();

	//Old and being refactorized
	/*
	public List<Play> playOrder = new List<Play> ();
	public int playNumber = -1;
	public int changesInPossession = 0;
	public int possessionsPerHalf = 10;
	*/

	public int homeScore = 0;
	public int awayScore = 0;
	public TeamController winner;
	public GameObject battleMarker;
	public SpriteRenderer homeTeamMarker;
	public SpriteRenderer awayTeamMarker;
	//public List<MatchText> matchStrings = new List<MatchText> ();

	public bool matchStarted = false;

	/*
	public bool postseasonMatch = false;

	private bool secondHalf = false;

	private List<Opportunity> opportunityList = new List<Opportunity> {
		//Opportunities w/ the Ball
		new Opportunity("pass", "Pass the Rock", "Pass to a teammate in the same position. After passing this athlete gets a play without the ball.", 75, 95, true, new List<string> {"T"}),
		new Opportunity("through", "Play it Through", "Pass ahead to a teammate. After passing this athlete gets a play without the ball.", 60, 90, true, new List<string> {"T"}),
		new Opportunity("shoot", "Take the Shot", "Take a shot on goal.", 40, 80, true, new List<string> {"F", "A"}),
		new Opportunity("heave", "Desperation Heave", "Desperate times call for desperate measures.", 10, 35, true, new List<string> {"F"}),
		new Opportunity("drive", "Dribble Drive", "Take a shot on goal from beyond the midline.", 80, 100, true, new List<string> {"A"}), //This one should be nearly automatic unless there are opponents in the same position as the one being advanced into
		new Opportunity("dunk", "Dunk on 'Em", "Forcefully dunk on your opponent.", 70, 95, true, new List<string> {"F"}),
		new Opportunity("juke", "Break their Ankles", "Juke out an opponent and send them falling backwards.", 50, 80, true, new List<string> {"A", "T"}),

		//Opportunities w/o the Ball
		new Opportunity("defense", "Defensive Stance", "When an opponent in your position has an opportunity, you have a chance to stop them.", 100, 100, false, new List<string> {"R", "A"}),
		new Opportunity("protect", "Protect the Net", "You have a chance to block the next shot on goal.", 100, 100, false, new List<string> {"R", "A"}),
		new Opportunity("steal", "Pick a Pocket", "Attempt to prevent the next opponent in this position's play and take the ball.", 100, 100, false, new List<string> {"R", "A", "T"})
	};
	*/

	private List<Opportunity> basicOpportunityList = new List<Opportunity> {
		//Without the ball
		new Opportunity("move", "Move", "Move to an adjacent field tile.", "Moving", 10, 100, 1), //Failure technically needs to be implemented

		//With the ball
		new Opportunity("dribble", "Dribble", "Dribble the ball to an adjacent field tile.", "Dribbling", 12, 100, 1), //Failure needs to be implemented
		new Opportunity("kick", "Short Kick", "Kick the ball to an adjacent tile.", "Kicking", 6, 70, 2)

		//new Opportunity("wait", "Hold Position", "Remain in this field tile for a few ticks.", "Waiting", 5, 100)
	};

	public void SetField() {
		fieldGrid = new List<List<FieldTile>> ();
		for (int x = 0; x < xFieldTiles; x++) {
			fieldGrid.Add (new List<FieldTile> ());
			for (int y = 0; y < yFieldTiles; y++) {
				fieldGrid [x].Add (new FieldTile ());
				fieldGrid [x] [y].gridX = x;
				fieldGrid [x] [y].gridY = y;
			}
		}
			
		for (int x = 0; x < fieldGrid.Count; x++) { //Set the tile neighbors and side
			for (int y = 0; y < fieldGrid[x].Count; y++) {
				FieldTile fieldTile = fieldGrid [x] [y];
				if (x < fieldGrid.Count - 1) {
					fieldTile.neighborList.Add (fieldGrid [x + 1] [y]);
				}
				if (x > 0) {
					fieldTile.neighborList.Add (fieldGrid [x - 1] [y]);
				}
				if (y < fieldGrid [x].Count - 1) {
					fieldTile.neighborList.Add (fieldGrid [x] [y + 1]);
				}
				if (y > 0) {
					fieldTile.neighborList.Add (fieldGrid [x] [y - 1]);
				}

				if (x < fieldGrid.Count / 2) {
					fieldTile.homeSide = true;
				} else {
					fieldTile.homeSide = false;
				}

				if (x == 0 || x == fieldGrid.Count - 1) {
					fieldGrid [x] [y].isGoal = true;
				}
			}
		}


        for (int i = 0; i < numBalls; i++) {
            ballList.Add(new Ball());
        }

		SetBallStart(homeTeam);
	}

	public void SetBallStart(TeamController team) {
		Debug.Log("Setting ball start");

		int xSpot;
		if(team == homeTeam) {
			xSpot = (fieldGrid.Count / 2) - 1;
		} else {
			xSpot = (fieldGrid.Count / 2);
		}
		int ySpot = (fieldGrid[xSpot].Count / 2);

		PlaceBallOnTile(ballList[0], fieldGrid[xSpot][ySpot]);
	}

	public void PlaceBallOnTile(Ball ball, FieldTile tile) {
		if(tile.athleteOnTile != null) {
			tile.athleteOnTile.heldBall = ball;
			ball.heldByAthlete = tile.athleteOnTile;
		} else {
			tile.ballLooseInTile = ball;
			ball.looseInFieldTile = tile;
		}

		if(matchManager.matchFieldParent.activeSelf == true) {
			matchManager.PlaceBallObject(ball, tile);
		}
	}

	public void SetLineup(TeamController team) {
		for (int i = 0; i < numAthletesOnFieldPerTeam; i++) {
			List<FieldTile> validPlacementTiles = new List<FieldTile> ();
			for (int x = 0; x < fieldGrid.Count; x++) {
				for (int y = 0; y < fieldGrid[x].Count; y++) {
					FieldTile tile = fieldGrid [x] [y];
					if (!tile.isGoal) {
						if (fieldGrid [x] [y].athleteOnTile == null) {
							if ((team == homeTeam && tile.homeSide) || (team == awayTeam && !tile.homeSide))
								validPlacementTiles.Add (fieldGrid [x] [y]);
						}
					}
				}
			}
			AddAthleteToFieldGrid (team.rosterList [i], validPlacementTiles [Random.Range (0, validPlacementTiles.Count)]);
		}
	}

	public void AddAthleteToFieldGrid(Athlete athlete, FieldTile tile) {
		tile.athleteOnTile = athlete;
		athlete.currentFieldTile = tile;
		athlete.originalLineupTile = tile;

		if(tile.ballLooseInTile != null) {
			//Give the player the ball
			Debug.Log("Athlete starts with a ball");
			Ball ball = tile.ballLooseInTile;

			StartAthleteWithBall(ball, athlete);
		}
	}

	public void StartAthleteWithBall(Ball ball, Athlete athlete) {
		GiveBallToAthlete(ball, athlete);

		if(matchManager.matchFieldParent.activeSelf == true) {
			matchManager.SetBallParentAthlete(ball, athlete);
		}
	}

	public void RemoveAthleteFromFieldGrid(Athlete athlete) {
		FieldTile tile = athlete.currentFieldTile;

		tile.athleteOnTile = null;
		athlete.currentFieldTile = null;
		athlete.originalLineupTile = null;

		if(athlete.heldBall != null) {
			Ball ball = athlete.heldBall;
			ball.looseInFieldTile = tile;
			tile.ballLooseInTile = ball;

			athlete.heldBall = null;
			ball.heldByAthlete = null;

			if(matchManager.matchFieldParent.activeSelf == true) {
				PlaceBallOnTile(ball, tile);
			}
		}
	}

	public void SimulateMatch() {
		Debug.Log("Simulate Match");

		homeScore = Random.Range (0, 8);
		awayScore = Random.Range (0, 7);

		ConcludeMatch ();
	}

	public void StartMatch() {
		Debug.Log ("Starting Match");
		matchStarted = true;

		athletesOnField = new List<Athlete> ();

		if (homeTeam.teamManager.currentFieldTile != null) {
			athletesOnField.Add (homeTeam.teamManager);
		}
		if (awayTeam.teamManager.currentFieldTile != null) {
			athletesOnField.Add (awayTeam.teamManager);
		}

		for (int i = 0; i < homeTeam.rosterList.Count; i++) {
			if (homeTeam.rosterList [i].currentFieldTile != null) {
				athletesOnField.Add (homeTeam.rosterList [i]);
			}
		}
		for (int i = 0; i < awayTeam.rosterList.Count; i++) {
			if (awayTeam.rosterList [i].currentFieldTile != null) {
				athletesOnField.Add (awayTeam.rosterList [i]);
			}
		}

		DetermineInitiativeOrder ();

		if (matchManager.matchFieldParent.activeSelf == true) {
			matchManager.StartCoroutine(matchManager.AnimateStartedMatchUI ());
		} else {
			SetNextTurn();
		}
	}

	public void DetermineInitiativeOrder() {
		List<Athlete> athleteSpeedOrder = new List<Athlete> ();

		for(int a = athletesOnField.Count - 1; a >= 0; a--) {
			Athlete speediestAthlete = null;
			int speediestSpeed = 0;

			for (int i = 0; i < athletesOnField.Count; i++) {
				if (athletesOnField [i].GetAttributeValue("Speed") > speediestSpeed) {
					speediestAthlete = athletesOnField [i];
					speediestSpeed = speediestAthlete.GetAttributeValue ("Speed");
				} else if (athletesOnField [i].GetAttributeValue("Speed") == speediestSpeed) {
					//Check for reflexes
					Debug.Log("Same speed, checking reflexes");
					if (athletesOnField [i].GetAttributeValue ("Reflex") > speediestAthlete.GetAttributeValue ("Reflex")) {
						speediestAthlete = athletesOnField [i];
						speediestSpeed = speediestAthlete.GetAttributeValue ("Speed");
					}
				}
			}

			athleteSpeedOrder.Add (speediestAthlete);
			athletesOnField.Remove (speediestAthlete);
		}

		athletesOnField = athleteSpeedOrder;

		for (int i = 0; i < athletesOnField.Count; i++) {
			Debug.Log (athletesOnField [i].name + " speed " + athletesOnField [i].attributeList [0].value);
		}
	}

	public void SetNextTurn() { //Grabs the first athlete that isn't performing an action and sets them as the athleteWithTurn
		//Debug.Log("Setting next turn");
		MatchManager.selectedOpportunityButton = null;

		athleteWithTurn = null;
		for (int i = 0; i < athletesOnField.Count; i++) {
			if (athletesOnField[i].activeAction == null || athletesOnField [i].activeAction.timeUnitsLeft <= 0) { //If they haven't chosen their first action or their cooldown has expired
				athletesOnField[i].activeAction = null;
				athleteWithTurn = athletesOnField [i];
				break;
			}
		}

		if (athleteWithTurn == null) { //All athletes are still on cooldown
			if (matchManager.matchFieldParent.activeSelf == true) {
				matchManager.StartCoroutine (matchManager.WaitThenAdvanceTime ());
			} else {
				AdvanceTimeUnit ();
			}
		} else {
			AssignAvailableOpportunities ();

			if (matchManager.matchFieldParent.activeSelf == true) {
				matchManager.DisplayNewMatchTurn (this);
			} else { //AI move
				//THIS DOESN'T EVEN DO ANYTHING RIGHT NOW YA STUPID
				Debug.Log("5 bucks say the game just stopped working.");
				//GetAIAction(athleteWithTurn);
			}
		}
	}

	public void AssignAvailableOpportunities() {
		Athlete a = athleteWithTurn;
		a.availableOpportunityList = new List<Opportunity> ();

		if (a.heldBall == null) { //If the athlete doesn't have a ball.
			Opportunity moveOpp = GetOpportunity ("move");
			moveOpp.possibleActions = GetActionsFromOpportunity(a, moveOpp);
			a.availableOpportunityList.Add(moveOpp);
		} else { //If the athlete does have a ball.
			Opportunity dribOpp = GetOpportunity("dribble");
			dribOpp.possibleActions = GetActionsFromOpportunity(a, dribOpp);
			a.availableOpportunityList.Add(dribOpp);

			Opportunity kickOpp = GetOpportunity("kick");
			kickOpp.possibleActions = GetActionsFromOpportunity(a, kickOpp);
			a.availableOpportunityList.Add(kickOpp);
		}
	}

	public List<Action> GetActionsFromOpportunity(Athlete athlete, Opportunity opp) {
		List<Action> newActions = new List<Action>();

		List<FieldTile> possibleTiles = new List<FieldTile>();
		for(int i = 0; i < athlete.currentFieldTile.neighborList.Count; i++) {
			possibleTiles.Add(athlete.currentFieldTile.neighborList[i]);

			if(opp.tileRange > 1) {
				for(int j = 0; j < athlete.currentFieldTile.neighborList[i].neighborList.Count; j++) {
					FieldTile tile = athlete.currentFieldTile.neighborList[i].neighborList[j];
					if(tile != athlete.currentFieldTile && !possibleTiles.Contains(tile)) {
						possibleTiles.Add(tile);
					}
				}
			}
		}

		for(int i = 0; i < possibleTiles.Count; i++) {
			Action newAction = new Action (athlete, opp, possibleTiles[i]);
			newAction.timeUnitsLeft = GetActionTime(newAction);
			newAction.chanceSuccess = GetActionChance(newAction);
			newActions.Add(newAction);
		}

		return newActions;
	}

	public Opportunity GetOpportunity(string oppName) {
		for (int i = 0; i < basicOpportunityList.Count; i++) {
			if (basicOpportunityList [i].id.ToLower() == oppName.ToLower()) {
				return basicOpportunityList [i];
			}
		}
		return null;
	}

	public int GetActionTime(Action action) {
		Athlete athlete = action.athlete;
		int time = 0;
		switch(action.opportunity.id) {
			case "move":
				time = GetOpportunity ("move").baseTimeTaken;
				float moveModifier = 1;
				moveModifier += athlete.GetAttributeValue ("speed") / 10; //-1 time per 10 points of speed
				moveModifier--;
				time -= (int)moveModifier;
				break;
			case "dribble":
				time = GetOpportunity("dribble").baseTimeTaken;
				float dribbleModifier = 1f;
				dribbleModifier += athlete.GetAttributeValue ("speed") / 10; 
				dribbleModifier--;
				time -= (int)dribbleModifier;
				break;
			case "kick":
				time = GetOpportunity("kick").baseTimeTaken;
				float kickModifier = 1f;
				kickModifier += athlete.GetAttributeValue("strength") / 20;
				kickModifier--;
				time -= (int)kickModifier;
				break;
			case "wait":
				time = GetOpportunity("wait").baseTimeTaken;
				break;
			default:
				Debug.Log("No action means no time. And you all out of actions so you all out of time.");
				break;
		}

		return time;
	}

	public int GetActionChance(Action action) {
		Athlete athlete = action.athlete;
		Athlete defender = action.fieldTile.athleteOnTile;
		int chance = 0;
		switch(action.opportunity.id) {
			case "move":
				chance = GetOpportunity("move").basePercentChanceSuccess;

				if(defender != null) {
					float invaderScore = athlete.GetAttributeValue("Strength");
					float defenderScore = defender.GetAttributeValue ("Strength");
					defenderScore += defender.GetAttributeValue("Defense");
					invaderScore = invaderScore / 3;
					defenderScore = defenderScore / 3;

					float difference = defenderScore - invaderScore;
					int percentSuccessForInvader = 50 - (int)difference;

					chance = (chance/100) * percentSuccessForInvader;
				}
				break;
			case "dribble":
				chance = GetOpportunity("dribble").basePercentChanceSuccess;
				float dribChanceModifier = 0f;
				dribChanceModifier += athlete.GetAttributeValue ("Ball Control") / 5; //+1 percent per 5 points of ball control
				chance += (int)dribChanceModifier;

				if(defender != null) {
					float invaderScore = athlete.GetAttributeValue("Strength");
					float defenderScore = defender.GetAttributeValue ("Strength");
					defenderScore += defender.GetAttributeValue("Defense");
					//Add chance to lose the ball as well depending on invader's Ball Control

					invaderScore = invaderScore / 3;
					defenderScore = defenderScore / 3;
					float difference = defenderScore - invaderScore;
					int percentSuccessForInvader = 50 - (int)difference;

					chance = (chance/100) * percentSuccessForInvader;
				}
				break;
			case "kick":
				float kickChanceModifier = 0f;
				kickChanceModifier += athlete.GetAttributeValue("Ball Control") / 10; //+1 per 10 points
				chance = GetOpportunity("kick").basePercentChanceSuccess + (int)kickChanceModifier;
				break;
			default:
				Debug.Log("You no have ding. Add ding pls.");
				break;
		}

		return chance;
	}

	public Opportunity GetAIOpportunity(Athlete athlete) {
		Opportunity chosenOpportunity = athlete.availableOpportunityList[Random.Range(0, athlete.availableOpportunityList.Count)];

		return chosenOpportunity;
	}

	public Action GetAIAction(Athlete athlete, Opportunity opp) {
		Action chosenAction = null;
		List<Action> priorityActions = new List<Action>();
		List<Action> neutralActions = new List<Action>();
		List<Action> avoidanceActions = new List<Action>();

		for(int i = 0; i < opp.possibleActions.Count; i++) {
			FieldTile tile = opp.possibleActions[i].fieldTile;
			if(tile.isGoal) {
				if((tile.homeSide && athlete.GetTeam() == awayTeam) || (!tile.homeSide && athlete.GetTeam() == homeTeam)) {
					priorityActions.Add(opp.possibleActions[i]);
				} else {
					avoidanceActions.Add(opp.possibleActions[i]);
				}
			} else {
				neutralActions.Add(opp.possibleActions[i]);
			}
		}



		if(priorityActions.Count > 0) {
			chosenAction = priorityActions[Random.Range(0, priorityActions.Count)];
		} else if(neutralActions.Count > 0){
			chosenAction = neutralActions[Random.Range(0, neutralActions.Count)];
		} else {
			chosenAction = opp.possibleActions[Random.Range(0, opp.possibleActions.Count)]; //Completely random
		}

		return chosenAction;
	}

	public void AdvanceTimeUnit() {
		timeUnitsLeft--;

		for (int i = 0; i < athletesOnField.Count; i++) {
			Athlete athlete = athletesOnField [i];
			athlete.activeAction.timeUnitsLeft--;
		}

		if(matchManager.matchFieldParent.activeSelf == true) {
			matchManager.UpdateTimer();
		}

		if(timeUnitsLeft >= 0) {
			SetNextTurn ();
		} else {
			ConcludeMatch();
		}
	}
		
	public void BeginAction(Athlete athlete, Action action, FieldTile tile) {
		//Debug.Log("Started action : " + action.opportunity.id);

		athlete.activeAction = action;
		action.fieldTile = tile;
		if(athlete.heldBall != null) {
			action.ballInvolved = athlete.heldBall;
		}

		AttemptAction(action);
	}

	public void AttemptAction(Action action) {
		//Debug.Log("Attempting Action");

		action.success = false;
		//I don't think I need this since it's already set before the action is attempted. But I guess it can't hurt.
		action.chanceSuccess = GetActionChance(action);
		//For realsies should delete I guess maybe?

		action.randomResult = Random.Range (0, 101);
		if (action.randomResult <= action.chanceSuccess) {
			action.success = true;
		} else {
			action.success = false;
			Debug.Log ("FAILED: " + action.opportunity.name);
		}

		switch (action.opportunity.id.ToLower()) {
		//Without Ball
		case "move":
			if (action.fieldTile.athleteOnTile != null) { //If there is an athlete in the next space
				BeginClash(action);
			} else {
				CompleteMovementAction (action);
			}
			break;


		//With Ball
		case "dribble":
			if(action.fieldTile.athleteOnTile != null) {
				BeginClash(action);
			} else {
				CompleteDribbleAction (action);
			}
			break;
		
		case "kick":
			List<Athlete> potentialInterceptors = GetAthletesBetweenTiles(action.athlete.currentFieldTile, action.fieldTile);

			//THIS SHIT NEEDS SO MUCH WORK BOI

			Debug.Log("Broke this kick right here dog");

			bool potentialForInterception = false;
			if(potentialInterceptors.Count > 0) {
				Debug.Log("POTENTIAL INTERCEPTION");
				potentialForInterception = true;

				for(int i = 0; i < potentialInterceptors.Count; i++) {
					AttemptInterception(action, potentialInterceptors[i]);
				}
			} else {
				Debug.Log("Don't worry there ain't nobody there.");
			}

			if(potentialForInterception == false) {
				CompleteKickAction(action);
			} else {
				//Wait for interception animations??
			}
			break;


		//With or Without Ball
		/*
		case "wait":
			Debug.Log("You'll never see this.");
			CompleteWaitAction (action);
			break;
		*/

		default:
			Debug.Log ("That action does not exist in any way, shape, or form.");
			break;
		}

		//action.athlete.activeAction = null;
		//actionExecutionOrder.Remove (action);
	}

	public void BeginClash(Action action) {
		if(matchManager.matchFieldParent.activeSelf == true) {
			matchManager.StartCoroutine(matchManager.AnimateClash(action));
		} else {
			CompleteClash(action);
		}
	}

	public void CompleteClash(Action action) {
		FieldTile tile = action.fieldTile;
		Athlete invader = action.athlete;
		Athlete defender = tile.athleteOnTile;

		action.success = false;
		action.randomResult = Random.Range(0, 101);
		if(action.randomResult <= action.chanceSuccess) {
			action.success = true;
		} else {
			action.success = false;
		}

		if(action.success) {
			FieldTile chosenTile = null;

			List<FieldTile> availableNeighbors = new List<FieldTile>();
			for(int i = 0; i < tile.neighborList.Count; i++) {
				if(tile.neighborList[i].athleteOnTile == null) {
					availableNeighbors.Add(tile.neighborList[i]);
				}
			}

			if(invader.currentFieldTile.gridY > tile.gridY && tile.gridY != 0) {
				FieldTile downTile = fieldGrid[tile.gridX][tile.gridY - 1];
				if(downTile.athleteOnTile == null) {
					chosenTile = downTile;
				}
			} else if(invader.currentFieldTile.gridY < tile.gridY && tile.gridY != fieldGrid[tile.gridX].Count - 1) {
				FieldTile upTile = fieldGrid[tile.gridX][tile.gridY + 1];
				if(upTile.athleteOnTile == null) {
					chosenTile = upTile;
				}
			}

			if(invader.currentFieldTile.gridX > tile.gridX && tile.gridX != 0) {
				FieldTile leftTile = fieldGrid[tile.gridX - 1][tile.gridY];
				if(leftTile.athleteOnTile == null) {
					chosenTile = leftTile;
				}
			} else if(invader.currentFieldTile.gridX < tile.gridX && tile.gridX != fieldGrid.Count - 1) {
				FieldTile rightTile = fieldGrid[tile.gridX + 1][tile.gridY];
				if(rightTile.athleteOnTile == null) {
					chosenTile = rightTile;
				}
			}

			if(chosenTile == null) {
				if(availableNeighbors.Count > 0) {
					chosenTile = availableNeighbors[Random.Range(0, availableNeighbors.Count)];
				} else {
					chosenTile = invader.currentFieldTile;
				}
			}

			if(defender.heldBall != null) {
				GiveBallToAthlete(defender.heldBall, invader);
			}

			MoveAthleteToTile(defender, chosenTile);
			MoveAthleteToTile(invader, tile);

			/*
			if(defender.activeAction != null) {
				if(actionExecutionOrder.Contains(defender.activeAction)) {
					Debug.Log("The action is in the action execution ORDER");
					actionExecutionOrder.Remove(defender.activeAction);	
				}

				defender.activeAction = null;
			}
			*/
		} else {
			//Leave both athletes in their current tile.
			if(invader.heldBall != null) {
				GiveBallToAthlete(invader.heldBall, defender);
			}
		}

		if(matchManager.matchFieldParent.activeSelf == true) {
			matchManager.StartCoroutine(matchManager.AnimateClashCompletion(action, defender));
		} else {
			ConcludeAction(action);
		}
	}

	//Could technically combine these two 
	public void CompleteMovementAction(Action action) {
		Athlete athlete = action.athlete;

		if (action.success) {
			MoveAthleteToTile(athlete, action.fieldTile);
		} else {
			Debug.Log ("Failed. I guess you stay where you're at.");
		}

		if (matchManager.matchFieldParent.activeSelf == true) { 
			matchManager.StartCoroutine (matchManager.AnimateMovementAction (action));
		} else {
			ConcludeAction (action);
		}
	}

	public void CompleteDribbleAction(Action action) {
		Athlete athlete = action.athlete;

		if (action.success) {
			MoveAthleteToTile(athlete, action.fieldTile);
		} else {
			Debug.Log ("Failed. I guess you fucked up your dribble");
		}

		if (matchManager.matchFieldParent.activeSelf == true) { 
			matchManager.StartCoroutine (matchManager.AnimateDribbleAction (action));
		} else {
			ConcludeAction (action);
		}
	}

	public void AttemptInterception(Action action, Athlete interceptor) {

	}

	public void CompleteKickAction(Action action) {
		Athlete athlete = action.athlete;

		if(action.success) {
			MoveBallToTile(athlete.heldBall, action.fieldTile);
		} else {
			Debug.Log("The ball stays where it is or goes to an adjancent tile, cuz you kicked it bad. Cuz you trash.");
		}

		if (matchManager.matchFieldParent.activeSelf == true) { 
			matchManager.StartCoroutine (matchManager.AnimateKickAction (action));
		} else {
			ConcludeAction (action);
		}
	}

/*
	public void CompleteWaitAction(Action action, bool success) {
		Debug.Log("Completing wait");
		//Always success

		//Should there be any type of animation? Prolly not.

		ConcludeAction(action);
	}
*/

	public void MoveAthleteToTile(Athlete athlete, FieldTile newTile) {
		FieldTile oldTile = athlete.currentFieldTile;

		oldTile.athleteOnTile = null;
		newTile.athleteOnTile = athlete;
		athlete.currentFieldTile = newTile;

		if(newTile.ballLooseInTile != null) { //If there's a ball loose on the tile
			Debug.Log("loose ball");
			if(athlete.heldBall == null) { //If the athlete doesn't have a ball, pick it up
				Ball looseBall = newTile.ballLooseInTile;
				athlete.heldBall = looseBall;
				looseBall.heldByAthlete = athlete;
				looseBall.looseInFieldTile = null;
				newTile.ballLooseInTile = null;
			}
		}

		if(athlete.heldBall != null && newTile.isGoal) {
			athlete.heldBall.scored = true;
		}
	}

	public void MoveBallToTile(Ball ball, FieldTile newTile) {
		if(ball.heldByAthlete != null) {
			ball.heldByAthlete.heldBall = null;
			ball.heldByAthlete = null;
		}
		if(ball.looseInFieldTile != null) {
			 ball.looseInFieldTile.ballLooseInTile = null;
			 ball.looseInFieldTile = null;
		}

		if(newTile.athleteOnTile != null) {
			ball.heldByAthlete = newTile.athleteOnTile;
			newTile.athleteOnTile.heldBall = ball;
		} else {
			ball.looseInFieldTile = newTile;
			newTile.ballLooseInTile = ball;
		}

		if(newTile.isGoal) {
			ball.scored = true;
		}
	}

	public void GiveBallToAthlete(Ball ball, Athlete athlete) {
		if(ball.heldByAthlete != null) {
			ball.heldByAthlete.heldBall = null;
		}

		if(ball.looseInFieldTile != null) {
			ball.looseInFieldTile.ballLooseInTile = null;
			ball.looseInFieldTile = null;
		}

		ball.heldByAthlete = athlete;
		athlete.heldBall = ball;

		/*
		if(matchManager.matchFieldParent.activeSelf == true) {
			matchManager.SetBallParentAthlete(ball, ball.heldByAthlete);
		}
		*/
	}

	public List<Athlete> GetAthletesBetweenTiles(FieldTile start, FieldTile destination) {
		List<Athlete> defenderList = new List<Athlete>();
		List<FieldTile> tilesInPath = new List<FieldTile>();

		int xDistance = destination.gridX - start.gridX;
		int yDistance = destination.gridY - start.gridY;

		//This entire function only works when the max range is = 2. Needs to be modified to allow for longer paths
		if(Mathf.Abs(xDistance) > Mathf.Abs(yDistance)) {
			//This code is so terrible bro
			tilesInPath.Add(fieldGrid[start.gridX + (xDistance/2)][start.gridY]);
		} else if(Mathf.Abs(xDistance) == Mathf.Abs(yDistance)) {
			//C'mon man
			tilesInPath.Add(fieldGrid[start.gridX + xDistance][start.gridY]);
			tilesInPath.Add(fieldGrid[start.gridX][start.gridY + yDistance]);
		} else {
			//Gross
			tilesInPath.Add(fieldGrid[start.gridX][start.gridY + (yDistance/2)]);
		}

		for(int i = 0; i < tilesInPath.Count; i++){
			Athlete potentialDefender = tilesInPath[i].athleteOnTile; 
			if(potentialDefender != null && potentialDefender.GetTeam() != start.athleteOnTile.GetTeam()) {
				defenderList.Add(potentialDefender);
			}
		}

		return defenderList;
	} 
	

	public void ScoreGoal(Action action, Ball ball) {
		Debug.Log("GOAL SCORED");
		ball.scored = false;
		FieldTile tile = action.fieldTile;

		if(tile.homeSide) {
			awayScore++;
		} else {
			homeScore++;
		}
		
		if(matchManager.matchFieldParent.activeSelf == true) {
			matchManager.StartCoroutine(matchManager.AnimateGoal(action, ball));
		} else {
			CompleteGoalScoring(ball, action.fieldTile);
		}
	}

	public void CompleteGoalScoring(Ball ball, FieldTile tileScoredOn) {
			if(ball.heldByAthlete != null) {
				ball.heldByAthlete.heldBall = null;
				ball.heldByAthlete = null;
			}
			if(ball.looseInFieldTile != null) {
				ball.looseInFieldTile.ballLooseInTile = null;
				ball.looseInFieldTile = null;
			}

			TeamController teamScoredOn;
			if(tileScoredOn.homeSide) {
				teamScoredOn = homeTeam;
			} else {
				teamScoredOn = awayTeam;
			}

			ReturnAthletesToOriginalTiles();

			SetBallStart(teamScoredOn);

			if(matchManager.matchFieldParent.activeSelf == true) {
				matchManager.StartCoroutine(matchManager.AnimateAthletesReturningToOriginalPositions(teamScoredOn));
			} else {
				SetNextTurn();
			}
	}

	public void ReturnAthletesToOriginalTiles() {
		for(int i = 0; i < athletesOnField.Count; i++) {
			if(athletesOnField[i].activeAction != null) {
				//actionExecutionOrder.Remove(athletesOnField[i].activeAction);
				athletesOnField[i].activeAction = null;
			}
			MoveAthleteToTile(athletesOnField[i], athletesOnField[i].originalLineupTile);
		}
	}


	public void ConcludeAction(Action action) {
		//Debug.Log("Concluding Action");
		
		Ball scoredBall = null;
		for(int i = 0; i < ballList.Count; i++) {
			if(ballList[i].scored) {
				scoredBall = ballList[i];
			}
		}

		if(timeUnitsLeft > 0) {
			if(scoredBall == null) {
				SetNextTurn();
			} else {
				ScoreGoal(action, scoredBall);
			}
		} else {
			Debug.Log("THIS SHOULD NEVER BE CALLED BECAUSE THE GAME SHOULD BE OVER");
			ConcludeMatch();
		}
	}

	//Right now this just gets the fastest athlete, not the closest.
	/*
	public Athlete GetClosestAthlete(bool home, FieldTile tile) {
		TeamController checkedTeam;
		if (home) {
			checkedTeam = homeTeam;
		} else {
			checkedTeam = awayTeam;
		}

		for (int i = 0; i < athletesOnField.Count; i++) {
			if (athletesOnField [i].GetTeam () == checkedTeam) {
				return athletesOnField [i];
			}
		}

		Debug.Log ("Major failure, major.");
		return null;
	}

	public void MoveBallToTile(Ball ball, FieldTile newTile) {
		if (ball.heldByAthlete != null) {
			ball.heldByAthlete.heldBall = null;
			ball.heldByAthlete = null;
		}

		if (ball.looseInFieldTile != null) {
			ball.looseInFieldTile.ballsLooseInTile.Remove (ball);
			ball.looseInFieldTile = null;
		}

		//Check to see if there are athletes in the new tile that can grab the ball
		ball.looseInFieldTile = newTile;
		newTile.ballsLooseInTile.Add (ball);
	}
	*/

	/*
	public void LoseBall(Action action) {
		int randoDirection = Random.Range (0, 4);
		FieldTile tileTested;
		FieldTile tileLostTo;
		if (action.fieldTile != null) {
			tileTested = action.fieldTile;
		} else {
			tileTested = action.athlete.currentFieldTile;
		}

		if (randoDirection < action.fieldTile.neighborList.Count) {
			tileLostTo = action.fieldTile.neighborList [randoDirection];
		} else { //It goes out of bounds or into the goal
			Debug.Log("Lost the ball out of bounds (or into the goal).");
		}
	}
	*/

	//public void GiveBallToRandomNearbyOpponent

	/*
	public void BeginNextPlay() {
		playNumber++;
		Play play = playOrder [playNumber];
		TeamController teamWithOpportunity = play.athlete.GetTeam ();

		if (!secondHalf && changesInPossession >= possessionsPerHalf) { //Halftime/GameOver condition
			//Debug.Log("Halftime");
			playNumber--;
			changesInPossession = 0;
			secondHalf = true;

			//Need to reset athlete positions
			for (int i = 0; i < activeHomeAthletes.Count; i++) { //Assumes activeHome.Count == activeAway.Count
				MoveAthleteToPosition(activeHomeAthletes[i], 0);
				MoveAthleteToPosition (activeAwayAthletes [i], fieldPositions.Count - 1);
			}

			if (gameController.matchUI.activeSelf == true) {
				AddAndUpdateMatchFeed (new MatchText (GenerateHalftimeString (), Color.black, true));

				gameController.matchInteractionButton.gameObject.SetActive (true);
				gameController.matchInteractionButton.onClick.RemoveAllListeners ();
				gameController.matchInteractionButton.onClick.AddListener (() => StartSecondHalf ());
				gameController.matchInteractionButton.GetComponentInChildren<Text> ().text = "Continue";
			} else {
				StartSecondHalf ();
			}

		} else if (secondHalf && changesInPossession >= possessionsPerHalf) {
			Debug.Log ("Concluding Match");

			ConcludeMatch ();
		} else {
			AssignAvailableOpportunities (play.athlete, play);

			//Clears the athlete's defensive status.
			play.athlete.defending = false;
			play.athlete.goalkeeping = false;
			play.athlete.stealing = false;

			if (gameController.matchUI.activeSelf == true) { //If the match panel is active then set the playHighlight to the current athlete's position
				gameController.playHighlight.SetActive (true);
				gameController.playHighlight.transform.position = play.athlete.athUI.transform.position;

				if (gameController.matchUI.activeSelf == true) {
					Vector3 indicatorOffset = new Vector3 (90, 40, 0);
					gameController.fieldPlayIndicatorObj.transform.SetParent (play.athlete.fieldUI.transform);
					gameController.fieldPlayIndicatorObj.transform.localPosition = Vector3.zero + indicatorOffset;

					if (play.athlete.GetTeam () == homeTeam) {
						gameController.fieldPlayIndicatorObj.transform.eulerAngles = new Vector3 (0, 0, 0);
					} else {
						gameController.fieldPlayIndicatorObj.transform.eulerAngles = new Vector3 (0, 180, 0);
					}
				}

				if (play.withBall) {
					gameController.ballIndicatorObj.transform.SetParent (play.athlete.fieldUI.transform);
					gameController.ballIndicatorObj.transform.localPosition = Vector3.zero;
				}
			}

			if (teamWithOpportunity == GameController.playerTeam) { //If it's the player's team, activate the opportunity panel
				gameController.opportunityPanel.SetActive(true);

				for (int i = 0; i < play.athlete.availableOpportunities.Count; i++) {
					OpportunityUIButton opporUI = gameController.opportunityButtons [i].GetComponent<OpportunityUIButton> ();
					opporUI.opportunityText.text = play.athlete.availableOpportunities [i].oppName;
					opporUI.opportunityDescriptionText.text = play.athlete.availableOpportunities [i].description;
					opporUI.percentageText.text = GetAthleteChanceForOpportunity (play.athlete, play.athlete.availableOpportunities [i]) + "%";

					int oppNum = i; //This needs to be assigned because of how AddListener works in Unity
					gameController.opportunityButtons [i].onClick.AddListener (() => gameController.StartWaitBeforeNextAttempt (this, play.athlete,
						play.athlete.availableOpportunities [oppNum]));

					if (play.athlete.GetTeam () == GameController.playerTeam) {
						if (play.athlete.availableOpportunities [i].name == "shoot") {
							//If they want the shot
							opporUI.opportunityQuote = GetAthleteWantShootQuote ();
							//else if they don't want the shot
						} else if (play.athlete.availableOpportunities [i].name == "heave") {
							opporUI.opportunityQuote = GetAthleteAvoidHeaveQuote ();
						} else if (play.athlete.availableOpportunities [i].name == "dunk") {
							opporUI.opportunityQuote = GetAthleteWantDunkQuote ();
						} else if (play.athlete.availableOpportunities [i].name == "drive") {
							opporUI.opportunityQuote = GetAthleteWantDriveQuote ();
						} else if (play.athlete.availableOpportunities [i].name == "pass") {
							opporUI.opportunityQuote = GetAthleteWantPassQuote ();
						} else if (play.athlete.availableOpportunities [i].name == "through") {
							opporUI.opportunityQuote = GetAthleteWantThroughQuote ();
						} else if (play.athlete.availableOpportunities [i].name == "juke") {
							opporUI.opportunityQuote = GetAthleteWantJukeQuote ();
						} else if (play.athlete.availableOpportunities [i].name == "defense") {
							opporUI.opportunityQuote = GetAthleteWantDefenseQuote ();
						} else if (play.athlete.availableOpportunities [i].name == "protect") {
							opporUI.opportunityQuote = GetAthleteWantProtectNetQuote ();
						}
					}
				}

				for (int q = play.athlete.availableOpportunities.Count; q < gameController.opportunityButtons.Length; q++) {
					gameController.opportunityButtons [q].gameObject.SetActive (false);
				}

			} else {
				//The opportunity that the AI selects is currently completely random.
				gameController.StartWaitBeforeNextAttempt (this, play.athlete, play.athlete.availableOpportunities [Random.Range (0, play.athlete.availableOpportunities.Count)]);
			}
		}
	}

	public void StartSecondHalf() {
		if (gameController.matchUI.activeSelf == true) {
			gameController.DestroyMatchNotifications ();

			gameController.matchInteractionButton.gameObject.SetActive (false);

			gameController.homeNotificationText.gameObject.SetActive (false);
			gameController.awayNotificationText.gameObject.SetActive (false);
		}

		BeginNextPlay ();
	}
	*/

	/*
	public void AssignAvailableOpportunities(Athlete athlete, Play play) {
		athlete.availableOpportunities = new List<Opportunity> ();

		int athleteRelativePosition;
		if (athlete.GetTeam () == homeTeam) {
			athleteRelativePosition = athlete.fieldPosition;
		} else { //Else they're away
			athleteRelativePosition = fieldPositions.Count - 1 - athlete.fieldPosition;
		}

		bool teammateInPosition = false;
		if (GetTeammates (fieldPositions[athlete.fieldPosition].athletesInPosition, athlete).Count > 0) {
			teammateInPosition = true;
		}

		bool teammateAhead = false;
		if (athlete.GetTeam () == homeTeam) {
			if (athlete.fieldPosition < 3 && GetTeammates (fieldPositions [athlete.fieldPosition + 1].athletesInPosition, athlete).Count > 0) {
				teammateAhead = true;
			}
		} else {
			if (athlete.fieldPosition > 0 && GetTeammates (fieldPositions [athlete.fieldPosition - 1].athletesInPosition, athlete).Count > 0) {
				teammateAhead = true;
			}
		}

		bool opponentInPosition = false;
		if (GetOpponents (fieldPositions[athlete.fieldPosition].athletesInPosition, athlete).Count > 0) {
			opponentInPosition = true;
		}

		if (play.withBall == true) { //If this athlete has the ball for this play
			if (athleteRelativePosition == 0) {
				athlete.availableOpportunities.Add (GetOpportunity ("heave"));
				athlete.availableOpportunities.Add (GetOpportunity ("drive"));
			} else if (athleteRelativePosition == 1) {
				athlete.availableOpportunities.Add (GetOpportunity ("heave"));
				athlete.availableOpportunities.Add (GetOpportunity ("drive"));
			} else if (athleteRelativePosition == 2) {
				athlete.availableOpportunities.Add (GetOpportunity ("shoot"));
				athlete.availableOpportunities.Add (GetOpportunity ("drive"));
			} else if (athleteRelativePosition == 3) {
				athlete.availableOpportunities.Add (GetOpportunity ("shoot"));

			} else {
				Debug.Log (athlete.name + " is in a non-existant field position.");
			}

			if (opponentInPosition && athleteRelativePosition != 3) { //Opponent can't be in the athlete's offensive goalzone because then they'd fly off the map.
				athlete.availableOpportunities.Add (GetOpportunity ("juke"));
			}
			if (teammateInPosition) {
				athlete.availableOpportunities.Add (GetOpportunity ("pass"));
			}
			if (teammateAhead) {
				athlete.availableOpportunities.Add (GetOpportunity ("through"));
			}

		} else { //If they don't have the ball.
			if (athleteRelativePosition== 0) {
				athlete.availableOpportunities.Add (GetOpportunity ("defense"));
				athlete.availableOpportunities.Add (GetOpportunity ("protect"));
			} else if (athleteRelativePosition == 1) {
				athlete.availableOpportunities.Add (GetOpportunity ("defense"));
			} else if (athleteRelativePosition == 2) {
				athlete.availableOpportunities.Add (GetOpportunity ("defense"));
			} else if (athleteRelativePosition == 3) {
				athlete.availableOpportunities.Add (GetOpportunity ("defense"));
			} else {
				Debug.Log ("You got somebody in a position with no balls and no options.");
			}

			if (GetOpponents(fieldPositions[athlete.fieldPosition].athletesInPosition, athlete).Count > 0) {
				athlete.availableOpportunities.Add (GetOpportunity ("steal"));
			}
		}
	}


	public int GetAthleteChanceForOpportunity(Athlete athlete, Opportunity opp) {
		int chance = opp.baseChance;
		int accumulatedStats = 0;

		for (int i = 0; i < opp.statsUsed.Count; i++) {
			if (opp.statsUsed [i] == "F") {
				accumulatedStats += athlete.force;
			} else if (opp.statsUsed [i] == "R") {
				accumulatedStats += athlete.resilience;
			} else if (opp.statsUsed [i] == "A") {
				accumulatedStats += athlete.agility;
			} else if (opp.statsUsed [i] == "T") {
				accumulatedStats += athlete.tactics;
			} else {
				Debug.Log("What stat is that? That ain't FRAT.");
			}
		}

		chance = chance + (((opp.maxChance - opp.baseChance) / (opp.statsUsed.Count * 10)) * accumulatedStats);

		return chance;

		return 0;
	}
	*/

	/*
	public void AttemptOpportunity(Athlete athlete, Opportunity opp) {
		string matchString = "";

		if (gameController.matchPanel.activeSelf == true) {
			gameController.homeNotificationText.gameObject.SetActive (false);
			gameController.awayNotificationText.gameObject.SetActive (false);

			gameController.DestroyMatchNotifications ();
		}

		Athlete successfulStealer = AttemptSteal (CheckForStealers (GetOpponents (fieldPositions [athlete.fieldPosition].athletesInPosition, athlete)), athlete);
		if(successfulStealer != null) {

			matchString = GenerateStealSuccessString (successfulStealer, athlete);

			Turnover (successfulStealer, athlete);
		} else { //The ball is not stolen and we can proceed normally.

			if (opp.name == "pass") {
				Athlete successfulDefender = AttemptDefense (CheckForDefenders (GetOpponents (fieldPositions [athlete.fieldPosition].athletesInPosition, athlete)), athlete);

				if (successfulDefender != null) { //Defense was successful
					matchString = GenerateInterceptSuccessString(successfulDefender, athlete);

					Turnover (successfulDefender, athlete);
				} else {
					List<Athlete> availableTeammates = GetTeammates (fieldPositions[athlete.fieldPosition].athletesInPosition, athlete);
					Athlete athletePassedTo = availableTeammates [Random.Range (0, availableTeammates.Count)]; 

					if (Roll(GetAthleteChanceForOpportunity(athlete, opp))) {
						matchString = GeneratePassSuccessString (athlete, athletePassedTo); 

						Pass (athlete, athletePassedTo);
					} else { //Out of bounds, possession awarded to the opposers
						matchString = GeneratePassOOBString (athlete, athletePassedTo);

						Turnover (null, athlete);
					}
				}

			} else if (opp.name == "through") {
				Athlete successfulDefender = AttemptDefense (CheckForDefenders (GetOpponents (fieldPositions [athlete.fieldPosition].athletesInPosition, athlete)), athlete);

				if (successfulDefender != null) { //Defense was successful
					matchString = GenerateInterceptSuccessString (successfulDefender, athlete);

					Turnover (successfulDefender, athlete);
				} else {
					List<Athlete> availableTeammates;
					if (athlete.GetTeam () == homeTeam) {
						availableTeammates = GetTeammates (fieldPositions [athlete.fieldPosition + 1].athletesInPosition, athlete);
					} else {
						availableTeammates = GetTeammates (fieldPositions [athlete.fieldPosition - 1].athletesInPosition, athlete);
					}
					Athlete athletePassedTo = availableTeammates [Random.Range (0, availableTeammates.Count)]; 

					if (Roll(GetAthleteChanceForOpportunity(athlete, opp))) {
						matchString = GenerateThroughSuccessString(athlete, athletePassedTo);

						Pass (athlete, athletePassedTo);
					} else {
						matchString = GenerateThroughFailString(athlete, athletePassedTo);

						Turnover (null, athlete);
					}
				}
			} else if (opp.name == "shoot") {
				List<Athlete> goalkeepingAttempts = CheckForKeepers (athlete.GetTeam().opponentThisWeek); //This should work
				List<Athlete> failedKeepers = new List<Athlete>();
				bool goalkept = false;

				for (int i = 0; i < goalkeepingAttempts.Count; i++) {
					Athlete keeper = goalkeepingAttempts [i];
					if (Roll(GetAthleteChanceForOpportunity(athlete, opp))) {
						goalkept = true;
						matchString = athlete.name + " took a shot but " + keeper.name + " the keeper snatched it out of the air.";

						Turnover (keeper, athlete);
						break;
					} else {
						failedKeepers.Add (keeper);
					}
				}

				if (!goalkept) {
					if (Roll(GetAthleteChanceForOpportunity(athlete, opp))) {
						if (failedKeepers.Count > 0) {
							if (failedKeepers.Count > 1) {
								matchString = "Goal! " + athlete.name + " sneaked the ball between " + failedKeepers [0].name + " and " + failedKeepers [1].name + "!";
							} else {
								matchString = athlete.name + " just blasted a shot past " + failedKeepers [0].name + "'s outstretched hands!";
							}
						} else {
							matchString = GenerateShootSuccessString (athlete);
						}

						Score (athlete);
					} else { //Missed the shot
						if (failedKeepers.Count > 0) {
							matchString = athlete.name + " hit the shot past the keeper but it bounced off the rim.";
						} else {
							matchString = GenerateShootFailString (athlete);
						}

						Miss (athlete);
					}
				}
			} else if (opp.name == "heave") {
				if (Roll(GetAthleteChanceForOpportunity(athlete, opp))) {
					matchString = GenerateHeaveSuccessString (athlete);

					Score (athlete);
				} else {
					matchString = GenerateHeaveFailString (athlete);

					Miss (athlete);
				}
			} else if (opp.name == "drive") {
				if (Roll(GetAthleteChanceForOpportunity(athlete, opp))) {
					matchString = GenerateDribbleSuccessString (athlete);

					if (athlete.GetTeam () == homeTeam) {
						MoveAthleteToPosition (athlete, athlete.fieldPosition + 1);
					} else {
						MoveAthleteToPosition (athlete, athlete.fieldPosition - 1);
					}

					InsertNextPlay (athlete, true);
				} else { //Lose the ball, change possession
					matchString = GenerateDribbleFailString (athlete);

					Turnover (null, athlete);
				}
			} else if (opp.name == "dunk") {
				if (Roll(GetAthleteChanceForOpportunity(athlete, opp))) {
					matchString = GenerateDunkSuccessString (athlete);

					Score (athlete);
				} else {
					matchString = GenerateDunkFailString (athlete);

					Turnover (null, athlete);
				}
			} else if (opp.name == "juke") {
				List<Athlete> opponentsInPosition = GetOpponents (fieldPositions [athlete.fieldPosition].athletesInPosition, athlete);
				Athlete opponentJuked = opponentsInPosition [Random.Range (0, opponentsInPosition.Count)];

				if (Roll(GetAthleteChanceForOpportunity(athlete, opp))) {
					matchString = athlete.name + " showcased some nasty moves and sent " + opponentJuked.name + " flying!";

					if (athlete.GetTeam () == homeTeam) {
						MoveAthleteToPosition (opponentJuked, opponentJuked.fieldPosition + 1);
					} else {
						MoveAthleteToPosition (opponentJuked, opponentJuked.fieldPosition - 1);
					}

					InsertNextPlay (athlete, true);
				} else {
					matchString = athlete.name + " made a fool of themself and gave the ball away to " + opponentJuked.name + ".";

					Turnover (opponentJuked, athlete);
				}
			} else if (opp.name == "defense") {
				if (Roll(GetAthleteChanceForOpportunity(athlete, opp))) {
					matchString = GenerateDefensiveStanceString (athlete);
					athlete.defending = true;
				} else {
					matchString = athlete.name + " tripped on a rough patch.";

					Turnover (null, athlete);
				}
			} else if (opp.name == "protect") {
				if (Roll(GetAthleteChanceForOpportunity(athlete, opp))) {
					matchString = athlete.name + " is ready to protect the net.";
					athlete.goalkeeping = true;
				} else {
					matchString = "Oh dear! " + athlete.name + " just collided with the rim!";

					Turnover (null, athlete);
				}
			} else if (opp.name == "steal") {
				if (Roll(GetAthleteChanceForOpportunity(athlete, opp))) {
					matchString = athlete.name + " is ready to intercept nearby passes.";
					athlete.stealing = true;
				} else {
					matchString = athlete.name + " took their eye off the play.";

					Turnover (null, athlete);
				}
			} else {
				Debug.Log ("That opportunity doesn't exist.");
			}
		}
			
		if (gameController.matchUI.activeSelf == true) { //If the match panel is being displayed
			AddAndUpdateMatchFeed (new MatchText(matchString, athlete.GetTeam ().teamColor)); //Could move this thang to each instance of matchString being assigned

			gameController.playHighlight.SetActive (false);
		}

		BeginNextPlay ();
	}

	//Could make this function take the relative field position to cut down on some lines
	public void MoveAthleteToPosition(Athlete athlete, int newFieldPosition) {
		fieldPositions [athlete.fieldPosition].athletesInPosition.Remove (athlete);

		athlete.fieldPosition = newFieldPosition;

		fieldPositions [athlete.fieldPosition].athletesInPosition.Add (athlete);

		if (gameController.matchUI.activeSelf == true) {
			athlete.fieldUI.transform.SetParent (fieldPositions [athlete.fieldPosition].fieldPositionObj.transform);
		}
	}

	public void AddAndUpdateMatchFeed(MatchText matchText) { //Could move this method to GameController
		matchStrings.Add (matchText);

		for (int t = 0; t < gameController.matchTexts.Length && t < matchStrings.Count; t++) {
			gameController.matchTexts [t].text = matchStrings [matchStrings.Count - 1 - t].matchString;
			gameController.matchTexts [t].color = matchStrings [matchStrings.Count - 1 - t].matchTextColor;
			if (matchStrings [matchStrings.Count - 1 - t].disabledOutline == true) {
				gameController.matchTexts [t].GetComponent<Outline> ().enabled = false;
			} else {
				gameController.matchTexts [t].GetComponent<Outline> ().enabled = true;
			}
		}
	}
	*/

	/*
	public List<Athlete> GetTeammates(List<Athlete> athletesBeingChecked, Athlete athlete) {
		List<Athlete> teammates = new List<Athlete> ();

		for (int i = 0; i < athletesBeingChecked.Count; i++) {
			if (athletesBeingChecked [i].GetTeam () == athlete.GetTeam () && athletesBeingChecked [i] != athlete) {
				teammates.Add (athletesBeingChecked [i]);
			}
		}

		return teammates;
	}

	public List<Athlete> GetOpponents(List<Athlete> athletesBeingChecked, Athlete athlete) { //Athlete in this case could be replaced by a TeamController, which would make more sense but isn't really a big deal.
		List<Athlete> opponents = new List<Athlete> ();

		for (int i = 0; i < athletesBeingChecked.Count; i++) {
			if (athletesBeingChecked [i].GetTeam () != athlete.GetTeam ()) {
				opponents.Add (athletesBeingChecked [i]);
			}
		}

		return opponents;
	}

	public List<Athlete> CheckForDefenders(List<Athlete> athletesBeingChecked) { //Could maybe universalize this for any boolean? I dunno bout that but it'd be slick
		List<Athlete> defenders = new List<Athlete> ();

		for (int i = 0; i < athletesBeingChecked.Count; i++) {
			if (athletesBeingChecked [i].defending == true) {
				defenders.Add (athletesBeingChecked [i]);
			}
		}

		return defenders;
	}

	public List<Athlete> CheckForStealers(List<Athlete> athletesBeingChecked) {
		List<Athlete> stealers = new List<Athlete> ();

		for (int i = 0; i < athletesBeingChecked.Count; i++) {
			if (athletesBeingChecked [i].stealing == true) {
				stealers.Add (athletesBeingChecked [i]);
			}
		}

		return stealers;
	}
		
	public List<Athlete> CheckForKeepers(TeamController teamBeingChecked) {
		List<Athlete> keepers = new List<Athlete> ();

		if (teamBeingChecked == homeTeam) {
			for (int i = 0; i < activeHomeAthletes.Count; i++) {
				if (activeHomeAthletes [i].goalkeeping == true) {
					keepers.Add (activeHomeAthletes [i]);
				}
			}
		} else if (teamBeingChecked == awayTeam) {
			for (int i = 0; i < activeAwayAthletes.Count; i++) {
				if (activeAwayAthletes [i].goalkeeping == true) {
					keepers.Add (activeAwayAthletes [i]);
				}
			}
		} else {
			Debug.Log ("Flosstown we got a boss down.");
		}

		return keepers;
	}
	*/


	//Convert this into a turnover function instead
	/*
	public Athlete GetFurthestOpponent(Athlete athlete) {
		List<Athlete> potentialNextAthletes = new List<Athlete> ();
		if (athlete.GetTeam () == homeTeam) {
			for (int i = fieldPositions.Count - 1; i >= 0; i--) {
				if (GetOpponents (fieldPositions [i].athletesInPosition, athlete).Count > 0) {
					potentialNextAthletes = GetOpponents (fieldPositions [i].athletesInPosition, athlete);
					break;
				}
			}
		} else {
			for (int i = 0; i < fieldPositions.Count; i++) {
				if (GetOpponents (fieldPositions [i].athletesInPosition, athlete).Count > 0) {
					potentialNextAthletes = GetOpponents (fieldPositions [i].athletesInPosition, athlete);
					break;
				}
			}
		}

		return potentialNextAthletes [Random.Range (0, potentialNextAthletes.Count)];
	}
	*/



	/*
	public Athlete AttemptDefense(List<Athlete> defenderList, Athlete target) {
		if (defenderList == null) {
			return null;
		}

		for (int i = 0; i < defenderList.Count; i++) {
			Athlete def = defenderList [i];
			if (Roll (GetOpportunity ("defense").baseChance)) { //If the defense roll is successful, intercept the pass
				//Debug.Log ("DEFENSED");

				return def;
			} else { //Else they fail to intercept. Could either include an extra match feed or leave unmentioned.
				//def failed
			}
		}

		return null;
	}

	public Athlete AttemptSteal(List<Athlete> stealerList, Athlete target) {
		if (stealerList == null) {
			return null;
		}

		for (int i = 0; i < stealerList.Count; i++) {
			Athlete stealer = stealerList [i];
			if (Roll (GetOpportunity ("steal").baseChance)) {
				//Debug.Log ("STOLEN");

				return stealer;
			} else { //The stealer fails.

			}
		}

		return null;
	}
	*/

	/*
	public void Pass(Athlete passer, Athlete receiver) {
		if (gameController.matchUI.activeSelf == true) {
			gameController.InstantiateMatchNotification (passer, "Pass");

			gameController.ballIndicatorObj.transform.SetParent (receiver.fieldUI.transform);
			gameController.ballIndicatorObj.transform.localPosition = Vector3.zero;
		}

		InsertNextPlay (receiver, true);
		InsertNextPlay (passer, false);
	}

	public void Score(Athlete scorer) {
		scorer.scores++;

		if (scorer.GetTeam () == homeTeam) {
			homeScore++;
			gameController.homeScoreText.text = homeScore.ToString ();
		} else if (scorer.GetTeam () == awayTeam) {
			awayScore++;
			gameController.awayScoreText.text = awayScore.ToString ();
		} else {
			Debug.Log ("This athlete don't belong to a team that's playing.");
		}

		if (gameController.matchUI.activeSelf == true) {
			gameController.InstantiateMatchNotification (scorer, "GOAL");
		}

		ChangePossession (GetFurthestOpponent (scorer));
	}

	//Could potentially combine miss/rebound into one function. Mustn't there always be a shot to be a rebound?

	public void Miss(Athlete misser) {
		misser.misses++;

		if (gameController.matchUI.activeSelf == true) {
			gameController.InstantiateMatchNotification (misser, "Miss");
		}

		Rebound (misser);
	}
		
	public void Rebound(Athlete shooter) {
		List<Athlete> reboundersInPosition = new List<Athlete> ();

		if (shooter.GetTeam () == homeTeam) {
			for (int i = 0; i < fieldPositions [fieldPositions.Count - 1].athletesInPosition.Count; i++) {
				reboundersInPosition.Add (fieldPositions [fieldPositions.Count - 1].athletesInPosition [i]);
			}
		} else if (shooter.GetTeam () == awayTeam) {
			for (int i = 0; i < fieldPositions [0].athletesInPosition.Count; i++) {
				reboundersInPosition.Add (fieldPositions [0].athletesInPosition [i]);
			}
		} else {
			Debug.Log ("WHO DAT TEAM?");
		}

		if (reboundersInPosition.Count == 0) {
			Turnover (null, shooter);

		} else {
			Athlete rebounder = reboundersInPosition [Random.Range (0, reboundersInPosition.Count)];

			rebounder.rebounds++;

			if (shooter.GetTeam () == rebounder.GetTeam ()) {
				InsertNextPlay (rebounder, true);

			} else {
				ChangePossession (rebounder);

			}

			if (gameController.matchUI.activeSelf == true) {
				gameController.InstantiateMatchNotification (rebounder, "Board");
			}
		}

	}

	public void Turnover(Athlete baller, Athlete loser) {
		loser.turnovers++;

		if (baller == null) {
			ChangePossession (GetFurthestOpponent (loser));
		} else {
			baller.steals++;

			ChangePossession (baller);
		}

		if (gameController.matchUI.activeSelf == true) {
			gameController.InstantiateMatchNotification (loser, "Turnover");
		}
	}

	public void ChangePossession(Athlete newbie) {
		changesInPossession++;

		if (!secondHalf) {
			gameController.possessionsLeftText.text = (possessionsPerHalf - changesInPossession).ToString ();
		} else {
			gameController.possessionsLeftText.text = (possessionsPerHalf - changesInPossession).ToString ();
		}

		InsertNextPlay (newbie, true);
	}

	public void InsertNextPlay(Athlete athlete, bool withBall) {
		Play nextPlay = new Play (athlete, withBall);
		playOrder.Insert (playNumber + 1, nextPlay); //Inserts the next play imediately after this play.
	}
	*/

	public void ConcludeMatch() {
		GameController.movementPaused = false;

		Debug.Log("Concluding match");

		if (homeScore >= awayScore) { //home wins (REMOVE EQUAL WHEN IMPLEMENTING TIES)
			winner = homeTeam;
			homeTeamMarker.sortingOrder = 2;
			homeTeam.winsThisSeason++;
			awayTeam.losesThisSeason++;

			homeTeam.gold += league.goldRewardPerWin;
			awayTeam.gold -= league.goldRewardPerWin;
			homeTeam.prestige += league.prestigeRewardPerWin;
		} else if (homeScore < awayScore) { //away wins
			winner = awayTeam;
			awayTeamMarker.sortingOrder = 2;
			awayTeam.winsThisSeason++;
			homeTeam.losesThisSeason++;

			awayTeam.gold += league.goldRewardPerWin;
			homeTeam.gold -= league.goldRewardPerWin;
			awayTeam.prestige += league.prestigeRewardPerWin;
		} else {
			//Overtime sudden DEATH where players can literally die
			Debug.Log("TIE");
		}
		if (matchManager.matchUIObject.activeSelf == true) {
			//matchManager.UndisplayMatchupUI (this);
			matchManager.DisplayConclusionPanel();
		}

		for(int i = 0; i < athletesOnField.Count; i++) {
			athletesOnField[i].activeAction = null;
			athletesOnField[i].currentFieldTile = null;
			athletesOnField[i].originalLineupTile = null;
			athletesOnField[i].heldBall = null;
		}

		if(GameController.week > -1) {
			bool allTeamsPlayed = true;
			for (int i = 0; i < homeTeam.league.weeklyListOfMatchupsForSeason [GameController.week].Count; i++) {
				if (homeTeam.league.weeklyListOfMatchupsForSeason [GameController.week] [i].winner == null) {
					allTeamsPlayed = false;
				}
			}
			if (allTeamsPlayed == true) {
				if (homeTeam.league == gameController.brimshireLeague) { //If this is the primary league's last game
					gameController.LastMatchCompleted ();
				}
			}
		}
	}






	//Match String Generation
	public string GenerateWelcomeString() {
		List<string> matchWelcomeStrings = new List<string> {
			"Today we have an excellent matchup for the people of Brimshire!", 
			"Today on the field we hope to see some heart and hustle!",
			"Please remember the mandatory curfew after the match tonight. Ride home safely.",
			"This sold out stadium is ready for an electric match!",
			"Let the match begin!", 
			"Prepare yourselves for the best match of the season!",
			"Not a particularly exciting match today, folks but we're here nonetheless!", 
			"Today's matchup is sponsored by Riverside Shipping.",
			"Another beautiful day in Brimshire with an exciting match ahead!",
			"The King himself is watching today's matchup via viewing portal!",
			"Let's play some Brimball!", 
			"Let the match begin!", 
			"Athletes, please take your positions.",
			"Today's matchup is one we've all been waiting for!"
		};

		matchWelcomeStrings.Add ("Welcome to the home of the " + homeTeam.teamName + "! Sit back and enjoy.");
		matchWelcomeStrings.Add ("Fans of " + homeTeam.teamName + " are hungry for a victory tonight!");
		matchWelcomeStrings.Add ("The " + awayTeam.teamName + " take on the " + homeTeam.teamName + " today.");
		matchWelcomeStrings.Add ("We've got a great matchup here in Week " + (GameController.week + 1) + " of the season.");
		matchWelcomeStrings.Add (awayTeam.teamName + " takes on the " + homeTeam.teamName + " in what's sure to be a battle.");
		matchWelcomeStrings.Add (homeTeam.teamName + " supporters are going wild for the home team!");

		return matchWelcomeStrings [Random.Range (0, matchWelcomeStrings.Count)];
	}

	public string GenerateHalftimeString() {
		List<string> halftimeStrings = new List<string> {
			"The managers will have a chat with their teams and we'll resume after halftime.",
			"And that's halftime. We'll resume shortly.",
			"The official has signaled for halftime.",
			"And that brings us to halftime, folks.",
			"Fans, be sure to stop by the concessiaries during halftime.",
			"These athletes will appreciate a quick breather as we enter halftime."
		};

		if (homeScore > awayScore) {
			halftimeStrings.Add ("The home crowd is showing their team appreciation as they head into halftime with the lead!");
			halftimeStrings.Add ("Fans of " + homeTeam.teamName + " are on their feet as we head into halftime.");
			halftimeStrings.Add(homeTeam.teamName + " have the lead over " + awayTeam.teamName + " as we hit the halfway mark.");
		} else if (awayScore > homeScore) {
			halftimeStrings.Add ("The home crowd is not happy with this performance as the visitors lead at halftime.");
			halftimeStrings.Add ("The visiting " + awayTeam.teamName + " hold the lead as we enter halftime.");
			halftimeStrings.Add ("Boos fill the stadium from disatisfied " + homeTeam.teamName + " supporters at the half!"); 
		} else { //They tied
			halftimeStrings.Add("And we're all tied up heading into halftime.");
			halftimeStrings.Add ("The score is all knotted up as we cross the halfway mark.");
			halftimeStrings.Add ("After the first half of play we've got a tie game!");
		}

		return halftimeStrings [Random.Range (0, halftimeStrings.Count)];
	}

	public string GenerateFinalString() {
		List<string> finalStrings = new List<string> {
			"And the official indicates that the match is over.",
			"And the match is over! Please ride home safely, folks!",
			//"An anticlimactic finish to an otherwise exciting match.", //This one should be conditional
			"The horn is blown and the match is concluded! What a contest!"
		};

		if (homeScore > awayScore) {
			finalStrings.Add ("The home crowd erupts as their " + homeTeam.teamName + " secure the victory!");
			finalStrings.Add (homeTeam.teamName + " defeat the visiting " + awayTeam.teamName + " to the delight of the crowd!");
			finalStrings.Add ("The crowd cheers as " + homeTeam.teamName + " defeat " + awayTeam.teamName + "!");
			//finalStrings.Add("There will be no riots in " + CITY + " tonight after this victory!");
		} else if (awayScore > homeScore) {
			finalStrings.Add ("The home fans are absolutely furious that " + awayTeam.teamName + " came away with the victory!");
			finalStrings.Add (awayTeam.teamName + " came into hositle territory and walk away victorious!");
			finalStrings.Add (homeTeam.teamName + " supporters are threating to riot after this loss at home!");
			finalStrings.Add ("The visiting " + awayTeam.teamName + " have left the crowd silent with this big time win!");
		} else {
			finalStrings = new List<string> (); //Prevents any standard strings from being displayed
			finalStrings.Add("As the match is called and we're still tied up the home team will be awarded the victory.");
			finalStrings.Add ("According to the rules this tie will count as a win for the home squad, " + homeTeam.teamName);
			finalStrings.Add ("Ties are so dull. This will go down as a victory for the home team in the standings.");
			finalStrings.Add ("You have to wonder when they'll change the tie game rules as the home team is awarded victory.");
		}

		return finalStrings [Random.Range (0, finalStrings.Count)];
	}

	public string GenerateShootSuccessString(Athlete a) {
		List<string> shootSuccessStrings = new List<string> ();

		shootSuccessStrings.Add (a.name + " took the shot and they scored easily.");
		shootSuccessStrings.Add ("GOAL! " + a.name + " knocked one off the rim and into the net!");
		shootSuccessStrings.Add ("Here's the shot. And a spectacular goal from " + a.name + "!");
		shootSuccessStrings.Add (a.name + " just launched a shot right and it's off the rim and in! GOAL!");
		shootSuccessStrings.Add ("GOAL! " + a.name + " took a shot that left the defenders absolutely stunned.");
		shootSuccessStrings.Add ("GOAL! " + a.name + " is making it look easy out there with that strike.");
		shootSuccessStrings.Add ("Goal for " + a.GetTeam ().teamName + " after " + a.name + " placed it in.");
		shootSuccessStrings.Add ("GOAL. " + a.name + " looks like they're out on the practice pitch taking shots on an empty net.");

		return shootSuccessStrings [Random.Range (0, shootSuccessStrings.Count)];
	}

	public string GenerateShootFailString(Athlete a) {
		List<string> shootFailStrings = new List<string> ();

		shootFailStrings.Add (a.name + " took the shot and just barely missed the target!");
		shootFailStrings.Add ("I can't believe it! " + a.name + " just missed high and wide!");
		shootFailStrings.Add (a.name + " put a bit too much spin on that one and it's bounced off the rim.");
		shootFailStrings.Add ("Oh my that's an unlucky strike. " + a.name + " only narrowly missed!");
		shootFailStrings.Add (a.name + " takes the shot and it's off the rim! That's got to hurt!");
		shootFailStrings.Add ("Here's a shot! " + a.name + " must have lost their footing with that miss.");
		shootFailStrings.Add (a.name + " just embarassed " + a.GetTeam ().teamName + " with that miss!");
		shootFailStrings.Add ("The fans are not happy with " + a.name + " after that misguided shot.");
		shootFailStrings.Add (a.name + " needs to spend more time on the practice pitch if they miss like.");
		shootFailStrings.Add (a.name + " shouldn't be taking those shots. That was bad.");

		return shootFailStrings [Random.Range (0, shootFailStrings.Count)];
	}

	public string GeneratePassSuccessString(Athlete a, Athlete p) {
		List<string> passSuccessStrings = new List<string> ();

		passSuccessStrings.Add (a.name + " made an easy pass to " + p.name + ".");
		passSuccessStrings.Add (a.name + " played the ball over to " + p.name + ".");
		passSuccessStrings.Add (a.name + " passed the ball to " + p.name + ".");
		passSuccessStrings.Add (a.name + " linked up with " + p.name + " for a quick pass.");
		passSuccessStrings.Add (a.name + " connected with " + p.name + " on an easy pass.");
		passSuccessStrings.Add (a.name + " played it simple to " + p.name + ".");

		return passSuccessStrings [Random.Range (0, passSuccessStrings.Count)];
	}

	public string GeneratePassOOBString(Athlete a, Athlete p) {
		List<string> passOOBStrings = new List<string> ();

		passOOBStrings.Add (a.name + " tried switching it over to " + p.name + " but the ball sailed out of play.");
		passOOBStrings.Add (a.name + " stepped on the boundary while trying to make a pass.");
		passOOBStrings.Add ("Out of play. " + a.name + " and " + p.name + " weren't on the same page clearly.");

		return passOOBStrings [Random.Range (0, passOOBStrings.Count)];
	}

	public string GenerateStealSuccessString(Athlete stealer, Athlete victim) {
		List<string> stealSuccessStrings = new List<string> ();

		stealSuccessStrings.Add (stealer.name + " just took the ball away from " + victim.name + "!");
		stealSuccessStrings.Add (stealer.name + " stole the ball from " + victim.name + " before they could react!");
		stealSuccessStrings.Add (stealer.name + " made " + victim.name + " look foolish by snatching the ball away!");

		return stealSuccessStrings [Random.Range (0, stealSuccessStrings.Count)];
	}

	public string GenerateStealFailString(Athlete s, Athlete v) {
		List<string> stealFailStrings = new List<string> ();

		stealFailStrings.Add (s.name + " tried to steal the ball from " + v.name + " but whiffed.");
		stealFailStrings.Add (s.name + " slide in for the steal but missed the mark!");
		stealFailStrings.Add (s.name + " went hard for the steal but missed their opportunity.");

		return stealFailStrings [Random.Range (0, stealFailStrings.Count)];
	}

	public string GenerateInterceptSuccessString(Athlete d, Athlete v) {
		List<string> interceptSuccessStrings = new List<string> ();

		interceptSuccessStrings.Add (d.name + " anticipated the move and stole the ball from " + v.name);
		interceptSuccessStrings.Add (d.name + " just snatched possession away from " + v.name);
		interceptSuccessStrings.Add ("Oh my! " + d.name + " just put " + v.name + " on the ground and took the puck!");

		return interceptSuccessStrings [Random.Range (0, interceptSuccessStrings.Count)];
	}

	public string GenerateInterceptFailString(Athlete d, Athlete v) {
		List<string> interceptFailStrings = new List<string> ();

		interceptFailStrings.Add (d.name + " tried to go for an interception but instead was embarassed.");
		interceptFailStrings.Add (d.name + " thought they were a clever defender but thought wrong.");
		interceptFailStrings.Add (d.name + " isn't well known for his defense as he's let the rock slide right past him.");

		return interceptFailStrings [Random.Range (0, interceptFailStrings.Count)];
	}

	public string GenerateDribbleSuccessString(Athlete a) {
		List<string> dribbleSuccessStrings = new List<string> ();

		dribbleSuccessStrings.Add (a.name + " has carried the rock upfield.");
		dribbleSuccessStrings.Add (a.name + " is moving quickly with the ball.");
		dribbleSuccessStrings.Add (a.name + " carried the ball forward into the next zone.");
		dribbleSuccessStrings.Add (a.name + " is charging towards the goal!");
		dribbleSuccessStrings.Add (a.name + " has their sights set on the goal and they're bringing the puck with.");
		dribbleSuccessStrings.Add (a.name + " is bringing the ball upfield.");
		dribbleSuccessStrings.Add (a.name + " is dribbling it forward.");

		return dribbleSuccessStrings [Random.Range (0, dribbleSuccessStrings.Count)];
	}

	public string GenerateDribbleFailString(Athlete a) {
		List<string> dribbleFaiLStrings = new List<string> ();

		dribbleFaiLStrings.Add (a.name + " was trying to bring the ball upfield but tripped on a rough patch.");
		dribbleFaiLStrings.Add (a.name + " lost control of their dribble and it's gotten away from them.");
		dribbleFaiLStrings.Add (a.name + " needs to spend some time in the academy leagues for giving it away like that.");

		return dribbleFaiLStrings [Random.Range (0, dribbleFaiLStrings.Count)];
	}

	public string GenerateThroughSuccessString(Athlete a, Athlete p) {
		List<string> throughSuccessStrings = new List<string> ();

		throughSuccessStrings.Add (a.name + " played a lovely through ball to " + p.name + ".");
		throughSuccessStrings.Add (a.name + " is sharing the ball well with that through ball to " + p.name + ".");
		throughSuccessStrings.Add (a.name + " played the puck ahead to " + p.name + ".");
		throughSuccessStrings.Add (a.name + " got " + p.name + " involved by lobbing the rock upfield.");
		throughSuccessStrings.Add (a.name + " placed it upfield perfectly for " + p.name + ".");

		return throughSuccessStrings [Random.Range (0, throughSuccessStrings.Count)];
	}

	public string GenerateThroughFailString(Athlete a, Athlete p) {
		List<string> throughFailStrings = new List<string> ();

		throughFailStrings.Add (a.name + " tried to pass it ahead to " + p.name + " but misjudged the distance.");
		throughFailStrings.Add (a.name + " attempted a through ball to " + p.name + "but they weren't on the same page.");
		throughFailStrings.Add (a.name + " was too ambitious with that through attempt to " + p.name + ".");
		throughFailStrings.Add (a.name + " misjudged " + p.name + "'s route and gave up the rock.");
		throughFailStrings.Add (a.name + " placed it upfield but it's nowhere near " + p.name + " and they've lost it.");

		return throughFailStrings [Random.Range (0, throughFailStrings.Count)];
	}

	public string GenerateHeaveSuccessString(Athlete a) {
		List<string> heaveSuccessStrings = new List<string> ();

		heaveSuccessStrings.Add ("GOAL!!! I don't believe it but " + a.name + " scored a shot from across midfield!");
		heaveSuccessStrings.Add (a.name + " is shooting from beyond the mid line and they've hit it. Goal!");
		heaveSuccessStrings.Add (a.name + " showcased their range by scoring from beyond the mid line!");
		heaveSuccessStrings.Add ("Goal. " + a.name + " had no business making a shot from so far.");
		heaveSuccessStrings.Add ("Incredible. " + a.name + " threw up a prayer and it was answered!");

		return heaveSuccessStrings [Random.Range (0, heaveSuccessStrings.Count)];
	}

	public string GenerateHeaveFailString(Athlete a) {
		List<string> heaveFailStrings = new List<string> ();

		//heaveFailStrings.Add (a.name + " will have a talk with " + a.GetTeam ().teamManager.name + " after that heave.");
		//heaveFailStrings.Add (a.GetTeam ().teamManager.name + " cant be happy about " + a.name + " taking a shot from that range.");
		heaveFailStrings.Add (a.name + " thought they were a true marksmun with that heave from downtown.");
		heaveFailStrings.Add (a.name + " tried a shot from long distance and it's bounced off the rim!");
		heaveFailStrings.Add (a.name + " punted the ball downfield and it fell short of the goal line.");
		heaveFailStrings.Add (a.name + " took an ill-advised shot from long distance and it's not even close.");
		heaveFailStrings.Add (a.name + " may need to get their eye sight checked because I'm not sure they know where the goal is!");

		return heaveFailStrings [Random.Range (0, heaveFailStrings.Count)];
	}

	public string GenerateDunkSuccessString(Athlete a) {
		List<string> dunkSuccessStrings = new List<string> ();

		dunkSuccessStrings.Add ("Goal. " + a.name + " rose up for the dunk and slammed it in.");
		dunkSuccessStrings.Add (a.name + " picked up the rock, drove to the goal and dunked it.");
		dunkSuccessStrings.Add (a.name + " ignited the crowd with that astounding dunk!");
		dunkSuccessStrings.Add ("GOAL. " + a.name + " got up close and personal for a clean finish.");
		dunkSuccessStrings.Add (a.name + " showcased their force with a brutal slam dunk.");

		return dunkSuccessStrings [Random.Range (0, dunkSuccessStrings.Count)];
	}

	public string GenerateDunkFailString(Athlete a) {
		List<string> dunkFailStrings = new List<string> ();

		dunkFailStrings.Add ("Oh my! " + a.name + " tried to get fancy and blew a wide open dunk.");
		dunkFailStrings.Add (a.name + " rises up for the dunk and oh dear! Stuffed by the rim.");
		dunkFailStrings.Add (a.name + " tried to put up a highlight for this crowd but lost control.");
		dunkFailStrings.Add (a.name + " needs to concentrate more if they hope to make easy dunks like that.");
		dunkFailStrings.Add (a.name + " missed a dunk that the King's jester could have landed.");

		return dunkFailStrings [Random.Range (0, dunkFailStrings.Count)];
	}

	public string GenerateDefensiveStanceString(Athlete a) {
		List<string> defenseStrings = new List<string> ();

		defenseStrings.Add (a.name + " is dropping into a defensive position.");
		defenseStrings.Add (a.name + " is looking to defend their position.");
		defenseStrings.Add (a.name + " dropped into a defensive stance.");
		defenseStrings.Add (a.name + " looks to be preparing to stop opportunities in their lane.");

		return defenseStrings [Random.Range (0, defenseStrings.Count)];
	}

	//Considering putting these quotes inside the athlete scripts but probably shouldn't.

	public string GetAthleteAvoidHeaveQuote() {
		List<string> quotes = new List<string> {
			"I'm not so sure about this one, coach. I don't think I have that kind of range.", "That's too far for me. I'd rather get closer.",
			"I'd prefer to be a bit closer to the goal when I shoot, coach.", "I dunno about this one coach. I'll probably miss."
		};

		string quote = '"' + quotes [Random.Range (0, quotes.Count)] + '"';

		return quote;
	}

	public string GetAthleteWantShootQuote() {
		List<string> quotes = new List<string> {
			"Let me take the shot, coach!", "Coach I've got this!", "I'm a shooter. Let me shoot.", "We need a goal here I've got this!",
			"They can't stop me. This is the easiest opportunity I'll have all match."
		};

		string quote = '"' + quotes [Random.Range (0, quotes.Count)] + '"';

		return quote;
	}

	public string GetAthleteWantDunkQuote() {
		List<string> quotes = new List<string> {
			"I'm bout to embarrass these chumps if you let me, coach.", "Let's put on a show for the crowd!", 
			"This is the easiest opportunity I'll have all day, coach.", "It's time to add some plays to my highlight reel!",
			"This is what I do best, coach. Lemme give 'em the business.", "Careful coach I might crack the rim with this jam!"
		};

		string quote = '"' + quotes [Random.Range (0, quotes.Count)] + '"';

		return quote;
	}

	public string GetAthleteWantDriveQuote() {
		List<string> quotes = new List<string> {
			"You know I'm best in the open field, coach. Lemme get closer to the goal.", "I've got the rock and I've got some space. Lemme go to work.",
			"Let's keep it moving forward!", "We gotta put some pressure on their goalzone. Lemme carry it up.", "I've got an open lane to drive to the goal, coach."
		};

		string quote = '"' + quotes [Random.Range (0, quotes.Count)] + '"';

		return quote;
	}

	public string GetAthleteWantPassQuote() {
		List<string> quotes = new List<string> {
			"Coach we gotta get the whole team involved. I don't mind sharing.", "Let's get somebody else the ball and I'll play some D.",
			"I love setting up my squadmates. Let me pass it, coach.", "I see somebody open if you let me pass the rock.",
			"I don't need to take every opportunity. I need to get my teammates involved."
		};

		string quote = '"' + quotes [Random.Range (0, quotes.Count)] + '"';

		return quote;
	}

	public string GetAthleteWantThroughQuote() {
		List<string> quotes = new List<string> {
			"Somebody's open up ahead. Let's keep the ball moving forward towards the goal.", "As good as I am, I think I should play this ball ahead.",
			"Let me play this puck through and I can get set on defense.", "I see an easy opportunity to play it through. Is that alright, coach?"
		};

		string quote = '"' + quotes [Random.Range (0, quotes.Count)] + '"';

		return quote;
	}

	public string GetAthleteWantJukeQuote() {
		List<string> quotes = new List<string> {
			"You know I've got the moves, coach. Lemme break some ankles.", "I can make this defender look like a kid dancing on a lake in Winterstep.",
			"Just lemme reach into my bag of tricks right quick, coach.", "Let's give these fans something to cheer for and someone to laugh at."
		};

		string quote = '"' + quotes [Random.Range (0, quotes.Count)] + '"';

		return quote;
	}

	public string GetAthleteWantDefenseQuote() {
		List<string> quotes = new List<string> {
			"I'll lock down this position.", "Lock down time. Ain't nobody getting past me.", "I can stop any fool bold enough to try to get by me.",
			"Scoring is cool and all but defense wins championships.", "I know all their moves, coach. I won't let em get by me.", 
			"They won't get anything done in this zone, coach."
		};

		string quote = '"' + quotes [Random.Range (0, quotes.Count)] + '"';

		return quote;
	}

	public string GetAthleteWantProtectNetQuote() {
		List<string> quotes = new List<string> {
			"I promise I won't let any shots get past me.", "I'll protect this net with my life, coach.", "I'll protect this net as if it were the King himself.",
			"I'll keep that net still and silent for you, coach.", "Let the rest of the squad do the scoring. I'll take care of the stopping."
		};

		string quote = '"' + quotes [Random.Range (0, quotes.Count)] + '"';

		return quote;
	}

}