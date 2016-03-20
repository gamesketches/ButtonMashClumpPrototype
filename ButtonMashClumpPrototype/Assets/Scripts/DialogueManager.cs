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
		inScene = true;
		scene = new List<string>(3);
		scene.Add("Sett is real mad");
		scene.Add("Horus is picking a fight");
		scene.Add("They did indeed fight");

		scene_iter = scene.GetEnumerator();
		StartScene();
	}

	public	void StartScene() {
		inScene = true;
		sceneText.text = (string)scene_iter.Current;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.anyKeyDown) {
			if(scene_iter.MoveNext()) {
			sceneText.text = (string)scene_iter.Current;
			}
			else {
			Debug.Log("conversation over");
			sceneText.text = "";
			inScene = false;
			}
		}
	}
}
