﻿using UnityEngine;
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

	public GameObject basicBulletPrefab;

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
		//InputsEqualAngle();
		InputEqualsSets();
		Debug.Log("Fire!");
		for(int i = 0; i < mashBufferSize; i++){
			Debug.Log(mashBuffer[i]);
		}
	}

	void InputsEqualAngle() {
		Rigidbody2D bullet;
		float angle = 0.0f;
		for(int i = 0; i < mashBufferSize; i++){
			if(mashBuffer[i] == 'A') {
				angle += 10.0f;
			}
			else if(mashBuffer[i] == 'B') {
				angle -= 10.0f;
			}
		}
		bullet = ((GameObject)Instantiate (basicBulletPrefab, transform.position, 
			Quaternion.Euler (0.0f, 0.0f, angle))).GetComponent<Rigidbody2D> ();

		bullet.velocity = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)) * 10;
	}

	void InputEqualsSets() {
		int horis = 0;
		int verts = 0;

		for(int i = 0; i < mashBufferSize; i++){
			if(mashBuffer[i] == 'A') {
				horis++;
			}
			else if(mashBuffer[i] == 'B') {
				verts++;
			}
		}

		for(int i = 0; i < horis; i++) {
			Rigidbody2D bullet;
			bullet = ((GameObject)Instantiate (basicBulletPrefab, transform.position, 
				Quaternion.Euler (0.0f, 0.0f, 10.0f * i))).GetComponent<Rigidbody2D> ();
			bullet.velocity = new Vector2(Mathf.Cos(10.0f * i * Mathf.Deg2Rad), Mathf.Sin(10.0f * i * Mathf.Deg2Rad)) * 10;

		}

		for(int i = 0; i < verts; i++) {
			Rigidbody2D bullet;
			bullet = ((GameObject)Instantiate (basicBulletPrefab, transform.position, 
				Quaternion.Euler (0.0f, 0.0f, 90.0f - (10.0f * i)))).GetComponent<Rigidbody2D> ();
			bullet.velocity = new Vector2(Mathf.Cos((90.0f - (10.0f * i)) * Mathf.Deg2Rad), Mathf.Sin((90.0f -10.0f * i) * Mathf.Deg2Rad)) * 10;

		}
	}
}
