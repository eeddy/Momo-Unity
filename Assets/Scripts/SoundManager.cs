using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Singleton
public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    
    public AudioClip coin;
    public AudioClip jump;
    public AudioClip loseLife;
    AudioSource audioSource;

    void Awake() {
        if (instance != null) { Destroy(gameObject); } 
        else{ instance = this; }
    }

    void Start() {
        audioSource = GetComponent<AudioSource>();
    }
    
    public void PlayJumpSound() {
        audioSource.PlayOneShot(jump, 0.7F);
    }

    public void PlayCoinCollectedSound() {
        audioSource.PlayOneShot(coin, 0.7f);
    }
    
    public void PlayLoseLifeSound() {
        audioSource.PlayOneShot(loseLife, 0.7f);
    }
}
