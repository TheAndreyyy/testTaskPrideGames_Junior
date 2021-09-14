using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> spawnPoints = new List<GameObject>();
    [SerializeField] public GameplayScript gameplayScript;
    public GameObject enemyPrefab;
    GameObject tempEnemy;

    void Start()
    {
        for (int i = 0; i < spawnPoints.Count; i++)
        {
            GenerateEnemy(spawnPoints[i]);
        }
    }

    private void GenerateEnemy(GameObject spawnPoint)
    {
        tempEnemy = Instantiate(enemyPrefab, spawnPoint.transform);
        tempEnemy.GetComponent<EnemyScript>().enemySpawner = this;
    }

    public void WaitingBeforeSpawn(GameObject spawnPoint)
    {
        StartCoroutine(ExampleCoroutine(spawnPoint));
    }

    IEnumerator ExampleCoroutine(GameObject spawnPoint)
    {
        //ждем столько времени, сколько было по ТЗ, берем из общего скрипта
        yield return new WaitForSeconds(gameplayScript.timeForRespawnGrenadesAndEnemies);
        GenerateEnemy(spawnPoint);
    }
}
