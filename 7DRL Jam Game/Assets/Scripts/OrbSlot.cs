using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbSlot : MonoBehaviour
{
    public GameObject player;
    public GameObject orbChild;
    public float rotateSpeed = 10f;
    private float currentRotation = 0.0f;
    private float radius = 3.0f;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        //currentRotation = Vector2.Angle(player.transform.position, transform.position);
    }

    void Update()
    {
        currentRotation += rotateSpeed;
        if (currentRotation >= 360) currentRotation -= 360;
        float newX = player.transform.position.x + (radius * Mathf.Cos(currentRotation * Mathf.Deg2Rad));
        float newY = player.transform.position.y + (radius * Mathf.Sin(currentRotation * Mathf.Deg2Rad));
        transform.position = new Vector2(newX, newY);
    }

    public void setRotation(float rot)
    {
        currentRotation = rot;
    }

    public IEnumerator regenOrb(GameObject oldOrb)
    {
        yield return new WaitForSeconds(2.0f);
        GameObject newOrb = Instantiate(orbChild);
        newOrb.transform.SetParent(transform);
        newOrb.transform.SetLocalPositionAndRotation(new Vector3(0, 0, 0), Quaternion.identity);
        newOrb.transform.localScale = new Vector3(1, 1, 1);
        Debug.Log("Orb regened!");
        Destroy(oldOrb);
    }
}
