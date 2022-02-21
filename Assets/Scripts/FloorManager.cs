using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorManager : MonoBehaviour
{
    // Public Variables
    public GameObject momo; 
    public bool leftSide; 
    
    public Sprite normalGround;
    public Sprite obstacle;
    public Sprite coin; 

    private float screenWidth;
    private float screenHeight;

    private List<GameObject> rightFloors; 
    private List<GameObject> leftFloors;
    private List<GameObject> obstacles;
    public List<GameObject> coins; //Public so it can be accessed from the game manager

    private float height, width;
    
    private float floorSpeed = 0.5f;

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

        InvokeRepeating("CreateFloor", 0f, 5f);
    }

    // Update is called once per frame
    void Update()
    {
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
        float spriteWidth = obstacle.texture.width / obstacle.pixelsPerUnit;
        float spriteHeight = obstacle.texture.height / obstacle.pixelsPerUnit;

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
        renderer.sprite = obstacle;
        obs.AddComponent<BoxCollider2D>();
        obstacles.Add(obs);
        return obs;
    }

    public void CreateCoin(float floorHeight, GameObject obs) {
        //One in 2 chance of coing being created
        int coinRandom = Random.Range(0,2);
        // Return if not 1
        if(coinRandom != 1) {
            return;
        }
        float spriteWidth = coin.texture.width / coin.pixelsPerUnit;
        float spriteHeight = coin.texture.height / coin.pixelsPerUnit;

        GameObject coinObj = new GameObject("Coin");
        coinObj.transform.localScale = new Vector3(1f, 1f);

        float location = Random.Range(0.5f,width/2-0.5f);
        if(leftSide) {  
            while(obs.GetComponent<BoxCollider2D>().bounds.Contains(new Vector3(-location,-height/2 + spriteHeight/2 + floorHeight))) {
                location = Random.Range(0.5f,width/2-0.5f);
            }
            coinObj.transform.position = new Vector3(-location,-height/2 + spriteHeight/2 + floorHeight);
        } else {
            while(obs.GetComponent<BoxCollider2D>().bounds.Contains(new Vector3(location,-height/2 + spriteHeight/2 + floorHeight))) {
                location = Random.Range(0.5f,width/2-0.5f);
            }
            coinObj.transform.position = new Vector3(location,-height/2 + spriteHeight/2 + floorHeight);
        }
        SpriteRenderer renderer = coinObj.AddComponent<SpriteRenderer>();
        renderer.sprite = coin;
        coins.Add(coinObj);
    }

    //public void CheckOverlap() --> This would be awesome to add at some point.
    
}
