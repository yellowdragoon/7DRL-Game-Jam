using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCenter : MonoBehaviour
{
   
    // Update is called once per frame
    void Update()
    {
        transform.position = transform.parent.transform.position;
    }
}
