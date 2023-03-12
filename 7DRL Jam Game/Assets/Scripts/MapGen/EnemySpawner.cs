using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject wolfPrefab;
    [SerializeField] private GameObject ratPrefab;
    [SerializeField] private GameObject minotaurPrefab;
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

    public void spawnBossRoom(RectInt roomBounds)
    {
        Instantiate(minotaurPrefab, new Vector3(roomBounds.x + roomBounds.width/2, roomBounds.y + roomBounds.height/2, 0) , Quaternion.identity);
        for (int j = 0; j < 10; j++)
        {
            Vector3 position = new Vector3(Random.Range(roomBounds.x + 1, roomBounds.x + roomBounds.width - 1), Random.Range(roomBounds.y + 1, roomBounds.y + roomBounds.height - 1), 0);
            spawnEnemy(position);
        }
    }

}
