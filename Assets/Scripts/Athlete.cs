using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Athlete {

	//Should be part of LexicNameGenerator
	private List<string> surnameList = new List<string> {
		" the Old", " the Brave", " the Crafty", " the Swift", " the Decisive", " the Incompetent", " the Ugly", " the Winner", " the Champion", " the Brain",
		" the Wise", " the Maverick", " the Bad", " the Good", " the Pro", " the Baller", " the Cheap", " the Sweaty", " the Strong", " the Manager", " the Rapper",
		" the Doc", " the Quick", " the Scandalous", " the Tiger", " the Warrior", " the Boss", " the Squire", " the Prodigy", " the Chosen", " the Drunkard", 
		" the High", " the Honorable", " the Fair", " the Fat", " the Wizard", " the Monotone", " the Studious", " the Bold", " the Pariah",
		" the Hero", " the Silent", " the Maximizer", " the Genius", " the Funky", " the Serious", " the Stern", " the Furious", " the Brutal", " the Cheater",
		" the Artist", " the Inheritor", " the Wildcat", " the Contrarian", " of Old School", " the Legend", " the Glorious", " the Beautiful",
		" the Charming", " of the People", ", ya boi", ", ya gurl", ", Destroyer", " the Best", " the Bigly", " the Big Baller", " the Royal", " the Regal", " the Regent",
		" the Ruler", " the Tactician", " the Athlete", " the Savage", " the Cunning", " the Mastermind", " the Gamesman", " One-Limb", " Two-Nosed", " the Blind",
		" the Seer", ", Chooken Chaser", " the Jerk", " the Insufferable", " Stooper", " Viceroy", " the Bully", " the Slim"
	};

	/*
	private List<string> positionNames = new List<string> {
		"Striker", "Defender", "Protector", "Guardian", "Winger", "Midfielder", "Playmaker", "Boomer", "Bruiser", "Builder", "Medic", "Center", "Wildcard",
		"Rookie", "Disruptor", "Charger", "Lunatic", "Student", "Scout"
	};
	*/

	private Lexic.NameGenerator nameGen = GameObject.FindGameObjectWithTag("NameGenerator").GetComponent<Lexic.NameGenerator>();
	private GameController gameController = GameObject.FindObjectOfType<GameController>();

	public bool manager;

	public int age = 0;
	public Race race;
	public string name;
	public string originCountyName;

	public int seasonsPlayed = 0;
	public int seasonsWithTeam = 0;

	public Sprite bodySprite;
	public Sprite jerseySprite;

	public int overallRating = 0;

	public List<Attribute> attributeList = new List<Attribute> ();
	public List<Statistic> statisticList = new List<Statistic> ();
	public string positionName;
	public int level = 1;

	private TeamController team;

	//On Field Data
	public List<Opportunity> availableOpportunityList = new List<Opportunity>();
	//public bool onField = false;
	//public bool performingAction = false;
	public GameObject athleteOnFieldObject;
	public AthleteMatchPanel athleteMatchPanel;
	public FieldTile originalLineupTile;
	public FieldTile currentFieldTile;
	public Action activeAction;
	/*
	public int rowNum = -1;
	public int columnNum = -1;
	*/
	public Ball heldBall = null;



	public Athlete(bool isManager) {

		name = nameGen.GetNextRandomName ();

		if (isManager) {
			manager = true;

			name += surnameList [Random.Range (0, surnameList.Count)];

			age += 3;

			positionName = "Level " + level + " Manager";
		} else {
			manager = false;

			positionName = "Level " + level + " Rookie";
		}
			
		race = gameController.races [Random.Range (0, gameController.races.Count)]; //Should be tailored carefully either in Race script or here with randos
		bodySprite = race.raceSprite;
		jerseySprite = race.raceJersey;

		age += Random.Range (6, 12);
		originCountyName = gameController.countyObjects [Random.Range (0, gameController.countyObjects.Count)].GetComponent<CountyController> ().countyName;

		attributeList.Add(new Attribute ("Speed")); //Movement time
		attributeList.Add(new Attribute ("Strength")); //How far they can hit the ball, any offensive shoving actions
		attributeList.Add(new Attribute ("Ball Control")); //Securing the ball, preventing steals, shot/pass accuracy
		attributeList.Add(new Attribute("Defense")); //Any defensive actions
		/*
		attributeList.Add (new Attribute ("Resilience")); //Health, how likely they are to be pushed
		//Strategy? Awareness?

		attributeList.Add (new Attribute ("Athleticism")); //Vertical and has slight impact on everything
		*/

		for (int i = 0; i < attributeList.Count; i++) {
			attributeList [i].value = Random.Range (1, 21);

			overallRating += attributeList[i].value;
		}

		overallRating = (int)(overallRating/attributeList.Count);

		//statisticList.Add (new Statistic ("Seasons Played"));
		statisticList.Add (new Statistic ("Matches Played"));
		statisticList.Add (new Statistic ("Wins"));
		statisticList.Add (new Statistic ("Match MVPs"));
		statisticList.Add (new Statistic ("Goals"));
		statisticList.Add (new Statistic ("Assists"));
		statisticList.Add (new Statistic ("Boards"));
		statisticList.Add (new Statistic ("Saves"));
		statisticList.Add (new Statistic ("Steals"));
		statisticList.Add (new Statistic ("Shoves"));

		//positionName = positionNames [Random.Range (0, positionNames.Count)];
	}

	public int GetAttributeValue(string id) {
		int v = -1;
		for (int i = 0; i < attributeList.Count; i++) {
			if (attributeList [i].attributeName.ToLower() == id.ToLower()) {
				v = attributeList [i].value;
			}
		}

		if (v == -1) {
			Debug.Log ("Attribute " + id + " doesn't exist and you a fool.");
		}
		return v;
	}

	public TeamController GetTeam() {
		return team;
	}

	public void SetTeam(TeamController newTeam) {
		team = newTeam;
	}



	//Everything below here should eventually be removed
	/*
	public List<Opportunity> availableOpportunities = new List<Opportunity>();

	public bool defending;
	public bool goalkeeping;
	public bool stealing;
	*/

	//Below here are the match stats for the athlete:
	/*
	public int seasonsPlayed = 0;
	public int matchesPlayed = 0;
	public int wins = 0;
	public int losses = 0;

	public int scores = 0;
	public int assists = 0;
	public int rebounds = 0; //boards
	public int tackles = 0;
	public int steals = 0;
	public int saves = 0;
	public int turnovers = 0;
	public int misses = 0;
	*/
}

[System.Serializable]
public class Race {
	public string raceName;
	public Sprite raceSprite;
	public Sprite raceJersey;

	public Race(string name, Sprite img, Sprite jersey) {
		raceName = name;
		raceSprite = img;
		raceJersey = jersey;
	}
}

public class Attribute {
	public string attributeName;

	public int value = 0;

	public Attribute(string name) {
		attributeName = name;
	}
}

public class Statistic {
	public string statName;

	public int value;

	public Statistic(string name) {
		statName = name;
	}
}
