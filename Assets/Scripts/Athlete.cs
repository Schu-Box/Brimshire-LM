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
		" the Charming", " of the People", ", ya boi", ", Destroyer", " the Best", " the Bigly", " the Big Baller", " the Royal", " the Regal", " the Regent",
		" the Ruler", " the Tactician", " the Athlete", " the Savage", " the Cunning", " the Mastermind", " the Gamesman", " One-Limb", " Two-Nosed", " the Blind",
		" the Seer", ", Chooken Chaser", " the Prick", " the Insufferable", " Stooper", " Viceroy"
	};

	private List<string> positionNames = new List<string> {
		"Striker", "Defender", "Protector", "Guardian", "Winger", "Midfielder", "Playmaker", "Boomer", "Bruiser", "Builder", "Medic", "Center", "Wildcard",
		"Rookie", "Disruptor", "Charger", "Lunatic", "Student", "Scout"
	};

	private Lexic.NameGenerator nameGen = GameObject.FindGameObjectWithTag("NameGenerator").GetComponent<Lexic.NameGenerator>();
	private GameController gameController = GameObject.FindObjectOfType<GameController>();

	public int age = 0;
	public Race race;
	public string name;
	public string originCountyName;

	public bool manager;
	public bool starting = false;

	public List<Attribute> attributeList = new List<Attribute> ();
	public List<Statistic> statisticList = new List<Statistic> ();
	public string positionName;
	public int level = 1;

	private TeamController team;

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
		age += Random.Range (6, 12);
		originCountyName = gameController.countyObjects [Random.Range (0, gameController.countyObjects.Count)].GetComponent<CountyController> ().countyName;

		attributeList.Add (new Attribute ("Strength")); //Things like punches
		attributeList.Add (new Attribute ("Speed")); //Things like movement time
		attributeList.Add (new Attribute ("Reflex")); //Things like saving the ball and turn order 
		attributeList.Add (new Attribute ("Accuracy")); //Their likelihood to score and pass
		attributeList.Add (new Attribute ("Ball Control")); //Securing the ball, preventing steals
		attributeList.Add (new Attribute ("Resilience")); //Health, how likely they are to be pushed
		attributeList.Add (new Attribute ("On Ball Defense")); //Attempting to make a play on the ball
		attributeList.Add (new Attribute ("Off Ball Defense")); //Marking an opponent, building defenses
		attributeList.Add (new Attribute ("Athleticism")); //Vertical and has slight impact on everything
		//Strategy? Awareness?

		for (int i = 0; i < attributeList.Count; i++) {
			attributeList [i].value = Random.Range (0, 2);
		}

		statisticList.Add (new Statistic ("Seasons Played"));
		statisticList.Add (new Statistic ("Matches Played"));
		statisticList.Add (new Statistic ("Wins"));
		statisticList.Add (new Statistic ("Goals"));
		statisticList.Add (new Statistic ("Assists"));
		statisticList.Add (new Statistic ("Boards"));
		statisticList.Add (new Statistic ("Saves"));
		statisticList.Add (new Statistic ("Steals"));
		statisticList.Add (new Statistic ("Shoves"));

		//positionName = positionNames [Random.Range (0, positionNames.Count)];

	}

	public TeamController GetTeam() {
		return team;
	}

	public void SetTeam(TeamController newTeam) {
		team = newTeam;
	}



	//Everything below here should eventually be removed
	public AthleteUI athUI; //I dunno if I should have this or nah

	public bool onActiveRoster = false;

	//Could consider creating an AthleteState obj
	public int fieldPosition; //0 is home team's goal, 1 is home's midfield, 2 is away's midfield, 3 is away team's goal
	public GameObject fieldUI;

	public List<Opportunity> availableOpportunities = new List<Opportunity>();
	public bool defending;
	public bool goalkeeping;
	public bool stealing;

	//Below here are the match stats for the athlete:
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
}

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

/*
public class Position {
	public string positionName;

	public Position(string name) {
		positionName = name;
	}
}
*/

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
