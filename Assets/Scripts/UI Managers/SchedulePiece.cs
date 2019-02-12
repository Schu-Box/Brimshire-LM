using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SchedulePiece: MonoBehaviour {
	public TextMeshProUGUI txt;
	public Image img;
	public Image borderImg;
	public TextMeshProUGUI headerTxt;
	public Button button;

	private GameController gameController;

	private TeamController subjectTeam;
	private Matchup match;

	public void SetSchedulePiece(TeamController opponentTeam, int weekIndex) {
		gameController = FindObjectOfType<GameController>();
		button.onClick.RemoveAllListeners();

		subjectTeam = opponentTeam;

		headerTxt.text = "Week " + (weekIndex + 1);

		string mainString;

		if(opponentTeam != null) {
			match = opponentTeam.seasonMatchups[weekIndex];

			img.color = opponentTeam.teamColor;
			borderImg.color = opponentTeam.teamCityColor;
			
			if(opponentTeam == match.homeTeam) {
				mainString = "@ ";
			} else {
				mainString = "vs ";
			}
			mainString += opponentTeam.teamAbbreviation;

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
			borderImg.enabled = false;
		}

		txt.text = mainString;
	}

	public void UpdateSchedulePiece() {
		if(match != null & match.winner != null) {

			string newString = txt.text + '\n';
			if(match.winner == subjectTeam) {
				newString += "Lost ";
			} else {
				newString += "Won ";
			}

			if(subjectTeam == match.homeTeam) {
				newString += match.awayScore + " - " + match.homeScore;
			} else {
				newString += match.homeScore + " - " + match.awayScore;
			}

			txt.text = newString;
		}
	}
}
