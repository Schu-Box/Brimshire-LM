using System.Collections.Generic;

namespace Lexic {
	//A dictionary containing syllables that will form fictional fantasy male names. Fits any fantasy game.
	public class AthleteNames : BaseNames {
		private static Dictionary<string, List<string>> syllableSets = new Dictionary<string, List<string>> () { 
			{
				"start",    new List<string>(){
					"Blip", "Gert", "Chitt", "Flex", "Bump", "Nip", "Yol", "Bimp", "Nort", "Tac", "Yim", "Fif", "Gerk", "Lead", "Rox", "Bav", "Piar", "Dhru",
					"Schu", "Nex", "Ven", "Kor", "Max", "Lief", "Olov", "Mid", "Pip", "Trev", "Trog", "Van", "Goat", "Kal", "Kod", "Gil", "Mo", "Und", "Bal",
					"Bar", "Hog", "Phuck", "Haz", "Good", "Bay", "Tom", "Ed", "Ruf", "Shad", "Sun", "Bro", "Rit", "Hans", "Aich", "Ait","Ter", "Gui", "Fan",
					"Pul", "Pil", "Row", "Mor", "Fen", "Lon", "Lor", "Bisch"
				}
			},
			/*
			{
				//Middle syllables are currently disabled
				"middle", new List<string>(){ 
					"al", "an", "ar", "el", "en", "ess", "ian", "onn", "or"
				}

			},
			*/
			{
				"end", new List<string>(){
					"ai", "an", "ar", "ath", "en", "eo", "ian", "is", "u", "or", "y", "id", "is", "erd", "mun", "ing", "leton", "er", "erg", "tha", "s", "es",
					"va", "al", "el", "ess", "onn", "na", "sil", "sul", "ly", "ry", "ow", "len", "of"
				}
			},
		};

		private static List<string> rules = new List<string>() {
			"%100start%80end"
		};

		public new static List<string> GetSyllableSet(string key) { return syllableSets[key]; }

		public new static List<string> GetRules() { return rules; }   
	}
}
