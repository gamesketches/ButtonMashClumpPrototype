using UnityEngine;
using System.Collections;

public class OwnerScript : MonoBehaviour {

	public GameObject mother;
	public Sprite setTexture;
	public Sprite horTexture;

    public GameObject bltCrashPrefab;

    public bool stray;
	public int bounces;

	private BulletType type;

	private Rigidbody2D rb2D;

	private GameObject target;
	private float headingTime = 0.0f;
	public float shelfLife;
	public float directionChangeRate = 1f;
    public float collisionScale;

	void Awake() {
		rb2D = GetComponent<Rigidbody2D>();
		if(shelfLife != 0.0f) {
			StartCoroutine(DoomsdayClock());
		}
		//rb2D.AddTorque(100.0f);
	}

	void Update() {
		if(type == BulletType.Roundabout && headingTime < 1.0f) {
			rb2D.velocity = Vector2.Lerp(rb2D.velocity, target.transform.position - gameObject.transform.position, 
																								headingTime);
			headingTime += directionChangeRate / (directionChangeRate * 60);
		}
	}

	void OnTriggerEnter2D(Collider2D collider) {
		string layerMask = LayerMask.LayerToName(collider.gameObject.layer);
		if(layerMask == "Players") {
			if(collider.gameObject != mother) {
                collider.gameObject.GetComponent<PlayerHealth>().TakeDamage();
                Destroy(gameObject);
            }
        } else if(layerMask == "Bounds") {
			if(stray) {
				bounces--;
				if(bounces <= 0) {
					Destroy(gameObject);
				}
			} else {
				Destroy(gameObject);
			}
		} else if(layerMask == "Bullets") {
			OwnerScript otherOwner = collider.gameObject.GetComponent<OwnerScript> ();
			if (otherOwner.mother == mother) {
				return;
			}
			BulletType otherType = otherOwner.GetType();
			if((type == otherType) || 
				(type == BulletType.Roundabout && otherType == BulletType.Point) ||
				(type == BulletType.Point && otherType == BulletType.Block) ||
				(type == BulletType.Block && otherType == BulletType.Roundabout)) {
                Debug.Log("Starting coroutine");
                StartCoroutine(PlayBulletCrash());
                Destroy(collider.gameObject);
            }
        }
	}

    IEnumerator PlayBulletCrash()
    {
        Debug.Log("inside coroutine");
        GameObject crash = ((GameObject)Instantiate(bltCrashPrefab, transform.position, Quaternion.identity));
        crash.GetComponent<Transform>().localScale = new Vector3(collisionScale, collisionScale);
        yield return null;//new WaitForSeconds(1.0f);
        Debug.Log("should be here");
        Debug.Log(crash);
        yield return null;
    }

    void OnCollisionEnter2D(Collision2D collision) {
		string layerMask = LayerMask.LayerToName(collision.gameObject.layer);
		if(layerMask == "Players") {
			if(collision.gameObject != mother) {
				collision.gameObject.GetComponent<PlayerHealth>().TakeDamage();
				Destroy(gameObject);
			}
		} else if(layerMask == "Bounds") {
			if(stray) {
				bounces--;
				if(bounces <= 0) {
					Destroy(gameObject);
				}
			} else {
				Destroy(gameObject);
			}
		}
	}

	public void SetType(BulletType bulletType){
		type = bulletType;
	}

	public void Initialize(BulletType bulletType, GameObject newMother, GameObject opponent, int playerNum) {
		mother = newMother;
		type = bulletType;
		if(playerNum == 0) {
			gameObject.GetComponent<SpriteRenderer>().sprite = setTexture;
		}
		else {
			gameObject.GetComponent<SpriteRenderer>().sprite = horTexture;
		}
		if(type == BulletType.Roundabout) {
			target = opponent;
			rb2D.velocity = rb2D.velocity * -1;
		}
	}

	public BulletType GetType() {
		return type;
	}

	IEnumerator DoomsdayClock() {
		yield return new WaitForSeconds(shelfLife);
		Destroy(gameObject);
	}
}
