using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Vectrosity;

public class ModeIndicators : MonoBehaviour {
	public static ModeIndicators instance;
	public static ModeIndicators Instance {
		get {
			if(instance == null) {
				instance = GameObject.FindObjectOfType<ModeIndicators>();
			}
			return instance;
		}
	}

	public float fontSize;

	public InputManager playerOne;
    public InputManager playerTwo;

    private VectorLine playerOneMode;
	private VectorLine playerTwoMode;

	void Awake() {
		playerOneMode = new VectorLine("Player One Mode", new List<Vector3>(), null, 2.0f, LineType.Discrete, Joins.Weld);
		playerTwoMode = new VectorLine("Player Two Mode", new List<Vector3>(), null, 2.0f, LineType.Discrete, Joins.Weld);
    }
    void Update() {
		playerOneMode.Draw();
		playerTwoMode.Draw();
	}

	public void UpdateMode(int number, string text) {
        Vector3 position = Vector3.zero;
        if (number == 0) {
			//position = new Vector3(-40.0f, 22.0f, 0.0f);
			position = new Vector3(-36.0f, -20.0f, 0.0f);
            playerOneMode.MakeText(text, position, fontSize);
		} else {
            //position = new Vector3(40.0f - text.Length * fontSize, 22.0f, 0.0f);
            position = new Vector3(36.0f - text.Length * fontSize, -20.0f, 0.0f);
            playerTwoMode.MakeText(text, position, fontSize);
		}
	}
}
