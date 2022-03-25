using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementControllerEMG : MonoBehaviour
{
    public float speed;
    public float upwardsForce;
    public Rigidbody2D rb;
    private Vector2 velocity;
    private MyoEMGRawReader emgReader;

    private SoundManager soundManager;
    private bool alreadyJumped = false; 

    void Start() {
        soundManager = FindObjectOfType<SoundManager>();
        emgReader = new MyoEMGRawReader();
        emgReader.StartReadingData();
    }

    void Update()
    {
        string control = emgReader.ReadControlFromArmband();
        if (control == "Wrist_Flexion") {
            alreadyJumped = false;
            rb.velocity = Vector3.zero;
            velocity = new Vector2(-speed, 0);
            rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
        } else if (control == "Wrist_Extension") {
            alreadyJumped = false;
            rb.velocity = Vector3.zero;
            velocity = new Vector2(speed, 0);
            rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
        } else if (control == "Hand_Closed") {
            if(!alreadyJumped) {
                rb.AddForce(new Vector2(0,1) * upwardsForce);
                soundManager.PlayJumpSound();
                alreadyJumped = true;
            }
        } else {
            alreadyJumped = false;
        }
    }
}
