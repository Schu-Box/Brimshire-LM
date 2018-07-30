using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScheduleObject : MonoBehaviour {

	public Button button;
	public Image img;
	public Text mainText;
	public Text headerText;

	private GameController gameController;
	
	public void SetScheduleObject(TeamController opponentTeam, int weekIndex) {
		gameController = FindObjectOfType<GameController>();
		button.onClick.RemoveAllListeners();

		headerText.text = "Week " + (weekIndex + 1);

		string mainString;

		if(opponentTeam != null) {
			Matchup match = opponentTeam.seasonMatchups[weekIndex];

			img.color = opponentTeam.teamColor;
			
			if(opponentTeam == match.homeTeam) {
				mainString = "@";
			} else {
				mainString = "vs";
			}
			mainString += '\n' + opponentTeam.teamName;

			if(match.winner != null) {
				mainString += '\n';
				if(match.winner == opponentTeam) {
					mainString += "Lost ";
				} else {
					mainString += "Won ";
				}

				if(opponentTeam == match.homeTeam) {
					mainString += match.awayScore + " - " + match.homeScore;
				} else {
					mainString += match.homeScore + " - " + match.awayScore;
				}
			}

			button.onClick.AddListener(() => gameController.StartCoroutine(gameController.MoveAndDisplayTeamPanel(opponentTeam)));
			button.interactable = true;
		} else {
			mainString = "Bye Week";
			button.interactable = false;
		}

		mainText.text = mainString;
	}
}
