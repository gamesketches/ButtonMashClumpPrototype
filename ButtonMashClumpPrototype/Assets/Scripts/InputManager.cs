using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InputManager : MonoBehaviour {

	public string fireAxis;
	public string buttonA;
	public string buttonB;
	public string buttonC;
	public string buttonD;
	public int mashBufferSize;

	public int ShotsPerMinute;
	private int shotCooldown;
	private char[] mashBuffer;
	private int bufferIter;

	private int interpreterIndex;

	public GameObject basicBulletPrefab;
	public GameObject meleeAttackPrefab;
	private PlayerMovement movementManager;

	// Use this for initialization
	void Start () {
		shotCooldown = Mathf.RoundToInt((60.0f / ShotsPerMinute) * 60.0f);
		mashBuffer = new char[mashBufferSize];
		for(int i = 0; i < mashBufferSize; i++){
			mashBuffer.SetValue('*', i);
		}
		bufferIter = 0;
		movementManager = gameObject.GetComponent<PlayerMovement>();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonUp(fireAxis) && shotCooldown <= 0) {
			InterpretInputs();
			for(int i = 0; i < mashBufferSize; i++){
				mashBuffer.SetValue('*', i);
			}
			bufferIter = 0;
			return;
		}

		if(Input.GetButtonDown(buttonA) || Input.GetButtonDown(buttonB) || 
			Input.GetButtonDown(buttonC) || Input.GetButtonDown(buttonD)) {
			if(Input.GetButtonDown(buttonA)) {
				mashBuffer.SetValue('A', bufferIter);
			} else if(Input.GetButtonDown(buttonB)) {
				mashBuffer.SetValue('B', bufferIter);
			} else if(Input.GetButtonDown(buttonC)) {
				mashBuffer.SetValue('C', bufferIter);
			} else if(Input.GetButtonDown(buttonD)) {
				mashBuffer.SetValue('D', bufferIter);
			}
			bufferIter = bufferIter >= mashBufferSize - 1 ? 0 : bufferIter + 1;
		} 
		// Hell yeah ternaries
		bufferIter = bufferIter >= mashBufferSize - 1 ? 0 : bufferIter + 1;
		shotCooldown--;

		if(Input.GetButtonDown("LeftScroll")) {
			interpreterIndex = ((interpreterIndex - 1) + 9) % 9;
		} else if(Input.GetButtonDown("RightScroll")) {
			interpreterIndex = (interpreterIndex + 1) % 9;
		}
	}

	void InterpretInputs() {
		switch(interpreterIndex) {
			case 0:
				// each A increases angle by 10%, B reduces by 10%
				InputsEqualAngle();
				break;
			case 1:
				// A sets a shot at 0 degrees, adds another projectile for each A.
				// B is the same thing but starting at 90 degrees
				InputEqualsSets();
				break;
			case 2:
				// Each two set of characters corresponds to a different set of projectiles
				InputPatterns();
				break;
			case 3:
				// Counts all inputs equally, number of projectiles is tied to num inputs
				InputEqualsNumber();
				break;
			case 4:
				// Another implementation of other
				InputEqualsNumberAlt();
				break;
			case 5:
				InputEqualsNumberInverse();
				break;
			case 6:
				InputEqualsNumberAltAlt();
				break;
			case 7:
				InputEqualsRandom();
				break;
			case 8:
				// A equals width, B equals height
				InputMeleeAttacks();
				break;
		}
		// Do some stuff here
		// each A increases angle by 10%, B reduces by 10%
		//InputsEqualAngle();
		// A sets a shot at 0 degrees, adds another projectile for each A.
		// B is the same thing but starting at 90 degrees
		//InputEqualsSets();
		// Each two set of characters corresponds to a different set of projectiles
		//InputPatterns();
		// Counts all inputs equally, number of projectiles is tied to num inputs
		//InputEqualsNumber();
		// Another implementation of other
		//InputEqualsNumberAlt();
		// dummy function
		//InputEqualsProjectileDecay()
		// A equals width, B equals height
		//InputEqualsNumberInverse();
		//InputEqualsNumberAltAlt();
		//InputEqualsRandom();
		//InputMeleeAttacks();
		//Debug.Log("Fire!");
		/*for(int i = 0; i < mashBufferSize; i++){
			Debug.Log(mashBuffer[i]);
		}*/
	}

	void InputsEqualAngle() {
		float angle = 0.0f;
		int numAs, numBs;
		TallyInputs(out numAs, out numBs);
		angle += 10.0f * numAs;
		angle -= 10.0f * numBs;
		createBullet(angle);
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
			createBullet(10.0f * i);

		}

		for(int i = 0; i < verts; i++) {
			createBullet(90.0f - (10.0f * i));
		}
	}

	void InputPatterns() {
		string pattern = "";

		string aa = "AA";
		string bb = "BB";
		string ab = "AB";
		string ba = "BA";

		for(int i = 0; i < mashBufferSize;i++){
			pattern = string.Concat(mashBuffer[i].ToString(), mashBuffer[++i].ToString());
			if(pattern.CompareTo(aa) == 1){

				createBullet(10.0f);
				createBullet(0.0f);
				createBullet(-10.0f);
			}

			else if(pattern.CompareTo(bb) == 1) {
				createBullet(100.0f);
				createBullet(90.0f);
				createBullet(80.0f);
			}
			else if(pattern.CompareTo(ab) == 1) {
				createBullet(-90.0f);
				createBullet(-80.0f);
				createBullet(-100.0f);
			}
			else if(pattern.CompareTo(ba) == 1){
				createBullet(180.0f);
				createBullet(190.0f);
				createBullet(200.0f);
			}
		}
	}

	void InputEqualsNumber() {
		int bulletNumber = 0;
		float baseAngle = 0.0f;
		bool firstButton = false;
		for(int i = 0; i < mashBufferSize; i++) {
			if(mashBuffer[i] != '*') {
				bulletNumber++;
				if(!firstButton) {
					firstButton = true;
					if(mashBuffer[i] == 'A') {
						baseAngle = 180.0f;
					} else if(mashBuffer[i] == 'B') {
						baseAngle = 270.0f;
					} else if(mashBuffer[i] == 'C') {
						baseAngle = 0.0f;
					} else if(mashBuffer[i] == 'D') {
						baseAngle = 90.0f;
					}
				}
			}
		}
		float angleDifference = 90.0f / mashBufferSize;
		List<float> bulletAngles = new List<float>();

		bulletAngles.Add(baseAngle);
		if(bulletNumber == mashBufferSize) {
			bulletAngles.Add(baseAngle + 90.0f);
			bulletAngles.Add(baseAngle - 90.0f);
		}

		if(bulletNumber > 1) {
			for(int i = 0; i < bulletNumber - 1; i++) {
				bulletAngles.Add(baseAngle + angleDifference * (i + 1));
				bulletAngles.Add(baseAngle - angleDifference * (i + 1));
			}
		}
		for(int i = 0; i < bulletAngles.Count; i++) {
			createBullet(bulletAngles[i]);
		}
	}

	void InputEqualsNumberInverse() {
		int bulletNumber = 0;
		float baseAngle = 0.0f;
		bool firstButton = false;
		for(int i = 0; i < mashBufferSize; i++) {
			if(mashBuffer[i] != '*') {
				bulletNumber++;
				if(!firstButton) {
					firstButton = true;
					if(mashBuffer[i] == 'A') {
						baseAngle = 180.0f;
					} else if(mashBuffer[i] == 'B') {
						baseAngle = 270.0f;
					} else if(mashBuffer[i] == 'C') {
						baseAngle = 0.0f;
					} else if(mashBuffer[i] == 'D') {
						baseAngle = 90.0f;
					}
				}
			}
		}
		float angleDifference = 90.0f / mashBufferSize;
		List<float> bulletAngles = new List<float>();

		bulletAngles.Add(baseAngle + 90.0f);
		bulletAngles.Add(baseAngle - 90.0f);
		if(bulletNumber == mashBufferSize) {
			bulletAngles.Add(baseAngle);
		}

		if(bulletNumber > 1) {
			for(int i = 0; i < bulletNumber; i++) {
				bulletAngles.Add(baseAngle + 90.0f - angleDifference * i);
				bulletAngles.Add(baseAngle - 90.0f + angleDifference * i);
			}
		}
		for(int i = 0; i < bulletAngles.Count; i++) {
			createBullet(bulletAngles[i]);
		}
	}

	void InputEqualsNumberAlt() {
		int bulletNumber = 0;
		for(int i = 0; i < mashBufferSize; i++) {
			if(mashBuffer[i] != '*') {
				bulletNumber++;
			}
		}
		float angleDifference;
		List<float> bulletAngles = new List<float>();
		if(bulletNumber == 0) {
			bulletAngles.Add(0.0f);
		} else if(bulletNumber % 2 == 0) {
			angleDifference = 90.0f / bulletNumber;
			for(int i = 0; i < bulletNumber / 2; i++) {
				bulletAngles.Add(angleDifference * (i + 1));
				bulletAngles.Add(-angleDifference * (i + 1));
			}
		} else {
			angleDifference = 90.0f / (bulletNumber - 1);
			bulletAngles.Add(0.0f);
			for(int i = 0; i < (bulletNumber - 1) / 2; i++) {
				bulletAngles.Add(angleDifference * (i + 1));
				bulletAngles.Add(-angleDifference * (i + 1));
			}
		}
		for(int i = 0; i < bulletAngles.Count; i++) {
			createBullet(bulletAngles[i]);
		}
	}

	void InputEqualsNumberAltAlt() {
		int bulletNumber = 0;
		for(int i = 0; i < mashBufferSize; i++) {
			if(mashBuffer[i] != '*') {
				bulletNumber++;
			}
		}
		float angleDifference = 90.0f / (mashBufferSize / 2.0f);
		List<float> bulletAngles = new List<float>();
		if(bulletNumber == 0) {
			bulletAngles.Add(0.0f);
		} else if(bulletNumber % 2 == 0) {
			for(int i = 0; i < bulletNumber / 2; i++) {
				bulletAngles.Add(angleDifference * (i + 1));
				bulletAngles.Add(-angleDifference * (i + 1));
			}
		} else {
			bulletAngles.Add(0.0f);
			for(int i = 0; i < (bulletNumber - 1) / 2; i++) {
				bulletAngles.Add(angleDifference * (i + 1));
				bulletAngles.Add(-angleDifference * (i + 1));
			}
		}
		for(int i = 0; i < bulletAngles.Count; i++) {
			createBullet(bulletAngles[i]);
		}
	}

	void InputEqualsRandom() {
		int bulletNumber = 0;
		for(int i = 0; i < mashBufferSize; i++) {
			if(mashBuffer[i] != '*') {
				bulletNumber++;
			}
		}

		for(int i = 0; i < bulletNumber; i++) {
			createBullet(Random.Range(0.0f, 360.0f));
		}
	}

	void InputMeleeAttacks() {
		int aCount, bCount;

		TallyInputs(out aCount, out bCount);

		float width = aCount * 0.3f;
		float height = bCount * 0.3f;

		// dem Lupin III references
		GameObject monkeyPunch;

		monkeyPunch = ((GameObject)Instantiate(meleeAttackPrefab, transform.position,
			Quaternion.Euler(0.0f, 0.0f, 0.0f)));

		monkeyPunch.transform.localScale = new Vector3(width, height, 0);

		Destroy(monkeyPunch, 0.5f);
	}
		
	void TallyInputs(out int num1, out int num2) {
		num1 = 0;
		num2 = 0;
		for(int i = 0; i < mashBufferSize; i++) {
			if(mashBuffer[i] == 'A') {
				num1++;
			}
			else if(mashBuffer[i] == 'B') {
				num2++;
			}
		}
	}

	void createBullet(float angle) {
		GameObject bullet;
		Rigidbody2D bulletRB;
		angle += movementManager.currentShotAngle();
		bullet = ((GameObject)Instantiate (basicBulletPrefab, transform.position, 
			Quaternion.Euler (0.0f, 0.0f, angle)));
		bulletRB = bullet.GetComponent<Rigidbody2D> ();

		bulletRB.velocity = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)) * 10;

		OwnerScript script = bullet.GetComponent<OwnerScript>();
		script.mother = gameObject;
	}
}
