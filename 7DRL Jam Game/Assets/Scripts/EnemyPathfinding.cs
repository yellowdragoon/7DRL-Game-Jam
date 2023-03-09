using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPathfinding : MonoBehaviour
{
    [SerializeField] Transform target;
    NavMeshAgent agent;
    public bool aggro = false;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (aggro)
        {
            agent.SetDestination(target.position);
        }
    }

    public void gainAggro()
    {
        aggro = true;
    }
}
