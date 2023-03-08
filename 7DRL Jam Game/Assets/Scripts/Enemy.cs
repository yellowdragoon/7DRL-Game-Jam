using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public int health = 10;
    public float moveSpeed = 5.0f;
    public int damageDealt = 10;
    public float minDistPlayer = 0.1f;
    public float knockBackDealt = 30.0f;
    GameObject player;

    private bool attacking = false;
    Animator animator;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        animator.SetBool("attacking", attacking);

        if (!attacking)
        {
            float step = moveSpeed * Time.deltaTime;
            if (Vector3.Distance(transform.position, player.transform.position) >= minDistPlayer)
            {
                Vector3 newScale = transform.localScale;
                newScale.x = transform.position.x > player.transform.position.x ? -1 : 1 ;
                transform.localScale = newScale;
                transform.position = Vector3.MoveTowards(transform.position, player.transform.position, step);
            }
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Player>().takeDamage(damageDealt, this.gameObject, knockBackDealt);
            StartCoroutine(Attack());
        }
    }

    private IEnumerator Attack()
    {
        attacking = true;
        yield return new WaitForSeconds(0.5f);
        attacking = false;
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
