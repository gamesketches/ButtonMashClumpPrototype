﻿using UnityEngine;
using System.Collections;

public class OwnerScript : MonoBehaviour {

	// This script exists entirely for this line lulz
	public GameObject mother;

	public bool stray;
	public int bounces;

	private int type;

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
			int otherType = collider.gameObject.GetComponent<OwnerScript>().GetType();
			if((type == 2 && otherType == 4) || (type == 4 && otherType == 5) || (type == 5 && otherType == 2)) {
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

	public void SetType(int t) {
		type = t;
	}

	public int GetType() {
		return type;
	}
}
