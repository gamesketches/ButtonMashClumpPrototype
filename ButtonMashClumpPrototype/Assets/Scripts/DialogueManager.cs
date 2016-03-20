using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {

	public Text sceneText;
	public bool inScene;


	IEnumerator scene_iter;
	private	List<string> scene;

	// Use this for initialization
	void Start () {
		inScene = false;
		scene = new List<string>();
		scene.Add("Sett is real mad");
		scene.Add("Horus is picking a fight");
		scene.Add("They did indeed fight");
	}

	public void StartScene() {
		inScene = true;
		scene_iter = scene.GetEnumerator();
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
