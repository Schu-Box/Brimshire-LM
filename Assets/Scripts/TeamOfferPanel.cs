using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeamOfferPanel : MonoBehaviour {

	public Image teamColorImg;
	public Text teamNameText;
	public Image countyColorImg;
	public Text teamTypeText;
	public Text rankText;

	private TeamController team;

	public void SetTeam(TeamController theTeam) {
		team = theTeam;

		teamColorImg.color = team.teamColor;
		teamNameText.text = team.teamName;

		countyColorImg.color = team.GetCity().GetCounty().countyColor;
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
}
