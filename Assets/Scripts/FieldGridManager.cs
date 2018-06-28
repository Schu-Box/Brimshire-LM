using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldGridManager : MonoBehaviour {

	public List<GameObject> fieldGridCells;

	private GameController gameController;

	void Start() {
		gameController = FindObjectOfType<GameController> ();
	}

	public void SetLineupPlacementCells(bool homeTeam) {
		Debug.Log ("Setting");

		fieldGridCells = new List<GameObject> ();
		for (int i = 0; i < transform.childCount; i++) {
			int num = i;
			fieldGridCells.Add (transform.GetChild (num).gameObject);
		}

		int halfway = fieldGridCells.Count / 2;

		Debug.Log (halfway);

		if (homeTeam) {
			for (int i = 0; i < halfway; i++) {
				fieldGridCells [i].GetComponent<FieldCellController> ().canSelect = true;
			}
			for (int i = halfway; i < fieldGridCells.Count; i++) {
				fieldGridCells [i].GetComponent<FieldCellController> ().canSelect = false;
			}
		} else {
			for (int i = 0; i < halfway; i++) {
				fieldGridCells [i].GetComponent<FieldCellController> ().canSelect = false;
			}
			for (int i = halfway; i < fieldGridCells.Count; i++) {
				fieldGridCells [i].GetComponent<FieldCellController> ().canSelect = true;
			}
		}
	}
		
	public void RemoveFog() {
		if (gameController.matchFog.activeSelf == true) {
			for (int i = 0; i < fieldGridCells.Count; i++) {
				fieldGridCells [i].GetComponent<SpriteRenderer> ().sortingOrder = 1;
			}

			gameController.matchFog.SetActive (false);
		}
	}

	public void ClearFieldCells() {
		for(int i = 0; i < fieldGridCells.Count; i++) {
			for (int j = fieldGridCells[i].transform.childCount - 1; j >= 0; j--) {
				Destroy (fieldGridCells [i].transform.GetChild (j).gameObject);
			}
		}
	}
}
