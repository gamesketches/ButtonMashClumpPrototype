using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {
	public float speed;

	private Rigidbody2D rb2D;

	private Player player;

	void Start() {
		player = GetComponent<Player>();
		rb2D = GetComponent<Rigidbody2D>();
	}

	void Update() {
		rb2D.velocity = (new Vector2(Input.GetAxisRaw("Horizontal" + player.number), Input.GetAxisRaw("Vertical" + player.number))).normalized * speed;
	}
}
