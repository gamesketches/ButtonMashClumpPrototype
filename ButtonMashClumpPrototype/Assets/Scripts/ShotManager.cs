using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public enum BulletType {Basic, Stray, Point, Block, Roundabout};

public class ShotManager : MonoBehaviour {

	public GameObject basicBulletPrefab;
	public GameObject strayBulletPrefab;
	public GameObject meleeAttackPrefab;
	public GameObject squareBulletPrefab;
	public GameObject xBulletPrefab;
	public GameObject circleBulletPrefab;
	public GameObject triangleBulletPrefab;
	public int meleeAttackDuration;

	private int mashBufferSize;
	private PlayerMovement movementManager;

	public delegate void ShotInterpreter(char[] mashBuffer);
	public ShotInterpreter shotInterpreter;

	void Start() {
		movementManager = gameObject.GetComponent<PlayerMovement>();
		shotInterpreter = InputEqualsNumber;
	}

	public void SetMashBufferSize(int bufferSize) {
		mashBufferSize = bufferSize;
	}

	public void TallyInputs(out int num1, out int num2, char[] mashBuffer) {
		num1 = 0;
		num2 = 0;
		for(int i = 0; i < mashBufferSize; i++) {
			if(mashBuffer[i] == 'A' || mashBuffer[i] == 'C') {
				num1++;
			}
			else if(mashBuffer[i] == 'B' || mashBuffer[i] == 'D') {
				num2++;
			}
		}
	}

	public void createBullet(float angle, float speed = 10.0f, BulletType type = BulletType.Basic){//int bulletType = 0) {
		GameObject bullet = null;
		Rigidbody2D bulletRB;
		angle += movementManager.currentShotAngle();
		if(type == BulletType.Basic){//bulletType == 0) {
			bullet = ((GameObject)Instantiate (basicBulletPrefab, transform.position, 
				Quaternion.Euler (0.0f, 0.0f, 0.0f)));
		} else if(type == BulletType.Stray){//bulletType == 1) {
			bullet = ((GameObject)Instantiate (strayBulletPrefab, transform.position, 
				Quaternion.Euler (0.0f, 0.0f, 0.0f)));
		} else if(type == BulletType.Block){//2) {
			bullet = ((GameObject)Instantiate (squareBulletPrefab, transform.position, 
				Quaternion.Euler (0.0f, 0.0f, 0.0f)));
		} else if(type == BulletType.Roundabout){//bulletType == 4) {
			bullet = ((GameObject)Instantiate (circleBulletPrefab, transform.position, 
				Quaternion.Euler (0.0f, 0.0f, 0.0f)));
		} else if(type == BulletType.Point){//bulletType == 5) {
			bullet = ((GameObject)Instantiate (triangleBulletPrefab, transform.position, 
				Quaternion.Euler (0.0f, 0.0f, 0.0f)));
		}
		bulletRB = bullet.GetComponent<Rigidbody2D> ();

		bulletRB.rotation = angle - 90f;

		bulletRB.velocity = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)) * speed;

		OwnerScript script = bullet.GetComponent<OwnerScript>();
		script.mother = gameObject;
		script.SetType(type);
	}

	public void InputEqualsNumber(char[] mashBuffer) {
		int bulletNumber = 0;
		List<char> meaningfulInput = new List<char>();
		for(int i = 0; i < mashBufferSize; i++) {
			if(mashBuffer[i] != '*') {
				bulletNumber++;
				meaningfulInput.Add(mashBuffer[i]);
			}
		}
		float angleDifference = 90.0f / mashBufferSize;
		List<float> bulletAngles = new List<float>();
		List<BulletType> bulletTypes = new List<BulletType>();

		bulletAngles.Add(0.0f);
		bulletTypes.Insert(0, (BulletType)Random.Range(2, 4));
		if(bulletNumber == mashBufferSize) {
			bulletAngles.Add(90.0f);
			bulletAngles.Add(-90.0f);
			bulletTypes.Insert(0, (BulletType)Random.Range(2, 4));
			bulletTypes.Insert(0, (BulletType)Random.Range(2, 4));
		}

		if(bulletNumber > 1) {
			for(int i = 0; i < bulletNumber - 1; i++) {
				bulletAngles.Add(angleDifference * (i + 1));
				bulletAngles.Add(-angleDifference * (i + 1));
				if(meaningfulInput[i] == 'A') {
					bulletTypes.Add(BulletType.Point);
					bulletTypes.Add(BulletType.Point);
				} else if(meaningfulInput[i] == 'B') {
					bulletTypes.Add(BulletType.Block);
					bulletTypes.Add(BulletType.Block);
				} else if(meaningfulInput[i] == 'C') {
					bulletTypes.Add(BulletType.Roundabout);
					bulletTypes.Add(BulletType.Roundabout);
				} 	
			}
		}
		for(int i = 0; i < bulletAngles.Count; i++) {
			createBullet(bulletAngles[i], Random.Range(15.0f, 25.0f), bulletTypes[i]);
		}
	}

	//ski's melee - similar to charged 360 degree attack in Zelda LTTP
	public void InputMeleeAttacksSki(char[] mashBuffer)
	{
		int aCount, bCount;

		TallyInputs(out aCount, out bCount, mashBuffer);

		float width = bCount * 0.75f;
		width = 4.3f;
		float height = 5f;
		int totalCount = aCount + bCount;

		// dem Lupin III references
		GameObject monkeyPunch;
		monkeyPunch = ((GameObject)Instantiate(meleeAttackPrefab, transform.position,
			Quaternion.Euler(0.0f, 0.0f, 0.0f)));

		monkeyPunch.transform.parent = transform;

		MeleeScript script = monkeyPunch.GetComponent<MeleeScript>();
		script.mother = gameObject;


		monkeyPunch.GetComponents<AudioSource>()[0].Play();

		StartCoroutine(SpinWeapon(monkeyPunch, meleeAttackDuration, width));

	}

	IEnumerator SpinWeapon(GameObject monkeyPunch, int totalCount, float width)
	{


		monkeyPunch.transform.localScale = new Vector3(width, 0.5f, 0); //we'll just give it dimensions for now
		monkeyPunch.transform.localPosition = Vector3.right * width / 2.0f; //extend it like sword


		for (int i = 0; i < totalCount * 5; i++)
		{
			//monkeyPunch.transform.Rotate(Vector3.forward, i * 20);
			monkeyPunch.transform.localRotation = Quaternion.Euler(Vector3.zero);
			transform.Rotate(Vector3.forward, i);
			monkeyPunch.GetComponent<Rigidbody2D>().MoveRotation(monkeyPunch.GetComponent<Rigidbody2D>().rotation * Mathf.Rad2Deg + i * 3 * Mathf.Rad2Deg);
			yield return null;
		}

		Destroy(monkeyPunch);

	}

	public void InputsEqualAngle(char[] mashBuffer) {
		float angle = 0.0f;
		int numAs, numBs;
		TallyInputs(out numAs, out numBs, mashBuffer);
		angle += 10.0f * numAs;
		angle -= 10.0f * numBs;
		createBullet(angle);
	}

	public void InputEqualsSets(char[] mashBuffer) {
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

	public void InputPatterns(char[] mashBuffer) {
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



	public void InputEqualsRandom(char[] mashBuffer) {
		int bulletNumber = 0;
		List<BulletType> bulletTypes = new List<BulletType>();
		for(int i = 0; i < mashBufferSize; i++) {
			if(mashBuffer[i] != '*') {
				bulletNumber++;
				if(mashBuffer[i] == 'A') {
					bulletTypes.Add(BulletType.Point);
				} else if(mashBuffer[i] == 'B') {
					bulletTypes.Add(BulletType.Block);
				} else if(mashBuffer[i] == 'C') {
					bulletTypes.Add(BulletType.Roundabout);
				} /*else if(mashBuffer[i] == 'D') {
					bulletTypes.Add(5);
				}*/
			}
		}

		for(int i = 0; i < bulletNumber; i++) {
			createBullet(Random.Range(0.0f, 360.0f), Random.Range(15.0f, 25.0f), bulletTypes[i]);
		}
	}

	public void InputMeleeAttacks(char[] mashBuffer) {
		int aCount, bCount;

		TallyInputs(out aCount, out bCount, mashBuffer);

		float width = aCount * 0.3f;
		float height = bCount * 0.3f;

		// dem Lupin III references
		GameObject monkeyPunch;

		monkeyPunch = ((GameObject)Instantiate(meleeAttackPrefab, transform.position,
			Quaternion.Euler(0.0f, 0.0f, 0.0f)));

		monkeyPunch.GetComponent<AudioSource>().Play();

		Destroy(monkeyPunch, 0.5f);
	}

}
