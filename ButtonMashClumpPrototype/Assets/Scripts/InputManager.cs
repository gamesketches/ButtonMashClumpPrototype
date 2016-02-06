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
		InputPatterns();
		InputEqualsNumber();
		Debug.Log("Fire!");
		/*for(int i = 0; i < mashBufferSize; i++){
			Debug.Log(mashBuffer[i]);
		}*/
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

	void InputPatterns() {
		string pattern = "";

		string aa = "AA";
		string bb = "BB";
		string ab = "AB";
		string ba = "BA";

		for(int i = 0; i < mashBufferSize;i++){
			pattern = string.Concat(mashBuffer[i].ToString(), mashBuffer[++i].ToString());
			if(pattern.CompareTo(aa) == 1){
				Rigidbody2D bullet;

				bullet = ((GameObject)Instantiate (basicBulletPrefab, transform.position, 
					Quaternion.Euler (0.0f, 0.0f, 10.0f))).GetComponent<Rigidbody2D> ();
				bullet.velocity = new Vector2(Mathf.Cos(10.0f * Mathf.Deg2Rad), Mathf.Sin(10.0f * Mathf.Deg2Rad)) * 10;

				bullet = ((GameObject)Instantiate (basicBulletPrefab, transform.position, 
					Quaternion.Euler (0.0f, 0.0f, 0.0f))).GetComponent<Rigidbody2D> ();

				bullet.velocity = new Vector2(Mathf.Cos(0.0f * Mathf.Deg2Rad), Mathf.Sin(0.0f * Mathf.Deg2Rad)) * 10;

				bullet = ((GameObject)Instantiate (basicBulletPrefab, transform.position, 
					Quaternion.Euler (0.0f, 0.0f, -10.0f))).GetComponent<Rigidbody2D> ();

				bullet.velocity = new Vector2(Mathf.Cos(-10.0f * Mathf.Deg2Rad), Mathf.Sin(-10.0f * Mathf.Deg2Rad)) * 10;
			}

			else if(pattern.CompareTo(bb) == 1) {
				Rigidbody2D bullet;

				bullet = ((GameObject)Instantiate (basicBulletPrefab, transform.position, 
					Quaternion.Euler (0.0f, 0.0f, 100.0f))).GetComponent<Rigidbody2D> ();
				bullet.velocity = new Vector2(Mathf.Cos(100.0f * Mathf.Deg2Rad), Mathf.Sin(100.0f * Mathf.Deg2Rad)) * 10;

				bullet = ((GameObject)Instantiate (basicBulletPrefab, transform.position, 
					Quaternion.Euler (0.0f, 0.0f, 90.0f))).GetComponent<Rigidbody2D> ();

				bullet.velocity = new Vector2(Mathf.Cos(90.0f * Mathf.Deg2Rad), Mathf.Sin(90.0f * Mathf.Deg2Rad)) * 10;

				bullet = ((GameObject)Instantiate (basicBulletPrefab, transform.position, 
					Quaternion.Euler (0.0f, 0.0f, 80.0f))).GetComponent<Rigidbody2D> ();

				bullet.velocity = new Vector2(Mathf.Cos(80.0f * Mathf.Deg2Rad), Mathf.Sin(80.0f * Mathf.Deg2Rad)) * 10;
			}
			else if(pattern.CompareTo(ab) == 1) {
				Rigidbody2D bullet;

				bullet = ((GameObject)Instantiate (basicBulletPrefab, transform.position, 
					Quaternion.Euler (0.0f, 0.0f, -90.0f))).GetComponent<Rigidbody2D> ();
				bullet.velocity = new Vector2(Mathf.Cos(-90.0f * Mathf.Deg2Rad), Mathf.Sin(-90.0f * Mathf.Deg2Rad)) * 10;

				bullet = ((GameObject)Instantiate (basicBulletPrefab, transform.position, 
					Quaternion.Euler (0.0f, 0.0f, -80.0f))).GetComponent<Rigidbody2D> ();

				bullet.velocity = new Vector2(Mathf.Cos(-80.0f * Mathf.Deg2Rad), Mathf.Sin(-80.0f * Mathf.Deg2Rad)) * 10;

				bullet = ((GameObject)Instantiate (basicBulletPrefab, transform.position, 
					Quaternion.Euler (0.0f, 0.0f, -100.0f))).GetComponent<Rigidbody2D> ();

				bullet.velocity = new Vector2(Mathf.Cos(-100.0f * Mathf.Deg2Rad), Mathf.Sin(-100.0f * Mathf.Deg2Rad)) * 10;
			}
			else if(pattern.CompareTo(ba) == 1){
				Rigidbody2D bullet;

				bullet = ((GameObject)Instantiate (basicBulletPrefab, transform.position, 
					Quaternion.Euler (0.0f, 0.0f, 180.0f))).GetComponent<Rigidbody2D> ();
				bullet.velocity = new Vector2(Mathf.Cos(180.0f * Mathf.Deg2Rad), Mathf.Sin(180.0f * Mathf.Deg2Rad)) * 10;

				bullet = ((GameObject)Instantiate (basicBulletPrefab, transform.position, 
					Quaternion.Euler (0.0f, 0.0f, 190.0f))).GetComponent<Rigidbody2D> ();

				bullet.velocity = new Vector2(Mathf.Cos(190.0f * Mathf.Deg2Rad), Mathf.Sin(190.0f * Mathf.Deg2Rad)) * 10;

				bullet = ((GameObject)Instantiate (basicBulletPrefab, transform.position, 
					Quaternion.Euler (0.0f, 0.0f, 200.0f))).GetComponent<Rigidbody2D> ();

				bullet.velocity = new Vector2(Mathf.Cos(200.0f * Mathf.Deg2Rad), Mathf.Sin(200.0f * Mathf.Deg2Rad)) * 10;
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
				bulletAngles.Add(angleDifference);
				bulletAngles.Add(-angleDifference);
			}
		} else {
			Debug.Log("Uneven");
			angleDifference = 90.0f / (bulletNumber - 1);
			bulletAngles.Add(0.0f);
			for(int i = 1; i < bulletNumber - 1; i++) {
				bulletAngles.Add(angleDifference * i);
				bulletAngles.Add(-angleDifference * i);
			}
		}
		Rigidbody2D bullet;
		for(int i = 0; i < bulletAngles.Count; i++) {
			float degrees = bulletAngles[i];
			float radians = degrees * Mathf.Deg2Rad;
			bullet = ((GameObject)Instantiate (basicBulletPrefab, transform.position, 
				Quaternion.Euler(0.0f, 0.0f, degrees))).GetComponent<Rigidbody2D> ();
			bullet.velocity = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians)) * 10;
		}
	}
}
