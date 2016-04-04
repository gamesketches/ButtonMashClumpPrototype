using UnityEngine;
using System.Collections;

public class ShowWins : MonoBehaviour {

    private Player player;

    // Use this for initialization
    void Start ()
    {
        player = GetComponent<Player>();
        if (player.GetComponent<PlayerHealth>().ReturnWinCount() > 0)
        {
            Debug.Log("win count in showwins" + player.GetComponent<PlayerHealth>().ReturnWinCount());
            gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update () {
	
	}
}
