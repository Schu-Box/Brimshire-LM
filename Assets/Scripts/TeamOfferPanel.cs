using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TeamOfferPanel : MonoBehaviour, IPointerEnterHandler {

	public Image teamColorImg;
	public Text teamNameText;
	public Image countyColorImg;
	public Text teamTypeText;
	public Text rankText;

	private TeamController team;
	private GameController gameController;

	public void SetTeam(TeamController theTeam) {
		gameController = FindObjectOfType<GameController>();

		team = theTeam;

		teamColorImg.color = team.teamColor;
		teamNameText.text = team.teamName;

		countyColorImg.color = team.teamCityColor;
		string typeString;
		if(team.GetCity().GetCounty().GetCapitalCity().GetTeamOfCity() == team) { //If the team is the capital
			typeString = "Capital";
		} else {
			typeString = "Lower";
		}
		typeString += " Team" + '\n' + "in " + team.GetCity().GetCounty().countyName;
		teamTypeText.text = typeString;
		
		rankText.text = "Ranked <99th> in Brimshire";
	}

	public void OnPointerEnter(PointerEventData eventData) {
		gameController.ViewTeamSelection(team);
	}
}
