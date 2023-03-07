using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public int health = 10;
    public float moveSpeed = 5.0f;
    public int damageDealt = 10;
    public float minDistPlayer = 1.0f;
    public float knockBackDealt = 30.0f;
    GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    // Update is called once per frame
    void Update()
    {
        float step = moveSpeed * Time.deltaTime;
        if(Vector3.Distance(transform.position, player.transform.position) >= minDistPlayer)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, step);
        }

    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Player>().takeDamage(damageDealt, this.gameObject, knockBackDealt);
        }
    }

    public void takeDamage(int amount)
    {
        health -= amount;
        if (health <= 0) die();
    }


    private void die()
    {
        Debug.Log("Enemy dead!");
        Destroy(gameObject);
    }
}
