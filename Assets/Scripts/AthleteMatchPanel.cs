using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AthleteMatchPanel : MonoBehaviour {

	public Athlete athlete;

	public Text nameText;
	public Image bodyImg;
	public Image jerseyImg;
	public Image ballImg;
	public Text positionText;

	public Text overallText;
	public Text speedText;
	public Text strengthText;
	public Text ballControlText;
	public Text defenseText;
	public Slider actionSlider;


	public Image selectionBorderImg;
	private Button button;

	private MatchManager matchManager;

	void Awake() {
		matchManager = FindObjectOfType<MatchManager>();
		button = GetComponent<Button>();
	}

	public void SetAthleteMatchPanel(Athlete a) {
		athlete = a;

		if(athlete != null) {
			athlete.athleteMatchPanel = this;

			Color teamColor = athlete.GetTeam().teamColor;

			nameText.text = athlete.name;
			bodyImg.sprite = athlete.bodySprite;
			jerseyImg.sprite = athlete.jerseySprite;
			jerseyImg.color = teamColor;
			positionText.text = athlete.positionName;

			ballImg.enabled = false;

			overallText.text = athlete.overallRating.ToString();
			speedText.text = athlete.GetAttributeValue("speed").ToString();
			strengthText.text = athlete.GetAttributeValue("strength").ToString();
			ballControlText.text = athlete.GetAttributeValue("ball control").ToString();
			defenseText.text = athlete.GetAttributeValue("defense").ToString();

			selectionBorderImg.color = teamColor;
			//selectionBorderImg.enabled = false;
			
			button.enabled = true;
			button.onClick.RemoveAllListeners();

			if(athlete.currentFieldTile == null) {
				actionSlider.value = 0;
				actionSlider.GetComponentInChildren<Text>().text = "On the Bench";

				if(a.GetTeam() == GameController.playerManager.GetTeam()) {
					button.onClick.AddListener (() => matchManager.SelectAthlete (this));
					selectionBorderImg.enabled = false;
				} else {
					button.enabled = false;
				}
			} else {
				actionSlider.GetComponentInChildren<Text>().text = "Waiting to Start the Match";

				if(a.GetTeam() == GameController.playerManager.GetTeam() && !matchManager.currentMatch.matchStarted) {
					button.onClick.AddListener(() => matchManager.RemoveAthleteFromField(this));
				} else {
					button.enabled = false;
				}

				selectionBorderImg.enabled = true;
			}
		} else {
			gameObject.SetActive(false);
		}
	}
}
