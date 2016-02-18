using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour {

    private Player player;
    private int health;
    public Text healthText;
	private AudioSource audioSource;

    void Start()
    {
        player = GetComponent<Player>();
		audioSource = GetComponent<AudioSource>();

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

    // Update is called once per frame
    void Update () {
	
	}
}
