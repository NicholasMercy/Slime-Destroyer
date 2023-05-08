using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] enemies; 
    public GameObject[] powerUps;

    int enemyCount;
    int waveCount = 1;
    int EnemySpawnNo = 2;

    int zRange = 23;
    int xRange = 23;

    void Start()
    {
        SpawnEnemy();
        SpawnPowerUp();
        Debug.Log("WAVE " + (waveCount-1));
    }

    // Update is called once per frame
    void Update()
    {
        enemyCount = FindObjectsOfType<Enemy>().Length;
        if(enemyCount == 0)
        {
            SpawnEnemy();
            Debug.Log("WAVE "+ (waveCount -1));
        }
    }
    
    void SpawnEnemy()
    {

       // Debug.Log(EnemySpawnNo);       
        EnemySpawnNo = waveCount + 2;
        for (int i = 0; i < EnemySpawnNo; i++)
        {
            //side choice
            int methodChoice = Random.Range(0, 4);
            //choice of enemy
            int randomEnemy = Random.Range(0, enemies.Length);
            //position random
            int randomX = Random.Range(-xRange, xRange);
            int randomZ = Random.Range(-zRange, zRange);
            Vector3 randomPos; 
            //left side
            if (methodChoice == 0)
            {
                randomPos = new Vector3(-xRange, 0, randomZ);
                Instantiate(enemies[randomEnemy], randomPos, Quaternion.identity);
            }
            //right side
            else if(methodChoice == 1)
            {

                randomPos = new Vector3(xRange, 0, randomZ);
                Instantiate(enemies[randomEnemy], randomPos, Quaternion.identity);
            }
            //top side
            else if (methodChoice == 2) 
            {
                randomPos = new Vector3(randomX, 0, zRange);
                Instantiate(enemies[randomEnemy], randomPos, Quaternion.identity);

            }
            //bottom side
            else if(methodChoice == 3) 
            {
                randomPos = new Vector3(randomX, 0, -zRange);
                Instantiate(enemies[randomEnemy], randomPos, Quaternion.identity);
            }           
              
           
        }       
        waveCount++;

    }
    void SpawnPowerUp()
    {
        int randomzPos = Random.Range(-zRange, zRange);
        int randomXPos = Random.Range(-xRange, xRange);

        int randomPowerUp = Random.Range(0, powerUps.Length);
        Vector3 randomPos = new Vector3(randomXPos, 0, randomzPos);

        Instantiate(powerUps[randomPowerUp], randomPos, Quaternion.identity);
        
    }

    
}
