using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject wolfPrefab;
    [SerializeField] private GameObject ratPrefab;
    [SerializeField] private float wolfChance = 0.3f;

    public void spawnEnemy(Vector3 position)
    {
        float randFactor = Random.value;
        if(randFactor < wolfChance)
        {
            Instantiate(wolfPrefab, position, Quaternion.identity);
        }
        else
        {
            Instantiate(ratPrefab, position, Quaternion.identity);
        }
        
    }

}
