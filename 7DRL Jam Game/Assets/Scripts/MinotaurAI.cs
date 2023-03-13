using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    private void OnDestroy()
    {
        if(enemyScript.health <= 0)
        {
            SceneManager.LoadScene("WinScreen");
        }
    }
}
