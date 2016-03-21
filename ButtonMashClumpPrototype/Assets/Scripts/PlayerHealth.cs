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

    public Text winCountText;
    private static int winCount = 0;
    public Text nameText;
    public GameObject opponent;

    bool deadNow;

    void Start()
    {
        player = GetComponent<Player>();
		audioSource = GetComponent<AudioSource>();
		quadRenderer = GetComponentInChildren<Renderer>();
		startColor = quadRenderer.material.color;

        victoryText.text = "";
        health = 100;
     

        deadNow = false;
        if (winCount > 0)
        {
            winCountText.gameObject.SetActive(true);
        }
        Debug.Log("win count = " + winCount);
    }

    public void TakeDamage()
    {

        if (deadNow)
            return;
        Debug.Log("Im taking damage!");
        health-=3;
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
        healthText.text += player.number == 1 ? "\nHORUS" : "\nSET";

        StartCoroutine(hitFlash());

		if(health <= 0) { // GAME IS ENDING - CURRENT PLAYER IS THE LOSER
            nameText.gameObject.SetActive(false);
            deadNow = true;


            //if (player.number == 1)
            {
                Debug.Log("player " + player.number + "wins");
                //quick way to add a win to opponent
                GetComponent<PlayerHealth>().opponent.GetComponent<PlayerHealth>().addWin();
            }

            string color = player.number == 1 ? "HORUS\n" : "SET\n";
            healthText.color = player.number == 1 ? Color.blue : Color.red;
            healthText.text = color + "Loses :(";
            string victory = player.number == 1 ? "SET  \n" : "HORUS\n";
			victoryText.color = player.number == 1 ? Color.red : Color.blue;
            victoryText.text = victory + "WINS";

            StartCoroutine(sceneReset(player.number));
		}
    }

	IEnumerator sceneReset(int playerNum) {
		DialogueManager dialogue = Camera.main.GetComponent<DialogueManager>();
		dialogue.StartScene(playerNum);
		while(dialogue.inScene) {
			yield return null;
		}
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

    public int returnHealth()
    {
        return health;
    }

    public void addWin()
    {
        winCount++;
        winCountText.text = winCount + " WINS";
        if (winCount > 0 && winCountText.IsActive() == false)
        {
            winCountText.gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update () {
	
	}
}
