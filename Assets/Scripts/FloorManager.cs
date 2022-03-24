using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorManager : MonoBehaviour
{
    // Public Variables
    public GameObject momo; 
    public bool leftSide; 
    
    public Sprite normalGround;
    public Sprite obstacleSprite;
    public Sprite obstacleSpike;
    public Sprite coin;
    public Sprite supporterCoin;
    public Sprite punisherCoin;
    

    private float screenWidth;
    private float screenHeight;

    private List<GameObject> rightFloors; 
    private List<GameObject> leftFloors;
    public List<GameObject> obstacles; // public so the danger wall can be activated in game manager
    public List<GameObject> coins; //Public so it can be accessed from the game manager

    private float height, width;

    public int player1Diff;
    public int player2Diff;
    
    private float floorSpeed = 0.5f;
    private float floorIncrement = 0.1f;
    private float previousTime = 0f;
    private float previousFloorSpawn = 0f;

    // Start is called before the first frame update
    void Start()
    {
        height = (float)(Camera.main.orthographicSize * 2.0);
        width = (float)(height / Screen.height * Screen.width);
        screenHeight = Screen.height;
        screenWidth = Screen.width;

        rightFloors = new List<GameObject>();
        leftFloors = new List<GameObject>();
        obstacles = new List<GameObject>();
        coins = new List<GameObject>();
        CreateFloor();
    }

    // Update is called once per frame
    void Update()
    {
        //Spawing floors relative to speed
        if (Time.timeSinceLevelLoad - previousFloorSpawn > 2.5f/floorSpeed){
            CreateFloor();
            previousFloorSpawn = Time.time;
        }
        //Increase speed every 10 seconds
        if (Time.timeSinceLevelLoad - previousTime > 10) {
            floorSpeed += floorIncrement;
            previousTime = Time.time;
        }
        //Update floors
        for(int i=0; i<rightFloors.Count; i++) {
            rightFloors[i].transform.position += new Vector3(0, floorSpeed * Time.deltaTime, 0);
            leftFloors[i].transform.position += new Vector3(0, floorSpeed * Time.deltaTime, 0);
        }
        //Update obstacles
        for(int i=0; i<obstacles.Count; i++) {
            obstacles[i].transform.position += new Vector3(0, floorSpeed * Time.deltaTime, 0);
        }
        //Update coins
        for(int i=0; i<coins.Count; i++) {
            coins[i].transform.position += new Vector3(0, floorSpeed * Time.deltaTime, 0);
        }
        DeleteFloors();
        DeleteObstacles();
        DeleteCoins();
    }

    //Called when the person dies
    public void Reset() 
    {
        floorSpeed = 0.5f;
        for(int i=0; i<coins.Count; i++) {
            Destroy(coins[i].gameObject);
        }
        coins = new List<GameObject>();
        for(int i=0; i<rightFloors.Count; i++) {
            Destroy(rightFloors[i].gameObject);
            Destroy(leftFloors[i].gameObject);
        }
        rightFloors = new List<GameObject>();
        leftFloors = new List<GameObject>();
        for(int i=0; i<obstacles.Count; i++) {
            Destroy(obstacles[i].gameObject);
        }
        obstacles = new List<GameObject>();
    }

    void DeleteFloors() 
    {
       for(int i=0; i<rightFloors.Count; i++) {
            if (rightFloors[i].transform.position.y > height) {
                Destroy(rightFloors[i].gameObject);
                Destroy(leftFloors[i].gameObject);
                rightFloors.RemoveAt(i);
                leftFloors.RemoveAt(i);
            }
        } 
    }
    
    void DeleteObstacles() 
    {
        for(int i=0; i<obstacles.Count; i++) {
            if (obstacles[i].transform.position.y > height) {
                Destroy(obstacles[i].gameObject);
                obstacles.RemoveAt(i);
            }
        }
    }

    void DeleteCoins() 
    {
        for(int i=0; i<coins.Count; i++) {
            if (coins[i].transform.position.y > height) {
                Destroy(coins[i].gameObject);
                coins.RemoveAt(i);
            }
        }
    }

    public void CreateFloor() {
        float spriteWidth = normalGround.texture.width / normalGround.pixelsPerUnit;
        float spriteHeight = normalGround.texture.height / normalGround.pixelsPerUnit;

        float hole = Random.Range(0.1f, 0.4f);
        
        float widthScale = width/(spriteWidth);
        float heightScale = height/(18*spriteHeight);

        float leftSideWidth = widthScale * hole - 0.15f;
        float rightSideWidth = widthScale * (0.5f - hole) - 0.15f;
        
        // Create left floor
        GameObject floorLeft = new GameObject("LeftFloor");
        floorLeft.transform.localScale = new Vector3(leftSideWidth, heightScale);
        if (leftSide) {
            floorLeft.transform.position = new Vector3((-width/2) + spriteWidth*leftSideWidth/2,(-height/2) + spriteHeight*heightScale/2,0);
        } else {
            floorLeft.transform.position = new Vector3(0 + spriteWidth*leftSideWidth/2,(-height/2) + spriteHeight*heightScale/2,0);
        }
        SpriteRenderer renderer = floorLeft.AddComponent<SpriteRenderer>();
        renderer.sprite = normalGround;
        floorLeft.AddComponent<BoxCollider2D>();

        // Create right floor
        GameObject floorRight = new GameObject("RightFloor");
        floorRight.transform.localScale = new Vector3(rightSideWidth, heightScale);
        if (leftSide) {
            floorRight.transform.position = new Vector3(0 - spriteWidth*rightSideWidth/2,(-height/2) + spriteHeight*heightScale/2,0);
        } else {
            floorRight.transform.position = new Vector3((width/2) - spriteWidth*rightSideWidth/2,(-height/2) + spriteHeight*heightScale/2,0);
        }
        SpriteRenderer rendererRight = floorRight.AddComponent<SpriteRenderer>();
        rendererRight.sprite = normalGround;
        floorRight.AddComponent<BoxCollider2D>();
    
        leftFloors.Add(floorLeft);
        rightFloors.Add(floorRight);
        GameObject obs = CreateObstacle(spriteHeight*heightScale, floorRight, floorLeft);
        CreateCoin(spriteHeight*heightScale,obs);
    }

    //Returns gameobject so we dont create coin overlapping it 
    public GameObject CreateObstacle(float floorHeight, GameObject rightFloor, GameObject leftFloor) {
        float spriteWidth = obstacleSprite.texture.width / obstacleSprite.pixelsPerUnit;
        float spriteHeight = obstacleSprite.texture.height / obstacleSprite.pixelsPerUnit;

        GameObject obs = new GameObject("Obstacle");
        obs.transform.localScale = new Vector3(2f, 2f);

        float location = Random.Range(0.5f,width/2-0.5f);

        BoxCollider2D leftCollider = leftFloor.GetComponent<BoxCollider2D>();
        BoxCollider2D rightCollider = rightFloor.GetComponent<BoxCollider2D>();

        if(leftSide) {
            //Checks for overlap 
            while (!leftCollider.bounds.Contains(new Vector3(-location, leftCollider.bounds.center.y, 0)) && !rightCollider.bounds.Contains(new Vector3(-location, rightCollider.bounds.center.y, 0))) {
                location = Random.Range(0.5f,width/2-0.5f);
            }
            obs.transform.position = new Vector3(-location,-height/2 + spriteHeight*2/2 + floorHeight);
        } else {
            //Checks for overlap 
            while (!leftCollider.bounds.Contains(new Vector3(location, leftCollider.bounds.center.y, 0)) && !rightCollider.bounds.Contains(new Vector3(location, rightCollider.bounds.center.y, 0))) {
                location = Random.Range(0.5f,width/2-0.5f);
            }
            obs.transform.position = new Vector3(location,-height/2 + spriteHeight*2/2 + floorHeight);
        }
        SpriteRenderer renderer = obs.AddComponent<SpriteRenderer>();
        renderer.sprite = obstacleSprite;
        obs.AddComponent<BoxCollider2D>();
        obs.AddComponent<Rigidbody2D>();
        obs.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        obs.AddComponent<Obstacle>();
        
        obstacles.Add(obs);
        return obs;
    }
    //HERE 
    public void CreateCoin(float floorHeight, GameObject obs) {

       

        float spriteWidth = coin.texture.width / coin.pixelsPerUnit;
        float spriteHeight = coin.texture.height / coin.pixelsPerUnit;
        string coinType = "";

        //Let's implement russian roulette baby!
        int roulette = Random.Range(0, 100);
        
        if (tag.Equals("FloorM1"))
        {
            int noCoin = player1Diff; // probability for not spawning coin => between 0 to 50
            int eventCoin =  50 + noCoin;

            if(roulette <= noCoin)
            {
                print("noCoin");
                return;
            }
            else if (roulette >= eventCoin)
            {
                print("eventCoin");
                int rand1 = Random.Range(0, 4);
                print(rand1);
                if (rand1 == 0) { coinType = "WallCoinS"; print("WallCoinS"); }
                else if (rand1 == 1) { coinType = "WallCoinP"; print("WallCoinP"); }
                else if (rand1 == 2) { coinType = "LifeCoinS"; print("LifeCoinS"); }
                else if (rand1 == 3) { coinType = "LifeCoinP"; print("LifeCoinP"); }
                
            }
            else
            {
                print("coin");
                coinType = "Coin";
            }

            
        }
        else if(tag.Equals("FloorM2"))
        {
            int noCoin = player2Diff; // probability for not spawning coin => between 0 to 50
            int eventCoin = 50 + noCoin;

            if (roulette <= noCoin)
            {
                return;
            }
            else if (roulette >= eventCoin)
            {
                int rand2 = Random.Range(0, 4); print(rand2);
                if (rand2 == 1) coinType = "WallCoinS";
                else if (rand2 == 2) coinType = "WallCoinP";
                else if (rand2 == 3) coinType = "LifeCoinS";
                else if (rand2 == 4) coinType = "LifeCoinP";

            }
            else
            {
                coinType = "Coin";
            }
        }
       
        
        GameObject coinObj = new GameObject(coinType);
        coinObj.transform.localScale = new Vector3(1f, 1f);

        float location = Random.Range(0.5f, width / 2 - 0.5f);
        if (leftSide) {
            while (obs.GetComponent<BoxCollider2D>().bounds.Contains(new Vector3(-location, -height / 2 + spriteHeight / 2 + floorHeight))) {
                location = Random.Range(0.5f, width / 2 - 0.5f);
            }
            coinObj.transform.position = new Vector3(-location, -height / 2 + spriteHeight / 2 + floorHeight);
        } else {
            while (obs.GetComponent<BoxCollider2D>().bounds.Contains(new Vector3(location, -height / 2 + spriteHeight / 2 + floorHeight))) {
                location = Random.Range(0.5f, width / 2 - 0.5f);
            }
            coinObj.transform.position = new Vector3(location, -height / 2 + spriteHeight / 2 + floorHeight);
        }

        SpriteRenderer renderer = coinObj.AddComponent<SpriteRenderer>();
        if (coinType == "Coin") renderer.sprite = coin;
        else if (coinType == "WallCoinP" || coinType == "LifeCoinP") renderer.sprite = punisherCoin;
        else if (coinType == "WallCoinS" || coinType == "LifeCoinS") renderer.sprite = supporterCoin;
        
        coins.Add(coinObj);
    }

    //public void CheckOverlap() --> This would be awesome to add at some point.
    
    public void DeactivateWalls()
    {
        foreach(GameObject obstacle in obstacles)
        {
            obstacle.SetActive(false);
        }
    }
    public void ActivateWalls()
    {
        foreach (GameObject obstacle in obstacles)
        {
            obstacle.SetActive(true);
        }
    }

    public void ChangeObsToSpike()
    {
        foreach(GameObject obstacle in obstacles)
        {
            obstacle.GetComponent<SpriteRenderer>().sprite = obstacleSpike;
        }
    }
    public void ChangeSpikeToObs()
    {
        foreach(GameObject obstacle in obstacles)
        {      
            obstacle.GetComponent<SpriteRenderer>().sprite = obstacleSprite;
        }

    }
}
