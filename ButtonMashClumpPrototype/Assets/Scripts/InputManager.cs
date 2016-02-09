using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
	public GameObject meleeAttackPrefab;

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
		//InputEqualsSets();
		//InputPatterns();
		InputEqualsNumber();
		//InputEqualsNumberAlt();
		//InputMeleeAttacks();
		//Debug.Log("Fire!");
		/*for(int i = 0; i < mashBufferSize; i++){
			Debug.Log(mashBuffer[i]);
		}*/
	}

	void InputsEqualAngle() {
		//Rigidbody2D bullet;
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
		Rigidbody2D bullet;
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
		Rigidbody2D bullet;
		for(int i = 0; i < bulletAngles.Count; i++) {
			createBullet(bulletAngles[i]);
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
		bullet = ((GameObject)Instantiate (basicBulletPrefab, transform.position, 
			Quaternion.Euler (0.0f, 0.0f, angle)));
		bulletRB = bullet.GetComponent<Rigidbody2D> ();

		bulletRB.velocity = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)) * 10;

		OwnerScript script = bullet.GetComponent<OwnerScript>();
		script.mother = gameObject;
	}
}
