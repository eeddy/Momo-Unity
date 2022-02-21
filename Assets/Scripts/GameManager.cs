using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Image p1h1, p1h2, p1h3, p2h1, p2h2, p2h3; //Hearts
    public Sprite fullHeart;
    public Sprite emptyHeart;
    public FloorManager p1FloorManager, p2FloorManager;

    public GameObject momo1, momo2, p1Spikes, p2Spikes; 

    private int player1Lives; 
    private int player2Lives; 

    private int player1Coins;
    private int player2Coins;

    private bool p1Touching, p2Touching;

    private SoundManager soundManager;

    // Start is called before the first frame update
    void Start()
    {
        //Set initial player lives 
        player1Lives = 3;
        player2Lives = 3; 
        //Set initial coins
        player1Coins = 0;
        player2Coins = 0;
        p1Touching = false;
        p2Touching = false;
        
        soundManager = FindObjectOfType<SoundManager>();
    }

    // Update is called once per frame
    void Update()
    {
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
                soundManager.PlayCoinCollectedSound();
                player1Coins++;
                Destroy(p1FloorManager.coins[i].gameObject);
                p1FloorManager.coins.RemoveAt(i);
            }
        }
        //Player 2:
        for(int i=0; i<p2FloorManager.coins.Count; i++) {
            if(momo2.GetComponent<BoxCollider2D>().bounds.Contains(p2FloorManager.coins[i].transform.position)) {
                soundManager.PlayCoinCollectedSound();
                player2Coins++;
                Destroy(p2FloorManager.coins[i].gameObject);
                p2FloorManager.coins.RemoveAt(i);
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
