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
    public GameObject gameOverScreen; 
    public Text time, winner; 

    private float previousTime = 0;
    private int player1Lives; 
    private int player2Lives; 

    private bool p1Touching, p2Touching;

    private SoundManager soundManager;
    private float gameStart = 0;
    private float gameEnd = 0;

    // Start is called before the first frame update
    void Start()
    {
        //Set initial player lives 
        player1Lives = 3;
        player2Lives = 3; 

        p1Touching = false;
        p2Touching = false;
        
        soundManager = FindObjectOfType<SoundManager>();
        gameStart = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        //Increase score every 5 seconds
        if (Time.timeSinceLevelLoad - previousTime > 5) {
            previousTime = Time.time;
        }
        if (player1Lives == 0 || player2Lives == 0) {
            if(gameEnd == 0) {
                gameEnd = Time.time;
            }
            gameOverScreen.SetActive(true);
            Time.timeScale = 0;
            if(player1Lives > player2Lives) {
                winner.text = "Player 1 wins!";
            } else {
                winner.text = "Player 2 wins!";
            }
            time.text = "Time: " + (gameEnd - gameStart);
        } else {
            CheckForPowerUpCollision();
            CheckForSpikeTouch();
            if(p1FloorManager.CheckSwordTouch()) {
                p1FloorManager.Reset();
                soundManager.PlayLoseLifeSound();
                player1Lives--;
            }
            if(p2FloorManager.CheckSwordTouch()) {
                p2FloorManager.Reset();
                soundManager.PlayLoseLifeSound();
                player2Lives--;
            }
        } 
        UpdateLives();
    }

    public void PlayAgain() {
        p1FloorManager.Reset();
        p2FloorManager.Reset();
        player1Lives = 3;
        player2Lives = 3;
        Time.timeScale = 1;
        gameOverScreen.SetActive(false);
        gameEnd = 0;
        gameStart = Time.time;
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

    void CheckForPowerUpCollision() 
    {
        //Player 1:
        for(int i=0; i<p1FloorManager.powerups.Count; i++) {
            if(momo1.GetComponent<BoxCollider2D>().bounds.Contains(p1FloorManager.powerups[i].transform.position)) {
                soundManager.PlayCoinCollectedSound();
                p1FloorManager.AcquiredPowerup(i);
            }
        }
        //Player 2:
        for(int i=0; i<p2FloorManager.powerups.Count; i++) {
            if(momo2.GetComponent<BoxCollider2D>().bounds.Contains(p2FloorManager.powerups[i].transform.position)) {
                soundManager.PlayCoinCollectedSound();
                p2FloorManager.AcquiredPowerup(i);
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
