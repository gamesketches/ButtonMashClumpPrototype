using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;
using SimpleJSON;

public class DialogueManager : MonoBehaviour {

	public class Scene {
		public string horusKey;
		public string setKey;
		public string[] hor50;
		public string[] set50;
		public List<string> lines;
		public string sceneKey;

		public Scene(string hor, string set, List<string> dia) {
			horusKey = hor;
			setKey = set;
			lines = dia;
		}
	}

	public Text sceneText;
	public bool inScene;
	private bool sceneStarted;


	IEnumerator scene_iter;
	private	List<string> scene;
	private Dictionary<string, Scene> scenes;
	private static Scene currentScene;
	private string[] curDialogue;
	void Start () {
		inScene = false;
		sceneStarted = false;
		TextAsset jsonText = (TextAsset) Resources.Load("output");

		var temp = JSON.Parse(jsonText.text);
		scenes = new Dictionary<string, Scene>();
		for(int i = 0; i < temp["data"].Count; i++) {
			List<string> sceneLines = new List<string>();
			for(int k = 0; k < temp["data"][i]["lines"].AsArray.Count; k++) {
				sceneLines.Add(temp["data"][i]["lines"].AsArray[k]);
			}
			scenes.Add(temp["data"][i]["sceneKey"], 
						new Scene(temp["data"][i]["horusKey"],
									temp["data"][i]["setKey"],
					sceneLines));
		}
		StreamReader damageLines = new StreamReader("Assets/Resources/TabSeparatedScript.tsv");
		string line;
		while((line = damageLines.ReadLine()) != null) {
			string[] demLines = line.Split('\t');
			// This garbage is setting up for horus/set  50%          response    Horuswin       SetWin
			scenes[demLines[0]].hor50 = new string[4] {demLines[1], demLines[2], demLines[3], demLines[4]};
			scenes[demLines[0]].set50 = new string[4] {demLines[5], demLines[6], demLines[7], demLines[8]};
		}


		if(currentScene == null)
			currentScene = scenes["Start"];
		
	}

	public void StartScene(int result) {
		if(sceneStarted) {
			return;
		}
		sceneStarted = true;
		inScene = true;
		//currentScene = result == 1 ? scenes[currentScene.horusKey] : scenes[currentScene.setKey];
		//scene_iter = currentScene.lines.GetEnumerator();
		sceneText.text = "";
		curDialogue = result == 1 ? currentScene.hor50 : currentScene.set50;
		sceneText.text = curDialogue[0];
	}


	public void EndScene(int result) {
		inScene = true;
		sceneText.text = "";
		sceneText.text = result == 1 ? curDialogue[2] : curDialogue[3];
		currentScene = result == 1 ? scenes[currentScene.horusKey] : scenes[currentScene.setKey];
		sceneStarted = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(inScene && Input.anyKeyDown) {
			if(sceneText.text == curDialogue[0]) {
			sceneText.text = curDialogue[1];//(string)scene_iter.Current;
			//changeColorForSpeaker("SET", Color.red, "HOR", Color.blue);
			}
			else {
				//if(sceneText.text == "THEND") {
				//	currentScene = scenes["Start"];
				//}
			sceneText.text = "";
			inScene = false;
			}
		}
	}

	void changeColorForSpeaker(string speakerName, Color color, string otherSpeaker, Color otherColor) {
		if(sceneText.text.Length < 3) {
			return;
		}
		if(sceneText.text.Substring(0, 3) == speakerName) {
			sceneText.color = color;
			return;
		}
		else if(sceneText.text.Substring(0, 3) == otherSpeaker) {
			sceneText.color = otherColor;
		}
	}
}
