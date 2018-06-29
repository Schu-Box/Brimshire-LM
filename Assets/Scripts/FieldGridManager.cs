using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldGridManager : MonoBehaviour {

	public List<List<FieldCellController>> fieldGridCells = new List<List<FieldCellController>> ();

	public Matchup currentMatch;

	private GameController gameController;

	void Awake() {
		gameController = FindObjectOfType<GameController> ();
	}

	public void SetFieldGridManager(Matchup match) {
		currentMatch = match;

		Debug.Log ("Setting Field Grid");

		fieldGridCells = new List<List<FieldCellController>> ();
		for (int i = 0; i < transform.childCount; i++) {
			fieldGridCells.Add (new List<FieldCellController> ());

			for(int j = 0; j < transform.GetChild(i).childCount; j++) {
				fieldGridCells[i].Add (transform.GetChild (i).GetChild (j).GetComponent<FieldCellController> ());
				fieldGridCells [i] [j].columnNum = i;
				fieldGridCells [i] [j].rowNum = j;
			}
		}
	}

	//Eventually need to create a function that takes the match's field data and generates the objects instead of relying on what's already there

	public void SetValidLineupPlacementCells(bool homeTeam) {
		int halfway = fieldGridCells.Count / 2;

		if (homeTeam) {
			for (int i = 0; i < halfway; i++) {
				for (int j = 0; j < fieldGridCells [i].Count; j++) {
					fieldGridCells [i][j].canSelect = true;
				}
			}
			for (int i = halfway; i < fieldGridCells.Count; i++) {
				for (int j = 0; j < fieldGridCells [i].Count; j++) {
					fieldGridCells [i] [j].canSelect = false;
				}
			}
		} else {
			for (int i = 0; i < halfway; i++) {
				for (int j = 0; j < fieldGridCells [i].Count; j++) {
					fieldGridCells [i][j].canSelect = false;
				}
			}
			for (int i = halfway; i < fieldGridCells.Count; i++) {
				for (int j = 0; j < fieldGridCells [i].Count; j++) {
					fieldGridCells [i] [j].canSelect = true;
				}
			}
		}
	}
		
	public void RemoveFog() {
		if (gameController.matchFog.activeSelf == true) {
			for (int i = 0; i < fieldGridCells.Count; i++) {
				for (int j = 0; j < fieldGridCells [i].Count; j++) {
					fieldGridCells [i][j].GetComponent<SpriteRenderer> ().sortingOrder = 1;
				}
			}

			gameController.matchFog.SetActive (false);
		}
	}

	public void ClearFieldCells() {
		Debug.Log ("Clearing Field");

		for(int i = 0; i < fieldGridCells.Count; i++) {
			for (int j = 0; j < fieldGridCells [i].Count; j++) {
				for (int c = fieldGridCells [i] [j].transform.childCount - 1; c >= 0; c--) {
					Destroy (fieldGridCells [i] [j].transform.GetChild (c).gameObject); //Destroys whatever gameObjects are held in each grid cell
				}
			}
		}
	}
		
	public FieldCellController GetFieldCellObject(int rowNum, int columnNum) {
		return fieldGridCells [rowNum] [columnNum];
	}
}
