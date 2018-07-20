using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AthleteMatchPanel : MonoBehaviour {

	public Athlete athlete;

	public Text nameText;
	public Image bodyImg;
	public Image jerseyImg;
	public Text positionText;
	public Slider actionSlider;
	public Image selectionBorderImg;
	private Button button;

	private MatchManager matchManager;

	void Start() {
		matchManager = FindObjectOfType<MatchManager>();
		button = GetComponent<Button>();
	}

	public void SetAthleteMatchPanel(Athlete a) {
		if(a != null) {
			athlete = a;

			Color teamColor = athlete.GetTeam().teamColor;

			nameText.text = athlete.name;
			bodyImg.sprite = athlete.bodySprite;
			jerseyImg.sprite = athlete.jerseySprite;
			jerseyImg.color = teamColor;
			positionText.text = athlete.positionName;

			selectionBorderImg.color = teamColor;
			//selectionBorderImg.enabled = false;
			
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

				if(a.GetTeam() == GameController.playerManager.GetTeam()) {
					button.onClick.AddListener(() => matchManager.RemoveAthleteFromField(this));
				} else {
					button.enabled = false;
				}
			}
		} else {
			gameObject.SetActive(false);
		}
	}
}
