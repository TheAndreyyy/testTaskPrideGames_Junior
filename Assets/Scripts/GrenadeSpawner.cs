using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> spawnPoints = new List<GameObject>();
    [SerializeField] public GameplayScript gameplayScript;
    public GameObject grenadePrefab;
    public GameObject tempGrenade;
    public List<string> randomColors = new List<string>();

    void Start()
    {
        randomColors = new List<string>(){
        "red",
        "green",
        "blue"
    };
        for (int i = 0; i < spawnPoints.Count; i++)
        {
            GenerateGrenade(spawnPoints[i]);
        }
    }

    private void GenerateGrenade(GameObject spawnPoint)
    {
        tempGrenade = Instantiate(grenadePrefab, spawnPoint.transform);
        tempGrenade.GetComponent<GrenadeScript>().grenadeSpawner = this;//сразу прокидываем ссылку на этот спавнер при создании гранаты
        Color32 generatedColor = Color.white;
        int generatedColorInt = -1;                                     //я понимаю, что это костыль, в смысле создавать отдельно string для хранения "имени" цвета, но почему-то оно первым пришло в голову
        switch (randomColors[Random.Range(0, randomColors.Count)])
        {
            case "red":
                generatedColor = Color.red;
                generatedColorInt = 0;
                break;
            case "green":
                generatedColor = Color.green;
                generatedColorInt = 1;
                break;
            case "blue":
                generatedColor = Color.blue;
                generatedColorInt = 2;
                break;
        }
        tempGrenade.GetComponent<GrenadeScript>().colorOfThisGrenade = generatedColorInt;
        tempGrenade.GetComponent<SphereCollider>().radius = gameplayScript.radiusForTakeGrenades;//радиус подбора
        tempGrenade.GetComponent<Renderer>().material.color = generatedColor;//рандомно берем цвет из списка уже представленных цветов
    }

    public void WaitingBeforeSpawn(GameObject spawnPoint)
    {
        StartCoroutine(ExampleCoroutine(spawnPoint));
    }

    IEnumerator ExampleCoroutine(GameObject spawnPoint)
    {
        //ждем столько времени, сколько было по ТЗ, берем из общего скрипта
        yield return new WaitForSeconds(gameplayScript.timeForRespawnGrenadesAndEnemies);
        GenerateGrenade(spawnPoint);
    }
}
