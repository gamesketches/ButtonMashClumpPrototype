using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RoundTimer : MonoBehaviour {

    Text m_nameText;
    private GameObject m_oTextObject;
    public float roundTime = 31.0f;
    bool gameOver;

    Text victoryText;

    GameObject player0, player1;

    void Start () {
        m_oTextObject = new GameObject();
        m_oTextObject.transform.parent = GameObject.Find("Canvas").transform;
        m_nameText = m_oTextObject.AddComponent<Text>();

        m_oTextObject.name = "Round Timer";
        m_nameText.verticalOverflow = VerticalWrapMode.Overflow;
        m_nameText.horizontalOverflow = HorizontalWrapMode.Overflow;
        m_nameText.alignment = TextAnchor.MiddleCenter;

        m_nameText.fontSize = 48;
        m_nameText.font = GetComponentInChildren<Text>().font;
        m_nameText.text = roundTime.ToString();

        m_oTextObject.transform.localPosition = Vector3.zero + new Vector3(0, 210f, 0);

        player0 = GameObject.Find("Player0");
        player1 = GameObject.Find("Player1");
        victoryText = GameObject.Find("Victory Text").GetComponent<Text>();
        gameOver = false;
    }

    // Update is called once per frame
    void Update()
    {
        //how to do this more efficiently?
        if (!gameOver)
        {
            if (Mathf.Floor(roundTime) == 0)
            {
                Invoke("TimeOver", 1.0f);
            }
            else
            {
                roundTime -= Time.deltaTime;
                m_nameText.text = ((int)roundTime).ToString();
            }
        }
    }

    void TimeOver()
    {
        victoryText.text = "TIME \nOVER";
        gameOver = true;
        Invoke("TimeOverWinner", 2.0f);
    }

    void TimeOverWinner()
    {

        int p0_health = player0.GetComponent<PlayerHealth>().returnHealth();
        int p1_health = player1.GetComponent<PlayerHealth>().returnHealth();


        Debug.Log("p1 health is " + p0_health + "p2 health is" + p1_health);

        if (p0_health < p1_health)
        {
            TimeKill(player0);
        } else if (p0_health > p1_health)
        {
            TimeKill(player1);
        }
        else if (p0_health == p1_health)
        {
            TieGame();
        }
    }

    //throw a bunch of damage at him to kill him for now its most convenient.
    void TimeKill(GameObject player)
    {
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();

        while (playerHealth.GetComponent<PlayerHealth>().returnHealth() > 0)
            playerHealth.TakeDamage();
    }

    void TieGame()
    {
        victoryText.text = "DRAW \nGAME";
        gameOver = true;
        Invoke("sceneReset", 2.0f);
    }

    void sceneReset()
    {
        Application.LoadLevel(Application.loadedLevel);
    }
}
