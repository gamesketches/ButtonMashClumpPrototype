using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {

	public class Scene {
		public string horusWin;
		public string setWin;
		public List<string> dialogue;

		public Scene(string hor, string set, List<string> dia) {
			horusWin = hor;
			setWin = set;
			dialogue = dia;
		}
	}

	public Text sceneText;
	public bool inScene;


	IEnumerator scene_iter;
	private	List<string> scene;
	private Dictionary<string, Scene> scenes;
	private static Scene currentScene;
	void Start () {
		inScene = false;
		List<string> dialogue = new List<string>();
		dialogue.Add("Sett is real mad");
		dialogue.Add("Horus is picking a fight");
		dialogue.Add("They did indeed fight");

		Scene startScene = new Scene("hor", "set", dialogue);

		List<string> horDialogue = new List<string>();
		horDialogue.Add("Horus grabbed Set's nads");
		horDialogue.Add("Heyo I said nads");

		Scene horWins = new Scene("gameover", "gameover", horDialogue);

		List<string> setDialogue = new List<string>();
		setDialogue.Add("Set used hyper beam");
		setDialogue.Add("Heyo I said nads");

		Scene setWins = new Scene("gameover", "gameover", setDialogue);

		List<string> gameOverDia = new List<string>();
		gameOverDia.Add("and what did we learn today");
		gameOverDia.Add("someone's nads got grabbed");

		Scene gameOver = new Scene("", "", gameOverDia);

		scenes = new Dictionary<string, Scene>();
		scenes.Add("start", startScene);
		scenes.Add("hor", horWins);
		scenes.Add("set", setWins);
		scenes.Add("gameover", gameOver);

		if(currentScene == null)
			currentScene = scenes["start"];
	}

	public void StartScene(int result) {
		inScene = true;
		currentScene = result == 1 ? scenes[currentScene.horusWin] : scenes[currentScene.setWin];
		scene_iter = currentScene.dialogue.GetEnumerator();
		sceneText.text = "heyo";
	}
	
	// Update is called once per frame
	void Update () {
		if(inScene && Input.anyKeyDown) {
			if(scene_iter.MoveNext()) {
			sceneText.text = (string)scene_iter.Current;
			}
			else {
			sceneText.text = "";
			inScene = false;
			}
		}
	}
}
