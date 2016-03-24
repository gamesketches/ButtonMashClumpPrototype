using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {
	public float speed;
	public float rotateSpeed;
	public float reticleRadius;
	public float reticleBlastRadius;

	public bool useBenAiming;

	public Gradient reticleGradient;

	public GameObject opponent;
	public GameObject reticle;
	public GameObject reticleBlast;

	public SpriteRenderer reticleRenderer;

	private float currentAngle;

	private Rigidbody2D rb2D;

	private Animator anim;

	private Player player;
	private Vector2 lastMovement;

	private float radians;
	private float degrees;

	void Start() {
		player = GetComponent<Player>();
		rb2D = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
		//reticleRenderer = GetComponent<SpriteRenderer>();
		reticleRenderer.color = reticleGradient.Evaluate(0.0f);
	}

	void Update() {
		//reticleRenderer.color = reticleGradient.Evaluate(
		HandleMovement();
	}

	void HandleMovement() {
		rb2D.velocity = (new Vector2(Input.GetAxisRaw("Horizontal" + player.number), Input.GetAxisRaw("Vertical" + player.number))).normalized * speed;
		if(rb2D.velocity.x != 0 || rb2D.velocity.y != 0) {
			lastMovement = new Vector2(rb2D.velocity.x, rb2D.velocity.y);
			radians = Mathf.Atan2(lastMovement.y, lastMovement.x);
			degrees = radians * Mathf.Rad2Deg;
			if(degrees >= -45.0f && degrees < 45.0f) {
				anim.SetTrigger("Walk East");
			} else if(degrees >= 45.0f && degrees < 135.0f) {
				anim.SetTrigger("Walk North");
			} else if(degrees >= 135.0f && degrees < 225.0f) {
				anim.SetTrigger("Walk West");
			} else {
				anim.SetTrigger("Walk South");
			}
		}
		reticle.transform.localPosition = new Vector3(Mathf.Cos(radians) * reticleRadius, Mathf.Sin(radians) * reticleRadius, 0.0f);
		reticle.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, degrees - 90.0f);
		reticleBlast.transform.localPosition = new Vector3(Mathf.Cos(radians) * reticleBlastRadius, Mathf.Sin(radians) * reticleBlastRadius, 0.0f);
		reticleBlast.transform.localRotation = reticle.transform.localRotation;
	}

	public void HandleRotation() {
		float targetAngle = Mathf.Atan2(opponent.transform.position.y - transform.position.y, opponent.transform.position.x - transform.position.x) * Mathf.Rad2Deg + player.number * 180.0f;
		currentAngle = Mathf.LerpAngle(currentAngle, targetAngle, rotateSpeed * Time.deltaTime);
		transform.rotation = Quaternion.Euler(0.0f, 0.0f, currentAngle);
	}

	public void PassBufferToReticle(int bufferIndex, int mashBufferSize) {
		reticleRenderer.color = reticleGradient.Evaluate((float)bufferIndex / mashBufferSize);
	}

	public void ResetReticle() {
		reticleRenderer.color = reticleGradient.Evaluate(0.0f);
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
