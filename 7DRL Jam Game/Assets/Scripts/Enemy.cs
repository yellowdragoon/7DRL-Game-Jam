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
    public float aggroRange = 10.0f;
    GameObject player;
    EnemyPathfinding pathfinding;

    private bool attacking = false;
    Animator animator;

    private Vector3 baseScale;


    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
        pathfinding = GetComponent<EnemyPathfinding>();
        baseScale = transform.localScale;
    }
    // Update is called once per frame
    void Update()
    {
        checkAggro();
        animator.SetBool("attacking", attacking);

        if (!attacking)
        {
            if (Vector3.Distance(transform.position, player.transform.position) >= minDistPlayer)
            {
                Vector3 newScale = transform.localScale;
                newScale.x = transform.position.x > player.transform.position.x ? -baseScale.x : baseScale.x ;
                transform.localScale = newScale;
                //transform.position = Vector3.MoveTowards(transform.position, player.transform.position, step);
            }
        }
    }

    private void checkAggro()
    {
        if(Vector2.Distance(player.transform.position, transform.position) < aggroRange)
        {
            pathfinding.gainAggro();
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
