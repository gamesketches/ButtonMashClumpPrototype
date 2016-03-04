using UnityEngine;
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

	//public Color chargeColor;

	public float inputCooldown;
	private float inputCooldownTimer;
	private bool mashing;

	private char[] mashBuffer;
	private int bufferIter;
	private int exponentCooldown;

	private int interpreterIndex = 3;

	public GameObject basicBulletPrefab;
	public GameObject strayBulletPrefab;
	public GameObject meleeAttackPrefab;
	public GameObject squareBulletPrefab;
	public GameObject xBulletPrefab;
	public GameObject circleBulletPrefab;
	public GameObject triangleBulletPrefab;
	private PlayerMovement movementManager;

	public Color noShootingColor;
	private Color startingColor;

	private Player player;
	private ShotManager shotManager;

	// Use this for initialization
	void Start () {
		player = GetComponent<Player>();
		mashBuffer = new char[mashBufferSize];
		for(int i = 0; i < mashBufferSize; i++){
			mashBuffer.SetValue('*', i);
		}
		bufferIter = 0;
		exponentCooldown = 0;
		movementManager = gameObject.GetComponent<PlayerMovement>();
		shotManager = gameObject.GetComponent<ShotManager>();
		startingColor = GetComponentInChildren<Renderer>().material.color;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.R)) {
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}

			GetComponentInChildren<Renderer>().material.color = startingColor;
			char button = GetButtonPress();
			if(button != '0'){//Input.GetButtonDown(buttonA) || Input.GetButtonDown(buttonB) || 
				//Input.GetButtonDown(buttonC) || Input.GetButtonDown(buttonD)) {
				inputCooldownTimer = inputCooldown;
				gameObject.transform.localScale = Vector3.Lerp(new Vector3(1f, 1f, 1f), 
														new Vector3(fullBufferScale, fullBufferScale, fullBufferScale),
														(float)bufferIter / (float)mashBufferSize);
				Debug.Log(fullBufferScale);
				if(!mashing) {
					mashing = true;
				}
				if(shootStrays) {
					FireStray();
				}
				if(exponentialBuffer && exponentCooldown <= 0) {
					ExponentShot();
				}
				mashBuffer.SetValue(button, bufferIter);
				/*if(Input.GetButtonDown(buttonA)) {
					mashBuffer.SetValue('A', bufferIter);
				} 
				else if(Input.GetButtonDown(buttonB)) {
					mashBuffer.SetValue('B', bufferIter);
				} 
				else if(Input.GetButtonDown(buttonC)) {
					mashBuffer.SetValue('C', bufferIter);
				} 
				else if(Input.GetButtonDown(buttonD)) {
					mashBuffer.SetValue('D', bufferIter);
				}*/
				if(shootFullBuffer) {
					bufferIter++;
					if(bufferIter >= mashBufferSize) {
						Fire();
					}
			} else {
				bufferIter = bufferIter >= mashBufferSize - 1 ? 0 : bufferIter + 1;
			}
		}else if(mashing && button == '0'){//!Input.GetButton(buttonA) && !Input.GetButton(buttonB) && 
			//!Input.GetButton(buttonC) && !Input.GetButton(buttonD)) {
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
		if(exponentCooldown > 0) {
			shotManager.InputMeleeAttacksSki(mashBuffer);
		}
		else {
			shotManager.shotInterpreter(mashBuffer);
		}
		for(int i = 0; i < mashBufferSize; i++){
			mashBuffer.SetValue('*', i);
		}
		// This will be the hardest part to get right
		if(exponentialBuffer) {
			int lockFrames = (bufferIter * (bufferIter + 1)) * 2;
			if (bufferIter < mashBufferSize) {
				lockFrames = lockFrames / 2;
			}
			exponentCooldown = lockFrames;
		}
		bufferIter = 0;
		mashing = false;
		gameObject.transform.localScale = new Vector3(1, 1, 1);
	}

	void FireStray() {
		shotManager.createBullet(Random.Range(0.0f, 360.0f), Random.Range(15.0f, 25.0f), 1);
	}

	void ExponentShot() {
		float incrementAngle = 45.0f;
		for(int i = 0; i < bufferIter; i++) {
			int type = 100;
			if(mashBuffer[i] == 'A') {
				type = 2;
			} else if(mashBuffer[i] == 'B') {
				type = 3;
			} else if(mashBuffer[i] == 'C') {
				type = 4;
			} else if(mashBuffer[i] == 'D') {
				type = 5;
			}
			if(bufferIter < 2) {
				shotManager.createBullet(0.0f, 20.0f, type);
				return;
			}
			else {
				float speed = (float)bufferIter * 2;
				float baseAngle = 0.0f;
				for(int k = 1; k < i; k++) {
					speed = speed > 1 ? speed -= 1 : 1;
					shotManager.createBullet(baseAngle + incrementAngle, speed, type);
					k++;
					shotManager.createBullet(-(baseAngle + incrementAngle), speed, type);
					baseAngle += incrementAngle;
				}
			}
			if(i >= 1) {
				incrementAngle /= i;
			}
		}
	}

	public int GetInterpreterIndex() {
		return interpreterIndex;
	}


}
