using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    GameManager manager;
    GameObject momo1_ref;
    GameObject momo2_ref;



    private void Awake()
    {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        momo1_ref = manager.momo1;
        momo2_ref = manager.momo2;

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.Equals(momo1_ref))
        {
            
            if (manager.player1WallDangerMode)
                manager.Player1Lives--;
        }
        else if (collision.gameObject.Equals(momo2_ref))
        {
            
            if (manager.player2WallDangerMode)
                manager.Player2Lives--;
        }
    }
}
