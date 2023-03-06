using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orb : MonoBehaviour
{
    public GameObject player;
    public float rotateSpeed = 100;

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(player.transform.position, Vector3.forward, rotateSpeed * Time.deltaTime);
    }
}
