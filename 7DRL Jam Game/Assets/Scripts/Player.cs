using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Vector2 speed = new Vector2(50, 50);
    public int health = 100;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }    
    // Update is called once per frame
    void Update()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(speed.x * inputX, speed.y * inputY, 0);

        movement *= Time.deltaTime;

        transform.Translate(movement);

        if (Input.GetMouseButtonDown(0))
        {
            GameObject orb = findClosestOrb();
            if(orb != null) orb.GetComponent<Orb>().releaseOrb();
        }
    }

    public void takeDamage(int amount, GameObject source, float knockBack)
    {
        health -= amount;
        Vector2 knockbackDir = (transform.position - source.transform.position).normalized;
        rb.AddForce(knockbackDir * knockBack, ForceMode2D.Impulse);
        if (health <= 0) die();
    }


    private GameObject findClosestOrb()
    {
        Component[] orbs = GetComponentsInChildren<Orb>();
        GameObject closest = null;
        float closestDist = Mathf.Infinity;

        foreach(Component orb in orbs)
        {
            float dist = Vector2.Distance(orb.gameObject.transform.position, Input.mousePosition);
            if (dist < closestDist)
            {
                closest = orb.gameObject;
                closestDist = dist;
            }
        }
        return closest;
    }

    private void die()
    {
        GameObject.Destroy(this.gameObject);
    }
}
