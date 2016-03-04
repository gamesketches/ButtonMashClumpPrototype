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
		SetInterpreterText();
		startingColor = GetComponentInChildren<Renderer>().material.color;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.R)) {
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}

			GetComponentInChildren<Renderer>().material.color = startingColor;
			if(Input.GetButtonDown(buttonA) || Input.GetButtonDown(buttonB) || 
				Input.GetButtonDown(buttonC) || Input.GetButtonDown(buttonD)) {
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
				if(Input.GetButtonDown(buttonA)) {
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
				}
				if(shootFullBuffer) {
					bufferIter++;
					if(bufferIter >= mashBufferSize) {
						Fire();
					}
			} else {
				bufferIter = bufferIter >= mashBufferSize - 1 ? 0 : bufferIter + 1;
			}
		}else if(mashing && !Input.GetButton(buttonA) && !Input.GetButton(buttonB) && 
			!Input.GetButton(buttonC) && !Input.GetButton(buttonD)) {
			inputCooldownTimer -= Time.deltaTime;
			if(inputCooldownTimer <= 0.0f) {
				Fire();
			}
		}
		if(exponentCooldown > 0) { 
			exponentCooldown--;
			GetComponentInChildren<Renderer>().material.color = noShootingColor;
		}
		/*if(Input.GetButtonDown(leftScroll)) {
			interpreterIndex = ((interpreterIndex - 1) + 7) % 7;
			SetInterpreterText();
		} else if(Input.GetButtonDown(rightScroll)) {
			interpreterIndex = (interpreterIndex + 1) % 7;
			SetInterpreterText();
		}*/

		if(movementManager.useBenAiming) {
			if(!mashing) {
				movementManager.HandleRotation();
			}
		}
	}

	void Fire() {
		InterpretInputs();
		for(int i = 0; i < mashBufferSize; i++){
			mashBuffer.SetValue('*', i);
		}
		// This will be the hardest part to get right
		if(exponentialBuffer) {
			int lockFrames = (bufferIter * (bufferIter + 1));
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

	void SetInterpreterText() {
		switch(interpreterIndex) {
		case 0:
			ModeIndicators.Instance.UpdateMode(player.number, "Inputs Equal Angle");
			break;
		case 1:
			ModeIndicators.Instance.UpdateMode(player.number, "Input Equals Sets");
			break;
		case 2:
			ModeIndicators.Instance.UpdateMode(player.number, "Input Patterns");
			break;
		case 3:
			ModeIndicators.Instance.UpdateMode(player.number, "Input Equals Number");
			break;
		case 4:
			ModeIndicators.Instance.UpdateMode(player.number, "Input Equals Random");
			break;
		case 5:
			ModeIndicators.Instance.UpdateMode(player.number, "Input Melee Attacks");
			break;
		case 6:
			ModeIndicators.Instance.UpdateMode(player.number, "Input Melee Attacks *SKI*");
			break;
		}
	}

	void InterpretInputs() {
		if(exponentCooldown > 0) {
			shotManager.InputMeleeAttacksSki(mashBuffer);
			return;
		}
		switch(interpreterIndex) {
			case 0:
				// each A increases angle by 10%, B reduces by 10%
				shotManager.InputsEqualAngle(mashBuffer);
				break;
			case 1:
				// A sets a shot at 0 degrees, adds another projectile for each A.
				// B is the same thing but starting at 90 degrees
				shotManager.InputEqualsSets(mashBuffer);
				break;
			case 2:
				// Each two set of characters corresponds to a different set of projectiles
				shotManager.InputPatterns(mashBuffer);
				break;
			case 3:
				// Counts all inputs equally, number of projectiles is tied to num inputs
				shotManager.InputEqualsNumber(mashBuffer);
				break;
			case 4:
				shotManager.InputEqualsRandom(mashBuffer);
				break;
            case 5:
                // A equals width, B equals height
                shotManager.InputMeleeAttacks(mashBuffer);
                break;
            case 6:
                // lets barrel
                shotManager.InputMeleeAttacksSki(mashBuffer);
                break;
        }
	}

}
