using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private GameObject bossMusicObject;
    [SerializeField] private GameObject levelMusicObject;
    private bool bossOn = false;
    
    public void startBossMusic()
    {
        if (!bossOn)
        {
            bossOn = true;
            Destroy(levelMusicObject);
            bossMusicObject.SetActive(true);
        }
    }
    
}
