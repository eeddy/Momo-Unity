using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    //Singleton
    public static SoundManager instance;

    public AudioClip jump;
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
}
