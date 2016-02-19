using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour {

    private Player player;
    private int health;
    public Text healthText;
	public int flashFrames;
	public int flashXTimes;
	public Color flashColor;
	private Color startColor;
	private AudioSource audioSource;
	private Renderer quadRenderer;


    void Start()
    {
        player = GetComponent<Player>();
		audioSource = GetComponent<AudioSource>();
		quadRenderer = GetComponentInChildren<Renderer>();
		startColor = quadRenderer.material.color;

        /*GameObject theCanvas = GameObject.Find("Canvas");
        health_texts = theCanvas.GetComponents<TextMesh>();
        foreach (TextMesh text in health_texts)
        {
            if (text.GetComponent<Player>().number == player.number)
            {
                health_text = text; //finding this players text
            }

        }*/
        health = 100;
    }

    public void TakeDamage()
    {
        Debug.Log("Im taking damage!");
        health--;
		audioSource.Play();
        if (player.number == 1)
        {
            healthText.text = "Blue\n" + health.ToString();
        } else if (player.number == 0)
        {
            healthText.text = "White\n" + health.ToString();
        }

		StartCoroutine(hitFlash());

		if(health <= 0) {
			string color = player.number == 1 ? "Blue\n" : "White\n";
			healthText.text = color + "Loses :(";
			StartCoroutine(sceneReset());
		}
    }

	IEnumerator sceneReset() {
		yield return new WaitForSeconds(2);
		Application.LoadLevel(Application.loadedLevel);
	}

	IEnumerator hitFlash() {
		for(int i = 0; i < flashXTimes; i++) {
			int temp = flashFrames;
			while(temp >= 0) {
				temp--;
				yield return null;
			}
			quadRenderer.material.color = quadRenderer.material.color == flashColor ? startColor : flashColor;
		}

		// Make sure we end where we started
		quadRenderer.material.color = startColor;
	}
    // Update is called once per frame
    void Update () {
	
	}
}
