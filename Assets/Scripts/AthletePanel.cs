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

	private GameController gameController;

	public void Awake() {
		gameController = FindObjectOfType<GameController> ();

		nameText = transform.GetChild (0).GetComponent<Text> ();
		bodyImg = transform.GetChild (1).GetComponent<Image> ();
		jerseyImg = transform.GetChild (2).GetComponent<Image> ();
		positionText = transform.GetChild (3).GetComponent<Text> ();
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

			if (!athlete.onField) { 
				button.onClick.AddListener (() => gameController.SelectAthlete (athlete));
			} else {
				bodyImg.color = Color.Lerp (bodyImg.color, Color.black, 0.7f);
				jerseyImg.color = Color.Lerp (jerseyImg.color, Color.black, 0.7f);
			}
		} else {
			nameText.text = "";
			bodyImg.enabled = false;
			jerseyImg.enabled = false;
			positionText.text = "";
		}
	}
}
