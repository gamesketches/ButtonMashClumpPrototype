using UnityEngine;
using System.Collections;

public class OwnerScript : MonoBehaviour {

	// This script exists entirely for this line lulz
	public GameObject mother;

	public bool stray;
	public int bounces;

	private BulletType type;

	private Rigidbody2D rb2D;

	void Awake() {
		rb2D = GetComponent<Rigidbody2D>();
		//rb2D.AddTorque(100.0f);
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
			if((type == BulletType.Roundabout && otherType == BulletType.Point) ||
				(type == BulletType.Point && otherType == BulletType.Block) ||
				(type == BulletType.Block && otherType == BulletType.Roundabout)) {
				Destroy(collider.gameObject);
			}
		}
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

	public BulletType GetType() {
		return type;
	}
}
