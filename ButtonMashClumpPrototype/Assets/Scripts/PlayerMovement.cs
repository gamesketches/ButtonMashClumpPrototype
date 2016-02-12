using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {
	public float speed;

	private Rigidbody2D rb2D;

	private Player player;
	private Vector2 lastMovement;

	void Start() {
		player = GetComponent<Player>();
		rb2D = GetComponent<Rigidbody2D>();
	}

	void Update() {
		rb2D.velocity = (new Vector2(Input.GetAxisRaw("Horizontal" + player.number), Input.GetAxisRaw("Vertical" + player.number))).normalized * speed;
		if(rb2D.velocity.x != 0 || rb2D.velocity.y != 0) {
			lastMovement = new Vector2(rb2D.velocity.x, rb2D.velocity.y);
		}
	}

	public float currentShotAngle() {
		float radians = Mathf.Atan2(lastMovement.y, lastMovement.x);
		return radians * Mathf.Rad2Deg;
	}
}
