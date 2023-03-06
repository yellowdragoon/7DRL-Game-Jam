using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orb : MonoBehaviour
{
    public GameObject player;
    public float rotateSpeed = 100;
    public float shotSpeed = 25.0f;
    Vector3 moveDir;
    private bool orbReleased = false;

    public int orbDamage = 5;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    // Update is called once per frame
    void Update()
    {
        if (orbReleased)
        {
            transform.Translate(moveDir * shotSpeed * Time.deltaTime);
        }
        else
        {
            transform.RotateAround(player.transform.position, Vector3.forward, rotateSpeed * Time.deltaTime);
        }
        
    }


    public void releaseOrb()
    {
        orbReleased = true;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0.0f;
        transform.rotation = Quaternion.identity;
        moveDir = new Vector3(mousePos.x - transform.position.x, mousePos.y - transform.position.y, 0.0f).normalized;

        
        transform.SetParent(null);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("OrbCollision!");
        if(orbReleased && collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<Enemy>().takeDamage(orbDamage);
            Destroy(gameObject);
        }
    }
}
