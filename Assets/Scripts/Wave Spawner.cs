using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{

    public bool waveFinished;
   

    [SerializeField] private List<Enemy> enemies = new List<Enemy>();
    public int waveValue;
    private List<GameObject> enemiesToSpawn = new List<GameObject>();
 
    [SerializeField] private Transform[] spawnLocation;
    private int spawnIndex;
 
    [SerializeField] private int waveDuration;
    private float waveTimer;
    private float spawnInterval;
    private float spawnTimer;
 
    public List<GameObject> spawnedEnemies = new List<GameObject>();

    void FixedUpdate()
    {
        if(spawnTimer <=0)
        {
            //spawn an enemy
            if(enemiesToSpawn.Count >0)
            {
                GameObject enemy = Instantiate(enemiesToSpawn[0], spawnLocation[spawnIndex].position,Quaternion.identity, transform.GetChild(0).transform); // spawn first enemy in our list
                enemiesToSpawn.RemoveAt(0); // and remove it
                spawnedEnemies.Add(enemy);
                spawnTimer = spawnInterval;
 
                if(spawnIndex + 1 <= spawnLocation.Length-1)
                {
                    spawnIndex++;
                }
                else
                {
                    spawnIndex = 0;
                }
            }
            else
            {
                waveTimer = 0; // if no enemies remain, end wave
            }
        }
        else
        {
            spawnTimer -= Time.fixedDeltaTime;
            waveTimer -= Time.fixedDeltaTime;
        }
 
        if(waveTimer<=0 && spawnedEnemies.Count <=0)
        {
            waveFinished = true;
        }
    }
 
    private void GenerateWave()
    {
        GenerateEnemies();
 
        spawnInterval = waveDuration / enemiesToSpawn.Count; // gives a fixed time between each enemies
        waveTimer = waveDuration; // wave duration is read only
    }
 
    private void GenerateEnemies()
    {
        // Create a temporary list of enemies to generate
        // 
        // in a loop grab a random enemy 
        // see if we can afford it
        // if we can, add it to our list, and deduct the cost.
 
        // repeat... 
 
        //  -> if we have no points left, leave the loop
 
        List<GameObject> generatedEnemies = new List<GameObject>();
        while(waveValue>0 || generatedEnemies.Count <50)
        {
            int randEnemyId = Random.Range(0, enemies.Count);
            int randEnemyCost = enemies[randEnemyId].value;
 
            if(waveValue-randEnemyCost>=0)
            {
                generatedEnemies.Add(enemies[randEnemyId].enemyPre);
                waveValue -= randEnemyCost;
            }
            else if(waveValue<=0)
            {
                break;
            }
        }
        enemiesToSpawn.Clear();
        enemiesToSpawn = generatedEnemies;
    }

    public void StartWave()
    {
        waveFinished = false;
        GenerateWave();
    }
  
}



[System.Serializable]
public class Enemy
{
    public GameObject enemyPre;
    public int value;
}
