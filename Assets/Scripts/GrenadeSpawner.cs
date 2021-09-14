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
        tempGrenade.GetComponent<GrenadeScript>().grenadeSpawner = this;//����� ����������� ������ �� ���� ������� ��� �������� �������
        Color32 generatedColor = Color.white;
        int generatedColorInt = -1;                                     //� �������, ��� ��� �������, � ������ ��������� �������� string ��� �������� "�����" �����, �� ������-�� ��� ������ ������ � ������
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
        tempGrenade.GetComponent<SphereCollider>().radius = gameplayScript.radiusForTakeGrenades;//������ �������
        tempGrenade.GetComponent<Renderer>().material.color = generatedColor;//�������� ����� ���� �� ������ ��� �������������� ������
    }

    public void WaitingBeforeSpawn(GameObject spawnPoint)
    {
        StartCoroutine(ExampleCoroutine(spawnPoint));
    }

    IEnumerator ExampleCoroutine(GameObject spawnPoint)
    {
        //���� ������� �������, ������� ���� �� ��, ����� �� ������ �������
        yield return new WaitForSeconds(gameplayScript.timeForRespawnGrenadesAndEnemies);
        GenerateGrenade(spawnPoint);
    }
}
