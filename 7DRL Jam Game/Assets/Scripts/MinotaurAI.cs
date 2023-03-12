using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinotaurAI : MonoBehaviour
{

    private Enemy enemyScript;
    private EnemyPathfinding pathFinding;
    private MusicManager musicManager;

    private void Start()
    {
        enemyScript = GetComponent<Enemy>();
        pathFinding = GetComponent<EnemyPathfinding>();
        musicManager = GameObject.FindGameObjectWithTag("MusicManager").GetComponent<MusicManager>();
    }
    // Update is called once per frame
    void Update()
    {
        if (pathFinding.aggro)
        {
            musicManager.startBossMusic();
        }
    }
}
