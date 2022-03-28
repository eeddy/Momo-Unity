using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalancingManager : MonoBehaviour
{
    GameManager gameManager;
    GameObject momo1, momo2;

    int p1Lives;
    int p2Lives;

    float p1Height;
    float p2Height;

    const decimal heightMin = 4, heightMax = -4;
    const decimal lifeMin = 1, lifeMax = 3;



    decimal p1lifePerformance;
    decimal p1HeightPerformance;
    public decimal p1PerformanceRating;
    
    decimal p2lifePerformance;
    decimal p2HeightPerformance;
    public decimal p2PerformanceRating;



    public decimal p1SpawnTreshold;
    public decimal p2SpawnTreshold;

    const decimal minSpawnThreshold = 12, maxSpawnTreshold = 50;

    const decimal minIncreaseSpeed = 1.12M, maxIncreaseSpeed = 1.5M;
    const decimal minSlowSpeed = 0.6M, maxSlowSpeed = 0.9M;

    public decimal p1IncreaseSpeed, p2IncreaseSpeed;
    public decimal p1SlowSpeed, p2SlowSpeed;

    void CalculatePerformance()
    {
        //rate each element from 0 to 10
         p1lifePerformance = CalcY_LinearEq(lifeMin,lifeMax, 0 , 10 ,  p1Lives);
         p1HeightPerformance = CalcY_LinearEq(heightMin, heightMax, 0, 10, (decimal) p1Height);

         p2lifePerformance = CalcY_LinearEq(lifeMin, lifeMax, 0, 10, p2Lives);
         p2HeightPerformance = CalcY_LinearEq(heightMin, heightMax, 0, 10, (decimal)p2Height);

        //rate performance from 1 to 100 
         p1PerformanceRating = Decimal.Round((p1lifePerformance * 3 + p1HeightPerformance) / 40, 2) * 100;
         p2PerformanceRating = Decimal.Round((p2lifePerformance * 3 + p2HeightPerformance) / 40, 2) * 100;

        CalculateTresholdSpawn();
        CalculateSpeedMultipliers();

    }

    private void CalculateSpeedMultipliers()
    {
        p1IncreaseSpeed = CalcY_LinearEq(100, 10, minIncreaseSpeed, maxIncreaseSpeed, p1PerformanceRating);
        p2IncreaseSpeed = CalcY_LinearEq(100, 10, minIncreaseSpeed, maxIncreaseSpeed, p2PerformanceRating);

        p1SlowSpeed = CalcY_LinearEq(100, 10, maxSlowSpeed, minSlowSpeed, p1PerformanceRating);
        p2SlowSpeed = CalcY_LinearEq(100, 10, maxSlowSpeed, minSlowSpeed, p2PerformanceRating);
    }

    private void CalculateTresholdSpawn()
    {
        p1SpawnTreshold = CalcY_LinearEq(100, 10, minSpawnThreshold, maxSpawnTreshold, p1PerformanceRating);
        p2SpawnTreshold = CalcY_LinearEq(100, 10, minSpawnThreshold, maxSpawnTreshold, p2PerformanceRating);
    }

    decimal CalcY_LinearEq(decimal x1, decimal x2, decimal y1, decimal y2, decimal input)
    {
        decimal slope = (y2 - y1) / (x2 - x1);
        return Decimal.Round(slope * (input - x1) + y1, 1);
    }

    private void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        momo1 = gameManager.momo1;
        momo2 = gameManager.momo2;
        
    }
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("Log", 1.0f, 2.0f);
    }

    // Update is called once per frame
    void Update()
    {
        p1Lives = gameManager.player1Lives;
        p2Lives = gameManager.player2Lives;
        p1Height = momo1.transform.position.y;
        p2Height = momo2.transform.position.y;

        CalculatePerformance();

    }

    void Log()
    {
        Debug.Log(
            "p1Perform= " + p1PerformanceRating +
            "\tp2Perform= " + p2PerformanceRating +
            "\tp1Treshold= " + p1SpawnTreshold +
            "\tp2Treshold= " + p2SpawnTreshold
            );
    }


}
