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
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.R)) {
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}

			GetComponentInChildren<Renderer>().material.color = startingColor;
			char button = GetButtonPress();
			if(button != '0'){
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
				if(button == 'D') {
					if(meleeCooldown <= 0) {
						shotManager.InputMeleeAttacksSki(meleeBuffer);
						meleeCooldown = meleeInputCooldown;
					}
				}
				else {
					mashBuffer.SetValue(button, bufferIter);
				}
				if(shootFullBuffer) {
					bufferIter++;
					if(bufferIter >= mashBufferSize) {
						Fire();
					}
			} else {
				bufferIter = bufferIter >= mashBufferSize - 1 ? 0 : bufferIter + 1;
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
			shotManager.InputMeleeAttacksSki(meleeBuffer);
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
		shotManager.createBullet(Random.Range(0.0f, 360.0f), Random.Range(15.0f, 25.0f), BulletType.Stray);
	}

	void ExponentShot() {
		float incrementAngle = 45.0f;
		for(int i = 0; i < bufferIter; i++) {
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
				return;
			}
			else {
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

}
