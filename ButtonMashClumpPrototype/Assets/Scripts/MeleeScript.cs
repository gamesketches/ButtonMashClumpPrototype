using UnityEngine;
using System.Collections;

public class MeleeScript : MonoBehaviour {

	// This script exists entirely for this line lulz
	public GameObject mother;

	public bool nullifiesProjectiles;

	void OnTriggerEnter2D(Collider2D collider) {
		string layerMask = LayerMask.LayerToName(collider.gameObject.layer);
		if(layerMask == "Players") {
			if(collider.gameObject != mother) {
				collider.gameObject.GetComponent<PlayerHealth>().TakeDamage();
			}
		} 
		else if(layerMask == "Bullets" && collider.gameObject.GetComponent<OwnerScript>().mother != mother) {
			if(nullifiesProjectiles){
				Debug.Log("TriggerWarning");
				Destroy(collider.gameObject);
				}
		}
	}

	void OnCollisionEnter2D(Collision2D collision) {
		string layerMask = LayerMask.LayerToName(collision.gameObject.layer);
		if(layerMask == "Players") {
			if(collision.gameObject != mother) {
				collision.gameObject.GetComponent<PlayerHealth>().TakeDamage();
			}
		} 
		else if(layerMask == "Bullets" && collision.gameObject.GetComponent<OwnerScript>().mother != mother) {
			if(nullifiesProjectiles){
				Debug.Log("Collision");
				Destroy(collision.gameObject);
			}
		}
	}
}