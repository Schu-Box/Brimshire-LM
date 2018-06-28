using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CityController : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {

	public string cityName;
	public int cityPopulation = 100;

	public Vector3 cityPanelHoverPos;
	public List<GameObject> teamObjects = new List<GameObject> ();

	private GameController gameController;
	private CountyController county;
	private TeamController currentCityTeam;

	public void InitializeCity(CountyController countyBelongingTo) {
		SetCounty (countyBelongingTo);

		gameController = FindObjectOfType<GameController> ();

		SetTeamOfCity (teamObjects [0].GetComponent<TeamController> ());

		for (int i = 0; i < teamObjects.Count; i++) {
			teamObjects [i].GetComponent<TeamController> ().InitializeTeam (this);
		}

		GetComponent<SpriteRenderer> ().enabled = false;
		GetComponent<Collider2D> ().enabled = false;
	}

	#region IPointerClickHandler implementation
	public void OnPointerClick (PointerEventData eventData) {
		if (GameController.canInteractWithMap && GetComponent<SpriteRenderer>().enabled == true) {
			gameController.DisplayTeamDetailPanel (GetTeamOfCity ());


		}
	}
	#endregion

	#region IPointerEnterHandler implementation
	public void OnPointerEnter (PointerEventData eventData) {
		if (GameController.canInteractWithMap && GetComponent<SpriteRenderer>().enabled == true) {
			currentCityTeam.gameObject.GetComponent<SpriteRenderer> ().enabled = true;

			gameController.DisplayCityPanel (this);
		}
	}
	#endregion

	#region IPointerExitHandler implementation
	public void OnPointerExit (PointerEventData eventData) {
		if (GameController.canInteractWithMap && GetComponent<SpriteRenderer>().enabled == true) {
			currentCityTeam.gameObject.GetComponent<SpriteRenderer> ().enabled = false;

			gameController.cityPanel.SetActive (false);
		}
	}
	#endregion

	public void SetCounty(CountyController countyBelongingTo) {
		county = countyBelongingTo;
	}

	public CountyController GetCounty() {
		return county;
	}

	public void SetTeamOfCity(TeamController team) {
		currentCityTeam = team;
	}

	public TeamController GetTeamOfCity() {
		return currentCityTeam;
	}

	public int GetCityPopulation() {
		return cityPopulation;
	}
}
