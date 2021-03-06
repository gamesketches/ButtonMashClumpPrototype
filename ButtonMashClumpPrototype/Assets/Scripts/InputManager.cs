﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class InputManager : MonoBehaviour {
	
	public string buttonA;
	public string buttonB;
	public string buttonC;
	public string buttonD;
	public string leftScroll;
	public string rightScroll;
	public int mashBufferSize;
	public float fullBufferScale;
	public bool shootFullBuffer;
	public bool shootStrays;
	public bool exponentialBuffer;
	public bool cooldown;

	public float inputCooldown;
	private float inputCooldownTimer;
	public int meleeInputCooldown;
	private bool mashing;

	private char[] mashBuffer;
	private char[] meleeBuffer;
	private int bufferIter;
	private int exponentCooldown;
	private int meleeCooldown;

	private PlayerMovement movementManager;

	public Color noShootingColor;
	private Color startingColor;

	private Player player;
	private ShotManager shotManager;

	public AudioClip bulletShot1;
	public AudioClip bulletShot2;
	private AudioSource audioOne;
	private AudioSource audioTwo;

	// Use this for initialization
	void Start () {
		player = GetComponent<Player>();
		mashBuffer = new char[mashBufferSize];
		meleeBuffer = new char[mashBufferSize];
		for(int i = 0; i < mashBufferSize; i++){
			mashBuffer.SetValue('*', i);
			meleeBuffer.SetValue('D', i);
		}
		bufferIter = 0;
		exponentCooldown = 0;
		movementManager = gameObject.GetComponent<PlayerMovement>();
		shotManager = gameObject.GetComponent<ShotManager>();
		shotManager.SetMashBufferSize(mashBufferSize);
		startingColor = GetComponentInChildren<Renderer>().material.color;
		AudioSource[] sources = GetComponents<AudioSource>();
		audioOne = sources[1];
		audioTwo = sources[2];
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.R)) {
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}

			GetComponentInChildren<Renderer>().material.color = startingColor;
			char button = GetButtonPress();
			if(button == 'D') {
				if(meleeCooldown <= 0) {
					shotManager.InputMeleeAttacksSki(meleeBuffer);
					meleeCooldown = meleeInputCooldown;
				}
			}
			else if(button != '0' && exponentCooldown <= 0){
				inputCooldownTimer = inputCooldown;
				gameObject.transform.localScale = Vector3.Lerp(new Vector3(1f, 1f, 1f), 
														new Vector3(fullBufferScale, fullBufferScale, fullBufferScale),
														(float)bufferIter / (float)mashBufferSize);
			{
				mashBuffer.SetValue(button, bufferIter);
			}
			if(!mashing) {
				mashing = true;
			}
			if(shootStrays) {
				FireStray();
			}
			if(exponentialBuffer && exponentCooldown <= 0) {
				ExponentShot();
			}
			if(shootFullBuffer) {
				bufferIter++;
				movementManager.PassBufferToReticle(bufferIter, mashBufferSize);
				if(bufferIter >= mashBufferSize) {
					Fire();
				}
			} else {
				if(bufferIter < mashBufferSize && exponentCooldown <= 0){
					bufferIter++;
					//bufferIter = bufferIter >= mashBufferSize - 1 ? 0 : bufferIter + 1;
				}
			}
		}else if(mashing && button == '0'){
			inputCooldownTimer -= Time.deltaTime;
			if(inputCooldownTimer <= 0.0f) {
				Fire();
			}
		}
		if(exponentCooldown > 0) { 
			exponentCooldown--;
			GetComponentInChildren<Renderer>().material.color = noShootingColor;
		}

		if(movementManager.useBenAiming) {
			if(!mashing) {
				movementManager.HandleRotation();
			}
		}
		meleeCooldown--;
		//movementManager.PassBufferToReticle(bufferIter, mashBufferSize);
	}

	char GetButtonPress() {
		if(Input.GetButtonDown(buttonA)) {
			return 'A';
		}
		else if(Input.GetButtonDown(buttonB)) {
			return 'B';
		}
		else if(Input.GetButtonDown(buttonC)) {
			return 'C';
		}
		else if(Input.GetButtonDown(buttonD)) {
			return 'D';
		}
		else {
			return '0';
		}
	}

	void Fire() {
		movementManager.ResetReticle();
		/*if(exponentCooldown > 0) {
			shotManager.InputMeleeAttacksSki(meleeBuffer);
		}
		else {
			shotManager.shotInterpreter(mashBuffer);
		}*/
		if(exponentCooldown <= 0) {
			shotManager.shotInterpreter(mashBuffer);
		}
		for(int i = 0; i < mashBufferSize; i++){
			mashBuffer.SetValue('*', i);
		}
		// This will be the hardest part to get right
		if(exponentialBuffer) {
			int lockFrames = (bufferIter * (bufferIter + 1));
			if (bufferIter < mashBufferSize) {
				lockFrames = lockFrames / 2;
			}
			if(cooldown) {
				exponentCooldown = lockFrames;
			}
		}
		bufferIter = 0;
		mashing = false;
		gameObject.transform.localScale = new Vector3(1, 1, 1);
	}

	void FireStray() {
		shotManager.createBullet(Random.Range(0.0f, 360.0f), Random.Range(15.0f, 25.0f), BulletType.Stray);
	}

	void ExponentShot() {
		float incrementAngle = 45.0f;
		AudioClip clip = bulletShot1;
		for(int i = 0; i < mashBuffer.Length; i++) {
			//int type = 100;
			BulletType type = BulletType.Stray;
			float speed = (float)bufferIter * 2;
			switch(mashBuffer[i]) {
				case 'A':
					type = BulletType.Point;
					speed = 28.0f;
					break;
				case 'B':
					type = BulletType.Roundabout;
					speed = 40.0f;
					break;
				case 'C':
					speed = 5.0f;
					type = BulletType.Block;
					clip = bulletShot2;
					break;
				case 'D':
					Debug.LogError("Melee button sent to projectile buffer");
					break;
				default:
					continue;
					break;
			}

			if(bufferIter < 2) {
				shotManager.createBullet(0.0f, speed, type);
				playAudio(clip);
				return;
			}
			else {
				float baseAngle = 0.0f;
				for(int k = 1; k < i; k++) {
					speed = speed > 1 ? speed -= 1 : 1;
					shotManager.createBullet(baseAngle + incrementAngle, speed, type);
					playAudio(clip);
					k++;
					shotManager.createBullet(-(baseAngle + incrementAngle), speed, type);
					playAudio(clip);
					baseAngle += incrementAngle;
				}
			}
			if(i >= 1) {
				incrementAngle /= i;
			}
		}
	}

	void playAudio(AudioClip clip) {
		if(audioOne.isPlaying && audioTwo.isPlaying) {
			return;
		}
		else {
			Debug.Log("playin audio");
			if(audioOne.isPlaying) {
				audioTwo.PlayOneShot(clip);
			}
			else {
				audioOne.PlayOneShot(clip);
			}
		}
	}

	public void forcedCooldown(int frames) {
		exponentCooldown = frames;
		for(int i = 0; i < mashBufferSize; i++){
			mashBuffer.SetValue('*', i);
		}
	}

}
