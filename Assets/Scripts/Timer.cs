using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{

    float totalSeconds;
    float elapsedSeconds;
    bool started = false;
    bool running = false;

    public float Duration
    {
        set
        {
            if(!running)
            {
                totalSeconds = value;
            }
        }

    }
      
    public bool Finished
    {
        get { return started && !running; }
    }
    
    public bool Started
    {
        get { return started && running; }
    }
    // Start is called before the first frame update
    void Start()
    { 
    }

    // Update is called once per frame
    void Update()
    {
       if(running)
       {
            elapsedSeconds += Time.deltaTime;
       }

       if (elapsedSeconds > totalSeconds)
       {
            running = false;
       }
    }

    public void Stop()
    {
        started = false;
        running = false;
    }

    public void Run()
    {
       if(totalSeconds > 0)
        {
            started = true;
            running = true;
            elapsedSeconds = 0;
        }

    }


}
