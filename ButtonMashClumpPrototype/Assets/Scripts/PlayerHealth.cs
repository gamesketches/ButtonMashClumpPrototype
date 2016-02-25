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
    public Text victoryText;


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
        victoryText.text = "";
        health = 100;
    }

    public void TakeDamage()
    {
        Debug.Log("Im taking damage!");
        health--;
		audioSource.Play();
        
        //"GREEN BARR!!"
        if ((33 < health) & (health < 100))
        {
            healthText.color = Color.yellow;
        }
        else if ((0 <= health) & (health <= 33))
        {
            healthText.color = Color.red;
        }


        healthText.text = "";

        //display 20 bars for health 
        for (int i = 0; i < health / 5; i++)
        {
            healthText.text += "|";
        }
        
        //magic pixel 
        if ((0 < health) & (health < 5))
        {
            healthText.text += "|";
        }

        //why dont they just make a 4player version??
        healthText.text += player.number == 1 ? "\nHORUS" : "\nSETT";

        StartCoroutine(hitFlash());

		if(health <= 0) {
			string color = player.number == 1 ? "HORUS\n" : "SETT\n";
            healthText.text = color + "Loses :(";
            string victory = player.number == 1 ? "SETT  \n" : "HORUS\n";
            victoryText.text = victory + "WINS";
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
