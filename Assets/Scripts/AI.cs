using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    enum State
    {
        Patrolling,
        Chasing,
        Attacking,
    }
    
    State currentState;
    NavMeshAgent agent;

    public Transform[] destinationPoints;
    int destinationIndex;

    public Transform player;
    [SerializeField]
    float visionRange;
    [SerializeField]
    float attackRange;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        currentState = State.Patrolling;
    }

    // Update is called once per frame
    void Update()
    {
        switch(currentState)
        {
            case State.Patrolling:
                Patrol();
            break;

            case State.Chasing:
                Chase();
            break;

            case State.Attacking:
                Attack();
            break;

            default:
                Chase();
            break;
            
        }
    }
  
    void Patrol()
    {
        agent.destination = destinationPoints[destinationIndex].position;

        if(Vector3.Distance(transform.position, destinationPoints[destinationIndex].position)<1)
        {
            
            
            if(destinationIndex < destinationPoints.Length)
            {
                Debug.Log("Siguiente");
                destinationIndex += 1;
                currentState = State.Patrolling;
            }
            if(destinationIndex == destinationPoints.Length)
            {
                destinationIndex = 0;     
            }
           
        }

        if(Vector3.Distance(transform.position, player.position) < visionRange)
        {
            Debug.Log("Persiguiendo");
            currentState = State.Chasing;
        }
        
      
    }

    void Attack()
    {
        
        agent.destination = player.position;

        if(Vector3.Distance(transform.position, player.position) > attackRange)
        {
            currentState = State.Chasing;
        }

        
    }

    void Chase()
    {
        agent.destination = player.position;

        if(Vector3.Distance(transform.position, player.position) > visionRange)
        {
            currentState = State.Patrolling;
        }
        
        if(Vector3.Distance(transform.position, player.position) < attackRange)
        {
            Debug.Log("Atacando");
            currentState = State.Attacking;
            
        }
        
    }

    void OnDrawGizmos()
    {
        foreach (Transform point in destinationPoints)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(point.position, 1);

        }

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, visionRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }


}
