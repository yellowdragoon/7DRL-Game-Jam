using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Vector2 moveInput;
    public float moveSpeed = 10000.0f;
    public int health = 100;
    private Rigidbody2D rb;
    private bool hit = false;
    public GameObject orbSlotPrefab;

    private void Start()
    {
        //orbSlotPrefab = GameObject.FindGameObjectWithTag("OrbSlot");
        rb = gameObject.GetComponent<Rigidbody2D>();
        createSlots(5);
    }    

   
    void Update()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");

        moveInput = new Vector2(inputX, inputY);

        /*
        if(!hit)
        {
            transform.Translate(movement);
        }
        */
        

        if (Input.GetMouseButtonDown(0))
        {
            GameObject orb = findClosestOrb();
            if(orb != null) orb.GetComponent<Orb>().releaseOrb();
        }
    }

    private void FixedUpdate()
    {
        if (!hit)
        {
            rb.AddForce(moveInput * moveSpeed * Time.fixedDeltaTime);
        }
    }

    IEnumerator dmgFrames()
    {
        yield return new WaitForSeconds(0.4f);

        hit = false;
    }

    public void takeDamage(int amount, GameObject source, float knockBack)
    {
        if (!hit)
        {
            StartCoroutine(dmgFrames());
            health -= amount;
            Vector2 knockbackDir = (transform.position - source.transform.position).normalized;
            rb.AddForce(knockbackDir * knockBack, ForceMode2D.Impulse);
            if (health <= 0) die();
        }
        hit = true;  
    }


    private GameObject findClosestOrb()
    {
        GameObject[] orbSlots = GameObject.FindGameObjectsWithTag("OrbSlot");
        GameObject closest = null;
        float closestDist = Mathf.Infinity;

        foreach(GameObject orbSlot in orbSlots)
        {
            Orb orb = orbSlot.GetComponentInChildren<Orb>();
            if (!orb) continue;
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


    private void createSlots(int numSlots)
    {
        for (int i = 0; i < numSlots; i++)
        {
            GameObject newSlot = Instantiate(orbSlotPrefab);
            newSlot.GetComponent<OrbSlot>().setRotation(i * (360.0f/(float)numSlots));
        }
    }
}
