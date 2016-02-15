using UnityEngine;
using System.Collections;

public class BloodScript : MonoBehaviour {

	public int framesOfExistence;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(framesOfExistence > 0 ) {
			framesOfExistence--;
		}
		else {
			Destroy(gameObject);
		}
	}
}
