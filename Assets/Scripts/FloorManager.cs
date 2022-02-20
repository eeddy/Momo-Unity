using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FloorManager : MonoBehaviour
{
    // Public Variables
    public GameObject momo; 
    public Sprite normalGround;
    public bool leftSide; 

    private float screenWidth;
    private float screenHeight;

    private List<GameObject> rightFloors; 
    private List<GameObject> leftFloors;
    
    private float floorSpeed = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        rightFloors = new List<GameObject>();
        leftFloors = new List<GameObject>();
        screenHeight = Screen.height;
        screenWidth = Screen.width;
        InvokeRepeating("CreateFloor", 0f, 5f);
    }

    // Update is called once per frame
    void Update()
    {
        for(int i=0; i<rightFloors.Count; i++) {
            rightFloors[i].transform.position += new Vector3(0, floorSpeed * Time.deltaTime, 0);
            leftFloors[i].transform.position += new Vector3(0, floorSpeed * Time.deltaTime, 0);
        }
        DeleteFloors();
    }

    void DeleteFloors() 
    {
       for(int i=0; i<rightFloors.Count; i++) {
            if (rightFloors[i].transform.position.y > Camera.main.orthographicSize * 2.0) {
                Destroy(rightFloors[i].gameObject);
                Destroy(leftFloors[i].gameObject);
                rightFloors.RemoveAt(i);
                leftFloors.RemoveAt(i);
            }
        } 
    }

    public void CreateFloor() {
        float height = (float)(Camera.main.orthographicSize * 2.0);
        float width = (float)(height / Screen.height * Screen.width);

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
    }
    
}
