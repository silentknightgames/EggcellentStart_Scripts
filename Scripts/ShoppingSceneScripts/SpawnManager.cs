using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private ObjectPooler objectPooler;
    private ShoppingManager shoppingManager;
    [SerializeField] private GameObject[] powerupPrefabs;
    [SerializeField] private GameObject bombPrefab;
    private readonly float objSpawnTime = 0.5f;
    private List<string> tags;
    private List<int> ranList;
    private bool changeSpawnRate = true;

    #region Singleton

    public static SpawnManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    #endregion

    private void Start()
    {
        objectPooler = ObjectPooler.Instance;
        shoppingManager = ShoppingManager.Instance;

        ShoppingManager.StartShopping += SpawnBomb;
        ShoppingManager.StartShopping += SpawnObj;
        ShoppingManager.StartShopping += SpawnPowerups;
    }

    private void Update()
    {
        if (shoppingManager.Timer == 10 && changeSpawnRate)
        {
            StopCoroutine(SpawnPowerups());
            StopCoroutine(SpawnBomb());
            StartCoroutine(SpawnPowerups());

            changeSpawnRate = false;
        }
    }

    private void SpawnFromPool()
    {
        Vector2 spawnPos = new Vector3(Random.Range(-5.5f, 5.5f), 6f);
        objectPooler.SpawnFromPool(tags[RandomTagIndex()], spawnPos);
    }

    IEnumerator SpawnObj()
    {
        //initialize the tags
        tags = new List<string>(objectPooler.poolDictionary.Keys);
        ranList = new List<int>();

        while (!shoppingManager.GameOver)
        {
            yield return new WaitForSeconds(objSpawnTime);

            Vector2 spawnPos = new Vector3(Random.Range(-5.5f, 5.5f), 6f);
            objectPooler.SpawnFromPool(tags[RandomTagIndex()], spawnPos);

        }
    }

    private int RandomTagIndex()
    {
        if (ranList.Count == 0)
        {
            for (int i = 0; i < objectPooler.poolDictionary.Count; i++)
            {
                ranList.Add(i);
            }
        }

        int listIndex = Random.Range(0, ranList.Count);
        int tagIndex = ranList[listIndex];
        ranList.RemoveAt(listIndex);
        return tagIndex;
    }

    IEnumerator SpawnPowerups()
    {
        while (!shoppingManager.GameOver)
        {
            float ranSpawnTime = Random.Range(5, 13);
            int powerupIndex = Random.Range(0, powerupPrefabs.Length);

            if (shoppingManager.Timer < 11)
            {
                ranSpawnTime = Random.Range(5, 7);
            }

            yield return new WaitForSeconds(ranSpawnTime);

            Vector3 spawnPos = new Vector3(Random.Range(-5.5f, 5.5f), 6f);
            Instantiate(powerupPrefabs[powerupIndex], spawnPos, powerupPrefabs[powerupIndex].transform.rotation);
        }
    }

    IEnumerator SpawnBomb()
    {
        while (!shoppingManager.GameOver)
        {
            float ranSpawnTime = Random.Range(5, 10);

            yield return new WaitForSeconds(ranSpawnTime);

            Vector3 spawnPos = new Vector3(Random.Range(-5.5f, 5.5f), 6f);
            Instantiate(bombPrefab, spawnPos, bombPrefab.transform.rotation);
        }
    }

    public void Spawn10Objects()
    {
        for (int i = 0; i < 10; i++)
        {
            SpawnFromPool();
        }
    }

}
