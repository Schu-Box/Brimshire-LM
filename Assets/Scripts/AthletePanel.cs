using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AthletePanel : MonoBehaviour {

	public Athlete athlete = null;

	public Text nameText;
	public Image bodyImg;
	public Image jerseyImg;
	public Text positionText;
	public Text overallStatText;
	public GameObject descriptorPanel;
	public Text descriptorText;
	[HideInInspector] public Button button;

	private GameController gameController;
	private MatchManager matchManager;

	public void Awake() {
		gameController = FindObjectOfType<GameController> ();
		matchManager = FindObjectOfType<MatchManager>();
		button = GetComponent<Button> ();
	}

	public void SetAthletePanel(Athlete a) {
		athlete = a;

		button.onClick.RemoveAllListeners ();

		if (athlete != null) {
			nameText.text = a.name;
			positionText.text = a.positionName;

			bodyImg.enabled = true;
			bodyImg.sprite = a.bodySprite;
			bodyImg.color = Color.white;
			jerseyImg.enabled = true;
			jerseyImg.sprite = a.jerseySprite;
			jerseyImg.color = a.GetTeam ().teamColor;

			overallStatText.text = a.overallRating.ToString();

			descriptorPanel.SetActive(false);

			button.onClick.AddListener (() => gameController.DisplayAthletePanel (gameObject, a));
		} else {

			for(int i = 0; i < transform.childCount; i++) {
				transform.GetChild(i).gameObject.SetActive(false);
			}
			button.interactable = false;

			/*
			nameText.text = "";
			bodyImg.enabled = false;
			jerseyImg.enabled = false;
			positionText.text = "";
			overallStatText.text = "";
			descriptorPanel.SetActive (false);
			*/
		}
	}
}
