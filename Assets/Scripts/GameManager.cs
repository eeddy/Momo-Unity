using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Image p1h1, p1h2, p1h3, p2h1, p2h2, p2h3; //Hearts
    public Sprite fullHeart;
    public Sprite emptyHeart;

    public GameObject momo1, momo2; 

    private int player1Lives; 
    private int player2Lives; 

    private int player1Coins;
    private int player2Coins;

    // Start is called before the first frame update
    void Start()
    {
        //Set initial player lives 
        player1Lives = 3;
        player2Lives = 3; 
        //Set initial coins
        player1Coins = 0;
        player2Coins = 0;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateLives();
    }

    void UpdateLives() 
    {
        UpdateplayerLives(player1Lives, p1h1, p1h2, p1h3);
        UpdateplayerLives(player2Lives, p2h1, p2h2, p2h3);
    }

    void UpdateplayerLives(int numLives, Image heart1, Image heart2, Image heart3) {
        if (numLives == 2) {
            heart3.sprite = emptyHeart;
        } else if (numLives == 1) {
            heart2.sprite = emptyHeart;
        } else {
            heart1.sprite = emptyHeart;
        }
    }
}
