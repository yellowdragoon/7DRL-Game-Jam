using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orb : MonoBehaviour
{
    public GameObject player;
    private GameObject orbSlotParent;
    public float shotSpeed = 25.0f;
    Vector3 moveDir;
    private bool orbReleased = false;

    public int orbDamage = 5;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        orbSlotParent = gameObject.transform.parent.gameObject;
    }
    // Update is called once per frame
    void Update()
    {
        if (orbReleased)
        {
            transform.Translate(moveDir * shotSpeed * Time.deltaTime);
        }
        
    }

    private IEnumerator orbLifeTime()
    {
        yield return new WaitForSeconds(3.0f);
        dissipateOrb();
    }



    public void fire()
    {
        Debug.Log("Fired");
        orbReleased = true;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0.0f;
        transform.rotation = Quaternion.identity;
        moveDir = new Vector3(mousePos.x - transform.position.x, mousePos.y - transform.position.y, 0.0f).normalized;

        transform.SetParent(null); // Detach from the orbslot
        Debug.Log(transform.position);
        Debug.Log(transform.parent);
        StartCoroutine(orbLifeTime());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("OrbCollision!");
        if(orbReleased && collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<Enemy>().takeDamage(orbDamage);
            dissipateOrb();
        }
    }

    private void dissipateOrb()
    {
        Destroy(gameObject);
    }

}
