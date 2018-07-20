using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Owner {
	public string name = "Dogger Gladwell";

	//What they care about
	//0 = Don't Care; 1 = Somewhat Care; 2 = Very Much Care;
	/*
	public int careWins = 1;
	public int carePrestige = 1;
	public int careGold = 1;
	//The stats below are unused so far
	public int careStars = 1;
	public int careDevelop = 1;
	public int careStats = 1;
	public int careFans = 1;
	*/

	public List<OwnerValue> ownerValues = new List<OwnerValue>();

	public bool givesPraise = true;
	public bool rude = false;
	public bool patient = false;
	public bool commoner = false;
	public bool watchesEveryGame = true;
	public bool understandsSports = false;

	public List<Objective> expectationsList = new List<Objective> ();
	public List<Objective> bonusObjectivesList = new List<Objective> ();

	private List<string> welcomeMessages = new List<string> ();
	private List<string> regularSeasonWinMessages = new List<string> ();
	private List<string> regularSeasonLossMessages = new List<string>();
	private List<string> regularSeasonByeMessages = new List<string>();

	private TeamController team;
	private string teamString = "";

	public void SetTeam(TeamController t) {
		team = t;
	}

	public void SetMessages() {

		if (team.thePronoun) {
			teamString = "the ";
		}
		teamString += team.teamName;

		regularSeasonWinMessages.Add ("Well done, " + team.teamManager.name + ". Victories like that are exactly what the people of " + team.GetCity ().cityName + " need. Keep it up.");
		regularSeasonWinMessages.Add ("That was quite the performance! Give my regards to our outstanding athletes and let them know that their hard work is appreciated.");
		regularSeasonWinMessages.Add ("There's hardly anything I can complain about after a victory like that. Clearly what you're doing is working and I expect that to continue throughout the season.");

		regularSeasonLossMessages.Add ("I expect better from you and our athletes. I understand we can't win every match but we certainly should have won this week. I'm not mad, I'm just disappointed in you.");
		regularSeasonLossMessages.Add ("You can do better. I know you can. I don't know enough to tell you what went wrong but I'm counting on you to figure it out. It is your job, after all. But I'll let you get to it.");

		if (givesPraise) {
			welcomeMessages.Add ("Welcome to " + teamString + "! We're extremely excited to have you on board for the inaugural season of the Brimshire League. We looked " +
			"at candidates from all across Brimshire for the managerial position and you were by far the most impressive. We're confident that you've got the skills to lead " +
			"us to the championship eventually. But of course you'll need plenty of time and resources to put together a powerful and cohesive squad. I've included " +
			"some reasonable expectations that you should be able to meet for the end of the season. We're looking forward to helping you build a championship roster.");

			regularSeasonWinMessages.Add ("Excellent work this week. I'm very impressed with both your performance and that of our athletes. Keep it up, coach.");
			regularSeasonWinMessages.Add ("That was some victory! Truly a brilliant performance from our squad. Your leadership is clearly vital to the success of this team. Thank you for your efforts.");
			regularSeasonWinMessages.Add ("You and our athletes make me so proud. Clearly I made the right decision hiring you. Enjoy the victory celebrations! I look forward to seeing our next match!");

			regularSeasonLossMessages.Add ("What a rough loss. It's clear that you're doing your best and I appreicate that. A couple of better plays from our athletes and we would've won that one. Keep up the great work.");
		}

		if (rude) {
			welcomeMessages.Add ("It's been 10 full years since the last Brimshire League match. That was probably before you were born, wasn't it? " +
			"Well let's just say things ended badly last time. Lots of people died and all that. Anyways welcome to " + teamString + ". You were the best candidate " +
			"available at the moment but don't get too comfortable. If you don't meet my expectations then you'll be looking for a position with some puny city " +
			"league team. Don't let that stress you out though. I'm sure you'll do just fine. You'd better. I'll be watching.");
			welcomeMessages.Add ("Let's get something clear. I've laid out a few expectations for this season. If you don't meet those then I'll find somebody else " +
			"who can. Your job isn't to listen to the fans or to the athletes. You listen to me, got it? Anyways we're really looking forward to having you on " +
			"board. This is the grand reopening of the Brimshire League and we believe our team is in position to become a true dynasty. Figure out which of our " +
			"athletes are keepers and get rid of the bad apples. Looking forward to seeing how you manage this roster.");


			regularSeasonWinMessages.Add ("You keep winning like that and I might just have to give you a raise! Haha that was a joke. But seriously that's the type of performance I expect from our team every week.");
			regularSeasonWinMessages.Add ("What a team I've assembled! What do I even pay you for? Surely with athletes like this they don't even need a manager! Nevertheless you've got the job and you're executing admirably. Stay the course.");
			regularSeasonWinMessages.Add ("What an ugly victory. Surely we could've doubled their score? I won't settle for simply defeating our opponents. I want to absolutely embarass them. Teams should tremble in fear when they see us walk out onto the pitch.");

			regularSeasonLossMessages.Add ("What was that!? I expect better from you. Keep losing like that and I'll have to deal with the smug faces of all the other owners at our next meeting. Don't make me suffer like that. Get back to winning.");
			regularSeasonLossMessages.Add ("If that's the type of performance we can expect from our athletes then it's time to ship them out and find some players with true passion that know how to win. If you don't make some changes then I'll find someone who will.");
			regularSeasonLossMessages.Add ("Are you serious? You expect me to believe that's the best our team can do? Perhaps you're being too soft on them. I need you to inspire them to victory and I don't care how you do it. Threaten their roles, salaries, or families if you must. Just get it done.");
		}

		if (patient) {
			regularSeasonWinMessages.Add ("That was a well fought win. You and our athletes deserve to celebrate. But don't let one little victory go to your head. Take some time to rest up, get the team ready, and do it all again next week. We're competing for something grander.");

			regularSeasonLossMessages.Add ("What a heart breaking loss. Surely we'll bounce back! I have complete faith in your abilities, coach.");
			regularSeasonLossMessages.Add ("Ahhh that was a rough one. No worries, though. I look forward to seeing how you respond next week.");
		}

		if (!commoner) {
			regularSeasonWinMessages.Add ("I'll be pouring a tall glass of victory champaign tonight! Someday maybe I'll invite you to join me. Perhaps after you bring our fine city a championship banner. But until that day just keep doing what you're doing. Fantastic work.");
			regularSeasonWinMessages.Add ("I sure do love winning! Keep racking up the Ws and I'll keep racking up the gold! Maybe some of that trickles down to you, eh?");

			regularSeasonLossMessages.Add ("Oh well. I've got much more important things to worry about at the moment. I'd appreciate it if you tried harder to win. That is your job though. I shouldn't have to tell you. Surely you'll make some changes and get us back on track.");
		} else {
			welcomeMessages.Add ("Welcome to the " + teamString + "! We're really looking forward to working with you throughout the season. " +
			"Unlike most of the other owners, I wasn't born into wealth so I understand the work that goes into building something from the ground up, which is what " +
			"we're doing here. The Brimshire League has been reestablished after a 10-year hiatus and we're hoping to make a lot of changes to the league structure. " +
			"But don't worry about that right now. I've laid out some reasonable expectations for you that you should take a look at. Focus on enabling our athletes to " +
			"reach their full potential and feel free to ship off those that don't fit with your vision.");

			regularSeasonWinMessages.Add ("Let's go, baby! All day! That's how you do it! I'll be celebrating in the streets of " + team.GetCity ().cityName + " with the townsfolk tonight!");
			regularSeasonWinMessages.Add ("With victoris lie that surly well win the championship!!!!!! Bit drunk at the moment. Talkt o you later. Good jog");

			regularSeasonLossMessages.Add ("Losing is a part of life. The other owners thought a commoner such as myself couldn't compete at this level but here I am. And here you are with me. It's no big deal we lost this week. I trust and believe in you, coach.");
		}

		if (!watchesEveryGame) {
			regularSeasonWinMessages.Add ("I didn't get a chance to catch the match last night but I hear it was quite a stunner. I'm sure you did an excellent job.");

			regularSeasonLossMessages.Add ("I'm told we lost last night. Why is that? It's my preference that we win every game, if possible. Next game I suppose I'll have to watch to see what you're messing up.");
		
			regularSeasonByeMessages.Add ("Did we not win this week? I didn't attend the match and I'm looking in the papers for the score but I can't seem to find anything about it. One of my staff " +
			"might just get fired for this. I need to know how we did. It went well, didn't it?");
		}

		if (!understandsSports) {
			regularSeasonWinMessages.Add ("Brilliant performance! You told the athletes to score more this week, didn't you? Or did you tell them to get scored on less? Either way, whatever you're doing is working. Maybe next week you can tell them to score even more!");

			regularSeasonLossMessages.Add ("Oh dear. Surely that isn't the type of performance I should expect from the squad on a regular basis? Isn't there some coaching or something that you can do? Can you tell our athletes to try scoring more? Regardless, I'm sure you'll get everyone back on track.");
		}

		regularSeasonByeMessages.Add ("We didn't have a match this week so I've got nothing to say. Hopefully you're keeping our athletes fresh and ready to roll.");
		regularSeasonByeMessages.Add ("We had a bye this week so I don't have much to say. Just be sure to prepare our athletes for their next match.");
	}

	public int GetOwnerValueValue(string id) {
		for (int i = 0; i < ownerValues.Count; i++) {
			if (ownerValues [i].valueID == id) {
				return ownerValues [i].valueValue;
			}
		}

		Debug.Log ("ID: " + id + " doesn't exist. You stoopid.");
		return -1;
	}

	public void AssignExpectations(Athlete athlete) {
		expectationsList = new List<Objective> ();
		bonusObjectivesList = new List<Objective> ();

		List<Objective> potentialExpectationsList = new List<Objective> ();
		List<Objective> potentialBonusObjectivesList = new List<Objective> ();

		int careGold = GetOwnerValueValue ("Gold");
		if (careGold > 0) {
			int goldNeeded = athlete.GetTeam().gold; //The amount of gold the team currently has
			int extraGold = Random.Range (10, 51) * 5; //Between 50 and 250 additional gold, multiples of 5
			goldNeeded += extraGold;
			int rewardGold = extraGold / 10; //A tenth of the extra gold will be rewarded to the manager
			Objective goldObjective = new Objective (athlete, "gold", goldNeeded, "gold", rewardGold);


			int highGoldNeeded = athlete.GetTeam ().gold; //The amount of gold the team currently has
			int highExtraGold = Random.Range (50, 101) * 5; //Between 250 and 500 additional gold, multiples of 5
			highGoldNeeded += highExtraGold;
			int highRewardGold = highExtraGold / 10; //A tenth of the extra gold will be rewarded to the manager
			Objective highGoldObjective = new Objective (athlete, "gold", highGoldNeeded, "gold", highRewardGold);

			if (careGold >= 2) {
				potentialExpectationsList.Add (goldObjective);
				potentialBonusObjectivesList.Add (highGoldObjective);
			} else if (careGold == 1) {
				potentialBonusObjectivesList.Add (goldObjective);
			}
		}

		int careWins = GetOwnerValueValue ("Wins");
		if (careWins > 0) {
			int winsNeeded = Random.Range (4, 7);
			int rewardGold = winsNeeded * 5;
			Objective winObjective = new Objective (athlete, "win", winsNeeded, "gold", rewardGold);

			int highWinsNeeded = Random.Range (7, 9);
			int highRewardGold = highWinsNeeded * 5;
			Objective highWinObjective = new Objective (athlete, "win", highWinsNeeded, "gold", highRewardGold);

			if (careWins >= 2) {
				potentialExpectationsList.Add (winObjective);
				potentialBonusObjectivesList.Add (highWinObjective);
			} else if (careWins == 1) {
				potentialBonusObjectivesList.Add (winObjective);
			}
		}

		int carePrestige = GetOwnerValueValue ("Prestige");
		if (carePrestige > 0) {
			int prestigeNeeded = Random.Range (10, 21) * 10; //Between 100 and 200, multiples of 10
			int rewardGold = prestigeNeeded / 5;
			Objective prestigeObjective = new Objective (athlete, "prestige", prestigeNeeded, "gold", rewardGold);

			int highPrestigeNeeded = Random.Range (20, 40) * 10; //Between 200 and 400, multiples of 10
			int highRewardGold = highPrestigeNeeded / 5;
			Objective highPrestigeObjective = new Objective (athlete, "prestige", highPrestigeNeeded, "gold", highRewardGold);

			if (carePrestige >= 2) {
				potentialExpectationsList.Add (prestigeObjective);
				potentialBonusObjectivesList.Add (highPrestigeObjective);
			} else if (carePrestige == 1) {
				potentialBonusObjectivesList.Add (prestigeObjective);
			}
		}

		int careDevelop = GetOwnerValueValue ("Development");
		if (careDevelop > 0) {
			int levelUpsNeeded = Random.Range (2, 5); //Between 2 and 4
			int rewardGold = levelUpsNeeded * 20;
			Objective levelUpObjective = new Objective (athlete, "levelUp", levelUpsNeeded, "gold", rewardGold);

			int highLevelUpsNeeded = Random.Range(5, 8); //Between 5 and 7
			int highRewardGold = highLevelUpsNeeded * 20;
			Objective highLevelUpObjective = new Objective (athlete, "levelUp", highLevelUpsNeeded, "gold", highRewardGold);

			/*
			Athlete specificAthlete = athlete.GetTeam ().rosterList [Random.Range (0, athlete.GetTeam ().rosterList.Count)];
			Objective levelUpSpecificAthleteObjective = new Objective (athlete, "levelUpSpecific", specificAthlete, "gold", rewardGold);
			*/
	
			if (careDevelop >= 2) {
				potentialExpectationsList.Add (levelUpObjective);
				potentialBonusObjectivesList.Add (highLevelUpObjective);
			} else if (careDevelop == 1) {
				potentialBonusObjectivesList.Add (levelUpObjective);
			}

			/*
			List<int> levelIntList = new List<int> ();
			for (int i = 0; i < athlete.GetTeam ().rosterList.Count; i++) {
				levelIntList.Add (athlete.GetTeam ().rosterList [i].level);
			}

			for (int i = 0; i < levelIntList.Count; i++) {

			}
			*/
		}




		for (int i = 0; i < 3; i++) {
			if (potentialExpectationsList.Count > 0) {
				Objective newExpectation = potentialExpectationsList [Random.Range (0, potentialExpectationsList.Count)];
				potentialExpectationsList.Remove (newExpectation);

				expectationsList.Add (newExpectation);
			}

			if (potentialBonusObjectivesList.Count > 0) {
				Objective newBonusObjective = potentialBonusObjectivesList [Random.Range (0, potentialBonusObjectivesList.Count)];
				potentialBonusObjectivesList.Remove (newBonusObjective);

				bonusObjectivesList.Add (newBonusObjective);
			}
		}
	}

	//Bonus Objectives and Expectations could potentiall be combined into one function, but they're checked at different rates.
	public List<Objective> GetCompletedBonusObjectives(Athlete manager) {
		for (int i = 0; i < bonusObjectivesList.Count; i++) {
			Objective objective = bonusObjectivesList [i];
			TeamController team = manager.GetTeam ();

			switch (objective.objectiveID) {
			case "gold":
				if (team.gold >= objective.objectiveValue) {
					Debug.Log ("ENOUGH GOLD HAS BEEN COLLECTED");
					objective.completed = true;
				}
				break;
			case "win":
				if (team.winsThisSeason >= objective.objectiveValue) {
					Debug.Log ("WINS HAVE BEEN COLLECTED ENOUGHLY");
					objective.completed = true;
				}
				break;
			case "prestige":
				if (team.prestige >= objective.objectiveValue) {
					Debug.Log ("GOT ALL THEM PRESTIGES");
					objective.completed = true;
				}
				break;
			case "levelUp":
				Debug.Log ("Not sure how to calculate");

				break;
			}
		}

		List<Objective> completedObjectives = new List<Objective> ();
		for (int i = 0; i < bonusObjectivesList.Count; i++) {
			if (bonusObjectivesList [i].completed) {
				completedObjectives.Add (bonusObjectivesList [i]);
			}
		}

		return completedObjectives;
	}

	public List<Objective> GetCompletedExpectations(Athlete manager) {
		for (int i = 0; i < expectationsList.Count; i++) {
			Objective expectation = expectationsList [i];
			TeamController team = manager.GetTeam ();

			switch (expectation.objectiveID) {
			case "gold":
				if (team.gold >= expectation.objectiveValue) {
					Debug.Log ("ENOUGH GOLD HAS BEEN COLLECTED");
					expectation.completed = true;
				}
				break;
			case "win":
				if (team.winsThisSeason >= expectation.objectiveValue) {
					Debug.Log ("WINS HAVE BEEN COLLECTED ENOUGHLY");
					expectation.completed = true;
				}
				break;
			case "prestige":
				if (team.prestige >= expectation.objectiveValue) {
					Debug.Log ("GOT ALL THEM PRESTIGES");
					expectation.completed = true;
				}
				break;
			case "levelUp":
				Debug.Log ("Not sure how to calculate");

				break;
			}
		}

		List<Objective> completedExpectations = new List<Objective> ();
		for (int i = 0; i < expectationsList.Count; i++) {
			if (expectationsList [i].completed) {
				completedExpectations.Add (expectationsList [i]);
			}
		}

		return completedExpectations;
	}

	public string GetWelcomeMessage() {

		if (welcomeMessages.Count == 0) { //Use the default messages
			welcomeMessages.Add ("Welcome to " + teamString + ". The Brimshire League has been reestablished after a 10-year hiatus and we believe that with your " +
				"leadership we can contend for the championship on a yearly basis. As manager of " + teamString + " we need you to evaluate the talent of our " +
				"athletes and find players that can help us win games." + '\n' + '\n' + "Since it's the first season of a new league we don't expect results " +
				"immediately. I've laid out a few expectations for the season that I think are realistic. I'm putting my trust in you to take care of this squad.");

			welcomeMessages.Add ("Welcome aboard! As the manager of " + teamString + " your top priority is to meet the expectations that I've laid out for you. You'll " +
				"have the final decision on all decisions regarding the roster of the team and I'm counting on you to figure out the best way to accomplish our goals." +
				'\n' + '\n' + "As you may know, the Brimshire League has been reestablished after a 10-year hiatus. Since it's the first season of the league we " +
				"aren't exactly shooting for the stars. It'll take time for you to get a feel for things so just do your best. You'll learn on the job.");
		} //Else welcome messages have already been assigned

		return welcomeMessages [Random.Range (0, welcomeMessages.Count)];
	}

	public string GetRegularSeasonMessage() {
		if (team.seasonMatchups[GameController.week] == null) { //If there was no game this week
			return regularSeasonByeMessages[Random.Range(0, regularSeasonByeMessages.Count)];
		} else if (team.seasonMatchups [GameController.week].winner == team) { //If they won this week
			return regularSeasonWinMessages[Random.Range(0, regularSeasonWinMessages.Count)];
		} else { //If they lost this week
			return regularSeasonLossMessages[Random.Range(0, regularSeasonLossMessages.Count)];
		}
	}

	public string GetEndMessage() {
		List<string> offseasonMessages = new List<string> ();

		if (team.losesThisSeason == 0) {
			offseasonMessages.Add ("Absolutely brilliant. Congratulations to you, my friend. We're the first ever team in the entire history of Brimshire to ever go " +
			"undefeated throughout an entire season. And you capped it off with a championship! Those expectations I laid out at the start don't even matter anymore. " +
			"Nothing can top what you've just done. I have no words to explain what this means to this city and the people of " + team.GetCity ().cityName + ". " +
			"You're a true hero in these parts. Now if you excuse me, I've got some celebrating to do."); //This assumes undefeated == championship
		} else {
			int numIncompleteExpectations = GetAmountIncompleteExpectations ();
			bool champions = false;
			if (team.league.leagueChampions [team.league.seasonsPlayed - 1] == team) {
				champions = true;
			}

			if (numIncompleteExpectations == 0) {
				if (champions) {
					offseasonMessages.Add ("Absolutely brilliant work. Not only did you lead " + teamString + " to a championship, but you've managed to meet all of my " +
					"expectations as well. I couldn't be more pleased with your results and I look forward to speaking with you again soon about my expecations " +
					"for next year. But don't worry about that yet. We've got plenty of celebrations to attend first!");
				} else {
					offseasonMessages.Add ("Excellent work. You met or exceeded all of my expectations for this season and I'm very pleased with your performance. " +
					"Although we weren't the champions this year I think we're certainly on the right track and I'm looking forward to our future together. " +
					"This offseason I'm confident that you'll make the right moves to keep this team competitive and I'll contact you shortly before the preseason " +
					"begins to discuss my expectations for next season.");
				}
			} else if (numIncompleteExpectations == 1) {
				if (champions) {
					offseasonMessages.Add ("Fantastic! Although you missed out on one of the expectations that I set for you this season, you brought this city a " +
					"championship banner and it's hard to complain about those type of results! We'll certainly be bringing you back for next season and I'll be " +
					"in touch soon to lay out my expectations for next season. Until then, be sure to enjoy the celebrations! I know I will!");
				} else {
					offseasonMessages.Add ("Overall I was mostly pleased with your performance with us this year. You failed to meet just one of my expectations, " +
					"which disappoints me but we can't always get everything we want now can we? Just be sure not to let it happen again. I know you're doing your " +
					"best. I'll send another message shortly before the next season starts laying out my expectations for next year. Hopefully next time you can " +
					"actually meet them all or else I might need to begin searching for someone who can.");
				}
			} else if (numIncompleteExpectations == 2) {
				if (champions) {
					offseasonMessages.Add ("Winning the championship like that makes it hard to critize your shortcomings! Although you failed to meet two of the expectations " +
						"I had, you brought " + team.GetCity().cityName + " a banner and the people would be furious if I were to repremand you. So congratulations on an " +
						"incredible season. I'll contact you during the preseason to provide my expectations for next season. This time you had best meet them all. " +
						"Although if you want to win another championship I don't think I'll be very upset hahaha. Time to celebrate!");
				} else {
					offseasonMessages.Add ("You're extremely fortunate that I don't fire you right here and now. Not only are our fans disappointed that we didn't win " +
					"the championship, but you failed two of my expectations. Those kind of results are simply unacceptable and I've begun scouting potential " +
					"replacements if this is what I should expect from you in the future. You'll be hearing from me soon about my expectations for next season. " +
					"You'd best focus on rebuilding this squad to prepare for them.");
				}
			} else {
				if (champions) {
					offseasonMessages.Add ("Full disclosure: I was going to fire you after this season. But then I looked out at our fans celebrating throughout " +
					team.GetCity ().cityName + " and I knew I had no choice but to retain you. You failed to meet every single expectation that I gave you this year " +
					"and yet you won the championship so I have no choice but to bring you back. Congratulations on your success, but if you fail to meet any of my " +
					"expectations for next season I promise you that it'll be your last with " + teamString + ".");
				} else {
					offseasonMessages.Add ("I gave you three simple expectations at the start of the season and you failed to meet each and every one of them. That's " +
					"simply unacceptable. You had an entire season to work towads them. What in the world were you even doing? You've wasted a year of my time " +
					"and resources and you've let down the good people of " + team.GetCity ().cityName + ". You're fired. Get out of my sight.");
				}
			}
		}

		if (offseasonMessages.Count > 0) {
			return offseasonMessages [Random.Range (0, offseasonMessages.Count)];
		} else {
			return "Dope season. Dope manager. Dope dingus.";
		}
	}

	public string GetPreseasonMessage() {
		List<string> preseasonMessages = new List<string> ();

		if (givesPraise) {
			preseasonMessages.Add ("Last season is behind us now. Time to focus on the future. With a talented manager such as yourself we have high expectations for " +
			"this squad, which I've included with this message. We're very pleased with your work with this roster so far and we're looking forward to seeing how you " +
			"adjust to the recent league rule changes. We've got a bit of time before the season begins so feel free to make any transactions that position the team to " +
			"achieve those expectations. We're fully confident in your abilities, coach.");
		}

		if (rude) {
			preseasonMessages.Add ("Another season, another opportunity. I've attached a few of my expectations for you and the team this seaon and you'd best meet " +
			"them if you want to continue working for me. You can be replaced in an instant if we don't find success but that's a lot of work for me so just get it " +
			"done.");
		}

		if (preseasonMessages.Count == 0) {
			preseasonMessages.Add ("We're really looking forward to bringing you back for another season to see what you can do with this squad. I've attached a few " +
			"expectations I have for you and the team this year that I'm confident you can achieve. It'll be interesting to see how the new league rules change your " +
			"approach. I'll be in touch with you throughout the season to discuss your progress.");
		}

		return preseasonMessages [Random.Range (0, preseasonMessages.Count)];
	}

	public int GetAmountIncompleteExpectations() {
		int didntDoEm = 0;

		for (int i = 0; i < expectationsList.Count; i++) {
			if (expectationsList [i].completed == false) {
				didntDoEm++;
			}
		}

		return didntDoEm;
	}
}

[System.Serializable]
public class OwnerValue {
	public string valueID;
	public int valueValue;

	public OwnerValue(string id, int val) {
		valueID = id;
		valueValue = val;
	}
}

public class Objective {
	public Athlete athlete;

	public string objectiveID; //win
	public int objectiveValue; //2
	public string objectiveString; //"Win 2 Matches"

	public string rewardID; //gold, prestige, etc
	public int rewardValue; //20
	public string rewardString = "Unassigned"; //20 Gold

	public bool completed = false;


	public Objective(Athlete a, string id, int value, string rid, int rvalue) {
		athlete = a;

		objectiveID = id;
		objectiveValue = value;

		objectiveString = "- ";

		switch(objectiveID) {
		case "win":
			objectiveString += "Win " + value + " Matches";
			break;
		case "gold":
			objectiveString += "Accumulate " + value + " Gold";
			break;
		case "prestige":
			objectiveString += "Have " + value + " Prestige";
			break;
		case "levelUp":
			objectiveString += "Level Up " + value + " Athletes";
			break;
		default:
			Debug.Log ("That objective does not exist.");
			break;
		} 

		rewardID = rid;
		rewardValue = rvalue;

		rewardString = "Reward: ";

		switch (rewardID) {
		case "gold":
			rewardString += rewardValue + " Gold";
			break;
		case "prestige":
			rewardString += rewardValue + " Prestige";
			break;
		default:
			Debug.Log ("That reward don't exist sucka.");
			break;
		}

		//rewardString = "Reward: " + rewardValue + (char.ToUpper (rewardID [0]) + rewardID.Substring (1));
		//Needs to be tested
	}
}
