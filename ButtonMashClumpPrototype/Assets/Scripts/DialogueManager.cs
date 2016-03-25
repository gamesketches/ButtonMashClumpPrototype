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


	IEnumerator scene_iter;
	private	List<string> scene;
	private Dictionary<string, Scene> scenes;
	private static Scene currentScene;
	void Start () {
		inScene = false;
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
		if(currentScene == null)
			currentScene = scenes["Start"];
		
	}

	public void StartScene(int result) {
		inScene = true;
		currentScene = result == 1 ? scenes[currentScene.horusKey] : scenes[currentScene.setKey];
		scene_iter = currentScene.lines.GetEnumerator();
		sceneText.text = "";
	}
	
	// Update is called once per frame
	void Update () {
		if(inScene && Input.anyKeyDown) {
			if(scene_iter.MoveNext()) {
			sceneText.text = (string)scene_iter.Current;
			changeColorForSpeaker("SET", Color.red, "HOR", Color.blue);
			}
			else {
				if(sceneText.text == "THEND") {
					currentScene = scenes["Start"];
				}
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
