using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {
	public float speed;
	public float rotateSpeed;

	public bool useBenAiming;

	public GameObject opponent;

	private float currentAngle;

	private Rigidbody2D rb2D;

	private Player player;
	private Vector2 lastMovement;

	void Start() {
		player = GetComponent<Player>();
		rb2D = GetComponent<Rigidbody2D>();
	}

	void Update() {
		HandleMovement();
	}

	void HandleMovement() {
		rb2D.velocity = (new Vector2(Input.GetAxisRaw("Horizontal" + player.number), Input.GetAxisRaw("Vertical" + player.number))).normalized * speed;
		if(rb2D.velocity.x != 0 || rb2D.velocity.y != 0) {
			lastMovement = new Vector2(rb2D.velocity.x, rb2D.velocity.y);
		}
	}

	public void HandleRotation() {
		float targetAngle = Mathf.Atan2(opponent.transform.position.y - transform.position.y, opponent.transform.position.x - transform.position.x) * Mathf.Rad2Deg + player.number * 180.0f;
		currentAngle = Mathf.LerpAngle(currentAngle, targetAngle, rotateSpeed * Time.deltaTime);
		transform.rotation = Quaternion.Euler(0.0f, 0.0f, currentAngle);
	}

	public float currentShotAngle() {
		if(useBenAiming) {
			return transform.rotation.eulerAngles.z + 180.0f * player.number;
		} else {
			float radians = Mathf.Atan2(lastMovement.y, lastMovement.x);
			return radians * Mathf.Rad2Deg;
		}
	}
}
