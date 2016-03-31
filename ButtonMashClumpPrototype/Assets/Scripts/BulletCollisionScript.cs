using UnityEngine;
using System.Collections;

public class BulletCollisionScript : MonoBehaviour {

	public int framesOfExistence;
	public Color startColor;
	public Color endColor;
	private Renderer quadColor;
	private int startFrames;

    
	// Use this for initialization
	void Start () {
		quadColor = gameObject.GetComponentsInChildren<Renderer>()[0];
		startFrames = framesOfExistence;
		startColor.a = 1.0f;
		endColor.a = 1.0f;
	}
	
	// Update is called once per frame
	void Update () {
		if(framesOfExistence > 0 ) {
			framesOfExistence--;
			//quadColor.material.color = Color.Lerp(endColor, startColor, 
				//						(float)framesOfExistence / (float)startFrames);
		}
		else {
			Destroy(gameObject);
		}
	}
}
