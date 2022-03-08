using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Image p1h1, p1h2, p1h3, p2h1, p2h2, p2h3; //Hearts
    public Text p1Score, p2Score;
    public Sprite fullHeart;
    public Sprite emptyHeart;
    public FloorManager p1FloorManager, p2FloorManager;

    public GameObject momo1, momo2, p1Spikes, p2Spikes; 

    private float previousTime = 0;
    private int player1Lives; 
    private int player2Lives; 

    private int player1Score;
    private int player2Score;

    bool player1WallDangerMode = false;
    bool player2WallDangerMode = false;

    private bool p1Touching, p2Touching;

    private SoundManager soundManager;

    public int Player1Lives { get => player1Lives; set => player1Lives = value; }
    public int Player2Lives { get => player2Lives; set => player2Lives = value; }

    // Start is called before the first frame update
    void Start()
    {
        //Set initial player lives 
        Player1Lives = 2;
        Player2Lives = 2; 
        //Set initial coins
        player1Score = 0;
        player2Score = 0;
        p1Touching = false;
        p2Touching = false;
        
        soundManager = FindObjectOfType<SoundManager>();
    }

    // Update is called once per frame
    void Update()
    {
        p1Score.text = player1Score + "";
        p2Score.text = player2Score + "";
        //Increase score every 5 seconds
        if (Time.timeSinceLevelLoad - previousTime > 5) {
            previousTime = Time.time;
            player1Score+=5;
            player2Score+=5;
        }
        UpdateLives();
        CheckForCoinCollisions();
        CheckForWallCollisions();
        CheckForSpikeTouch();
    }

    void CheckForSpikeTouch() 
    {
        if (p1Spikes.GetComponent<BoxCollider2D>().IsTouching(momo1.GetComponent<BoxCollider2D>()) && !p1Touching) {
            p1Touching = true;
            p1FloorManager.Reset();
            soundManager.PlayLoseLifeSound();
            Player1Lives--;
        } else if(!p1Spikes.GetComponent<BoxCollider2D>().IsTouching(momo1.GetComponent<BoxCollider2D>())) {
            p1Touching = false;
        }
        if (p2Spikes.GetComponent<BoxCollider2D>().IsTouching(momo2.GetComponent<BoxCollider2D>()) && !p2Touching) {
            p2Touching = true;
            p2FloorManager.Reset();
            soundManager.PlayLoseLifeSound();
            Player2Lives--;
        } else if(!p2Spikes.GetComponent<BoxCollider2D>().IsTouching(momo2.GetComponent<BoxCollider2D>())) {
            p2Touching = false;
        }
    }

    void CheckForCoinCollisions() 
    {
        //Player 1:
        for(int i=0; i<p1FloorManager.coins.Count; i++) {
            if(momo1.GetComponent<BoxCollider2D>().bounds.Contains(p1FloorManager.coins[i].transform.position))
            {
                LifeCoinCheckP1(i);

                soundManager.PlayCoinCollectedSound();
                player1Score += 5;
                Destroy(p1FloorManager.coins[i].gameObject);
                p1FloorManager.coins.RemoveAt(i);
            }
        }
        //Player 2:
        for(int i=0; i<p2FloorManager.coins.Count; i++) {
            if(momo2.GetComponent<BoxCollider2D>().bounds.Contains(p2FloorManager.coins[i].transform.position))
            {
                LifeCoinCheckP2(i);
                soundManager.PlayCoinCollectedSound();
                player2Score += 5;
                Destroy(p2FloorManager.coins[i].gameObject);
                p2FloorManager.coins.RemoveAt(i);
            }
        }
    }

    private void LifeCoinCheckP1(int i)
    {
        if (p1FloorManager.coins[i].name.Equals("LifeCoinS"))
        {
            LifeCoinEvent(1, "S");
        }
        else if (p1FloorManager.coins[i].name.Equals("LifeCoinP"))
        {
            LifeCoinEvent(1, "P");
        }
    }

    private void LifeCoinCheckP2(int i)
    {
        if (p2FloorManager.coins[i].name.Equals("LifeCoinS"))
        {
            LifeCoinEvent(2, "S");
        }
        else if (p2FloorManager.coins[i].name.Equals("LifeCoinP"))
        {
            LifeCoinEvent(2, "P");
        }
    }

    private void LifeCoinEvent(int playerNum, string coinType)
    {
        if (playerNum == 1)
        {
            if (coinType == "S")
            {
                Player1Lives++;
            }
            else if (coinType == "P")
            {
                Player2Lives--;
            }
            
        
        }
        else if (playerNum == 2)
        {
            if(coinType == "S")
            {
                Player2Lives++;
            }
            else if (coinType == "P")
            {
                Player1Lives--;
            }

        }        
    }

    private void CheckForWallCollisions()
    {
        for (int i = 0; i < p1FloorManager.obstacles.Count; i++)
        {
            if (momo1.GetComponent<BoxCollider2D>().bounds.Contains(p1FloorManager.obstacles[i].transform.position))
            {
                if(player1WallDangerMode) { player1Lives--; }
            }
        }
        //Player 2:
        for (int i = 0; i < p2FloorManager.coins.Count; i++)
        {
            if (momo2.GetComponent<BoxCollider2D>().bounds.Contains(p2FloorManager.obstacles[i].transform.position))
            {
                if (player2WallDangerMode) { player2Lives--; }
            }
        }
    }
    
    private void WallCoinCheckp1(int i)
    {
        if (p1FloorManager.coins[i].name.Equals("LifeCoinS"))
        {
            LifeCoinEvent(1, "S");
        }
        else if (p1FloorManager.coins[i].name.Equals("LifeCoinP"))
        {
            LifeCoinEvent(1, "P");
        }
    }
    void UpdateLives() 
    {
        UpdateplayerLives(Player1Lives, p1h1, p1h2, p1h3);
        UpdateplayerLives(Player2Lives, p2h1, p2h2, p2h3);
    }

    void UpdateplayerLives(int numLives, Image heart1, Image heart2, Image heart3) {
        heart1.sprite = fullHeart;
        heart2.sprite = fullHeart;
        heart3.sprite = fullHeart;
        if (numLives < 3) {
            heart3.sprite = emptyHeart;
        } 
        if (numLives < 2) {
            heart2.sprite = emptyHeart;
        } 
        if (numLives < 1) {
            heart1.sprite = emptyHeart;
        }
    }


}
