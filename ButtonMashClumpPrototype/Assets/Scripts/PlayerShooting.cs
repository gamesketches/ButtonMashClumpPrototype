using UnityEngine;
using System.Collections;

public class PlayerShooting : MonoBehaviour {
	public GameObject basicBulletPrefab;

	void Update() {
		if(Input.GetKeyDown(KeyCode.Space)) {
			Shoot(0, 0.0f, 20.0f);
		}
	}

	public void Shoot(int bulletType, float angleInDegrees, float speed) {
		Rigidbody2D bullet;
		//if(bulletType == 0) {
			bullet = ((GameObject)Instantiate (basicBulletPrefab, transform.position, Quaternion.Euler (0.0f, 0.0f, angleInDegrees))).GetComponent<Rigidbody2D> ();
		//}
		bullet.velocity = new Vector2(Mathf.Cos(angleInDegrees * Mathf.Deg2Rad), Mathf.Sin(angleInDegrees * Mathf.Deg2Rad)) * speed;
	}
}
