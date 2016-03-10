using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {
	public float speed;
	public float rotateSpeed;
	//public float theCurrentAngle;

	public bool useBenAiming;

	public GameObject opponent;

	private float currentAngle;

	private Rigidbody2D rb2D;

	private Animator anim;

	private Player player;
	private Vector2 lastMovement;

	void Start() {
		player = GetComponent<Player>();
		rb2D = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
	}

	void Update() {
		HandleMovement();
	}

	void HandleMovement() {
		rb2D.velocity = (new Vector2(Input.GetAxisRaw("Horizontal" + player.number), Input.GetAxisRaw("Vertical" + player.number))).normalized * speed;
		if(rb2D.velocity.x != 0 || rb2D.velocity.y != 0) {
			lastMovement = new Vector2(rb2D.velocity.x, rb2D.velocity.y);
			float radians = Mathf.Atan2(lastMovement.y, lastMovement.x);
			float degrees = radians * Mathf.Rad2Deg;
			if(degrees >= -45.0f && degrees < 45.0f) {
				anim.SetTrigger("Walk East");
			} else if(degrees >= 45.0f && degrees < 135.0f) {
				anim.SetTrigger("Walk North");
			} else if(degrees >= 135.0f && degrees < 225.0f) {
				anim.SetTrigger("Walk West");
			} else {
				anim.SetTrigger("Walk South");
			}
			/*if(degrees >= 225.0f && degrees < 315.0f) {
				anim.SetTrigger("Walk South");
			}*/
			/*if(degrees >= 22.5f && degrees < 67.5f) {
				anim.SetTrigger("Walk East");
			} else if(degrees >= 67.5f && degrees < 112.5f) {
				anim.SetTrigger("Walk Northeast");
			} else if(degrees >= 112.5f && degrees < 157.5f) {
				anim.SetTrigger("Walk North");
			} else if(degrees >= 157.5f && degrees < 202.5f) {
				anim.SetTrigger("Walk Northwest");
			} else if(degrees >= 202.5f && degrees < 247.5f) {
				anim.SetTrigger("Walk West");
			} else if(degrees >= 247.5f && degrees < 292.5f) {
				anim.SetTrigger("Walk Southwest");
			} else if(degrees >= 292.5f && degrees < 337.5f) {
				anim.SetTrigger("Walk South");
			} else {
				anim.SetTrigger("Walk Southeast");
			}*/
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
