using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour {

    private Player player;
    private int health;
    public Text healthText;
	private AudioSource audio;

    void Start()
    {
        player = GetComponent<Player>();
		audio = GetComponent<AudioSource>();

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
		audio.Play();
        if (player.number == 1)
        {
            healthText.text = "Blue\n" + health.ToString();
        } else if (player.number == 0)
        {
            healthText.text = "White\n" + health.ToString();
        }
    }

    // Update is called once per frame
    void Update () {
	
	}
}
