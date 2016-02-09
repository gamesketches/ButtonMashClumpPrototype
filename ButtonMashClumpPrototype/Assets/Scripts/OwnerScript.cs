using UnityEngine;
using System.Collections;

public class OwnerScript : MonoBehaviour {

	// This script exists entirely for this line lulz
	public GameObject mother;

	void OnTriggerEnter2D(Collider2D collider) {
		string layerMask = LayerMask.LayerToName(collider.gameObject.layer);
		if(layerMask == "Players") {
			if(collider.gameObject != mother) {
				Debug.Log("TRIGGER WARNING!");
			}
			//Debug.Log("TRIGGER WARNING!");
		}
	}
}
