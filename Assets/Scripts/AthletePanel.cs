using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AthletePanel : MonoBehaviour {

	public Athlete athlete = null;

	[HideInInspector] public Text nameText;
	[HideInInspector] public Image bodyImg;
	[HideInInspector] public Image jerseyImg;
	[HideInInspector] public Text positionText;
	[HideInInspector] public Button button;
	[HideInInspector] public GameObject descriptorPanel;
	[HideInInspector] public Text descriptorText;

	private GameController gameController;
	private MatchManager matchManager;

	public void Awake() {
		gameController = FindObjectOfType<GameController> ();
		matchManager = FindObjectOfType<MatchManager>();

		nameText = transform.GetChild (0).GetComponent<Text> ();
		bodyImg = transform.GetChild (1).GetComponent<Image> ();
		jerseyImg = transform.GetChild (2).GetComponent<Image> ();
		positionText = transform.GetChild (3).GetComponent<Text> ();
		descriptorPanel = transform.GetChild (4).gameObject;
		descriptorText = transform.GetChild (4).GetComponentInChildren<Text> ();
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

			if (a.activeAction == null) { //Should always be the case since panels are set at the start of matches and in between matches
				//descriptorText.text = "";
				descriptorPanel.SetActive(false);
			} else {
				//descriptorText.text = "";
				Debug.Log("Called (but I don't think it will be.");
				descriptorPanel.SetActive(true);
			}

			if (matchManager.matchFieldParent.activeSelf == false) { //If the match is not active
				button.onClick.AddListener (() => gameController.DisplayAthletePanel (gameObject, a));
			} else { //If the match is active
				if (athlete.currentFieldTile == null) { 
					button.onClick.AddListener (() => matchManager.SelectAthlete (athlete));
				} else { //Athlete is on field
					bodyImg.color = Color.Lerp (bodyImg.color, Color.black, 0.7f);
					jerseyImg.color = Color.Lerp (jerseyImg.color, Color.black, 0.7f);

					button.onClick.AddListener (() => matchManager.RemoveAthleteFromField (athlete));
				}
			}
		} else {
			nameText.text = "";
			bodyImg.enabled = false;
			jerseyImg.enabled = false;
			positionText.text = "";
			descriptorPanel.SetActive (false);

			//athleteButton.onClick.AddListener (() => UndisplayAthletePanel ());

			button.interactable = false;
		}
	}
}
