using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CountyController : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {

	public string countyName;
	public int countyPopulationOutsideCities = 100; //These are citizens living outside of cities but still within the county

	public float countyZoomSize = 2.5f;
	public Vector3 countyCamFocalPos;
	public Vector3 countyHoverPos;
	public List<GameObject> cityObjects = new List<GameObject> ();
	[HideInInspector] public Color countyColor;

	public League countyLeague;

	private SpriteRenderer spriteRenderer;
	private GameController gameController;
	private CityController currentCapitalCity;

	/*
	private int totalPopulation = 0;
	private int totalPrestige = 0;
	private int totalGold = 0;
	*/

	public bool hoverable = true;

	public void InitializeCounty() {

		countyLeague = new League (new List<TeamController> ());

		SetCapitalCity (cityObjects [0].GetComponent<CityController> ()); //Make the first city on the list the capital by default

		for (int i = 0; i < cityObjects.Count; i++) {
			cityObjects [i].GetComponent<CityController> ().InitializeCity (this);
		}

		countyLeague.SetSeason ();

		spriteRenderer = GetComponent<SpriteRenderer> ();
		countyColor = spriteRenderer.color;
		gameController = FindObjectOfType<GameController> ();
	}

	#region IPointerClickHandler implementation
	public void OnPointerClick (PointerEventData eventData) {
		if (GameController.canInteractWithMap) {
			/*
			if (GameController.displayTeam != null) {
				GameController.displayTeam.gameObject.GetComponent<SpriteRenderer> ().enabled = false;
			}
			*/

			gameController.FocusCounty (this);
		}
	}
	#endregion


	#region IPointerEnterHandler implementation
	public void OnPointerEnter (PointerEventData eventData) {
		if (GameController.canInteractWithMap) {
			
			if (hoverable) {
				gameController.DisplayCountyPanel (this);
			}

			spriteRenderer.sortingLayerName = "Focal County";

			gameController.fogLayer.SetActive (true);
		}
	}
	#endregion

	#region IPointerExitHandler implementation
	public void OnPointerExit (PointerEventData eventData) {
		if (GameController.canInteractWithMap) {
			
			if (hoverable) {
				gameController.UndisplayCountyPanel ();
			}

			if (GameController.focusedCounty != this) { //Will not revert layers if the current county is the one that has been clicked on to focus
				if (GameController.focusedCounty == null) { //If no county has been clicked on then the fog will deactivate upon exit
					gameController.fogLayer.SetActive (false);
				}

				spriteRenderer.sortingLayerName = "World";
			}
		}
	}
	#endregion

	public void SetCapitalCity(CityController city) {
		currentCapitalCity = city;
	}

	public CityController GetCapitalCity() {
		return currentCapitalCity;
	}

	public int GetTotalPopulation() {
		int total = countyPopulationOutsideCities;

		for (int i = 0; i < cityObjects.Count; i++) {
			total += cityObjects [i].GetComponent<CityController> ().cityPopulation;
		}

		return total;
	}

	public void IncreaseCountyPopulationByWeek() {
		//In this format, the increase is static across the county
		int rando = Random.Range (0, 10);
		float increasePercent = (float)rando / 150f;

		countyPopulationOutsideCities += (int)(countyPopulationOutsideCities * increasePercent);

		//Eventually this should be a function on CityController to check for factors like gold
		for (int i = 0; i < cityObjects.Count; i++) {
			cityObjects [i].GetComponent<CityController> ().cityPopulation += (int)(cityObjects [i].GetComponent<CityController> ().cityPopulation * increasePercent);
		}
	}

	public int GetTotalPrestige() {
		int total = 0;

		for (int i = 0; i < cityObjects.Count; i++) {
			total += cityObjects [i].GetComponent<CityController> ().GetTeamOfCity ().prestige;
		}

		return total;
	}

	public int GetTotalGold() {
		//return totalGold;

		int total = 0;

		for (int i = 0; i < cityObjects.Count; i++) {
			total += cityObjects [i].GetComponent<CityController> ().GetTeamOfCity ().gold;
		}

		return total;
	}
}
