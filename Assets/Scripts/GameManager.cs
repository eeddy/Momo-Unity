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

    private bool p1Touching, p2Touching;

    private SoundManager soundManager;

    // Start is called before the first frame update
    void Start()
    {
        //Set initial player lives 
        player1Lives = 3;
        player2Lives = 3; 
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
        CheckForCoinCollisisons();
        CheckForSpikeTouch();
    }

    void CheckForSpikeTouch() 
    {
        if (p1Spikes.GetComponent<BoxCollider2D>().IsTouching(momo1.GetComponent<BoxCollider2D>()) && !p1Touching) {
            p1Touching = true;
            p1FloorManager.Reset();
            soundManager.PlayLoseLifeSound();
            player1Lives--;
        } else if(!p1Spikes.GetComponent<BoxCollider2D>().IsTouching(momo1.GetComponent<BoxCollider2D>())) {
            p1Touching = false;
        }
        if (p2Spikes.GetComponent<BoxCollider2D>().IsTouching(momo2.GetComponent<BoxCollider2D>()) && !p2Touching) {
            p2Touching = true;
            p2FloorManager.Reset();
            soundManager.PlayLoseLifeSound();
            player2Lives--;
        } else if(!p2Spikes.GetComponent<BoxCollider2D>().IsTouching(momo2.GetComponent<BoxCollider2D>())) {
            p2Touching = false;
        }
    }

    void CheckForCoinCollisisons() 
    {
        //Player 1:
        for(int i=0; i<p1FloorManager.coins.Count; i++) {
            if(momo1.GetComponent<BoxCollider2D>().bounds.Contains(p1FloorManager.coins[i].transform.position)) {
                if(p1FloorManager.coins[i].name.Equals("LifeCoinS"))
                {
                    LifeCoinEvent(1,"S");
                }
                else if (p1FloorManager.coins[i].name.Equals("LifeCoinP"))
                {
                    LifeCoinEvent(1, "P");
                }
                
                soundManager.PlayCoinCollectedSound();
                player1Score+=5;
                Destroy(p1FloorManager.coins[i].gameObject);
                p1FloorManager.coins.RemoveAt(i);
            }
        }
        //Player 2:
        for(int i=0; i<p2FloorManager.coins.Count; i++) {
            if(momo2.GetComponent<BoxCollider2D>().bounds.Contains(p2FloorManager.coins[i].transform.position)) {
                if (p2FloorManager.coins[i].name.Equals("LifeCoinS"))
                {
                    LifeCoinEvent(2,"S");
                }
                else if (p2FloorManager.coins[i].name.Equals("LifeCoinP"))
                {
                    LifeCoinEvent(2, "P");
                }
                soundManager.PlayCoinCollectedSound();
                player2Score+=5;
                Destroy(p2FloorManager.coins[i].gameObject);
                p2FloorManager.coins.RemoveAt(i);
            }
        }
    }

    private void LifeCoinEvent(int v, string u)
    {
        if (v == 1)
        { 
            if (u == "S")
            {
                player1Lives++;
            }
            else if ( u == "P")
            {
                player2Lives--;
            }

            
        
        }

        else if (v == 2)
        {
            if(u == "S")
            {
                player2Lives++;
            }
            else if (u == "P")
            {
                player1Lives--;
            }

        }
        

    }

    void UpdateLives() 
    {
        UpdateplayerLives(player1Lives, p1h1, p1h2, p1h3);
        UpdateplayerLives(player2Lives, p2h1, p2h2, p2h3);
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
