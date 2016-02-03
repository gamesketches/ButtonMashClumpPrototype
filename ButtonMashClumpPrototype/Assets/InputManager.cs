using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {

	public string fireAxis;
	public string buttonA;
	public string buttonB;
	public int mashBufferSize;

	public int ShotsPerMinute;
	private int shotCooldown;
	private char[] mashBuffer;
	private int bufferIter;

	// Use this for initialization
	void Start () {
		// BPM to frames conversion is (60 / BPM) * F where F is frames per second
		// Also: rounding simplicity
		shotCooldown = Mathf.RoundToInt((60.0f / ShotsPerMinute) * 60.0f);
		mashBuffer = new char[mashBufferSize];
		bufferIter = 0;
	}
	
	// Update is called once per frame
	void Update () {
		// This will probably be better as a GetKeyUp or Down
		if(Input.GetButtonUp(fireAxis) && shotCooldown <= 0) {
			InterpretInputs();
			for(int i = 0; i < mashBufferSize; i++){
				mashBuffer.SetValue('*', i);
			}
			bufferIter = 0;
			return;
		}
		if(Input.GetButtonUp(buttonA)) {
			mashBuffer.SetValue('A', bufferIter);
		}
		else if(Input.GetButtonUp(buttonB)) {
			mashBuffer.SetValue('B', bufferIter);
		}
		// Hell yeah ternaries
		bufferIter = bufferIter >= mashBufferSize - 1 ? 0 : bufferIter + 1;
		shotCooldown--;
	}

	void InterpretInputs() {
		// Do some stuff here
		Debug.Log("Fire!");
		for(int i = 0; i < mashBufferSize; i++){
			Debug.Log(mashBuffer[i]);
		}
	}
}
