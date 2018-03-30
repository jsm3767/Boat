using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public enum EnemyType
{
    Basic
}

[Serializable]
public struct Wave
{
    public WaveEnemy[] enemies;
    public string waveName;
}

[Serializable]
public struct WaveEnemy
{
    public Vector2 spawnPosition;
    public EnemyType enemyType;

    public WaveEnemy(Vector2 spawnPosition_in, EnemyType enemyType_in)
    {
        spawnPosition = spawnPosition_in;
        enemyType = enemyType_in;
    }
}


public class WaveManager : Singleton<WaveManager>
{
    public GameObject spawnParent;
    public GameObject BasicEnemy;

    private Dictionary<EnemyType, GameObject> enemyDictionary;

    private Wave currentWave;
    public int waveIndex = 1;

    List<GameObject> enemyShips;

    private int currentWaveEnemyCount;

    private void Awake()
    {
        enemyDictionary = new Dictionary<EnemyType, GameObject>();
        enemyDictionary.Add(EnemyType.Basic, BasicEnemy);
        
    }
    
    public int CountAliveShips()
    {
        //return enemyShips.Count;
        return 0;
    }

    //I think spawning everything at once is fine for this game
    public void SpawnWave()
    {
        LoadWaveFromJSON(waveIndex);
        enemyShips = new List<GameObject>();
        for( int i = 0; i < currentWave.enemies.Length; i++)
        {
            enemyShips.Add(Instantiate(enemyDictionary[currentWave.enemies[i].enemyType],
                new Vector3(currentWave.enemies[i].spawnPosition.x,.3f, currentWave.enemies[i].spawnPosition.y),
                Quaternion.identity,
                spawnParent.transform));
        }
        
    }

    //loads a json file holding level data into this class's data
    //Other classes can call spawnwave alone
    private void LoadWaveFromJSON(int index)
    {
        using (StreamReader stream = new StreamReader("Assets/Resources/Waves/" + index))
        {
            string json = stream.ReadToEnd();
            currentWave = JsonUtility.FromJson<Wave>(json);
            currentWaveEnemyCount = currentWave.enemies.Length;
        }
    }

    //creates a JSON file holding level data 
    private void WriteWave()
    {
        Wave test = new Wave();
        test.enemies = new WaveEnemy[] {
            new WaveEnemy(new Vector2(0, 0), EnemyType.Basic),
            new WaveEnemy(new Vector2(2.0f, 0.0f), EnemyType.Basic),
            new WaveEnemy(new Vector2(5.0f, -2.0f), EnemyType.Basic)
        };

        test.waveName = "Test Wave - 3 Enemies";

        using (StreamWriter stream = new StreamWriter("Assets/Resources/Waves/" + 1)) 
        {
            string json = JsonUtility.ToJson(test);
            stream.Write(json);
        }

    }
}
