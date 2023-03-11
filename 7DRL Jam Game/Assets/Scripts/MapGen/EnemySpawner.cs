using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject wolfPrefab;
    [SerializeField] private GameObject ratPrefab;

    public void spawnEnemy(Vector3 position)
    {
        Instantiate(wolfPrefab, position, Quaternion.identity);
    }

}
