using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TopUIManager : MonoBehaviour {

	public Vector3 topOffPosition;
	public Vector3 topOnPosition;

	public GameObject timelineArrow;

	public SchedulePiece preseasonPiece;
	public List<SchedulePiece> regularSeasonPieces;
	public SchedulePiece postseasonPiece;

	//private GameController gameController;
	private TeamController team;

	public void HideTeamScheduleBeforeTeamSelection() {
		for(int i = 0; i < regularSeasonPieces.Count; i++) {
			regularSeasonPieces[i].gameObject.SetActive(false);
		}
		postseasonPiece.gameObject.SetActive(false);
	}

	public void SetTeamSchedule(TeamController t) {
		//gameController = FindObjectOfType<GameController>();
		team = t;

		//Assumes that seasonMatchups == regularSeasonPieces.Count
		for (int i = 0; i < team.seasonMatchups.Count; i++) {
			regularSeasonPieces[i].gameObject.SetActive(true);

			Matchup matchForWeek = team.seasonMatchups[i];
			TeamController opponentForWeek = null;

			if(matchForWeek != null) {
				if(team == matchForWeek.homeTeam) {
					opponentForWeek = matchForWeek.awayTeam;
				} else {
					opponentForWeek = matchForWeek.homeTeam;
				}
			}

			int weekInt = i;
			regularSeasonPieces[i].SetSchedulePiece(opponentForWeek, weekInt);
		}

		postseasonPiece.gameObject.SetActive(true);
	}

	public IEnumerator UpdateForNewWeek() {

		Vector3 timelineArrowStart = timelineArrow.transform.localPosition;
		Vector3 timelineArrowEnd;
		if(GameController.week < regularSeasonPieces.Count) {
			timelineArrowEnd = regularSeasonPieces[GameController.week].transform.localPosition;
		} else {
			Debug.Log("It's probably the postseason.");
			timelineArrowEnd = postseasonPiece.transform.localPosition;
		}
		
		WaitForFixedUpdate waiter = new WaitForFixedUpdate();
		float timer = 0f;
		float duration = 0.7f;
		while(timer < duration) {
			timer += Time.deltaTime;

			float step = timer/duration;

			timelineArrow.transform.localPosition = Vector3.Lerp(timelineArrowStart, timelineArrowEnd, step);

			yield return waiter;
		}
	}
}
