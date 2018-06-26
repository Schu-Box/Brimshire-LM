using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class League {

	public List<TeamController> teamList;
	public List<List<Matchup>> weeklyListOfMatchupsForSeason;
	public int regularSeasonLength = 9;

	public int numPostseasonRounds = 0;
	public int numPostseasonTeams = 0;

	public int goldRewardPerWin = 5;
	public int prestigeRewardPerWin = 5;

	public int seasonsPlayed = 0; //Should be private

	public List<TeamController> leagueChampions = new List<TeamController>();
	public TeamController[] teamStandings;

	//Everything below is up for review

	public int draftClassSize = 18;
	public int activeAthletesPerTeam = 3;
	public int numFieldPositions = 4;

	private GameController gameController;
	private TeamController[] postseasonTeams;

	private List<TeamController> draftOrder = new List<TeamController> ();
	private List<Athlete> draftClassList = new List<Athlete> ();
	private int draftTurn = 0;

	public League(List<TeamController> teams) {
		gameController = GameObject.FindObjectOfType<GameController>();

		teamList = teams;
	}

	public void SetSeason() {
		weeklyListOfMatchupsForSeason = new List<List<Matchup>> ();

		for (int i = 0; i < regularSeasonLength + numPostseasonRounds; i++) { //Create an empty matchup list for every week
			weeklyListOfMatchupsForSeason.Add(new List<Matchup>());
		}

		//Clears the match list. Eventually in the interest of stat tracking I may want to retain this information, either by adding to the list or by storing the data in some historical arrays
		for (int i = 0; i < teamList.Count; i++) {
			teamList [i].seasonMatchups = new List<Matchup> ();

			teamList [i].winsThisSeason = 0;
			teamList [i].losesThisSeason = 0;
		}

		SetSeasonMatchups ();
	}

	private void SetSeasonMatchups() {

		bool switcher = false; //This will alternate teams being slotted into home and away
		//Not working correctly. Several teams have uneven home/away games. May be fixable by giving a bye week or having an even number of games.

		List<TeamController> unassignedTeamsForWeek = new List<TeamController> ();
		for (int t = 0; t < teamList.Count; t++) {
			unassignedTeamsForWeek.Add (teamList [t]);
		}
		if (teamList.Count % 2 == 1) { //If the team array is odd, add a null
			unassignedTeamsForWeek.Add(null);
		}

		TeamController[] randomOrder = new TeamController[unassignedTeamsForWeek.Count];

		for (int t = 0; t < randomOrder.Length; t++) {
			TeamController randomTeam = unassignedTeamsForWeek [Random.Range (0, unassignedTeamsForWeek.Count)];
			randomOrder [t] = randomTeam;
			unassignedTeamsForWeek.Remove (randomTeam);
		}

		//Debug.Log (teamArray [0].teamName);

		for (int i = 0; i < regularSeasonLength; i++) { //For each week of the regular season

			int[] teamSlots = new int[randomOrder.Length];
			int slotInt;

			for(int j = 0; j < teamSlots.Length; j++) {

				slotInt = (j + i) % (teamSlots.Length - 1);

				teamSlots [j] = slotInt;
			}
				
			for (int m = 0; m < randomOrder.Length / 2; m++) { //For each matchup (equal to half the amount of teams in the randomOrder)

				TeamController team1;
				TeamController team2;

				if(m == 0) {
					team1 = randomOrder [teamSlots [0]];
					team2 = randomOrder [randomOrder.Length - 1]; //One team has to remain stationary in the rotation while all others rotate
				} else {
					team1 = randomOrder [teamSlots [m]];
					team2 = randomOrder [teamSlots [randomOrder.Length - 1 - m]];
				}

				if (team1 == null) {
					team2.seasonMatchups.Add (null); //Add bye week
				} else if (team2 == null) {
					team1.seasonMatchups.Add (null); //Add bye week
				} else {

					Matchup newMatch;

					if (switcher == true) { //team1 will be home
						newMatch = new Matchup (this, i, team1, team2);
					} else { //team2 will be home
						newMatch = new Matchup (this, i, team2, team1);
					}

					weeklyListOfMatchupsForSeason [i].Add (newMatch);

					team1.seasonMatchups.Add (newMatch); //Sets a reference to this matchup for the first team
					team2.seasonMatchups.Add (newMatch); //Ditto for the other team
			
					//Debug.Log (weeklyListOfMatchupsForSeason [i] [m].homeTeam.teamName + " vs " + weeklyListOfMatchupsForSeason [i] [m].awayTeam.teamName);
				}

			}

			switcher = !switcher; 
			//This method doesn't provide entirely accurate results. For example, one team ends up with 6 home games and 3 away games. Everyone else is good tho so it's aight for now
		}
	}

	public void SetTeamStandings() {
		List<TeamController> unrankedTeams = new List<TeamController> ();
		teamStandings = new TeamController[teamList.Count];

		//Debug.Log (teamStandings.Length);

		for (int i = 0; i < teamList.Count; i++) {
			unrankedTeams.Add (teamList [i]);
		}

		for (int i = 0; i < teamList.Count; i++) {
			int highestAmountOfWins = -1;
			TeamController bestTeam = teamList [0];

			for (int j = 0; j < unrankedTeams.Count; j++) {

				if (unrankedTeams [j].winsThisSeason > highestAmountOfWins) {
					highestAmountOfWins = unrankedTeams [j].winsThisSeason;
					bestTeam = unrankedTeams [j];
				} else if (unrankedTeams [j].winsThisSeason == highestAmountOfWins) { //If team j has the same amount of wins as the previous highest win holder
					//Debug.Log("TIE in standings");

					//Check head to head of team j and current best team
					TeamController winnerOfHeadToHead = CheckHeadToHead(unrankedTeams[j], bestTeam);
					if (winnerOfHeadToHead != null) { 
						bestTeam = winnerOfHeadToHead;
					} else { //If that's a tie, check goal differential between the two
						//Debug.Log("STILL TIED");

						TeamController betterGoalDifferential = CheckGoalDifferential (unrankedTeams [j], bestTeam);
						if (betterGoalDifferential != null) {
							bestTeam = betterGoalDifferential;
						} else {
							//Debug.Log ("Same goal differential. Still tied.");

							TeamController moreTotalGoals = CheckTotalGoals (unrankedTeams [j], bestTeam);
							if (moreTotalGoals != null) {
								bestTeam = moreTotalGoals;
							} else {
								//Debug.Log ("And they have the exact same amount of total goals.");
							}
						}
					}
				}
			}

			teamStandings [i] = bestTeam;
			unrankedTeams.Remove (teamStandings [i]);
		}

		/*
		for (int i = 0; i < teamStandings.Length; i++) {
			Debug.Log (teamStandings [i].teamName + " - " + teamStandings [i].winsThisSeason);
		}
		*/
	}

	public TeamController CheckHeadToHead(TeamController team1, TeamController team2) {
		int team1WinsVsTeam2 = 0;
		int team2WinsVsTeam1 = 0;

		for (int i = 0; i < team1.seasonMatchups.Count; i++) {
			Matchup matchChecked = team1.seasonMatchups [i];

			//This function could be simplified if I stored some sort of opponent reference, but might be adding unneccessary data and be complicated elsewhere.
			if (matchChecked.homeTeam == team1) {
				if (matchChecked.awayTeam == team2) {
					if (matchChecked.winner == team1) {
						team1WinsVsTeam2++;
					} else if (matchChecked.winner == team2) {
						team2WinsVsTeam1++;
					} //else the match hasn't been played yet or I fucked something up big time
				}
			} else if (matchChecked.awayTeam == team1) {
				if (matchChecked.homeTeam == team2) {
					if (matchChecked.winner == team1) {
						team1WinsVsTeam2++;
					} else if (matchChecked.winner == team2) {
						team2WinsVsTeam1++;
					}
				}
			}
		}

		if (team1WinsVsTeam2 > team2WinsVsTeam1) {
			return team1;
		} else if (team2WinsVsTeam1 > team1WinsVsTeam2) {
			return team2;
		} else { //They tied on head to head matches as well
			return null;
		}
	}

	public TeamController CheckGoalDifferential(TeamController team1, TeamController team2) {
		int team1GoalDifferential = 0;
		int team2GoalDifferential = 0;

		for (int i = 0; i < team1.seasonMatchups.Count; i++) {
			Matchup matchChecked = team1.seasonMatchups [i];

			if (matchChecked.homeTeam == team1) {
				team1GoalDifferential += matchChecked.homeScore;
				team1GoalDifferential -= matchChecked.awayScore;
			} else if (matchChecked.awayTeam == team1) {
				team1GoalDifferential += matchChecked.awayScore;
				team1GoalDifferential -= matchChecked.homeScore;
			}
		}

		for (int i = 0; i < team2.seasonMatchups.Count; i++) {
			Matchup matchChecked = team2.seasonMatchups [i];

			if (matchChecked.homeTeam == team2) {
				team2GoalDifferential += matchChecked.homeScore;
				team2GoalDifferential -= matchChecked.awayScore;
			} else if (matchChecked.awayTeam == team2) {
				team2GoalDifferential += matchChecked.awayScore;
				team2GoalDifferential -= matchChecked.homeScore;
			}
		}

		//Debug.Log (team1GoalDifferential + " for team1 vs " + team2GoalDifferential + " for team2");
		if (team1GoalDifferential > team2GoalDifferential) {
			return team1;
		} else if (team2GoalDifferential > team1GoalDifferential) {
			return team2;
		} else {
			return null;
		}
	}

	public TeamController CheckTotalGoals(TeamController team1, TeamController team2) {
		int team1Goals = 0;
		int team2Goals = 0;

		for (int i = 0; i < team1.seasonMatchups.Count; i++) {
			Matchup matchChecked = team1.seasonMatchups [i];

			if (matchChecked.homeTeam == team1) {
				team1Goals += matchChecked.homeScore;
			} else if (matchChecked.awayTeam == team1) {
				team1Goals += matchChecked.awayScore;
			}
		}

		for (int i = 0; i < team2.seasonMatchups.Count; i++) {
			Matchup matchChecked = team2.seasonMatchups [i];

			if (matchChecked.homeTeam == team2) {
				team2Goals += matchChecked.homeScore;
			} else if (matchChecked.awayTeam == team2) {
				team2Goals += matchChecked.awayScore;
			}
		}

		if (team1Goals > team2Goals) {
			return team1;
		} else if (team2Goals > team1Goals) {
			return team2;
		} else {
			return null;
		}
	}

	public void DetermineChampion() {
		SetTeamStandings ();

		leagueChampions.Add (teamStandings [0]);

		Debug.Log (teamStandings [0].teamName + " is the CHAMPION!");
	}

	/*

	public void SetupPostseason() {

		postseasonTeams = new TeamController[numPostseasonTeams];
		for(int i = 0; i < numPostseasonTeams; i++) {
			postseasonTeams [i] = teamStandings [i];
		}

		for (int i = 0; i < postseasonTeams.Length / 2; i++) {
			weeklyListOfMatchupsForSeason [regularSeasonLength] [i] = new Matchup (this, regularSeasonLength, postseasonTeams [i], postseasonTeams [postseasonTeams.Length - 1 - i]);
			Matchup match = weeklyListOfMatchupsForSeason [regularSeasonLength] [i];
			match.homeTeam.seasonMatchups.Add (match);
			match.awayTeam.seasonMatchups.Add (match);
		}
	}
	//THESE THREE FUNCTIONS Could easily be refactored into a single, modular function. But I'm layZ
	public void SetupPostseasonRound2() {
		for (int i = 0; i < postseasonTeams.Length / 4; i++) {
			TeamController homeTeam = weeklyListOfMatchupsForSeason [regularSeasonLength] [i].winner;
			TeamController awayTeam = weeklyListOfMatchupsForSeason [regularSeasonLength] [weeklyListOfMatchupsForSeason [regularSeasonLength].Count - 1 - i].winner;

			weeklyListOfMatchupsForSeason [regularSeasonLength + 1] [i] = new Matchup (this, regularSeasonLength + 1, homeTeam, awayTeam);
			Matchup match = weeklyListOfMatchupsForSeason [regularSeasonLength + 1] [i];
			homeTeam.seasonMatchups.Add (match);
			awayTeam.seasonMatchups.Add (match);
		}
	}

	public void SetupPostseasonFinal() {
		TeamController homeTeam = weeklyListOfMatchupsForSeason [regularSeasonLength + 1] [0].winner;
		TeamController awayTeam = weeklyListOfMatchupsForSeason [regularSeasonLength + 1] [1].winner;

		weeklyListOfMatchupsForSeason [regularSeasonLength + 2] [0] = new Matchup (this, regularSeasonLength + 2, homeTeam, awayTeam);
		Matchup match = weeklyListOfMatchupsForSeason [regularSeasonLength + 2] [0];
		homeTeam.seasonMatchups.Add (match);
		awayTeam.seasonMatchups.Add (match);
	}
	*/

	/*
	public void SetupDraft() {
		GameController.displayTeam = GameController.playerTeam;
		gameController.DisplayTeamPanel (GameController.displayTeam);
		gameController.draftPanel.SetActive (true);
		gameController.seasonDraftText.text = "Season " + seasonsPlayed + " Draft";
		SetTeamStandings ();

		for (int i = 0; i < teamArray.Length; i++) { //Here I can alter the amount of rounds by using for loops.
			draftOrder.Add (teamStandings [teamStandings.Length - 1 - i]);
			gameController.draftOrderUIArray [i].teamText.text = draftOrder [i].teamName;
			gameController.draftOrderUIArray [i].draftedAthleteText.text = "";

			draftOrder [i].SetDraftOrderUI (gameController.draftOrderUIArray [i]);
			draftOrder [i].GetDraftOrderUI ().draftPickText.text = i.ToString ();

			gameController.draftOrderUIArray [i].GetComponent<Image> ().color = draftOrder [i].teamColor;
		}

		for (int i = 0; i < draftClassSize; i++) { //Sets the draft class that teams can choose from.
			Athlete draftee = new Athlete (false);
			draftClassList.Add (draftee);

			AthleteUI athUI = gameController.draftAthleteUIDisplayArray [i];
			athUI.draftee = true;
			athUI.athlete = draftee;
			athUI.nameText.text = draftee.name;
			athUI.forceText.text = draftee.force.ToString ();
			athUI.resilienceText.text = draftee.resilience.ToString ();
			athUI.agilityText.text = draftee.agility.ToString ();
			athUI.tacticsText.text = draftee.tactics.ToString ();



			draftee.athUI = athUI;
		}

		StartDraft ();
	}
	*/
		
	/*
	public void StartDraft() {
		//If this were to be playable with multiple player-controlled teams this would be a double for loop checking if any drafting teams are owned by a player.
		draftTurn = -1;
		gameController.StartCoroutine (gameController.WaitThenDraftStep (this));
	}

	public void ContinueDraft() {
		if (draftTurn >= draftOrder.Count - 1) {
			EndDraft ();
		} else {
			gameController.StartCoroutine (gameController.WaitThenDraftStep (this));
		}
	}

	public void DraftStep() {
		draftTurn++;
		//draftOrder [draftTurn].SetDrafting (true);
		if (draftOrder [draftTurn] != GameController.playerTeam) {
			Athlete draftChoice = draftClassList [Random.Range (0, draftClassList.Count)];

			gameController.ConfirmDraftChoice (draftChoice, draftOrder [draftTurn]);
			draftClassList.Remove (draftChoice);
		} else {
			Debug.Log ("You can draft now?");
			GameController.playerTeam.SetDrafting (true);
		}
	}

	public void EndDraft() {
		Debug.Log ("Ending draft");
		GameController.playerTeam.SetDrafting (false);
		gameController.teamPanel.SetActive (false);
		gameController.draftPanel.SetActive(false);
		gameController.notificationText.gameObject.SetActive (false);

		//gameController.StartPreseason ();
	}

	public void RemoveFromDraftList(Athlete ath) {
		draftClassList.Remove (ath);
	}

	public List<Athlete> GetDraftList() {
		return draftClassList;
	}

	public void SetDraftTurn(int num) {
		draftTurn = num;
	}

	public int GetDraftTurn() {
		return draftTurn;
	}
	*/
}
