using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamController : MonoBehaviour {

	//[HideInInspector] public bool playerControlled = false;

	public int gold = 0;
	public int prestige = 0;

	public Sprite logoPrimarySprite;
	public Sprite logoSecondarySprite;

	public string teamName;
	public bool thePronoun = true;
	public string teamAbbreviation;
	public int yearEstablished = 3000;
	[TextArea(1, 16)] public string teamDescription;

	private int startingRosterCount = 3;

	public Owner owner;

	//Should consider using get/set methods eventually
	[HideInInspector] public Color teamColor;
	[HideInInspector] public int rosterMax = 10;
	[HideInInspector] public List<Athlete> rosterList = new List<Athlete> ();
	[HideInInspector] public Athlete teamManager;
	[HideInInspector] public List<Matchup> seasonMatchups = new List<Matchup>();
	[HideInInspector] public TeamController opponentThisWeek; //Note that this is assigned during the setup phase of each match
	[HideInInspector] public int winsThisSeason = 0;
	[HideInInspector] public int losesThisSeason = 0;

	public League league;

	private CityController city;
	private Vector2 cityLocation;

	private GameController gameController;
	private SpriteRenderer spriteRenderer;

	private bool drafting = false;

	//Everything below here is up for review

	private DraftOrderUI draftOrderUI;

	/*
	private List<string> nameList = new List<string> {
		"Taco", "Yimmy", "Yolga", "Gerkin", "Leady", "Fif", "Rox", "Bavid", "Piars", "Durga", "Walgerg", "Schumy", "Hollow", "Blip", "Croft", "Yaegar",
		"Venus", "Nexus", "Nellie", "Korgol", "Cart", "Maxxie", "Quinn", "Olof", "Lief", "Rainny", "Mildson", "Midlunder", "Pippy", "Trev", "Trogdor", "Vance",
		"Smelly", "Shorty", "Longard", "Welsby", "Visby", "Frito", "Dorina", "Uggs", "Daimen", "Gary", "Yomir", "Boromir", "Mirmir", "Flex", "Lil Bud", "Jordan",
		"Goaty", "Kaliko", "Koding", "Gip", "Fi", "Vi", "Mo", "Godhem", "Underbri", "Hoxby", "Tenpence", "Bikboy", "Stunna", "Balair", "Barth", "Hogar", "Phuck",
		"Hazel", "Goodie", "Bayoneta", "Bavmorda", "Tommy", "Edey", "Rufus", "Shadow", "Sunny", "Bro", "Rito", "Hans", "Winthrup", "Birby", "Aicho"
	};
	*/

	public void InitializeTeam(CityController cityBelongingTo, bool primaryCityTeam) {
		SetCity (cityBelongingTo);

		gameController = FindObjectOfType<GameController> ();
		spriteRenderer = GetComponent<SpriteRenderer> ();
		teamColor = spriteRenderer.color;
		spriteRenderer.enabled = false;

		gameObject.name = teamName;
		transform.localPosition = Vector2.zero;
		InitializeRoster ();

		//Will need to modify this if teams can start in the primary league without being the capital.
		if (GetCity ().GetCounty ().GetCapitalCity () == GetCity () && primaryCityTeam) {
			gameController.brimshireLeague.teamList.Add (this); //Add this team to the primary league
			league = gameController.brimshireLeague;
		} else {
			if(cityBelongingTo.GetTeamOfCity() == this) { //If they're the only team in the city. (Additional teams are only used for the tutorial for now)
				GetCity ().GetCounty ().countyLeague.teamList.Add (this);
			}
			league = GetCity ().GetCounty ().countyLeague;
		}

		owner.SetTeam (this);
		owner.SetMessages ();
	}

	public void InitializeRoster() {

		for(int i = 0; i < startingRosterCount; i++) {
			rosterList.Add (new Athlete(false));
			rosterList [i].SetTeam (this);
		}

		teamManager = new Athlete (true);
		teamManager.SetTeam (this);
	}

	public void AddAthleteToRoster(Athlete athlete) {
		if (rosterList.Count >= rosterMax) {
			Debug.Log ("Can't exceed maximum roster list.");
		} else {
			rosterList.Add (athlete);
			athlete.SetTeam (this);
			//athlete.onActiveRoster = false;
		}
	}

	public void SetCity(CityController cityBelongingTo) {
		city = cityBelongingTo;
		cityLocation = city.transform.position;
	}

	public CityController GetCity() {
		return city;
	}

	public Vector2 GetCityLocation() {
		return cityLocation;
	}

	public void SetDrafting(bool d) { 
		drafting = d;
	}

	public bool GetDrafting() {
		return drafting;
	}

	public void SetDraftOrderUI(DraftOrderUI d) {
		draftOrderUI = d;
	}

	public DraftOrderUI GetDraftOrderUI() {
		return draftOrderUI;
	}
}
