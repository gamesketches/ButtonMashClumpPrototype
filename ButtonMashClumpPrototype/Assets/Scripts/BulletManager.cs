using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BulletManager : MonoBehaviour {
	private static BulletManager instance;
	public static BulletManager Instance {
		get {
			if(instance == null) {
				instance = GameObject.FindObjectOfType<BulletManager>();
			}
			return instance;
		}
	}

	public int maxBulletsOnScreen;

	private List<GameObject> bulletsOnScreen;

	void Start() {
		bulletsOnScreen = new List<GameObject>();
	}

	public void AddBullet(GameObject bullet) {
		bulletsOnScreen.Add(bullet);
		if(bulletsOnScreen.Count > maxBulletsOnScreen) {
			Destroy(bulletsOnScreen[0]);
			bulletsOnScreen.RemoveAt(0);
		}
	}
}
