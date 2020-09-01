using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    bool genera;
    [SerializeField] int maxEnemyInGame;
    public float timeSpawnAsteroid;
    float defaultTimeSpawnAsteroid;
    [SerializeField]
    GameObject asteroidObj;
    [SerializeField]
    GameObject enemyObj;
    void Start()
    {
        defaultTimeSpawnAsteroid = timeSpawnAsteroid;
        genera = true;
        //StartCoroutine(SpawnSpeedLine(timeSpawnAsteroid));
        StartCoroutine(SpawnAsteroid(timeSpawnAsteroid));
        GenerateEnemy();
    }

    void Update()
    {
        
    }

    IEnumerator SpawnAsteroid(float time)
    {
        yield return new WaitForSeconds(time);
        if (genera)
        {
            Instantiate(asteroidObj, gameObject.transform.position + (new Vector3(0, Random.Range(-600, 600), 0)), gameObject.transform.rotation);
        }
        if(GameManager.Instance.timeOfGame > 600)
        {
            timeSpawnAsteroid = 0.05f;
        }
        else
        {
            timeSpawnAsteroid = defaultTimeSpawnAsteroid / ((1 + (GameManager.Instance.difficultMolt)));
        }
        StartCoroutine(SpawnAsteroid(time));
    }

    IEnumerator SpawnEnemy(float time)
    {
        yield return new WaitForSeconds(time);
        if(GameManager.Instance.enemyIngame < maxEnemyInGame && genera)
        {
            GameObject tmpEnemy = Instantiate(enemyObj, gameObject.transform.position + (new Vector3(0, Random.Range(-300, 300), 0)), Quaternion.identity);
            tmpEnemy.transform.rotation = Quaternion.Euler(0, 0, -90);
        }
    }

    public void GenerateEnemy()
    {
        StartCoroutine(SpawnEnemy(Random.Range(2f,10f)));
    }

    public void StopGenerateForTime(float time)
    {
        StartCoroutine(StopGenerate(time));
    }

    IEnumerator StopGenerate(float time)
    {
        genera = false;
        yield return new WaitForSecondsRealtime(time);
        genera = true;
    }

    public bool GetGenera()
    {
        return genera;
    }
}
