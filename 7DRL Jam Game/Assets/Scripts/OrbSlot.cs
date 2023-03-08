using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbSlot : MonoBehaviour
{
    public GameObject player;
    public GameObject orbChild;
    public float rotateSpeed = 10f;
    private float currentRotation = 0.0f;
    [SerializeField] private float xRadius = 3.0f;
    [SerializeField] private float yRadius = 3.0f;
    [SerializeField] private float xOffset = 0.0f;
    [SerializeField] private float yOffset = 0.0f;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        //currentRotation = Vector2.Angle(player.transform.position, transform.position);
    }

    void Update()
    {
        currentRotation += rotateSpeed;
        if (currentRotation >= 360) currentRotation -= 360;
        float newX = player.transform.position.x + xOffset + (xRadius * Mathf.Cos(currentRotation * Mathf.Deg2Rad));
        float newY = player.transform.position.y + yOffset + (yRadius * Mathf.Sin(currentRotation * Mathf.Deg2Rad));
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

        newOrb.transform.localScale = orbChild.transform.localScale;
        Debug.Log("Orb regened!");
        Destroy(oldOrb);
    }
}
